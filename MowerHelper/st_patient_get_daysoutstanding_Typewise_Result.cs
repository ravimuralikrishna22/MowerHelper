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
    
    public partial class st_patient_get_daysoutstanding_Typewise_Result
    {
        public long Transaction_ID { get; set; }
        public string Transaction_Date { get; set; }
        public Nullable<decimal> chargeamount { get; set; }
        public Nullable<decimal> paymentamount { get; set; }
        public string NOTES { get; set; }
        public Nullable<long> PatientLogin_ID { get; set; }
        public Nullable<int> Patient_ID { get; set; }
        public long FromAccount_ID { get; set; }
        public Nullable<int> FromReference_ID { get; set; }
        public Nullable<byte> FromReferenceType_ID { get; set; }
        public Nullable<byte> ToReferenceType_ID { get; set; }
        public Nullable<int> ToReference_ID { get; set; }
        public string InvoiceGenerated_Ind { get; set; }
        public string Transactiontype { get; set; }
        public string PaymentMode { get; set; }
        public string BillableParty { get; set; }
        public Nullable<long> ToReferenceLogin_ID { get; set; }
        public Nullable<int> apppointment_id { get; set; }
        public string Appointmentdate { get; set; }
        public string updated_apptdate { get; set; }
        public string updated_transdate { get; set; }
    }
}