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
    
    public partial class ut_ForumMessage_SHADOW
    {
        public int Message_ID { get; set; }
        public int Topic_ID { get; set; }
        public string MessageName { get; set; }
        public string MessageDescription { get; set; }
        public string image_Name { get; set; }
        public Nullable<bool> Locked_Ind { get; set; }
        public Nullable<int> Reply_Message_ID { get; set; }
        public string Status_Ind { get; set; }
        public string IsMainMessage { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public long Shadow_ID { get; set; }
        public Nullable<System.DateTime> Shadow_Date { get; set; }
        public string updatedFields { get; set; }
    }
}
