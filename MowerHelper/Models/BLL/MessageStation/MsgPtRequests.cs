using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.MessageStation
{
    public class MsgPtRequests
    {
        public int LoginID
        {
            get;
            set;
        }
        public int RoleID
        {
            get;
            set;
        }
        public int CategoryId
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
        public int SlNo
        {
            get;
            set;
        }
        public int Request_ID
        {
            get;
            set;
        }
        public string RequestedBy
        {
            get;
            set;
        }
        public int Patient_ID
        {
            get;
            set;
        }
        public int PatientLoginID
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
        public string ChangeRelatedTo
        {
            get;
            set;
        }
        public string RequestedDate
        {
            get;
            set;
        }
        public string EffectiveDate
        {
            get;
            set;
        }
        public string PatientNotes
        {
            get;
            set;

        }
        public string ProviderNotes
        {
            get;
            set;

        }
        public static List<MsgPtRequests> GetMsgPatientRequests(MsgPtRequests ObjMsgPtReuests)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.GetMsgPatientRequests(ObjMsgPtReuests);
        }
    }
}