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
    
    public partial class ut_Appointment_Tracking_SHADOW
    {
        public int AppointmentTracking_ID { get; set; }
        public Nullable<int> Appointment_ID { get; set; }
        public System.DateTime AppointmentDate { get; set; }
        public Nullable<System.DateTime> AppointmentTime { get; set; }
        public Nullable<int> Duration { get; set; }
        public string Notes { get; set; }
        public Nullable<byte> AppointmentStatus_ID { get; set; }
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
