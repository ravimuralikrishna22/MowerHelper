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
    
    public partial class ut_Component_JobSettings
    {
        public int ComponentJobsetting_ID { get; set; }
        public int Component_ID { get; set; }
        public int Jobtype_ID { get; set; }
        public Nullable<int> IntervalValue { get; set; }
        public string NoOfOccurences { get; set; }
        public Nullable<System.DateTime> OnTime { get; set; }
        public Nullable<System.DateTime> NextRuntime { get; set; }
        public Nullable<int> Duration { get; set; }
        public System.DateTime Startdate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> NextRundate { get; set; }
        public Nullable<System.DateTime> RunTime { get; set; }
        public Nullable<System.DateTime> LastRunDate { get; set; }
        public string Weekdays { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
    }
}