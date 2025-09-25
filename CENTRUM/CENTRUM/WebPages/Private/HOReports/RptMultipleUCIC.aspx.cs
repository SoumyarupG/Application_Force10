using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Configuration;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class RptMultipleUCIC : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Session[gblValue.FinFromDt].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Multiple UCIC Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuMultipleUcic);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Multiple UCIC Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = Convert.ToString(ViewState["BrCode"]);
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();

            //TimeSpan t = vToDt - vFromDt;
            //if (t.TotalDays > 2)
            //{
            //    gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
            //    return;
            //}
            dt = oRpt.rptMultipleUCIC(vFromDt, vToDt);
            if (pMode == "Excel")
            {
                string vFileNm = "attachment;filename=Multiple UCIC Report.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>Multiple UCIC</font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>For the Period from " + txtDtFrm.Text + " to " + txtToDt.Text + "</font></b></td></tr>");
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
                PrintTxt(dt);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {            
            SetParameterForRptData("Excel");
        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("CSV");
        }

        private void PrintTxt(DataTable dt)
        {
            string vFolderPath = "C:\\BijliReport";
            string vFileNm = "";
            vFileNm = vFolderPath + "\\MultipleUCIC_Report.csv";

            try
            {
                if (System.IO.Directory.Exists(vFolderPath))
                {
                    foreach (var file in Directory.GetFiles(vFolderPath))
                    {
                        if (File.Exists(vFileNm) == true)
                            File.Delete(vFileNm);
                    }
                }
                else
                {
                    Directory.CreateDirectory(vFolderPath);
                }
                StreamWriter sw = new StreamWriter(vFileNm, false);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sw.Write(dt.Columns[i]);
                    if (i < dt.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            string value = dr[i].ToString();
                            if (value.Contains(","))
                            {
                                value = String.Format("\"{0}\"", value);
                                sw.Write(value);
                            }
                            else
                            {
                                sw.Write(dr[i].ToString());
                            }
                        }
                        if (i < dt.Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
                // Write(dt, vFileNm);
                downloadfile(vFileNm);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                gblFuction.MsgPopup("Done");
                btnExit.Enabled = true;

            }
        }

        private void Write(DataTable dt, string outputFilePath)
        {
            int[] maxLengths = new int[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                maxLengths[i] = dt.Columns[i].ColumnName.Length;
                foreach (DataRow row in dt.Rows)
                {
                    if (!row.IsNull(i))
                    {
                        int length = row[i].ToString().Length;
                        if (length > maxLengths[i])
                        {
                            maxLengths[i] = length;
                        }
                    }
                }
            }
            using (StreamWriter sw = new StreamWriter(outputFilePath, false))
            {

                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    sw.Write(dt.Columns[i].ColumnName.ToString().Trim() + '|');

                }
                sw.WriteLine();

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!row.IsNull(i))
                        {
                            sw.Write(row[i].ToString().Trim() + '|');
                        }

                    }
                    sw.WriteLine();
                }
                sw.Close();
            }
        }

        private void downloadfile(string filename)
        {
            // check to see that the file exists 
            if (File.Exists(filename))
            {
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(filename);
                Response.End();
                File.Delete(filename);
            }
            else
            {
                gblFuction.AjxMsgPopup("File could not be found");
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}