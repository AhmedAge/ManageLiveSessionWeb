using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageLiveSessionWeb.utility
{
    public class UserStudio
    {
        public string user;
        public string term;
        public string liveSession;
        public List<AssignDays> days;
         
    }

    public class AssignDays
    {
        public int daynumber;
        public List<int> studios;
    }
    //public class Studio
    //{
    //    public int studio;
    //}
}