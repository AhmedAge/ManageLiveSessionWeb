
using ManageLiveSessionWeb.utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ManageLiveSessionWeb.DAL.D_liveDoctors
{
    public class LiveDoctors : DataServiceBase
    {
        public static DataSet SelectDoctorSessions(string email)
        {
            LiveDoctors ld = new LiveDoctors();
            string sql = "select * from AG_LIVESESSIONEXCEL where EMAIL = :email order by APP_DAY ,APP_SESSION_NUMBER";

            email = email.Contains("@kfu.edu.sa") ? email : email + "@kfu.edu.sa";

            DataSet ds = ld.ExecuteSelectCommand(sql, new OracleParameter("email", email));

            return ds;
        }


        public static DataSet SelectDoctorCourseType(string course_ID)
        {
            LiveDoctors ld = new LiveDoctors();
            string sql = "select COURSE_ID, count(*) COUNT from shaban123.LMS_ENROLLS_4010@VLS2BBLCLDLNK  where COURSE_ID like :courseid  group by COURSE_ID ";

            DataSet ds = ld.ExecuteSelectCommand(sql, new OracleParameter("courseid", course_ID + "%"));

            return ds;
        }

        internal static DataSet AvailableDoctorSessions(AG_LIVESESSIONEXCEL liveSession)
        {
            LiveDoctors ld = new LiveDoctors();
            string sql =
            //"SELECT * FROM AG_LIVESESSIONEXCEL WHERE " +
            //                    " APP_TERM =:APP_TERM AND" +
            //                    " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
            //                    " SPECIFIC_MAJOR =:SPECIFIC_MAJOR AND" +
            //                    " GENERAL_MAJOR=:GENERAL_MAJOR AND" +
            //                    " STUDY_LEVEL=:STUDY_LEVEL AND" +
            //                    " APP_DAY <> :APP_DAY AND" +
            //                    " STAFF_NAME <>  :STAFF_NAME AND" +
            //                    " GROUPTYPE <> 'projects' AND" +
            //                    " SESSIONTYPE =:SESSIONTYPE  " +
            //                    "UNION ALL SELECT * FROM AG_LIVESESSIONEXCEL WHERE COURSE_NUMBER IS NULL";
            "SELECT *  FROM AG_LIVESESSIONEXCEL WHERE course_number IS NULL AND (app_day,studio_number) IN (SELECT app_day, studio_number FROM (  SELECT app_day,studio_number, COUNT(1) FROM AG_LIVESESSIONEXCEL WHERE course_number IS NULL AND SESSIONTYPE = 'ALL' GROUP BY app_day , studio_number HAVING COUNT (1) > 1))" +
            " AND SYSDATE <= APP_DATE AND APP_TERM=:APP_TERM AND  APP_LIVESESSION_NO=:APP_LIVESESSION_NO  MINUS SELECT * FROM AG_LIVESESSIONEXCEL WHERE STUDY_LEVEL = :STUDY_LEVEL" +
            " AND APP_SESSION_NUMBER in (  " + GetSessionNames(liveSession.APP_SESSION_NUMBER) + " ) " +
            " AND APP_DAY = :APP_DAY " +
            " AND GENERAL_MAJOR = :GENERAL_MAJOR " +
            " AND SPECIFIC_MAJOR = :SPECIFIC_MAJOR " +
            " AND APP_TERM=:APP_TERM AND  APP_LIVESESSION_NO=:APP_LIVESESSION_NO ";


            List<OracleParameter> params1 = new List<OracleParameter>();

            params1.Add(new OracleParameter("APP_TERM", liveSession.APP_TERM));
            params1.Add(new OracleParameter("APP_LIVESESSION_NO", liveSession.APP_LIVESESSION_NO));
            params1.Add(new OracleParameter("SPECIFIC_MAJOR", liveSession.SPECIFIC_MAJOR));
            params1.Add(new OracleParameter("GENERAL_MAJOR", liveSession.GENERAL_MAJOR));
            params1.Add(new OracleParameter("STUDY_LEVEL", liveSession.STUDY_LEVEL));
            params1.Add(new OracleParameter("APP_DAY", liveSession.APP_DAY));
            //params1.Add(new OracleParameter("STAFF_NAME", liveSession.STAFF_NAME));
            //params1.Add(new OracleParameter("SESSIONTYPE", liveSession.SessionType.ToString()));

            DataSet ds = ld.ExecuteSelectCommandParamList(sql, params1);

            return ds;
        }

        internal static DataSet AvailableDoctorSessionsProjects(AG_LIVESESSIONEXCEL liveSession)
        {
            LiveDoctors ld = new LiveDoctors();
            string sql =
            //"SELECT * FROM AG_LIVESESSIONEXCEL WHERE " +
            //                    " APP_TERM =:APP_TERM AND" +
            //                    " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
            //                    " SPECIFIC_MAJOR =:SPECIFIC_MAJOR AND" +
            //                    " GENERAL_MAJOR=:GENERAL_MAJOR AND" +
            //                    " STUDY_LEVEL=:STUDY_LEVEL AND" +
            //                    " APP_DAY <> :APP_DAY AND" +
            //                    " STAFF_NAME <>  :STAFF_NAME AND" +
            //                    " GROUPTYPE <> 'projects' AND" +
            //                    " SESSIONTYPE =:SESSIONTYPE  " +
            //                    "UNION ALL SELECT * FROM AG_LIVESESSIONEXCEL WHERE COURSE_NUMBER IS NULL";
            " SELECT * FROM AG_LIVESESSIONEXCEL WHERE COURSE_NUMBER IS NULL AND APP_SESSION_TIME like :APP_SESSION_TIME AND sysdate <= app_date";


            List<OracleParameter> params1 = new List<OracleParameter>();

            params1.Add(new OracleParameter("APP_SESSION_TIME", liveSession.APP_SESSION_TIME.Split('[')[0].Trim() + "%"));
            //params1.Add(new OracleParameter("APP_LIVESESSION_NO", liveSession.APP_LIVESESSION_NO));
            //params1.Add(new OracleParameter("SPECIFIC_MAJOR", liveSession.SPECIFIC_MAJOR));
            //params1.Add(new OracleParameter("GENERAL_MAJOR", liveSession.GENERAL_MAJOR));
            //params1.Add(new OracleParameter("STUDY_LEVEL", liveSession.STUDY_LEVEL));
            //params1.Add(new OracleParameter("APP_DAY", liveSession.APP_DAY));
            //params1.Add(new OracleParameter("STAFF_NAME", liveSession.STAFF_NAME));
            //params1.Add(new OracleParameter("SESSIONTYPE", liveSession.SessionType.ToString()));

            DataSet ds = ld.ExecuteSelectCommandParamList(sql, params1);

            return ds;
        }

        internal static string ChangeLiveSessionsTime(AG_LIVESESSIONEXCEL myLiveSession, AG_LIVESESSIONEXCEL new_LiveSession, string changeType, bool within_3Days, Execuses execuses)
        {
            try
            {
                LiveDoctors ld = new LiveDoctors();

                #region check for same time doctor change if already exists
                
                string sqlCheck = "";

                if (changeType == "1")
                {
                    sqlCheck = "SELECT * FROM AG_LIVESESSIONCONFIRMCHANGE WHERE " +
                              " APP_TERM =:APP_TERM AND" +
                              " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                              " NEW_APP_SESSION_TIME LIKE :APP_SESSION_TIME AND " +
                              " NEW_APP_DAY =:APP_DAY AND " +
                              " NEW_STUDIO_NUMBER =:STUDIO_NUMBER  AND " +
                              " NEW_APP_SESSION_NUMBER =:APP_SESSION_NUMBER ";

                }
                else if (changeType == "2")
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
                if (changeType == "1")
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
                        if (changeType == "1")
                        {
                            sql3 = "SELECT * FROM AG_LIVESESSIONEXCEL WHERE " +
                                        " APP_TERM =:APP_TERM AND" +
                                        " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                                        " STUDIO_NUMBER =:STUDIO_NUMBER AND " +
                                        " APP_DAY =:APP_DAY AND" +
                                        " GROUPTYPE =:GROUPTYPE  AND  APP_SESSION_NUMBER =:APP_SESSION_NUMBER";

                        }
                        else if (changeType == "2")
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
                        if (changeType == "1")
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

                if (changeType == "1")
                {
                 sqlMy=   "SELECT * FROM AG_LIVESESSIONEXCEL WHERE " +
                                  " APP_TERM =:APP_TERM AND" +
                                  " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                                  " SPECIFIC_MAJOR =:SPECIFIC_MAJOR AND" +
                                  " GENERAL_MAJOR=:GENERAL_MAJOR AND" +
                                  " STUDY_LEVEL=:STUDY_LEVEL AND" +
                                  " APP_DAY =:APP_DAY AND" +
                                  " STAFF_NAME =:STAFF_NAME AND" +
                                  " SESSIONTYPE =:SESSIONTYPE AND  APP_SESSION_NUMBER =:APP_SESSION_NUMBER";
                }
                else if(changeType == "2")
                {
                    sqlMy = "SELECT * FROM AG_LIVESESSIONEXCEL WHERE " +
                               " APP_TERM =:APP_TERM AND" +
                               " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                               " SPECIFIC_MAJOR =:SPECIFIC_MAJOR AND" +
                               " GENERAL_MAJOR=:GENERAL_MAJOR AND" +
                               " STUDY_LEVEL=:STUDY_LEVEL AND" +
                               " APP_DAY =:APP_DAY AND" +
                               " STAFF_NAME =:STAFF_NAME   AND "+
                               " SESSIONTYPE =:SESSIONTYPE AND APP_SESSION_NUMBER in (  " + GetSessionNames(myLiveSession.APP_SESSION_NUMBER) + "  )";
                }
               
                List<OracleParameter> paramsMy = new List<OracleParameter>();

                paramsMy.Add(new OracleParameter("APP_TERM", myLiveSession.APP_TERM));
                paramsMy.Add(new OracleParameter("APP_LIVESESSION_NO", myLiveSession.APP_LIVESESSION_NO));
                paramsMy.Add(new OracleParameter("SPECIFIC_MAJOR", myLiveSession.SPECIFIC_MAJOR));
                paramsMy.Add(new OracleParameter("GENERAL_MAJOR", myLiveSession.GENERAL_MAJOR));
                paramsMy.Add(new OracleParameter("STUDY_LEVEL", myLiveSession.STUDY_LEVEL));
                paramsMy.Add(new OracleParameter("APP_DAY", myLiveSession.APP_DAY));
                paramsMy.Add(new OracleParameter("STAFF_NAME", myLiveSession.STAFF_NAME));
                paramsMy.Add(new OracleParameter("SESSIONTYPE", myLiveSession.SessionType.ToString()));
                if (changeType == "1")
                {
                    paramsMy.Add(new OracleParameter("APP_SESSION_NUMBER", myLiveSession.APP_SESSION_NUMBER.ToString()));
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

                #region insert Audit Data old and new (FROM , TO) live session
                string cmd = "INSERT INTO AG_LIVESESSIONCONFIRMCHANGE(" +
                  " APP_TERM, " +
    "APP_LIVESESSION_NO, " +
    "APP_DATE, " +
    "APP_DAY, " +
    "APP_SESSION_NUMBER, " +
    "APP_SESSION_TIME, " +
    "STUDIO_NUMBER, " +
    "FACULTY_NAME, " +
    "GENERAL_MAJOR, " +
    "SPECIFIC_MAJOR, " +
    "STUDY_LEVEL, " +
    "COURSE_NUMBER, " +
    "FIRST_SEMESTER_CRN_MALE, " +
    "FIRST_SEMESTER_CRN_FEMALE, " +
    "SECOND_SEMESTER_CRN_MALE, " +
    "SECOND_SEMESTER_CRN_FEMALE, " +
    "FEMALE_COURSE_COLLAB_LINK, " +
    "MALE_COURSE_COLLAB_LINK, " +
    "COURSE_NAME, " +
    "STAFF_NAME, " +
    "EMAIL, " +
    "MOBILE, " +
    "STAFF_NUMBER, " +
    "EXAMDAY_PERIOD, " +
    "GROUPTYPE, " +
    "SESSIONTYPE, " +
    "TEACHTYPE, " +
    "NEW_APP_DATE, " +
    "NEW_APP_DAY, " +
    "NEW_APP_SESSION_NUMBER, " +
    "NEW_APP_SESSION_TIME, " +
    "NEW_STUDIO_NUMBER, " +
    "NEW_FACULTY_NAME, " +
    "NEW_GENERAL_MAJOR, " +
    "NEW_SPECIFIC_MAJOR, " +
    "NEW_STUDY_LEVEL, " +
    "NEW_COURSE_NUMBER, " +
    "NEW_FIRST_SEMESTER_CRN_MALE, " +
    "NEW_FIRST_SEMESTER_CRN_FEMALE, " +
    "NEW_SECOND_SEMESTER_CRN_MALE, " +
    "NEW_SECOND_SEMESTER_CRN_FEMALE, " +
    "NEW_FEMALE_COURSE_COLLAB_LINK, " +
    "NEW_MALE_COURSE_COLLAB_LINK, " +
    "NEW_COURSE_NAME, " +
    "NEW_STAFF_NAME, " +
    "NEW_EMAIL, " +
    "NEW_MOBILE, " +
    "NEW_STAFF_NUMBER, " +
    "NEW_EXAMDAY_PERIOD, " +
    "NEW_GROUPTYPE, " +
    "NEW_SESSIONTYPE, " +
    "NEW_TEACHTYPE, " +
    "NEW_NOTES, " +
    "NEW_DOC_PATH, " +
    "NEW_CONFIRM_STATUS, " +
    "NEW_IS_ALREADY_EMPTY, " +
    "NEW_WITHIN_3_DAYS " +
    " ) VALUES (" +
    ":APP_TERM, " +
    ":APP_LIVESESSION_NO, " +
    ":APP_DATE, " +
    ":APP_DAY, " +
    ":APP_SESSION_NUMBER, " +
    ":APP_SESSION_TIME, " +
    ":STUDIO_NUMBER, " +
    ":FACULTY_NAME, " +
    ":GENERAL_MAJOR, " +
    ":SPECIFIC_MAJOR, " +
    ":STUDY_LEVEL, " +
    ":COURSE_NUMBER, " +
    ":FIRST_SEMESTER_CRN_MALE, " +
    ":FIRST_SEMESTER_CRN_FEMALE, " +
    ":SECOND_SEMESTER_CRN_MALE, " +
    ":SECOND_SEMESTER_CRN_FEMALE, " +
    ":FEMALE_COURSE_COLLAB_LINK, " +
    ":MALE_COURSE_COLLAB_LINK, " +
    ":COURSE_NAME, " +
    ":STAFF_NAME, " +
    ":EMAIL, " +
    ":MOBILE, " +
    ":STAFF_NUMBER, " +
    ":EXAMDAY_PERIOD, " +
    ":GROUPTYPE, " +
    ":SESSIONTYPE, " +
    ":TEACHTYPE, " +
    ":NEW_APP_DATE, " +
    ":NEW_APP_DAY, " +
    ":NEW_APP_SESSION_NUMBER, " +
    ":NEW_APP_SESSION_TIME, " +
    ":NEW_STUDIO_NUMBER, " +
    ":NEW_FACULTY_NAME, " +
    ":NEW_GENERAL_MAJOR, " +
    ":NEW_SPECIFIC_MAJOR, " +
    ":NEW_STUDY_LEVEL, " +
    ":NEW_COURSE_NUMBER, " +
    ":NEW_FIRST_SEMESTER_CRN_MALE, " +
    ":NEW_FIRST_SEMESTER_CRN_FEMALE, " +
    ":NEW_SECOND_SEMESTER_CRN_MALE, " +
    ":NEW_SECOND_SEMESTER_CRN_FEMALE, " +
    ":NEW_FEMALE_COURSE_COLLAB_LINK, " +
    ":NEW_MALE_COURSE_COLLAB_LINK, " +
    ":NEW_COURSE_NAME, " +
    ":NEW_STAFF_NAME, " +
    ":NEW_EMAIL, " +
    ":NEW_MOBILE, " +
    ":NEW_STAFF_NUMBER, " +
    ":NEW_EXAMDAY_PERIOD, " +
    ":NEW_GROUPTYPE, " +
    ":NEW_SESSIONTYPE, " +
    ":NEW_TEACHTYPE, " +
    ":NEW_NOTES, " +
    ":NEW_DOC_PATH, " +
    ":NEW_CONFIRM_STATUS, " +
    ":NEW_IS_ALREADY_EMPTY, " +
    ":NEW_WITHIN_3_DAYS ) ";

                List<OracleParameter> myParamsLog = null;

                for (int i = 0; i < selectExistMyLiveSessionList.Count(); i++)
                {
                    myParamsLog = new List<OracleParameter>();

                    myParamsLog.Add(new OracleParameter("APP_TERM", selectExistMyLiveSessionList[i].APP_TERM));//old live
                    myParamsLog.Add(new OracleParameter("APP_LIVESESSION_NO", selectExistMyLiveSessionList[i].APP_LIVESESSION_NO));
                    myParamsLog.Add(new OracleParameter("APP_DATE", selectExistMyLiveSessionList[i].APP_DATE));
                    myParamsLog.Add(new OracleParameter("APP_DAY", selectExistMyLiveSessionList[i].APP_DAY));
                    myParamsLog.Add(new OracleParameter("APP_SESSION_NUMBER", selectExistMyLiveSessionList[i].APP_SESSION_NUMBER));
                    myParamsLog.Add(new OracleParameter("APP_SESSION_TIME", selectExistMyLiveSessionList[i].APP_SESSION_TIME));
                    myParamsLog.Add(new OracleParameter("STUDIO_NUMBER", selectExistMyLiveSessionList[i].STUDIO_NUMBER));
                    myParamsLog.Add(new OracleParameter("FACULTY_NAME", selectExistMyLiveSessionList[i].FACULTY_NAME));
                    myParamsLog.Add(new OracleParameter("GENERAL_MAJOR", selectExistMyLiveSessionList[i].GENERAL_MAJOR));
                    myParamsLog.Add(new OracleParameter("SPECIFIC_MAJOR", selectExistMyLiveSessionList[i].SPECIFIC_MAJOR));
                    myParamsLog.Add(new OracleParameter("STUDY_LEVEL", selectExistMyLiveSessionList[i].STUDY_LEVEL));
                    myParamsLog.Add(new OracleParameter("COURSE_NUMBER", selectExistMyLiveSessionList[i].COURSE_NUMBER));
                    myParamsLog.Add(new OracleParameter("FIRST_SEMESTER_CRN_MALE", selectExistMyLiveSessionList[i].FIRST_SEMESTER_CRN_MALE));
                    myParamsLog.Add(new OracleParameter("FIRST_SEMESTER_CRN_FEMALE", selectExistMyLiveSessionList[i].FIRST_SEMESTER_CRN_FEMALE));
                    myParamsLog.Add(new OracleParameter("SECOND_SEMESTER_CRN_MALE", selectExistMyLiveSessionList[i].SECOND_SEMESTER_CRN_MALE));
                    myParamsLog.Add(new OracleParameter("SECOND_SEMESTER_CRN_FEMALE", selectExistMyLiveSessionList[i].SECOND_SEMESTER_CRN_FEMALE));
                    myParamsLog.Add(new OracleParameter("FEMALE_COURSE_COLLAB_LINK", selectExistMyLiveSessionList[i].FEMALE_COURSE_COLLAB_LINK));
                    myParamsLog.Add(new OracleParameter("MALE_COURSE_COLLAB_LINK", selectExistMyLiveSessionList[i].MALE_COURSE_COLLAB_LINK));
                    myParamsLog.Add(new OracleParameter("COURSE_NAME", selectExistMyLiveSessionList[i].COURSE_NAME));
                    myParamsLog.Add(new OracleParameter("STAFF_NAME", selectExistMyLiveSessionList[i].STAFF_NAME));
                    myParamsLog.Add(new OracleParameter("EMAIL", selectExistMyLiveSessionList[i].EMAIL));
                    myParamsLog.Add(new OracleParameter("MOBILE", selectExistMyLiveSessionList[i].MOBILE));
                    myParamsLog.Add(new OracleParameter("STAFF_NUMBER", selectExistMyLiveSessionList[i].STAFF_NUMBER));
                    myParamsLog.Add(new OracleParameter("EXAMDAY_PERIOD", selectExistMyLiveSessionList[i].EXAMDAY_PERIOD));
                    myParamsLog.Add(new OracleParameter("GROUPTYPE", selectExistMyLiveSessionList[i].GROUPTYPE));
                    myParamsLog.Add(new OracleParameter("SESSIONTYPE", selectExistMyLiveSessionList[i].SESSIONTYPE));
                    myParamsLog.Add(new OracleParameter("TEACHTYPE", selectExistMyLiveSessionList[i].TEACHTYPE));

                    myParamsLog.Add(new OracleParameter("NEW_APP_DATE", selectExistLiveSessionList[i].APP_DATE));//new live
                    myParamsLog.Add(new OracleParameter("NEW_APP_DAY", selectExistLiveSessionList[i].APP_DAY));
                    myParamsLog.Add(new OracleParameter("NEW_APP_SESSION_NUMBER", selectExistLiveSessionList[i].APP_SESSION_NUMBER));
                    myParamsLog.Add(new OracleParameter("NEW_APP_SESSION_TIME", selectExistLiveSessionList[i].APP_SESSION_TIME));
                    myParamsLog.Add(new OracleParameter("NEW_STUDIO_NUMBER", selectExistLiveSessionList[i].STUDIO_NUMBER));
                    myParamsLog.Add(new OracleParameter("NEW_FACULTY_NAME", selectExistLiveSessionList[i].FACULTY_NAME));
                    myParamsLog.Add(new OracleParameter("NEW_GENERAL_MAJOR", selectExistLiveSessionList[i].GENERAL_MAJOR));
                    myParamsLog.Add(new OracleParameter("NEW_SPECIFIC_MAJOR", selectExistLiveSessionList[i].SPECIFIC_MAJOR));
                    myParamsLog.Add(new OracleParameter("NEW_STUDY_LEVEL", selectExistLiveSessionList[i].STUDY_LEVEL));
                    myParamsLog.Add(new OracleParameter("NEW_COURSE_NUMBER", selectExistLiveSessionList[i].COURSE_NUMBER));
                    myParamsLog.Add(new OracleParameter("NEW_FIRST_SEMESTER_CRN_MALE", selectExistLiveSessionList[i].FIRST_SEMESTER_CRN_MALE));
                    myParamsLog.Add(new OracleParameter("NEW_FIRST_SEMESTER_CRN_FEMALE", selectExistLiveSessionList[i].FIRST_SEMESTER_CRN_FEMALE));
                    myParamsLog.Add(new OracleParameter("NEW_SECOND_SEMESTER_CRN_MALE", selectExistLiveSessionList[i].SECOND_SEMESTER_CRN_MALE));
                    myParamsLog.Add(new OracleParameter("NEW_SECOND_SEMESTER_CRN_FEMALE", selectExistLiveSessionList[i].SECOND_SEMESTER_CRN_FEMALE));
                    myParamsLog.Add(new OracleParameter("NEW_FEMALE_COURSE_COLLAB_LINK", selectExistLiveSessionList[i].FEMALE_COURSE_COLLAB_LINK));
                    myParamsLog.Add(new OracleParameter("NEW_MALE_COURSE_COLLAB_LINK", selectExistLiveSessionList[i].MALE_COURSE_COLLAB_LINK));
                    myParamsLog.Add(new OracleParameter("NEW_COURSE_NAME", selectExistLiveSessionList[i].COURSE_NAME));
                    myParamsLog.Add(new OracleParameter("NEW_STAFF_NAME", selectExistLiveSessionList[i].STAFF_NAME));
                    myParamsLog.Add(new OracleParameter("NEW_EMAIL", selectExistLiveSessionList[i].EMAIL));
                    myParamsLog.Add(new OracleParameter("NEW_MOBILE", selectExistLiveSessionList[i].MOBILE));
                    myParamsLog.Add(new OracleParameter("NEW_STAFF_NUMBER", selectExistLiveSessionList[i].STAFF_NUMBER));
                    myParamsLog.Add(new OracleParameter("NEW_EXAMDAY_PERIOD", selectExistLiveSessionList[i].EXAMDAY_PERIOD));
                    myParamsLog.Add(new OracleParameter("NEW_GROUPTYPE", selectExistLiveSessionList[i].GROUPTYPE));
                    myParamsLog.Add(new OracleParameter("NEW_SESSIONTYPE", selectExistLiveSessionList[i].SESSIONTYPE));
                    myParamsLog.Add(new OracleParameter("NEW_TEACHTYPE", selectExistLiveSessionList[i].TEACHTYPE));

                    if (within_3Days)
                    {

                        myParamsLog.Add(new OracleParameter("NEW_NOTES", null));
                        myParamsLog.Add(new OracleParameter("NEW_DOC_PATH", null));
                        myParamsLog.Add(new OracleParameter("NEW_CONFIRM_STATUS", "Y"));
                        myParamsLog.Add(new OracleParameter("NEW_IS_ALREADY_EMPTY", null));
                        myParamsLog.Add(new OracleParameter("NEW_WITHIN_3_DAYS", "Y"));
                    }
                    else
                    {

                        myParamsLog.Add(new OracleParameter("NEW_NOTES", execuses.NEW_NOTES));
                        myParamsLog.Add(new OracleParameter("NEW_DOC_PATH", execuses.NEW_DOC_PATH));
                        myParamsLog.Add(new OracleParameter("NEW_CONFIRM_STATUS", "N"));
                        myParamsLog.Add(new OracleParameter("NEW_IS_ALREADY_EMPTY", "Y"));
                        myParamsLog.Add(new OracleParameter("NEW_WITHIN_3_DAYS", "N"));
                    }

                    int res = ld.ExecuteNonQueryText(cmd, myParamsLog);
                }


                #endregion


                if (within_3Days)
                {
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
                            if (changeType == "1")
                            {
                                sql1 = "DELETE FROM AG_LIVESESSIONEXCEL WHERE " +
                                         " APP_TERM =:APP_TERM AND" +
                                         " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                                         " STUDIO_NUMBER =:STUDIO_NUMBER AND " +
                                         " APP_DAY =:APP_DAY AND" +
                                         " GROUPTYPE =:GROUPTYPE AND  APP_SESSION_NUMBER in (  '" + new_LiveSession.APP_SESSION_NUMBER + "'  )";
                            }
                            else if (changeType == "2")
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



                        int res3 = ld.ExecuteNonQueryText(command, myParams);
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



                        int res3 = ld.ExecuteNonQueryText(command, myParams);
                    }



                    #endregion
                }

                return "Success change";
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }

        internal static int UpdateLiveSessionsCollabLink(AG_LIVESESSIONEXCEL liveSession, string gender)
        {
            LiveDoctors ld = new LiveDoctors();
            string sql1 = "UPDATE AG_LIVESESSIONEXCEL SET COLLAB_FEMALE_OPEN=:COLLAB_FEMALE_OPEN , COLLAB_MALE_OPEN =:COLLAB_MALE_OPEN  WHERE " +
                                  " APP_TERM =:APP_TERM AND" +
                                  " APP_LIVESESSION_NO =:APP_LIVESESSION_NO AND " +
                                  " SPECIFIC_MAJOR =:SPECIFIC_MAJOR AND" +
                                  " GENERAL_MAJOR=:GENERAL_MAJOR AND" +
                                  " STUDY_LEVEL=:STUDY_LEVEL AND" +
                                  " APP_DAY =:APP_DAY AND" +
                                  " STAFF_NAME =:STAFF_NAME AND" +
                                  " APP_SESSION_NUMBER =:APP_SESSION_NUMBER AND" +
                                  " SESSIONTYPE =:SESSIONTYPE ";



            string male = null;
            string female = null;
            if (gender.ToLower() == "male")
                male = "Y";
            else if (gender.ToLower() == "female")
                female = "Y";

            int res = ld.ExecuteNonQueryText(sql1, new OracleParameter("COLLAB_FEMALE_OPEN", female),
                 new OracleParameter("COLLAB_MALE_OPEN", male),
                 new OracleParameter("APP_TERM", liveSession.APP_TERM),
                new OracleParameter("APP_LIVESESSION_NO", liveSession.APP_LIVESESSION_NO),
                new OracleParameter("SPECIFIC_MAJOR", liveSession.SPECIFIC_MAJOR),
                new OracleParameter("GENERAL_MAJOR", liveSession.GENERAL_MAJOR),
                new OracleParameter("STUDY_LEVEL", liveSession.STUDY_LEVEL),
                new OracleParameter("APP_DAY", liveSession.APP_DAY),
                new OracleParameter("STAFF_NAME", liveSession.STAFF_NAME),
                new OracleParameter("APP_SESSION_NUMBER", liveSession.APP_SESSION_NUMBER),
                new OracleParameter("SESSIONTYPE", liveSession.SessionType.ToString()));

            return res;
        }

        internal static DataSet LiveSessionStart(AG_LIVESESSIONEXCEL liveSession)
        {
            LiveDoctors ld = new LiveDoctors();
            string sql =
              "SELECT APP_DATE as LIVE_START FROM AG_LIVESESSIONEXCEL WHERE rownum = 1 AND" +
                                " APP_TERM =:APP_TERM AND" +
                               " APP_LIVESESSION_NO =:APP_LIVESESSION_NO order by APP_DATE";


            List<OracleParameter> params1 = new List<OracleParameter>();

            params1.Add(new OracleParameter("APP_TERM", liveSession.APP_TERM));
            params1.Add(new OracleParameter("APP_LIVESESSION_NO", liveSession.APP_LIVESESSION_NO));

            DataSet ds = ld.ExecuteSelectCommandParamList(sql, params1);

            return ds;
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
    }

}
