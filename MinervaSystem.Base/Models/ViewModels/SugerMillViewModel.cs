using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MinervaSystem.Base;

namespace MinervaSystem.Base.Models.ViewModels
{
    public class SugerMillViewModel
    {
        public Int64 Id { get; set; }
        public string Code { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Establish Date")]
        public DateTime? EstablishDate { get; set; }

        [Display(Name = "Photo")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PhotoFile { get; set; }
        public byte[] Photo { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }
 
        [Display(Name = "Division")]
        public string Division { get; set; }

        [Display(Name = "District")]
        public string District { get; set; }

        [Display(Name = "Thana")]
        public string Thana { get; set; }

        [Display(Name = "Village")]
        public string Village { get; set; }

        [Display(Name = "PostalCode")]
        public string PostalCode { get; set; }
        [Display(Name = "Note")]
        public string Note { get; set; }
    }
    public class FarmerViewModel
    {
        public Int64 Id { get; set; }
        public string FarmerIdNo { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "User Name")]
        public string Name { get; set; }
        
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Photo")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PhotoFile { get; set; }
        public byte[] Photo { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }
        
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "National Id No")]
        public string NationalIdNo { get; set; }
        
        [Display(Name = "Division")]
        public string Division { get; set; }

        [Display(Name = "District")]
        public string District { get; set; }

        [Display(Name = "Thana")]
        public string Thana { get; set; }

        [Display(Name = "Village")]
        public string Village { get; set; }

        [Display(Name = "PostalCode")]
        public string PostalCode { get; set; }
        [Display(Name = "Emergency Contact")]
        public string EmergencyContact { get; set; }
        [Display(Name = "Emergency Contact Phone")]
        public string EmergencyContactPhone { get; set; }
        [Display(Name = "Note")]
        public string Note { get; set; }
    }
    public class SupplyInformationViewModel
    {
        public Int64 Id { get; set; }
        public Int64 FarmerId { get; set; }
        [Display(Name = "Suger Mill Name")]
        public Int64 SugerMillId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AllSugerMills { get; set; }
        [Display(Name = "Farmer Id")]
        public string FarmerKey { get; set; }
        [Display(Name = "Farmer Name")]
        public string FarmerName { get; set; }
        [Required]
        [Display(Name = "Cane Variety")]
        public CaneVariety? CaneVariety { get; set; }
        [Required]
        [Display(Name = "Plant/Ratoon")]
        public PlantRatoon? PlantRatoon { get; set; }
        [Required]
        [Display(Name = "Land Area")]
        public Decimal LandArea { get; set; }
        [Required]
        [Display(Name = "Estimated Amount")]
        public Decimal EstimatedAmount { get; set; }
        [Display(Name = "Plant Date")]
        public DateTime DateofPlanting { get; set; }
        [Display(Name = "Supply Date")]
        public DateTime SupplyDate { get; set; }
        [Display(Name = "Note")]
        public string Note { get; set; }

    }
}