using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.DALPATIENT;
using MowerHelper.Models.Classes;
using System.Collections;
using System.Data;
using MowerHelper.Models.DAL;
namespace MowerHelper.Models.BLL.Patients
{
    public class patients
    {
        public int Message_ID
        {
            get;
            set;
        }
        public string SendingDate
        {
            get;
            set;
        }
        public string Sender
        {
            get;
            set;
        }
        public string Patientname
        {
            get;
            set;
        }
        public string topic
        {
            get;
            set;
        }
        public string Receiver
        {
            get;
            set;
        }
        public string AttachmentFilename
        {
            get;
            set;
            }
        public string MessageBody
        {
            get;
            set;
        }
        public int Reciever_ID
        {
            get;
            set;
        }
        public string ReadInd
        {
            get;
            set;
        }
        public int Reply_ID
        {
            get;
            set;
        }
        //public int SlNo
        //{
        //    get;
        //    set;
        //}
        public static int MsgCount
        {
            get;
            set;
        }
        public int EmailMessage_Transaction_ID
        {
            get;
            set;
        }
        public string Mail_To
        {
            get;
            set;
        }
        public int Provider_ID
        {
            get;
            set;
        }
        public string Mail_From
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
        public string Message_Body
        {
            get;
            set;
        }
        public string InsuredDOB { get; set; }
        public string ReferenceTypeID { get; set; }
        public string Name { get; set; }
        public string licenseexpiredate
        {
            get;
            set;
        }


        public int Practice_ID { get; set; }
        public static int TotalRecords { get; set; }
        public string OrderBy { get; set; }
        public int NoOfRecords { get; set; }
        public string orderByItem { get; set; }
        public int page { get; set; }
        public string Appointment_Date { get; set; }
        public int Patient_ID { get; set; }
        public int? Appointment_ID { get; set; }
        public string Appointment_Time { get; set; }
        public string GeneralNarrative { get; set; }
        public int AppointmentStatus_ID { get; set; }
        public int Claim_ind { get; set; }
        public string StatusMsg { get; set; }

        public string OrderByItem
        {
            get;
            set;
        }
        public string order
        {
            get;
            set;
        }
        public int NoofRecords
        {
            get;
            set;
        }
        public string ApptStatus
        {
            get;
            set;
        }
        public int SLno
        {
            get;
            set;
        }
        //public int Appointment_id
        //{
        //    get;
        //    set;
        //}
        public int Length
        {
            get;
            set;
        }
        public string Notes
        {
            get;
            set;
        }
        public string Order
        {
            get;
            set;
        }
        //public static int Totalrecords
        //{
        //    get;
        //    set;
        //}
        public List<patients> PatientsList { get; set; }
        public static List<patients> GetMessageDeatails(int UserID, string Type, int SenderID, string OrderByitem, string orderBy, string Pagesize = null, int Pageno = 0)
        {
            PatientSQLDBlayer layer = new PatientSQLDBlayer();
            return layer.GetMessageDeatails(UserID, Type, SenderID, OrderByitem, orderBy, Pagesize, Pageno);
        }
        //public static List<patients> get_patient_partiesinfo(int patientid, int PracticeID)
        //{
        //    PatientSQLDBlayer layer = new PatientSQLDBlayer();
        //    string[] LoginIDs = null;
        //    int cnt = 0;
        //    int icount = 0;
        //    DataView dvpatientinfo = new DataView();
        //    clsCommonFunctions objCommon = new clsCommonFunctions();
        //    List<patients> collPatient = new List<patients>();
        //    DataSet dspatientinfo = default(DataSet);
        //    dspatientinfo = layer.get_patient_parties_info( patientid, PracticeID);
        //    if ((dspatientinfo != null))
        //    {
        //        LoginIDs = objCommon.returndistinctDS(dspatientinfo, "Login_id");
        //        //int ubound=Array.Get
        //        for (cnt = 0; cnt <= LoginIDs.GetUpperBound(0); cnt++)
        //        {
        //            dvpatientinfo = new DataView();
        //            dvpatientinfo.Table = dspatientinfo.Tables[0];
        //            dvpatientinfo.RowFilter = "Login_id=" + LoginIDs[cnt];
        //            patients objpat = new patients();
        //            for (icount = 0; icount <= dvpatientinfo.Count - 1; icount++)
        //            {
        //                //if (dvpatientinfo[icount]["Patient_ID"]!=null)
        //                //{
        //                //    objpat.Patient_ID = dvpatientinfo[icount]["Patient_ID"].ToString();
        //                //}
        //                //if (dvpatientinfo[icount]["PatientLogin_ID"]!=null)
        //                //{
        //                //    objpat.PatientLogin_ID = dvpatientinfo[icount]["PatientLogin_ID"].ToString();
        //                //}
        //                //if (dvpatientinfo[icount]["Login_id"]!=null)
        //                //{
        //                //    objpat.Login_ID = dvpatientinfo[icount]["Login_id"].ToString();
        //                //}
        //                //if (dvpatientinfo[icount]["ReferenceType_ID"]!=null)
        //                //{
        //                //    objpat.ReferenceTypeID = dvpatientinfo[icount]["ReferenceType_ID"].ToString();
        //                //}
        //                if (dvpatientinfo[icount]["ReferenceType_ID"]!=null)
        //                {
        //                    if (dvpatientinfo[icount]["ReferenceType_ID"].ToString() == "3")
        //                    {
        //                        if ((objpat.Name == null))
        //                        {
        //                            objpat.Name = "Patient";
        //                        }
        //                        else
        //                        {
        //                            objpat.Name = "Patient" + "," + " " + objpat.Name;
        //                        }
        //                    }
        //                    else if (dvpatientinfo[icount]["ReferenceType_ID"].ToString() == "8")
        //                    {
        //                        if ((objpat.Name == null))
        //                        {
        //                            objpat.Name = "Responsible Party";
        //                        }
        //                        else
        //                        {
        //                            if (objpat.Name.Contains("Responsible Party") == false)
        //                            {
        //                                objpat.Name = objpat.Name + "," + " " + "Responsible Party";
        //                            }
        //                        }
        //                    }
        //                    else if (dvpatientinfo[icount]["ReferenceType_ID"].ToString() == "18")
        //                    {

        //                        if ((objpat.Name == null))
        //                        {
        //                            objpat.Name = "Insurance Party";
        //                        }
        //                        else
        //                        {
        //                            if (objpat.Name.Contains("Insurance Party") == false)
        //                            {
        //                                objpat.Name = objpat.Name + "," + " " + "Insurance Party";
        //                            }
        //                        }
        //                    }
                            
        //                }
        //                if (dvpatientinfo[icount]["Name"]!=null)
        //                {
        //                    objpat.Patientname = dvpatientinfo[icount]["Name"].ToString();
        //                }
        //                if (dvpatientinfo[icount]["DOB"]!=null)
        //                {
        //                    objpat.InsuredDOB = dvpatientinfo[icount]["DOB"].ToString();
        //                }
                        
        //            }
        //            collPatient.Add(objpat);
        //        }
        //    }
        //    return collPatient;
          
        //}
        public static List<patients> GetServiceRenewalsMailStatus(string ToreferenceId)
        {
            PatientSQLDBlayer layer = new PatientSQLDBlayer();
            return layer.GetServiceRenewalsMailStatus(ToreferenceId);
        }
        public static List<patients> GetlicenseRenewalsMailStatus(string ToreferenceId)
        {
            PatientSQLDBlayer layer = new PatientSQLDBlayer();
            return layer.GetlicenseRenewalsMailStatus(ToreferenceId);
        }
        public static patients Get_licenseexpireUserDetails(string Provider_ID)
        {
            PatientSQLDBlayer layer = new PatientSQLDBlayer();
            return layer.Get_licenseexpireUserDetails(Provider_ID);
        }

        public List<patients> ListOfProgressNotes(patients ObjcdLstProgress)
        {
            SQLDataAccessLayer objCdSql = new SQLDataAccessLayer();
            return objCdSql.ListOfProgressNotes(ObjcdLstProgress);
        }
        public static List<patients> Get_AppointmentHistory(patients objapphistory)
        {
            SQLDataAccessLayer obj = new SQLDataAccessLayer();
            return obj.Get_AppointmentHistory(objapphistory);
        }
    }
}