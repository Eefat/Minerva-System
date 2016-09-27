using System.Linq;
using System.Web.Mvc;
using MinervaSystem.Base.Attributes;

namespace MinervaSystem.Base.Controllers
{
    [BaseAuthorize]
    public class BaseController : Controller
    {
        protected string CurrentController
        {
            get
            {
                return this.ControllerContext.RouteData.Values["controller"].ToString();
            }
        }

        protected int CurrentPermissionLevel
        {
            get
            {
                var controller = ContextPerRequest.CurrentContext.PageControllers.Where(c => c.Title == CurrentController).FirstOrDefault();
                if (controller != null)
                {
                    var currentPermissions = controller.PagePermissions.Where(p => ContextPerRequest.CurrentApplicationUser.Roles.Any(r => r.RoleId == p.UserGroup_Id));
                    if (currentPermissions != null)
                        return currentPermissions.OrderByDescending(p => p.PermissionLevel).FirstOrDefault().PermissionLevel;
                }

                return 0;
            }
        }

        protected bool CanRead
        {
            get
            {
                if (ContextPerRequest.CurrentUser.IsSystemOrSiteAdmin) return true;
                return CurrentPermissionLevel >= (int)PermissionTypes.Read;
            }
        }

        protected bool CanAdd
        {
            get
            {
                if (ContextPerRequest.CurrentUser.IsSystemOrSiteAdmin) return true;
                return CurrentPermissionLevel >= (int)PermissionTypes.Add;
            }
        }

        protected bool CanEdit
        {
            get
            {
                if (ContextPerRequest.CurrentUser.IsSystemOrSiteAdmin) return true;
                return CurrentPermissionLevel >= (int)PermissionTypes.Edit;
            }
        }

        protected bool CanDelete
        {
            get
            {
                if (ContextPerRequest.CurrentUser.IsSystemOrSiteAdmin) return true;
                return CurrentPermissionLevel >= (int)PermissionTypes.FullControl;
            }
        }

        public bool HasPermission(string controller, PermissionTypes permissionType)
        {
            if (ContextPerRequest.CurrentUser.IsSystemOrSiteAdmin)
                return true;

            var pageController = ContextPerRequest.CurrentContext.PageControllers.Where(c => c.Title == controller).FirstOrDefault();
            var currentPermissions = pageController.PagePermissions.Where(p => ContextPerRequest.CurrentApplicationUser.Roles.Any(r => r.RoleId == p.UserGroup_Id));
            if (currentPermissions != null)
                return currentPermissions.OrderByDescending(p => p.PermissionLevel).FirstOrDefault().PermissionLevel >= (int)permissionType;
            return false;
        }
    }
}