﻿using System;
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
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? UpazilaId { get; set; }
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
        public string StateName { get; set; }

        [Display(Name = "District")]
        public string DistrictName { get; set; }

        [Display(Name = "Upazila")]
        public string UpazilaName { get; set; }

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
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? UpazilaId { get; set; }
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
        public string StateName { get; set; }

        [Display(Name = "District")]
        public string DistrictName { get; set; }

        [Display(Name = "Upazila")]
        public string UpazilaName { get; set; }

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
    public class SupplyOrderViewModel
    {
        public Int64 Id { get; set; }
        public Int64 SugerMillId { get; set; }
        public Int64 SupplyInformationId { get; set; }
        public Int64? ZoneId { get; set; }
        public Int64? ZoneManagerId { get; set; }
        [Display(Name = "Suger Mill")]
        public string SugerMillName { get; set; }
        [Display(Name = "Farmer")]
        public string FarmerName { get; set; }
        [Display(Name = "Farmer")]
        public string MemberKey { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Code")]
        public string Code { get; set; }
        [Display(Name = "Collection Date")]
        public DateTime? CollectionDate { get; set; }
        [Display(Name = "Estimated Amount")]
        public Decimal? EstimatedAmount { get; set; }
        [Display(Name = "Collected Amount")]
        public Decimal? CollectedAmount { get; set; }
        [Display(Name = "Is Collected")]
        public Boolean? IsCollected { get; set; }
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
        [Required]
        [Display(Name = "Plant Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateofPlanting { get; set; }
        [Display(Name = "Supply Date")]

        private DateTime _returnDate = DateTime.MinValue;
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime SupplyDate
        {
            get
            {
                return (_returnDate == DateTime.MinValue) ? DateTime.Now : _returnDate;
            }
            set { _returnDate = value; }
        }
        [Display(Name = "Note")]
        public string Note { get; set; }

    }


    public class SupplyOrderRequest
    {
        public Int64 Id { get; set; }
        public Int64 SugerMillId { get; set; }
        public Int64 SupplyInformationId { get; set; }
        public Int64? ZoneId { get; set; }
        public Int64? ZoneManagerId { get; set; }
        public Decimal? CollectedAmount { get; set; }
        public Decimal? EstimatedAmount { get; set; }
        public string Code { get; set; }
        public Boolean? IsCollected { get; set; }
        public String CollectionDate { get; set; }
        public String MemberKey { get; set; }
        public String MemberName { get; set; }
        public string Note { get; set; }
        public string MobileNo { get; set; }
    }
    public class SupplyOrderResponseMsg
    {
        public int status { get; set; }
        public string responseMsg { get; set; }
        public string mobileNo { get; set; }
        public string url { get; set; }
        public string from { get; set; }
        public string authorization { get; set; }
        public string countryCode { get; set; }
    }
    public static class BasicInformation
    {
       public static string SmsUrl = "https://api.infobip.com/sms/1/text/single";
       public static string CountryCode = "88";
       public static string UserId = "itel786";
       public static string Password = "01818856792SMS";
       public static string smsAuth = "Basic "+Base64Encode(UserId + ":"+ Password);
       public static string From = "InfoSMS";

        #region----- Utility-----
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        #endregion
    }


    public class Thing
    {
        public int Id { get; set; }
        public string Color { get; set; }
    }

}