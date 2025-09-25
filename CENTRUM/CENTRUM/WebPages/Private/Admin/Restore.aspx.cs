using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using FORCECA;
using FORCEBA;
using FORCEDA;


namespace CENTRUM.WebPages.Private.Admin
{
    public partial class Restore : CENTRUMBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBasePage();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            this.PageHeading = "Data Restore";
            this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
            this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
            this.GetModuleByRole(mnuID.mnuBrnchBackupRestr);
            if (this.UserID == 1) return;
            if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
            {
                btnRes.Visible = false;
            }
            else
            {
                Server.Transfer("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Data Restore", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRes_Click(object sender, EventArgs e)
        {
            string vFileName = "", vBrCode = "";            
            string vSrvName = ConfigurationManager.AppSettings["SrvName"];
            string vDBName = ConfigurationManager.AppSettings["DBName"];
            string vPw = ConfigurationManager.AppSettings["PassPW"];
            string vDirPath = txtPath.Text;
            string vLogPath = vDirPath + "\\BranchLog.txt";

            if (vDirPath == "")
            {
                gblFuction.MsgPopup("Please Select Restoration Folder");
                return;
            }         
            if (File.Exists(vLogPath) == true)
            {
                StreamReader sw = new StreamReader(vLogPath);
                {
                    vBrCode = sw.ReadLine();
                }
            }
            else
            {
                gblFuction.MsgPopup("No Branch Log Exist");
                return;
            }

            string execPath = @"C:\Program Files\Microsoft SQL Server\100\Tools\Binn\bcp.exe";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            if (System.IO.Directory.Exists(vDirPath))
            {
                SqlCommand oCmdUp = new SqlCommand();
                oCmdUp.CommandType = CommandType.Text;
                oCmdUp.CommandText = "EXEC sp_msforeachtable \"ALTER TABLE ? NOCHECK CONSTRAINT all\"";
                DBUtility.Execute(oCmdUp);
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(vDirPath);
                System.IO.FileInfo[] fi = di.GetFiles("*.csv");
                foreach (System.IO.FileInfo file in fi)
                {
                    string QString = "";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.FileName = execPath;
                    SqlCommand oCmd = new SqlCommand();
                    SqlCommand oCmd1 = new SqlCommand();
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    oCmd.CommandType = CommandType.Text;
                    oCmd1.CommandType = CommandType.Text;
                    vFileName = file.Name;
                    vFileName = vFileName.Replace(".csv", "");
                    oCmd.CommandText = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + vFileName + "' AND  COLUMN_NAME = 'BranchCode'";
                    DBUtility.ExecuteForSelect(oCmd, dt);
                    if (dt.Rows.Count > 0)
                        QString = "DELETE FROM " + vDBName + ".dbo." + vFileName + " WHERE BranchCode IN (" + vBrCode + ")";
                    else
                        QString = "DELETE FROM " + vFileName + " ";
                    oCmd1.CommandText = QString;
                    DBUtility.Execute(oCmd1);
                    process.StartInfo.Arguments = "" + vDBName + ".dbo." + vFileName + " in " + vDirPath + "\\" + vFileName + ".csv -E -c -t || -S" + vSrvName + " -Usa -P" + vPw + "";
                    process.Start();
                    process.WaitForExit();
                    String processError = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    String processOutput = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    process.Close();
                }
                oCmdUp.CommandText = "exec sp_msforeachtable @command1=\"print '?'\", @command2=\"ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all\"";
                DBUtility.Execute(oCmdUp);
                gblFuction.MsgPopup("Database Restored Successfully");
                oCmdUp.Dispose();
                process.Dispose();
            }
            else
            {
                gblFuction.MsgPopup("There is no backup folder in D:\\");
                return;
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBrowse_Click(object sender, EventArgs e)
        {
            //string strPath;
            //string strCaption = "Select a directory.";
            //DialogResult dlgResult;

            //Shell32.ShellClass shl = new Shell32.ShellClass();
            //Shell32.Folder2 fld = (Shell32.Folder2)shl.BrowseForFolder(0, strCaption, 0,
            //            System.Reflection.Missing.Value);

            //if (fld == null)
            //{
            //    dlgResult = DialogResult.Cancel;
            //}
            //else
            //{
            //    strPath = fld.Self.Path;
            //    txtPath.Text = strPath;
            //    dlgResult = DialogResult.OK;
            //}
        }
    }
}