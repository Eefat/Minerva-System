using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinervaSystem.Base.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, string> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //Custom user properties here
        public bool Active { get; set; }
        [ForeignKey("Id")]
        public virtual ApplicationUserProfile Profile { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public string Notes { get; set; }
    }

    [Table("AspNetUserProfiles")]
    public class ApplicationUserProfile : BaseEntity
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public byte[] Photo { get; set; }
        public string AlternateEmail { get; set; }
        public string CellPhone { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyContactPhone { get; set; }
        public string Notes { get; set; }

        [Required, ForeignKey("Id")]
        public virtual ApplicationUser User { get; set; }
    }

    public class BaseEntity
    {
        public DateTime Created { get; set; }
        public string Author { get; set; }
        public DateTime Modified { get; set; }
        public string Editor { get; set; }
    }
}