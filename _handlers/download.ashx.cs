using ManageLiveSessionWeb.DAL.D_Admin;
using ManageLiveSessionWeb.utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ManageLiveSessionWeb._handlers
{
    /// <summary>
    /// Summary description for download
    /// </summary>
    public class download : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            AG_LIVESESSIONEXCEL ag = new AG_LIVESESSIONEXCEL();
           
            ag.APP_TERM = context.Request.QueryString["APP_TERM"].ToString();
            ag.APP_LIVESESSION_NO = context.Request.QueryString["APP_LIVESESSION_NO"].ToString();
            ag.APP_DAY = context.Request.QueryString["APP_DAY"].ToString();
            ag.STUDIO_NUMBER = context.Request.QueryString["STUDIO_NUMBER"].ToString();
            ag.COURSE_NUMBER = context.Request.QueryString["COURSE_NUMBER"].ToString();
            ag.STUDY_LEVEL = context.Request.QueryString["STUDY_LEVEL"].ToString();
            ag.EMAIL = context.Request.QueryString["EMAIL"].ToString();
            ag.APP_SESSION_NUMBER = context.Request.QueryString["APP_SESSION_NUMBER"].ToString();

            ChangeLiveSession data = AdminOperations.GetDocReasonPath(ag);
           // var data =   DB.Contents.FirstOrDefault(x => x.contentId == Convert.ToInt32(id));
            if (data != null && !String.IsNullOrEmpty(data.NEW_DOC_PATH))
            {
                string ext = Path.GetExtension(data.NEW_DOC_PATH);
                string path = "";
                if (ext == ".pdf")
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/pdf";
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" + data.STAFF_NAME + "_" + data.COURSE_NAME + ".pdf");
                    //int indexofslash = data.uploadedFile.LastIndexOf('/');
                    //int indexofdot = data.uploadedFile.Length-1;
                    //string uploadedfile = data.uploadedFile.Remove(indexofslash, indexofdot);
                    //string newname = uploadedfile + data.contentNameEn + ".pdf";
                    path = context.Server.MapPath(data.NEW_DOC_PATH).Replace(@"\_handlers", string.Empty);
                }
                else if (ext == ".jpg")
                {
                    context.Response.Clear();
                    context.Response.ContentType = "image/jpeg";
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" + data.STAFF_NAME + "_" + data.COURSE_NAME + ".jpg");
                    //int indexofslash = data.uploadedFile.LastIndexOf('/');
                    //int indexofdot = data.uploadedFile.Length-1;
                    //string uploadedfile = data.uploadedFile.Remove(indexofslash, indexofdot);
                    //string newname = uploadedfile + data.contentNameEn + ".pdf";
                    path = context.Server.MapPath(data.NEW_DOC_PATH).Replace(@"\_handlers", string.Empty);
                }


                if (File.Exists(path))
                {
                    context.Response.WriteFile(path);
                    context.Response.Flush();
                    context.Response.End();
                }
                else
                {
                    context.Response.ContentType = "text/plain";

                    context.Response.Write("File not Exists");
                    context.Response.Flush();
                    context.Response.End();
                }
                return;

            }

            context.Response.ContentType = "text/plain";
            context.Response.Write("No file to download");
            context.Response.Flush();
            context.Response.End();



        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}