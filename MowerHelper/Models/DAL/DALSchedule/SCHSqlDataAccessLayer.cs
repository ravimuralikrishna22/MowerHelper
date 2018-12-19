using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MowerHelper.Models.BLL.BLLSchedule;
using MowerHelper.Models.Classes;
namespace MowerHelper.Models.DAL.DALSchedule
{
    public class SCHSqlDataAccessLayer:SCHDataAccessLayerBaseClass
    {
        string ClassName = "SCHSqlDataAccessLayer";
        clsDBWrapper objWrapper = new clsDBWrapper();
        public override DataSet GetDayTemplate(string ProviderID, string ApptDate, string ShowAll, string WeekDay)
        {

            try
            {
                clsCommonFunctions objcom = new clsCommonFunctions();
                DataSet dsAppointmentsList = new DataSet();
                IDataParameter[] InParamList = {
		                                        new SqlParameter("@in_ProviderID", ProviderID),
		                                        new SqlParameter("@in_AppointmentDate", ApptDate),
		                                        new SqlParameter("@In_ShowAll", ShowAll),
		                                        new SqlParameter("@in_WeekDay", WeekDay)
	                                            };
                IDataParameter[] OutParamList = { new SqlParameter("@Out_OpenInd", SqlDbType.Char, 1) };

                objcom.AddInParameters(InParamList);
                objcom.AddOutParameters(OutParamList);
                dsAppointmentsList = null;
                dsAppointmentsList = objcom.GetDataSet("Help_dbo.st_Appointment_List_DayAppointmentsVercdft");
                return dsAppointmentsList;
               
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetDayTemplate", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override DataSet GetDayAppointments(string ProviderID,  string ApptDate, string ShowAll,string Tech_id)
        {

            try
            {
                clsCommonFunctions objcom = new clsCommonFunctions();
                DataSet dsAppointmentsList = new DataSet();
                IDataParameter[] DayViewInParamList = {
		                                          new SqlParameter("@in_Provider_ID", ProviderID),
		                                          new SqlParameter("@in_ApptDate", ApptDate),
		                                          new SqlParameter("@In_ShowAll", ShowAll),
                                                  new SqlParameter("@in_technician_id", Tech_id)
                                                  
	                                                    };
                objcom.AddInParameters(DayViewInParamList);
                dsAppointmentsList = objcom.GetDataSet("Help_dbo.st_Scheduling_List_Appointments");
                return dsAppointmentsList;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetDayAppointments", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;

        }
        public override List<GetAppointment> GetPatientAppointment(string AppointmentID, string GroupNo, string AppointmentDate)
        {
            try
            {
                IDataParameter[] InParamList = {
		                                  new SqlParameter("@in_Appointment_ID", (AppointmentID!=null ? AppointmentID : null)),		                                 
		                                  new SqlParameter("@In_AppointmentDate", AppointmentDate)
	                                          };
                clsDBWrapper objWrapper = new clsDBWrapper();
                SqlDataReader ObjReader;
                objWrapper.AddInParameters(InParamList);
                ObjReader = objWrapper.GetDataReader("Help_dbo.st_Appointment_Get_Appointment");
                List<GetAppointment> objGetAppointmentList = new List<GetAppointment>();
                while (ObjReader.Read())
                {
                    GetAppointment objGetAppointment = new GetAppointment((ObjReader["Appointment_ID"]!=null ?Convert.ToInt32(ObjReader["Appointment_ID"]) : -1), 
                        (ObjReader["FromReferenceType_ID"]!=null ?Convert.ToInt32(ObjReader["FromReferenceType_ID"]) : -1), 
                        (ObjReader["FromReference_ID"]!=null ? Convert.ToInt32(ObjReader["FromReference_ID"]) : -1), 
                        (ObjReader["FromReferenceLogin_ID"]!=null ?Convert.ToInt32(ObjReader["FromReferenceLogin_ID"]) : -1),
                        (ObjReader["ProviderName"].ToString() != null ? ObjReader["ProviderName"].ToString() : null),                         
                        (ObjReader["PatientName"].ToString() != null ? ObjReader["PatientName"].ToString() : null),
                        (ObjReader["ToReferenceType_ID"]!= null ?Convert.ToInt32(ObjReader["ToReferenceType_ID"]) : -1),
                        //(ObjReader["FacilityReference_ID"]!=null ? Convert.ToInt32(ObjReader["FacilityReference_ID"]) : -1),
                        //(ObjReader["PlaceofService"].ToString() != null ? ObjReader["PlaceofService"].ToString() : null),                        
                        (ObjReader["Startdate"].ToString() != null ? ObjReader["Startdate"].ToString() : null),                                          
                        (ObjReader["AppointmentType_ID"] != null ?Convert.ToInt32(ObjReader["AppointmentType_ID"]) : -1),                        
                        (ObjReader["AppointmentStatus"].ToString() != null ? ObjReader["AppointmentStatus"].ToString() : null),
                        (ObjReader["Notes"].ToString() != null ? ObjReader["Notes"].ToString() : null),
                        (ObjReader["CurrentAppointmentTracking_ID"] != null ?Convert.ToInt32(ObjReader["CurrentAppointmentTracking_ID"]) : -1),
                        (ObjReader["AppointmentDate"].ToString() != null ? ObjReader["AppointmentDate"].ToString() : null),
                        (ObjReader["AppointmentTime"].ToString() != null ? ObjReader["AppointmentTime"].ToString() : null),
                        (ObjReader["Duration"] != null ? Convert.ToInt32(ObjReader["Duration"]) : -1),
                         
                        (ObjReader["AppointmentStatus_ID"] != null ? Convert.ToInt32(ObjReader["AppointmentStatus_ID"]) : -1),
                        (ObjReader["Defaultaddress_Ind"].ToString() != null ? ObjReader["Defaultaddress_Ind"].ToString() : null));

                        //(ObjReader["Practice_ID"] != null ? Convert.ToInt32(ObjReader["Practice_ID"]) : -1));     
                    if (!DBNull.Value.Equals(ObjReader["phonenumber"]))// it is client phone no
                    {
                        objGetAppointment.phoneReminderHR = Convert.ToString(ObjReader["phonenumber"]);
                    }
                    if (!DBNull.Value.Equals(ObjReader["IntervalType"]))
                    {
                        objGetAppointment.IntervalType = Convert.ToString(ObjReader["IntervalType"]);
                    }
                    if (!DBNull.Value.Equals(ObjReader["Enddate"]))
                    {
                        objGetAppointment.Enddate = Convert.ToString(ObjReader["Enddate"]);
                    }
                    if (!DBNull.Value.Equals(ObjReader["SelectedWeekdays"]))
                    {
                        objGetAppointment.SelectedWeekdays = Convert.ToString(ObjReader["SelectedWeekdays"]);
                    }
                    if(!DBNull.Value.Equals(ObjReader["ToReference_ID"]))
                    {
                        objGetAppointment.ToReference_ID = Convert.ToInt32(ObjReader["ToReference_ID"]);
                    }
                    if (!DBNull.Value.Equals(ObjReader["ToReferenceLogin_ID"]))
                    {
                        objGetAppointment.ToReferenceLogin_ID = Convert.ToInt32(ObjReader["ToReferenceLogin_ID"]);
                    }
                    if (!DBNull.Value.Equals(ObjReader["AppointmentRecurrence_ID"]))
                    {
                        objGetAppointment.AppointmentRecurrence_ID = Convert.ToInt32(ObjReader["AppointmentRecurrence_ID"]);
                    }
                    if (!DBNull.Value.Equals(ObjReader["IntervalType_ID"]))
                    {
                        objGetAppointment.IntervalType_ID = Convert.ToInt32(ObjReader["IntervalType_ID"]);
                    }
                    if (!DBNull.Value.Equals(ObjReader["IntervalValue"]))
                    {
                        objGetAppointment.IntervalValue = Convert.ToInt32(ObjReader["IntervalValue"]);
                    }
                    if (!DBNull.Value.Equals(ObjReader["NumofOccurrences"]))
                    {
                        objGetAppointment.NumofOccurrences = Convert.ToInt32(ObjReader["NumofOccurrences"]);
                    }
                    //if (!DBNull.Value.Equals(ObjReader["ToReferenceLogin_ID"]))
                    //{
                    //    objGetAppointment.ToReferenceLogin_ID = Convert.ToInt32(ObjReader["ToReferenceLogin_ID"]);
                    //}
                    objGetAppointment.AppointmentBy = ObjReader["AppointmentBy"].ToString() != null ? ObjReader["AppointmentBy"].ToString() : null;                                       
                    objGetAppointment.PatientEmail = ObjReader["PatientEmail"].ToString() != null ? ObjReader["PatientEmail"].ToString() : null;
                  //  objGetAppointment.Technician_ids = ObjReader["Technician_ids"].ToString() != null ? ObjReader["Technician_ids"].ToString() : null;
                   // objGetAppointment.Technicianname = ObjReader["Technicianname"].ToString() != null ? ObjReader["Technicianname"].ToString() : null;
                  //  objGetAppointment.TechnicianPositions = ObjReader["pos"].ToString() != null ? ObjReader["pos"].ToString() : null;
                    objGetAppointment.Aptplaceofservice= ObjReader["placeofservice"].ToString() != null ? ObjReader["placeofservice"].ToString() : null;
                    objGetAppointment.Aptplaceofservicename = ObjReader["Placeofservicename"].ToString() != null ? ObjReader["Placeofservicename"].ToString() : null;
                    objGetAppointment.VoiceFileName = ObjReader["notes_voice_filename"].ToString() != null ? ObjReader["notes_voice_filename"].ToString() : null;
                    if (!DBNull.Value.Equals(ObjReader["Appointment_address_id"]))
                    {
                        objGetAppointment.Appointment_address_ID = Convert.ToInt32(ObjReader["Appointment_address_id"]);
                    }
                    else
                    {
                        objGetAppointment.Appointment_address_ID = null;
                    }
                    objGetAppointmentList.Add(objGetAppointment);
                }
                return objGetAppointmentList;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetPatientAppointment", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<GetAppointment> GetPendingAppointment(string AppointmentID, string AppointmentDate)
        {
            try
            {
                IDataParameter[] InParamList = {
		                                  new SqlParameter("@in_Appointment_ID", (AppointmentID!=null ? AppointmentID : null)),		                                 
		                                  new SqlParameter("@In_AppointmentDate", AppointmentDate)
	                                          };
                clsDBWrapper objWrapper = new clsDBWrapper();
                SqlDataReader ObjReader;
                objWrapper.AddInParameters(InParamList);
                ObjReader = objWrapper.GetDataReader("Help_dbo.st_Appointment_Get_pending_Appointment");
                List<GetAppointment> objGetAppointmentList = new List<GetAppointment>();
                while (ObjReader.Read())
                {
                    GetAppointment objGetAppointment = new GetAppointment((ObjReader["Appointment_ID"] != null ? Convert.ToInt32(ObjReader["Appointment_ID"]) : -1),
                        (ObjReader["FromReferenceType_ID"] != null ? Convert.ToInt32(ObjReader["FromReferenceType_ID"]) : -1),
                        (ObjReader["FromReference_ID"] != null ? Convert.ToInt32(ObjReader["FromReference_ID"]) : -1),
                        (ObjReader["FromReferenceLogin_ID"] != null ? Convert.ToInt32(ObjReader["FromReferenceLogin_ID"]) : -1),
                        (ObjReader["ProviderName"].ToString() != null ? ObjReader["ProviderName"].ToString() : null),
                        (ObjReader["PatientName"].ToString() != null ? ObjReader["PatientName"].ToString() : null),
                        (ObjReader["ToReferenceType_ID"] != null ? Convert.ToInt32(ObjReader["ToReferenceType_ID"]) : -1),
                        //(ObjReader["FacilityReference_ID"]!=null ? Convert.ToInt32(ObjReader["FacilityReference_ID"]) : -1),
                        //(ObjReader["PlaceofService"].ToString() != null ? ObjReader["PlaceofService"].ToString() : null),                        
                        (ObjReader["Startdate"].ToString() != null ? ObjReader["Startdate"].ToString() : null),
                        (ObjReader["AppointmentType_ID"] != null ? Convert.ToInt32(ObjReader["AppointmentType_ID"]) : -1),
                        (ObjReader["AppointmentStatus"].ToString() != null ? ObjReader["AppointmentStatus"].ToString() : null),
                        (ObjReader["Notes"].ToString() != null ? ObjReader["Notes"].ToString() : null),
                        (ObjReader["CurrentAppointmentTracking_ID"] != null ? Convert.ToInt32(ObjReader["CurrentAppointmentTracking_ID"]) : -1),
                        (ObjReader["AppointmentDate"].ToString() != null ? ObjReader["AppointmentDate"].ToString() : null),
                        (ObjReader["AppointmentTime"].ToString() != null ? ObjReader["AppointmentTime"].ToString() : null),
                        (ObjReader["Duration"] != null ? Convert.ToInt32(ObjReader["Duration"]) : -1),
                        (ObjReader["AppointmentStatus_ID"] != null ? Convert.ToInt32(ObjReader["AppointmentStatus_ID"]) : -1),
                         (ObjReader["Defaultaddress_Ind"].ToString() != null ? ObjReader["Defaultaddress_Ind"].ToString() : null));
                    //(ObjReader["Practice_ID"] != null ? Convert.ToInt32(ObjReader["Practice_ID"]) : -1));
                    if (!DBNull.Value.Equals(ObjReader["ToReference_ID"]))
                    {
                        objGetAppointment.ToReference_ID = Convert.ToInt32(ObjReader["ToReference_ID"]);
                    }
                    if (!DBNull.Value.Equals(ObjReader["ToReferenceLogin_ID"]))
                    {
                        objGetAppointment.ToReferenceLogin_ID = Convert.ToInt32(ObjReader["ToReferenceLogin_ID"]);
                    }
                    if (!DBNull.Value.Equals(ObjReader["ToReferenceLogin_ID"]))
                    {
                        objGetAppointment.ToReferenceLogin_ID = Convert.ToInt32(ObjReader["ToReferenceLogin_ID"]);
                    }
                    objGetAppointment.AppointmentBy = ObjReader["AppointmentBy"].ToString() != null ? ObjReader["AppointmentBy"].ToString() : null;
                    objGetAppointment.PatientEmail = ObjReader["PatientEmail"].ToString() != null ? ObjReader["PatientEmail"].ToString() : null;
                   // objGetAppointment.Technician_ids = ObjReader["Technician_ids"].ToString() != null ? ObjReader["Technician_ids"].ToString() : null;
                    //objGetAppointment.Technicianname = ObjReader["Technicianname"].ToString() != null ? ObjReader["Technicianname"].ToString() : null;
                    objGetAppointment.TechnicianPositions = ObjReader["pos"].ToString() != null ? ObjReader["pos"].ToString() : null;
                    objGetAppointmentList.Add(objGetAppointment);
                }
                return objGetAppointmentList;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetPatientAppointment", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override System.Collections.Generic.List<MowerHelper.Models.BLL.MessageStation.Category> SentEmailLog(MowerHelper.Models.BLL.MessageStation.Category objCategory)
        {
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] inParameter = {
		new SqlParameter("@in_EmailMessage_Category_ID", objCategory.EmailCategoryID),
		new SqlParameter("@in_CatCount", objCategory.EmailCategoryCount + 1),
		new SqlParameter("@in_FromReference_id", objCategory.FromReference_id),
		new SqlParameter("@in_Toreference_id", objCategory.Toreference_id),
		new SqlParameter("@in_ISWebService_IND", objCategory.ISWebService_IND),
		new SqlParameter("@in_Application_Id", objCategory.Application_Id),
        new SqlParameter("@in_Fromreferencetype_id", objCategory.FromReferenceType_id),
        new SqlParameter("@in_Toreferencetype_id", objCategory.Toreferencetype_id)
	};
                objWrapper = new clsDBWrapper();
                objWrapper.AddInParameters(inParameter);
                objWrapper.ExecuteNonQuerySP("Help_dbo.St_Ins_SentEmailLog");
                return null;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "SentEmailLog", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override int EmailCategoryCount(int intCategoryID)
        {
            try
            {

                clsCommonFunctions objCommon = new clsCommonFunctions();
                DataSet ds = new DataSet();
                IDataParameter[] inParameter = { new SqlParameter("@in_EmailMessage_Category_ID", intCategoryID) };
                IDataParameter[] objout = { new SqlParameter("@Out_Count", SqlDbType.Int) };
                objCommon.AddInParameters(inParameter);
                objCommon.AddOutParameters(objout);
                ds = objCommon.GetDataSet("Help_dbo.St_Get_Category_EmailMsgcount");
                if (objCommon.objdbCommandWrapper.Parameters["@Out_Count"].Value != null & objCommon.objdbCommandWrapper.Parameters["@Out_Count"].Value != DBNull.Value)
                {
                    return Convert.ToInt32(objCommon.objdbCommandWrapper.Parameters["@Out_Count"].Value);
                }

               
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "EmailCategoryCount", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
            }
        public override DataSet GetWeekAppointments(string ProviderID, string FromDate, string ToDate, string Techid)
        {
            try
            {
                clsCommonFunctions objcom = new clsCommonFunctions();
                DataSet dsWeeklyApptList = new DataSet();
                IDataParameter[] WeekViewInParamList = {
		                                          new SqlParameter("@in_FromReference_ID", ProviderID),
		                                          new SqlParameter("@in_FromDate", FromDate),
		                                          new SqlParameter("@in_Todate", ToDate),
                                                  new SqlParameter("@in_technician_id", Techid)
		                                          
	                                                    };
                objcom.AddInParameters(WeekViewInParamList);
                dsWeeklyApptList = objcom.GetDataSet("Help_dbo.st_Appointment_List_WeekAppointments");
                return dsWeeklyApptList;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetWeekAppointments", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override DataSet GetMonthAppointments(string ProviderID, string Month, string Year, string ApptStatusID, string Tech_id)
        {
            try
            {
                clsCommonFunctions objcom = new clsCommonFunctions();
                DataSet dsMonthlyApptList = new DataSet();
                IDataParameter[] MonthViewInParamList = {
		                                          new SqlParameter("@in_FromReference_ID", ProviderID),
		                                          new SqlParameter("@in_Month", Month),
		                                          new SqlParameter("@in_Year", Year),
                                                  new SqlParameter("@in_technician_id", Tech_id)
                                                  
		                                          
	                                                    };
                objcom.AddInParameters(MonthViewInParamList);
                dsMonthlyApptList = objcom.GetDataSet("Help_dbo.st_Appointment_List_MonthAppointments");
                return dsMonthlyApptList;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetMonthAppointments", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }        
        public string checkApptExist(string ApptDate, string PatientID, string Apttime, string Provider_ID,string  ApptRecID)
        {
            try
            {
                string strcheck = string.Empty;
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                      new SqlParameter("@In_Date",ApptDate),
                                      new SqlParameter("@in_Patient_ID",PatientID),
                                      new SqlParameter("@in_Appt_time",Apttime),                                     
                                      new SqlParameter("@in_Provider_ID",Provider_ID),
                                      new SqlParameter("@In_AppointmentRec_ID",ApptRecID),
                                      };
                IDataParameter objreturn = new SqlParameter("@Loc_Appt_Ind", SqlDbType.Char, 1);
                objcommon.AddInParameters(insparam);
                objcommon.AddReturnParameters(objreturn);
                objcommon.ExecuteStringFunction("Help_dbo.FXn_Patient_GET_Appt");
                if (!DBNull.Value.Equals(objcommon.GetCurrentCommand.Parameters["@Loc_Appt_Ind"].Value))
                {
                    strcheck = objcommon.GetCurrentCommand.Parameters["@Loc_Appt_Ind"].Value.ToString();
                }
                return strcheck;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "checkApptExist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;

        }
        public string weeklyApptDates(string apptDate, string recNo, string endDate, string selectedWeek)
        {
            try
            {
                string strweekselectiondates = string.Empty;
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@In_Startdate",apptDate),
                                           new SqlParameter("@In_RecNo",recNo),
                                           new SqlParameter("@In_EndDate",endDate),
                                           new SqlParameter("@In_OccurNo",null),
                                           new SqlParameter("@In_SelectedWeekdays",selectedWeek)
                                      };
                IDataParameter objreturn = new SqlParameter("@loc_weeklydatestring", SqlDbType.VarChar, 8000);
                objcommon.AddInParameters(insparam);
                objcommon.AddReturnParameters(objreturn);
                objcommon.ExecuteStringFunction("Help_dbo.Fxn_Schedule_Get_WeeklyAppointmentDates_string");
                if (!DBNull.Value.Equals(objcommon.GetCurrentCommand.Parameters["@loc_weeklydatestring"].Value))
                {
                    strweekselectiondates = objcommon.GetCurrentCommand.Parameters["@loc_weeklydatestring"].Value.ToString();
                }
                return strweekselectiondates;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "weeklyApptDates", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;

        }
        public override DataSet GetProviderOpenSlots(int? ProviderId, string ApptStartdate, string ApptEnddate,string Starttime,string Endtime,int? Duration)
        {
            try
            {
                clsCommonFunctions objcom = new clsCommonFunctions();
                DataSet dsProviderOpenSlots = new DataSet();
                IDataParameter[] InParamList = {
		                                  new SqlParameter("@In_Provider_ID", (ProviderId!=null ? ProviderId : null)),		                                 
		                                  new SqlParameter("@In_AppointmentStart_Date", (ApptStartdate!=null ? ApptStartdate : null)),
                                          new SqlParameter("@In_AppointmentEnd_Date", (ApptEnddate!=null ? ApptEnddate : null)),
                                          new SqlParameter("@In_StartTime", (Starttime!=null ? Starttime : null)),
                                          new SqlParameter("@In_EndTime", (Endtime!=null ? Endtime : null)),
                                          new SqlParameter("@In_Duration", (Duration!=null ? Duration : null))
	                                          };

                objcom.AddInParameters(InParamList);
                dsProviderOpenSlots = objcom.GetDataSet("Help_dbo.st_Appointments_GET_ProviderOpenSlots");
                return dsProviderOpenSlots;

            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetProviderOpenSlots", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<GetAppointment> GetCustomerdetails(string Email, string Phonenumber, string Provider_ID)
        {
            try
            {
                IDataParameter[] InParamList = {
		                                  new SqlParameter("@In_Email", (Email!=null ? Email : null)),		                                 
		                                  new SqlParameter("@In_phonenumber", (Phonenumber!=null ? Phonenumber : null)),
                                          new SqlParameter("@In_provider_id", (Provider_ID!=null ? Provider_ID : null))
	                                          };
                clsDBWrapper objWrapper = new clsDBWrapper();
                SqlDataReader ObjRead;
                objWrapper.AddInParameters(InParamList);
                ObjRead = objWrapper.GetDataReader("Help_dbo.ST_Check_GET_customerdetails");
                List<GetAppointment> objGetProvDetailsList = new List<GetAppointment>();
                while (ObjRead.Read())
                {
                    GetAppointment objGetProvDetails = new GetAppointment();
                    if (ObjRead["Customerexists"].ToString() == "Y")
                    {
                        objGetProvDetails.Customerexists = ObjRead["Customerexists"].ToString();
                        objGetProvDetails.Provider_ID = Convert.ToInt32(ObjRead["provider_id"]);
                        objGetProvDetails.ProviderLogin_ID = Convert.ToInt32(ObjRead["ProviderLogin_ID"]);
                        objGetProvDetails.Patient_ID = Convert.ToInt32(ObjRead["Patient_ID"]);
                        objGetProvDetails.PatientLogin_ID = Convert.ToInt32(ObjRead["PatientLogin_ID"]);
                        objGetProvDetails.Technician_ids = ObjRead["Technician_ID"].ToString();
                        objGetProvDetailsList.Add(objGetProvDetails);
                    }
                    else
                    {
                        objGetProvDetails.Customerexists = ObjRead["Customerexists"].ToString();
                        objGetProvDetails.Provider_ID = Convert.ToInt32(ObjRead["provider_id"]);
                        objGetProvDetails.Technician_ids = ObjRead["Technician_ID"].ToString();
                        objGetProvDetails.ProviderLogin_ID = Convert.ToInt32(ObjRead["ProviderLogin_ID"]);
                        objGetProvDetailsList.Add(objGetProvDetails);
                    }
                }
                return objGetProvDetailsList;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetPatientAppointment", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        }

    }
