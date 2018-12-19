using System.Collections.Generic;
using System.Data;
using MowerHelper.Models.DAL.DALBilling;
namespace MowerHelper.Models.BLL.Billing
{
    public class PatientBalance
    {
        public string PatientName { get; set; }
        public int? ServiceID { get; set; }
        public string PatientLoginID { get; set; }
        public string TorefID { get; set; }
        public string PaidBy { get; set; }
        public string PlanID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string PracticeID { get; set; }
        public string ProviderID { get; set; }
        public string ProviderName{ get; set; }
public string PracticeName{ get; set; }
public string PaidByCity{ get; set; }
public string PaidByAddress1{ get; set; }
public string PaidByAddress2{ get; set; }
public string PaidByState{ get; set; }
public string PaidByZip{ get; set; }
public string ProviderAddress1{ get; set; }
public string ProviderAddress2{ get; set; }
public string ProviderCity{ get; set; }
public string ProviderState{ get; set; }
public string ProviderZip{ get; set; }
public string Providerphone{ get; set; }
public string TAX_ID{ get; set; }
public string ChargeType{ get; set; }
public string sessiondate{ get; set; }
public string Transaction_Date{ get; set; }
public string CPTCode{ get; set; }
public string DiagnosisCode{ get; set; }
public string SessionTotal{ get; set; }
public string Transaction_Amount { get; set; }
public string Totalbalance { get; set; }
        public static List<PatientBalance> Get_PrintBillstatement(PatientBalance obj)
        {
            BillingSqlDataAccessLayer objlayer = new BillingSqlDataAccessLayer();
            return objlayer.Get_PrintBillstatement(obj);
        }
        public static DataSet Get_PatientresposiblepartyTransactions(string PatientLogin_id, string ProviderID, string ToreferenceID, string ToreferencetypeID)
        {
            BillingSqlDataAccessLayer objlayer = new BillingSqlDataAccessLayer();
            return objlayer.Get_PatientresposiblepartyTransactions( PatientLogin_id, ProviderID,  ToreferenceID,ToreferencetypeID);
     }
    }
}