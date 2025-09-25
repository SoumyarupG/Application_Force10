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
    public partial class Backup : CENTRUMBase 
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
                PopBranch();
                CheckAll();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            this.PageHeading = "Data Backup";
            this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
            this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
            this.GetModuleByRole(mnuID.mnuBrnchBackup);
            if (this.UserID == 1) return;
            if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
            {
                btnBack.Visible = false;
            }
            else
            {
                Server.Transfer("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Data Backup", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopBranch()
        {
            DataTable dt = null;
            CBranch oBr = null;
            try
            {
                oBr = new CBranch();
                dt = oBr.GetBranchList();
                lvwBr.DataTextField = "BranchName";
                lvwBr.DataValueField = "BranchCode";
                lvwBr.DataSource = dt;
                lvwBr.DataBind();
                CheckAll();
            }
            finally
            {
                oBr = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckAll()
        {
            Int32 vRow;
            if (rdbOpt.SelectedValue == "rdbAll")
            {
                for (vRow = 0; vRow < lvwBr.Items.Count; vRow++)
                    lvwBr.Items[vRow].Selected = true;
                lvwBr.Enabled = false;
            }
            if (rdbOpt.SelectedValue == "rdbSel")
            {
                for (vRow = 0; vRow < lvwBr.Items.Count; vRow++)
                    lvwBr.Items[vRow].Selected = false;
                lvwBr.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Int32 vRow;
            string vTable = "", vBrCode = "";           
            string vSrvName = ConfigurationManager.AppSettings["SrvName"];
            string vDBName = ConfigurationManager.AppSettings["DBName"];
            string vPw = ConfigurationManager.AppSettings["PassPW"];
            string vPath = txtPath.Text;
            if (vPath == "")
            {
                gblFuction.MsgPopup("Please Select Backup Location");
                return;
            }
            for (vRow = 0; vRow < lvwBr.Items.Count; vRow++)
            {
                if (lvwBr.Items[vRow].Selected == true)
                {
                    if (vBrCode == "")
                        vBrCode = "'" + lvwBr.Items[vRow].Value + "'";
                    else
                        vBrCode = vBrCode + ",'" + lvwBr.Items[vRow].Value + "'";
                }
            }
            if (System.IO.Directory.Exists(vPath))
            {
                foreach (var file in Directory.GetFiles(vPath))
                    File.Delete(file);
            }
            string vLogPath = vPath + "\\BranchLog.txt";
            using (StreamWriter sw = File.CreateText(vLogPath))
            {
                sw.WriteLine(vBrCode);
            }
            string execPath = @"C:\Program Files\Microsoft SQL Server\100\Tools\Binn\bcp.exe";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            oCmd.CommandType = CommandType.Text;
            oCmd.CommandText = "SELECT * FROM sys.Tables where is_ms_shipped<>1";
            DBUtility.ExecuteForSelect(oCmd, dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dR in dt.Rows)
                {
                    SqlCommand oCmd1 = new SqlCommand();
                    DataTable dt1 = new DataTable();
                    vTable = dR["name"].ToString();
                    oCmd1.CommandType = CommandType.Text;
                    oCmd1.CommandText = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + vTable + "' AND  COLUMN_NAME = 'BranchCode'";
                    DBUtility.ExecuteForSelect(oCmd1, dt1);
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.FileName = execPath;
                    if (dt1.Rows.Count > 0)
                        process.StartInfo.Arguments = "\" SELECT * FROM " + vDBName + ".dbo." + vTable + " WHERE BranchCode IN (" + vBrCode + ")\" queryout " + vPath + "\\" + vTable + ".csv -E -c -t || -S" + vSrvName + " -Usa -P" + vPw + "";
                    else
                        process.StartInfo.Arguments = "" + vDBName + ".dbo." + vTable + " out " + vPath + "\\" + vTable + ".csv -E -c -t || -S" + vSrvName + " -Usa -P" + vPw + "";
                    process.Start();
                    process.WaitForExit();
                    String processError = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    String processOutput = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    process.Close();
                }
            }
            gblFuction.MsgPopup("Database Backup Successfully");
            oCmd.Dispose();
            process.Dispose();
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
        protected void rdbOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
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