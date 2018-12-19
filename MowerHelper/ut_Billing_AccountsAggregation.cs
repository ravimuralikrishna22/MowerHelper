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
    using System.Collections.Generic;
    
    public partial class ut_Billing_AccountsAggregation
    {
        public int Aggregation_ID { get; set; }
        public long Account_ID { get; set; }
        public Nullable<int> Reference_ID { get; set; }
        public Nullable<byte> ReferenceType_ID { get; set; }
        public Nullable<byte> AggregateReferenceType_ID { get; set; }
        public Nullable<int> AggregateReference_ID { get; set; }
        public long AggregateAccount_ID { get; set; }
        public Nullable<decimal> TotalCharges { get; set; }
        public Nullable<decimal> TotalPayments { get; set; }
        public Nullable<decimal> TotalPositiveAdjstmnts { get; set; }
        public Nullable<decimal> TotalNegativeAdjstmnts { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public string BalanceAmountType { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual um_ReferenceTypes um_ReferenceTypes { get; set; }
        public virtual um_ReferenceTypes um_ReferenceTypes1 { get; set; }
        public virtual ut_Billing_Accounts ut_Billing_Accounts { get; set; }
        public virtual ut_Billing_Accounts ut_Billing_Accounts1 { get; set; }
    }
}
