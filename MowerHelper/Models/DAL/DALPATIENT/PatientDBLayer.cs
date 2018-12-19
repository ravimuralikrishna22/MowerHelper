using System.Collections.Generic;
using System.Data;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Patients;
namespace MowerHelper.Models.DAL.DALPATIENT
{
        public abstract class PatientDataAccessLayerBaseClass
    {
                public abstract DataSet getPatientAlphabets();
                public abstract List<patients> GetMessageDeatails(int UserID, string Type, int SenderID, string OrderByitem, string orderBy, string Pagesize = null, int Pageno = 0);
                public abstract Patient_Info ViewPatineInfo(Patient_Info objpatient);
                //public abstract DataSet get_patient_parties_info( int? patientid, int PracticeID);
                public abstract string EditPatientPersonalInfo(Patient_Info objpatient, ref string Out_Msg);
                public abstract List<patients> GetServiceRenewalsMailStatus(string ToreferenceId);
                public abstract List<patients> GetlicenseRenewalsMailStatus(string ToreferenceId);
                public abstract patients Get_licenseexpireUserDetails(string Provider_ID);
                public abstract DataSet GetAdminInfo(string LoginID);
                public abstract string updateAdminInfo(AdminProfile objProfile);
                public abstract List<EmailOptions> Emails_List(EmailOptions objEmail, ref int Total_records);
                public abstract bool Emails_insert(EmailOptions objEmail);
                public abstract bool Emails_upd(EmailOptions objEmail);
                public abstract EmailOptions Emails_Get(int? EmailMessageOptionType_ID);
                public abstract bool Emails_Delete(EmailOptions objEmail);
                public abstract DataSet ListDBobjects(string FromDate, string ToDate, string Objectname, string IsScriptGenerated);
                public abstract DataSet Fillusers(string spName);
                public abstract string InsertObjectDetails(string Objectname, string Objecttype, string ObjectDesc, string userID, string createdDate);
                public abstract string udpateDbscript(string dbScript, string ScriptID);
                public abstract string upDateScriptIDs(string ScriptIDs);


    }
}