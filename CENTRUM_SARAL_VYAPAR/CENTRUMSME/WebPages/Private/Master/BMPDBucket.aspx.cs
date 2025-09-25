using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Configuration;

namespace CENTRUM_SARALVYAPAR.WebPages.Private.Master
{
    public partial class BMPDBucket : CENTRUMBAse
    {
        string ImgPath = ConfigurationManager.AppSettings["PathImage"];
        string PdBucket = ConfigurationManager.AppSettings["PdBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBasePage();
                ViewState["PdId"] = "0";
                txtDtFrm.Text = txtDtTo.Text = Session[gblValue.LoginDate].ToString();
                popCaste();
                popStateList();
                PopQualification();
                PopRelation();
                PopVillage();
                PopPurpose();
                popBusinessType();
                popIdentityProof();
                popAddProof();
                popSpeciallyAbled();
                StatusButton("View");
                EnablePdControl(false);
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                {
                    Response.Redirect("~/login.aspx", false);
                }
                this.Menu = false;
                this.PageHeading = "Personal Discussion (BM)";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuPDBM);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanReject != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnSendBack.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanReject != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSendBack.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanReject != "Y")
                {
                    btnDelete.Visible = false;
                    btnSendBack.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanReject != "Y")
                {
                    btnAdd.Visible = false;
                    btnDelete.Visible = false;
                    btnSendBack.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanReject != "Y")
                {                  
                    btnSendBack.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanReject == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Personal Discussion", false);
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
                    btnSendBack.Enabled = false;
                    ClearControls();

                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnSendBack.Enabled = false;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnSendBack.Enabled = true;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnSendBack.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnSendBack.Enabled = false;
                    EnableControl(false);
                    break;
            }
        }

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
            //StatusButton("Add");
            //ClearControls();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.CanEdit == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Edit);
                return;
            }
            tbEmp.ActiveTabIndex = 1;
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbEmp.ActiveTabIndex = 0;
            StatusButton("View");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                EnableControl(false);
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
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


        private void EnableControl(Boolean Status)
        {
            ddlAbledYN.Enabled = false;
            ddlSpclAbled.Enabled = false;

            ddlPreScrQ1.Enabled = Status;
            ddlPreScrQ2.Enabled = Status;
            ddlPreScrQ3.Enabled = Status;
            ddlPreScrQ4.Enabled = Status;
            ddlPreScrQ5.Enabled = Status;
            ddlPreScrQ6.Enabled = Status;
            ddlPreScrQ7.Enabled = Status;

            ddlLoanReqDtlCorrect.Enabled = Status;
            ddlApplDtlCorrect.Enabled = Status;
            ddlCoApplDtlcorrect.Enabled = Status;
            ddlOthrInfocorrect.Enabled = Status;
            ddlProxyInfoCorrect.Enabled = Status;
            ddlBankInfoCorrect.Enabled = Status;
            ddlApplResiRelCorrect.Enabled = Status;
            ddlApplBusInfoCorrect.Enabled = Status;
            ddlCoApplBusInfoCorrect.Enabled = Status;
            ddlBusinessPhotoCorrect.Enabled = Status;
            ddlSendBackReason.Enabled = Status;
        }

        private void EnablePdControl(Boolean Status)
        {
            ddlAbledYN.Enabled = false;
            ddlSpclAbled.Enabled = false;
            ddlLnPurpose.Enabled = Status;
            ddlLoanPurpose.Enabled = Status;
            txtExpLnAmt.Enabled = Status;
            ddlExpTenure.Enabled = Status;
            txtEMIPayingCap.Enabled = Status;
            txtExistingLoan.Enabled = Status;
            txtTotalOutstanding.Enabled = Status;
            ddlTitle.Enabled = Status;
            txtApplFName.Enabled = Status;
            txtApplMName.Enabled = Status;
            txtApplLName.Enabled = Status;
            ddlApplGender.Enabled = Status;
            ddlApplMaritalStatus.Enabled = Status;
            ddlApplEdu.Enabled = Status;
            ddlApplReligion.Enabled = Status;
            ddlApplReligStat.Enabled = Status;
            ddlApplCast.Enabled = Status;
            ddlApplAddrType.Enabled = Status;
            txtApplHouNo.Enabled = false;
            txtApplStrName.Enabled = false;
            txtApplLandMark.Enabled = false;
            txtApplArea.Enabled = false;
            txtApplVillg.Enabled = false;
            txtApplWardNo.Enabled = false;
            txtApplPOff.Enabled = false;
            txtApplPin.Enabled = false;
            txtApplDist.Enabled = false;
            ddlApplStat.Enabled = false;
            ddlMinorityYN.Enabled = false;

            ddlApplCommAddrType.Enabled = Status;
            txtApplCommHouseNo.Enabled = Status;
            txtApplCommSt.Enabled = Status;
            txtApplCommLandmark.Enabled = Status;
            txtApplCommArea.Enabled = Status;
            ddlApplCommVill.Enabled = Status;
            txtApplCommSubDist.Enabled = Status;
            txtApplCommPost.Enabled = Status;
            txtApplCommPin.Enabled = Status;
            txtApplMobile.Enabled = Status;
            txtApplContactNo.Enabled = Status;
            ddlApplPhyfit.Enabled = Status;

            ddlCoAppTitle.Enabled = Status;
            txtCoApplName.Enabled = Status;
            txtCoApplDOB.Enabled = Status;
            txtCoApplAge.Enabled = Status;
            ddlCoApplGen.Enabled = Status;
            ddlCoApplMaritalStat.Enabled = Status;
            ddlCoApplRel.Enabled = Status;
            ddlCoApplEdu.Enabled = Status;

            txtCoApplAddr.Enabled = false;
            ddlCoApplState.Enabled = false;
            txtCoApplPin.Enabled = false;
            txtCoApplMobile.Enabled = Status;
            txtCoApplContactNo.Enabled = Status;
            ddlCoApplPhyFitness.Enabled = Status;

            ddlCpAppOthrIncm.Enabled = Status;
            ddlCoApplTypeOfInc.Enabled = Status;
            ddlAgeIncEar.Enabled = Status;
            ddlAnnulInc.Enabled = Status;
            ddlHouseStab.Enabled = Status;
            ddlTypeOfOwnership.Enabled = Status;
            ddlTypeOfResidence.Enabled = Status;
            ddlResidenceCategory.Enabled = Status;
            ddlTotalNoOfFamily.Enabled = Status;
            ddlNoOfChild.Enabled = Status;
            ddlNoOfDependent.Enabled = Status;
            ddlNoOfFamilyEM.Enabled = Status;
            chkFamilyAsset.Enabled = Status;
            chkOtherAsset.Enabled = Status;
            ddlLandHolding.Enabled = Status;
            ddlBankHabit.Enabled = Status;
            chkOtherSavings.Enabled = Status;
            txtPersonalRef.Enabled = Status;
            txtOtherInfoAddr.Enabled = Status;
            txtOtherInfoMob.Enabled = Status;
            ddlOtherInfoValidate.Enabled = Status;

            ddlProxyMob.Enabled = Status;
            ddlProxyRef.Enabled = Status;
            ddlProxy2Wheeler.Enabled = Status;
            ddlProxy3Wheeler.Enabled = Status;
            ddlProxy4Wheeler.Enabled = Status;
            ddlProxyAC.Enabled = Status;
            ddlProxyWashingMachine.Enabled = Status;
            ddlProxyEmailId.Enabled = Status;
            ddlProxyPAN.Enabled = Status;
            ddlProxyGSTNo.Enabled = Status;
            ddlProxyITR.Enabled = Status;
            ddlProxyWhatsApp.Enabled = Status;
            ddlProxyFB.Enabled = Status;

            txtACHolderName.Enabled = Status;
            txtACNo.Enabled = Status;
            txtReAcNo.Enabled = Status;
            txtIFSC.Enabled = Status;
            ddlAcType.Enabled = Status;

            txtApplBusiName.Enabled = Status;
            ddlApplBusiNameBoard.Enabled = Status;
            ddlApplPrimaryBusiType.Enabled = Status;
            ddlApplPrimaryBusiSeaso.Enabled = Status;
            ddlApplPrimaryBusiSubType.Enabled = Status;
            ddlApplPrimaryBusiActivity.Enabled = Status;

            ddlApplNoOfWorkDay.Enabled = Status;
            ddlApplMonthTrun.Enabled = Status;
            ddlApplLocalityArea.Enabled = Status;
            txtBusiEstDt.Enabled = Status;

            txtApplBusiAddr.Enabled = Status;
            chkApplOtherBusi.Enabled = Status;
            ddlApplBusiVintage.Enabled = Status;
            ddlApplBusiOwnerType.Enabled = Status;
            ddlApplBusiHndlPerson.Enabled = Status;
            ddlApplBusiPart.Enabled = Status;
            ddlApplBusiNoOfEmp.Enabled = Status;

            ddlApplBusiValueOfStock.Enabled = Status;
            ddlApplBusiValueOfMachinery.Enabled = Status;
            ddlApplBusiHours.Enabled = Status;
            ddlApplBusiAppName.Enabled = Status;
            txtApplBusiVPAId.Enabled = Status;
            ddlApplBusiTranProofType.Enabled = Status;
            txtApplBusiCashInHand.Enabled = Status;
            txtApplBusiRef.Enabled = Status;
            txtApplBusiAddress.Enabled = Status;
            txtApplBusiMobNo.Enabled = Status;
            ddlApplBusiValidated.Enabled = Status;
            ddlApplSecBusiYN.Enabled = Status;
            chkApplSecBusi.Enabled = Status;

            ddlApplSecBusiType1.Enabled = Status;
            ddlApplSecBusiSeas1.Enabled = Status;
            ddlApplSecBusiSubType1.Enabled = Status;
            ddlApplSecBusiAct1.Enabled = Status;

            ddlApplSecBusiType2.Enabled = Status;
            ddlApplSecBusiSeas2.Enabled = Status;
            ddlApplSecBusiSubType2.Enabled = Status;
            ddlApplSecBusiAct2.Enabled = Status;

            ddlCoApplBusiIncYN.Enabled = Status;
            ddlCoApplBusiName.Enabled = Status;
            txtCoApplBusiName.Enabled = Status;
            ddlCoApplPrimaryBusiType.Enabled = Status;
            ddlCoApplPrimaryBusiSeas.Enabled = Status;
            ddlCoApplPrimaryBusiSubType.Enabled = Status;
            ddlCoApplPrimaryBusiAct.Enabled = Status;
            ddlCoApplBusiMonthlyTrunover.Enabled = Status;
            txtCoApplBusiAddr.Enabled = Status;

            ddlCoApplBusiVintage.Enabled = Status;
            ddlCoApplBusiOwnerType.Enabled = Status;
            ddlCoApplBusiStock.Enabled = Status;

            txtApplDOB.Enabled = Status;
            txtApplAge.Enabled = Status;
            txtApplAdiContactNo.Enabled = Status;
            txtBankName.Enabled = Status;
            ddlCoAppCommState.Enabled = Status;
            txtCoAppCommAddr.Enabled = Status;
            txtCoAppCommPin.Enabled = Status;

            ddlCoApplPerAddrType.Enabled = Status;
            ddlCoApplPreAddrType.Enabled = Status;

            ddlIdentyProf.Enabled = Status;
            ddlAddPrf.Enabled = Status;
            txtIdntPrfNo.Enabled = Status;
            txtAddPrfNo.Enabled = Status;
            ddlCoAppID.Enabled = Status;
            txtCoAppID.Enabled = Status;
        }

        private void ClearControls()
        {
            ddlAbledYN.SelectedIndex = -1;
            ddlSpclAbled.SelectedIndex = -1;
            hdnEnqId.Value = "";
            ddlLoanPurpose.SelectedIndex = -1;
            ddlLnPurpose.SelectedIndex = -1;
            txtExpLnAmt.Text = "0";
            ddlExpTenure.SelectedIndex = -1;
            txtEMIPayingCap.Text = "0";
            txtExistingLoan.Text = "0";
            txtTotalOutstanding.Text = "0";
            ddlTitle.SelectedIndex = -1;
            txtApplFName.Text = "";
            txtApplMName.Text = "";
            txtApplLName.Text = "";
            ddlApplGender.SelectedIndex = -1;
            ddlApplMaritalStatus.SelectedIndex = -1;
            ddlApplEdu.SelectedIndex = -1;
            ddlApplReligion.SelectedIndex = -1;
            ddlApplReligStat.SelectedIndex = -1;
            ddlApplCast.SelectedIndex = -1;
            ddlApplAddrType.SelectedIndex = -1;
            txtApplHouNo.Text = "";
            txtApplStrName.Text = "";
            txtApplLandMark.Text = "";
            txtApplArea.Text = "";
            txtApplVillg.Text = "";
            txtApplWardNo.Text = "";
            txtApplPOff.Text = "";
            txtApplPin.Text = "";
            txtApplDist.Text = "";
            ddlApplStat.SelectedIndex = -1;

            ddlApplCommAddrType.SelectedIndex = -1;
            txtApplCommHouseNo.Text = "";
            txtApplCommSt.Text = "";
            txtApplCommLandmark.Text = "";
            txtApplCommArea.Text = "";
            ddlApplCommVill.SelectedIndex = -1;
            txtApplCommSubDist.Text = "";
            txtApplCommPost.Text = "";
            txtApplCommPin.Text = "";
            txtApplMobile.Text = "";
            txtApplContactNo.Text = "";
            ddlApplPhyfit.SelectedIndex = -1;

            ddlCoAppTitle.SelectedIndex = -1;
            txtCoApplName.Text = "";
            txtCoApplDOB.Text = "";
            txtCoApplAge.Text = "0";
            ddlCoApplGen.SelectedIndex = -1;
            ddlCoApplMaritalStat.SelectedIndex = -1;
            ddlCoApplRel.SelectedIndex = -1;
            ddlCoApplEdu.SelectedIndex = -1;

            txtCoApplAddr.Text = "";
            ddlCoApplState.SelectedIndex = -1;
            txtCoApplPin.Text = "";
            txtCoApplMobile.Text = "";
            txtCoApplContactNo.Text = "";
            ddlCoApplPhyFitness.SelectedIndex = -1;

            ddlCpAppOthrIncm.SelectedIndex = -1;
            ddlCoApplTypeOfInc.SelectedIndex = -1;
            ddlAgeIncEar.SelectedIndex = -1;
            ddlAnnulInc.SelectedIndex = -1;
            ddlHouseStab.SelectedIndex = -1;
            ddlTypeOfOwnership.SelectedIndex = -1;
            ddlTypeOfResidence.SelectedIndex = -1;
            ddlResidenceCategory.SelectedIndex = -1;
            ddlTotalNoOfFamily.SelectedIndex = -1;
            ddlNoOfChild.SelectedIndex = -1;
            ddlNoOfDependent.SelectedIndex = -1;
            ddlNoOfFamilyEM.SelectedIndex = -1;
            //chkFamilyAsset.Text = "";
            //chkOtherAsset.Text = "";
            ddlLandHolding.SelectedIndex = -1;
            ddlBankHabit.SelectedIndex = -1;
            // chkOtherSavings.Text = "";
            txtPersonalRef.Text = "";
            txtOtherInfoAddr.Text = "";
            txtOtherInfoMob.Text = "";
            ddlOtherInfoValidate.SelectedIndex = -1;

            ddlProxyMob.SelectedIndex = -1;
            ddlProxyRef.SelectedIndex = -1;
            ddlProxy2Wheeler.SelectedIndex = -1;
            ddlProxy3Wheeler.SelectedIndex = -1;
            ddlProxy4Wheeler.SelectedIndex = -1;
            ddlProxyAC.SelectedIndex = -1;
            ddlProxyWashingMachine.SelectedIndex = -1;
            ddlProxyEmailId.SelectedIndex = -1;
            ddlProxyPAN.SelectedIndex = -1;
            ddlProxyGSTNo.SelectedIndex = -1;
            ddlProxyITR.SelectedIndex = -1;
            ddlProxyWhatsApp.SelectedIndex = -1;
            ddlProxyFB.SelectedIndex = -1;

            txtACHolderName.Text = "";
            txtACNo.Text = "";
            txtReAcNo.Text = "";
            txtIFSC.Text = "";
            ddlAcType.SelectedIndex = -1;

            txtApplBusiName.Text = "";
            ddlApplBusiNameBoard.SelectedIndex = -1;
            ddlApplPrimaryBusiType.SelectedIndex = -1;
            ddlApplPrimaryBusiSeaso.SelectedIndex = -1;
            ddlApplPrimaryBusiSubType.SelectedIndex = -1;
            ddlApplPrimaryBusiActivity.SelectedIndex = -1;

            ddlApplNoOfWorkDay.SelectedIndex = -1;
            ddlApplMonthTrun.SelectedIndex = -1;
            ddlApplLocalityArea.SelectedIndex = -1;
            txtBusiEstDt.Text = "";

            txtApplBusiAddr.Text = "";
            //chkApplOtherBusi.Text = "";
            ddlApplBusiVintage.SelectedIndex = -1;
            ddlApplBusiOwnerType.SelectedIndex = -1;
            ddlApplBusiHndlPerson.SelectedIndex = -1;
            ddlApplBusiPart.SelectedIndex = -1;
            ddlApplBusiNoOfEmp.SelectedIndex = -1;

            ddlApplBusiValueOfStock.SelectedIndex = -1;
            ddlApplBusiValueOfMachinery.SelectedIndex = -1;
            ddlApplBusiHours.SelectedIndex = -1;
            ddlApplBusiAppName.SelectedIndex = -1;
            txtApplBusiVPAId.Text = "";
            ddlApplBusiTranProofType.SelectedIndex = -1;
            txtApplBusiCashInHand.Text = "0";
            txtApplBusiRef.Text = "";
            txtApplBusiAddress.Text = "";
            txtApplBusiMobNo.Text = "";
            ddlApplBusiValidated.SelectedIndex = -1;
            ddlApplSecBusiYN.SelectedIndex = -1;


            ddlApplSecBusiType1.SelectedIndex = -1;
            ddlApplSecBusiSeas1.SelectedIndex = -1;
            ddlApplSecBusiSubType1.SelectedIndex = -1;
            ddlApplSecBusiAct1.SelectedIndex = -1;

            ddlApplSecBusiType2.SelectedIndex = -1;
            ddlApplSecBusiSeas2.SelectedIndex = -1;
            ddlApplSecBusiSubType2.SelectedIndex = -1;
            ddlApplSecBusiAct2.SelectedIndex = -1;

            ddlCoApplBusiIncYN.SelectedIndex = -1;
            ddlCoApplBusiName.SelectedIndex = -1;
            txtCoApplBusiName.Text = "";
            ddlCoApplPrimaryBusiType.SelectedIndex = -1;
            ddlCoApplPrimaryBusiSeas.SelectedIndex = -1;
            ddlCoApplPrimaryBusiSubType.SelectedIndex = -1;
            ddlCoApplPrimaryBusiAct.SelectedIndex = -1;
            ddlCoApplBusiMonthlyTrunover.SelectedIndex = -1;
            txtCoApplBusiAddr.Text = "";

            ddlCoApplBusiVintage.SelectedIndex = -1;
            ddlCoApplBusiOwnerType.SelectedIndex = -1;
            ddlCoApplBusiStock.SelectedIndex = -1;


            ddlPreScrQ1.SelectedIndex = -1;
            ddlPreScrQ2.SelectedIndex = -1;
            ddlPreScrQ3.SelectedIndex = -1;
            ddlPreScrQ4.SelectedIndex = -1;
            ddlPreScrQ5.SelectedIndex = -1;
            ddlPreScrQ6.SelectedIndex = -1;
            ddlPreScrQ7.SelectedIndex = -1;

            ddlLoanReqDtlCorrect.SelectedIndex = -1;
            ddlApplDtlCorrect.SelectedIndex = -1;
            ddlCoApplDtlcorrect.SelectedIndex = -1;
            ddlOthrInfocorrect.SelectedIndex = -1;
            ddlProxyInfoCorrect.SelectedIndex = -1;
            ddlBankInfoCorrect.SelectedIndex = -1;
            ddlApplResiRelCorrect.SelectedIndex = -1;
            ddlApplBusInfoCorrect.SelectedIndex = -1;
            ddlCoApplBusInfoCorrect.SelectedIndex = -1;
            ddlBusinessPhotoCorrect.SelectedIndex = -1;

            txtApplDOB.Text = "";
            txtApplAge.Text = "";
            txtApplAdiContactNo.Text = "";
            txtBankName.Text = "";
            txtCoAppCommAddr.Text = "";
            txtCoAppCommPin.Text = "";
            ddlCoAppCommState.SelectedIndex = -1;
            ddlCoApplPerAddrType.SelectedIndex = -1;
            ddlCoApplPreAddrType.SelectedIndex = -1;

            ddlIdentyProf.SelectedIndex = -1;
            ddlAddPrf.SelectedIndex = -1;
            txtIdntPrfNo.Text = "";
            txtAddPrfNo.Text = "";
            txtCoAppID.Text = "";
            ddlCoAppID.SelectedIndex = -1;
        }

        protected void chkApplCommAddr_CheckedChanged(object sender, EventArgs e)
        {
            if (chkApplCommAddr.Checked == true)
            {
                ddlApplCommAddrType.SelectedIndex = ddlApplAddrType.SelectedIndex;
                txtApplCommHouseNo.Text = txtApplHouNo.Text;
                txtApplCommSt.Text = txtApplStrName.Text;
                txtApplCommSubDist.Text = txtApplWardNo.Text;
                txtApplCommPost.Text = txtApplPOff.Text;
                txtApplCommPin.Text = txtApplPin.Text;
                txtApplCommLandmark.Text = txtApplLandMark.Text;
                txtApplCommArea.Text = txtApplArea.Text;

                txtApplCommHouseNo.Enabled = false;
                txtApplCommSt.Enabled = false;
                txtApplCommSubDist.Enabled = false;
                txtApplCommPost.Enabled = false;
                txtApplCommPin.Enabled = false;
                // ddlApplCommVill.Enabled = false;
                txtApplLandMark.Enabled = false;
                txtApplCommArea.Enabled = false;
                txtApplCommLandmark.Enabled = false;
            }
            else
            {
                txtApplCommHouseNo.Text = "";
                txtApplCommSt.Text = "";
                txtApplCommSubDist.Text = "";
                txtApplCommPost.Text = "";
                txtApplCommPin.Text = "";
                ddlApplCommVill.SelectedIndex = -1;
                ddlApplCommDist.SelectedIndex = -1;
                ddlApplCommState.SelectedIndex = -1;
                ddlApplCommAddrType.SelectedIndex = -1;

                txtApplCommHouseNo.Enabled = true;
                txtApplCommSt.Enabled = true;
                txtApplCommSubDist.Enabled = true;
                txtApplCommPost.Enabled = true;
                txtApplCommPin.Enabled = true;
                ddlApplCommVill.Enabled = true;
                txtApplCommLandmark.Enabled = true;
                txtApplCommArea.Enabled = true;
            }
        }

        protected void chkCoApplPerAddr_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCoApplPerAddr.Checked == true)
            {
                txtCoApplAddr.Text = txtApplHouNo.Text + "," + txtApplCommSt.Text + "," + txtApplVillg.Text + "," + txtApplWardNo.Text + "," + txtApplPOff.Text + "," + txtApplCommPin.Text;
                ddlCoApplState.SelectedIndex = ddlApplStat.SelectedIndex;
                txtCoApplPin.Text = txtApplPin.Text;

                txtCoApplAddr.Enabled = false;
                ddlCoApplState.Enabled = false;
                txtCoApplPin.Enabled = false;
            }
            else
            {
                txtCoApplAddr.Text = "";
                txtCoApplPin.Text = "";
                ddlCoApplState.SelectedIndex = -1;

                txtCoApplAddr.Enabled = true;
                ddlCoApplState.Enabled = true;
                txtCoApplPin.Enabled = true;
            }
        }

        protected void chkCoAppCommAdr_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void ddlApplReligStat_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pRMId = Convert.ToInt32(ddlApplReligStat.SelectedValue);
            PopReligion(pRMId);
        }

        protected void ddlApplCommVill_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CVillage oVlg = null;
            string vVlgId = ddlApplCommVill.SelectedValue;
            try
            {
                oVlg = new CVillage();
                dt = oVlg.GetGpBlkDistStateList(vVlgId);
                ddlApplCommState.DataSource = ddlApplCommDist.DataSource = dt;

                ddlApplCommDist.DataTextField = "DistrictName";
                ddlApplCommDist.DataValueField = "DistrictId";
                ddlApplCommDist.DataBind();

                ddlApplCommState.DataTextField = "StateName";
                ddlApplCommState.DataValueField = "StateId";
                ddlApplCommState.DataBind();
                ddlApplCommState.Enabled = false;
                ddlApplCommDist.Enabled = false;
            }
            finally
            {
                oVlg = null;
                dt = null;
            }
        }

        protected void ddlLoanPurpose_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopSubPurpose(Convert.ToInt32(ddlLoanPurpose.SelectedValue));
        }

        private void popCaste()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetCaste();
                ddlApplCast.DataSource = dt;
                ddlApplCast.DataTextField = "Caste";
                ddlApplCast.DataValueField = "CasteId";
                ddlApplCast.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlApplCast.Items.Insert(0, oli1);
            }
            finally
            {
            }
        }

        private void popStateList()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetStateList();
                ddlApplStat.DataSource = dt;
                ddlApplStat.DataTextField = "StateName";
                ddlApplStat.DataValueField = "StateId";
                ddlApplStat.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlApplStat.Items.Insert(0, oli1);

                ddlCoApplState.DataSource = dt;
                ddlCoApplState.DataTextField = "StateName";
                ddlCoApplState.DataValueField = "StateId";
                ddlCoApplState.DataBind();
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ddlCoApplState.Items.Insert(0, oli2);

                ddlCoAppCommState.DataSource = dt;
                ddlCoAppCommState.DataTextField = "StateName";
                ddlCoAppCommState.DataValueField = "StateId";
                ddlCoAppCommState.DataBind();
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ddlCoAppCommState.Items.Insert(0, oli3);
            }
            finally
            {
            }
        }

        private void PopQualification()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetQualification();
                ddlCoApplEdu.DataSource = dt;
                ddlCoApplEdu.DataTextField = "QualificationName";
                ddlCoApplEdu.DataValueField = "QualificationId";
                ddlCoApplEdu.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlCoApplEdu.Items.Insert(0, oli1);

                ddlApplEdu.DataSource = dt;
                ddlApplEdu.DataTextField = "QualificationName";
                ddlApplEdu.DataValueField = "QualificationId";
                ddlApplEdu.DataBind();
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ddlApplEdu.Items.Insert(0, oli2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void PopRelation()
        {
            DataTable dt = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                dt = oMem.GetRelationList();
                ddlCoApplRel.DataSource = dt;
                ddlCoApplRel.DataTextField = "Relation";
                ddlCoApplRel.DataValueField = "RelationId";
                ddlCoApplRel.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlCoApplRel.Items.Insert(0, oli1);
            }
            finally
            {
                oMem = null;
                dt = null;
            }
        }

        private void PopVillage()
        {
            DataTable dt = null;
            CVillage oGb = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oGb = new CVillage();
                dt = oGb.PopVillage(vBrCode);
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlApplCommVill.DataSource = dt;
                ddlApplCommVill.DataTextField = "VillageName";
                ddlApplCommVill.DataValueField = "VillageId";
                ddlApplCommVill.DataBind();
                ddlApplCommVill.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void PopPurpose()
        {
            DataTable dt = null;
            CPurpose oGb = null;
            oGb = new CPurpose();
            dt = oGb.PopPurpose();
            ddlLoanPurpose.DataSource = dt;
            ddlLoanPurpose.DataTextField = "PurposeName";
            ddlLoanPurpose.DataValueField = "PurposeID";
            ddlLoanPurpose.DataBind();
            ListItem oli = new ListItem("<--Select-->", "-1");
            ddlLoanPurpose.Items.Insert(0, oli);
        }

        private void PopSubPurpose(int pPurposeId)
        {
            DataTable dt = null;
            CPurpose oGb = null;
            oGb = new CPurpose();
            dt = oGb.PopSubPurpose(pPurposeId);
            ddlLnPurpose.DataSource = dt;
            ddlLnPurpose.DataTextField = "PurposeName";
            ddlLnPurpose.DataValueField = "PurposeID";
            ddlLnPurpose.DataBind();
            ListItem oli = new ListItem("<--Select-->", "-1");
            ddlLnPurpose.Items.Insert(0, oli);
        }

        private void PopReligion(int pRMId)
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetReligionById(pRMId);
                ddlApplReligion.DataSource = dt;
                ddlApplReligion.DataTextField = "Religion";
                ddlApplReligion.DataValueField = "ReligionId";
                ddlApplReligion.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlApplReligion.Items.Insert(0, oli1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void PopDistrictStatebyVillageId(string vVlgId)
        {
            DataTable dt = null;
            CVillage oVlg = null;
            try
            {
                oVlg = new CVillage();
                dt = oVlg.GetGpBlkDistStateList(vVlgId);
                ddlApplCommState.DataSource = ddlApplCommDist.DataSource = dt;

                ddlApplCommDist.DataTextField = "DistrictName";
                ddlApplCommDist.DataValueField = "DistrictId";
                ddlApplCommDist.DataBind();

                ddlApplCommState.DataTextField = "StateName";
                ddlApplCommState.DataValueField = "StateId";
                ddlApplCommState.DataBind();
                ddlApplCommState.Enabled = false;
                ddlApplCommDist.Enabled = false;
            }
            finally
            {
                oVlg = null;
                dt = null;
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
                ddlApplPrimaryBusiType.DataSource = dt;
                ddlApplPrimaryBusiType.DataTextField = "BusinessTypeName";
                ddlApplPrimaryBusiType.DataValueField = "BusinessTypeId";
                ddlApplPrimaryBusiType.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlApplPrimaryBusiType.Items.Insert(0, oli);

                ddlApplSecBusiType1.DataSource = dt;
                ddlApplSecBusiType1.DataTextField = "BusinessTypeName";
                ddlApplSecBusiType1.DataValueField = "BusinessTypeId";
                ddlApplSecBusiType1.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlApplSecBusiType1.Items.Insert(0, oli1);

                ddlApplSecBusiType2.DataSource = dt;
                ddlApplSecBusiType2.DataTextField = "BusinessTypeName";
                ddlApplSecBusiType2.DataValueField = "BusinessTypeId";
                ddlApplSecBusiType2.DataBind();
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ddlApplSecBusiType2.Items.Insert(0, oli2);

                ddlCoApplPrimaryBusiType.DataSource = dt;
                ddlCoApplPrimaryBusiType.DataTextField = "BusinessTypeName";
                ddlCoApplPrimaryBusiType.DataValueField = "BusinessTypeId";
                ddlCoApplPrimaryBusiType.DataBind();
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ddlCoApplPrimaryBusiType.Items.Insert(0, oli3);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popApplBusiSubType(int pBusinessTypeId)
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            try
            {
                oGb = new CBusinessType();
                dt = oGb.PopBusiSubTypeByTypeId(pBusinessTypeId);
                ddlApplPrimaryBusiSubType.DataSource = dt;
                ddlApplPrimaryBusiSubType.DataTextField = "BusinessSubType";
                ddlApplPrimaryBusiSubType.DataValueField = "BusinessSubTypeID";
                ddlApplPrimaryBusiSubType.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlApplPrimaryBusiSubType.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popApplSecBusiSubType1(int pBusinessTypeId)
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            try
            {
                oGb = new CBusinessType();
                dt = oGb.PopBusiSubTypeByTypeId(pBusinessTypeId);
                ddlApplSecBusiSubType1.DataSource = dt;
                ddlApplSecBusiSubType1.DataTextField = "BusinessSubType";
                ddlApplSecBusiSubType1.DataValueField = "BusinessSubTypeID";
                ddlApplSecBusiSubType1.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlApplSecBusiSubType1.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popApplSecBusiSubType2(int pBusinessTypeId)
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            try
            {
                oGb = new CBusinessType();
                dt = oGb.PopBusiSubTypeByTypeId(pBusinessTypeId);
                ddlApplSecBusiSubType2.DataSource = dt;
                ddlApplSecBusiSubType2.DataTextField = "BusinessSubType";
                ddlApplSecBusiSubType2.DataValueField = "BusinessSubTypeID";
                ddlApplSecBusiSubType2.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlApplSecBusiSubType2.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popCoApplSecBusiSubType(int pBusinessTypeId)
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            try
            {
                oGb = new CBusinessType();
                dt = oGb.PopBusiSubTypeByTypeId(pBusinessTypeId);
                ddlCoApplPrimaryBusiSubType.DataSource = dt;
                ddlCoApplPrimaryBusiSubType.DataTextField = "BusinessSubType";
                ddlCoApplPrimaryBusiSubType.DataValueField = "BusinessSubTypeID";
                ddlCoApplPrimaryBusiSubType.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCoApplPrimaryBusiSubType.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void PopApplBusiActivity(int pBusinessSubTypeId)
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            try
            {
                oGb = new CBusinessType();
                dt = oGb.PopBusiActivityBySubTypeId(pBusinessSubTypeId);
                ddlApplPrimaryBusiActivity.DataSource = dt;
                ddlApplPrimaryBusiActivity.DataTextField = "BusinessActivity";
                ddlApplPrimaryBusiActivity.DataValueField = "BusinessActivityID";
                ddlApplPrimaryBusiActivity.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlApplPrimaryBusiActivity.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void PopApplSecBusiActivity1(int pBusinessSubTypeId)
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            try
            {
                oGb = new CBusinessType();
                dt = oGb.PopBusiActivityBySubTypeId(pBusinessSubTypeId);
                ddlApplSecBusiAct1.DataSource = dt;
                ddlApplSecBusiAct1.DataTextField = "BusinessActivity";
                ddlApplSecBusiAct1.DataValueField = "BusinessActivityID";
                ddlApplSecBusiAct1.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlApplSecBusiAct1.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void PopApplSecBusiActivity2(int pBusinessSubTypeId)
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            try
            {
                oGb = new CBusinessType();
                dt = oGb.PopBusiActivityBySubTypeId(pBusinessSubTypeId);
                ddlApplSecBusiAct2.DataSource = dt;
                ddlApplSecBusiAct2.DataTextField = "BusinessActivity";
                ddlApplSecBusiAct2.DataValueField = "BusinessActivityID";
                ddlApplSecBusiAct2.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlApplSecBusiAct2.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void PopCoApplPrimaryBusiActivity(int pBusinessSubTypeId)
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            try
            {
                oGb = new CBusinessType();
                dt = oGb.PopBusiActivityBySubTypeId(pBusinessSubTypeId);
                ddlCoApplPrimaryBusiAct.DataSource = dt;
                ddlCoApplPrimaryBusiAct.DataTextField = "BusinessActivity";
                ddlCoApplPrimaryBusiAct.DataValueField = "BusinessActivityID";
                ddlCoApplPrimaryBusiAct.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCoApplPrimaryBusiAct.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void LoadGrid()
        {
            DataTable dt = new DataTable();
            CPdMst oMem = new CPdMst();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vType = rdbOpt.SelectedValue;
            dt = oMem.GetBMPdPendingMember(gblFuction.setDate(txtDtFrm.Text), gblFuction.setDate(txtDtTo.Text), vBrCode, txtSearch.Text, vType,
                Convert.ToInt32(Session[gblValue.BCProductId]), Convert.ToInt32(Session[gblValue.UserId]));
            if (dt.Rows.Count > 0)
            {
                gvPenPD.DataSource = dt;
                gvPenPD.DataBind();
            }

        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void btnSendBack_Click(object sender, EventArgs e)
        {
            string vEnqId = hdnEnqId.Value, vRejectReason = ddlSendBackReason.SelectedValue;
            int vCreatedBy = Convert.ToInt32(Session[gblValue.UserId]), vErr = 0;
            CPdMst oPD = new CPdMst();

            vErr = oPD.RejectPD(vEnqId, vRejectReason, vCreatedBy, "PdByBm");
            if (vErr > 0)
            {
                gblFuction.MsgPopup("Data Rejected Successfully");
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }

        }

        protected void gvPenPD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vPdId = 0;
            vPdId = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                foreach (GridViewRow gr in gvPenPD.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
            }
            ViewState["PdId"] = vPdId;
            FillPdDtl(vPdId);
        }

        private void FillPdDtl(Int32 vPdId)
        {
            DataSet ds = null;
            DataTable dt, dt1, dt2, dt3, dt4 = null;
            CPdMst oPD = null;
            try
            {
                ds = new DataSet();
                dt = new DataTable();
                dt1 = new DataTable();
                dt2 = new DataTable();
                dt3 = new DataTable();
                dt4 = new DataTable();

                ViewState["PdId"] = vPdId;
                oPD = new CPdMst();
                ds = oPD.GetPdDtlByPdId(vPdId, Convert.ToInt32(Session[gblValue.UserId]), "");
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                dt2 = ds.Tables[2];
                dt3 = ds.Tables[3];
                dt4 = ds.Tables[4];

                if (dt.Rows.Count > 0)
                {
                    hdnEnqId.Value = dt.Rows[0]["EnquiryId"].ToString();
                    //ddlLnPurpose.SelectedIndex = ddlLnPurpose.Items.IndexOf(ddlLnPurpose.Items.FindByValue(dt.Rows[0]["PurposeId"].ToString()));
                    ddlLoanPurpose.SelectedIndex = ddlLoanPurpose.Items.IndexOf(ddlLoanPurpose.Items.FindByValue(dt.Rows[0]["SectorId"].ToString()));
                    PopSubPurpose(Convert.ToInt32(dt.Rows[0]["SectorId"].ToString()));
                    ddlLnPurpose.SelectedIndex = ddlLnPurpose.Items.IndexOf(ddlLnPurpose.Items.FindByValue(dt.Rows[0]["PurposeId"].ToString()));
                    txtExpLnAmt.Text = Convert.ToString(dt.Rows[0]["ExpLoanAmt"]);
                    ddlExpTenure.SelectedIndex = ddlExpTenure.Items.IndexOf(ddlExpTenure.Items.FindByValue(dt.Rows[0]["ExpLoanTenure"].ToString()));
                    ddlMinorityYN.SelectedIndex = ddlMinorityYN.Items.IndexOf(ddlMinorityYN.Items.FindByValue(dt.Rows[0]["MinorityYN"].ToString()));
                    txtEMIPayingCap.Text = Convert.ToString(dt.Rows[0]["EmiPayingCapacity"]);
                    txtExistingLoan.Text = Convert.ToString(dt.Rows[0]["ExistingLoanNo"]);
                    txtTotalOutstanding.Text = Convert.ToString(dt.Rows[0]["TotLnOS"]);
                    ddlTitle.SelectedIndex = ddlTitle.Items.IndexOf(ddlTitle.Items.FindByValue(dt.Rows[0]["ApplTitle"].ToString()));
                    txtApplFName.Text = Convert.ToString(dt.Rows[0]["ApplFName"]);
                    txtApplMName.Text = Convert.ToString(dt.Rows[0]["ApplMName"]);
                    txtApplLName.Text = Convert.ToString(dt.Rows[0]["ApplLName"]);
                    // txtApplAge.Text = Convert.ToString(dt.Rows[0]["MF_Name"]);
                    ddlApplGender.SelectedIndex = ddlApplGender.Items.IndexOf(ddlApplGender.Items.FindByValue(dt.Rows[0]["ApplGender"].ToString()));
                    ddlApplMaritalStatus.SelectedIndex = ddlApplMaritalStatus.Items.IndexOf(ddlApplMaritalStatus.Items.FindByValue(dt.Rows[0]["ApplMaritalStatus"].ToString()));
                    ddlApplEdu.SelectedIndex = ddlApplEdu.Items.IndexOf(ddlApplEdu.Items.FindByValue(dt.Rows[0]["ApplEduId"].ToString()));
                    ddlApplReligStat.SelectedIndex = ddlApplReligStat.Items.IndexOf(ddlApplReligStat.Items.FindByValue(dt.Rows[0]["ApplReliStat"].ToString()));
                    PopReligion(Convert.ToInt32(dt.Rows[0]["ApplReliStat"]));
                    ddlApplReligion.SelectedIndex = ddlApplReligion.Items.IndexOf(ddlApplReligion.Items.FindByValue(dt.Rows[0]["ApplReligion"].ToString()));
                    ddlApplCast.SelectedIndex = ddlApplCast.Items.IndexOf(ddlApplCast.Items.FindByValue(dt.Rows[0]["ApplCaste"].ToString()));

                    ddlAbledYN.SelectedIndex = ddlAbledYN.Items.IndexOf(ddlAbledYN.Items.FindByValue(dt.Rows[0]["IsAbledYN"].ToString()));
                    ddlSpclAbled.SelectedIndex = ddlSpclAbled.Items.IndexOf(ddlSpclAbled.Items.FindByValue(dt.Rows[0]["SpeciallyAbled"].ToString()));

                    ddlApplAddrType.SelectedIndex = ddlApplAddrType.Items.IndexOf(ddlApplAddrType.Items.FindByValue(dt.Rows[0]["ApplPerAddrType"].ToString()));
                    txtApplHouNo.Text = Convert.ToString(dt.Rows[0]["ApplPerHouseNo"]);
                    txtApplStrName.Text = Convert.ToString(dt.Rows[0]["ApplPerStreet"]);
                    txtApplWardNo.Text = Convert.ToString(dt.Rows[0]["ApplPerSubDist"]);
                    txtApplPOff.Text = Convert.ToString(dt.Rows[0]["ApplPerPostOffice"]);
                    txtApplLandMark.Text = Convert.ToString(dt.Rows[0]["ApplPerLandmark"]);
                    txtApplArea.Text = Convert.ToString(dt.Rows[0]["ApplPerArea"]);
                    txtApplPin.Text = Convert.ToString(dt.Rows[0]["ApplPerPIN"]);
                    txtApplDist.Text = Convert.ToString(dt.Rows[0]["ApplPerDist"]);
                    txtApplVillg.Text = Convert.ToString(dt.Rows[0]["ApplPerVillage"]);
                    txtApplAdiContactNo.Text = Convert.ToString(dt.Rows[0]["ApplPerContactNo"]);
                    txtApplMobile.Text = Convert.ToString(dt.Rows[0]["ApplMobileNo"]);
                    ddlApplStat.SelectedIndex = ddlApplStat.Items.IndexOf(ddlApplStat.Items.FindByValue(dt.Rows[0]["ApplPerStateId"].ToString()));

                    ddlApplCommAddrType.SelectedIndex = ddlApplCommAddrType.Items.IndexOf(ddlApplCommAddrType.Items.FindByValue(dt.Rows[0]["ApplPreAddrType"].ToString()));
                    txtApplCommHouseNo.Text = Convert.ToString(dt.Rows[0]["ApplPreHouseNo"]);
                    txtApplCommSt.Text = Convert.ToString(dt.Rows[0]["ApplPreStreet"]);
                    ddlApplCommVill.SelectedIndex = ddlApplCommVill.Items.IndexOf(ddlApplCommVill.Items.FindByValue(dt.Rows[0]["ApplPreVillageId"].ToString()));
                    txtApplCommSubDist.Text = Convert.ToString(dt.Rows[0]["ApplPreSubDist"]);
                    txtApplCommPost.Text = Convert.ToString(dt.Rows[0]["ApplPrePostOffice"]);
                    txtApplCommPin.Text = Convert.ToString(dt.Rows[0]["ApplPrePIN"]);
                    txtApplCommLandmark.Text = Convert.ToString(dt.Rows[0]["ApplPreLandmark"]);
                    txtApplCommArea.Text = Convert.ToString(dt.Rows[0]["ApplPreArea"]);
                    PopDistrictStatebyVillageId(dt.Rows[0]["ApplPreVillageId"].ToString());
                    ddlApplPhyfit.SelectedIndex = ddlApplPhyfit.Items.IndexOf(ddlApplPhyfit.Items.FindByValue(dt.Rows[0]["ApplPhyFitness"].ToString()));

                    ddlCoAppTitle.SelectedIndex = ddlCoAppTitle.Items.IndexOf(ddlCoAppTitle.Items.FindByValue(dt.Rows[0]["CoApplTitle"].ToString()));
                    txtCoApplName.Text = Convert.ToString(dt.Rows[0]["CoApplName"]);
                    txtCoApplDOB.Text = Convert.ToString(dt.Rows[0]["CoApplDOB"]);
                    txtCoApplAge.Text = Convert.ToString(dt.Rows[0]["CoApplAge"]);
                    ddlCoApplGen.SelectedIndex = ddlCoApplGen.Items.IndexOf(ddlCoApplGen.Items.FindByValue(dt.Rows[0]["CoApplGender"].ToString()));
                    ddlCoApplMaritalStat.SelectedIndex = ddlCoApplMaritalStat.Items.IndexOf(ddlCoApplMaritalStat.Items.FindByValue(dt.Rows[0]["CoApplMaritalStatus"].ToString()));
                    ddlCoApplRel.SelectedIndex = ddlCoApplRel.Items.IndexOf(ddlCoApplRel.Items.FindByValue(dt.Rows[0]["CoApplRelation"].ToString()));
                    ddlCoApplEdu.SelectedIndex = ddlCoApplEdu.Items.IndexOf(ddlCoApplEdu.Items.FindByValue(dt.Rows[0]["CoApplEduId"].ToString()));

                    txtCoApplAddr.Text = Convert.ToString(dt.Rows[0]["CoApplPerAddr"]);
                    ddlCoApplState.SelectedIndex = ddlCoApplState.Items.IndexOf(ddlCoApplState.Items.FindByValue(dt.Rows[0]["CoApplPerStateId"].ToString()));
                    txtCoApplPin.Text = Convert.ToString(dt.Rows[0]["CoApplPerPIN"]);
                    txtCoApplMobile.Text = Convert.ToString(dt.Rows[0]["CoApplMobileNo"]);
                    txtCoApplContactNo.Text = Convert.ToString(dt.Rows[0]["CoApplAddiContactNo"]);
                    ddlCoApplPhyFitness.SelectedIndex = ddlCoApplPhyFitness.Items.IndexOf(ddlCoApplPhyFitness.Items.FindByValue(dt.Rows[0]["CoApplPhyFitness"].ToString()));

                    ddlCpAppOthrIncm.SelectedIndex = ddlCpAppOthrIncm.Items.IndexOf(ddlCpAppOthrIncm.Items.FindByValue(dt1.Rows[0]["CoAppIncYN"].ToString()));
                    ddlCoApplTypeOfInc.SelectedIndex = ddlCoApplTypeOfInc.Items.IndexOf(ddlCoApplTypeOfInc.Items.FindByValue(dt1.Rows[0]["TypeOfInc"].ToString()));
                    ddlAgeIncEar.SelectedIndex = ddlAgeIncEar.Items.IndexOf(ddlAgeIncEar.Items.FindByValue(dt1.Rows[0]["AgeOfKeyIncEar"].ToString()));
                    ddlAnnulInc.SelectedIndex = ddlAnnulInc.Items.IndexOf(ddlAnnulInc.Items.FindByValue(dt1.Rows[0]["AnnulInc"].ToString()));
                    ddlHouseStab.SelectedIndex = ddlHouseStab.Items.IndexOf(ddlHouseStab.Items.FindByValue(dt1.Rows[0]["HouseStability"].ToString()));
                    ddlTypeOfOwnership.SelectedIndex = ddlTypeOfOwnership.Items.IndexOf(ddlTypeOfOwnership.Items.FindByValue(dt1.Rows[0]["TypeOfOwnerShip"].ToString()));
                    ddlTypeOfResidence.SelectedIndex = ddlTypeOfResidence.Items.IndexOf(ddlTypeOfResidence.Items.FindByValue(dt1.Rows[0]["TypeOfResi"].ToString()));
                    ddlResidenceCategory.SelectedIndex = ddlResidenceCategory.Items.IndexOf(ddlResidenceCategory.Items.FindByValue(dt1.Rows[0]["ResiCategory"].ToString()));
                    ddlTotalNoOfFamily.SelectedIndex = ddlTotalNoOfFamily.Items.IndexOf(ddlTotalNoOfFamily.Items.FindByValue(dt1.Rows[0]["TotalFamMember"].ToString()));
                    ddlNoOfChild.SelectedIndex = ddlNoOfChild.Items.IndexOf(ddlNoOfChild.Items.FindByValue(dt1.Rows[0]["NoOfChild"].ToString()));
                    ddlNoOfDependent.SelectedIndex = ddlNoOfDependent.Items.IndexOf(ddlNoOfDependent.Items.FindByValue(dt1.Rows[0]["NoOfDependent"].ToString()));
                    ddlNoOfFamilyEM.SelectedIndex = ddlNoOfFamilyEM.Items.IndexOf(ddlNoOfFamilyEM.Items.FindByValue(dt1.Rows[0]["NoOfFamEarMember"].ToString()));

                    //chkFamilyAsset
                    //chkOtherAsset
                    ddlLandHolding.SelectedIndex = ddlLandHolding.Items.IndexOf(ddlLandHolding.Items.FindByValue(dt1.Rows[0]["LandHolding"].ToString()));
                    ddlBankHabit.SelectedIndex = ddlBankHabit.Items.IndexOf(ddlBankHabit.Items.FindByValue(dt1.Rows[0]["BankingHabit"].ToString()));
                    //chkOtherSavings
                    txtPersonalRef.Text = Convert.ToString(dt1.Rows[0]["PersonalRef"]);
                    txtOtherInfoAddr.Text = Convert.ToString(dt1.Rows[0]["Addr"]);
                    txtOtherInfoMob.Text = Convert.ToString(dt1.Rows[0]["MobileNo"]);
                    ddlOtherInfoValidate.SelectedIndex = ddlOtherInfoValidate.Items.IndexOf(ddlOtherInfoValidate.Items.FindByValue(dt1.Rows[0]["ValidatedYN"].ToString()));

                    ddlProxyMob.SelectedIndex = ddlProxyMob.Items.IndexOf(ddlProxyMob.Items.FindByValue(dt2.Rows[0]["MobilePhone"].ToString()));
                    ddlProxyRef.SelectedIndex = ddlProxyRef.Items.IndexOf(ddlProxyRef.Items.FindByValue(dt2.Rows[0]["Refrigerator"].ToString()));
                    ddlProxy2Wheeler.SelectedIndex = ddlProxy2Wheeler.Items.IndexOf(ddlProxy2Wheeler.Items.FindByValue(dt2.Rows[0]["TwoWheeler"].ToString()));
                    ddlProxy3Wheeler.SelectedIndex = ddlProxy3Wheeler.Items.IndexOf(ddlProxy3Wheeler.Items.FindByValue(dt2.Rows[0]["ThreeWheeler"].ToString()));
                    ddlProxy4Wheeler.SelectedIndex = ddlProxy4Wheeler.Items.IndexOf(ddlProxy4Wheeler.Items.FindByValue(dt2.Rows[0]["FourWheeler"].ToString()));
                    ddlProxyAC.SelectedIndex = ddlProxyAC.Items.IndexOf(ddlProxyAC.Items.FindByValue(dt2.Rows[0]["AirConditioner"].ToString()));
                    ddlProxyWashingMachine.SelectedIndex = ddlProxyWashingMachine.Items.IndexOf(ddlProxyWashingMachine.Items.FindByValue(dt2.Rows[0]["WashingMachine"].ToString()));
                    ddlProxyEmailId.SelectedIndex = ddlProxyEmailId.Items.IndexOf(ddlProxyEmailId.Items.FindByValue(dt2.Rows[0]["EmailId"].ToString()));
                    ddlProxyPAN.SelectedIndex = ddlProxyPAN.Items.IndexOf(ddlProxyPAN.Items.FindByValue(dt2.Rows[0]["PAN"].ToString()));
                    ddlProxyGSTNo.SelectedIndex = ddlProxyGSTNo.Items.IndexOf(ddlProxyGSTNo.Items.FindByValue(dt2.Rows[0]["GSTno"].ToString()));
                    ddlProxyITR.SelectedIndex = ddlProxyITR.Items.IndexOf(ddlProxyITR.Items.FindByValue(dt2.Rows[0]["ITR"].ToString()));
                    ddlProxyWhatsApp.SelectedIndex = ddlProxyWhatsApp.Items.IndexOf(ddlProxyWhatsApp.Items.FindByValue(dt2.Rows[0]["Whatsapp"].ToString()));
                    ddlProxyFB.SelectedIndex = ddlProxyFB.Items.IndexOf(ddlProxyFB.Items.FindByValue(dt2.Rows[0]["FacebookAc"].ToString()));

                    txtACHolderName.Text = Convert.ToString(dt.Rows[0]["ACHolderName"]);
                    txtACNo.Attributes.Add("value", Convert.ToString(dt.Rows[0]["AccNo"]));
                    txtIFSC.Text = Convert.ToString(dt.Rows[0]["IfscCode"]);
                    ddlAcType.SelectedIndex = ddlAcType.Items.IndexOf(ddlAcType.Items.FindByValue(dt.Rows[0]["AccType"].ToString()));

                    txtApplBusiName.Text = Convert.ToString(dt3.Rows[0]["BusinessName"]);
                    ddlApplBusiNameBoard.SelectedIndex = ddlApplBusiNameBoard.Items.IndexOf(ddlApplBusiNameBoard.Items.FindByValue(dt3.Rows[0]["BusiNameOnBoard"].ToString().Trim()));
                    ddlApplPrimaryBusiType.SelectedIndex = ddlApplPrimaryBusiType.Items.IndexOf(ddlApplPrimaryBusiType.Items.FindByValue(dt3.Rows[0]["PrimaryBusiType"].ToString()));
                    popApplBusiSubType(Convert.ToInt32(dt3.Rows[0]["PrimaryBusiType"]));
                    ddlApplPrimaryBusiSeaso.SelectedIndex = ddlApplPrimaryBusiSeaso.Items.IndexOf(ddlApplPrimaryBusiSeaso.Items.FindByValue(dt3.Rows[0]["PrimaryBusiSeaso"].ToString().Trim()));
                    ddlApplPrimaryBusiSubType.SelectedIndex = ddlApplPrimaryBusiSubType.Items.IndexOf(ddlApplPrimaryBusiSubType.Items.FindByValue(dt3.Rows[0]["PrimaryBusiSubType"].ToString()));
                    PopApplBusiActivity(Convert.ToInt32(dt3.Rows[0]["PrimaryBusiSubType"]));
                    ddlApplPrimaryBusiActivity.SelectedIndex = ddlApplPrimaryBusiActivity.Items.IndexOf(ddlApplPrimaryBusiActivity.Items.FindByValue(dt3.Rows[0]["PrimaryBusiActivity"].ToString()));

                    ddlApplNoOfWorkDay.SelectedIndex = ddlApplNoOfWorkDay.Items.IndexOf(ddlApplNoOfWorkDay.Items.FindByValue(dt3.Rows[0]["WorkingDays"].ToString()));
                    ddlApplMonthTrun.SelectedIndex = ddlApplMonthTrun.Items.IndexOf(ddlApplMonthTrun.Items.FindByValue(dt3.Rows[0]["MonthlyTrunOver"].ToString()));
                    ddlApplLocalityArea.SelectedIndex = ddlApplLocalityArea.Items.IndexOf(ddlApplLocalityArea.Items.FindByValue(dt3.Rows[0]["LocalityArea"].ToString()));
                    txtBusiEstDt.Text = Convert.ToString(dt3.Rows[0]["BusiEstdDt"]);
                    txtApplBusiAddr.Text = Convert.ToString(dt3.Rows[0]["BusinessAddr"]);
                    //chkApplOtherBusi
                    ddlApplBusiVintage.SelectedIndex = ddlApplBusiVintage.Items.IndexOf(ddlApplBusiVintage.Items.FindByValue(dt3.Rows[0]["BusinessVintage"].ToString().Trim()));
                    ddlApplBusiOwnerType.SelectedIndex = ddlApplBusiOwnerType.Items.IndexOf(ddlApplBusiOwnerType.Items.FindByValue(dt3.Rows[0]["BusiOwnerType"].ToString().Trim()));
                    ddlApplBusiHndlPerson.SelectedIndex = ddlApplBusiHndlPerson.Items.IndexOf(ddlApplBusiHndlPerson.Items.FindByValue(dt3.Rows[0]["BusiHndlPerson"].ToString().Trim()));
                    ddlApplBusiPart.SelectedIndex = ddlApplBusiPart.Items.IndexOf(ddlApplBusiPart.Items.FindByValue(dt3.Rows[0]["PartnerYN"].ToString().Trim()));
                    ddlApplBusiNoOfEmp.SelectedIndex = ddlApplBusiNoOfEmp.Items.IndexOf(ddlApplBusiNoOfEmp.Items.FindByValue(dt3.Rows[0]["NoOfEmp"].ToString().Trim()));
                    ddlApplBusiValueOfStock.SelectedIndex = ddlApplBusiValueOfStock.Items.IndexOf(ddlApplBusiValueOfStock.Items.FindByValue(dt3.Rows[0]["ValueOfStock"].ToString().Trim()));
                    ddlApplBusiValueOfMachinery.SelectedIndex = ddlApplBusiValueOfMachinery.Items.IndexOf(ddlApplBusiValueOfMachinery.Items.FindByValue(dt3.Rows[0]["ValueOfMachinery"].ToString().Trim()));
                    ddlApplBusiHours.SelectedIndex = ddlApplBusiHours.Items.IndexOf(ddlApplBusiHours.Items.FindByValue(dt3.Rows[0]["BusiHours"].ToString().Trim()));
                    ddlApplBusiAppName.SelectedIndex = ddlApplBusiAppName.Items.IndexOf(ddlApplBusiAppName.Items.FindByValue(dt3.Rows[0]["AppName"].ToString().Trim()));

                    txtApplBusiVPAId.Text = Convert.ToString(dt3.Rows[0]["VPAID"]);
                    ddlApplBusiTranProofType.SelectedIndex = ddlApplBusiTranProofType.Items.IndexOf(ddlApplBusiTranProofType.Items.FindByValue(dt3.Rows[0]["BusiTranProofType"].ToString().Trim()));
                    txtApplBusiCashInHand.Text = Convert.ToString(dt3.Rows[0]["CashInHand"]);
                    txtApplBusiRef.Text = Convert.ToString(dt3.Rows[0]["BusiRef"]);
                    txtApplBusiAddress.Text = Convert.ToString(dt3.Rows[0]["Addr"]);
                    txtApplBusiMobNo.Text = Convert.ToString(dt3.Rows[0]["BusiMobileNo"]);
                    ddlApplBusiValidated.SelectedIndex = ddlApplBusiValidated.Items.IndexOf(ddlApplBusiValidated.Items.FindByValue(dt3.Rows[0]["ValidateYN"].ToString().Trim()));
                    ddlApplSecBusiYN.SelectedIndex = ddlApplSecBusiYN.Items.IndexOf(ddlApplSecBusiYN.Items.FindByValue(dt3.Rows[0]["SecondaryBusiYN"].ToString().Trim()));
                    //chkApplSecBusi
                    // ddlApplSecBusiYN.SelectedIndex = ddlApplSecBusiYN.Items.IndexOf(ddlApplSecBusiYN.Items.FindByValue(dt3.Rows[0]["NoOfSecBusi"].ToString().Trim()));

                    ddlApplSecBusiType1.SelectedIndex = ddlApplSecBusiType1.Items.IndexOf(ddlApplSecBusiType1.Items.FindByValue(dt3.Rows[0]["SecBusiType1"].ToString()));
                    ddlApplSecBusiSeas1.SelectedIndex = ddlApplSecBusiSeas1.Items.IndexOf(ddlApplSecBusiSeas1.Items.FindByValue(dt3.Rows[0]["SecBusiSeaso1"].ToString()));
                    popApplSecBusiSubType1(Convert.ToInt32(dt3.Rows[0]["SecBusiType1"]));
                    ddlApplSecBusiSubType1.SelectedIndex = ddlApplSecBusiSubType1.Items.IndexOf(ddlApplSecBusiSubType1.Items.FindByValue(dt3.Rows[0]["SecBusiSubType1"].ToString()));
                    PopApplSecBusiActivity1(Convert.ToInt32(dt3.Rows[0]["SecBusiSubType1"]));
                    ddlApplSecBusiAct1.SelectedIndex = ddlApplSecBusiAct1.Items.IndexOf(ddlApplSecBusiAct1.Items.FindByValue(dt3.Rows[0]["SecBusiActivity1"].ToString()));

                    ddlApplSecBusiType2.SelectedIndex = ddlApplSecBusiType2.Items.IndexOf(ddlApplSecBusiType2.Items.FindByValue(dt3.Rows[0]["SecBusiType2"].ToString()));
                    ddlApplSecBusiSeas2.SelectedIndex = ddlApplSecBusiSeas2.Items.IndexOf(ddlApplSecBusiSeas2.Items.FindByValue(dt3.Rows[0]["SecBusiSeaso2"].ToString()));
                    popApplSecBusiSubType2(Convert.ToInt32(dt3.Rows[0]["SecBusiType2"]));
                    ddlApplSecBusiSubType2.SelectedIndex = ddlApplSecBusiSubType2.Items.IndexOf(ddlApplSecBusiSubType2.Items.FindByValue(dt3.Rows[0]["SecBusiSubType2"].ToString()));
                    PopApplSecBusiActivity2(Convert.ToInt32(dt3.Rows[0]["SecBusiSubType2"]));
                    ddlApplSecBusiAct2.SelectedIndex = ddlApplSecBusiAct2.Items.IndexOf(ddlApplSecBusiAct2.Items.FindByValue(dt3.Rows[0]["SecBusiActivity2"].ToString()));


                    ddlCoApplBusiIncYN.SelectedIndex = ddlCoApplBusiIncYN.Items.IndexOf(ddlCoApplBusiIncYN.Items.FindByValue(dt4.Rows[0]["BusiIncYN"].ToString()));
                    ddlCoApplBusiName.SelectedIndex = ddlCoApplBusiName.Items.IndexOf(ddlCoApplBusiName.Items.FindByValue(dt4.Rows[0]["AppCoAppSameBusiYN"].ToString().Trim()));
                    txtCoApplBusiName.Text = Convert.ToString(dt4.Rows[0]["BusinessName"]);
                    ddlCoApplPrimaryBusiType.SelectedIndex = ddlCoApplPrimaryBusiType.Items.IndexOf(ddlCoApplPrimaryBusiType.Items.FindByValue(dt4.Rows[0]["PrimaryBusiType"].ToString()));
                    ddlCoApplPrimaryBusiSeas.SelectedIndex = ddlCoApplPrimaryBusiSeas.Items.IndexOf(ddlCoApplPrimaryBusiSeas.Items.FindByValue(dt4.Rows[0]["PrimaryBusiSeaso"].ToString()));
                    popCoApplSecBusiSubType(Convert.ToInt32(dt4.Rows[0]["PrimaryBusiType"]));
                    ddlCoApplPrimaryBusiSubType.SelectedIndex = ddlCoApplPrimaryBusiSubType.Items.IndexOf(ddlCoApplPrimaryBusiSubType.Items.FindByValue(dt4.Rows[0]["PrimaryBusiSubType"].ToString()));
                    PopCoApplPrimaryBusiActivity(Convert.ToInt32(dt4.Rows[0]["PrimaryBusiSubType"]));
                    ddlCoApplPrimaryBusiAct.SelectedIndex = ddlCoApplPrimaryBusiAct.Items.IndexOf(ddlCoApplPrimaryBusiAct.Items.FindByValue(dt4.Rows[0]["PrimaryBusiActivity"].ToString()));
                    ddlCoApplBusiMonthlyTrunover.SelectedIndex = ddlCoApplBusiMonthlyTrunover.Items.IndexOf(ddlCoApplBusiMonthlyTrunover.Items.FindByValue(dt4.Rows[0]["MonthlyTrunOver"].ToString()));
                    txtCoApplBusiAddr.Text = Convert.ToString(dt4.Rows[0]["BusinessAddr"]);
                    ddlCoApplBusiVintage.SelectedIndex = ddlCoApplBusiVintage.Items.IndexOf(ddlCoApplBusiVintage.Items.FindByValue(dt4.Rows[0]["BusinessVintage"].ToString().Trim()));
                    ddlCoApplBusiOwnerType.SelectedIndex = ddlCoApplBusiOwnerType.Items.IndexOf(ddlCoApplBusiOwnerType.Items.FindByValue(dt4.Rows[0]["BusiOwnerType"].ToString().Trim()));
                    ddlCoApplBusiStock.SelectedIndex = ddlCoApplBusiStock.Items.IndexOf(ddlCoApplBusiStock.Items.FindByValue(dt4.Rows[0]["ValueOfStock"].ToString().Trim()));

                    txtApplAge.Text = Convert.ToString(dt.Rows[0]["ApplAge"]);
                    txtApplDOB.Text = Convert.ToString(dt.Rows[0]["ApplDOB"]);
                    txtBankName.Text = Convert.ToString(dt.Rows[0]["BankName"]);
                    ddlCoApplPerAddrType.SelectedIndex = ddlCoApplPerAddrType.Items.IndexOf(ddlCoApplPerAddrType.Items.FindByValue(dt.Rows[0]["CoAppPerAddrType"].ToString().Trim()));
                    ddlCoApplPreAddrType.SelectedIndex = ddlCoApplPreAddrType.Items.IndexOf(ddlCoApplPreAddrType.Items.FindByValue(dt.Rows[0]["CoApplPreAddrType"].ToString().Trim()));
                    txtCoAppCommAddr.Text = Convert.ToString(dt.Rows[0]["CoApplPreAddr"]);
                    txtCoAppCommPin.Text = Convert.ToString(dt.Rows[0]["CoApplPrePIN"]);
                    ddlCoAppCommState.SelectedIndex = ddlCoAppCommState.Items.IndexOf(ddlCoAppCommState.Items.FindByValue(dt.Rows[0]["CoApplPreStateId"].ToString().Trim()));


                    CheckCheckBox(chkFamilyAsset, Convert.ToString(dt1.Rows[0]["FamilyAsset"]));
                    CheckCheckBox(chkOtherAsset, Convert.ToString(dt1.Rows[0]["OtherAsset"]));
                    CheckCheckBox(chkOtherSavings, Convert.ToString(dt1.Rows[0]["OtherSavings"]));
                    CheckCheckBox(chkApplOtherBusi, Convert.ToString(dt3.Rows[0]["OtherBusinessProof"]));

                    //-----------------------------------Identy Proof-------------------------------------------------
                    ddlAddPrf.SelectedIndex = ddlAddPrf.Items.IndexOf(ddlAddPrf.Items.FindByValue(dt.Rows[0]["ApplAddProfId"].ToString()));
                    ddlIdentyProf.SelectedIndex = ddlIdentyProf.Items.IndexOf(ddlIdentyProf.Items.FindByValue(dt.Rows[0]["ApplIdentyPRofId"].ToString()));
                    ddlCoAppID.SelectedIndex = ddlCoAppID.Items.IndexOf(ddlCoAppID.Items.FindByValue(dt.Rows[0]["CoApplIdentyPRofId"].ToString()));

                    if (Convert.ToInt32(Session[gblValue.RoleId]) != 1)
                    {
                        if (dt.Rows[0]["ApplIdentyPRofId"].ToString() == "1")
                        {
                            //hdnApplAadhar.Value = Convert.ToString(dt.Rows[0]["ApplIdentyProfNo"]);
                            txtIdntPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["ApplIdentyProfNo"]).Substring(Convert.ToString(dt.Rows[0]["ApplIdentyProfNo"]).Length - 4, 4));
                        }
                        else
                        {
                            txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["ApplIdentyProfNo"]);
                        }
                        if (dt.Rows[0]["ApplAddProfId"].ToString() == "1")
                        {
                            //hdnApplAadhar.Value = Convert.ToString(dt.Rows[0]["ApplAddProfNo"]);
                            txtAddPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["ApplAddProfNo"]).Substring(Convert.ToString(dt.Rows[0]["ApplAddProfNo"]).Length - 4, 4));
                        }
                        else
                        {
                            txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["ApplAddProfNo"]);
                        }
                        if (dt.Rows[0]["CoApplIdentyPRofId"].ToString() == "1")
                        {
                            //hdnCoApplAadhar.Value = Convert.ToString(dt.Rows[0]["CoApplIdentyProfNo"]);
                            txtCoAppID.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["CoApplIdentyProfNo"]).Substring(Convert.ToString(dt.Rows[0]["CoApplIdentyProfNo"]).Length - 4, 4));
                        }
                        else
                        {
                            txtCoAppID.Text = Convert.ToString(dt.Rows[0]["CoApplIdentyProfNo"]);
                        }
                    }
                    else
                    {
                        //if (dt.Rows[0]["ApplIdentyPRofId"].ToString() == "1")
                        //{
                        //    hdnApplAadhar.Value = Convert.ToString(dt.Rows[0]["ApplIdentyProfNo"]);
                        //}
                        //else if (dt.Rows[0]["ApplAddProfId"].ToString() == "1")
                        //{
                        //    hdnApplAadhar.Value = Convert.ToString(dt.Rows[0]["ApplAddProfNo"]);
                        //}
                        //if (dt.Rows[0]["CoApplIdentyPRofId"].ToString() == "1")
                        //{
                        //    hdnCoApplAadhar.Value = Convert.ToString(dt.Rows[0]["CoApplIdentyProfNo"]);
                        //}
                        txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["ApplIdentyProfNo"]);
                        txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["ApplAddProfNo"]);
                        txtCoAppID.Text = Convert.ToString(dt.Rows[0]["CoApplIdentyProfNo"]);
                    }
                    //----------------------------------------------------------------------------------------------


                    tbEmp.ActiveTabIndex = 1;
                    if (Session[gblValue.BrnchCode].ToString() == "0000")
                    {
                        StatusButton("View");
                    }
                    else
                    {
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                oPD = null;
            }
        }

        private void CheckCheckBox(CheckBoxList CBL, string Value)
        {
            string[] vValue = Value.Split(',');
            foreach (ListItem Lst in CBL.Items)
            {
                foreach (string item in vValue)
                {
                    if (Lst.Text == item)
                    {
                        Lst.Selected = true;
                    }
                }
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            CPdMst oPD = new CPdMst();
            int vErr = 0;
            Boolean vResult = false;
            int vPdId = Convert.ToInt32(ViewState["PdId"]);
            vErr = oPD.SavePdByBM(vPdId, ddlPreScrQ1.SelectedValue, ddlPreScrQ2.SelectedValue, ddlPreScrQ3.SelectedValue, ddlPreScrQ4.SelectedValue,
                ddlPreScrQ5.SelectedValue, ddlPreScrQ6.SelectedValue, ddlPreScrQ7.SelectedValue, ddlLoanReqDtlCorrect.SelectedValue,
                ddlApplDtlCorrect.SelectedValue, ddlCoApplDtlcorrect.SelectedValue, ddlOthrInfocorrect.SelectedValue,
                ddlProxyInfoCorrect.SelectedValue, ddlBankInfoCorrect.SelectedValue, ddlApplResiRelCorrect.SelectedValue,
                ddlApplBusInfoCorrect.SelectedValue, ddlCoApplBusInfoCorrect.SelectedValue, ddlBusinessPhotoCorrect.SelectedValue,
                gblFuction.setDate(Session[gblValue.LoginDate].ToString()), Convert.ToInt32(Session[gblValue.UserId]), Mode, chkApplPhotoVer.SelectedValue);
            if (vErr > 0)
            {
                if (fuBMSelfieApplAndBusi.HasFile)
                    SaveMemberImages(fuBMSelfieApplAndBusi, vPdId.ToString(), "BmWithAppBusiness", "Edit", "N", ImgPath + "PD/");
                if (fuBMSelfieApplRes.HasFile)
                    SaveMemberImages(fuBMSelfieApplRes, vPdId.ToString(), "BmWithAppResidence", "Edit", "N", ImgPath + "PD/");
                vResult = true;
            }
            return vResult;
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
            }
            finally
            {
                oNM = null;
                dt = null;
            }
        }

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

                ddlCoAppID.DataSource = dt;
                ddlCoAppID.DataTextField = "IDProofName";
                ddlCoAppID.DataValueField = "IDProofId";
                ddlCoAppID.DataBind();
                ddlCoAppID.Items.Insert(0, oli1);
            }
            finally
            {
                oNM = null;
                dt = null;
            }
        }

        #region SaveMemberImages
        private string SaveMemberImages(FileUpload flup, string imageGroup, string imageName, string Mode, string isImageSaved, string ImagePath)
        {
            if (MinioYN == "N")
            {
                try
                {
                    string folderPath = string.Format("{0}", ImagePath);
                    System.IO.Directory.CreateDirectory(folderPath);
                    string filePath = string.Format("{0}/{1}.png", folderPath, imageGroup + "_" + imageName);
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
                            ReduceImageSize(0.5, strm, targetFile);
                            isImageSaved = "Y";
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                CApiCalling oAC = new CApiCalling();
                byte[] ImgByte = ConvertFileToByteArray(flup.PostedFile);
                isImageSaved = oAC.UploadFileMinio(ResizeImage(ImgByte, 0.8), imageName + ".png", imageGroup, PdBucket, MinioUrl);
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
        #endregion

        #region ConvertFileToByteArray
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
        #endregion

        #region ResizeImage
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
        #endregion
    }
}
