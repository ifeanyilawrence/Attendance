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
    
    public partial class ROLE
    {
        public ROLE()
        {
            this.MENU_IN_ROLE = new HashSet<MENU_IN_ROLE>();
            this.USER = new HashSet<USER>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    
        public virtual ICollection<MENU_IN_ROLE> MENU_IN_ROLE { get; set; }
        public virtual ICollection<USER> USER { get; set; }
    }
}
