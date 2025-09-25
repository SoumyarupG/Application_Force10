using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class PopUpImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string vGrpID = Request.QueryString["mid"];
            string vBranchID = Request.QueryString["bid"];
            //string vType = Request.QueryString["disb"];
            imgMem.ImageUrl = "~/getImage.ashx?Gid=" + vGrpID + "&bid=" + vBranchID;

        }
    }
}