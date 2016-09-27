using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using MinervaSystem.Base;
using MinervaSystem.Base.Controllers;
using MinervaSystem.Base.jQueryDataTableWrapper;
using MinervaSystem.Base.Models;

namespace MinervaSystem.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            ModelBinders.Binders.Add(typeof(DefaultDataTablesRequest), new DataTablesBinder());

            //ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationcallback;

            RazorViewEngine razorEngine = (RazorViewEngine)ViewEngines.Engines .Where(e => e.GetType() == typeof(RazorViewEngine)).FirstOrDefault();
            if (razorEngine != null)
            {
                string[] additionalViewLocations = new[] { "~/Views/Base/{1}/{0}.cshtml", "~/Views/Base/Shared/{0}.cshtml" };
                razorEngine.PartialViewLocationFormats = razorEngine.PartialViewLocationFormats.Union(additionalViewLocations).ToArray();
                razorEngine.ViewLocationFormats = razorEngine.ViewLocationFormats.Union(additionalViewLocations).ToArray();
                razorEngine.MasterLocationFormats = razorEngine.MasterLocationFormats.Union(additionalViewLocations).ToArray();
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.IsAuthenticated && HttpContext.Current.Session["Identity"] == null)
                FormsAuthentication.SignOut();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var appContext = HttpContext.Current.Items["ApplicationContext"] as ApplicationContext;
            if (appContext != null) appContext.Dispose();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Server.ClearError();

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "Index");
            routeData.Values.Add("exception", exception);

            if (exception.GetType() == typeof(HttpException))
                routeData.Values.Add("statusCode", ((HttpException)exception).GetHttpCode());
            else
                routeData.Values.Add("statusCode", 500);

            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            Response.End();
        }
    }
}
