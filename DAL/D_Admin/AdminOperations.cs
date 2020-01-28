using ManageLiveSessionWeb.BLL.B_Admin;
using ManageLiveSessionWeb.utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ManageLiveSessionWeb.DAL.D_Admin
{
    public class AdminOperations : DataServiceBase
    {
        public static DataSet SelectUsers(string active = "Y")
        {
            AdminOperations ld = new AdminOperations();
            string sql = "";
            if (active == "ALL")
                sql = "select * from AG_LIVESESSION_USERS where user_id <> 'Admin' AND IS_DELETE='N'";
            else
                sql = "select * from AG_LIVESESSION_USERS where user_id <> 'Admin' AND ACTIVE=:ACTIVE  AND IS_DELETE='N'";

            DataSet ds = null;
            if (active == "ALL")
                ds = ld.ExecuteSelectCommand(sql);
            else
                ds = ld.ExecuteSelectCommand(sql, new OracleParameter("ACTIVE", active));

            return ds;
        }

        public static int UpdateUsers(User user)
        {
            AdminOperations ld = new AdminOperations();
            string sql = "UPDATE AG_LIVESESSION_USERS SET USER_ID = :USER_ID, USER_NAME = :USER_NAME, MOBILE = :MOBILE, USER_EMAIL = :USER_EMAIL, ACTIVE = :ACTIVE WHERE ID = :ID";


            int res = ld.ExecuteNonQueryText(sql, new OracleParameter("USER_ID", user.USER_ID), new OracleParameter("USER_NAME", user.USER_NAME), new OracleParameter("MOBILE", user.MOBILE),
                new OracleParameter("USER_EMAIL", user.USER_EMAIL), new OracleParameter("ACTIVE", user.ACTIVE), new OracleParameter("ID", user.ID));

            return res;
        }


        internal static int NewUsers(User user)
        {
            AdminOperations ld = new AdminOperations();
            string sql1 = "SELECT  * FROM AG_LIVESESSION_USERS WHERE  USER_ID=:USER_ID AND IS_DELETE='N'";
            DataSet check = ld.ExecuteSelectCommand(sql1, new OracleParameter("USER_ID", user.USER_ID));
            if (check.Tables[0].Rows.Count > 0)
                return -1;

            string sql = "INSERT INTO AG_LIVESESSION_USERS (USER_ID ,USER_NAME , MOBILE, USER_EMAIL,ACTIVE ,IS_DELETE ) VALUES ( :USER_ID,   :USER_NAME,   :MOBILE,  :USER_EMAIL,   :ACTIVE, 'N' )";

            int res = ld.ExecuteNonQueryText(sql, new OracleParameter("USER_ID", user.USER_ID), new OracleParameter("USER_NAME", user.USER_NAME), new OracleParameter("MOBILE", user.MOBILE),
                new OracleParameter("USER_EMAIL", user.USER_EMAIL), new OracleParameter("ACTIVE", user.ACTIVE));

            return res;
        }

        public static int DeleteUser(string ID)
        {
            AdminOperations ld = new AdminOperations();
            string sql = "UPDATE AG_LIVESESSION_USERS SET  IS_DELETE=:IS_DELETE ,  ACTIVE=:ACTIVE   WHERE ID = :ID";


            int res = ld.ExecuteNonQueryText(sql, new OracleParameter("IS_DELETE", "Y"), new OracleParameter("ACTIVE", "N"), new OracleParameter("ID", ID));

            return res;
        }


        public static DataSet CheckAssignDay(string termCode, string livesession, string Day, string SESSION_NO)
        {
            AdminOperations ld = new AdminOperations();
            string sql = "";

            if (SESSION_NO == "ALL")
                sql = "SELECT * FROM AG_LIVESESSION_DAYASSIGN WHERE  TERM_CODE = :TERM_CODE AND LIVESESSION_NO = :LIVESESSION_NO AND DAY=:DAY";
            else
            {
                sql = "SELECT * FROM AG_LIVESESSION_DAYASSIGN WHERE  TERM_CODE = :TERM_CODE AND LIVESESSION_NO = :LIVESESSION_NO AND DAY=:DAY AND SESSION_NO=:SESSION_NO ";

            }
            DataSet assignDay = new DataSet();
            if (SESSION_NO == "ALL")
            {
                assignDay = ld.ExecuteSelectCommand(sql, new OracleParameter("TERM_CODE", termCode),
                                                                new OracleParameter("LIVESESSION_NO", livesession),
                                                  new OracleParameter("DAY", Day));
            }
            else
            {

                assignDay = ld.ExecuteSelectCommand(sql, new OracleParameter("TERM_CODE", termCode),
                                                                new OracleParameter("LIVESESSION_NO", livesession),
                                                                      new OracleParameter("DAY", Day),
                                                          new OracleParameter("SESSION_NO", SESSION_NO));

            }
            return assignDay;
        }


        public static DataSet AssignDay(string user_id, string termCode, string livesession, string Day, string SESSION_NO)
        {
            AdminOperations ld = new AdminOperations();

            string sql = "";

            if (SESSION_NO == "ALL")//USER_ID=:USER_ID AND
                sql = "DELETE FROM AG_LIVESESSION_DAYASSIGN WHERE  TERM_CODE = :TERM_CODE AND LIVESESSION_NO = :LIVESESSION_NO AND DAY=:DAY";
            else
                sql = "DELETE FROM AG_LIVESESSION_DAYASSIGN WHERE  TERM_CODE = :TERM_CODE AND LIVESESSION_NO = :LIVESESSION_NO AND DAY=:DAY AND SESSION_NO=:SESSION_NO ";


            int delResult = 0;
            if (SESSION_NO == "ALL")
                delResult = ld.ExecuteNonQueryText(sql, new OracleParameter("TERM_CODE", termCode),
                    new OracleParameter("LIVESESSION_NO", livesession), new OracleParameter("DAY", Day));
            else
            {
                delResult = ld.ExecuteNonQueryText(sql, new OracleParameter("TERM_CODE", termCode),
                                                                new OracleParameter("LIVESESSION_NO", livesession),
                                                                      new OracleParameter("DAY", Day),
                                                          new OracleParameter("SESSION_NO", SESSION_NO));

            }


            string sqlAssign = "INSERT INTO AG_LIVESESSION_DAYASSIGN (USER_ID,TERM_CODE,LIVESESSION_NO,DAY,SESSION_NO) VALUES (:USER_ID,:TERM_CODE,:LIVESESSION_NO,:DAY,:SESSION_NO)";
            int InsResult = 0;

            if (SESSION_NO == "ALL")
            {
                InsResult = ld.ExecuteNonQueryText(sqlAssign, new OracleParameter("USER_ID", user_id),
               new OracleParameter("TERM_CODE", termCode),
               new OracleParameter("LIVESESSION_NO", livesession), new OracleParameter("DAY", Day), new OracleParameter("SESSION_NO", "session1"));

                InsResult = ld.ExecuteNonQueryText(sqlAssign, new OracleParameter("USER_ID", user_id),
                new OracleParameter("TERM_CODE", termCode),
                new OracleParameter("LIVESESSION_NO", livesession), new OracleParameter("DAY", Day), new OracleParameter("SESSION_NO", "session2"));

                InsResult = ld.ExecuteNonQueryText(sqlAssign, new OracleParameter("USER_ID", user_id),
                new OracleParameter("TERM_CODE", termCode),
                new OracleParameter("LIVESESSION_NO", livesession), new OracleParameter("DAY", Day), new OracleParameter("SESSION_NO", "session3"));
            }
            else
            {
                InsResult = ld.ExecuteNonQueryText(sqlAssign, new OracleParameter("USER_ID", user_id),
                 new OracleParameter("TERM_CODE", termCode),
                 new OracleParameter("LIVESESSION_NO", livesession), new OracleParameter("DAY", Day), new OracleParameter("SESSION_NO", SESSION_NO));
            }

            string sqlAssignDays = "select e.* from AG_LIVESESSION_DAYASSIGN u, AG_LIVESESSIONEXCEL e where u.USER_ID = :USER_ID and u.TERM_CODE = e.APP_TERM and u.LIVESESSION_NO = e.APP_LIVESESSION_NO  and u.DAY = e.APP_DAY AND u.SESSION_NO = e.APP_SESSION_NUMBER order by e.APP_DAY , e.APP_SESSION_NUMBER,TO_NUMBER(e.STUDIO_NUMBER)";
            DataSet assignDay = ld.ExecuteSelectCommand(sqlAssignDays, new OracleParameter("USER_ID", user_id));

            return assignDay;
        }

        public static DataSet SearchLiveSession(string user_id, string termCode, string livesession, string Day)
        {
            AdminOperations ld = new AdminOperations();

            string sqlAssignDays = "";
            StringBuilder sb = new StringBuilder();
            List<OracleParameter> lstparams = new List<OracleParameter>();

            sb.Append("select e.*,u.USER_ID from AG_LIVESESSION_DAYASSIGN u, AG_LIVESESSIONEXCEL e where");
            OracleParameter parameter = null;

            if (termCode != "0")
            {
                sb.Append(" AND u.TERM_CODE = :termCode ");
                parameter = new OracleParameter("termCode", termCode);
                lstparams.Add(parameter);
            }
            if (livesession != "0")
            {
                sb.Append(" AND u.LIVESESSION_NO = :livesession ");
                parameter = new OracleParameter("livesession", livesession);
                lstparams.Add(parameter);
            }

            if (user_id != "0")
            {
                sb.Append(" AND u.USER_ID = :USER_ID ");
                parameter = new OracleParameter("USER_ID", user_id);
                lstparams.Add(parameter);
            }
            if (Day != "0")
            {
                sb.Append(" AND u.DAY = :Day ");
                parameter = new OracleParameter("Day", Day);
                lstparams.Add(parameter);
            }
            sb.Append(" AND u.TERM_CODE = e.APP_TERM and u.LIVESESSION_NO = e.APP_LIVESESSION_NO  and u.DAY = e.APP_DAY AND u.SESSION_NO = e.APP_SESSION_NUMBER ORDER BY e.APP_DAY , e.APP_SESSION_NUMBER,TO_NUMBER(e.STUDIO_NUMBER)");


            DataSet assignDay = ld.ExecuteSelectCommandParamList(sb.ToString().Replace("where AND", " WHERE "), lstparams);

            return assignDay;
        }

        public static DataSet Create_SearchLiveSession(string user_id, string termCode, string livesession, string Day)
        {
            AdminOperations ld = new AdminOperations();

            StringBuilder sb = new StringBuilder();
            List<OracleParameter> lstparams = new List<OracleParameter>();

            sb.Append("SELECT u.user_id, e.* " +
                    " FROM AG_LIVESESSION_DAYASSIGN u , AG_LIVESESSIONEXCEL e " +
                " WHERE u.TERM_CODE = E.APP_TERM " +
                                " AND u.LIVESESSION_NO = E.APP_LIVESESSION_NO " +
                                " AND u.DAY = E.APP_DAY " +
                                " AND u.SESSION_NO = E.APP_SESSION_NUMBER " +
                                " AND u.user_id =:USER_ID " +
                            " AND u.TERM_CODE =:TERM_CODE " +
                            " AND u.LIVESESSION_NO =:LIVESESSION_NO " +
                            " AND u.DAY =:DAY " +
                " ORDER BY e.APP_DAY , e.APP_SESSION_NUMBER, TO_NUMBER (e.STUDIO_NUMBER) ");


            DataSet creationCollaborate = ld.ExecuteSelectCommand(sb.ToString(), new OracleParameter("USER_ID", user_id), new OracleParameter("TERM_CODE", termCode),
                 new OracleParameter("LIVESESSION_NO", livesession), new OracleParameter("DAY", Day));

            return creationCollaborate;
        }

        public static DataSet CreatedLiveSession(string user_id, string termCode, string livesession, string Day)
        {
            AdminOperations ld = new AdminOperations();

            StringBuilder sb = new StringBuilder();
            List<OracleParameter> lstparams = new List<OracleParameter>();

            sb.Append(" SELECT * FROM   AG_LIVESESSION_CREATE " +
                                " WHERE user_id =:USER_ID " +
                            " AND TERM_CODE =:TERM_CODE " +
                            " AND LIVESESSION_NO =:LIVESESSION_NO " +
                            " AND DAY =:DAY ");


            DataSet creationCollaborate = ld.ExecuteSelectCommand(sb.ToString(), new OracleParameter("USER_ID", user_id), new OracleParameter("TERM_CODE", termCode),
                 new OracleParameter("LIVESESSION_NO", livesession), new OracleParameter("DAY", Day));

            return creationCollaborate;
        }


        internal static int AddEntry_LiveSession(string user_id, string termCode, string livesession, string day, string sESSION_NO, string sTUDIO_NO, string gender)
        {
            AdminOperations ld = new AdminOperations();

            string sqlAssignDays = "SELECT * FROM AG_LIVESESSION_CREATE WHERE USER_ID=:USER_ID AND TERM_CODE =:TERM_CODE AND LIVESESSION_NO =:LIVESESSION_NO AND DAY=:DAY AND SESSION_NO=:SESSION_NO AND STUDIO_NO=:STUDIO_NO AND GENDER =:GENDER";
            DataSet checkCreate = ld.ExecuteSelectCommand(sqlAssignDays, new OracleParameter("USER_ID", user_id),
               new OracleParameter("TERM_CODE", termCode),
               new OracleParameter("LIVESESSION_NO", livesession), new OracleParameter("DAY", day), new OracleParameter("SESSION_NO", sESSION_NO), new OracleParameter("STUDIO_NO", sTUDIO_NO)
               , new OracleParameter("GENDER", gender));
            if (checkCreate.Tables.Count > 0)
            {
                if (checkCreate.Tables[0].Rows.Count > 0)
                {
                    return 0;
                }
            }


            string sqlAssign = "INSERT INTO AG_LIVESESSION_CREATE (USER_ID,  TERM_CODE , LIVESESSION_NO,DAY, SESSION_NO, STUDIO_NO,GENDER) VALUES (:USER_ID,  :TERM_CODE , :LIVESESSION_NO, :DAY, :SESSION_NO, :STUDIO_NO,:GENDER)";

            int InsResult = 0;

            InsResult = ld.ExecuteNonQueryText(sqlAssign, new OracleParameter("USER_ID", user_id),
           new OracleParameter("TERM_CODE", termCode),
           new OracleParameter("LIVESESSION_NO", livesession), new OracleParameter("DAY", day), new OracleParameter("SESSION_NO", sESSION_NO), new OracleParameter("STUDIO_NO", sTUDIO_NO), new OracleParameter("GENDER", gender));
            if (InsResult > 0)
                return InsResult;


            return -1;

        }

        internal static DataSet ShowUsersDays(string termCode, string livesession)
        {
            AdminOperations ld = new AdminOperations();
            string sqlAssignDays = "SELECT * FROM AG_LIVESESSION_DAYASSIGN WHERE  TERM_CODE =:TERM_CODE AND LIVESESSION_NO =:LIVESESSION_NO ORDER BY DAY, USER_ID, SESSION_NO";
            DataSet selectAssign = ld.ExecuteSelectCommand(sqlAssignDays, new OracleParameter("TERM_CODE", termCode),
               new OracleParameter("LIVESESSION_NO", livesession));
            return selectAssign;

        }

        internal static DataSet ShowSingleUserDays(string user_id, string termCode, string livesession)
        {
            AdminOperations ld = new AdminOperations();
            string sqlAssignDays = "SELECT * FROM AG_LIVESESSION_DAYASSIGN WHERE USER_ID=:USER_ID AND TERM_CODE =:TERM_CODE AND LIVESESSION_NO =:LIVESESSION_NO ORDER BY DAY, USER_ID, SESSION_NO";
            DataSet selectAssign = ld.ExecuteSelectCommand(sqlAssignDays, new OracleParameter("USER_ID", user_id), new OracleParameter("TERM_CODE", termCode),
               new OracleParameter("LIVESESSION_NO", livesession));
            return selectAssign;

        }

        internal static List<AG_LIVESESSIONEXCEL> SelectLiveSessionTable(string termCode, string livesession)
        {
            try
            {
                AdminOperations ld = new AdminOperations();
                List<AG_LIVESESSIONEXCEL> selectExistLiveSessionList = new List<AG_LIVESESSIONEXCEL>();


                #region if change livesession with already exist one

                string sql = "SELECT * FROM AG_LIVESESSIONEXCEL WHERE " +
                                  " APP_TERM =:APP_TERM AND" +
                                  " APP_LIVESESSION_NO =:APP_LIVESESSION_NO  ";


                List<OracleParameter> param = new List<OracleParameter>();

                param.Add(new OracleParameter("APP_TERM", termCode));
                param.Add(new OracleParameter("APP_LIVESESSION_NO", livesession));


                DataSet ds = ld.ExecuteSelectCommandParamList(sql, param);
                AG_LIVESESSIONEXCEL selectExistLiveSession = null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            selectExistLiveSession = new AG_LIVESESSIONEXCEL();
                            selectExistLiveSession.APP_LIVESESSION_NO = dr["APP_LIVESESSION_NO"].ToString();
                            selectExistLiveSession.APP_TERM = dr["APP_TERM"].ToString();
                            selectExistLiveSession.APP_DAY = dr["APP_DAY"].ToString();
                            selectExistLiveSession.APP_SESSION_NUMBER = dr["APP_SESSION_NUMBER"].ToString();
                            selectExistLiveSession.APP_SESSION_TIME = dr["APP_SESSION_TIME"].ToString();
                            selectExistLiveSession.STUDIO_NUMBER = dr["STUDIO_NUMBER"].ToString();
                            selectExistLiveSession.FACULTY_NAME = dr["FACULTY_NAME"].ToString();
                            selectExistLiveSession.GENERAL_MAJOR = dr["GENERAL_MAJOR"].ToString();
                            selectExistLiveSession.SPECIFIC_MAJOR = dr["SPECIFIC_MAJOR"].ToString();
                            selectExistLiveSession.STUDY_LEVEL = dr["STUDY_LEVEL"].ToString();
                            selectExistLiveSession.COURSE_NUMBER = dr["COURSE_NUMBER"].ToString();
                            selectExistLiveSession.FIRST_SEMESTER_CRN_MALE = dr["FIRST_SEMESTER_CRN_MALE"].ToString();
                            selectExistLiveSession.FIRST_SEMESTER_CRN_FEMALE = dr["FIRST_SEMESTER_CRN_FEMALE"].ToString();
                            selectExistLiveSession.SECOND_SEMESTER_CRN_MALE = dr["SECOND_SEMESTER_CRN_MALE"].ToString();
                            selectExistLiveSession.SECOND_SEMESTER_CRN_FEMALE = dr["SECOND_SEMESTER_CRN_FEMALE"].ToString();
                            selectExistLiveSession.FEMALE_COURSE_COLLAB_LINK = dr["FEMALE_COURSE_COLLAB_LINK"].ToString();
                            selectExistLiveSession.MALE_COURSE_COLLAB_LINK = dr["MALE_COURSE_COLLAB_LINK"].ToString();
                            selectExistLiveSession.COURSE_NAME = dr["COURSE_NAME"].ToString();
                            selectExistLiveSession.STAFF_NAME = dr["STAFF_NAME"].ToString();
                            selectExistLiveSession.EMAIL = dr["EMAIL"].ToString();
                            selectExistLiveSession.MOBILE = dr["MOBILE"].ToString();
                            selectExistLiveSession.STAFF_NUMBER = dr["STAFF_NUMBER"].ToString();
                            selectExistLiveSession.EXAMDAY_PERIOD = dr["EXAMDAY_PERIOD"].ToString();
                            selectExistLiveSession.APP_DATE = dr["APP_DATE"].ToString();// Convert.ToDateTime(dr["APP_DATE"]);
                            selectExistLiveSession.TeachType = dr["TEACHTYPE"].ToString();
                            selectExistLiveSession.GroupType = (GroupDataType)Enum.Parse(typeof(GroupDataType), dr["GROUPTYPE"].ToString(), true);
                            selectExistLiveSession.SessionType = (SessionType)Enum.Parse(typeof(SessionType), dr["SESSIONTYPE"].ToString(), true);
                            selectExistLiveSessionList.Add(selectExistLiveSession);
                        }
                    }
                }
                return selectExistLiveSessionList;
                #endregion
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        internal static int AssignUserStudios(UserStudio userstudios)
        {
            AdminOperations ld = new AdminOperations();

            string sqlAssignDays = "DELETE FROM AG_LIVESESSION_USERSTUDIO WHERE USER_ID=:USER_ID AND TERM =:TERM AND LIVESESSION =:LIVESESSION";

            int res = ld.ExecuteNonQueryText(sqlAssignDays, new OracleParameter("USER_ID", userstudios.user), new OracleParameter("TERM", userstudios.term),
               new OracleParameter("LIVESESSION", userstudios.liveSession));

            string sql = "INSERT INTO AG_LIVESESSION_USERSTUDIO( TERM , LIVESESSION , USER_ID , DAY , STUDIOS) VALUES ( :TERM , :LIVESESSION , :USER_ID , :DAY , :STUDIOS  )";

            foreach (AssignDays days in userstudios.days)
            {

                res = ld.ExecuteNonQueryText(sql, new OracleParameter("TERM", userstudios.term),
                    new OracleParameter("LIVESESSION", userstudios.liveSession),
                    new OracleParameter("USER_ID", userstudios.user),
                       new OracleParameter("DAY", days.daynumber),
                         new OracleParameter("STUDIOS", Stringify(days.studios))
                         );
            }
            return res;
        }

        private static string Stringify(List<int> studios)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int i in studios)
            {
                sb.Append(i + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        internal static DataSet UserStudios(string user, string term, string liveSession)
        {
            AdminOperations ld = new AdminOperations();
            string USERSTUDIO = "SELECT * FROM AG_LIVESESSION_USERSTUDIO WHERE USER_ID=:USER_ID AND TERM =:TERM AND LIVESESSION =:LIVESESSION ORDER BY DAY";
            DataSet selectAssign = ld.ExecuteSelectCommand(USERSTUDIO, new OracleParameter("USER_ID", user), new OracleParameter("TERM", term),
               new OracleParameter("LIVESESSION", liveSession));
            return selectAssign;
        }

        internal static DataSet LiveSessionDates(string term, string liveSession)
        {
            AdminOperations ld = new AdminOperations();
            string liveSessiondates = "SELECT DISTINCT APP_DAY, APP_DATE FROM AG_LIVESESSIONEXCEL WHERE  APP_TERM =:APP_TERM AND APP_LIVESESSION_NO =:APP_LIVESESSION_NO ORDER BY APP_DAY";
            DataSet selectAssign = ld.ExecuteSelectCommand(liveSessiondates, new OracleParameter("APP_TERM", term),
               new OracleParameter("APP_LIVESESSION_NO", liveSession));
            return selectAssign;
        }

        internal static DataSet EvalQuestions(string term, string liveSession, string stdInfo, string course_doctor)
        {
            AdminOperations ld = new AdminOperations();
            string[] info = stdInfo.Split('-');
            StudioInfo studioInfo = new StudioInfo();
            studioInfo.studio = info[0];
            studioInfo.daynumber = info[1];
            studioInfo.date = info[2];
            studioInfo.dayName = info[3];


            string[] course_Email = course_doctor.Split('#');



            string questions = //"SELECT * FROM AG_LIVESESSION_QUESTIONS ORDER BY QUESTIONORDER";
                "SELECT * FROM(  SELECT Q.ID,Q.QUESTIONTEXT, A.ANSWER,A.NOTES, q.QUESTIONORDER FROM AG_LIVESESSION_QUESTIONS q left join AG_LIVESESSION_QUESTANSWER a" +
  " on q.ID = a.QUESTION_ID and A.DOCTOR_ID = :DOCTOR_ID  and a.COURSEID =:COURSEID and a.STUDIONO = :STUDIONO and a.TERM_CODE = :TERM_CODE " +
  "and a.LIVESESSION =:LIVESESSION  union " +
   " select 0, '','', NOTES,100 from AG_LIVESESSION_QUESTANSWER a where QUESTION_ID = 0 and A.DOCTOR_ID = :DOCTOR_ID and a.COURSEID = :COURSEID and a.STUDIONO = :STUDIONO and a.TERM_CODE = :TERM_CODE and a.LIVESESSION = :LIVESESSION ) order by QUESTIONORDER ";

            DataSet questionsData = ld.ExecuteSelectCommand(questions, new OracleParameter("DOCTOR_ID", course_Email[1]),
               new OracleParameter("COURSEID", course_Email[0]), new OracleParameter("STUDIONO", studioInfo.studio),
                new OracleParameter("TERM_CODE", term),
               new OracleParameter("LIVESESSION", liveSession));
            return questionsData;
        }


        internal static DataSet DayStudioSessions(string term, string liveSession, string stdInfo)
        {
            AdminOperations ld = new AdminOperations();

            string[] info = stdInfo.Split('-');
            StudioInfo studioInfo = new StudioInfo();
            studioInfo.studio = info[0];
            studioInfo.daynumber = info[1];
            studioInfo.date = info[2];
            studioInfo.dayName = info[3];


            string sql3 = "SELECT distinct COURSE_NAME, STAFF_NAME ,EMAIL,   FACULTY_NAME , COURSE_NUMBER  FROM AG_LIVESESSIONEXCEL WHERE " +
                                      " APP_TERM =:APP_TERM AND" +
                                      " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                                      " APP_DAY =:APP_DAY AND" +
                                      " STUDIO_NUMBER =:STUDIO_NUMBER  AND COURSE_NUMBER IS NOT NULL";

            List<OracleParameter> params3 = new List<OracleParameter>();

            params3.Add(new OracleParameter("APP_TERM", term));
            params3.Add(new OracleParameter("APP_LIVESESSION_NO", liveSession));
            params3.Add(new OracleParameter("APP_DAY", studioInfo.daynumber));
            params3.Add(new OracleParameter("STUDIO_NUMBER", studioInfo.studio));

            DataSet ds = ld.ExecuteSelectCommandParamList(sql3, params3);
            return ds;
        }


        internal static int DoctorEvaluation(DoctorEval answer)
        {
            AdminOperations ld = new AdminOperations();

            string sqlAssignDays = "DELETE FROM AG_LIVESESSION_QUESTANSWER WHERE USER_ID=:USER_ID " +
                "AND TERM_CODE =:TERM_CODE " +
                "AND LIVESESSION =:LIVESESSION " +
                "AND COURSEID =:COURSEID " +
                "AND STUDIONO =:STUDIONO ";

            //INFO:             std+"-"+ value.daynumber + "-" + date.date + "-" + date.dayName
            string[] info = answer.info.Split('-');
            StudioInfo studioInfo = new StudioInfo();
            studioInfo.studio = info[0];
            studioInfo.daynumber = info[1];
            studioInfo.date = info[2];
            studioInfo.dayName = info[3];

            string[] course_Email = answer.coursenumber.Split('#');

            int res = ld.ExecuteNonQueryText(sqlAssignDays, new OracleParameter("USER_ID", answer.user),
                                                            new OracleParameter("TERM_CODE", answer.term),
                                                            new OracleParameter("LIVESESSION", answer.liveSession),
                                                            new OracleParameter("COURSEID", course_Email[0]),
                                                            new OracleParameter("STUDIONO", studioInfo.studio));





            string sql = "INSERT INTO AG_LIVESESSION_QUESTANSWER(" +
                " QUESTION_ID , USER_ID , TERM_CODE , LIVESESSION , ANSWER , NOTES , DOCTOR_ID, COURSEID , LIVE_DATE , STUDIONO , ANSWER_DATE )" +
                " VALUES (  :QUESTION_ID , :USER_ID , :TERM_CODE , :LIVESESSION , :ANSWER , :NOTES , :DOCTOR_ID, :COURSEID , :LIVE_DATE , :STUDIONO , sysdate )";

            foreach (QuestionsInfo questionsInfo in answer.questionsInfo)
            {
                var Qanswer = questionsInfo.Yes == "yes" ? "Y" : "N";

                res = ld.ExecuteNonQueryText(sql,
                    new OracleParameter("QUESTION_ID", questionsInfo.ID),
                    new OracleParameter("USER_ID", answer.user),
                    new OracleParameter("TERM_CODE", answer.term),
                    new OracleParameter("LIVESESSION", answer.liveSession),
                    new OracleParameter("ANSWER", Qanswer),
                    new OracleParameter("NOTES", questionsInfo.Notes),
                    new OracleParameter("DOCTOR_ID", course_Email[1]),
                    new OracleParameter("COURSEID", course_Email[0]),
                    new OracleParameter("LIVE_DATE", DateTime.ParseExact(studioInfo.date, "dd/MM/yyyy", null)),
                    new OracleParameter("STUDIONO", studioInfo.studio));
            }

            res = ld.ExecuteNonQueryText(sql,
               new OracleParameter("QUESTION_ID", "0"),
               new OracleParameter("USER_ID", answer.user),
               new OracleParameter("TERM_CODE", answer.term),
               new OracleParameter("LIVESESSION", answer.liveSession),
               new OracleParameter("ANSWER", ""),
               new OracleParameter("NOTES", answer.GeneralNotes),
               new OracleParameter("DOCTOR_ID", course_Email[1]),
               new OracleParameter("COURSEID", course_Email[0]),
               new OracleParameter("LIVE_DATE", DateTime.ParseExact(studioInfo.date, "dd/MM/yyyy", null)),
               new OracleParameter("STUDIONO", studioInfo.studio));


            string updateSqlEval = "UPDATE AG_LIVESESSIONEXCEL SET EVALUATION = 'Y' WHERE STUDIO_NUMBER=:STUDIO_NUMBER AND  APP_LIVESESSION_NO=:APP_LIVESESSION_NO" +
                " AND COURSE_NUMBER=:COURSE_NUMBER  AND EMAIL like :EMAIL  AND APP_DAY=:APP_DAY ";


            int reseval = ld.ExecuteNonQueryText(updateSqlEval,
                new OracleParameter("STUDIO_NUMBER", studioInfo.studio), new OracleParameter("APP_LIVESESSION_NO", answer.liveSession), new OracleParameter("COURSE_NUMBER", course_Email[0]),
                new OracleParameter("EMAIL", course_Email[1] + "%"), new OracleParameter("APP_DAY", studioInfo.daynumber));

            return res;
        }

        internal static DataSet UserStudiosView(string term, string liveSession)
        {
            AdminOperations ld = new AdminOperations();
            string USERSTUDIO = "SELECT * FROM AG_LIVESESSION_USERSTUDIO  WHERE TERM =:TERM AND LIVESESSION =:LIVESESSION ORDER BY user_id, DAY";
            DataSet selectAssign = ld.ExecuteSelectCommand(USERSTUDIO, new OracleParameter("TERM", term),
               new OracleParameter("LIVESESSION", liveSession));
            return selectAssign;
        }

        internal static int EmptyLiveSessionStudio(AG_LIVESESSIONEXCEL liveSession)
        {
            AdminOperations ld = new AdminOperations();
            List<AG_LIVESESSIONEXCEL> ls = new List<AG_LIVESESSIONEXCEL>();
            string SelectSql = "";
            DataSet ds = null;

            if (liveSession.SessionType == SessionType.ALL)
            {
                SelectSql = "SELECT * FROM AG_LIVESESSIONEXCEL " +
                " WHERE APP_TERM = :APP_TERM AND APP_LIVESESSION_NO=:APP_LIVESESSION_NO  AND APP_DAY=:APP_DAY AND STUDIO_NUMBER=:STUDIO_NUMBER AND COURSE_NUMBER=:COURSE_NUMBER AND STUDY_LEVEL=:STUDY_LEVEL AND  EMAIL LIKE :EMAIL ";


                ds = ld.ExecuteSelectCommand(SelectSql,
                            new OracleParameter("APP_TERM", liveSession.APP_TERM),
                            new OracleParameter("APP_LIVESESSION_NO", liveSession.APP_LIVESESSION_NO),
                            new OracleParameter("APP_DAY", liveSession.APP_DAY),
                            new OracleParameter("STUDIO_NUMBER", liveSession.STUDIO_NUMBER),
                            new OracleParameter("COURSE_NUMBER", liveSession.COURSE_NUMBER),
                            new OracleParameter("STUDY_LEVEL", liveSession.STUDY_LEVEL),
                            new OracleParameter("EMAIL", liveSession.EMAIL + "%")
                    );
            }
            else
            {
                SelectSql = "SELECT * FROM AG_LIVESESSIONEXCEL " +
             " WHERE APP_TERM = :APP_TERM AND APP_LIVESESSION_NO=:APP_LIVESESSION_NO  AND APP_DAY=:APP_DAY AND STUDIO_NUMBER=:STUDIO_NUMBER AND COURSE_NUMBER=:COURSE_NUMBER AND STUDY_LEVEL=:STUDY_LEVEL AND  EMAIL LIKE :EMAIL" +
             " AND APP_SESSION_NUMBER=:APP_SESSION_NUMBER ";


                ds = ld.ExecuteSelectCommand(SelectSql,
                            new OracleParameter("APP_TERM", liveSession.APP_TERM),
                            new OracleParameter("APP_LIVESESSION_NO", liveSession.APP_LIVESESSION_NO),
                            new OracleParameter("APP_DAY", liveSession.APP_DAY),
                            new OracleParameter("STUDIO_NUMBER", liveSession.STUDIO_NUMBER),
                            new OracleParameter("COURSE_NUMBER", liveSession.COURSE_NUMBER),
                            new OracleParameter("STUDY_LEVEL", liveSession.STUDY_LEVEL),
                            new OracleParameter("EMAIL", liveSession.EMAIL + "%"),
                            new OracleParameter("APP_SESSION_NUMBER", liveSession.APP_SESSION_NUMBER)
                    );
            }

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    AG_LIVESESSIONEXCEL obj = null;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        obj = new AG_LIVESESSIONEXCEL();
                        obj.EVALUATION = dr["EVALUATION"].ToString();
                        obj.COLLAB_FEMALE_OPEN = dr["COLLAB_FEMALE_OPEN"].ToString();
                        obj.COLLAB_MALE_OPEN = dr["COLLAB_MALE_OPEN"].ToString();
                        obj.EXAMDAY_PERIOD = dr["EXAMDAY_PERIOD"].ToString();
                        obj.STAFF_NUMBER = dr["STAFF_NUMBER"].ToString();
                        obj.MOBILE = dr["MOBILE"].ToString();
                        obj.EMAIL = dr["EMAIL"].ToString();
                        obj.STAFF_NAME = dr["STAFF_NAME"].ToString();
                        obj.COURSE_NAME = dr["COURSE_NAME"].ToString();
                        obj.MALE_COURSE_COLLAB_LINK = dr["MALE_COURSE_COLLAB_LINK"].ToString();
                        obj.FEMALE_COURSE_COLLAB_LINK = dr["FEMALE_COURSE_COLLAB_LINK"].ToString();
                        obj.SECOND_SEMESTER_CRN_FEMALE = dr["SECOND_SEMESTER_CRN_FEMALE"].ToString();
                        obj.SECOND_SEMESTER_CRN_MALE = dr["SECOND_SEMESTER_CRN_MALE"].ToString();
                        obj.FIRST_SEMESTER_CRN_FEMALE = dr["FIRST_SEMESTER_CRN_FEMALE"].ToString();
                        obj.FIRST_SEMESTER_CRN_MALE = dr["FIRST_SEMESTER_CRN_MALE"].ToString();
                        obj.COURSE_NUMBER = dr["COURSE_NUMBER"].ToString();
                        obj.STUDY_LEVEL = dr["STUDY_LEVEL"].ToString();
                        obj.SPECIFIC_MAJOR = dr["SPECIFIC_MAJOR"].ToString();
                        obj.GENERAL_MAJOR = dr["GENERAL_MAJOR"].ToString();
                        obj.FACULTY_NAME = dr["FACULTY_NAME"].ToString();
                        obj.TeachType = dr["TEACHTYPE"].ToString();
                        obj.GroupType = (GroupDataType)Enum.Parse(typeof(GroupDataType), dr["GROUPTYPE"].ToString(), true);
                        obj.SessionType = (SessionType)Enum.Parse(typeof(SessionType), dr["SESSIONTYPE"].ToString(), true);
                        obj.APP_LIVESESSION_NO = dr["APP_LIVESESSION_NO"].ToString();
                        obj.APP_DATE = dr["APP_DATE"].ToString();
                        obj.STUDIO_NUMBER = dr["STUDIO_NUMBER"].ToString();
                        obj.APP_SESSION_TIME = dr["APP_SESSION_TIME"].ToString();
                        obj.APP_SESSION_NUMBER = dr["APP_SESSION_NUMBER"].ToString();
                        obj.APP_DAY = dr["APP_DAY"].ToString();
                        obj.APP_TERM = dr["APP_TERM"].ToString();
                        ls.Add(obj);
                    }
                }
            }



            string sql = "";
            int res = 0;
            if (liveSession.SessionType == SessionType.ALL)
            {
                sql = "UPDATE AG_LIVESESSIONEXCEL SET 	TEACHTYPE=null,	FACULTY_NAME=null	,GENERAL_MAJOR=null,	SPECIFIC_MAJOR=null,	STUDY_LEVEL=null	,COURSE_NUMBER=null	," +
        "FIRST_SEMESTER_CRN_MALE=null,	FIRST_SEMESTER_CRN_FEMALE=null,	SECOND_SEMESTER_CRN_MALE=null,	SECOND_SEMESTER_CRN_FEMALE=null,	FEMALE_COURSE_COLLAB_LINK=null,	MALE_COURSE_COLLAB_LINK=null,	" +
        "COURSE_NAME=null,	STAFF_NAME=null,	EMAIL=null,	MOBILE=null,	STAFF_NUMBER=null,	EXAMDAY_PERIOD=null,	COLLAB_MALE_OPEN=null,	COLLAB_FEMALE_OPEN=null,	EVALUATION=null , SESSIONTYPE='ALL' , GROUPTYPE='EMPTY'  " +
        " WHERE APP_TERM = :APP_TERM AND APP_LIVESESSION_NO=:APP_LIVESESSION_NO  AND APP_DAY=:APP_DAY AND STUDIO_NUMBER=:STUDIO_NUMBER AND COURSE_NUMBER=:COURSE_NUMBER AND STUDY_LEVEL=:STUDY_LEVEL AND  EMAIL LIKE :EMAIL ";


                res = ld.ExecuteNonQueryText(sql,
                           new OracleParameter("APP_TERM", liveSession.APP_TERM),
                           new OracleParameter("APP_LIVESESSION_NO", liveSession.APP_LIVESESSION_NO),
                           new OracleParameter("APP_DAY", liveSession.APP_DAY),
                           new OracleParameter("STUDIO_NUMBER", liveSession.STUDIO_NUMBER),
                           new OracleParameter("COURSE_NUMBER", liveSession.COURSE_NUMBER),
                           new OracleParameter("STUDY_LEVEL", liveSession.STUDY_LEVEL),
                           new OracleParameter("EMAIL", liveSession.EMAIL + "%")
                   );
            }
            else
            {
                sql = "UPDATE AG_LIVESESSIONEXCEL SET 	TEACHTYPE=null,	FACULTY_NAME=null	,GENERAL_MAJOR=null,	SPECIFIC_MAJOR=null,	STUDY_LEVEL=null	,COURSE_NUMBER=null	," +
   "FIRST_SEMESTER_CRN_MALE=null,	FIRST_SEMESTER_CRN_FEMALE=null,	SECOND_SEMESTER_CRN_MALE=null,	SECOND_SEMESTER_CRN_FEMALE=null,	FEMALE_COURSE_COLLAB_LINK=null,	MALE_COURSE_COLLAB_LINK=null,	" +
   "COURSE_NAME=null,	STAFF_NAME=null,	EMAIL=null,	MOBILE=null,	STAFF_NUMBER=null,	EXAMDAY_PERIOD=null,	COLLAB_MALE_OPEN=null,	COLLAB_FEMALE_OPEN=null,	EVALUATION=null , SESSIONTYPE='ALL' , GROUPTYPE='EMPTY'  " +
   " WHERE APP_TERM = :APP_TERM AND APP_LIVESESSION_NO=:APP_LIVESESSION_NO  AND APP_DAY=:APP_DAY AND STUDIO_NUMBER=:STUDIO_NUMBER AND COURSE_NUMBER=:COURSE_NUMBER AND STUDY_LEVEL=:STUDY_LEVEL AND  EMAIL LIKE :EMAIL AND  APP_SESSION_NUMBER= :APP_SESSION_NUMBER";


                res = ld.ExecuteNonQueryText(sql,
                           new OracleParameter("APP_TERM", liveSession.APP_TERM),
                           new OracleParameter("APP_LIVESESSION_NO", liveSession.APP_LIVESESSION_NO),
                           new OracleParameter("APP_DAY", liveSession.APP_DAY),
                           new OracleParameter("STUDIO_NUMBER", liveSession.STUDIO_NUMBER),
                           new OracleParameter("COURSE_NUMBER", liveSession.COURSE_NUMBER),
                           new OracleParameter("STUDY_LEVEL", liveSession.STUDY_LEVEL),
                           new OracleParameter("EMAIL", liveSession.EMAIL + "%"),
                           new OracleParameter("APP_SESSION_NUMBER", liveSession.APP_SESSION_NUMBER)

                   );
            }


            string insertDeletedSql = "  INSERT INTO AG_LIVESESSION_DELETED ( EVALUATION," +
"COLLAB_FEMALE_OPEN,COLLAB_MALE_OPEN,EXAMDAY_PERIOD,STAFF_NUMBER,MOBILE,EMAIL,STAFF_NAME,COURSE_NAME,MALE_COURSE_COLLAB_LINK,FEMALE_COURSE_COLLAB_LINK,SECOND_SEMESTER_CRN_FEMALE,SECOND_SEMESTER_CRN_MALE," +
"FIRST_SEMESTER_CRN_FEMALE,FIRST_SEMESTER_CRN_MALE,COURSE_NUMBER,STUDY_LEVEL,SPECIFIC_MAJOR,GENERAL_MAJOR,FACULTY_NAME,TEACHTYPE,SESSIONTYPE,GROUPTYPE,APP_LIVESESSION_NO,APP_DATE,STUDIO_NUMBER," +
"APP_SESSION_TIME,APP_SESSION_NUMBER,APP_DAY,APP_TERM ) " +
           " VALUES(:EVALUATION,:COLLAB_FEMALE_OPEN,:COLLAB_MALE_OPEN,:EXAMDAY_PERIOD,:STAFF_NUMBER,:MOBILE,:EMAIL,:STAFF_NAME,:COURSE_NAME,:MALE_COURSE_COLLAB_LINK,:FEMALE_COURSE_COLLAB_LINK," +
":SECOND_SEMESTER_CRN_FEMALE,:SECOND_SEMESTER_CRN_MALE,:FIRST_SEMESTER_CRN_FEMALE,:FIRST_SEMESTER_CRN_MALE,:COURSE_NUMBER,:STUDY_LEVEL,:SPECIFIC_MAJOR,:GENERAL_MAJOR,:FACULTY_NAME,:TEACHTYPE," +
":SESSIONTYPE,:GROUPTYPE,:APP_LIVESESSION_NO,:APP_DATE,:STUDIO_NUMBER,:APP_SESSION_TIME,:APP_SESSION_NUMBER,:APP_DAY,:APP_TERM)";

            foreach (AG_LIVESESSIONEXCEL aG_ in ls)
            {
                int resInsDel = ld.ExecuteNonQueryText(insertDeletedSql, new OracleParameter("EVALUATION", aG_.EVALUATION),
       new OracleParameter("COLLAB_FEMALE_OPEN", aG_.COLLAB_FEMALE_OPEN),
       new OracleParameter("COLLAB_MALE_OPEN", aG_.COLLAB_MALE_OPEN),
       new OracleParameter("EXAMDAY_PERIOD", aG_.EXAMDAY_PERIOD),
       new OracleParameter("STAFF_NUMBER", aG_.STAFF_NUMBER),
       new OracleParameter("MOBILE", aG_.MOBILE),
       new OracleParameter("EMAIL", aG_.EMAIL),
       new OracleParameter("STAFF_NAME", aG_.STAFF_NAME),
       new OracleParameter("COURSE_NAME", aG_.COURSE_NAME),
       new OracleParameter("MALE_COURSE_COLLAB_LINK", aG_.MALE_COURSE_COLLAB_LINK),
       new OracleParameter("FEMALE_COURSE_COLLAB_LINK", aG_.FEMALE_COURSE_COLLAB_LINK),
       new OracleParameter("SECOND_SEMESTER_CRN_FEMALE", aG_.SECOND_SEMESTER_CRN_FEMALE),
       new OracleParameter("SECOND_SEMESTER_CRN_MALE", aG_.SECOND_SEMESTER_CRN_MALE),
       new OracleParameter("FIRST_SEMESTER_CRN_FEMALE", aG_.FIRST_SEMESTER_CRN_FEMALE),
       new OracleParameter("FIRST_SEMESTER_CRN_MALE", aG_.FIRST_SEMESTER_CRN_MALE),
       new OracleParameter("COURSE_NUMBER", aG_.COURSE_NUMBER),
       new OracleParameter("STUDY_LEVEL", aG_.STUDY_LEVEL),
       new OracleParameter("SPECIFIC_MAJOR", aG_.SPECIFIC_MAJOR),
       new OracleParameter("GENERAL_MAJOR", aG_.GENERAL_MAJOR),
       new OracleParameter("FACULTY_NAME", aG_.FACULTY_NAME),
       new OracleParameter("TEACHTYPE", aG_.TeachType),
       new OracleParameter("SESSIONTYPE", aG_.SessionType.ToString()),
       new OracleParameter("GROUPTYPE", aG_.GroupType.ToString()),
       new OracleParameter("APP_LIVESESSION_NO", aG_.APP_LIVESESSION_NO),
       new OracleParameter("APP_DATE", Convert.ToDateTime(aG_.APP_DATE)),
       new OracleParameter("STUDIO_NUMBER", aG_.STUDIO_NUMBER),
       new OracleParameter("APP_SESSION_TIME", aG_.APP_SESSION_TIME),
       new OracleParameter("APP_SESSION_NUMBER", aG_.APP_SESSION_NUMBER),
       new OracleParameter("APP_DAY", aG_.APP_DAY),
       new OracleParameter("APP_TERM", aG_.APP_TERM));
            }

            return res;
        }

        internal static DataSet ShowRequests(string term, string liveSession)
        {
            AdminOperations ld = new AdminOperations();

            string SelectSql = "SELECT * FROM AG_LIVESESSIONCONFIRMCHANGE " +
          " WHERE APP_TERM = :APP_TERM AND APP_LIVESESSION_NO=:APP_LIVESESSION_NO  AND NEW_CONFIRM_STATUS='N' ORDER BY INSERT_DATE, APP_DAY, APP_SESSION_NUMBER ";


            DataSet ds = ld.ExecuteSelectCommand(SelectSql,
                       new OracleParameter("APP_TERM", term),
                       new OracleParameter("APP_LIVESESSION_NO", liveSession));

            return ds;
        }

        internal static string ApproveRequets(AG_LIVESESSIONEXCEL myLiveSession, AG_LIVESESSIONEXCEL new_LiveSession)
        {
            try
            {
                AdminOperations ld = new AdminOperations();

                #region check for same time doctor change if already exists

                string sqlCheck = "";

                if (myLiveSession.SessionType != SessionType.ALL)
                {
                    sqlCheck = "SELECT * FROM AG_LIVESESSIONCONFIRMCHANGE WHERE " +
                              " APP_TERM =:APP_TERM AND" +
                              " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +

                              " NEW_APP_SESSION_TIME LIKE :APP_SESSION_TIME AND " +
                              " NEW_APP_DAY =:APP_DAY AND " +
                              " NEW_STUDIO_NUMBER =:STUDIO_NUMBER  AND " +
                              " NEW_APP_SESSION_NUMBER =:APP_SESSION_NUMBER ";

                }
                else if (myLiveSession.SessionType == SessionType.ALL)
                {
                    sqlCheck = "SELECT * FROM AG_LIVESESSIONCONFIRMCHANGE WHERE " +
                          " APP_TERM =:APP_TERM AND" +
                          " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +

                          " NEW_APP_SESSION_TIME LIKE :APP_SESSION_TIME AND " +
                          " NEW_APP_DAY =:APP_DAY AND " +
                          " NEW_STUDIO_NUMBER =:STUDIO_NUMBER AND " +
                          " NEW_APP_SESSION_NUMBER in (  " + GetSessionNames(new_LiveSession.APP_SESSION_NUMBER) + "  )";

                }


                List<OracleParameter> paramsCheck = new List<OracleParameter>();

                paramsCheck.Add(new OracleParameter("APP_SESSION_TIME", new_LiveSession.APP_SESSION_TIME.Split('[')[0].Trim() + "%"));
                paramsCheck.Add(new OracleParameter("APP_TERM", new_LiveSession.APP_TERM));
                paramsCheck.Add(new OracleParameter("APP_LIVESESSION_NO", new_LiveSession.APP_LIVESESSION_NO));
                paramsCheck.Add(new OracleParameter("APP_DAY", new_LiveSession.APP_DAY));
                paramsCheck.Add(new OracleParameter("STUDIO_NUMBER", new_LiveSession.STUDIO_NUMBER));
                if (myLiveSession.SessionType != SessionType.ALL)
                {
                    paramsCheck.Add(new OracleParameter("APP_SESSION_NUMBER", new_LiveSession.APP_SESSION_NUMBER.ToString()));
                }


                DataSet dsCheck = ld.ExecuteSelectCommandParamList(sqlCheck, paramsCheck);
                if (dsCheck.Tables.Count > 0)
                {
                    if (dsCheck.Tables[0].Rows.Count > 0)
                    {
                        return "نأسف تم حجز الموعد من قبل";
                    }
                }
                #endregion

                List<ChangeLiveSession> selectExistLiveSessionList = new List<ChangeLiveSession>();
                List<ChangeLiveSession> selectExistMyLiveSessionList = new List<ChangeLiveSession>();

                #region select current livesession
                if (new_LiveSession.GroupType != GroupDataType.projects)
                {
                    if (new_LiveSession.GroupType != GroupDataType.EMPTY)
                    {
                        #region if change livesession with already exist one

                        string sql3 = "SELECT * FROM AG_LIVESESSIONEXCEL WHERE " +
                                          " APP_TERM =:APP_TERM AND" +
                                          " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                                          " SPECIFIC_MAJOR =:SPECIFIC_MAJOR AND" +
                                          " GENERAL_MAJOR=:GENERAL_MAJOR AND" +
                                          " STUDY_LEVEL=:STUDY_LEVEL AND" +
                                          " APP_DAY =:APP_DAY AND" +
                                          " STAFF_NAME =:STAFF_NAME AND" +
                                          " SESSIONTYPE =:SESSIONTYPE ";

                        List<OracleParameter> params3 = new List<OracleParameter>();

                        params3.Add(new OracleParameter("APP_TERM", new_LiveSession.APP_TERM));
                        params3.Add(new OracleParameter("APP_LIVESESSION_NO", new_LiveSession.APP_LIVESESSION_NO));
                        params3.Add(new OracleParameter("SPECIFIC_MAJOR", new_LiveSession.SPECIFIC_MAJOR));
                        params3.Add(new OracleParameter("GENERAL_MAJOR", new_LiveSession.GENERAL_MAJOR));
                        params3.Add(new OracleParameter("STUDY_LEVEL", new_LiveSession.STUDY_LEVEL));
                        params3.Add(new OracleParameter("APP_DAY", new_LiveSession.APP_DAY));
                        params3.Add(new OracleParameter("STAFF_NAME", new_LiveSession.STAFF_NAME));
                        params3.Add(new OracleParameter("SESSIONTYPE", new_LiveSession.SessionType.ToString()));

                        DataSet ds = ld.ExecuteSelectCommandParamList(sql3, params3);
                        ChangeLiveSession selectExistLiveSession = null;
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {

                                    selectExistLiveSession = new ChangeLiveSession();
                                    selectExistLiveSession.APP_LIVESESSION_NO = dr["APP_LIVESESSION_NO"].ToString();
                                    selectExistLiveSession.APP_TERM = dr["APP_TERM"].ToString();
                                    selectExistLiveSession.APP_DAY = dr["APP_DAY"].ToString();
                                    selectExistLiveSession.APP_SESSION_NUMBER = dr["APP_SESSION_NUMBER"].ToString();
                                    selectExistLiveSession.APP_SESSION_TIME = dr["APP_SESSION_TIME"].ToString();
                                    selectExistLiveSession.STUDIO_NUMBER = dr["STUDIO_NUMBER"].ToString();
                                    selectExistLiveSession.FACULTY_NAME = dr["FACULTY_NAME"].ToString();
                                    selectExistLiveSession.GENERAL_MAJOR = dr["GENERAL_MAJOR"].ToString();
                                    selectExistLiveSession.SPECIFIC_MAJOR = dr["SPECIFIC_MAJOR"].ToString();
                                    selectExistLiveSession.STUDY_LEVEL = dr["STUDY_LEVEL"].ToString();
                                    selectExistLiveSession.COURSE_NUMBER = dr["COURSE_NUMBER"].ToString();
                                    selectExistLiveSession.FIRST_SEMESTER_CRN_MALE = dr["FIRST_SEMESTER_CRN_MALE"].ToString();
                                    selectExistLiveSession.FIRST_SEMESTER_CRN_FEMALE = dr["FIRST_SEMESTER_CRN_FEMALE"].ToString();
                                    selectExistLiveSession.SECOND_SEMESTER_CRN_MALE = dr["SECOND_SEMESTER_CRN_MALE"].ToString();
                                    selectExistLiveSession.SECOND_SEMESTER_CRN_FEMALE = dr["SECOND_SEMESTER_CRN_FEMALE"].ToString();
                                    selectExistLiveSession.FEMALE_COURSE_COLLAB_LINK = dr["FEMALE_COURSE_COLLAB_LINK"].ToString();
                                    selectExistLiveSession.MALE_COURSE_COLLAB_LINK = dr["MALE_COURSE_COLLAB_LINK"].ToString();
                                    selectExistLiveSession.COURSE_NAME = dr["COURSE_NAME"].ToString();
                                    selectExistLiveSession.STAFF_NAME = dr["STAFF_NAME"].ToString();
                                    selectExistLiveSession.EMAIL = dr["EMAIL"].ToString();
                                    selectExistLiveSession.MOBILE = dr["MOBILE"].ToString();
                                    selectExistLiveSession.STAFF_NUMBER = dr["STAFF_NUMBER"].ToString();
                                    selectExistLiveSession.EXAMDAY_PERIOD = dr["EXAMDAY_PERIOD"].ToString();
                                    selectExistLiveSession.APP_DATE = Convert.ToDateTime(dr["APP_DATE"]);
                                    selectExistLiveSession.TEACHTYPE = dr["TEACHTYPE"].ToString();
                                    selectExistLiveSession.GROUPTYPE = dr["GROUPTYPE"].ToString();
                                    selectExistLiveSession.SESSIONTYPE = dr["SESSIONTYPE"].ToString();
                                    selectExistLiveSessionList.Add(selectExistLiveSession);
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region if change livesession empty
                        string sql3 = "";
                        if (myLiveSession.SessionType != SessionType.ALL) // == "1")
                        {
                            sql3 = "SELECT * FROM AG_LIVESESSIONEXCEL WHERE " +
                                        " APP_TERM =:APP_TERM AND" +
                                        " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                                        " STUDIO_NUMBER =:STUDIO_NUMBER AND " +
                                        " APP_DAY =:APP_DAY AND" +
                                        " GROUPTYPE =:GROUPTYPE  AND  APP_SESSION_NUMBER =:APP_SESSION_NUMBER";

                        }
                        else if (myLiveSession.SessionType == SessionType.ALL)// == "2")
                        {
                            sql3 = "SELECT * FROM AG_LIVESESSIONEXCEL WHERE " +
                                       " APP_TERM =:APP_TERM AND" +
                                       " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                                       " STUDIO_NUMBER =:STUDIO_NUMBER AND " +
                                       " APP_DAY =:APP_DAY AND" +
                                       " GROUPTYPE =:GROUPTYPE  AND  APP_SESSION_NUMBER in (  " + GetSessionNames(new_LiveSession.APP_SESSION_NUMBER) + "  )";


                        }

                        List<OracleParameter> params3 = new List<OracleParameter>();

                        params3.Add(new OracleParameter("APP_TERM", new_LiveSession.APP_TERM));
                        params3.Add(new OracleParameter("APP_LIVESESSION_NO", new_LiveSession.APP_LIVESESSION_NO));
                        params3.Add(new OracleParameter("STUDIO_NUMBER", new_LiveSession.STUDIO_NUMBER));
                        params3.Add(new OracleParameter("APP_DAY", new_LiveSession.APP_DAY));
                        params3.Add(new OracleParameter("GROUPTYPE", new_LiveSession.GroupType.ToString()));
                        if (myLiveSession.SessionType != SessionType.ALL)
                        {
                            params3.Add(new OracleParameter("APP_SESSION_NUMBER", new_LiveSession.APP_SESSION_NUMBER.ToString()));
                        }

                        DataSet ds = ld.ExecuteSelectCommandParamList(sql3, params3);
                        ChangeLiveSession selectExistLiveSession = null;
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {

                                    selectExistLiveSession = new ChangeLiveSession();
                                    selectExistLiveSession.APP_LIVESESSION_NO = dr["APP_LIVESESSION_NO"].ToString();
                                    selectExistLiveSession.APP_TERM = dr["APP_TERM"].ToString();
                                    selectExistLiveSession.APP_DAY = dr["APP_DAY"].ToString();
                                    selectExistLiveSession.APP_SESSION_NUMBER = dr["APP_SESSION_NUMBER"].ToString();
                                    selectExistLiveSession.APP_SESSION_TIME = dr["APP_SESSION_TIME"].ToString();
                                    selectExistLiveSession.STUDIO_NUMBER = dr["STUDIO_NUMBER"].ToString();
                                    selectExistLiveSession.FACULTY_NAME = dr["FACULTY_NAME"].ToString();
                                    selectExistLiveSession.GENERAL_MAJOR = dr["GENERAL_MAJOR"].ToString();
                                    selectExistLiveSession.SPECIFIC_MAJOR = dr["SPECIFIC_MAJOR"].ToString();
                                    selectExistLiveSession.STUDY_LEVEL = dr["STUDY_LEVEL"].ToString();
                                    selectExistLiveSession.COURSE_NUMBER = dr["COURSE_NUMBER"].ToString();
                                    selectExistLiveSession.FIRST_SEMESTER_CRN_MALE = dr["FIRST_SEMESTER_CRN_MALE"].ToString();
                                    selectExistLiveSession.FIRST_SEMESTER_CRN_FEMALE = dr["FIRST_SEMESTER_CRN_FEMALE"].ToString();
                                    selectExistLiveSession.SECOND_SEMESTER_CRN_MALE = dr["SECOND_SEMESTER_CRN_MALE"].ToString();
                                    selectExistLiveSession.SECOND_SEMESTER_CRN_FEMALE = dr["SECOND_SEMESTER_CRN_FEMALE"].ToString();
                                    selectExistLiveSession.FEMALE_COURSE_COLLAB_LINK = dr["FEMALE_COURSE_COLLAB_LINK"].ToString();
                                    selectExistLiveSession.MALE_COURSE_COLLAB_LINK = dr["MALE_COURSE_COLLAB_LINK"].ToString();
                                    selectExistLiveSession.COURSE_NAME = dr["COURSE_NAME"].ToString();
                                    selectExistLiveSession.STAFF_NAME = dr["STAFF_NAME"].ToString();
                                    selectExistLiveSession.EMAIL = dr["EMAIL"].ToString();
                                    selectExistLiveSession.MOBILE = dr["MOBILE"].ToString();
                                    selectExistLiveSession.STAFF_NUMBER = dr["STAFF_NUMBER"].ToString();
                                    selectExistLiveSession.EXAMDAY_PERIOD = dr["EXAMDAY_PERIOD"].ToString();
                                    selectExistLiveSession.APP_DATE = Convert.ToDateTime(dr["APP_DATE"]);
                                    selectExistLiveSession.TEACHTYPE = dr["TEACHTYPE"].ToString();
                                    selectExistLiveSession.GROUPTYPE = dr["GROUPTYPE"].ToString();
                                    selectExistLiveSession.SESSIONTYPE = dr["SESSIONTYPE"].ToString();
                                    selectExistLiveSessionList.Add(selectExistLiveSession);
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                #region select My LiveSession Data

                string sqlMy = "";
                if (myLiveSession.SessionType != SessionType.ALL) // == "1")
                {
                    sqlMy = "SELECT * FROM AG_LIVESESSIONEXCEL WHERE " +
                                    " APP_TERM =:APP_TERM AND" +
                                    " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                                    " STUDIO_NUMBER =:STUDIO_NUMBER AND" +
                                    " STUDY_LEVEL=:STUDY_LEVEL AND" +
                                    " APP_DAY =:APP_DAY AND" +
                                    " STAFF_NAME like :STAFF_NAME AND" +
                                    " SESSIONTYPE =:SESSIONTYPE AND" +
                    " APP_SESSION_NUMBER =:APP_SESSION_NUMBER  ";
                }
                else if(myLiveSession.SessionType == SessionType.ALL)
                {
                    sqlMy = "SELECT * FROM AG_LIVESESSIONEXCEL WHERE " +
                                   " APP_TERM =:APP_TERM AND" +
                                   " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                                   " STUDIO_NUMBER =:STUDIO_NUMBER AND" +
                                   " STUDY_LEVEL=:STUDY_LEVEL AND" +
                                   " APP_DAY =:APP_DAY AND" +
                                   " STAFF_NAME like :STAFF_NAME AND" +
                                   " SESSIONTYPE =:SESSIONTYPE AND" + 
                                  " APP_SESSION_NUMBER in (  " + GetSessionNames(myLiveSession.APP_SESSION_NUMBER) + "  )";
                }
                List<OracleParameter> paramsMy = new List<OracleParameter>();

                paramsMy.Add(new OracleParameter("APP_TERM", myLiveSession.APP_TERM));
                paramsMy.Add(new OracleParameter("APP_LIVESESSION_NO", myLiveSession.APP_LIVESESSION_NO));
                paramsMy.Add(new OracleParameter("STUDIO_NUMBER", myLiveSession.STUDIO_NUMBER));

                paramsMy.Add(new OracleParameter("STUDY_LEVEL", myLiveSession.STUDY_LEVEL));
                paramsMy.Add(new OracleParameter("APP_DAY", myLiveSession.APP_DAY));
                paramsMy.Add(new OracleParameter("STAFF_NAME", "%" + myLiveSession.STAFF_NAME + "%"));
                paramsMy.Add(new OracleParameter("SESSIONTYPE", myLiveSession.SessionType.ToString()));
                if (myLiveSession.SessionType != SessionType.ALL)
                {
                    paramsMy.Add(new OracleParameter("APP_SESSION_NUMBER", myLiveSession.APP_SESSION_NUMBER));

                }


                DataSet dsMy = ld.ExecuteSelectCommandParamList(sqlMy, paramsMy);
                ChangeLiveSession selectExistMYLiveSession = null;
                if (dsMy.Tables.Count > 0)
                {
                    if (dsMy.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsMy.Tables[0].Rows)
                        {

                            selectExistMYLiveSession = new ChangeLiveSession();
                            selectExistMYLiveSession.APP_LIVESESSION_NO = dr["APP_LIVESESSION_NO"].ToString();
                            selectExistMYLiveSession.APP_TERM = dr["APP_TERM"].ToString();
                            selectExistMYLiveSession.APP_DAY = dr["APP_DAY"].ToString();
                            selectExistMYLiveSession.APP_SESSION_NUMBER = dr["APP_SESSION_NUMBER"].ToString();
                            selectExistMYLiveSession.APP_SESSION_TIME = dr["APP_SESSION_TIME"].ToString();
                            selectExistMYLiveSession.STUDIO_NUMBER = dr["STUDIO_NUMBER"].ToString();
                            selectExistMYLiveSession.FACULTY_NAME = dr["FACULTY_NAME"].ToString();
                            selectExistMYLiveSession.GENERAL_MAJOR = dr["GENERAL_MAJOR"].ToString();
                            selectExistMYLiveSession.SPECIFIC_MAJOR = dr["SPECIFIC_MAJOR"].ToString();
                            selectExistMYLiveSession.STUDY_LEVEL = dr["STUDY_LEVEL"].ToString();
                            selectExistMYLiveSession.COURSE_NUMBER = dr["COURSE_NUMBER"].ToString();
                            selectExistMYLiveSession.FIRST_SEMESTER_CRN_MALE = dr["FIRST_SEMESTER_CRN_MALE"].ToString();
                            selectExistMYLiveSession.FIRST_SEMESTER_CRN_FEMALE = dr["FIRST_SEMESTER_CRN_FEMALE"].ToString();
                            selectExistMYLiveSession.SECOND_SEMESTER_CRN_MALE = dr["SECOND_SEMESTER_CRN_MALE"].ToString();
                            selectExistMYLiveSession.SECOND_SEMESTER_CRN_FEMALE = dr["SECOND_SEMESTER_CRN_FEMALE"].ToString();
                            selectExistMYLiveSession.FEMALE_COURSE_COLLAB_LINK = dr["FEMALE_COURSE_COLLAB_LINK"].ToString();
                            selectExistMYLiveSession.MALE_COURSE_COLLAB_LINK = dr["MALE_COURSE_COLLAB_LINK"].ToString();
                            selectExistMYLiveSession.COURSE_NAME = dr["COURSE_NAME"].ToString();
                            selectExistMYLiveSession.STAFF_NAME = dr["STAFF_NAME"].ToString();
                            selectExistMYLiveSession.EMAIL = dr["EMAIL"].ToString();
                            selectExistMYLiveSession.MOBILE = dr["MOBILE"].ToString();
                            selectExistMYLiveSession.STAFF_NUMBER = dr["STAFF_NUMBER"].ToString();
                            selectExistMYLiveSession.EXAMDAY_PERIOD = dr["EXAMDAY_PERIOD"].ToString();
                            selectExistMYLiveSession.APP_DATE = Convert.ToDateTime(dr["APP_DATE"]);
                            selectExistMYLiveSession.TEACHTYPE = dr["TEACHTYPE"].ToString();
                            selectExistMYLiveSession.GROUPTYPE = dr["GROUPTYPE"].ToString();
                            selectExistMYLiveSession.SESSIONTYPE = dr["SESSIONTYPE"].ToString();

                            selectExistMyLiveSessionList.Add(selectExistMYLiveSession);

                        }
                    }
                }
                #endregion

                #region UPDATE Audit Data old and new (FROM , TO) live session
                string cmd = "";
                if (myLiveSession.SessionType == SessionType.ALL)
                {
                    cmd = "UPDATE AG_LIVESESSIONCONFIRMCHANGE SET NEW_CONFIRM_STATUS ='Y' , NEW_WITHIN_3_DAYS='N' WHERE  APP_TERM =:APP_TERM AND " +
                            " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                           //" EMAIL LIKE :EMAIL AND " +
                           " APP_DAY =:APP_DAY AND " +
                           " STUDIO_NUMBER =:STUDIO_NUMBER AND " +
                            " COURSE_NUMBER =:COURSE_NUMBER AND" +
                            " APP_SESSION_NUMBER in (  " + GetSessionNames(myLiveSession.APP_SESSION_NUMBER) + ")";
                }
                else if (myLiveSession.SessionType != SessionType.ALL)
                {
                    cmd = "UPDATE AG_LIVESESSIONCONFIRMCHANGE SET NEW_CONFIRM_STATUS ='Y' , NEW_WITHIN_3_DAYS='N' WHERE APP_TERM =:APP_TERM AND" +
                           " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                          // " EMAIL LIKE :EMAIL AND " +
                           " APP_DAY =:APP_DAY AND " +
                           " STUDIO_NUMBER =:STUDIO_NUMBER AND " +
                            " COURSE_NUMBER =:COURSE_NUMBER AND"+
                            " APP_SESSION_NUMBER =:APP_SESSION_NUMBER ";

                }

                List<OracleParameter> myParamsLog = null;

                for (int i = 0; i < selectExistMyLiveSessionList.Count(); i++)
                {
                    myParamsLog = new List<OracleParameter>();

                    myParamsLog.Add(new OracleParameter("APP_TERM", selectExistMyLiveSessionList[i].APP_TERM));//old live
                    myParamsLog.Add(new OracleParameter("APP_LIVESESSION_NO", selectExistMyLiveSessionList[i].APP_LIVESESSION_NO)); 
                   // myParamsLog.Add(new OracleParameter("EMAIL", selectExistMyLiveSessionList[i].EMAIL.Trim() + "%"));
                    myParamsLog.Add(new OracleParameter("APP_DAY", selectExistMyLiveSessionList[i].APP_DAY));
                    myParamsLog.Add(new OracleParameter("STUDIO_NUMBER", selectExistMyLiveSessionList[i].STUDIO_NUMBER));
                    myParamsLog.Add(new OracleParameter("COURSE_NUMBER", selectExistMyLiveSessionList[i].COURSE_NUMBER));

                    if (myLiveSession.SessionType != SessionType.ALL)
                    {
                        myParamsLog.Add(new OracleParameter("APP_SESSION_NUMBER", selectExistMyLiveSessionList[i].APP_SESSION_NUMBER));
                    }

                   // myParamsLog.Add(new OracleParameter("NEW_CONFIRM_STATUS", "Y"));


                    int res = ld.ExecuteNonQueryText(cmd, myParamsLog);
                }


                #endregion

                #region Delete OLD livesession VALUES

                string sql = "DELETE FROM AG_LIVESESSIONEXCEL WHERE " +
                                  " APP_TERM =:APP_TERM AND" +
                                  " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                                  " SPECIFIC_MAJOR =:SPECIFIC_MAJOR AND" +
                                  " GENERAL_MAJOR=:GENERAL_MAJOR AND" +
                                  " STUDY_LEVEL=:STUDY_LEVEL AND" +
                                  " APP_DAY =:APP_DAY AND" +
                                  " STAFF_NAME =:STAFF_NAME AND" +
                                  " APP_SESSION_NUMBER =:APP_SESSION_NUMBER AND" +

                                  " SESSIONTYPE =:SESSIONTYPE  ";
                for (int i = 0; i < selectExistMyLiveSessionList.Count(); i++)
                {
                    List<OracleParameter> params1 = new List<OracleParameter>();

                    params1.Add(new OracleParameter("APP_TERM", selectExistMyLiveSessionList[i].APP_TERM));
                    params1.Add(new OracleParameter("APP_LIVESESSION_NO", selectExistMyLiveSessionList[i].APP_LIVESESSION_NO));
                    params1.Add(new OracleParameter("SPECIFIC_MAJOR", selectExistMyLiveSessionList[i].SPECIFIC_MAJOR));
                    params1.Add(new OracleParameter("GENERAL_MAJOR", selectExistMyLiveSessionList[i].GENERAL_MAJOR));
                    params1.Add(new OracleParameter("STUDY_LEVEL", selectExistMyLiveSessionList[i].STUDY_LEVEL));
                    params1.Add(new OracleParameter("APP_DAY", selectExistMyLiveSessionList[i].APP_DAY));
                    params1.Add(new OracleParameter("STAFF_NAME", selectExistMyLiveSessionList[i].STAFF_NAME));
                    params1.Add(new OracleParameter("APP_SESSION_NUMBER", selectExistMyLiveSessionList[i].APP_SESSION_NUMBER));
                    params1.Add(new OracleParameter("SESSIONTYPE", selectExistMyLiveSessionList[i].SESSIONTYPE.ToString()));

                    int res1 = ld.ExecuteNonQueryText(sql, params1);
                }

                for (int i = 0; i < selectExistLiveSessionList.Count(); i++)
                {
                    if (new_LiveSession.GroupType != GroupDataType.EMPTY)
                    {
                        string sql1 = "DELETE FROM AG_LIVESESSIONEXCEL WHERE " +
                                  " APP_TERM =:APP_TERM AND" +
                                  " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                                  " SPECIFIC_MAJOR =:SPECIFIC_MAJOR AND" +
                                  " GENERAL_MAJOR=:GENERAL_MAJOR AND" +
                                  " STUDY_LEVEL=:STUDY_LEVEL AND" +
                                  " APP_DAY =:APP_DAY AND" +
                                  " STAFF_NAME =:STAFF_NAME AND" +
                                  " APP_SESSION_NUMBER =:APP_SESSION_NUMBER AND" +
                                  " SESSIONTYPE =:SESSIONTYPE ";

                        List<OracleParameter> params11 = new List<OracleParameter>();

                        params11.Add(new OracleParameter("APP_TERM", selectExistLiveSessionList[i].APP_TERM));
                        params11.Add(new OracleParameter("APP_LIVESESSION_NO", selectExistLiveSessionList[i].APP_LIVESESSION_NO));
                        params11.Add(new OracleParameter("SPECIFIC_MAJOR", selectExistLiveSessionList[i].SPECIFIC_MAJOR));
                        params11.Add(new OracleParameter("GENERAL_MAJOR", selectExistLiveSessionList[i].GENERAL_MAJOR));
                        params11.Add(new OracleParameter("STUDY_LEVEL", selectExistLiveSessionList[i].STUDY_LEVEL));
                        params11.Add(new OracleParameter("APP_DAY", selectExistLiveSessionList[i].APP_DAY));
                        params11.Add(new OracleParameter("STAFF_NAME", selectExistLiveSessionList[i].STAFF_NAME));
                        params11.Add(new OracleParameter("APP_SESSION_NUMBER", selectExistMyLiveSessionList[i].APP_SESSION_NUMBER));

                        params11.Add(new OracleParameter("SESSIONTYPE", selectExistLiveSessionList[i].SESSIONTYPE.ToString()));

                        int res2 = ld.ExecuteNonQueryText(sql1, params11);
                    }
                    else
                    {
                        string sql1 = "";
                        if (myLiveSession.SessionType != SessionType.ALL)// == "1")
                        {
                            sql1 = "DELETE FROM AG_LIVESESSIONEXCEL WHERE " +
                                     " APP_TERM =:APP_TERM AND" +
                                     " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                                     " STUDIO_NUMBER =:STUDIO_NUMBER AND " +
                                     " APP_DAY =:APP_DAY AND" +
                                     " GROUPTYPE =:GROUPTYPE AND  APP_SESSION_NUMBER in (  '" + new_LiveSession.APP_SESSION_NUMBER + "'  )";
                        }
                        else if (myLiveSession.SessionType == SessionType.ALL)
                        {
                            sql1 = "DELETE FROM AG_LIVESESSIONEXCEL WHERE " +
                                     " APP_TERM =:APP_TERM AND" +
                                     " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                                     " STUDIO_NUMBER =:STUDIO_NUMBER AND " +
                                     " APP_DAY =:APP_DAY AND" +
                                     " GROUPTYPE =:GROUPTYPE AND  APP_SESSION_NUMBER in (  " + GetSessionNames(new_LiveSession.APP_SESSION_NUMBER) + "  )";
                        }
                        List<OracleParameter> params3 = new List<OracleParameter>();

                        params3.Add(new OracleParameter("APP_TERM", new_LiveSession.APP_TERM));
                        params3.Add(new OracleParameter("APP_LIVESESSION_NO", new_LiveSession.APP_LIVESESSION_NO));
                        params3.Add(new OracleParameter("STUDIO_NUMBER", new_LiveSession.STUDIO_NUMBER));
                        params3.Add(new OracleParameter("APP_DAY", new_LiveSession.APP_DAY));
                        params3.Add(new OracleParameter("GROUPTYPE", new_LiveSession.GroupType.ToString()));


                        int res2 = ld.ExecuteNonQueryText(sql1, params3);

                    }
                }
                #endregion

                #region insert replaced live session 

                string command = "INSERT INTO AG_LIVESESSIONEXCEL(" +
      "	APP_LIVESESSION_NO," +
      "	APP_TERM," +

      "	APP_DAY," +
        "	APP_DATE," +
      "	APP_SESSION_NUMBER," +
      "	APP_SESSION_TIME," +
      "	STUDIO_NUMBER," +
      "	GROUPTYPE," +
      "	SESSIONTYPE," +
      "	TEACHTYPE, " +

      "	FACULTY_NAME," +
      "	GENERAL_MAJOR," +
      "	SPECIFIC_MAJOR," +
      "	STUDY_LEVEL," +
      "	COURSE_NUMBER," +
      "	FIRST_SEMESTER_CRN_MALE," +
      "	FIRST_SEMESTER_CRN_FEMALE," +
      "	SECOND_SEMESTER_CRN_MALE," +
      "	SECOND_SEMESTER_CRN_FEMALE," +
      "	FEMALE_COURSE_COLLAB_LINK," +
      "	MALE_COURSE_COLLAB_LINK," +
      "	COURSE_NAME," +
      "	STAFF_NAME," +
      "	EMAIL," +
      "	MOBILE," +
      "	STAFF_NUMBER," +
      "	EXAMDAY_PERIOD" +

      "	) VALUES (" +
      "	:APP_LIVESESSION_NO," +
      "	:APP_TERM," +

      "	:PP_DAY," +
      " :APP_DATE," +
      "	:APP_SESSION_NUMBER," +
      "	:APP_SESSION_TIME," +
      "	:STUDIO_NUMBER," +
      "	:GROUPTYPE," +
      "	:SESSIONTYPE," +
      "	:TEACHTYPE," +

      "	:FACULTY_NAME," +
      "	:GENERAL_MAJOR," +
      "	:SPECIFIC_MAJOR," +
      "	:STUDY_LEVEL," +
      "	:COURSE_NUMBER," +
      "	:FIRST_SEMESTER_CRN_MALE," +
      "	:FIRST_SEMESTER_CRN_FEMALE," +
      "	:SECOND_SEMESTER_CRN_MALE," +
      "	:SECOND_SEMESTER_CRN_FEMALE," +
      "	:FEMALE_COURSE_COLLAB_LINK," +
      "	:MALE_COURSE_COLLAB_LINK," +
      "	:COURSE_NAME," +
      "	:STAFF_NAME," +
      "	:EMAIL," +
      "	:MOBILE," +
      "	:STAFF_NUMBER," +
      "	:EXAMDAY_PERIOD" +

      "	)";
                int res3 = 0;
                for (int i = 0; i < selectExistMyLiveSessionList.OrderBy(x => x.APP_SESSION_NUMBER).Count(); i++)
                {
                    List<OracleParameter> myParams = new List<OracleParameter>();

                    myParams.Add(new OracleParameter("APP_LIVESESSION_NO", selectExistMyLiveSessionList[i].APP_LIVESESSION_NO));
                    myParams.Add(new OracleParameter("APP_TERM", selectExistMyLiveSessionList[i].APP_TERM));
                    //replace new values 27 field
                    myParams.Add(new OracleParameter("APP_DAY", selectExistLiveSessionList[i].APP_DAY));
                    myParams.Add(new OracleParameter("APP_DATE", selectExistLiveSessionList[i].APP_DATE));
                    myParams.Add(new OracleParameter("APP_SESSION_NUMBER", selectExistLiveSessionList[i].APP_SESSION_NUMBER));
                    myParams.Add(new OracleParameter("APP_SESSION_TIME", selectExistLiveSessionList[i].APP_SESSION_TIME));
                    myParams.Add(new OracleParameter("STUDIO_NUMBER", selectExistLiveSessionList[i].STUDIO_NUMBER));
                    //
                    if (new_LiveSession.GroupType != GroupDataType.EMPTY)
                    {
                        myParams.Add(new OracleParameter("GROUPTYPE", selectExistLiveSessionList[i].GROUPTYPE.ToString()));
                        myParams.Add(new OracleParameter("SESSIONTYPE", selectExistLiveSessionList[i].SESSIONTYPE.ToString()));
                        myParams.Add(new OracleParameter("TEACHTYPE", selectExistLiveSessionList[i].TEACHTYPE.ToString()));
                    }
                    else
                    {
                        myParams.Add(new OracleParameter("GROUPTYPE", selectExistMyLiveSessionList[i].GROUPTYPE.ToString()));
                        myParams.Add(new OracleParameter("SESSIONTYPE", selectExistMyLiveSessionList[i].SESSIONTYPE.ToString()));
                        myParams.Add(new OracleParameter("TEACHTYPE", selectExistMyLiveSessionList[i].TEACHTYPE.ToString()));
                    }
                    myParams.Add(new OracleParameter("FACULTY_NAME", selectExistMyLiveSessionList[i].FACULTY_NAME));
                    myParams.Add(new OracleParameter("GENERAL_MAJOR", selectExistMyLiveSessionList[i].GENERAL_MAJOR));
                    myParams.Add(new OracleParameter("SPECIFIC_MAJOR", selectExistMyLiveSessionList[i].SPECIFIC_MAJOR));
                    myParams.Add(new OracleParameter("STUDY_LEVEL", selectExistMyLiveSessionList[i].STUDY_LEVEL));
                    myParams.Add(new OracleParameter("COURSE_NUMBER", selectExistMyLiveSessionList[i].COURSE_NUMBER));
                    myParams.Add(new OracleParameter("FIRST_SEMESTER_CRN_MALE", selectExistMyLiveSessionList[i].FIRST_SEMESTER_CRN_MALE));
                    myParams.Add(new OracleParameter("FIRST_SEMESTER_CRN_FEMALE", selectExistMyLiveSessionList[i].FIRST_SEMESTER_CRN_FEMALE));
                    myParams.Add(new OracleParameter("SECOND_SEMESTER_CRN_MALE", selectExistMyLiveSessionList[i].SECOND_SEMESTER_CRN_MALE));
                    myParams.Add(new OracleParameter("SECOND_SEMESTER_CRN_FEMALE", selectExistMyLiveSessionList[i].SECOND_SEMESTER_CRN_FEMALE));
                    myParams.Add(new OracleParameter("FEMALE_COURSE_COLLAB_LINK", selectExistMyLiveSessionList[i].FEMALE_COURSE_COLLAB_LINK));
                    myParams.Add(new OracleParameter("MALE_COURSE_COLLAB_LINK", selectExistMyLiveSessionList[i].MALE_COURSE_COLLAB_LINK));
                    myParams.Add(new OracleParameter("COURSE_NAME", selectExistMyLiveSessionList[i].COURSE_NAME));
                    myParams.Add(new OracleParameter("STAFF_NAME", selectExistMyLiveSessionList[i].STAFF_NAME));
                    myParams.Add(new OracleParameter("EMAIL", selectExistMyLiveSessionList[i].EMAIL));
                    myParams.Add(new OracleParameter("MOBILE", selectExistMyLiveSessionList[i].MOBILE));
                    myParams.Add(new OracleParameter("STAFF_NUMBER", selectExistMyLiveSessionList[i].STAFF_NUMBER));
                    myParams.Add(new OracleParameter("EXAMDAY_PERIOD", selectExistMyLiveSessionList[i].EXAMDAY_PERIOD));



                    res3 = ld.ExecuteNonQueryText(command, myParams);
                }

                for (int i = 0; i < selectExistLiveSessionList.OrderBy(x => x.APP_SESSION_NUMBER).Count(); i++)
                {
                    List<OracleParameter> myParams = new List<OracleParameter>();

                    myParams.Add(new OracleParameter("APP_LIVESESSION_NO", selectExistLiveSessionList[i].APP_LIVESESSION_NO));
                    myParams.Add(new OracleParameter("APP_TERM", selectExistLiveSessionList[i].APP_TERM));
                    //replace new values
                    myParams.Add(new OracleParameter("APP_DAY", selectExistMyLiveSessionList[i].APP_DAY));
                    myParams.Add(new OracleParameter("APP_DATE", selectExistMyLiveSessionList[i].APP_DATE));
                    myParams.Add(new OracleParameter("APP_SESSION_NUMBER", selectExistMyLiveSessionList[i].APP_SESSION_NUMBER));
                    myParams.Add(new OracleParameter("APP_SESSION_TIME", selectExistMyLiveSessionList[i].APP_SESSION_TIME));
                    myParams.Add(new OracleParameter("STUDIO_NUMBER", selectExistMyLiveSessionList[i].STUDIO_NUMBER));
                    //
                    if (new_LiveSession.GroupType != GroupDataType.EMPTY)
                    {
                        myParams.Add(new OracleParameter("GROUPTYPE", selectExistMyLiveSessionList[i].GROUPTYPE.ToString()));
                        myParams.Add(new OracleParameter("SESSIONTYPE", selectExistMyLiveSessionList[i].SESSIONTYPE.ToString()));
                        myParams.Add(new OracleParameter("TEACHTYPE", selectExistMyLiveSessionList[i].TEACHTYPE.ToString()));
                    }
                    else
                    {
                        myParams.Add(new OracleParameter("GROUPTYPE", selectExistLiveSessionList[i].GROUPTYPE.ToString()));
                        myParams.Add(new OracleParameter("SESSIONTYPE", selectExistLiveSessionList[i].SESSIONTYPE.ToString()));
                        myParams.Add(new OracleParameter("TEACHTYPE", selectExistLiveSessionList[i].TEACHTYPE.ToString()));
                    }


                    myParams.Add(new OracleParameter("FACULTY_NAME", selectExistLiveSessionList[i].FACULTY_NAME));
                    myParams.Add(new OracleParameter("GENERAL_MAJOR", selectExistLiveSessionList[i].GENERAL_MAJOR));
                    myParams.Add(new OracleParameter("SPECIFIC_MAJOR", selectExistLiveSessionList[i].SPECIFIC_MAJOR));
                    myParams.Add(new OracleParameter("STUDY_LEVEL", selectExistLiveSessionList[i].STUDY_LEVEL));
                    myParams.Add(new OracleParameter("COURSE_NUMBER", selectExistLiveSessionList[i].COURSE_NUMBER));
                    myParams.Add(new OracleParameter("FIRST_SEMESTER_CRN_MALE", selectExistLiveSessionList[i].FIRST_SEMESTER_CRN_MALE));
                    myParams.Add(new OracleParameter("FIRST_SEMESTER_CRN_FEMALE", selectExistLiveSessionList[i].FIRST_SEMESTER_CRN_FEMALE));
                    myParams.Add(new OracleParameter("SECOND_SEMESTER_CRN_MALE", selectExistLiveSessionList[i].SECOND_SEMESTER_CRN_MALE));
                    myParams.Add(new OracleParameter("SECOND_SEMESTER_CRN_FEMALE", selectExistLiveSessionList[i].SECOND_SEMESTER_CRN_FEMALE));
                    myParams.Add(new OracleParameter("FEMALE_COURSE_COLLAB_LINK", selectExistLiveSessionList[i].FEMALE_COURSE_COLLAB_LINK));
                    myParams.Add(new OracleParameter("MALE_COURSE_COLLAB_LINK", selectExistLiveSessionList[i].MALE_COURSE_COLLAB_LINK));
                    myParams.Add(new OracleParameter("COURSE_NAME", selectExistLiveSessionList[i].COURSE_NAME));
                    myParams.Add(new OracleParameter("STAFF_NAME", selectExistLiveSessionList[i].STAFF_NAME));
                    myParams.Add(new OracleParameter("EMAIL", selectExistLiveSessionList[i].EMAIL));
                    myParams.Add(new OracleParameter("MOBILE", selectExistLiveSessionList[i].MOBILE));
                    myParams.Add(new OracleParameter("STAFF_NUMBER", selectExistLiveSessionList[i].STAFF_NUMBER));
                    myParams.Add(new OracleParameter("EXAMDAY_PERIOD", selectExistLiveSessionList[i].EXAMDAY_PERIOD));



                    res3 = ld.ExecuteNonQueryText(command, myParams);
                }



                #endregion


                return res3 > 0 ? "تم تغيير مياعد المحاضرة" : "حدث خطأ , لم يتم تغيير ميعاد المحاضرة المباشرة";
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }

        private static string GetSessionNames(string aPP_SESSION_NUMBER)
        {
            switch (Convert.ToInt32(aPP_SESSION_NUMBER.Replace("session", string.Empty)))
            {
                case 1:
                case 2:
                    return "'session1','session2'";
                case 3:
                case 4:
                    return "'session3','session4'";
                case 5:
                case 6:
                    return "'session5','session6'";

            }
            return null;
        }



        internal static ChangeLiveSession GetDocReasonPath(AG_LIVESESSIONEXCEL liveSession)
        {
            AdminOperations ld = new AdminOperations();

            string SelectSql = "";
            DataSet ds = null;

            SelectSql = "SELECT * FROM AG_LIVESESSIONCONFIRMCHANGE " +
         " WHERE APP_TERM = :APP_TERM AND APP_LIVESESSION_NO=:APP_LIVESESSION_NO  AND APP_DAY=:APP_DAY AND STUDIO_NUMBER=:STUDIO_NUMBER AND COURSE_NUMBER=:COURSE_NUMBER AND STUDY_LEVEL=:STUDY_LEVEL AND  EMAIL LIKE :EMAIL" +
         " AND APP_SESSION_NUMBER=:APP_SESSION_NUMBER ";

            ds = ld.ExecuteSelectCommand(SelectSql,
                        new OracleParameter("APP_TERM", liveSession.APP_TERM),
                        new OracleParameter("APP_LIVESESSION_NO", liveSession.APP_LIVESESSION_NO),
                        new OracleParameter("APP_DAY", liveSession.APP_DAY),
                        new OracleParameter("STUDIO_NUMBER", liveSession.STUDIO_NUMBER),
                        new OracleParameter("COURSE_NUMBER", liveSession.COURSE_NUMBER),
                        new OracleParameter("STUDY_LEVEL", liveSession.STUDY_LEVEL),
                        new OracleParameter("EMAIL", liveSession.EMAIL + "%"),
                        new OracleParameter("APP_SESSION_NUMBER", liveSession.APP_SESSION_NUMBER)
                );

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ChangeLiveSession obj = null;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        obj = new ChangeLiveSession();
                        obj.APP_DATE = Convert.ToDateTime(dr["APP_DATE"].ToString());
                        obj.APP_DAY = dr["APP_DAY"].ToString();
                        obj.APP_LIVESESSION_NO = dr["APP_LIVESESSION_NO"].ToString();
                        obj.APP_SESSION_NUMBER = dr["APP_SESSION_NUMBER"].ToString();
                        obj.APP_SESSION_TIME = dr["APP_SESSION_TIME"].ToString();
                        obj.APP_TERM = dr["APP_TERM"].ToString();
                        obj.COURSE_NAME = dr["COURSE_NAME"].ToString();
                        obj.COURSE_NUMBER = dr["COURSE_NUMBER"].ToString();
                        obj.EMAIL = dr["EMAIL"].ToString();
                        obj.EXAMDAY_PERIOD = dr["EXAMDAY_PERIOD"].ToString();
                        obj.FACULTY_NAME = dr["FACULTY_NAME"].ToString();
                        obj.FEMALE_COURSE_COLLAB_LINK = dr["FEMALE_COURSE_COLLAB_LINK"].ToString();
                        obj.FIRST_SEMESTER_CRN_FEMALE = dr["FIRST_SEMESTER_CRN_FEMALE"].ToString();
                        obj.FIRST_SEMESTER_CRN_MALE = dr["FIRST_SEMESTER_CRN_MALE"].ToString();
                        obj.GENERAL_MAJOR = dr["GENERAL_MAJOR"].ToString();
                        obj.GROUPTYPE = dr["GROUPTYPE"].ToString();
                        //obj.INSERT_DATE =Convert.ToDateTime( dr["INSERT_DATE"].ToString());
                        obj.MALE_COURSE_COLLAB_LINK = dr["MALE_COURSE_COLLAB_LINK"].ToString();
                        obj.MOBILE = dr["MOBILE"].ToString();
                        obj.NEW_APP_DATE = Convert.ToDateTime(dr["NEW_APP_DATE"].ToString());
                        obj.NEW_APP_DAY = dr["NEW_APP_DAY"].ToString();
                        obj.NEW_APP_SESSION_NUMBER = dr["NEW_APP_SESSION_NUMBER"].ToString();
                        obj.NEW_APP_SESSION_TIME = dr["NEW_APP_SESSION_TIME"].ToString();
                        obj.NEW_CONFIRM_STATUS = dr["NEW_CONFIRM_STATUS"].ToString();
                        obj.NEW_COURSE_NAME = dr["NEW_COURSE_NAME"].ToString();
                        obj.NEW_COURSE_NUMBER = dr["NEW_COURSE_NUMBER"].ToString();
                        obj.NEW_DOC_PATH = dr["NEW_DOC_PATH"].ToString();
                        obj.NEW_EMAIL = dr["NEW_EMAIL"].ToString();
                        obj.NEW_EXAMDAY_PERIOD = dr["NEW_EXAMDAY_PERIOD"].ToString();
                        obj.NEW_FACULTY_NAME = dr["NEW_FACULTY_NAME"].ToString();
                        obj.NEW_FEMALE_COURSE_COLLAB_LINK = dr["NEW_FEMALE_COURSE_COLLAB_LINK"].ToString();
                        obj.NEW_FIRST_SEMESTER_CRN_FEMALE = dr["NEW_FIRST_SEMESTER_CRN_FEMALE"].ToString();
                        obj.NEW_FIRST_SEMESTER_CRN_MALE = dr["NEW_FIRST_SEMESTER_CRN_MALE"].ToString();
                        obj.NEW_GENERAL_MAJOR = dr["NEW_GENERAL_MAJOR"].ToString();
                        obj.NEW_GROUPTYPE = dr["NEW_GROUPTYPE"].ToString();
                        obj.NEW_IS_ALREADY_EMPTY = dr["NEW_IS_ALREADY_EMPTY"].ToString();
                        obj.NEW_MALE_COURSE_COLLAB_LINK = dr["NEW_MALE_COURSE_COLLAB_LINK"].ToString();
                        obj.NEW_MOBILE = dr["NEW_MOBILE"].ToString();
                        obj.NEW_NOTES = dr["NEW_NOTES"].ToString();
                        obj.NEW_SECOND_SEMESTER_CRN_FEMALE = dr["NEW_SECOND_SEMESTER_CRN_FEMALE"].ToString();
                        obj.NEW_SECOND_SEMESTER_CRN_MALE = dr["NEW_SECOND_SEMESTER_CRN_MALE"].ToString();
                        obj.NEW_SESSIONTYPE = dr["NEW_SESSIONTYPE"].ToString();
                        obj.NEW_SPECIFIC_MAJOR = dr["NEW_SPECIFIC_MAJOR"].ToString();
                        obj.NEW_STAFF_NAME = dr["NEW_STAFF_NAME"].ToString();
                        obj.NEW_STAFF_NUMBER = dr["NEW_STAFF_NUMBER"].ToString();
                        obj.NEW_STUDIO_NUMBER = dr["NEW_STUDIO_NUMBER"].ToString();
                        obj.NEW_STUDY_LEVEL = dr["NEW_STUDY_LEVEL"].ToString();
                        obj.NEW_TEACHTYPE = dr["NEW_TEACHTYPE"].ToString();
                        obj.NEW_WITHIN_3_DAYS = dr["NEW_WITHIN_3_DAYS"].ToString();
                        obj.SECOND_SEMESTER_CRN_FEMALE = dr["SECOND_SEMESTER_CRN_FEMALE"].ToString();
                        obj.SECOND_SEMESTER_CRN_MALE = dr["SECOND_SEMESTER_CRN_MALE"].ToString();
                        obj.SESSIONTYPE = dr["SESSIONTYPE"].ToString();
                        obj.SPECIFIC_MAJOR = dr["SPECIFIC_MAJOR"].ToString();
                        obj.STAFF_NAME = dr["STAFF_NAME"].ToString();
                        obj.STAFF_NUMBER = dr["STAFF_NUMBER"].ToString();
                        obj.STUDIO_NUMBER = dr["STUDIO_NUMBER"].ToString();
                        obj.STUDY_LEVEL = dr["STUDY_LEVEL"].ToString();
                        obj.TEACHTYPE = dr["TEACHTYPE"].ToString();


                        return obj;
                    }

                    return null;
                }
            }
            return null;

        }
    }
}