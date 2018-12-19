using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.Classes;
namespace MowerHelper.Models
{
    public class AddAppointmentModel
    {
       // private int? _FromReference_ID;
       // private int? _FromReferenceLogin_ID;
       // private int? _FromReferenceType_ID;
       // private int? _ToReference_ID;
       // private int? _ToReferenceLogin_ID;
       // private int? _ToReferenceType_ID;
       // private string _ToReference_IDs;
       // private Nullable<int> _CPTCode;
       // private string _GroupInd;
       // private string _IntervalType_ID;
       // private string _IntervalValue;
       // private int? _NumofOccurrences;
       // private string _StartDate;
       // private string _EndDate;
       // private string _SelectedWeekdays;
       // private int? _PlaceOfService_ID;
       // private int? _AppointmentType_ID;
       // private int? _AppointmentMode_ID;
       // private string _Notes;
       // private string _AppointmentTime;
       // private int? _Duration;
       // private int? _AppointmentStatus_ID;
       // private int? _CreatedBy;
       // private string _IsValidate_Ind;
       // private string _Autoreminder_Ind;
       // private string _reminderHR;
       //// private string _PatientEmail;
       // private string _PhoneAutoreminder_Ind;

       // private string _PhoneremainderHR;
       // private int? _intSenderID;
       // private string _strSubject;
       // private string _strMsgbody;
       // private int? _intMsgCatID;
       // private string _strAddSignature;
       // private int? _intReplyID;
       // private int? _intReciverIDs;
       // private int? _intServiceID;
       // private int? _intPatient_ID;
       // private int? _intProvider_ID;
       // private int? _intFrom_RoleID;
       // private int? _intReciever_RoleID;
       // private int? _intPractice_ID;
       // private int? _intAppointmentID;
       // private string _strEmail;
       // private string _cellphone;
       // private int? _FacilityReference_ID;
       // private int? _Appointment_ID;
       // private int? _AppointmentTracking_ID;
       // private int? _AppointmentRecurrence_ID;
       // private string _EventTitle;
       // private int? _UpdatedBy;
       // private string _Edit_Ind;
       // private string _Rescheduledate;
       // private string _Editfutreapnt_Ind;
       // private string _AutoreminderAllow_Ind;
       // private string _phoneAuto_Reminder_Allow_Ind;
       // private string _phoneReminderHR;
       // private int? _GroupNo;
       
        public AddAppointmentModel()
        {
        }
        public string cellphone
        {
            get;
            set;
        }
        //public string Autoreminder_Ind
        //{
        //    get;
        //    set;
        //}
        //public string reminderHR
        //{
        //    get;
        //    set;
        //}
        //public string phoneAutoreminder_Ind
        //{
        //    get;
        //    set;
        //}
        //public string phoneremainderHR
        //{
        //    get;
        //    set;
        //}
        //public string Subject
        //{
        //    get;
        //    set;
        //}
        //public string Msgbody
        //{
        //    get;
        //    set;
        //}
        //public string AddSignature
        //{
        //    get;
        //    set;
        //}

        //public string Email
        //{
        //    get;
        //    set;
        //}
        public int? AppointmentID
        {
            get;
            set;
        }
        //public int? SenderID
        //{
        //    get;
        //    set;
        //}
        //public int? MsgCatID
        //{
        //    get;
        //    set;
        //}
        //public int? ReplyID
        //{
        //    get;
        //    set;
        //}
        //public int? ReciverIDs
        //{
        //    get;
        //    set;
        //}

        //public int? ServiceID
        //{
        //    get;
        //    set;
        //}
        public int? Patient_ID
        {
            get;
            set;
        }
        public int? Provider_ID
        {
            get;
            set;
        }
        //public int? From_RoleID
        //{
        //    get;
        //    set;
        //}
        //public int? Reciever_RoleID
        //{
        //    get;
        //    set;
        //}
        public int? Practice_ID
        {
            get;
            set;
        }

        //'
        public int? FromReference_ID
        {
            get;
            set;
        }

        public int? FromReferenceLogin_ID
        {
            get;
            set;
        }

        public int? FromReferenceType_ID
        {
            get;
            set;
        }

        public int? ToReference_ID
        {
            get;
            set;
        }
        public int? AddToReference_ID
        {
            get;
            set;
        }

        public int? ToReferenceLogin_ID
        {
            get;
            set;
        }

        public int? ToReferenceType_ID
        {
            get;
            set;
        }
        public string VoiceFileName { get; set; }
        //public string ToReference_IDs
        //{
        //    get;
        //    set;
        //}

        //public Nullable<int> CPTCode
        //{
        //    get;
        //    set;
        //}

        public string GroupInd
        {
            get;
            set;
        }

        public string IntervalType_ID
        {
            get;
            set;
        }

        public string IntervalValue
        {
            get;
            set;
        }
        public string interdayvalue { get; set; }
        public string interweekvalue { get; set; }
        public int? NumofOccurrences
        {
            get;
            set;
        }

        public string StartDate
        {
            get;
            set;
        }

        public string EndDate
        {
            get;
            set;
        }
        public string dayEndDate
        {
            get;
            set;
        }
        public string weekEndDate
        {
            get;
            set;
        }
        public string SelectedWeekdays
        {
            get;
            set;
        }

        public int? PlaceOfService_ID
        {
            get;
            set;
        }

        public int? AppointmentType_ID
        {
            get;
            set;
        }

        public int? AppointmentMode_ID
        {
            get;
            set;
        }

        public string Notes
        {
            get;
            set;
        }

        public string AppointmentTime
        {
            get;
            set;
        }

        public int? Duration
        {
            get;
            set;
        }

        public int? AppointmentStatus_ID
        {
            get;
            set;
        }

        public int? CreatedBy
        {
            get;
            set;
        }

        public string IsValidate_Ind
        {
            get;
            set;
        }
        public int? FacilityReference_ID
        {
            get;
            set;
        }
        public int? Appointment_ID
        {
            get;
            set;
        }

        public int? AppointmentTracking_ID
        {
            get;
            set;
        }
        public int? AppointmentRecurrence_ID
        {
            get;
            set;
        }
        //public string EventTitle
        //{
        //    get;
        //    set;
        //}
        public int? UpdatedBy
        {
            get;
            set;
        }
        public string Edit_Ind
        {
            get;
            set;
        }
        public string phoneReminderHR
        {
            get;
            set;
        }
        //public string phoneAuto_Reminder_Allow_Ind
        //{
        //    get;
        //    set;
        //}
        //public string AutoreminderAllow_Ind
        //{
        //    get;
        //    set;
        //}
        public string Editfutreapnt_Ind
        {
            get;
            set;
        }
        //public string Rescheduledate
        //{
        //    get;
        //    set;
        //}
        public int? GroupNo
        {
            get;
            set;
        }
        public int? RefLoginID { get; set; }
        public string monthname { get;set;}
        //public string Technician_ids
        //{
        //    get;
        //    set;
        //}
        public string Aptplaceofservice { get; set; }
        public string Aptplaceofservicename { get; set; }
       // public string TechnicianPositions { get; set; }
        public string status_ind { get; set; }
        public string otherAddress { get; set; }
      

        public string defaultCoachAddress { get; set; }    
        public int? otherAddressid { get; set; }
        public int? stateid { get; set; }
        public int? cityid { get; set; }       
        public string caddress1 { get; set; }
        public string zip { get; set; }
        public string CourtLocationName { get; set; }
        public string ProviderName { get; set; }
        public string PatientName { get; set; }
        public string ApptType { get; set; }
        public string AppointmentDate { get; set; }
        public string RepeatType { get; set; }
        public string WeekDay { get; set; }
        public bool ChkDefaultCourt{ get; set; }
        public string Recurring { get; set; }
        public string ERecurring { get; set; }
        public string DailyReccur { get; set; }
        public string AddDailyReccur { get; set; }
        public string EndDateForDaily { get; set; }
        public string WeeklyReccur { get; set; }
        public string AddWeeklyReccur { get; set; }
        public string EndDateForWeek { get; set; }
        public Reg_ProvidersDetailInfo ProvAddrInfo { get; set; }
        public string FromPage { get; set; }
        clsDBWrapper objWrapper;
        #region "Appointment Creation"
        const string ClassName = "Cognodelite.Scheule.DataAccessLayer.SCHSqlDataAccessLayer";
        public string InsertAppointment(AddAppointmentModel objCreateAppointment,  int? aptid)
        {
            try
            {
                IDataParameter[] InparamList = {
					new SqlParameter("@In_FromReference_ID", (objCreateAppointment.FromReference_ID != null ? objCreateAppointment.FromReference_ID : null)),
					new SqlParameter("@in_FromReferenceLogin_ID", (objCreateAppointment.FromReferenceLogin_ID != null ? objCreateAppointment.FromReferenceLogin_ID : null)),
					new SqlParameter("@in_FromReferenceType_ID", (objCreateAppointment.FromReferenceType_ID != null ? objCreateAppointment.FromReferenceType_ID :null)),
					new SqlParameter("@In_ToReference_ID", (objCreateAppointment.ToReference_ID != null ? objCreateAppointment.ToReference_ID :null)),
					new SqlParameter("@in_ToReferenceLogin_ID", (objCreateAppointment.ToReferenceLogin_ID != null ? objCreateAppointment.ToReferenceLogin_ID : null)),
					new SqlParameter("@in_ToReferenceType_ID", (objCreateAppointment.ToReferenceType_ID != null ? objCreateAppointment.ToReferenceType_ID : null)),                    
					new SqlParameter("@In_GroupInd", objCreateAppointment.GroupInd),
					new SqlParameter("@In_IntervalType_ID", objCreateAppointment.IntervalType_ID),
					new SqlParameter("@In_IntervalValue", objCreateAppointment.IntervalValue),
					new SqlParameter("@In_NumofOccurrences", (objCreateAppointment.NumofOccurrences != null ? objCreateAppointment.NumofOccurrences:null)),
					new SqlParameter("@In_StartDate", objCreateAppointment.StartDate),
					new SqlParameter("@In_EndDate", objCreateAppointment.EndDate),
					new SqlParameter("@in_SelectedWeekdays", objCreateAppointment.SelectedWeekdays),                 
                    //new SqlParameter("@In_PlaceOfService_ID", (objCreateAppointment.PlaceOfService_ID != null ? objCreateAppointment.PlaceOfService_ID : null)),
					new SqlParameter("@In_AppointmentType_ID", (objCreateAppointment.AppointmentType_ID != null ? objCreateAppointment.AppointmentType_ID : null)),                   
					new SqlParameter("@In_Notes", objCreateAppointment.Notes),
					new SqlParameter("@In_AppointmentTime", objCreateAppointment.AppointmentTime),
					new SqlParameter("@In_Duration", (objCreateAppointment.Duration != null ? objCreateAppointment.Duration.ToString() : null)),
					new SqlParameter("@In_AppointmentStatus_ID", (objCreateAppointment.AppointmentStatus_ID != null ? objCreateAppointment.AppointmentStatus_ID.ToString() : null)),
					new SqlParameter("@in_CreatedBy", (objCreateAppointment.CreatedBy != null ? objCreateAppointment.CreatedBy.ToString() : null)),
					new SqlParameter("@in_IsValidate_Ind", objCreateAppointment.IsValidate_Ind),                    
					new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"]),
                    new SqlParameter("@in_Technician_ids",null),
                    new SqlParameter("@in_Technician_Position",null),
                    new SqlParameter("@In_placeofservice",objCreateAppointment.Aptplaceofservice),
                    new SqlParameter("@in_Address1",objCreateAppointment.caddress1),
                    new SqlParameter("@In_otherAddress1",objCreateAppointment.otherAddress),
                    new SqlParameter("@In_otherAddress2",null),
                    new SqlParameter("@In_City_id",objCreateAppointment.cityid !=0 ? objCreateAppointment.cityid : null),
                    new SqlParameter("@In_State_id",objCreateAppointment.stateid !=0 ? objCreateAppointment.stateid :null),
                    new SqlParameter("@In_Country_id",1),
                    new SqlParameter("@In_Zip",objCreateAppointment.zip),
                    new SqlParameter("@In_Appointment_address_id",objCreateAppointment.otherAddressid!=0 ?objCreateAppointment.otherAddressid:null),
                    new SqlParameter("@In_Court_location_name",objCreateAppointment.CourtLocationName),
                    new SqlParameter("@In_Default_address",objCreateAppointment.defaultCoachAddress),
                    new SqlParameter("@in_site_ind","W")
				};
                IDataParameter[] OutParamlist = {
					new SqlParameter("@out_msg", SqlDbType.VarChar, 6000),
					new SqlParameter("@Out_Appointment_Id", SqlDbType.BigInt, 100)
				};
                objWrapper = new clsDBWrapper();
                var _with1 = objWrapper;
                _with1.AddInParameters(InparamList);
                _with1.AddOutParameters(OutParamlist);
                _with1.ExecuteNonQuerySP("Help_dbo.st_Appointment_INS_ProviderRecurrenceAppointments");
                string strMessage = null;
                if (objWrapper.objdbCommandWrapper.Parameters["@out_msg"].Value != null)
                {
                    strMessage = objWrapper.objdbCommandWrapper.Parameters["@out_msg"].Value.ToString();
                }
                if (!DBNull.Value.Equals(objWrapper.objdbCommandWrapper.Parameters["@Out_Appointment_Id"].Value))
                {
                    aptid =  Convert.ToInt32(objWrapper.objdbCommandWrapper.Parameters["@Out_Appointment_Id"].Value);
                }
                return strMessage;
            }
            catch (Exception ex)
            {
                //ErrorLog.clsExceptionLog clsCustomEx = new ErrorLog.clsExceptionLog();
                //clsCustomEx.LogException(ex, ClassName, "InsertAppointment", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        //public int Ins_Appointment(AddAppointmentModel objCreateAppointment)
        //{
        //    try
        //    {
        //        //clsCommonFunctions objcommon = new clsCommonFunctions();
        //        IDataParameter[] InparamList = {   new SqlParameter("@In_FromReference_ID",objCreateAppointment.FromReference_ID),
        //                                           new SqlParameter("@in_FromReferenceType_ID",objCreateAppointment.FromReferenceType_ID),
        //                                           new SqlParameter("@in_FromReferenceLogin_ID",objCreateAppointment.FromReferenceLogin_ID),
        //                                           new SqlParameter("@in_ToReferenceLogin_ID",objCreateAppointment.ToReferenceLogin_ID),
        //                                           new SqlParameter("@In_ToReference_ID",objCreateAppointment.ToReference_ID),
        //                                           new SqlParameter("@in_ToReferenceType_ID",objCreateAppointment.ToReferenceType_ID),
        //                                           new SqlParameter("@In_PlaceOfService_ID",objCreateAppointment.PlaceOfService_ID),
        //                                           new SqlParameter("@In_AppointmentRecurrence_ID",objCreateAppointment.AppointmentRecurrence_ID),
        //                                           new SqlParameter("@In_AppointmentType_ID",objCreateAppointment.AppointmentType_ID),
        //                                           new SqlParameter("@In_AppointmentMode_ID",objCreateAppointment.AppointmentMode_ID),
        //                                           new SqlParameter("@In_GroupInd",objCreateAppointment._GroupInd),
        //                                           new SqlParameter("@In_GroupNo",objCreateAppointment.GroupNo),
        //                                           //new SqlParameter("@In_CPTCode",objCreateAppointment.CPTCode),
        //                                           new SqlParameter("@In_Notes",objCreateAppointment.Notes),
        //                                           new SqlParameter("@In_AppointmentDate",objCreateAppointment.StartDate),
        //                                           new SqlParameter("@In_AppointmentTime",objCreateAppointment.AppointmentTime),
        //                                           new SqlParameter("@In_Duration",objCreateAppointment.Duration),
        //                                           new SqlParameter("@In_AppointmentStatus_ID",objCreateAppointment.AppointmentStatus_ID),
        //                                           new SqlParameter("@In_EventTitle",objCreateAppointment.EventTitle),
        //                                           new SqlParameter("@in_CreatedBy",objCreateAppointment.CreatedBy),
        //                                           new SqlParameter("@In_ReferenceLogin_ID",objCreateAppointment.RefLoginID)
        //                                        };
        //        IDataParameter[] outparamslist = { new SqlParameter("@Out_Appointment_ID", SqlDbType.Int,100) };
        //        clsDBWrapper objWrapper = new clsDBWrapper();

        //        objWrapper.AddInParameters(InparamList);
        //        objWrapper.AddOutParameters(outparamslist);
        //        objWrapper.ExecuteNonQuerySP("Help_dbo.st_Appointment_INS_Appointment");
        //        int apptid;
               
        //        if (!DBNull.Value.Equals(objWrapper.objdbCommandWrapper.Parameters["@Out_Appointment_ID"].Value))
        //        {
        //            apptid = Convert.ToInt32(objWrapper.objdbCommandWrapper.Parameters["@Out_Appointment_ID"].Value);
        //        }
        //        else
        //        {
        //            apptid = 0;
        //        }
        //        return apptid;
               
        //    }
        //    catch (Exception)
        //    {
                
        //        throw;
        //    }
        //}
        #endregion
        #region "EditAppointment"
        public string EditAppointment(AddAppointmentModel objEditAppointment, ref string strFuturemessage, ref string strChangeRecur_Ind)
        {
            try
            {

                IDataParameter[] InparamList = {
                                        new SqlParameter("@in_FromReferenceType_ID", (objEditAppointment.FromReferenceType_ID != null? objEditAppointment.FromReferenceType_ID:null)), 
                                        new SqlParameter("@In_FromReference_ID", (objEditAppointment.FromReference_ID != null ? objEditAppointment.FromReference_ID: null)), 
                                        new SqlParameter("@In_FromReferenceLogin_ID", (objEditAppointment.FromReferenceLogin_ID != null?  objEditAppointment.FromReferenceLogin_ID: null)), 
                                        new SqlParameter("@in_ToReferenceType_ID", (objEditAppointment.ToReferenceType_ID != null? objEditAppointment.ToReferenceType_ID: null)), 
                                        new SqlParameter("@In_ToReference_ID", (objEditAppointment.ToReference_ID != null? objEditAppointment.ToReference_ID: null)), 
                                        new SqlParameter("@In_ToReferenceLogin_ID", (objEditAppointment.ToReferenceLogin_ID != null? objEditAppointment.ToReferenceLogin_ID: null)),                                        
                                         new SqlParameter("@In_GroupNo", (objEditAppointment.GroupNo != null? objEditAppointment.GroupNo: null)), 
                                        new SqlParameter("@In_GroupInd", objEditAppointment.GroupInd), 
                                        new SqlParameter("@In_IntervalType_ID", (objEditAppointment.IntervalType_ID != null ? objEditAppointment.IntervalType_ID: null)), 
                                        new SqlParameter("@In_IntervalValue", (objEditAppointment.IntervalValue != null ? objEditAppointment.IntervalValue: null)), 
                                        new SqlParameter("@In_NumofOccurrences", (objEditAppointment.NumofOccurrences !=  null ? objEditAppointment.NumofOccurrences : null)), 
                                        new SqlParameter("@in_AppointmentRecurrence_ID", (objEditAppointment.AppointmentRecurrence_ID != null? objEditAppointment.AppointmentRecurrence_ID: null)), 
                                        new SqlParameter("@In_StartDate", objEditAppointment.StartDate), 
                                        new SqlParameter("@In_EndDate", objEditAppointment.EndDate), 
                                        new SqlParameter("@in_SelectedWeekdays", objEditAppointment.SelectedWeekdays),                                       
                                        new SqlParameter("@in_Appointment_ID", (objEditAppointment.Appointment_ID != null ? objEditAppointment.Appointment_ID: null)),                                        
                                        new SqlParameter("@In_Notes", objEditAppointment.Notes), 
                                        new SqlParameter("@in_AppointmentTime", objEditAppointment.AppointmentTime), 
                                        new SqlParameter("@in_Duration", (objEditAppointment.Duration !=  null? objEditAppointment.Duration: null)), 
                                        new SqlParameter("@in_AppointmentTracking_ID",objEditAppointment.AppointmentTracking_ID),
                                        new SqlParameter("@in_AppointmentStatus_ID", (objEditAppointment.AppointmentStatus_ID != null? objEditAppointment.AppointmentStatus_ID: null)),  
                                         new SqlParameter("@In_AppointmentType_ID", (objEditAppointment.AppointmentType_ID != null? objEditAppointment.AppointmentType_ID: null)), 
                                        new SqlParameter("@in_UpdatedBy", (objEditAppointment.UpdatedBy != null? objEditAppointment.UpdatedBy: null)), 
                                        new SqlParameter("@in_IsValidate_Ind", objEditAppointment.IsValidate_Ind), 
                                        new SqlParameter("@in_Edit_Ind", objEditAppointment.Edit_Ind), 
                                        new SqlParameter("@In_RescheduleDate", null), 
                                        new SqlParameter("@In_Editfutreapnt_Ind", (objEditAppointment.Editfutreapnt_Ind != ""? objEditAppointment.Editfutreapnt_Ind:null)),                                   
                                        new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"]),
                                        new SqlParameter("@in_Technician_ids", null),  
                                        new SqlParameter("@in_Technician_Position", null),
                                        new SqlParameter("@in_status_ind", (objEditAppointment.status_ind != null ? objEditAppointment.status_ind: null)),
                                        new SqlParameter("@In_placeofservice",objEditAppointment.Aptplaceofservice),
                                        new SqlParameter("@In_address1",objEditAppointment.caddress1),
                    new SqlParameter("@In_otherAddress1",objEditAppointment.otherAddress),
                    new SqlParameter("@In_otherAddress2",null),
                    new SqlParameter("@In_City_ID",objEditAppointment.cityid !=0 ? objEditAppointment.cityid : null),
                    new SqlParameter("@In_State_ID",objEditAppointment.stateid !=0 ? objEditAppointment.stateid :null),
                    new SqlParameter("@In_Country_id",1),
                    new SqlParameter("@In_ZIP",objEditAppointment.zip),
                    new SqlParameter("@In_Appointment_address_id",objEditAppointment.otherAddressid!=0?objEditAppointment.otherAddressid:null),
                    new SqlParameter("@In_Court_location_name",objEditAppointment.CourtLocationName),                    
                                        new SqlParameter("@in_site_ind", "W"),
                                         new SqlParameter("@In_Default_address",objEditAppointment.defaultCoachAddress) ,
                                         new SqlParameter("@in_filename",objEditAppointment.VoiceFileName) 
                            };
                IDataParameter[] OutParam = { new SqlParameter("@out_msg", SqlDbType.VarChar, 6000), new SqlParameter("@Out_Future_Ind", SqlDbType.VarChar, 6000), new SqlParameter("@Out_ChangeRecur_Ind", SqlDbType.VarChar, 6000) };
                clsDBWrapper objWrapper = new clsDBWrapper();
                objWrapper.AddInParameters(InparamList);
                objWrapper.AddOutParameters(OutParam);
                objWrapper.ExecuteNonQuerySP("Help_dbo.st_Appointment_UPD_RecurrenceAppointmentOnly");
                string strMessage = null;
                if (!DBNull.Value.Equals(objWrapper.objdbCommandWrapper.Parameters["@out_msg"].Value))
                {
                    strMessage = objWrapper.objdbCommandWrapper.Parameters["@out_msg"].Value.ToString();
                }
                if (!DBNull.Value.Equals(objWrapper.objdbCommandWrapper.Parameters["@Out_Future_Ind"].Value))
                {
                    strFuturemessage = objWrapper.objdbCommandWrapper.Parameters["@Out_Future_Ind"].Value.ToString();
                }
                if (!DBNull.Value.Equals(objWrapper.objdbCommandWrapper.Parameters["@Out_ChangeRecur_Ind"].Value))
                {
                    strChangeRecur_Ind = objWrapper.objdbCommandWrapper.Parameters["@Out_ChangeRecur_Ind"].Value.ToString();
                }
                return strMessage;
            }
            catch (Exception ex)
            {
            }
            return null;
           
        }
        #endregion
//        #region "Change Appointment Status"
//        public void ChangeApptStatus(int? aptid)
//        {
//            try
//            {
//                IDataParameter[] inParamlist={
//                                                 new SqlParameter("@in_Appointment_ID",aptid)
//                                             };
//                clsDBWrapper objWrapper = new clsDBWrapper();
//                objWrapper.AddInParameters(inParamlist);
//                objWrapper.ExecuteNonQuerySP("Help_dbo.st_Scheduling_UPD_SingleAppointment_CheckIn");
//            }
//            catch (Exception ex)
//            {
                
//                throw;
//            }
//        }
//#endregion
        #region "SendEmailAfterAddNewAppointment"
        //public int getEmailTranId(AddAppointmentModel objins,int? aptid)
        //{
        //    try
        //    {
        //        clsDBWrapper obj = new clsDBWrapper();
        //        IDataParameter[] insparam ={
        //                                       new SqlParameter("@In_Patient_ID", objins.ToReference_ID),
        //                                       new SqlParameter("@In_Provider_ID", objins.FromReference_ID),
        //                                       new SqlParameter("@In_Appointmentdate",objins.StartDate ),
        //                                       new SqlParameter("@In_AppointmentTime",objins.AppointmentTime),
        //                                       new SqlParameter("@In_Appointment_ID",aptid),
        //                                       new SqlParameter("@in_duration",objins.Duration)
        //                                  };
        //        IDataParameter[] outparam ={
        //                                       new SqlParameter("@Out_emailmessagetransaction_id",SqlDbType.Int,100)
        //                                  };
        //        obj.AddInParameters(insparam);
        //        obj.AddOutParameters(outparam);
        //        obj.ExecuteNonQuerySP("Help_dbo.st_Provider_INS_AppointmentReminderTransaction");
        //        int EmailTranid;
        //        if (!DBNull.Value.Equals(obj.objdbCommandWrapper.Parameters["@Out_emailmessagetransaction_id"].Value))
        //        {
        //            EmailTranid = Convert.ToInt32(obj.objdbCommandWrapper.Parameters["@Out_emailmessagetransaction_id"].Value);
        //        }
        //        else
        //        {
        //            EmailTranid = 0;
        //        }
        //        return EmailTranid;

        //    }
        //    catch (Exception)
        //    {
                
        //        throw;
        //    }
        //}
        //public void updateEmailSattus(string EmailTransactionID)
        //{
        //    try
        //    {
        //        clsDBWrapper obj1 = new clsDBWrapper();
        //        IDataParameter[] insparam = { 
        //                                         new SqlParameter("@In_EmailMessage_Transaction_IDs",EmailTransactionID)
        //                                         };
        //        obj1.AddInParameters(insparam);
        //        obj1.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
        //    }
        //    catch (Exception)
        //    {
                
        //        throw;
        //    }
           
        //}

        public string InsertPublicAppointment(AddAppointmentModel objCreateAppointment, ref int? aptid)
        {
            try
            {
                IDataParameter[] InparamList = {
					new SqlParameter("@In_FromReference_ID", (objCreateAppointment.FromReference_ID != null ? objCreateAppointment.FromReference_ID : null)),
					new SqlParameter("@in_FromReferenceLogin_ID", (objCreateAppointment.FromReferenceLogin_ID != null ? objCreateAppointment.FromReferenceLogin_ID : null)),
					new SqlParameter("@in_FromReferenceType_ID", (objCreateAppointment.FromReferenceType_ID != null ? objCreateAppointment.FromReferenceType_ID :null)),
					new SqlParameter("@In_ToReference_ID", (objCreateAppointment.ToReference_ID != null ? objCreateAppointment.ToReference_ID :null)),
					new SqlParameter("@in_ToReferenceLogin_ID", (objCreateAppointment.ToReferenceLogin_ID != null ? objCreateAppointment.ToReferenceLogin_ID : null)),
					new SqlParameter("@in_ToReferenceType_ID", (objCreateAppointment.ToReferenceType_ID != null ? objCreateAppointment.ToReferenceType_ID : null)),                    
					new SqlParameter("@In_StartDate", objCreateAppointment.StartDate),
					new SqlParameter("@In_EndDate", objCreateAppointment.EndDate),                    
                    //new SqlParameter("@In_PlaceOfService_ID", (objCreateAppointment.PlaceOfService_ID != null ? objCreateAppointment.PlaceOfService_ID : null)),
					new SqlParameter("@In_AppointmentType_ID", (objCreateAppointment.AppointmentType_ID != null ? objCreateAppointment.AppointmentType_ID : null)),                   
					new SqlParameter("@In_Notes", objCreateAppointment.Notes),
					new SqlParameter("@In_AppointmentTime", objCreateAppointment.AppointmentTime),
					new SqlParameter("@In_Duration", (objCreateAppointment.Duration != null ? objCreateAppointment.Duration.ToString() : null)),
					new SqlParameter("@In_AppointmentStatus_ID", (objCreateAppointment.AppointmentStatus_ID != null ? objCreateAppointment.AppointmentStatus_ID.ToString() : null)),
					new SqlParameter("@in_CreatedBy", (objCreateAppointment.CreatedBy != null ? objCreateAppointment.CreatedBy.ToString() : null)),
					new SqlParameter("@in_IsValidate_Ind", objCreateAppointment.IsValidate_Ind),                    
					new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"]),
                    new SqlParameter("@in_Technician_ids",null),
                    new SqlParameter("@in_Technician_Position",null)
				};
                IDataParameter[] OutParamlist = {
					new SqlParameter("@out_msg", SqlDbType.VarChar, 6000),
					new SqlParameter("@Out_Appointment_Id", SqlDbType.BigInt, 100)
				};
                objWrapper = new clsDBWrapper();
                var _with1 = objWrapper;
                _with1.AddInParameters(InparamList);
                _with1.AddOutParameters(OutParamlist);
                _with1.ExecuteNonQuerySP("Help_dbo.st_Appointment_INS_ProviderRecurrenceAppointments_Public");
                string strMessage = null;
                if (objWrapper.objdbCommandWrapper.Parameters["@out_msg"].Value != null)
                {
                    strMessage = objWrapper.objdbCommandWrapper.Parameters["@out_msg"].Value.ToString();
                }
                if (!DBNull.Value.Equals(objWrapper.objdbCommandWrapper.Parameters["@Out_Appointment_Id"].Value))
                {
                    aptid = Convert.ToInt32(objWrapper.objdbCommandWrapper.Parameters["@Out_Appointment_Id"].Value);
                }
                return strMessage;
            }
            catch (Exception ex)
            {
                //ErrorLog.clsExceptionLog clsCustomEx = new ErrorLog.clsExceptionLog();
                //clsCustomEx.LogException(ex, ClassName, "InsertAppointment", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
#endregion
    }
}


