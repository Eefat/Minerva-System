using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinervaSystem.Base.Attributes
{
    public class BaseAuthorize : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(HttpContext.Current.Request.IsAuthenticated)
            {
                if (!ContextPerRequest.CurrentUser.IsSystemOrSiteAdmin)
                {
                    var actionName = filterContext.ActionDescriptor.ActionName;
                    var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    var RequiredPermission = (int)PermissionTypes.Read;

                    var customAttr = filterContext.ActionDescriptor.GetCustomAttributes(typeof(RequiredPermission), false);
                    if (customAttr.Length > 0)
                    {
                        //check if controller is specified in the attribute
                        controllerName = ((RequiredPermission)customAttr[0]).SiteController ?? controllerName;
                        RequiredPermission = (int)((RequiredPermission)customAttr[0]).PermissionType;
                    }
                    else
                    {
                        if (actionName == "Create" || actionName.StartsWith("Create_")) RequiredPermission = (int)PermissionTypes.Add;
                        if (actionName == "Edit" || actionName.StartsWith("Edit_")) RequiredPermission = (int)PermissionTypes.Edit;
                        if (actionName == "Delete" || actionName.StartsWith("Delete_")) RequiredPermission = (int)PermissionTypes.FullControl;
                    }

                    var pageController = ContextPerRequest.CurrentContext.PageControllers.Where(c => c.Title == controllerName).FirstOrDefault();
                    if (pageController == null)
                    {
                        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "Controller", "Error" }, { "Action", "AccessDenied" } });
                    }
                    else
                    {
                        var currentPermission = pageController.PagePermissions
                            .Where(p => ContextPerRequest.CurrentApplicationUser.Roles.Any(r => r.RoleId == p.UserGroup.Id))
                            .OrderByDescending(p => p.PermissionLevel).FirstOrDefault();

                        if (currentPermission == null || currentPermission.PermissionLevel < RequiredPermission)
                            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "Controller", "Error" }, { "Action", "AccessDenied" } });
                    }
                }
            }
            else
            {   //Redirect to LogIn page
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    { "Controller", "Account" },
                    { "Action", "Login" },
                    { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
                });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}