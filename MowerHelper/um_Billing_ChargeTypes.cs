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
    
    public partial class um_Billing_ChargeTypes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public um_Billing_ChargeTypes()
        {
            this.um_Billing_Service_Fee_XREF = new HashSet<um_Billing_Service_Fee_XREF>();
            this.um_BillingService_ChargeTemplate = new HashSet<um_BillingService_ChargeTemplate>();
            this.ut_Billing_Admin_Transactions = new HashSet<ut_Billing_Admin_Transactions>();
            this.ut_Billing_Custom_Fee_XREF = new HashSet<ut_Billing_Custom_Fee_XREF>();
            this.ut_Billing_InvoiceLineItems = new HashSet<ut_Billing_InvoiceLineItems>();
            this.ut_Billing_ProviderPatient_Transactions = new HashSet<ut_Billing_ProviderPatient_Transactions>();
        }
    
        public int BillingChargeType_ID { get; set; }
        public Nullable<int> Billing_serviceType_ID { get; set; }
        public string BillingChargeTypeTitle { get; set; }
        public string ChargeTypeDescription { get; set; }
        public Nullable<int> ChargeTypeValue { get; set; }
        public string Status_Ind { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual um_Billing_ServiceTypes um_Billing_ServiceTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<um_Billing_Service_Fee_XREF> um_Billing_Service_Fee_XREF { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<um_BillingService_ChargeTemplate> um_BillingService_ChargeTemplate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_Billing_Admin_Transactions> ut_Billing_Admin_Transactions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_Billing_Custom_Fee_XREF> ut_Billing_Custom_Fee_XREF { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_Billing_InvoiceLineItems> ut_Billing_InvoiceLineItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_Billing_ProviderPatient_Transactions> ut_Billing_ProviderPatient_Transactions { get; set; }
    }
}
