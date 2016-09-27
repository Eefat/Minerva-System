using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MinervaSystem.Base.Models;
using System;
using System.Net;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace MinervaSystem.Base.Controllers
{
    [Authorize(Roles = "System Administrators, Site Administrators")]
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AdminController() { }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                //return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                return _signInManager ?? new ApplicationSignInManager(UserManager, AuthenticationManager);
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                //return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                return _userManager ?? new ApplicationUserManager(new ApplicationUserStore(new ApplicationContext()));
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index(string message)
        {
            ViewBag.StatusMessage = message;
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewmodel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Active = true };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    
                    return RedirectToAction("Index", new { Message = "New user was created successfully!"});
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ResetPassword(string code)
        {
            //return code == null ? View("Error") : View();
            ResetPasswordViewModel viewModel = new ResetPasswordViewModel();
            viewModel.AllUsers = ContextPerRequest.GetAllUsers(currentUser: false, inactiveUsers: false)
                .Select(u => new SelectListItem() { Value = u.Id, Text = (u.Profile == null ? u.UserName : u.Profile.Name) });
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            model.AllUsers = model.AllUsers ?? ContextPerRequest.GetAllUsers(currentUser: false, inactiveUsers: false)
                .Select(u => new SelectListItem() { Value = u.Id, Text = (u.Profile == null ? u.UserName : u.Profile.Name) });

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    ModelState.AddModelError("", "User Not Found!");
                    return View();
                }

                //var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                IdentityResult result1 = await UserManager.RemovePasswordAsync(user.Id);
                IdentityResult result2 = await UserManager.AddPasswordAsync(user.Id, model.Password);
                if (result2.Succeeded)
                    return RedirectToAction("Index", new { Message = "Password Reset was successful!" });

                AddErrors(result2);
                return View();
            }

            return View(model);
        }

        public ActionResult DisableEnableUser()
        {
            return View();
        }

        public ActionResult EnableDisableUser(string userId, bool enable)
        {
            var user = ContextPerRequest.CurrentContext.Users.Find(userId);
            if(user != null)
            {
                user.Active = enable;
                user.Roles.Clear();
                ContextPerRequest.CurrentContext.SaveChanges();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult ViewUserGroups()
        {
            return View();
        }

        public ActionResult ViewUserProfile()
        {
            return View();
        }

        #region ----- UserGroup Management Section -----

        public ActionResult ManageUserGroups()
        {
            return View();
        }

        public JsonResult GetAllUserGroups()
        {
            var groups = ContextPerRequest.GetAllUserGroups()
                .Select(g => new { g.Id, GroupName = g.Name, CountUsers = g.Users.Count, g.Notes }).ToList();

            return Json(new { aaData = groups }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateOrUpdateUserGroup(string Id, string name, string notes)
        {
            if(string.IsNullOrEmpty(name))
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, "Group Name is required");

            try
            {
                if(string.IsNullOrEmpty(Id))
                {   //Creating group
                    ApplicationRole group = new ApplicationRole() { Name = name, Notes = notes };
                    ContextPerRequest.CurrentContext.Roles.Add(group);
                }
                else
                {   //Updating group
                    var group = ContextPerRequest.CurrentContext.Roles.Find(Id);
                    group.Name = name;
                    group.Notes = notes;
                }
                
                ContextPerRequest.CurrentContext.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.Message);
            }
        }

        public ActionResult DeleteUserGroup(string id)
        {
            var group = ContextPerRequest.CurrentContext.Roles.Find(id);
            if (group.Name == "System Administrators" || group.Name == "Site Administrators")
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, "This user group cannot be deleted. It is used by the system.");
            ContextPerRequest.CurrentContext.Roles.Remove(group);
            ContextPerRequest.CurrentContext.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public JsonResult GetUserIdsByUserGroup(string groupId)
        {
            var userIds = ContextPerRequest.CurrentContext.Roles.Find(groupId).Users.Select(u => u.UserId).ToList();
            return Json(userIds, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserNamesByUserGroup(string groupId)
        {
            List<string> userNames = new List<string>();
            foreach(var user in ContextPerRequest.CurrentContext.Roles.Find(groupId).Users.ToList())
            {
                userNames.Add(ContextPerRequest.GetUserNameById(user.UserId));
            }
            return Json(userNames.OrderBy(n => n), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateUsersInUserGroup(string groupId, string[] userIds)
        {
            try
            {
                ApplicationRole group = ContextPerRequest.CurrentContext.Roles.Find(groupId);
                if (userIds == null || userIds.Length == 0)
                {
                    group.Users.Clear();
                }
                else
                {
                    //Loop through existing users and delete any users if not included in updated list
                    foreach (var userRole in group.Users.ToList())
                    {
                        if (!userIds.Contains(userRole.UserId))
                            group.Users.Remove(userRole);
                    }
                    ContextPerRequest.CurrentContext.SaveChanges();

                    //Loop through updated user list and add users where not already added
                    foreach (var userId in userIds)
                    {
                        if (!group.Users.Any(g => g.UserId == userId))
                            group.Users.Add(new IdentityUserRole { RoleId = groupId, UserId = userId });
                    }
                }
				
				ContextPerRequest.CurrentContext.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.Message);
            }
        }

        public JsonResult GetGroupsByUser(string userId)
        {
            var groups = ContextPerRequest.CurrentContext.Roles.Where(g => g.Users.Any(u => u.UserId == userId)).OrderBy(g => g.Name).Select(g => g.Name).ToList();
            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ----- Controller Permissions -----

        public ActionResult ManageControllerPermission()
        {
            return View();
        }

        public JsonResult GetAllControllers()
        {
            var controllers = ContextPerRequest.CurrentContext.PageControllers.OrderBy(c => c.Title)
                .Select(c => new { c.Id, Controller = c.Title, CountGroups = c.PagePermissions.Count }).ToList();

            return Json(new { aaData = controllers }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPermissionsByController(int controllerId)
        {
            var controller = ContextPerRequest.CurrentContext.PageControllers.Find(controllerId);
            var permissions = controller.PagePermissions
                .Select(p => new { group = p.UserGroup.Name, permission = Enum.GetName(typeof(PermissionTypes), p.PermissionLevel) });

            return Json(permissions.OrderBy(p => p.group), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPermissionByControllerAndUserGroup(int controllerId, string groupId)
        {
            var permission = ContextPerRequest.CurrentContext.PagePermissions.Find(controllerId, groupId);
            return Json(permission.PermissionLevel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNonExistingUserGroupsByController(int controllerId)
        {
            var groups = ContextPerRequest.GetAllUserGroups(systemAdmin: false, siteAdmin: false).ToList();
            var existingGroups = ContextPerRequest.CurrentContext.PageControllers.Find(controllerId).PagePermissions.Select(p => p.UserGroup_Id).ToList();
            foreach(var groupId in existingGroups)
            {
                var group = ContextPerRequest.CurrentContext.Roles.Find(groupId);
                groups.Remove(group);
            }

            return Json(groups.Select(g => new { g.Id, g.Name }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExistingUserGroupsByController(int controllerId)
        {
            var groupIds = ContextPerRequest.CurrentContext.PageControllers.Find(controllerId).PagePermissions.Select(p => p.UserGroup_Id).ToList();
            var groups = new List<ApplicationRole>();
            foreach (var groupId in groupIds)
            {
                groups.Add(ContextPerRequest.CurrentContext.Roles.Find(groupId));
            }

            return Json(groups.OrderBy(g => g.Name).Select(g => new { g.Id, g.Name }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddPermissionsInController(int controllerId, string[] groupIds, int permLevel)
        {
            try
            {
                if(groupIds.Length > 0)
                {
                    var controller = ContextPerRequest.CurrentContext.PageControllers.Find(controllerId);
                    foreach (var groupId in groupIds)
                    {   
                        var permission = ContextPerRequest.CurrentContext.PagePermissions.Find(controller.Id, groupId);
                        if(permission == null)
                        {
                            permission = new PagePermission() { PageController_Id = controllerId, UserGroup_Id = groupId, PermissionLevel = permLevel };
                            controller.PagePermissions.Add(permission);
                        }
                    }
                    ContextPerRequest.CurrentContext.SaveChanges();
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.Message);
            }
        }

        public ActionResult EditPermissionInController(int controllerId, string groupId, int permLevel)
        {
            try
            {
                ContextPerRequest.CurrentContext.PagePermissions.Find(controllerId, groupId).PermissionLevel = permLevel;
                ContextPerRequest.CurrentContext.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.Message);
            }

        }

        public ActionResult RemovePermissionsInController(int controllerId, string[] groupIds)
        {
            if (groupIds.Length > 0)
            {
                var controller = ContextPerRequest.CurrentContext.PageControllers.Find(controllerId);
                foreach (var groupId in groupIds)
                {
                    var permission = ContextPerRequest.CurrentContext.PagePermissions.Find(controller.Id, groupId);
                    if (permission != null) ContextPerRequest.CurrentContext.PagePermissions.Remove(permission);
                }
                ContextPerRequest.CurrentContext.SaveChanges();
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion

        #region ----- Manage Announcements -----

        public ActionResult ManageAnnouncements()
        {
            ViewBag.HeaderContent = new HeaderContentModel("Manage Announcements",
                new BreadcrumbModel("Manage Announcements", new List<LinkModel>() { new LinkModel("/Admin", "Site Actions") }));
            return View();
        }

        public JsonResult GetAllAnnouncements()
        {
            var announcements = ContextPerRequest.CurrentContext.Announcements.OrderBy(a => a.Title)
                .Select(a => new { a.Id, a.Title, a.Body, a.Expires }).ToList();

            return Json(new { aaData = announcements }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAnnouncementDetails(int announcementId)
        {
            var announcement = ContextPerRequest.CurrentContext.Announcements.Find(announcementId);
            var Author = ContextPerRequest.GetUserNameById(announcement.Author);
            var Editor = ContextPerRequest.GetUserNameById(announcement.Editor);
            var json = new { announcement.Id, announcement.Title, announcement.Body, announcement.Expires, announcement.Created, Author, announcement.Modified, Editor };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddAnnouncement(string title, string body, DateTime? expires)
        {
            try
            {
                var announcement = new Announcement(title, body, expires);
                ContextPerRequest.CurrentContext.Announcements.Add(announcement);
                ContextPerRequest.CurrentContext.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.Message);
            }
        }

        public ActionResult EditAnnouncement(int announcementId, string title, string body, DateTime? expires)
        {
            try
            {
                var announcement = ContextPerRequest.CurrentContext.Announcements.Find(announcementId);
                announcement.Title = title;
                announcement.Body = body;
                announcement.Expires = expires;
                ContextPerRequest.CurrentContext.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.Message);
            }
        }

        public ActionResult DeleteAnnouncement(int announcementId)
        {
            try
            {
                var announcement = ContextPerRequest.CurrentContext.Announcements.Find(announcementId);
                ContextPerRequest.CurrentContext.Announcements.Remove(announcement);
                ContextPerRequest.CurrentContext.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.Message);
            }
        }
        #endregion

        #region ----- Advanced Operations -----

        /*public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        public ActionResult SetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region ----- Ajax Requests -----

        public JsonResult GetUserProfile(string userId)
        {
            var user = ContextPerRequest.CurrentContext.Users.Find(userId);
            if (user != null && user.Profile != null)
            {
                var profile = user.Profile;
                return Json(new
                {
                    Photo = (profile.Photo == null) ? null : Convert.ToBase64String(profile.Photo),
                    profile.Name,
                    profile.BirthDate,
                    profile.Phone,
                    profile.CellPhone,
                    profile.AlternateEmail,
                    profile.Address,
                    profile.City,
                    profile.Province,
                    profile.PostalCode,
                    profile.EmergencyContact,
                    profile.EmergencyContactPhone,
                    profile.Notes
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ----- Helpers -----
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}