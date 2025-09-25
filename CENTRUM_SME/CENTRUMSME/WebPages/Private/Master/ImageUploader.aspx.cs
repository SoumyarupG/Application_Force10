using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
//** Resize Image **//
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class ImageUploader : CENTRUMBAse
    {
        public const string MemImage = "Image";
        public const string MemImgType = "Type";
        public const string MemImgString = "ImageString";

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSaveImg_Click(object sender, EventArgs e)
        {
            byte[] vImg = null;
            string vImgType=null;
            
            if (fuIdProff.HasFile == false)
            {
                gblFuction.AjxMsgPopup("Please Select an image file...");
            }

            try
            {
                if (fuIdProff.HasFile == true)
                {
                    if (fuIdProff.PostedFile.InputStream.Length > 1048576)
                    {
                        gblFuction.AjxMsgPopup("Please Select a file of less than 1Mb.. ");
                    }
                    else
                    {
                        vImg = new byte[fuIdProff.PostedFile.InputStream.Length + 1];
                        fuIdProff.PostedFile.InputStream.Read(vImg, 0, vImg.Length);
                        string DispImg = Convert.ToBase64String(vImg, 0, vImg.Length);
                        vImgType = System.IO.Path.GetExtension(fuIdProff.FileName).ToLower();
                        imgIdPrf1.ImageUrl = "data:image/png;base64," + DispImg;
                        imgIdPrf1.Visible = true;
                        Session[ImageUploader.MemImage] = vImg;
                        Session[ImageUploader.MemImgType] = vImgType;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            finally
            {
            }
        }


        protected void Resize(object sender, EventArgs e)
        {
            // GET THE UPLOADED FILE.
            HttpFileCollection hfc = Request.Files;
            string sFileExt;
            string sImageName;

            if (hfc.Count > 0)
            {
                HttpPostedFile hpf = hfc[0];
                if (hpf.ContentLength > 0)
                {
                    sImageName = hpf.FileName;

                    // FIRST SAVE THE FILE ON THE SERVER.
                    hpf.SaveAs(Server.MapPath("~/" + Path.GetFileName(sImageName)));
                    sFileExt = Path.GetExtension(sImageName).ToLower();

                    // ORIGINAL WIDTH AND HEIGHT.
                    Bitmap newBitmap = new Bitmap(Server.MapPath("~/" + Path.GetFileName(sImageName)));

                    int iwidth = newBitmap.Width;
                    int iheight = newBitmap.Height;
                    newBitmap.Dispose();
                    
                    System.Drawing.Image objOptImage = new System.Drawing.Bitmap(iwidth, iheight, System.Drawing.Imaging.
                                                                                                PixelFormat.Format16bppRgb555);


                    // GET THE ORIGINAL IMAGE.
                    using (System.Drawing.Image objImg = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath("~/" + sImageName)))
                    {
                        // RE-DRAW THE IMAGE USING THE NEWLY OBTAINED PIXEL FORMAT.
                        using (System.Drawing.Graphics oGraphic = System.Drawing.Graphics.FromImage(objOptImage))
                        {
                            var _1 = oGraphic;
                            System.Drawing.Rectangle oRectangle = new System.Drawing.Rectangle(0, 0, iwidth, iheight);
                            _1.DrawImage(objImg, oRectangle);
                        }

                        // SAVE THE OPTIMIZED IMAGE.
                        objOptImage.Save(HttpContext.Current.Server.MapPath("~/images/New" + sImageName), System.Drawing.Imaging.ImageFormat.Png);
                        objImg.Dispose();
                    }                    
                    objOptImage.Dispose(); 

                    // FINALLY SHOW THE OPTIMIZED IMAGE DETAILS WITH SIZE.
                    Bitmap bitmap_Opt = new Bitmap(Server.MapPath("~/images/" + Path.GetFileName("New" + sImageName)));

                    var ms = new System.IO.MemoryStream();
                    bitmap_Opt.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();

                    // Convert byte[] to Base64 String
                    var base64String = Convert.ToBase64String(byteImage);
                    imgIdPrf1.ImageUrl = "data:image/png;base64," + base64String;
                    Session[ImageUploader.MemImage] = byteImage;
                    Session[ImageUploader.MemImgType] = sFileExt;
                    
                    int iwidth_Opt = bitmap_Opt.Width;
                    int iheight_Opt = bitmap_Opt.Height;
                    bitmap_Opt.Dispose();
                    imgIdPrf1.Visible = true;

                    if (File.Exists(Server.MapPath("~/" + Path.GetFileName(sImageName))))
                    {
                        File.Delete(Server.MapPath("~/" + Path.GetFileName(sImageName)));
                    }
                    if (File.Exists(Server.MapPath("~/" + Path.GetFileName("New" + sImageName))))
                    {
                        File.Delete(Server.MapPath("~/" + Path.GetFileName("New" + sImageName)));
                    }                    

                }
            }
        }
    }
}