using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinervaSystem.Base.Models
{
    public class Announcement : BaseEntity
    {
        public Announcement() { }

        public Announcement(string title, string body, DateTime? expires)
        {
            Title = title;
            Body = body;
            Expires = expires;
        }

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTime? Expires { get; set; }
    }

    public partial class PageController
    {
        public PageController() { }

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public virtual ICollection<PagePermission> PagePermissions { get; set; }
    }

    public partial class PagePermission
    {
        public PagePermission() { }

        [Key, Column(Order = 0), ForeignKey("PageController")]
        public int PageController_Id { get; set; }
        [Key, Column(Order = 1), ForeignKey("UserGroup")]
        public string UserGroup_Id { get; set; }
        public int PermissionLevel { get; set; }

        public virtual PageController PageController { get; set; }
        public virtual ApplicationRole UserGroup { get; set; }
    }
}
