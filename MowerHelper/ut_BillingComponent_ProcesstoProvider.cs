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
    
    public partial class ut_BillingComponent_ProcesstoProvider
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ut_BillingComponent_ProcesstoProvider()
        {
            this.ut_Component_SucceddUsers = new HashSet<ut_Component_SucceddUsers>();
            this.ut_Component_CCProcess_Failed = new HashSet<ut_Component_CCProcess_Failed>();
            this.ut_Component_CreditCard_Expiry = new HashSet<ut_Component_CreditCard_Expiry>();
        }
    
        public long ProcessProvider_ID { get; set; }
        public Nullable<long> Provider_ID { get; set; }
        public Nullable<long> Practice_ID { get; set; }
        public Nullable<long> Login_ID { get; set; }
        public string BillingService_ID { get; set; }
        public Nullable<System.DateTime> NextRenewdate { get; set; }
        public Nullable<long> CredicardInfo_ID { get; set; }
        public Nullable<byte> Cardexpirymonth { get; set; }
        public Nullable<int> cardexpiryyear { get; set; }
        public string CardExpiry { get; set; }
        public string ProviderEmail { get; set; }
        public string IsCCExpired { get; set; }
        public string IsCCFailed { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<byte> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<byte> UpdatedBy { get; set; }
        public string IsRenewSucced_Ind { get; set; }
        public string Billingservice_Title { get; set; }
        public Nullable<decimal> Total_Amount { get; set; }
        public Nullable<int> RunningSequence { get; set; }
        public Nullable<long> RefProcessProvider_ID { get; set; }
        public string CCDetaislUpdated_Ind { get; set; }
        public string paypal_vaultid { get; set; }
        public Nullable<int> Bschargetemplate_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_Component_SucceddUsers> ut_Component_SucceddUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_Component_CCProcess_Failed> ut_Component_CCProcess_Failed { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_Component_CreditCard_Expiry> ut_Component_CreditCard_Expiry { get; set; }
    }
}