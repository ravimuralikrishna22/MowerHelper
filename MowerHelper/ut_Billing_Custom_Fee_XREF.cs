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
    
    public partial class ut_Billing_Custom_Fee_XREF
    {
        public int BillingCustomFee_ID { get; set; }
        public Nullable<int> BillingServiceFee_ID { get; set; }
        public Nullable<long> ReferenceLogin_ID { get; set; }
        public Nullable<int> Reference_ID { get; set; }
        public byte ReferenceType_ID { get; set; }
        public int BillingService_ID { get; set; }
        public Nullable<int> BillingChargeType_ID { get; set; }
        public Nullable<int> BillingserviceType_ID { get; set; }
        public Nullable<int> BSChargeTemplate_ID { get; set; }
        public Nullable<decimal> ServiceFee { get; set; }
        public decimal CustomFee { get; set; }
        public string ServiceDescription { get; set; }
        public Nullable<double> ServiceTaxInPercentage { get; set; }
        public string IsThisTaxable { get; set; }
        public Nullable<System.DateTime> RequestedDate { get; set; }
        public Nullable<System.DateTime> ActivatedDate { get; set; }
        public Nullable<System.DateTime> RenewDate { get; set; }
        public Nullable<System.DateTime> NextRenewDate { get; set; }
        public Nullable<int> NbrAllotment { get; set; }
        public Nullable<int> RemainingAllotment { get; set; }
        public string IsActive { get; set; }
        public string IsDeafultService { get; set; }
        public Nullable<int> RenewCount { get; set; }
        public string Status_Ind { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<System.DateTime> ActualActiveDate { get; set; }
        public string IsCustomFeeEdited { get; set; }
        public Nullable<long> Promocode_ID { get; set; }
        public Nullable<int> Days_Duration { get; set; }
        public Nullable<System.DateTime> Formated_NextRnewdate { get; set; }
        public string IsAdminActive_ind { get; set; }
        public Nullable<byte> FailedCount { get; set; }
    
        public virtual um_Billing_ChargeTypes um_Billing_ChargeTypes { get; set; }
        public virtual um_ReferenceTypes um_ReferenceTypes { get; set; }
        public virtual um_User um_User { get; set; }
    }
}
