//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class University
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public University()
        {
            this.Faculty_of_University = new HashSet<Faculty_of_University>();
        }
    
        public int id { get; set; }
        public string Site { get; set; }
        public string UniversityName { get; set; }
        public string UniversityCode { get; set; }
        public string HotKey { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Faculty_of_University> Faculty_of_University { get; set; }
    }
}
