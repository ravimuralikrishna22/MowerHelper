using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MowerHelper.Models.Classes;
using MowerHelper.Controllers;
using MowerHelper.Models;
namespace MowerHelper.App_Start
{
     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class HandleExceptionAttribute:FilterAttribute,IExceptionFilter
    {
        
        
         public Exception Exception { get; set; }
         public void OnException(ExceptionContext filterContext )
        {
           
            if (filterContext.Exception != null)
            {
                clsExceptionLog obj = new clsExceptionLog();
                string[] Errorparam = filterContext.Exception.StackTrace.ToString().Split(':');
                string path;
                if (Errorparam.Length == 2 || Errorparam.Length >= 3)
                {
                    path = Errorparam[1].ToString();
                    obj.LogException(filterContext.Exception, filterContext.Controller.ToString(), path, System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session, "Y");
                }
                else
                {
                    obj.LogException(filterContext.Exception, filterContext.Controller.ToString(), null, System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                }
                             if (filterContext.HttpContext.Request.IsAjaxRequest())
                             {

                                 filterContext.ExceptionHandled = true;
                                 filterContext.HttpContext.Response.Clear();
                                 filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                                 filterContext.Result = new RedirectToRouteResult(
                                                    new RouteValueDictionary {
                                        { "Controller", "Error" },
                                        { "Action", "DisplayErrorPopup" }
                                });

                                 if (HttpContext.Current.Request.HttpMethod == "POST")
                                 {
                                     filterContext.Result = new JsonResult
                                     {
                                         JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                                         Data = new
                                         {
                                             error = "-Error-"

                                         }
                                     };
                                 }
                               
                                

                             }
                else
                {
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                    filterContext.Result = new RedirectToRouteResult(
                                            new RouteValueDictionary {
                                        { "Controller", "Error" },
                                        { "Action", "Display" }
                                });    
                }
                
              return;
            }
           
        }
    }
}