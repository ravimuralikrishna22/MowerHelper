using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.Admin
{
    public class Admin_postajob
    {
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
        public int? PageNo
        {
            get;
            set;
        }
        public int? NoOfRecords
        {
            get;
            set;
        }
        public string Full_Name
        {
            get;
            set;
        }
        public string LastName
        {
            get;
            set;
        }
        public string slno
        {
            get;
            set;
        }
        public string fulladdress
        {
            get;
            set;
        }
        public string WorkPhone
        {
            get;
            set;
        }
        public int? verification_User_Id
        {
            get;
            set;
        }
        public int? Login_ID
        {
            get;
            set;
        }
        public char? Status_Ind
        {
            get;
            set;
        }
        public string RoleType
        {
            get;
            set;
        }
        public int Total_Records
        {
            get;
            set;
        }
        public string Address1
        {
            get;
            set;
        }
        public string Address2
        {
            get;
            set;
        }
        public string CellPhone
        {
            get;
            set;
        }
        public Nullable<int> City_ID{
            get;
            set;
        }
        public Nullable<int> Country_ID
        {
            get;
            set;
        }
        public int CreatedBy
        {
            get;
            set;
        }
        public string DOB
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
        public string Fax
        {
            get;
            set;
        }
        public string FirstName
        {
            get;
            set;
        }
        public string Gender
        {
            get;
            set;
        }
        public string HomePhone
        {
            get;
            set;
        }
        public string HPExtension
        {
            get;
            set;
        }
        public string HPLeaveMsg_Ind
        {
            get;
            set;
        }
        public string MI
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public Nullable<int> State_ID
        {
            get;
            set;
        }
        public string Suffix
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }
        public string WPExtension
        {
            get;
            set;
        }
        public string WPLeaveMsg_Ind
        {
            get;
            set;
        }
        public string zip
        {
            get;
            set;
        }
        public string FIsUSFormat_Ind
        {
            get;
            set;
        }
        public string FLeaveMsg_Ind
        {
            get;
            set;
        }
        public string HPIsUSFormat_Ind
        {
            get;
            set;
        }
        public string MobilePhone
        {
            get;
            set;
        }
        public string MPLeaveMsg_Ind
        {
            get;
            set;
        }
        public string Pager
        {
            get;
            set;
        }
        public string PLeaveMsg_Ind
        {
            get;
            set;
        }
        public string PrimaryAddress_Ind
        {
            get;
            set;
        }
        public string WPIsUSFormat_Ind
        {
            get;
            set;
        }
        public string city
        {
            get;
            set;
        }
        public string Country
        {
            get;
            set;
        }
        public string Prefix
        {
            get;
            set;
        }
        public string role_id
        {
            get;
            set;
        }
        public string Rolename
        {
            get;
            set;
        }
        public string statename
        {
            get;
            set;
        }
        public string WorkPhoneExtn
        {
            get;
            set;
        }
        public string company_name
        {
            get;
            set;
        }
        public int? JobPosting_User_ID
        {
            get;
            set;
        }
        public string ImageFileName_1
        {
            get;
            set;
        }
        public string SectionDescription
        {
            get;
            set;
        }
        public string SectionName
        {
            get;
            set;
        }
        public string SectionPath
        {
            get;
            set;
        }
        public static List<Admin_postajob> verificationusersList(Admin_postajob objusersList, ref int Total_Records)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.verificationusersList(objusersList,ref Total_Records);
        }
        public static object Insert_verificationusers(Admin_postajob ObjIns, ref string Out_Msg, ref int Out_verification_User_ID, ref int Out_Login_ID, int? loginhistoryid = null)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.Insert_verificationusers(ObjIns,ref Out_Msg,ref Out_verification_User_ID,ref Out_Login_ID, loginhistoryid);
        }
        public static List<Admin_postajob> verificationusersView(int? loginID, int? verification_User_ID)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.verificationusersView(loginID, verification_User_ID);
        }
        public static string verificationuserUpdate(Admin_postajob objuser)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.verificationuserUpdate(objuser);
        }
        public static string InActive_verificationusers(Admin_postajob objstatus, int Loginhistory_id)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.InActive_verificationusers(objstatus, Loginhistory_id);
        }
        public static List<Admin_postajob> ChildSections(string prov_id, string practice_id, string login_id, string section_id, string Role_id)
        {
            SQLDataAccessLayer obj = new SQLDataAccessLayer();
            return obj.ChildSections(prov_id, practice_id, login_id, section_id, Role_id);
        }
    }
}