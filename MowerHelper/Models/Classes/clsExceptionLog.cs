using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Web.Mvc;
namespace MowerHelper.Models.Classes
{
    public class clsExceptionLog
    {
        string ApplicationUrl = null;
        public void LogException(Exception ex, string Str_ControllerName, string Str_ActionName, HttpRequest Request, System.Web.SessionState.HttpSessionState Session,string Actionindicator=null)
        {
           
            try
            {
                string str = HttpContext.Current.Request.Url.ToString();
                string[] strUrl = str.Split('/');
                ApplicationUrl = strUrl[0] + "//" + strUrl[2] + "/" ;
                EnterLogDetails(ex, Str_ControllerName, Str_ActionName, Request, Session, Actionindicator);

            }
            catch (Exception exChild)
            {

                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clsExceptionLog", "LogException", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            finally
            {
              
               // HttpContext.Current.Response.Redirect(ApplicationUrl + "/Views/Shared/Error.cshtml");
            }
        }
        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        } 
        private void EnterLogDetails(Exception ex, string Str_ControllerName, string Str_ActionName, HttpRequest Request, System.Web.SessionState.HttpSessionState Session,string actionindicator)
        {
            try
            {
                string Str_CustomerRefer = null;
                Str_CustomerRefer = Session.SessionID + "-" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                string[] Errorparam = ex.StackTrace.ToString().Split(':');
                string path;
                int line;
                if (Errorparam.Length >= 2)
                {
                    string[] lineno = Errorparam[2].Split('\r');
                    string[] linespace;
                    
                    if (actionindicator == "Y")
                    {
                        path = Errorparam[1].ToString();
                        linespace = lineno[0].ToString().Split(' ');
                        line = Convert.ToInt32(linespace[1].ToString());
                    }
                    else
                    {
                        path = ex.Source;
                        line = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(":line") + 5));
                    }
                }
                else
                {
                    path = null;
                    line = 0;
                }

                IDataParameter[] objInParameters = {
			new SqlParameter("@in_SessionID", Session.SessionID.ToString()!=null? Session.SessionID :Convert.ToString(DBNull.Value)),
			new SqlParameter("@in_RequestMethod", Request.ServerVariables["REQUEST_METHOD"]),
			new SqlParameter("@in_ServerPort", Request.ServerVariables["SERVER_PORT"]),
			new SqlParameter("@in_HTTPS", Request.ServerVariables["HTTPS"]),
			new SqlParameter("@in_LocalAddr", Request.ServerVariables["LOCAL_ADDR"]),
			new SqlParameter("@in_HostAddress", GetIp()),
			new SqlParameter("@in_UserAgent", Request.ServerVariables["HTTP_USER_AGENT"]),
			new SqlParameter("@in_PageNo", DBNull.Value),
			new SqlParameter("@in_URL", HttpContext.Current.Request.Url.ToString()),
			new SqlParameter("@in_CustomerRefID", Str_CustomerRefer!=null?Str_CustomerRefer:Convert.ToString(DBNull.Value)),
			new SqlParameter("@in_FormData", DBNull.Value),
			new SqlParameter("@in_AllHTTP", Request.ServerVariables["ALL_HTTP"]),
			new SqlParameter("@in_ErrASPCode", ex.GetType().ToString()),
			new SqlParameter("@in_ErrNumber", ex.GetType().ToString()),
			new SqlParameter("@in_Exception_Name", ex.GetType().Name),
			new SqlParameter("@in_ErrSource", Str_ControllerName + ":" + Str_ActionName),
			new SqlParameter("@in_ErrCategory", ex.GetType().ToString()),
			 new SqlParameter("@in_ErrFile", path),
			new SqlParameter("@in_ErrLine",line),
			new SqlParameter("@in_ErrColumn", "0"),
			new SqlParameter("@in_ErrDescription", ex.Message),
			new SqlParameter("@in_ErrAspDescription", ex.Message),
			new SqlParameter("@in_CreatedBy", Session["UserID"]!=null?Session["UserID"]:DBNull.Value),
			new SqlParameter("@in_Project_ID", 20),
			new SqlParameter("@in_Class_Name", Str_ControllerName),
			new SqlParameter("@in_Method_Name", Str_ActionName)
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

                if (ObjDb.GetCurrentCommand.Parameters["@Out_LogID"].Value!=null)
                {
                    Session.Add("Log_ID", ObjDb.GetCurrentCommand.Parameters["@Out_LogID"].Value.ToString());
                }
                Session.Add("Actual_ErrorMessage", ex.StackTrace);
            }
            catch (Exception ex1)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clsExceptionLog", "LogException", HttpContext.Current.Request, HttpContext.Current.Session);
            }

        }
    }
}