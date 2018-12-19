using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;
using System.Data.SqlClient;
using System.Data;

namespace MowerHelper.Models.BLL.Admin
{
    public class Login_history
    {

        public int NoofRecords
        {
            get;
            set;
        }
        public int PageNo
        {
            get;
            set;
        }
        public string OrderBy
        {
            get;
            set;
        }
        public string OrderByItem
        {
            get;
            set;
        }
        public int? Login_ID
        {
            get;
            set;
        }
        public string BeginDate
        {
            get;
            set;
        }
        public string EndDate
        {
            get;
            set;
        }
        public string Sino
        {
            get;
            set;
        }
        public string Username
        {
            get;
            set;
        }
        public string RoleName
        {
            get;
            set;
        }
        public string IPAddress
        {
            get;
            set;
        }
        public string LogIn_DateTime
        {
            get;
            set;
        }
        public bool LoginSuccess_Ind
        {
            get;
            set;
        }
        public string logsuccess
        {
            get;
            set;
        }
        public int? LoginHistory_ID { get; set; }
        public string Password { get; set; }
        public string LogOut_DateTime { get; set; }
        public int Project_ID { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string IP_Address { get; set; }
        public string UserAgent { get; set; }
        public string CreatedOn { get; set; }
        public string Siteurl { get; set; }
        public string loginstatus { get; set; }
        public string Mobileowner_id { get; set; }
        public string deviceownername { get; set; }
        public string deviceaddress { get; set; }
        public string SimOperatorName { get; set; }
        public string devicePhone { get; set; }
        public string Mobile_type { get; set; }
        
        
        
        public Login_history()
        {
        }

        public Login_history(int _LoginHistory_ID, string _Username, string _Password, string _RoleName, string _IPAddress, string _LogIn_DateTime, string _LogOut_DateTime, bool _LoginSuccess_Ind, int _Project_ID)
{
	LoginHistory_ID = _LoginHistory_ID;
	Username = _Username;
	Password = _Password;
	RoleName = _RoleName;
	IPAddress = _IPAddress;
	LogIn_DateTime = _LogIn_DateTime;
	LogOut_DateTime = _LogOut_DateTime;
	LoginSuccess_Ind = _LoginSuccess_Ind;
    if (_LoginSuccess_Ind == true)
    {
        loginstatus = "Yes";
    }
    else
    {
        loginstatus = "No";
    }
	Project_ID = _Project_ID;
}
        public static List<Login_history> Admin_Provider_Login_history(Login_history objlogindata, ref int Total_Records)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Admin_Provider_Login_history(objlogindata,ref Total_Records);
        }

        public static List<Login_history> Admin_List_Login_history(Login_history objloginhistory, ref int Total_Records, string Rolename)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Admin_List_Login_history(objloginhistory, ref Total_Records, Rolename);
        }

        public static DataSet Admin_List_Site_history(string IPAddress, string ProjectName, string StateName, string CityName, string CountryName, string OrderBy, string OrderBYItem, int PageNo, int NoOfrec, string FromDate,
        string ToDate, ref int Total_Records)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Admin_List_Site_history(IPAddress, ProjectName, StateName, CityName, CountryName, OrderBy, OrderBYItem, PageNo, NoOfrec, FromDate,
            ToDate,ref Total_Records);
        }

        public static DataSet Admin_List_Site_history_Mobile(string FromDate, string ToDate, string orderby, string orderbyitem)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Admin_List_Site_history_Mobile(FromDate, ToDate, orderby, orderbyitem);
        }
        public static DataSet Admin_Get_Site_history_Mobile(int Mobileowner_id)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Admin_Get_Site_history_Mobile(Mobileowner_id);
        }
        public static List<Login_history> WebLoginHistoryDetails(Login_history objloginhistory, ref int Total_Records)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.WebLoginHistoryDetails(objloginhistory, ref Total_Records);
        }
    }
}