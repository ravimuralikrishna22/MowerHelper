using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.Providers
{
    public class Provider_Common
    {
        public int ProviderID
        {
            get;
            set;
        }
        public string FromDate
        {
            get;
            set;
        }
        public string ToDate
        {
            get;
            set;

        }
        public string NoOfRecords
        {
            get;
            set;
        }
        public string PageNO
        {
            get;
            set;
        }
        public string OrderBYItem
        {
            get;
            set;
        }
        public string OrderBy
        {
            get;
            set;
        }
        public int Loginid
        {
            get;
            set;

        }
        public string CreatedOn
        {
            get;
            set;
        }



        public string Sender_Name
        {
            get;
            set;
        }
        public string Sender_Email
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
        public string Message
        {
            get;
            set;
        }
        public string Status_Ind
        {
            get;
            set;
        }
        public int Provider_PublicMessages_ID
        {
            get;
            set;
        }

        public string contactmessage
        {
            get;
            set;
        }
        public static DataSet Providerpublicmessages(Provider_Common obj, string Status)
        {
            SQLDataAccessLayer DALLayer = new SQLDataAccessLayer();

            return DALLayer.Providerpublicmessages(obj, Status);
        }
        public static DataSet GetProviderpublicmessage(int Msg_id)
        {
            SQLDataAccessLayer DALLayer = new SQLDataAccessLayer();

            return DALLayer.GetProviderpublicmessage(Msg_id);
        }
        
    }
}