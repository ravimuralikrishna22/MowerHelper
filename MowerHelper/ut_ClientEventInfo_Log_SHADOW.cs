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
    
    public partial class ut_ClientEventInfo_Log_SHADOW
    {
        public int EventInfoLog_ID { get; set; }
        public Nullable<byte> EventCategory_ID { get; set; }
        public Nullable<int> GoalEventtype_ID { get; set; }
        public Nullable<long> ClientLogin_ID { get; set; }
        public Nullable<int> EventReference_ID { get; set; }
        public string Title { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<System.DateTime> Shadow_Date { get; set; }
        public string updatedFields { get; set; }
        public long Shadow_id { get; set; }
    }
}