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
    
    public partial class st_Billing_List_AdmintoPracticeChargesToGenerateInvoice_Result
    {
        public long Transaction_ID { get; set; }
        public Nullable<long> ROWNUMBER { get; set; }
        public Nullable<int> BillingService_ID { get; set; }
        public string ServiceName { get; set; }
        public Nullable<int> BillingChargeType_ID { get; set; }
        public string ChargeType { get; set; }
        public Nullable<System.DateTime> Transaction_Date { get; set; }
        public decimal Transaction_Amount { get; set; }
        public string Notes { get; set; }
        public byte TransactionType_ID { get; set; }
    }
}
