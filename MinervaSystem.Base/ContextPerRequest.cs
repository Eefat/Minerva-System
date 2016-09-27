using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MinervaSystem.Base.Models;

namespace MinervaSystem.Base
{
    public static class ContextPerRequest
    {
        private static ApplicationUserManager _userManager;
        private static ApplicationRole _sysAdminGroup;
        private static ApplicationRole _siteAdminGroup;
        private static ApplicationUser _currentApplicationUser;
        private static CurrentUser _currentUser;

        public static ApplicationContext CurrentContext
        {
            get
            {
                if (!HttpContext.Current.Items.Contains("ApplicationContext"))
                    HttpContext.Current.Items.Add("ApplicationContext", new ApplicationContext());
                return HttpContext.Current.Items["ApplicationContext"] as ApplicationContext;
            }
        }

        public static ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? new ApplicationUserManager(new ApplicationUserStore(new ApplicationContext()));
            }
            private set
            {
                _userManager = value;
            }
        }

        public static ApplicationRole SystemAdminGroup
        {
            get
            {
                return _sysAdminGroup ?? CurrentContext.Roles.Where(g => g.Name == "System Administrators").FirstOrDefault();
            }
            private set
            {
                _sysAdminGroup = value;
            }
        }

        public static ApplicationRole SiteAdminGroup
        {
            get
            {
                return _siteAdminGroup ?? CurrentContext.Roles.Where(g => g.Name == "Site Administrators").FirstOrDefault();
            }
            private set
            {
                _siteAdminGroup = value;
            }
        }

        public static CurrentUser CurrentUser
        {
            get
            {
                if (_currentUser != null && _currentUser.Id == HttpContext.Current.User.Identity.GetUserId())
                    return _currentUser;

                CurrentUser currentUser = new CurrentUser();
                var user = CurrentApplicationUser;

                currentUser.Id = user.Id;
                currentUser.LoginName = user.UserName;
                currentUser.Name = (user.Profile == null) ? user.UserName : user.Profile.Name;
                currentUser.IsSystemAdmin = user.Roles.Any(g => g.RoleId == SystemAdminGroup.Id);
                currentUser.IsSiteAdmin = user.Roles.Any(g => g.RoleId == SiteAdminGroup.Id);
                currentUser.IsSystemOrSiteAdmin = currentUser.IsSystemAdmin || currentUser.IsSiteAdmin;
                //Fill Groups
                currentUser.UserGroups = new List<ApplicationRole>();
                if (user.Roles.Count > 0)
                {
                    foreach (var role in user.Roles)
                    {
                        currentUser.UserGroups.Add(CurrentContext.Roles.Find(role.RoleId));
                    }
                }

                return currentUser;
            }
            private set
            {
                _currentUser = value;
            }
        }

        public static ApplicationUser CurrentApplicationUser
        {
            get
            {
                if (_currentApplicationUser != null && _currentApplicationUser.Id == HttpContext.Current.User.Identity.GetUserId())
                    return _currentApplicationUser;
                return UserManager.FindById(HttpContext.Current.User.Identity.GetUserId());
            }
            private set
            {
                _currentApplicationUser = value;
            }
        }

        public static string GetUserNameById(string userId, bool loginName = false)
        {
            var user = UserManager.FindById(userId);
            if (user == null) return "";
            return (user.Profile == null || loginName) ? user.UserName : user.Profile.Name;
        }

        public static List<ApplicationUser> GetAllUsers(bool currentUser = true, bool inactiveUsers = true, bool activeUsers = true, bool systemUsers = true)
        {
            return CurrentContext.Users
                .Where(u => (currentUser || u.Id != CurrentApplicationUser.Id)
                    && (inactiveUsers || u.Active)
                    && (activeUsers || !u.Active)
                    && (systemUsers || (u.UserName != "sysadmin" && u.UserName != "siteadmin")))
                .OrderBy(u => u.UserName).ToList();
        }

        public static List<ApplicationRole> GetAllUserGroups(bool systemAdmin = true, bool siteAdmin = true)
        {
            return CurrentContext.Roles.Where(g => (systemAdmin || g.Id != SystemAdminGroup.Id) && (siteAdmin || g.Id != SiteAdminGroup.Id))
                .OrderBy(g => g.Name).ToList();
        }
    }

    public class CurrentUser
    {
        public string Id { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public bool IsSystemAdmin { get; set; }
        public bool IsSiteAdmin { get; set; }
        public bool IsSystemOrSiteAdmin { get; set; }
        public List<ApplicationRole> UserGroups { get; set; }
    }
}