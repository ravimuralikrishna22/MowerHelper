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
    
    public partial class Um_FAQ_Questions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Um_FAQ_Questions()
        {
            this.Um_FAQ_Question_Rating = new HashSet<Um_FAQ_Question_Rating>();
            this.Um_FAQ_RelatedLinks = new HashSet<Um_FAQ_RelatedLinks>();
            this.Ut_FAQ_Question_category_Xref = new HashSet<Ut_FAQ_Question_category_Xref>();
            this.Um_FAQ_RelatedFaqs = new HashSet<Um_FAQ_RelatedFaqs>();
        }
    
        public long Question_Id { get; set; }
        public Nullable<int> FromRole_Id { get; set; }
        public Nullable<int> ToRole_ID { get; set; }
        public string Category_id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Question { get; set; }
        public string Answertext { get; set; }
        public string Tags { get; set; }
        public Nullable<System.DateTime> PostedDate { get; set; }
        public string Status { get; set; }
        public string public_ind { get; set; }
        public string provider_Ind { get; set; }
        public string Status_ind { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> ViewedCount { get; set; }
        public Nullable<long> Loginhistory_ID { get; set; }
        public string verificationuser_Ind { get; set; }
        public Nullable<decimal> Rate_Value { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Um_FAQ_Question_Rating> Um_FAQ_Question_Rating { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Um_FAQ_RelatedLinks> Um_FAQ_RelatedLinks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ut_FAQ_Question_category_Xref> Ut_FAQ_Question_category_Xref { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Um_FAQ_RelatedFaqs> Um_FAQ_RelatedFaqs { get; set; }
    }
}
