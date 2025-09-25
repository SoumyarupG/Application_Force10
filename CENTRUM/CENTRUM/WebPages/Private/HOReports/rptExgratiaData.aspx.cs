using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class rptExgratiaData : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Exgratia Data";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuExgratiaData);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Exgratia Data", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void SetParameterForRptData(string pMode)
        {
            string vBranch = Session[gblValue.BrName].ToString();
            DataTable dt = null;
            CReports oRpt = new CReports();
            string vFileNm = "";
            try
            {
                oRpt = new CReports();
                dt = oRpt.rptExgratiaData(txtLoanNo.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    if (pMode == "Excel")
                    {
                        vFileNm = "attachment;filename=" + (txtLoanNo.Text) + "_Exgratia_Data.xls";
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", vFileNm);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";
                        HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                        Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress2(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>Loan No- " + txtLoanNo.Text + "</font></td></tr>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>Exgratia Data</font></td></tr>");
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
                                if (dt.Columns[j].ColumnName == "MemberNo")
                                {
                                    Response.Write("<td &nbsp nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                                }
                                else
                                {
                                    Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                                }
                            }
                            Response.Write("</tr>");
                        }

                        Response.Write("</table>");
                        Response.End();
                    }
                    if (pMode == "CSV")
                    {
                        PrintTxt(dt);
                    }
                    else
                    {
                        gblFuction.MsgPopup("No data found...");
                    }
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
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

        protected void btnCsv_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("CSV");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        
        private void PrintTxt(DataTable dt)
        {
            string vFolderPath = "C:\\BijliReport";
            string vFileNm = "";
            vFileNm = vFolderPath + "\\" + txtLoanNo.Text + "_ExGratia_Data.txt";

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
                Write(dt, vFileNm);
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

        
        
    }
}