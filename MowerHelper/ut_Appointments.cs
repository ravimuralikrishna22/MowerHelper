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
    
    public partial class ut_Appointments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ut_Appointments()
        {
            this.ut_Appointment_Tracking = new HashSet<ut_Appointment_Tracking>();
        }
    
        public int Appointment_ID { get; set; }
        public Nullable<int> Practice_ID { get; set; }
        public Nullable<byte> FromReferenceType_ID { get; set; }
        public int FromReference_ID { get; set; }
        public long FromReferenceLogin_ID { get; set; }
        public Nullable<long> ToReferenceLogin_ID { get; set; }
        public Nullable<int> ToReference_ID { get; set; }
        public Nullable<byte> ToReferenceType_ID { get; set; }
        public Nullable<int> FacilityReference_ID { get; set; }
        public Nullable<byte> AppointmentType_ID { get; set; }
        public Nullable<int> CurrentAppointmentTracking_ID { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> Loginhistory_ID { get; set; }
        public string Technician_ids { get; set; }
        public string Tech_pos_mobile { get; set; }
        public string Msg_Ind { get; set; }
        public Nullable<int> AppointmentRecurrence_ID { get; set; }
        public string placeofservice { get; set; }
        public Nullable<int> Appointment_address_id { get; set; }
    
        public virtual um_AppointmentTypes um_AppointmentTypes { get; set; }
        public virtual um_ReferenceTypes um_ReferenceTypes { get; set; }
        public virtual um_ReferenceTypes um_ReferenceTypes1 { get; set; }
        public virtual um_User um_User { get; set; }
        public virtual um_User um_User1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_Appointment_Tracking> ut_Appointment_Tracking { get; set; }
        public virtual ut_Appointment_Tracking ut_Appointment_Tracking1 { get; set; }
        public virtual ut_AppointmentRecurrence ut_AppointmentRecurrence { get; set; }
    }
}
