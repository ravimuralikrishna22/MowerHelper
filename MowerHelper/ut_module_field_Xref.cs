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
    
    public partial class ut_module_field_Xref
    {
        public int Fieldid { get; set; }
        public Nullable<int> Module_id { get; set; }
        public string Fieldname { get; set; }
        public string Description { get; set; }
        public string Status_ind { get; set; }
        public Nullable<System.DateTime> Createdon { get; set; }
        public Nullable<int> Createdby { get; set; }
        public Nullable<int> Updatedby { get; set; }
        public Nullable<System.DateTime> Updatedon { get; set; }
    
        public virtual Um_Modules Um_Modules { get; set; }
    }
}
