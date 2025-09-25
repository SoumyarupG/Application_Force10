using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class DeathDeclare : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                StatusButton("View");
                txtDate.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtFromDt.Text = txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                LoadGrid();
                PopWaveofReason();
            }

        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Death Declare";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDeathDeclare);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnDelete.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Premature Collection", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        private void EnableControl(Boolean Status)
        {
            txtDeathDate.Enabled = Status;
            txtLoanId.Enabled = Status;
            txtMemberName.Enabled = Status;
            //ddlDPerson.Enabled = Status;
            ddlWvReason.Enabled = Status;
            txtDate.Enabled = Status;
        }

        private void ClearControls()
        {
            txtLoanId.Text = "";
            txtMemberName.Text = "";
            ViewState["PDCID"] = 0;
            hdnLoanId.Value = "";
            txtPrinAmt.Text = "0.00";
            txtIntAmt.Text = "0.00";
            txtAdvanceAmt.Text = "0.00";
            txtTotal.Text = "0.00";
            ddlDPerson.SelectedIndex = -1;
            ddlWvReason.SelectedIndex = -1;
            txtDeathDate.Text = "";
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = "Add";
                tbPDC.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbPDC.ActiveTabIndex = 0;
            EnableControl(false);
            ClearControls();
            StatusButton("View");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null || vStateEdit == "")
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                StatusButton("Show");
                ViewState["StateEdit"] = null;
            }

        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0, vDeathType=0;
            string vNaration = "", pBranch = "", vWhoDied = "", vRetMsg = "X";
            double vPrinCollAmt = 0, vIntCollAmt = 0, vAdvCollAmt = 0, vTotalCollAmt = 0;
            CLoanRecovery oLR = null;
            DateTime vAccDate = gblFuction.setDate(txtDate.Text);
            DateTime vDeathDate = gblFuction.setDate(txtDeathDate.Text);
            vDeathType = Convert.ToInt32(ddlWvReason.SelectedValue);
            vWhoDied = hdDeathPerson.Value;

            vNaration = "Being the Amt of Loan Collection from ";
            string vActMstTbl = Session[gblValue.ACVouMst].ToString();
            string vActDtlTbl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();
            pBranch = Session[gblValue.BrnchCode].ToString();

            vPrinCollAmt = Convert.ToDouble(txtPrinAmt.Text);
            vIntCollAmt = Convert.ToDouble(txtIntAmt.Text);
            vAdvCollAmt = Convert.ToDouble(txtAdvanceAmt.Text);
            vTotalCollAmt = Convert.ToDouble(txtTotal.Text);
           

            try
            {
                if (Convert.ToInt32(Session[gblValue.RoleId]) != 1)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= vAccDate)
                        {
                            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                            return false;
                        }
                    }
                }
                oLR = new CLoanRecovery();

                vRetMsg = oLR.ChkAllDue(hdnLoanId.Value, vAccDate, pBranch);
                if (vRetMsg != "X")
                {
                    gblFuction.MsgPopup(vRetMsg);
                    return false;
                }

                vErr = oLR.SaveDeathCollection(hdnLoanId.Value, vAccDate, pBranch, vActMstTbl, vActDtlTbl, vFinYear, vNaration,
                    Convert.ToInt32(Session[gblValue.UserId]), vPrinCollAmt, vIntCollAmt, vAdvCollAmt, vTotalCollAmt,vDeathType,vWhoDied,vDeathDate);

                if (vErr == 0)
                {
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                    vResult = false;
                }
            }
            catch (Exception ex)
            {
                gblFuction.AjxMsgPopup(ex.ToString());
            }
            return vResult;

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    //LoadGrid(1);
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadGrid()
        {
            DataTable dt = null;
            string vBrCode = "";
            CLoanRecovery oBr = null;
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oBr = new CLoanRecovery();
                dt = oBr.GetDeathClientList(gblFuction.setDate(txtFromDt.Text), gblFuction.setDate(txtToDt.Text), txtLoanNoSearch.Text, vBrCode);
                gvPreMatureList.DataSource = dt;
                gvPreMatureList.DataBind();

            }
            finally
            {
                oBr = null;
                dt = null;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void PopWaveofReason()
        {
            Int32 vRows = 0;
            DataTable dt = null;
            CWaveOff oWv = new CWaveOff();
            dt = oWv.GetWavePG(1, ref vRows);
            ddlWvReason.DataTextField = "LoanWaveReason";
            ddlWvReason.DataValueField = "LoanWaveoffId";
            ddlWvReason.DataSource = dt;
            ddlWvReason.DataBind();
            ListItem oItem = new ListItem();
            oItem.Text = "<--- Select --->";
            oItem.Value = "-1";
            ddlWvReason.Items.Insert(0, oItem);
        }
    }
}