using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMBA;
using System.Data;
using System.Xml;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class PopUpReportWindow : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                ViewState["ReportOrderNo"] = Request.QueryString["A0A"];
                CHighmarkMember Obj = new CHighmarkMember();
                DataTable dt = new DataTable();
                dt = Obj.GetHighmarkReportData(Convert.ToString(ViewState["ReportOrderNo"]));
                if (dt.Rows.Count > 0)
                {
                    XmlDocument xdRpt = new XmlDocument();
                    xdRpt.LoadXml(Convert.ToString(dt.Rows[0]["XMLDetails"]));
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
                    //LiteralControl imageGallery = new LiteralControl();
                    ltPrint.Text += data;
                    DivPrint.Controls.Add(ltPrint);
                    // Response.Write(data);

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