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
    
    public partial class STUDENT
    {
        public STUDENT()
        {
            this.ABSENT_LOG = new HashSet<ABSENT_LOG>();
            this.ATTENDANCE = new HashSet<ATTENDANCE>();
            this.STUDENT_LEVEL = new HashSet<STUDENT_LEVEL>();
        }
    
        public long Person_Id { get; set; }
        public string Matric_Number { get; set; }
        public Nullable<int> Hall_Id { get; set; }
        public bool Active { get; set; }
    
        public virtual ICollection<ABSENT_LOG> ABSENT_LOG { get; set; }
        public virtual ICollection<ATTENDANCE> ATTENDANCE { get; set; }
        public virtual HALL HALL { get; set; }
        public virtual PERSON PERSON { get; set; }
        public virtual ICollection<STUDENT_LEVEL> STUDENT_LEVEL { get; set; }
    }
}