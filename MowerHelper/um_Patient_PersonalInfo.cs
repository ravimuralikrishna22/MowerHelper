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
    
    public partial class um_Patient_PersonalInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public um_Patient_PersonalInfo()
        {
            this.um_Practice_Provider_Patient_XREF = new HashSet<um_Practice_Provider_Patient_XREF>();
        }
    
        public int Patient_ID { get; set; }
        public Nullable<long> Login_ID { get; set; }
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string PatientEmail { get; set; }
        public string DrivingLicenceNo { get; set; }
        public Nullable<int> Patient_Status_ID { get; set; }
        public Nullable<System.DateTime> Registerdate { get; set; }
        public string SelfRegistered_Ind { get; set; }
        public Nullable<int> RegByUserRefType_ID { get; set; }
        public Nullable<int> RegByUserRef_ID { get; set; }
        public Nullable<long> RegByUserLogin_ID { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string Full_name { get; set; }
        public Nullable<long> Loginhistory_ID { get; set; }
        public Nullable<long> Account_ID { get; set; }
    
        public virtual um_Patient_Status um_Patient_Status { get; set; }
        public virtual um_User um_User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<um_Practice_Provider_Patient_XREF> um_Practice_Provider_Patient_XREF { get; set; }
    }
}