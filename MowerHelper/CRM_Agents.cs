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
    
    public partial class CRM_Agents
    {
        public long CRM_Agent_ID { get; set; }
        public string AgentCode { get; set; }
        public string CompanyName { get; set; }
        public string Prefix { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string MI { get; set; }
        public string Suffix { get; set; }
        public string WorkPhone { get; set; }
        public string MobilePhon { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public Nullable<long> State_ID { get; set; }
        public Nullable<long> City_ID { get; set; }
        public string Email { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<long> CRM_Promocode_ID { get; set; }
        public Nullable<System.DateTime> Expirydate { get; set; }
        public string ISFTAgent_Ind { get; set; }
        public string DCity_Name { get; set; }
        public string DState_Name { get; set; }
        public string DCountry_Name { get; set; }
        public string ZIP { get; set; }
        public Nullable<int> provider_id { get; set; }
        public Nullable<int> Providerlogin_id { get; set; }
    
        public virtual CRM_PromoCodes CRM_PromoCodes { get; set; }
    }
}
