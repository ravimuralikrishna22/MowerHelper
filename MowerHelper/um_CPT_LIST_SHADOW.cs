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
    
    public partial class um_CPT_LIST_SHADOW
    {
        public int CPTCode_ID { get; set; }
        public int CPTCode { get; set; }
        public string Description { get; set; }
        public Nullable<bool> RelatedToMentalHealth { get; set; }
        public Nullable<byte> Duration { get; set; }
        public Nullable<short> Priority { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<System.DateTime> Shadow_Date { get; set; }
        public string updatedFields { get; set; }
        public long Shadow_id { get; set; }
        public Nullable<long> Loginhistory_ID { get; set; }
        public string regular_ind { get; set; }
    }
}
