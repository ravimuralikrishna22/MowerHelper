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
    
    public partial class um_EmailMessageOption_Types
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public um_EmailMessageOption_Types()
        {
            this.um_EmailMessage_Options = new HashSet<um_EmailMessage_Options>();
        }
    
        public int EmailMessageOptionType_ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status_ind { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UPdatedOn { get; set; }
        public Nullable<long> RefEmailMsgOptionType_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<um_EmailMessage_Options> um_EmailMessage_Options { get; set; }
    }
}
