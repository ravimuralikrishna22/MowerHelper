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
    
    public partial class st_billing_view_TransactionsDistributionDetail_Result
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
        public Nullable<System.DateTime> Transaction_Date { get; set; }
        public string ChargeType { get; set; }
        public string AdjustmentType { get; set; }
        public Nullable<int> AdjustmentType_ID { get; set; }
        public Nullable<System.DateTime> PatientRegDate { get; set; }
        public Nullable<long> RefTransaction_ID { get; set; }
        public Nullable<long> Ref_ManualTran_id { get; set; }
    }
}