using System.ComponentModel;

namespace MinervaSystem.Base
{
    public static class SiteConfig
    {
        public const string SiteTitle = "Application Name";
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
        A, B, C, D, E, F
    }
    public enum PlantRatoon
    {
        Plant, Ratoon
    }
    #endregion
}
