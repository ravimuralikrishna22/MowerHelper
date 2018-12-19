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
    
    public partial class um_Role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public um_Role()
        {
            this.um_SectionsInRole = new HashSet<um_SectionsInRole>();
            this.um_UsersInRole = new HashSet<um_UsersInRole>();
            this.ut_Message_RelatedRoles = new HashSet<ut_Message_RelatedRoles>();
            this.ut_Message_RelatedRoles1 = new HashSet<ut_Message_RelatedRoles>();
            this.ut_MessageCategoryInRole = new HashSet<ut_MessageCategoryInRole>();
        }
    
        public byte Role_ID { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string IsSystemRole { get; set; }
        public string Changeable_Ind { get; set; }
        public string ViewableToBillingService_Ind { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<um_SectionsInRole> um_SectionsInRole { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<um_UsersInRole> um_UsersInRole { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_Message_RelatedRoles> ut_Message_RelatedRoles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_Message_RelatedRoles> ut_Message_RelatedRoles1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_MessageCategoryInRole> ut_MessageCategoryInRole { get; set; }
    }
}
