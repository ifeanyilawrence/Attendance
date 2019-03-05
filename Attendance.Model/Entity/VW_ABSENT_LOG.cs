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
    
    public partial class VW_ABSENT_LOG
    {
        public long Id { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Other_Name { get; set; }
        public int Absent_Type_Id { get; set; }
        public long Student_Id { get; set; }
        public int Duration_In_Days { get; set; }
        public System.DateTime Start_Date { get; set; }
        public System.DateTime End_Date { get; set; }
        public bool Approved { get; set; }
        public string Reject_Reason { get; set; }
        public string Remark { get; set; }
        public Nullable<long> User_Id { get; set; }
        public string Matric_Number { get; set; }
        public Nullable<int> Hall_Id { get; set; }
        public bool Active { get; set; }
        public long Person_Id { get; set; }
        public int Gender_Id { get; set; }
        public int Department_Id { get; set; }
        public string Department_Name { get; set; }
        public int Programme_Id { get; set; }
        public string Programme_Name { get; set; }
        public int Level_Id { get; set; }
        public string Level_Name { get; set; }
        public int Session_Id { get; set; }
        public long Course_Id { get; set; }
        public string Code { get; set; }
        public string CourseName { get; set; }
        public Nullable<long> Staff_Id { get; set; }
        public bool Activated { get; set; }
    }
}
