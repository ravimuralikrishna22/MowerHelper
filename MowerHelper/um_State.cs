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
    
    public partial class um_State
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public um_State()
        {
            this.um_City = new HashSet<um_City>();
            this.um_CreditCardInfo = new HashSet<um_CreditCardInfo>();
            this.um_Practice_Provider_Patient_XREF = new HashSet<um_Practice_Provider_Patient_XREF>();
            this.ut_Addresses = new HashSet<ut_Addresses>();
            this.um_ProviderPublicList = new HashSet<um_ProviderPublicList>();
        }
    
        public int State_ID { get; set; }
        public string State_Name { get; set; }
        public string State { get; set; }
        public string Abbrevation { get; set; }
        public Nullable<int> Country_ID { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string IsPublicTherapist_Ind { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<um_City> um_City { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<um_CreditCardInfo> um_CreditCardInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<um_Practice_Provider_Patient_XREF> um_Practice_Provider_Patient_XREF { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_Addresses> ut_Addresses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<um_ProviderPublicList> um_ProviderPublicList { get; set; }
    }
}