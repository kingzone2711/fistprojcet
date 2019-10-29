using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CM.Models
{
    public class ImportEvent
    {
        public Type eventType { get; set; } = Type.isValid;
        public string courseCode { get; set; }
        public string changeCourseCode { get; set; }
        public bool isErrorCourseCode { get; set; } = false;
        public bool isWarningCourseCode { get; set; } = false;
        public string site { get; set; }
        public bool isErrorSite { get; set; } = false;
        public string courseName { get; set; }
        public bool isErrorCourseName { get; set; } = false;
        public string subjectType { get; set; }
        public bool isErrorSubjectType { get; set; } = false;
        public string subSubjectType { get; set; }
        public bool isErrorSubSubjectType { get; set; } = false;
        public string formatType { get; set; }
        public bool isErrorFormatType { get; set; } = false;
        public string supplierPartner { get; set; }
        public bool isErrorSupplierPartner { get; set; } = false;
        public DateTime? plannedStartDate { get; set; }
        public bool isErrorPlannedStartDate { get; set; } = false;
        public DateTime? plannedEndDate { get; set; }
        public bool isErrorPlannedEndDate { get; set; } = false;
        public double? plannedExpense { get; set; }
        public bool isErrorPlannedExpense { get; set; } = false;
        public string budgetCode { get; set; }
        public DateTime? actualStartDate { get; set; }
        public bool isErrorActualStartDate { get; set; } = false;
        public DateTime? actualEndDate { get; set; }
        public bool isErrorActualEndDate { get; set; } = false;
        public double? actualExpense { get; set; }
        public bool isErrorActualExpense { get; set; } = false;
        public string feedback { get; set; }
        public string feedbackContent { get; set; }
        public string feedbackTeacher { get; set; }
        public string feedbackOrganization { get; set; }
        public string updateBy { get; set; }
        public bool isErrorUpdateBy { get; set; } = false;
        public DateTime updateDate { get; set; }
        public bool isErrorUpdateDate { get; set; } = false;
        public string note { get; set; }
        public string status { get; set; }
        public bool isErrorStatus { get; set; } = false;
        public string faculty { get; set; }
        public bool isErrorFaculty { get; set; } = false;
        public int number { get; set; }
    }
    public enum Type { isValid, isWarning, isError }
        
    
}