using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using MowerHelper.Models.Classes;

namespace MowerHelper.Controllers
{
    public class ErrorController : Controller
    {
        
        public ActionResult PageNotFound()
        {
           
           // System.Web.HttpContext.Current.Response.Expires = -1;
            Session.RemoveAll();
            Session.Abandon();
           // Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId") { Expires = DateTime.Now.AddDays(-1d) });
            ViewBag.ipaddress = GetIp();
            LogError();
            return View();
        }
        void LogError()
        {
            try
            {
            string Str_CustomerRefer = null;
            Str_CustomerRefer = Session.SessionID + "-" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            IDataParameter[] objInParameters = {
			new SqlParameter("@in_SessionID", Session.SessionID!=null? Session.SessionID :Convert.ToString(DBNull.Value)),
			new SqlParameter("@in_RequestMethod", Request.ServerVariables["REQUEST_METHOD"]),
			new SqlParameter("@in_ServerPort", Request.ServerVariables["SERVER_PORT"]),
			new SqlParameter("@in_HTTPS", Request.ServerVariables["HTTPS"]),
			new SqlParameter("@in_LocalAddr", Request.ServerVariables["LOCAL_ADDR"]),
			new SqlParameter("@in_HostAddress", ViewBag.ipaddress),
			new SqlParameter("@in_UserAgent", Request.ServerVariables["HTTP_USER_AGENT"]),
			new SqlParameter("@in_PageNo", DBNull.Value),
			new SqlParameter("@in_URL", Convert.ToString(System.Web.HttpContext.Current.Request.Url)),
			new SqlParameter("@in_CustomerRefID",Str_CustomerRefer),
			new SqlParameter("@in_FormData", DBNull.Value),
			new SqlParameter("@in_AllHTTP", Request.ServerVariables["ALL_HTTP"]),
			new SqlParameter("@in_ErrASPCode", null),
			new SqlParameter("@in_ErrNumber", null),
			new SqlParameter("@in_Exception_Name", null),
			new SqlParameter("@in_ErrSource", "ErrorController" + ":PageNotFound" ),
			new SqlParameter("@in_ErrCategory", null),
			 new SqlParameter("@in_ErrFile", null),
			new SqlParameter("@in_ErrLine",null),
			new SqlParameter("@in_ErrColumn", "0"),
			new SqlParameter("@in_ErrDescription",null),
			new SqlParameter("@in_ErrAspDescription", null),
			new SqlParameter("@in_CreatedBy", Session["UserID"]!=null?Session["UserID"]:DBNull.Value),
			new SqlParameter("@in_Project_ID", 20),
			new SqlParameter("@in_Class_Name", "ErrorController"),
			new SqlParameter("@in_Method_Name", "PageNotFound")
		};

            IDataParameter[] objOutParameters = {
			new SqlParameter("@Out_ErrorMsg", SqlDbType.VarChar, 500),
			new SqlParameter("@Out_MethodID", SqlDbType.Int, 15),
			new SqlParameter("@Out_ClassID", SqlDbType.Int, 15),
			new SqlParameter("@Out_LogID", SqlDbType.Int, 15)
		};
            clsDBWrapper ObjDb = new clsDBWrapper();

            ObjDb.AddInParameters(objInParameters);
            ObjDb.AddOutParameters(objOutParameters);
            ObjDb.ExecuteNonQuerySP("Help_dbo.st_INS_Errors_ASPX");

            if (ObjDb.GetCurrentCommand.Parameters["@Out_LogID"].Value != null)
            {
                Session.Add("Log_ID", Convert.ToString(ObjDb.GetCurrentCommand.Parameters["@Out_LogID"].Value));
            }
            // Session.Add("Actual_ErrorMessage", ex.StackTrace);
              }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "ErrorController", "LogError", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public ActionResult Display()
        {
          
           // System.Web.HttpContext.Current.Response.Expires = -1;
            Session.RemoveAll();
            Session.Abandon();
           // Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId") { Expires = DateTime.Now.AddDays(-1d) });
            ViewBag.ipaddress = GetIp();
          return View();
        }
        public ActionResult DisplayErrorPopup()
        {
          
          //  System.Web.HttpContext.Current.Response.Expires = -1;
            Session.RemoveAll();
            Session.Abandon();
          //  Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId") { Expires = DateTime.Now.AddDays(-1d) });
            ViewBag.ipaddress = GetIp();
            return View();
        }
        public string GetIp()
        {
            try{
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "ErrorController", "GetIp", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        } 
    }
}
