using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageLiveSessionWeb.utility
{
    public class DoctorEval
    {
        public string user;
        public string term;
        public string liveSession;
        public string info;
        public string coursenumber; 
        public string GeneralNotes; 
        public List<QuestionsInfo> questionsInfo;

    }

    //std+"-"+ value.daynumber + "-" + date.date + "-" + date.dayName
    public class StudioInfo
    {
        public string studio;
        public string daynumber;
        public string date;
        public string dayName;
        public string COURSE_NUMBER;
         
    }

    public class QuestionsInfo
    {
        public string ID;
        public string Yes;
        public string No;
        public string Notes;
    }
}