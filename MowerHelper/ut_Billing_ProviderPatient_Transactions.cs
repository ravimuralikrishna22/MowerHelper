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
    
    public partial class ut_Billing_ProviderPatient_Transactions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ut_Billing_ProviderPatient_Transactions()
        {
            this.ut_Billing_ProviderPatient_ChequeTransactions = new HashSet<ut_Billing_ProviderPatient_ChequeTransactions>();
            this.ut_Billing_ProviderPatient_TransactionsDistribution = new HashSet<ut_Billing_ProviderPatient_TransactionsDistribution>();
            this.ut_Billing_ProviderPatient_TransactionsDistribution1 = new HashSet<ut_Billing_ProviderPatient_TransactionsDistribution>();
        }
    
        public long Transaction_ID { get; set; }
        public long FromAccount_ID { get; set; }
        public Nullable<int> FromReference_ID { get; set; }
        public Nullable<byte> FromReferenceType_ID { get; set; }
        public Nullable<byte> ToReferenceType_ID { get; set; }
        public Nullable<int> ToReference_ID { get; set; }
        public Nullable<long> ToReferenceLogin_ID { get; set; }
        public long ToAccount_ID { get; set; }
        public Nullable<long> PatientLogin_ID { get; set; }
        public Nullable<int> Practice_ID { get; set; }
        public Nullable<int> BillingService_ID { get; set; }
        public Nullable<int> BillingChargetype_ID { get; set; }
        public Nullable<System.DateTime> Transaction_Date { get; set; }
        public byte TransactionType_ID { get; set; }
        public decimal Transaction_Amount { get; set; }
        public Nullable<decimal> DistributedoRAppliedAmount { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public string Notes { get; set; }
        public Nullable<byte> PaymentType_ID { get; set; }
        public Nullable<long> ChequeOrCCTran_ID { get; set; }
        public Nullable<long> RefTransaction_ID { get; set; }
        public string InvoiceGenerated_Ind { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string IsRefundTransaction { get; set; }
        public Nullable<int> AdjustmentType_ID { get; set; }
        public Nullable<long> Ref_ManualTran_id { get; set; }
        public Nullable<long> Loginhistory_id { get; set; }
        public string Authorizationnumber { get; set; }
        public Nullable<long> Provider_id { get; set; }
        public Nullable<int> apppointment_id { get; set; }
        public Nullable<System.DateTime> Appointmentdate { get; set; }
    
        public virtual um_Billing_ChargeTypes um_Billing_ChargeTypes { get; set; }
        public virtual um_Billing_PaymentTypes um_Billing_PaymentTypes { get; set; }
        public virtual um_Billing_TransactionTypes um_Billing_TransactionTypes { get; set; }
        public virtual um_ReferenceTypes um_ReferenceTypes { get; set; }
        public virtual um_ReferenceTypes um_ReferenceTypes1 { get; set; }
        public virtual um_User um_User { get; set; }
        public virtual ut_Billing_Accounts ut_Billing_Accounts { get; set; }
        public virtual ut_Billing_Accounts ut_Billing_Accounts1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_Billing_ProviderPatient_ChequeTransactions> ut_Billing_ProviderPatient_ChequeTransactions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_Billing_ProviderPatient_TransactionsDistribution> ut_Billing_ProviderPatient_TransactionsDistribution { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_Billing_ProviderPatient_TransactionsDistribution> ut_Billing_ProviderPatient_TransactionsDistribution1 { get; set; }
    }
}
