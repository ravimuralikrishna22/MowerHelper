using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.Classes;

namespace MowerHelper.Models.DAL.DALPATIENT
{
    public class PatientSQLDBlayer : PatientDataAccessLayerBaseClass
    {
        string ClassName = "PatientSQLDBlayer";
        clsCommonFunctions clscommon = new clsCommonFunctions();
        VBVMD5CryptoServiceProvider objmd5 = new VBVMD5CryptoServiceProvider();
        clsDBWrapper objclsdbwraper = new clsDBWrapper();
        public override DataSet getPatientAlphabets()
        {
            try
            {

                IDataParameter[] objparam = {
			//new SqlParameter("@in_Patient_Status_ID", HttpContext.Current.Session["GlbSearch_PatientType"]!=null?HttpContext.Current.Session["GlbSearch_PatientType"]:DBNull.Value),
			//new SqlParameter("@in_Practice_ID", HttpContext.Current.Session["Practice_ID"] !=null ? HttpContext.Current.Session["Practice_ID"] :null),
			new SqlParameter("@in_Provider_ID", HttpContext.Current.Session["Prov_ID"] !=null ? HttpContext.Current.Session["Prov_ID"] :null)
		};
                clsCommonFunctions objcommon = new clsCommonFunctions();
                objcommon.AddInParameters(objparam);
               return objcommon.GetDataSet("Help_dbo.st_Patient_LIST_Alphabets");
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "getPatientAlphabets", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<patients> GetMessageDeatails(int UserID, string Type, int SenderID, string OrderByitem, string orderBy, string Pagesize = null, int Pageno = 0)
        {
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                SqlDataReader NewsDeatails = default(SqlDataReader);
                IDataParameter[] InParamList = {
			new SqlParameter("@in_messagetype", (!string.IsNullOrEmpty(Type) ? Type : null)),
			new SqlParameter("@in_OrderbyItem", OrderByitem),
			new SqlParameter("@in_Order", orderBy),
			new SqlParameter("@in_SenderID", (SenderID != 0 ? Convert.ToInt32(SenderID).ToString() : null)),
			new SqlParameter("@in_noofrowsperpage", (Pagesize != "0" ? Pagesize : "50")),
			new SqlParameter("@in_pageno", (Pageno != 0 ? Pageno : 1))
		};
                objCommon.AddInParameters(InParamList);
                NewsDeatails = objCommon.GetDataReader("Help_dbo.st_list_sentmessages");
                List<patients> objPtMsglist = new List<patients>();
                patients objpat = new patients();
                int SlNo = Convert.ToInt32(Pagesize) * (Pageno - 1);
                while (NewsDeatails.Read())
                {
                    patients objPt = new patients();
                    SlNo += 1;
                    objPt.SLno = SlNo;
                    objPt.Message_ID = Convert.ToInt32(NewsDeatails["Message_ID"].ToString() != null ? NewsDeatails["Message_ID"].ToString() : null);
                    objPt.SendingDate = (NewsDeatails["SendingDate"].ToString() != null ? NewsDeatails["SendingDate"].ToString() : null);
                    objPt.Sender = (NewsDeatails["Sender"].ToString() != null ? NewsDeatails["Sender"].ToString() : null);
                    objPt.Patientname = (NewsDeatails["PatientName"].ToString() != null ? NewsDeatails["PatientName"].ToString() : null);
                    objPt.topic = (NewsDeatails["topic"].ToString() != null ? NewsDeatails["topic"].ToString() : null);
                    objPt.Receiver = (NewsDeatails["Receiver"].ToString() != null ? NewsDeatails["Receiver"].ToString() : null);
                    objPt.AttachmentFilename = (NewsDeatails["AttachmentFilename"].ToString() != null ? NewsDeatails["AttachmentFilename"].ToString() : null);
                    objPt.MessageBody = (NewsDeatails["MessageBody"].ToString() != null ? NewsDeatails["MessageBody"].ToString() : null);
                    objPt.Reciever_ID = Convert.ToInt32(NewsDeatails["Reciever_ID"].ToString() != "" ? NewsDeatails["Reciever_ID"].ToString() : null);
                    objPt.ReadInd = (NewsDeatails["Read_Ind"].ToString() != null ? NewsDeatails["Read_Ind"].ToString() : null);
                    objPt.Reply_ID = Convert.ToInt32(NewsDeatails["Reply_ID"].ToString() !="" ? NewsDeatails["Reply_ID"].ToString() : null);
                    objPtMsglist.Add(objPt);
                }
                NewsDeatails.NextResult();
                if (NewsDeatails.Read())
                {
                    if (NewsDeatails["MsgCount"] != null)
                    {
                        patients.MsgCount = Convert.ToInt32(NewsDeatails["MsgCount"]);

                    }
                }

                return objPtMsglist;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetMessageDeatails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override Patient_Info ViewPatineInfo(Patient_Info objpatient)
        {
             SqlDataReader objread = default(SqlDataReader);
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objInParamList = {
		new SqlParameter("@in_AuthorizedProvider_ID", objpatient.ProviderID),
		new SqlParameter("@in_Patient_ID", objpatient.PatientID),
		new SqlParameter("@in_Practice_ID", objpatient.PracticeID)
	};
                objcommon.AddInParameters(objInParamList);
                objread = objcommon.GetDataReader("Help_dbo.st_ProviderPatient_Edit_GET_PersonalInfo");
                if (objread.Read() == true)
                {
                    objpatient = new Patient_Info();
                    objpatient.Prefix = objread["Prefix"] != null ? objread["Prefix"].ToString() : null;
                    objpatient.FirstName = objread["FirstName"] != null ? objread["FirstName"].ToString() : null;
                    objpatient.MI= objread["MI"]!=null ? objread["MI"].ToString() : null;
                    objpatient.LastName=objread["LastName"]!=null ? objread["LastName"].ToString() : null;
                    objpatient.Suffix=objread["Suffix"]!=null ? objread["Suffix"].ToString() : null;
                    objpatient.DrivingLicenceNo = objread["DrivingLicenceNo"].ToString() != "" ? objread["DrivingLicenceNo"].ToString() : null;
                    objpatient.PatientEmail=objread["PatientEmail"]!=null ? objread["PatientEmail"].ToString() : null;
                    objpatient.Address1=objread["Address1"]!=null ? objread["Address1"].ToString() : null;
                    objpatient.Address2=objread["Address2"]!=null ? objread["Address2"].ToString() : null;
                    objpatient.ZIP=objread["ZIP"]!=null ? objread["ZIP"].ToString() : null;
                    objpatient.State=objread["State"]!=null ? objread["State"].ToString() : null;
                    objpatient.City=objread["City"]!=null ? objread["City"].ToString() : null;
                    objpatient.HomePhone=objread["HomePhone"]!=null ? objread["HomePhone"].ToString() : null;
                    objpatient.WorkPhone=objread["WorkPhone"] != null ? objread["WorkPhone"].ToString() : null;
                    objpatient.MobilePhone = objread["CellPhone"] != null ? objread["CellPhone"].ToString() : null;
                    objpatient.State_ID = objread["State_ID"] != null ? objread["State_ID"].ToString() : null;
                    objpatient.City_ID = objread["City_ID"] != null ? objread["City_ID"].ToString() : null;
                    objpatient.ProviderPatient_ID = objread["ProviderPatient_ID"] != null ? objread["ProviderPatient_ID"].ToString() : null;
                    objpatient.PatientLoginID = objread["PatientLogin_ID"] != null ? objread["PatientLogin_ID"].ToString() : null;
                    objpatient.LastVisit = objread["LastAppointment"] != null ? objread["LastAppointment"].ToString() : null;
                    objpatient.NextVisit = objread["NextAppointment"] != null ? objread["NextAppointment"].ToString() : null;
                    objpatient.RegisteredDate = objread["Active_date"] != null ? objread["Active_date"].ToString() : null;
                    objpatient.Patientname = objread["Patientname"] != null ? objread["Patientname"].ToString() : null;
                    objpatient.l_GoogleMapPath = objread["l_GoogleMapPath"] != null ? objread["l_GoogleMapPath"].ToString() : null;
                    objpatient.GmapLatitude = objread["LATITUDE"] != null ? objread["LATITUDE"].ToString() : null;
                    objpatient.GmapLongitude = objread["LONGITUDE"] != null ? objread["LONGITUDE"].ToString() : null;
                    return objpatient;
                }
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "ViewPatineInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;

           

        }
    //    public override DataSet get_patient_parties_info( int? patientid, int PracticeID)
    //    {
    //        try
    //        {
    //            clsCommonFunctions clscommon = new clsCommonFunctions();
    //            DataSet dspatientinfo = new DataSet();
    //            IDataParameter[] paramlist = {
    //    new SqlParameter("@In_Patient_id", patientid),
    //    new SqlParameter("@In_Practice_id", PracticeID)
    //};
    //            clscommon.AddInParameters(paramlist);
               
    //            dspatientinfo = clscommon.GetDataSet("Help_dbo.St_Practice_GET_PatientOtherprofiles");
    //            if ((dspatientinfo != null))
    //            {
    //                if (dspatientinfo.Tables[0].Rows.Count > 0)
    //                {
    //                    return dspatientinfo;
    //                }
    //                else
    //                {
    //                    return null;
    //                }
    //            }
    //            else
    //            {
    //                return null;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            clsExceptionLog clsex = new clsExceptionLog();
    //            clsex.LogException(ex, "PatientSQLDBLayer", "get_patient_parties_info", HttpContext.Current.Request, HttpContext.Current.Session);
    //        }
    //        return null;
    //    }
        public override string EditPatientPersonalInfo(Patient_Info objpatient, ref string Out_Msg)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();

                IDataParameter[] paramlist = {
		new SqlParameter("@in_ProviderPatient_ID", objpatient.ProviderPatient_ID),
		new SqlParameter("@in_Title", objpatient.Prefix),
		new SqlParameter("@in_FirstName", objpatient.FirstName),
		new SqlParameter("@in_MI", objpatient.MI),
		new SqlParameter("@in_LastName", objpatient.LastName),
		new SqlParameter("@in_Suffix", objpatient.Suffix),
		new SqlParameter("@in_PatientEmail", objpatient.PatientEmail),
		new SqlParameter("@in_HomePhone", objpatient.HomePhone),
		new SqlParameter("@in_WorkPhone", objpatient.WorkPhone),
		new SqlParameter("@in_CellPhone", objpatient.MobilePhone),
		new SqlParameter("@in_Address1", objpatient.Address1),
		new SqlParameter("@in_Address2", objpatient.Address2),
		new SqlParameter("@in_City_ID", objpatient.City_ID),
		new SqlParameter("@in_State_ID", objpatient.State_ID),
		new SqlParameter("@in_Country_ID", 1),
		new SqlParameter("@in_ZIP", objpatient.ZIP),
		new SqlParameter("@in_DrivingLicenceNO", objpatient.DrivingLicenceNo),
		new SqlParameter("@in_UpdatedBy", HttpContext.Current.Session["userID"]),
		new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"]),
        new SqlParameter("@In_Latitude",objpatient.GmapLatitude),
        new SqlParameter("@In_Longitude",objpatient.GmapLongitude),
        new SqlParameter("@In_Ind",objpatient.Ind)
	};
                IDataParameter[] objOutparameters = {
			new SqlParameter("@Out_Msg", SqlDbType.VarChar, 100, null)
		};
                //IDataParameter[] OutParam = { new SqlParameter("@Out_Msg", SqlDbType.VarChar, 200) };

                var _with1 = objcommon;
                _with1.AddInParameters(paramlist);
                _with1.AddOutParameters(objOutparameters);
                _with1.ExecuteNonQuerySP("Help_dbo.st_ProviderPatient_Edit_UPD_PersonalInfo");
                //if (objcommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString()!="")
                //{
                //    return objcommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString();
                //}
                //else
                //{
                //    return null;
                //}
                if (_with1.objdbCommandWrapper.Parameters["@Out_Msg"].Value != null)
                {
                    Out_Msg = _with1.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString();
                }
                else
                {
                    Out_Msg = null;
                }                
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "PatientSQLDbLayer", "EditPatientPersonalInfo", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<patients> GetServiceRenewalsMailStatus(string ToreferenceId)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader objdr = default(SqlDataReader);
                IDataParameter[] strInsParam = { new SqlParameter("@In_ToReference_IDs", ToreferenceId) };
                objcommon.AddInParameters(strInsParam);
                objdr = objcommon.GetDataReader("Help_dbo.st_Admin_get_SendMessages");
                List<patients> servicelst = new List<patients>();
                while (objdr.Read())
                {
                    patients objServiceInfo = new patients();
                    objServiceInfo.EmailMessage_Transaction_ID = Convert.ToInt32(objdr["EmailMessage_Transaction_ID"] != null ? objdr["EmailMessage_Transaction_ID"] : null);
                    objServiceInfo.Mail_To = Convert.ToString(objdr["Mail_To"] != null ? objdr["Mail_To"] : null);
                    objServiceInfo.Provider_ID = Convert.ToInt32(objdr["ToRefernce_ID"] != null ? objdr["ToRefernce_ID"] : null);
                    objServiceInfo.Mail_From = Convert.ToString(objdr["Mail_From"] != null ? objdr["Mail_From"] : null);
                    objServiceInfo.Subject = Convert.ToString(objdr["Subject"] != null ? objdr["Subject"] : null);
                    objServiceInfo.Message_Body = Convert.ToString(objdr["Message_Body"] != null ? objdr["Message_Body"] : null);
                    servicelst.Add(objServiceInfo);

                }
                return servicelst;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetServiceRenewalsMailStatus", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<patients> GetlicenseRenewalsMailStatus(string ToreferenceId)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader objdr = default(SqlDataReader);
                IDataParameter[] strInsParam = { new SqlParameter("@In_ToReference_IDs", ToreferenceId) };
                objcommon.AddInParameters(strInsParam);
                objdr = objcommon.GetDataReader("st_Admin_get_licenseexpirySendMessages");
                List<patients> servicelst = new List<patients>();

                while (objdr.Read())
                {
                    patients objServiceInfo = new patients();

                    objServiceInfo.EmailMessage_Transaction_ID = Convert.ToInt32(objdr["EmailMessage_Transaction_ID"] != null ? objdr["EmailMessage_Transaction_ID"] : null);
                    objServiceInfo.Mail_To = Convert.ToString(objdr["Mail_To"] != null ? objdr["Mail_To"] : null);
                    objServiceInfo.Provider_ID = Convert.ToInt32(objdr["ToRefernce_ID"] != null ? objdr["ToRefernce_ID"] : null);
                    objServiceInfo.Mail_From = Convert.ToString(objdr["Mail_From"] != null ? objdr["Mail_From"] : null);
                    objServiceInfo.Subject = Convert.ToString(objdr["Subject"] != null ? objdr["Subject"] : null);
                    objServiceInfo.Message_Body = Convert.ToString(objdr["Message_Body"] != null ? objdr["Message_Body"] : null);
                    servicelst.Add(objServiceInfo);

                }
                return servicelst;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetlicenseRenewalsMailStatus", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override patients Get_licenseexpireUserDetails(string Provider_ID)
        {
            try
            {
                SqlDataReader objread = default(SqlDataReader);
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objInparamlist = { new SqlParameter("@In_provider_ID", (Provider_ID != "0" ? Provider_ID : null)) };

                objcommon.AddInParameters(objInparamlist);
                objread = objcommon.GetDataReader("st_Security_GET_licenseexpire_Provider");
                patients ObjSecurity = new patients();
                if (objread.Read() == true)
                {
                    if (objread["Full_name"] != null)
                    {
                        ObjSecurity.Name = Convert.ToString(objread["Full_name"]);
                    }
                    else
                    {
                        ObjSecurity.Name = null;
                    }
                    if (objread["LicenseExpirydate"] != null)
                    {
                        ObjSecurity.licenseexpiredate = Convert.ToString(objread["LicenseExpirydate"]);
                    }
                    else
                    {
                        ObjSecurity.licenseexpiredate = null;
                    }

                }
                return ObjSecurity;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Get_licenseexpireUserDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet getProviderAlphabet()
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                
                DataSet dsProvideralphabets = new DataSet();
                dsProvideralphabets = objcommon.GetDataSet("Help_dbo.st_Provider_LIST_Alphabets");
                return dsProvideralphabets;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "getProviderAlphabet", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override DataSet GetAdminInfo(string LoginID)
        {
            try
            {
                DataSet dsInfo = new DataSet();
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] strParam = { new SqlParameter("@In_LoginID", LoginID) };
                objcommon.AddInParameters(strParam);
                dsInfo = objcommon.GetDataSet("Help_dbo.st_Admin_GET_AdminInfo");
                return dsInfo;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetAdminInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override string updateAdminInfo(AdminProfile objProfile)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] strBasicInfoParam = {
			new SqlParameter("@in_Login_ID", HttpContext.Current.Session["userID"]),
			new SqlParameter("@In_UserName", (string.IsNullOrEmpty(objProfile.Username) ? null : objProfile.Username)),
			new SqlParameter("@In_Password", (string.IsNullOrEmpty(objProfile.Password) ? null : objmd5.getMd5Hash(objProfile.Password))),
			new SqlParameter("@In_EncryptPassword", (string.IsNullOrEmpty(objProfile.Password) ? null : objmd5.getMd5Hash(objProfile.Password))),
			new SqlParameter("@In_PREFIX", (string.IsNullOrEmpty(objProfile.Prefix) ? null : objProfile.Prefix)),
			new SqlParameter("@In_FIRSTNAME", (string.IsNullOrEmpty(objProfile.FirstName) ? null : objProfile.FirstName)),
			new SqlParameter("@In_MI", (string.IsNullOrEmpty(objProfile.MI) ? null : objProfile.MI)),
			new SqlParameter("@In_LASTNAME", (string.IsNullOrEmpty(objProfile.LastName) ? null : objProfile.LastName)),
			new SqlParameter("@In_SUFFIX", (string.IsNullOrEmpty(objProfile.Suffix) ? null : objProfile.Suffix)),
			new SqlParameter("@in_NICKNAME", (string.IsNullOrEmpty(objProfile.NickName) ? null : objProfile.NickName)),
			new SqlParameter("@in_Email1", (string.IsNullOrEmpty(objProfile.Email) ? null : objProfile.Email)),
			new SqlParameter("@in_CompanyName", (string.IsNullOrEmpty(objProfile.CompanyName) ? null : objProfile.CompanyName)),
			new SqlParameter("@In_Address1", (string.IsNullOrEmpty(objProfile.Address1) ? null : objProfile.Address1)),
			new SqlParameter("@In_Address2", (string.IsNullOrEmpty(objProfile.Address2) ? null : objProfile.Address2)),
			new SqlParameter("@In_City_ID", (string.IsNullOrEmpty(objProfile.City_ID) ? null : objProfile.City_ID)),
			new SqlParameter("@In_State_ID", (string.IsNullOrEmpty(objProfile.State_ID) ? null : objProfile.State_ID)),
			new SqlParameter("@In_Country_ID", 19),
			new SqlParameter("@In_ZIP", (string.IsNullOrEmpty(objProfile.ZIP) ? null : objProfile.ZIP)),
			new SqlParameter("@In_HomePhone", (string.IsNullOrEmpty(objProfile.HomePhone) ? null : objProfile.HomePhone)),
			new SqlParameter("@In_WorkPhone", (string.IsNullOrEmpty(objProfile.WorkPhone) ? null : objProfile.WorkPhone)),
			new SqlParameter("@In_MobilePhone", (string.IsNullOrEmpty(objProfile.MobilePhone) ? null : objProfile.MobilePhone)),
			new SqlParameter("@In_Fax", (string.IsNullOrEmpty(objProfile.Fax) ? null : objProfile.Fax)),
			new SqlParameter("@In_Website", (string.IsNullOrEmpty(objProfile.Website) ? null : objProfile.Website)),
			new SqlParameter("@In_DateofBirth", (string.IsNullOrEmpty(objProfile.DateofBirth) ? null : objProfile.DateofBirth)),
			new SqlParameter("@in_ReferenceType_ID", 1),
			new SqlParameter("@in_UpdatedBy", HttpContext.Current.Session["userid"]),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};
                IDataParameter[] strOutParam = { new SqlParameter("@out_Msg", SqlDbType.Char, 50) };
                objcommon.AddInParameters(strBasicInfoParam);
                objcommon.AddOutParameters(strOutParam);
                string strMsg = "";
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Admin_UPD_adminInfo");
                if ((objcommon.objdbCommandWrapper.Parameters["@out_Msg"].Value)!=null)
                {
                    strMsg = objcommon.objdbCommandWrapper.Parameters["@out_Msg"].Value.ToString();
                }
                return strMsg;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "updateAdminInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override System.Collections.Generic.List<EmailOptions> Emails_List(EmailOptions objEmail, ref int Total_records)
        {
            try
            {
                clsCommonFunctions clscommon = new clsCommonFunctions();
                SqlDataReader objread = null;
                IDataParameter[] objinparam = { new SqlParameter("@In_EmailMessageOptionType_ID", objEmail.EmailMessageOptionType_ID) };
                clscommon.AddInParameters(objinparam);
                objread = clscommon.GetDataReader("Help_dbo.st_ADMIN_GET_EmailMessageOptionTypes");
                List<EmailOptions> email = new List<EmailOptions>();
                while (objread.Read())
                {
                    EmailOptions objemaillist = new EmailOptions();
                    objemaillist.EmailMessageOptionType_ID = Convert.ToString(objread["EmailMessageOptionType_ID"]!=null ? objread["EmailMessageOptionType_ID"] : null);
                    objemaillist.Title = Convert.ToString(objread["Title"]!=null ? objread["Title"] : null);
                    objemaillist.Description = Convert.ToString((objread["Description"]!=null ? objread["Description"] : null));
                    email.Add(objemaillist);
                }
                objread.NextResult();
                if (objread.HasRows == true)
                {
                    if (objread.Read())
                    {
                        Total_records = Convert.ToInt32(objread[0]);
                    }
                }
                return email;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Emails_List", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool Emails_insert(EmailOptions objEmail)
        {
            try
            {
                clsDBWrapper objdbwrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
			new SqlParameter("@in_Title", objEmail.Title),
			new SqlParameter("@in_Description", objEmail.Description),
			new SqlParameter("@in_CreatedBy", objEmail.CreatedBy)
		};


                objdbwrapper.AddInParameters(objparm);
                objdbwrapper.ExecuteNonQuerySP("Help_dbo.st_ADMIN_INS_EmailMessageOption_Types");
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Emails_insert", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override bool Emails_upd(EmailOptions objEmail)
        {
            try
            {
                clsDBWrapper objdbwrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
			new SqlParameter("@in_EmailMessageOptionType_ID", (objEmail.EmailMessageOptionType_ID == "0" ? null : objEmail.EmailMessageOptionType_ID)),
			new SqlParameter("@in_Title", objEmail.Title),
			new SqlParameter("@in_Description", objEmail.Description),
			new SqlParameter("@in_UpdatedBy", objEmail.UpdatedBy)
		};


                objdbwrapper.AddInParameters(objparm);
                objdbwrapper.ExecuteNonQuerySP("Help_dbo.st_ADMIN_UPD_Email_OptionTypes");
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Emails_upd", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override EmailOptions Emails_Get(int? EmailMessageOptionType_ID)
        {

            try
            {
                clsDBWrapper objdbwrapper = new clsDBWrapper();
                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] objparm = { new SqlParameter("@In_EmailMessageOptionType_ID", (EmailMessageOptionType_ID == 0 ? null : EmailMessageOptionType_ID)) };
                objdbwrapper.AddInParameters(objparm);
                objread = objdbwrapper.GetDataReader("Help_dbo.st_ADMIN_GET_EmailMessageOptionTypes");
                EmailOptions objData = new EmailOptions();
                if (objread.Read() == true)
                {
                    objData.EmailMessageOptionType_ID = Convert.ToString(objread["EmailMessageOptionType_ID"]!=null ? objread["EmailMessageOptionType_ID"] : null);
                    objData.Title = Convert.ToString(objread["Title"]!=null ? objread["Title"] : null);
                    objData.Description = Convert.ToString((objread["Description"]!=null ? objread["Description"] : null));
                }
                return objData;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Emails_Get", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool Emails_Delete(EmailOptions objEmail)
        {
            try
            {
                IDataParameter[] objParm = {
			new SqlParameter("@in_EmailMessageOptionType_ID", objEmail.EmailMessageOptionType_ID),
			new SqlParameter("@in_UpdatedBy", objEmail.UpdatedBy)
		};

                objclsdbwraper.AddInParameters(objParm);
                objclsdbwraper.ExecuteNonQuerySP("Help_dbo.st_ADMIN_DEL_Email_OptionTypes");
                return true;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Emails_Delete", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;

        }
        public override DataSet ListDBobjects(string Fromdate, string ToDate, string Objectname, string IsScriptGenerated)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objParam = {
			new SqlParameter("@in_FromDate", (!string.IsNullOrEmpty(Fromdate) ? Fromdate : null)),
			new SqlParameter("@in_ToDate", (!string.IsNullOrEmpty(ToDate) ? ToDate : null)),
			new SqlParameter("@in_Objectname", (!string.IsNullOrEmpty(Objectname) ? Objectname : null)),
			new SqlParameter("@in_IsGenerated", (!string.IsNullOrEmpty(IsScriptGenerated) ? IsScriptGenerated : null))
		};
                objcommon.AddInParameters(objParam);
                DataSet dsUsers = new DataSet();
                dsUsers = objcommon.GetDataSet("Help_dbo.st_Admin_List_DBobjects");
                return dsUsers;



            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "ListDBobjects", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override DataSet Fillusers(string spName)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet dsUsers = new DataSet();
                dsUsers = objcommon.GetDataSet(spName);
                return dsUsers;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Fillusers", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override string InsertObjectDetails(string Objectname, string Objecttype, string ObjectDesc, string userID, string createdDate)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objParam1 = {
			new SqlParameter("@in_Objectname", Objectname),
			new SqlParameter("@in_Objecttype", Objecttype),
			new SqlParameter("@in_ObjectDesc", ObjectDesc),
			new SqlParameter("@in_UserID", userID),
			new SqlParameter("@In_CreatedOn", (!string.IsNullOrEmpty(createdDate) ? createdDate : null)),
			new SqlParameter("@In_Createdby", HttpContext.Current.Session["userid"])
		};
                objcommon.AddInParameters(objParam1);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Admin_Ins_DBobjects");
                return null;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "InsertObjectDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override string udpateDbscript(string dbScript, string ScriptID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objParam1 = {
			new SqlParameter("@in_DBScript_ID", ScriptID),
			new SqlParameter("@in_DBScript", dbScript),
			new SqlParameter("@In_UpdatedBy", HttpContext.Current.Session["userid"])
		};

                objcommon.AddInParameters(objParam1);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Admin_Upd_DBScript");
                return null;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "udpateDbscript", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override string upDateScriptIDs(string ScriptIDs)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objParam1 = { new SqlParameter("@In_FileIds", ScriptIDs) };

                objcommon.AddInParameters(objParam1);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Script_UPD_GeneratedScript");
                return null;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "upDateScriptIDs", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }


    }
}