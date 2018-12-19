using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.Providers
{
    public class Provider_ProviderList
    {
        public string RoleID
        {
            get;
            set;
        }
        public string state_id
        {
            get;
            set;
        }
        public string providerdocuments
        {
            get;
            set;
        }
        public string verifiedon
        {
            get;
            set;
        }
        public string verifiedby
        {
            get;
            set;
        }
        public string TreatmentPlannerInd
        {
            get;
            set;
        }
        public static string TotalRecords
        {
            get;
            set;
        }
        public int SlNo
        {
            get;
            set;
        }
        public int Provider_ID 
        {
            get;
            set;
        }
        public string ProviderName
        {
            get;
            set;
        }
        public string Address
        {
            get;
            set;
        }
        public string DOB
        {
            get;
            set;
        }
        public string UpdatedOn
        {
            get;
            set;
        }
        public int? Login_ID
        {
            get;
            set;
        }
        public int? Practice_ID
        {
            get;
            set;
        }
        public string Status_Ind
        {
            get;
            set;
        }
        public string ServiceUser_ID
        {
            get;
            set;
        }
        public string ServiceUserstatus
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
        public string LicenseNo
        {
            get;
            set;
        }
        public string licenseexpirydate
        {
            get;
            set;
        }
        public string CellPhone
        {
            get;
            set;
        }
        public string provideremail
        {
            get;
            set;
        }
        public string RegisteredDate
        {
            get;
            set;
        }
        public int? Techniciancount
        {
            get;
            set;
        }
        public string ElectricianFillename { get; set; }
        public string DriverFilename { get; set; }
        public string DriverCustomiseImage { get; set; }
        public string ElctricianCustmiseImage { get; set; }
        public string Registeredin { get; set;}
        public string January { get; set; }
        public string February { get; set; }
        public string March { get; set; }
        public string April { get; set; }
        public string May { get; set; }
        public string June { get; set; }
        public string July { get; set; }
        public string August { get; set; }
        public string September { get; set; }
        public string October { get; set; }
        public string November { get; set; }
        public string December { get; set; }
        public string Debug_mode { get; set; }
        public Provider_ProviderList()
        {
        }
        public Provider_ProviderList(int _SlNo, int _Provider_ID, string _ProviderName, string _Address, string _DOB, string _UpdatedOn, int? _Login_ID, int? _Practice_ID, string _Status_Ind,string _cellphone,string _provideremail,string _registerdon,int? _techcnt)
{
	SlNo = _SlNo;
	Provider_ID = _Provider_ID;
	ProviderName = _ProviderName;
	Address = _Address;
	DOB = _DOB;
	UpdatedOn = _UpdatedOn;
	Login_ID = _Login_ID;
	Practice_ID = _Practice_ID;
	Status_Ind = _Status_Ind;
    CellPhone = _cellphone;
    provideremail = _provideremail;
    RegisteredDate = _registerdon;
    Techniciancount = _techcnt;
}
        public Provider_ProviderList(int _SlNo, int _Provider_ID, string _ProviderName, string _Address, string _DOB, string _cellphone, string _provideremail, string _registerdon, string _Registeredin)
        {
            SlNo = _SlNo;
            Provider_ID = _Provider_ID;
            ProviderName = _ProviderName;
            Address = _Address;
            DOB = _DOB;
            CellPhone = _cellphone;
            provideremail = _provideremail;
            RegisteredDate = _registerdon;
            Registeredin = _Registeredin;
        }
        public static List<Provider_ProviderList> Get_ElProviderInProfile(string FirstName, string LastName, int? Practice_ID, int PlaceOfService_ID, string OrderByItem, string OrderBy, int? Login_ID, int Role_ID, string Status_Ind, int NoOfRecord,
int PageNo, string providertype, ref int TotalRecords, string Email, int? Provider_ID = 0)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Get_ElProviderInProfile(FirstName, LastName, Practice_ID, PlaceOfService_ID, OrderByItem, OrderBy, Login_ID, Role_ID, Status_Ind, NoOfRecord,
            PageNo, providertype, ref TotalRecords, Email, Provider_ID);
        }
        public static List<Provider_ProviderList> Get_NewElesignups(string OrderByItem, string OrderBy, int NoOfRecord, string Fromdate, string Todate,
       int PageNo, ref int TotalRecords, string Email, int? Provider_ID = 0)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Get_NewElesignups( OrderByItem, OrderBy, NoOfRecord,Fromdate, Todate,
            PageNo, ref TotalRecords, Email, Provider_ID);
        }
        public static List<Provider_ProviderList> list_licenseExpired(Nullable<int> Practice_ID, string fromdate, string todate, string OrderByItem, string OrderBy, int NoOfRecord, int PageNo, string toreferenceids)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.list_licenseExpired(Practice_ID, fromdate, todate, OrderByItem, OrderBy, NoOfRecord, PageNo, toreferenceids);
        }
        public static List<Provider_ProviderList> signups_monthWisecount(string Year)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.signups_monthWisecount(Year);
        }
        public static bool Inactivate_Provider_Status(int Provider_ID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Inactivate_Provider_Status(Provider_ID);
        }
        public static bool Upd_Vacation_Provider_Status(int Provider_ID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Upd_Vacation_Provider_Status(Provider_ID);
        }
        public static bool Del_Provider(int Provider_ID, int DeletedBy)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Del_Provider(Provider_ID, DeletedBy);
        }
    }
}