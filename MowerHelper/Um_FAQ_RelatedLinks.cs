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
    
    public partial class Um_FAQ_RelatedLinks
    {
        public long RelatedLink_Id { get; set; }
        public Nullable<long> Question_id { get; set; }
        public string LinkName { get; set; }
        public string Status_ind { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> Video_id { get; set; }
    
        public virtual Um_FAQ_Questions Um_FAQ_Questions { get; set; }
        public virtual ut_HelpMedia ut_HelpMedia { get; set; }
    }
}