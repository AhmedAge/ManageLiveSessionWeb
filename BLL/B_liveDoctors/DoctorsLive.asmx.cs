using ManageLiveSessionWeb.DAL.D_liveDoctors;
using Newtonsoft.Json;
using ReadLiveSessionExcel.utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Services;
using ManageLiveSessionWeb.utility;

namespace ManageLiveSessionWeb.BLL.B_liveDoctors
{
    /// <summary>
    /// Summary description for DoctorsLive
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class DoctorsLive : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string SelectDoctorSessions()
        {
            DataSet ds = LiveDoctors.SelectDoctorSessions(Utilities.Email);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    List<AG_LIVESESSIONEXCEL> aG_LIVESESSIONEXCELlst = new List<AG_LIVESESSIONEXCEL>();
                    AG_LIVESESSIONEXCEL aG = null;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        aG = new AG_LIVESESSIONEXCEL();
                        aG.APP_LIVESESSION_NO = dr["APP_LIVESESSION_NO"].ToString();
                        aG.APP_TERM = dr["APP_TERM"].ToString();
                        aG.APP_DAY = dr["APP_DAY"].ToString();
                        aG.APP_SESSION_NUMBER = dr["APP_SESSION_NUMBER"].ToString();
                        aG.APP_SESSION_TIME = dr["APP_SESSION_TIME"].ToString();
                        aG.STUDIO_NUMBER = dr["STUDIO_NUMBER"].ToString();
                        aG.FACULTY_NAME = dr["FACULTY_NAME"].ToString();
                        aG.GENERAL_MAJOR = dr["GENERAL_MAJOR"].ToString();
                        aG.SPECIFIC_MAJOR = dr["SPECIFIC_MAJOR"].ToString();
                        aG.STUDY_LEVEL = dr["STUDY_LEVEL"].ToString();
                        aG.COURSE_NUMBER = dr["COURSE_NUMBER"].ToString();
                        aG.FIRST_SEMESTER_CRN_MALE = dr["FIRST_SEMESTER_CRN_MALE"].ToString();
                        aG.FIRST_SEMESTER_CRN_FEMALE = dr["FIRST_SEMESTER_CRN_FEMALE"].ToString();
                        aG.SECOND_SEMESTER_CRN_MALE = dr["SECOND_SEMESTER_CRN_MALE"].ToString();
                        aG.SECOND_SEMESTER_CRN_FEMALE = dr["SECOND_SEMESTER_CRN_FEMALE"].ToString();
                        aG.FEMALE_COURSE_COLLAB_LINK = dr["FEMALE_COURSE_COLLAB_LINK"].ToString();
                        aG.MALE_COURSE_COLLAB_LINK = dr["MALE_COURSE_COLLAB_LINK"].ToString();
                        aG.COURSE_NAME = dr["COURSE_NAME"].ToString();
                        aG.STAFF_NAME = dr["STAFF_NAME"].ToString();
                        aG.EMAIL = dr["EMAIL"].ToString();
                        aG.MOBILE = dr["MOBILE"].ToString();
                        aG.STAFF_NUMBER = dr["STAFF_NUMBER"].ToString();
                        aG.EXAMDAY_PERIOD = dr["EXAMDAY_PERIOD"].ToString();
                        aG.APP_DATE = dr["APP_DATE"].ToString();// Convert.ToDateTime(dr["APP_DATE"]);
                        aG.TeachType = dr["TEACHTYPE"].ToString();
                        aG.GroupType = (GroupDataType)Enum.Parse(typeof(GroupDataType), dr["GROUPTYPE"].ToString(), true);
                        aG.SessionType = (SessionType)Enum.Parse(typeof(SessionType), dr["SESSIONTYPE"].ToString(), true);

                        aG_LIVESESSIONEXCELlst.Add(aG);

                    }

                    List<Days> days = new List<Days>();

                    var groupDays1 = (from ee in aG_LIVESESSIONEXCELlst
                                      group ee by new { ee.APP_DATE, ee.APP_DAY } into grp
                                      select new
                                      {
                                          date = grp.Key.APP_DATE,
                                          day = grp.Key.APP_DAY,
                                          count = grp.Count()
                                      }).OrderBy(x => x.date).ToList();
                    Days day = null;
                    foreach (var singleVal in groupDays1)
                    {
                        day = new Days();
                        //  day.DayDate = Convert.ToDateTime(singleVal.date);
                        var da = aG_LIVESESSIONEXCELlst.Where(x => x.APP_DAY == singleVal.day).FirstOrDefault();
                        day.DayDate = Convert.ToDateTime(da.APP_DATE);
                        day.DayNameEn = Convert.ToDateTime(da.APP_DATE).DayOfWeek.ToString();
                        day.DayNameAr = Utilities.DayEn_AR(Convert.ToDateTime(da.APP_DATE).DayOfWeek.ToString());
                        day.DayNumber = Convert.ToInt32(singleVal.day);
                        Session se = null;

                        var currentDayList = aG_LIVESESSIONEXCELlst.Where(x => x.APP_DAY == singleVal.day).ToList();

                        for (int session = 1; session <= 6; session++)
                        {
                            se = new Session();
                            try
                            {
                                se.liveSession = currentDayList.Where(x => x.APP_SESSION_NUMBER.ToLower().Replace("session", string.Empty) == session.ToString()
                                && x.APP_DAY == singleVal.day).FirstOrDefault();
                            }
                            catch
                            {
                                se.liveSession = null;
                            }

                            day.sessions[session - 1, 0] = se;
                        }
                        days.Add(day);
                    }

                    sb.Append("<h3 style='font-weight:bold;'>Live Session Schedule - " + aG_LIVESESSIONEXCELlst[0].APP_LIVESESSION_NO + "</h3>");

                    int Count = 0;
                    int button = 1;
                    foreach (Days d in days)
                    {
                        sb.Append("<div class='panel panel-info'><div class='panel-heading' style='font-weight:bold;font-size:16px;'><i class='fa fa-table' aria-hidden='true'></i> Day: " +
                            d.DayNumber + "&nbsp;&nbsp;" +
                            string.Format("{0:dd/MM/yyyy}", d.DayDate.Date) +
                            "&nbsp;&nbsp;" + d.DayNameEn + "&nbsp;&nbsp;" +
                            d.DayNameAr + "<span style='float:right'>" + aG_LIVESESSIONEXCELlst[0].APP_LIVESESSION_NO + "</span> </div><div class='panel-body'>");

                        sb.Append("<table class='table table-bordered table-sm' id='table" + (Count + 1) + "'>");


                        sb.Append("<tr style='font-weight:bold;'><td></td> ");
                        sb.Append("<td colspan=2 style='background-color:aliceblue;'>" +
                            "الفترة الأولي" +
                            "</td> <td colspan=2 style='background-color:white;' >" +
                            "الفترة الثانية" +
                            "</td> <td colspan=2 style='background-color:antiquewhite;'>" +
                            "الفترة الثالثة" +
                            "</td></tr>");

                        sb.Append("<tr><td>الوقت <br> رقم الاستوديو </td> ");
                        sb.Append("<td style='background-color:aliceblue;'>Female [5:00 pm , 5:30 pm] </td> <td style='background-color:aliceblue;'> Male [5:45 pm , 6:15 pm]</td>" +
                            " <td style='background-color:white;'>Female [6:30 pm , 7:00 pm] </td> <td style='background-color:white;'> Male [7:15 pm , 7:45 pm]</td>" +
                            " <td style='background-color:antiquewhite;'>Female [8:00 pm , 8:30 pm] </td> <td style='background-color:antiquewhite;'> Male [8:45 pm , 9:15 pm]</td></tr>");

                        string studio = string.Empty;
                        for (int s = 0; s < 6; s++)
                        {
                            if (d.sessions[s, 0].liveSession != null)
                            {
                                studio = d.sessions[s, 0].liveSession.STUDIO_NUMBER;
                                break;
                            }
                        }

                        sb.Append("<tr>");
                        sb.Append("<td> " + studio + "</td>");
                        for (int session = 0; session < 6; session++)
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

                            if (d.sessions[session, 0].liveSession == null)
                            {
                                sb.Append("<td  style='background-color:" + color + "'></td>");
                                continue;
                            }

                            if (d.sessions[session, 0].liveSession.APP_DAY == "5")
                            {

                            }

                                string TeachTypeColor = "";
                            if (d.sessions[session, 0].liveSession.TeachType == "إشراف")
                                TeachTypeColor = "background-color:greenyellow";
                            else
                                TeachTypeColor = "background-color:lightskyblue";

                            if (d.sessions[session, 0].liveSession.GroupType == GroupDataType.projects)
                            {
                                if (d.sessions[session, 0].liveSession.SessionType == SessionType.Female)
                                {
                                    sb.Append("<td  style='background-color:" + color + "'> <table style='border-collapse: collapse!important;width: 100%;text-align: center;background-color:" + color + "'><tbody><tr><td>" + d.sessions[session, 0].liveSession.COURSE_NUMBER + "</td>");
                                    sb.Append("<td>" + d.sessions[session, 0].liveSession.COURSE_NAME + "</td></tr>");
                                    sb.Append("<tr><td>" + d.sessions[session, 0].liveSession.STAFF_NAME + "</td>");
                                    sb.Append("<td>" + d.sessions[session, 0].liveSession.FACULTY_NAME + "</td></tr>");
                                    sb.Append("<tr><td colspan=2  style='" + TeachTypeColor + "'><b>" + " طالبات - " + d.sessions[session, 0].liveSession.TeachType + "</b></td></tr>");


                                    sb.Append("<tr><td colspan=2 style='padding:5px;'>" +
                              "<button id='linkbtn" + button + "' style='font-weight:bold;font-size:14px;' class='btn btn-minier btn-info btn-block' " +
                              "onclick='collab(" + button + ")'  '" + Data(d.sessions[session, 0].liveSession) + "'><i class='ace-icon fa fa-link bigger-110'></i> <span class='bigger-110 no-text-shadow'>رابط المحاضرة المباشرة</span></button>" +
                              "</td></tr>");

                                    sb.Append("<tr><td colspan=2 style='padding:5px;'>" +
                   "<button id='button" + button + "' style='font-weight:bold;font-size:14px;' class='btn btn-minier btn-warning btn-block' " +
                   "onclick='edit(" + button++ + ")' ' " + Data(d.sessions[session, 0].liveSession) + "'><i class='ace-icon fa fa-pencil-square-o bigger-110'></i> <span class='bigger-110 no-text-shadow'>تعديل</span></button>" +
                   "</td></tr>");
                                    sb.Append("</tbody></table></td>");
                                }
                                else if (d.sessions[session, 0].liveSession.SessionType == SessionType.Male)
                                {
                                    sb.Append("<td  style='background-color:" + color + "'> <table style='border-collapse: collapse!important;width: 100%;text-align: center;background-color:" + color + "'><tbody><tr><td>" + d.sessions[session, 0].liveSession.COURSE_NUMBER + "</td>");
                                    sb.Append("<td>" + d.sessions[session, 0].liveSession.COURSE_NAME + "</td></tr>");
                                    sb.Append("<tr><td>" + d.sessions[session, 0].liveSession.STAFF_NAME + "</td>");
                                    sb.Append("<td>" + d.sessions[session, 0].liveSession.FACULTY_NAME + "</td></tr>");
                                    sb.Append("<tr><td  colspan=2  style='" + TeachTypeColor + "'><b>" + " طلاب - " + d.sessions[session, 0].liveSession.TeachType + "</b></td></tr>");

                                    sb.Append("<tr><td colspan=2 style='padding:5px;'>" +
                "<button id='linkbtn" + button + "' style='font-weight:bold;font-size:14px;' class='btn btn-minier btn-info btn-block' " +
                "onclick='collab(" + button + ")' '" + Data(d.sessions[session, 0].liveSession) + "'><i class='ace-icon fa fa-link bigger-110'></i> <span class='bigger-110 no-text-shadow'>رابط المحاضرة المباشرة</span></button>" +
                "</td></tr>");

                                    sb.Append("<tr><td colspan=2 style='padding:5px;'>" +
                   "<button id='button" + button + "' style='font-weight:bold;font-size:14px;' class='btn btn-minier btn-warning btn-block' " +
                   "onclick='edit(" + button++ + ")' ' " + Data(d.sessions[session, 0].liveSession) + "'><i class='ace-icon fa fa-pencil-square-o bigger-110'></i> <span class='bigger-110 no-text-shadow'>تعديل</span></button>" +
                   "</td></tr>");
                                    sb.Append("</tbody></table></td>");
                                }
                                else if (d.sessions[session, 0].liveSession.SessionType == SessionType.ALL)
                                {
                                    sb.Append("<td colspan=2  style='background-color:" + color + "'> <table style='border-collapse: collapse!important;width: 100%;text-align: center;background-color:" + color + "'><tbody><tr><td>" + d.sessions[session, 0].liveSession.COURSE_NUMBER + "</td>");
                                    sb.Append("<td>" + d.sessions[session, 0].liveSession.COURSE_NAME + "</td></tr>");
                                    sb.Append("<tr><td>" + d.sessions[session, 0].liveSession.STAFF_NAME + "</td>");
                                    sb.Append("<td>" + d.sessions[session, 0].liveSession.FACULTY_NAME + "</td></tr>");
                                    sb.Append("<tr><td  style='" + TeachTypeColor + "'><b>" + d.sessions[session, 0].liveSession.TeachType + "</b></td></tr>");


                                    sb.Append("<tr><td colspan=2 style='padding:5px;'>" +
                         "<button id='button" + button + "' style='font-weight:bold;font-size:14px;' class='btn btn-minier btn-warning btn-block' " +
                         "onclick='edit(" + button++ + ")' ' " + Data(d.sessions[session, 0].liveSession) + "'><i class='ace-icon fa fa-link bigger-110'></i> <span class='bigger-110 no-text-shadow'>تعديل</span></button>" +
                         "</td></tr>");
                                    sb.Append("</tbody></table></td>");
                                }
                                //  session++;
                            }
                            else
                            {
                                if (d.sessions[session, 0].liveSession.SessionType == SessionType.Female)
                                {
                                    sb.Append("<td  style='background-color:" + color + "'> <table style='border-collapse: collapse!important;width: 100%;text-align: center;background-color:" + color + "'><tbody><tr><td>" + d.sessions[session, 0].liveSession.COURSE_NUMBER + "</td>");
                                    sb.Append("<td>" + d.sessions[session, 0].liveSession.COURSE_NAME + "</td></tr>");
                                    sb.Append("<tr><td>" + d.sessions[session, 0].liveSession.STAFF_NAME + "</td>");
                                    sb.Append("<td>" + d.sessions[session, 0].liveSession.FACULTY_NAME + "</td></tr>");
                                    sb.Append("<tr><td colspan=2  style='" + TeachTypeColor + "'><b> " + " طالبات - " + d.sessions[session, 0].liveSession.TeachType + "</b></td></tr>");


                                    sb.Append("<tr><td colspan=2 style='padding:5px;'>" +
                              "<button id='linkbtn" + button + "' style='font-weight:bold;font-size:14px;' class='btn btn-minier btn-info btn-block' " +
                              "onclick='collab(" + button + ")' '" + Data(d.sessions[session, 0].liveSession) + "'><i class='ace-icon fa fa-link bigger-110'></i> <span class='bigger-110 no-text-shadow'>رابط المحاضرة المباشرة</span></button>" +
                              "</td></tr>");

                                    sb.Append("<tr><td colspan=2 style='padding:5px;'>" +
                               "<button id='button" + button + "' style='font-weight:bold;font-size:14px;' class='btn btn-minier btn-warning btn-block' " +
                               "onclick='edit(" + button++ + ")' ' " + Data(d.sessions[session, 0].liveSession) + "'><i class='ace-icon fa fa-pencil-square-o bigger-110'></i> <span class='bigger-110 no-text-shadow'>تعديل</span></button>" +
                               "</td></tr>");
                                    sb.Append("</tbody></table></td>");

                                 //   sb.Append("<td></td>");
                                  //  session++;
                                }
                                else if (d.sessions[session, 0].liveSession.SessionType == SessionType.Male)
                                {
                                  //  sb.Append("<td></td>");

                                    sb.Append("<td  style='background-color:" + color + "'> <table style='border-collapse: collapse!important;width: 100%;text-align: center;background-color:" + color + "'><tbody><tr><td>" + d.sessions[session, 0].liveSession.COURSE_NUMBER + "</td>");
                                    sb.Append("<td>" + d.sessions[session, 0].liveSession.COURSE_NAME + "</td></tr>");
                                    sb.Append("<tr><td>" + d.sessions[session, 0].liveSession.STAFF_NAME + "</td>");
                                    sb.Append("<td>" + d.sessions[session, 0].liveSession.FACULTY_NAME + "</td></tr>");
                                    sb.Append("<tr><td colspan=2  style='" + TeachTypeColor + "'><b>" + " طلاب - " + d.sessions[session, 0].liveSession.TeachType + "</b></td></tr>");

                                    sb.Append("<tr><td colspan=2 style='padding:5px;'>" +
                         "<button id='linkbtn" + button + "' style='font-weight:bold;font-size:14px;' class='btn btn-minier btn-info btn-block' " +
                         "onclick='collab(" + button + ")' '" + Data(d.sessions[session, 0].liveSession) + "'><i class='ace-icon fa fa-link bigger-110'></i> <span class='bigger-110 no-text-shadow'>رابط المحاضرة المباشرة</span></button>" +
                         "</td></tr>");

                                    sb.Append("<tr><td colspan=2 style='padding:5px;'>" +
                                   "<button id='button" + button + "' style='font-weight:bold;font-size:14px;' class='btn btn-minier btn-warning btn-block' " +
                                   "onclick='edit(" + button++ + ")' ' " + Data(d.sessions[session, 0].liveSession) + "'><i class='ace-icon fa fa-pencil-square-o bigger-110'></i> <span class='bigger-110 no-text-shadow'>تعديل</span></button>" +
                                   "</td></tr>");
                                    sb.Append("</tbody></table></td>");


                                 //   session++;
                                }
                                else if (d.sessions[session, 0].liveSession.SessionType == SessionType.ALL)
                                {
                                    sb.Append("<td  style='background-color:" + color + "'> <table style='border-collapse: collapse!important;width: 100%;text-align: center;background-color:" + color + "'><tbody><tr><td>" + d.sessions[session, 0].liveSession.COURSE_NUMBER + "</td>");
                                    sb.Append("<td>" + d.sessions[session, 0].liveSession.COURSE_NAME + "</td></tr>");
                                    sb.Append("<tr><td>" + d.sessions[session, 0].liveSession.STAFF_NAME + "</td>");
                                    sb.Append("<td>" + d.sessions[session, 0].liveSession.FACULTY_NAME + "</td></tr>");
                                    sb.Append("<tr><td  colspan=2 style='" + TeachTypeColor + "'><b>" + d.sessions[session, 0].liveSession.TeachType + "</b></td></tr>");
                                    if (session % 2 == 0)
                                    {

                                        sb.Append("<tr><td colspan=2 style='padding:5px;'>" +
                                  "<button id='linkbtn" + button + "' style='font-weight:bold;font-size:14px;' class='btn btn-minier btn-info btn-block' " +
                                  "onclick='collab(" + button + ")'  '" + Data(d.sessions[session, 0].liveSession) + "'><i class='ace-icon fa fa-link bigger-110'></i> <span class='bigger-110 no-text-shadow'>رابط المحاضرة المباشرة</span></button>" +
                                  "</td></tr>");

                                        sb.Append("<tr><td colspan=2 style='padding:5px;'>" +
                                       "<button id='button" + button + "' style='font-weight:bold;font-size:14px;' class='btn btn-minier btn-warning btn-block' " +
                                       "onclick='edit(" + button + ")' '" + Data(d.sessions[session, 0].liveSession) + "'><i class='ace-icon fa fa-pencil-square-o bigger-110'></i> <span class='bigger-110 no-text-shadow'>تعديل</span></button>" +
                                       "</td></tr>");
                                    }
                                    else
                                    {
                                        sb.Append("<tr><td colspan=2 style='padding:5px;'>" +
                               "<button id='linkbtn" + button + "' style='font-weight:bold;font-size:14px;' class='btn btn-minier btn-info btn-block' " +
                               "onclick='collab(" + button + ")' '" + Data(d.sessions[session, 0].liveSession) + "'><i class='ace-icon fa fa-link bigger-110'></i> <span class='bigger-110 no-text-shadow'>رابط المحاضرة المباشرة</span></button>" +
                               "</td></tr>");
                                    }

                                    button++;

                                    sb.Append("</tbody></table></td>");


                                }
                                //  session++;

                            }

                        }
                        sb.Append("</tr>");


                        sb.Append("</table>");
                        sb.Append("</div></div>");
                    }


                    return sb.ToString();
                }
                return string.Empty;
            }
            return string.Empty;
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
            sb.Append(" data-colabf='" + liveSession.FEMALE_COURSE_COLLAB_LINK + "' ");
            sb.Append(" data-colabm='" + liveSession.MALE_COURSE_COLLAB_LINK + "' ");


            return sb.ToString();
        }


        [WebMethod(EnableSession = true)]
        public string AvailableDoctorSessions(AG_LIVESESSIONEXCEL liveSession)
        {
            DataSet ds = LiveDoctors.AvailableDoctorSessions(liveSession);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    List<AG_LIVESESSIONEXCEL> aG_LIVESESSIONEXCELAvailablelst = new List<AG_LIVESESSIONEXCEL>();
                    AG_LIVESESSIONEXCEL aG = null;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        aG = new AG_LIVESESSIONEXCEL();
                        aG.APP_LIVESESSION_NO = dr["APP_LIVESESSION_NO"].ToString();
                        aG.APP_TERM = dr["APP_TERM"].ToString();
                        aG.APP_DAY = dr["APP_DAY"].ToString();
                        aG.APP_SESSION_NUMBER = dr["APP_SESSION_NUMBER"].ToString();
                        aG.APP_SESSION_TIME = dr["APP_SESSION_TIME"].ToString();
                        aG.STUDIO_NUMBER = dr["STUDIO_NUMBER"].ToString();
                        aG.FACULTY_NAME = dr["FACULTY_NAME"].ToString();
                        aG.GENERAL_MAJOR = dr["GENERAL_MAJOR"].ToString();
                        aG.SPECIFIC_MAJOR = dr["SPECIFIC_MAJOR"].ToString();
                        aG.STUDY_LEVEL = dr["STUDY_LEVEL"].ToString();
                        aG.COURSE_NUMBER = dr["COURSE_NUMBER"].ToString();
                        aG.FIRST_SEMESTER_CRN_MALE = dr["FIRST_SEMESTER_CRN_MALE"].ToString();
                        aG.FIRST_SEMESTER_CRN_FEMALE = dr["FIRST_SEMESTER_CRN_FEMALE"].ToString();
                        aG.SECOND_SEMESTER_CRN_MALE = dr["SECOND_SEMESTER_CRN_MALE"].ToString();
                        aG.SECOND_SEMESTER_CRN_FEMALE = dr["SECOND_SEMESTER_CRN_FEMALE"].ToString();
                        aG.FEMALE_COURSE_COLLAB_LINK = dr["FEMALE_COURSE_COLLAB_LINK"].ToString();
                        aG.MALE_COURSE_COLLAB_LINK = dr["MALE_COURSE_COLLAB_LINK"].ToString();
                        aG.COURSE_NAME = dr["COURSE_NAME"].ToString();
                        aG.STAFF_NAME = dr["STAFF_NAME"].ToString();
                        aG.EMAIL = dr["EMAIL"].ToString();
                        aG.MOBILE = dr["MOBILE"].ToString();
                        aG.STAFF_NUMBER = dr["STAFF_NUMBER"].ToString();
                        aG.EXAMDAY_PERIOD = dr["EXAMDAY_PERIOD"].ToString();
                        aG.APP_DATE = dr["APP_DATE"].ToString();// Convert.ToDateTime(dr["APP_DATE"]);
                        aG.TeachType = dr["TEACHTYPE"].ToString();
                        aG.GroupType = (GroupDataType)Enum.Parse(typeof(GroupDataType), dr["GROUPTYPE"].ToString(), true);
                        aG.SessionType = (SessionType)Enum.Parse(typeof(SessionType), dr["SESSIONTYPE"].ToString(), true);

                        aG_LIVESESSIONEXCELAvailablelst.Add(aG);

                    }

                    sb.Append("<table class='table table-bordered table-hover' style='border-collapse: collapse!important;width: 100%;text-align: center;'>");

                    sb.Append("<tr>" +
                        "<td>الأستوديو</td> ");
                    sb.Append("<td> التاريخ</td>");
                    sb.Append("<td> الفترة</td>" +
                        " <td>الوقت</td> </tr>");
                    var days = (from i in aG_LIVESESSIONEXCELAvailablelst
                                select i.APP_DAY).Distinct();
                    string color = "background-color:white";
                    string[] colors = { "aliceblue", "antiquewhite", "honeydew", "azure", "lemonchiffon", "lavenderblush", "aliceblue" };
                    int counter = 0;
                    foreach (AG_LIVESESSIONEXCEL d in aG_LIVESESSIONEXCELAvailablelst.OrderBy(x => x.APP_DATE))
                    {

                        //sb.Append("<tr><td>" + d.APP_DAY + "</td>");
                        color = colors[Convert.ToInt32(d.APP_DAY) - 1];

                        if (counter % 2 == 0)
                        {
                            sb.Append("<td  rowspan=2  style='background-color:" + color + "'>" + d.STUDIO_NUMBER + "</td>");

                            sb.Append("<td rowspan=2 style='background-color:" + color + "' >" +
                           "<button id='button" + counter + "' style='font-weight:bold;font-size:16px;' class='btn btn-minier btn-warning' " +
                           "onclick='edit(" + counter + ")' ' " + Data(d) + "'><i class='ace-icon fa fa-pencil-square-o bigger-110'></i> <span class='bigger-110 no-text-shadow'>" + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(d.APP_DATE)) + " - " + Utilities.DayEn_AR(Convert.ToDateTime(d.APP_DATE).DayOfWeek.ToString()) + "</span></button>" +
                           "</td>"); ;
                        }
                        //else { 
                        //    sb.Append("<td style='background-color:" + color + "'>" + string.Format("{0:dd/MM/yyyy}", d.APP_DATE) + "</td>"); 
                        //}

                        string session = "";
                        switch (Convert.ToInt32(d.APP_SESSION_NUMBER.Replace("session", "")))
                        {
                            case 1:
                            case 2:
                                session = "الفترة الأولي";
                                break;
                            case 3:
                            case 4:
                                session = "الفترة الثانية";
                                break;
                            case 5:
                            case 6:
                                session = "الفترة الثالثة";
                                break;

                        }
                        if (counter % 2 == 0)
                        {
                            sb.Append("<td rowspan=2 style='background-color:" + color + "' >" +
                        session
                           + "</td>");
                        }

                        sb.Append("<td style='background-color:" + color + "'>" + d.APP_SESSION_TIME + "</td>");
                        sb.Append("</tr>");
                        counter++;

                    }
                    sb.Append("</table>");

                    DataSet dsLiveStart = LiveDoctors.LiveSessionStart(liveSession);
                    DateTime firstDate = (from i in aG_LIVESESSIONEXCELAvailablelst
                                          select Convert.ToDateTime(i.APP_DATE)).Distinct().OrderBy(x => x).FirstOrDefault();
                    if (dsLiveStart.Tables.Count > 0)
                    {
                        if (dsLiveStart.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsLiveStart.Tables[0].Rows)
                            {
                                firstDate = (DateTime)((dr["LIVE_START"]));

                            }
                        }
                    }

                    bool within_3Days = false;
                    if (DateTime.Compare(firstDate.AddDays(3), DateTime.Now) <= 0) //Less than zero	t1 is earlier than t2.
                    {
                        within_3Days = false;

                    }
                    else
                        within_3Days = true;//true


                    AvailableDates availableDates = new AvailableDates();
                    availableDates.within_3Days = within_3Days;
                    availableDates.availableDatesTable = sb.ToString();
                    availableDates.changeSessionType = ChangeSessionType.DoubleChange;

                    string jsonString;
                    jsonString = JsonConvert.SerializeObject(availableDates);

                    return jsonString;
                }
            }
            return "";
        }


        [WebMethod(EnableSession = true)]
        public string AvailableDoctorSessionsProjects(AG_LIVESESSIONEXCEL liveSession)
        {
            DataSet ds = LiveDoctors.AvailableDoctorSessionsProjects(liveSession);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    List<AG_LIVESESSIONEXCEL> aG_LIVESESSIONEXCELAvailablelst = new List<AG_LIVESESSIONEXCEL>();
                    AG_LIVESESSIONEXCEL aG = null;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        aG = new AG_LIVESESSIONEXCEL();
                        aG.APP_LIVESESSION_NO = dr["APP_LIVESESSION_NO"].ToString();
                        aG.APP_TERM = dr["APP_TERM"].ToString();
                        aG.APP_DAY = dr["APP_DAY"].ToString();
                        aG.APP_SESSION_NUMBER = dr["APP_SESSION_NUMBER"].ToString();
                        aG.APP_SESSION_TIME = dr["APP_SESSION_TIME"].ToString();
                        aG.STUDIO_NUMBER = dr["STUDIO_NUMBER"].ToString();
                        aG.FACULTY_NAME = dr["FACULTY_NAME"].ToString();
                        aG.GENERAL_MAJOR = dr["GENERAL_MAJOR"].ToString();
                        aG.SPECIFIC_MAJOR = dr["SPECIFIC_MAJOR"].ToString();
                        aG.STUDY_LEVEL = dr["STUDY_LEVEL"].ToString();
                        aG.COURSE_NUMBER = dr["COURSE_NUMBER"].ToString();
                        aG.FIRST_SEMESTER_CRN_MALE = dr["FIRST_SEMESTER_CRN_MALE"].ToString();
                        aG.FIRST_SEMESTER_CRN_FEMALE = dr["FIRST_SEMESTER_CRN_FEMALE"].ToString();
                        aG.SECOND_SEMESTER_CRN_MALE = dr["SECOND_SEMESTER_CRN_MALE"].ToString();
                        aG.SECOND_SEMESTER_CRN_FEMALE = dr["SECOND_SEMESTER_CRN_FEMALE"].ToString();
                        aG.FEMALE_COURSE_COLLAB_LINK = dr["FEMALE_COURSE_COLLAB_LINK"].ToString();
                        aG.MALE_COURSE_COLLAB_LINK = dr["MALE_COURSE_COLLAB_LINK"].ToString();
                        aG.COURSE_NAME = dr["COURSE_NAME"].ToString();
                        aG.STAFF_NAME = dr["STAFF_NAME"].ToString();
                        aG.EMAIL = dr["EMAIL"].ToString();
                        aG.MOBILE = dr["MOBILE"].ToString();
                        aG.STAFF_NUMBER = dr["STAFF_NUMBER"].ToString();
                        aG.EXAMDAY_PERIOD = dr["EXAMDAY_PERIOD"].ToString();
                        aG.APP_DATE = dr["APP_DATE"].ToString();// Convert.ToDateTime(dr["APP_DATE"]);
                        aG.TeachType = dr["TEACHTYPE"].ToString();
                        aG.GroupType = (GroupDataType)Enum.Parse(typeof(GroupDataType), dr["GROUPTYPE"].ToString(), true);
                        aG.SessionType = (SessionType)Enum.Parse(typeof(SessionType), dr["SESSIONTYPE"].ToString(), true);

                        aG_LIVESESSIONEXCELAvailablelst.Add(aG);

                    }

                    sb.Append("<table class='table table-bordered table-hover' style='border-collapse: collapse!important;width: 100%;text-align: center;'>");

                    sb.Append("<tr>" +
                        "<td>الأستوديو</td> ");
                    sb.Append("<td> التاريخ</td>");
                    sb.Append("<td> الفترة</td>" +
                        " <td>الوقت</td> </tr>");
                    var days = (from i in aG_LIVESESSIONEXCELAvailablelst
                                select i.APP_DAY).Distinct();
                    string color = "background-color:white";
                    string[] colors = { "aliceblue", "antiquewhite", "honeydew", "azure", "lemonchiffon", "lavenderblush", "aliceblue" };

                    int counter = 0;
                    foreach (AG_LIVESESSIONEXCEL d in aG_LIVESESSIONEXCELAvailablelst.OrderBy(x => Convert.ToDateTime(x.APP_DATE)))
                    {


                        color = colors[Convert.ToInt32(d.APP_DAY) - 1];


                        sb.Append("<td     style='background-color:" + color + "'>" + d.STUDIO_NUMBER + "</td>");

                        sb.Append("<td   style='background-color:" + color + "' >" +
                       "<button id='button" + counter + "' style='font-weight:bold;font-size:16px;' class='btn btn-minier btn-warning' " +
                       "onclick='edit(" + counter + ")' ' " + Data(d) + "'><i class='ace-icon fa fa-pencil-square-o bigger-110'></i> <span class='bigger-110 no-text-shadow'>" + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(d.APP_DATE)) + " - " + Utilities.DayEn_AR(Convert.ToDateTime(d.APP_DATE).DayOfWeek.ToString()) + "</span></button>" +
                       "</td>");


                        string session = "";
                        switch (Convert.ToInt32(d.APP_SESSION_NUMBER.Replace("session", "")))
                        {
                            case 1:
                            case 2:
                                session = "الفترة الأولي";
                                break;
                            case 3:
                            case 4:
                                session = "الفترة الثانية";
                                break;
                            case 5:
                            case 6:
                                session = "الفترة الثالثة";
                                break;

                        }
                        sb.Append("<td   style='background-color:" + color + "' >" +
                     session
                        + "</td>");

                        sb.Append("<td style='background-color:" + color + "'>" + d.APP_SESSION_TIME + "</td>");
                        sb.Append("</tr>");
                        counter++;

                    }
                    sb.Append("</table>");

                    DataSet dsLiveStart = LiveDoctors.LiveSessionStart(liveSession);
                    DateTime firstDate = (from i in aG_LIVESESSIONEXCELAvailablelst
                                          select Convert.ToDateTime(i.APP_DATE)).Distinct().OrderBy(x => x).FirstOrDefault();
                    if (dsLiveStart.Tables.Count > 0)
                    {
                        if (dsLiveStart.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsLiveStart.Tables[0].Rows)
                            {
                                firstDate = Convert.ToDateTime(dr["LIVE_START"]);

                            }
                        }
                    }

                    bool within_3Days = false;
                    if (DateTime.Compare(firstDate.AddDays(3), DateTime.Now) <= 0) //Less than zero	t1 is earlier than t2.
                    {
                        within_3Days = false;

                    }
                    else
                        within_3Days = true;//true



                    AvailableDates availableDates = new AvailableDates();
                    availableDates.within_3Days = within_3Days;
                    availableDates.availableDatesTable = sb.ToString();
                    availableDates.changeSessionType = ChangeSessionType.SingleChange;

                    string jsonString;
                    jsonString = JsonConvert.SerializeObject(availableDates);

                    return jsonString;
                }
            }
            return "";
        }
        private class AvailableDates
        {
            public bool within_3Days;
            public string availableDatesTable;
            public ChangeSessionType changeSessionType;

        }
        private enum ChangeSessionType
        {
            SingleChange = 1,
            DoubleChange = 2
        };

        [WebMethod(EnableSession = true)]
        public string ChangeLiveSessionsTime(AG_LIVESESSIONEXCEL MyLiveSession, AG_LIVESESSIONEXCEL New_LiveSession, string changeType, bool within_3Days, Execuses execuses)
        {
            string ds = LiveDoctors.ChangeLiveSessionsTime(MyLiveSession, New_LiveSession, changeType, within_3Days, execuses);

            return ds;
        }

        [WebMethod(EnableSession = true)]
        public int UpdateLiveSessionsCollabLink(AG_LIVESESSIONEXCEL liveupdate, string gender)
        {
            int res = LiveDoctors.UpdateLiveSessionsCollabLink(liveupdate, gender);
            return res;
        }
    }
}
