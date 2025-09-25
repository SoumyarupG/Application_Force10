using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using CENTRUMCA;
using CENTRUMBA;


namespace CENTRUM_SARALVYAPAR
{
    public partial class DC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string vEnquiryId = Convert.ToString(Server.UrlDecode(Request.QueryString["p"]));
            LoadRecord(vEnquiryId);
        }
        //protected void ddlDCRpt_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadRecord(Convert.ToInt32(ddlDCRpt.SelectedValue.ToString()));
        //}
        private void LoadRecord(string vEnquiryId)
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            CReports DC = new CReports();
            dt = DC.DC_ByID(vEnquiryId);
            if (dt.Rows.Count > 0)
            {
                sb.AppendFormat(@"<b>{0}</b></td>", "<meta charset=\"UTF-8\">");
                sb.AppendFormat(@"<b>{0}</b></td>", dt.Rows[0]["ConsentForm"].ToString());
            }
            else
            {
                sb.Append(@"Please Contact To Admin");
            }

            Literal1.Text = sb.ToString();
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "mywindow", "printform();", true);
        }
    }
}