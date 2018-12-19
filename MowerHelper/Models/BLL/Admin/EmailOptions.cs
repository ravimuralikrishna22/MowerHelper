using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.DALPATIENT;

namespace MowerHelper.Models.BLL.Admin
{
    public class EmailOptions
    {
        public string EmailMessageOptionType_ID
        {
            get;
            set;
        }
        public string Title
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
        public string UpdatedBy
        {
            get;
            set;
        }
        public static List<EmailOptions> Emails_List(EmailOptions objEmail, ref int Total_records)
        {
            PatientSQLDBlayer DBLayer = new PatientSQLDBlayer();
            return DBLayer.Emails_List(objEmail,ref Total_records);
        }
        public static bool Emails_insert(EmailOptions objEmail)
        {
            PatientSQLDBlayer DBLayer = new PatientSQLDBlayer();
            return DBLayer.Emails_insert(objEmail);
        }
        public static bool Emails_upd(EmailOptions objEmail)
        {
            PatientSQLDBlayer DBLayer = new PatientSQLDBlayer();
            return DBLayer.Emails_upd(objEmail);
        }
        public static EmailOptions Emails_Get(int? EmailMessageOptionType_ID)
        {
            PatientSQLDBlayer DBLayer = new PatientSQLDBlayer();
            return DBLayer.Emails_Get(EmailMessageOptionType_ID);
        }
        public static bool Emails_Delete(EmailOptions objEmail)
        {
            PatientSQLDBlayer DBLayer = new PatientSQLDBlayer();
            return DBLayer.Emails_Delete(objEmail);
        }

    }
}