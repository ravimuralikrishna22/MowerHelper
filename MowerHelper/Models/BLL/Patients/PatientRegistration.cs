using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;
using MowerHelper.Models.Classes;

namespace MowerHelper.Models.BLL.Patients
{
    public class PatientRegistration
    {
        public string secinsind
        {
            get;set;
        }
        public string praid
        {
             get;set;
        }
        public string proid
        {
            get;set;
        }
        public string patid
        {
           get;set;
        }
        public int ReferenceLogin_ID
        {
            get;set;
        }
        public int SlNo
        {
           get;set;
        }

        public int NoOfRecords
        {
            get;set;
        }

        public int PageNo
        {
            get;set;
        }

        public string OrderByitem
        {
           get;set;
        }

        public string OrderBy
        {
           get;set;
        }

        public string PublicURL
        {
            get;set;
        }
        public string Website
        {
            get;set;
        }
        public string FullAddress
        {
           get;set;
        }

        public string FullName
        {
           get;set;
        }

        public string ContactName
        {
            get;set;
        }

        public int Contact_Id
        {
            get;set;
        }
        public int? Reference_ID
        {
            get;set;
        }
        public string PlaceOfService
        {
            get;set;
        }
        public string Country
        {
            get;set;
        }

        public string PatientName
        {
           get;set;
        }

        public int? Provider_ID
        {
           get;set;
        }

        public string ProviderName
        {
            get;set;
        }

        public string PracticeName
        {
            get;set;
        }

        public int? PlaceOfService_ID
        {
           get;set;
        }

        public int? ProviderPatient_ID
        {
            get;set;
        }

        public string Prefix
        {
           get;set;
        }

        public string FirstName
        {
           get;set;
        }

        public string MI
        {
           get;set;
        }

        public string LastName
        {
           get;set;
        }

        public string Suffix
        {
            get;set;
        }

        public string PatientEmail
        {
            get;set;
        }

        public string DrivingLicenceNo
        {
            get;set;
        }

        public string HomePhone
        {
           get;set;
        }

        public string WPhone
        {
           get;set;
        }

        public string MPhone
        {
           get;set;
        }

        public string Address1
        {
           get;set;
        }

        public string Address2
        {
           get;set;
        }

        public int? City_ID
        {
           get;set;
        }

        public string City
        {
            get;set;
        }

        public int? State_ID
        {
           get;set;
        }

        public string State
        {
            get;set;
        }

        public int? Country_ID
        {
            get;set;
        }

        public string ZIP
        {
            get;set;
        }

        public int? Login_ID
        {
           get;set;
        }
        //public string ProvPrac_ID
        //{
        //    get;
        //    set;
        //}
        public string Username
        {
            get;set;
        }
        public string Password
        {
            get;set;
        }

        public int? Practice_ID
        {
            get;set;
        }

        public int? Patient_Status_ID
        {
           get;set;
        }

        public int? Patient_ID
        {
           get;set;
        }
        public int? UserId
        {
           get;set;
        }

        public int Role_ID
        {
           get;set;
        }
        public string RoleName
        {
            get;set;
        }
        public int ReferenceType_ID
        {
            get;set;
        }
        public string Status_Ind
        {
           get;set;
        }
        public string ImagePath
        {
            get;set;
        }
        public string Customer_Name
        {
            get;
            set;
        }
        public string Customer_Id
        {
            get;
            set;
        }
        public string Customer_LoginId
        {
            get;
            set;
        }
        public string Customer_Email
        {
            get;
            set;
        }
        public string GmapLatitude { get; set; }
        public string GmapLongitude { get; set; }
        //public string comboPracid { get; set; }
        public string HomePhone1 { get; set; }
        public string HomePhone2 { get; set; }
        public string HomePhone3 { get; set; }
        public string WorkPhone1 { get; set; }
        public string WorkPhone2 { get; set; }
        public string WorkPhone3 { get; set; }
        public string MobilePhone1 { get; set; }
        public string MobilePhone2 { get; set; }
        public string MobilePhone3 { get; set; }
        public string Ind { get; set; }
        public static string Insert_Patinet_FT(PatientRegistration objIns, ref string Out_Msg, ref string Out_login_id)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Insert_Patinet_FT(objIns,ref Out_Msg, ref Out_login_id);
        }

        public List<PatientRegistration> BindComboPracticeProviderSearch(string PracticeName)
        {
            clsCommonFunctions obj = new clsCommonFunctions();
            return obj.BindComboPracticeProviderSearch(PracticeName);
        }
        public List<PatientRegistration> BindComboverificationusersearch(string PracticeName,string roleid)
        {
            clsCommonFunctions obj = new clsCommonFunctions();
            return obj.BindComboverificationusersearch(PracticeName, roleid);
        }
        public static PatientRegistration Get_Random_UserCredentials()
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Get_Random_UserCredentials();
        }
        public static string Insert_Security_INS_FTUser(PatientRegistration objUser)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Insert_Security_INS_FTUser(objUser);
        }
    }
}