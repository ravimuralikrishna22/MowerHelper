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
    
    public partial class um_ClientEvent_Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public um_ClientEvent_Category()
        {
            this.ut_ClientEventInfo_Log = new HashSet<ut_ClientEventInfo_Log>();
        }
    
        public int EventCategory_ID { get; set; }
        public string Event_Title { get; set; }
        public string status_ind { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public Nullable<int> Createdby { get; set; }
        public Nullable<System.DateTime> Updatedon { get; set; }
        public Nullable<int> Updatedby { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ut_ClientEventInfo_Log> ut_ClientEventInfo_Log { get; set; }
    }
}
