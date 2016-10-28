using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MinervaSystem.Base
{
    public static class SiteConfig
    {
        public const string SiteTitle = "Home";
    }

    #region ----- Base Enums -----

    public enum SystemControllers
    {
        //Exclude reserved controllers from the system
        Account,
        Admin,
        Base,
        Error,
        Home
    }

    public enum PermissionTypes
    {
        Limited = 0,
        Read = 1,
        Add = 2,
        Edit = 3,
        FullControl = 4
    }

    public enum ReportFormats
    {
        //Supports XML, NULL, CSV, IMAGE, PDF, HTML4.0, HTML3.2, MHTML, EXCEL, and Word.
        //Following Names will accept by the ReportExecutionServiceSoapClient.Render() method
        PDF = 0,
        EXCEL,
        Word
    }

    #endregion

    #region ----- Site Enums -----

    public enum SiteControllers
    {
        //Exclude reserved controllers from the system
        FinancialOrganization,
        Client
    }

    public enum ReportFiles
    {
        [Description("All Financial Organizations Report")]
        AllFinancialOrganizations,

        [Description("Completed Ratings By Employee")]
        EmployeesCompletedRatings
    }
    public enum CaneVariety
    {
        [Display(Name = "ISD 1/53")]
        ISD153,
        [Display(Name = "ISD 1/54")]
        ISD154,
        [Display(Name = "ISD 16")]
        ISD16,
        [Display(Name = "ISD 17")]
        ISD17,
        [Display(Name = "ISD 18")]
        ISD18,
        [Display(Name = "ISD 19")]
        ISD19,
        [Display(Name = "ISD 20")]
        ISD20,
        [Display(Name = "ISD 25")]
        ISD25,
        [Display(Name = "ISD 28")]
        ISD28,
        [Display(Name = "LJC")]
        LJC
    }
    public enum PlantRatoon
    {
        Plant, Ratoon
    }
    public enum Zone
    {
        [Display(Name = "Zone 1")]
        Zone1,
        [Display(Name = "Zone 2")]
        Zone2 ,
        [Display(Name = "Zone 3")]
        Zone3,
        [Display(Name = "Zone 4")]
        Zone4
    }
    #endregion
}
