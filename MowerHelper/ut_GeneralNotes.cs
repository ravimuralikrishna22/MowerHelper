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
    
    public partial class ut_GeneralNotes
    {
        public int GeneralNote_ID { get; set; }
        public int FromReference_ID { get; set; }
        public int FromReferenceType_ID { get; set; }
        public Nullable<int> ToReference_ID { get; set; }
        public Nullable<int> ToReferenceType_ID { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> Notes_Date { get; set; }
        public string IsPatientNote_Ind { get; set; }
        public string status_ind { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<long> Practice_ID { get; set; }
        public Nullable<long> LoginHistory_ID { get; set; }
        public Nullable<int> Appointment_id { get; set; }
        public Nullable<int> Transaction_id { get; set; }
        public string Image_file { get; set; }
    }
}
