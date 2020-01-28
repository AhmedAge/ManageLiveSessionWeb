using ManageLiveSessionWeb.DAL.D_Admin;
using ReadLiveSessionExcel.utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using ManageLiveSessionWeb.utility;
using Newtonsoft.Json;

namespace ManageLiveSessionWeb.BLL.B_Admin
{
    /// <summary>
    /// Summary description for adminOp
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class adminOp : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string SelectUsers()
        {
            DataSet ds = AdminOperations.SelectUsers();

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sb.Append("<option value='" + dr["USER_ID"] + "'>" + dr["USER_ID"] + " - " + dr["USER_NAME"] + "</option>");
                    }
                    return sb.ToString();
                }
            }
            return string.Empty;

        }

        [WebMethod(EnableSession = true)]
        public string SelectUsersTable()
        {
            DataSet ds = AdminOperations.SelectUsers("ALL");

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string active = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        active = dr["ACTIVE"].ToString() == "Y" ? "background-color:white;" : "background-color:lavenderblush;";
                        sb.Append("<tr style='" + active + "'><td>" + dr["USER_ID"] + "</td><td>" + dr["USER_NAME"] + "</td><td>" + dr["MOBILE"] + "</td><td>" + dr["USER_EMAIL"] + "</td><td>" + dr["ACTIVE"].ToString().Replace("Y", "Yes").Replace("N", "No") + "</td><td><button class='btn btn-primary btn-minier' id='edit" + dr["ID"] + "' data-info='" + dr["ID"] + "&" + dr["USER_ID"] + "&" + dr["USER_NAME"] + "&" + dr["MOBILE"] + "&" + dr["USER_EMAIL"] + "&" + dr["ACTIVE"] + "' onclick='editUser(" + dr["ID"] + ");'>Edit</td><td><button class='btn btn-danger btn-minier' data-info='" + dr["ID"] + "&" + dr["USER_ID"] + "&" + dr["USER_NAME"] + "&" + dr["MOBILE"] + "&" + dr["USER_EMAIL"] + "&" + dr["ACTIVE"] + "' onclick='deleteUser(" + dr["ID"] + ");'>Delete</td></tr>");
                    }
                    return sb.ToString();
                }
            }
            return string.Empty;

        }
        [WebMethod(EnableSession = true)]
        public int SaveUser(User user)
        {
            if (user.ID != 0) //update
            {
                int res = AdminOperations.UpdateUsers(user);
                return res;
            }
            else
            {
                int res = AdminOperations.NewUsers(user);
                if (res == 1)
                    return 2;
                else
                    return -1;
            }

        }

        [WebMethod(EnableSession = true)]
        public int DeleteUser(string ID)
        {
            int res = AdminOperations.DeleteUser(ID);
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string CheckAssignDay(string user_id, string termCode, string livesession, string Day, string SESSION_NO)
        {
            DataSet checkRes = AdminOperations.CheckAssignDay(termCode, livesession, Day, SESSION_NO);
            if (checkRes.Tables.Count > 0)
            {
                if (checkRes.Tables[0].Rows.Count > 0)
                {
                    return "There is another user in that day user: " + checkRes.Tables[0].Rows[0]["USER_ID"].ToString();
                }
            }
            return "0";

        }

        [WebMethod(EnableSession = true)]
        public string AssignDay(string user_id, string termCode, string livesession, string Day, string SESSION_NO)
        {

            DataSet resDs = AdminOperations.AssignDay(user_id, termCode, livesession, Day, SESSION_NO);
            if (resDs.Tables.Count > 0)
            {
                if (resDs.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<thead><tr>")
                              .Append("<th>STD_NUM</th><th>DATE</th><th>LIVESESSION_NO</th><th>DAY</th><th>SESSION_NUM</th><th>SESSION_TIME</th>")
                              .Append("<th>FACULTY_NAME</th><th>COURSE_ID</th><th>GENERAL_MAJOR</th><th>SPECIFIC_MAJOR</th><th>STAFF_NAME</th><th>EMAIL</th><th>MOBILE</th></tr></thead><tbody>");

                    foreach (DataRow dr in resDs.Tables[0].Rows)
                    {
                        sb.Append("<tr><td>" + dr["STUDIO_NUMBER"] + "</td><td>" + String.Format("{0:dd/MM/yyyy}", dr["APP_DATE"]) + "</td><td>" + dr["APP_LIVESESSION_NO"] + "</td><td>" + dr["APP_DAY"] + "</td><td>" + dr["APP_SESSION_NUMBER"].ToString().Replace("session", " الفترة ") + "</td><td>" + dr["APP_SESSION_TIME"] + "</td>")
                           .Append("<td>" + dr["FACULTY_NAME"] + "</td><td>" + dr["COURSE_NUMBER"] + "</td><td>" + dr["GENERAL_MAJOR"] + "</td><td>" + dr["SPECIFIC_MAJOR"] + "</td><td>" + dr["STAFF_NAME"] + "</td><td>" + dr["EMAIL"] + "</td><td>" + dr["MOBILE"] + "</td></tr>");

                        if (Int32.Parse(dr["STUDIO_NUMBER"].ToString()) == 18)
                        {
                            sb.Append("<tr style='background-color:yellow;'><td></td><td></td>" +
                       "<td></td>" +
                       "<td></td><td></td><td></td><td></td>" +
                       "<td></td><td></td>" +
                       "<td></td><td></td><td></td><td></td><td></td></tr>");
                        }

                    }
                    sb.Append("</tbody>");
                    return sb.ToString();
                }
            }
            return "لا يوجد بيانات متاحه";

        }

        [WebMethod(EnableSession = true)]
        public string SearchLiveSession(string user_id, string termCode, string livesession, string Day)
        {
            DataSet resDs = AdminOperations.SearchLiveSession(user_id, termCode, livesession, Day);
            StringBuilder sb = new StringBuilder();
            if (resDs.Tables.Count > 0)
            {
                if (resDs.Tables[0].Rows.Count > 0)
                {

                    sb.Append("<thead><tr>")
                              .Append("<th>USER ID</th><th>STUDIO_NUMBER</th><th>DATE</th><th>LIVESESSION_NO</th><th>DAY</th><th>SESSION_NUMBER</th><th>SESSION_TIME</th>")
                              .Append("<th>FACULTY_NAME</th><th>COURSE_NUMBER</th><th>GENERAL_MAJOR</th><th>SPECIFIC_MAJOR</th><th>STAFF_NAME</th><th>EMAIL</th><th>MOBILE</th></tr></thead><tbody>");

                    foreach (DataRow dr in resDs.Tables[0].Rows)
                    {
                        sb.Append("<tr><td>" + dr["USER_ID"] + "</td><td>" + dr["STUDIO_NUMBER"] + "</td><td>" + String.Format("{0:dd/MM/yyyy}", dr["APP_DATE"]) + "</td><td>" + dr["APP_LIVESESSION_NO"] + "</td><td>" + dr["APP_DAY"] + "</td><td>" + dr["APP_SESSION_NUMBER"].ToString().Replace("session", " الفترة ") + "</td><td>" + dr["APP_SESSION_TIME"] + "</td>")
                           .Append("<td>" + dr["FACULTY_NAME"] + "</td><td>" + dr["COURSE_NUMBER"] + "</td><td>" + dr["GENERAL_MAJOR"] + "</td><td>" + dr["SPECIFIC_MAJOR"] + "</td><td>" + dr["STAFF_NAME"] + "</td><td>" + dr["EMAIL"] + "</td><td>" + dr["MOBILE"] + "</td></tr>");
                        if (Int32.Parse(dr["STUDIO_NUMBER"].ToString()) == 18)
                        {
                            sb.Append("<tr style='background-color:yellow;'><td></td><td></td>" +
                       "<td></td>" +
                       "<td></td><td></td><td></td><td></td>" +
                       "<td></td><td></td>" +
                       "<td></td><td></td><td></td><td></td><td></td></tr>");
                        }
                    }
                    sb.Append("</tbody>");
                    return sb.ToString();
                }
            }
            sb.Append("<thead><tr>")
                              .Append("<th>USER ID</th><th>STUDIO_NUMBER</th><th>DATE</th><th>LIVESESSION_NO</th><th>DAY</th><th>SESSION_NUMBER</th><th>SESSION_TIME</th>")
                              .Append("<th>FACULTY_NAME</th><th>COURSE_NUMBER</th><th>GENERAL_MAJOR</th><th>SPECIFIC_MAJOR</th><th>STAFF_NAME</th><th>EMAIL</th><th>MOBILE</th></tr></thead><tbody>")
                              .Append("<tr><td></td><td></td>" +
                              "<td></td>" +
                              "<td></td><td></td><td></td><td style='font-size:22px;font-weight:bold;'>لا يوجد بيانات متاحه</td>" +
                              "<td></td><td></td>" +
                              "<td></td><td></td><td></td><td></td><td></td></tr></tbody>");

            return sb.ToString();
        }

        [WebMethod(EnableSession = true)]
        public string CreateSearchLiveSession(string user_id, string termCode, string livesession, string Day)
        {
            DataSet resDs = AdminOperations.Create_SearchLiveSession(user_id, termCode, livesession, Day);
            DataSet resCreated = AdminOperations.CreatedLiveSession(user_id, termCode, livesession, Day);

            StringBuilder sb = new StringBuilder();
            if (resDs.Tables.Count > 0)
            {
                if (resDs.Tables[0].Rows.Count > 0)
                {

                    List<DataRow> alreadyCreatedSessions = new List<DataRow>();
                    if (resCreated.Tables.Count > 0)
                    {
                        if (resCreated.Tables[0].Rows.Count > 0)
                        {
                            alreadyCreatedSessions = (from i in resCreated.Tables[0].AsEnumerable() select i).ToList();
                        }
                    }
                    sb.Append("<thead><tr>")
                              .Append("<th>STD_NUM</th><th>USER ID</th><th>LIVE_NO</th><th>DATE</th><th>DAY</th><th>FEMALE</th><th>SESSION</th><th>MALE</th>")
                              .Append("<th>STAFF_NAME</th><th>EMAIL</th><th>MOBILE</th></tr></thead><tbody>");

                    string starsF = "";
                    string starsM = "";

                    int id = 1;
                    foreach (DataRow dr in resDs.Tables[0].Rows)
                    {
                        var d = (from i in alreadyCreatedSessions
                                 where i.Field<string>("USER_ID") == dr["USER_ID"].ToString() &&
                                 i.Field<string>("TERM_CODE") == dr["APP_TERM"].ToString() &&
                                 i.Field<string>("LIVESESSION_NO") == dr["APP_LIVESESSION_NO"].ToString() &&
                                 i.Field<string>("DAY") == dr["APP_DAY"].ToString() &&
                                 i.Field<string>("SESSION_NO") == dr["APP_SESSION_NUMBER"].ToString() &&
                                 i.Field<string>("STUDIO_NO") == dr["STUDIO_NUMBER"].ToString()
                                 select i).ToList();


                        starsF = d.Where(x => x.Field<string>("GENDER") == "female").Count() > 0 ? "<i class='message-star ace-icon fa fa-star orange2'></i> " : "<i class='message-star ace-icon fa fa-star-o light-grey'></i> ";
                        starsM = d.Where(x => x.Field<string>("GENDER") == "male").Count() > 0 ? "<i class='message-star ace-icon fa fa-star orange2'></i> " : "<i class='message-star ace-icon fa fa-star-o light-grey'></i> ";

                        //حاضر العالم الإسلامي-طالبات-المحاضرة المباشرة الرابعـــة- DL-7404-206-Term144010-F
                        //dr["APP_LIVESESSION_NO"].ToString()+"طالبات"+dr["COURSE_NAME"].ToString()+dr["COURSE_NUMBER"].ToString() + "-Term" + termCode + "-F"
                        string sessionNameF = dr["COURSE_NAME"].ToString() + "-طالبات-" + dr["APP_LIVESESSION_NO"].ToString() + "-" + dr["COURSE_NUMBER"].ToString() + "-Term" + termCode + "-F";
                        string sessionNameM = dr["COURSE_NAME"].ToString() + "-طلاب-" + dr["APP_LIVESESSION_NO"].ToString() + "-" + dr["COURSE_NUMBER"].ToString() + "-Term" + termCode + "-M";

                        sb.Append("<tr><td>" + dr["STUDIO_NUMBER"] + "</td><td>" + dr["USER_ID"] + "</td><td>" + dr["APP_LIVESESSION_NO"] + "</td><td>" + String.Format("{0:dd/MM/yyyy}", dr["APP_DATE"]) + "</td><td>" + dr["APP_DAY"] + "</td><td>" +
                          starsF + " <a data-gender='female' title='click to open collaborate and copy this title' data-link='" + dr["FEMALE_COURSE_COLLAB_LINK"] + "' data-sessionno='" + dr["APP_SESSION_NUMBER"] + "' data-studiono='" + dr["STUDIO_NUMBER"] + "' id='addfemale" + id + "' onclick='addEntryFemale(" + id + ")'  >" + sessionNameF + "</a></td><td>" +
                           dr["APP_SESSION_NUMBER"].ToString().Replace("session", " الفترة ") + "</td><td>" +
                          starsM + " <a data-gender='male'  title='click to open collaborate and copy this title' data-link='" + dr["MALE_COURSE_COLLAB_LINK"] + "' data-sessionno='" + dr["APP_SESSION_NUMBER"] + "' data-studiono='" + dr["STUDIO_NUMBER"] + "' id='addmale" + id + "' onclick='addEntryMale(" + id + ")'  >" + sessionNameM + "</a></td>")
                           .Append("<td>" + dr["STAFF_NAME"] + "</td><td>" + dr["EMAIL"] + "</td><td>" + dr["MOBILE"] + "</td></tr>");

                        id++;

                        if (Int32.Parse(dr["STUDIO_NUMBER"].ToString()) == 18)
                        {
                            sb.Append("<tr style='background-color:yellow;'><td></td><td></td>" +
                       "<td></td><td></td><td></td><td></td>" +
                       "<td></td><td></td><td></td><td></td><td></td></tr>");
                        }
                    }
                    sb.Append("</tbody>");
                    return sb.ToString();

                }
            }
            sb.Append("<thead><tr>")
                              .Append("<th>USER ID</th><th>STUDIO_NUMBER</th><th>DATE</th><th>LIVESESSION_NO</th><th>DAY</th><th>Female</th><th>SESSION</th><th>Male</th>")
                              .Append("<th>STAFF_NAME</th><th>EMAIL</th><th>MOBILE</th></tr></thead><tbody>")
                              .Append("<tr><td></td><td></td>" +

                              "<td></td><td></td><td></td><td style='font-size:22px;font-weight:bold;'>لا يوجد بيانات متاحه</td>" +
                              "<td></td><td></td><td></td><td></td><td></td></tr></tbody>");

            return sb.ToString();
        }

        [WebMethod(EnableSession = true)]
        public int AddEntry_CreateLiveSession(string user_id, string termCode, string livesession, string Day, string SESSION_NO, string STUDIO_NO, string GENDER)
        {
            int res = AdminOperations.AddEntry_LiveSession(user_id, termCode, livesession, Day, SESSION_NO, STUDIO_NO, GENDER);

            return res;
        }

        [WebMethod(EnableSession = true)]
        public string ShowUsersDays(string termCode, string livesession)
        {
            DataSet res = AdminOperations.ShowUsersDays(termCode, livesession);
            StringBuilder sb = new StringBuilder();
            if (res.Tables.Count > 0)
            {
                if (res.Tables[0].Rows.Count > 0)
                {
                    List<string> users = new List<string>();

                    users = (from i in res.Tables[0].AsEnumerable() select i.Field<string>("USER_ID")).Distinct().ToList();
                    sb.Append("<thead><tr>")
                          .Append("<th>User/Day</th><th>1</th><th>2</th><th>3</th><th>4</th><th>5</th><th>6</th>")
                          .Append("<th>7</th></tr></thead><tbody>");

                    foreach (string user in users)
                    {
                        sb.Append("<tr><td style='font-size:18px!important;font-weight:bold!important;'>" + user + "</td>");
                        for (int i = 1; i <= 7; i++)
                        {
                            var userSessions = (from u in res.Tables[0].AsEnumerable()
                                                where u.Field<string>("USER_ID") == user
                                                && u.Field<string>("TERM_CODE") == termCode
                                                && u.Field<string>("LIVESESSION_NO") == livesession
                                                && u.Field<string>("DAY") == i.ToString()
                                                select u).ToList();
                            if (userSessions.Count == 0)
                            {
                                sb.Append("<td></td>");
                                continue;
                            }
                            sb.Append("<td>");
                            foreach (DataRow dr in userSessions)
                            {
                                sb.Append(dr["SESSION_NO"].ToString().ToUpper().Replace("SESSION", " الفترة ") + "<br>");
                            }
                            sb.Append("</td>");
                        }
                        sb.Append("</tr>");
                    }
                    return sb.ToString();
                }
            }
            return "";
        }

        [WebMethod(EnableSession = true)]
        public string User_days(string user_id, string termCode, string livesession)
        {
            DataSet res = AdminOperations.ShowSingleUserDays(user_id, termCode, livesession);
            StringBuilder sb = new StringBuilder();
            if (res.Tables.Count > 0)
            {
                if (res.Tables[0].Rows.Count > 0)
                {
                    var days = (from i in res.Tables[0].AsEnumerable()
                                select i.Field<string>("DAY")).Distinct().ToList();

                    foreach (string dr in days)
                    {
                        sb.Append("<option value='" + dr + "'>" + dr + "</option>");
                    }
                    return sb.ToString();
                }
            }
            return "";
        }

        private int studiosCount = 18;
        private int sessionsCount = 6;

        [WebMethod(EnableSession = true)]
        public string LiveSessionTable(string termCode, string livesession)
        {
            List<AG_LIVESESSIONEXCEL> liveSessionLst = AdminOperations.SelectLiveSessionTable(termCode, livesession);
            List<Days> days = new List<Days>();

            for (int i = 0; i < 7; i++)
            {
                Days d = new Days();

                List<AG_LIVESESSIONEXCEL> liveDay = liveSessionLst.Where(x => x.APP_DAY == (i + 1).ToString()).ToList();

                d.DayNumber = Convert.ToInt32(liveDay[i].APP_DAY);
                d.DayDate = Convert.ToDateTime(liveDay[i].APP_DATE);
                d.DayNameAr = Utilities.DayEn_AR(Convert.ToDateTime(liveDay[i].APP_DATE).DayOfWeek.ToString());
                d.DayNameEn = Convert.ToDateTime(liveDay[i].APP_DATE).DayOfWeek.ToString();

                for (int studio = 0; studio < studiosCount; studio++)
                {
                    for (int session = 0; session < sessionsCount; session++)
                    {
                        Session s = new Session();

                        s.liveSession = new AG_LIVESESSIONEXCEL();
                        d.sessions[session, studio] = s;

                        var live = liveDay.Where(x => x.APP_SESSION_NUMBER.Replace("session", string.Empty).ToString() == (session + 1).ToString() && x.STUDIO_NUMBER == (studio + 1).ToString()).FirstOrDefault();

                        d.sessions[session, studio].liveSession.APP_DATE = live.APP_DATE;
                        d.sessions[session, studio].liveSession.APP_DAY = live.APP_DAY;
                        d.sessions[session, studio].liveSession.APP_LIVESESSION_NO = live.APP_LIVESESSION_NO;
                        d.sessions[session, studio].liveSession.APP_SESSION_NUMBER = live.APP_SESSION_NUMBER;
                        d.sessions[session, studio].liveSession.APP_SESSION_TIME = live.APP_SESSION_TIME;
                        d.sessions[session, studio].liveSession.APP_TERM = live.APP_TERM;
                        d.sessions[session, studio].liveSession.COURSE_NAME = live.COURSE_NAME;
                        d.sessions[session, studio].liveSession.COURSE_NUMBER = live.COURSE_NUMBER;
                        d.sessions[session, studio].liveSession.EMAIL = live.EMAIL;
                        d.sessions[session, studio].liveSession.EXAMDAY_PERIOD = live.EXAMDAY_PERIOD;
                        d.sessions[session, studio].liveSession.FACULTY_NAME = live.FACULTY_NAME;
                        d.sessions[session, studio].liveSession.FEMALE_COURSE_COLLAB_LINK = live.FEMALE_COURSE_COLLAB_LINK;
                        d.sessions[session, studio].liveSession.FIRST_SEMESTER_CRN_FEMALE = live.FIRST_SEMESTER_CRN_FEMALE;
                        d.sessions[session, studio].liveSession.FIRST_SEMESTER_CRN_MALE = live.FIRST_SEMESTER_CRN_MALE;
                        d.sessions[session, studio].liveSession.GENERAL_MAJOR = live.GENERAL_MAJOR;
                        d.sessions[session, studio].liveSession.GroupType = live.GroupType;
                        d.sessions[session, studio].liveSession.MALE_COURSE_COLLAB_LINK = live.MALE_COURSE_COLLAB_LINK;
                        d.sessions[session, studio].liveSession.MOBILE = live.MOBILE;
                        d.sessions[session, studio].liveSession.SECOND_SEMESTER_CRN_FEMALE = live.SECOND_SEMESTER_CRN_FEMALE;
                        d.sessions[session, studio].liveSession.SECOND_SEMESTER_CRN_MALE = live.SECOND_SEMESTER_CRN_MALE;
                        d.sessions[session, studio].liveSession.SessionType = live.SessionType;
                        d.sessions[session, studio].liveSession.SPECIFIC_MAJOR = live.SPECIFIC_MAJOR;
                        d.sessions[session, studio].liveSession.STAFF_NAME = live.STAFF_NAME;
                        d.sessions[session, studio].liveSession.STAFF_NUMBER = live.STAFF_NUMBER;
                        d.sessions[session, studio].liveSession.STUDIO_NUMBER = live.STUDIO_NUMBER;
                        d.sessions[session, studio].liveSession.STUDY_LEVEL = live.STUDY_LEVEL;
                        d.sessions[session, studio].liveSession.TeachType = live.TeachType;

                    }
                }
                days.Add(d);
            }

            StringBuilder sb = new StringBuilder();
            int Count = 0;
            int button = 1;
            foreach (Days d in days)
            {
                sb.Append("<table  id='table" + (Count + 1) + "'><tr style='background-color:yellow;font-weight:bold;'><td colspan=7>Day: " + d.DayNumber + "\t&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy}", d.DayDate.Date) + "\t&nbsp;&nbsp;" + d.DayNameEn + "\t&nbsp;&nbsp;" + d.DayNameAr + "</td></tr>");

                sb.Append("<tr style='font-weight:bold;'><td style='width:10%!important;'></td> ");
                sb.Append("<td colspan=2 >Session 1</td> <td colspan=2>Session 2</td> <td colspan=2>Session 3</td></tr>");

                sb.Append("<tr><td style='width:10%!important;'>الوقت <br> رقم الاستوديو </td> ");
                sb.Append("<td>Female [5:00 pm , 5:30 pm] </td> <td> Male [5:45 pm , 6:15 pm]</td> <td>Female [6:30 pm , 7:00 pm] </td> <td> Male [7:15 pm , 7:45 pm]</td> <td>Female [8:00 pm , 8:30 pm] </td> <td> Male [8:45 pm , 9:15 pm]</td></tr>");
              
                for (int studio = 0; studio < studiosCount; studio++)
                {
                    sb.Append("<tr>");
                    sb.Append("<td style='width:10%!important;'> " + (studio + 1) + "</td>");
                    for (int session = 0; session < sessionsCount; session++)
                    {
                        string color = "";
                        switch (session)
                        {
                            case 0:
                            case 1:
                                color = "aliceblue";
                                break;
                            case 2:
                            case 3:
                                color = "white";
                                break;
                            case 4:
                            case 5:
                                color = "antiquewhite";
                                break;
                        }
                        if (d.sessions[session, studio].liveSession.GroupType == GroupDataType.projects)
                        {
                            if (d.sessions[session, studio].liveSession.SessionType == SessionType.Female)
                            {
                                sb.Append("<td style='width:15%!important;'> <table style='border-collapse: collapse!important;width:100%;text-align: center;background-color:" + color + "'><tbody><tr><td>" + d.sessions[session, studio].liveSession.COURSE_NUMBER + "</td>");
                                sb.Append("<td>" + d.sessions[session, studio].liveSession.COURSE_NAME +
                                      "<button id='delbtn" + button + "'  class='btn btn-minier btn-danger pull-right' " +
                                  "onclick='Delete(" + button + ")'  '" + Data(d.sessions[session, studio].liveSession) + "'><i class='ace-icon fa fa fa-trash'></i></button>" +
                                          "<button id='editbtn" + button + "'   class='btn btn-minier btn-warning pull-right' " +
                                  "onclick='Edit(" + button + ")'  '" + Data(d.sessions[session, studio].liveSession) + "'><i class='ace-icon fa fa-pencil-square-o'></i></button>" +
                                    " </td></tr>");
                                sb.Append("<tr><td>" + d.sessions[session, studio].liveSession.STAFF_NAME + "</td>");
                                sb.Append("<td>" + d.sessions[session, studio].liveSession.FACULTY_NAME + "</td></tr>");
                                sb.Append("<tr><td><b>" + d.sessions[session, studio].liveSession.STUDY_LEVEL + "</b></td>");
                                sb.Append("<td><b>" + d.sessions[session, studio].liveSession.SPECIFIC_MAJOR + "</b></td></tr>");
                                sb.Append("<tr><td colspan=2><b>" + " طالبات - " + d.sessions[session, studio].liveSession.GENERAL_MAJOR + "</b></td></tr></tbody></table></td>");
                            }
                            else if (d.sessions[session, studio].liveSession.SessionType == SessionType.Male)
                            {
                                sb.Append("<td style='width:15%!important;'> <table style='border-collapse: collapse!important;width: 100%;text-align: center;background-color:" + color + "'><tbody><tr><td>" + d.sessions[session, studio].liveSession.COURSE_NUMBER + "</td>");
                                sb.Append("<td>" + d.sessions[session, studio].liveSession.COURSE_NAME +
                                    "<button id='delbtn" + button + "'  class='btn btn-minier btn-danger pull-right' " +
                                "onclick='Delete(" + button + ")'  '" + Data(d.sessions[session, studio].liveSession) + "'><i class='ace-icon fa fa fa-trash'></i></button>" +
                                        "<button id='editbtn" + button + "'   class='btn btn-minier btn-warning pull-right' " +
                               "onclick='Edit(" + button + ")'  '" + Data(d.sessions[session, studio].liveSession) + "'><i class='ace-icon fa fa-pencil-square-o'></i></button>" +
                                  " </td></tr>");
                                sb.Append("<tr><td>" + d.sessions[session, studio].liveSession.STAFF_NAME + "</td>");
                                sb.Append("<td>" + d.sessions[session, studio].liveSession.FACULTY_NAME + "</td></tr>");
                                sb.Append("<tr><td><b>" + d.sessions[session, studio].liveSession.STUDY_LEVEL + "</b></td>");
                                sb.Append("<td><b>" + d.sessions[session, studio].liveSession.SPECIFIC_MAJOR + "</b></td></tr>");
                                sb.Append("<tr><td  colspan=2><b>" + " طلاب - " + d.sessions[session, studio].liveSession.GENERAL_MAJOR + "</b></td></tr></tbody></table></td>");
                            }
                            else if (d.sessions[session, studio].liveSession.SessionType == SessionType.ALL)
                            {
                                sb.Append("<td colspan=2 style='width:30%!important;'> <table style='border-collapse: collapse!important;width: 100%;text-align: center;background-color:" + color + "'><tbody><tr><td>" + d.sessions[session, studio].liveSession.COURSE_NUMBER + "</td>");
                                sb.Append("<td>" + d.sessions[session, studio].liveSession.COURSE_NAME +
                                       "<button id='delbtn" + button + "'  class='btn btn-minier btn-danger pull-right' " +
                                   "onclick='Delete(" + button + ")'  '" + Data(d.sessions[session, studio].liveSession) + "'><i class='ace-icon fa fa fa-trash'></i></button>" +
                                          "<button id='editbtn" + button + "'   class='btn btn-minier btn-warning pull-right' " +
                                   "onclick='Edit(" + button + ")'  '" + Data(d.sessions[session, studio].liveSession) + "'><i class='ace-icon fa fa-pencil-square-o'></i></button>" +
                                     " </td></tr>");
                                sb.Append("<tr><td>" + d.sessions[session, studio].liveSession.STAFF_NAME + "</td>");
                                sb.Append("<td>" + d.sessions[session, studio].liveSession.FACULTY_NAME + "</td></tr>");
                                sb.Append("<tr><td><b>" + d.sessions[session, studio].liveSession.STUDY_LEVEL + "</b></td>");
                                sb.Append("<td><b>" + d.sessions[session, studio].liveSession.SPECIFIC_MAJOR + "</b></td></tr>");
                                sb.Append("<tr><td><b>" + d.sessions[session, studio].liveSession.GENERAL_MAJOR + "</b></td></tr></tbody></table></td>");
                            }
                        }
                        else
                        {
                            if (d.sessions[session, studio].liveSession.SessionType == SessionType.Female)
                            {
                                sb.Append("<td style='width:15%!important;'> <table style='border-collapse: collapse!important;width: 100%;text-align: center;background-color:" + color + "'><tbody><tr><td>" + d.sessions[session, studio].liveSession.COURSE_NUMBER + "</td>");
                                sb.Append("<td>" + d.sessions[session, studio].liveSession.COURSE_NAME +
                                     "<button id='delbtn" + button + "'  class='btn btn-minier btn-danger pull-right' " +
                                 "onclick='Delete(" + button + ")'  '" + Data(d.sessions[session, studio].liveSession) + "'><i class='ace-icon fa fa fa-trash'></i></button>" +
                                         "<button id='editbtn" + button + "'   class='btn btn-minier btn-warning pull-right' " +
                                 "onclick='Edit(" + button + ")'  '" + Data(d.sessions[session, studio].liveSession) + "'><i class='ace-icon fa fa-pencil-square-o'></i></button>" +
                                   " </td></tr>");
                                sb.Append("<tr><td>" + d.sessions[session, studio].liveSession.STAFF_NAME + "</td>");
                                sb.Append("<td>" + d.sessions[session, studio].liveSession.FACULTY_NAME + "</td></tr>");
                                sb.Append("<tr><td><b>" + d.sessions[session, studio].liveSession.STUDY_LEVEL + "</b></td>");
                                sb.Append("<td><b>" + d.sessions[session, studio].liveSession.SPECIFIC_MAJOR + "</b></td></tr>");
                                sb.Append("<tr><td colspan=2><b> " + " طالبات - " + d.sessions[session, studio].liveSession.GENERAL_MAJOR + "</b></td></tr></tbody></table></td>");
                                // sb.Append("<td></td>");
                                //sb.Append("<td style='background-color:yellow;'>EMPTY</td>");

                            }
                            else if (d.sessions[session, studio].liveSession.SessionType == SessionType.Male)
                            {
                                //  sb.Append("<td></td>");
                                //sb.Append("<td style='background-color:yellow;'>EMPTY</td>");
                                sb.Append("<td  style='width:15%!important;'> <table style='border-collapse: collapse!important;width: 100%;text-align: center;background-color:" + color + "'><tbody><tr><td>" + d.sessions[session, studio].liveSession.COURSE_NUMBER + "</td>");
                                sb.Append("<td>" + d.sessions[session, studio].liveSession.COURSE_NAME +
                                      "<button id='delbtn" + button + "'  class='btn btn-minier btn-danger pull-right' " +
                                  "onclick='Delete(" + button + ")'  '" + Data(d.sessions[session, studio].liveSession) + "'><i class='ace-icon fa fa fa-trash'></i></button>" +
                                          "<button id='editbtn" + button + "'   class='btn btn-minier btn-warning pull-right' " +
                                  "onclick='Edit(" + button + ")'  '" + Data(d.sessions[session, studio].liveSession) + "'><i class='ace-icon fa fa-pencil-square-o'></i></button>" +
                                    " </td></tr>");
                                sb.Append("<tr><td>" + d.sessions[session, studio].liveSession.STAFF_NAME + "</td>");
                                sb.Append("<td>" + d.sessions[session, studio].liveSession.FACULTY_NAME + "</td></tr>");
                                sb.Append("<tr><td><b>" + d.sessions[session, studio].liveSession.STUDY_LEVEL + "</b></td>");
                                sb.Append("<td><b>" + d.sessions[session, studio].liveSession.SPECIFIC_MAJOR + "</b></td></tr>");
                                sb.Append("<tr><td colspan=2><b>" + " طلاب - " + d.sessions[session, studio].liveSession.GENERAL_MAJOR + "</b></td></tr></tbody></table></td>");
                            }
                            else if (d.sessions[session, studio].liveSession.SessionType == SessionType.ALL && !string.IsNullOrEmpty(d.sessions[session, studio].liveSession.COURSE_NAME))
                            {
                                sb.Append("<td colspan=2  style='width:30%!important;'> <table style='border-collapse: collapse!important;width: 100%;text-align: center;background-color:" + color + "'><tbody><tr><td>" + d.sessions[session, studio].liveSession.COURSE_NUMBER + "</td>");
                                sb.Append("<td>" + d.sessions[session, studio].liveSession.COURSE_NAME +
                                      "<button id='delbtn" + button + "'  class='btn btn-minier btn-danger pull-right' " +
                                  "onclick='Delete(" + button + ")'  '" + Data(d.sessions[session, studio].liveSession) + "'><i class='ace-icon fa fa fa-trash'></i></button>" +
                                          "<button id='editbtn" + button + "'   class='btn btn-minier btn-warning pull-right' " +
                                  "onclick='Edit(" + button + ")'  '" + Data(d.sessions[session, studio].liveSession) + "'><i class='ace-icon fa fa-pencil-square-o'></i></button>" +
                                    " </td></tr>");
                                sb.Append("<tr><td>" + d.sessions[session, studio].liveSession.STAFF_NAME + "</td>");
                                sb.Append("<td>" + d.sessions[session, studio].liveSession.FACULTY_NAME + "</td></tr>");
                                sb.Append("<tr><td><b>" + d.sessions[session, studio].liveSession.STUDY_LEVEL + "</b></td>");
                                sb.Append("<td><b>" + d.sessions[session, studio].liveSession.SPECIFIC_MAJOR + "</b></td></tr>");
                                sb.Append("<tr><td  colspan=2><b>" + d.sessions[session, studio].liveSession.GENERAL_MAJOR + "</b></td></tr></tbody></table></td>");
                                session++;
                            }
                            else
                            {
                                sb.Append("<td style='background-color:yellow;'>EMPTY</td>");
                            }


                        }
                        button++;
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</table><hr>");
            }






            return sb.ToString();
        }

        private string Data(AG_LIVESESSIONEXCEL liveSession)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" data-term='" + liveSession.APP_TERM + "' ");//144010
            sb.Append(" data-livesessionno='" + liveSession.APP_LIVESESSION_NO + "' ");//المحاضرة المباشرة الأولي
            sb.Append(" data-livesessionnumber='" + liveSession.APP_SESSION_NUMBER + "' ");//session1
            sb.Append(" data-appday='" + liveSession.APP_DAY + "' ");// 1    OR 2 or ....    7
            sb.Append(" data-sessiontime='" + liveSession.APP_SESSION_TIME + "' ");//Female [8:00 pm , 8:30 pm] OR Male [8:45 pm , 9:15 pm] ...
            sb.Append(" data-coursenumber='" + liveSession.COURSE_NUMBER + "' "); //DL-0608-201
            sb.Append(" data-staffname='" + liveSession.STAFF_NAME + "' ");//د. فارس العصيمي
            sb.Append(" data-studionumber='" + liveSession.STUDIO_NUMBER + "' ");//1-18
            sb.Append(" data-studylevel='" + liveSession.STUDY_LEVEL + "' ");//1... 8 .....
            sb.Append(" data-specificmajor='" + liveSession.SPECIFIC_MAJOR + "' ");//ترخ سلم ....
            sb.Append(" data-generalmajor='" + liveSession.GENERAL_MAJOR + "' ");//إدارة ....
            sb.Append(" data-email='" + liveSession.EMAIL + "' ");//email ....
            sb.Append(" data-sessiontype='" + liveSession.SessionType + "' ");//SessionType: ALL, Male,Female ....
            sb.Append(" data-grouptype='" + liveSession.GroupType + "' ");//grouptype: 1_8 , combined, Our_doctors, projects ....
            sb.Append(" data-coursename='" + liveSession.COURSE_NAME + "' ");
            sb.Append(" data-appdate='" + String.Format("{0:dd/MM/yyyy}", liveSession.APP_DATE) + "' ");
           // sb.Append(" data-colabf='" + liveSession.FEMALE_COURSE_COLLAB_LINK + "' ");
           // sb.Append(" data-colabm='" + liveSession.MALE_COURSE_COLLAB_LINK + "' ");


            return sb.ToString();
        }


        [WebMethod(EnableSession = true)]
        public string AssignUserStudios(UserStudio userstudios)
        {
            if (userstudios.days.Count > 0)
            {
                int res = AdminOperations.AssignUserStudios(userstudios);
                if (res > 0)
                    return "تم تخصيص الايام و الاستوديوهات للمستخدم بنجاح";
            }
            return "برجاء اختيار التيرم و المحاضرة المباشرة و المستخدم و من ثم اختر الايام و الاستوديوهات بكل يوم";


        }
         
        [WebMethod(EnableSession = true)]
        public string UserStudios(string user, string term, string liveSession)
        {
            DataSet res = AdminOperations.UserStudios(user, term, liveSession);

            UserStudio us = new UserStudio();
            us.liveSession = liveSession;
            us.term = term;
            us.user = user;
            List<AssignDays> assignDays = new List<AssignDays>();

            AssignDays singleDay = null;
            if (res.Tables.Count > 0)
            {
                if (res.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in res.Tables[0].Rows)
                    {
                        singleDay = new AssignDays();
                        singleDay.daynumber = Convert.ToInt32(dr["DAY"]);
                        string[] stdstr = dr["STUDIOS"].ToString().Split(',');
                        List<int> stdlst = new List<int>();

                        foreach (string str in stdstr)
                        {
                            stdlst.Add(Convert.ToInt32(str));
                        }
                        singleDay.studios = stdlst;

                        assignDays.Add(singleDay);
                    }

                    us.days = assignDays;

                    string data = JsonConvert.SerializeObject(us);
                    return data;
                }
            }
            return "";

        }


        [WebMethod(EnableSession = true)]
        public string LiveSessionDates(string term, string liveSession)
        {
            DataSet res = AdminOperations.LiveSessionDates(term, liveSession);
            LiveDates liveDates = null;
            List<LiveDates> liveDatesLst = new List<LiveDates>();

            if (res.Tables.Count > 0)
            {
                if (res.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in res.Tables[0].Rows)
                    {
                        liveDates = new LiveDates();
                        liveDates.day = Convert.ToInt32(dr["APP_DAY"]);
                        liveDates.date = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["APP_DATE"]));
                        liveDates.dayName = Utilities.DayEn_AR(Convert.ToDateTime(dr["APP_DATE"]).DayOfWeek.ToString());

                        liveDatesLst.Add(liveDates);
                    }
                    string data = JsonConvert.SerializeObject(liveDatesLst);
                    return data;

                }
            }
            return "لا يوجد تواريخ";

        }

        private class Eval
        {
            public string QUESTIONTEXT;
            public string QUESTIONID;
            public string ANSWER;
            public string NOTES;

        }

        //term: data.term, liveSession: data.liveSession, stdInfo: data.info, course_doctor:  c
        [WebMethod(EnableSession = true)]
        public string EvalQuestions(string term, string liveSession, string stdInfo, string course_doctor)
        {
            DataSet res = AdminOperations.EvalQuestions(term, liveSession, stdInfo, course_doctor);
            if (res.Tables.Count > 0)
            {
                if (res.Tables[0].Rows.Count > 0)
                {

                    List<Eval> questions = new List<Eval>();
                    Eval eval = null;
                    foreach (DataRow dr in res.Tables[0].Rows)
                    {
                        eval = new Eval();
                        eval.QUESTIONID = dr["ID"].ToString();
                        eval.QUESTIONTEXT = dr["QUESTIONTEXT"].ToString();
                        eval.ANSWER = dr["ANSWER"].ToString();
                        eval.NOTES = dr["NOTES"].ToString();

                        questions.Add(eval);

                    }
                    string data = JsonConvert.SerializeObject(questions);
                    return data;
                }
            }
            return "";
        }
         
        [WebMethod(EnableSession = true)]
        public string DayStudioSessions(string term, string liveSession, string stdInfo)
        {
            DataSet res = AdminOperations.DayStudioSessions(term, liveSession, stdInfo);
            if (res.Tables.Count > 0)
            {
                if (res.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (DataRow dr in res.Tables[0].Rows)
                    {
                        sb.Append("<option value='" + dr["COURSE_NUMBER"] + "#" + dr["EMAIL"].ToString().Replace("@kfu.edu.sa", string.Empty) + "'>" + dr["STAFF_NAME"] + " - " + dr["COURSE_NAME"] + " - " + dr["FACULTY_NAME"] + "</option>");
                    }
                    return sb.ToString();
                }
            }
            return "";
        }

        //user: data.user, term: data.term, liveSession: data.liveSession, info: data.info, GeneralNotes: $("#GeneralNotes").val(), questions: questionsAns
        [WebMethod(EnableSession = true)]
        public string DoctorEvaluation(DoctorEval answer)
        {
            try
            {
                int res = AdminOperations.DoctorEvaluation(answer);
                if (res > 0)
                    return "تم اضافة التقييم بنجاح";
                return "حدث خطأ";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }

        [WebMethod(EnableSession = true)]
        public string UserStudiosView(string term, string liveSession)
        {
            try
            {
                DataSet res = AdminOperations.UserStudiosView(term, liveSession);
                if (res.Tables.Count > 0)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                     
                        foreach (DataRow dr in res.Tables[0].Rows)
                        { 
                            sb.Append("<tr><td>"+ dr["USER_ID"] +"</td>");
                            sb.Append("<td>" + dr["DAY"] + "</td>");
                            sb.Append("<td>" + dr["STUDIOS"] + "</td></tr>"); 
                        }
                        return sb.ToString();
                    }
                }
                return "لا توجد بيانات متاحه";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        public string EmptyLiveSessionStudio(AG_LIVESESSIONEXCEL liveSession)
        {
            try
            {
                int res = AdminOperations.EmptyLiveSessionStudio(liveSession);
                if (res > 0)
                    return "تم حذف المحاضرة المباشرة بنجاح  ";
                return "حدث خطأ";
            }
            catch (Exception ex)
            {
                return ex.Message;
            } 
        }

        [WebMethod(EnableSession = true )]
        public string ShowRequests(string term, string liveSession)
        {
            DataSet res = AdminOperations.ShowRequests( term, liveSession);
            if (res.Tables.Count > 0)
            {
                if (res.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div class='table-responsive'><table class='table table-bordered'><thead><tr> <td>اليوم</td> <td>التاريخ</td> <td>الفترة</td> <td>الاستوديو</td> <td>الكلية</td> <td>المستوي</td> <td>رقم المقرر</td> <td>المقرر</td><td>الدكتور</td>" +
                        " <td>الجوال</td>  <td>سبب التعديل</td> <td>اليوم المطلوب</td> <td>التاريخ المطلوب</td><td>الفترة</td><td>الاستوديو</td><td>قبول</td><td>رفض</td></tr> </thead><tbody>");
                    int button = 1;
                    for (int i =0;i< res.Tables[0].Rows.Count ;i++)
                    {
                        DataRow dr = res.Tables[0].Rows[i];

                     
                        if (dr["SESSIONTYPE"].ToString() == "ALL")
                        { 
                            i++;
                        } 
                        sb.Append("<tr><td>" + dr["APP_DAY"] + "</td>");
                        sb.Append("<td>" + string.Format("{0:dd/MM/yyyy}", dr["APP_DATE"] ) + "</td>");
                        sb.Append("<td>" + GetSessionNumber( dr["APP_SESSION_NUMBER"].ToString()) + "</td>"); 
                        sb.Append("<td>" + dr["STUDIO_NUMBER"] + "</td>");
                        sb.Append("<td>" + dr["GENERAL_MAJOR"] + "</td>");
                        //sb.Append("<td>" + dr["SPECIFIC_MAJOR"] + "</td>");
                        sb.Append("<td>" + dr["STUDY_LEVEL"] + "</td>");
                        sb.Append("<td>" + dr["COURSE_NUMBER"] + "</td>");
                        sb.Append("<td>" + dr["COURSE_NAME"] + "</td>");
                        sb.Append("<td>" + dr["STAFF_NAME"] + "</td>");
                        sb.Append("<td>" + dr["MOBILE"] + "</td>");
                        sb.Append("<td><button id='downloadbtn" + button + "'  class='btn btn-minier btn-link' " +
                                  "onclick='Download(" + button + ")'  " + DataApprove(dr) + " ><i class='ace-icon fa fa fa fa-download'></i> " + dr["NEW_NOTES"]+"</button></td>");

                        sb.Append("<td style='background-color:aliceblue;'>" + dr["NEW_APP_DAY"] + "</td>");
                        sb.Append("<td style='background-color:aliceblue;'>" + string.Format("{0:dd/MM/yyyy}", dr["NEW_APP_DATE"]) + "</td>");
                        sb.Append("<td style='background-color:aliceblue;'>" + GetSessionNumber(dr["NEW_APP_SESSION_NUMBER"].ToString())  + "</td>"); 
                        sb.Append("<td style='background-color:aliceblue;'>" + dr["NEW_STUDIO_NUMBER"] + "</td>");
                     

                        sb.Append("<td style='background-color:aliceblue;'><button id='approvebtn" + button + "'  class='btn btn-minier btn-success' " +
                                  "onclick='Approve(" + button + ")'  " + DataApprove(dr) + " ><i class='ace-icon fa fa fa fa-check'></i> قبول</button></td>");
                        sb.Append("<td style='background-color:aliceblue;'><button id='rejectbtn" + button + "'  class='btn btn-minier btn-danger' " +
                                "onclick='Approve(" + button + ")'  " + DataApprove(dr) + " ><i class='ace-icon fa fa fa fa-ban'></i> رفض</button></td>");

                        sb.Append("</tr>");
                        button++;

                    }
                    sb.Append("</tbody></table></div>");
                    return sb.ToString();
                }
            }
            return "<h2>لا توجد بيانات متاحه</h2>";
                                                                                      
        }

        private string DataApprove(DataRow dr)
        {
            ChangeLiveSession liveSession = new ChangeLiveSession();
            liveSession.NEW_STUDY_LEVEL = dr["NEW_STUDY_LEVEL"].ToString();
            liveSession.NEW_SPECIFIC_MAJOR = dr["NEW_SPECIFIC_MAJOR"].ToString();
            liveSession.NEW_GENERAL_MAJOR = dr["NEW_GENERAL_MAJOR"].ToString();
            liveSession.NEW_FACULTY_NAME = dr["NEW_FACULTY_NAME"].ToString();
            liveSession.NEW_STUDIO_NUMBER = dr["NEW_STUDIO_NUMBER"].ToString();
            liveSession.NEW_APP_SESSION_TIME = dr["NEW_APP_SESSION_TIME"].ToString();
            liveSession.NEW_APP_SESSION_NUMBER = dr["NEW_APP_SESSION_NUMBER"].ToString();
            liveSession.NEW_APP_DAY = dr["NEW_APP_DAY"].ToString();
            liveSession.NEW_APP_DATE = Convert.ToDateTime(dr["NEW_APP_DATE"].ToString());
            liveSession.TEACHTYPE = dr["TEACHTYPE"].ToString();
            liveSession.SESSIONTYPE = dr["SESSIONTYPE"].ToString();
            liveSession.GROUPTYPE = dr["GROUPTYPE"].ToString();
            liveSession.EXAMDAY_PERIOD = dr["EXAMDAY_PERIOD"].ToString();
            liveSession.STAFF_NUMBER = dr["STAFF_NUMBER"].ToString();
            liveSession.MOBILE = dr["MOBILE"].ToString();
            liveSession.EMAIL = dr["EMAIL"].ToString();
            liveSession.STAFF_NAME = dr["STAFF_NAME"].ToString();
            liveSession.COURSE_NAME = dr["COURSE_NAME"].ToString();
            liveSession.MALE_COURSE_COLLAB_LINK = dr["MALE_COURSE_COLLAB_LINK"].ToString();
            liveSession.FEMALE_COURSE_COLLAB_LINK = dr["FEMALE_COURSE_COLLAB_LINK"].ToString();
            liveSession.SECOND_SEMESTER_CRN_FEMALE = dr["SECOND_SEMESTER_CRN_FEMALE"].ToString();
            liveSession.SECOND_SEMESTER_CRN_MALE = dr["SECOND_SEMESTER_CRN_MALE"].ToString();
            liveSession.FIRST_SEMESTER_CRN_FEMALE = dr["FIRST_SEMESTER_CRN_FEMALE"].ToString();
            liveSession.FIRST_SEMESTER_CRN_MALE = dr["FIRST_SEMESTER_CRN_MALE"].ToString();
            liveSession.COURSE_NUMBER = dr["COURSE_NUMBER"].ToString();
            liveSession.STUDY_LEVEL = dr["STUDY_LEVEL"].ToString();
            liveSession.SPECIFIC_MAJOR = dr["SPECIFIC_MAJOR"].ToString();
            liveSession.GENERAL_MAJOR = dr["GENERAL_MAJOR"].ToString();
            liveSession.FACULTY_NAME = dr["FACULTY_NAME"].ToString();
            liveSession.STUDIO_NUMBER = dr["STUDIO_NUMBER"].ToString();
            liveSession.APP_SESSION_TIME = dr["APP_SESSION_TIME"].ToString();
            liveSession.APP_SESSION_NUMBER = dr["APP_SESSION_NUMBER"].ToString();
            liveSession.APP_DAY = dr["APP_DAY"].ToString();
            liveSession.APP_DATE = Convert.ToDateTime( dr["APP_DATE"].ToString());
            liveSession.APP_LIVESESSION_NO = dr["APP_LIVESESSION_NO"].ToString();
            liveSession.APP_TERM = dr["APP_TERM"].ToString();

            liveSession.NEW_WITHIN_3_DAYS = dr["NEW_WITHIN_3_DAYS"].ToString();
            liveSession.NEW_IS_ALREADY_EMPTY = dr["NEW_IS_ALREADY_EMPTY"].ToString();
            liveSession.NEW_CONFIRM_STATUS = dr["NEW_CONFIRM_STATUS"].ToString();
            liveSession.NEW_DOC_PATH = dr["NEW_DOC_PATH"].ToString();
            liveSession.NEW_NOTES = dr["NEW_NOTES"].ToString();
            liveSession.NEW_TEACHTYPE = dr["NEW_TEACHTYPE"].ToString();
            liveSession.NEW_SESSIONTYPE = dr["NEW_SESSIONTYPE"].ToString();
            liveSession.NEW_GROUPTYPE = dr["NEW_GROUPTYPE"].ToString();
            liveSession.NEW_EXAMDAY_PERIOD = dr["NEW_EXAMDAY_PERIOD"].ToString();
            liveSession.NEW_STAFF_NUMBER = dr["NEW_STAFF_NUMBER"].ToString();
            liveSession.NEW_MOBILE = dr["NEW_MOBILE"].ToString();
            liveSession.NEW_EMAIL = dr["NEW_EMAIL"].ToString();
            liveSession.NEW_STAFF_NAME = dr["NEW_STAFF_NAME"].ToString();
            liveSession.NEW_COURSE_NAME = dr["NEW_COURSE_NAME"].ToString();
            liveSession.NEW_MALE_COURSE_COLLAB_LINK = dr["NEW_MALE_COURSE_COLLAB_LINK"].ToString();
            liveSession.NEW_FEMALE_COURSE_COLLAB_LINK = dr["NEW_FEMALE_COURSE_COLLAB_LINK"].ToString();
            liveSession.NEW_SECOND_SEMESTER_CRN_FEMALE = dr["NEW_SECOND_SEMESTER_CRN_FEMALE"].ToString();
            liveSession.NEW_SECOND_SEMESTER_CRN_MALE = dr["NEW_SECOND_SEMESTER_CRN_MALE"].ToString();
            liveSession.NEW_FIRST_SEMESTER_CRN_FEMALE = dr["NEW_FIRST_SEMESTER_CRN_FEMALE"].ToString();
            liveSession.NEW_FIRST_SEMESTER_CRN_MALE = dr["NEW_FIRST_SEMESTER_CRN_MALE"].ToString();
            liveSession.NEW_COURSE_NUMBER = dr["NEW_COURSE_NUMBER"].ToString();




            StringBuilder sb = new StringBuilder();
            sb.Append(" data-term='" + liveSession.APP_TERM + "' ");//144010
            sb.Append(" data-livesessionno='" + liveSession.APP_LIVESESSION_NO + "' ");//المحاضرة المباشرة الأولي
            sb.Append(" data-livesessionnumber='" + liveSession.APP_SESSION_NUMBER + "' ");//session1
            sb.Append(" data-appday='" + liveSession.APP_DAY + "' ");// 1    OR 2 or ....    7
            sb.Append(" data-sessiontime='" + liveSession.APP_SESSION_TIME + "' ");//Female [8:00 pm , 8:30 pm] OR Male [8:45 pm , 9:15 pm] ...
            sb.Append(" data-coursenumber='" + liveSession.COURSE_NUMBER + "' "); //DL-0608-201
            sb.Append(" data-staffname='" + liveSession.STAFF_NAME + "' ");//د. فارس العصيمي
            sb.Append(" data-studionumber='" + liveSession.STUDIO_NUMBER + "' ");//1-18
            sb.Append(" data-studylevel='" + liveSession.STUDY_LEVEL + "' ");//1... 8 .....
            sb.Append(" data-specificmajor='" + liveSession.SPECIFIC_MAJOR + "' ");//ترخ سلم ....
            sb.Append(" data-generalmajor='" + liveSession.GENERAL_MAJOR + "' ");//إدارة ....
            sb.Append(" data-email='" + liveSession.EMAIL + "' ");//email ....
            sb.Append(" data-sessiontype='" + liveSession.SESSIONTYPE + "' ");//SessionType: ALL, Male,Female ....
            sb.Append(" data-grouptype='" + liveSession.GROUPTYPE + "' ");//grouptype: 1_8 , combined, Our_doctors, projects ....
            sb.Append(" data-coursename='" + liveSession.COURSE_NAME + "' ");
            sb.Append(" data-appdate='" + String.Format("{0:dd/MM/yyyy}", liveSession.APP_DATE) + "' ");

           
            sb.Append(" data-newlivesessionnumber='" + liveSession.NEW_APP_SESSION_NUMBER + "' ");//session1
            sb.Append(" data-newappday='" + liveSession.NEW_APP_DAY + "' ");// 1    OR 2 or ....    7
            sb.Append(" data-newsessiontime='" + liveSession.NEW_APP_SESSION_TIME + "' ");//Female [8:00 pm , 8:30 pm] OR Male [8:45 pm , 9:15 pm] ...
            sb.Append(" data-newcoursenumber='" + liveSession.NEW_COURSE_NUMBER + "' "); //DL-new0608-new201
            sb.Append(" data-newstaffname='" + liveSession.NEW_STAFF_NAME + "' ");//د. فارس العصيمي
            sb.Append(" data-newstudionumber='" + liveSession.NEW_STUDIO_NUMBER + "' ");//1-new18
            sb.Append(" data-newstudylevel='" + liveSession.NEW_STUDY_LEVEL + "' ");//1... 8 .....
            sb.Append(" data-newspecificmajor='" + liveSession.NEW_SPECIFIC_MAJOR + "' ");//ترخ سلم ....
            sb.Append(" data-newgeneralmajor='" + liveSession.NEW_GENERAL_MAJOR + "' ");//إدارة ....
            sb.Append(" data-newemail='" + liveSession.NEW_EMAIL + "' ");//email ....
            sb.Append(" data-newsessiontype='" + liveSession.NEW_SESSIONTYPE + "' ");//SessionType: ALL, Male,Female ....
            sb.Append(" data-newgrouptype='" + liveSession.NEW_GROUPTYPE + "' ");//grouptype: 1_8 , combined, Our_doctors, projects ....
            sb.Append(" data-newcoursename='" + liveSession.NEW_COURSE_NAME + "' ");
            sb.Append(" data-newappdate='" + String.Format("{0:dd/MM/yyyy}", liveSession.NEW_APP_DATE) + "' ");
            // sb.Append(" data-newcolabf='" + liveSession.FEMALE_COURSE_COLLAB_LINK + "' ");
            // sb.Append(" data-colabm='" + liveSession.MALE_COURSE_COLLAB_LINK + "' ");


            return sb.ToString();
        }

        private string GetSessionNumber(string v)
        {
             switch(Convert.ToInt32( v.Replace("session",string.Empty)))
            {
                case 1:
                case 2:
                    return "الفترة الأولي";
                case 3:
                case 4:
                    return "الفترة الثانية";
                case 5:
                case 6:
                    return "الفترة الثالثة";
            }
            return "الفترة الأولي";
        }


        [WebMethod(EnableSession = true)]
        public string ApproveRequets(AG_LIVESESSIONEXCEL MyLiveSession, AG_LIVESESSIONEXCEL New_LiveSession)
        {
            string res = AdminOperations.ApproveRequets(MyLiveSession,   New_LiveSession);

            return res; //"تم تغيير مياعد المحاضرة" : "حدث خطأ , لم يتم تغيير ميعاد المحاضرة المباشرة";
            
        }
    }
}
