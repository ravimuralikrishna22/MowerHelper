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
    
    public partial class st_Billing_List_ApplyingChargeTransaction_Result
    {
        public Nullable<int> Distribution_ID { get; set; }
        public Nullable<decimal> AppliedAmount { get; set; }
        public decimal Transaction_Amount { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public int ApplicableAppliedAmount { get; set; }
        public Nullable<decimal> DistributedoRAppliedAmount { get; set; }
        public Nullable<System.DateTime> Transaction_Date { get; set; }
        public Nullable<long> PatientLogin_ID { get; set; }
        public string PaymentType { get; set; }
        public Nullable<long> ChargeTransaction_ID { get; set; }
        public long PaymentTransaction_ID { get; set; }
        public string PaidByName { get; set; }
        public Nullable<int> ToReference_ID { get; set; }
    }
}
