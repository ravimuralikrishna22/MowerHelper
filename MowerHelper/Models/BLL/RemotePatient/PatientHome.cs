using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.VisualBasic;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Collections.Specialized;

using MowerHelper.Models.DAL;
using MowerHelper.Models.DAL.DALPATIENT;


namespace MowerHelper.Models.BLL.RemotePatient
{
    public class PTHome
    {
        #region " Feilds Declarations  "


//private string _PaapLogin_Id;
//private int _Authorizedparty_ID;
private int _Appointment_ID;
private int _FromReferencetype_ID;
private int _FromReference_ID;
private int _FromReferenceLogin_ID;
private int _ToReferenceLogin_ID;
private int _ToReference_ID;
private int _ToReferenceType_ID;
//private int _FacilityReference_ID;
//private int _AppointmentRecurrence_ID;
private int _AppointmentType_ID;
private int _AppointmentMode_ID;
//private int _CurrentAppointmentTracking_ID;
private string _AppointmentDate;
private string _AppointmentTime;
private int _Duration;
//static string _Notes;
//static string _RequestedDate;
//private int _AppointmentStatus_ID;
private string _AppointmentStatus;
private int _IntervalType_ID;
private int _IntervalValue;
//private int _NumofOccurrences;
private DateTime _StartDate;
private string _EndDate;
//private string _SelectedWeekdays;
private int _CPTCode;
//private int _GroupNo;
private char _Group_Ind;
//private DateTime _CreatedOn;
//private DateTime _UpdatedOn;
//private char _Status_Ind;
private int _ProviderPatient_ID;
private int _Practice_ID;
//private int _PracticeProvider_ID;
private int _Provider_ID;
private int _ProviderLogin_ID;
//private int _ProviderAccount_ID;
private int? _PlaceOfService_ID;
private int _Patient_ID;
private int _PatientLogin_ID;
//private int _PatientAccount_ID;
private string _Prefix;
private string _FirstName;
private string _MI;
private string _LastName;
private string _PatientName;
private string _Suffix;
private string _DOB;
//private int _Race_ID;
private string _Gender;
//private string _SSN;
private string _PatientEmail;
//private string _MaritalStatus;
//private string _EmployeeType;
//private string _EmployerName;
//private char _IsPatientRespParty;
//private string _DrivingLicenceNo;
//private string _EmergencyContactName;
//private string _EmergencyContactPhone;
//private string _EmergencyContactExtension;
//private char _EmergencyContactPhoneLeaveMsg;
//private string _EmergencyRelation;
//private char _SendToMainDB_Ind;
//private char _IsCoveredByInsurance;
//private char _IsOwnRespParty;
//private char _IsBenifitProfileCreated_ind;
//private char _IsBenifitProfileVerified_ind;
//private char _ShouldVerifyBenefits;
//private char _PreCertsRequired_Ind;
//private char _CheckWithInsuranceCompany;
//private char _DigitalSOF_Ind;
//private DateTime _DigitalSOF_Date;
//private char _PaperSOF_Ind;
//private DateTime _PaperSOF_Date;
//private char _IsQuestionnaireDone_Ind;
//private char _DidPatientHasInsurance_Ind;
//private char _DidPatientHasRespParty_Ind;
//private char _DidPatientHasQuestionnaire_Ind;
//private char _DidPatientHasRefPhysician_Ind;
//private int _Patient_Status_ID;
//private char _OnlineSession_Ind;
//private string _MedicalAllergies;
//private string _RegisteredBy_Name;
//private char _Auto_Reminder_Allow_Ind;
//private char _SelfRegistered_Ind;
//private int _AddressType_ID;
//private string _Email;
//private string _Email2;
private string _Address1;
private string _Address2;
private int _City_ID;
private int _State_ID;
//private int _Country_ID;
private string _ZIP;
private string _HomePhone;
private string _HPExtension;
private char _HPLeaveMsg_Ind;
//private char _HPIsUSFormat_Ind;
private string _WorkPhone;
private string _WPExtension;
private char _WPLeaveMsg_Ind;
//private char _WPIsUSFormat_Ind;
//private string _CellPhone;
private string _MobilePhone;
private char _MPLeaveMsg_Ind;
//private string _Pager;
//private char _PLeaveMsg_Ind;
//private string _Fax;
//private char _FLeaveMsg_Ind;
//private char _FIsUSFormat_Ind;
//private string _Website;
//private char _PrimaryAddress_Ind;
//private int _PatientAuthorization_ID;
private int _ActualProvider_ID;
private int _ActualProviderLogin_ID;
private int _AuthorizedProvider_ID;
//private int _AuthorizedProviderLogin_ID;
//private char _IsAuthorizationActive;
//private DateTime _AssignedDate;
//private DateTime _LastActivatedDate;
//private DateTime _LastClosedDate;
private string _LastApptDate;
private string _NextApptDate;
private double _RespPartyDueAmount;
private int _NextApptID;
private int _Message_ID;
private int _Slno;
private string _SendingDate;
private string _Sender;
private string _topic;
private string _NewsHeader;
private string _NewsFull;
private int _News_ID;
private DateTime _News_Date;
private string _State;
private string _city;
private int _Login_ID;
private string _PlaceOfService;
private string _ProviderName;
private string _PTNotes;
//private string _ProviderNotes;
private double _TotalCharges;
private double _ToBePaidByRespParty;
private double _RespBalance;
private double _TotalBalance;
private string _WDay;
private string _Practicename;
//static int _ProviderUserID;
//static string _MsgCount;
private string _Nickname;
//private int _Reference_ID;
//private int _ReferenceType_ID;
//private int _ReferenceLogin_ID;
private string _Name;
//private string _UserName;
//private string _Password;
//private int _Patient_AuthorizedParty_ID;
//private string _IsResponsble;
//private string _InsuredDOB;
//private string _InsuredSSN;
private string _ImagePath;
private string _EncryptedImagePath;
//private string _InsInfo;

//static string _ImageFilePath;
//private string _transcript;
//private string _CreatedDate;
//private string _TimePeriod;
//private string _LoginCount;
//private string _CreatedBy;
//private string _IsEditable;
//private string _IsExist;
//private int _Percentage;
//private string _AppointmentBy;
//private string _strBalanceInd;
//private string _PartyName;
//private string _Roles;
//private string _DateCreated;
//private string _OwnerLoginName;
//private string _LastRunDate;
//private string _Category;
//private string _DateLastModified;
//private string _NextRunDate;
//private string _IsEnabled;
//private string _LastRunOutcome;

//private string _RoleID;
private string _IsHomephoneMain_Ind;
private string _IsWorkphoneMain_Ind;

private string _IsMobilephoneMain_Ind;
//private string _CancelEmail_Ind;
	#endregion
private string _MPtotext;

        #region " Constructors  "


public PTHome()
{
}
//13
public PTHome(int FromReference_ID, int FromReferenceLogin_ID, int FromReferenceType_ID, int ToReference_ID, int ToReferenceLogin_ID, int ToReferenceType_ID, int CPTCode, string StartDate, int PlaceOfService_ID, int AppointmentMode_ID,
string Notes, string AppointmentTime, int Duration)
{
	_FromReference_ID = FromReference_ID;
	_FromReferenceLogin_ID = FromReferenceLogin_ID;
	_FromReferencetype_ID = FromReferenceType_ID;
	_ToReference_ID = ToReference_ID;
	_ToReferenceLogin_ID = ToReferenceLogin_ID;
	_ToReferenceType_ID = ToReferenceType_ID;
    _StartDate = Convert.ToDateTime(StartDate);
	_PlaceOfService_ID = PlaceOfService_ID;
	_AppointmentMode_ID = AppointmentMode_ID;
	_PTNotes = Notes;
	_AppointmentTime = AppointmentTime;
	_Duration = Duration;
	_CPTCode = CPTCode;
}
//2
public PTHome(int CptCode, int Duration)
{
	_CPTCode = CptCode;
	_Duration = Duration;
}
//1
public PTHome(int Login_ID)
{
	_Login_ID = Login_ID;
}
public string Objectname
{
    get;
    set;
}
public string ObjectType
{
    get;
    set;
}
public string Description
{
    get;
    set;
}
public string CreatedBy
{
    get;
    set;
}
public string CreatedOn
{
    get;
    set;
}
public string modify_date
{
    get;
    set;
}
public string IsScriptGen
{
    get;
    set;
}
public string DBA_Script_ID
{
    get;
    set;
}

public PTHome(int Patient_ID, int PatientLogin_ID, string Name, int Practice_ID)
{
	_Patient_ID = Patient_ID;
	_PatientLogin_ID = PatientLogin_ID;
	_Name = Name;
	_Practice_ID = Practice_ID;
}
//9
public PTHome(int Practice_ID, string Practicename, int Provider_ID, string Providername, int? PlaceOfService_ID, string PlaceOfService, string str1, string str2, string str3)
{
	_Practice_ID = Practice_ID;
	_Practicename = Practicename;
	_Provider_ID = Provider_ID;
	_ProviderName = Providername;
	_PlaceOfService_ID = PlaceOfService_ID;
	_PlaceOfService = PlaceOfService;
}
//10
public PTHome(Int16 SlNo, Int16 Appointment_ID, string AppointmentDate, string AppointmentTime, string AppointmentStatus, string PtNotes, double TotalCharges, double ToBePaidByRespParty, double RespBalance, double TotalBalance)
{
	_Slno = SlNo;
	_Appointment_ID = Appointment_ID;
	_AppointmentDate = AppointmentDate;
	_AppointmentTime = AppointmentTime;
	_AppointmentStatus = AppointmentStatus;
	_PTNotes = PtNotes;
	_TotalCharges = TotalCharges;
	_ToBePaidByRespParty = ToBePaidByRespParty;
	_RespBalance = RespBalance;
	_TotalBalance = TotalBalance;
}
//16
public PTHome(string PlaceOfService, string ProviderName, string PatientName, Int16 AppointmentMode_ID, Int16 AppointmentType_ID, string Notes, string AppointmentDate, string AppointmentTime, Int16 Duration, Int16 IntervalType_ID,
Int16 IntervalValue, string Enddate, string AppointmentStatus, char Group_Ind, int CPTCode, int Login_ID)
{
	_PlaceOfService = PlaceOfService;
	_ProviderName = ProviderName;
	_PatientName = PatientName;
	_AppointmentMode_ID = AppointmentMode_ID;
	_AppointmentType_ID = AppointmentType_ID;
	_PTNotes = Notes;
	_AppointmentDate = AppointmentDate;
	_AppointmentTime = AppointmentTime;
	_Duration = Duration;
	_IntervalType_ID = IntervalType_ID;
	_IntervalValue = IntervalValue;
	_EndDate = Enddate;
	_AppointmentStatus = AppointmentStatus;
	_Group_Ind = Group_Ind;
	_CPTCode = CPTCode;
	_Login_ID = Login_ID;
}
//33
public PTHome(string Address1, string Address2, string State, string city, string ZIP, string HomePhone, string HPExtension, char HPLeaveMsg_Ind, string MobilePhone, char MPLeaveMsg_Ind,
string WorkPhone, string WpExtension, char WPLeaveMsg_Ind, string homemain, string workmain, string cellmain, string PatientEmail, int Patient_ID, int Login_ID, int AuthorizedProvider_ID,
int ActualProviderLogin_ID, int ProviderPatient_ID, int Practice_ID, string PatientName, int State_ID, int City_ID, string Nickname, string Prefix = null, string Fname = null, string Mname = null,
string lastname = null, string suffix = null, string Dob = null, string Gender = null, string ImagePath = null, string EncryptedImagePath = null, string Celltotext = null)
{
	_Address1 = Address1;
	_Address2 = Address2;
	_State = State;
	_city = city;
	_ZIP = ZIP;
	_HomePhone = HomePhone;
	_HPExtension = HPExtension;
	_HPLeaveMsg_Ind = HPLeaveMsg_Ind;
	_WorkPhone = WorkPhone;
	_WPExtension = WpExtension;
	_HPLeaveMsg_Ind = HPLeaveMsg_Ind;
	_WPLeaveMsg_Ind = WPLeaveMsg_Ind;
	_MobilePhone = MobilePhone;
	_MPLeaveMsg_Ind = MPLeaveMsg_Ind;
	_IsHomephoneMain_Ind = homemain;
	_IsWorkphoneMain_Ind = workmain;
	_IsMobilephoneMain_Ind = cellmain;
	_PatientEmail = PatientEmail;
	_Patient_ID = Patient_ID;
	_Login_ID = Login_ID;
	_AuthorizedProvider_ID = AuthorizedProvider_ID;
	_ActualProviderLogin_ID = ActualProviderLogin_ID;
	_ProviderPatient_ID = ProviderPatient_ID;
	_Practice_ID = Practice_ID;
	_PatientName = PatientName;
	_State_ID = State_ID;
	_City_ID = City_ID;
	_Nickname = Nickname;
	_Prefix = Prefix;
	_FirstName = Fname;
	_MI = Mname;
	_LastName = lastname;
	_Suffix = suffix;
	_DOB = Dob;
	_Gender = Gender;
	_ImagePath = ImagePath;
	_EncryptedImagePath = EncryptedImagePath;
	_MPtotext = Celltotext;
}
//8
public PTHome(int Patient_ID, string PatientName, string DOB, string LastApptDate, string NextApptDate, double RespPartyDueAmount, int NextApptID, int ActualProvider_ID)
{
	_Patient_ID = Patient_ID;
	_PatientName = PatientName;
	_DOB = DOB;
	_LastApptDate = LastApptDate;
	_NextApptDate = NextApptDate;
	_RespPartyDueAmount = RespPartyDueAmount;
	_NextApptID = NextApptID;
	_ActualProvider_ID = ActualProvider_ID;
}
//7
public PTHome(int SlNo, string AppointmentDate, string AppointmentTime, string WDay, int ProviderLogin_ID, string Str1, string str2)
{
	_AppointmentTime = AppointmentTime;
	_AppointmentDate = AppointmentDate;
	_WDay = WDay;
	_ProviderLogin_ID = ProviderLogin_ID;
	_Slno = SlNo;
}
//5
public PTHome(int Message_ID, string SendingDate, string Sender, string topic, int Slno)
{
	_Message_ID = Message_ID;
	_SendingDate = SendingDate;
	_Sender = Sender;
	_topic = topic;
	_Slno = Slno;
}
//6
public PTHome(int News_ID, DateTime News_Date, string NewsHeader, int Slno, string NewsFull, string Str)
{
	_News_ID = News_ID;
	_News_Date = News_Date;
	_NewsHeader = NewsHeader;
	_Slno = Slno;
	_NewsFull = NewsFull;
}
#endregion


        public static DataSet getPatientAlphabets()
        {
           // PatientDataAccessLayerBaseClass DBLayer = PatientDataAccessLayerBaseClassHelper.GetDataAccessLayer();
            PatientSQLDBlayer obj =new PatientSQLDBlayer();
            return obj.getPatientAlphabets();
        }
        public static int GetProviderLoginID(int PatientProviderID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.GetProviderLoginID(PatientProviderID);
        }
        public static List<PTHome> GetPracticesOfPatient(int PatientLogin_ID, int? practice_ID, int? ProviderID = null)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.GetPracticesOfPatient(PatientLogin_ID, practice_ID, ProviderID);
        }

        public static DataSet getProviderAlphabet()
        {
            PatientSQLDBlayer obj = new PatientSQLDBlayer();
            return obj.getProviderAlphabet();
        }

        public static DataSet ListDBobjects(string Fromdate, string ToDate, string Objectname, string IsScriptGenerated)
        {
            PatientSQLDBlayer DBLayer = new PatientSQLDBlayer();
            return DBLayer.ListDBobjects(Fromdate, ToDate, Objectname, IsScriptGenerated);
        }

        public static DataSet Fillusers(string spName)
        {
            PatientSQLDBlayer DBLayer = new PatientSQLDBlayer();
            return DBLayer.Fillusers(spName);
        }

        public static string InsertObjectDetails(string Objectname, string Objecttype, string ObjectDesc, string userID, string createdDate)
        {
            PatientSQLDBlayer DBLayer = new PatientSQLDBlayer();
            return DBLayer.InsertObjectDetails(Objectname, Objecttype, ObjectDesc, userID, createdDate);
        }

        public static string udpateDbscript(string dbScript, string ScriptID)
        {
            PatientSQLDBlayer DBLayer = new PatientSQLDBlayer();
            return DBLayer.udpateDbscript(dbScript, ScriptID);
        }

        public static string upDateScriptIDs(string ScriptIDs)
        {
            PatientSQLDBlayer DBLayer = new PatientSQLDBlayer();
            return DBLayer.upDateScriptIDs(ScriptIDs);
        }


    }
}