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
	using System.ComponentModel.DataAnnotations;

	public partial class CampuslinkProgram
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CampuslinkProgram()
        {
            this.Events = new HashSet<Event>();
        }
    
        public int id { get; set; }
        public string Code { get; set; }
		[Required]
		public string CP_Name { get; set; }
        public Nullable<int> LearningTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Event> Events { get; set; }
    }
}
