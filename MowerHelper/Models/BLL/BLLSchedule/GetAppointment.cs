using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using MowerHelper.Models.Classes;
using MowerHelper.Models.DAL.DALSchedule;
namespace MowerHelper.Models.BLL.BLLSchedule
{
    public class GetAppointment
    {
        //private string _Auto_Reminder_Allow_Ind;
        //private int _Appointment_ID;
        //private int _FromReferenceType_ID;
        //private int _FromReference_ID;
        //private int _FromReferenceLogin_ID;
        //private string _ProviderName;
        //private int _ToReference_ID;
        //private int _ToReferenceLogin_ID;
        //private string _PatientName;
        //private int _ToReferenceType_ID;
        //private int _FacilityReference_ID;
        //private string _PlaceofService;
        //private int _AppointmentRecurrence_ID;
        //private int _IntervalType_ID;
        //private string _IntervalType;
        //private int _IntervalValue;
        //private int _NumofOccurrences;
        //private string _Startdate;
        //private string _Enddate;
        //private string _SelectedWeekdays;
        //private int _AppointmentType_ID;
        //private int _AppointmentMode_ID;
        //private Nullable<int> _CPTCode;
        //private char _Group_Ind;
        //private int _GroupNo;
        //private string _AppointmentStatus;
        //private string _Notes;
        //private string _PTNotes;
        //private int _CurrentAppointmentTracking_ID;
        //private string _AppointmentDate;
        //private string _AppointmentTime;
        //private int _Duration;
        //private int _AppointmentStatus_ID;
        //private int _PracticeID;
        //private int _Patient_ID;
        //private int _PatientLogin_ID;
        //private int _Provider_ID;
        //private int _ProviderLogin_ID;
        //private int _PlaceOfService_ID;
        //private string _AppointmentBy;
        //private string _ReminderHR;
        //private string _PatientEmail;
        //private string _phoneautoremainder_ind;
        //private string _phoneremainderhr;
        //private string _cellphone;
        public string Cellphone
        {
            get;
            set;
        }
        public string PatientEmail
        {
            get;
            set;
        }
        public string ReminderHR
        {
            get;
            set;
        }
        public string Auto_Reminder_Allow_Ind
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
        public string AppointmentBy
        {
            get;
            set;
        }
        public int PlaceOfService_ID
        {
            get;
            set;
        }
        public int ProviderLogin_ID
        {
            get;
            set;
        }
        public int Provider_ID
        {
            get;
            set;
        }
        public int PatientLogin_ID
        {
            get;
            set;
        }
        public int Patient_ID
        {
            get;
            set;
        }

        public int Appointment_ID
        {
            get;
            set;
        }

        public int FromReferenceType_ID
        {
            get;
            set;
        }

        public int FromReference_ID
        {
            get;
            set;
        }

        public int FromReferenceLogin_ID
        {
            get;
            set;
        }

        public string ProviderName
        {
            get;
            set;
        }

        public int ToReference_ID
        {
            get;
            set;
        }

        public int ToReferenceLogin_ID
        {
            get;
            set;
        }

        public string PatientName
        {
            get;
            set;
        }
        public int ToReferenceType_ID
        {
            get;
            set;
        }

        public int? FacilityReference_ID
        {
            get;
            set;
        }

        public string PlaceofService
        {
            get;
            set;
        }

        public int AppointmentRecurrence_ID
        {
            get;
            set;
        }

        public int IntervalType_ID
        {
            get;
            set;
        }

        public string IntervalType
        {
            get;
            set;
        }

        public int IntervalValue
        {
            get;
            set;
        }

        public int NumofOccurrences
        {
            get;
            set;
        }

        public string Startdate
        {
            get;
            set;
        }

        public string Enddate
        {
            get;
            set;
        }

        public string SelectedWeekdays
        {
            get;
            set;
        }

        public int AppointmentType_ID
        {
            get;
            set;
        }

        public int AppointmentMode_ID
        {
            get;
            set;
        }

        public Nullable<int> CPTCode
        {
            get;
            set;
        }

        public char Group_Ind
        {
            get;
            set;
        }

        public int GroupNo
        {
            get;
            set;
        }

        public string AppointmentStatus
        {
            get;
            set;
        }
        //public string PTNotes
        //{
        //    get;
        //    set;
        //}
        public string Notes
        {
            get;
            set;
        }

        public int? CurrentAppointmentTracking_ID
        {
            get;
            set;
        }

        public string AppointmentDate
        {
            get;
            set;
        }

        public string AppointmentTime
        {
            get;
            set;
        }

        public int Duration
        {
            get;
            set;
        }

        public int? AppointmentStatus_ID
        {
            get;
            set;
        }

        public int PracticeID
        {
            get;
            set;
        }
        public int? Appointment_address_ID { get; set; }
        public string Defaultaddress_Ind { get; set; }
        public string motivationid { get; set; }
        public string motivation { get; set; }
        public string problem { get; set; }
        public string problemid { get; set; }
        public string attributeid { get; set; }
        public string attribute { get; set; }
        public string intervention { get; set; }
        public string interventionId { get; set; }
        public string ObjectiveId { get; set; }
        public string Objectivepr { get; set; }
        public string narrative { get; set; }
        public string overallPrgress { get; set; }
        public string Userid { get; set; }
        public string VerInd { get; set; }
        public string ProgressDate { get; set; }
        public string Appointment_Length { get; set; }
        public Boolean GeneralNarrativeAlert_Ind { get; set; }
        public Boolean NewProblemsIdentifiedAlert_Ind { get; set; }
        public string IsCompleted { get; set; }
        public int ProgressNotes_ID{get;set;}
        public string Technician_ids { get; set; }
        public string Technicianname { get; set; }
        public string TechnicianPositions { get; set; }
        public string Aptplaceofservice { get; set; }
        public string Aptplaceofservicename { get; set; }
        public string VoiceFileName { get; set; }
        public string Customerexists { get; set; }
             public GetAppointment()
        {
        }
        public GetAppointment(int Appointment_ID, int FromReferenceType_ID, int FromReference_ID, int FromReferenceLogin_ID, string ProviderName, string PatientName, int ToReferenceType_ID,
 string Startdate, int AppointmentType_ID, string AppointmentStatus, string Notes, int CurrentAppointmentTracking_ID, string AppointmentDate, string AppointmentTime, int Duration, int AppointmentStatus_ID, string Defaultaddress_Ind)
{
	this.Appointment_ID = Appointment_ID;
    this.FromReferenceType_ID = FromReferenceType_ID;
    this.FromReference_ID = FromReference_ID;
    this.FromReferenceLogin_ID = FromReferenceLogin_ID;
    this.ProviderName = ProviderName;    
    this.PatientName = PatientName;
    this.ToReferenceType_ID = ToReferenceType_ID;
    this.Startdate = Startdate;
    this.AppointmentType_ID = AppointmentType_ID;    
    this.AppointmentStatus = AppointmentStatus;
    this.Notes = Notes;
    this.CurrentAppointmentTracking_ID = CurrentAppointmentTracking_ID;
    this.AppointmentDate = AppointmentDate;
    this.AppointmentTime = AppointmentTime;
    this.Duration = Duration;
    this.AppointmentStatus_ID = AppointmentStatus_ID;
    this.Defaultaddress_Ind = Defaultaddress_Ind;
  
}

        public GetAppointment(string PlaceOfService, string ProviderName, string PatientName, Nullable<int> CPTCode, int Duration, int Patient_ID, int PatientLogin_ID, int Provider_ID, int ProviderLogin_ID, int PlaceOfService_ID,
string Auto_Reminder_Allow_Ind, string PatientEmail)
{
    this.PlaceofService = PlaceOfService;
    this.ProviderName = ProviderName;
    this.PatientName = PatientName;
    this.CPTCode = CPTCode;
    this.Duration = Duration;
    this.Patient_ID = Patient_ID;
    this.PatientLogin_ID = PatientLogin_ID;
    this.Provider_ID = Provider_ID;
    this.ProviderLogin_ID = ProviderLogin_ID;
    this.PlaceOfService_ID = PlaceOfService_ID;
    this.Auto_Reminder_Allow_Ind = Auto_Reminder_Allow_Ind;
    this.PatientEmail = PatientEmail;
}
        public static List<GetAppointment> GetPatientAppointment(string AppointmentID, string GroupNo, string AppointmentDate)
        {
            SCHSqlDataAccessLayer objschedule = new SCHSqlDataAccessLayer();
            return objschedule.GetPatientAppointment(AppointmentID, GroupNo, AppointmentDate);
            //return SCHDataAccessLayerBaseClassHelper.GetDataAccessLayer().GetPatientAppointment(AppointmentID, GroupNo, AppointmentDate);
        }
        public static List<GetAppointment> GetPendingAppointment(string AppointmentID, string AppointmentDate)
        {
            SCHSqlDataAccessLayer objschedule = new SCHSqlDataAccessLayer();
            return objschedule.GetPendingAppointment(AppointmentID, AppointmentDate);
            //return SCHDataAccessLayerBaseClassHelper.GetDataAccessLayer().GetPatientAppointment(AppointmentID, GroupNo, AppointmentDate);
        }

        public static DataSet GetDayTemplate(string ProviderID, string ApptDate, string ShowAll, string WeekDay)
        {
            SCHSqlDataAccessLayer objschedule = new SCHSqlDataAccessLayer();
            return objschedule.GetDayTemplate(ProviderID,  ApptDate, ShowAll, WeekDay);
            //return SCHDataAccessLayerBaseClassHelper.GetDataAccessLayer().GetDayTemplate(ProviderID, FacilityID, ApptDate, ShowAll, WeekDay);
        }
        public static DataSet GetDayAppointments(string ProviderID,  string ApptDate, string ShowAll,string Tech_id)
        {
            SCHSqlDataAccessLayer objschedule = new SCHSqlDataAccessLayer();
            return objschedule.GetDayAppointments(ProviderID, ApptDate, ShowAll,Tech_id);
            //return SCHDataAccessLayerBaseClassHelper.GetDataAccessLayer().GetDayAppointments(ProviderID, FacilityID, ApptDate, ShowAll, Begindate, Enddate);
        }
        public static DataSet GetWeekAppointments(string ProviderID, string FromDate, string ToDate,string Techid)
        {
            SCHSqlDataAccessLayer objschedule = new SCHSqlDataAccessLayer();
           return objschedule.GetWeekAppointments(ProviderID, FromDate, ToDate,Techid);
            //return SCHDataAccessLayerBaseClassHelper.GetDataAccessLayer().GetWeekAppointments(ProviderID, FacilityID, FromDate, ToDate);
        }
        public static DataSet GetMonthAppointments(string ProviderID, string Month, string Year, string ApptStatusID, string Tech_id)
        {
            SCHSqlDataAccessLayer objschedule = new SCHSqlDataAccessLayer();
            return objschedule.GetMonthAppointments(ProviderID, Month, Year, ApptStatusID, Tech_id);
        //return SCHDataAccessLayerBaseClassHelper.GetDataAccessLayer().GetMonthAppointments(ProviderID, FacilityID, Month, Year, ApptStatusID);
        }       
        public string checkApptExist(string ApptDate, string PatientID, string Apttime, string Provider_ID, string ApptRecID)
        {
            SCHSqlDataAccessLayer objsch = new SCHSqlDataAccessLayer();
            return objsch.checkApptExist(ApptDate, PatientID, Apttime, Provider_ID, ApptRecID);
        }
        public string weeklyApptDates(string apptDate, string recNo, string endDate, string selectedWeek)
        {
            SCHSqlDataAccessLayer objSelectWeekly = new SCHSqlDataAccessLayer();
            return objSelectWeekly.weeklyApptDates(apptDate, recNo, endDate, selectedWeek);
        }

        public static DataSet GetProviderOpenSlots(int? ProviderId, string ApptStartdate, string ApptEnddate, string Starttime, string Endtime, int? Duration)
        {
            SCHSqlDataAccessLayer objschedule = new SCHSqlDataAccessLayer();
            return objschedule.GetProviderOpenSlots(ProviderId, ApptStartdate, ApptEnddate, Starttime, Endtime, Duration);
            //return SCHDataAccessLayerBaseClassHelper.GetDataAccessLayer().GetPatientAppointment(AppointmentID, GroupNo, AppointmentDate);
        }
        public static List<GetAppointment> GetCustomerdetails(string Email, string Phonenumber, string Provider_ID)
        {
            SCHSqlDataAccessLayer objschedule = new SCHSqlDataAccessLayer();
            return objschedule.GetCustomerdetails(Email, Phonenumber, Provider_ID);
        }
        
    }
    public class DeleteAppointment
    {
        public string getPagedate { get; set; }
        public string getWMdate { get; set; }
        public string getMMdate { get; set; }
        public string fromPage { get; set; }
        public string apptdate { get; set; }
        public string providerid { get; set; }
        public string apptDelid { get; set; }
        public string ToRefId { get; set; }
        public string recId { get; set; }
        public string enddate { get; set; }
        public string futureInd { get; set; }
        public string checkDel(string Appointment_ID)
        {
            IDataParameter[] inparamlist ={
                                              new SqlParameter("@in_Appointment_ID",Appointment_ID),
                                              new SqlParameter("@in_ApptRecurrence_ID",null)
                                         };
            IDataParameter[] outparamslist = { new SqlParameter("@in_DeleteInd", SqlDbType.Char, 1) };
            clsDBWrapper obj = new clsDBWrapper();
            obj.AddInParameters(inparamlist);
            obj.AddOutParameters(outparamslist);
            obj.ExecuteNonQuerySP("cogadmin_dbo.st_Scheduling_claim_Existence");
            if (!DBNull.Value.Equals(obj.objdbCommandWrapper.Parameters["@in_DeleteInd"].Value))
            {
                return obj.objdbCommandWrapper.Parameters["@in_DeleteInd"].Value.ToString();
            }
            return null;

        }
        public DeleteAppointment DeleteAppt(DeleteAppointment objDelappt, string GroupNo, string appttime) //String Appointment_ID, String GroupNo, String startdate, String FromRefId, String ToRefId, String DelFuture, String appttime, String ApptRecurrenceID)
        {
            IDataParameter[] InParamList = {
	new SqlParameter("@in_Appointment_ID", objDelappt.apptDelid), //Appointment_ID),
	new SqlParameter("@in_GroupNo", GroupNo),
	new SqlParameter("In_Startdate", objDelappt.getPagedate), //startdate),
	new SqlParameter("@In_FromReference_ID", objDelappt.providerid), //FromRefId),
	new SqlParameter("@In_ToReference_ID", objDelappt.ToRefId), //ToRefId),
	new SqlParameter("@In_delFutureappnt", objDelappt.futureInd), //DelFuture),
	new SqlParameter("@In_AppointmentRecurrence_ID", objDelappt.recId), //(!string.IsNullOrEmpty(ApptRecurrenceID) ? ApptRecurrenceID : null)),
	new SqlParameter("@in_Appt_time", appttime),
	new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
};
            clsDBWrapper objWrapper;
            objWrapper = new clsDBWrapper();
            var _with1 = objWrapper;
            _with1.AddInParameters(InParamList);
            _with1.ExecuteNonQuerySP("Help_dbo.st_Scheduling_Del_Appointment_new");
            return null;
        }        
        public DeleteAppointment DeleteAppt(String Appointment_ID)
        {
            IDataParameter[] InParamList = {
	new SqlParameter("@in_Appointment_ID", Appointment_ID),	
	new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
};
            clsDBWrapper objWrapper;
            objWrapper = new clsDBWrapper();
            var _with1 = objWrapper;
            _with1.AddInParameters(InParamList);
            _with1.ExecuteNonQuerySP("Help_dbo.st_Scheduling_Del_Appointment");
            return null;
        }        
    }
}