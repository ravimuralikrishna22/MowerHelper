using System.Web;
using System.Web.Mvc;
using MowerHelper.App_Start;
namespace MowerHelper
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
           filters.Add(new SessionExpireFilterAttribute());
            filters.Add(new HandleExceptionAttribute());
        // filters.Add(new ValidateAntiForgeryTokenOnAllPosts());
        }
    }
}