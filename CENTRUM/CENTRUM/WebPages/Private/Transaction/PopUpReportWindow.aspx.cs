using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class PopUpReportWindow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string vEnqId = Request.QueryString["EnqId"];
                string vCBID = Request.QueryString["CBID"];
                DataTable dt = null;
                CReports oRpt = null;               
                string vBranch = Session[gblValue.BrName].ToString();

                oRpt = new CReports();
                dt = oRpt.GetHighmarkReportData(vEnqId, Convert.ToInt32(vCBID));
                if (dt.Rows.Count > 0)
                {
                    XmlDocument xdRpt = new XmlDocument();
                    xdRpt.LoadXml(Convert.ToString(dt.Rows[0]["CBRequestFile"]));
                    XmlNodeList elemListRpt = xdRpt.GetElementsByTagName("PRINTABLE-REPORT");
                    string data = "";
                    for (int i = 0; i < elemListRpt.Count; i++)
                    {
                        data = elemListRpt[i].ChildNodes[2].InnerXml.Replace("]]>", "");
                    }
                    data = data.Replace("&lt;", "<");
                    data = data.Replace("&gt;", ">");
                    data = data.Replace("&amp;", "&");
                    data = data.Replace("<![CDATA[<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">", "");

                    ltPrint.Text += data;
                    DivPrint.Controls.Add(ltPrint);
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "mywindow", "printDiv('DivPrint');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Records has found related to Highmark Report...");
                    return;
                }
            }
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "mywindow", "printform();", true);
        }
    }
}