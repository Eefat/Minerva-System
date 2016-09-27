using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinervaSystem.Web.Models.ViewModel
{
    class FarmerSearch
    {
        public string MemberKey { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string NationalIdNo { get; set; }
        public Decimal? TotalLand { get; set; }
        public Int64? SugerMillId { get; set; }
    }
}
