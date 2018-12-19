using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;
using System.Data;
using System.Data.SqlClient;

namespace MowerHelper.Models.BLL.Patients
{
    public class NotesInfo
    {
        public string Transaction_id
        {
            get;
            set;
        }
        public string Appointment_id
        {
            get;
            set;
        }
        public string Notes
        {
            get;
            set;
        }
        public string status_ind
        {
            get;
            set;
        }
        public int NoOfrecords
        {
            get;
            set;
        }
        public int PageNO
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
        public int? PracticeID
        {
            get;
            set;
        }
        public int? FromReference_ID
        {
            get;
            set;
        }
        public int? ToReference_ID
        {
            get;
            set;
        }
        public string IsPatientNote_Ind
        {
            get;
            set;
        }
        public int SINo
        {
            get;
            set;
        }
        public int GeneralNote_ID
        {
            get;
            set;
        }
        public DateTime? Notes_Date
        {
            get;
            set;
        }
        public string PatientName
        {
            get;
            set;
        }
        public string notetype
        {
            get;
            set;
        }
        public string CreatedBy
        {
            get;
            set;
        }
        public int TotalRecords
        {
            get;
            set;
        }
        public int? ToReferenceType_ID
        {
            get;
            set;
        }
        public int FromReferenceType_ID
        {
            get;
            set;
        }
        public int LoginHistory_ID
        {
            get;
            set;
        }
        public string contactDate
        {
            get;
            set;
        }
        public string TypeofContact
        {
            get;
            set;
        }
        public int? ProviderID
        {
            get;
            set;
        }
        public int patientID
        {
            get;
            set;
        }
        public string from_date
        {
            get;
            set;
        }
        public string to_date
        {
            get;
            set;
        }
        public string notes_keyword
        {
            get;
            set;
        }
        public static System.Collections.Generic.List<NotesInfo> GetCustomerNotesInfo(NotesInfo objNotesInfo)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.GetCustomerNotesInfo(objNotesInfo);
        }
        public static System.Collections.Generic.List<NotesInfo> GetNotesInfo(NotesInfo objNotesInfo)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.GetNotesInfo(objNotesInfo);
        }
        public static bool InsNotesInfo(NotesInfo objInsNotesInfo)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.InsNotesInfo(objInsNotesInfo);
        }
        public static NotesInfo GetupdNotesInfo(int GeneralNote_ID)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.GetupdNotesInfo(GeneralNote_ID);
        }
        public static bool UpdNotesInfo(NotesInfo objUpdNotesInfo)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.UpdNotesInfo(objUpdNotesInfo);
        }
        public static DataSet Getgeneralnotesinfo(int? generalnoteid)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.Getgeneralnotesinfo(generalnoteid);
        }
        public System.Collections.Generic.List<NotesInfo> NotesType1(string In_NoOfRecords, string pageno, string userid)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            List<NotesInfo> val = DBlayer.NotesType(In_NoOfRecords, pageno, userid);
            return val;
        }
    
    }
}