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
    
    public partial class ABSENT_LOG
    {
        public long Id { get; set; }
        public int Absent_Type_Id { get; set; }
        public long Student_Id { get; set; }
        public Nullable<int> Duration_In_Days { get; set; }
        public System.DateTime Start_Date { get; set; }
        public System.DateTime End_Date { get; set; }
        public Nullable<bool> Approved { get; set; }
        public string Reject_Reason { get; set; }
        public string Remark { get; set; }
        public Nullable<long> User_Id { get; set; }
        public long Event_Id { get; set; }
    
        public virtual ABSENT_TYPE ABSENT_TYPE { get; set; }
        public virtual STUDENT STUDENT { get; set; }
        public virtual USER USER { get; set; }
        public virtual EVENT EVENT { get; set; }
    }
}
