//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MowerHelper
{
    using System;
    
    public partial class st_CreditCard_Get_PatientTransactionInfo_Result
    {
        public Nullable<System.DateTime> Tran_Date { get; set; }
        public Nullable<int> FromReference_ID { get; set; }
        public string FromUserName { get; set; }
        public Nullable<byte> FromReferenceType_ID { get; set; }
        public Nullable<int> ToReference_ID { get; set; }
        public Nullable<byte> ToReferenceType_ID { get; set; }
        public Nullable<int> Practice_ID { get; set; }
        public string Posted_Data { get; set; }
        public string InvoiceNum { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string CreditCardNo { get; set; }
        public Nullable<int> CreditCardType_ID { get; set; }
        public string CVV2 { get; set; }
        public Nullable<decimal> Totalamount { get; set; }
        public string CardHolderName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Nullable<byte> CardExpiryMonth { get; set; }
        public Nullable<short> CardExpiryYear { get; set; }
        public string BillingAddress_1 { get; set; }
        public string BillingAddress_2 { get; set; }
        public Nullable<int> BillingCity_ID { get; set; }
        public string City { get; set; }
        public Nullable<int> BillingState_ID { get; set; }
        public string State { get; set; }
        public Nullable<int> BillingCountry_ID { get; set; }
        public string Country { get; set; }
        public string BillingZIP { get; set; }
        public string TransactionNo { get; set; }
        public Nullable<bool> ManualTransaction_Ind { get; set; }
        public Nullable<int> TransactionNoRef_ID { get; set; }
        public Nullable<int> TransactionNoRefType_ID { get; set; }
        public Nullable<long> ParentTransaction_ID { get; set; }
        public string IsRefundable_Ind { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<long> RefPatientLogin_ID { get; set; }
        public string GatewayDetail_ID { get; set; }
        public string IsPayPalProcess_Ind { get; set; }
        public Nullable<int> BillingService_ID { get; set; }
        public Nullable<int> Provider_ID { get; set; }
        public string paypalsaletransactionid { get; set; }
        public string paypaltransactionid { get; set; }
        public string To_Emailid { get; set; }
    }
}