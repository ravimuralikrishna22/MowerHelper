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
    
    public partial class st_billing_view_Chargepayment_Result
    {
        public Nullable<int> FromReference_ID { get; set; }
        public Nullable<byte> FromReferenceType_ID { get; set; }
        public Nullable<int> ToReference_ID { get; set; }
        public Nullable<byte> ToReferenceType_ID { get; set; }
        public string ProviderName { get; set; }
        public string ToRefName { get; set; }
        public decimal Transaction_Amount { get; set; }
        public string Patientname { get; set; }
        public string Notes { get; set; }
        public Nullable<int> BillingService_ID { get; set; }
        public string Transaction_Date { get; set; }
        public Nullable<byte> PaymentType_ID { get; set; }
        public string Authorizationnumber { get; set; }
        public string ChequeNo { get; set; }
        public string Email { get; set; }
        public byte TransactionType_ID { get; set; }
    }
}
