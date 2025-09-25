using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Data;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Xml;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class JocataApproval : CENTRUMBase
    {
        protected int vPgNo = 1;
        protected string vMemberId = "";
        string path = "", pathMember = "", pathImage = "";
        string CCRUserName = "", CCRPassword = "";
        Int32 vPathDeviationMaxLength = Convert.ToInt32(ConfigurationManager.AppSettings["PathDeviationMaxLength"]);
        string vPathJocata = ConfigurationManager.AppSettings["PathJocata"];

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
                ViewState["AadhaarScan"] = "M";
                string vLoginDt = Session[gblValue.LoginDate].ToString();
                txtDtFrm.Text = txtDtTo.Text = vLoginDt;
                ceDtTo.EndDate = gblFuction.setDate(vLoginDt);
                popRelation();
                GenerateEarningMember();
                popBusinessType();
                popOccupation();
                popBusinessActivityAll();
                ViewState["MemberId"] = null;
                ViewState["CGTId"] = null;
                ViewState["ScreeningID"] = null;
            }
        }

        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
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
                this.PageHeading = "Jocata Approval";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuJocataApprove);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Jocata Approval", false);
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
                    EnableControl(false);
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
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            CMember oMem = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            int vRet = 0;
            string vMemId = Convert.ToString(ViewState["MemId"]);
            //this.GetModuleByRole(mnuID.mnuInitialApproachApprove);
            //if (this.CanEdit == "N")
            //{
            //    gblFuction.MsgPopup(MsgAccess.Edit);
            //    return;
            //}
            //btnCgtApply.Enabled = false;
            tbEmp.ActiveTabIndex = 1;
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");

            //if (this.RoleId != 1)  //&& this.RoleId != 5 && this.RoleId != 10 && this.RoleId != 25 && this.RoleId != 13
            //{
            //    oMem = new CMember();
            //    vRet = oMem.ChkMemEdit(vMemId, vLogDt);
            //    if (vRet == 0)
            //    {
            //        EnableControl(false);
            //    }
            //}
            //if (Convert.ToString(ViewState["AadhaarScan"]) == "A")
            //{
            //    if (ddlIdentyProf.SelectedValue == "1")
            //    {
            //        txtIdntPrfNo.Enabled = false;
            //    }
            //    else if (ddlAddPrf.SelectedValue == "1")
            //    {
            //        txtAddPrfNo.Enabled = false;
            //    }
            //    else if (ddlIdProof3.SelectedValue == "1")
            //    {
            //        txtIdProof3.Enabled = false;
            //    }
            //}
            EnableControl(false);
            ddlApprove.Enabled = true;
            txtRemarks.Enabled = true;
            //if (this.RoleId != 1)
            //{
            //    EnableOCRControl();
            //}
            //else
            //{
            //    EnableControl(true);
            //}
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
                LoadGrid();
                EnableControl(false);
                ViewState["MemberId"] = null;
                ViewState["CGTId"] = null;
                ViewState["ScreeningID"] = null;
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
            dt = oMem.GetJocataApprMember(gblFuction.setDate(txtDtFrm.Text), gblFuction.setDate(txtDtTo.Text), vBrCode, txtSearch.Text);
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
                Label lblRisk_Changed_Dt = (Label)gvRow.FindControl("lblRisk_Changed_Dt");
                ViewState["MemberId"] = gvRow.Cells[0].Text;
                ViewState["CGTId"] = gvRow.Cells[1].Text;
                ViewState["ScreeningID"] = gvRow.Cells[2].Text;

                ViewState["ScreeningStatus"] = gvRow.Cells[8].Text.Trim();
                ViewState["Remarks"] = gvRow.Cells[9].Text.Trim();
                ViewState["RiskCat"] = gvRow.Cells[10].Text.Trim();

                ViewState["RiskChangedDt"] = lblRisk_Changed_Dt.Text; //gvRow.Cells[11].Text.Trim();

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
            ViewState["CBID"] = "";
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
                    lblAadharScan.Text = dt.Rows[0]["AadhaarScan"].ToString();
                    ViewState["CBID"] = dt.Rows[0]["CBID"].ToString();
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

                    if (Convert.ToInt32(Session[gblValue.RoleId]) != 1)
                    {
                        if (dt.Rows[0]["IdentyPRofId"].ToString() == "1")
                        {
                            lblAadhar.Text = Convert.ToString(dt.Rows[0]["IdentyProfNo"]);
                            txtIdntPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["IdentyProfNo"]).Substring(Convert.ToString(dt.Rows[0]["IdentyProfNo"]).Length - 4, 4));
                            txtConfrmAadhar1.Attributes.Add("value", Convert.ToString(dt.Rows[0]["IdentyProfNo"]));
                        }
                        else
                        {
                            txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["IdentyProfNo"]);
                        }
                        if (dt.Rows[0]["AddProfId"].ToString() == "1")
                        {
                            lblAadhar.Text = Convert.ToString(dt.Rows[0]["AddProfNo"]);
                            txtAddPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["AddProfNo"]).Substring(Convert.ToString(dt.Rows[0]["AddProfNo"]).Length - 4, 4));
                            txtConfrmAadhar2.Attributes.Add("value", Convert.ToString(dt.Rows[0]["AddProfNo"]));
                        }
                        else
                        {
                            txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["AddProfNo"]);
                        }
                        if (dt.Rows[0]["AddProfId2"].ToString() == "1")
                        {
                            lblAadhar.Text = Convert.ToString(dt.Rows[0]["AddProfId2"]);
                            txtIdProof3.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["AddProfNo2"]).Substring(Convert.ToString(dt.Rows[0]["AddProfNo2"]).Length - 4, 4));
                            txtConfrmAadhar3.Attributes.Add("value", Convert.ToString(dt.Rows[0]["AddProfId2"]));
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
                            txtConfrmAadhar1.Attributes.Add("value", Convert.ToString(dt.Rows[0]["IdentyProfNo"]));
                        }
                        else if (dt.Rows[0]["AddProfId"].ToString() == "1")
                        {
                            lblAadhar.Text = Convert.ToString(dt.Rows[0]["AddProfNo"]);
                            txtConfrmAadhar2.Attributes.Add("value", Convert.ToString(dt.Rows[0]["AddProfNo"]));
                        }
                        else if (dt.Rows[0]["AddProfId2"].ToString() == "1")
                        {
                            lblAadhar.Text = Convert.ToString(dt.Rows[0]["AddProfId2"]);
                            txtConfrmAadhar3.Attributes.Add("value", Convert.ToString(dt.Rows[0]["AddProfNo2"]));
                        }

                        txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["IdentyProfNo"]);
                        txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["AddProfNo"]);
                        txtIdProof3.Text = Convert.ToString(dt.Rows[0]["AddProfNo2"]);
                    }

                    txtDist.Text = Convert.ToString(dt.Rows[0]["District"]);
                    ddlAddPrf.SelectedIndex = ddlAddPrf.Items.IndexOf(ddlAddPrf.Items.FindByValue(dt.Rows[0]["AddProfId"].ToString()));
                    ddlIdentyProf.SelectedIndex = ddlIdentyProf.Items.IndexOf(ddlIdentyProf.Items.FindByValue(dt.Rows[0]["IdentyPRofId"].ToString()));
                    ddlCoAppID.SelectedIndex = ddlCoAppID.Items.IndexOf(ddlCoAppID.Items.FindByValue(dt.Rows[0]["CoAppIdentyProfId"].ToString()));
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
                    FillOCRDtl(vEnqId);

                    // ddlApprove.SelectedIndex = ddlApprove.Items.IndexOf(ddlApprove.Items.FindByValue(dt.Rows[0]["OCRApproveYN"].ToString()));

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
                    }

                    tbEmp.ActiveTabIndex = 1;
                    StatusButton("Show");
                    //--------------------------------------------
                    FillJocataStatus();
                    EnableControl(false);
                    gvEarningMember.Enabled = false;
                }
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void FillOCRDtl(string vEnqId)
        {
            ViewState["EnqId"] = "";
            DataSet ds = new DataSet();
            DataTable dt1, dt2, dt3, dt4, dt5, dt6, dt7, dt8, dt9, dt10, dt11, dt12, dt13 = null;
            CMember oMem = null;
            try
            {
                ViewState["EnqId"] = vEnqId;
                oMem = new CMember();
                ds = oMem.GetOCRdDataByEnqId(vEnqId);
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                dt4 = ds.Tables[3];
                dt5 = ds.Tables[4];
                dt6 = ds.Tables[5];
                dt7 = ds.Tables[6];
                dt8 = ds.Tables[7];
                dt9 = ds.Tables[8];
                dt10 = ds.Tables[9];
                dt11 = ds.Tables[10];
                dt12 = ds.Tables[11];
                dt13 = ds.Tables[12];


                if (dt1.Rows.Count > 0)
                {
                    lblId1OCRName.Text = dt1.Rows[0]["Name"].ToString();
                    lblId1OCRDOB.Text = dt1.Rows[0]["DOB"].ToString();
                    lblId1OCRGender.Text = dt1.Rows[0]["Gender"].ToString();
                    lblId1OCRCo.Text = dt1.Rows[0]["Father"].ToString();
                    lblId1OCRNo.Text = dt1.Rows[0]["ID1"].ToString();
                }
                if (dt2.Rows.Count > 0)
                {
                    lblId1OCRAddress.Text = dt2.Rows[0]["Address"].ToString();
                    lblId1OCRHouse.Text = dt2.Rows[0]["House"].ToString();
                    lblId1OCRLocation.Text = dt2.Rows[0]["Location"].ToString();

                    lblId1OCRStreet.Text = dt2.Rows[0]["Street"].ToString();
                    lblId1OCRLandmark.Text = dt2.Rows[0]["Landmark"].ToString();
                    lblId1OCRVillage.Text = dt2.Rows[0]["City"].ToString();
                    lblId1OCRDistrict.Text = dt2.Rows[0]["District"].ToString();
                    lblId1OCRState.Text = dt2.Rows[0]["state"].ToString();
                }

                if (dt3.Rows.Count > 0)
                {
                    lblId1Name.Text = dt3.Rows[0]["Name"].ToString();
                    lblId1DOB.Text = dt3.Rows[0]["DOB"].ToString();
                    lblId1Gender.Text = dt3.Rows[0]["Gender"].ToString();
                    lblId1Co.Text = dt3.Rows[0]["CO"].ToString();
                    lblId1No.Text = dt3.Rows[0]["ID1"].ToString();

                    lblId1Address.Text = dt3.Rows[0]["Address"].ToString();
                    lblId1House.Text = dt3.Rows[0]["House"].ToString();
                    lblId1Location.Text = dt3.Rows[0]["Location"].ToString();

                    lblId1Street.Text = dt3.Rows[0]["Street"].ToString();
                    lblId1Landmark.Text = dt3.Rows[0]["Landmark"].ToString();
                    lblId1Village.Text = dt3.Rows[0]["City"].ToString();
                    lblId1District.Text = dt3.Rows[0]["District"].ToString();
                    lblId1State.Text = dt3.Rows[0]["state"].ToString();

                }
                if (dt4.Rows.Count > 0)
                {
                    lblNameMatchYN.Text = dt4.Rows[0]["Result"].ToString();
                    lblNameMatchScore.Text = dt4.Rows[0]["Score"].ToString();
                }
                else
                {
                    lblNameMatchYN.Text = "";
                    lblNameMatchScore.Text = "";
                }
                if (dt5.Rows.Count > 0)
                {
                    lblAddMatchYN.Text = dt5.Rows[0]["Result"].ToString();
                    lblAddMatchScore.Text = dt5.Rows[0]["Score"].ToString();
                }
                else
                {
                    lblAddMatchYN.Text = "";
                    lblAddMatchScore.Text = "";
                }

                if (dt6.Rows.Count > 0)
                {
                    lblFaceMatchYN.Text = dt6.Rows[0]["Result"].ToString();
                    lblFaceMatchScore.Text = dt6.Rows[0]["Score"].ToString();
                }
                else
                {
                    lblFaceMatchYN.Text = "";
                    lblFaceMatchScore.Text = "";
                }
                if (dt7.Rows.Count > 0)
                {
                    lblId2OCRName.Text = dt7.Rows[0]["Name"].ToString();
                    lblId2OCRDOB.Text = dt7.Rows[0]["DOB"].ToString();
                    lblId2OCRGender.Text = dt7.Rows[0]["Gender"].ToString();
                    lblId2OCRCo.Text = dt7.Rows[0]["Father"].ToString();
                    lblId2OCRNo.Text = dt7.Rows[0]["ID2"].ToString();
                }
                else
                {
                    lblId2OCRName.Text = "";
                    lblId2OCRDOB.Text = "";
                    lblId2OCRGender.Text = "";
                    lblId2OCRCo.Text = "";
                    lblId2OCRNo.Text = "";
                }
                if (dt8.Rows.Count > 0)
                {
                    lblId2OCRAddress.Text = dt8.Rows[0]["Address"].ToString();
                    lblId2OCRHouse.Text = dt8.Rows[0]["House"].ToString();
                    lblId2OCRLocation.Text = dt8.Rows[0]["Location"].ToString();
                    lblId2OCRStreet.Text = dt8.Rows[0]["Street"].ToString();
                    lblId2OCRLandmark.Text = dt8.Rows[0]["Landmark"].ToString();
                    lblId2OCRVillage.Text = dt8.Rows[0]["City"].ToString();
                    lblId2OCRDistrict.Text = dt8.Rows[0]["District"].ToString();
                    lblId2OCRState.Text = dt8.Rows[0]["state"].ToString();
                }
                else
                {
                    lblId2OCRAddress.Text = "";
                    lblId2OCRHouse.Text = "";
                    lblId2OCRLocation.Text = "";
                    lblId2OCRStreet.Text = "";
                    lblId2OCRLandmark.Text = "";
                    lblId2OCRVillage.Text = "";
                    lblId2OCRDistrict.Text = "";
                    lblId2OCRState.Text = "";
                }
                if (dt1.Rows.Count > 0)
                {
                    if (ddlIdentyProf.SelectedValue == "1")
                    {
                        string ID1OCRNo = lblId1OCRNo.Text;
                        string Id1No = lblId1No.Text;
                        string FourDigitID1OCRNo = ID1OCRNo.Substring(ID1OCRNo.Length - 4);
                        string FourId1No = Id1No.Substring(Id1No.Length - 4);
                        if (FourDigitID1OCRNo == FourId1No)
                        {
                            lblIDMatchYN.Text = "True";
                            lblIDMatchScore.Text = "100";
                        }
                        else
                        {
                            lblIDMatchYN.Text = "False";
                            lblIDMatchScore.Text = "0";
                        }
                    }
                    else if (ddlIdentyProf.SelectedValue == "3")
                    {
                        if (lblId1OCRNo.Text.Trim() == lblId1No.Text.Trim())
                        {
                            lblIDMatchYN.Text = "True";
                            lblIDMatchScore.Text = "100";
                        }
                        else
                        {
                            lblIDMatchYN.Text = "False";
                            lblIDMatchScore.Text = "0";
                        }
                    }
                }
                //------------------------------------Co-Applicant--------------------------------------
                if (dt9.Rows.Count > 0)
                {
                    lblCoAppId1OCRName.Text = dt9.Rows[0]["Name"].ToString();
                    lblCoAppId1OCRDOB.Text = dt9.Rows[0]["DOB"].ToString();
                    lblCoAppId1OCRGender.Text = dt9.Rows[0]["Gender"].ToString();
                    lblCoAppId1OCRCo.Text = dt9.Rows[0]["Father"].ToString();
                    lblCoAppId1OCRNo.Text = dt9.Rows[0]["ID1"].ToString();
                }
                if (dt10.Rows.Count > 0)
                {
                    lblCoAppId1OCRAddress.Text = dt10.Rows[0]["Address"].ToString();
                    lblCoAppId1OCRHouse.Text = dt10.Rows[0]["House"].ToString();
                    lblCoAppId1OCRLocation.Text = dt10.Rows[0]["Location"].ToString();
                    lblCoAppId1OCRStreet.Text = dt10.Rows[0]["Street"].ToString();
                    lblCoAppId1OCRLandmark.Text = dt10.Rows[0]["Landmark"].ToString();
                    lblCoAppId1OCRVillage.Text = dt10.Rows[0]["City"].ToString();
                    lblCoAppId1OCRDistrict.Text = dt10.Rows[0]["District"].ToString();
                    lblCoAppId1OCRState.Text = dt10.Rows[0]["state"].ToString();
                }

                if (dt11.Rows.Count > 0)
                {
                    lblCoAppId1Name.Text = dt11.Rows[0]["Name"].ToString();
                    lblCoAppId1DOB.Text = dt11.Rows[0]["DOB"].ToString();
                    lblCoAppId1Gender.Text = dt11.Rows[0]["Gender"].ToString();
                    lblCoAppId1Co.Text = dt11.Rows[0]["CO"].ToString();
                    lblCoAppId1No.Text = dt11.Rows[0]["ID1"].ToString();
                    lblCoAppId1Address.Text = dt11.Rows[0]["Address"].ToString();
                    lblCoAppId1House.Text = dt11.Rows[0]["House"].ToString();
                    lblCoAppId1Location.Text = dt11.Rows[0]["Location"].ToString();
                    lblCoAppId1Street.Text = dt11.Rows[0]["Street"].ToString();
                    lblCoAppId1Landmark.Text = dt11.Rows[0]["Landmark"].ToString();
                    lblCoAppId1Village.Text = dt11.Rows[0]["City"].ToString();
                    lblCoAppId1District.Text = dt11.Rows[0]["District"].ToString();
                    lblCoAppId1State.Text = dt11.Rows[0]["state"].ToString();
                }
                if (dt12.Rows.Count > 0)
                {
                    lblCoAppNameMatchYN.Text = dt12.Rows[0]["Result"].ToString();
                    lblCoAppNameMatchScore.Text = dt12.Rows[0]["Score"].ToString();
                }
                else
                {
                    lblCoAppNameMatchYN.Text = "";
                    lblCoAppNameMatchScore.Text = "";
                }
                if (dt13.Rows.Count > 0)
                {
                    lblCoAppAddMatchYN.Text = dt13.Rows[0]["Result"].ToString();
                    lblCoAppAddMatchScore.Text = dt13.Rows[0]["Score"].ToString();
                }
                else
                {
                    lblAddMatchYN.Text = "";
                    lblAddMatchScore.Text = "";
                }

                if (dt9.Rows.Count > 0)
                {
                    if (ddlCoAppID.SelectedValue == "1")
                    {
                        string CoApplID1OCRNo = lblCoAppId1OCRNo.Text;
                        string CoApplId1No = lblCoAppId1No.Text;
                        string FourDigitCoApplID1OCRNo = CoApplID1OCRNo.Substring(CoApplID1OCRNo.Length - 4);
                        string FourCoApplId1No = CoApplId1No.Substring(CoApplId1No.Length - 4);
                        if (FourDigitCoApplID1OCRNo == FourCoApplId1No)
                        {
                            lblCoAppIDMatchYN.Text = "True";
                            lblCoAppIDMatchScore.Text = "100";
                        }
                        else
                        {
                            lblCoAppIDMatchYN.Text = "False";
                            lblCoAppIDMatchScore.Text = "0";
                        }
                    }
                    else if (ddlCoAppID.SelectedValue == "3")
                    {
                        if (lblCoAppId1OCRNo.Text.Trim() == lblCoAppId1No.Text.Trim())
                        {
                            lblCoAppIDMatchYN.Text = "True";
                            lblCoAppIDMatchScore.Text = "100";
                        }
                        else
                        {
                            lblCoAppIDMatchYN.Text = "False";
                            lblCoAppIDMatchScore.Text = "0";
                        }
                    }
                }
                //---------------------------------------------------------------------------------------
            }
            finally
            {
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
            imgSelfie.ImageUrl = vUrl + imgFolder + "/FrontSelfeImage.png";
        }

        private void FillJocataStatus()
        {
            //string vScreeningID = Convert.ToString(ViewState["ScreeningID"]);
            //DataTable dt = new DataTable();
            //CMember oMem = new CMember();
            //dt = oMem.GetJocataDtlByScrningID(vScreeningID);
            //if (dt.Rows.Count > 0)
            //{
            //    txtScreeningId.Text = Convert.ToString(dt.Rows[0]["screeningId"]);
            //    txtJocataRemarks.Text = Convert.ToString(dt.Rows[0]["remarks"]);
            //    txtScreeningStatus.Text = Convert.ToString(dt.Rows[0]["screeningStatus"]);
            //}
            txtScreeningId.Text = Convert.ToString(ViewState["ScreeningID"]); 
            txtScreeningStatus.Text = Convert.ToString(ViewState["ScreeningStatus"]);
            txtJocataRemarks.Text = Convert.ToString(ViewState["Remarks"]);
            string RiskCat= Convert.ToString(ViewState["RiskCat"]);
            ddlRiskCategory.SelectedIndex = ddlRiskCategory.Items.IndexOf(ddlRiskCategory.Items.FindByValue(RiskCat));
            txtRiskChangedDt.Text = Convert.ToString(ViewState["RiskChangedDt"]);

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
            CHouseVisit oHv = new CHouseVisit();
            int vErr = 0;
            Boolean vResult = false;
            string vMemberId = Convert.ToString(ViewState["MemberId"]);
            int vCGTID = Convert.ToInt32(ViewState["CGTId"]);
            string vLogInDt = Session[gblValue.LoginDate].ToString();

            vErr = oHv.UpdateJocataStatus(vMemberId, vCGTID, txtScreeningId.Text, ddlApprove.SelectedValue, Convert.ToInt32(Session[gblValue.UserId]), txtRemarks.Text, ddlRiskCategory.SelectedValue);
            if (vErr == 0)
            {
                if (ddlApprove.SelectedValue == "Y")
                {
                    ProsiReq pReq = new ProsiReq();
                    pReq.pMemberId = vMemberId;
                    pReq.pCreatedBy = Convert.ToString(Session[gblValue.UserId]);
                    pReq.pCGTId = Convert.ToString(vCGTID);
                    Prosidex(pReq);
                }
                if (ddlApprove.SelectedValue == "C")
                {
                    oHv = new CHouseVisit();
                    oHv.RejectGRT(gblFuction.setDate(vLogInDt), vMemberId, vCGTID, Session[gblValue.BrnchCode].ToString(), "", "IR", "Jocata Matching", Convert.ToInt32(Session[gblValue.UserId]), txtScreeningId.Text);
                }
                if (fuJocata.HasFile)
                {
                    if (fuJocata.PostedFile.ContentLength > vPathDeviationMaxLength)
                    {
                        gblFuction.AjxMsgPopup("File size is Exceeded");
                        return false;
                    }
                    else
                    {                      
                        System.IO.Directory.CreateDirectory(vPathJocata);                       
                        if (fuJocata.HasFile)
                        {
                            string vExt = Path.GetExtension(fuJocata.PostedFile.FileName);
                            fuJocata.PostedFile.SaveAs(vPathJocata + "_" + vCGTID.ToString() + vExt);
                        }
                    }
                }
                vResult = true;
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

            ddlApprove.Enabled = Status;
            txtRemarks.Enabled = Status;

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

            txtScreeningId.Enabled = false;
            txtScreeningStatus.Enabled = false;
            txtJocataRemarks.Enabled = false;
            ddlRiskCategory.Enabled = Status;
        }

        private void EnableOCRControl()
        {
            Int32 vUserId = Convert.ToInt32(Session[gblValue.UserId]);
            if (this.RoleId != 1 && vUserId != 13 && vUserId != 69 && vUserId != 1511)//CENTR - 4103
            {
                if (Convert.ToDouble(lblAddMatchScore.Text == "" ? "0" : lblAddMatchScore.Text) < 60)
                {
                    ddlCommAddrType.Enabled = true;
                    txtCommHouseNo.Enabled = true;
                    txtCommLandmark.Enabled = true;
                    txtCommSt.Enabled = true;
                    txtCommArea.Enabled = true;
                    ddlCommVill.Enabled = true;
                    txtCommSubDist.Enabled = true;
                    txtCommPost.Enabled = true;
                    txtCommMob.Enabled = true;
                    txtCommPin.Enabled = true;
                    txtCommPhone.Enabled = true;
                    ddlCommDist.Enabled = true;
                    ddlCommState.Enabled = true;

                    fuIdProof1Front.Enabled = true;
                    fuIdProof1Back.Enabled = true;
                    txtDOB.Enabled = true;
                    txtAge.Enabled = true;
                    ddlApprove.Enabled = true;
                }
                else
                {
                    ddlCommAddrType.Enabled = false;
                    txtCommHouseNo.Enabled = false;
                    txtCommLandmark.Enabled = false;
                    txtCommSt.Enabled = false;
                    txtCommArea.Enabled = false;
                    ddlCommVill.Enabled = false;
                    txtCommSubDist.Enabled = false;
                    txtCommPost.Enabled = false;
                    txtCommMob.Enabled = false;
                    txtCommPin.Enabled = false;
                    txtCommPhone.Enabled = false;
                    ddlCommDist.Enabled = false;
                    ddlCommState.Enabled = false;

                    fuIdProof1Front.Enabled = false;
                    fuIdProof1Back.Enabled = false;
                    txtDOB.Enabled = false;
                    txtAge.Enabled = false;
                    // ddlApprove.Enabled = false;                  
                }
                if (Convert.ToDouble(lblIDMatchScore.Text == "" ? "0" : lblIDMatchScore.Text) < 80)
                {
                    txtIdntPrfNo.Enabled = true;
                }
                else
                {
                    txtIdntPrfNo.Enabled = false;
                }
                fuMemPhoto.Enabled = false;
            }
            else
            {
                ddlCommAddrType.Enabled = true;
                txtCommHouseNo.Enabled = true;
                txtCommLandmark.Enabled = true;
                txtCommSt.Enabled = true;
                txtCommArea.Enabled = true;
                ddlCommVill.Enabled = true;
                txtCommSubDist.Enabled = true;
                txtCommPost.Enabled = true;
                txtCommMob.Enabled = true;
                txtCommPin.Enabled = true;
                txtCommPhone.Enabled = true;
                ddlCommDist.Enabled = true;
                ddlCommState.Enabled = true;

                fuIdProof1Front.Enabled = true;
                fuIdProof1Back.Enabled = true;
                txtDOB.Enabled = true;
                txtAge.Enabled = true;
                ddlApprove.Enabled = true;
                txtIdntPrfNo.Enabled = true;
                fuMemPhoto.Enabled = true;
                txtAddPrfNo.Enabled = true;
                ddlAddPrf.Enabled = true;
            }

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
            txtRemarks.Text = "";
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

            txtScreeningId.Text = "";
            txtScreeningStatus.Text = "";
            txtJocataRemarks.Text = "";
            ddlRiskCategory.SelectedIndex = -1;
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
            string vXml = "";
            return vXml;
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
            int Years = 0;
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
            string vXml = "";
            return vXml;
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

        #region Prosidex Integration
        public ProsidexResponse ProsidexSearchCustomer(ProsidexRequest prosidexRequest)
        {
            string vResponse = "", vUCIC = "", vRequestId = "", vMemberId = "", vPotentialYN = "N", vPotenURL = "";
            Int32 vCreatedBy = 1, vCGTID = 0;
            ProsidexResponse oProsidexResponse = null;
            CHouseVisit oHv = new CHouseVisit();
            try
            {
                vRequestId = prosidexRequest.Request.UnitySfb_RequestId;
                vMemberId = prosidexRequest.Request.DG.CUST_ID;
                vCGTID = Convert.ToInt32(prosidexRequest.Request.DG.APPLICATIONID);

                string Requestdata = JsonConvert.SerializeObject(prosidexRequest);
                string postURL = "https://ucic.unitybank.co.in:9002/UnitySfbWS/searchCustomer";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();

                dynamic res = JsonConvert.DeserializeObject(vResponse);
                if (res.Response.StatusInfo.ResponseCode == "200")
                {
                    vUCIC = res.Response.StatusInfo.POSIDEX_GENERATED_UCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, vUCIC == null ? 300 : 200);
                    vPotentialYN = vUCIC == null ? "Y" : "N";
                    vPotenURL = vUCIC == null ? res.Response.StatusInfo.CRM_URL : "";
                }
                else
                {
                    vUCIC = vUCIC == null ? "" : vUCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 500);
                }
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oHv.SaveProsidexLog(vMemberId, vCGTID, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
                //--------------------------------------------------------------------------------------                  
                return oProsidexResponse;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oHv.SaveProsidexLog(vMemberId, vCGTID, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
                //--------------------------------------------------------------------------------------
                oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 500);
            }
            finally
            {
            }
            return oProsidexResponse;
        }

        public ProsidexResponse Prosidex(ProsiReq pProsiReq)
        {
            DataTable dt = new DataTable();
            ProsidexRequest pReqData = new ProsidexRequest();
            Request pReq = new Request();
            DG pDg = new DG();
            ProsidexResponse pResponseData = null;
            CHouseVisit oHv = new CHouseVisit();
            dt = oHv.GetProsidexReqData(pProsiReq.pMemberId, pProsiReq.pCreatedBy);
            if (dt.Rows.Count > 0)
            {
                pDg.ACCOUNT_NUMBER = dt.Rows[0]["ACCOUNT_NUMBER"].ToString();
                pDg.ALIAS_NAME = dt.Rows[0]["ALIAS_NAME"].ToString();
                pDg.APPLICATIONID = pProsiReq.pCGTId;
                pDg.Aadhar = dt.Rows[0]["Aadhar"].ToString();
                pDg.CIN = dt.Rows[0]["CIN"].ToString();
                pDg.CKYC = dt.Rows[0]["CKYC"].ToString();
                pDg.CUSTOMER_STATUS = dt.Rows[0]["CUSTOMER_STATUS"].ToString();
                pDg.CUST_ID = dt.Rows[0]["CUST_ID"].ToString();
                pDg.DOB = dt.Rows[0]["DOB"].ToString();
                pDg.DrivingLicense = dt.Rows[0]["DrivingLicense"].ToString();
                pDg.Father_First_Name = dt.Rows[0]["Father_First_Name"].ToString();
                pDg.Father_Last_Name = dt.Rows[0]["Father_Last_Name"].ToString();
                pDg.Father_Middle_Name = dt.Rows[0]["Father_Middle_Name"].ToString();
                pDg.Father_Name = dt.Rows[0]["Father_Name"].ToString();
                pDg.First_Name = dt.Rows[0]["First_Name"].ToString();
                pDg.Middle_Name = dt.Rows[0]["Middle_Name"].ToString();
                pDg.Last_Name = dt.Rows[0]["Last_Name"].ToString();
                pDg.Gender = dt.Rows[0]["Gender"].ToString();
                pDg.GSTIN = dt.Rows[0]["GSTIN"].ToString();
                pDg.Lead_Id = dt.Rows[0]["Lead_Id"].ToString();
                pDg.NREGA = dt.Rows[0]["NREGA"].ToString();
                pDg.Pan = dt.Rows[0]["Pan"].ToString();
                pDg.PassportNo = dt.Rows[0]["PassportNo"].ToString();
                pDg.RELATION_TYPE = dt.Rows[0]["RELATION_TYPE"].ToString();
                pDg.RationCard = dt.Rows[0]["RationCard"].ToString();
                pDg.Registration_NO = dt.Rows[0]["Registration_NO"].ToString();
                pDg.SALUTATION = dt.Rows[0]["SALUTATION"].ToString();
                pDg.TAN = dt.Rows[0]["TAN"].ToString();
                pDg.Udyam_aadhar_number = dt.Rows[0]["Udyam_aadhar_number"].ToString();
                pDg.VoterId = dt.Rows[0]["VoterId"].ToString();
                pDg.Tasc_Customer = dt.Rows[0]["Tasc_Customer"].ToString();
                pReq.ACE = new List<object>();
                pReq.UnitySfb_RequestId = dt.Rows[0]["RequestId"].ToString();
                pReq.CUST_TYPE = "I";
                pReq.CustomerCategory = "I";
                pReq.MatchingRuleProfile = "1";
                pReq.Req_flag = "QA";
                pReq.SourceSystem = "BIJLI";
                pReq.DG = pDg;
                pReqData.Request = pReq;
            }
            pResponseData = ProsidexSearchCustomer(pReqData);
            return pResponseData;
        }
        #endregion  

        public string AsString(XmlDocument xmlDoc)
        {
            string strXmlText = null;
            if (xmlDoc != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter tx = new XmlTextWriter(sw))
                    {
                        xmlDoc.WriteTo(tx);
                        strXmlText = sw.ToString();
                    }
                }
            }
            return strXmlText;
        }
    }
}