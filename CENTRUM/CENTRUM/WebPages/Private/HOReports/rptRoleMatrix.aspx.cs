using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class rptRoleMatrix : CENTRUMBase
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
                this.PageHeading = "Role Matrix Report";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuRoleMatrix);
                if (this.UserID == 1) return;
                if (this.CanView == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Consolidated Reports", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }

        protected void btnCsv_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("CSV");
        }

        private void SetParameterForRptData(string pMode)
        {
            DataTable dt = null;
            CReports oRpt = new CReports();
            string vFileNm = "attachment;filename=Role_Matrix_Report_" + DateTime.Now + ".xls";
            try
            {
                oRpt = new CReports();
                dt = oRpt.RptRoleMatrix();
                if (dt.Rows.Count > 0)
                {
                    if (pMode == "Excel")
                    {
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", vFileNm);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";
                        HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                        Response.Write("<table border='1' cellpadding='5' widht='120%'>");
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
                    else if (pMode == "CSV")
                    {
                        PrintTxt(dt);
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Data Found.");
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }

        private void PrintTxt(DataTable dt)
        {
            string vFolderPath = "C:\\BijliReport";
            string vFileNm = "";
            vFileNm = vFolderPath + "\\" + gblFuction.setDate(Session[gblValue.LoginDate].ToString()).ToString("yyyyMMdd") + "_ " + Session[gblValue.BrnchCode].ToString() + "_Role_Matrix.txt";

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