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
    
    public partial class COURSE
    {
        public COURSE()
        {
            this.EVENT = new HashSet<EVENT>();
        }
    
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Programme_Id { get; set; }
        public int Department_Id { get; set; }
        public int Level_Id { get; set; }
        public Nullable<long> Staff_Id { get; set; }
        public bool Active { get; set; }
    
        public virtual DEPARTMENT DEPARTMENT { get; set; }
        public virtual LEVEL LEVEL { get; set; }
        public virtual PROGRAMME PROGRAMME { get; set; }
        public virtual STAFF STAFF { get; set; }
        public virtual ICollection<EVENT> EVENT { get; set; }
    }
}
