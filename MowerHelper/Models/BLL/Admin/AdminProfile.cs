using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MowerHelper.Models.DAL.DALPATIENT;
using MowerHelper.Models.DAL.CDAL;

namespace MowerHelper.Models.BLL.Admin
{
    public class AdminProfile
    {
        public string Username
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public string Prefix
        {
            get;
            set;
        }
        public string FirstName
        {
            get;
            set;
        }
        public string MI
        {
            get;
            set;
        }
        public string LastName
        {
            get;
            set;
        }
        public string Suffix
        {
            get;
            set;
        }
        public string NickName
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
        //public string Email2
        //{
        //    get;
        //    set;
        //}
        public string CompanyName
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
        public string City_ID
        {
            get;
            set;
        }
        public string State_ID
        {
            get;
            set;
        }
        public string ZIP
        {
            get;
            set;
        }
         public string HomePhone
        {
            get;
            set;
        }
        public string WorkPhone
        {
            get;
            set;
        }
        public string MobilePhone
        {
            get;
            set;
        }
        //public string Pager
        //{
        //    get;
        //    set;
        //}
        public string Fax
        {
            get;
            set;
        }
        public string Website
        {
            get;
            set;
        }
        public string DateofBirth
        {
            get;
            set;
        }
        public static DataSet GetAdminInfo(string LoginID)
        {
            PatientSQLDBlayer DBLayer = new PatientSQLDBlayer();
            return DBLayer.GetAdminInfo(LoginID);
        }
        public static string updateAdminInfo(AdminProfile objProfile)
        {
            PatientSQLDBlayer DBLayer = new PatientSQLDBlayer();
            return DBLayer.updateAdminInfo(objProfile);
        }
        public static string GetObjectname(string ObjectID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.GetObjectname(ObjectID);
        }
    }
}