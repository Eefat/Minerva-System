using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MinervaSystem.Base;
using MinervaSystem.Base.Models;

namespace MinervaSystem.Base.Models
{
    public class SugerMill : BaseEntity
    {
        public SugerMill() { }
        public Int64 Id { get; set; }
        //public string UserId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? EstablishDate { get; set; }
        public byte[] Photo { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public string Thana { get; set; }
        public string Village { get; set; }
        public string PostalCode { get; set; }
        public string Note { get; set; }
        //public virtual ICollection<User> SupplyInformation { get; set; }
        public virtual ICollection<SupplyInformation> SupplyInformation { get; set; }
    }
    public class Farmer : BaseEntity
    {
        public Farmer() { }
        public Int64 Id { get; set; }
        public string FarmerIdNo { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public byte[] Photo { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string NationalIdNo { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public string Thana { get; set; }
        public string Village { get; set; }
        public string PostalCode { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyContactPhone { get; set; }
        public string Note { get; set; }
        public virtual ICollection<SupplyInformation> SupplyInformation { get; set; }
    }
    public class SupplyInformation : BaseEntity
    {
        public SupplyInformation() { }
        public Int64 Id { get; set; }
        [ForeignKey("Farmer")]
        public Int64 FarmerId { get; set; }
        [ForeignKey("SugerMill")]
        public Int64 SugerMillId { get; set; }
        public CaneVariety? CaneVariety { get; set; }
        public PlantRatoon? PlantRatoon { get; set; }
        public Decimal LandArea { get; set; }
        public  Decimal EstimatedAmount { get; set; }
        public DateTime DateofPlanting { get; set; }
        public DateTime SupplyDate { get; set; }
        public string Note { get; set; }
        public virtual Farmer Farmer { get; set; }
        public virtual SugerMill SugerMill { get; set; }
    }
    public class SupplyInformationSearch
    {
        public string MemberKey { get; set; }
        public string Name {get; set;}
        public Int64? SugerMillId { get; set; }
        public int? CaneVariety { get; set;}
        public int? PlantRatoon { get; set;}
        public Decimal? LandArea { get; set; }
        public Decimal? EstimatedAmount { get; set;}
        public String DateofPlanting { get; set;}
        public String SupplyDate { get; set;}
    }
    public class FarmerSearch
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
