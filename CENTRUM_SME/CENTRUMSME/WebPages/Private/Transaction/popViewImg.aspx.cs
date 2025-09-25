using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class popViewImg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null && Request.QueryString["type"] != null)
            {
                imgDoc.ImageUrl = "DocImgHandler.ashx?id=" + Request.QueryString["id"].ToString() + "&type=" + Request.QueryString["type"].ToString() + "&APCP=" + Request.QueryString["APCP"].ToString() + "&Side=Front";
                imgDocBack.ImageUrl = "DocImgHandler.ashx?id=" + Request.QueryString["id"].ToString() + "&type=" + Request.QueryString["type"].ToString() + "&APCP=" + Request.QueryString["APCP"].ToString() + "&Side=Back";
                imgView.Visible = false;
                ImgPassBook.Visible = false;
            }
        }
    }
}