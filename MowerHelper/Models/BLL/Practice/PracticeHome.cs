using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MowerHelper.Models.DAL.DataAccessLayer;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.Practice
{
    public class PracticeblHome
    {


        public string Patient_Status_ID
        {
            get;
            set;
        }
        public string Login_ID
        {
            get;
            set;
        }
        public string Address
        {
            get;
            set;
        }
        public string Practice_ID
        {
            get;
            set;
        }
        public string Provider_ID
        {
            get;
            set;
        }
        public string LastName
        {
            get;
            set;
        }
        public string NoofRecords
        {
            get;
            set;
        }
        public string PageNo
        {
            get;
            set;
        }
        public string fullname
        {
            get;
            set;
        }
        public string Orderbyitem
        {
            get;
            set;
        }
        public string OrderBy
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
        public string CellPhone
        {
            get;
            set;
        }
        public string Status_Ind
        {
            get;
            set;
        }

        public static int Totalnoofrecords
        {
            get;
            set;
        
    }
        public string insname1
        {

            get;
            set;
        }
        public string insname2
        {

            get;
            set;
        }
        public string insid1
        {

            get;
            set;
        }
        public string insid2
        {

            get;
            set;
        }
        public string username
        {
            get;
            set;
        }
        public string password
        {
            get;
            set;
        }
        public string Userfullname
        {
            get;
            set;
        }
        public string email
        {
            get;
            set;
        }
        public string Class_name { get; set; }
        public string Method_name { get; set; }
        public DateTime Startruntime { get; set; }
        public DateTime Endruntime { get; set; }
        public int? Time_Difference { get; set; }
        public string ProviderName { get; set; }
        public List<PracticeblHome> PatientList { get; set; }
        public PracticeblHome()
        {
        }
        public PracticeblHome(string _username, string _password, string _Userfullname, string _email)
{
    this.username = _username;
    this.password = _password;
    this.Userfullname = _Userfullname;
    this.email = _email;
}

              public PracticeblHome(string StrPatientName, int intpatientid,   string StrNextAppt, string StrApptCount,    double StrTotal,   string StrPatientLogin_ID,
              string StrPractice_ID, string StrAuthorizedProvider_ID, string StrActualProvider_ID, string DefaultCourtLocation_ID)
{
	this.StrPatientName = StrPatientName;
    this.intpatientid = intpatientid;
    this.StrNextAppt = StrNextAppt;
    this.StrApptCount = StrApptCount;
    this.StrTotal = StrTotal;
    this.StrPatientLogin_ID = StrPatientLogin_ID;
    this.StrPractice_ID = StrPractice_ID;
    this.StrAuthorizedProvider_ID = StrAuthorizedProvider_ID;
    this.StrActualProvider_ID = StrActualProvider_ID;
    this.DefaultCourtLocation_ID = DefaultCourtLocation_ID;

}
              public string StrPatientName
        {

            get;
            set;
        }
              public int intpatientid
        {
            get;
            set;
        }
              public string StrDOB
        {

            get;
            set;
        }
              public string StrNextAppt
        {

            get;
            set;
        }
              public string StrApptCount
        {

            get;
            set;
        }
              public string StrRespPartiesList
        {

            get;
            set;
        }
              public double StrTotal
        {

            get;
            set;
        }
              public string StrPatientLogin_ID
        {

            get;
            set;
        }
              public string StrPractice_ID
              {

                  get;
                  set;
              }
              public string StrAuthorizedProvider_ID
              {
                  get;
                  set;
              }
              public string StrActualProvider_ID
              {

                  get;
                  set;
              }
              public string Transactionexist
              {

                  get;
                  set;
              }
              public string DefaultCourtLocation_ID
              {
                  get;
                  set;
              }
        public static List<PracticeblHome> Get_PracticeHomePagePatientsList(PracticeblHome objgethome)
        {
            SQLDataAccessLayer obj = new SQLDataAccessLayer();
            return obj.Get_PracticeHomePagePatientsList(objgethome);
        }
        public static PracticeblHome Get_SendMailsToProviderDetails(int? providerLogin_id, string Provider_ID = null)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Get_SendMailsToProviderDetails(providerLogin_id, Provider_ID);
        }
        public static PracticeblHome UpdateDetails(int providerid, string EmailID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.UpdateDetails(providerid, EmailID);
        }
        public string getpaymentType(int providerlogin)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.getpaymentType(providerlogin);
        }
        public void updatePaytype(int plId, int DefaultPaytype)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            dblayer.updatePaytype(plId,DefaultPaytype);
        }
        public string getDebugType(int providerlogin)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.getDebugMode(providerlogin);
        }
        public void updateDebugtype(int plId, string DefaultDebugtype)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            dblayer.updateDebugMode(plId, DefaultDebugtype);
        }
        public static List<PracticeblHome> DebugItemsList(PracticeblHome objdbug)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.DebugItemsList(objdbug);
        }
    }
}