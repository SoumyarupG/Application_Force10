using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.IO;
using System.Data;
using System.Configuration;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class DownloadReports : CENTRUMBase
    {
        string path = ConfigurationManager.AppSettings["ReportDrive"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                GetFilesFromFolder();
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);


                this.Menu = false;
                this.PageHeading = "REPORT DOWNLOAD";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuSFTPFILEDOWNLOAD);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void GetFilesFromFolder()
        {
            string vUserId = Convert.ToString(Session[gblValue.UserId]);
            string vFolderPath = path + vUserId;
            DataTable dt = null;
            DataRow dr = null;
            string vFileNm = "", vFileNmCut = "", vFileExt = "";
            Int32 vRow = -1;
            try
            {
                if (System.IO.Directory.Exists(vFolderPath))
                {
                    dt = new DataTable("Table1");
                    dt.Columns.Add("FileName");
                    dt.Columns.Add("vFileName");
                    dt.AcceptChanges();

                    foreach (var file in Directory.GetFiles(vFolderPath))
                    {
                        vFileNmCut = Path.GetFileName(file);
                        vFileExt = Path.GetExtension(file);
                        vFileNm = vFolderPath + "/" + vFileNmCut;

                        dr = dt.NewRow();
                        dt.Rows.Add(dr);
                        vRow = vRow + 1;
                        dt.Rows[vRow][0] = vFileNmCut;
                        dt.Rows[vRow][1] = vFileNm;
                    }

                    dt.AcceptChanges();
                    gvFile.DataSource = dt;
                    gvFile.DataBind();
                }
                else
                {
                    gblFuction.MsgPopup("No Reports Found");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;

            }
        }

        protected void gvFile_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDown")
            {
                string filePath = e.CommandArgument.ToString();
                var vFileName = filePath.Split('/');
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + vFileName.Last());
                Response.WriteFile(filePath);
                Response.Flush();
                Response.End();
            }
            else if (e.CommandName == "cmdDel")
            {
                string filePath = e.CommandArgument.ToString();
                File.Delete(filePath);
                GetFilesFromFolder();
                gblFuction.AjxMsgPopup("Report Deleted Successfully.");                
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");

        }
    }
}