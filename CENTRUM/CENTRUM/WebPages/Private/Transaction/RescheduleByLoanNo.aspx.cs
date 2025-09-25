using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;
namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class RescheduleByLoanNo : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                lblMemName.Text = "";
                ddlLoanNo.SelectedIndex = -1;
                ddlMem.SelectedIndex = -1;
                PopRO();
                //txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                //txtToDt.Text = Session[gblValue.LoginDate].ToString();
                //LoadGrid(txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Reschedule By Loan No:";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuRescheduleByLoanNo);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void PopRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode;
            vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlRO.DataSource = dt;
                ddlRO.DataTextField = "EoName";
                ddlRO.DataValueField = "EoId";
                ddlRO.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRO.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }
        protected void ddlRO_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vRoId = "";
            string vCentId = "";
            vCentId = Convert.ToString(ddlRO.SelectedValue);
            vRoId = Convert.ToString(ddlRO.SelectedValue);
            //PopCenter(vRoId);
            PopGroup(vCentId);
        }
        private void PopGroup(string vCenterID)
        {
            ddlGroup.Items.Clear();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "GroupID", "GroupName", "GroupMst", vCenterID, "MarketID", "Tra_DropDate", vLogDt, vBrCode);
                ddlGroup.DataSource = dt;
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataValueField = "GroupID";
                ddlGroup.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlGroup.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vGropId = "";
            vGropId = ddlGroup.SelectedValue.ToString();
            PopMember(vGropId);
        }
        private void PopMember(string vGroupID)
        {
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CMember oMem = null;
            string vBrCode = "";

            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oMem = new CMember();
                dt = oMem.GetMemListByGroupId(vGroupID, vBrCode);
                ddlMem.DataSource = dt;
                ddlMem.DataTextField = "MemberName";
                ddlMem.DataValueField = "MemberID";
                ddlMem.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlMem.Items.Insert(0, oli);
            }
            finally
            {
                oMem = null;
                dt = null;
            }
        }
        protected void ddlMem_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            string vMemId = ddlMem.SelectedValue;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CMember oMem = null;
            lblMemName.Text = "";
            ddlLoanNo.Items.Clear();
            try
            {
                oMem = new CMember();
                dt = oMem.GetMemberDtlByMemberNo(vMemId, vBrCode);
                if (dt.Rows.Count > 0)
                {
                    lblMemName.Text = dt.Rows[0]["MemberNo"].ToString();
                    PopLoan(vMemId);
                }
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        private void PopLoan(string pMemberID)
        {
            ddlLoanNo.Items.Clear();
            DataTable dt = null;
            CLoan oLn = null;
            string vBrCode = "";
            DateTime logindate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oLn = new CLoan();
                dt = oLn.GetLoanByMemId(pMemberID, vBrCode, logindate);
                ddlLoanNo.DataSource = dt;
                ddlLoanNo.DataTextField = "LoanNo";
                ddlLoanNo.DataValueField = "LoanId";
                ddlLoanNo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLoanNo.Items.Insert(0, oli);
            }
            finally
            {
                oLn = null;
                dt = null;
            }
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            LoadGrid(1);
        }
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CReScheduling oLD = null;
            Int32 vRows = 0;
            string vBrCode = string.Empty;
            vBrCode = (string)Session[gblValue.BrnchCode];
            //DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            //DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            oLD = new CReScheduling();
            dt = oLD.GetLoanReSchedule(vBrCode, ddlLoanNo.SelectedItem.Text, pPgIndx, ref vRows);
            gvResch.DataSource = dt.DefaultView;
            gvResch.DataBind();
            ViewState["Reschedule"] = dt;
            lblTotalPages.Text = CalTotPgs(vRows).ToString();
            lblCurrentPage.Text = cPgNo.ToString();
            if (cPgNo == 1)
            {
                Btn_Previous.Enabled = false;
                if (Int32.Parse(lblTotalPages.Text) > 1)
                    Btn_Next.Enabled = true;
                else
                    Btn_Next.Enabled = false;
            }
            else
            {
                Btn_Previous.Enabled = true;
                if (cPgNo == Int32.Parse(lblTotalPages.Text))
                    Btn_Next.Enabled = false;
                else
                    Btn_Next.Enabled = true;
            }
        }
        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid(cPgNo);
            //tabLoanDisb.ActiveTabIndex = 0;
            //tabLoanDisb.Tabs[0].Enabled = true;
            //tabLoanDisb.Tabs[1].Enabled = false;
            //tabLoanDisb.Tabs[2].Enabled = false;
        }
        protected void btnDone_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Reschedule"];
            string vXml = "";
            Int32 vErr = 0;
            CReScheduling oMic = null;
            if (dt == null) return;
            if (dt.Rows.Count > 0)
            {
                for (int r = 0; gvResch.Rows.Count > r; r++)
                {
                    TextBox Duedt = (TextBox)gvResch.Rows[r].FindControl("txtDueDt");
                    dt.Rows[r]["DueDate"] = Duedt.Text.Substring(3,2)+"/"+Duedt.Text.Substring(0,2)+"/"+Duedt.Text.Substring(6,4);
                }
                dt.AcceptChanges();
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
                oMic = new CReScheduling();
                vErr = oMic.SaveDueDt(vXml);
                if (vErr == 1 )
                {
                    gblFuction.MsgPopup("Record Updated successfully..");
                }
                else if (vErr == 2 || vErr == 0)
                {
                    gblFuction.MsgPopup("Record not Updated successfully..");

                }
            }
        }
        protected void gvResch_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}