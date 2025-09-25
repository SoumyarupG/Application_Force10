using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.IO;
using System.Web.Hosting;
using System.Web;
using System.Configuration;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Globalization;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class InitialApproachEdit : CENTRUMBase
    {
        protected int vPgNo = 1;
        protected string vMemberId = "";
        string path = "", pathMember = "", pathImage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBasePage();
                StatusButton("View");
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                popVillage();
                popIdentityProof();
                popAddProof();
                popCltype();
                tbEmp.ActiveTabIndex = 0;
                Panel2.Visible = false;
                btnAdd.Visible = false;
                btnEdit.Enabled = false;
                popState();
                clearMemPhoto();
                popRelation();
                ViewState["AadhaarScan"] = "M";
                GenerateEarningMember();
                popBusinessType();
                popOccupation();
                //popCoAppIdProof();
                popBusinessActivityAll();
            }
        }

        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                //dt = oGb.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                dt = oGb.GetAllStateList();
                ddlStat.DataSource = dt;
                ddlStat.DataTextField = "StateName";
                ddlStat.DataValueField = "StateId";
                ddlStat.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlStat.Items.Insert(0, oli);

                ddlCoAppState.DataSource = dt;
                ddlCoAppState.DataTextField = "StateName";
                ddlCoAppState.DataValueField = "StateId";
                ddlCoAppState.DataBind();
                ddlCoAppState.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Initial Approach";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuMemberMst);
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
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {

                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Member Master", false);
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
        /// <param name="pMode"></param>
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
                    EnableControl(false);
                    break;
            }
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUserHO(pUser, Convert.ToInt32(Session[gblValue.RoleId]));

                if (dt.Rows.Count > 0)
                {

                    ListItem liSel = new ListItem("<--- Select --->", "-1");

                }
                else
                {
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pEoId"></param>
        private void PopCenter(string pEoId)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("S", "N", "AA", "MarketID", "Market", "MarketMSt", pEoId, "EoId", "AA", gblFuction.setDate("01/01/1900"), "0000");
                if (dt.Rows.Count > 0)
                {
                    //ViewState["CentreID"] = Convert.ToString(dt.Rows[0]["Marketid"]).Trim();
                    ddlCenter.DataSource = dt;
                    ddlCenter.DataTextField = "Market";
                    ddlCenter.DataValueField = "MarketID";
                    ddlCenter.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlCenter.Items.Insert(0, oli);
                }
            }
            finally
            {
                dt = null;
                oGb = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popVillage()
        {
            DataTable dt = null;
            CVillage oGb = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oGb = new CVillage();
                dt = oGb.PopVillage(vBrCode);
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCommVill.DataSource = dt;
                ddlCommVill.DataTextField = "VillageName";
                ddlCommVill.DataValueField = "VillageId";
                ddlCommVill.DataBind();
                ddlCommVill.Items.Insert(0, oli);

            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlVillg_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CVillage oVlg = null;
            string vVlgId = txtVillg.Text;
            try
            {
                oVlg = new CVillage();
                dt = oVlg.GetGpBlkDistStateList(vVlgId);
                ddlMuPanc.DataSource = ddlBlk.DataSource = ddlStat.DataSource = dt;
                ddlMuPanc.DataTextField = "GPName";
                ddlMuPanc.DataValueField = "GPId";
                ddlMuPanc.DataBind();
                ddlBlk.DataTextField = "BlockName";
                ddlBlk.DataValueField = "BlockId";
                ddlBlk.DataBind();
                ddlStat.DataTextField = "StateName";
                ddlStat.DataValueField = "StateId";
                ddlStat.DataBind();
                ddlMuPanc.Enabled = false;
                ddlBlk.Enabled = false;
                ddlStat.Enabled = false;
            }
            finally
            {
                oVlg = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>



        /// <summary>
        /// 
        /// </summary>
        private void popAddProof()
        {
            DataTable dt = null;
            CNewMember oNM = null;
            try
            {
                oNM = new CNewMember();
                dt = oNM.popIdAddrProof("Y", "N");
                ddlAddPrf.DataSource = dt;
                ddlAddPrf.DataTextField = "IDProofName";
                ddlAddPrf.DataValueField = "IDProofId";
                ddlAddPrf.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlAddPrf.Items.Insert(0, oli1);

                ddlIdProof3.DataSource = dt;
                ddlIdProof3.DataTextField = "IDProofName";
                ddlIdProof3.DataValueField = "IDProofId";
                ddlIdProof3.DataBind();
                ddlIdProof3.Items.Insert(0, oli1);
            }
            finally
            {
                oNM = null;
                dt = null;
            }
        }

        private void popAgainstVillage2()
        {
            DataTable dt = null;
            CVillage oVlg = null;
            string vVlgId = ddlCommVill.SelectedValue;
            try
            {
                oVlg = new CVillage();
                dt = oVlg.GetGpBlkDistStateList(vVlgId);
                ddlCommMuni.DataSource = ddlCommBlock.DataSource = ddlCommDist.DataSource = ddlCommState.DataSource = dt;
                ddlCommMuni.DataTextField = "GPName";
                ddlCommMuni.DataValueField = "GPId";
                ddlCommMuni.DataBind();
                ddlCommBlock.DataTextField = "BlockName";
                ddlCommBlock.DataValueField = "BlockId";
                ddlCommBlock.DataBind();
                ddlCommDist.DataTextField = "DistrictName";
                ddlCommDist.DataValueField = "DistrictId";
                ddlCommDist.DataBind();
                ddlCommState.DataTextField = "StateName";
                ddlCommState.DataValueField = "StateId";
                ddlCommState.DataBind();
            }
            finally
            {
                oVlg = null;
                dt = null;
            }
        }

        protected void chkCommAddr_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCommAddr.Checked == true)
            {
                ddlCommAddrType.SelectedIndex = ddlAddrType.SelectedIndex;
                txtCommHouseNo.Text = txtHouNo.Text;
                txtCommSt.Text = txtStName.Text;
                txtCommSubDist.Text = txtWardNo.Text;
                txtCommPost.Text = txtPOff.Text;
                txtCommPin.Text = txtPin.Text;
                txtCommLandmark.Text = txtLandMark.Text;
                txtCommArea.Text = txtArea.Text;
                txtCommMob.Text = txtMob.Text;
                txtCommPhone.Text = txtPhNo.Text;
                txtCommHouseNo.Enabled = false;
                txtCommSt.Enabled = false;
                txtCommSubDist.Enabled = false;
                txtCommPost.Enabled = false;
                txtCommPin.Enabled = false;
                ddlCommVill.Enabled = false;
            }
            else
            {
                txtCommHouseNo.Text = "";
                txtCommSt.Text = "";
                txtCommSubDist.Text = "";
                txtCommPost.Text = "";
                txtCommPin.Text = "";
                ddlCommVill.SelectedIndex = -1;
                ddlCommMuni.SelectedIndex = -1;
                ddlCommBlock.SelectedIndex = -1;
                ddlCommDist.SelectedIndex = -1;
                ddlCommState.SelectedIndex = -1;
                txtCommHouseNo.Enabled = true;
                txtCommSt.Enabled = true;
                txtCommSubDist.Enabled = true;
                txtCommPost.Enabled = true;
                txtCommPin.Enabled = true;
                ddlCommVill.Enabled = true;
            }
        }

        protected void ddlCommVill_SelectedIndexChanged(object sender, EventArgs e)
        {
            popAgainstVillage2();
        }

        /// <summary>
        /// 
        /// </summary>
        private void popIdentityProof()
        {
            DataTable dt = null;
            CNewMember oNM = null;
            try
            {
                oNM = new CNewMember();
                dt = oNM.popIdAddrProof("N", "Y");
                ddlIdentyProf.DataSource = dt;
                ddlIdentyProf.DataTextField = "IDProofName";
                ddlIdentyProf.DataValueField = "IDProofId";
                ddlIdentyProf.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlIdentyProf.Items.Insert(0, oli);

                ddlCoAppID.DataSource = dt;
                ddlCoAppID.DataTextField = "IDProofName";
                ddlCoAppID.DataValueField = "IDProofId";
                oli = new ListItem("<--Select-->", "-1");
                ddlCoAppID.DataBind();
                ddlCoAppID.Items.Insert(0, oli);
            }
            finally
            {
                oNM = null;
                dt = null;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (this.CanAdd == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Add);
                return;
            }
            ViewState["StateEdit"] = "Add";
            tbEmp.ActiveTabIndex = 1;

            StatusButton("Add");
            ClearControls();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            CMember oMem = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            int vRet = 0;
            string vMemId = Convert.ToString(ViewState["MemId"]);
            if (this.CanEdit == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Edit);
                return;
            }
            //btnCgtApply.Enabled = false;
            tbEmp.ActiveTabIndex = 1;
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
            this.GetModuleByRole(mnuID.mnuMemberMst);

            if (this.RoleId != 1)  //&& this.RoleId != 5 && this.RoleId != 10 && this.RoleId != 25 && this.RoleId != 13
            {
                oMem = new CMember();
                vRet = oMem.ChkMemEdit(vMemId, vLogDt);
                if (vRet == 0)
                {
                    EnableControl(false);
                }
            }
            if (Convert.ToString(ViewState["AadhaarScan"]) == "A")
            {
                if (ddlIdentyProf.SelectedValue == "1")
                {
                    txtIdntPrfNo.Enabled = false;
                }
                else if (ddlAddPrf.SelectedValue == "1")
                {
                    txtAddPrfNo.Enabled = false;
                }
                else if (ddlIdProof3.SelectedValue == "1")
                {
                    txtIdProof3.Enabled = false;
                }
            }
            gvEarningMember.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbEmp.ActiveTabIndex = 0;
            StatusButton("View");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                EnableControl(false);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateField(string pMemberId, string pBrCode)
        {
            bool vRst = true;
            DataTable dt1 = null, dt2 = null;
            Int32 vRet1 = 0;
            DataSet ds = null;
            CMember oMem = null;

            oMem = new CMember();
            ds = oMem.ChkMemStatus(pMemberId, pBrCode);
            dt1 = ds.Tables[0];
            if (Convert.ToString(ViewState["StateEdit"]) == "Add")
            {
                if (Convert.ToInt32(Session[gblValue.StateID].ToString()) != 3)
                {
                    if (ddlAddPrf.SelectedValue != "4" && ddlIdentyProf.SelectedValue != "4")
                    {
                        gblFuction.MsgPopup("Adhar Card is mandatory for your Address Proof or Identity Proof");
                        vRst = false;
                    }
                }
            }

            return vRst;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGrid()
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            dt = oMem.GetIniApprMember(gblFuction.setDate(txtDtFrm.Text), gblFuction.setDate(txtDtTo.Text), vBrCode, txtSearch.Text);
            if (dt.Rows.Count > 0)
            {
                gvMemApp.DataSource = dt;
                gvMemApp.DataBind();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMemApp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vEnqId = "";
            vEnqId = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                foreach (GridViewRow gr in gvMemApp.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
            }
            FillMemberDtl(vEnqId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vMemberId"></param>
        private void FillMemberDtl(string vEnqId)
        {
            ViewState["EnqId"] = "";
            DataTable dt = null;
            DataTable dt1 = null;
            CMember oMem = null;
            try
            {
                lblAadhar.Text = "";
                ViewState["EnqId"] = vEnqId;
                oMem = new CMember();
                dt = new DataTable();
                dt1 = new DataTable();
                dt = oMem.GetIniApprMemDtlByEnqId(vEnqId);
                dt1 = oMem.GetIniApprEarningMemDtlByEnqId(vEnqId);

                if (dt.Rows.Count > 0)
                {
                    ViewState["AadhaarScan"] = dt.Rows[0]["AadhaarScan"].ToString();
                    txtAmountApplied.Text = Convert.ToString(dt.Rows[0]["AmountApplied"]);
                    txtFName.Text = Convert.ToString(dt.Rows[0]["MF_Name"]);
                    txtMName.Text = Convert.ToString(dt.Rows[0]["MM_Name"]);
                    txtLName.Text = Convert.ToString(dt.Rows[0]["ML_Name"]);
                    txtHFName.Text = Convert.ToString(dt.Rows[0]["FamilyPersonName"]);
                    txtDOB.Text = Convert.ToString(dt.Rows[0]["MDOB"]);
                    txtAge.Text = Convert.ToString(dt.Rows[0]["Age"]);
                    txtHouNo.Text = Convert.ToString(dt.Rows[0]["HouseNo"]);
                    txtStName.Text = Convert.ToString(dt.Rows[0]["Street"]);
                    txtWardNo.Text = Convert.ToString(dt.Rows[0]["WardNo"]);
                    txtPOff.Text = Convert.ToString(dt.Rows[0]["PostOff"]);
                    txtLandMark.Text = Convert.ToString(dt.Rows[0]["Landmark"]);
                    txtArea.Text = Convert.ToString(dt.Rows[0]["Area"]);
                    txtPin.Text = Convert.ToString(dt.Rows[0]["PIN"]);
                    txtVillg.Text = Convert.ToString(dt.Rows[0]["Village"]);
                    txtMob.Text = Convert.ToString(dt.Rows[0]["MobileNo"]);
                    txtPhNo.Text = Convert.ToString(dt.Rows[0]["MobileNo_p"]);
                    txtCommHouseNo.Text = Convert.ToString(dt.Rows[0]["HouseNo_p"]);
                    txtCommSt.Text = Convert.ToString(dt.Rows[0]["Street_p"]);
                    txtCommSubDist.Text = Convert.ToString(dt.Rows[0]["WardNo_p"]);
                    txtCommPost.Text = Convert.ToString(dt.Rows[0]["PostOff_p"]);
                    txtCommPin.Text = Convert.ToString(dt.Rows[0]["PIN_p"]);
                    txtCommMob.Text = Convert.ToString(dt.Rows[0]["MobileNo_p"]);
                    txtCommLandmark.Text = Convert.ToString(dt.Rows[0]["Landmark_p"]);
                    txtCommArea.Text = Convert.ToString(dt.Rows[0]["Area_p"]);

                    //txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["AddProfNo"]);
                    //txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["IdentyProfNo"]);
                    if (Convert.ToInt32(Session[gblValue.RoleId]) != 1)
                    {
                        if (dt.Rows[0]["IdentyPRofId"].ToString() == "1")
                        {
                            lblAadhar.Text = Convert.ToString(dt.Rows[0]["IdentyProfNo"]);
                            txtIdntPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["IdentyProfNo"]).Substring(Convert.ToString(dt.Rows[0]["IdentyProfNo"]).Length - 4, 4));
                        }
                        else
                        {
                            txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["IdentyProfNo"]);
                        }
                        if (dt.Rows[0]["AddProfId"].ToString() == "1")
                        {
                            lblAadhar.Text = Convert.ToString(dt.Rows[0]["AddProfNo"]);
                            txtAddPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["AddProfNo"]).Substring(Convert.ToString(dt.Rows[0]["AddProfNo"]).Length - 4, 4));
                        }
                        else
                        {
                            txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["AddProfNo"]);
                        }
                        if (dt.Rows[0]["AddProfId2"].ToString() == "1")
                        {
                            lblAadhar.Text = Convert.ToString(dt.Rows[0]["AddProfId2"]);
                            txtIdProof3.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["AddProfNo2"]).Substring(Convert.ToString(dt.Rows[0]["AddProfNo2"]).Length - 4, 4));
                        }
                        else
                        {
                            txtIdProof3.Text = Convert.ToString(dt.Rows[0]["AddProfNo2"]);
                        }
                    }
                    else
                    {
                        if (dt.Rows[0]["IdentyPRofId"].ToString() == "1")
                        {
                            lblAadhar.Text = Convert.ToString(dt.Rows[0]["IdentyProfNo"]);
                        }
                        else if (dt.Rows[0]["AddProfId"].ToString() == "1")
                        {
                            lblAadhar.Text = Convert.ToString(dt.Rows[0]["AddProfNo"]);
                        }
                        else if (dt.Rows[0]["AddProfId2"].ToString() == "1")
                        {
                            lblAadhar.Text = Convert.ToString(dt.Rows[0]["AddProfId2"]);
                        }
                        txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["IdentyProfNo"]);
                        txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["AddProfNo"]);
                        txtIdProof3.Text = Convert.ToString(dt.Rows[0]["AddProfNo2"]);
                    }

                    txtDist.Text = Convert.ToString(dt.Rows[0]["District"]);
                    ddlAddPrf.SelectedIndex = ddlAddPrf.Items.IndexOf(ddlAddPrf.Items.FindByValue(dt.Rows[0]["AddProfId"].ToString()));
                    ddlIdentyProf.SelectedIndex = ddlIdentyProf.Items.IndexOf(ddlIdentyProf.Items.FindByValue(dt.Rows[0]["IdentyPRofId"].ToString()));
                    ddlIdProof3.SelectedIndex = ddlIdProof3.Items.IndexOf(ddlIdProof3.Items.FindByValue(dt.Rows[0]["AddProfId2"].ToString()));
                    ddlAddrType.SelectedIndex = ddlAddrType.Items.IndexOf(ddlAddrType.Items.FindByValue(dt.Rows[0]["AddrType"].ToString()));
                    ddlCommAddrType.SelectedIndex = ddlCommAddrType.Items.IndexOf(ddlCommAddrType.Items.FindByValue(dt.Rows[0]["AddrType_p"].ToString()));
                    ddlCommVill.SelectedIndex = ddlCommVill.Items.IndexOf(ddlCommVill.Items.FindByValue(dt.Rows[0]["VillageId_p"].ToString()));
                    ddlStat.SelectedIndex = ddlStat.Items.IndexOf(ddlStat.Items.FindByText(dt.Rows[0]["State"].ToString()));
                    popAgainstVillage2();

                    string vEO = Convert.ToString(dt.Rows[0]["EoId"]);
                    PopCenter(vEO);
                    ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["MarketId"].ToString()));
                    string vMarket = Convert.ToString(dt.Rows[0]["MarketId"]);
                    PopGroup(vMarket);
                    ddlGroup.SelectedIndex = ddlGroup.Items.IndexOf(ddlGroup.Items.FindByValue(dt.Rows[0]["GroupId"].ToString()));
                    ddlRelationType.SelectedIndex = ddlRelationType.Items.IndexOf(ddlRelationType.Items.FindByValue(dt.Rows[0]["HumanRelationId"].ToString()));
                    memberKYC(vEnqId);

                    if (dt.Rows[0]["AadhaarScan"].ToString() == "M")
                    {
                        if (dt.Rows[0]["IdentyPRofId"].ToString() == "1")
                        {
                            lblId1.Visible = true;
                            txtConfrmAadhar1.Visible = true;
                        }
                        else if (dt.Rows[0]["AddProfId"].ToString() == "1")
                        {
                            lblId2.Visible = true;
                            txtConfrmAadhar2.Visible = true;
                        }
                        else if (dt.Rows[0]["AddProfId2"].ToString() == "1")
                        {
                            lblId3.Visible = true;
                            txtConfrmAadhar3.Visible = true;
                        }
                    }

                    txtCoAppName.Text = Convert.ToString(dt.Rows[0]["CoAppName"]);
                    txtCoAppFName.Text = Convert.ToString(dt.Rows[0]["CoAppFName"]);
                    txtCoAppMName.Text = Convert.ToString(dt.Rows[0]["CoAppMName"]);
                    txtCoAppLName.Text = Convert.ToString(dt.Rows[0]["CoAppLName"]);
                    txtCoAppDOB.Text = Convert.ToString(dt.Rows[0]["CoApplicantDOB"]);
                    if (Convert.ToString(dt.Rows[0]["CoApplicantAge"]) == "" || Convert.ToString(dt.Rows[0]["CoApplicantAge"]) == "0")
                    {
                        txtCoAppAge.Text = "0";
                        if (Convert.ToString(dt.Rows[0]["CoApplicantDOB"]) != "")
                        {
                            //txtCoAppAge.Text = Convert.ToString(CalAge(Convert.ToString(dt.Rows[0]["CoApplicantDOB"])));
                            txtCoAppAge.Text = Convert.ToString(AgeCount(Convert.ToString(dt.Rows[0]["CoApplicantDOB"]), Convert.ToString(Session[gblValue.LoginDate])));
                        }
                    }
                    else
                    {
                        txtCoAppAge.Text = Convert.ToString(dt.Rows[0]["CoApplicantAge"]);
                    }
                    txtCoAppMobile.Text = Convert.ToString(dt.Rows[0]["CoAppMobileNo"]);
                    txtCoAppAddr.Text = Convert.ToString(dt.Rows[0]["CoAppAddress"]);
                    txtCoAppPin.Text = Convert.ToString(dt.Rows[0]["CoAppPinCode"]);
                    txtCoAppID.Text = Convert.ToString(dt.Rows[0]["CoAppIdentyProfNo"]);
                    ddlCoAppRel.SelectedIndex = ddlCoAppRel.Items.IndexOf(ddlCoAppRel.Items.FindByValue(dt.Rows[0]["CoAppRelationId"].ToString()));
                    ddlCoAppID.SelectedIndex = ddlCoAppID.Items.IndexOf(ddlCoAppID.Items.FindByValue(dt.Rows[0]["CoAppIdentyProfId"].ToString()));
                    ddlCoAppState.SelectedIndex = ddlCoAppState.Items.IndexOf(ddlCoAppState.Items.FindByText(dt.Rows[0]["CoAppState"].ToString()));

                    ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByValue(dt.Rows[0]["Gender"].ToString()));
                    ddlCoAppGender.SelectedIndex = ddlCoAppGender.Items.IndexOf(ddlCoAppGender.Items.FindByValue(dt.Rows[0]["CoAppGender"].ToString()));
                    ddlBusType.SelectedIndex = ddlBusType.Items.IndexOf(ddlBusType.Items.FindByValue(dt.Rows[0]["BusinessTypeId"].ToString()));
                    ddlOccupation.SelectedIndex = ddlOccupation.Items.IndexOf(ddlOccupation.Items.FindByValue(dt.Rows[0]["OccupationId"].ToString()));
                    txtDeclIncome.Text = Convert.ToString(dt.Rows[0]["DeclaredIncome"]);
                    ddlIncFrequency.SelectedIndex = ddlIncFrequency.Items.IndexOf(ddlIncFrequency.Items.FindByValue(dt.Rows[0]["IncFrequency"].ToString()));
                    ddlCoAppBusType.SelectedIndex = ddlCoAppBusType.Items.IndexOf(ddlCoAppBusType.Items.FindByValue(dt.Rows[0]["CoAppBusinessTypeId"].ToString()));
                    ddlCoAppOccupation.SelectedIndex = ddlCoAppOccupation.Items.IndexOf(ddlCoAppOccupation.Items.FindByValue(dt.Rows[0]["CoAppOccupationId"].ToString()));
                    txtCoAppDeclIncome.Text = Convert.ToString(dt.Rows[0]["CoAppDeclaredIncome"]);
                    ddlCoAppIncFrequency.SelectedIndex = ddlCoAppIncFrequency.Items.IndexOf(ddlCoAppIncFrequency.Items.FindByValue(dt.Rows[0]["CoAppIncFrequency"].ToString()));

                    txtFamilyInc.Text = Convert.ToString(dt.Rows[0]["FamilyIncome"]);
                    txtSelfInc.Text = Convert.ToString(dt.Rows[0]["MemIncome"]);
                    txtTotInc.Text = Convert.ToString(dt.Rows[0]["TotFamilyIncome"]);

                    popBusinessActv(Convert.ToInt32(ddlBusType.SelectedValue));
                    popCoAppBusinessActv(Convert.ToInt32(ddlCoAppBusType.SelectedValue));
                    ddlBusActivity.SelectedIndex = ddlBusActivity.Items.IndexOf(ddlBusActivity.Items.FindByValue(dt.Rows[0]["BusinessActvId"].ToString()));
                    ddlCoAppBusActivity.SelectedIndex = ddlCoAppBusActivity.Items.IndexOf(ddlCoAppBusActivity.Items.FindByValue(dt.Rows[0]["CoAppBusinessActvId"].ToString()));

                    if (dt1.Rows.Count > 0)
                    {
                        ViewState["EarningMember"] = dt1;
                        gvEarningMember.DataSource = dt1;
                        gvEarningMember.DataBind();
                    }
                    else
                    {
                        GenerateEarningMember();
                        //ViewState["EarningMember"] = dt1;
                        //gvEarningMember_Row_Initialize();                        
                    }

                    tbEmp.ActiveTabIndex = 1;
                    if (Session[gblValue.BrnchCode].ToString() == "0000")
                    {
                        StatusButton("View");
                    }
                    else
                    {
                        StatusButton("Show");
                    }

                    EnableControl(false);
                    gvEarningMember.Enabled = false;
                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                oMem = null;
            }
        }

        private void memberKYC(string InitialApproachId)
        {
            pathImage = ConfigurationManager.AppSettings["PathImage"];
            string imgFolder = InitialApproachId;
            string vUrl = pathImage + "InitialApproach/";
            imgMemPhoto.ImageUrl = vUrl + imgFolder + "/MemberPhoto.png";
            imgMemIdProof.ImageUrl = vUrl + imgFolder + "/IDProofImage.png";
            imgMemIdProofBack.ImageUrl = vUrl + imgFolder + "/IDProofImageBack.png";
            imgMemAddrProof.ImageUrl = vUrl + imgFolder + "/AddressProofImage.png";
            imgMemAddrProofBack.ImageUrl = vUrl + imgFolder + "/AddressProofImageBack.png";
            imgMemAddrProof2.ImageUrl = vUrl + imgFolder + "/AddressProofImage2.png";
            imgMemAddrProofBack2.ImageUrl = vUrl + imgFolder + "/AddressProofImage2Back.png";
            //imgCoAppPhoto.ImageUrl = vUrl + imgFolder + "/CoAppPhoto.png";
            //imgCoAppIdProof1Front.ImageUrl = vUrl + imgFolder + "/CoAppIDProofImage.png";
            //imgCoAppIdProof1Back.ImageUrl = vUrl + imgFolder + "/CoAppIDProofImageBack.png";      
            imgSelfie.ImageUrl = vUrl + imgFolder + "/FrontSelfeImage.png";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void ChkMemDayEnd()
        {
            string vMemberId = ViewState["MemId"].ToString();
            CMember oMem = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            int vRet = 0;

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oMem = new CMember();
                vRet = oMem.ChkMemDayEnd(vMemberId, vBrCode);
                if (vRet > 0)
                {
                    gblFuction.MsgPopup("Day End is already done.You can not edit the member records");

                }
                else
                {
                    gblFuction.MsgPopup("");
                }

            }
            finally
            {
                oMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popCltype()
        {
            ListItem[] items = new ListItem[3];
            items[0] = new ListItem("<--Select-->", "-1");
            items[1] = new ListItem("Normal", "N");
            items[2] = new ListItem("Unsatisfied", "U");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            CMember oMem = new CMember();
            int vErr = 0;
            Boolean vResult = false;
            path = ConfigurationManager.AppSettings["PathInitialApproach"];
            string vEnqId = Convert.ToString(ViewState["EnqId"]);
            string vBrCode = Convert.ToString(Session[gblValue.BrnchCode]);
            string vCoAppName = "", vCoAppFName = "", vCoAppMName = "", vCoAppLName = "",
                vCoAppMobileNo = "", vCoAppIdentyProfNo = "", vCoAppAddress = "", vCoAppState = "", vCoAppPinCode = "";
            DateTime vCoAppDOB;
            Int32 vCoAppRelationId, vCoAppIdentyProfId;
            string vXmlEarningMemDtl;
            Int32 vBusTypeId = 0, vOccupationId = 0, vIncFrequency = 0;
            Int32 vCoAppBusTypeId = 0, vCoAppOccupationId = 0, vCoAppIncFrequency = 0;
            double vDeclIncome = 0, vCoAppDeclIncome = 0;
            string vGender, vCoAppGender;
            Int32 pCoAppAge = 0;
            Int32 vBusActvId = 0, vCoAppBusActvId = 0;

            vCoAppFName = txtCoAppFName.Text.Trim().Replace("'", "").ToUpper();
            vCoAppMName = txtCoAppMName.Text.Trim().Replace("'", "").ToUpper();
            vCoAppLName = txtCoAppLName.Text.Trim().Replace("'", "").ToUpper();
            vCoAppName = vCoAppMName.Length > 0 ? vCoAppFName + " " + vCoAppMName : vCoAppFName;
            vCoAppName = vCoAppLName.Length > 0 ? vCoAppName + " " + vCoAppLName : vCoAppName;
            vCoAppDOB = gblFuction.setDate(txtCoAppDOB.Text);
            vCoAppRelationId = Convert.ToInt32(ddlCoAppRel.SelectedValue);
            vCoAppMobileNo = txtCoAppMobile.Text.Trim();
            vCoAppIdentyProfId = Convert.ToInt32(ddlCoAppID.SelectedValue);
            vCoAppIdentyProfNo = txtCoAppID.Text.Trim();
            vCoAppAddress = txtCoAppAddr.Text.Trim().Replace("'", "");
            vCoAppState = ddlCoAppState.SelectedItem.Text;
            vCoAppPinCode = txtCoAppPin.Text.Trim();
            //vXmlEarningMemDtl = EarningMemDtlDtToXml();
            vXmlEarningMemDtl = EarningMemDtlDtToXmlFinal();

            vGender = Convert.ToString(ddlGender.SelectedValue);
            vCoAppGender = Convert.ToString(ddlCoAppGender.SelectedValue);
            vBusTypeId = Convert.ToInt32(ddlBusType.SelectedValue);
            vOccupationId = Convert.ToInt32(ddlOccupation.SelectedValue);
            if (txtDeclIncome.Text.Trim() == "") txtDeclIncome.Text = "0";
            if (txtDeclIncome.Text.Trim() == "")
            {
                vDeclIncome = Convert.ToDouble("0");
            }
            else
            {
                vDeclIncome = Convert.ToDouble(txtDeclIncome.Text.Trim());
            }
            vIncFrequency = Convert.ToInt32(ddlIncFrequency.SelectedValue);
            vCoAppBusTypeId = Convert.ToInt32(ddlCoAppBusType.SelectedValue);
            vCoAppOccupationId = Convert.ToInt32(ddlCoAppOccupation.SelectedValue);
            if (txtCoAppDeclIncome.Text == "") txtCoAppDeclIncome.Text = "0";
            if (txtCoAppDeclIncome.Text.Trim() == "")
            {
                vCoAppDeclIncome = Convert.ToDouble("0");
            }
            else
            {
                vCoAppDeclIncome = Convert.ToDouble(txtCoAppDeclIncome.Text.Trim());
            }
            vCoAppIncFrequency = Convert.ToInt32(ddlCoAppIncFrequency.SelectedValue);
            if (txtCoAppAge.Text.Trim() == "") txtCoAppAge.Text = "0";
            if (txtCoAppAge.Text.Trim() == "")
            {
                pCoAppAge = Convert.ToInt32("0");
            }
            else
            {
                pCoAppAge = Convert.ToInt32(txtCoAppAge.Text.Trim());
            }
            vBusActvId = Convert.ToInt32(ddlBusActivity.SelectedValue);
            vCoAppBusActvId = Convert.ToInt32(ddlCoAppBusActivity.SelectedValue);

            vErr = oMem.UpdateInitialApproach(vEnqId, ddlGroup.SelectedValue, Convert.ToDouble(txtAmountApplied.Text), txtFName.Text, txtMName.Text, txtLName.Text, gblFuction.setDate(txtDOB.Text),
                Convert.ToInt16(txtAge.Text), txtHFName.Text, Convert.ToInt16(ddlRelationType.SelectedValue), Convert.ToInt16(ddlIdentyProf.SelectedValue),
                ddlIdentyProf.SelectedValue == "1" ? txtConfrmAadhar1.Text : txtIdntPrfNo.Text, Convert.ToInt16(ddlAddPrf.SelectedValue),
                ddlAddPrf.SelectedValue == "1" ? txtConfrmAadhar2.Text : txtAddPrfNo.Text, Convert.ToInt32(ddlIdProof3.SelectedValue),
                ddlIdProof3.SelectedValue == "1" ? txtConfrmAadhar3.Text : txtIdProof3.Text, Convert.ToInt16(ddlAddrType.SelectedValue),
                txtHouNo.Text, txtStName.Text, txtArea.Text, txtVillg.Text, txtWardNo.Text, txtDist.Text, ddlStat.SelectedItem.Text, txtLandMark.Text, txtPOff.Text,
                txtPin.Text, txtMob.Text, txtEmail.Text, Convert.ToInt16(ddlCommAddrType.SelectedValue), txtCommHouseNo.Text, txtCommSt.Text, txtCommArea.Text, Convert.ToInt32(ddlCommVill.SelectedValue),
                txtCommSubDist.Text, txtCommLandmark.Text, txtCommPost.Text, txtCommPin.Text, txtCommPin.Text, "", vBrCode, gblFuction.setDate(Session[gblValue.LoginDate].ToString()),
                Convert.ToInt16(Session[gblValue.UserId]), "", ""
                , vCoAppName, vCoAppFName, vCoAppMName, vCoAppLName
                , vCoAppDOB, vCoAppRelationId, vCoAppMobileNo, vCoAppIdentyProfId, vCoAppIdentyProfNo, vCoAppAddress, vCoAppState, vCoAppPinCode
                , vXmlEarningMemDtl, vGender, vCoAppGender, vBusTypeId, vOccupationId, vDeclIncome, vIncFrequency
                , vCoAppBusTypeId, vCoAppOccupationId, vCoAppDeclIncome, vCoAppIncFrequency, pCoAppAge
                , vBusActvId, vCoAppBusActvId);

            if (vErr == 0)
            {
                try
                {
                    if (fuMemPhoto.HasFile)
                    {
                        string vMessage = SaveMemberImages(fuMemPhoto, vEnqId, "MemberPhoto", "Edit", "N", path);
                    }
                    if (fuIdProof1Front.HasFile)
                    {
                        string vMessage = SaveMemberImages(fuIdProof1Front, vEnqId, "IDProofImage", "Edit", "N", path);
                    }
                    if (fuIdProof1Back.HasFile)
                    {
                        string vMessage = SaveMemberImages(fuIdProof1Back, vEnqId, "IDProofImageBack", "Edit", "N", path);
                    }
                    if (fuIdProof2Front.HasFile)
                    {
                        string vMessage = SaveMemberImages(fuIdProof2Front, vEnqId, "AddressProofImage", "Edit", "N", path);
                    }
                    if (fuIdProof2Back.HasFile)
                    {
                        string vMessage = SaveMemberImages(fuIdProof2Back, vEnqId, "AddressProofImageBack", "Edit", "N", path);
                    }
                    if (fuIdProof3Front.HasFile)//Co-Applicant Id Proof Image Front
                    {
                        string vMessage = SaveMemberImages(fuIdProof3Front, vEnqId, "AddressProofImage2", "Edit", "N", path);
                    }
                    if (fuIdProof3Back.HasFile)//Co-Applicant Id Proof Image Back
                    {
                        string vMessage = SaveMemberImages(fuIdProof3Back, vEnqId, "AddressProofImage2Back", "Edit", "N", path);
                    }
                    //if (fuCoAppPhoto.HasFile)
                    //{
                    //    string vMessage = SaveMemberImages(fuCoAppPhoto, vEnqId, "CoAppPhoto", "Edit", "N", path);
                    //}
                    //if (fuCoAppIdProof1Front.HasFile)
                    //{
                    //    
                    //    string vMessage = SaveMemberImages(fuCoAppIdProof1Front, vEnqId, "CoAppIDProofImage", "Edit", "N", path);
                    //}
                    //if (fuCoAppIdProof1Back.HasFile)
                    //{
                    //    
                    //    string vMessage = SaveMemberImages(fuCoAppIdProof1Back, vEnqId, "CoAppIDProofImageBack", "Edit", "N", path);
                    //}
                    if (fuSelfie.HasFile)
                    {
                        string vMessage = SaveMemberImages(fuSelfie, vEnqId, "FrontSelfeImage", "Edit", "N", path);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                vResult = true;
            }
            else if (vErr == 5)
            {
                gblFuction.MsgPopup("KYC Id can not be duplicate");
            }
            return vResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PrevDt"></param>
        /// <param name="NxtDt"></param>
        /// <returns></returns>
        protected Boolean Datechk(string PrevDt, string NxtDt)
        {
            if (gblFuction.setDate(PrevDt) < gblFuction.setDate(NxtDt))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pVillageId"></param>
        private void PopMVillDtl(Int32 pVillageId)
        {
            DataTable dt = null;
            CGroup oGb = null;
            try
            {
                if (pVillageId > 0)
                {
                    oGb = new CGroup();
                    dt = oGb.GetVillageDtls(pVillageId);
                    ddlStat.DataSource = dt;
                    ddlStat.DataTextField = "StateName";
                    ddlStat.DataValueField = "StateID";
                    ddlStat.DataBind();
                    ddlBlk.DataSource = dt;
                    ddlBlk.DataTextField = "BlockName";
                    ddlBlk.DataValueField = "BlockId";
                    ddlBlk.DataBind();
                    ddlMuPanc.DataSource = dt;
                    ddlMuPanc.DataTextField = "GPName";
                    ddlMuPanc.DataValueField = "GPId";
                    ddlMuPanc.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlStat.Items.Insert(1, oli);
                    //ddlDist.Items.Insert(1, oli);
                    ddlBlk.Items.Insert(1, oli);
                    ddlMuPanc.Items.Insert(1, oli);
                }
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>

        private void EnableControl(Boolean Status)
        {
            txtFName.Enabled = Status;
            txtMName.Enabled = Status;
            txtLName.Enabled = Status;
            txtHFName.Enabled = Status;
            txtDOB.Enabled = Status;
            txtAge.Enabled = false;
            txtHouNo.Enabled = Status;
            txtStName.Enabled = Status;
            txtWardNo.Enabled = Status;
            txtPOff.Enabled = Status;
            txtPin.Enabled = Status;
            txtMob.Enabled = Status;
            txtPhNo.Enabled = Status;
            txtPin.Enabled = Status;
            ddlAddPrf.Enabled = Status;
            ddlIdentyProf.Enabled = Status;
            ddlAreaCategory.Enabled = Status;
            txtIdntPrfNo.Enabled = Status;
            txtAddPrfNo.Enabled = Status;

            ddlIdProof3.Enabled = Status;
            txtIdProof3.Enabled = Status;
            ddlAddrType.Enabled = Status;
            txtArea.Enabled = Status;
            txtLandMark.Enabled = Status;
            txtEmail.Enabled = Status;

            ddlCommAddrType.Enabled = Status;
            txtCommHouseNo.Enabled = Status;
            txtCommLandmark.Enabled = Status;
            txtCommSt.Enabled = Status;
            txtCommArea.Enabled = Status;
            ddlCommVill.Enabled = Status;
            txtCommSubDist.Enabled = Status;
            txtCommPost.Enabled = Status;
            txtCommMob.Enabled = Status;
            txtCommPin.Enabled = Status;
            txtCommPhone.Enabled = Status;
            ddlCommDist.Enabled = Status;
            ddlCommState.Enabled = Status;

            chkCommAddr.Enabled = Status;
            txtVillg.Enabled = Status;
            txtDist.Enabled = Status;
            ddlStat.Enabled = Status;

            ddlGroup.Enabled = Status;
            txtAmountApplied.Enabled = Status;
            ddlRelationType.Enabled = Status;
            ddlCenter.Enabled = false;

            txtConfrmAadhar1.Enabled = Status;
            txtConfrmAadhar2.Enabled = Status;
            txtConfrmAadhar3.Enabled = Status;

            txtCoAppName.Enabled = Status;
            txtCoAppFName.Enabled = Status;
            txtCoAppMName.Enabled = Status;
            txtCoAppLName.Enabled = Status;
            txtCoAppDOB.Enabled = Status;
            txtCoAppAge.Enabled = false;
            txtCoAppMobile.Enabled = Status;
            txtCoAppAddr.Enabled = Status;
            txtCoAppPin.Enabled = Status;
            ddlCoAppRel.Enabled = Status;
            ddlCoAppID.Enabled = Status;
            txtCoAppID.Enabled = Status;
            ddlCoAppState.Enabled = Status;
            gvEarningMember.Enabled = Status;

            ddlGender.Enabled = Status;
            ddlBusType.Enabled = Status;
            txtDeclIncome.Enabled = Status;
            ddlOccupation.Enabled = Status;
            ddlIncFrequency.Enabled = Status;
            ddlCoAppGender.Enabled = Status;
            ddlCoAppBusType.Enabled = Status;
            ddlCoAppOccupation.Enabled = Status;
            txtCoAppDeclIncome.Enabled = Status;
            ddlCoAppIncFrequency.Enabled = Status;

            txtFamilyInc.Enabled = false;
            txtSelfInc.Enabled = false;
            txtTotInc.Enabled = false;

            ddlBusActivity.Enabled = Status;
            ddlCoAppBusActivity.Enabled = Status;

            Boolean vStatus = Status;
            vStatus = Convert.ToInt32(Session[gblValue.RoleId]) == 1 ? Status : false;

            ddlIdentyProf.Enabled = (ddlIdentyProf.SelectedValue == "13") ? false : Status;
            ddlAddPrf.Enabled = (ddlAddPrf.SelectedValue == "13") ? false : Status;
            txtIdntPrfNo.Enabled = (ddlIdentyProf.SelectedValue == "13") ? false : Status;
            txtAddPrfNo.Enabled = (ddlAddPrf.SelectedValue == "13") ? false : Status;
            ddlCoAppID.Enabled = (ddlCoAppID.SelectedValue == "13") ? false : Status;
            txtCoAppID.Enabled = (ddlCoAppID.SelectedValue == "13") ? false : Status;

            vStatus = Convert.ToInt32(Session[gblValue.RoleId]) == 1 || Convert.ToInt32(Session[gblValue.RoleId]) == 25 ? Status : false;

            txtFName.Enabled = vStatus;
            txtMName.Enabled = vStatus;
            txtLName.Enabled = vStatus;
            //txtBFName.Enabled = vStatus;
            //txtBMName.Enabled = vStatus;
            //txtBLName.Enabled = vStatus;
            //txtGuarLName.Enabled = vStatus;
            //txtGuarName.Enabled = vStatus;


        }
        /// <summary>
        /// 
        /// </summary>

        private void ClearControls()
        {
            txtFName.Text = "";
            txtMName.Text = "";
            txtLName.Text = "";
            txtHFName.Text = "";
            txtAmountApplied.Text = "";
            txtDOB.Text = "";
            txtAge.Text = "0";
            txtHouNo.Text = "";
            txtStName.Text = "";
            txtWardNo.Text = "";
            txtPOff.Text = "";
            txtPin.Text = "";
            txtMob.Text = "";
            txtPhNo.Text = "";
            txtPin.Text = "";

            txtAddPrfNo.Text = "";
            txtIdntPrfNo.Text = "";

            ddlMuPanc.Items.Clear();
            ddlMuPanc.SelectedIndex = -1;
            ddlBlk.Items.Clear();
            ddlBlk.SelectedIndex = -1;
            ddlStat.Items.Clear();
            ddlStat.SelectedIndex = -1;
            ddlAddPrf.SelectedIndex = -1;
            ddlIdentyProf.SelectedIndex = -1;
            ddlGroup.Items.Clear();
            ddlGroup.SelectedIndex = -1;

            ddlCenter.Items.Clear();
            ddlCenter.SelectedIndex = -1;

            lblDate.Text = "";
            lblUser.Text = "";

            ddlIdProof3.SelectedIndex = -1;
            txtIdProof3.Text = "";
            ddlAddrType.SelectedIndex = -1;
            txtLandMark.Text = "";
            txtArea.Text = "";
            txtEmail.Text = "";
            chkCommAddr.Checked = false;
            ddlCommAddrType.SelectedIndex = -1;
            txtCommLandmark.Text = "";
            txtCommHouseNo.Text = "";
            txtCommSt.Text = "";
            txtCommArea.Text = "";
            ddlCommVill.SelectedIndex = -1;
            txtCommSubDist.Text = "";
            txtCommPost.Text = "";
            txtCommPin.Text = "";
            txtCommMob.Text = "";
            txtCommPhone.Text = "";
            ddlCommDist.SelectedIndex = -1;
            ddlCommState.SelectedIndex = -1;
            ddlAreaCategory.SelectedIndex = 0;
            ddlRelationType.SelectedIndex = -1;
            txtConfrmAadhar1.Text = "";
            txtConfrmAadhar2.Text = "";
            txtConfrmAadhar3.Text = "";
            txtConfrmAadhar1.Visible = false;
            txtConfrmAadhar2.Visible = false;
            txtConfrmAadhar3.Visible = false;
            lblId1.Visible = false;
            lblId2.Visible = false;
            lblId3.Visible = false;
            lblAadhar.Text = "";
            ///
            clearMemPhoto();

            txtCoAppName.Text = "";
            txtCoAppFName.Text = "";
            txtCoAppMName.Text = "";
            txtCoAppLName.Text = "";
            txtCoAppDOB.Text = "";
            txtCoAppAge.Text = "0";
            txtCoAppMobile.Text = "";
            txtCoAppAddr.Text = "";
            txtCoAppPin.Text = "";
            txtCoAppID.Text = "";
            ddlCoAppState.SelectedIndex = -1;
            ddlCoAppRel.SelectedIndex = -1;
            ddlCoAppID.SelectedIndex = -1;

            ddlGender.SelectedIndex = -1;
            ddlBusType.SelectedIndex = -1;
            txtDeclIncome.Text = "0";
            ddlOccupation.SelectedIndex = -1;
            ddlIncFrequency.SelectedIndex = -1;
            ddlCoAppGender.SelectedIndex = -1;
            ddlCoAppBusType.SelectedIndex = -1;
            ddlCoAppOccupation.SelectedIndex = -1;
            txtCoAppDeclIncome.Text = "0";
            ddlCoAppIncFrequency.SelectedIndex = -1;

            txtFamilyInc.Text = "0.00";
            txtSelfInc.Text = "0.00";
            txtTotInc.Text = "0.00";

            ddlBusActivity.SelectedIndex = -1;
            ddlCoAppBusActivity.SelectedIndex = -1;
        }

        private void clearMemPhoto()
        {
            string imgUrl = "~/Images/no-image-icon.jpg";
            imgMemPhoto.ImageUrl = imgUrl;
            imgMemIdProof.ImageUrl = imgUrl;
            imgMemIdProofBack.ImageUrl = imgUrl;
            imgMemAddrProof.ImageUrl = imgUrl;
            imgMemAddrProofBack.ImageUrl = imgUrl;
            imgMemAddrProof2.ImageUrl = imgUrl;
            imgMemAddrProofBack2.ImageUrl = imgUrl;
            imgCoAppPhoto.ImageUrl = imgUrl;
            imgCoAppIdProof1Front.ImageUrl = imgUrl;
            imgCoAppIdProof1Back.ImageUrl = imgUrl;
            imgSelfie.ImageUrl = imgUrl;
        }

        public static int CalAge(string dobString)
        {
            var dob = DateTime.ParseExact(dobString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var current = DateTime.Now;
            var yearDiff = current.Year - dob.Year;
            if (dob.Month > current.Month)
            {
                yearDiff--;
            }
            if (dob.Month == current.Month && dob.Day > current.Day)
            {
                yearDiff--;
            }
            return yearDiff;
        }

        public static int AgeCount(string dobString, string vToday)
        {
            DateTime Bday = gblFuction.setDate(dobString);
            DateTime Cday = gblFuction.setDate(vToday);
            int years = 0, months = 0, days = 0;

            if ((Cday.Year - Bday.Year) > 0 ||
                (((Cday.Year - Bday.Year) == 0) && ((Bday.Month < Cday.Month) ||
                  ((Bday.Month == Cday.Month) && (Bday.Day <= Cday.Day)))))
            {
                int DaysInBdayMonth = DateTime.DaysInMonth(Bday.Year, Bday.Month);
                int DaysRemain = Cday.Day + (DaysInBdayMonth - Bday.Day);

                if (Cday.Month > Bday.Month)
                {
                    years = Cday.Year - Bday.Year;
                    months = Cday.Month - (Bday.Month + 1) + Math.Abs(DaysRemain / DaysInBdayMonth);
                    days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                }
                else if (Cday.Month == Bday.Month)
                {
                    if (Cday.Day >= Bday.Day)
                    {
                        years = Cday.Year - Bday.Year;
                        months = 0;
                        days = Cday.Day - Bday.Day;
                    }
                    else
                    {
                        years = (Cday.Year - 1) - Bday.Year;
                        months = 11;
                        days = DateTime.DaysInMonth(Bday.Year, Bday.Month) - (Bday.Day - Cday.Day);
                    }
                }
                else
                {
                    years = (Cday.Year - 1) - Bday.Year;
                    months = Cday.Month + (11 - Bday.Month) + Math.Abs(DaysRemain / DaysInBdayMonth);
                    days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                }
            }
            else
            {
                years = 0;
            }
            if (years >= 59 && days > 0 && (months == 0 || months > 0))
                years = years + 1;
            return years;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtDOB_TextChanged(object sender, EventArgs e)
        {
            // int Years = CalAge(txtDOB.Text);
            int Years = AgeCount(txtDOB.Text, Convert.ToString(Session[gblValue.LoginDate]));
            if (Years < 18)
            {
                gblFuction.AjxMsgPopup("Member age should Greater than 18 Years.");
                txtAge.Text = "0";
                txtDOB.Text = "";
            }
            else if (Years > 59)
            {
                gblFuction.AjxMsgPopup("Member age should less than 59 Years.");
                txtAge.Text = "0";
                txtDOB.Text = "";
            }
            else
            {
                if (txtDOB.Text.Length >= 10)
                    txtAge.Text = Years.ToString();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtAge_TextChanged(object sender, EventArgs e)
        {
            if (txtAge.Text != "")
            {
                Int32 vAge = Convert.ToInt32(txtAge.Text);
                DateTime vCurrDate = System.DateTime.Now.AddYears(-vAge);
                txtDOB.Text = gblFuction.putStrDate(vCurrDate);
                //txtAge.Focus();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkMProf_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMProf.Checked == true)
            {
                if (ddlIdentyProf.SelectedIndex > 0)
                {
                    if (Convert.ToInt32(ddlIdentyProf.SelectedValue) <= 5)
                    {
                        ddlAddPrf.SelectedIndex = ddlIdentyProf.SelectedIndex;
                        txtAddPrfNo.Text = txtIdntPrfNo.Text;
                    }
                }
            }
            else
            {
                ddlAddPrf.SelectedIndex = -1;
                txtAddPrfNo.Text = "";
            }
        }

        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAddPrf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAddPrf.SelectedValue == "1")
            {
                if (ddlIdentyProf.SelectedValue == "1")
                {
                    ddlAddPrf.SelectedIndex = -1;
                }
                else if (ddlIdProof3.SelectedValue == "1")
                {
                    ddlAddPrf.SelectedIndex = -1;
                }
                else
                {
                    lblId2.Visible = true;
                    txtConfrmAadhar2.Visible = true;
                    txtAddPrfNo.Text = "";
                    if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                    {
                        txtAddPrfNo.Attributes["type"] = "password";
                    }
                }
            }
            else
            {
                txtConfrmAadhar2.Attributes.Add("value", "");
                lblId2.Visible = false;
                txtConfrmAadhar2.Visible = false;
                txtAddPrfNo.Attributes["type"] = "Text";
                txtAddPrfNo.Text = "";
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlIdentyProf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlIdentyProf.SelectedValue == "1")
            {
                if (ddlAddPrf.SelectedValue == "1")
                {
                    ddlIdentyProf.SelectedIndex = -1;
                }
                else if (ddlIdProof3.SelectedValue == "1")
                {
                    ddlIdentyProf.SelectedIndex = -1;
                }
                else
                {
                    lblId1.Visible = true;
                    txtConfrmAadhar1.Visible = true;
                    txtIdntPrfNo.Text = "";
                    if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                    {
                        txtIdntPrfNo.Attributes["type"] = "password";
                    }
                }
            }
            else
            {
                txtConfrmAadhar1.Attributes.Add("value", "");
                lblId1.Visible = false;
                txtConfrmAadhar1.Visible = false;
                txtIdntPrfNo.Attributes["type"] = "Text";
                txtIdntPrfNo.Text = "";
            }

        }

        /// <summary>

        protected void ddlIdProof3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlIdProof3.SelectedValue == "1")
            {
                if (ddlIdentyProf.SelectedValue == "1")
                {
                    ddlIdProof3.SelectedIndex = -1;
                }
                else if (ddlAddPrf.SelectedValue == "1")
                {
                    ddlIdProof3.SelectedIndex = -1;
                }
                else
                {
                    lblId3.Visible = true;
                    txtConfrmAadhar3.Visible = true;
                    txtIdProof3.Text = "";
                    if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                    {
                        txtIdProof3.Attributes["type"] = "password";
                    }
                }
            }
            else
            {
                txtConfrmAadhar3.Attributes.Add("value", "");
                lblId3.Visible = false;
                txtConfrmAadhar3.Visible = false;
                txtIdProof3.Attributes["type"] = "Text";
                txtIdProof3.Text = "";
            }
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            gvMemApp.DataSource = null;
            gvMemApp.DataBind();
        }

        public void GetData()
        {

        }

        public void GetData1()
        {

        }

        private string SaveMemberImages(FileUpload flup, string imageGroup, string imageName, string Mode, string isImageSaved, string ImagePath)
        {
            try
            {
                string folderPath = string.Format("{0}/{1}", ImagePath, imageGroup/*, folderName*/);
                System.IO.Directory.CreateDirectory(folderPath);
                string filePath = string.Format("{0}/{1}.png", folderPath, imageName);
                Stream strm = flup.PostedFile.InputStream;
                var targetFile = filePath;

                if ((Mode == "Delete"))
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    isImageSaved = "N";
                }
                else
                {
                    if (flup.HasFile)
                    {
                        if (Mode == "Edit")
                        {
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                        }
                        File.WriteAllBytes(filePath, Convert.FromBase64String(getBase64String(flup)));
                        //ReduceImageSize(0.5, strm, targetFile); 
                        isImageSaved = "Y";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isImageSaved;
        }

        private string getBase64String(FileUpload flup)
        {
            string base64String = "";
            try
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(flup.PostedFile.InputStream))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return base64String;
        }

        private void ReduceImageSize(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vMarketId"></param>
        private void PopGroup(string vMarketId)
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vAdmDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CCenter oCent = null;
            try
            {
                oCent = new CCenter();
                dt = oCent.GetMeetingDayByCenterId(vMarketId, vBrCode, vAdmDt);
                if (dt.Rows.Count > 0)
                {
                    ddlGroup.DataSource = dt;
                    ddlGroup.DataTextField = "GroupName";
                    ddlGroup.DataValueField = "Groupid";
                    ddlGroup.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlGroup.Items.Insert(0, oli);
                }
            }
            finally
            {
                dt = null;
                oCent = null;
            }
        }

        private void popRelation()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "HumanRelationId", "HumanRelationName", "HumanRelationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlCoAppRel.DataSource = dt;
                ddlCoAppRel.DataTextField = "HumanRelationName";
                ddlCoAppRel.DataValueField = "HumanRelationId";
                ddlCoAppRel.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCoAppRel.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void GenerateEarningMember()
        {
            DataTable dt = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                dt = oMem.GenerateEarningMember();
                DataRow dF;
                dF = dt.NewRow();
                dt.Rows.Add(dF);
                dt.AcceptChanges();
                ViewState["EarningMember"] = dt;
                gvEarningMember.DataSource = dt;
                gvEarningMember.DataBind();
            }
            finally
            {
            }
        }
        protected void gvEarningMember_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDel2")
            {
                DataTable dt = null;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                dt = (DataTable)ViewState["EarningMember"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    ViewState["EarningMember"] = dt;
                    gvEarningMember.DataSource = dt;
                    gvEarningMember.DataBind();
                }
                else
                {
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
        }
        protected void gvEarningMember_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataSet ds = null;
            DataTable dt, dt1, dt2 = null;
            DataTable dt3, dt4, dt5 = null;
            CMember oMem = null;
            CBusinessType oGb = null;
            CGblIdGenerator oGb1 = null;
            CNewMember oNM = null;
            ListItem oL1 = new ListItem("<-- Select -->", "-1");
            ListItem oL2 = new ListItem("<-- Select -->", "-1");
            Int32 vBusId = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlRelation = (DropDownList)e.Row.FindControl("ddlRelation");
                    HiddenField hdRelation = (HiddenField)e.Row.FindControl("hdRelation");
                    oMem = new CMember();
                    dt = new DataTable();
                    dt = oMem.GetRelationList();
                    ddlRelation.Items.Clear();
                    ddlRelation.DataSource = dt;
                    ddlRelation.DataTextField = "Relation";
                    ddlRelation.DataValueField = "RelationId";
                    ddlRelation.DataBind();
                    oL1 = new ListItem("<-- Select -->", "-1");
                    ddlRelation.Items.Insert(0, oL1);
                    //ddlRelation.SelectedIndex = ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue(e.Row.Cells[13].Text));
                    ddlRelation.ClearSelection();
                    ddlRelation.SelectedIndex = ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue(Convert.ToString(hdRelation.Value)));

                    DropDownList ddlState = (DropDownList)e.Row.FindControl("ddlState");
                    HiddenField hdStateId = (HiddenField)e.Row.FindControl("hdStateId");
                    oMem = new CMember();
                    dt1 = new DataTable();
                    dt1 = oMem.GetStateName();
                    ddlState.Items.Clear();
                    ddlState.DataSource = dt1;
                    ddlState.DataTextField = "StateName";
                    ddlState.DataValueField = "StateId";
                    ddlState.DataBind();
                    oL1 = new ListItem("<-- Select -->", "-1");
                    ddlState.Items.Insert(0, oL1);
                    ddlState.ClearSelection();
                    ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(Convert.ToString(hdStateId.Value)));

                    DropDownList ddlKYCtype = (DropDownList)e.Row.FindControl("ddlKYCtype");
                    HiddenField hdKYCType = (HiddenField)e.Row.FindControl("hdKYCType");
                    dt2 = new DataTable();
                    oNM = new CNewMember();
                    dt2 = oNM.popCoAppIdProof("Y", "Y");
                    ddlKYCtype.Items.Clear();
                    ddlKYCtype.DataSource = dt2;
                    ddlKYCtype.DataTextField = "IDProofName";
                    ddlKYCtype.DataValueField = "IDProofId";
                    ddlKYCtype.DataBind();
                    oL1 = new ListItem("<-- Select -->", "-1");
                    ddlKYCtype.Items.Insert(0, oL1);
                    ddlKYCtype.ClearSelection();
                    ddlKYCtype.SelectedIndex = ddlKYCtype.Items.IndexOf(ddlKYCtype.Items.FindByValue(Convert.ToString(hdKYCType.Value)));

                    DropDownList ddlBusinessType = (DropDownList)e.Row.FindControl("ddlBusinessType");
                    HiddenField hdBusinessTypeId = (HiddenField)e.Row.FindControl("hdBusinessTypeId");
                    oGb = new CBusinessType();
                    dt3 = new DataTable();
                    dt3 = oGb.PopBusinessType();
                    ddlBusinessType.Items.Clear();
                    ddlBusinessType.DataSource = dt3;
                    ddlBusinessType.DataTextField = "BusinessTypeName";
                    ddlBusinessType.DataValueField = "BusinessTypeId";
                    ddlBusinessType.DataBind();
                    oL1 = new ListItem("<-- Select -->", "-1");
                    ddlBusinessType.Items.Insert(0, oL1);
                    ddlBusinessType.ClearSelection();
                    ddlBusinessType.SelectedIndex = ddlBusinessType.Items.IndexOf(ddlBusinessType.Items.FindByValue(Convert.ToString(hdBusinessTypeId.Value)));

                    DropDownList ddlOccupationType = (DropDownList)e.Row.FindControl("ddlOccupationType");
                    HiddenField hdOccupationId = (HiddenField)e.Row.FindControl("hdOccupationId");
                    oGb1 = new CGblIdGenerator();
                    dt4 = new DataTable();
                    dt4 = oGb1.PopComboMIS("N", "N", "AA", "OccupationId", "OccupationName", "OccupationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                    ddlOccupationType.Items.Clear();
                    ddlOccupationType.DataSource = dt4;
                    ddlOccupationType.DataTextField = "OccupationName";
                    ddlOccupationType.DataValueField = "OccupationId";
                    ddlOccupationType.DataBind();
                    oL1 = new ListItem("<-- Select -->", "-1");
                    ddlOccupationType.Items.Insert(0, oL1);
                    ddlOccupationType.ClearSelection();
                    ddlOccupationType.SelectedIndex = ddlOccupationType.Items.IndexOf(ddlOccupationType.Items.FindByValue(Convert.ToString(hdOccupationId.Value)));

                    DropDownList ddlIncomeFrequency = (DropDownList)e.Row.FindControl("ddlIncomeFrequency");
                    HiddenField hdIncomeFrequencyId = (HiddenField)e.Row.FindControl("hdIncomeFrequencyId");
                    //ddlIncomeFrequency.ClearSelection();
                    ddlIncomeFrequency.SelectedIndex = ddlIncomeFrequency.Items.IndexOf(ddlIncomeFrequency.Items.FindByValue(Convert.ToString(hdIncomeFrequencyId.Value)));

                    vBusId = Convert.ToInt32(ddlBusinessType.SelectedValue);
                    if (Convert.ToString(vBusId) == "-1") vBusId = 1;
                    DropDownList ddlBusinessActv = (DropDownList)e.Row.FindControl("ddlBusinessActv");
                    HiddenField hdBusinessActvId = (HiddenField)e.Row.FindControl("hdBusinessActvId");
                    oGb = new CBusinessType();
                    dt5 = new DataTable();
                    dt5 = oGb.PopBusActvByBusTypeId(vBusId);
                    ddlBusinessActv.Items.Clear();
                    ddlBusinessActv.DataSource = dt5;
                    ddlBusinessActv.DataTextField = "BusinessActivity";
                    ddlBusinessActv.DataValueField = "BusinessActivityId";
                    ddlBusinessActv.DataBind();
                    oL1 = new ListItem("<-- Select -->", "-1");
                    ddlBusinessActv.Items.Insert(0, oL1);
                    ddlBusinessActv.ClearSelection();
                    ddlBusinessActv.SelectedIndex = ddlBusinessActv.Items.IndexOf(ddlBusinessActv.Items.FindByValue(Convert.ToString(hdBusinessActvId.Value)));
                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                dt2 = null;
                dt3 = null;
                dt4 = null;
                oMem = null;
                oNM = null;
                oGb = null;
                oGb1 = null;
            }
        }
        protected void btnAddNew1_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            Int32 vR = 0;
            DataRow dr;
            dt = (DataTable)ViewState["EarningMember"];
            if (dt.Rows.Count > 0)
            {
                vR = dt.Rows.Count - 1;
                Label lblSlNo = (Label)gvEarningMember.Rows[vR].FindControl("lblSlNo");
                dt.Rows[vR]["SlNo"] = lblSlNo.Text;
                TextBox txtName = (TextBox)gvEarningMember.Rows[vR].FindControl("txtName");
                dt.Rows[vR]["Name"] = txtName.Text;
                TextBox txtDOB = (TextBox)gvEarningMember.Rows[vR].FindControl("txtDOB");
                dt.Rows[vR]["DOB"] = txtDOB.Text;
                DropDownList ddlRelation = (DropDownList)gvEarningMember.Rows[vR].FindControl("ddlRelation");
                dt.Rows[vR]["Relation"] = ddlRelation.SelectedValue;
                TextBox txtAddress = (TextBox)gvEarningMember.Rows[vR].FindControl("txtAddress");
                dt.Rows[vR]["Address1"] = txtAddress.Text;
                DropDownList ddlState = (DropDownList)gvEarningMember.Rows[vR].FindControl("ddlState");
                dt.Rows[vR]["StateId"] = ddlState.SelectedValue;
                TextBox txtPinCode = (TextBox)gvEarningMember.Rows[vR].FindControl("txtPinCode");
                dt.Rows[vR]["PinCode"] = txtPinCode.Text;
                TextBox txtMobile = (TextBox)gvEarningMember.Rows[vR].FindControl("txtMobile");
                dt.Rows[vR]["MobileNo"] = txtMobile.Text;
                DropDownList ddlKYCType = (DropDownList)gvEarningMember.Rows[vR].FindControl("ddlKYCType");
                dt.Rows[vR]["KYCType"] = ddlKYCType.SelectedValue;
                TextBox txtKYCNo = (TextBox)gvEarningMember.Rows[vR].FindControl("txtKYCNo");
                dt.Rows[vR]["KYCNo"] = txtKYCNo.Text;
                DropDownList ddlBusinessType = (DropDownList)gvEarningMember.Rows[vR].FindControl("ddlBusinessType");
                dt.Rows[vR]["BusinessTypeId"] = ddlBusinessType.SelectedValue;
                DropDownList ddlOccupationType = (DropDownList)gvEarningMember.Rows[vR].FindControl("ddlOccupationType");
                dt.Rows[vR]["OccupationId"] = ddlOccupationType.SelectedValue;
                TextBox txtDeclaredIncome = (TextBox)gvEarningMember.Rows[vR].FindControl("txtDeclaredIncome");
                dt.Rows[vR]["DeclaredIncome"] = txtDeclaredIncome.Text;
                DropDownList ddlIncomeFrequency = (DropDownList)gvEarningMember.Rows[vR].FindControl("ddlIncomeFrequency");
                dt.Rows[vR]["IncomeFrequencyId"] = ddlIncomeFrequency.SelectedValue;
                DropDownList ddlBusinessActv = (DropDownList)gvEarningMember.Rows[vR].FindControl("ddlBusinessActv");
                dt.Rows[vR]["BusinessActvId"] = ddlBusinessActv.SelectedValue;
            }
            dt.AcceptChanges();

            if (dt.Rows[vR]["Name"].ToString() != "-1")
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            else
            {
                gblFuction.MsgPopup("Earning member name is Blank...");
            }
            ViewState["EarningMember"] = dt;
            gvEarningMember.DataSource = dt;
            gvEarningMember.DataBind();
        }
        private void gvEarningMember_Row_Initialize()
        {
            Int32 vR = 0;
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["EarningMember"];
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            Label lblSlNo = (Label)gvEarningMember.Rows[vR].FindControl("lblSlNo");
            dt.Rows[vR]["SlNo"] = lblSlNo.Text;
            TextBox txtName = (TextBox)gvEarningMember.Rows[vR].FindControl("txtName");
            dt.Rows[vR]["Name"] = txtName.Text;
            TextBox txtDOB = (TextBox)gvEarningMember.Rows[vR].FindControl("txtDOB");
            dt.Rows[vR]["DOB"] = txtDOB.Text;
            DropDownList ddlRelation = (DropDownList)gvEarningMember.Rows[vR].FindControl("ddlRelation");
            dt.Rows[vR]["Relation"] = ddlRelation.SelectedValue;
            TextBox txtAddress = (TextBox)gvEarningMember.Rows[vR].FindControl("txtAddress");
            dt.Rows[vR]["Address1"] = txtAddress.Text;
            DropDownList ddlState = (DropDownList)gvEarningMember.Rows[vR].FindControl("ddlState");
            dt.Rows[vR]["StateId"] = ddlState.SelectedValue;
            TextBox txtPinCode = (TextBox)gvEarningMember.Rows[vR].FindControl("txtPinCode");
            dt.Rows[vR]["PinCode"] = txtPinCode.Text;
            TextBox txtMobile = (TextBox)gvEarningMember.Rows[vR].FindControl("txtMobile");
            dt.Rows[vR]["MobileNo"] = txtMobile.Text;
            DropDownList ddlKYCType = (DropDownList)gvEarningMember.Rows[vR].FindControl("ddlKYCType"); ;
            dt.Rows[vR]["KYCType"] = ddlKYCType.SelectedValue;
            TextBox txtKYCNo = (TextBox)gvEarningMember.Rows[vR].FindControl("txtKYCNo");
            dt.Rows[vR]["KYCNo"] = txtKYCNo.Text;
            DropDownList ddlBusinessType = (DropDownList)gvEarningMember.Rows[vR].FindControl("ddlBusinessType");
            dt.Rows[vR]["BusinessTypeId"] = ddlBusinessType.SelectedValue;
            DropDownList ddlOccupationType = (DropDownList)gvEarningMember.Rows[vR].FindControl("ddlOccupationType");
            dt.Rows[vR]["OccupationId"] = ddlOccupationType.SelectedValue;
            DropDownList ddlIncomeFrequency = (DropDownList)gvEarningMember.Rows[vR].FindControl("ddlIncomeFrequency");
            dt.Rows[vR]["IncomeFrequencyId"] = ddlIncomeFrequency.SelectedValue;
            TextBox txtDeclaredIncome = (TextBox)gvEarningMember.Rows[vR].FindControl("txtDeclaredIncome");
            dt.Rows[vR]["DeclaredIncome"] = txtDeclaredIncome.Text;
            DropDownList ddlBusinessActv = (DropDownList)gvEarningMember.Rows[vR].FindControl("ddlBusinessActv");
            dt.Rows[vR]["BusinessActvId"] = ddlBusinessActv.SelectedValue;
            dt.AcceptChanges();
            gvEarningMember.DataSource = dt;
            gvEarningMember.DataBind();
        }
        private string EarningMemDtlDtToXml()
        {
            string vXml = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                //btnAddNew1_Click(null,null);                
                GetDataEarningMember();
                dt = (DataTable)ViewState["EarningMember"];
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXml = oSW.ToString().Replace("T00:00:00+05:30", "");
                }
                return vXml;
            }
            finally
            {
                dt = null;
            }
        }
        public void GetDataEarningMember()
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["EarningMember"];
            foreach (GridViewRow gr in gvEarningMember.Rows)
            {
                Label lblSlNo = (Label)gvEarningMember.Rows[gr.RowIndex].FindControl("lblSlNo");
                TextBox txtName = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtName");
                TextBox txtDOB = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtDOB");
                DropDownList ddlRelation = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlRelation");
                TextBox txtAddress = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtAddress");
                DropDownList ddlState = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlState");
                TextBox txtPinCode = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtPinCode");
                TextBox txtMobile = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtMobile");
                DropDownList ddlKYCType = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlKYCType");
                TextBox txtKYCNo = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtKYCNo");
                DropDownList ddlBusinessType = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlBusinessType");
                DropDownList ddlOccupationType = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlOccupationType");
                DropDownList ddlIncomeFrequency = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlIncomeFrequency");
                TextBox txtDeclaredIncome = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtDeclaredIncome");
                DropDownList ddlBusinessActv = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlBusinessActv");

                dt.Rows[gr.RowIndex]["SlNo"] = lblSlNo.Text;
                dt.Rows[gr.RowIndex]["Name"] = txtName.Text;
                dt.Rows[gr.RowIndex]["DOB"] = txtDOB.Text;
                dt.Rows[gr.RowIndex]["Relation"] = ddlRelation.SelectedValue;
                dt.Rows[gr.RowIndex]["Address1"] = txtAddress.Text;
                dt.Rows[gr.RowIndex]["StateId"] = ddlState.SelectedValue;
                dt.Rows[gr.RowIndex]["PinCode"] = txtPinCode.Text;
                dt.Rows[gr.RowIndex]["MobileNo"] = txtMobile.Text;
                dt.Rows[gr.RowIndex]["KYCType"] = ddlKYCType.SelectedValue;
                dt.Rows[gr.RowIndex]["KYCNo"] = txtKYCNo.Text;
                dt.Rows[gr.RowIndex]["BusinessTypeId"] = ddlBusinessType.SelectedValue;
                dt.Rows[gr.RowIndex]["OccupationId"] = ddlOccupationType.SelectedValue;
                dt.Rows[gr.RowIndex]["IncomeFrequencyId"] = ddlIncomeFrequency.SelectedValue;
                dt.Rows[gr.RowIndex]["DeclaredIncome"] = txtDeclaredIncome.Text.Trim() == "" ? "0" : txtDeclaredIncome.Text.Trim();
                dt.Rows[gr.RowIndex]["BusinessActvId"] = ddlBusinessActv.SelectedValue;
            }
            dt.AcceptChanges();
            ViewState["EarningMember"] = dt;
            gvEarningMember.DataSource = dt;
            gvEarningMember.DataBind();
        }
        protected void txtCoAppDOB_TextChanged(object sender, EventArgs e)
        {
            //int Years = CalAge(txtCoAppDOB.Text);
            int Years = AgeCount(txtCoAppDOB.Text, Convert.ToString(Session[gblValue.LoginDate]));
            txtCoAppAge.Text = Years.ToString();
        }
        private void popBusinessType()
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            try
            {
                oGb = new CBusinessType();
                dt = oGb.PopBusinessType();
                ddlBusType.DataSource = dt;
                ddlBusType.DataTextField = "BusinessTypeName";
                ddlBusType.DataValueField = "BusinessTypeId";
                ddlBusType.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBusType.Items.Insert(0, oli);

                ddlCoAppBusType.DataSource = dt;
                ddlCoAppBusType.DataTextField = "BusinessTypeName";
                ddlCoAppBusType.DataValueField = "BusinessTypeId";
                ddlCoAppBusType.DataBind();
                ddlCoAppBusType.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        private void popOccupation()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "OccupationId", "OccupationName", "OccupationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlOccupation.DataSource = dt;
                ddlOccupation.DataTextField = "OccupationName";
                ddlOccupation.DataValueField = "OccupationId";
                ddlOccupation.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCoAppOccupation.Items.Insert(0, oli);
                ddlCoAppOccupation.DataSource = dt;
                ddlCoAppOccupation.DataTextField = "OccupationName";
                ddlCoAppOccupation.DataValueField = "OccupationId";
                ddlCoAppOccupation.DataBind();
                ddlCoAppOccupation.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        private void popCoAppIdProof()
        {
            DataTable dt = null;
            CNewMember oNM = null;
            try
            {
                oNM = new CNewMember();
                dt = oNM.popCoAppIdProof("Y", "Y");
                ddlCoAppID.DataSource = dt;
                ddlCoAppID.DataTextField = "IDProofName";
                ddlCoAppID.DataValueField = "IDProofId";
                ddlCoAppID.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCoAppID.Items.Insert(0, oli);
            }
            finally
            {
                oNM = null;
                dt = null;
            }
        }
        private void popBusinessActivityAll()
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            Int32 vBusinessTypeId = -1;
            try
            {
                oGb = new CBusinessType();
                dt = oGb.PopBusActvByBusTypeId(vBusinessTypeId);
                ddlBusActivity.DataSource = dt;
                ddlBusActivity.DataTextField = "BusinessActivity";
                ddlBusActivity.DataValueField = "BusinessActivityId";
                ddlBusActivity.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBusActivity.Items.Insert(0, oli);

                ddlCoAppBusActivity.DataSource = dt;
                ddlCoAppBusActivity.DataTextField = "BusinessActivity";
                ddlCoAppBusActivity.DataValueField = "BusinessActivityID";
                ddlCoAppBusActivity.DataBind();
                oli = new ListItem("<--Select-->", "-1");
                ddlCoAppBusActivity.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        protected void ddlBusType_SelectedIndexChanged(object sender, EventArgs e)
        {
            popBusinessActv(Convert.ToInt32(ddlBusType.SelectedValue));
        }
        protected void ddlCoAppBusType_SelectedIndexChanged(object sender, EventArgs e)
        {
            popCoAppBusinessActv(Convert.ToInt32(ddlCoAppBusType.SelectedValue));
        }
        protected void ddlBusinessType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            Int32 vBusinessTypeId = 0;
            try
            {
                DropDownList ddlBusinessType = (DropDownList)sender;
                GridViewRow row = (GridViewRow)ddlBusinessType.NamingContainer;
                DropDownList ddlBusinessActv = (DropDownList)row.FindControl("ddlBusinessActv");

                vBusinessTypeId = Convert.ToInt32(ddlBusinessType.SelectedValue);
                oGb = new CBusinessType();
                dt = oGb.PopBusActvByBusTypeId(vBusinessTypeId);
                ddlBusinessActv.DataSource = dt;
                ddlBusinessActv.DataTextField = "BusinessActivity";
                ddlBusinessActv.DataValueField = "BusinessActivityId";
                ddlBusinessActv.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBusinessActv.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        private void popBusinessActv(Int32 pId)
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            Int32 vBusinessTypeId = pId;
            try
            {
                vBusinessTypeId = Convert.ToInt32(ddlBusType.SelectedValue);
                oGb = new CBusinessType();
                dt = oGb.PopBusActvByBusTypeId(vBusinessTypeId);
                ddlBusActivity.DataSource = dt;
                ddlBusActivity.DataTextField = "BusinessActivity";
                ddlBusActivity.DataValueField = "BusinessActivityId";
                ddlBusActivity.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBusActivity.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        private void popCoAppBusinessActv(Int32 pId)
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            Int32 vBusinessTypeId = pId;
            try
            {
                vBusinessTypeId = Convert.ToInt32(ddlCoAppBusType.SelectedValue);
                oGb = new CBusinessType();
                dt = oGb.PopBusActvByBusTypeId(vBusinessTypeId);
                ddlCoAppBusActivity.DataSource = dt;
                ddlCoAppBusActivity.DataTextField = "BusinessActivity";
                ddlCoAppBusActivity.DataValueField = "BusinessActivityId";
                ddlCoAppBusActivity.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCoAppBusActivity.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        private string EarningMemDtlDtToXmlFinal()
        {
            string vXml = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                dt = GetDataEarningMemberFinal();
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXml = oSW.ToString().Replace("T00:00:00+05:30", "");
                }
                return vXml;
            }
            finally
            {
                dt = null;
            }
        }
        public DataTable GetDataEarningMemberFinal()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt = ((DataTable)ViewState["EarningMember"]).Clone();
            foreach (GridViewRow gr in gvEarningMember.Rows)
            {
                Label lblSlNo = (Label)gvEarningMember.Rows[gr.RowIndex].FindControl("lblSlNo");
                TextBox txtName = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtName");
                TextBox txtDOB = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtDOB");
                DropDownList ddlRelation = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlRelation");
                TextBox txtAddress = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtAddress");
                DropDownList ddlState = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlState");
                TextBox txtPinCode = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtPinCode");
                TextBox txtMobile = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtMobile");
                DropDownList ddlKYCType = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlKYCType");
                TextBox txtKYCNo = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtKYCNo");
                DropDownList ddlBusinessType = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlBusinessType");
                DropDownList ddlOccupationType = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlOccupationType");
                DropDownList ddlIncomeFrequency = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlIncomeFrequency");
                TextBox txtDeclaredIncome = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtDeclaredIncome");
                DropDownList ddlBusinessActv = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlBusinessActv");

                if (txtName.Text.Trim() != "")
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);

                    dt.Rows[gr.RowIndex]["SlNo"] = lblSlNo.Text;
                    dt.Rows[gr.RowIndex]["Name"] = txtName.Text;
                    if (txtDOB.Text.Trim() == "")
                    {
                        dt.Rows[gr.RowIndex]["DOB"] = txtDOB.Text;
                    }
                    else
                    {
                        dt.Rows[gr.RowIndex]["DOB"] = Convert.ToString(gblFuction.setDate(txtDOB.Text).ToString("MM/dd/yyyy"));
                    }
                    dt.Rows[gr.RowIndex]["Relation"] = ddlRelation.SelectedValue;
                    dt.Rows[gr.RowIndex]["Address1"] = txtAddress.Text;
                    dt.Rows[gr.RowIndex]["StateId"] = ddlState.SelectedValue;
                    dt.Rows[gr.RowIndex]["PinCode"] = txtPinCode.Text;
                    dt.Rows[gr.RowIndex]["MobileNo"] = txtMobile.Text;
                    dt.Rows[gr.RowIndex]["KYCType"] = ddlKYCType.SelectedValue;
                    dt.Rows[gr.RowIndex]["KYCNo"] = txtKYCNo.Text;
                    dt.Rows[gr.RowIndex]["BusinessTypeId"] = ddlBusinessType.SelectedValue;
                    dt.Rows[gr.RowIndex]["OccupationId"] = ddlOccupationType.SelectedValue;
                    dt.Rows[gr.RowIndex]["IncomeFrequencyId"] = ddlIncomeFrequency.SelectedValue;
                    dt.Rows[gr.RowIndex]["DeclaredIncome"] = txtDeclaredIncome.Text.Trim() == "" ? "0" : txtDeclaredIncome.Text.Trim();
                    dt.Rows[gr.RowIndex]["BusinessActvId"] = ddlBusinessActv.SelectedValue;
                }
            }
            dt.AcceptChanges();
            return dt;
        }

    }
}
