using System;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.Private.Webpages.Admin
{
    public partial class YearEnd : CENTRUMBAse
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
            { }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.PageHeading = "Process Carry Forward";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear =  Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuYearEnd);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnProc.Visible = false;
                    btnExit.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnProc.Visible = false;
                    btnExit.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnProc.Visible = false;
                    btnExit.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Day End Process", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateDate()
        {
            bool vRst = true;
            string vLogDate = Session[gblValue.LoginDate].ToString();
            DateTime vLogDt = gblFuction.setDate(vLogDate);
            DateTime vEndDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            if (vLogDt != vEndDt)
            {
                gblFuction.MsgPopup("You Can Not Process Carry Forward On " + vLogDate);
                vRst = false;
            }
            return vRst;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProc_Click(object sender, EventArgs e)
        {
            Int32 vRst = 0;
            CYearEnd oYEnd = null;
            if (ValidateDate() == false) return;
            Int32 vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
            string vBranch = Session[gblValue.BrnchCode].ToString();
            oYEnd = new CYearEnd();
            vRst = oYEnd.ProcYearEnd(vYrNo, vBranch,this.UserID,gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
            if (vRst == 1)
                gblFuction.MsgPopup("Carry Forward Completed Successfully.");
            else
                gblFuction.MsgPopup("Carry Forward Not Completed Successfully.");
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