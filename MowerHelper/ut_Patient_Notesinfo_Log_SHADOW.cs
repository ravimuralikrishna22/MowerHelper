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
    
    public partial class ut_Patient_Notesinfo_Log_SHADOW
    {
        public int Notes_Loginfo_id { get; set; }
        public Nullable<int> EventInfoLog_ID { get; set; }
        public Nullable<int> PatientLogin_id { get; set; }
        public string Field { get; set; }
        public string PreviousValue { get; set; }
        public string PresentValue { get; set; }
        public string Status_ind { get; set; }
        public Nullable<System.DateTime> Createdon { get; set; }
        public Nullable<int> Createdby { get; set; }
        public Nullable<System.DateTime> Updatedon { get; set; }
        public Nullable<int> Updatedby { get; set; }
        public Nullable<System.DateTime> Shadow_Date { get; set; }
        public string updatedFields { get; set; }
        public long Shadow_id { get; set; }
    }
}
