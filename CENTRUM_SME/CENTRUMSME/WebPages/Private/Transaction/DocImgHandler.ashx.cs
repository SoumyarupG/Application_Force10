using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using CENTRUMBA;
using System.IO;
using System.Configuration;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    /// <summary>
    /// Summary description for DocImgHandler
    /// </summary>
    public class DocImgHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = null;
            DataTable dt = new DataTable();
            DataTable dt1 = null, dt2 = null, dt3 = null, dt4 = null;
            CApplication oLoanApp = null;
            CMember oMem = new CMember();

            if (context.Request.QueryString["id"] != null && context.Request.QueryString["type"] != null)
            {
                #region Applicant
                if (context.Request.QueryString["APCP"].ToString() == "A")
                {

                    if (context.Request.QueryString["type"].ToString() == "CustAddProof" || context.Request.QueryString["type"].ToString() == "CustAddProof1")
                    {

                        try
                        {
                            if (context.Request.QueryString["Side"] != null)
                            {
                                // Read the file and convert it to Byte Array
                                //string filePath = "E:\\WebApps\\PratamMobService\\CustomerKYCImage\\";
                                string filePath = ConfigurationManager.AppSettings["PathCustomerKYCImage"];
                                string filename = "", filepathURL = "";
                                filename = context.Request.QueryString["id"].ToString() + "\\AddressImage" + context.Request.QueryString["Side"] + ".png";
                                filepathURL = filePath + "\\" + filename;
                                string contenttype = "image/" + Path.GetExtension(filename).Replace(".", "");
                                if (File.Exists(filepathURL))
                                {
                                   // FileStream fs = new FileStream(filePath + filename, FileMode.Open, FileAccess.Read);
                                    FileStream fs = new FileStream(filepathURL, FileMode.Open, FileAccess.Read);
                                    BinaryReader br = new BinaryReader(fs);
                                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                    br.Close();
                                    fs.Close();

                                    //Write the file to response Stream
                                    context.Response.Buffer = true;
                                    context.Response.Charset = "";
                                    context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    context.Response.ContentType = contenttype;
                                    context.Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                                    context.Response.BinaryWrite(bytes);
                                }
                                else
                                {
                                    context.Response.ContentType = "image/png";
                                    context.Response.WriteFile("~/Images/NoImageAvailable.png");

                                }

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (context.Request.QueryString["type"].ToString() == "CustPhotoProof" || context.Request.QueryString["type"].ToString() == "CustPhotoProof1")
                    {

                        try
                        {
                            if (context.Request.QueryString["Side"] != null)
                            {
                                // Read the file and convert it to Byte Array
                                //string filePath = "E:\\WebApps\\PratamMobService\\CustomerKYCImage\\";
                                string filePath = ConfigurationManager.AppSettings["PathCustomerKYCImage"];
                                string filename = "", filepathURL = "";
                                filename = context.Request.QueryString["id"].ToString() + "\\PhotoImage" + context.Request.QueryString["Side"] + ".png";
                                filepathURL = filePath + "\\" + filename;
                                string contenttype = "image/" + Path.GetExtension(filename).Replace(".", "");
                                if (File.Exists(filepathURL))
                                {
                                    //FileStream fs = new FileStream(filePath + filename, FileMode.Open, FileAccess.Read);
                                    FileStream fs = new FileStream(filepathURL, FileMode.Open, FileAccess.Read);
                                    BinaryReader br = new BinaryReader(fs);
                                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                    br.Close();
                                    fs.Close();

                                    //Write the file to response Stream
                                    context.Response.Buffer = true;
                                    context.Response.Charset = "";
                                    context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    context.Response.ContentType = contenttype;
                                    context.Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                                    context.Response.BinaryWrite(bytes);
                                }
                                else
                                {
                                    context.Response.ContentType = "image/png";
                                    context.Response.WriteFile("~/Images/NoImageAvailable.png");

                                }

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                #endregion
                #region CoApplicant
                else if (context.Request.QueryString["APCP"].ToString() == "C")
                {
                    if (context.Request.QueryString["type"].ToString() == "CAAddProof")
                    {
                        try
                        {
                            if (context.Request.QueryString["Side"] != null)
                            {
                                // Read the file and convert it to Byte Array
                                //string filePath = "E:\\WebApps\\PratamMobService\\CoApplicantKYCImage\\";
                                string filePath = ConfigurationManager.AppSettings["PathCoApplicantKYCImage"];
                                string filename = "", filepathURL = "";
                                filename = context.Request.QueryString["id"].ToString() + "\\AddressImage" + context.Request.QueryString["Side"] + ".png";
                                filepathURL = filePath + "\\" + filename;
                                string contenttype = "image/" + Path.GetExtension(filename).Replace(".", "");
                                if (File.Exists(filepathURL))
                                {
                                   // FileStream fs = new FileStream(filePath + filename, FileMode.Open, FileAccess.Read);
                                    FileStream fs = new FileStream(filepathURL, FileMode.Open, FileAccess.Read);
                                    BinaryReader br = new BinaryReader(fs);
                                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                    br.Close();
                                    fs.Close();

                                    //Write the file to response Stream
                                    context.Response.Buffer = true;
                                    context.Response.Charset = "";
                                    context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    context.Response.ContentType = contenttype;
                                    context.Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                                    context.Response.BinaryWrite(bytes);
                                }
                                else
                                {
                                    context.Response.ContentType = "image/png";
                                    context.Response.WriteFile("~/Images/NoImageAvailable.png");
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (context.Request.QueryString["type"].ToString() == "CAPhotoProof")
                    {

                        try
                        {
                            if (context.Request.QueryString["Side"] != null)
                            {
                                // Read the file and convert it to Byte Array
                                //string filePath = "E:\\WebApps\\PratamMobService\\CoApplicantKYCImage\\";
                                string filePath = ConfigurationManager.AppSettings["PathCoApplicantKYCImage"];
                                string filename = "", filepathURL = "";
                                filename = context.Request.QueryString["id"].ToString() + "\\PhotoImage" + context.Request.QueryString["Side"] + ".png";
                                filepathURL = filePath + "\\" + filename;
                                string contenttype = "image/" + Path.GetExtension(filename).Replace(".", "");
                                if (File.Exists(filepathURL))
                                {
                                    //FileStream fs = new FileStream(filePath + filename, FileMode.Open, FileAccess.Read);
                                    FileStream fs = new FileStream(filepathURL, FileMode.Open, FileAccess.Read);
                                    BinaryReader br = new BinaryReader(fs);
                                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                    br.Close();
                                    fs.Close();

                                    //Write the file to response Stream
                                    context.Response.Buffer = true;
                                    context.Response.Charset = "";
                                    context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    context.Response.ContentType = contenttype;
                                    context.Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                                    context.Response.BinaryWrite(bytes);
                                }
                                else
                                {
                                    context.Response.ContentType = "image/png";
                                    context.Response.WriteFile("~/Images/NoImageAvailable.png");
                                }

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                #endregion

            }
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
