using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.DALPATIENT;
using MowerHelper.Models.DAL;
namespace MowerHelper.Models.BLL.Patients
{
    public class Patient_Info
    {

      
        public string PracticeID{get;set;}
        public string ProviderID{get;set;}
        public string PatientID{get;set;}
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string DrivingLicenceNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZIP { get; set; }
        public string HomePhone { get; set; }
        public string IsHomephoneMain_Ind { get; set; }
        public string WorkPhone { get; set; }
        public string MobilePhone { get; set; }
        public string PatientEmail { get; set; }
        public string State_ID { get; set; }
        public string City_ID { get; set; }
        public string ProviderPatient_ID { get; set; }
        public string LastVisit { get; set; }
        public string NextVisit { get; set; }
        public string PatientLoginID { get; set; }
        public string RegisteredDate { get; set; }
        public string Patientname { get; set; }
        public string l_GoogleMapPath { get; set; }
        public string GmapLatitude { get; set; }
        public string GmapLongitude { get; set; }
        public string cityzip { get; set; }
        public string CourtLocationName { get; set; }
        public string DefCourtLocationName { get; set; }
        public string Default_addr { get; set; }
        public string Ind { get; set; }
 public Patient_Info()
 {
    }
        


        public static Patient_Info ViewPatineInfo(Patient_Info objpatient)
        {
            PatientSQLDBlayer layer = new PatientSQLDBlayer();
            return layer.ViewPatineInfo(objpatient);
        }
        public static string EditPatientPersonalInfo(Patient_Info objpatient, ref string Out_Msg)
        {
            PatientSQLDBlayer layer = new PatientSQLDBlayer();
            return layer.EditPatientPersonalInfo(objpatient, ref Out_Msg);
        }
        public string UpdateDefaultCourt(Patient_Info objpatient, int? DefaultCrtId, int countryid)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.UpdateDefaultCourt(objpatient, DefaultCrtId, countryid);
        }
    }
}