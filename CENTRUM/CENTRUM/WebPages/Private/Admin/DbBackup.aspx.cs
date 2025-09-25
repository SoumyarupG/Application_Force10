using System;
using System.Collections;
using System.Configuration;
using System.Data;
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

namespace CENTRUM.WebPages.Private.Admin
{
    public partial class DbBackup : CENTRUMBase
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
                PopFiles();
                InitBasePage();
                lblPath.Text = "Note: Database Backup Path is " + ConfigurationManager.AppSettings["DBPath"];
                txtPath.Text = gblFuction.setJpnFrmt(Session[gblValue.LoginDate].ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            this.PageHeading = "Database Backup";
            this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
            this.ShowFinYear =Session[gblValue.FinYear].ToString();
            this.GetModuleByRole(mnuID.mnuFullBackup);
            if (this.UserID == 1) return;
            if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
            {
                btnBack.Visible = false;
            }
            else
            {
                Server.Transfer("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Database Backup", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopFiles()
        {
            string vFolderPath = ConfigurationManager.AppSettings["DBPath"];
            try
            {
                DirectoryInfo oDI = new DirectoryInfo(vFolderPath);
                if (oDI.Exists)
                {
                    lbxF.Items.Clear();
                    lbxF.DataSource = oDI.GetFiles();
                    lbxF.DataBind();
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Int32 vRst = 0;
            string vFolderPath = ConfigurationManager.AppSettings["DBPath"];
            string vDBNm = ConfigurationManager.AppSettings["DBName"];
            string vBrCode = Session[gblValue.BrnchCode].ToString();           
            string vFileNm = vFolderPath + vDBNm + "_" + txtPath.Text + "_" + System.DateTime.Now.ToShortTimeString() + ".bak";
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

                vRst = CGblIdGenerator.ProcDbBackup(vDBNm, vFolderPath, vBrCode, txtPath.Text);
                if (vRst > 0)
                    gblFuction.AjxMsgPopup("Database Backup made Successfully.");
                else
                    gblFuction.AjxMsgPopup("Failed to take Database Backup.");
                PopFiles();
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