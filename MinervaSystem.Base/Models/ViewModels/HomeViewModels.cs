using System.Collections.Generic;

namespace MinervaSystem.Base.Models
{
    public class HomeViewModel
    {
        public List<Announcement> Announcements { get; set; }
    }

    public class MoreAnnouncementsViewModel
    {
        public IEnumerable<Announcement> Announcements { get; set; }
    }
}
