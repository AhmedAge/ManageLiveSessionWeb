using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageLiveSessionWeb.utility
{
    public static class Utilities
    {

        public static bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.Session["IsAuthenticated"] == null ? false : Convert.ToBoolean(HttpContext.Current.Session["IsAuthenticated"].ToString());

            }
            set
            {
                HttpContext.Current.Session["IsAuthenticated"] = value;
            }
        }

        public static string Email
        {
            get
            {
                return HttpContext.Current.Session["email"] == null ? "" : HttpContext.Current.Session["email"].ToString();

            }
            set
            {
                HttpContext.Current.Session["email"] = value;
            }
        }

        public static string FullName
        {
            get
            {
                return HttpContext.Current.Session["fullname"] == null ? "" : HttpContext.Current.Session["fullname"].ToString();

            }
            set
            {
                HttpContext.Current.Session["fullname"] = value;
            }
        }

        public static string UserName
        {
            get
            {
                return HttpContext.Current.Session["UserName"] == null ? "" : HttpContext.Current.Session["UserName"].ToString();

            }
            set
            {
                HttpContext.Current.Session["UserName"] = value;
            }
        }

        public static string Mobile
        {
            get
            {
                return HttpContext.Current.Session["Mobile"] == null ? "" : HttpContext.Current.Session["Mobile"].ToString();

            }
            set
            {
                HttpContext.Current.Session["Mobile"] = value;
            }
        }
        public static string DayEn_AR(string dayofWeek)
        {
            switch (dayofWeek)
            {
                case "Saturday":
                    return "السبت";
                case "Sunday":
                    return "الأحد";
                case "Monday":
                    return "الإثنين";
                case "Tuesday":
                    return "الثلاثاء";
                case "Wednesday":
                    return "الأربعاء";
                case "Thursday":
                    return "الخميس";
            }
            return "السبت";
        }

        public static string GetSessionName(int session)
        {
            switch (session)
            {
                case 1:
                    return "Female [5:00 pm , 5:30 pm]";

                case 2:
                    return "Male [5:45 pm , 6:15 pm]";

                case 3:
                    return "Female [6:30 pm , 7:00 pm]";
                case 4:
                    return "Male [7:15 pm , 7:45 pm]";

                case 5:
                    return "Female [8:00 pm , 8:30 pm]";

                case 6:
                    return "Male [8:45 pm , 9:15 pm]";

            }
            return "";
        }


    }
}