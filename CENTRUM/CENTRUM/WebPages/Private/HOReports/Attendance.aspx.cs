using System;
using System.Data;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Web;
using System.Configuration;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class Attendance : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                //PopBranch(Session[gblValue.UserName].ToString());
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDate.Text = Session[gblValue.LoginDate].ToString();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Attendance Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuAttendanceRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Attendance Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }

        private void SetParameterForRptData(string pMode)
        {
            CAttendance obj;
            DataTable dt = null;
            if (txtFrmDt.Text.Trim().Length == 0)
            {
                gblFuction.MsgPopup("From date can not blank...");
                return;
            }
            if (txtToDate.Text.Trim().Length == 0)
            {
                gblFuction.MsgPopup("To date can not blank...");
                return;
            }
            ClearField();
            try
            {
                TimeSpan t = gblFuction.setDate(txtToDate.Text.Trim()) - gblFuction.setDate(txtFrmDt.Text.Trim());
                if (t.TotalDays > 2)
                {
                    gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                    return;
                }

                obj = new CAttendance();
                dt = obj.RptAttandanceXLS_Photo(gblFuction.setDate(txtFrmDt.Text.Trim())
                                                , gblFuction.setDate(txtToDate.Text.Trim())
                                                , hdnUserName.Value.ToString().Trim().Length == 0 ? "-1" : hdnUserName.Value.ToString().Trim());
                if (dt.Rows.Count > 0)
                {
                    string vFileNm = "attachment;filename=Attendance Report.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                    Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                    //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>Attendance Report Status For the Period from " + txtFrmDt.Text + " to " + txtToDate.Text + "</font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'></font></b></td></tr>");
                    string tab = string.Empty;
                    Response.Write("<tr>");
                    foreach (DataColumn dtcol in dt.Columns)
                    {
                        Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
                    }
                    Response.Write("</tr>");
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        Response.Write("<tr style='height:20px;'>");
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                        }
                        Response.Write("</tr>");
                    }
                    Response.Write("</table>");
                    Response.End();
                }
                else
                {
                    gblFuction.MsgPopup("No record found to generate report.");
                    return;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        public void ClearField()
        {
            txtTimeIn.Text = "";
            txtLocationIn.Text = "";
            txtAddressIn.Text = "";
            txtTimeOut.Text = "";
            txtLocationOut.Text = "";
            txtAddressOut.Text = "";
            ImgIn.ImageUrl = "";
            ImgOut.ImageUrl = "";
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            CAttendance obj;
            DataSet ds = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            String vPath = "";
            String ConfigPath = "";
            try
            {
                obj = new CAttendance();
                dt1 = new DataTable();
                dt2 = new DataTable();
                ds = new DataSet();
                ConfigPath = ConfigurationManager.AppSettings["AttendanceImagePath"].ToString();

                vPath = ConfigPath + hdnUserName.Value.ToString() + "_" + txtToDate.Text.Trim().Replace("/", "-").ToString();
                if (txtToDate.Text.Trim().Length == 0)
                {
                    gblFuction.MsgPopup("To date can not blank...");
                    return;
                }
                if (hdnUserName.Value.ToString().Trim().Length == 0)
                {
                    gblFuction.MsgPopup("User can not be blank...");
                    return;
                }
                ds = obj.ViewAttendanceReport(gblFuction.setDate(txtToDate.Text.Trim()), hdnUserName.Value.ToString().Trim());
                if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0 )
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    ClearField();
                    if (dt1.Rows.Count > 0)
                    {
                        txtTimeIn.Text = dt1.Rows[0]["EffTime"].ToString();
                        txtLocationIn.Text = dt1.Rows[0]["Latitute"].ToString() + "," + dt1.Rows[0]["Longitute"].ToString();
                        txtAddressIn.Text = dt1.Rows[0]["Address_LILO"].ToString();
                        ImgIn.ImageUrl = vPath + "_LI.png";
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        txtTimeOut.Text = dt2.Rows[0]["EffTime"].ToString();
                        txtLocationOut.Text = dt2.Rows[0]["Latitute"].ToString() + "," + dt2.Rows[0]["Longitute"].ToString();
                        txtAddressOut.Text = dt2.Rows[0]["Address_LILO"].ToString();
                        ImgOut.ImageUrl = vPath + "_LO.png";
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No record found to view.");
                    return;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                if (ConfigPath.ToString().Trim() == "")
                {
                    gblFuction.MsgPopup("Image Configuration path not Configured..");
                    return;
                }
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}