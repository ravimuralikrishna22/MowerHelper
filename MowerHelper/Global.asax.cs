using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MowerHelper
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //CaptchaUtils.CaptchaManager.StorageProvider = new SessionStorageProvider();
            //CaptchaUtils.ImageGenerator.Width = 130;
            //CaptchaUtils.ImageGenerator.Height = 50;
           // CaptchaUtils.ImageGenerator.Background = System.Drawing.Color.White;
            //AreaRegistration.RegisterAllAreas();

          //  WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = true;
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
          //  AuthConfig.RegisterAuth();
        }
        
    }
}