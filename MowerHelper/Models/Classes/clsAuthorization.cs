using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
namespace MowerHelper.Models.Classes
{
    public class clsAuthorization:clsDBWrapper
    {

        public int IsValidUser(string UserName, string Password, ref string OutMsg)
        {
            var objmd5 = new VBVMD5CryptoServiceProvider();
            string strOutMessage = null;
            int isvaliduser1 = 3;

            IDataParameter[] objInparameters ={new SqlParameter("@in_UserName", UserName),
                                                new SqlParameter("@in_password", objmd5.getMd5Hash(Password))};
            AddInParameters(objInparameters);
            SqlDataReader objUserDataReader = GetDataReader("Help_dbo.st_Security_Authenticate_Login_new");

            if (objUserDataReader.Read())
            {
                if (objUserDataReader["validMessage1"] == DBNull.Value)
                {
                    if (objUserDataReader["RP_User_Ind"].ToString() == "0")
                    {
                        if (objUserDataReader["CCexists"].ToString() != null)
                        {
                            HttpContext.Current.Session.Add("CCexists", objUserDataReader["CCexists"].ToString());
                        }
                        if (objUserDataReader["ccexpiry"].ToString() != null)
                        {
                            HttpContext.Current.Session.Add("ccexpiry", objUserDataReader["ccexpiry"].ToString());
                        }
                        if (objUserDataReader["Message1"].ToString() != null && objUserDataReader["Message1"].ToString() != "")
                        {
                            HttpContext.Current.Session.Add("Msgtext1", objUserDataReader["Message1"].ToString());
                            HttpContext.Current.Session.Add("Msgitem", "1");
                        }
                        else if (objUserDataReader["Message2"].ToString() != null && objUserDataReader["Message2"].ToString() != "")
                        {
                            HttpContext.Current.Session.Add("Msgtext2", objUserDataReader["Message2"].ToString());
                            HttpContext.Current.Session.Add("Msgitem", "2");
                        }
                        else if (objUserDataReader["Message3"].ToString() != null && objUserDataReader["Message3"].ToString() != "")
                        {
                            HttpContext.Current.Session.Add("Msgtext3", objUserDataReader["Message3"].ToString());
                            HttpContext.Current.Session.Add("Msgitem", "3");
                        }
                        else if (objUserDataReader["Message4"].ToString() != null && objUserDataReader["Message4"].ToString() != "")
                        {
                            HttpContext.Current.Session.Add("Msgtext4", objUserDataReader["Message4"].ToString());
                            HttpContext.Current.Session.Add("Msgitem", "4");
                        }
                        else
                        {
                            HttpContext.Current.Session.Add("Msgitem", "5");
                        }
                        if (objUserDataReader["Serviceactive"].ToString() != null)
                        {
                            HttpContext.Current.Session.Add("Serviceactive", objUserDataReader["Serviceactive"].ToString());
                        }
                        if (HttpContext.Current.Session["UserID"] != null)
                        {
                            if (objUserDataReader["Login_ID"].ToString() != null)
                            {
                                if (objUserDataReader["Login_ID"].ToString() != HttpContext.Current.Session["UserID"].ToString())
                                {
                                    OutMsg = "@You have already logged in as a different user in other browser/tab. Please log out from that browser/tab.";
                                    isvaliduser1 = 0;

                                }
                            }
                        }
                        HttpContext.Current.Session.Add("UserID", (objUserDataReader["Login_ID"] ?? null));
                        HttpContext.Current.Session.Add("Licenseverified", (objUserDataReader["Licenseverified"] ?? null));
                        HttpContext.Current.Session.Add("FullName", (objUserDataReader["Name"] ?? null));
                        HttpContext.Current.Session.Add("Registeredin", (objUserDataReader["Registeredin"] ?? null));
                        HttpContext.Current.Session.Add("phoneno_read_ind", (objUserDataReader["phoneno_read_ind"] ?? null));
                        if (HttpContext.Current.Session["UserID"].ToString() != "1")
                        {
                            HttpContext.Current.Session.Add("PracticeName", (objUserDataReader["PracticeName"] ?? null));

                        }
                        HttpContext.Current.Session.Add("Providerphone", (objUserDataReader["Provider_phoneno"] ?? null));
                        if (objUserDataReader["providercount"] != null)
                        {
                            HttpContext.Current.Session.Add("providercount", objUserDataReader["providercount"]);
                        }
                        HttpContext.Current.Session.Add("Prov_ID", (objUserDataReader["Provider_ID"] ?? null));
                        HttpContext.Current.Session.Add("RoleId", (objUserDataReader["Role_ID"] ?? null));
                        HttpContext.Current.Session.Add("RoleName", (objUserDataReader["RoleName"] ?? null));
                        if (objUserDataReader["Practice_ID"] != null)
                        {
                            HttpContext.Current.Session.Add("Practice_ID", objUserDataReader["Practice_ID"]);
                        }
                        if (objUserDataReader["PlaceofService_ID"] != null)
                        {
                            HttpContext.Current.Session.Add("PrimaryPOSID", objUserDataReader["PlaceofService_ID"]);
                        }
                        isvaliduser1 = 1;
                        if (objUserDataReader["lastlogintime_web"] != null)
                        {
                            HttpContext.Current.Session.Add("BeforeLoginDate", objUserDataReader["lastlogintime_web"]);
                        }
                        if (objUserDataReader["lastlogintime_mobile"] != null)
                        {
                            HttpContext.Current.Session.Add("MobileLoginDate", objUserDataReader["lastlogintime_mobile"]);
                        }
                        UserLoginHistory(UserName, objUserDataReader["Password"].ToString(), 1, null);
                        if (objUserDataReader["Role_ID"].ToString() == "39")
                        {
                            HttpContext.Current.Session.Add("Techlog_id", objUserDataReader["Technicianlogin_id"]);
                        }
                    }
                    else if (objUserDataReader["RP_User_Ind"].ToString() == "1")
                    {
                        if (HttpContext.Current.Session["UserID"].ToString() != null)
                        {
                            if (objUserDataReader["Login_ID"].ToString() != null)
                            {
                                if (objUserDataReader["Login_ID"].ToString() != HttpContext.Current.Session["UserID"].ToString())
                                {
                                    OutMsg = "@You have already logged in as a different user in other browser/tab. Please log out from that browser/tab.";
                                    isvaliduser1 = 0;

                                }
                            }
                        }
                        HttpContext.Current.Session.Add("userid", objUserDataReader["Login_ID"]);
                        HttpContext.Current.Session.Add("FullName", objUserDataReader["Name"]);
                        HttpContext.Current.Session.Add("RoleId", (objUserDataReader["Role_ID"] ?? null));
                        if (objUserDataReader["Practice_ID"] != null)
                        {
                            HttpContext.Current.Session.Add("Practice_ID", objUserDataReader["Practice_ID"]);
                        }
                        isvaliduser1 = 2;
                        if (HttpContext.Current.Session["Roleid"].ToString() == "5")
                        {
                            HttpContext.Current.Session.Add("PatientID", objUserDataReader["Reference_ID"]);
                        }
                        if (objUserDataReader["lastlogintime_web"] != null)
                        {
                            HttpContext.Current.Session.Add("BeforeLoginDate", objUserDataReader["lastlogintime_web"]);
                        }
                        if (objUserDataReader["lastlogintime_mobile"] != null)
                        {
                            HttpContext.Current.Session.Add("MobileLoginDate", objUserDataReader["lastlogintime_mobile"]);
                        }
                        UserLoginHistory(UserName, objUserDataReader["Password"].ToString(), 1, null);


                    }

                }
                else if (objUserDataReader["validMessage1"] != null)
                {
                    strOutMessage = objUserDataReader["validMessage1"].ToString();
                    isvaliduser1 = 0;
                    UserLoginHistory(UserName, Password, 0, strOutMessage);
                }
            }
            if (strOutMessage != null)
            {
                OutMsg = strOutMessage;
            }
            return isvaliduser1;

        }

        public void UserLoginHistory(string UserName,string Password,Int16 intsucessind, string out_msg)
        {
            try
            {
          string remoteAdd;
        int cookieenableind;
        int javaenableind ;
        int boolonLine;
        string strsystemLanguage =null;
        string struserAgent  = null;
            if(HttpContext.Current.Request.ServerVariables["remote_addr"]!=null)
            {
                 remoteAdd = HttpContext.Current.Request.ServerVariables["remote_addr"].Length>50 ? HttpContext.Current.Request.ServerVariables["remote_addr"].Substring(1, 50) : HttpContext.Current.Request.ServerVariables["remote_addr"];
            }
            else
            {
                remoteAdd=null;
            }
            if (HttpContext.Current.Request.Form["cookieEnabled"] != null)
            {
                cookieenableind = HttpContext.Current.Request.Form["cookieEnabled"].ToUpper() == "TRUE" ? 1 : 0;
            }
            else
            {
                cookieenableind = 0;
            }
            if (HttpContext.Current.Request.Form["javaEnabled"] != null)
            {
                javaenableind = HttpContext.Current.Request.Form["javaEnabled"].ToUpper() == "TRUE" ? 1 : 0;
            }
            else
            {
                javaenableind = 0;
            }
            if (HttpContext.Current.Request.Form["onLine"] != null)
            {
                boolonLine = HttpContext.Current.Request.Form["onLine"].ToUpper() == "TRUE" ? 1 : 0;
            }
            else
            {
                boolonLine = 0;
            }
            if (HttpContext.Current.Request.Form["systemLanguage"] != null)
            {
                strsystemLanguage = HttpContext.Current.Request.Form["systemLanguage"].Length > 100 ? HttpContext.Current.Request.Form["systemLanguage"].Substring(1, 100) : HttpContext.Current.Request.Form["systemLanguage"];
            }
            else
            {
                strsystemLanguage = null;
            }
            string StateName = null ;
            string CityName=null;
            if ((HttpContext.Current.Request.Cookies["State"] != null))
                {
                    if (HttpContext.Current.Request.Cookies["State"].Value != null)
                    {
                        StateName = HttpContext.Current.Request.Cookies["State"].Value.Replace("%20", " ");
                    }
                }
            if ((HttpContext.Current.Request.Cookies["City"] != null))
                {
                    if (HttpContext.Current.Request.Cookies["City"].Value != null)
                    {
                        CityName = HttpContext.Current.Request.Cookies["City"].Value.Replace("%20", " ");
                    }
                }
            if (HttpContext.Current.Request.UserAgent != null)
            {
                struserAgent = HttpContext.Current.Request.UserAgent.Length > 1000 ? HttpContext.Current.Request.UserAgent.Substring(1, 1000) : HttpContext.Current.Request.UserAgent;
            }
            else
            {
                struserAgent = null;
            }
            var objcommon = new clsCommonFunctions();
            IDataParameter[]objinHistryparam={ new SqlParameter("@in_Username", UserName), 
                                                                new SqlParameter("@in_Password", Password), 
                                                                new SqlParameter("@in_RP_User_Ind", "0"), 
                                                                new SqlParameter("@in_IPAddress", remoteAdd), 
                                                                new SqlParameter("@in_CookieEnabled_Ind", cookieenableind), 
                                                                new SqlParameter("@in_JavaEnabled_Ind", javaenableind), 
                                                                new SqlParameter("@in_Online_Ind", boolonLine), 
                                                                new SqlParameter("@in_SystemLanguage", strsystemLanguage), 
                                                                new SqlParameter("@in_UserAgent", struserAgent), 
                                                                new SqlParameter("@in_UserLanguage", struserAgent), 
                                                                new SqlParameter("@IN_Success_Ind", intsucessind), 
                                                                new SqlParameter("@in_Project_ID", 20),
                                                                new SqlParameter("@in_State", StateName),
                                                                new SqlParameter("@in_city ", CityName)
                                                                };

            IDataParameter[] outparam = { new SqlParameter("@Out_loginhistory_ID", SqlDbType.VarChar, 100) };
            AddInParameters(objinHistryparam);
            AddOutParameters(outparam);
            ExecuteNonQuerySP("Help_dbo.st_INS_LoginHistory");
                if(GetCurrentCommand.Parameters["@Out_loginhistory_ID"].Value!=null)
            {
                string outMessage=GetCurrentCommand.Parameters["@Out_loginhistory_ID"].Value.ToString();
                HttpContext.Current.Session.Add("Loginhistory_ID", (outMessage ?? null));
            }
            }
            catch(Exception ex)
            {
                HttpContext.Current.Response.Write(ex.ToString());
            }

        }

        [Required(), DisplayName("User name")]
        public string Username { get; set; }

        [Required(), DataType(DataType.Password), DisplayName("Password")]
        public string Password { get; set; }
    }
}