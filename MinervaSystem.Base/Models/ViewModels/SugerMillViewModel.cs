using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MinervaSystem.Base;
using System.Reflection;
using System.ComponentModel;

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
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
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
        [Display(Name = "Zone")]
        public Zone? ZoneId { get; set; }
        public string FarmerIdNo { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "User Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
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

        [Display(Name = "Postal Code")]
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
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
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
        [Required]
        public Int64 FarmerId { get; set; }
        [Display(Name = "Suger Mill Name")]
        [Required]
        public Int64 SugerMillId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AllSugerMills { get; set; }
        [Required]
        [Display(Name = "Farmer Id")]
        public string FarmerKey { get; set; }
        [Display(Name = "Farmer Name")]
        public string FarmerName { get; set; }
        [Display(Name = "Cane Variety")]
        public CaneVariety? CaneVariety { get; set; }
        [Required]
        [Display(Name = "Plant/Ratoon")]
        public PlantRatoon? PlantRatoon { get; set; }
        [Required]
        [Display(Name = "Land Area")]
        public Decimal LandArea { get; set; }
        [Display(Name = "Estimated Amount")]
        public Decimal? EstimatedAmount { get; set; }

        private DateTime _returnDate = DateTime.MinValue;
        [Required]
        [Display(Name = "Plant Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateofPlanting { get; set; }
        //public DateTime DateofPlanting
        //{
        //    get
        //    {
        //        return (_returnDate == DateTime.MinValue) ? DateTime.Now : _returnDate;
        //    }
        //    set { _returnDate = value; }
        //}
        [Required]
        [Display(Name = "Supply Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime SupplyDate { get; set; }
        //public DateTime SupplyDate
        //{
        //    get
        //    {
        //        return (_returnDate == DateTime.MinValue) ? DateTime.Now : _returnDate;
        //    }
        //    set { _returnDate = value; }
        //}
        //[Display(Name = "Note")]
        public string Note { get; set; }

    }


    public class SupplyOrderRequest
    {
        public Int64 Id { get; set; }
        public Int64 SugerMillId { get; set; }
        public Int64 SupplyInformationId { get; set; }
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
        public static string smsAuth = "Basic " + Base64Encode(UserId + ":" + Password);
        public static string From = "BSFIC Admin";

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

        public static T StringToEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name);
        }

        public static string ToDescriptionString(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }
    }


    public class Thing
    {
        public int Id { get; set; }
        public string Color { get; set; }
    }

}