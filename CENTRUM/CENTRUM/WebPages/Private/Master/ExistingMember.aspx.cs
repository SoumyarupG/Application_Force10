using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.IO;
using System.Web.Hosting;
using System.Net;
using System.Globalization;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class ExistingMember : CENTRUMBase
    {
        protected int vPgNo = 1;
        protected string vMemberId = "";
        string pathMember = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                popMember();
                gvAsset.DataSource = null;
                InitBasePage();
                StatusButton("View");
                hdnApplyCgt.Value = "N";
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                GenerateGrid1();
                popQualification();
                PopGender();
                popHouseHold();
                popRelation();
                popRelation1();
                popCaste();
                popOccupation();
                popVillage();
                popReligion();
                popIdentityProof();
                popAddProof();
                //popBank();        
                btnCgtApply.Enabled = false;
                tbEmp.ActiveTabIndex = 0;
                popState();
                popBusinessType();
                popOtherIncomeSource();
                GenerateEarningMember();
                popBusinessActivityAll();
                clearMemPhoto();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopGender()
        {
            Dictionary<string, string> oGen = new Dictionary<string, string>();
            oGen.Add("<-Select->", "-1");
            oGen.Add("Male", "M");
            oGen.Add("Female", "F");
            oGen.Add("Transgender", "O");

            ddlGend.DataSource = oGen;
            ddlGend.DataValueField = "value";
            ddlGend.DataTextField = "key";
            ddlGend.DataBind();

            ddlBGend.DataSource = oGen;
            ddlBGend.DataValueField = "value";
            ddlBGend.DataTextField = "key";
            ddlBGend.DataBind();

            ddlGuarGen.DataSource = oGen;
            ddlGuarGen.DataValueField = "value";
            ddlGuarGen.DataTextField = "key";
            ddlGuarGen.DataBind();
        }

        private void popMember()
        {
            CExistMember oNM = new CExistMember();
            DataTable dt = new DataTable();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            dt = oNM.GetExistMember(vBrCode);
            ddlMemName.DataSource = ddlGroupName.DataSource = ddlCOName.DataSource = ddlGroup.DataSource = ddlRO.DataSource = ddlMarket.DataSource = ddlCenter.DataSource = dt;
            ddlMemName.DataTextField = "Name";
            ddlMemName.DataValueField = "EnquiryId";
            ddlMemName.DataBind();
            ddlMemName.Items.Insert(0, new ListItem("<---Select--->", "0"));

            ddlGroupName.DataTextField = "GroupName";
            ddlGroupName.DataValueField = "Groupid";
            ddlGroupName.DataBind();
            ddlGroupName.Items.Insert(0, new ListItem("<---Select--->", "0"));

            ddlCOName.DataTextField = "EoName";
            ddlCOName.DataValueField = "Eoid";
            ddlCOName.DataBind();
            ddlCOName.Items.Insert(0, new ListItem("<---Select--->", "0"));

            ddlGroup.DataTextField = "GroupName";
            ddlGroup.DataValueField = "Groupid";
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, new ListItem("<---Select--->", "0"));

            ddlRO.DataTextField = "EoName";
            ddlRO.DataValueField = "Eoid";
            ddlRO.DataBind();
            ddlRO.Items.Insert(0, new ListItem("<---Select--->", "0"));

            ddlMarket.DataTextField = "Market";
            ddlMarket.DataValueField = "MarketID";
            ddlMarket.DataBind();
            ddlMarket.Items.Insert(0, new ListItem("<---Select--->", "0"));

            ddlCenter.DataTextField = "Market";
            ddlCenter.DataValueField = "MarketID";
            ddlCenter.DataBind();
            ddlCenter.Items.Insert(0, new ListItem("<---Select--->", "0"));
        }

        /// <summary>
        /// 
        /// </summary>
        private void popCaste()
        {
            Dictionary<string, Int32> oDic = new Dictionary<string, Int32>();
            oDic.Add("<-Select->", 0);
            oDic.Add("General", 1);
            oDic.Add("SC", 2);
            oDic.Add("ST", 3);
            oDic.Add("OBC", 4);
            oDic.Add("Minority", 5);
            oDic.Add("Other", 6);
            ddlCaste.DataSource = oDic;
            ddlCaste.DataValueField = "value";
            ddlCaste.DataTextField = "key";
            ddlCaste.DataBind();
            ddlBCaste.DataSource = oDic;
            ddlBCaste.DataValueField = "value";
            ddlBCaste.DataTextField = "key";
            ddlBCaste.DataBind();
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
        private void popAddProof()
        {
            DataTable dt = null;
            CNewMember oNM = null;
            try
            {
                oNM = new CNewMember();
                dt = oNM.popIdAddrProof("Y", "N");

                //ddlBIdntyProf.DataSource = dt;
                //ddlBIdntyProf.DataTextField = "IDProofName";
                //ddlBIdntyProf.DataValueField = "IDProofId";
                //ddlBIdntyProf.DataBind();
                //ListItem oli3 = new ListItem("<--Select-->", "-1");
                //ddlBIdntyProf.Items.Insert(0, oli3);

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

                ddlBIdntyProf.DataSource = dt;
                ddlBIdntyProf.DataTextField = "IDProofName";
                ddlBIdntyProf.DataValueField = "IDProofId";
                ddlBIdntyProf.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlBIdntyProf.Items.Insert(0, oli1);
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
        private void popReligion()
        {
            Dictionary<string, string> oDic = new Dictionary<string, string>();
            oDic.Add("<-Select->", "0");
            oDic.Add("Buddhist", "R05");
            oDic.Add("Christian", "R03");
            oDic.Add("Hindu", "R01");
            oDic.Add("Jain", "R06");
            oDic.Add("Muslim", "R02");
            oDic.Add("Others", "R07");
            oDic.Add("Sikh", "R04");
            oDic.Add("Jewish", "R08");
            oDic.Add("Parsi", "R09");
            ddlRelg.DataSource = oDic;
            ddlRelg.DataValueField = "value";
            ddlRelg.DataTextField = "key";
            ddlRelg.DataBind();
            ddlBRelg.DataSource = oDic;
            ddlBRelg.DataValueField = "value";
            ddlBRelg.DataTextField = "key";
            ddlBRelg.DataBind();
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
                dt = oGb.PopComboMIS("N", "N", "AA", "HumanRelationId", "HumanRelationName", "HumanRelationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
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


        private void popBank()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                //oGb = new CGblIdGenerator();
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

        protected void ddlBVillg_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopAllAgainstVillage();
        }

        protected void ddlVillg_SelectedIndexChanged(object sender, EventArgs e)
        {
            popAgainstVillage();
        }
        private void popAgainstVillage()
        {
            //DataTable dt = null;
            //CVillage oVlg = null;
            //string vVlgId = ddlVillg.SelectedValue;
            //try
            //{
            //    oVlg = new CVillage();
            //    dt = oVlg.GetGpBlkDistStateList(vVlgId);
            //    ddlMuPanc.DataSource = ddlBlk.DataSource = ddlDist.DataSource = ddlStat.DataSource = dt;
            //    ddlMuPanc.DataTextField = "GPName";
            //    ddlMuPanc.DataValueField = "GPId";
            //    ddlMuPanc.DataBind();
            //    ddlBlk.DataTextField = "BlockName";
            //    ddlBlk.DataValueField = "BlockId";
            //    ddlBlk.DataBind();
            //    //ddlDist.DataTextField = "DistrictName";
            //    //ddlDist.DataValueField = "DistrictId";
            //    //ddlDist.DataBind();
            //    //ddlStat.DataTextField = "StateName";
            //    //ddlStat.DataValueField = "StateId";
            //    //ddlStat.DataBind();
            //    ddlMuPanc.Enabled = false;
            //    ddlBlk.Enabled = false;
            //    //ddlDist.Enabled = false;
            //    //ddlStat.Enabled = false;
            //}
            //finally
            //{
            //    oVlg = null;
            //    dt = null;
            //}
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
                    btnAdd.Visible = true;
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
                    btnAprv.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess == "Y")
                {
                    btnAprv.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                    btnAprv.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                    btnAprv.Visible = true;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
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
            txtHFName.Enabled = false;
            chkFath.Enabled = false;
            txtDOB.Enabled = false;
            txtAge.Enabled = false;
            txtHouNo.Enabled = false;
            txtStName.Enabled = false;
            txtWardNo.Enabled = false;
            txtPOff.Enabled = false;
            txtPin.Enabled = false;
            txtMob.Enabled = false;
            txtPhNo.Enabled = Status;
            txtPin.Enabled = false;
            txtBDOBDt.Enabled = Status;
            txtBAge.Enabled = Status;
            txtBHouNo.Enabled = Status;
            txtBStreet.Enabled = Status;
            txtNoOfDpndnts.Enabled = Status;
            txtBWardNo.Enabled = Status;
            txtBPOff.Enabled = Status;
            txtBPin.Enabled = Status;
            txtBMobNo.Enabled = Status;
            txtBPhNo.Enabled = Status;
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
            txtVillg.Enabled = Status;
            ddlAddPrf.Enabled = false;
            ddlIdentyProf.Enabled = false;
            txtBankName.Enabled = Status;
            txtBranch.Enabled = Status;
            txtAccNo.Enabled = Status;
            txtIFSC.Enabled = Status;
            txtMemNamePBook.Enabled = Status;
            ddlBRel.Enabled = Status;
            ddlBEdu.Enabled = Status;
            ddlBOcup.Enabled = Status;
            ddlBGend.Enabled = Status;
            ddlBRelg.Enabled = Status;
            ddlBCaste.Enabled = Status;
            ddlBVillg.Enabled = Status;
            ddlBAddProf.Enabled = Status;
            ddlBIdntyProf.Enabled = Status;
            ddlRO.Enabled = Status;
            ddlCenter.Enabled = Status;
            ddlGroup.Enabled = Status;
            ddlHHoldTyp.Enabled = Status;
            ddlMemName.Enabled = Status;
            ddlGroupName.Enabled = false;
            ddlMarket.Enabled = false;
            ddlCOName.Enabled = false;
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
            //txtTotInc.Enabled = Status;
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
            txtEligibleEMI.Enabled = Status;
            txtFdAmt.Enabled = Status;
            txtGuarName.Enabled = Status;
            ddlGuarRel.Enabled = Status;
            txtGuarDOB.Enabled = Status;
            txtGuarAge.Enabled = Status;
            ddlGuarGen.Enabled = Status;
            txtSavingsAcNo.Enabled = Status;
            txtCustId.Enabled = Status;
            txtMaidenNmF.Enabled = Status;
            txtMaidenNmM.Enabled = Status;
            txtMaidenNmL.Enabled = Status;
            txtIdntPrfNo.Enabled = false;
            txtAddPrfNo.Enabled = false;
            txtMetDay.Enabled = false;
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
            txtBLandmark.Enabled = Status;
            txtBArea.Enabled = Status;
            txtBEmail.Enabled = Status;
            txtBIdntPrfNo.Enabled = Status;
            txtBAddPrfNo.Enabled = Status;
            txtGuarLName.Enabled = Status;
            txtNoOfHouseMember.Enabled = Status;
            txtNoOfChild.Enabled = Status;

            //txtFamilyInc.Enabled = Status;
            //txtSelfInc.Enabled = Status;
            ddlOtherIncSrc.Enabled = Status;
            txtOtherInc.Enabled = Status;

            txtFuelExp.Enabled = Status;
            txtElecExp.Enabled = Status;
            txtTransExp.Enabled = Status;
            chkCoAdd.Enabled = Status;
            chkCommAddr.Enabled = Status;
            txtOtherExp.Enabled = Status;
            ddlStat.Enabled = Status;
            ddlAreaCategory.Enabled = false;
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
            txtGuarLName.Enabled = vStatus;
            txtGuarName.Enabled = vStatus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                // ddlCommVill.SelectedIndex = ddlVillg.SelectedIndex;
                if (ddlBVillg.SelectedIndex >= 0)
                {
                    popAgainstVillage2();
                    ddlCommMuni.SelectedIndex = ddlMuPanc.SelectedIndex;
                    ddlCommBlock.SelectedIndex = ddlBlk.SelectedIndex;
                    // ddlCommDist.SelectedIndex = ddlDist.SelectedIndex;
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
        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                // dt = oGb.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
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
            //ddlBankName.SelectedIndex = -1; 
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
            txtVillg.Text = "";
            ddlMuPanc.Items.Clear();
            ddlMuPanc.SelectedIndex = -1;
            ddlBlk.Items.Clear();
            ddlBlk.SelectedIndex = -1;
            //ddlDist.Items.Clear();
            //ddlDist.SelectedIndex = -1;
            //ddlStat.Items.Clear();
            //ddlStat.SelectedIndex = -1;
            txtDist.Text = "";
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
            //ddlRO.Items.Clear();
            ddlRO.SelectedIndex = -1;
            ddlCOName.SelectedIndex = -1;
            // popEO();
            ////ddlMetDay.Items.Clear();  
            // ddlMetDay.SelectedIndex = -1;
            ddlCenter.Items.Clear();
            ddlCenter.SelectedIndex = -1;
            //ddlGroup.Items.Clear();
            ddlGroup.SelectedIndex = -1;
            chkCoAdd.Checked = false;
            ddlHHoldTyp.SelectedIndex = -1;
            ddlGroupName.SelectedIndex = -1;
            ddlMarket.SelectedIndex = -1;
            ddlMemName.SelectedIndex = -1;
            hdnApplyCgt.Value = "N";
            txtPjMeetDt.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
            ///
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
            txtEligibleEMI.Text = "0.00";
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
            txtMaidenNmF.Text = "";
            txtMaidenNmM.Text = "";
            txtMaidenNmL.Text = "";
            txtGuarLName.Text = "";

            txtBConfrmAadhar1.Text = "";
            txtBConfrmAadhar2.Text = "";
            txtConfrmAadhar3.Text = "";
            txtBConfrmAadhar1.Visible = false;
            txtBConfrmAadhar2.Visible = false;
            txtConfrmAadhar3.Visible = false;
            lblId1.Visible = false;
            lblId2.Visible = false;
            lblId3.Visible = false;
            lblAadhar.Text = "";
            clearMemPhoto();

            txtDeclIncome.Text = "0.00";
            ddlIncFrequency.SelectedIndex = -1;
            ddlCoAppBusType.SelectedIndex = -1;
            txtCoAppDeclIncome.Text = "0.00";
            ddlCoAppIncFrequency.SelectedIndex = -1;
            ddlBusActivity.SelectedIndex = -1;
            ddlCoAppBusActivity.SelectedIndex = -1;
            hdEMIObligation.Value = "0";
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

        protected void ddlMemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CExistMember oNM = new CExistMember();
            DataTable dt1 = null, dt2 = null, dt3 = null, dt4 = null; ;
            ViewState["MemId"] = "";
            DataSet ds = new DataSet();
            lblAadhar.Text = "";
            ds = oNM.GetExistMemberData(ddlMemName.SelectedValue.ToString(), Convert.ToInt32(Session[gblValue.UserId]));
            dt1 = ds.Tables[0];
            dt2 = ds.Tables[1];
            dt3 = ds.Tables[2];
            dt4 = ds.Tables[3];

            if (dt1.Rows.Count > 0)
            {
                txtMemID.Text = Convert.ToString(dt1.Rows[0]["MemberId"]);
                ViewState["MemId"] = Convert.ToString(dt1.Rows[0]["MemberId"]);
                txtFName.Text = Convert.ToString(dt1.Rows[0]["MF_Name"]);
                txtMName.Text = Convert.ToString(dt1.Rows[0]["MM_Name"]);
                txtLName.Text = Convert.ToString(dt1.Rows[0]["ML_Name"]);
                txtHFName.Text = Convert.ToString(dt1.Rows[0]["FamilyPersonName"]);
                string vStat = Convert.ToString(dt1.Rows[0]["HumanRelationId"]);
                if (vStat == "4")
                    chkFath.Checked = true;
                else
                    chkFath.Checked = false;
                txtDOB.Text = Convert.ToString(dt1.Rows[0]["MDOB"]);
                txtAge.Text = Convert.ToString(dt1.Rows[0]["Age"]);
                txtCustId.Text = Convert.ToString(dt1.Rows[0]["IDBICustId"]);
                txtSavingsAcNo.Text = Convert.ToString(dt1.Rows[0]["IDBISavingsAcNo"]);
                txtMaidenNmF.Text = Convert.ToString(dt1.Rows[0]["MaidenNmF"]);
                txtMaidenNmM.Text = Convert.ToString(dt1.Rows[0]["MaidenNmM"]);
                txtMaidenNmL.Text = Convert.ToString(dt1.Rows[0]["MaidenNmL"]);

                txtBranchDistance.Text = Convert.ToString(dt1.Rows[0]["Distance_frm_Branch"]);
                txtCollCenterDistance.Text = Convert.ToString(dt1.Rows[0]["Distance_frm_Coll_Center"]);
                //txtIdProof3.Text = Convert.ToString(dt1.Rows[0]["AddProfNo2"]);

                txtHouNo.Text = Convert.ToString(dt1.Rows[0]["HouseNo"]);
                txtStName.Text = Convert.ToString(dt1.Rows[0]["Street"]);
                txtWardNo.Text = Convert.ToString(dt1.Rows[0]["WardNo"]);
                txtPOff.Text = Convert.ToString(dt1.Rows[0]["PostOff"]);
                txtPin.Text = Convert.ToString(dt1.Rows[0]["PIN"]);
                txtMob.Text = Convert.ToString(dt1.Rows[0]["MobileNo"]);
                txtLandMark.Text = Convert.ToString(dt1.Rows[0]["Landmark"]);
                txtArea.Text = Convert.ToString(dt1.Rows[0]["Area"]);
                txtEmail.Text = Convert.ToString(dt1.Rows[0]["EmailId"]);

                txtCommHouseNo.Text = Convert.ToString(dt1.Rows[0]["HouseNo_p"]);
                txtCommSt.Text = Convert.ToString(dt1.Rows[0]["Street_p"]);
                txtCommSubDist.Text = Convert.ToString(dt1.Rows[0]["WardNo_p"]);
                txtCommPost.Text = Convert.ToString(dt1.Rows[0]["PostOff_p"]);
                txtCommPin.Text = Convert.ToString(dt1.Rows[0]["PIN_p"]);
                txtCommMob.Text = Convert.ToString(dt1.Rows[0]["MobileNo_p"]);
                txtCommLandmark.Text = Convert.ToString(dt1.Rows[0]["Landmark_p"]);
                txtCommArea.Text = Convert.ToString(dt1.Rows[0]["Area_p"]);
                txtCommEmail.Text = Convert.ToString(dt1.Rows[0]["EmailId_p"]);
                txtStayYear.Text = Convert.ToString(dt1.Rows[0]["YearsOfStay"]);

                //txtAddPrfNo.Text = Convert.ToString(dt1.Rows[0]["AddProfNo"]);
                //txtIdntPrfNo.Text = Convert.ToString(dt1.Rows[0]["IdentyProfNo"]);

                txtPhNo.Text = Convert.ToString(dt1.Rows[0]["M_Phone"]);
                txtBranch.Text = Convert.ToString(dt1.Rows[0]["BankBranch"]);
                txtAccNo.Attributes.Add("value", Convert.ToString(dt1.Rows[0]["AccNo"]));
                //txtAccNo.Text = Convert.ToString(dt1.Rows[0]["AccNo"]);
                txtIFSC.Text = Convert.ToString(dt1.Rows[0]["IFSCCode"]);
                txtMemNamePBook.Text = Convert.ToString(dt1.Rows[0]["MemNamePBook"]);
                txtBFName.Text = Convert.ToString(dt1.Rows[0]["B_FName"]);
                txtBMName.Text = Convert.ToString(dt1.Rows[0]["B_MName"]);
                txtBLName.Text = Convert.ToString(dt1.Rows[0]["B_LName"]);
                txtBAge.Text = Convert.ToString(dt1.Rows[0]["B_Age"]);
                txtBDOBDt.Text = Convert.ToString(dt1.Rows[0]["DOB"]);
                txtBHouNo.Text = Convert.ToString(dt1.Rows[0]["B_HouseNo"]);
                txtBStreet.Text = Convert.ToString(dt1.Rows[0]["B_Street"]);
                txtBPOff.Text = Convert.ToString(dt1.Rows[0]["B_PostOff"]);
                txtBWardNo.Text = Convert.ToString(dt1.Rows[0]["B_WardNo"]);
                txtBPin.Text = Convert.ToString(dt1.Rows[0]["B_PIN"]);
                txtCoBrwrAddr.Text = Convert.ToString(dt1.Rows[0]["CoBrwrAddr"]);
                txtBHouNo.Text = Convert.ToString(dt1.Rows[0]["B_HouseNo"]);
                txtBHouNo.Text = Convert.ToString(dt1.Rows[0]["B_HouseNo"]);

                if (dt1.Rows[0]["B_IdentyProfId"].ToString() == "1")
                {
                    lblBAadhar.Value = Convert.ToString(dt1.Rows[0]["B_IdentyProfNo"]);
                    txtBIdntPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt1.Rows[0]["B_IdentyProfNo"]).Substring(Convert.ToString(dt1.Rows[0]["B_IdentyProfNo"]).Length - 4, 4));
                }
                else
                {
                    txtBIdntPrfNo.Text = Convert.ToString(dt1.Rows[0]["B_IdentyProfNo"]);
                }
                if (dt1.Rows[0]["B_AddProfId"].ToString() == "1")
                {
                    lblBAadhar.Value = Convert.ToString(dt1.Rows[0]["B_AddProfNo"]);
                    txtBAddPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt1.Rows[0]["B_AddProfNo"]).Substring(Convert.ToString(dt1.Rows[0]["B_AddProfNo"]).Length - 4, 4));
                }
                else
                {
                    txtBAddPrfNo.Text = Convert.ToString(dt1.Rows[0]["B_AddProfNo"]);
                }

                //txtBIdntPrfNo.Text = Convert.ToString(dt1.Rows[0]["B_IdentyProfNo"]);
                //txtBAddPrfNo.Text = Convert.ToString(dt1.Rows[0]["B_AddProfNo"]);
                txtBLandmark.Text = Convert.ToString(dt1.Rows[0]["B_Landmark"]);
                txtBArea.Text = Convert.ToString(dt1.Rows[0]["B_Area"]);
                txtBEmail.Text = Convert.ToString(dt1.Rows[0]["B_Email"]);
                txtBMobNo.Text = Convert.ToString(dt1.Rows[0]["B_Mobile"]);

                txtGuarName.Text = Convert.ToString(dt1.Rows[0]["GuarFName"]);
                txtGuarLName.Text = Convert.ToString(dt1.Rows[0]["GuarLName"]);
                txtGuarDOB.Text = Convert.ToString(dt1.Rows[0]["GuarDOB"]);
                txtGuarAge.Text = Convert.ToString(dt1.Rows[0]["GuarAge"]);

                txtIncome.Text = Convert.ToString(dt1.Rows[0]["IncAmt"]);
                txtExpnses.Text = Convert.ToString(dt1.Rows[0]["ExpAmt"]);
                txtPjMeetDt.Text = Convert.ToString(dt1.Rows[0]["PjMeetDt"]);
                txtBPLNo.Text = Convert.ToString(dt1.Rows[0]["BPLNO"]);
                txtNoOfDpndnts.Text = Convert.ToString(dt1.Rows[0]["NoOfDependants"]);
                txtNoOfHouseMember.Text = Convert.ToString(dt1.Rows[0]["No_of_House_Member"]);
                txtNoOfChild.Text = Convert.ToString(dt1.Rows[0]["No_of_Children"]);
                txtBankName.Text = Convert.ToString(dt1.Rows[0]["BankName"]);

                if (Convert.ToInt32(Session[gblValue.RoleId]) != 1)
                {
                    if (dt1.Rows[0]["IdentyPRofId"].ToString() == "1")
                    {
                        lblAadhar.Text = Convert.ToString(dt1.Rows[0]["IdentyProfNo"]);
                        txtIdntPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt1.Rows[0]["IdentyProfNo"]).Substring(Convert.ToString(dt1.Rows[0]["IdentyProfNo"]).Length - 4, 4));
                    }
                    else
                    {
                        txtIdntPrfNo.Text = Convert.ToString(dt1.Rows[0]["IdentyProfNo"]);
                    }
                    if (dt1.Rows[0]["AddProfId"].ToString() == "1")
                    {
                        lblAadhar.Text = Convert.ToString(dt1.Rows[0]["AddProfNo"]);
                        txtIdntPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt1.Rows[0]["AddProfNo"]).Substring(Convert.ToString(dt1.Rows[0]["AddProfNo"]).Length - 4, 4));
                    }
                    else
                    {
                        txtAddPrfNo.Text = Convert.ToString(dt1.Rows[0]["AddProfNo"]);
                    }
                    if (dt1.Rows[0]["AddProfId2"].ToString() == "1")
                    {
                        lblAadhar.Text = Convert.ToString(dt1.Rows[0]["AddProfId2"]);
                        txtIdProof3.Text = String.Format("{0}{1}", "********", Convert.ToString(dt1.Rows[0]["AddProfNo2"]).Substring(Convert.ToString(dt1.Rows[0]["AddProfNo2"]).Length - 4, 4));
                    }
                    else
                    {
                        txtIdProof3.Text = Convert.ToString(dt1.Rows[0]["AddProfNo2"]);
                    }
                }
                else
                {
                    if (dt1.Rows[0]["IdentyPRofId"].ToString() == "1")
                    {
                        lblAadhar.Text = Convert.ToString(dt1.Rows[0]["IdentyProfNo"]);
                    }
                    else if (dt1.Rows[0]["AddProfId"].ToString() == "1")
                    {
                        lblAadhar.Text = Convert.ToString(dt1.Rows[0]["AddProfNo"]);
                    }
                    else if (dt1.Rows[0]["AddProfId2"].ToString() == "1")
                    {
                        lblAadhar.Text = Convert.ToString(dt1.Rows[0]["AddProfId2"]);
                    }
                    txtIdntPrfNo.Text = Convert.ToString(dt1.Rows[0]["IdentyProfNo"]);
                    txtAddPrfNo.Text = Convert.ToString(dt1.Rows[0]["AddProfNo"]);
                    txtIdProof3.Text = Convert.ToString(dt1.Rows[0]["AddProfNo2"]);
                }

                ddlIdentyProf.SelectedIndex = ddlIdentyProf.Items.IndexOf(ddlIdentyProf.Items.FindByValue(dt1.Rows[0]["IdentyPRofId"].ToString()));
                ddlAddPrf.SelectedIndex = ddlAddPrf.Items.IndexOf(ddlAddPrf.Items.FindByValue(dt1.Rows[0]["AddProfId"].ToString()));

                ddlPvLine.SelectedIndex = ddlPvLine.Items.IndexOf(ddlPvLine.Items.FindByValue(dt1.Rows[0]["APL"].ToString()));

                ddlGuarRel.SelectedIndex = ddlGuarRel.Items.IndexOf(ddlGuarRel.Items.FindByValue(dt1.Rows[0]["GuarRel"].ToString()));
                ddlGuarGen.SelectedIndex = ddlGuarGen.Items.IndexOf(ddlGuarGen.Items.FindByValue(dt1.Rows[0]["GuarGen"].ToString()));
                //ddlVillg.SelectedIndex = ddlVillg.Items.IndexOf(ddlVillg.Items.FindByValue(dt1.Rows[0]["VillageId"].ToString()));
                txtVillg.Text = dt1.Rows[0]["Village"].ToString();
                ddlStat.SelectedIndex = ddlStat.Items.IndexOf(ddlStat.Items.FindByText(dt1.Rows[0]["State"].ToString()));
                txtDist.Text = Convert.ToString(dt1.Rows[0]["District"]);

                ddlCOName.SelectedIndex = ddlCOName.Items.IndexOf(ddlCOName.Items.FindByValue(dt1.Rows[0]["EoId"].ToString()));
                ddlMarket.SelectedIndex = ddlMarket.Items.IndexOf(ddlMarket.Items.FindByValue(dt1.Rows[0]["MarketId"].ToString()));
                ddlGroupName.SelectedIndex = ddlGroupName.Items.IndexOf(ddlGroupName.Items.FindByValue(dt1.Rows[0]["GroupId"].ToString()));
                ddlRO.SelectedIndex = ddlRO.Items.IndexOf(ddlRO.Items.FindByValue(dt1.Rows[0]["EoId"].ToString()));
                ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt1.Rows[0]["MarketId"].ToString()));
                ddlGroup.SelectedIndex = ddlGroup.Items.IndexOf(ddlGroup.Items.FindByValue(dt1.Rows[0]["GroupId"].ToString()));
                ddlEduc.SelectedIndex = ddlEduc.Items.IndexOf(ddlEduc.Items.FindByValue(dt1.Rows[0]["M_QualificationId"].ToString()));
                ddlMrySts.SelectedIndex = ddlMrySts.Items.IndexOf(ddlMrySts.Items.FindByValue(dt1.Rows[0]["MM_Status"].ToString()));
                ddlOcup.SelectedIndex = ddlOcup.Items.IndexOf(ddlOcup.Items.FindByValue(dt1.Rows[0]["M_OccupationId"].ToString()));

                ddlGend.SelectedIndex = ddlGend.Items.IndexOf(ddlGend.Items.FindByValue(dt1.Rows[0]["M_Gender"].ToString()));
                ddlRelg.SelectedIndex = ddlRelg.Items.IndexOf(ddlRelg.Items.FindByValue(dt1.Rows[0]["M_RelgId"].ToString()));
                ddlCaste.SelectedIndex = ddlCaste.Items.IndexOf(ddlCaste.Items.FindByValue(dt1.Rows[0]["M_Caste"].ToString()));
                ddlBusType.SelectedIndex = ddlBusType.Items.IndexOf(ddlBusType.Items.FindByValue(dt1.Rows[0]["MemBusinessTypeId"].ToString()));
                txtMemEmail.Text = dt1.Rows[0]["MemEmailId"].ToString();
                ddlCoAppMaritalStat.SelectedIndex = ddlCoAppMaritalStat.Items.IndexOf(ddlCoAppMaritalStat.Items.FindByValue(dt1.Rows[0]["CoAppMaritalStat"].ToString()));
                //ddlBankName.SelectedIndex = ddlBankName.Items.IndexOf(ddlBankName.Items.FindByText(dt1.Rows[0]["BankName"].ToString()));

                ddlBRel.SelectedIndex = ddlBRel.Items.IndexOf(ddlBRel.Items.FindByValue(dt1.Rows[0]["B_HumanRelationId"].ToString()));
                ddlBEdu.SelectedIndex = ddlBEdu.Items.IndexOf(ddlBEdu.Items.FindByValue(dt1.Rows[0]["B_QualificationId"].ToString()));
                ddlBOcup.SelectedIndex = ddlBOcup.Items.IndexOf(ddlBOcup.Items.FindByValue(dt1.Rows[0]["B_OccupationId"].ToString()));
                ddlBGend.SelectedIndex = ddlBGend.Items.IndexOf(ddlBGend.Items.FindByValue(dt1.Rows[0]["B_Gender"].ToString()));
                ddlBRelg.SelectedIndex = ddlBRelg.Items.IndexOf(ddlBRelg.Items.FindByValue(dt1.Rows[0]["B_RelgId"].ToString()));
                ddlBCaste.SelectedIndex = ddlBCaste.Items.IndexOf(ddlBCaste.Items.FindByValue(dt1.Rows[0]["B_Caste"].ToString()));
                ddlBVillg.SelectedIndex = ddlBVillg.Items.IndexOf(ddlBVillg.Items.FindByValue(dt1.Rows[0]["B_VillageID"].ToString()));
                ddlBIdntyProf.SelectedIndex = ddlBIdntyProf.Items.IndexOf(ddlBIdntyProf.Items.FindByValue(dt1.Rows[0]["B_IdentyProfId"].ToString()));
                ddlBAddProf.SelectedIndex = ddlBAddProf.Items.IndexOf(ddlBAddProf.Items.FindByValue(dt1.Rows[0]["B_AddProfId"].ToString()));
                ddlHHoldTyp.SelectedIndex = ddlHHoldTyp.Items.IndexOf(ddlHHoldTyp.Items.FindByValue(dt1.Rows[0]["HouseHoldId"].ToString()));
                ddlAccType.SelectedIndex = ddlAccType.Items.IndexOf(ddlAccType.Items.FindByValue(dt1.Rows[0]["Acc_Type"].ToString()));

                popAgainstVillage();
                //popAgainstGroup();
                PopCollDay();

                // txtMemAddr.Text = txtHouNo.Text + "," + txtStName.Text + "," + ddlVillg.SelectedItem.Text.ToString() + "," + ddlMuPanc.SelectedItem.Text.ToString() + "," + ddlBlk.SelectedItem.Text.ToString() + "," + txtWardNo.Text + "," + txtPOff.Text + "," + txtPin.Text;
                txtMemAddr.Text = txtHouNo.Text + "," + txtStName.Text + "," + txtVillg.Text + "," + txtWardNo.Text + "," + txtPOff.Text + "," + txtPin.Text;

                txtFamilyInc.Text = Convert.ToString(dt1.Rows[0]["FamilyIncome"]);
                txtSelfInc.Text = Convert.ToString(dt1.Rows[0]["MemIncome"]);
                txtTotInc.Text = Convert.ToString(dt1.Rows[0]["TotFamilyIncome"]);
                txtLnInsAmt.Text = Convert.ToString(dt1.Rows[0]["EMI_Obligation"]);
                txtTotExp.Text = Convert.ToString(dt1.Rows[0]["EMI_Obligation"]);
                hdEMIObligation.Value = Convert.ToString(dt1.Rows[0]["EMI_Obligation"]);
                txtSurplus.Text = Convert.ToString(Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text));
                txtEligibleEMI.Text = Convert.ToString(dt1.Rows[0]["EligibleEMI"]);

                //if (dt2.Rows.Count > 0)
                //{

                //    ddlOtherIncSrc.SelectedIndex = ddlOtherIncSrc.Items.IndexOf(ddlOtherIncSrc.Items.FindByValue(Convert.ToString(dt2.Rows[0]["OtherIncomeSrcId"].ToString())));
                //    txtOtherInc.Text = Convert.ToString(dt2.Rows[0]["OtherIncome"]);


                //    txtHsRntAmt.Text = Convert.ToString(dt2.Rows[0]["ExHsRntAmt"]);
                //    txtFdAmt.Text = Convert.ToString(dt2.Rows[0]["ExpFdAmt"]);
                //    txtEduAmt.Text = Convert.ToString(dt2.Rows[0]["ExpEduAmt"]);
                //    txtMedAmt.Text = Convert.ToString(dt2.Rows[0]["ExpMedAmt"]);
                //    txtLnInsAmt.Text = Convert.ToString(dt2.Rows[0]["ExpLnInsAmt"]);
                //    txtFuelExp.Text = Convert.ToString(dt2.Rows[0]["ExpFuelAmt"]);
                //    txtOtherExp.Text = Convert.ToString(dt2.Rows[0]["ExpOtherAmt"]);
                //    txtElecExp.Text = Convert.ToString(dt2.Rows[0]["ExpElectricAmt"]);
                //    txtTransExp.Text = Convert.ToString(dt2.Rows[0]["ExpTransAmt"]);
                //    txtTotExp.Text = Convert.ToString(dt2.Rows[0]["TotalExp"]);
                //    txtSurplus.Text = Convert.ToString(dt2.Rows[0]["Surplus"]);
                //}
                ddlIdProof3.SelectedIndex = ddlIdProof3.Items.IndexOf(ddlIdProof3.Items.FindByValue(dt1.Rows[0]["AddProfId2"].ToString()));
                ddlAddrType.SelectedIndex = ddlAddrType.Items.IndexOf(ddlAddrType.Items.FindByValue(dt1.Rows[0]["AddrType"].ToString()));
                ddlCommAddrType.SelectedIndex = ddlCommAddrType.Items.IndexOf(ddlCommAddrType.Items.FindByValue(dt1.Rows[0]["AddrType_p"].ToString()));
                ddlCommVill.SelectedIndex = ddlCommVill.Items.IndexOf(ddlCommVill.Items.FindByValue(dt1.Rows[0]["VillageId_p"].ToString()));
                ddlAreaCategory.SelectedIndex = ddlAreaCategory.Items.IndexOf(ddlAreaCategory.Items.FindByValue(dt1.Rows[0]["Area_Category"].ToString()));
                if (dt3.Rows.Count > 0)
                {
                    DataRow dF1;
                    dF1 = dt3.NewRow();
                    dt3.Rows.Add(dF1);
                    dt3.AcceptChanges();
                    ViewState["Asset"] = dt3;
                    gvAsset.DataSource = dt3;
                    gvAsset.DataBind();
                }
                memberKYC(ddlMemName.SelectedValue.ToString());

                txtDeclIncome.Text = dt1.Rows[0]["DeclaredInc"].ToString();
                ddlIncFrequency.SelectedIndex = ddlIncFrequency.Items.IndexOf(ddlIncFrequency.Items.FindByValue(dt1.Rows[0]["IncFrequency"].ToString()));
                txtCoAppDeclIncome.Text = dt1.Rows[0]["CoAppDeclaredInc"].ToString();
                ddlCoAppBusType.SelectedIndex = ddlCoAppBusType.Items.IndexOf(ddlCoAppBusType.Items.FindByValue(dt1.Rows[0]["CoAppBusinessTypeId"].ToString()));
                ddlCoAppIncFrequency.SelectedIndex = ddlCoAppIncFrequency.Items.IndexOf(ddlCoAppIncFrequency.Items.FindByValue(dt1.Rows[0]["CoAppIncFrequency"].ToString()));

                popBusinessActv(Convert.ToInt32(ddlBusType.SelectedValue));
                popCoAppBusinessActv(Convert.ToInt32(ddlCoAppBusType.SelectedValue));
                ddlBusActivity.SelectedIndex = ddlBusActivity.Items.IndexOf(ddlBusActivity.Items.FindByValue(dt1.Rows[0]["BusinessActvId"].ToString()));
                ddlCoAppBusActivity.SelectedIndex = ddlCoAppBusActivity.Items.IndexOf(ddlCoAppBusActivity.Items.FindByValue(dt1.Rows[0]["CoAppBusinessActvId"].ToString()));

                if (dt4.Rows.Count > 0)
                {
                    dt4.TableName = "TableData";
                    ViewState["EarningMember"] = dt4;
                    gvEarningMember.DataSource = dt4;
                    gvEarningMember.DataBind();
                }
                else
                {
                    GenerateEarningMember();
                }
                gvEarningMember.Enabled = false;
            }
            else
            {
                ClearControls();
            }
        }

        private void PopCollDay()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vGroupId = ddlGroupName.SelectedValue.ToString();
            DateTime vAdmDt = gblFuction.setDate(txtAdmDt.Text);
            CCenter oCent = null;
            try
            {
                oCent = new CCenter();
                dt = oCent.GetCollDayByGroupId(vGroupId, vBrCode, vAdmDt);
                if (dt.Rows.Count > 0)
                {
                    txtMetDay.Text = dt.Rows[0]["CollDay"].ToString();
                    //popRoutineDay();
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
            popMember();
            ViewState["StateEdit"] = "Add";
            btnCgtApply.Enabled = false;
            StatusButton("Add");
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
                // LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
                EnableControl(false);

            }
        }

        private Boolean SaveRecords(string Mode)
        {
            pathMember = ConfigurationManager.AppSettings["pathMember"];
            this.GetModuleByRole(mnuID.mnuMemberMst);
            DateTime vEndDt;
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vMemId = Convert.ToString(ViewState["MemId"]);
            Int32 vErr = 0, vRec = 0, vQuliId = 0, vMAge = 0, vMOcupId = 0, vMVilgId = 0, vMAddProfId = 0, vMIdProfId = 0;
            Int32 vBQuliId = 0, vBAge = 0, vBOcupId = 0, vBVilgId = 0, vBAddProfId = 0, vBIdProfId = 0, vHHoldId = 0, vNoOfDpndnt = 0;
            Int32 vBRelId = 0;
            string vErrDesc = "", vXmlEarningMemDtl = "";
            string vDrop = Convert.ToString(ViewState["vDropState"]);
            string vMM_Stat = "", vChkFath = "N", vCltype = "", vMsg = "";
            double vIncAmt = 0, vExpAmt = 0;
            DateTime vAdmDt = gblFuction.setDate(txtAdmDt.Text);
            DateTime vClDt = gblFuction.setDate(txtDtCl.Text);
            DateTime vDOBDt = gblFuction.setDate(txtDOB.Text);
            DateTime vBDOBDt = gblFuction.setDate(txtBDOBDt.Text);
            DateTime vPjMeetDt = gblFuction.setDate(txtPjMeetDt.Text);
            CApplication oCG = null;
            Int32 vMemBusTypeId = -1, vOtherIncomeSrcId = -1;
            string vMemEMailId = string.Empty, vCoAppMaritalStat = string.Empty;
            Int32 vIncFrequency = 0, vCoAppBusTypeId = 0, vCoAppIncFrequency = 0;
            double vDeclIncome = 0, vCoAppDeclIncome = 0;
            Int32 vBusActvId = 0, vCoAppBusActvId = 0;

            string vMaidenNmF = txtMaidenNmF.Text == "" ? "" : txtMaidenNmF.Text.Trim();
            string vMaidenNmM = txtMaidenNmM.Text == "" ? "" : txtMaidenNmM.Text.Trim();
            string vMaidenNmL = txtMaidenNmL.Text == "" ? "" : txtMaidenNmL.Text.Trim();
            string vMemAddr = txtMemAddr.Text == "" ? "" : txtMemAddr.Text.Trim();
            string vCoBrwrAddr = txtCoBrwrAddr.Text == "" ? "" : txtCoBrwrAddr.Text.Trim();

            DataTable dt2 = null;
            string vXmlAsset = "";

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
            //}
            dt2.AcceptChanges();
            dt2.TableName = "Table2";
            using (StringWriter oSW = new StringWriter())
            {
                dt2.WriteXml(oSW);
                vXmlAsset = oSW.ToString();
            }

            string vCGTYN = hdnApplyCgt.Value;
            if (vCGTYN == null) vCGTYN = "N";
            CMember oMem = null;
            CNewMember oNmem = null;
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

            //if (cbDrp.Checked == false)
            //{
            //    if (ValidateField(vMemId, vBrCode) == false) return false;
            //}
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
                //vMVilgId = Convert.ToInt32(ddlVillg.SelectedValue);
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
                vCltype = Convert.ToString(ddlClTyp.SelectedValue);

                if (txtNoOfDpndnts.Text != "")
                {
                    vNoOfDpndnt = Convert.ToInt32(txtNoOfDpndnts.Text);
                }
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
                    vRec = oGbl.ChkDuplicate("MemberMst", "M_Mobile", txtMob.Text.Replace("'", "''"), "", "", "MemberID", vMemId, "Edit");
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

                    //////////////////////////////////////////////////////                    

                    oTra = new CTransfer();
                    dt = oTra.ChkMemberByGroupId(ddlGroup.SelectedValue, vBrCode);
                    if (Convert.ToInt32(dt.Rows[0]["MemCount"].ToString()) > 35)
                    {
                        gblFuction.AjxMsgPopup("You can not enter More than 35 member in one Group..");
                        return false;
                    }

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

                    oNmem = new CNewMember();
                    vErr = oNmem.SaveNewMember(ref vMemId, vAdmDt, txtFName.Text.Replace("'", "''"),
                      txtMName.Text.Replace("'", "''"), txtLName.Text.Replace("'", "''"),
                      vMM_Stat, vQuliId, txtHFName.Text.Replace("'", "''"), vChkFath, vDOBDt, vMAge,
                      vMOcupId, ddlGend.SelectedValue, ddlRelg.SelectedValue, ddlCaste.SelectedValue,
                      txtHouNo.Text.Replace("'", "''"), txtStName.Text.Replace("'", "''"), txtVillg.Text,
                      txtWardNo.Text.Replace("'", "''"), txtPOff.Text.Replace("'", "''"), txtPin.Text,
                      txtMob.Text, txtPhNo.Text, vMAddProfId, vMAddProfId == 1 ? lblAadhar.Text : txtAddPrfNo.Text.Replace("'", "''"),
                      vMIdProfId, vMIdProfId == 1 ? lblAadhar.Text : txtIdntPrfNo.Text.Replace("'", "''"), txtBFName.Text.Replace("'", "''"),
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
                      txtIFSC.Text, "N", vBrCode, this.UserID, "Update", vCGTYN, txtMemNamePBook.Text, vMaidenNmF, vMaidenNmM, vMaidenNmL, vMemAddr,
                      vCoBrwrAddr, vXmlAsset, Convert.ToDouble(txtFamilyInc.Text), Convert.ToDouble(txtSelfInc.Text), Convert.ToDouble(txtOtherInc.Text),
                      Convert.ToDouble(txtTotInc.Text), Convert.ToDouble(txtHsRntAmt.Text), Convert.ToDouble(txtFdAmt.Text), Convert.ToDouble(txtEduAmt.Text), Convert.ToDouble(txtMedAmt.Text), Convert.ToDouble(txtLnInsAmt.Text)
                      , Convert.ToDouble(txtFuelExp.Text), Convert.ToDouble(txtElecExp.Text), Convert.ToDouble(txtTransExp.Text), Convert.ToDouble(txtOtherExp.Text), Convert.ToDouble(txtTotExp.Text), Convert.ToDouble(txtSurplus.Text),
                      gblFuction.setDate(Session[gblValue.LoginDate].ToString()), txtGuarName.Text, txtGuarLName.Text, Convert.ToInt32(ddlGuarRel.SelectedValue), ddlGuarGen.SelectedValue, txtGuarDOB.Text, Convert.ToInt32(txtGuarAge.Text), txtCustId.Text, txtSavingsAcNo.Text,
                      Convert.ToString(ddlMemName.SelectedValue), Convert.ToInt16(ddlAddrType.SelectedValue), txtLandMark.Text.ToUpper(), txtArea.Text.ToUpper(), txtEmail.Text, Convert.ToInt16(ddlCommAddrType.SelectedValue), txtCommHouseNo.Text, txtCommSt.Text, txtCommLandmark.Text,
                      txtCommArea.Text, Convert.ToInt16(ddlCommVill.SelectedValue), txtCommSubDist.Text, txtCommPost.Text, txtCommPin.Text, txtCommMob.Text, txtCommPhone.Text, txtCommEmail.Text, Convert.ToInt16(ddlAreaCategory.SelectedValue), Convert.ToDouble(txtStayYear.Text), Convert.ToInt16(ddlIdProof3.SelectedValue),
                      txtIdProof3.Text, txtBLandmark.Text, txtBArea.Text, txtBEmail.Text, Convert.ToInt16(ddlAccType.SelectedValue), Convert.ToInt16(txtNoOfHouseMember.Text), Convert.ToInt16(txtNoOfChild.Text),
                      Convert.ToDouble(txtBranchDistance.Text), Convert.ToDouble(txtCollCenterDistance.Text), ref vErrDesc, ddlStat.SelectedItem.Text, txtDist.Text,
                      vMemBusTypeId, vMemEMailId, vCoAppMaritalStat, vOtherIncomeSrcId,
                      vXmlEarningMemDtl, vDeclIncome, vIncFrequency, vCoAppBusTypeId, vCoAppDeclIncome, vCoAppIncFrequency, vBusActvId, vCoAppBusActvId);

                    if (vErr == 1)
                    {
                        if (fuMemberKYC.HasFile)
                        {
                            //string extension = Path.GetExtension(fuMemberKYC.PostedFile.FileName);
                            //string fileName = Path.Combine(Server.MapPath("~/G:/WebApps/CentrumMobile/Files/Member/" + vMemId), "PassbookImage" + extension);
                            //fuMemberKYC.SaveAs(fileName);
                            string vMessage = SaveMemberImages(fuMemberKYC, vMemId, "PassbookImage", "Edit", "N", pathMember);
                        }
                        ViewState["EoId"] = vMemId;
                        ViewState["MemId"] = vMemId;
                        vResult = true;
                    }
                    else if (vErr == 5)
                    {
                        gblFuction.AjxMsgPopup("Provided Bank AC No. already registered with Other Member.");
                        vResult = false;
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
                //ddlBVillg.SelectedIndex = ddlVillg.SelectedIndex;
                if (ddlBVillg.SelectedIndex >= 0)
                {
                    PopAllAgainstVillage();
                    ddlBMunPanca.SelectedIndex = ddlMuPanc.SelectedIndex;
                    ddlBBlk.SelectedIndex = ddlBlk.SelectedIndex;
                    //ddlBDist.SelectedIndex = ddlDist.SelectedIndex;
                    // ddlBStat.SelectedIndex = ddlStat.SelectedIndex;
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
            // int Years = CalAge(txtBDOBDt.Text);
            int Years = AgeCount(txtBDOBDt.Text, Convert.ToString(Session[gblValue.LoginDate]));
            if (Years < 18)
            {
                gblFuction.AjxMsgPopup("Co borrower age should Greater than 18 Years.");
                txtBAge.Text = "0";
                txtBDOBDt.Text = "";
            }
            else if (Years > 59)
            {
                gblFuction.AjxMsgPopup("Co borrower age should less than 59 Years.");
                txtBAge.Text = "0";
                txtBDOBDt.Text = "";
            }
            else
            {
                if (txtBDOBDt.Text.Length >= 10)
                    txtBAge.Text = Years.ToString();
            }
            //txtBDOBDt.Focus();
        }


        protected void txtGuarDOB_TextChanged(object sender, EventArgs e)
        {
            //int Years = CalAge(txtGuarDOB.Text);
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

        //private void memberKYC(string InitialApproachId)
        //{
        //    string imgFolder = InitialApproachId;
        //    string vUrl = "https://centrummob.bijliftt.com/Files/InitialApproach/";
        //    //string vUrl = "https://centrummobtest.bijliftt.com/Files/InitialApproach/";

        //    imgMemPhoto.ImageUrl = vUrl + imgFolder + "/MemberPhoto.png";
        //    imgMemIdProof.ImageUrl = vUrl + imgFolder + "/IDProofImage.png";
        //    imgMemAddrProof.ImageUrl = vUrl + imgFolder + "/AddressProofImage.png";

        //    WebRequest request = WebRequest.Create("https://centrummob.bijliftt.com/Files/InitialApproach/" + imgFolder + "/MemberPhoto.png");
        //    //WebRequest request = WebRequest.Create("https://centrummobtest.com/Files/InitialApproach/" + imgFolder + "/MemberPhoto.png");
        //    try
        //    {
        //        request.GetResponse();
        //        txtUrl.Text = "0";
        //    }
        //    catch (Exception ex)
        //    {
        //        txtUrl.Text = "1";
        //    }  

        //}
        private void memberKYC(string InitialApproachId)
        {
            string imgFolder = InitialApproachId;
            string pathImage = ConfigurationManager.AppSettings["PathImage"];
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

            WebRequest request = WebRequest.Create(vUrl + imgFolder + "/MemberPhoto.png");
            WebRequest request1 = WebRequest.Create(vUrl + imgFolder + "/IDProofImage.png");
            WebRequest request2 = WebRequest.Create(vUrl + imgFolder + "/AddressProofImage.png");
            //WebRequest request3 = WebRequest.Create(vUrl + imgFolder + "/CoAppPhoto.png");
            //WebRequest request4 = WebRequest.Create(vUrl + imgFolder + "/CoAppIDProofImage.png");
            WebRequest request3 = WebRequest.Create(vUrl + imgFolder + "/AddressProofImage2.png");
            WebRequest request4 = WebRequest.Create(vUrl + imgFolder + "/FrontSelfeImage.png");

            try
            {
                request.GetResponse();
                request1.GetResponse();
                request2.GetResponse();
                request3.GetResponse();
                request4.GetResponse();
                txtUrl.Text = "0";
            }
            catch (Exception ex)
            {
                txtUrl.Text = "1";
            }
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
                        // ReduceImageSize(0.5, strm, targetFile);
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
                    txtIdProof3.Attributes["type"] = "password";
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
                    lblId1.Visible = true;
                    txtBConfrmAadhar1.Visible = true;
                    txtBIdntPrfNo.Text = "";
                    txtBIdntPrfNo.Attributes["type"] = "password";
                }
            }
            else
            {
                txtBConfrmAadhar1.Attributes.Add("value", "");
                lblId1.Visible = false;
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
                    lblId2.Visible = true;
                    txtBConfrmAadhar2.Visible = true;
                    txtBAddPrfNo.Text = "";
                    txtBAddPrfNo.Attributes["type"] = "password";
                }
            }
            else
            {
                txtBConfrmAadhar2.Attributes.Add("value", "");
                lblId2.Visible = false;
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
                DropDownList ddlBusinessType = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlBusinessType");
                DropDownList ddlOccupationType = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlOccupationType");
                DropDownList ddlIncomeFrequency = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlIncomeFrequency");
                TextBox txtDeclaredIncome = (TextBox)gvEarningMember.Rows[gr.RowIndex].FindControl("txtDeclaredIncome");
                DropDownList ddlBusinessActv = (DropDownList)gvEarningMember.Rows[gr.RowIndex].FindControl("ddlBusinessActv");
                HiddenField hdnKYCNo = (HiddenField)gvEarningMember.Rows[gr.RowIndex].FindControl("hdnKYCNo");


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
                    dt.Rows[gr.RowIndex]["KYCNo"] = hdnKYCNo.Value;
                    dt.Rows[gr.RowIndex]["MaskedKYCNo"] = ddlKYCType.SelectedValue == "1" ? String.Format("{0}{1}", "********", txtKYCNo.Text.Substring(txtKYCNo.Text.Length - 4, 4)) : txtKYCNo.Text;
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
