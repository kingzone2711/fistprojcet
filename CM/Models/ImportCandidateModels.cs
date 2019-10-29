using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CM.Models
{
    public class ImportCandidate
    {
        public Type candidateType { get; set; } = Type.isValid;
        public string Account { get; set; }
        public bool isErrorAccount { get; set; } = false;
        public int National_id { get; set; }
        public bool isErrorNational_id { get; set; } = false;
        public string CandidateName { get; set; }
        public bool isErrorCandidateName { get; set; } = false;
        public DateTime? DOB { get; set; }
        public bool isErrorDOB { get; set; } = false;
        public string Gender { get; set; }
        public bool isErrorGender { get; set; } = false;
        public string Email { get; set; }
        public bool isErrorEmail { get; set; } = false;
        public string Phone { get; set; }
        public bool isErrorPhone { get; set; } = false;
        public string Facebook { get; set; }
        public bool isErrorFacebook { get; set; } = false;
        public DateTime? GraduationDate { get; set; }
        public bool isErrorGraduationDate { get; set; } = false;
        public int WorkingTimeAvaiable { get; set; }
        public bool isErrorWorkingTimeAvaiable { get; set; } = false;
        public string Skill { get; set; }
        public bool isErrorSkill { get; set; } = false;
        public Double? GPA { get; set; }
        public bool isErrorGPA { get; set; } = false;
        public int Faculty_of_University_id { get; set; }
        public bool isErrorFaculty_of_University_id { get; set; } = false;
        public virtual Faculty_of_University Faculty_Of_University { get; set; }
        public bool isErrorFaculty_of_University { get; set; } = false;
        public virtual ICollection<Candidate_in_Event> Candidate_In_Events { get; set; }
        public bool isErrorCandidate_In_Events { get; set; } = false;
        public virtual ICollection<CandidateHistory> CandidateHistories { get; set; }
        public bool isErrorCandidateHistories { get; set; } = false;


    }


}