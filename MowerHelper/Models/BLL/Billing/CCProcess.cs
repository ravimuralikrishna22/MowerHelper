using System.Collections.Generic;
using System.Data;
using MowerHelper.Models.DAL;
namespace MowerHelper.Models.BLL.Billing
{
    public class CCProcess
    {
                    public string StrChargebleRefTypeID { get; set; }
                    public string StrChargebleRefID { get; set; }
                    public string strpost { get; set; }
                    public string StrPaidRefID { get; set; }
                    public string StrPaidRefTypeID { get; set; }
                    public string strx_invoice_num { get; set; }
                    public double dblx_amount { get; set; }
                    public string strx_card_num{get;set;}
                    public string strStrCardType {get;set;}
                    public string strx_card_code {get;set;}
                    public string CVV2 {get;set;}
                    public string FirstName {get;set;}
                    public string LastName{get;set;}
                    public string IsPaypalInd{get;set;}
                    public int? StrExpMon {get;set;}
                    public int? StrExpYear {get;set;}
                    public string strBillAddress1 {get;set;}
                    public string strBillAddress2{get;set;}
                    public string strCityID{get;set;}
                    public string strStateID {get;set;}
                    public string strZipCode{get;set;}
                    public string strParentTransID {get;set;}
                    public string strPracticeID { get; set; }
                    public int RefLoginID { get; set; }
                    public string Provider_ID { get; set; }
                    public string paypaltransactionid { get; set; }
                    public string paypalsaletransactionid { get; set; }
                    public string ALLowCCCharges { get; set; }
                    public int? CreatedBy { get; set; }
                    public string practice_ind { get; set; }
                    public string CardID { get; set; }
                    public string state { get; set; }
                    public string city { get; set; }
                    public string DefaultChargeCC_ID { get; set; }
                    public string STARTDATE { get; set; }
                    public string ENDDATE { get; set; }
                    public int? StatusCode { get; set; }
                    public string OrderbyItem { get; set; }
                    public string Orderby { get; set; }
                    public int NoofRecords { get; set; }
                    public int PageNo { get; set; }
                    public string practicename { get; set; }
                    public string FromReferenceType { get; set; }
                    public string FullName { get; set; }
                    public string ToReferenceType { get; set; }
                    public string CreatedOn { get; set; }
                    public int Transaction_ID { get; set; }
                    public string Response_Data { get; set; }
                    public string Status { get; set; }
                    public static int Totalnoofrecords { get; set; }
                    public string ccholdername { get; set; }
                    public string transactionAmount { get; set; }
                    public string refundAmount { get; set; }
                    public string totalamount { get; set; }
                    public string Tran_Date { get; set; }
                    public int? strTransactionID { get; set; }
                    public string StrRespStatusCode { get; set; }
                    public string strRetval { get; set; }
                    public string ResponseCode { get; set; }
                    public int? strUserID { get; set; }
                    public string GatewayDetail_ID { get; set; }
                    public string PaypalProcessInd { get; set; }
                    public string IsRefundable_Ind { get; set; }
                    public int PaymentTransID { get; set; }
                    public string paypalsaleid { get; set; }
                    public string ToReference_ID { get; set; }
                    public string Transactionno { get; set; }
                    public string Refund_amount { get; set; }
                    public string ReferenceTypeID { get; set; }
                    public string ReferenceID { get; set; }
                    public string ccinfo_id { get; set; }
                    public string Refund_ind { get; set; }
                    public string ServiceSt { get; set; }
                    public int? CreatedBY { get; set; }
                    public string NextRenewDate { get; set; }
                    public int? CCTransaction_ID { get; set; }
                    public string LastFourdigitCCNo { get; set; }
                    public string IssuingBank
                    {
                        get;
                        set;
                    }
        public string Paidby{get;set;}
        public string PaidTo { get; set; }
                    public static int PatientInsertCCTransactionDetails(CCProcess objcc, int? loginhistoryid)
                    {
                        SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
                        return DBLayer.PatientInsertCCTransactionDetails(objcc, loginhistoryid);
                    }
                    public static bool Insert_NewRegProviderCreditcard(CCProcess objCreditCard, ref int? Out_CCID,string CardId)
                    {
                        SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
                        return DBLayer.Insert_NewRegProviderCreditcard(objCreditCard, ref Out_CCID,CardId);
                    }
                    public static bool UpdateCreditCardVaultID(string ProviderID, string Vaultid, int? Out_CCID, string customer_id = null)
                    {
                        SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
                        return DBLayer.UpdateCreditCardVaultID(ProviderID, Vaultid, Out_CCID, customer_id);
                    }
                    public static bool DeleteCreditCard(string CreditCardInfo_ID, string UserID)
                    {
                        SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
                        return DBLayer.DeleteCreditCard(CreditCardInfo_ID, UserID);
                    }
                    public static string GetVaultID(string cardinfoid)
                    {
                        SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
                        return DBLayer.GetVaultID(cardinfoid);
                    }
                    public static List<CCProcess> CreditCard_Get_paymentinfo(CCProcess obj)
                    {
                        SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
                        return DBLayer.CreditCard_Get_paymentinfo(obj);
                    }
                    public static string Insert_ServiceChargeInfo(CCProcess ObjInsServiceCharge)
                    {
                        SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
                        return DBLayer.Insert_ServiceChargeInfo(ObjInsServiceCharge);
                    }
                    public static string upgrade_ServiceCharge(CCProcess ObjInsServiceCharge)
                    {
                        SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
                        return DBLayer.upgrade_ServiceCharge(ObjInsServiceCharge);
                    }
                    public static string Insert_ServicePaymentInfo(CCProcess ObjInsServicePayment)
                    {
                        SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
                        return DBLayer.Insert_ServicePaymentInfo(ObjInsServicePayment);
                    }
        #region"Admin Module by Srinivas"
                    public static DataSet viewRequest(int? tranId)
                    {
                        SQLDataAccessLayer objsda = new SQLDataAccessLayer();
                        return objsda.viewRequest(tranId);
                    }
                    public List<BLL.Billing.CCProcess> ccProcessResponse(BLL.Billing.CCProcess objResponse)
                    {
                        SQLDataAccessLayer objsda = new SQLDataAccessLayer();
                        return objsda.ccProcessResponse(objResponse);
                    }
                    public List<BLL.Billing.CCProcess> Get_AdminCreditCardTransaction(BLL.Billing.CCProcess objcclist)
                    {
                        SQLDataAccessLayer objsda = new SQLDataAccessLayer();
                        return objsda.Get_AdminCreditCardTransaction(objcclist);
                    }
                    public static int InsertCreditCardResponse(CCProcess obj)
                    {
                        SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
                        return DBLayer.InsertCreditCardResponse(obj);
                    }
                    public static string LoadCreditCardInfo(CCProcess objcc)
                    {
                        SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
                        return DBLayer.LoadCreditCardInfo(objcc);
                    }
        #endregion
    }
}