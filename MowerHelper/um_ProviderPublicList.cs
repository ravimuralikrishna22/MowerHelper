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
    
    public partial class um_ProviderPublicList
    {
        public int Provider_ID { get; set; }
        public string Lastname { get; set; }
        public string Fullname { get; set; }
        public string Businessname { get; set; }
        public string l_Address { get; set; }
        public string l_City { get; set; }
        public Nullable<int> City_ID { get; set; }
        public Nullable<int> State_ID { get; set; }
        public string l_State { get; set; }
        public Nullable<int> Contry_ID { get; set; }
        public string l_Country { get; set; }
        public string l_zipcode { get; set; }
        public string l_vmoffice { get; set; }
        public string l_fax { get; set; }
        public string l_webaddress { get; set; }
        public string l_paragraph { get; set; }
        public string PublicURL { get; set; }
        public string Status_ind { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<System.DateTime> RegisteredDate { get; set; }
        public Nullable<long> County_ID { get; set; }
        public string CountName { get; set; }
        public string State { get; set; }
        public string Is_Contact_Info { get; set; }
        public string DisplayFee_ind { get; set; }
        public Nullable<long> Loginhistory_ID { get; set; }
        public string Firstfreesession { get; set; }
        public Nullable<int> l_yearsinpractice { get; set; }
        public string Facebookurl { get; set; }
        public string Twitterurl { get; set; }
        public string Showcomments { get; set; }
        public string Random_number { get; set; }
    
        public virtual um_City um_City { get; set; }
        public virtual um_State um_State { get; set; }
    }
}
