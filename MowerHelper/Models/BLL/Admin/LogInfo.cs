using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.Admin
{
    public class LogInfo 
    {

        #region "Properties"
        public int? Eventtype_id
        {
            get;
            set;
        }
        public string LoggedOn
        {
            get;
            set;
        }
        public string Patientname
        {
            get;
            set;
        }
        public string Patientscreenname
        {
            get;
            set;
        }
        public string Modifiedby
        {
            get;
            set;
        }
        public string Modifiedreference
        {
            get;
            set;
        }
        public int EventInfoLog_ID
        {
            get;
            set;
        }
        public string Event_Title
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
        public int? PageNo
        {
            get;
            set;
        }
        public int? NoofRecords
        {
            get;
            set;
        }
        public int? ReferencetypeID
        {
            get;
            set;
        }
        public int? ProviderLoginID
        {
            get;
            set;
        }
        public int? EventCategory_ID
        {
            get;
            set;
        }
        public int? PatientLoginID
        {
            get;
            set;
        }
        public int TotalRecords
        {
            get;
            set;
        }
        public string Field
        {
            get;
            set;
        }
        public string PreviousValue
        {
            get;
            set;
        }
        public string Presentvalue
        {
            get;
            set;
        }
        public List<LogInfo> LogInfoList { get; set; }
        #endregion
        public static List<LogInfo> Get_PatientLoginfo(string EventLogID, string EventTypeID)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.Get_PatientLoginfo(EventLogID, EventTypeID);
        }
        public static List<LogInfo> Get_EvenLogInfo(LogInfo objLogInfo)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.Get_EvenLogInfo(objLogInfo);
        }
    }
}
