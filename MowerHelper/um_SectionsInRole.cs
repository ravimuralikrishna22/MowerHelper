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
    
    public partial class um_SectionsInRole
    {
        public int SectionsInRole_ID { get; set; }
        public Nullable<int> Section_ID { get; set; }
        public Nullable<byte> Role_ID { get; set; }
        public Nullable<long> Login_ID { get; set; }
        public Nullable<int> Parent_Section_ID { get; set; }
        public string AliasName { get; set; }
        public string SectionPath { get; set; }
        public Nullable<bool> Function_Ind { get; set; }
        public Nullable<short> DisplayPriority { get; set; }
        public string ImageFilename_1 { get; set; }
        public string ImageFilename_2 { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> Loginhistory_ID { get; set; }
    
        public virtual um_Role um_Role { get; set; }
        public virtual um_User um_User { get; set; }
    }
}
