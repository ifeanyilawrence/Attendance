USE [Attendance]
GO

/****** Object:  View [dbo].[VW_ABSENT_LOG]    Script Date: 2/28/2019 8:02:34 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VW_ABSENT_LOG]
AS
SELECT        AL.Id, P.Last_Name, P.First_Name, ISNULL(P.Other_Name, ' ') AS Other_Name, AL.Absent_Type_Id, AL.Student_Id, AL.Duration_In_Days, AL.Start_Date, AL.End_Date, AL.Approved, AL.Reject_Reason, AL.Remark, AL.User_Id, 
                         S.Matric_Number, S.Hall_Id, S.Active, P.Id AS Person_Id, P.Gender_Id, D.Id AS Department_Id, D.Name AS Department_Name, PE.Id AS Programme_Id, PE.Name AS Programme_Name, L.Id AS Level_Id, L.Name AS Level_Name, 
                         SL.Session_Id, C.Id AS Course_Id, C.Code, C.Name AS CourseName, C.Staff_Id, C.Active AS Activated
FROM            dbo.ABSENT_LOG AS AL INNER JOIN
                         dbo.STUDENT AS S ON S.Person_Id = AL.Student_Id INNER JOIN
                         dbo.PERSON AS P ON P.Id = AL.Student_Id INNER JOIN
                         dbo.STUDENT_LEVEL AS SL ON SL.Student_Id = AL.Student_Id INNER JOIN
                         dbo.EVENT AS E ON E.Id = AL.Event_Id INNER JOIN
                         dbo.COURSE AS C ON C.Id = E.Course_Id INNER JOIN
                         dbo.[LEVEL] AS L ON L.Id = E.Level_Id INNER JOIN
                         dbo.DEPARTMENT AS D ON D.Id = E.Department_Id INNER JOIN
                         dbo.PROGRAMME AS PE ON PE.Id = E.Programme_Id
WHERE        (AL.Approved = 0) AND (C.Active = 1)

GO

