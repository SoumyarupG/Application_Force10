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
using Newtonsoft.Json;
using System.Drawing.Imaging;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class Member : CENTRUMBase
    {
        protected int vPgNo = 1;
        protected string vMemberId = "";
        string path = "", pathMember = "", pathImage = "", pathG = "";
        string InitialBucket = ConfigurationManager.AppSettings["InitialBucket"];
        string MemberBucket = ConfigurationManager.AppSettings["MemberBucket"];
        string GroupBucket = ConfigurationManager.AppSettings["GroupBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvFamily.DataSource = null;
                gvAsset.DataSource = null;
                InitBasePage();
                StatusButton("View");
                //ViewState["CGTYN"] = "N";
                hdnApplyCgt.Value = "N";
                //txtAdmDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                ViewState["AadhaarScan"] = null;
                ViewState["OCRYN"] = null;
                GenerateGrid();
                GenerateGrid1();
                popQualification();
                PopGender();
                //popRoutineDay();
                popHouseHold();
                popEO();
                popRelation();
                popRelation1();
                popCaste();
                popOccupation();
                popVillage();
                popReligion();
                popIdentityProof();
                popAddProof();
                popCltype();
                popBank();
                popSpeciallyAbled();
                if (ddlAbledYN.SelectedValue != "Y")
                {
                    ddlSpclAbled.Enabled = false;
                    ddlSpclAbled.SelectedIndex = -1;
                }
                else
                {
                    ddlSpclAbled.Enabled = true;
                }
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    LoadList();
                    divTv.Visible = true;
                }
                else { divTv.Visible = false; }
                LoadGrid();
                btnCgtApply.Enabled = false;
                tbEmp.ActiveTabIndex = 0;
                //PopBranch(Session[gblValue.UserName].ToString());
                txtFromDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                Panel1.Visible = false;
                Panel2.Visible = false;

                //}
                btnAdd.Visible = false;
                btnEdit.Enabled = false;
                popState();
                vMemberId = Request.QueryString["id"];
                //if (vMemberId.Length > 0)
                if (vMemberId != null)
                {
                    MemberVerify(vMemberId);
                }
                popBusinessType();
                popOtherIncomeSource();
                GenerateEarningMember();
                popBusinessActivityAll();
                clearMemPhoto();
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
                this.PageHeading = "Member";
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
                    btnAprv.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnAprv.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess != "Y")
                {
                    btnAprv.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess == "Y")
                {
                    btnAprv.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess == "Y")
                {
                    btnAprv.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                    btnAprv.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                    btnAprv.Visible = false;
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
                    btnAprv.Enabled = false;
                    btnExit.Enabled = false;
                    ClearControls();
                    cbDrp.Enabled = false;
                    txtDtCl.Enabled = false;
                    ddlClTyp.Enabled = false;
                    txtRemarks.Enabled = false;
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnAprv.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnAprv.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    ddlAddrType.Enabled = false;
                    txtHouNo.Enabled = false;
                    txtStName.Enabled = false;
                    txtLandMark.Enabled = false;
                    txtArea.Enabled = false;
                    txtVillg.Enabled = false;
                    ddlMuPanc.Enabled = false;
                    ddlBlk.Enabled = false;
                    txtPin.Enabled = false;
                    txtMob.Enabled = false;
                    txtPhNo.Enabled = false;
                    txtDist.Enabled = false;
                    txtEmail.Enabled = false;
                    txtMemEmail.Enabled = false;
                    txtMemAddr.Enabled = false;

                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnAprv.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnAprv.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MemberVerify(string vMemberId)
        {
            tbEmp.ActiveTabIndex = 1;
            FillMemberDtl(vMemberId, "");
        }
        private void popEO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlRO.DataSource = dt;
                ddlRO.DataTextField = "EoName";
                ddlRO.DataValueField = "Eoid";
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRO.SelectedValue != "-1")
            {
                string vEoId = ddlRO.SelectedItem.Value;
                PopCenter(vEoId);
                ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(Convert.ToString(ViewState["CentreID"]).Trim()));
                PopGroup(ddlCenter.SelectedValue);
            }
            if (ddlRO.SelectedValue == "-1")
            {
                ddlCenter.SelectedValue = "-1";
                ddlGroup.SelectedValue = "-1";
                // ddlMetDay.SelectedValue="0";

            }
        }

        protected void ddlBRel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vRelId = ddlBRel.SelectedValue;
            if (vRelId == "11" || vRelId == "13")
            {
                ddlBRel.SelectedIndex = -1;
                ddlGuarRel.SelectedIndex = -1;
                gblFuction.AjxMsgPopup("Please select a valid relation");
                return;
            }
            else if (vRelId == "1" || vRelId == "2" || vRelId == "4" || vRelId == "5"
            || vRelId == "11" || vRelId == "8" || vRelId == "15" || vRelId == "16"
            || vRelId == "19" || vRelId == "22" || vRelId == "23" || vRelId == "25")
            {
                ddlBGend.SelectedValue = "M";
            }
            else if (vRelId == "3" || vRelId == "9" || vRelId == "7" || vRelId == "10"
            || vRelId == "13" || vRelId == "12" || vRelId == "17" ||
            vRelId == "24" || vRelId == "21" || vRelId == "18" || vRelId == "20")
            {
                ddlBGend.SelectedValue = "F";
            }
            else if (vRelId == "6")
            {
                if (ddlGend.SelectedValue == "F")
                {
                    ddlBGend.SelectedValue = "M";
                }
                else if (ddlGend.SelectedValue == "M")
                {
                    ddlBGend.SelectedValue = "F";
                }
            }
            ddlGuarRel.SelectedIndex = ddlGuarRel.Items.IndexOf(ddlGuarRel.Items.FindByValue(ddlBRel.SelectedValue));
            ddlGuarGen.SelectedIndex = ddlGuarGen.Items.IndexOf(ddlGuarGen.Items.FindByValue(ddlBGend.SelectedValue));
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                // dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                dt = oUsr.GetBranchByUserHO(pUser, Convert.ToInt32(Session[gblValue.RoleId]));

                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
                else
                {
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
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
                    ddlCenter.DataSource = dt;
                    ddlCenter.DataTextField = "Market";
                    ddlCenter.DataValueField = "MarketID";
                    ddlCenter.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlCenter.Items.Insert(0, oli);

                    ViewState["CentreID"] = Convert.ToString(dt.Rows[0]["Marketid"]).Trim();
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

            string vGroupId = ddlGroup.SelectedValue;
            if (ddlGroup.SelectedValue != "-1")
            {
                PopCollDay(vGroupId);
            }

            if (ddlGroup.SelectedValue == "-1")
            {
                // ddlMetDay.SelectedValue ="0";
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
            DateTime vAdmDt = gblFuction.setDate(txtAdmDt.Text);
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
                    //ddlMetDay.SelectedIndex = ddlMetDay.Items.IndexOf(ddlMetDay.Items.FindByValue(dt.Rows[0]["CollDay"].ToString()));
                }
            }
            finally
            {
                dt = null;
                oCent = null;
            }
        }
        private void PopCollDay(string vGroupId)
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vAdmDt = gblFuction.setDate(txtAdmDt.Text);
            CCenter oCent = null;
            try
            {
                oCent = new CCenter();
                dt = oCent.GetCollDayByGroupId(vGroupId, vBrCode, vAdmDt);
                if (dt.Rows.Count > 0)
                {
                    //ddlGroup.DataSource = dt;
                    //ddlGroup.DataTextField = "GroupName";
                    //ddlGroup.DataValueField = "Groupid";
                    //ddlGroup.DataBind();
                    //ListItem oli = new ListItem("<--Select-->", "-1");
                    //ddlGroup.Items.Insert(0, oli);
                    // popRoutineDay();
                    txtMetDay.Text = dt.Rows[0]["CollDay"].ToString();
                    //ddlMetDay.SelectedIndex = ddlMetDay.Items.IndexOf(ddlMetDay.Items.FindByValue(dt.Rows[0]["CollDay"].ToString()));
                }
            }
            finally
            {
                dt = null;
                oCent = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popQualification()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "QualificationId", "QualificationName", "QualificationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlEduc.DataSource = dt;
                ddlEduc.DataTextField = "QualificationName";
                ddlEduc.DataValueField = "QualificationId";
                ddlEduc.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlEduc.Items.Insert(0, oli);
                ddlBEdu.DataSource = dt;
                ddlBEdu.DataTextField = "QualificationName";
                ddlBEdu.DataValueField = "QualificationId";
                ddlBEdu.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlBEdu.Items.Insert(0, oli1);
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
        private void popOccupation()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "OccupationId", "OccupationName", "OccupationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlOcup.DataSource = dt;
                ddlOcup.DataTextField = "OccupationName";
                ddlOcup.DataValueField = "OccupationId";
                ddlOcup.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlOcup.Items.Insert(0, oli);
                ddlBOcup.DataSource = dt;
                ddlBOcup.DataTextField = "OccupationName";
                ddlBOcup.DataValueField = "OccupationId";
                ddlBOcup.DataBind();
                ListItem ol1 = new ListItem("<--Select-->", "-1");
                ddlBOcup.Items.Insert(0, ol1);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        protected void ddlAbledYN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAbledYN.SelectedValue != "Y")
            {
                ddlSpclAbled.Enabled = false;
                ddlSpclAbled.SelectedIndex = -1;
            }
            else
            {
                ddlSpclAbled.Enabled = true;
            }
        }

        private void popSpeciallyAbled()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            oGb = new CGblIdGenerator();
            dt = oGb.GetSpclAble();

            ddlSpclAbled.DataSource = dt;
            ddlSpclAbled.DataTextField = "Question";
            ddlSpclAbled.DataValueField = "Qno";
            ddlSpclAbled.DataBind();
            ListItem oli = new ListItem("<--Select-->", "-1");
            ddlSpclAbled.Items.Insert(0, oli);

        }
        /// <summary>
        /// 
        /// </summary>
        private void popRelation()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "HumanRelationId", "HumanRelationName", "HumanRelationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlBRel.DataSource = dt;
                ddlBRel.DataTextField = "HumanRelationName";
                ddlBRel.DataValueField = "HumanRelationId";
                ddlBRel.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBRel.Items.Insert(0, oli);

                ddlGuarRel.DataSource = dt;
                ddlGuarRel.DataTextField = "HumanRelationName";
                ddlGuarRel.DataValueField = "HumanRelationId";
                ddlGuarRel.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlGuarRel.Items.Insert(0, oli1);


                foreach (DataRow dR in dt.Rows)
                {
                    int vRelationId = Convert.ToInt32(dR["HumanRelationId"]);
                    if (vRelationId != 4 && vRelationId != 25 && vRelationId != 7)
                    {
                        dR.Delete();
                    }
                }
                dt.AcceptChanges();

                ddlMHFRelation.DataSource = dt;
                ddlMHFRelation.DataTextField = "HumanRelationName";
                ddlMHFRelation.DataValueField = "HumanRelationId";
                ddlMHFRelation.DataBind();
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ddlMHFRelation.Items.Insert(0, oli2);

            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        private void popRelation1()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                // dt = oGb.PopComboMIS("N", "N", "AA", "HumanRelationId", "HumanRelationName", "HumanRelationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                //ddlReltFam.DataSource = dt;
                //ddlBRel.DataTextField = "HumanRelationName";
                //ddlBRel.DataValueField = "HumanRelationId";
                //ddlBRel.DataBind();
                //ListItem oli = new ListItem("<--Select-->", "-1");
                //ddlBRel.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popBank()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                //dt = oGb.PopComboMIS("N", "N", "AA", "BankId", "BankName", "BankMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                //ddlBankName.DataSource = dt;
                //ddlBankName.DataTextField = "BankName";
                //ddlBankName.DataValueField = "BankId";
                //ddlBankName.DataBind();
                //ListItem oli = new ListItem("<--Select-->", "-1");
                //ddlBankName.Items.Insert(0, oli);
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
        private void popVillage()
        {
            DataTable dt = null;
            CVillage oGb = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oGb = new CVillage();
                dt = oGb.PopVillage(vBrCode);
                //ddlVillg.DataSource = dt;
                //ddlVillg.DataTextField = "VillageName";
                //ddlVillg.DataValueField = "VillageId";
                //ddlVillg.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                //ddlVillg.Items.Insert(0, oli);
                ddlBVillg.DataSource = dt;
                ddlBVillg.DataTextField = "VillageName";
                ddlBVillg.DataValueField = "VillageId";
                ddlBVillg.DataBind();
                ListItem ol1 = new ListItem("<--Select-->", "-1");
                ddlBVillg.Items.Insert(0, ol1);

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
                // ddlDist.DataTextField = "DistrictName";
                //ddlDist.DataValueField = "DistrictId";
                //ddlDist.DataBind();
                ddlStat.DataTextField = "StateName";
                ddlStat.DataValueField = "StateId";
                ddlStat.DataBind();
                ddlMuPanc.Enabled = false;
                ddlBlk.Enabled = false;
                //ddlDist.Enabled = false;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBVillg_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopAllAgainstVillage();
        }
        private void PopAllAgainstVillage()
        {
            DataTable dt = null;
            CVillage oVlg = null;
            string vVlgId = ddlBVillg.SelectedValue;
            try
            {
                oVlg = new CVillage();
                dt = oVlg.GetGpBlkDistStateList(vVlgId);
                ddlBMunPanca.DataSource = ddlBBlk.DataSource = ddlBDist.DataSource = ddlBStat.DataSource = dt;
                ddlBMunPanca.DataTextField = "GPName";
                ddlBMunPanca.DataValueField = "GPId";
                ddlBMunPanca.DataBind();
                ddlBBlk.DataTextField = "BlockName";
                ddlBBlk.DataValueField = "BlockId";
                ddlBBlk.DataBind();
                ddlBDist.DataTextField = "DistrictName";
                ddlBDist.DataValueField = "DistrictId";
                ddlBDist.DataBind();
                ddlBStat.DataTextField = "StateName";
                ddlBStat.DataValueField = "StateId";
                ddlBStat.DataBind();
                ddlBMunPanca.Enabled = false;
                ddlBBlk.Enabled = false;
                ddlBDist.Enabled = false;
                ddlBStat.Enabled = false;
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
        private void popReligion()
        {
            //Dictionary<string, string> oDic = new Dictionary<string, string>();
            //oDic.Add("<-Select->", "0");
            //oDic.Add("Buddhist", "R05");
            //oDic.Add("Christian", "R03");
            //oDic.Add("Hindu", "R01");
            //oDic.Add("Jain", "R06");
            //oDic.Add("Muslim", "R02");
            //oDic.Add("Others", "R07");
            //oDic.Add("Sikh", "R04");
            //oDic.Add("Jewish", "R08");
            //oDic.Add("Parsi", "R09");
            //ddlRelg.DataSource = oDic;
            //ddlRelg.DataValueField = "value";
            //ddlRelg.DataTextField = "key";
            //ddlRelg.DataBind();
            //ddlBRelg.DataSource = oDic;
            //ddlBRelg.DataValueField = "value";
            //ddlBRelg.DataTextField = "key";
            //ddlBRelg.DataBind();
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            ListItem oli = new ListItem("<--Select-->", "-1");
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "ReligionId", "Religion", "ReligionMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlRelg.DataSource = dt;
                ddlRelg.DataTextField = "Religion";
                ddlRelg.DataValueField = "ReligionId";
                ddlRelg.DataBind();
                ddlRelg.Items.Insert(0, oli);

                ddlBRelg.DataSource = dt;
                ddlBRelg.DataTextField = "Religion";
                ddlBRelg.DataValueField = "ReligionId";
                ddlBRelg.DataBind();
                ddlBRelg.Items.Insert(0, oli);
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
        private void popAddProof()
        {
            DataTable dt = null;
            CNewMember oNM = null;
            try
            {
                oNM = new CNewMember();
                dt = oNM.popIdAddrProof("Y", "N");

                ddlBIdntyProf.DataSource = dt;
                ddlBIdntyProf.DataTextField = "IDProofName";
                ddlBIdntyProf.DataValueField = "IDProofId";
                ddlBIdntyProf.DataBind();
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ddlBIdntyProf.Items.Insert(0, oli3);

                ddlBAddProf.DataSource = dt;
                ddlBAddProf.DataTextField = "IDProofName";
                ddlBAddProf.DataValueField = "IDProofId";
                ddlBAddProf.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBAddProf.Items.Insert(0, oli);

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

                //ddlBIdntyProf.DataSource = dt;
                //ddlBIdntyProf.DataTextField = "IDProofName";
                //ddlBIdntyProf.DataValueField = "IDProofId";
                //ddlBIdntyProf.DataBind();
                //ListItem oli3 = new ListItem("<--Select-->", "-1");
                //ddlBIdntyProf.Items.Insert(0, oli3);
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
                txtCommEmail.Text = txtEmail.Text;
                //ddlCommVill.SelectedIndex = ddlVillg.SelectedIndex;
                if (ddlBVillg.SelectedIndex >= 0)
                {
                    popAgainstVillage2();
                    ddlCommMuni.SelectedIndex = ddlMuPanc.SelectedIndex;
                    ddlCommBlock.SelectedIndex = ddlBlk.SelectedIndex;
                    //ddlCommDist.SelectedIndex = ddlDist.SelectedIndex;
                    //ddlCommState.SelectedIndex = ddlStat.SelectedIndex;
                    txtMemCommAddr.Text = txtCommHouseNo.Text + "," + txtCommSt.Text + "," + ddlCommVill.SelectedItem.Text.ToString() + "," + ddlCommMuni.SelectedItem.Text.ToString() + "," + ddlCommBlock.SelectedItem.Text.ToString() + "," + txtCommSubDist.Text + "," + txtCommPost.Text + "," + txtCommPin.Text;

                }
                else
                {
                }
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
            SetMinority();
        }

        /// <summary>
        /// 
        /// </summary>        


        /// <summary>
        /// 
        /// </summary>
        private void popCaste()
        {
            //Dictionary<string, Int32> oDic = new Dictionary<string, Int32>();
            //oDic.Add("<-Select->", 0);
            //oDic.Add("General", 1);
            //oDic.Add("SC", 2);
            //oDic.Add("ST", 3);
            //oDic.Add("OBC", 4);
            //oDic.Add("Minority", 5);
            //oDic.Add("Other", 6);
            //ddlCaste.DataSource = oDic;
            //ddlCaste.DataValueField = "value";
            //ddlCaste.DataTextField = "key";
            //ddlCaste.DataBind();
            //ddlBCaste.DataSource = oDic;
            //ddlBCaste.DataValueField = "value";
            //ddlBCaste.DataTextField = "key";
            //ddlBCaste.DataBind();

            DataTable dt = null;
            CGblIdGenerator oGb = null;
            ListItem oli = new ListItem("<--Select-->", "-1");
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "CasteId", "Caste", "CasteMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlCaste.DataSource = dt;
                ddlCaste.DataTextField = "Caste";
                ddlCaste.DataValueField = "CasteId";
                ddlCaste.DataBind();
                ddlCaste.Items.Insert(0, oli);

                ddlBCaste.DataSource = dt;
                ddlBCaste.DataTextField = "Caste";
                ddlBCaste.DataValueField = "CasteId";
                ddlBCaste.DataBind();
                ddlBCaste.Items.Insert(0, oli);
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
        private void popHouseHold()
        {
            Dictionary<string, Int32> oDic = new Dictionary<string, Int32>();
            oDic.Add("<-Select->", 0);
            oDic.Add("Rural", 1);
            oDic.Add("Semi Urban", 2);
            oDic.Add("Urban", 3);
            ddlHHoldTyp.DataSource = oDic;
            ddlHHoldTyp.DataValueField = "value";
            ddlHHoldTyp.DataTextField = "key";
            ddlHHoldTyp.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopGender()
        {
            Dictionary<string, string> oGen = new Dictionary<string, string>();
            oGen.Add("<-Select->", "-1");
            oGen.Add("Female", "F");
            oGen.Add("Transgender", "O");
            oGen.Add("Third Gender", "T");

            ddlGend.DataSource = oGen;
            ddlGend.DataValueField = "value";
            ddlGend.DataTextField = "key";
            ddlGend.DataBind();

            Dictionary<string, string> oFGen = new Dictionary<string, string>();
            oFGen.Add("<-Select->", "-1");
            oFGen.Add("Male", "M");
            oFGen.Add("Female", "F");
            oFGen.Add("Transgender", "O");
            oFGen.Add("Third Gender", "T");

            ddlBGend.DataSource = oFGen;
            ddlBGend.DataValueField = "value";
            ddlBGend.DataTextField = "key";
            ddlBGend.DataBind();

            ddlGuarGen.DataSource = oFGen;
            ddlGuarGen.DataValueField = "value";
            ddlGuarGen.DataTextField = "key";
            ddlGuarGen.DataBind();
        }

        private void SetMinority()
        {
            Int32 CommState = Convert.ToInt32(ddlCommState.SelectedValue);
            string Religion = ddlRelg.SelectedValue;

            if ((CommState == 11 && Religion == "R02") || (CommState == 18 && Religion == "R03") || (CommState == 19 && Religion == "R03")
                || (CommState == 20 && Religion == "R03") || (CommState == 22 && Religion == "R04") || Religion == "R01" || Religion == "R10")
            {
                ddlMinority.SelectedIndex = ddlMinority.Items.IndexOf(ddlMinority.Items.FindByValue("N"));
            }
            else
            {
                ddlMinority.SelectedIndex = ddlMinority.Items.IndexOf(ddlMinority.Items.FindByValue("Y"));
            }
        }
        protected void ddlRelg_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMinority();
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
            btnCgtApply.Enabled = false;
            tbEmp.ActiveTabIndex = 1;
            //pnlFamDtl.ActiveTabIndex = 1;
            GenerateGrid();
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
            CRole oRl = null;
            DataTable dt = null;
            Int32 vRoleId = Convert.ToInt32(Session[gblValue.RoleId].ToString());
            int vRet = 0;
            string vMemId = Convert.ToString(ViewState["MemId"]);
            if (this.CanEdit == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Edit);
                return;
            }
            btnCgtApply.Enabled = false;
            tbEmp.ActiveTabIndex = 1;
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
            gvEarningMember.Enabled = false;
            //this.GetModuleByRole(mnuID.mnuMemberMst);
            oRl = new CRole();
            dt = oRl.GetRoleById(vRoleId);
            //if (this.RoleId != 1)  //&& this.RoleId != 5 && this.RoleId != 10 && this.RoleId != 25 && this.RoleId != 13
            //{
            oMem = new CMember();
            vRet = oMem.ChkMemEdit(vMemId, vLogDt);
            if (vRet == 0)
            {
                EnableControl(false);
                //txtCustId.Enabled = true;
                //txtSavingsAcNo.Enabled = true;
                btnSave.Enabled = false;
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["DropMember"]) == "N")
                    {
                        cbDrp.Enabled = false;
                        ddlClTyp.Enabled = false;
                        txtDtCl.Enabled = false;
                    }
                }
                else
                {
                    cbDrp.Enabled = false;
                    ddlClTyp.Enabled = false;
                    txtDtCl.Enabled = false;
                }
            }
            if (Convert.ToString(ViewState["AadhaarScan"]) == "A")
            {
                if (ddlIdentyProf.SelectedValue == "1")
                {
                    ddlIdentyProf.Enabled = false;
                    txtIdntPrfNo.Enabled = false;
                }
            }
            if (Convert.ToInt32(Session[gblValue.UserId]) != 1)
            {
                if (Convert.ToString(ViewState["OCRYN"]) == "Y")
                {
                    //ddlAreaCategory.Enabled = false;
                    ddlAddPrf.Enabled = false;
                    ddlIdentyProf.Enabled = false;
                    txtIdntPrfNo.Enabled = false;
                    txtAddPrfNo.Enabled = false;

                    txtMetDay.Enabled = false;
                    ddlGroup.Enabled = false;
                    ddlCenter.Enabled = false;
                    ddlRO.Enabled = false;

                    // txtFName.Enabled = false;
                    //  txtMName.Enabled = false;
                    //txtLName.Enabled = false;
                    txtHFName.Enabled = false;
                    chkFath.Enabled = false;
                    txtMob.Enabled = false;
                    // txtDOB.Enabled = false;
                    // txtAge.Enabled = false;

                    //txtHouNo.Enabled = false;
                    //txtStName.Enabled = false;
                    //txtWardNo.Enabled = false;
                    //txtPOff.Enabled = false;
                    //txtPin.Enabled = false;

                    //txtMob.Enabled = false;
                    //txtLandMark.Enabled = false;
                    //txtArea.Enabled = false;
                    //txtVillg.Enabled = false;
                    //ddlStat.Enabled = false;
                    //txtDist.Enabled = false;
                    //txtMemAddr.Enabled = false;
                }
                //txtFName.Enabled = false;
                //txtMName.Enabled = false;
                //txtLName.Enabled = false;
                //}
                //EnableCGT();


                //if (cbDrp.Checked == true)
                //{
                //    if (this.RoleId != 1) //&& this.RoleId != 10
                //    {
                //        cbDrp.Enabled = false;
                //        ddlClTyp.Enabled = false;
                //        txtDtCl.Enabled = false;
                //    }
                //}
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCgtApply_Click(object sender, EventArgs e)
        {

            hdnApplyCgt.Value = "Y";

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAprv_Click(object sender, EventArgs e)
        {
            string vMemId = string.Empty;
            CMember oMember = null;
            Int32 vRet = 0;
            try
            {
                vMemId = Convert.ToString(ViewState["MemId"]);
                oMember = new CMember();
                vRet = oMember.ApproveMember(vMemId, "Y");
                if (vRet > 0)
                {
                    gblFuction.MsgPopup("Member Approved Successfully.");
                    btnAprv.Enabled = false;
                }
                else
                    gblFuction.MsgPopup(gblMarg.DBError);
            }
            finally
            {
                oMember = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbEmp.ActiveTabIndex = 0;
            //ClearControls();
            //EnableControl(false);
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
                LoadList();
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
            //dt2 = ds.Tables[1];


            if (ddlBGend.SelectedValue == "F" && ddlBRel.SelectedItem.Text == "HUSBAND")
            {
                gblFuction.MsgPopup("Please Husband cannot be female");
                vRst = false;
            }

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
            //if (Convert.ToString(ViewState["StateEdit"]) == "Edit")
            //{
            //    if (Convert.ToInt32(Session[gblValue.StateID].ToString()) != 3)
            //    {
            //        if (ddlAddPrf.SelectedValue != "4" && ddlIdentyProf.SelectedValue != "4")
            //        {
            //            gblFuction.MsgPopup("Adhar Card is mandatory for your Address Proof or Identity Proof");
            //            vRst = false;
            //        }
            //    }
            //}            


            return vRst;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadList()
        {
            DataTable dtRoot = null;
            TreeNode tnRoot = null, tnGrp = null;
            string vEoId = "";
            tvMem.Nodes.Clear();
            CEO oRO = null;

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dtRoot = oRO.PopRO(Session[gblValue.BrnchCode].ToString(), "0", "0", vLogDt, this.UserID);
                foreach (DataRow dr in dtRoot.Rows)
                {
                    tnRoot = new TreeNode(dr["EoName"].ToString());
                    tnRoot.PopulateOnDemand = false;
                    tnRoot.SelectAction = TreeNodeSelectAction.None;
                    tvMem.Nodes.Add(tnRoot);
                    tnRoot.Value = Convert.ToString("CM" + dr["Eoid"]);
                    vEoId = Convert.ToString(dr["Eoid"]);
                    tnGrp = new TreeNode("No Record");
                    tnGrp.Value = "CM";
                    tnRoot.ChildNodes.Add(tnGrp);
                    tnRoot.CollapseAll();
                }
            }
            finally
            {
                dtRoot = null;
                oRO = null;
            }
        }

        private void LoadTree()
        {
            DataTable dtRoot = null;
            TreeNode tnRoot = null, tnGrp = null;
            string vEoId = "";
            tvMem.Nodes.Clear();
            CEO oRO = null;
            string vBrCode = ddlBranch.SelectedValue;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dtRoot = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                foreach (DataRow dr in dtRoot.Rows)
                {
                    tnRoot = new TreeNode(dr["EoName"].ToString());
                    tnRoot.PopulateOnDemand = false;
                    tnRoot.SelectAction = TreeNodeSelectAction.None;
                    tvMem.Nodes.Add(tnRoot);
                    tnRoot.Value = Convert.ToString("CM" + dr["Eoid"]);
                    vEoId = Convert.ToString(dr["Eoid"]);
                    tnGrp = new TreeNode("No Record");
                    tnGrp.Value = "CM";
                    tnRoot.ChildNodes.Add(tnGrp);
                    tnRoot.CollapseAll();
                }
            }
            finally
            {
                dtRoot = null;
                oRO = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGrid()
        {
            DataTable dt1 = null;
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBranchCode = ddlBranch.SelectedValue;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DateTime vFrmDt = gblFuction.setDate(txtFromDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vMode = rdbOpt.SelectedValue;
            try
            {
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                dt1 = oMem.LoadMemDtl(vBrCode, vLogDt, this.UserID, txtSearch.Text.Trim());
                //}
                //else
                //{
                //    dt1 = oMem.LoadMemDtl_KYCVERI(vBranchCode, vLogDt, this.UserID, txtSearch.Text.Trim(), vFrmDt, vToDt, vMode);
                //}
                gvMemApp.DataSource = dt1;
                gvMemApp.DataBind();



            }
            finally
            {
                dt1 = null;
                oMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMemApp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vMemID = "", BrCode = "";
            vMemID = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                foreach (GridViewRow gr in gvMemApp.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                BrCode = gvRow.Cells[8].Text;
                btnShow.ForeColor = System.Drawing.Color.Red;
            }
            FillMemberDtl(vMemID, BrCode);
            //txtConfrmAadhar1.Text = "";
            //txtConfrmAadhar2.Text = "";
            //txtConfrmAadhar3.Text = "";
            //txtConfrmAadhar1.Visible = false;
            //txtConfrmAadhar2.Visible = false;
            //txtConfrmAadhar3.Visible = false;
            //lblId1.Visible = false;
            //lblId2.Visible = false;
            //lblId3.Visible = false;
            ddlBRel_SelectedIndexChanged(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vMemberId"></param>
        private void FillMemberDtl(string vMemberId, string vBranchCode)
        {
            string vStat = "";
            ViewState["MemId"] = "";
            ViewState["LoanAppId"] = "";
            lblAadhar.Value = "";
            lblBAadhar.Value = "";
            DataSet ds = null;
            DataTable dt = null, dt2 = null, dt3 = null, dt4 = null, dt5 = null, dt6 = null;
            CMember oMem = null;
            try
            {
                ViewState["MemId"] = vMemberId;
                oMem = new CMember();
                ds = oMem.GetMemberDetails(vMemberId, "", vBranchCode, Convert.ToInt32(Session[gblValue.UserId]));
                dt = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                dt4 = ds.Tables[3];
                dt5 = ds.Tables[4];
                dt6 = ds.Tables[5];

                if (dt.Rows.Count > 0)
                {
                    //string pReqData = "{\"pId\":\"" + vMemberId + "\"}";
                    //GenerateReport("GetPassBookImage", pReqData);
                    txtMemID.Text = Convert.ToString(dt.Rows[0]["MemberID"]);
                    txtAdmDt.Text = Convert.ToString(dt.Rows[0]["AdmDate"]);
                    txtFName.Text = Convert.ToString(dt.Rows[0]["MF_Name"]);
                    txtMName.Text = Convert.ToString(dt.Rows[0]["MM_Name"]);
                    txtLName.Text = Convert.ToString(dt.Rows[0]["ML_Name"]);

                    txtFirstName.Text = Convert.ToString(dt.Rows[0]["MF_Name"]);
                    txtMiddleName.Text = Convert.ToString(dt.Rows[0]["MM_Name"]);
                    txtLastName.Text = Convert.ToString(dt.Rows[0]["ML_Name"]);

                    txtHFName.Text = Convert.ToString(dt.Rows[0]["MHF_Name"]);
                    vStat = Convert.ToString(dt.Rows[0]["Father_YN"]);
                    if (vStat == "Y")
                        chkFath.Checked = true;
                    else
                        chkFath.Checked = false;
                    txtDOB.Text = Convert.ToString(dt.Rows[0]["M_DOB"]);
                    txtAge.Text = Convert.ToString(dt.Rows[0]["M_Age"]);

                    txtMDOB.Text = Convert.ToString(dt.Rows[0]["M_DOB"]);
                    txtMAge.Text = Convert.ToString(dt.Rows[0]["M_Age"]);

                    txtHouNo.Text = Convert.ToString(dt.Rows[0]["M_HouseNo"]);
                    txtStName.Text = Convert.ToString(dt.Rows[0]["M_Street"]);
                    txtWardNo.Text = Convert.ToString(dt.Rows[0]["M_WardNo"]);
                    txtPOff.Text = Convert.ToString(dt.Rows[0]["M_PostOff"]);
                    txtLandMark.Text = Convert.ToString(dt.Rows[0]["Landmark"]);
                    txtArea.Text = Convert.ToString(dt.Rows[0]["Area"]);
                    txtPin.Text = Convert.ToString(dt.Rows[0]["M_PIN"]);
                    txtMemAddr.Text = Convert.ToString(dt.Rows[0]["MemAddr"]);
                    txtMob.Text = Convert.ToString(dt.Rows[0]["M_Mobile"]);
                    txtPhNo.Text = Convert.ToString(dt.Rows[0]["M_Phone"]);
                    txtCustId.Text = Convert.ToString(dt.Rows[0]["IDBICustId"]);
                    txtSavingsAcNo.Text = Convert.ToString(dt.Rows[0]["IDBISavingsAcNo"]);

                    txtCommHouseNo.Text = Convert.ToString(dt.Rows[0]["HouseNo_p"]);
                    txtCommSt.Text = Convert.ToString(dt.Rows[0]["Street_p"]);
                    txtCommSubDist.Text = Convert.ToString(dt.Rows[0]["WardNo_p"]);
                    txtCommPost.Text = Convert.ToString(dt.Rows[0]["PostOff_p"]);
                    txtCommPin.Text = Convert.ToString(dt.Rows[0]["PIN_p"]);
                    txtCommMob.Text = Convert.ToString(dt.Rows[0]["MobileNo_p"]);
                    txtCommLandmark.Text = Convert.ToString(dt.Rows[0]["Landmark_p"]);
                    txtCommArea.Text = Convert.ToString(dt.Rows[0]["Area_p"]);
                    txtCommEmail.Text = Convert.ToString(dt.Rows[0]["EmailId_p"]);
                    txtStayYear.Text = Convert.ToString(dt.Rows[0]["YearsOfStay"]);
                    hdRBILogic.Value = Convert.ToString(dt.Rows[0]["RBILogic"]);

                    ddlAbledYN.SelectedIndex = ddlAbledYN.Items.IndexOf(ddlAbledYN.Items.FindByValue(dt.Rows[0]["IsAbledYN"].ToString()));
                    ddlSpclAbled.SelectedIndex = ddlSpclAbled.Items.IndexOf(ddlSpclAbled.Items.FindByValue(dt.Rows[0]["SpeciallyAbled"].ToString()));

                    if (Convert.ToInt32(Session[gblValue.RoleId]) == 1)
                    {
                        txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["M_AddProfNo"]);
                        txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]);
                        txtIdProof3.Text = Convert.ToString(dt.Rows[0]["AddProfNo2"]);
                        txtBAddPrfNo.Text = Convert.ToString(dt.Rows[0]["B_AddProfNo"]);
                        txtBIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["B_IdentyProfNo"]);

                        if (dt.Rows[0]["M_IdentyPRofId"].ToString() == "1")
                        {
                            lblAadhar.Value = Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]);
                        }
                        if (dt.Rows[0]["M_AddProfId"].ToString() == "1")
                        {
                            lblAadhar.Value = Convert.ToString(dt.Rows[0]["M_AddProfNo"]);
                        }
                        if (dt.Rows[0]["AddProfId2"].ToString() == "1")
                        {
                            lblAadhar.Value = Convert.ToString(dt.Rows[0]["AddProfNo2"]);
                        }

                        if (dt.Rows[0]["B_IdentyProfId"].ToString() == "1")
                        {
                            lblBAadhar.Value = Convert.ToString(dt.Rows[0]["B_IdentyProfNo"]);
                        }
                        else if (dt.Rows[0]["B_AddProfId"].ToString() == "1")
                        {
                            lblBAadhar.Value = Convert.ToString(dt.Rows[0]["B_AddProfNo"]);
                        }

                    }
                    else
                    {
                        if (dt5.Rows[0]["ViewAadhar"].ToString() == "N" && Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                        {
                            if (dt.Rows[0]["M_IdentyPRofId"].ToString() == "1")
                            {
                                lblAadhar.Value = Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]);
                                txtIdntPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]).Substring(Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]).Length - 4, 4));
                            }
                            else
                            {
                                txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]);
                            }

                            if (dt.Rows[0]["M_AddProfId"].ToString() == "1")
                            {
                                lblAadhar.Value = Convert.ToString(dt.Rows[0]["M_AddProfNo"]);
                                txtAddPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["M_AddProfNo"]).Substring(Convert.ToString(dt.Rows[0]["M_AddProfNo"]).Length - 4, 4));
                            }
                            else
                            {
                                txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["M_AddProfNo"]);
                            }

                            if (dt.Rows[0]["AddProfId2"].ToString() == "1")
                            {
                                lblAadhar.Value = Convert.ToString(dt.Rows[0]["AddProfNo2"]);
                                txtIdProof3.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["AddProfNo2"]).Substring(Convert.ToString(dt.Rows[0]["AddProfNo2"]).Length - 4, 4));
                            }
                            else
                            {
                                txtIdProof3.Text = Convert.ToString(dt.Rows[0]["AddProfNo2"]);
                            }

                            if (dt.Rows[0]["B_IdentyProfId"].ToString() == "1")
                            {
                                lblBAadhar.Value = Convert.ToString(dt.Rows[0]["B_IdentyProfNo"]);
                                txtBIdntPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["B_IdentyProfNo"]).Substring(Convert.ToString(dt.Rows[0]["B_IdentyProfNo"]).Length - 4, 4));
                            }
                            else
                            {
                                txtBIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["B_IdentyProfNo"]);
                            }

                            if (dt.Rows[0]["B_AddProfId"].ToString() == "1")
                            {
                                lblBAadhar.Value = Convert.ToString(dt.Rows[0]["B_AddProfNo"]);
                                txtBAddPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["B_AddProfNo"]).Substring(Convert.ToString(dt.Rows[0]["B_AddProfNo"]).Length - 4, 4));
                            }
                            else
                            {
                                txtBAddPrfNo.Text = Convert.ToString(dt.Rows[0]["B_AddProfNo"]);
                            }
                        }
                        else
                        {
                            if (dt.Rows[0]["M_IdentyPRofId"].ToString() == "1")
                            {
                                lblAadhar.Value = Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]);
                            }
                            else if (dt.Rows[0]["M_AddProfId"].ToString() == "1")
                            {
                                lblAadhar.Value = Convert.ToString(dt.Rows[0]["M_AddProfNo"]);
                            }
                            else if (dt.Rows[0]["AddProfId2"].ToString() == "1")
                            {
                                lblAadhar.Value = Convert.ToString(dt.Rows[0]["AddProfNo2"]);
                            }

                            if (dt.Rows[0]["B_IdentyProfId"].ToString() == "1")
                            {
                                lblBAadhar.Value = Convert.ToString(dt.Rows[0]["B_IdentyProfNo"]);
                            }
                            else if (dt.Rows[0]["B_AddProfId"].ToString() == "1")
                            {
                                lblBAadhar.Value = Convert.ToString(dt.Rows[0]["B_AddProfNo"]);
                            }

                            txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["M_AddProfNo"]);
                            txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]);
                            txtIdProof3.Text = Convert.ToString(dt.Rows[0]["AddProfNo2"]);
                            txtBAddPrfNo.Text = Convert.ToString(dt.Rows[0]["B_AddProfNo"]);
                            txtBIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["B_IdentyProfNo"]);
                        }
                    }

                    txtBFName.Text = Convert.ToString(dt.Rows[0]["B_FName"]);
                    txtBMName.Text = Convert.ToString(dt.Rows[0]["B_MName"]);
                    txtBLName.Text = Convert.ToString(dt.Rows[0]["B_LName"]);
                    txtBDOBDt.Text = Convert.ToString(dt.Rows[0]["B_DOB"]);
                    txtBAge.Text = Convert.ToString(dt.Rows[0]["B_Age"]);
                    txtBHouNo.Text = Convert.ToString(dt.Rows[0]["B_HouseNo"]);
                    txtBStreet.Text = Convert.ToString(dt.Rows[0]["B_Street"]);
                    txtBWardNo.Text = Convert.ToString(dt.Rows[0]["B_WardNo"]);
                    txtBPOff.Text = Convert.ToString(dt.Rows[0]["B_PostOff"]);
                    txtBPin.Text = Convert.ToString(dt.Rows[0]["B_PIN"]);
                    txtCoBrwrAddr.Text = Convert.ToString(dt.Rows[0]["CoBrwrAddr"]);
                    txtBMobNo.Text = Convert.ToString(dt.Rows[0]["B_Mobile"]);
                    txtBPhNo.Text = Convert.ToString(dt.Rows[0]["B_Phone"]);
                    txtBLandmark.Text = Convert.ToString(dt.Rows[0]["B_Landmark"]);
                    txtBArea.Text = Convert.ToString(dt.Rows[0]["B_Area"]);
                    txtBEmail.Text = Convert.ToString(dt.Rows[0]["B_Email"]);

                    txtBVillage.Text = Convert.ToString(dt.Rows[0]["B_Village"]);
                    txtBDist.Text = Convert.ToString(dt.Rows[0]["B_District"]);

                    ddlBStat.DataSource = dt;
                    ddlBStat.DataTextField = "B_StateName";
                    ddlBStat.DataValueField = "B_StateId";
                    ddlBStat.DataBind();

                    txtIncome.Text = Convert.ToString(dt.Rows[0]["IncAmt"]);
                    txtExpnses.Text = Convert.ToString(dt.Rows[0]["ExpAmt"]);
                    txtMFIName.Text = Convert.ToString(dt.Rows[0]["MFI_1"]);
                    txtLnAmt.Text = Convert.ToString(dt.Rows[0]["LoanAmt_1"]);
                    if (dt.Rows[0]["LoanDt_1"].ToString() != "01/01/1900")
                        txtLnDt.Text = Convert.ToString(dt.Rows[0]["LoanDt_1"]);
                    txtInstAmt.Text = Convert.ToString(dt.Rows[0]["InstAmt_1"]);
                    txtInstLeft.Text = Convert.ToString(dt.Rows[0]["InstNo_1"]);
                    txtDueAmt.Text = Convert.ToString(dt.Rows[0]["DueAmt_1"]);
                    txtMFIName1.Text = Convert.ToString(dt.Rows[0]["MFI_2"]);
                    txtLnAmt1.Text = Convert.ToString(dt.Rows[0]["LoanAmt_2"]);
                    if (dt.Rows[0]["LoanDt_2"].ToString() != "01/01/1900")
                        txtLnDt1.Text = Convert.ToString(dt.Rows[0]["LoanDt_2"]);
                    txtInstAmt1.Text = Convert.ToString(dt.Rows[0]["InstAmt_2"]);
                    txtInstLeft1.Text = Convert.ToString(dt.Rows[0]["InstNo_2"]);
                    txtDueAmt1.Text = Convert.ToString(dt.Rows[0]["DueAmt_2"]);
                    txtBPLNo.Text = Convert.ToString(dt.Rows[0]["BPLNo"]);
                    txtPjMeetDt.Text = Convert.ToString(dt.Rows[0]["PjMeetDt"]);
                    txtBankName.Text = Convert.ToString(dt.Rows[0]["BankName"]);

                    ddlPvLine.SelectedIndex = ddlPvLine.Items.IndexOf(ddlPvLine.Items.FindByValue(dt.Rows[0]["APL"].ToString()));
                    ddlMrySts.SelectedIndex = ddlMrySts.Items.IndexOf(ddlMrySts.Items.FindByValue(dt.Rows[0]["MM_Status"].ToString()));
                    ddlEduc.SelectedIndex = ddlEduc.Items.IndexOf(ddlEduc.Items.FindByValue(dt.Rows[0]["M_QualificationId"].ToString()));
                    ddlOcup.SelectedIndex = ddlOcup.Items.IndexOf(ddlOcup.Items.FindByValue(dt.Rows[0]["M_OccupationId"].ToString()));
                    ddlGend.SelectedIndex = ddlGend.Items.IndexOf(ddlGend.Items.FindByValue(dt.Rows[0]["M_Gender"].ToString()));
                    ddlRelg.SelectedIndex = ddlRelg.Items.IndexOf(ddlRelg.Items.FindByValue(dt.Rows[0]["M_RelgId"].ToString()));
                    ddlCaste.SelectedIndex = ddlCaste.Items.IndexOf(ddlCaste.Items.FindByValue(dt.Rows[0]["M_Caste"].ToString()));
                    ddlBusType.SelectedIndex = ddlBusType.Items.IndexOf(ddlBusType.Items.FindByValue(dt.Rows[0]["MemBusinessTypeId"].ToString()));
                    txtMemEmail.Text = dt.Rows[0]["MemEmailId"].ToString();
                    ddlCoAppMaritalStat.SelectedIndex = ddlCoAppMaritalStat.Items.IndexOf(ddlCoAppMaritalStat.Items.FindByValue(dt.Rows[0]["CoAppMaritalStat"].ToString()));
                    txtDeclIncome.Text = dt.Rows[0]["DeclaredInc"].ToString();
                    ddlIncFrequency.SelectedIndex = ddlIncFrequency.Items.IndexOf(ddlIncFrequency.Items.FindByValue(dt.Rows[0]["IncFrequency"].ToString()));
                    txtCoAppDeclIncome.Text = dt.Rows[0]["CoAppDeclaredInc"].ToString();
                    ddlCoAppBusType.SelectedIndex = ddlCoAppBusType.Items.IndexOf(ddlCoAppBusType.Items.FindByValue(dt.Rows[0]["CoAppBusinessTypeId"].ToString()));
                    ddlCoAppIncFrequency.SelectedIndex = ddlCoAppIncFrequency.Items.IndexOf(ddlCoAppIncFrequency.Items.FindByValue(dt.Rows[0]["CoAppIncFrequency"].ToString()));
                    popBusinessActv(Convert.ToInt32(ddlBusType.SelectedValue));
                    popCoAppBusinessActv(Convert.ToInt32(ddlCoAppBusType.SelectedValue));
                    ddlBusActivity.SelectedIndex = ddlBusActivity.Items.IndexOf(ddlBusActivity.Items.FindByValue(dt.Rows[0]["BusinessActvId"].ToString()));
                    ddlCoAppBusActivity.SelectedIndex = ddlCoAppBusActivity.Items.IndexOf(ddlCoAppBusActivity.Items.FindByValue(dt.Rows[0]["CoAppBusinessActvId"].ToString()));

                    txtVillg.Text = dt.Rows[0]["Village"].ToString();
                    ddlStat.SelectedIndex = ddlStat.Items.IndexOf(ddlStat.Items.FindByText(dt.Rows[0]["State"].ToString()));
                    txtDist.Text = Convert.ToString(dt.Rows[0]["District"]);
                    ddlAddPrf.SelectedIndex = ddlAddPrf.Items.IndexOf(ddlAddPrf.Items.FindByValue(dt.Rows[0]["M_AddProfId"].ToString()));
                    ddlIdentyProf.SelectedIndex = ddlIdentyProf.Items.IndexOf(ddlIdentyProf.Items.FindByValue(dt.Rows[0]["M_IdentyPRofId"].ToString()));
                    txtBranch.Text = Convert.ToString(dt.Rows[0]["BankBranch"]);
                    txtAccNo.Attributes.Add("value", Convert.ToString(dt.Rows[0]["AccNo"]));
                    txtReAccNo.Text = Convert.ToString(dt.Rows[0]["AccNo"]);
                    txtIFSC.Text = Convert.ToString(dt.Rows[0]["IFSCCode"]);
                    txtMemNamePBook.Text = Convert.ToString(dt.Rows[0]["MemNamePBook"]);

                    ddlBRel.SelectedIndex = ddlBRel.Items.IndexOf(ddlBRel.Items.FindByValue(dt.Rows[0]["B_HumanRelationId"].ToString()));
                    ddlBEdu.SelectedIndex = ddlBEdu.Items.IndexOf(ddlBEdu.Items.FindByValue(dt.Rows[0]["B_QualificationId"].ToString()));
                    ddlBOcup.SelectedIndex = ddlBOcup.Items.IndexOf(ddlBOcup.Items.FindByValue(dt.Rows[0]["B_OccupationId"].ToString()));
                    ddlBGend.SelectedIndex = ddlBGend.Items.IndexOf(ddlBGend.Items.FindByValue(dt.Rows[0]["B_Gender"].ToString()));
                    ddlBRelg.SelectedIndex = ddlBRelg.Items.IndexOf(ddlBRelg.Items.FindByValue(dt.Rows[0]["B_RelgId"].ToString()));
                    ddlBCaste.SelectedIndex = ddlBCaste.Items.IndexOf(ddlBCaste.Items.FindByValue(dt.Rows[0]["B_Caste"].ToString()));
                    ddlBVillg.SelectedIndex = ddlBVillg.Items.IndexOf(ddlBVillg.Items.FindByValue(dt.Rows[0]["B_VillageID"].ToString()));
                    //PopBVillDtl(Convert.ToInt32(dt.Rows[0]["B_VillageID"].ToString()));
                    ddlBAddProf.SelectedIndex = ddlBAddProf.Items.IndexOf(ddlBAddProf.Items.FindByValue(dt.Rows[0]["B_AddProfId"].ToString()));
                    ddlBIdntyProf.SelectedIndex = ddlBIdntyProf.Items.IndexOf(ddlBIdntyProf.Items.FindByValue(dt.Rows[0]["B_IdentyProfId"].ToString()));

                    //ddlRO.SelectedIndex = ddlRO.Items.IndexOf(ddlRO.Items.FindByValue(dt.Rows[0]["DueAmt_2"].ToString()));
                    ddlAccType.SelectedIndex = ddlAccType.Items.IndexOf(ddlAccType.Items.FindByValue(dt.Rows[0]["Acc_Type"].ToString()));
                    ddlIdProof3.SelectedIndex = ddlIdProof3.Items.IndexOf(ddlIdProof3.Items.FindByValue(dt.Rows[0]["AddProfId2"].ToString()));
                    ddlAddrType.SelectedIndex = ddlAddrType.Items.IndexOf(ddlAddrType.Items.FindByValue(dt.Rows[0]["AddrType"].ToString()));
                    ddlCommAddrType.SelectedIndex = ddlCommAddrType.Items.IndexOf(ddlCommAddrType.Items.FindByValue(dt.Rows[0]["AddrType_p"].ToString()));
                    ddlCommVill.SelectedIndex = ddlCommVill.Items.IndexOf(ddlCommVill.Items.FindByValue(dt.Rows[0]["VillageId_p"].ToString()));
                    popAgainstVillage2();
                    ddlAreaCategory.SelectedIndex = ddlAreaCategory.Items.IndexOf(ddlAreaCategory.Items.FindByValue(dt.Rows[0]["Area_Category"].ToString()));
                    //ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["MarketId"].ToString()));
                    //ddlGroup.SelectedIndex = ddlGroup.Items.IndexOf(ddlGroup.Items.FindByValue(dt.Rows[0]["GroupId"].ToString()));
                    GetGroup(dt.Rows[0]["GroupId"].ToString());
                    PopCollDay(ddlGroup.SelectedValue);
                    //ddlMetDay.SelectedIndex = ddlMetDay.Items.IndexOf(ddlMetDay.Items.FindByValue(dt.Rows[0]["MeetingDay"].ToString()));
                    txtMetDay.Text = dt.Rows[0]["MeetingDay"].ToString();
                    ddlHHoldTyp.SelectedIndex = ddlHHoldTyp.Items.IndexOf(ddlHHoldTyp.Items.FindByValue(dt.Rows[0]["HouseHoldId"].ToString()));
                    txtNoOfDpndnts.Text = Convert.ToString(dt.Rows[0]["NoOfDependants"]);

                    if (ddlCommMuni.SelectedIndex > 0)
                    {
                        txtMemCommAddr.Text = txtCommHouseNo.Text + "," + txtCommSt.Text + "," + ddlCommVill.SelectedItem.Text.ToString() + "," + ddlCommMuni.SelectedItem.Text.ToString() + "," + ddlCommBlock.SelectedItem.Text.ToString() + "," + txtCommSubDist.Text + "," + txtCommPost.Text + "," + txtCommPin.Text;
                    }

                    if (dt.Rows[0]["Tra_DropDate"].ToString() != "01/01/1900" && dt.Rows[0]["Tra_DropDate"].ToString() != "")
                    {
                        txtDtCl.Text = dt.Rows[0]["Tra_DropDate"].ToString();
                        cbDrp.Checked = true;
                        ddlClTyp.SelectedIndex = ddlClTyp.Items.IndexOf(ddlClTyp.Items.FindByValue(dt.Rows[0]["Tra_Drop"].ToString()));
                        txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                    }
                    else
                    {
                        txtDtCl.Text = "";
                        cbDrp.Checked = false;
                        ddlClTyp.SelectedIndex = -1;
                        txtRemarks.Text = "";
                    }
                    txtNoOfHouseMember.Text = Convert.ToString(dt.Rows[0]["No_of_House_Member"]);
                    txtNoOfChild.Text = Convert.ToString(dt.Rows[0]["No_of_Children"]);
                    txtBranchDistance.Text = Convert.ToString(dt.Rows[0]["Distance_frm_Branch"]);
                    txtCollCenterDistance.Text = Convert.ToString(dt.Rows[0]["Distance_frm_Coll_Center"]);

                    txtMaidenNmF.Text = Convert.ToString(dt.Rows[0]["MaidenNmF"].ToString());
                    txtMaidenNmM.Text = Convert.ToString(dt.Rows[0]["MaidenNmM"].ToString());
                    txtMaidenNmL.Text = Convert.ToString(dt.Rows[0]["MaidenNmL"].ToString());
                    txtGuarName.Text = Convert.ToString(dt.Rows[0]["GuarFName"].ToString());
                    txtGuarLName.Text = Convert.ToString(dt.Rows[0]["GuarLName"].ToString());
                    ddlGuarRel.SelectedIndex = ddlGuarRel.Items.IndexOf(ddlGuarRel.Items.FindByValue(Convert.ToString(dt.Rows[0]["GuarRel"].ToString())));
                    txtGuarDOB.Text = Convert.ToString(dt.Rows[0]["GuarDOB"].ToString());
                    txtGuarAge.Text = Convert.ToString(dt.Rows[0]["GuarAge"].ToString());
                    ddlGuarGen.SelectedIndex = ddlGuarGen.Items.IndexOf(ddlGuarGen.Items.FindByValue(Convert.ToString(dt.Rows[0]["GuarGen"].ToString())));

                    ddlMHFRelation.SelectedIndex = ddlMHFRelation.Items.IndexOf(ddlMHFRelation.Items.FindByValue(dt.Rows[0]["MHF_Relation"].ToString()));
                    ddlMinority.SelectedIndex = ddlMinority.Items.IndexOf(ddlMinority.Items.FindByValue(dt.Rows[0]["MinorityYN"].ToString()));
                    lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                    lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                    //if (Session[gblValue.BrnchCode].ToString() == "0000")
                    //{
                    //    hdnLoanAppId.Value = Convert.ToString(dt.Rows[0]["LoanAppId"].ToString());
                    //}

                    ViewState["LoanAppId"] = hdnLoanAppId.Value;
                    ////
                    DataRow dF;
                    dF = dt2.NewRow();
                    dt2.Rows.Add(dF);
                    dt2.AcceptChanges();
                    ViewState["Fam"] = dt2;
                    gvFamily.DataSource = dt2;
                    gvFamily.DataBind();
                    ////
                    DataRow dF1;
                    dF1 = dt3.NewRow();
                    dt3.Rows.Add(dF1);
                    dt3.AcceptChanges();
                    ViewState["Asset"] = dt3;
                    gvAsset.DataSource = dt3;
                    gvAsset.DataBind();

                    ///income/expenses
                    ///
                    if (dt4.Rows.Count > 0)
                    {
                        txtFamilyInc.Text = Convert.ToString(dt4.Rows[0]["FamilyIncome"]);
                        txtSelfInc.Text = Convert.ToString(dt4.Rows[0]["SlefIncome"]);
                        ddlOtherIncSrc.SelectedIndex = ddlOtherIncSrc.Items.IndexOf(ddlOtherIncSrc.Items.FindByValue(Convert.ToString(dt4.Rows[0]["OtherIncomeSrcId"].ToString())));
                        txtOtherInc.Text = Convert.ToString(dt4.Rows[0]["OtherIncome"]);
                        txtTotInc.Text = Convert.ToString(dt4.Rows[0]["TotIncome"]);
                        txtHsRntAmt.Text = Convert.ToString(dt4.Rows[0]["ExHsRntAmt"]);
                        txtFdAmt.Text = Convert.ToString(dt4.Rows[0]["ExpFdAmt"]);
                        txtEduAmt.Text = Convert.ToString(dt4.Rows[0]["ExpEduAmt"]);
                        txtMedAmt.Text = Convert.ToString(dt4.Rows[0]["ExpMedAmt"]);
                        txtLnInsAmt.Text = Convert.ToString(dt4.Rows[0]["ExpLnInsAmt"]);
                        txtFuelExp.Text = Convert.ToString(dt4.Rows[0]["ExpFuelAmt"]);
                        txtOtherExp.Text = Convert.ToString(dt4.Rows[0]["ExpOtherAmt"]);
                        txtElecExp.Text = Convert.ToString(dt4.Rows[0]["ExpElectricAmt"]);
                        txtTransExp.Text = Convert.ToString(dt4.Rows[0]["ExpTransAmt"]);
                        txtTotExp.Text = Convert.ToString(dt4.Rows[0]["TotalExp"]);
                        txtSurplus.Text = Convert.ToString(dt4.Rows[0]["Surplus"]);
                    }
                    DataTable dtEnq = new DataTable();
                    oMem = new CMember();
                    dtEnq = oMem.GetLastEnqIdByMemId(txtMemID.Text.ToString());
                    if (dtEnq.Rows.Count > 0)
                    {
                        Session["EnquiryId"] = dtEnq.Rows[0]["EnquiryId"].ToString();
                        hdnEnqId.Value = dtEnq.Rows[0]["EnquiryId"].ToString();
                        ViewState["AadhaarScan"] = dtEnq.Rows[0]["AadhaarScan"].ToString();
                        ViewState["OCRYN"] = dtEnq.Rows[0]["OCRYN"].ToString();
                        hdnEnqDate.Value = dtEnq.Rows[0]["EnqDate"].ToString();
                        Session["EnquiryDate"] = dtEnq.Rows[0]["EnquiryDate"].ToString();
                        Session["EnqDate"] = dtEnq.Rows[0]["EnqDate"].ToString();
                    }
                    else
                    {
                        Session["EnquiryId"] = "";
                        Session["EnqDate"] = "";
                        Session["EnquiryDate"] = "";
                        hdnEnqId.Value = "";
                        hdnEnqDate.Value = "";
                        ViewState["AadhaarScan"] = "M";
                        ViewState["OCRYN"] = "N";
                    }

                    lblAadharScan.Text = Convert.ToString(ViewState["AadhaarScan"]);

                    if (Convert.ToString(ViewState["AadhaarScan"]) == "M")
                    {
                        if (dt.Rows[0]["M_IdentyPRofId"].ToString() == "1")
                        {
                            lblId1.Visible = true;
                            txtConfrmAadhar1.Visible = true;
                        }
                        else if (dt.Rows[0]["M_AddProfId"].ToString() == "1")
                        {
                            lblId2.Visible = true;
                            txtConfrmAadhar2.Visible = true;
                        }
                        else if (dt.Rows[0]["AddProfId2"].ToString() == "1")
                        {
                            lblId3.Visible = true;
                            txtConfrmAadhar3.Visible = true;
                        }
                        else
                        {
                            lblId1.Visible = false;
                            txtConfrmAadhar1.Visible = false;
                            lblId2.Visible = false;
                            txtConfrmAadhar2.Visible = false;
                            lblId3.Visible = false;
                            txtConfrmAadhar3.Visible = false;
                        }
                    }

                    if (dt.Rows[0]["B_IdentyProfId"].ToString() == "1")
                    {
                        lblBId1.Visible = true;
                        txtBConfrmAadhar1.Visible = true;
                    }
                    else if (dt.Rows[0]["B_AddProfId"].ToString() == "1")
                    {
                        lblBId2.Visible = true;
                        txtBConfrmAadhar2.Visible = true;
                    }
                    else
                    {
                        lblBId1.Visible = false;
                        txtBConfrmAadhar1.Visible = false;
                        lblBId2.Visible = false;
                        txtBConfrmAadhar2.Visible = false;
                    }

                    if (dt6.Rows.Count > 0)
                    {
                        dt6.TableName = "TableData";
                        ViewState["EarningMember"] = dt6;
                        gvEarningMember.DataSource = dt6;
                        gvEarningMember.DataBind();
                    }
                    else
                    {
                        GenerateEarningMember();
                    }
                    gvEarningMember.Enabled = false;

                    //if (dt.Rows[0]["M_IdentyPRofId"].ToString() == "13")
                    //{
                    //    fuIdProof1Front.Enabled = false;
                    //    fuIdProof1Back.Enabled = false;
                    //}
                    //if (dt.Rows[0]["M_AddProfId"].ToString() == "13")
                    //{
                    //    fuIdProof2Front.Enabled = false;
                    //    fuIdProof2Back.Enabled = false;
                    //}
                    //if (dt.Rows[0]["B_IdentyProfId"].ToString() == "13")
                    //{
                    //    fuIdProof3Front.Enabled = false;
                    //    fuIdProof3Back.Enabled = false;
                    //}           
                    
                    if (Convert.ToInt32(Session[gblValue.RoleId]) == 1 || Convert.ToInt32(Session[gblValue.RoleId]) == 11
                        || Convert.ToInt32(Session[gblValue.RoleId]) == 25 || Convert.ToInt32(Session[gblValue.RoleId]) == 57)
                    {
                        fuMemPhoto.Visible = true;                        
                    }
                    else
                    {
                        fuMemPhoto.Visible = false;
                        if (gblFuction.setDate(Session["EnquiryDate"].ToString()) >= gblFuction.setDate("12/06/2025"))
                        {
                            if (ddlIdentyProf.SelectedValue == "13")
                            {
                                fuIdProof1Front.Visible = false;
                                fuIdProof1Back.Visible = false;
                            }
                            else
                            {
                                fuIdProof1Front.Visible = true;
                                fuIdProof1Back.Visible = true;
                            }
                            if (ddlBIdntyProf.SelectedValue == "13")
                            {
                                fuIdProof3Front.Visible = false;
                                fuIdProof3Back.Visible = false;
                            }
                            else
                            {
                                fuIdProof3Front.Visible = true;
                                fuIdProof3Back.Visible = true;
                            }
                        }
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

                    // EnableControl(false);
                }

            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void memberKYC(string InitialApproachId)
        {
            pathImage = ConfigurationManager.AppSettings["PathImage"];
            string imgFolder = InitialApproachId;
            string vUrl = pathImage + "InitialApproach/";
            //string vUrl = "https://centrummobtest.bijliftt.com/Files/InitialApproach/";
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

        private void memberPassbook(string MemberId)
        {
            string imgFolder = MemberId;
            //imgMemPassbook.ImageUrl = "https://centrummobtest.bijliftt.com/Files/Member/" + MemberId + "/PassbookImage.png";
            pathImage = ConfigurationManager.AppSettings["PathImage"];
            imgMemPassbook.ImageUrl = pathImage + "/Member/" + MemberId + "/PassbookImage.png";
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
        protected void btnShow1_Click(object sender, EventArgs e)
        {
            LoadTree();
        }


        /// <summary>
        /// 
        /// </summary>
        private void EnableCGT()
        {
            string vMemberId = ViewState["MemId"].ToString();
            CMember oMem = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            int vRet = 0;

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oMem = new CMember();
                vRet = oMem.ChkApplycgt(vMemberId, vLogDt);
                if (vRet > 0)
                    btnCgtApply.Enabled = true;
                else
                    btnCgtApply.Enabled = false;

            }
            finally
            {
                oMem = null;
            }
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
        protected void cblTrnsDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vRec = 0;
            string vEoId = Convert.ToString(ViewState["EoId"]);
            CEO oEo = new CEO();
            CGblIdGenerator oGbl = new CGblIdGenerator();
            vRec = oGbl.ChkDeleteString(vEoId, "EOID", "MarketMSt");
            if (vRec > 0)
            {
                gblFuction.MsgPopup("The RO has Center, you can not Transfer the RO.");
                //cblTrnsDrop.ClearSelection();
            }
            else
            {
                //cblTrnsDrop.Items.FindByValue("D").Enabled = false;
                //cblTrnsDrop.Items.FindByValue("P").Enabled = false;
                //popDesignation();
                //ddlTrnsBr.SelectedIndex = -1;
                //txtTrnDt.Text = "";
                //ddlDesig.SelectedIndex = -1;
                //ddlTrnsBr.Enabled = true;
                //txtTrnDt.Enabled = true;
                //ddlDesig.Enabled = false;
            }
            if (vRec > 0)
            {
                gblFuction.MsgPopup("The RO has Center, you can not Dropout the RO.");
                //cblTrnsDrop.ClearSelection();
            }
            else
            {
                //cblTrnsDrop.Items.FindByValue("T").Enabled = false;
                //cblTrnsDrop.Items.FindByValue("P").Enabled = false;
                //ddlTrnsBr.SelectedIndex = -1;
                //txtTrnDt.Text = "";
                //ddlDesig.SelectedIndex = -1;
                //ddlTrnsBr.Enabled = false;
                //txtTrnDt.Enabled = true;
                //ddlDesig.Enabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMrySts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMrySts.SelectedValue == "M01" && ddlGend.SelectedValue == "F")
            {
                chkFath.Enabled = false;
                chkFath.Checked = false;
                txtBFName.Text = txtHFName.Text;
                txtGuarName.Text = txtHFName.Text;

                ddlBRel.SelectedIndex = ddlBRel.Items.IndexOf(ddlBRel.Items.FindByValue(ddlMHFRelation.SelectedValue == "25" ? "25" : "7"));
                ddlBGend.SelectedIndex = ddlBGend.Items.IndexOf(ddlBGend.Items.FindByValue(ddlMHFRelation.SelectedValue == "25" ? "M" : "F"));
                ddlGuarRel.SelectedIndex = ddlGuarRel.Items.IndexOf(ddlGuarRel.Items.FindByText(ddlMHFRelation.SelectedValue == "25" ? "25" : "7"));
                ddlGuarGen.SelectedIndex = ddlGuarGen.Items.IndexOf(ddlGuarGen.Items.FindByText(ddlMHFRelation.SelectedValue == "25" ? "M" : "F"));

            }
            else if (ddlMrySts.SelectedValue == "M05" && ddlGend.SelectedValue == "F")
            {
                chkFath.Enabled = false;
                chkFath.Checked = ddlMHFRelation.SelectedValue == "4" ? true : false;
                txtBFName.Text = txtHFName.Text;
                txtGuarName.Text = txtHFName.Text;
                ddlGuarRel.SelectedIndex = -1;
                ddlBRel.SelectedIndex = -1;
                ddlBGend.SelectedIndex = ddlBGend.Items.IndexOf(ddlBGend.Items.FindByText(ddlMHFRelation.SelectedValue == "4" ? "Male" : "Female"));
                ddlGuarGen.SelectedIndex = ddlGuarGen.Items.IndexOf(ddlGuarGen.Items.FindByText(ddlMHFRelation.SelectedValue == "4" ? "Male" : "Female"));
            }
            else
            {
                chkFath.Enabled = true;
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
            ddlClTyp.Items.AddRange(items);
            ddlClTyp.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            this.GetModuleByRole(mnuID.mnuMemberMst);
            DateTime vEndDt;
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vMemId = Convert.ToString(ViewState["MemId"]);
            string EnqId = Convert.ToString(Session["EnquiryId"]);
            string EnqDate = Convert.ToString(Session["EnqDate"]);
            path = ConfigurationManager.AppSettings["PathInitialApproach"];
            pathG = ConfigurationManager.AppSettings["PathG"];
            pathMember = ConfigurationManager.AppSettings["pathMember"];
            string vMVilg = "", vXmlEarningMemDtl = "";
            Int32 vErr = 0, vRec = 0, vQuliId = 0, vMAge = 0, vMOcupId = 0, vMAddProfId = 0, vMIdProfId = 0;
            Int32 vBQuliId = 0, vBAge = 0, vBOcupId = 0, vBVilgId = 0, vBAddProfId = 0, vBIdProfId = 0, vHHoldId = 0, vNoOfDpndnt = 0;
            Int32 vInstNo1 = 0, vInstNo2 = 0, vBRelId = 0;
            string vDrop = Convert.ToString(ViewState["vDropState"]);
            string vMM_Stat = "", vChkFath = "N", vCltype = "", vMsg = "";
            double vIncAmt = 0, vExpAmt = 0, vLnAmt1 = 0, vLnAmt2 = 0, vInstAmt1 = 0, vInstAmt2 = 0, vDueAmt1 = 0, vDueAmt2 = 0;
            DateTime vAdmDt = gblFuction.setDate(txtAdmDt.Text);
            DateTime vClDt = gblFuction.setDate(txtDtCl.Text);
            DateTime vDOBDt = gblFuction.setDate(txtDOB.Text);
            DateTime vBDOBDt = gblFuction.setDate(txtBDOBDt.Text);
            DateTime vPjMeetDt = gblFuction.setDate(txtPjMeetDt.Text);
            DateTime vLnDt1 = gblFuction.setDate(txtLnDt.Text);
            DateTime vLnDt2 = gblFuction.setDate(txtLnDt1.Text);
            CApplication oCG = null;
            string vViewAAdhar = Convert.ToString(Session[gblValue.ViewAAdhar]);

            string vMaidenNmF = txtMaidenNmF.Text == "" ? "" : txtMaidenNmF.Text.Trim();
            string vMaidenNmM = txtMaidenNmM.Text == "" ? "" : txtMaidenNmM.Text.Trim();
            string vMaidenNmL = txtMaidenNmL.Text == "" ? "" : txtMaidenNmL.Text.Trim();
            string vMemAddr = txtMemAddr.Text == "" ? "" : txtMemAddr.Text.Trim();
            string vCoBrwrAddr = txtCoBrwrAddr.Text == "" ? "" : txtCoBrwrAddr.Text.Trim();
            Int32 vMemBusTypeId = -1, vOtherIncomeSrcId = -1;
            string vMemEMailId = string.Empty, vCoAppMaritalStat = string.Empty;
            Int32 vIncFrequency = 0, vCoAppBusTypeId = 0, vCoAppIncFrequency = 0;
            double vDeclIncome = 0, vCoAppDeclIncome = 0;
            Int32 vBusActvId = 0, vCoAppBusActvId = 0, vMHF_RelationId = -1;

            DataTable dt1 = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            string vXmlFam = "";
            string vXmlAsset = "";
            //Family
            if (cbDrp.Checked == false)
            {
                GetData();
                dt1 = (DataTable)ViewState["Fam"];

                ////sourav
                if (dt1.Rows.Count == 0)
                {
                    gblFuction.MsgPopup("Please insert atleast one Family Details");
                    return false;
                }
                ////sourav
                //}
                dt1.AcceptChanges();
                dt1.TableName = "Table1";
                using (StringWriter oSW = new StringWriter())
                {
                    dt1.WriteXml(oSW);
                    vXmlFam = oSW.ToString();
                }
                /////asset
                GetData1();
                dt2 = (DataTable)ViewState["Asset"];
                Int32 vNumOfAsset = 0;
                DataRow[] vrows;
                vrows = dt2.Select("AssetName <> '' and AssetName <> '-1'");
                vNumOfAsset = vrows.Length;
                if (vNumOfAsset == 0)
                {
                    gblFuction.MsgPopup("Please insert atleast one Asset Details");
                    return false;
                }
                //if (dt2.Rows.Count == 0)
                //{
                //    gblFuction.MsgPopup("Please insert atleast one Asset Details");
                //    return false;
                //}
                //}
                dt2.AcceptChanges();
                dt2.TableName = "Table2";
                using (StringWriter oSW = new StringWriter())
                {
                    dt2.WriteXml(oSW);
                    vXmlAsset = oSW.ToString();
                }
            }


            //string vCGTYN = ViewState["CGTYN"].ToString();
            string vCGTYN = hdnApplyCgt.Value;
            if (vCGTYN == null) vCGTYN = "N";
            CMember oMem = null;
            CTransfer oTra = null;
            CGblIdGenerator oGbl = null;
            DataTable dt = null;
            /////sourav
            if (Session[gblValue.EndDate] != null)
            {
                vEndDt = gblFuction.setDate(Session[gblValue.EndDate].ToString());
                if (vAdmDt > vEndDt.AddDays(1))
                {
                    gblFuction.MsgPopup("Admission Date must be Less than or equal with End Date...");
                    return false;
                }
            }

            if (cbDrp.Checked == false)
            {
                if (ValidateField(vMemId, vBrCode) == false) return false;
            }
            //----Ipsita----
            if (cbDrp.Checked == true)
            {
                oCG = new CApplication();
                vMsg = oCG.ChkLoanOther(vMemId, 0, "A");
                if (vMsg != "")
                {
                    gblFuction.MsgPopup(vMsg);
                    return false;
                }
                if (ddlClTyp.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("InValid Dropout Type...");
                    return false;
                }
                if (vClDt < vAdmDt)
                {
                    gblFuction.MsgPopup("InValid Dropout Date...");
                    return false;
                }
                DateTime Logindt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                //vMsg = oCG.SaveDropOutMember(vMemId, Logindt, txtRemarks.Text.ToString());
                //if (vMsg != "")
                //{
                //    gblFuction.MsgPopup(vMsg);
                //    return false;
                //}
            }

            if (ddlIdentyProf.SelectedItem.Text == ddlAddPrf.SelectedItem.Text)
            {
                gblFuction.MsgPopup("Address Proof and Identity Proof must be different..");
                ddlAddPrf.Focus();
                return false;
            }
            if (txtIdntPrfNo.Text.ToString().Trim() == txtAddPrfNo.Text.ToString().Trim())
            {
                gblFuction.MsgPopup("Address Proof No and Identity Proof No not be Same..");
                txtAddPrfNo.Focus();
                return false;
            }

            /////sourav
            try
            {
                vMM_Stat = ddlMrySts.SelectedValue;
                vQuliId = Convert.ToInt32(ddlEduc.SelectedValue);
                if (chkFath.Checked == true) vChkFath = "Y";
                vMAge = Convert.ToInt32(txtAge.Text);
                vBAge = txtBAge.Text == "" ? 0 : Convert.ToInt32(txtBAge.Text);
                vMOcupId = Convert.ToInt32(ddlOcup.SelectedValue);
                vBOcupId = Convert.ToInt32(ddlBOcup.SelectedValue);
                vMVilg = Convert.ToString(txtVillg.Text);
                vBVilgId = Convert.ToInt32(ddlBVillg.SelectedValue);
                vMAddProfId = Convert.ToInt32(ddlAddPrf.SelectedValue);
                vBAddProfId = Convert.ToInt32(ddlBAddProf.SelectedValue);
                vMIdProfId = Convert.ToInt32(ddlIdentyProf.SelectedValue);
                vBIdProfId = Convert.ToInt32(ddlBIdntyProf.SelectedValue);
                vBQuliId = Convert.ToInt32(ddlBEdu.SelectedValue);
                vBRelId = Convert.ToInt32(ddlBRel.SelectedValue);
                vIncAmt = Convert.ToDouble(txtIncome.Text);
                vExpAmt = Convert.ToDouble(txtExpnses.Text);
                vHHoldId = Convert.ToInt32(ddlHHoldTyp.SelectedValue);
                vLnAmt1 = Convert.ToDouble(txtLnAmt.Text);
                vLnAmt2 = Convert.ToDouble(txtLnAmt1.Text);
                vInstAmt1 = Convert.ToDouble(txtInstAmt.Text);
                vInstAmt2 = Convert.ToDouble(txtInstAmt1.Text);
                vInstNo1 = Convert.ToInt32(txtInstLeft.Text);
                vInstNo2 = Convert.ToInt32(txtInstLeft1.Text);
                vDueAmt1 = Convert.ToDouble(txtDueAmt.Text);
                vDueAmt2 = Convert.ToDouble(txtDueAmt1.Text);
                vCltype = Convert.ToString(ddlClTyp.SelectedValue);
                vMHF_RelationId = Convert.ToInt32(ddlMHFRelation.SelectedValue);
                if (txtNoOfDpndnts.Text != "")
                {
                    vNoOfDpndnt = Convert.ToInt32(txtNoOfDpndnts.Text);
                }

                vMemBusTypeId = Convert.ToInt32(ddlBusType.SelectedValue);
                vMemEMailId = txtMemEmail.Text.Trim();
                vCoAppMaritalStat = Convert.ToString(ddlCoAppMaritalStat.SelectedValue);
                vOtherIncomeSrcId = Convert.ToInt32(ddlOtherIncSrc.SelectedValue);
                //vXmlEarningMemDtl = EarningMemDtlDtToXml();
                vXmlEarningMemDtl = EarningMemDtlDtToXmlFinal();
                vIncFrequency = Convert.ToInt32(ddlIncFrequency.SelectedValue);
                if (txtDeclIncome.Text.Trim() == "") txtDeclIncome.Text = "0";
                if (txtDeclIncome.Text.Trim() == "")
                {
                    vDeclIncome = Convert.ToDouble("0");
                }
                else
                {
                    vDeclIncome = Convert.ToDouble(txtDeclIncome.Text.Trim());
                }
                vCoAppBusTypeId = Convert.ToInt32(ddlCoAppBusType.SelectedValue);
                vCoAppIncFrequency = Convert.ToInt32(ddlCoAppIncFrequency.SelectedValue);
                if (txtCoAppDeclIncome.Text == "") txtCoAppDeclIncome.Text = "0";
                if (txtCoAppDeclIncome.Text.Trim() == "")
                {
                    vCoAppDeclIncome = Convert.ToDouble("0");
                }
                else
                {
                    vCoAppDeclIncome = Convert.ToDouble(txtCoAppDeclIncome.Text.Trim());
                }

                vBusActvId = Convert.ToInt32(ddlBusActivity.SelectedValue);
                vCoAppBusActvId = Convert.ToInt32(ddlCoAppBusActivity.SelectedValue);

                if (Mode == "Save")
                {
                    if (txtBankName.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Please Enter a Bank..");
                        return false;
                    }
                    if (txtAccNo.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Please Enter a Bank Account No..");
                        return false;
                    }
                    if (txtIFSC.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Please Enter the Bank IFSC Code..");
                        return false;
                    }
                    if (txtBranch.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Please Enter the Bank Branch Name..");
                        return false;
                    }
                    if (txtMemNamePBook.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Please Enter the Member Name in Bank Passbook..");
                        return false;
                    }
                    if (txtCustId.Text != "")
                    {
                        if (txtCustId.Text.Length < 0)
                        {
                            gblFuction.AjxMsgPopup("Cust Id must be 8 digits");
                            return false;
                        }
                    }
                    if (txtSavingsAcNo.Text != "")
                    {
                        if (txtSavingsAcNo.Text.Length < 0)
                        {
                            gblFuction.AjxMsgPopup("Savings Account No must be 16 digits");
                            return false;
                        }
                    }


                }
                else if (Mode == "Edit")
                {
                    oMem = new CMember();
                    dt3 = oMem.ChkMemberLnCycle(vMemId, vBrCode);
                    if (dt3.Rows.Count > 0 && (cbDrp.Checked == false))
                    {
                        if (Convert.ToInt32(dt3.Rows[0]["RecCnt"].ToString()) <= 0)
                        {
                            if (txtBankName.Text == "")
                            {
                                gblFuction.AjxMsgPopup("Please Select a Bank..");
                                return false;
                            }
                            if (txtAccNo.Text == "")
                            {
                                gblFuction.AjxMsgPopup("Please Enter a Bank Account No..");
                                return false;
                            }
                            if (txtIFSC.Text == "")
                            {
                                gblFuction.AjxMsgPopup("Please Enter the Bank IFSC Code..");
                                return false;
                            }
                            if (txtBranch.Text == "")
                            {
                                gblFuction.AjxMsgPopup("Please Enter the Bank Branch Name..");
                                return false;
                            }
                            if (txtMemNamePBook.Text == "")
                            {
                                gblFuction.AjxMsgPopup("Please Enter the Member Name in Bank Passbook..");
                                return false;
                            }
                        }
                    }
                }
                //-----------------------------------------------------------------------------------------------
                if (Mode == "Save")
                {

                    oMem = new CMember();
                    oGbl = new CGblIdGenerator();

                    if (this.RoleId != 1)
                    {
                        if (Session[gblValue.EndDate] != null)
                        {
                            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtAdmDt.Text))
                            {
                                gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                                return false;
                            }
                        }
                    }
                    /////sourav
                    vRec = oGbl.ChkDuplicate("MemberMst", "M_Mobile", txtMob.Text.Replace("'", "''"), "", "", "MemberID", vMemId, "Save");
                    if (vRec > 0)
                    {
                        string MobUser = "";
                        MobUser = oGbl.getMobileUser(txtMob.Text.ToString());
                        gblFuction.MsgPopup(MobUser.ToString() + "Already Access this Mobile Number");
                        return false;
                    }
                    if (this.RoleId != 1)
                    {
                        if (Session[gblValue.EndDate] != null)
                        {
                            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtAdmDt.Text))
                            {
                                gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                                return false;
                            }
                        }
                    }
                    /////////////////////////////////////////////////////////
                    if (txtIdntPrfNo.Text != "")
                    {
                        oMem = new CMember();

                        vMsg = oMem.ChkIDProf(txtIdntPrfNo.Text);
                        if (vMsg != "")
                        {
                            //gblFuction.MsgPopup(vMsg);
                            gblFuction.AjxMsgPopup(vMsg);
                            txtIdntPrfNo.Text = "";
                            txtIdntPrfNo.Focus();
                            return false;
                        }

                    }
                    if (txtAddPrfNo.Text != "")
                    {
                        oMem = new CMember();

                        vMsg = oMem.ChkIDProf(txtAddPrfNo.Text);
                        if (vMsg != "")
                        {
                            //gblFuction.MsgPopup(vMsg);
                            gblFuction.AjxMsgPopup(vMsg);
                            txtIdntPrfNo.Text = "";
                            txtIdntPrfNo.Focus();
                            return false;
                        }

                    }
                    if (txtBAddPrfNo.Text != "")
                    {
                        oMem = new CMember();

                        vMsg = oMem.ChkIDProf(txtBAddPrfNo.Text);
                        if (vMsg != "")
                        {
                            gblFuction.AjxMsgPopup(vMsg);
                            txtBAddPrfNo.Text = "";
                            txtBAddPrfNo.Focus();
                            return false;
                        }
                    }
                    if (txtBIdntPrfNo.Text != "")
                    {
                        oMem = new CMember();

                        vMsg = oMem.ChkIDProf(txtBIdntPrfNo.Text);
                        if (vMsg != "")
                        {
                            gblFuction.AjxMsgPopup(vMsg);
                            txtBIdntPrfNo.Text = "";
                            txtBIdntPrfNo.Focus();
                            return false;
                        }
                    }
                    if (txtBAddPrfNo.Text != "")
                    {
                        oMem = new CMember();

                        vMsg = oMem.ChkIDProf(txtBAddPrfNo.Text);
                        if (vMsg != "")
                        {
                            gblFuction.AjxMsgPopup(vMsg);
                            txtBAddPrfNo.Text = "";
                            txtBAddPrfNo.Focus();
                            return false;
                        }
                    }
                    //////////////////////////////////////////////////////


                    //dt = oMem.ChkMemberByMarketId(ddlCenter.SelectedValue, vBrCode);
                    //if (dt.Rows.Count >= 36)
                    //{
                    //    gblFuction.AjxMsgPopup("You can not enter More than 36 member in one center..");
                    //    return false;
                    //}
                    oTra = new CTransfer();
                    dt = oTra.ChkMemberByGroupId(ddlGroup.SelectedValue, vBrCode);
                    if (Convert.ToInt32(dt.Rows[0]["MemCount"].ToString()) > 35)
                    {
                        gblFuction.AjxMsgPopup("You can not enter More than 35 member in one Group..");
                        return false;
                    }
                    //if ((ddlIdentyProf.SelectedValue != "") && (ddlAddPrf.SelectedValue != ""))
                    //{
                    //    if (ValidateField(vMemId, vBrCode) == false) return false;
                    //}
                    /////sourav

                    if (txtAmtA.Text != "" && txtAmtA.Text != "0.00")
                    {
                        if (txtIncA.Text == "")
                        {
                            gblFuction.MsgPopup("Income Source of (a) is Blank...");
                            return false;
                        }
                    }

                    if (txtAmtB.Text != "" && txtAmtB.Text != "0.00")
                    {
                        if (txtIncB.Text == "")
                        {
                            gblFuction.MsgPopup("Income Source of (b) is Blank...");
                            return false;
                        }
                    }
                    if (txtAmtC.Text != "" && txtAmtC.Text != "0.00")
                    {
                        if (txtIncC.Text == "")
                        {
                            gblFuction.MsgPopup("Income Source of (c) is Blank...");
                            return false;
                        }
                    }
                    if (txtAmtD.Text != "" && txtAmtD.Text != "0.00")
                    {
                        if (txtIncD.Text == "")
                        {
                            gblFuction.MsgPopup("Income Source of (d) is Blank...");
                            return false;
                        }
                    }
                    if (txtAmtE.Text != "" && txtAmtE.Text != "0.00")
                    {
                        if (txtIncE.Text == "")
                        {
                            gblFuction.MsgPopup("Income Source of (e) is Blank...");
                            return false;
                        }
                    }
                    if (txtAmtF.Text != "" && txtAmtF.Text != "0.00")
                    {
                        if (txtIncF.Text == "")
                        {
                            gblFuction.MsgPopup("Income Source of (f) is Blank...");
                            return false;
                        }
                    }

                    if (txtExAmtB.Text == "" && txtExAmtB.Text == "0.00")
                    {
                        if (txtExB.Text != "")
                        {
                            gblFuction.MsgPopup("Expense Source of (b) is Blank...");
                            return false;
                        }
                    }
                    if (txtExAmtA.Text != "" && txtExAmtA.Text != "0.00")
                    {
                        if (txtExA.Text == "")
                        {
                            gblFuction.MsgPopup("Expense Source of (a) is Blank...");
                            return false;
                        }
                    }

                    CBank oBnk = new CBank();
                    int vExist = 0;
                    vExist = oBnk.chkIfscExistOrNot(txtIFSC.Text.Trim());
                    if (vExist == 0)
                    {
                        gblFuction.MsgPopup("Please Enter a Valid IFSC...!!");
                        return false;
                    }

                    vErr = oMem.SaveMember(ref vMemId, vAdmDt, txtFName.Text.Replace("'", "''"),
                         txtMName.Text.Replace("'", "''"), txtLName.Text.Replace("'", "''"),
                         vMM_Stat, vQuliId, txtHFName.Text.Replace("'", "''"), vChkFath, vDOBDt, vMAge,
                         vMOcupId, ddlGend.SelectedValue, ddlRelg.SelectedValue, ddlCaste.SelectedValue,
                         txtHouNo.Text.Replace("'", "''"), txtStName.Text.Replace("'", "''"), txtVillg.Text,
                         txtWardNo.Text.Replace("'", "''"), txtPOff.Text.Replace("'", "''"), txtPin.Text,
                         txtMob.Text, txtPhNo.Text, vMAddProfId, txtAddPrfNo.Text.Replace("'", "''"),
                         vMIdProfId, txtIdntPrfNo.Text.Replace("'", "''"), txtBFName.Text.Replace("'", "''"),
                         txtBMName.Text.Replace("'", "''"), txtBLName.Text.Replace("'", "''"), vBRelId, vBQuliId,
                         vBDOBDt, vBAge, vBOcupId, ddlBGend.SelectedValue, ddlBCaste.SelectedValue,
                         ddlBRelg.SelectedValue, txtBHouNo.Text.Replace("'", "''"),
                         txtBStreet.Text.Replace("'", "''"), vBVilgId, txtBWardNo.Text.Replace("'", "''"),
                         txtBPOff.Text.Replace("'", "''"), txtBPin.Text, txtBMobNo.Text, txtBPhNo.Text,
                         vBAddProfId, txtBAddPrfNo.Text.Replace("'", "''"), vBIdProfId, txtBIdntPrfNo.Text.Replace("'", "''"),
                         txtMetDay.Text, ddlCenter.SelectedValue, ddlGroup.SelectedValue,
                         vIncAmt, vExpAmt, vHHoldId, vPjMeetDt, ddlClTyp.SelectedValue, vClDt, ddlPvLine.SelectedValue, txtBPLNo.Text, vNoOfDpndnt,
                         txtRemarks.Text, txtBankName.Text, txtBranch.Text, txtAccNo.Text,
                         txtIFSC.Text, "N", vBrCode, this.UserID, "Save", vCGTYN, txtMemNamePBook.Text, vMaidenNmF, vMaidenNmM, vMaidenNmL, vMemAddr,
                         vCoBrwrAddr, vXmlAsset, Convert.ToDouble(txtFamilyInc.Text), Convert.ToDouble(txtSelfInc.Text), Convert.ToDouble(txtOtherInc.Text),
                         Convert.ToDouble(txtTotInc.Text), Convert.ToDouble(txtHsRntAmt.Text), Convert.ToDouble(txtFdAmt.Text), Convert.ToDouble(txtEduAmt.Text), Convert.ToDouble(txtMedAmt.Text), Convert.ToDouble(txtLnInsAmt.Text)
                         , Convert.ToDouble(txtFuelExp.Text), Convert.ToDouble(txtElecExp.Text), Convert.ToDouble(txtTransExp.Text), Convert.ToDouble(txtOtherExp.Text == "" ? "0" : txtOtherExp.Text), Convert.ToDouble(txtTotExp.Text), Convert.ToDouble(txtSurplus.Text),
                         gblFuction.setDate(Session[gblValue.LoginDate].ToString()), txtGuarName.Text, txtGuarLName.Text, Convert.ToInt32(ddlGuarRel.SelectedValue), ddlGuarGen.SelectedValue, txtGuarDOB.Text, Convert.ToInt32(txtGuarAge.Text), txtCustId.Text, txtSavingsAcNo.Text,
                         "0", Convert.ToInt32(ddlAddrType.SelectedValue), txtLandMark.Text.ToUpper(), txtArea.Text.ToUpper(), txtEmail.Text, Convert.ToInt32(ddlCommAddrType.SelectedValue), txtCommHouseNo.Text, txtCommSt.Text, txtCommLandmark.Text,
                         txtCommArea.Text, Convert.ToInt32(ddlCommVill.SelectedValue), txtCommSubDist.Text, txtCommPost.Text, txtCommPin.Text, txtCommMob.Text, txtCommPhone.Text, txtCommEmail.Text, Convert.ToInt32(ddlAreaCategory.SelectedValue), Convert.ToDouble(txtStayYear.Text), Convert.ToInt32(ddlIdProof3.SelectedValue),
                         txtIdProof3.Text, txtBLandmark.Text, txtBArea.Text, txtBEmail.Text, Convert.ToInt32(ddlAccType.SelectedValue), Convert.ToInt32(txtNoOfHouseMember.Text), Convert.ToInt32(txtNoOfChild.Text),
                         Convert.ToDouble(txtBranchDistance.Text), Convert.ToDouble(txtCollCenterDistance.Text), ddlStat.SelectedItem.Text, txtDist.Text,
                         vMemBusTypeId, vMemEMailId, vCoAppMaritalStat, vOtherIncomeSrcId, vXmlEarningMemDtl, vDeclIncome, vIncFrequency, vCoAppBusTypeId,
                         vCoAppDeclIncome, vCoAppIncFrequency, vBusActvId, vCoAppBusActvId, vMHF_RelationId, ddlMinority.SelectedValue, txtBVillage.Text,
                         txtBDist.Text, Convert.ToInt32(ddlBStat.SelectedValue), Convert.ToString(ddlAbledYN.SelectedValue), Convert.ToInt32(ddlSpclAbled.SelectedValue));

                    if (vErr > 0)
                    {
                        ViewState["EoId"] = vMemId;
                        ViewState["MemId"] = vMemId;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oMem = new CMember();
                    oGbl = new CGblIdGenerator();
                    /////////sourav
                    if (cbDrp.Checked == false)
                    {
                        vRec = oGbl.ChkDuplicate("MemberMst", "M_Mobile", txtMob.Text.Replace("'", "''"), "", "", "MemberID", vMemId, "Edit");
                        if (vRec > 0)
                        {
                            string MobUser = "";
                            MobUser = oGbl.getMobileUser(txtMob.Text.ToString());
                            gblFuction.MsgPopup(MobUser.ToString() + " Already Access this Mobile Number");
                            return false;
                        }
                        vRec = oGbl.ChkDuplicate("MemberMst", "AccNo", txtAccNo.Text.Replace("'", "''"), "", "", "MemberID", vMemId, "Edit");
                        if (vRec > 0)
                        {
                            string AccNoUser = "";
                            AccNoUser = oGbl.getAccountUser(txtAccNo.Text.ToString());
                            gblFuction.MsgPopup(AccNoUser.ToString() + " Already Access this Account Number");
                            return false;
                        }
                        if (txtIdntPrfNo.Text != "")
                        {
                            oMem = new CMember();
                            vMsg = oMem.ChkIDProf_Edit(vMIdProfId == 1 ? lblAadhar.Value : txtIdntPrfNo.Text, vMemId);
                            if (vMsg != "")
                            {
                                gblFuction.AjxMsgPopup(vMsg);
                                txtIdntPrfNo.Text = "";
                                txtIdntPrfNo.Focus();
                                return false;
                            }
                        }
                        if (txtAddPrfNo.Text != "")
                        {
                            oMem = new CMember();
                            vMsg = oMem.ChkIDProf_Edit(vMAddProfId == 1 ? lblAadhar.Value : txtAddPrfNo.Text, vMemId);
                            if (vMsg != "")
                            {
                                gblFuction.AjxMsgPopup(vMsg);
                                txtIdntPrfNo.Text = "";
                                txtIdntPrfNo.Focus();
                                return false;
                            }
                        }
                        if (txtIdProof3.Text != "")
                        {
                            oMem = new CMember();
                            vMsg = oMem.ChkIDProf_Edit(Convert.ToInt32(ddlIdProof3.SelectedValue) == 1 ? lblAadhar.Value : txtIdProof3.Text, vMemId);
                            if (vMsg != "")
                            {
                                gblFuction.AjxMsgPopup(vMsg);
                                txtIdProof3.Text = "";
                                txtIdProof3.Focus();
                                return false;
                            }
                        }
                        if (txtBIdntPrfNo.Text != "")
                        {
                            oMem = new CMember();
                            vMsg = oMem.ChkIDProf_Edit(vBIdProfId == 1 ? lblBAadhar.Value : txtBIdntPrfNo.Text, vMemId);
                            if (vMsg != "")
                            {
                                gblFuction.AjxMsgPopup(vMsg);
                                txtBIdntPrfNo.Text = "";
                                txtBIdntPrfNo.Focus();
                                return false;
                            }
                        }
                        if (txtBAddPrfNo.Text != "")
                        {
                            oMem = new CMember();
                            vMsg = oMem.ChkIDProf_Edit(vBAddProfId == 1 ? lblBAadhar.Value : txtBAddPrfNo.Text, vMemId);
                            if (vMsg != "")
                            {
                                gblFuction.AjxMsgPopup(vMsg);
                                txtBAddPrfNo.Text = "";
                                txtBAddPrfNo.Focus();
                                return false;
                            }
                        }
                    }

                    dt = oMem.ChkMemberByLoanId(vMemId);
                    if (dt.Rows.Count == 0)
                    {
                        vErr = oGbl.ChkDeleteString(vMemId, "MemberID", "LoanMst");
                        if (vErr > 0 && cbDrp.Checked == true)
                        {
                            gblFuction.MsgPopup("This Member has active Loan, Dropout is Not Allowed");
                            return false;
                        }

                        vMsg = oGbl.ChkforOpenApplication(vMemId);
                        if (cbDrp.Checked == true)
                        {
                            if (vMsg != "")
                            {
                                gblFuction.MsgPopup(vMsg);
                                return false;
                            }
                        }

                        if (this.RoleId != 1 && this.RoleId != 5 && this.RoleId != 10)
                        {

                        }
                        /////////////////////////////////////////////////////////
                        if (cbDrp.Checked == true)
                        {

                        }
                        //////////////////////////////////////////////////////txtAddPrfNo
                        /////////sourav
                        //else if (this.RoleId != 1 && vErr > 0 && cbDrp.Checked == false)
                        //{
                        //    gblFuction.MsgPopup("This Member has active Loan, Edit is Not Allowed");
                        //    return false;
                        //}

                        vErr = oMem.SaveMember(ref vMemId, vAdmDt, txtFName.Text.Replace("'", "''"),
                        txtMName.Text.Replace("'", "''"), txtLName.Text.Replace("'", "''"),
                        vMM_Stat, vQuliId, txtHFName.Text.Replace("'", "''"), vChkFath, vDOBDt, vMAge,
                        vMOcupId, ddlGend.SelectedValue, ddlRelg.SelectedValue, ddlCaste.SelectedValue,
                        txtHouNo.Text.Replace("'", "''"), txtStName.Text.Replace("'", "''"), txtVillg.Text,
                        txtWardNo.Text.Replace("'", "''"), txtPOff.Text.Replace("'", "''"), txtPin.Text,
                        txtMob.Text, txtPhNo.Text, vMAddProfId, vMAddProfId == 1 ? lblAadhar.Value : txtAddPrfNo.Text.Replace("'", "''"),
                        vMIdProfId, vMIdProfId == 1 ? lblAadhar.Value : txtIdntPrfNo.Text.Replace("'", "''"),
                        txtBFName.Text.Replace("'", "''"),
                        txtBMName.Text.Replace("'", "''"), txtBLName.Text.Replace("'", "''"), vBRelId, vBQuliId,
                        vBDOBDt, vBAge, vBOcupId, ddlBGend.SelectedValue, ddlBCaste.SelectedValue,
                        ddlBRelg.SelectedValue, txtBHouNo.Text.Replace("'", "''"),
                        txtBStreet.Text.Replace("'", "''"), vBVilgId, txtBWardNo.Text.Replace("'", "''"),
                        txtBPOff.Text.Replace("'", "''"), txtBPin.Text, txtBMobNo.Text, txtBPhNo.Text,
                        vBAddProfId, vBAddProfId == 1 ? lblBAadhar.Value : txtBAddPrfNo.Text.Replace("'", "''"),
                        vBIdProfId, vBIdProfId == 1 ? lblBAadhar.Value : txtBIdntPrfNo.Text.Replace("'", "''"),
                        txtMetDay.Text, ddlCenter.SelectedValue, ddlGroup.SelectedValue,
                        vIncAmt, vExpAmt, vHHoldId, vPjMeetDt, ddlClTyp.SelectedValue, vClDt, ddlPvLine.SelectedValue, txtBPLNo.Text, vNoOfDpndnt,
                        txtRemarks.Text, txtBankName.Text, txtBranch.Text, txtAccNo.Text,
                        txtIFSC.Text, "N", vBrCode, Convert.ToInt32(Session[gblValue.UserId]), "Edit", vCGTYN, txtMemNamePBook.Text, vMaidenNmF, vMaidenNmM, vMaidenNmL, vMemAddr,
                        vCoBrwrAddr, vXmlAsset, Convert.ToDouble(txtFamilyInc.Text), Convert.ToDouble(txtSelfInc.Text), Convert.ToDouble(txtOtherInc.Text),
                        Convert.ToDouble(txtTotInc.Text), Convert.ToDouble(txtHsRntAmt.Text), Convert.ToDouble(txtFdAmt.Text), Convert.ToDouble(txtEduAmt.Text), Convert.ToDouble(txtMedAmt.Text), Convert.ToDouble(txtLnInsAmt.Text)
                        , Convert.ToDouble(txtFuelExp.Text), Convert.ToDouble(txtElecExp.Text), Convert.ToDouble(txtTransExp.Text), Convert.ToDouble(txtOtherExp.Text == "" ? "0" : txtOtherExp.Text), Convert.ToDouble(txtTotExp.Text), Convert.ToDouble(txtSurplus.Text),
                        gblFuction.setDate(Session[gblValue.LoginDate].ToString()), txtGuarName.Text, txtGuarLName.Text, Convert.ToInt32(ddlGuarRel.SelectedValue), ddlGuarGen.SelectedValue, txtGuarDOB.Text, Convert.ToInt32(txtGuarAge.Text), txtCustId.Text, txtSavingsAcNo.Text,
                        "0", Convert.ToInt32(ddlAddrType.SelectedValue), txtLandMark.Text.ToUpper(), txtArea.Text.ToUpper(), txtEmail.Text, Convert.ToInt32(ddlCommAddrType.SelectedValue), txtCommHouseNo.Text, txtCommSt.Text, txtCommLandmark.Text,
                        txtCommArea.Text, Convert.ToInt32(ddlCommVill.SelectedValue), txtCommSubDist.Text, txtCommPost.Text, txtCommPin.Text, txtCommMob.Text, txtCommPhone.Text, txtCommEmail.Text, Convert.ToInt32(ddlAreaCategory.SelectedValue), Convert.ToDouble(txtStayYear.Text == "" ? "0" : txtStayYear.Text), Convert.ToInt32(ddlIdProof3.SelectedValue),
                        Convert.ToInt32(ddlIdProof3.SelectedValue) == 1 ? lblAadhar.Value : txtIdProof3.Text, txtBLandmark.Text, txtBArea.Text, txtBEmail.Text, Convert.ToInt32(ddlAccType.SelectedValue), Convert.ToInt32(txtNoOfHouseMember.Text == "" ? "0" : txtNoOfHouseMember.Text), Convert.ToInt32(txtNoOfChild.Text == "" ? "0" : txtNoOfChild.Text),
                        Convert.ToDouble(txtBranchDistance.Text == "" ? "0" : txtBranchDistance.Text), Convert.ToDouble(txtCollCenterDistance.Text == "" ? "0" : txtCollCenterDistance.Text), ddlStat.SelectedItem.Text, txtDist.Text,
                        vMemBusTypeId, vMemEMailId, vCoAppMaritalStat, vOtherIncomeSrcId, vXmlEarningMemDtl, vDeclIncome, vIncFrequency, vCoAppBusTypeId, vCoAppDeclIncome,
                        vCoAppIncFrequency, vBusActvId, vCoAppBusActvId, vMHF_RelationId, ddlMinority.SelectedValue, txtBVillage.Text, txtBDist.Text,
                        Convert.ToInt32(ddlBStat.SelectedValue), Convert.ToString(ddlAbledYN.SelectedValue), Convert.ToInt32(ddlSpclAbled.SelectedValue));
                        if (vErr > 0)
                        {
                            //string extension="",fileName="";                  

                            try
                            {
                                if (fuMemberKYC.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuMemberKYC, vMemId, "PassbookImage", "Edit", "N", pathMember);
                                    }
                                    else
                                    {
                                        byte[] PassBookImg = ConvertFileToByteArray(fuMemberKYC.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(PassBookImg, 0.8), "PassbookImage.png", vMemId, MemberBucket, MinioUrl);
                                    }

                                }
                                if (fuMemPhoto.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuMemPhoto, EnqId, "MemberPhoto", "Edit", "N", path);
                                        SaveMemberImages(fuMemPhoto, EnqId, "MemberPhoto", "Edit", "N", pathG);
                                    }
                                    else
                                    {
                                        byte[] MemberImg = ConvertFileToByteArray(fuMemPhoto.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(MemberImg, 0.8), "MemberPhoto.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }

                                if (fuIdProof1Front.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuIdProof1Front, EnqId, "IDProofImage", "Edit", "N", path);
                                        SaveMemberImages(fuIdProof1Front, EnqId, "IDProofImage", "Edit", "N", pathG);
                                    }
                                    else
                                    {
                                        byte[] IdProof1Front = ConvertFileToByteArray(fuIdProof1Front.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(IdProof1Front, 0.8), "IDProofImage.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }
                                if (fuIdProof1Back.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuIdProof1Back, EnqId, "IDProofImageBack", "Edit", "N", path);
                                        SaveMemberImages(fuIdProof1Back, EnqId, "IDProofImageBack", "Edit", "N", pathG);
                                    }
                                    else
                                    {
                                        byte[] IDProofImageBack = ConvertFileToByteArray(fuIdProof1Back.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(IDProofImageBack, 0.8), "IDProofImageBack.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }

                                if (fuIdProof2Front.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuIdProof2Front, EnqId, "AddressProofImage", "Edit", "N", path);
                                        SaveMemberImages(fuIdProof2Front, EnqId, "AddressProofImage", "Edit", "N", pathG);
                                    }
                                    else
                                    {
                                        byte[] AddressProofImage = ConvertFileToByteArray(fuIdProof2Front.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(AddressProofImage, 0.8), "AddressProofImage.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }
                                if (fuIdProof2Back.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuIdProof2Back, EnqId, "AddressProofImageBack", "Edit", "N", path);
                                        SaveMemberImages(fuIdProof2Back, EnqId, "AddressProofImageBack", "Edit", "N", pathG);
                                    }
                                    else
                                    {
                                        byte[] AddressProofImageBack = ConvertFileToByteArray(fuIdProof2Back.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(AddressProofImageBack, 0.8), "AddressProofImageBack.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }
                                if (fuIdProof3Front.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuIdProof3Front, EnqId, "AddressProofImage2", "Edit", "N", path);
                                        SaveMemberImages(fuIdProof3Front, EnqId, "AddressProofImage2", "Edit", "N", pathG);
                                    }
                                    else
                                    {
                                        byte[] AddressProofImage2 = ConvertFileToByteArray(fuIdProof3Front.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(AddressProofImage2, 0.8), "AddressProofImage2.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }
                                if (fuIdProof3Back.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuIdProof3Back, EnqId, "AddressProofImage2Back", "Edit", "N", path);
                                        SaveMemberImages(fuIdProof3Back, EnqId, "AddressProofImage2Back", "Edit", "N", pathG);
                                    }
                                    else
                                    {
                                        byte[] AddressProofImage2Back = ConvertFileToByteArray(fuIdProof3Back.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(AddressProofImage2Back, 0.8), "AddressProofImage2Back.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }
                                //if (fuCoAppPhoto.HasFile)
                                //{
                                //    string vMessage = SaveMemberImages(fuCoAppPhoto, EnqId, "CoAppPhoto", "Edit", "N", path);
                                //}
                                //if (fuCoAppIdProof1Front.HasFile)
                                //{
                                //    string vMessage = SaveMemberImages(fuCoAppIdProof1Front, EnqId, "CoAppIDProofImage", "Edit", "N", path);
                                //}
                                //if (fuCoAppIdProof1Back.HasFile)
                                //{
                                //    string vMessage = SaveMemberImages(fuCoAppIdProof1Back, EnqId, "CoAppIDProofImageBack", "Edit", "N", path);
                                //}
                                if (fuSelfie.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuSelfie, EnqId, "FrontSelfeImage", "Edit", "N", path);
                                    }
                                    else
                                    {
                                        byte[] FrontSelfeImage = ConvertFileToByteArray(fuSelfie.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(FrontSelfeImage, 0.8), "FrontSelfeImage.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                            gblFuction.MsgPopup(gblMarg.EditMsg);
                            clearMemPhoto();
                            memberKYC(EnqId);
                            memberPassbook(vMemId);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                            vResult = false;
                        }
                        //}
                    }
                    else
                    {
                        vErr = oGbl.ChkDeleteString(vMemId, "MemberID", "LoanMst");
                        if (vErr > 0 && cbDrp.Checked == true)
                        {
                            gblFuction.MsgPopup("This Member has active Loan, Dropout is Not Allowed");
                            return false;
                        }

                        if (this.RoleId != 1 && this.RoleId != 5 && this.RoleId != 10)
                        {

                        }
                        else if (this.RoleId != 1 && vErr > 0 && cbDrp.Checked == false)
                        {
                            gblFuction.MsgPopup("This Member has active Loan, Edit is Not Allowed");
                            return false;
                        }

                        vErr = oMem.SaveMember(ref vMemId, vAdmDt, txtFName.Text.Replace("'", "''"),
                         txtMName.Text.Replace("'", "''"), txtLName.Text.Replace("'", "''"),
                         vMM_Stat, vQuliId, txtHFName.Text.Replace("'", "''"), vChkFath, vDOBDt, vMAge,
                         vMOcupId, ddlGend.SelectedValue, ddlRelg.SelectedValue, ddlCaste.SelectedValue,
                         txtHouNo.Text.Replace("'", "''"), txtStName.Text.Replace("'", "''"), txtVillg.Text,
                         txtWardNo.Text.Replace("'", "''"), txtPOff.Text.Replace("'", "''"), txtPin.Text,
                         txtMob.Text, txtPhNo.Text, vMAddProfId, vMAddProfId == 1 ? lblAadhar.Value : txtAddPrfNo.Text.Replace("'", "''"),
                         vMIdProfId, vMIdProfId == 1 ? lblAadhar.Value : txtIdntPrfNo.Text.Replace("'", "''"), txtBFName.Text.Replace("'", "''"),
                         txtBMName.Text.Replace("'", "''"), txtBLName.Text.Replace("'", "''"), vBRelId, vBQuliId,
                         vBDOBDt, vBAge, vBOcupId, ddlBGend.SelectedValue, ddlBCaste.SelectedValue,
                         ddlBRelg.SelectedValue, txtBHouNo.Text.Replace("'", "''"),
                         txtBStreet.Text.Replace("'", "''"), vBVilgId, txtBWardNo.Text.Replace("'", "''"),
                         txtBPOff.Text.Replace("'", "''"), txtBPin.Text, txtBMobNo.Text, txtBPhNo.Text,
                         vBAddProfId, vBAddProfId == 1 ? lblBAadhar.Value : txtBAddPrfNo.Text.Replace("'", "''"),
                         vBIdProfId, vBIdProfId == 1 ? lblBAadhar.Value : txtBIdntPrfNo.Text.Replace("'", "''"),
                         txtMetDay.Text, ddlCenter.SelectedValue, ddlGroup.SelectedValue,
                         vIncAmt, vExpAmt, vHHoldId, vPjMeetDt, ddlClTyp.SelectedValue, vClDt, ddlPvLine.SelectedValue, txtBPLNo.Text, vNoOfDpndnt,
                         txtRemarks.Text, txtBankName.Text, txtBranch.Text, txtAccNo.Text,
                         txtIFSC.Text, "N", vBrCode, this.UserID, "Edit", vCGTYN, txtMemNamePBook.Text, vMaidenNmF, vMaidenNmM, vMaidenNmL, vMemAddr,
                         vCoBrwrAddr, vXmlAsset
                         , Convert.ToDouble(txtFamilyInc.Text == "" ? "0" : txtFamilyInc.Text)
                         , Convert.ToDouble(txtSelfInc.Text == "" ? "0" : txtSelfInc.Text)
                         , Convert.ToDouble(txtOtherInc.Text == "" ? "0" : txtOtherInc.Text)
                         , Convert.ToDouble(txtTotInc.Text == "" ? "0" : txtTotInc.Text)
                         , Convert.ToDouble(txtHsRntAmt.Text == "" ? "0" : txtHsRntAmt.Text)
                         , Convert.ToDouble(txtFdAmt.Text == "" ? "0" : txtFdAmt.Text)
                         , Convert.ToDouble(txtEduAmt.Text == "" ? "0" : txtEduAmt.Text)
                         , Convert.ToDouble(txtMedAmt.Text == "" ? "0" : txtMedAmt.Text)
                         , Convert.ToDouble(txtLnInsAmt.Text == "" ? "0" : txtLnInsAmt.Text)
                         , Convert.ToDouble(txtFuelExp.Text == "" ? "0" : txtFuelExp.Text)
                         , Convert.ToDouble(txtElecExp.Text == "" ? "0" : txtElecExp.Text)
                         , Convert.ToDouble(txtTransExp.Text == "" ? "0" : txtTransExp.Text)
                         , Convert.ToDouble(txtOtherExp.Text == "" ? "0" : txtOtherExp.Text)
                         , Convert.ToDouble(txtTotExp.Text == "" ? "0" : txtTotExp.Text)
                         , Convert.ToDouble(txtSurplus.Text == "" ? "0" : txtSurplus.Text),
                         gblFuction.setDate(Session[gblValue.LoginDate].ToString()), txtGuarName.Text, txtGuarLName.Text
                         , Convert.ToInt32(ddlGuarRel.SelectedValue == "" ? "0" : ddlGuarRel.SelectedValue)
                         , ddlGuarGen.SelectedValue, txtGuarDOB.Text
                         , Convert.ToInt32(txtGuarAge.Text == "" ? "0" : txtGuarAge.Text)
                         , txtCustId.Text, txtSavingsAcNo.Text,
                         "0"
                         , Convert.ToInt32(ddlAddrType.SelectedValue == "" ? "0" : ddlAddrType.SelectedValue)
                         , txtLandMark.Text.ToUpper(), txtArea.Text.ToUpper(), txtEmail.Text
                         , Convert.ToInt32(ddlCommAddrType.SelectedValue == "" ? "0" : ddlCommAddrType.SelectedValue)
                         , txtCommHouseNo.Text, txtCommSt.Text, txtCommLandmark.Text, txtCommArea.Text
                         , Convert.ToInt32(ddlCommVill.SelectedValue == "" ? "0" : ddlCommVill.SelectedValue)
                         , txtCommSubDist.Text, txtCommPost.Text, txtCommPin.Text, txtCommMob.Text, txtCommPhone.Text, txtCommEmail.Text
                         , Convert.ToInt32(ddlAreaCategory.SelectedValue == "" ? "0" : ddlAreaCategory.SelectedValue)
                         , Convert.ToDouble(txtStayYear.Text == "" ? "0" : txtStayYear.Text)
                         , Convert.ToInt32(ddlIdProof3.SelectedValue == "" ? "0" : ddlIdProof3.SelectedValue)
                         , Convert.ToInt32(ddlIdProof3.SelectedValue) == 1 ? lblAadhar.Value : txtIdProof3.Text
                         , txtBLandmark.Text
                         , txtBArea.Text
                         , txtBEmail.Text
                         , Convert.ToInt32(ddlAccType.SelectedValue == "" ? "0" : ddlAccType.SelectedValue)
                         , Convert.ToInt32(txtNoOfHouseMember.Text == "" ? "0" : txtNoOfHouseMember.Text)
                         , Convert.ToInt32(txtNoOfChild.Text == "" ? "0" : txtNoOfChild.Text)
                         , Convert.ToDouble(txtBranchDistance.Text == "" ? "0" : txtBranchDistance.Text)
                         , Convert.ToDouble(txtCollCenterDistance.Text == "" ? "0" : txtCollCenterDistance.Text)
                         , ddlStat.SelectedItem.Text, txtDist.Text
                         , vMemBusTypeId, vMemEMailId, vCoAppMaritalStat, vOtherIncomeSrcId,
                         vXmlEarningMemDtl, vDeclIncome, vIncFrequency, vCoAppBusTypeId, vCoAppDeclIncome, vCoAppIncFrequency, vBusActvId, vCoAppBusActvId,
                         vMHF_RelationId, ddlMinority.SelectedValue, txtBVillage.Text, txtBDist.Text,
                         Convert.ToInt32(ddlBStat.SelectedValue), Convert.ToString(ddlAbledYN.SelectedValue), Convert.ToInt32(ddlSpclAbled.SelectedValue));

                        if (vErr > 0)
                        {
                            try
                            {
                                if (fuMemberKYC.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuMemberKYC, vMemId, "PassbookImage", "Edit", "N", pathMember);
                                    }
                                    else
                                    {
                                        byte[] PassBookImg = ConvertFileToByteArray(fuMemberKYC.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(PassBookImg, 0.8), "PassbookImage.png", vMemId, MemberBucket, MinioUrl);
                                    }
                                }
                                if (fuMemPhoto.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuMemPhoto, EnqId, "MemberPhoto", "Edit", "N", path);
                                        SaveMemberImages(fuMemPhoto, EnqId, "MemberPhoto", "Edit", "N", pathG);
                                    }
                                    else
                                    {
                                        byte[] MemberImg = ConvertFileToByteArray(fuMemPhoto.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(MemberImg, 0.8), "MemberPhoto.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }

                                if (fuIdProof1Front.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuIdProof1Front, EnqId, "IDProofImage", "Edit", "N", path);
                                        SaveMemberImages(fuIdProof1Front, EnqId, "IDProofImage", "Edit", "N", pathG);
                                    }
                                    else
                                    {
                                        byte[] IdProof1Front = ConvertFileToByteArray(fuIdProof1Front.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(IdProof1Front, 0.8), "IDProofImage.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }
                                if (fuIdProof1Back.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuIdProof1Back, EnqId, "IDProofImageBack", "Edit", "N", path);
                                        SaveMemberImages(fuIdProof1Back, EnqId, "IDProofImageBack", "Edit", "N", pathG);
                                    }
                                    else
                                    {
                                        byte[] IDProofImageBack = ConvertFileToByteArray(fuIdProof1Back.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(IDProofImageBack, 0.8), "IDProofImageBack.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }

                                if (fuIdProof2Front.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuIdProof2Front, EnqId, "AddressProofImage", "Edit", "N", path);
                                        SaveMemberImages(fuIdProof2Front, EnqId, "AddressProofImage", "Edit", "N", pathG);
                                    }
                                    else
                                    {
                                        byte[] AddressProofImage = ConvertFileToByteArray(fuIdProof2Front.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(AddressProofImage, 0.8), "AddressProofImage.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }
                                if (fuIdProof2Back.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuIdProof2Back, EnqId, "AddressProofImageBack", "Edit", "N", path);
                                        SaveMemberImages(fuIdProof2Back, EnqId, "AddressProofImageBack", "Edit", "N", pathG);
                                    }
                                    else
                                    {
                                        byte[] AddressProofImageBack = ConvertFileToByteArray(fuIdProof2Back.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(AddressProofImageBack, 0.8), "AddressProofImageBack.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }
                                if (fuIdProof3Front.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuIdProof3Front, EnqId, "AddressProofImage2", "Edit", "N", path);
                                        SaveMemberImages(fuIdProof3Front, EnqId, "AddressProofImage2", "Edit", "N", pathG);
                                    }
                                    else
                                    {
                                        byte[] AddressProofImage2 = ConvertFileToByteArray(fuIdProof3Front.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(AddressProofImage2, 0.8), "AddressProofImage2.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }
                                if (fuIdProof3Back.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuIdProof3Back, EnqId, "AddressProofImage2Back", "Edit", "N", path);
                                        SaveMemberImages(fuIdProof3Back, EnqId, "AddressProofImage2Back", "Edit", "N", pathG);
                                    }
                                    else
                                    {
                                        byte[] AddressProofImage2Back = ConvertFileToByteArray(fuIdProof3Back.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(AddressProofImage2Back, 0.8), "AddressProofImage2Back.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }

                                if (fuSelfie.HasFile)
                                {
                                    if (MinioYN == "N")
                                    {
                                        string vMessage = SaveMemberImages(fuSelfie, EnqId, "FrontSelfeImage", "Edit", "N", path);
                                    }
                                    else
                                    {
                                        byte[] FrontSelfeImage = ConvertFileToByteArray(fuSelfie.PostedFile);
                                        string vMessage = UploadFileMinio(ResizeImage(FrontSelfeImage, 0.8), "FrontSelfeImage.png", EnqDate + EnqId, InitialBucket, MinioUrl);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                            gblFuction.MsgPopup(gblMarg.EditMsg);
                            clearMemPhoto();
                            memberKYC(EnqId);
                            memberPassbook(vMemId);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                            vResult = false;
                        }
                    }
                }
                else if (Mode == "Delete")
                {
                    oMem = new CMember();
                    oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDeleteString(vMemId, "MemberID", "LoanMst");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("The Member has Loan, you can not delete the Member.");
                    //    return false;
                    //}
                    vErr = oMem.SaveMember(ref vMemId, vAdmDt, txtFName.Text.Replace("'", "''"),
                         txtMName.Text.Replace("'", "''"), txtLName.Text.Replace("'", "''"),
                         vMM_Stat, vQuliId, txtHFName.Text.Replace("'", "''"), vChkFath, vDOBDt, vMAge,
                         vMOcupId, ddlGend.SelectedValue, ddlRelg.SelectedValue, ddlCaste.SelectedValue,
                         txtHouNo.Text.Replace("'", "''"), txtStName.Text.Replace("'", "''"), txtVillg.Text,
                         txtWardNo.Text.Replace("'", "''"), txtPOff.Text.Replace("'", "''"), txtPin.Text,
                         txtMob.Text, txtPhNo.Text, vMAddProfId, vMAddProfId == 1 ? lblAadhar.Value : txtAddPrfNo.Text.Replace("'", "''"),
                         vMIdProfId, vMIdProfId == 1 ? lblAadhar.Value : txtIdntPrfNo.Text.Replace("'", "''"), txtBFName.Text.Replace("'", "''"),
                         txtBMName.Text.Replace("'", "''"), txtBLName.Text.Replace("'", "''"), vBRelId, vBQuliId,
                         vBDOBDt, vBAge, vBOcupId, ddlBGend.SelectedValue, ddlBCaste.SelectedValue,
                         ddlBRelg.SelectedValue, txtBHouNo.Text.Replace("'", "''"),
                         txtBStreet.Text.Replace("'", "''"), vBVilgId, txtBWardNo.Text.Replace("'", "''"),
                         txtBPOff.Text.Replace("'", "''"), txtBPin.Text, txtBMobNo.Text, txtBPhNo.Text,
                         vBAddProfId, vBAddProfId == 1 ? lblBAadhar.Value : txtBAddPrfNo.Text.Replace("'", "''"),
                         vBIdProfId, vBIdProfId == 1 ? lblBAadhar.Value : txtBIdntPrfNo.Text.Replace("'", "''"),
                         txtMetDay.Text, ddlCenter.SelectedValue, ddlGroup.SelectedValue,
                         vIncAmt, vExpAmt, vHHoldId, vPjMeetDt, ddlClTyp.SelectedValue, vClDt, ddlPvLine.SelectedValue, txtBPLNo.Text, vNoOfDpndnt,
                         txtRemarks.Text, txtBankName.Text, txtBranch.Text, txtAccNo.Text,
                         txtIFSC.Text, "N", vBrCode, this.UserID, "Delet", vCGTYN, txtMemNamePBook.Text, vMaidenNmF, vMaidenNmM, vMaidenNmL, vMemAddr,
                         vCoBrwrAddr, vXmlAsset, Convert.ToDouble(txtFamilyInc.Text), Convert.ToDouble(txtSelfInc.Text), Convert.ToDouble(txtOtherInc.Text),
                         Convert.ToDouble(txtTotInc.Text), Convert.ToDouble(txtHsRntAmt.Text), Convert.ToDouble(txtFdAmt.Text), Convert.ToDouble(txtEduAmt.Text), Convert.ToDouble(txtMedAmt.Text), Convert.ToDouble(txtLnInsAmt.Text)
                         , Convert.ToDouble(txtFuelExp.Text), Convert.ToDouble(txtElecExp.Text), Convert.ToDouble(txtTransExp.Text), Convert.ToDouble(txtOtherExp.Text == "" ? "0" : txtOtherExp.Text), Convert.ToDouble(txtTotExp.Text), Convert.ToDouble(txtSurplus.Text),
                         gblFuction.setDate(Session[gblValue.LoginDate].ToString()), txtGuarName.Text, txtGuarLName.Text, Convert.ToInt32(ddlGuarRel.SelectedValue), ddlGuarGen.SelectedValue, txtGuarDOB.Text, Convert.ToInt32(txtGuarAge.Text), txtCustId.Text, txtSavingsAcNo.Text,
                         "0", Convert.ToInt32(ddlAddrType.SelectedValue), txtLandMark.Text.ToUpper(), txtArea.Text.ToUpper(), txtEmail.Text, Convert.ToInt32(ddlCommAddrType.SelectedValue), txtCommHouseNo.Text, txtCommSt.Text, txtCommLandmark.Text,
                         txtCommArea.Text, Convert.ToInt32(ddlCommVill.SelectedValue), txtCommSubDist.Text, txtCommPost.Text, txtCommPin.Text, txtCommMob.Text, txtCommPhone.Text, txtCommEmail.Text, Convert.ToInt32(ddlAreaCategory.SelectedValue), Convert.ToDouble(txtStayYear.Text), Convert.ToInt32(ddlIdProof3.SelectedValue),
                         Convert.ToInt32(ddlIdProof3.SelectedValue) == 1 ? lblAadhar.Value : txtIdProof3.Text, txtBLandmark.Text, txtBArea.Text, txtBEmail.Text, Convert.ToInt32(ddlAccType.SelectedValue), Convert.ToInt32(txtNoOfHouseMember.Text), Convert.ToInt32(txtNoOfChild.Text),
                         Convert.ToDouble(txtBranchDistance.Text), Convert.ToDouble(txtCollCenterDistance.Text), ddlStat.SelectedItem.Text, txtDist.Text,
                         vMemBusTypeId, vMemEMailId, vCoAppMaritalStat, vOtherIncomeSrcId, vXmlEarningMemDtl, vDeclIncome, vIncFrequency, vCoAppBusTypeId,
                         vCoAppDeclIncome, vCoAppIncFrequency, vBusActvId, vCoAppBusActvId, vMHF_RelationId, ddlMinority.SelectedValue, txtBVillage.Text,
                         txtBDist.Text, Convert.ToInt32(ddlBStat.SelectedValue), Convert.ToString(ddlAbledYN.SelectedValue), Convert.ToInt32(ddlSpclAbled.SelectedValue));
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.DeleteMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }
            finally
            {
                //oEo = null;
                oGbl = null;
                dt = null;
            }
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvMem_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            DataTable dtMem = null, dtGroup = null, dtCent = null;
            CMember oMem = null;
            string vMemId = "", vCMId = "", vCentId = "";
            TreeNode tnMem = null, tnGroup = null, tnCent = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBranchCode = ddlBranch.SelectedValue;
            DateTime vFrmDt = gblFuction.setDate(txtFromDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vMode = rdbOpt.SelectedValue;

            try
            {
                oMem = new CMember();
                if (e.Node.Value.Substring(0, 2) == "CM")
                {
                    e.Node.ChildNodes.Clear();
                    vCentId = Convert.ToString(e.Node.Value.Substring(2));
                    if (Session[gblValue.BrnchCode].ToString() != "0000")
                    {
                        dtCent = oMem.GetCenterByEoId(vCentId, vBrCode, vToDt);
                    }
                    else
                    {
                        dtCent = oMem.GetCenterByEoId(vCentId, vBranchCode, vToDt);
                    }
                    foreach (DataRow drBr in dtCent.Rows)
                    {
                        tnCent = new TreeNode(Convert.ToString(drBr["Market"]));
                        tnCent.Value = Convert.ToString("CN" + drBr["MarketId"]);
                        e.Node.ChildNodes.Add(tnCent);
                        tnGroup = new TreeNode("No Record");
                        e.Node.SelectAction = TreeNodeSelectAction.None;
                        tnGroup.Value = "0";
                        tnCent.ChildNodes.Add(tnGroup);
                    }
                }
                else if (e.Node.Value.Substring(0, 2) == "CN")
                {
                    e.Node.ChildNodes.Clear();
                    vCentId = Convert.ToString(e.Node.Value.Substring(2));
                    if (Session[gblValue.BrnchCode].ToString() != "0000")
                    {
                        dtGroup = oMem.GetGroupByCenterId(vCentId, vBrCode, vToDt);
                    }
                    else
                    {
                        dtGroup = oMem.GetGroupByCenterId(vCentId, vBranchCode, vToDt);
                    }
                    foreach (DataRow drGrp in dtGroup.Rows)
                    {
                        tnGroup = new TreeNode(Convert.ToString(drGrp["GrpCode"]));
                        tnGroup.Value = "GM" + Convert.ToString(drGrp["GroupId"]);
                        e.Node.ChildNodes.Add(tnGroup);
                        tnMem = new TreeNode("No Record");
                        e.Node.SelectAction = TreeNodeSelectAction.None;
                        tnMem.Value = "MM";
                        tnGroup.ChildNodes.Add(tnMem);
                    }
                }
                else if (e.Node.Value.Substring(0, 2) == "GM")
                {
                    e.Node.ChildNodes.Clear();
                    vCentId = Convert.ToString(e.Node.Value.Substring(2));
                    if (Session[gblValue.BrnchCode].ToString() != "0000")
                    {
                        dtMem = oMem.GetMemListByGroupId(vCentId, vBrCode);
                    }
                    else
                    {
                        dtMem = oMem.GetMemListByGroup_HO(vCentId, vBranchCode, vFrmDt, vToDt, vMode);
                    }
                    foreach (DataRow drMem in dtMem.Rows)
                    {
                        tnMem = new TreeNode(Convert.ToString(drMem["MemberCode"]));
                        tnMem.Value = Convert.ToString("MM" + drMem["MemberId"]);
                        vMemId = Convert.ToString(drMem["MemberId"]);
                        if (Convert.ToString(drMem["Tra_Drop"]) == "C" || Convert.ToString(drMem["Tra_Drop"]) == "N" || Convert.ToString(drMem["Tra_Drop"]) == "U")
                        {
                            tnMem.Text = "<span style='color: red;'>" + Convert.ToString(drMem["MemberCode"]) + "</span>";
                        }
                        else
                        {
                            tnMem.Text = "<span style='color: blue;'>" + Convert.ToString(drMem["MemberCode"]) + "</span>";
                        }
                        tnMem.PopulateOnDemand = false;
                        e.Node.SelectAction = TreeNodeSelectAction.Select;
                        e.Node.ChildNodes.Add(tnMem);
                    }
                }
            }
            finally
            {
                dtMem = null;
                dtGroup = null;
                dtCent = null;
                oMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvMem_SelectedNodeChanged(object sender, EventArgs e)
        {
            string vMemberId = Convert.ToString(tvMem.SelectedNode.Value.Substring(2));
            ViewState["EoId"] = vMemberId;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            FillMemberDtl(vMemberId, vBrCode);
            ddlBRel_SelectedIndexChanged(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pGroupId"></param>
        private void GetGroup(string pGroupId)
        {
            DataTable dt = null, dtCent = null, dtRo = null;
            CMember oMem = null;
            CGblIdGenerator oGb = null;
            try
            {
                oMem = new CMember();
                dt = oMem.GetMemberByGroupId(pGroupId);
                if (dt.Rows.Count > 0)
                {
                    ddlGroup.DataSource = dt;
                    ddlGroup.DataTextField = "GroupName";
                    ddlGroup.DataValueField = "Groupid";
                    ddlGroup.DataBind();
                    ddlGroup.SelectedIndex = ddlGroup.Items.IndexOf(ddlGroup.Items.FindByValue(pGroupId));
                    //Get Center
                    oGb = new CGblIdGenerator();
                    dtCent = oGb.PopComboMIS("S", "N", "AA", "MarketID", "Market", "MarketMSt", dt.Rows[0]["MarketID"].ToString(), "MarketID", "AA", gblFuction.setDate("01/01/1900"), "0000");
                    if (dtCent.Rows.Count > 0)
                    {
                        ddlCenter.DataSource = dtCent;
                        ddlCenter.DataTextField = "Market";
                        ddlCenter.DataValueField = "MarketID";
                        ddlCenter.DataBind();
                        ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["MarketId"].ToString()));
                    }
                    //Get RO
                    oGb = new CGblIdGenerator();
                    dtRo = oGb.PopComboMIS("S", "N", "AA", "Eoid", "EoName", "EoMst", dt.Rows[0]["Eoid"].ToString(), "Eoid", "AA", gblFuction.setDate("01/01/1900"), "0000");
                    if (dtRo.Rows.Count > 0)
                    {
                        ddlRO.DataSource = dtRo;
                        ddlRO.DataTextField = "EoName";
                        ddlRO.DataValueField = "Eoid";
                        ddlRO.DataBind();
                        ddlRO.SelectedIndex = ddlRO.Items.IndexOf(ddlRO.Items.FindByValue(dt.Rows[0]["Eoid"].ToString()));
                    }
                }
            }
            finally
            {
                dt = null;
                dtCent = null;
                dtRo = null;
                oMem = null;
                oGb = null;
            }
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
                    //ddlDist.DataSource = dt;
                    //ddlDist.DataTextField = "DistrictName";
                    //ddlDist.DataValueField = "DistrictId";
                    //ddlDist.DataBind();
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
        /// <param name="pVillageId"></param>
        private void PopBVillDtl(Int32 pVillageId)
        {
            DataTable dt = null;
            CGroup oGb = null;
            try
            {
                if (pVillageId > 0)
                {
                    oGb = new CGroup();
                    dt = oGb.GetVillageDtls(pVillageId);
                    ddlBStat.DataSource = dt;
                    ddlBStat.DataTextField = "StateName";
                    ddlBStat.DataValueField = "StateID";
                    ddlBStat.DataBind();
                    ddlBDist.DataSource = dt;
                    ddlBDist.DataTextField = "DistrictName";
                    ddlBDist.DataValueField = "DistrictId";
                    ddlBDist.DataBind();
                    ddlBBlk.DataSource = dt;
                    ddlBBlk.DataTextField = "BlockName";
                    ddlBBlk.DataValueField = "BlockId";
                    ddlBBlk.DataBind();
                    ddlBMunPanca.DataSource = dt;
                    ddlBMunPanca.DataTextField = "GPName";
                    ddlBMunPanca.DataValueField = "GPId";
                    ddlBMunPanca.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlBStat.Items.Insert(1, oli);
                    ddlBDist.Items.Insert(1, oli);
                    ddlBBlk.Items.Insert(1, oli);
                    ddlBMunPanca.Items.Insert(1, oli);
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
            txtMemID.Enabled = Status;
            txtMemAddr.Enabled = Status;
            txtCoBrwrAddr.Enabled = Status;
            txtPjMeetDt.Enabled = Status;
            cbDrp.Enabled = Status;
            txtDtCl.Enabled = Status;
            ddlClTyp.Enabled = Status;
            txtRemarks.Enabled = Status;
            ddlMinority.Enabled = false;

            txtHFName.Enabled = Status;
            chkFath.Enabled = Status;
            txtDOB.Enabled = Status;
            txtAge.Enabled = false;
            txtHouNo.Enabled = Status;
            txtStName.Enabled = Status;
            txtWardNo.Enabled = Status;

            txtPin.Enabled = Status;
            txtMob.Enabled = Status;
            txtPhNo.Enabled = Status;
            txtPin.Enabled = Status;

            txtBDOBDt.Enabled = Status;
            txtBAge.Enabled = Status;

            txtNoOfDpndnts.Enabled = Status;
            txtIncome.Enabled = Status;
            txtExpnses.Enabled = Status;
            ddlMrySts.Enabled = Status;
            ddlEduc.Enabled = Status;
            ddlOcup.Enabled = Status;
            ddlGend.Enabled = Status;
            ddlRelg.Enabled = Status;
            ddlCaste.Enabled = Status;
            ddlBusType.Enabled = Status;
            txtMemEmail.Enabled = Status;
            ddlCoAppMaritalStat.Enabled = Status;
            // ddlVillg.Enabled = false;
            ddlAddPrf.Enabled = Status;
            ddlIdentyProf.Enabled = Status;
            txtBankName.Enabled = Status;
            txtBranch.Enabled = Status;
            txtAccNo.Enabled = Status;
            txtIFSC.Enabled = Status;
            txtMemNamePBook.Enabled = Status;
            ddlBRel.Enabled = Status;
            ddlBEdu.Enabled = Status;
            ddlBOcup.Enabled = Status;
            ddlBGend.Enabled = false;
            ddlBRelg.Enabled = Status;
            ddlBCaste.Enabled = Status;
            ddlBVillg.Enabled = Status;

            ddlBAddProf.Enabled = Status;
            ddlBIdntyProf.Enabled = Status;
            ddlCenter.Enabled = Status;
            ddlHHoldTyp.Enabled = Status;
            ddlAreaCategory.Enabled = false;

            ddlGroup.Enabled = Status;
            ddlRO.Enabled = Status;
            gvAsset.Enabled = Status;
            txtNetIncome.Enabled = Status;
            txtIncA.Enabled = Status;
            txtAmtA.Enabled = Status;
            txtIncB.Enabled = Status;
            txtAmtB.Enabled = Status;
            txtIncC.Enabled = Status;
            txtAmtC.Enabled = Status;
            txtIncD.Enabled = Status;
            txtAmtD.Enabled = Status;
            txtIncE.Enabled = Status;
            txtAmtE.Enabled = Status;
            txtIncF.Enabled = Status;
            txtAmtF.Enabled = Status;
            txtTotInc.Enabled = Status;
            txtHsRntAmt.Enabled = Status;
            txtEduAmt.Enabled = Status;
            txtMedAmt.Enabled = Status;
            txtLnInsAmt.Enabled = Status;
            txtExA.Enabled = Status;
            txtExAmtA.Enabled = Status;
            txtExB.Enabled = Status;
            txtExAmtB.Enabled = Status;
            txtTotExp.Enabled = Status;
            txtSurplus.Enabled = Status;
            txtFdAmt.Enabled = Status;

            ddlGuarRel.Enabled = false;
            txtGuarDOB.Enabled = false;
            txtGuarAge.Enabled = false;
            ddlGuarGen.Enabled = false;
            txtSavingsAcNo.Enabled = Status;
            txtCustId.Enabled = Status;
            txtMaidenNmF.Enabled = Status;
            txtMaidenNmM.Enabled = Status;
            txtMaidenNmL.Enabled = Status;
            txtIdntPrfNo.Enabled = Status;
            txtAddPrfNo.Enabled = Status;
            txtMetDay.Enabled = Status;
            txtReAccNo.Enabled = Status;
            txtCollCenterDistance.Enabled = Status;
            txtBranchDistance.Enabled = Status;
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
            txtCommEmail.Enabled = Status;
            txtStayYear.Enabled = Status;
            txtMemCommAddr.Enabled = Status;
            ddlAccType.Enabled = Status;

            txtBIdntPrfNo.Enabled = Status;
            txtBAddPrfNo.Enabled = Status;
            txtNoOfHouseMember.Enabled = Status;
            txtNoOfChild.Enabled = Status;
            txtFamilyInc.Enabled = Status;
            txtSelfInc.Enabled = Status;
            ddlOtherIncSrc.Enabled = Status;
            txtOtherInc.Enabled = Status;
            txtFuelExp.Enabled = Status;
            txtElecExp.Enabled = Status;
            txtTransExp.Enabled = Status;
            chkCoAdd.Enabled = Status;
            chkCommAddr.Enabled = Status;
            txtOtherExp.Enabled = Status;
            txtVillg.Enabled = Status;

            txtConfrmAadhar1.Enabled = Status;
            txtConfrmAadhar2.Enabled = Status;
            txtConfrmAadhar3.Enabled = Status;
            txtBConfrmAadhar1.Enabled = Status;
            txtBConfrmAadhar2.Enabled = Status;
            if (ddlPvLine.SelectedIndex > 0)
            {
                if (ddlPvLine.SelectedValue == "BPL")
                    txtBPLNo.Enabled = Status;
                else
                    txtBPLNo.Enabled = false;
            }
            else
            {
                txtBPLNo.Enabled = false;
            }

            txtDeclIncome.Enabled = false;
            ddlIncFrequency.Enabled = false;
            ddlCoAppBusType.Enabled = false;
            txtCoAppDeclIncome.Enabled = false;
            ddlCoAppIncFrequency.Enabled = false;
            gvEarningMember.Enabled = false;
            txtFamilyInc.Enabled = false;
            txtSelfInc.Enabled = false;
            txtTotInc.Enabled = false;
            ddlBusActivity.Enabled = Status;
            ddlCoAppBusActivity.Enabled = Status;
            ddlMHFRelation.Enabled = Status;
            Boolean vStatus = Status;
            vStatus = Convert.ToInt32(Session[gblValue.RoleId]) == 1 ? Status : false;

            ddlIdentyProf.Enabled = (ddlIdentyProf.SelectedValue == "13") ? false : Status;
            ddlAddPrf.Enabled = (ddlAddPrf.SelectedValue == "13") ? false : Status;
            txtIdntPrfNo.Enabled = (ddlIdentyProf.SelectedValue == "13") ? false : Status;
            txtAddPrfNo.Enabled = (ddlAddPrf.SelectedValue == "13") ? false : Status;
            ddlBIdntyProf.Enabled = (ddlBIdntyProf.SelectedValue == "13") ? false : Status;
            txtBIdntPrfNo.Enabled = (ddlBIdntyProf.SelectedValue == "13") ? false : Status;
            ddlBAddProf.Enabled = (ddlBAddProf.SelectedValue == "13") ? false : Status;
            txtBAddPrfNo.Enabled = (ddlBAddProf.SelectedValue == "13") ? false : Status;

            vStatus = Convert.ToInt32(Session[gblValue.RoleId]) == 1 || Convert.ToInt32(Session[gblValue.RoleId]) == 25 ? Status : false;

            txtFName.Enabled = vStatus;
            txtMName.Enabled = vStatus;
            txtLName.Enabled = vStatus;

            txtBFName.Enabled = vStatus;
            txtBMName.Enabled = vStatus;
            txtBLName.Enabled = vStatus;
            txtGuarLName.Enabled = false;
            txtGuarName.Enabled = false;

            txtBHouNo.Enabled = vStatus;
            txtBStreet.Enabled = vStatus;
            txtBLandmark.Enabled = vStatus;
            txtBArea.Enabled = vStatus;
            txtBEmail.Enabled = vStatus;
            txtBVillage.Enabled = vStatus;
            txtBWardNo.Enabled = vStatus;
            txtBPOff.Enabled = vStatus;
            txtBPin.Enabled = vStatus;
            txtBMobNo.Enabled = vStatus;
            txtBPhNo.Enabled = vStatus;
            txtBDist.Enabled = vStatus;
            txtPOff.Enabled = vStatus;
            ddlStat.Enabled = vStatus;
            txtDist.Enabled = vStatus;
            ddlAbledYN.Enabled = Status;
            if (ddlAbledYN.SelectedValue != "Y")
            {
                ddlSpclAbled.Enabled = false;
            }
            else
            {
                ddlSpclAbled.Enabled = Status;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtCoBrwrAddr.Text = "";
            txtMemAddr.Text = "";
            txtMemID.Text = "";
            cbDrp.Checked = false;
            txtDtCl.Text = "";
            ddlClTyp.SelectedIndex = -1;
            txtRemarks.Text = "";
            txtAdmDt.Text = Session[gblValue.LoginDate].ToString();
            txtFName.Text = "";
            txtMName.Text = "";
            txtLName.Text = "";
            txtHFName.Text = "";
            chkFath.Checked = false;
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
            txtBankName.Text = "";
            txtBranch.Text = "";
            //txtAccNo.Text = "";
            txtAccNo.Attributes.Add("value", "");
            txtIFSC.Text = "";
            txtMemNamePBook.Text = "";
            txtBFName.Text = "";
            txtBMName.Text = "";
            txtBLName.Text = "";
            txtNoOfDpndnts.Text = "";
            txtBDOBDt.Text = "";
            txtBAge.Text = "0";
            txtBHouNo.Text = "";
            txtBStreet.Text = "";
            txtBWardNo.Text = "";
            txtBPOff.Text = "";
            txtBPin.Text = "";
            txtBMobNo.Text = "";
            txtBPhNo.Text = "";
            txtAddPrfNo.Text = "";
            txtIdntPrfNo.Text = "";
            txtBAddPrfNo.Text = "";
            txtBIdntPrfNo.Text = "";
            txtIncome.Text = "0";
            txtExpnses.Text = "0";
            txtReAccNo.Text = "";
            ddlMrySts.SelectedIndex = -1;
            ddlEduc.SelectedIndex = -1;
            ddlOcup.SelectedIndex = -1;
            ddlGend.SelectedIndex = -1;
            ddlRelg.SelectedIndex = -1;
            ddlCaste.SelectedIndex = -1;
            ddlBusType.SelectedIndex = -1;
            txtMemEmail.Text = "";
            ddlCoAppMaritalStat.SelectedIndex = -1;
            //ddlVillg.SelectedIndex = -1;
            ddlMuPanc.Items.Clear();
            ddlMuPanc.SelectedIndex = -1;
            ddlBlk.Items.Clear();
            ddlBlk.SelectedIndex = -1;
            //ddlDist.Items.Clear();
            //ddlDist.SelectedIndex = -1;
            ddlStat.Items.Clear();
            ddlStat.SelectedIndex = -1;
            ddlAddPrf.SelectedIndex = -1;
            ddlIdentyProf.SelectedIndex = -1;
            ddlBRel.SelectedIndex = -1;
            ddlBEdu.SelectedIndex = -1;
            ddlBOcup.SelectedIndex = -1;
            ddlBGend.SelectedIndex = -1;
            ddlBRelg.SelectedIndex = -1;
            ddlBCaste.SelectedIndex = -1;
            ddlBVillg.SelectedIndex = -1;
            ddlBMunPanca.Items.Clear();
            ddlBMunPanca.SelectedIndex = -1;
            ddlBBlk.Items.Clear();
            ddlBBlk.SelectedIndex = -1;
            ddlBDist.Items.Clear();
            ddlBDist.SelectedIndex = -1;
            ddlBStat.Items.Clear();
            ddlBStat.SelectedIndex = -1;
            ddlBAddProf.SelectedIndex = -1;
            ddlBIdntyProf.SelectedIndex = -1;
            ddlRO.SelectedIndex = -1;
            ddlCenter.Items.Clear();
            ddlCenter.SelectedIndex = -1;
            ddlGroup.SelectedIndex = -1;
            chkCoAdd.Checked = false;
            ddlHHoldTyp.SelectedIndex = -1;
            hdnApplyCgt.Value = "N";
            txtPjMeetDt.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
            txtNetIncome.Text = "0.00";
            txtIncA.Text = "";
            txtAmtA.Text = "0.00";
            txtIncB.Text = "";
            txtAmtB.Text = "0.00";
            txtIncC.Text = "";
            txtAmtC.Text = "0.00";
            txtIncD.Text = "";
            txtAmtD.Text = "0.00";
            txtIncE.Text = "";
            txtAmtE.Text = "0.00";
            txtIncF.Text = "";
            txtAmtF.Text = "0.00";
            txtTotInc.Text = "0.00";
            txtHsRntAmt.Text = "0.00";
            txtEduAmt.Text = "0.00";
            txtMedAmt.Text = "0.00";
            txtFdAmt.Text = "0.00";
            txtLnInsAmt.Text = "0.00";
            txtExA.Text = "";
            txtExAmtA.Text = "0.00";
            txtExB.Text = "";
            txtExAmtB.Text = "0.00";
            txtTotExp.Text = "0.00";
            txtSurplus.Text = "0.00";
            txtGuarName.Text = "";
            ddlGuarRel.SelectedIndex = -1;
            txtGuarDOB.Text = "";
            txtGuarAge.Text = "0";
            ddlGuarGen.SelectedIndex = -1;
            txtCustId.Text = "";
            txtSavingsAcNo.Text = "";
            txtMetDay.Text = "";
            txtFamilyInc.Text = "0.00";
            txtSelfInc.Text = "0.00";
            ddlOtherIncSrc.SelectedIndex = -1;
            txtOtherInc.Text = "0.00";
            txtFuelExp.Text = "0.00";
            txtElecExp.Text = "0.00";
            txtTransExp.Text = "0.00";
            txtOtherExp.Text = "0.00";
            txtBranchDistance.Text = "";
            txtCollCenterDistance.Text = "";
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
            txtCommEmail.Text = "";
            txtStayYear.Text = "";
            ddlAreaCategory.SelectedIndex = 0;
            txtMemCommAddr.Text = "";
            ddlAccType.SelectedIndex = -1;
            txtBLandmark.Text = "";
            txtBArea.Text = "";
            txtBEmail.Text = "";
            txtNoOfHouseMember.Text = "";
            txtNoOfChild.Text = "";
            ///
            clearMemPhoto();

            txtDeclIncome.Text = "0.00";
            ddlIncFrequency.SelectedIndex = -1;
            ddlCoAppBusType.SelectedIndex = -1;
            txtCoAppDeclIncome.Text = "0.00";
            ddlCoAppIncFrequency.SelectedIndex = -1;
            ddlBusActivity.SelectedIndex = -1;
            ddlCoAppBusActivity.SelectedIndex = -1;

            txtBVillage.Text = "";
            txtBDist.Text = "";
            ddlAbledYN.SelectedIndex = -1;
            ddlSpclAbled.SelectedIndex = -1;
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
            imgMemPassbook.ImageUrl = imgUrl;
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
        protected void txtGuarDOB_TextChanged(object sender, EventArgs e)
        {
            // int Years = CalAge(txtGuarDOB.Text);
            int Years = AgeCount(txtGuarDOB.Text, Convert.ToString(Session[gblValue.LoginDate]));
            if (Years < 18)
            {
                gblFuction.AjxMsgPopup("Nominee age should Greater than 18 Years.");
                txtGuarAge.Text = "0";
                txtGuarDOB.Text = "";
            }
            else if (Years > 59)
            {
                gblFuction.AjxMsgPopup("Nominee age should less than 59 Years.");
                txtGuarAge.Text = "0";
                txtGuarDOB.Text = "";
            }
            else
            {
                if (txtGuarDOB.Text.Length >= 10)
                    txtGuarAge.Text = Years.ToString();
            }
        }

        //public int AgeCalculation(string Dob)
        //{
        //    DateTime vDOB = gblFuction.setDate(Dob);
        //    int Years = new DateTime(DateTime.Now.Subtract(vDOB).Ticks).Year - 1;
        //    return Years;
        //}

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
        protected void cbDrp_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDrp.Checked == true)
            {
                txtDtCl.Text = Session[gblValue.LoginDate].ToString();
                txtDtCl.Enabled = false;
                txtRemarks.Enabled = true;
                hdDropOut.Value = "Y";
            }
            if (cbDrp.Checked == false)
            {
                txtDtCl.Text = "";
                txtDtCl.Enabled = true;
                ddlClTyp.SelectedIndex = -1;
                txtRemarks.Text = "";
                txtRemarks.Enabled = false;
                hdDropOut.Value = "N";
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
        protected void txtIncome_TextChanged(object sender, EventArgs e)
        {
            if (txtIncome.Text != "")
            {
                if (Convert.ToDouble(txtIncome.Text) <= 8333)
                    ddlHHoldTyp.SelectedIndex = ddlHHoldTyp.Items.IndexOf(ddlHHoldTyp.Items.FindByText("Rural"));
                else
                    ddlHHoldTyp.SelectedIndex = ddlHHoldTyp.Items.IndexOf(ddlHHoldTyp.Items.FindByText("Urban"));

                if (Convert.ToDouble(txtIncome.Text) <= 1500)
                    ddlPvLine.SelectedIndex = ddlPvLine.Items.IndexOf(ddlPvLine.Items.FindByText("BPL"));
                else
                    ddlPvLine.SelectedIndex = ddlPvLine.Items.IndexOf(ddlPvLine.Items.FindByText("APL"));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkCoAdd_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCoAdd.Checked == true)
            {
                txtBHouNo.Text = txtHouNo.Text;
                txtBStreet.Text = txtStName.Text;
                txtBWardNo.Text = txtWardNo.Text;
                txtBPOff.Text = txtPOff.Text;
                txtBPin.Text = txtPin.Text;
                // ddlBVillg.SelectedIndex = ddlVillg.SelectedIndex;
                if (ddlBVillg.SelectedIndex >= 0)
                {
                    PopAllAgainstVillage();
                    ddlBMunPanca.SelectedIndex = ddlMuPanc.SelectedIndex;
                    ddlBBlk.SelectedIndex = ddlBlk.SelectedIndex;
                    //ddlBDist.SelectedIndex = ddlDist.SelectedIndex;
                    ddlBStat.SelectedIndex = ddlStat.SelectedIndex;
                }
                else
                {
                }
                txtBHouNo.Enabled = false;
                txtBStreet.Enabled = false;
                txtBWardNo.Enabled = false;
                txtBPOff.Enabled = false;
                txtBPin.Enabled = false;
                ddlBVillg.Enabled = false;
            }
            else
            {
                txtBHouNo.Text = "";
                txtBStreet.Text = "";
                txtBWardNo.Text = "";
                txtBPOff.Text = "";
                txtBPin.Text = "";
                ddlBVillg.SelectedIndex = -1;
                ddlBMunPanca.SelectedIndex = -1;
                ddlBBlk.SelectedIndex = -1;
                ddlBDist.SelectedIndex = -1;
                ddlBStat.SelectedIndex = -1;
                txtBHouNo.Enabled = true;
                txtBStreet.Enabled = true;
                txtBWardNo.Enabled = true;
                txtBPOff.Enabled = true;
                txtBPin.Enabled = true;
                ddlBVillg.Enabled = true;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkBProf_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBProf.Checked == true)
            {
                if (ddlBIdntyProf.SelectedIndex > 0)
                {
                    if (Convert.ToInt32(ddlBIdntyProf.SelectedValue) <= 5)
                    {
                        ddlBAddProf.SelectedIndex = ddlBIdntyProf.SelectedIndex;
                        txtBAddPrfNo.Text = txtBIdntPrfNo.Text;
                    }
                }
            }
            else
            {
                ddlBAddProf.SelectedIndex = -1;
                txtBAddPrfNo.Text = "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBDOBDt_TextChanged(object sender, EventArgs e)
        {
            //Int32 vAge = 0, vNoYr = 0;
            //Int32 vCurrYear = System.DateTime.Now.Year;
            ////vAge = Convert.ToInt32(txtBDOBDt.Text.Substring(6, 4));
            //DateTime vBDOB = gblFuction.setDate(txtBDOBDt.Text);
            //vNoYr = vCurrYear - vAge;
            // int Years = CalAge(txtBDOBDt.Text);
            int Years = AgeCount(txtBDOBDt.Text, Convert.ToString(Session[gblValue.LoginDate]));
            if (Years < 18)
            {
                gblFuction.AjxMsgPopup("Co borrower age should Greater than 18 Years.");
                txtBAge.Text = "0";
                txtBDOBDt.Text = "";
                txtGuarAge.Text = "0";
                txtGuarDOB.Text = "";
            }
            else if (Years > 59)
            {
                gblFuction.AjxMsgPopup("Co borrower age should less than 59 Years.");
                txtBAge.Text = "0";
                txtBDOBDt.Text = "";
                txtGuarAge.Text = "0";
                txtGuarDOB.Text = "";
            }
            else
            {
                if (txtBDOBDt.Text.Length >= 10)
                    txtGuarDOB.Text = txtBDOBDt.Text;
                txtBAge.Text = Years.ToString();
                txtGuarAge.Text = Years.ToString();
            }
            //txtBDOBDt.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBAge_TextChanged(object sender, EventArgs e)
        {
            if (txtBAge.Text != "")
            {
                Int32 vAge = Convert.ToInt32(txtBAge.Text);
                if (vAge < 18)
                {
                    gblFuction.AjxMsgPopup("Co borrower age should Greater than 21");
                    txtAge.Text = "0";
                    txtBDOBDt.Text = "";
                }
                else
                {
                    DateTime vCurrDate = System.DateTime.Now.AddYears(-vAge);
                    txtBDOBDt.Text = gblFuction.putStrDate(vCurrDate);
                }
                //txtBAge.Focus();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGend_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMrySts.SelectedValue == "M01" && ddlGend.SelectedValue == "F")
            {
                ddlBGend.SelectedIndex = ddlBGend.Items.IndexOf(ddlBGend.Items.FindByText("Male"));
            }
            if (ddlMrySts.SelectedValue == "M01" && ddlGend.SelectedValue == "M")
            {
                ddlBGend.SelectedIndex = ddlBGend.Items.IndexOf(ddlBGend.Items.FindByText("Female"));
            }
            ddlBGend.Enabled = (ddlGend.SelectedValue == "T" && ddlBRel.SelectedValue == "6") ? true : false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ddlAddPrf_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    txtAddPrfNo.Enabled = true;

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ddlIdentyProf_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    txtIdntPrfNo.Enabled = true;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ddlBAddProf_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    txtBAddPrfNo.Enabled = true;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ddlBIdntyProf_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    txtBIdntPrfNo.Enabled = true;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPvLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPvLine.SelectedIndex > 0)
            {
                if (ddlPvLine.SelectedValue == "BPL")
                    txtBPLNo.Enabled = true;
                else
                {
                    txtBPLNo.Text = "";
                    txtBPLNo.Enabled = false;
                }
            }
            else
            {
                txtBPLNo.Text = "";
                txtBPLNo.Enabled = false;
            }
        }


        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            gvMemApp.DataSource = null;
            gvMemApp.DataBind();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {

            DataTable dt = null;
            Int32 vR = 0;
            DataRow dr;
            dt = (DataTable)ViewState["Fam"];

            if (dt.Rows.Count > 0)
            {
                vR = dt.Rows.Count - 1;
                TextBox txtFN = (TextBox)gvFamily.Rows[vR].FindControl("txtFamNm");

                dt.Rows[vR]["FName"] = txtFN.Text;

                DropDownList ddlGen = (DropDownList)gvFamily.Rows[vR].FindControl("ddlGndrFam");
                dt.Rows[vR]["Gender"] = ddlGen.SelectedValue;
                DropDownList ddlRl = (DropDownList)gvFamily.Rows[vR].FindControl("ddlReltFam");
                dt.Rows[vR]["RelationId"] = ddlRl.SelectedValue;
                DropDownList ddlSI = (DropDownList)gvFamily.Rows[vR].FindControl("ddlIncmFam");
                dt.Rows[vR]["OccupationId"] = ddlSI.SelectedValue;
                DropDownList ddlEdu = (DropDownList)gvFamily.Rows[vR].FindControl("ddlEduFam");
                dt.Rows[vR]["QualificationId"] = ddlEdu.SelectedValue;
                TextBox txtAg = (TextBox)gvFamily.Rows[vR].FindControl("txtFamAge");
                dt.Rows[vR]["Age"] = txtAg.Text == "" ? "0" : txtAg.Text;
                TextBox txtIncome = (TextBox)gvFamily.Rows[vR].FindControl("txtFamIncome");
                dt.Rows[vR]["Income"] = txtIncome.Text == "" ? "0" : txtIncome.Text;
            }
            dt.AcceptChanges();
            if (dt.Rows[vR]["FName"] != "")
            {
                dr = dt.NewRow();

                dt.Rows.Add(dr);
            }
            else
            {
                gblFuction.MsgPopup("Family member's name is Blank...");
            }

            ViewState["Fam"] = dt;
            gvFamily.DataSource = dt;
            gvFamily.DataBind();
        }

        protected void gvFamily_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null, dt1 = null, dt2 = null;
            CGblIdGenerator oGbl = null;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlGen = (DropDownList)e.Row.FindControl("ddlGndrFam");
                    ddlGen.SelectedIndex = ddlGen.Items.IndexOf(ddlGen.Items.FindByValue(e.Row.Cells[0].Text));

                    DropDownList ddlMrtlStatus = (DropDownList)e.Row.FindControl("ddlMrtlStatus");
                    ddlMrtlStatus.SelectedIndex = ddlMrtlStatus.Items.IndexOf(ddlMrtlStatus.Items.FindByText
                        (e.Row.Cells[4].Text));

                    DropDownList ddlRel = (DropDownList)e.Row.FindControl("ddlReltFam");
                    oGbl = new CGblIdGenerator();
                    dt = oGbl.PopComboMIS("N", "N", "AA", "HumanRelationId", "HumanRelationName", "HumanRelationMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                    ddlRel.DataSource = dt;
                    ddlRel.DataTextField = "HumanRelationName";
                    ddlRel.DataValueField = "HumanRelationId";
                    ddlRel.DataBind();
                    ListItem oL1 = new ListItem("<-- Select -->", "-1");
                    ddlRel.Items.Insert(0, oL1);
                    ddlRel.SelectedIndex = ddlRel.Items.IndexOf(ddlRel.Items.FindByValue(e.Row.Cells[1].Text));

                    DropDownList ddlQly = (DropDownList)e.Row.FindControl("ddlEduFam");
                    dt1 = oGbl.PopComboMIS("N", "N", "AA", "QualificationId", "QualificationName", "QualificationMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                    ddlQly.DataSource = dt1;
                    ddlQly.DataTextField = "QualificationName";
                    ddlQly.DataValueField = "QualificationId";
                    ddlQly.DataBind();
                    ListItem oL2 = new ListItem("<-- Select -->", "-1");
                    ddlQly.Items.Insert(0, oL2);
                    ddlQly.SelectedIndex = ddlQly.Items.IndexOf(ddlQly.Items.FindByValue(e.Row.Cells[3].Text));

                    DropDownList ddlSI = (DropDownList)e.Row.FindControl("ddlIncmFam");
                    dt2 = oGbl.PopComboMIS("N", "N", "AA", "OccupationId", "OccupationName", "OccupationMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                    ddlSI.DataSource = dt2;
                    ddlSI.DataTextField = "OccupationName";
                    ddlSI.DataValueField = "OccupationId";
                    ddlSI.DataBind();
                    ListItem oL3 = new ListItem("<-- Select -->", "-1");
                    ddlSI.Items.Insert(0, oL3);
                    ddlSI.SelectedIndex = ddlSI.Items.IndexOf(ddlSI.Items.FindByValue(e.Row.Cells[2].Text));



                    DropDownList ddOlSI = (DropDownList)e.Row.FindControl("ddlOthIncmFam");
                    dt2 = oGbl.PopComboMIS("N", "N", "AA", "OccupationId", "OccupationName", "OccupationMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                    ddOlSI.DataSource = dt2;
                    ddOlSI.DataTextField = "OccupationName";
                    ddOlSI.DataValueField = "OccupationId";
                    ddOlSI.DataBind();
                    ListItem oL4 = new ListItem("<-- Select -->", "-1");
                    ddOlSI.Items.Insert(0, oL3);
                    ddOlSI.SelectedIndex = ddlSI.Items.IndexOf(ddlSI.Items.FindByValue(e.Row.Cells[2].Text));

                    TextBox txtIncome = (TextBox)e.Row.FindControl("txtFamIncome");
                    TextBox txtAg = (TextBox)e.Row.FindControl("txtFamAge");
                    //        if (txtAg != null)
                    //        {
                    //            txtAg.TextChanged += new EventHandler(txtFamAge_TextChanged);
                    //        }
                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                dt2 = null;
                oGbl = null;
            }
        }

        protected void gvFamily_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDel")
            {
                DataTable dt = null;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                dt = (DataTable)ViewState["Fam"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    ViewState["Fam"] = dt;
                    gvFamily.DataSource = dt;
                    gvFamily.DataBind();
                }
                else
                {
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
        }

        private void GenerateGrid()
        {
            DataSet ds = null;
            DataTable dt = null, dt1 = null, dt2 = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                ds = oMem.GenerateGrid();
                dt = ds.Tables[0];
                //dt1 = ds.Tables[1];
                //dt2 = ds.Tables[2];
                DataRow dF;
                dF = dt.NewRow();
                dt.Rows.Add(dF);
                dt.AcceptChanges();
                ViewState["Fam"] = dt;
                gvFamily.DataSource = dt;
                gvFamily.DataBind();
                //DataRow dL;
                //dL = dt1.NewRow();
                //dt1.Rows.Add(dL);
                //dt1.AcceptChanges();
                ViewState["Loan"] = dt1;

                ViewState["IncExp"] = dt2;
                //pnlFamDtl.ActiveTabIndex = 1;
                StatusButton("Show");

            }

            finally
            {
            }
        }

        public void GetData()
        {
            DataTable dt = (DataTable)ViewState["Fam"];
            foreach (GridViewRow gr in gvFamily.Rows)
            {
                TextBox txtFN = (TextBox)gvFamily.Rows[gr.RowIndex].FindControl("txtFamNm");
                DropDownList ddlGen = (DropDownList)gvFamily.Rows[gr.RowIndex].FindControl("ddlGndrFam");
                DropDownList ddlRl = (DropDownList)gvFamily.Rows[gr.RowIndex].FindControl("ddlReltFam");
                DropDownList ddlSI = (DropDownList)gvFamily.Rows[gr.RowIndex].FindControl("ddlIncmFam");
                DropDownList ddlEdu = (DropDownList)gvFamily.Rows[gr.RowIndex].FindControl("ddlEduFam");
                TextBox txtAg = (TextBox)gvFamily.Rows[gr.RowIndex].FindControl("txtFamAge");
                TextBox txtIncome = (TextBox)gvFamily.Rows[gr.RowIndex].FindControl("txtFamIncome");
                DropDownList ddlMrtlStatus = (DropDownList)gvFamily.Rows[gr.RowIndex].FindControl("ddlMrtlStatus");
                DropDownList ddlOthIncmFam = (DropDownList)gvFamily.Rows[gr.RowIndex].FindControl("ddlOthIncmFam");
                dt.Rows[gr.RowIndex]["FName"] = txtFN.Text;
                dt.Rows[gr.RowIndex]["Gender"] = ddlGen.SelectedValue;
                dt.Rows[gr.RowIndex]["RelationId"] = ddlRl.SelectedValue;
                dt.Rows[gr.RowIndex]["OccupationId"] = ddlSI.SelectedValue;
                dt.Rows[gr.RowIndex]["QualificationId"] = ddlEdu.SelectedValue;
                dt.Rows[gr.RowIndex]["Age"] = Convert.ToInt32(txtAg.Text == "" ? "0" : txtAg.Text);
                dt.Rows[gr.RowIndex]["Income"] = Convert.ToDouble(txtIncome.Text == "" ? "0" : txtIncome.Text);

                dt.Rows[gr.RowIndex]["MaritialStatus"] = ddlMrtlStatus.SelectedValue;
                dt.Rows[gr.RowIndex]["ExOccupationId"] = ddlOthIncmFam.SelectedValue;
            }
            dt.AcceptChanges();
            ViewState["Fam"] = dt;
            gvFamily.DataSource = dt;
            gvFamily.DataBind();
        }

        protected void gvAsset_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null, dt1 = null, dt2 = null;
            CGblIdGenerator oGbl = null;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {


                    DropDownList ddlAsset = (DropDownList)e.Row.FindControl("ddlAsset");
                    oGbl = new CGblIdGenerator();
                    dt = oGbl.PopComboMIS("N", "N", "AA", "AssetTypeId", "AssteName", "AssetypeMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                    ddlAsset.DataSource = dt;
                    ddlAsset.DataTextField = "AssteName";
                    ddlAsset.DataValueField = "AssetTypeId";
                    ddlAsset.DataBind();
                    ListItem oL1 = new ListItem("<-- Select -->", "-1");
                    ddlAsset.Items.Insert(0, oL1);
                    ddlAsset.SelectedIndex = ddlAsset.Items.IndexOf(ddlAsset.Items.FindByValue(e.Row.Cells[0].Text));


                    TextBox txtAstQty = (TextBox)e.Row.FindControl("txtAstQty");
                    TextBox txtAstAmt = (TextBox)e.Row.FindControl("txtAstAmt");
                    //        if (txtAg != null)
                    //        {
                    //            txtAg.TextChanged += new EventHandler(txtFamAge_TextChanged);
                    //        }
                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                dt2 = null;
                oGbl = null;
            }
        }

        public void GetData1()
        {
            DataTable dt = (DataTable)ViewState["Asset"];
            foreach (GridViewRow gr in gvAsset.Rows)
            {

                DropDownList ddlAsset = (DropDownList)gvAsset.Rows[gr.RowIndex].FindControl("ddlAsset");

                TextBox txtAstQty = (TextBox)gvAsset.Rows[gr.RowIndex].FindControl("txtAstQty");
                TextBox txtAstAmt = (TextBox)gvAsset.Rows[gr.RowIndex].FindControl("txtAstAmt");


                dt.Rows[gr.RowIndex]["AssetName"] = ddlAsset.SelectedValue;
                dt.Rows[gr.RowIndex]["AssetQty"] = Convert.ToInt32(txtAstQty.Text == "" ? "0" : txtAstQty.Text);
                dt.Rows[gr.RowIndex]["AssetAmt"] = Convert.ToDouble(txtAstAmt.Text == "" ? "0" : txtAstAmt.Text);


            }
            dt.AcceptChanges();
            ViewState["Asset"] = dt;
            gvAsset.DataSource = dt;
            gvAsset.DataBind();
        }
        private void GenerateGrid1()
        {
            DataSet ds = null;
            DataTable dt = null, dt1 = null, dt2 = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                ds = oMem.GenerateGrid1();
                dt = ds.Tables[0];
                //dt1 = ds.Tables[1];
                //dt2 = ds.Tables[2];
                DataRow dF;
                dF = dt.NewRow();
                dt.Rows.Add(dF);
                dt.AcceptChanges();
                ViewState["Asset"] = dt;
                gvAsset.DataSource = dt;
                gvAsset.DataBind();

                StatusButton("Show");

            }

            finally
            {
            }
        }
        protected void gvAsset_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDel2")
            {
                DataTable dt = null;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                dt = (DataTable)ViewState["Asset"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    ViewState["Asset"] = dt;
                    gvAsset.DataSource = dt;
                    gvAsset.DataBind();
                }
                else
                {
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
        }
        protected void btnAddNew1_Click(object sender, EventArgs e)
        {

            DataTable dt = null;
            Int32 vR = 0;
            DataRow dr;
            dt = (DataTable)ViewState["Asset"];
            if (dt.Rows.Count > 0)
            {
                vR = dt.Rows.Count - 1;

                DropDownList ddlAsset = (DropDownList)gvAsset.Rows[vR].FindControl("ddlAsset");
                dt.Rows[vR]["AssetName"] = ddlAsset.SelectedValue;
                TextBox txtAstQty = (TextBox)gvAsset.Rows[vR].FindControl("txtAstQty");
                dt.Rows[vR]["AssetQty"] = txtAstQty.Text == "" ? "0" : txtAstQty.Text;
                TextBox txtAstAmt = (TextBox)gvAsset.Rows[vR].FindControl("txtAstAmt");
                dt.Rows[vR]["AssetAmt"] = txtAstAmt.Text == "" ? "0" : txtAstAmt.Text;
            }
            dt.AcceptChanges();

            if (dt.Rows[vR]["AssetName"].ToString() != "-1")
            {

                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            else
            {
                gblFuction.MsgPopup("Asset name is Blank...");
            }
            ViewState["Asset"] = dt;
            gvAsset.DataSource = dt;
            gvAsset.DataBind();
        }
        protected void txtNetIncome_Textchange(object sender, EventArgs e)
        {
            txtTotInc.Text = (Convert.ToDouble(txtNetIncome.Text) + Convert.ToDouble(txtAmtA.Text) + Convert.ToDouble(txtAmtB.Text) + Convert.ToDouble(txtAmtC.Text)
                + Convert.ToDouble(txtAmtD.Text) + Convert.ToDouble(txtAmtE.Text) + Convert.ToDouble(txtAmtF.Text)).ToString();

            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtAmtA_Textchange(object sender, EventArgs e)
        {
            txtTotInc.Text = (Convert.ToDouble(txtNetIncome.Text) + Convert.ToDouble(txtAmtA.Text) + Convert.ToDouble(txtAmtB.Text) + Convert.ToDouble(txtAmtC.Text)
                + Convert.ToDouble(txtAmtD.Text) + Convert.ToDouble(txtAmtE.Text) + Convert.ToDouble(txtAmtF.Text)).ToString();

            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtAmtB_Textchange(object sender, EventArgs e)
        {
            txtTotInc.Text = (Convert.ToDouble(txtNetIncome.Text) + Convert.ToDouble(txtAmtA.Text) + Convert.ToDouble(txtAmtB.Text) + Convert.ToDouble(txtAmtC.Text)
                + Convert.ToDouble(txtAmtD.Text) + Convert.ToDouble(txtAmtE.Text) + Convert.ToDouble(txtAmtF.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtAmtC_Textchange(object sender, EventArgs e)
        {
            txtTotInc.Text = (Convert.ToDouble(txtNetIncome.Text) + Convert.ToDouble(txtAmtA.Text) + Convert.ToDouble(txtAmtB.Text) + Convert.ToDouble(txtAmtC.Text)
                + Convert.ToDouble(txtAmtD.Text) + Convert.ToDouble(txtAmtE.Text) + Convert.ToDouble(txtAmtF.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtAmtD_Textchange(object sender, EventArgs e)
        {
            txtTotInc.Text = (Convert.ToDouble(txtNetIncome.Text) + Convert.ToDouble(txtAmtA.Text) + Convert.ToDouble(txtAmtB.Text) + Convert.ToDouble(txtAmtC.Text)
                + Convert.ToDouble(txtAmtD.Text) + Convert.ToDouble(txtAmtE.Text) + Convert.ToDouble(txtAmtF.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtAmtE_Textchange(object sender, EventArgs e)
        {
            txtTotInc.Text = (Convert.ToDouble(txtNetIncome.Text) + Convert.ToDouble(txtAmtA.Text) + Convert.ToDouble(txtAmtB.Text) + Convert.ToDouble(txtAmtC.Text)
                + Convert.ToDouble(txtAmtD.Text) + Convert.ToDouble(txtAmtE.Text) + Convert.ToDouble(txtAmtF.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtAmtF_Textchange(object sender, EventArgs e)
        {
            txtTotInc.Text = (Convert.ToDouble(txtNetIncome.Text) + Convert.ToDouble(txtAmtA.Text) + Convert.ToDouble(txtAmtB.Text) + Convert.ToDouble(txtAmtC.Text)
                + Convert.ToDouble(txtAmtD.Text) + Convert.ToDouble(txtAmtE.Text) + Convert.ToDouble(txtAmtF.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtHsRntAmt_Textchange(object sender, EventArgs e)
        {
            txtTotExp.Text = (Convert.ToDouble(txtHsRntAmt.Text) + Convert.ToDouble(txtMedAmt.Text) + Convert.ToDouble(txtEduAmt.Text) + Convert.ToDouble(txtLnInsAmt.Text)
                + Convert.ToDouble(txtExAmtA.Text) + Convert.ToDouble(txtExAmtB.Text) + Convert.ToDouble(txtFdAmt.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtEduAmt_Textchange(object sender, EventArgs e)
        {
            txtTotExp.Text = (Convert.ToDouble(txtHsRntAmt.Text) + Convert.ToDouble(txtMedAmt.Text) + Convert.ToDouble(txtEduAmt.Text) + Convert.ToDouble(txtLnInsAmt.Text)
                + Convert.ToDouble(txtExAmtA.Text) + Convert.ToDouble(txtExAmtB.Text) + Convert.ToDouble(txtFdAmt.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtMedAmt_Textchange(object sender, EventArgs e)
        {
            txtTotExp.Text = (Convert.ToDouble(txtHsRntAmt.Text) + Convert.ToDouble(txtMedAmt.Text) + Convert.ToDouble(txtEduAmt.Text) + Convert.ToDouble(txtLnInsAmt.Text)
                + Convert.ToDouble(txtExAmtA.Text) + Convert.ToDouble(txtExAmtB.Text) + Convert.ToDouble(txtFdAmt.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtLnInsAmt_Textchange(object sender, EventArgs e)
        {
            txtTotExp.Text = (Convert.ToDouble(txtHsRntAmt.Text) + Convert.ToDouble(txtMedAmt.Text) + Convert.ToDouble(txtEduAmt.Text) + Convert.ToDouble(txtLnInsAmt.Text)
                + Convert.ToDouble(txtExAmtA.Text) + Convert.ToDouble(txtExAmtB.Text) + Convert.ToDouble(txtFdAmt.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtExAmtA_Textchange(object sender, EventArgs e)
        {
            txtTotExp.Text = (Convert.ToDouble(txtHsRntAmt.Text) + Convert.ToDouble(txtMedAmt.Text) + Convert.ToDouble(txtEduAmt.Text) + Convert.ToDouble(txtLnInsAmt.Text)
                + Convert.ToDouble(txtExAmtA.Text) + Convert.ToDouble(txtExAmtB.Text) + Convert.ToDouble(txtFdAmt.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();

        }
        protected void txtExB_Textchange(object sender, EventArgs e)
        {
            txtTotExp.Text = (Convert.ToDouble(txtHsRntAmt.Text) + Convert.ToDouble(txtMedAmt.Text) + Convert.ToDouble(txtEduAmt.Text) + Convert.ToDouble(txtLnInsAmt.Text)
                + Convert.ToDouble(txtExAmtA.Text) + Convert.ToDouble(txtExAmtB.Text) + Convert.ToDouble(txtFdAmt.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();

        }
        protected void txtFdAmt_Textchange(object sender, EventArgs e)
        {
            txtTotExp.Text = (Convert.ToDouble(txtHsRntAmt.Text) + Convert.ToDouble(txtMedAmt.Text) + Convert.ToDouble(txtEduAmt.Text) + Convert.ToDouble(txtLnInsAmt.Text)
                + Convert.ToDouble(txtExAmtA.Text) + Convert.ToDouble(txtExAmtB.Text) + Convert.ToDouble(txtFdAmt.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();

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
                        //File.WriteAllBytes(filePath, Convert.FromBase64String(getBase64String(flup)));
                        ReduceImageSize(0.5, strm, targetFile);
                        isImageSaved = "Y";
                    }
                }
            }
            catch (Exception ex)
            {
                //DBUtility.WriteErrorLog(ex);
                //isImageSaved = "N";
                throw ex;
            }

            return isImageSaved;
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

        public static byte[] ResizeImage(byte[] imageBytes, double scale)
        {
            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                using (Bitmap original = new Bitmap(memoryStream))
                {
                    int newWidth = (int)(original.Width * scale);
                    int newHeight = (int)(original.Height * scale);

                    using (Bitmap resized = new Bitmap(original, newWidth, newHeight))
                    {
                        using (MemoryStream resultStream = new MemoryStream())
                        {
                            resized.Save(resultStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            return resultStream.ToArray();
                        }
                    }
                }
            }
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

        protected void ddlBIdntyProf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBIdntyProf.SelectedValue == "1")
            {
                if (ddlBAddProf.SelectedValue == "1")
                {
                    ddlBIdntyProf.SelectedIndex = -1;
                }
                else
                {
                    lblBId1.Visible = true;
                    txtBConfrmAadhar1.Visible = true;
                    txtBIdntPrfNo.Text = "";
                    if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                    {
                        txtBIdntPrfNo.Attributes["type"] = "password";
                    }
                }
            }
            else
            {
                txtBConfrmAadhar1.Attributes.Add("value", "");
                lblBId1.Visible = false;
                txtBConfrmAadhar1.Visible = false;
                txtBIdntPrfNo.Attributes["type"] = "Text";
                txtBIdntPrfNo.Text = "";
            }
        }

        protected void ddlBAddProf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBAddProf.SelectedValue == "1")
            {
                if (ddlBIdntyProf.SelectedValue == "1")
                {
                    ddlBAddProf.SelectedIndex = -1;
                }
                else
                {
                    lblBId2.Visible = true;
                    txtBConfrmAadhar2.Visible = true;
                    txtBAddPrfNo.Text = "";
                    if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                    {
                        txtBAddPrfNo.Attributes["type"] = "password";
                    }
                }
            }
            else
            {
                txtBConfrmAadhar2.Attributes.Add("value", "");
                lblBId2.Visible = false;
                txtBConfrmAadhar2.Visible = false;
                txtBAddPrfNo.Attributes["type"] = "Text";
                txtBAddPrfNo.Text = "";
            }
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

        private void popOtherIncomeSource()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "OthIncSrcId", "OthIncSrcName", "OthIncomeSourceMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                DataView dv = new DataView(dt);
                dv.Sort = "OthIncSrcId ASC";
                ddlOtherIncSrc.DataSource = dv;
                //ddlOtherIncSrc.DataSource = dt;                
                ddlOtherIncSrc.DataTextField = "OthIncSrcName";
                ddlOtherIncSrc.DataValueField = "OthIncSrcId";
                ddlOtherIncSrc.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlOtherIncSrc.Items.Insert(0, oli);
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
        protected void btnAddNew11_Click(object sender, EventArgs e)
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
                dt.Rows[vR]["MaskedKYCNo"] = ddlKYCType.SelectedValue == "1" ? String.Format("{0}{1}", "********", txtKYCNo.Text.Substring(txtKYCNo.Text.Length - 4, 4)) : txtKYCNo.Text;
                HiddenField hdnKYCNo = (HiddenField)gvEarningMember.Rows[vR].FindControl("hdnKYCNo");
                dt.Rows[vR]["KYCNo"] = hdnKYCNo.Value;
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
                HiddenField hdnKYCNo = (HiddenField)gvEarningMember.Rows[gr.RowIndex].FindControl("hdnKYCNo");
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
                    dt.Rows[gr.RowIndex]["MaskedKYCNo"] = ddlKYCType.SelectedValue == "1" ? String.Format("{0}{1}", "********", txtKYCNo.Text.Substring(txtKYCNo.Text.Length - 4, 4)) : txtKYCNo.Text;
                    dt.Rows[gr.RowIndex]["KYCNo"] = hdnKYCNo.Value;
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

        private void GenerateReport(string pApiName, string pRequestdata)
        {
            string vMsg = "";
            CApiCalling oAPI = new CApiCalling();
            try
            {
                vMsg = oAPI.GenerateReport(pApiName, pRequestdata, "https://unityimage.bijliftt.com/ImageDownloadService.svc");
            }
            finally
            {
            }
        }

        private readonly Encoding encoding = Encoding.UTF8;
        #region GetMultipartFormData
        private byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;
            foreach (var param in postParameters)
            {
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                    boundary,
                    param.Key,
                    param.Value);
                formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();
            return formData;
        }
        #endregion

        #region Minio Image Upload
        public string UploadFileMinio(byte[] image, string fileName, string enqId, string bucketName, string minioUrl)
        {
            string fullResponse = "", isImageSaved = "N";
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("image", Convert.ToBase64String(image));
            postParameters.Add("KycId", enqId);
            postParameters.Add("BucketName", bucketName);
            postParameters.Add("ImageName", fileName);
            // Create request and receive response
            //  string postURL = "https://ocr.bijliftt.com/KYCFileUploadBase64";
            string postURL = minioUrl;
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;
            byte[] formDataforRequest = GetMultipartFormData(postParameters, formDataBoundary);
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = contentType;
                request.CookieContainer = new CookieContainer();
                request.KeepAlive = false;
                request.Timeout = System.Threading.Timeout.Infinite;
                request.ContentLength = formDataforRequest.Length;

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(formDataforRequest, 0, formDataforRequest.Length);
                    requestStream.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    fullResponse = reader.ReadToEnd();
                }
            }
            finally
            {
            }
            dynamic obj = JsonConvert.DeserializeObject(fullResponse);
            bool status = obj.status;
            if (status == true)
            {
                isImageSaved = "Y";
            }
            return isImageSaved;
        }

        public static byte[] strmToByte(Stream vStream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                vStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
        #endregion

        private byte[] ConvertFileToByteArray(HttpPostedFile postedFile)
        {
            using (Stream stream = postedFile.InputStream)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

    }
}
