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
    
    public partial class STAFF_HALL
    {
        public long Id { get; set; }
        public long Staff_Id { get; set; }
        public int Hall_Id { get; set; }
    
        public virtual HALL HALL { get; set; }
        public virtual STAFF STAFF { get; set; }
    }
}
