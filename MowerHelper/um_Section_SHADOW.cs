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
    
    public partial class um_Section_SHADOW
    {
        public int Section_ID { get; set; }
        public string SectionName { get; set; }
        public string SectionPath { get; set; }
        public string SectionTitle { get; set; }
        public string SectionMenuTitle { get; set; }
        public string SectionDescription { get; set; }
        public bool Section_isEnabled { get; set; }
        public Nullable<int> Section_ParentID { get; set; }
        public Nullable<bool> Section_isParent { get; set; }
        public Nullable<short> Level_No { get; set; }
        public string DisplayLevel { get; set; }
        public Nullable<int> Section_sortOrder { get; set; }
        public bool Section_IsSystem { get; set; }
        public Nullable<bool> AllowOthersToAccess_Ind { get; set; }
        public Nullable<short> DisplayPriority { get; set; }
        public string ImageFilename_1 { get; set; }
        public string ImageFilename_2 { get; set; }
        public Nullable<bool> OpenInNewWindow_Ind { get; set; }
        public Nullable<bool> Service_Ind { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<System.DateTime> Shadow_Date { get; set; }
        public string updatedFields { get; set; }
        public long Shadow_id { get; set; }
        public Nullable<long> Loginhistory_ID { get; set; }
    }
}
