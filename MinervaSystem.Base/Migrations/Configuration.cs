namespace MinervaSystem.Base.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationContext context)
        {
            //Add or update default users and groups
            var hasher = new PasswordHasher();
            var user1 = new ApplicationUser() { UserName = "sysadmin", Email = "sysadmin@template.com", Active = true, PasswordHash = hasher.HashPassword("P@ssw0rd"), SecurityStamp = Guid.NewGuid().ToString() };
            var user2 = new ApplicationUser() { UserName = "siteadmin", Email = "siteadmin@template.com", Active = true, PasswordHash = hasher.HashPassword("P@ssw0rd"), SecurityStamp = Guid.NewGuid().ToString() };
            context.Users.AddOrUpdate(u => u.UserName, user1);
            context.Users.AddOrUpdate(u => u.UserName, user2);

            var group1 = new ApplicationRole { Name = "System Administrators", Notes = "Responsilble for settings & configuration of this site." };
            var group2 = new ApplicationRole { Name = "Site Administrators", Notes = "Responsilble for settings & configuration of this site." };
            context.Roles.AddOrUpdate(g => g.Name, group1);
            context.Roles.AddOrUpdate(g => g.Name, group2);
            context.SaveChanges();

            //include users in corresponding groups
            ApplicationRole groupSysAdmin = context.Roles.Where(g => g.Name == "System Administrators").FirstOrDefault();
            ApplicationRole groupSiteAdmin = context.Roles.Where(g => g.Name == "Site Administrators").FirstOrDefault();

            ApplicationUser userSysAdmin = context.Users.Where(u => u.UserName == "sysadmin").FirstOrDefault();
            ApplicationUser userSiteAdmin = context.Users.Where(u => u.UserName == "siteadmin").FirstOrDefault();

            if (!userSysAdmin.Roles.Any(g => g.RoleId == groupSysAdmin.Id))
                groupSysAdmin.Users.Add(new IdentityUserRole() { UserId = userSysAdmin.Id, RoleId = groupSysAdmin.Id });

            if (!userSiteAdmin.Roles.Any(g => g.RoleId == groupSiteAdmin.Id))
                groupSiteAdmin.Users.Add(new IdentityUserRole() { UserId = userSiteAdmin.Id, RoleId = groupSiteAdmin.Id });
        }
    }
}
