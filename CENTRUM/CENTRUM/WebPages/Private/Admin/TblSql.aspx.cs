using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using FORCECA;
using FORCEBA;
using FORCEDA;

namespace CENTRUM.WebPages.Private.Admin
{
    public partial class TblSql : CENTRUMBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            this.PageHeading = "Alter Tables";
            this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
            this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
            this.GetModuleByRole(mnuID.mnuAltrTab);
            if (this.UserID == 1) return;
            if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
            {
                btnBack.Visible = false;
            }
            else
            {
                Server.Transfer("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Alter Tables", false);
            }
        }         

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            int vEx;
            vEx=AlterTables();
            if (vEx == 0)
                gblFuction.MsgPopup("Tables Altered Successfully..");
            else if (vEx == 1)
                gblFuction.MsgPopup("ERROR:Failed To Alter Tables..");
        }

        /// <summary>
        /// 
        /// </summary>
        private int AlterTables()
        {
            int vExp = 0;
            try 
            {
                return vExp;
            }
            catch(Exception ex)
            {
                vExp = 1;
                return vExp;
                throw ex;
            }
            finally{}
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
