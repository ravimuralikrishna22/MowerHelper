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
    
    public partial class ut_Billing_Accounts_SHADOW
    {
        public long Account_ID { get; set; }
        public string AccountNo { get; set; }
        public Nullable<long> ReferenceLogin_ID { get; set; }
        public Nullable<int> Reference_ID { get; set; }
        public Nullable<byte> ReferenceType_ID { get; set; }
        public string OpeningBalanceType { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public string CurrentBalanceType { get; set; }
        public Nullable<decimal> CurrentBalance { get; set; }
        public string AccountDescription { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<System.DateTime> Shadow_Date { get; set; }
        public string updatedFields { get; set; }
        public long Shadow_id { get; set; }
    }
}