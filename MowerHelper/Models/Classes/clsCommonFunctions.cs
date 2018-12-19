using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Drawing;
using MowerHelper.Models.BLL.Patients;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MowerHelper.Models.BLL.Admin;
namespace MowerHelper.Models.Classes
{
    public class clsCommonFunctions:clsDBWrapper
    {
        public clsCommonFunctions(string InstanceName = "")
        {
            try
            {
                if (InstanceName.Equals(""))
                {
                    objDB = DatabaseFactory.CreateDatabase();
                }
                else
                {
                    objDB = DatabaseFactory.CreateDatabase(InstanceName);
                    SetConnection(objDB);
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clscommonfunctions", "clsCommonFunctions", HttpContext.Current.Request, HttpContext.Current.Session);
                //clsCustomEx = null;
            }
         }
        public SqlDataReader GetPatientfullnameSearch(int PracticeID, int ProviderID)
        {
            SqlDataReader dr = default(SqlDataReader);
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objparam = {
		new SqlParameter("@In_practice_id", (PracticeID != 0 ? PracticeID.ToString(): null)),
		new SqlParameter("@in_provider_id", (ProviderID != 0 ? ProviderID.ToString() : null))
	};
                objcommon.AddInParameters(objparam);
               
                dr = objcommon.GetDataReader("Help_dbo.St_practice_Typeahead_Patientname");
                return dr;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clscommonfunctions", "GetPatientfullnameSearch", HttpContext.Current.Request, HttpContext.Current.Session);
                //clsCustomEx = null;
                return dr;
            }
        }
        public string GetCaptcha(string length)
        {
             try
            {
            SqlDataReader drcaptcha = default(SqlDataReader);
            IDataParameter[] objparam = {
			new SqlParameter("@in_number", length)};
            AddInParameters(objparam);
            drcaptcha = GetDataReader("Help_dbo.ST_get_random_alphanummeric");
            if (drcaptcha.Read())
            {
                return drcaptcha["Captcha"].ToString();
            }
            return null;
            }
             catch (Exception ex)
             {
                 clsExceptionLog clsCustomEx = new clsExceptionLog();
                 clsCustomEx.LogException(ex, "clscommonfunctions", "GetCaptcha", HttpContext.Current.Request, HttpContext.Current.Session);
                 //clsCustomEx = null;
                 return null;
             }
        }
        public string GetSiteInfo(string SitePageID, object SiteCategory_ID, string Application_ID)
        {
            try
            {
                SqlDataReader drSite = default(SqlDataReader);
                IDataParameter[] objparam = {
			new SqlParameter("@In_SitePage_ID", SitePageID),
			new SqlParameter("@In_SiteCategory_ID", SiteCategory_ID),
			new SqlParameter("@In_Application_ID", Application_ID)
		};
                AddInParameters(objparam);
                drSite = GetDataReader("Help_dbo.st_ADMIN_GET_SitePages");
                if (drSite.Read())
                {
                    return drSite["Subject_Text"].ToString();
                }
                return null;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clscommonfunctions", "GetSiteInfo", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public string EmailExistsFunction(string Email)
        {
            try
            {
                IDataParameter[] strParam = { new SqlParameter("@In_email", (Email != null ? Email : null)) };
                IDataParameter[] objOutparameters = { new SqlParameter("@Out_Msg", SqlDbType.VarChar, 100, null) };
                AddInParameters(strParam);
                AddOutParameters(objOutparameters);
                ExecuteNonQuerySP("Help_dbo.st_EMailalreadyexists");
                if (objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString() != null & objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString() != "")
                {
                    return objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clscommonfunctions", "EmailExistsFunction", HttpContext.Current.Request, HttpContext.Current.Session);
                //clsCustomEx = null;
                return null;
            }
        }
        public string getprovidername( string Prov_ID)
{
	try {
		string ProviderName = "";
		SqlDataReader DRProvinfo = default(SqlDataReader);
		IDataParameter[] ObjInParam = {
			new SqlParameter("@in_Role_ID", 4),
			new SqlParameter("@in_Login_ID", null),
			new SqlParameter("@in_Ref_ID", Prov_ID)
		};
		AddInParameters(ObjInParam);
		DRProvinfo = GetDataReader("Help_dbo.st_Get_EntityName");
		if (DRProvinfo.Read()) {
			if ((DRProvinfo["Name"]!=null)) {
				ProviderName = DRProvinfo["Name"].ToString();

			}
		}
		return ProviderName;


	} catch (Exception ex) {
        clsExceptionLog clsCustomEx = new clsExceptionLog();
        clsCustomEx.LogException(ex, "clscommonfunctions", "getprovidername", HttpContext.Current.Request, HttpContext.Current.Session);
	}
    return null;
}
        public Int32 CommandDB(string SpName, IDataParameter[] InobjParameters, IDataParameter RetObjParm)
        {
            try
            {
                string sqlCommand = SpName;
                AddReturnParameters(RetObjParm);
                AddInParameters(InobjParameters);
                Int32 intretval = ExecuteFunction(SpName);
                return intretval;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clscommonfunctions", "CommandDB", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return 0;
        }
        public Int32 GetProviderLoginID(string ProviderID)
        {
            try
            {
                IDataParameter[] paramlist = { new SqlParameter("@in_Provider_ID", ProviderID) };
                IDataParameter ReturnParam = new SqlParameter("@Loc_Login_ID", null);
                return CommandDB("Help_dbo.st_Provider_GET_Provider_UserID", paramlist, ReturnParam);
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clscommonfunctions", "GetProviderLoginID", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return 0;
        }
        public List<PatientRegistration> BindComboPracticeProviderSearch(string PracticeName)
        {
            List<PatientRegistration> objDataList = new List<PatientRegistration>();
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                ICacheManager ConfigCacheManager = CacheFactory.GetCacheManager();
                DataSet dssset = null;
                IDataParameter[] objparam = {
			new SqlParameter("@In_PracticeName", PracticeName)
		};
                objcommon.AddInParameters(objparam);
                dssset = objcommon.GetDataSet("Help_dbo.St_practice_OboutCombo_Providername");
                if (dssset.Tables.Count > 0)
                {
                    if (dssset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dssset.Tables[0].Rows)
                        {
                            PatientRegistration obj = new PatientRegistration();
                            if (dr["ProviderName"] != null)
                            {
                                obj.ProviderName = Convert.ToString(dr["ProviderName"]);
                            }
                            else
                            {
                                obj.ProviderName = null;
                            }
                            if (dr["Provider_ID"] != null)
                            {
                                obj.Provider_ID = (int)dr["Provider_ID"];
                            }
                            else
                            {
                                obj.Provider_ID = null;
                            }
                            if (!DBNull.Value.Equals(dr["Login_ID"]))
                            {
                                obj.Login_ID = Convert.ToInt32(dr["Login_ID"]);
                            }
                            else
                            {
                                obj.Login_ID = null;
                            }
                            objDataList.Add(obj);
                            obj = null;
                        }
                    }
                    return objDataList;
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clscommonfunctions", "BindComboPracticeProviderSearch", HttpContext.Current.Request, HttpContext.Current.Session);
                //clsCustomEx = null;
                return objDataList;
            }
        }
        public string SearchPatientExists(string patid, string Providerid)
        {
            try
            {
                SqlDataReader drinfo = default(SqlDataReader);
                IDataParameter[] objparam = {
			new SqlParameter("@in_provider_id", Providerid),
			new SqlParameter("@in_patient_id", patid)
		};
                AddInParameters(objparam);
                drinfo = GetDataReader("Help_dbo.St_get_patientexists_provider");
                if (drinfo.Read())
                {


                    return drinfo["Patientexist"].ToString();
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clscommonfunctions", "SearchPatientExists", HttpContext.Current.Request, HttpContext.Current.Session);
                //clsCustomEx = null;
                return null;
            }
            return null;
        }
        public string[] returndistinctDS(DataSet DS, string ColumnName)
        {
            try
            {
                int[] Newarr = new int[DS.Tables[0].Rows.Count];
                string[] FinalArray = null;
                int i = 0;
                int count = 0;

                for (i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
                {
                    if ((Array.IndexOf(Newarr, Convert.ToInt32(DS.Tables[0].Rows[i][ColumnName]))) == -1)
                    {
                        Newarr[count] = Convert.ToInt32(DS.Tables[0].Rows[i][ColumnName]);
                        count = count + 1;
                    }
                    else
                    {
                    }
                }
                FinalArray = new string[count];
                for (i = 0; i <= FinalArray.GetUpperBound(0); i++)
                {
                    FinalArray[i] = Newarr[i].ToString();
                }
                return FinalArray;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clscommonfunctions", "returndistinctDS", HttpContext.Current.Request, HttpContext.Current.Session);
                //clsCustomEx = null;
                return null;
            }
        }
        public List<PatientRegistration> BindComboverificationusersearch(string PracticeName, string roleid)
        {
            List<PatientRegistration> objDataList = new List<PatientRegistration>();
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet dssset = null;
                IDataParameter[] objparam = {
			new SqlParameter("@In_userName", PracticeName),
			new SqlParameter("@in_roleID",roleid)
		};
                objcommon.AddInParameters(objparam);
                dssset = objcommon.GetDataSet("Help_dbo.St_practice_OboutCombo_verificationusername");
                if (dssset.Tables.Count > 0)
                {
                    if (dssset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dssset.Tables[0].Rows)
                        {
                            PatientRegistration obj = new PatientRegistration();
                            if (dr["Full_name"] != null)
                            {
                                obj.ProviderName = (string)dr["Full_name"];
                            }
                            else
                            {
                                obj.ProviderName = null;
                            }
                            if (!DBNull.Value.Equals(dr["Login_id"]))
                            {
                                obj.Login_ID = Convert.ToInt32(dr["Login_id"]);
                            }
                            else
                            {
                                obj.Login_ID = null;
                            }
                            objDataList.Add(obj);
                            obj = null;
                        }
                    }
                    return objDataList;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clscommonfunctions", "BindComboverificationusersearch", HttpContext.Current.Request, HttpContext.Current.Session);
                //clsCustomEx = null;
                return objDataList;
            }
            return objDataList;
        }
        public static string UsPhoneFormat(string Phoneno)
        {
            try
            {
                return Phoneno.Substring(0, 3) + "-" + Phoneno.Substring(3, 3) + "-" + Phoneno.Substring(6, 4);
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clscommonfunctions", "UsPhoneFormat", HttpContext.Current.Request, HttpContext.Current.Session);
                //clsCustomEx = null;
                return null;
            }
        }
        public static string TransactionSms(int smsBodyId)
        {
            var Subject = string.Empty;
            var Content = string.Empty;
            try
            {
                //var objconfig = new clsWebConfigsettings();
                if ((clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper()) == "YES")
                {
                    var EMO = EmailMessageOption.GET_EmailMessage_OptionBasedonID(smsBodyId, 0);
                    if ((EMO != null))
                    {
                        Subject = EMO.Msg_Subject ?? null;
                        Content = EMO.Msg_Body ?? null;
                    }
                }
                return Content;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clscommonfunctions", "TransactionSms", HttpContext.Current.Request, HttpContext.Current.Session);
                //clsCustomEx = null;
                return Content;
            }
        }
        public static void TwilioSms(string strbussinessname, string strproviderphone, string strclientphone, string appatdate, string ApptTime, string PayAmount, int ddlPayment)
        {// string clientphone,
            //string appatdate,ApptTime;
            // Do stuff
            //string strbussinessname = string.Empty;
            //string strproviderphone = string.Empty;
            //string strclientphone = string.Empty;
            //if (!string.IsNullOrEmpty(Convert.ToString(Session["PracticeName"])))
            //{
            //    strbussinessname = Convert.ToString(Session["PracticeName"]);
            //}
            //if (!string.IsNullOrEmpty(Convert.ToString(Session["Providerphone"])))
            //{
            //    strproviderphone = Convert.ToString(Session["Providerphone"]);
            //}
            //if (!string.IsNullOrEmpty(clientphone))
            //{
            //    strclientphone = clientphone;
            //    strclientphone = strclientphone.Replace("-", "");
            //}
            try
            {
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["TwilioSms"]))
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["TwilioSms"].ToUpper() == "Y")
                    {
                        if ((!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AccountSid"])) & (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AuthToken"])))
                        {
                            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["TwilosmsPhone"]))
                            {

                                string AccountSid = System.Configuration.ConfigurationManager.AppSettings["AccountSid"];
                                string AuthToken = System.Configuration.ConfigurationManager.AppSettings["AuthToken"];
                                if ((!string.IsNullOrEmpty(AccountSid)) & (!string.IsNullOrEmpty(AuthToken)))
                                {
                                    //var Subject = string.Empty;
                                    var Content = string.Empty;
                                    //var objconfig = new clsWebConfigsettings();
                                    //if ((objconfig.GetConfigSettingsValue("SendMailFlag").ToUpper()) == "YES")
                                    //{
                                    //    var EMO = EmailMessageOption.GET_EmailMessage_OptionBasedonID(88, 0);
                                    //    if ((EMO != null))
                                    //    {
                                    //        Subject = EMO.Msg_Subject ?? null;
                                    //       Content = EMO.Msg_Body ?? null;
                                    //    }
                                    //}
                                    Content = clsCommonFunctions.TransactionSms(88);
                                    var twilio = new Twilio.TwilioRestClient(AccountSid, AuthToken);
                                    var msg = string.Empty;
                                    // string str_Content = string.Empty;
                                    if (!(string.IsNullOrEmpty(Content)))
                                    {
                                        //if (appatdate != null)
                                        //{
                                        //    msg = "Hello from " + strbussinessname + " This is to confirm your appointment, on " + appatdate + " at " + ApptTime + ". If needed please call us at  " + strproviderphone + ".";
                                        //}
                                        //if (!(string.IsNullOrEmpty(strbussinessname)))                    
                                        //{
                                        //    Content = Content.Replace("$BussinessName$", strbussinessname);
                                        //}
                                        if (!(string.IsNullOrEmpty(appatdate)))
                                        {
                                            Content = Content.Replace("$BussinessName$", strbussinessname);
                                            Content = Content.Replace("$AppatDate$", appatdate);
                                            Content = Content.Replace("$ApptTime$", ApptTime);
                                            Content = Content.Replace("$ProviderPhone$", strproviderphone);
                                        }
                                    }
                                    //if (!(string.IsNullOrEmpty(ApptTime)))
                                    //{
                                    //    Content = Content.Replace("$ApptTime$", ApptTime);
                                    //}
                                    //if (!(string.IsNullOrEmpty(strproviderphone)))
                                    //{
                                    //    Content = Content.Replace("$ProviderPhone$", strproviderphone);
                                    //}
                                    if ((ddlPayment == 2) || (ddlPayment == 3))
                                    {
                                        Content = clsCommonFunctions.TransactionSms(89);
                                        Content = Content.Replace("$BussinessName$", strbussinessname);
                                        Content = Content.Replace("$PayAmount$", PayAmount);
                                        //msg = "Hello from " + strbussinessname + "! Thank you for your payment of $" + PayAmount;
                                    }
                                    else if (ddlPayment == 1)
                                    {
                                        // msg = "Hello from " + strbussinessname + "! Thank you for your business. Our service fee is $ " + PayAmount;
                                        Content = clsCommonFunctions.TransactionSms(90);
                                        Content = Content.Replace("$BussinessName$", strbussinessname);
                                        Content = Content.Replace("$PayAmount$", PayAmount);
                                    }

                                    //var msg = "Hello from " + strbussinessname + " This is to confirm your appointment, on " + strDates + " at " + strappointmentTime + ". If needed please call us at  " + strproviderphone + ".";
                                    if (!string.IsNullOrEmpty(strclientphone))
                                    {
                                        if (strclientphone == "8106095419")
                                        {
                                            strclientphone = "+91" + strclientphone;
                                        }
                                        else if (strclientphone == "8885552546")
                                        {
                                            strclientphone = "+91" + strclientphone;
                                        }
                                        else
                                        {
                                            strclientphone = "+1" + strclientphone;
                                        }

                                        // msg = !(string.IsNullOrEmpty(Convert.ToString(msg))) ? msg : Content;
                                        var message = twilio.SendMessage(System.Configuration.ConfigurationManager.AppSettings["TwilosmsPhone"], strclientphone, Content);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clscommonfunctions", "TwilioSms", HttpContext.Current.Request, HttpContext.Current.Session);
                //clsCustomEx = null;

            }
        }
    }
}