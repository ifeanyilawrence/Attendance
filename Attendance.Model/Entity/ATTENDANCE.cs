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
    
    public partial class ATTENDANCE
    {
        public long Id { get; set; }
        public long Event_Id { get; set; }
        public long Student_Id { get; set; }
        public System.DateTime Time_Taken { get; set; }
        public bool Status { get; set; }
    
        public virtual EVENT EVENT { get; set; }
        public virtual STUDENT STUDENT { get; set; }
    }
}
