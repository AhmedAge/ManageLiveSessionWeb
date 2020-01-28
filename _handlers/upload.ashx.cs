using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions; 

namespace ManageLiveSessionWeb._handlers
{
    /// <summary>
    /// Summary description for upload
    /// </summary>
    public class upload : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Session.Count == 0)
            {
                context.Response.End();
            }
            if (context.Request.UrlReferrer == null)
            {

                context.Response.Write("<html><head><title>لم يتم العثور على الصفحة</title></head><body><Center><div class='content_heading'><div class='sheading'><div class='sheadings'><h2>لم يتم العثور على الصفحة</h2></div></div></div><p class='text'><h2>عذرا، لم يتم العثور على الصفحة المطلوبة .</h2></p ></Center></body></html>");
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }

            if (context.Request.UrlReferrer.ToString().Contains("editlivesessiontime.aspx"))
                
            {


                var ControlName = string.Empty;
                if (context.Request.Files["videoFile"] != null)
                {
                    ControlName = "videoFile";
                }
                if (context.Request.Files["pdfFile"] != null)
                {
                    ControlName = "pdfFile";
                }
                if (context.Request.Files["iconFile"] != null)
                {
                    ControlName = "iconFile";
                }
                if (context.Request.Files["iconFile1"] != null)
                {
                    ControlName = "iconFile1";
                }
                if (context.Request.Files[ControlName].ContentLength > 0)
                {
                    string Extension = GetExtension(context.Request.Files[ControlName]).ToLower();
                    // To get Extension From File Name
                    string PathFolder = null;
                    // the Path where the file will be saved
                    string FilenameGUID = null;
                    string _FileURL = string.Empty;
                    // to save GUID value
                    FilenameGUID = GUID();
                    PathFolder = GetFilePath(Extension);
                    // Check if the folder is already exist or not
                    if (System.IO.Directory.Exists(PathFolder) == true)
                    {
                        // If the folder alrady exist do nothing
                    }
                    else
                    {
                        //if the folder not exist, create the folder
                        System.IO.Directory.CreateDirectory(PathFolder);
                    }
                    // to save the file on Server.
                    string savefile = null;
                    _FileURL = GetFilePath2(Extension, FilenameGUID).ToString();
                    savefile = Getpathsave(PathFolder, FilenameGUID, Extension);
                    // context.Request.Files[ControlName].SaveAs(savefile);
                    context.Response.ContentType = "text/plain";
                    //   _FileURL;
                    string pattern = @"\`|\~|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\'|\<|\,|\>|\?|\""|\;|\s";
                    Regex rgx = new Regex(pattern);
                    string replacement = "_";
                    string resultPath = rgx.Replace(savefile, replacement).Replace(" ", "");
                    _FileURL = rgx.Replace(_FileURL, replacement).Replace(" ", "");


                    if (ControlName == "videoFile")
                    {
                        context.Request.Files["videoFile"].SaveAs(resultPath);
                   
                        context.Response.Write(_FileURL);
                        context.Response.StatusCode = 200;
                        context.Response.Flush();
                        context.Response.End();
                        return;
                    }
                    if (ControlName == "pdfFile")
                    {
                        context.Request.Files["pdfFile"].SaveAs(resultPath);

                        context.Response.Write(_FileURL);
                        context.Response.StatusCode = 200;
                        context.Response.Flush();
                        context.Response.End();
                        return;
                    }
                    if (ControlName == "iconFile")
                    {
                        context.Request.Files["iconFile"].SaveAs(resultPath);

                        context.Response.Write(_FileURL);
                        context.Response.StatusCode = 200;
                        context.Response.Flush();
                        context.Response.End();
                        return;
                    }
                    if (ControlName == "iconFile1")
                    {
                        context.Request.Files["iconFile1"].SaveAs(resultPath);

                        context.Response.Write(_FileURL);
                        context.Response.StatusCode = 200;
                        context.Response.Flush();
                        context.Response.End();
                        return;
                    }
                }
            }
            else
            {
                context.Response.End();
                return;
            }
        }


        #region "Function"
        // <summary>
        // This Function To Return the value of Generate unique ID
        // </summary>
        // <returns></returns>
        // <remarks></remarks>
        public string GUID()
        {
            return System.Guid.NewGuid().ToString();
        }
        // <summary>
        // this function to return Pathe of Audio file
        // </summary>
        // <param name="Exten"></param>
        // <returns></returns>
        // <remarks></remarks>
        public string GetFilePath(string Exten)
        {
            string FilePath = System.Web.HttpContext.Current.Server.MapPath("~/uploads/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/" + Exten);
            return FilePath;
        }
        //  <summary>
        //  this function to return Pathe of file
        //  </summary>
        //  <param name="Exten"></param>
        //  <param name="Name"></param>
        //   <returns></returns>
        //  <remarks></remarks>
        public string GetFilePath2(string Exten, string Name)
        {
            string FilePath = "uploads/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/" + Exten + "/" + Name + "." + Exten;
            return FilePath;
        }
        //  <summary>
        //  this function to return the Extension of file
        //  </summary>
        //  <returns></returns>
        //  <remarks></remarks>
        public string GetExtension(System.Web.HttpPostedFile _obj)
        {
            string UploadFileName = null;
            int posetionofdot = 0;
            // The posetion of (.) in File Name
            int lngth = 0;
            // the length of File Extension string
            UploadFileName = _obj.FileName;
            posetionofdot = UploadFileName.LastIndexOf(".") + 1;
            lngth = UploadFileName.Length - posetionofdot;
            return UploadFileName.Substring(posetionofdot, lngth);
        }
        //  <summary>
        //  this function to return the path where file will be saving
        //  </summary>
        //  <param name="Pathfile"></param>
        //  <param name="FileName"></param>
        //  <param name="Ext"></param>
        //  <returns></returns>
        //  <remarks></remarks>
        public string Getpathsave(string Pathfile, string FileName, string Ext)
        {
            string savefile = null;
            savefile = Pathfile + "\\" + FileName + "." + Ext;
            return savefile;
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}