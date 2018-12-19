using System.Collections.Generic;
using System.Data;
using System.Text;
using MowerHelper.Models.DAL.DALBilling;
namespace MowerHelper.Models.BLL.Billing
{
    public class RPBilling
    {
        public int? PatientLogin_ID { get; set; }
        public int? Practice_ID { get; set; }
        public int? Provider_id { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int? Reference_id { get; set; }
        public int? fromReferenceId { get; set; }
        public string NoOfRecords { get; set; }
        public string PageNo { get; set; }
        public string orderBy { get; set; }
        public string OrderbyItem { get; set; }
        public int? AuthorizedPatientLoginID { get; set; }
        public string ReferenceType_ID { get; set; }
        public string Notes { get; set; }
        public string RoleName { get; set; }
        public string sessiondate { get; set; }
        public string PatientID { get; set; }
        public string Transaction_Date { get; set; }
        public int Transaction_ID { get; set; }
        public string Transaction_Amount { get; set; }
        public string ChargeType {get; set;}
        public string BalanceAmount { get; set; }
      //  public string Claim_ID { get; set; }
        public string Apply { get; set; }
        public string FromAccount_ID { get; set; }
        public string ToReference_ID { get; set; }
        public string FromReferenceType_ID { get; set; }
        public string ToReferenceType_ID { get; set; }
        public string ToAccount_ID { get; set; }
        public string Facility_ID { get; set; }
        public string BillingService_ID { get; set; }
        public string BillingChargetype_ID { get; set; }
       // public string ServiceTaxAmount { get; set; }
       // public string ServiceTaxPercentage { get; set; }
       // public string DistributedoRAppliedAmount { get; set; }
        public string InvoiceGenerated_Ind { get; set; }
        public string TransactionType_ID { get; set; }
        public string Appointment_ID { get; set; }
        public string BillableParty { get; set; }
        public string BilledBy { get; set; }
        public static int Totalnoofrecords { get; set; }
        public string GrandTotal { get; set; }
        public string ServiceTaxTotal { get; set; }
        public string Total { get; set; }
        public string ProviderName { get; set; }
        public int? PaymentType_ID { get; set; }
        public int PcType_ID { get; set; }
        public string TransactionType{get;set;}
       // public string Distribute { get; set; }
      //  public string AppliedAmount { get; set; }
        public string PaidBytype_ID { get; set; }
        public string PaidBytype { get; set; }
        public string PaidBy_ID { get; set; }
        public string PaymentMode { get; set; }
        public string PaidByName { get; set; }
        public int PaymentMode_ID { get; set; }
        public int AdjustmentType_ID { get; set; }
        public string Adj_Title {get;set;}
        public string UserId { get; set; }
        public string patBillName { get; set; }
        public string patSInd { get; set; }
        public string RegOndate { get; set; }
        public string Ind { get; set; }
        public string SessionTotal { get; set; }
       // public string ApplicableDistributedAmount { get; set; }
       // public string Distribution_ID{get;set;}
        public string chargeTransaction_ID { get; set; }
        public string PaymentTransaction_ID{get;set;}
        public string ChecksNo { get; set; }
        public string CheckDate { get; set; }
        public string BankName { get; set; }
        public int? planID { get; set; }
        public string Authorizednumber { get; set; }
        public int? CCTransactionID { get; set; }
        public string IsCCchargetoPrv { get; set; }
        public string TransactionTypeIDs { get; set; }
        public string ServiceID { get; set; }
        public string servicename { get; set; }
       public string Invoice_Date { get; set; }
public int? Invoice_ID {get; set;}
public string InvoiceNo {get ;set;}
public string InvoiceCharges {get; set;}
public string InvoicePaments {get;set;}
public string InvoiceBalance {get;set;}
public string InvoicePosAdjstmnts {get;set;}
public string InvoiceNegAdjstmnts{get;set;}
public string PrevBalance{get;set;}
public string PrevAdvance { get; set; }
public string RecieptNo { get; set; }
public string TotalCharges { get; set; }
public string TotalPayments { get; set; }
public string TotalCredits { get; set; }
public string Balence { get; set; }
public string TotalSumCharges { get; set; }
public string TotalSumPayments { get; set; }
public string TotalSumCredits { get; set; }
public string SumBalence { get; set; }
public int ChequeOrCCTran_ID { get; set; }
public int RefTransID { get; set; }
public string IsRefundTransaction { get; set; }
public string IsReference { get; set; }
public string ReturnAmount { get; set; }
public string strmanualrefid { get; set; }
public static StringBuilder stradminSummary { get; set; }
public string Messagetouser { get; set; }
public string TermsAndConditions { get; set; }
public string paytype { get; set; }
public string CreditInd { get; set; }
public string Transaction_IDs { get; set; }
public string apptdate { get; set; }
public string Customer_Email { get; set; }
public int? RefTransaction_ID { get; set; }
public string schedule_type { get; set; }
public string page_ind { get; set; }
public string Date_range { get; set; }
public string emailcheck { get; set; }
public static string PageChargeTotal { get; set; }
public static string PagePaymentTotal { get; set; }
public string formanme { get; set; }
public int? Formreference_id { get; set; }
public int? type_id { get; set; }
public static string GrandChargeTotal { get; set; }
public static string GrandPaymentTotal { get; set; }
public string custname { get; set; }
public string clientphone { get; set; }
public bool chkemail { get; set; }
public bool Echkemail { get; set; }
public bool ECchkemail { get; set; }
public bool EPchkemail { get; set; }
public List<RPBilling> BillingTransList { get; set; }
public int ProviderLoginID { get; set; }
        public static DataSet PracticePatientIncome(RPBilling billInfo)
        {
            BillingSqlDataAccessLayer objbillInfo = new BillingSqlDataAccessLayer();
            return objbillInfo.PracticePatientIncome(billInfo);

        }
        public static DataSet Get_PracticeresposiblepartyTransactions(RPBilling patRespbill)
        {
            BillingSqlDataAccessLayer objRespBillinfo = new BillingSqlDataAccessLayer();
            return objRespBillinfo.Get_PracticeresposiblepartyTransactions(patRespbill);
        }
        //public List<RPBilling> GetPatientChargeslist(RPBilling patCharge)
        //{
        //    BillingSqlDataAccessLayer objCharge = new BillingSqlDataAccessLayer();
        //    return objCharge.GetPatientChargeslist(patCharge);
        //}
        public List<RPBilling> GetPatienttransactionlist(RPBilling patCharge)
        {
            BillingSqlDataAccessLayer objCharge = new BillingSqlDataAccessLayer();
            return objCharge.GetPatienttransactionlist(patCharge);
        }
        public List<RPBilling> GetPatienttransactionlist1(RPBilling patCharge)
        {
            BillingSqlDataAccessLayer objCharge = new BillingSqlDataAccessLayer();
            return objCharge.GetPatienttransactionlist1(patCharge);
        }
        //public List<RPBilling> GetPatientPaymentList(RPBilling patPay)
        //{
        //    BillingSqlDataAccessLayer objpay = new BillingSqlDataAccessLayer();
        //    return objpay.GetPatientPaymentList(patPay);
        //}
        //public List<RPBilling> GetPatientAdjustmenttList(RPBilling patAdj)
        //{
        //    BillingSqlDataAccessLayer objAdj = new BillingSqlDataAccessLayer();
        //    return objAdj.GetPatientAdjustmenttList(patAdj);
        //}
        public List<RPBilling> GetPaymentModes()
        {
            BillingSqlDataAccessLayer objPayModes = new BillingSqlDataAccessLayer();
            return objPayModes.GetPaymentModes();
        }
        //public List<RPBilling> getAdjTypes()
        //{
        //    BillingSqlDataAccessLayer objAdjTypes = new BillingSqlDataAccessLayer();
        //    return objAdjTypes.getAdjTypes();
        //}
        public RPBilling Del_ProviderPatientTransaction(RPBilling objtran,int Loginhistoryid)
        {
            BillingSqlDataAccessLayer objDel = new BillingSqlDataAccessLayer();
            return objDel.Del_ProviderPatientTransaction(objtran, Loginhistoryid);
        }
        //public DataSet GETPatientPayment_Details(string patientid, string practiceId)
        //{
        //    BillingSqlDataAccessLayer objPatPaydet = new BillingSqlDataAccessLayer();
        //    return objPatPaydet.GETPatientPayment_Details(patientid, practiceId);
        //}
        public List<RPBilling> getUnpaidBalance(RPBilling patinfo)
        {
            BillingSqlDataAccessLayer objBsda = new BillingSqlDataAccessLayer();
            return objBsda.getUnpaidBalance(patinfo);
        }
        //public  int MakeTransactionlist(RPBilling pattran,int Loginhistoryid)
        //{
        //    BillingSqlDataAccessLayer objChgIns = new BillingSqlDataAccessLayer();
        //    return objChgIns.MakeTransactionlist(pattran, Loginhistoryid);
        //}
        //public int MakePaymentTransactionlist(RPBilling pattran, int Loginhistoryid)
        //{
        //    BillingSqlDataAccessLayer objbillingSql = new BillingSqlDataAccessLayer();
        //    return objbillingSql.MakePaymentTransactionlist(pattran,Loginhistoryid);
        //}
        //public List<RPBilling> viewpaymentdistrib(RPBilling objpayd)
        //{
        //    BillingSqlDataAccessLayer paydist = new BillingSqlDataAccessLayer();
        //    return paydist.viewpaymentdistrib(objpayd);
        //}
        //public DataSet viewpaymentAutodistrib(RPBilling objpayd)
        //{
        //    BillingSqlDataAccessLayer dynamicditr = new BillingSqlDataAccessLayer();
        //    return dynamicditr.viewpaymentAutodistrib(objpayd);
        //}
        //public int Ins_ChequePayment(RPBilling patchkTran, int Loginhistoryid) {
        //    BillingSqlDataAccessLayer objchktran = new BillingSqlDataAccessLayer();
        //   return objchktran.Ins_ChequePayment(patchkTran,Loginhistoryid);
        //}
        //public string Get_UnAppliedAmount(RPBilling patunApplied)
        //{
        //    BillingSqlDataAccessLayer objUnapplied = new BillingSqlDataAccessLayer();
        //    return objUnapplied.Get_UnAppliedAmount(patunApplied);
        //}
        //public int MakePayCcTransaction(RPBilling pattran, int Loginhistoryid)
        //{
        //    BillingSqlDataAccessLayer objbillingSql = new BillingSqlDataAccessLayer();
        //    return objbillingSql.MakePayCcTransaction(pattran, Loginhistoryid);
        //}
        //public DataSet ShowChargeAppliedAmount(RPBilling Showchgdetail)
        //{
        //    BillingSqlDataAccessLayer objchgdetails = new BillingSqlDataAccessLayer();
        //   return objchgdetails.ShowChargeAppliedAmount(Showchgdetail);
        //}
        //public DataSet showPayAmount(RPBilling showPaydeatails)
        //{
        //    BillingSqlDataAccessLayer objchgdetails = new BillingSqlDataAccessLayer();
        //    return objchgdetails.showPayAmount(showPaydeatails);
        //}
        public DataSet showtransactiondetails(RPBilling showPaydeatails)
        {
            BillingSqlDataAccessLayer objchgdetails = new BillingSqlDataAccessLayer();
            return objchgdetails.showtransactiondetails(showPaydeatails);
        }
        //public DataSet showAdjustDetails(RPBilling showAdjDetails)
        //{
        //    BillingSqlDataAccessLayer objadjdetails = new BillingSqlDataAccessLayer();
        //    return objadjdetails.showAdjustDetails(showAdjDetails);
        //}
        public void Edit_Payment_charges(RPBilling editPAy, int Loginhistoryid,ref string Out_Msg)
        {
            BillingSqlDataAccessLayer objpayEdit = new BillingSqlDataAccessLayer();
            objpayEdit.Edit_Payment_charges(editPAy,Loginhistoryid, ref Out_Msg);
        }
        //public RPBilling Ins_Disttransaction(RPBilling paydis, int Loginhistoryid)
        //{
        //    BillingSqlDataAccessLayer objpaydis = new BillingSqlDataAccessLayer();
        //   return objpaydis.Ins_Disttransaction(paydis, Loginhistoryid);
        //}
        //public int MakeAdjustmentTransactionlist(RPBilling pattran, int Loginhistoryid)
        //{
        //    BillingSqlDataAccessLayer objinsAdj = new BillingSqlDataAccessLayer();
        //    return objinsAdj.MakeAdjustmentTransactionlist(pattran, Loginhistoryid);
        //}
        public DataSet patientwiseDaysoutstanding(RPBilling daysinfo, int typeid)
        {
            BillingSqlDataAccessLayer objdays = new BillingSqlDataAccessLayer();
            return objdays.patientwiseDaysoutstanding(daysinfo,typeid);
        }
        //public clsClaimSessionInfo LoadPaymentDistributionForClaim(int Claim_ID, int Patient_ID, int CPTCode, int Provider_ID)
        //{
        //    BillingSqlDataAccessLayer objbsda = new BillingSqlDataAccessLayer();
        //    return objbsda.LoadPaymentDistributionForClaim(Claim_ID, Patient_ID, CPTCode, Provider_ID);
        //}
        //public DataSet RPDetailsNewForClaim(int PatientID, int Claim_ID, int PracticeID)
        //{
        //    BillingSqlDataAccessLayer objRPclaim = new BillingSqlDataAccessLayer();
        //    return objRPclaim.RPDetailsNewForClaim(PatientID, Claim_ID, PracticeID);
        //}
        public DataSet PracticeAdminSummary(RPBilling prSummary)
        {
            BillingSqlDataAccessLayer objsum = new BillingSqlDataAccessLayer();
            return objsum.PracticeAdminSummary(prSummary);
        }
        public List<RPBilling> Get_PracticeUserTrasnactions(RPBilling prTrans)
        {
            BillingSqlDataAccessLayer objChgtran = new BillingSqlDataAccessLayer();
            return objChgtran.Get_PracticeUserTrasnactions(prTrans);
        }
        public List<RPBilling> GET_Invoice(RPBilling prInvoice)
        {
            BillingSqlDataAccessLayer objInvoiceInfo = new BillingSqlDataAccessLayer();
            return objInvoiceInfo.GET_Invoice(prInvoice);
        }
        public List<RPBilling> Get_AdmintopracticeInvoiceReciept(RPBilling prReciept)
        {
            BillingSqlDataAccessLayer objInvoiceInfo = new BillingSqlDataAccessLayer();
            return objInvoiceInfo.Get_AdmintopracticeInvoiceReciept(prReciept);
        }
        public DataSet getPatientFileDayout(int patLiogn, int providerId,string startdate,string endDate)
        {
            BillingSqlDataAccessLayer objDayout = new BillingSqlDataAccessLayer();
            return objDayout.getPatientFileDayout(patLiogn, providerId,startdate,endDate);
        }
        public DataSet getChargePartition(int Transaction_ID)
        {
            BillingSqlDataAccessLayer objChargeInfo = new BillingSqlDataAccessLayer();
            return objChargeInfo.getChargePartition(Transaction_ID);
        }
        public DataSet GetOpenPayments(RPBilling objPay)
        {
            BillingSqlDataAccessLayer objPayInfo = new BillingSqlDataAccessLayer();
            return objPayInfo.GetOpenPayments(objPay);
        }
        public DataSet GetAdminSummary(RPBilling AdminSummary)
        {
            BillingSqlDataAccessLayer objAdminInfo = new BillingSqlDataAccessLayer();
            return objAdminInfo.GetAdminSummary(AdminSummary);
        }
        public DataSet GetSearchAdminSummary(RPBilling AdminSummary)
        {
            BillingSqlDataAccessLayer objsearch = new BillingSqlDataAccessLayer();
            return objsearch.GetSearchAdminSummary(AdminSummary);
        }
        public List<RPBilling> GetAdminListSummary(RPBilling AdminSummary)
        {
            BillingSqlDataAccessLayer objsearch = new BillingSqlDataAccessLayer();
            return objsearch.GetAdminListSummary(AdminSummary);
        }
        public List<RPBilling> GetAdminSearchListSummary(RPBilling AdminSummary)
        {
            BillingSqlDataAccessLayer objsearch = new BillingSqlDataAccessLayer();
            return objsearch.GetAdminSearchListSummary(AdminSummary);
        }
        public int MakeAdminChgTransactionlist(RPBilling provtran, int Loginhistoryid)
        {
            BillingSqlDataAccessLayer objAdminChg = new BillingSqlDataAccessLayer();
            return objAdminChg.MakeAdminChgTransactionlist(provtran, Loginhistoryid);
        }
        public int MakeAdminPayTransaction(RPBilling provtran, int Loginhistoryid)
        {
            BillingSqlDataAccessLayer objAdminPay = new BillingSqlDataAccessLayer();
            return objAdminPay.MakeAdminPayTransaction(provtran, Loginhistoryid);
        }
        public List<RPBilling> Get_PracticeTrasnactions(RPBilling AdminTransactions)
        {
            BillingSqlDataAccessLayer objAdminPay = new BillingSqlDataAccessLayer();
            return objAdminPay.Get_PracticeTrasnactions(AdminTransactions);
        }
        public DataSet getprovider(int? practiceId)
        {
            BillingSqlDataAccessLayer objAdminPay = new BillingSqlDataAccessLayer();
            return objAdminPay.getprovider(practiceId);
        }
        public DataSet getViewPrTransaction(int tranId)
        {
            BillingSqlDataAccessLayer objAdminPay = new BillingSqlDataAccessLayer();
            return objAdminPay.getViewPrTransaction(tranId);
        }
        public string Get_InvoiceBeginDate(RPBilling adminbilling)
        {
            BillingSqlDataAccessLayer objAdminPay = new BillingSqlDataAccessLayer();
            return objAdminPay.Get_InvoiceBeginDate(adminbilling);
        }
        public List<RPBilling> List_AdmintoPracticeChargesToGenerateInvoice(RPBilling adminBilling)
        {
            BillingSqlDataAccessLayer objinvoice = new BillingSqlDataAccessLayer();
            return objinvoice.List_AdmintoPracticeChargesToGenerateInvoice(adminBilling);
        }
        public int INS_AdmintoPracticeInvoice(RPBilling obj)
        {
            BillingSqlDataAccessLayer objinvoice = new BillingSqlDataAccessLayer();
            return objinvoice.INS_AdmintoPracticeInvoice(obj);
        }
        public DataSet practiceInvoicelist(RPBilling pracInvoice)
        {
            BillingSqlDataAccessLayer objpracinvoice = new BillingSqlDataAccessLayer();
            return objpracinvoice.practiceInvoicelist(pracInvoice);
        }
        public List<RPBilling> Get_AdminAddRecipts(RPBilling adminRecipt)
        {
            BillingSqlDataAccessLayer objrecipt = new BillingSqlDataAccessLayer();
            return objrecipt.Get_AdminAddRecipts(adminRecipt);
        }
        public int INS_AdmintoPracticeReceipt(RPBilling objinjsrecipt)
        {
            BillingSqlDataAccessLayer objinsRecipt = new BillingSqlDataAccessLayer();
            return objinsRecipt.INS_AdmintoPracticeReceipt(objinjsrecipt);
        }
        public int MakeAdminAdjTransaction(RPBilling provtran, int Loginhistoryid)
        {
            BillingSqlDataAccessLayer objAdj = new BillingSqlDataAccessLayer();
            return objAdj.MakeAdminAdjTransaction(provtran, Loginhistoryid);
        }
        public RPBilling Del_AdminTransaction(RPBilling objtran, int Loginhistoryid)
        {
            BillingSqlDataAccessLayer objdel = new BillingSqlDataAccessLayer();
            return objdel.Del_AdminTransaction(objtran, Loginhistoryid);
        }
        public void INS_AdminChequeTransaction(RPBilling objbill)
        {
            BillingSqlDataAccessLayer objinsCheck = new BillingSqlDataAccessLayer();
            objinsCheck.INS_AdminChequeTransaction(objbill);
        }
        public void invoiceRegenerate(RPBilling objgenrate)
        {
            BillingSqlDataAccessLayer objinv = new BillingSqlDataAccessLayer();
            objinv.invoiceRegenerate(objgenrate);
        }
        public DataSet Insert_Customertransaction(RPBilling ObjInsTransaction, ref string Out_Msg)
        {
            BillingSqlDataAccessLayer objinv = new BillingSqlDataAccessLayer();
           return objinv.Insert_Customertransaction(ObjInsTransaction, ref Out_Msg);
        }

        public string Get_Appointmentdate(RPBilling patappid)
        {
            BillingSqlDataAccessLayer objUnapplied = new BillingSqlDataAccessLayer();
            return objUnapplied.Get_Appointmentdate(patappid);
        }
    }
}