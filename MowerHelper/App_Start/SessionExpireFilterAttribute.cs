using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace MowerHelper.App_Start
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
           // session.Timeout = 480;
            if (filterContext.Controller.ToString() == "MowerHelper.Controllers.ToolsController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.BillingController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.MessagesController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.AdminController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.DirectoryController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.NotesController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.PracticeController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.ProfileMaintenanceController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.ClientsController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.ClientRegController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.OfficeExpensesController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.ProgressReportsController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.ScheduleController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.TaskController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.FAQsController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.AdminFaqsController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.AdminHomeController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.AdminprofileController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.CREDITCARDController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.TechnicalareaController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.VerificationUserController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.AdminareaController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.ServicesController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.CommonController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.ForumsController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.ProfilesController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.ProvidersController" || filterContext.Controller.ToString() == "MowerHelper.Controllers.ResellersController")
            {
                
                if (session["UserID"] == null)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        var methodName = filterContext.ActionDescriptor.ActionName;
                        if (methodName != "GetCitiesByStates" && methodName != "GetStateByZip" && methodName != "GetStateCitiesByZip" && methodName != "GetCitiesByZip" && methodName != "Getaddress" && methodName != "GetPatientInfo" && methodName != "GetrefundDetails12" && methodName != "GetMapAddressDetails")
                        {
                            if (filterContext.HttpContext.Request.HttpMethod.ToUpper() == "GET")
                            {
                                filterContext.Result = new RedirectToRouteResult(
                                                                  new RouteValueDictionary {
                                { "Controller", "Home" },
                                { "Action", "SessionExpirepopup" }
                        });
                            }
                            else
                            {
                                filterContext.Result = new JsonResult { Data = "_Logon_",JsonRequestBehavior=JsonRequestBehavior.AllowGet };
                            }

                        }
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary {
                                { "Controller", "Home" },
                                { "Action", "SessionExpire" }
                        });
                    }

                    return;
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}