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
    
    public partial class um_CognodeDetails_SHADOW
    {
        public int CognodeDetail_ID { get; set; }
        public string Merchant_Email { get; set; }
        public string Ship_to_first_name { get; set; }
        public string Ship_to_last_name { get; set; }
        public string Ship_to_company { get; set; }
        public string Ship_to_address { get; set; }
        public Nullable<int> Ship_to_city_ID { get; set; }
        public Nullable<int> Ship_to_state_ID { get; set; }
        public string Ship_to_zip { get; set; }
        public Nullable<int> Ship_to_country_ID { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public Nullable<int> City_ID { get; set; }
        public Nullable<int> State_ID { get; set; }
        public string ZIP { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<System.DateTime> Shadow_Date { get; set; }
        public string updatedFields { get; set; }
        public long Shadow_id { get; set; }
    }
}