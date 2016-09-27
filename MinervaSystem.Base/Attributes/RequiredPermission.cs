using System;

namespace MinervaSystem.Base.Attributes
{
    public class RequiredPermission : Attribute
    {
        public PermissionTypes PermissionType { get; set; }
        public string SiteController { get; set; }

        public RequiredPermission(PermissionTypes PermissionType)
        {
            this.PermissionType = PermissionType;
        }
    }
}