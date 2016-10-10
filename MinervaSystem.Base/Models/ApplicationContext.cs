using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MinervaSystem.Base.Models
{
    public partial class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        static ApplicationContext()
        {
            Database.SetInitializer<ApplicationContext>(null);
        }

        public ApplicationContext() : base("DefaultConnection") { }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<PageController> PageControllers { get; set; }
        public DbSet<PagePermission> PagePermissions { get; set; }
        public DbSet<Farmer> Farmer { get; set; }
        public DbSet<SupplyInformation> SupplyInformation { get; set; }
        public DbSet<SupplyOrder> SupplyOrder { get; set; }
        public DbSet<SugerMill> SugerMill { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            string userId = (HttpContext.Current != null && HttpContext.Current.User != null) ? HttpContext.Current.User.Identity.GetUserId() : null;

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).Created = DateTime.Now;
                    ((BaseEntity)entity.Entity).Author = userId;
                }
                ((BaseEntity)entity.Entity).Modified = DateTime.Now;
                ((BaseEntity)entity.Entity).Editor = userId;
            }

            return base.SaveChanges();
        }
    }
}

