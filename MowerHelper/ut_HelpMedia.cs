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
    
    public partial class ut_HelpMedia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ut_HelpMedia()
        {
            this.Um_FAQ_RelatedLinks = new HashSet<Um_FAQ_RelatedLinks>();
        }
    
        public int HelpMedia_ID { get; set; }
        public Nullable<int> Reference_ID { get; set; }
        public Nullable<byte> ReferenceType_ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string FilePath_Encrypted { get; set; }
        public string ImagepathSuffix { get; set; }
        public string IsPracticeMedia_Ind { get; set; }
        public string IsDisplayToPractice { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> Loginhistory_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Um_FAQ_RelatedLinks> Um_FAQ_RelatedLinks { get; set; }
    }
}
