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
    
    public partial class ut_Task_Actionitems_SHADOW
    {
        public int Actionitem_id { get; set; }
        public Nullable<int> Task_id { get; set; }
        public string Actionitemtitle { get; set; }
        public Nullable<System.DateTime> LastAction { get; set; }
        public Nullable<int> ByReference_ID { get; set; }
        public Nullable<int> ByReferenceType_ID { get; set; }
        public string Staus_ind { get; set; }
        public Nullable<System.DateTime> Createdon { get; set; }
        public Nullable<int> createdby { get; set; }
        public Nullable<System.DateTime> updatedon { get; set; }
        public Nullable<int> updatedby { get; set; }
        public Nullable<System.DateTime> Shadow_Date { get; set; }
        public string updatedFields { get; set; }
        public long Shadow_id { get; set; }
    }
}