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
    
    public partial class USER
    {
        public USER()
        {
            this.ABSENT_LOG = new HashSet<ABSENT_LOG>();
            this.EVENT = new HashSet<EVENT>();
        }
    
        public long Id { get; set; }
        public long Person_Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Password_Recovery { get; set; }
        public int Role_Id { get; set; }
        public bool Active { get; set; }
    
        public virtual ICollection<ABSENT_LOG> ABSENT_LOG { get; set; }
        public virtual ICollection<EVENT> EVENT { get; set; }
        public virtual PERSON PERSON { get; set; }
        public virtual ROLE ROLE { get; set; }
    }
}