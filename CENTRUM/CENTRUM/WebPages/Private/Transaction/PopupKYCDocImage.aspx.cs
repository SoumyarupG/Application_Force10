using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


namespace CENTRUM.WebPages.Private.Transaction
{
	public partial class PopupKYCDocImage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            Int32 vSlNo = 0;
            string vLoanAppID = Request.QueryString["id"];
            string vSubID = Request.QueryString["SubId"];
            vSlNo = Convert.ToInt32(vSubID);
            //string vType = Request.QueryString["disb"];
            imgLoanApp.ImageUrl = "~/getImage1.ashx?Id=" + vLoanAppID + "&SubId=" + vSlNo;
		}
	}
}