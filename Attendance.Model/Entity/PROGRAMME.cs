//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Attendance.Model.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class PROGRAMME
    {
        public PROGRAMME()
        {
            this.COURSE = new HashSet<COURSE>();
            this.EVENT = new HashSet<EVENT>();
            this.STUDENT_LEVEL = new HashSet<STUDENT_LEVEL>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    
        public virtual ICollection<COURSE> COURSE { get; set; }
        public virtual ICollection<EVENT> EVENT { get; set; }
        public virtual ICollection<STUDENT_LEVEL> STUDENT_LEVEL { get; set; }
    }
}
