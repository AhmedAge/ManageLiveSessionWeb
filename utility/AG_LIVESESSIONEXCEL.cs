using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageLiveSessionWeb
{
    public enum GroupDataType
    {
        from1_8,
        Our_Doctors,
        combined,
        projects,
        EMPTY
    }
    public enum SessionType
    {
        Male,
        Female,
        ALL
    };
    public class AG_LIVESESSIONEXCEL
    {
        public string APP_LIVESESSION_NO;
        public string APP_TERM;
        public string APP_DAY;
        public string APP_SESSION_NUMBER;
        public string APP_SESSION_TIME;
        public string STUDIO_NUMBER;
        public string FACULTY_NAME;
        public string GENERAL_MAJOR;
        public string SPECIFIC_MAJOR;
        public string STUDY_LEVEL;
        public string COURSE_NUMBER;
        public string FIRST_SEMESTER_CRN_MALE;
        public string FIRST_SEMESTER_CRN_FEMALE;
        public string SECOND_SEMESTER_CRN_MALE;
        public string SECOND_SEMESTER_CRN_FEMALE;
        public string FEMALE_COURSE_COLLAB_LINK;
        public string MALE_COURSE_COLLAB_LINK;
        public string COURSE_NAME;
        public string STAFF_NAME;
        public string EMAIL;
        public string MOBILE;
        public string STAFF_NUMBER;
        public string EXAMDAY_PERIOD;
        public string APP_DATE;
        public string TeachType;
        public GroupDataType GroupType;
        public SessionType SessionType;
        public string COLLAB_FEMALE_OPEN;
        public string COLLAB_MALE_OPEN;
        public string EVALUATION;
    }
}