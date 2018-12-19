using System.Collections.Generic;
using System.Data;
using MowerHelper.Models.DAL.DALBilling;
namespace MowerHelper.Models.BLL.Billing
{
    public class Billingreports
    {
        public string PatientLoginID { get; set; }
        public string PracticeID { get; set; }
        public string ProviderID { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string Status { get; set; }
        public string MessageOnBill_ID { get; set; }
        public string MessageOnBill { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string PatientID { get; set; }
        public string Ind { get; set; }
      //  public string Claim_ID { get; set; }
        public string ID { get; set; }
        public string Status_Ind { get; set; }
        public static DataSet Get_Patientfiledetails(Billingreports obj)
        {
            BillingSqlDataAccessLayer objlayer = new BillingSqlDataAccessLayer();
            return   objlayer.Get_Patientfiledetails(obj);
        }
        public static string GetPatientLoginID(string PatientID)
        {
            BillingSqlDataAccessLayer objlayer = new BillingSqlDataAccessLayer();
            return objlayer.GetPatientLoginID(PatientID);
        }
        public static List<Billingreports> Get_PrintBillMessages()
        {
            BillingSqlDataAccessLayer objlayer = new BillingSqlDataAccessLayer();
            return objlayer.Get_PrintBillMessages();
        }
        //public static List<Billingreports> Get_BillablePartyList(Billingreports obj)
        //{
        //    BillingSqlDataAccessLayer objlayer = new BillingSqlDataAccessLayer();
        //    return objlayer.Get_BillablePartyList(obj);
        //}
    }
}