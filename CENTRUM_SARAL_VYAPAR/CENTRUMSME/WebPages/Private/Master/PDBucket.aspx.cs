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
using Newtonsoft.Json;
using System.Xml;
using System.Net;

namespace CENTRUM_SARALVYAPAR.WebPages.Private.Master
{
    public partial class PDBucket : CENTRUMBAse
    {
        string ImgPath = ConfigurationManager.AppSettings["PathImage"];
        string vJocataToken = ConfigurationManager.AppSettings["JocataToken"];

        string InitialBucket = ConfigurationManager.AppSettings["InitialBucket"];
        string PdBucket = ConfigurationManager.AppSettings["PdBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBasePage();
                // PopPendingPdCust();
                popCaste();
                popStateList();
                PopQualification();
                PopRelation();
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    PopVillage();
                }
                PopPurpose();
                popBusinessType();
                popIdentityProof();
                popAddProof();
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

                StatusButton("View");
                txtDtFrm.Text = txtDtTo.Text = Convert.ToString(Session[gblValue.LoginDate]);
                tbEmp.ActiveTabIndex = 0;
                ViewState["PdId"] = "0";
                ViewState["EnqId"] = "";
                ViewState["MemberId"] = "";
                ViewState["PreApproveLnYN"] = "N";
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Personal Discussion(SO)";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuPD);
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
                    btnSave.Enabled = false;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnSendBack.Enabled = false;
                    ClearControls();
                    // gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_ddlLoanSector");
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
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

                    // gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_ddlLoanSector");
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
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        private void EnableControl(Boolean Status)
        {
            Int32 vRoleId = Convert.ToInt32(Session[gblValue.RoleId]);
            ddlMHFRelation.Enabled = Status;
            txtMHFName.Enabled = Status;
            ddlCustomer.Enabled = Status;
            ddlLnPurpose.Enabled = Status;
            ddlLoanPurpose.Enabled = Status;
            txtExpLnAmt.Enabled = Status;
            ddlExpTenure.Enabled = Status;
            txtEMIPayingCap.Enabled = Status;
            txtExistingLoan.Enabled = Status;
            txtTotalOutstanding.Enabled = Status;
            ddlTitle.Enabled = Status;
            txtApplAge.Enabled = Status;
            ddlApplGender.Enabled = Status;
            ddlApplMaritalStatus.Enabled = Status;
            ddlApplEdu.Enabled = Status;
            ddlApplReligion.Enabled = Status;
            ddlApplReligStat.Enabled = Status;
            ddlApplCast.Enabled = Status;

            ddlApplCommAddrType.Enabled = Status;
            txtApplCommHouseNo.Enabled = Status;
            txtApplCommSt.Enabled = Status;
            txtApplCommLandmark.Enabled = Status;
            txtApplCommArea.Enabled = Status;
            ddlApplCommVill.Enabled = Status;
            txtApplCommSubDist.Enabled = Status;
            txtApplCommPost.Enabled = Status;
            txtApplCommPin.Enabled = Status;
            //txtApplMobile.Enabled = Status;
            //txtApplContactNo.Enabled = Status;
            ddlApplPhyfit.Enabled = Status;

            ddlCoAppTitle.Enabled = Status;
            txtCoApplDOB.Enabled = Status;
            txtCoApplAge.Enabled = Status;
            ddlCoApplGen.Enabled = Status;
            ddlCoApplMaritalStat.Enabled = false;
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
            ddlSendBackReason.Enabled = Status;
            ddlMinorityYN.Enabled = false;
            ddlAbledYN.Enabled = Status;
            if (ddlAbledYN.SelectedValue != "Y")
            {
                ddlSpclAbled.Enabled = false;
            }
            else
            {
                ddlSpclAbled.Enabled = Status;
            }

            foreach (ListItem Lst in chkApplSecBusi.Items)
            {
                if (Lst.Selected)
                {
                    if (Lst.Value == "1")
                    {
                        ddlApplSecBusiType1.Enabled = true;
                        ddlApplSecBusiSeas1.Enabled = true;
                        ddlApplSecBusiSubType1.Enabled = true;
                        ddlApplSecBusiAct1.Enabled = true;
                    }
                    else
                    {
                        ddlApplSecBusiType1.Enabled = false;
                        ddlApplSecBusiSeas1.Enabled = false;
                        ddlApplSecBusiSubType1.Enabled = false;
                        ddlApplSecBusiAct1.Enabled = false;
                    }
                    if (Lst.Value == "2")
                    {
                        ddlApplSecBusiType2.Enabled = true;
                        ddlApplSecBusiSeas2.Enabled = true;
                        ddlApplSecBusiSubType2.Enabled = true;
                        ddlApplSecBusiAct2.Enabled = true;
                    }
                    else
                    {
                        ddlApplSecBusiType2.Enabled = false;
                        ddlApplSecBusiSeas2.Enabled = false;
                        ddlApplSecBusiSubType2.Enabled = false;
                        ddlApplSecBusiAct2.Enabled = false;
                    }
                }
                else
                {
                    ddlApplSecBusiType1.Enabled = false;
                    ddlApplSecBusiSeas1.Enabled = false;
                    ddlApplSecBusiSubType1.Enabled = false;
                    ddlApplSecBusiAct1.Enabled = false;
                    ddlApplSecBusiType2.Enabled = false;
                    ddlApplSecBusiSeas2.Enabled = false;
                    ddlApplSecBusiSubType2.Enabled = false;
                    ddlApplSecBusiAct2.Enabled = false;
                }
            }

            if (ddlCoApplBusiIncYN.SelectedValue == "N")
            {
                ddlCoApplBusiName.Enabled = false;
                txtCoApplBusiName.Enabled = false;
                ddlCoApplPrimaryBusiType.Enabled = false;
                ddlCoApplPrimaryBusiSeas.Enabled = false;
                ddlCoApplPrimaryBusiSubType.Enabled = false;
                ddlCoApplPrimaryBusiAct.Enabled = false;
                ddlCoApplBusiMonthlyTrunover.Enabled = false;
                txtCoApplBusiAddr.Enabled = false;
                ddlCoApplBusiVintage.Enabled = false;
                ddlCoApplBusiOwnerType.Enabled = false;
                ddlCoApplBusiStock.Enabled = false;
            }
            else
            {
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
            }

            txtCoApplAddr.Enabled = false;
            ddlCoAppCommState.Enabled = Status;
            txtCoAppCommPin.Enabled = Status;
            chkApplCommAddr.Enabled = Status;

            ddlCoApplPreAddrType.Enabled = Status;
            ddlCoApplPerAddrType.Enabled = Status;
            txtBankName.Enabled = Status;
            txtCoAppCommAddr.Enabled = Status;
            txtApplDOB.Enabled = Status;
            //------------- Applicant Address ----------------
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
            txtApplEmail.Enabled = Status;
            txtApplMobile.Enabled = Status;
            txtApplContactNo.Enabled = Status;
            //------------- ID proof --------------------------
            Boolean vStatus = Status;
            vStatus = Convert.ToInt32(Session[gblValue.RoleId]) == 1 ? Status : false;
            ddlIdentyProf.Enabled = (ddlIdentyProf.SelectedValue == "13") ? false : vStatus;
            ddlAddPrf.Enabled = (ddlAddPrf.SelectedValue == "13") ? false : vStatus;
            txtIdntPrfNo.Enabled = (ddlIdentyProf.SelectedValue == "13") ? false : vStatus;
            txtAddPrfNo.Enabled = (ddlAddPrf.SelectedValue == "13") ? false : vStatus;
            ddlCoAppID.Enabled = (ddlCoAppID.SelectedValue == "13") ? false : vStatus;
            txtCoAppID.Enabled = (ddlCoAppID.SelectedValue == "13") ? false : vStatus;

            vStatus = Convert.ToInt32(Session[gblValue.RoleId]) == 1 || Convert.ToInt32(Session[gblValue.RoleId]) == 25 ? Status : false;
            txtApplFName.Enabled = vStatus;
            txtApplMName.Enabled = vStatus;
            txtApplLName.Enabled = vStatus;
            txtCoApplName.Enabled = vStatus;

            vStatus = Convert.ToInt32(Session[gblValue.RoleId]) == 11 || Convert.ToInt32(Session[gblValue.RoleId]) == 57 ? Status : false;
            fuMemberPhoto.Enabled = vStatus;

            if (vRoleId != 1 && vRoleId != 11 && vRoleId != 25 && vRoleId != 57)
            {
                fuCoAppPhoto.Enabled = false;
            }
            else
            {
                fuCoAppPhoto.Enabled = true;
            }
        }

        private void ClearControls()
        {
            ddlMHFRelation.SelectedIndex = -1;
            txtMHFName.Text = "";
            ddlCustomer.SelectedIndex = -1;
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
            txtApplAge.Text = "";
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
            chkFamilyAsset.SelectedIndex = -1;
            chkOtherAsset.SelectedIndex = -1;
            ddlLandHolding.SelectedIndex = -1;
            ddlBankHabit.SelectedIndex = -1;
            chkOtherSavings.SelectedIndex = -1;
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
            chkApplOtherBusi.SelectedIndex = -1;
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

            txtCoApplAddr.Text = "";
            ddlCoAppCommState.SelectedIndex = -1;
            txtCoAppCommPin.Text = "";

            chkApplCommAddr.Checked = false;

            ddlCoApplPreAddrType.SelectedIndex = -1;
            ddlCoApplPerAddrType.SelectedIndex = -1;
            txtBankName.Text = "";
            txtCoAppCommAddr.Text = "";
            txtApplDOB.Text = "";

            ddlIdentyProf.SelectedIndex = -1;
            ddlAddPrf.SelectedIndex = -1;
            txtIdntPrfNo.Text = "";
            txtAddPrfNo.Text = "";
            txtCoAppID.Text = "";
            ddlCoAppID.SelectedIndex = -1;
            hdnApplAadhar.Value = "";
            hdnCoApplAadhar.Value = "";
            ddlMinorityYN.SelectedIndex = -1;
            ddlAbledYN.SelectedIndex = -1;
            ddlSpclAbled.SelectedIndex = -1;

            foreach (ListItem Lst in chkApplSecBusi.Items)
            {
                Lst.Selected = false;
                Lst.Enabled = false;
            }
            foreach (ListItem Lst in chkFamilyAsset.Items)
            {
                Lst.Selected = false;
            }
            foreach (ListItem Lst in chkOtherAsset.Items)
            {
                Lst.Selected = false;
            }
            foreach (ListItem Lst in chkOtherSavings.Items)
            {
                Lst.Selected = false;
            }
            foreach (ListItem Lst in chkApplOtherBusi.Items)
            {
                Lst.Selected = false;
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
            PopPendingPdCust();
            ViewState["StateEdit"] = "Add";
            tbEmp.ActiveTabIndex = 1;
            StatusButton("Add");
            ClearControls();
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

        protected void ddlApplReligStat_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pRMId = Convert.ToInt32(ddlApplReligStat.SelectedValue);
            PopReligion(pRMId);
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

                foreach (DataRow dR in dt.Rows)
                {
                    int vRelationId = Convert.ToInt32(dR["RelationId"]);
                    if (vRelationId != 2 && vRelationId != 3 && vRelationId != 8)
                    {
                        dR.Delete();
                    }
                }
                dt.AcceptChanges();

                ddlMHFRelation.DataSource = dt;
                ddlMHFRelation.DataTextField = "Relation";
                ddlMHFRelation.DataValueField = "RelationId";
                ddlMHFRelation.DataBind();
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ddlMHFRelation.Items.Insert(0, oli2);

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

        private void PopVillageByBranchCode(string vBrCode)
        {
            DataTable dt = null;
            CVillage oGb = null;
            try
            {
                ddlApplCommVill.Items.Clear();
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

        protected void ddlApplCommVill_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vVlgId = ddlApplCommVill.SelectedValue;
            PopDistrictStatebyVillageId(vVlgId);
            SetMinority();
        }

        protected void ddlApplReligion_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMinority();
        }

        private void SetMinority()
        {
            Int32 vCommState = Convert.ToInt32(ddlApplCommState.SelectedValue);
            Int32 vApplReligion = Convert.ToInt32(ddlApplReligion.SelectedValue);
            if ((vCommState == 11 && vApplReligion == 3) || (vCommState == 18 && vApplReligion == 5) || (vCommState == 19 && vApplReligion == 5)
            || (vCommState == 20 && vApplReligion == 5) || (vCommState == 22 && vApplReligion == 4) || (vCommState == 35 && vApplReligion == 3))
            {
                ddlMinorityYN.SelectedIndex = ddlMinorityYN.Items.IndexOf(ddlMinorityYN.Items.FindByValue("N"));
            }
            else if (vApplReligion == 1 || vApplReligion == 9)
            {
                ddlMinorityYN.SelectedIndex = ddlMinorityYN.Items.IndexOf(ddlMinorityYN.Items.FindByValue("N"));
            }
            else
            {
                ddlMinorityYN.SelectedIndex = ddlMinorityYN.Items.IndexOf(ddlMinorityYN.Items.FindByValue("Y"));
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

        protected void ddlLoanPurpose_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopSubPurpose(Convert.ToInt32(ddlLoanPurpose.SelectedValue));
        }

        protected void ddlApplPrimaryBusiType_SelectedIndexChanged(object sender, EventArgs e)
        {
            popApplBusiSubType(Convert.ToInt32(ddlApplPrimaryBusiType.SelectedValue));
        }

        protected void ddlApplSecBusiType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            popApplSecBusiSubType1(Convert.ToInt32(ddlApplSecBusiType1.SelectedValue));
        }

        protected void ddlApplSecBusiType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            popApplSecBusiSubType2(Convert.ToInt32(ddlApplSecBusiType2.SelectedValue));
        }

        protected void ddlCoApplPrimaryBusiType_SelectedIndexChanged(object sender, EventArgs e)
        {
            popCoApplSecBusiSubType(Convert.ToInt32(ddlCoApplPrimaryBusiType.SelectedValue));
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

        protected void ddlApplPrimaryBusiSubType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopApplBusiActivity(Convert.ToInt32(ddlApplPrimaryBusiSubType.SelectedValue));
        }

        protected void ddlApplSecBusiSubType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopApplSecBusiActivity1(Convert.ToInt32(ddlApplSecBusiSubType1.SelectedValue));
        }

        protected void ddlApplSecBusiSubType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopApplSecBusiActivity2(Convert.ToInt32(ddlApplSecBusiSubType2.SelectedValue));
        }

        protected void ddlCoApplPrimaryBusiSubType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopCoApplPrimaryBusiActivity(Convert.ToInt32(ddlCoApplPrimaryBusiSubType.SelectedValue));
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillMemberDtl(ddlCustomer.SelectedValue);                       
        }

        private void PopPendingPdCust()
        {
            DataTable dt = null;
            CPdMst oPD = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vBranchCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oPD = new CPdMst();
                dt = oPD.GetPendingPDMember("", vBranchCode, vLogDt);
                ddlCustomer.DataSource = dt;
                ddlCustomer.DataTextField = "MemberName";
                ddlCustomer.DataValueField = "EnquiryId";
                ddlCustomer.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlCustomer.Items.Insert(0, oli1);
            }
            finally
            {
                oPD = null;
                dt = null;
            }
        }

        private void FillMemberDtl(string vEnqId)
        {
            DataTable dt = null;
            DataTable dt1 = null;
            CPdMst oPD = null;
            try
            {
                ViewState["EnqId"] = vEnqId;
                oPD = new CPdMst();
                dt = new DataTable();
                dt1 = new DataTable();
                dt = oPD.GetMemberByEnqId(vEnqId, Convert.ToInt32(Session[gblValue.UserId]));

                if (dt.Rows.Count > 0)
                {
                    ViewState["AadhaarScan"] = dt.Rows[0]["AadhaarScan"].ToString();
                    ViewState["PreApproveLnYN"] = dt.Rows[0]["PreApproveLnYN"].ToString();

                    hdnAmtEligible.Value = dt.Rows[0]["AmountEligible"].ToString();
                    hdnMinLoanAmt.Value = dt.Rows[0]["MinLoanAmt"].ToString();
                    hdnMaxLoanAmt.Value = dt.Rows[0]["MaxLoanAmt"].ToString();

                    txtApplFName.Text = Convert.ToString(dt.Rows[0]["MF_Name"]);
                    txtApplMName.Text = Convert.ToString(dt.Rows[0]["MM_Name"]);
                    txtApplLName.Text = Convert.ToString(dt.Rows[0]["ML_Name"]);
                    txtApplAge.Text = Convert.ToString(dt.Rows[0]["Age"]);
                    txtApplDOB.Text = Convert.ToString(dt.Rows[0]["DOB"]);

                    txtApplHouNo.Text = Convert.ToString(dt.Rows[0]["HouseNo"]);
                    txtApplStrName.Text = Convert.ToString(dt.Rows[0]["Street"]);
                    txtApplWardNo.Text = Convert.ToString(dt.Rows[0]["WardNo"]);
                    txtApplPOff.Text = Convert.ToString(dt.Rows[0]["PostOff"]);
                    txtApplLandMark.Text = Convert.ToString(dt.Rows[0]["Landmark"]);
                    txtApplArea.Text = Convert.ToString(dt.Rows[0]["Area"]);
                    txtApplPin.Text = Convert.ToString(dt.Rows[0]["PIN"]);
                    txtApplDist.Text = Convert.ToString(dt.Rows[0]["District"]);
                    txtApplVillg.Text = Convert.ToString(dt.Rows[0]["Village"]);
                    txtApplMobile.Text = Convert.ToString(dt.Rows[0]["MobileNo"]);
                    ddlApplStat.SelectedIndex = ddlApplStat.Items.IndexOf(ddlApplStat.Items.FindByText(dt.Rows[0]["State"].ToString()));
                    ddlApplAddrType.SelectedIndex = ddlApplAddrType.Items.IndexOf(ddlApplAddrType.Items.FindByValue(dt.Rows[0]["AddrType"].ToString()));

                    ddlMHFRelation.SelectedIndex = ddlMHFRelation.Items.IndexOf(ddlMHFRelation.Items.FindByValue(dt.Rows[0]["HumanRelationId"].ToString()));
                    txtMHFName.Text = Convert.ToString(dt.Rows[0]["FamilyPersonName"]);

                    ddlAbledYN.SelectedIndex = ddlAbledYN.Items.IndexOf(ddlAbledYN.Items.FindByValue(dt.Rows[0]["IsAbledYN"].ToString()));
                    ddlSpclAbled.SelectedIndex = ddlSpclAbled.Items.IndexOf(ddlSpclAbled.Items.FindByValue(dt.Rows[0]["SpeciallyAbled"].ToString()));

                    ddlApplCommAddrType.SelectedIndex = ddlApplCommAddrType.Items.IndexOf(ddlApplCommAddrType.Items.FindByValue(dt.Rows[0]["AddrType_p"].ToString()));
                    txtApplCommHouseNo.Text = Convert.ToString(dt.Rows[0]["HouseNo_p"]);
                    txtApplCommSt.Text = Convert.ToString(dt.Rows[0]["Street_p"]);
                    ddlApplCommVill.SelectedIndex = ddlApplCommVill.Items.IndexOf(ddlApplCommVill.Items.FindByValue(dt.Rows[0]["VillageId_p"].ToString()));
                    txtApplCommSubDist.Text = Convert.ToString(dt.Rows[0]["WardNo_p"]);
                    txtApplCommPost.Text = Convert.ToString(dt.Rows[0]["PostOff_p"]);
                    txtApplCommPin.Text = Convert.ToString(dt.Rows[0]["PIN_p"]);
                    txtApplCommLandmark.Text = Convert.ToString(dt.Rows[0]["Landmark_p"]);
                    txtApplCommArea.Text = Convert.ToString(dt.Rows[0]["Area_p"]);
                    PopDistrictStatebyVillageId(dt.Rows[0]["VillageId_p"].ToString());


                    txtCoApplName.Text = Convert.ToString(dt.Rows[0]["CoAppName"]);
                    txtCoApplDOB.Text = Convert.ToString(dt.Rows[0]["CoAppDOB"]);
                    ddlCoApplRel.SelectedIndex = ddlCoApplRel.Items.IndexOf(ddlCoApplRel.Items.FindByValue(dt.Rows[0]["CoAppRelationId"].ToString()));

                    txtCoApplAddr.Text = Convert.ToString(dt.Rows[0]["CoAppAddress"]);
                    ddlCoApplState.SelectedIndex = ddlCoApplState.Items.IndexOf(ddlCoApplState.Items.FindByValue(dt.Rows[0]["CoAppState"].ToString()));
                    txtCoApplPin.Text = Convert.ToString(dt.Rows[0]["CoAppPinCode"]);
                    txtCoApplMobile.Text = Convert.ToString(dt.Rows[0]["CoAppMobileNo"]);
                    //-----------------------------------Identy Proof-------------------------------------------------
                    ddlAddPrf.SelectedIndex = ddlAddPrf.Items.IndexOf(ddlAddPrf.Items.FindByValue(dt.Rows[0]["AddProfId"].ToString()));
                    ddlIdentyProf.SelectedIndex = ddlIdentyProf.Items.IndexOf(ddlIdentyProf.Items.FindByValue(dt.Rows[0]["IdentyPRofId"].ToString()));
                    ddlCoAppID.SelectedIndex = ddlCoAppID.Items.IndexOf(ddlCoAppID.Items.FindByValue(dt.Rows[0]["CoAppIdentyPRofId"].ToString()));

                    if (Convert.ToInt32(Session[gblValue.RoleId]) != 1)
                    {
                        if (dt.Rows[0]["IdentyPRofId"].ToString() == "1")
                        {
                            hdnApplAadhar.Value = Convert.ToString(dt.Rows[0]["IdentyProfNo"]);
                            txtIdntPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["IdentyProfNo"]).Substring(Convert.ToString(dt.Rows[0]["IdentyProfNo"]).Length - 4, 4));
                        }
                        else
                        {
                            txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["IdentyProfNo"]);
                        }
                        if (dt.Rows[0]["AddProfId"].ToString() == "1")
                        {
                            hdnApplAadhar.Value = Convert.ToString(dt.Rows[0]["AddProfNo"]);
                            txtAddPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["AddProfNo"]).Substring(Convert.ToString(dt.Rows[0]["AddProfNo"]).Length - 4, 4));
                        }
                        else
                        {
                            txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["AddProfNo"]);
                        }
                        if (dt.Rows[0]["CoAppIdentyPRofId"].ToString() == "1")
                        {
                            hdnCoApplAadhar.Value = Convert.ToString(dt.Rows[0]["CoAppIdentyProfNo"]);
                            txtCoAppID.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["CoAppIdentyProfNo"]).Substring(Convert.ToString(dt.Rows[0]["CoAppIdentyProfNo"]).Length - 4, 4));
                        }
                        else
                        {
                            txtCoAppID.Text = Convert.ToString(dt.Rows[0]["CoAppIdentyProfNo"]);
                        }
                    }
                    else
                    {
                        if (dt.Rows[0]["IdentyPRofId"].ToString() == "1")
                        {
                            hdnApplAadhar.Value = Convert.ToString(dt.Rows[0]["IdentyProfNo"]);
                        }
                        else if (dt.Rows[0]["AddProfId"].ToString() == "1")
                        {
                            hdnApplAadhar.Value = Convert.ToString(dt.Rows[0]["AddProfNo"]);
                        }
                        if (dt.Rows[0]["CoAppIdentyPRofId"].ToString() == "1")
                        {
                            hdnCoApplAadhar.Value = Convert.ToString(dt.Rows[0]["CoAppIdentyProfNo"]);
                        }
                        txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["IdentyProfNo"]);
                        txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["AddProfNo"]);
                        txtCoAppID.Text = Convert.ToString(dt.Rows[0]["CoAppIdentyProfNo"]);
                    }
                    //----------------------------------------------------------------------------
                    if (Convert.ToString(ViewState["PreApproveLnYN"]) == "Y")
                    {
                        ddlTypeOfOwnership.SelectedIndex = 1;
                        ddlTypeOfResidence.SelectedIndex = 2;
                        ddlApplBusiNameBoard.SelectedIndex = 1;
                        ddlApplPrimaryBusiSeaso.SelectedIndex = 6;
                        ddlApplNoOfWorkDay.SelectedIndex = 5;
                        ddlApplMonthTrun.SelectedIndex = 5;
                        ddlApplBusiVintage.SelectedIndex = 2;
                        ddlApplBusiOwnerType.SelectedIndex = 1;
                    } 
                    //----------------------------------------------------------------------------
                    tbEmp.ActiveTabIndex = 1;
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

        private void EnableDisableApplSecBusiness(string Value)
        {
            if (Value.Contains("-1") == false)
            {
                if (Value.Contains("1"))
                {
                    ddlApplSecBusiType1.Enabled = true;
                    ddlApplSecBusiSeas1.Enabled = true;
                    ddlApplSecBusiSubType1.Enabled = true;
                    ddlApplSecBusiAct1.Enabled = true;
                }
                else
                {
                    ddlApplSecBusiType1.Enabled = false;
                    ddlApplSecBusiSeas1.Enabled = false;
                    ddlApplSecBusiSubType1.Enabled = false;
                    ddlApplSecBusiAct1.Enabled = false;
                }
                if (Value.Contains("2"))
                {
                    ddlApplSecBusiType2.Enabled = true;
                    ddlApplSecBusiSeas2.Enabled = true;
                    ddlApplSecBusiSubType2.Enabled = true;
                    ddlApplSecBusiAct2.Enabled = true;
                }
                else
                {
                    ddlApplSecBusiType2.Enabled = false;
                    ddlApplSecBusiSeas2.Enabled = false;
                    ddlApplSecBusiSubType2.Enabled = false;
                    ddlApplSecBusiAct2.Enabled = false;
                }
            }
            else
            {
                ddlApplSecBusiType1.Enabled = false;
                ddlApplSecBusiSeas1.Enabled = false;
                ddlApplSecBusiSubType1.Enabled = false;
                ddlApplSecBusiAct1.Enabled = false;
                ddlApplSecBusiType2.Enabled = false;
                ddlApplSecBusiSeas2.Enabled = false;
                ddlApplSecBusiSubType2.Enabled = false;
                ddlApplSecBusiAct2.Enabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            CPdMst oPD = new CPdMst();
            int vErr = 0;
            Boolean vResult = false;
            int vPdId = Convert.ToInt32(ViewState["PdId"]);
            string vBrCode = Convert.ToString(Session[gblValue.BrnchCode]);
            string vEnqId = Convert.ToString(ViewState["EnqId"]);
            string vMemberId = "", vErrDesc = "";

            vErr = oPD.SavePdBySO(ref vPdId, ddlCustomer.SelectedValue, ref vMemberId,
                Convert.ToInt32(ddlLnPurpose.SelectedValue), Convert.ToDouble(txtExpLnAmt.Text),
                Convert.ToInt32(ddlExpTenure.SelectedValue), Convert.ToInt32(txtEMIPayingCap.Text), Convert.ToInt32(txtExistingLoan.Text),
                Convert.ToDouble(txtTotalOutstanding.Text),

                ddlTitle.SelectedValue, txtApplFName.Text, txtApplMName.Text, txtApplLName.Text,
                ddlApplGender.SelectedValue, ddlApplMaritalStatus.SelectedValue, Convert.ToInt32(ddlApplEdu.SelectedValue),
                Convert.ToInt32(ddlApplReligStat.SelectedValue), Convert.ToInt32(ddlApplReligion.SelectedValue), Convert.ToInt32(ddlApplCast.SelectedValue),

                Convert.ToInt32(ddlApplAddrType.SelectedValue), txtApplHouNo.Text, txtApplStrName.Text, txtApplLandMark.Text, txtApplArea.Text,
                txtApplVillg.Text, txtApplWardNo.Text, txtApplPOff.Text, txtApplPin.Text, txtApplDist.Text, Convert.ToInt32(ddlApplStat.SelectedValue),
                txtApplEmail.Text,

                Convert.ToInt32(ddlApplCommAddrType.SelectedValue), txtApplCommHouseNo.Text, txtApplCommSt.Text, txtApplCommLandmark.Text, txtApplCommArea.Text,
                Convert.ToInt32(ddlApplCommVill.SelectedValue), txtApplCommSubDist.Text, txtApplCommPost.Text, txtApplCommPin.Text, ddlApplCommDist.SelectedValue,
                Convert.ToInt32(ddlApplCommState.SelectedValue), txtApplMobile.Text, txtApplContactNo.Text, Convert.ToInt32(ddlApplPhyfit.SelectedValue),

                ddlCoAppTitle.SelectedValue, txtCoApplName.Text, gblFuction.setDate(txtCoApplDOB.Text), Convert.ToInt32(txtCoApplAge.Text), ddlCoApplGen.SelectedValue,
                ddlCoApplMaritalStat.SelectedValue, Convert.ToInt32(ddlCoApplRel.SelectedValue), Convert.ToInt32(ddlCoApplEdu.SelectedValue), txtCoApplAddr.Text,
                Convert.ToInt32(ddlCoApplState.SelectedValue), txtCoApplPin.Text, txtCoApplMobile.Text, txtCoApplContactNo.Text,
                Convert.ToInt32(ddlCoApplPhyFitness.SelectedValue), txtACHolderName.Text, Convert.ToString(txtMHFName.Text), Convert.ToInt32(ddlMHFRelation.SelectedValue), txtACNo.Text, txtIFSC.Text, ddlAcType.SelectedValue,

                ddlCpAppOthrIncm.SelectedValue, Convert.ToInt32(ddlCoApplTypeOfInc.SelectedValue), Convert.ToInt32(ddlAgeIncEar.SelectedValue),
                Convert.ToInt32(ddlAnnulInc.SelectedValue), Convert.ToInt32(ddlHouseStab.SelectedValue), Convert.ToInt32(ddlTypeOfOwnership.SelectedValue),
                Convert.ToInt32(ddlTypeOfResidence.SelectedValue), ddlResidenceCategory.SelectedValue, Convert.ToInt32(ddlTotalNoOfFamily.SelectedValue),
                Convert.ToInt32(ddlNoOfChild.SelectedValue), Convert.ToInt32(ddlNoOfDependent.SelectedValue), Convert.ToInt32(ddlNoOfFamilyEM.SelectedValue),
                chkFamilyAsset.SelectedValue, chkOtherAsset.SelectedValue, Convert.ToInt32(ddlLandHolding.SelectedValue), Convert.ToInt32(ddlBankHabit.SelectedValue),
                chkOtherSavings.SelectedValue, txtPersonalRef.Text, txtOtherInfoAddr.Text, txtOtherInfoMob.Text, ddlOtherInfoValidate.SelectedValue,

                ddlProxyMob.SelectedValue,
                ddlProxyRef.SelectedValue, ddlProxy2Wheeler.SelectedValue, ddlProxy3Wheeler.SelectedValue, ddlProxy4Wheeler.SelectedValue, ddlProxyAC.SelectedValue,
                ddlProxyWashingMachine.SelectedValue, ddlProxyEmailId.SelectedValue, ddlProxyPAN.SelectedValue, ddlProxyGSTNo.SelectedValue, ddlProxyITR.SelectedValue,
                ddlProxyWhatsApp.SelectedValue, ddlProxyFB.SelectedValue,

                txtApplBusiName.Text, ddlApplBusiNameBoard.SelectedValue, Convert.ToInt32(ddlApplPrimaryBusiType.SelectedValue)
                , ddlApplPrimaryBusiSeaso.SelectedValue, Convert.ToInt32(ddlApplPrimaryBusiSubType.SelectedValue == "" ? "-1" : ddlApplPrimaryBusiSubType.SelectedValue),
                Convert.ToInt32(ddlApplPrimaryBusiActivity.SelectedValue == "" ? "-1" : ddlApplPrimaryBusiActivity.SelectedValue),
                Convert.ToInt32(ddlApplNoOfWorkDay.SelectedValue), Convert.ToInt32(ddlApplMonthTrun.SelectedValue), Convert.ToInt32(ddlApplLocalityArea.SelectedValue),
                gblFuction.setDate(txtBusiEstDt.Text), txtApplBusiAddr.Text, chkApplOtherBusi.SelectedValue, Convert.ToInt32(ddlApplBusiVintage.SelectedValue),
                ddlApplBusiOwnerType.SelectedValue, ddlApplBusiHndlPerson.SelectedValue, ddlApplBusiPart.SelectedValue, Convert.ToInt32(ddlApplBusiNoOfEmp.SelectedValue),
                Convert.ToInt32(ddlApplBusiValueOfStock.SelectedValue), Convert.ToInt32(ddlApplBusiValueOfMachinery.SelectedValue), ddlApplBusiHours.SelectedValue,
                ddlApplBusiAppName.SelectedValue, txtApplBusiVPAId.Text, ddlApplBusiTranProofType.SelectedValue, Convert.ToDouble(txtApplBusiCashInHand.Text),
                txtApplBusiRef.Text, txtApplBusiAddress.Text, txtApplBusiMobNo.Text, ddlApplBusiValidated.SelectedValue, ddlApplSecBusiYN.SelectedValue,
                Convert.ToInt32(chkApplSecBusi.SelectedValue == "" ? "-1" : chkApplSecBusi.SelectedValue), Convert.ToInt32(ddlApplSecBusiType1.SelectedValue == "" ? "-1" : ddlApplSecBusiType1.SelectedValue),
                ddlApplSecBusiSeas1.SelectedValue, Convert.ToInt32(ddlApplSecBusiSubType1.SelectedValue == "" ? "-1" : ddlApplSecBusiSubType1.SelectedValue),
                Convert.ToInt32(ddlApplSecBusiAct1.SelectedValue == "" ? "-1" : ddlApplSecBusiAct1.SelectedValue),
                Convert.ToInt32(ddlApplSecBusiType2.SelectedValue == "" ? "-1" : ddlApplSecBusiType2.SelectedValue), ddlApplSecBusiSeas2.SelectedValue,
                Convert.ToInt32(ddlApplSecBusiSubType2.SelectedValue == "" ? "-1" : ddlApplSecBusiSubType2.SelectedValue),
                Convert.ToInt32(ddlApplSecBusiAct2.SelectedValue == "" ? "-1" : ddlApplSecBusiAct2.SelectedValue),
                ddlCoApplBusiIncYN.SelectedValue, ddlCoApplBusiName.SelectedValue, txtCoApplBusiName.Text,
                Convert.ToInt32(ddlCoApplPrimaryBusiType.SelectedValue == "" ? "-1" : ddlCoApplPrimaryBusiType.SelectedValue),
                ddlCoApplPrimaryBusiSeas.SelectedValue, Convert.ToInt32(ddlCoApplPrimaryBusiSubType.SelectedValue == "" ? "-1" : ddlCoApplPrimaryBusiSubType.SelectedValue),
                Convert.ToInt32(ddlCoApplPrimaryBusiAct.SelectedValue == "" ? "-1" : ddlCoApplPrimaryBusiAct.SelectedValue)
                , Convert.ToInt32(ddlCoApplBusiMonthlyTrunover.SelectedValue == "" ? "-1" : ddlCoApplBusiMonthlyTrunover.SelectedValue),
                txtCoApplBusiAddr.Text, "", Convert.ToInt32(ddlCoApplBusiVintage.SelectedValue == "" ? "-1" : ddlCoApplBusiVintage.SelectedValue),
                ddlCoApplBusiOwnerType.SelectedValue, Convert.ToInt32(ddlCoApplBusiStock.SelectedValue == "" ? "-1" : ddlCoApplBusiStock.SelectedValue),
                Convert.ToInt32(Session[gblValue.UserId]), "I", Mode, gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode,
                txtBankName.Text, Convert.ToInt32(ddlCoApplPerAddrType.SelectedValue), Convert.ToInt32(ddlCoApplPreAddrType.SelectedValue),
                txtCoAppCommAddr.Text, Convert.ToInt32(ddlCoAppCommState.SelectedValue), txtCoAppCommPin.Text, gblFuction.setDate(txtApplDOB.Text),
                Convert.ToInt32(txtApplAge.Text), Convert.ToInt32(ddlAddPrf.SelectedValue), ddlAddPrf.SelectedValue == "1" ? hdnApplAadhar.Value : txtAddPrfNo.Text, Convert.ToInt32(ddlIdentyProf.SelectedValue),
                ddlIdentyProf.SelectedValue == "1" ? hdnApplAadhar.Value : txtIdntPrfNo.Text, Convert.ToInt32(ddlCoAppID.SelectedValue), ddlCoAppID.SelectedValue == "1" ? hdnCoApplAadhar.Value :
                txtCoAppID.Text, ddlMinorityYN.SelectedValue, Convert.ToString(ddlAbledYN.SelectedValue), Convert.ToInt32(ddlSpclAbled.SelectedValue), ref vErrDesc);
            if (vErr > 0)
            {
                if (fuMemberPhoto.HasFile)
                    SaveMemberImages(fuMemberPhoto, vEnqId, "MemberPhoto", "Edit", "N", ImgPath + "InitialApproach/", "I");
                if (fuCoAppPhoto.HasFile)
                    SaveMemberImages(fuCoAppPhoto, vEnqId, "CoAppPhoto", "Edit", "N", ImgPath + "InitialApproach/", "I");
                if (fuApplID1F.HasFile)
                    SaveMemberImages(fuApplID1F, vEnqId, "IDProofImage", "Edit", "N", ImgPath + "InitialApproach/", "I");
                if (fuApplID1B.HasFile)
                    SaveMemberImages(fuApplID1B, vEnqId, "IDProofImageBack", "Edit", "N", ImgPath + "InitialApproach/", "I");
                if (fuApplID2F.HasFile)
                    SaveMemberImages(fuApplID2F, vEnqId, "AddressProofImage", "Edit", "N", ImgPath + "InitialApproach/", "I");
                if (fuApplID2B.HasFile)
                    SaveMemberImages(fuApplID2B, vEnqId, "AddressProofImageBack", "Edit", "N", ImgPath + "InitialApproach/", "I");
                if (fuCoApplID1F.HasFile)
                    SaveMemberImages(fuCoApplID1F, vEnqId, "AddressProofImage2", "Edit", "N", ImgPath + "InitialApproach/", "I");
                if (fuCoApplID1B.HasFile)
                    SaveMemberImages(fuCoApplID1B, vEnqId, "AddressProofImage2Back", "Edit", "N", ImgPath + "InitialApproach/", "I");
                //Save Image to PD bucket
                if (fuApplPhoto.HasFile)
                    SaveMemberImages(fuApplPhoto, vPdId.ToString(), "AppHouseImage", "Edit", "N", ImgPath + "PD/", "P");
                if (fuBankPassbook.HasFile)
                    SaveMemberImages(fuBankPassbook, vPdId.ToString(), "PassbookImage", "Edit", "N", ImgPath + "PD/", "P");
                if (fuOwnershipProof.HasFile)
                    SaveMemberImages(fuOwnershipProof, vPdId.ToString(), "AppOwnerShipProof", "Edit", "N", ImgPath + "PD/", "P");
                if (fuBusiImg.HasFile)
                    SaveMemberImages(fuBusiImg, vPdId.ToString(), "AppBusiness1Image", "Edit", "N", ImgPath + "PD/", "P");
                if (fuBusiIdProof.HasFile)
                    SaveMemberImages(fuBusiIdProof, vPdId.ToString(), "AppBusinessIdImage", "Edit", "N", ImgPath + "PD/", "P");
                if (fuLightBill.HasFile)
                    SaveMemberImages(fuLightBill, vPdId.ToString(), "appLightbillImage", "Edit", "N", ImgPath + "PD/", "P");
                if (fuBusiTranProof.HasFile)
                    SaveMemberImages(fuBusiTranProof, vPdId.ToString(), "AppBusiness2Image", "Edit", "N", ImgPath + "PD/", "P");
                if (fuCoAppBusiInc.HasFile)
                    SaveMemberImages(fuCoAppBusiInc, vPdId.ToString(), "CoAppBusiness1Image", "Edit", "N", ImgPath + "PD/", "P");
                //--------------------Jocata Calling---------------------------
                if (Mode == "Save")
                {
                    JocataRequest(vMemberId, vPdId, Convert.ToInt32(Session[gblValue.UserId]));
                }
                //-------------------------------------------------------------
                vResult = true;
            }
            else
            {
                gblFuction.MsgPopup(vErrDesc);
                vResult = false;
            }
            return vResult;

        }

        private void LoadGrid()
        {
            DataTable dt = new DataTable();
            CPdMst oMem = new CPdMst();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            dt = oMem.GetPdMember(gblFuction.setDate(txtDtFrm.Text), gblFuction.setDate(txtDtTo.Text), vBrCode, txtSearch.Text, Convert.ToInt32(Session[gblValue.BCProductId]));
            if (dt.Rows.Count > 0)
            {
                gvPDBySo.DataSource = dt;
                gvPDBySo.DataBind();
            }

        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void btnSendBack_Click(object sender, EventArgs e)
        {
            string vEnqId = "", vRejectReason = ddlSendBackReason.SelectedValue;
            int vCreatedBy = Convert.ToInt32(Session[gblValue.UserId]), vErr = 0;
            CPdMst oPD = new CPdMst();

            if (ddlCustomer.SelectedValue == "" || ddlCustomer.SelectedValue == "-1")
            {
                gblFuction.MsgPopup("Customer Can not left Blank.");
                return;
            }
            else
            {
                vEnqId = ddlCustomer.SelectedValue;
            }

            vErr = oPD.RejectPD(vEnqId, vRejectReason, vCreatedBy, "PdBySo");
            if (vErr > 0)
            {
                gblFuction.MsgPopup("Data Rejected Successfully.");
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }

        }

        protected void gvPDBySo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vPdId = 0;
            vPdId = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                foreach (GridViewRow gr in gvPDBySo.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
            }
            ClearControls();
            FillPdDtl(vPdId);            
        }

        private void FillPdDtl(Int32 vPdId)
        {
            DataSet ds = null;
            DataTable dt, dt1, dt2, dt3, dt4 = null;
            CPdMst oPD = null;
            ViewState["PdId"] = vPdId;
            try
            {
                ds = new DataSet();
                dt = new DataTable();
                dt1 = new DataTable();
                dt2 = new DataTable();
                dt3 = new DataTable();
                dt4 = new DataTable();
                oPD = new CPdMst();
                ds = oPD.GetPdDtlByPdId(vPdId, Convert.ToInt32(Session[gblValue.UserId]), "");
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                dt2 = ds.Tables[2];
                dt3 = ds.Tables[3];
                dt4 = ds.Tables[4];

                if (dt.Rows.Count > 0)
                {
                    Session["EnquiryDate"] = dt.Rows[0]["EnquiryDate"];
                    ViewState["PreApproveLnYN"] = dt.Rows[0]["PreApproveLnYN"];
                    ddlCustomer.Items.Clear();
                    PopVillageByBranchCode(Convert.ToString(dt.Rows[0]["BranchCode"]));
                    string vMemberName = dt.Rows[0]["EnquiryId"].ToString() + '-' + Convert.ToString(dt.Rows[0]["ApplName"]);
                    ListItem oli1 = new ListItem(vMemberName, dt.Rows[0]["EnquiryId"].ToString());
                    ddlCustomer.Items.Insert(0, oli1);
                    ViewState["EnqId"] = dt.Rows[0]["EnquiryId"].ToString();
                    ViewState["MemberId"] = dt.Rows[0]["MemberId"].ToString();

                    hdnAmtEligible.Value = dt.Rows[0]["AmountEligible"].ToString();
                    hdnMinLoanAmt.Value = dt.Rows[0]["MinLoanAmt"].ToString();
                    hdnMaxLoanAmt.Value = dt.Rows[0]["MaxLoanAmt"].ToString();
                    hdnSLYN.Value = dt.Rows[0]["SLYN"].ToString();

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

                    ddlMHFRelation.SelectedIndex = ddlMHFRelation.Items.IndexOf(ddlMHFRelation.Items.FindByValue(dt.Rows[0]["HumanRelationId"].ToString()));
                    txtMHFName.Text = Convert.ToString(dt.Rows[0]["FamilyPersonName"]);

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
                    txtReAcNo.Text = Convert.ToString(dt.Rows[0]["AccNo"]);

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

                    CheckCheckBox(chkApplSecBusi, Convert.ToString(dt3.Rows[0]["NoOfSecBusi"]));
                    // EnableDisableApplSecBusiness(Convert.ToString(dt3.Rows[0]["NoOfSecBusi"]).Trim());

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

                    txtUrnNo.Text = Convert.ToString(dt.Rows[0]["UrnNo"]);
                    txtEnterprise.Text = Convert.ToString(dt3.Rows[0]["EnterpriseType"]);
                    txtIndustry.Text = Convert.ToString(dt3.Rows[0]["Industry"]);
                    txtMajorActivity.Text = Convert.ToString(dt3.Rows[0]["MajorActivity"]);

                    ddlCoApplBusiIncYN.SelectedIndex = ddlCoApplBusiIncYN.Items.IndexOf(ddlCoApplBusiIncYN.Items.FindByValue(dt4.Rows[0]["BusiIncYN"].ToString()));
                    ddlCoApplBusiName.SelectedIndex = ddlCoApplBusiName.Items.IndexOf(ddlCoApplBusiName.Items.FindByValue(dt4.Rows[0]["AppCoAppSameBusiYN"].ToString().Trim()));
                    txtCoApplBusiName.Text = Convert.ToString(dt4.Rows[0]["BusinessName"]);
                    ddlCoApplPrimaryBusiType.SelectedIndex = ddlCoApplPrimaryBusiType.Items.IndexOf(ddlCoApplPrimaryBusiType.Items.FindByValue(dt4.Rows[0]["PrimaryBusiType"].ToString()));
                    ddlCoApplPrimaryBusiSeas.SelectedIndex = ddlCoApplPrimaryBusiSeas.Items.IndexOf(ddlCoApplPrimaryBusiSeas.Items.FindByValue(dt4.Rows[0]["PrimaryBusiSeaso"].ToString()));
                    popCoApplSecBusiSubType(Convert.ToInt32(dt3.Rows[0]["PrimaryBusiType"]));
                    ddlCoApplPrimaryBusiSubType.SelectedIndex = ddlCoApplPrimaryBusiSubType.Items.IndexOf(ddlCoApplPrimaryBusiSubType.Items.FindByValue(dt4.Rows[0]["PrimaryBusiSubType"].ToString()));
                    PopCoApplPrimaryBusiActivity(Convert.ToInt32(dt3.Rows[0]["PrimaryBusiSubType"]));
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

                    bool vViewPotenMem = false;
                    CPdMst oRl = new CPdMst();
                    DataTable dtRole = new DataTable();
                    dtRole = oRl.GetRoleById(Convert.ToInt32(Session[gblValue.RoleId].ToString()));
                    if (dtRole.Rows.Count > 0)
                    {
                        vViewPotenMem = Convert.ToString(dt.Rows[0]["ShowPotential"]) == "Y" && Convert.ToString(dtRole.Rows[0]["PotenMemYN"]) == "Y" ? true : false;
                    }
                    btnShwPotenMem.Visible = vViewPotenMem;
                    btnUpdateUcic.Visible = vViewPotenMem;
                    hdnProtenUrl.Value = Convert.ToString(dt.Rows[0]["PotenURL"]);
                    //-----------------------------------Identy Proof-------------------------------------------------
                    ddlAddPrf.SelectedIndex = ddlAddPrf.Items.IndexOf(ddlAddPrf.Items.FindByValue(dt.Rows[0]["ApplAddProfId"].ToString()));
                    ddlIdentyProf.SelectedIndex = ddlIdentyProf.Items.IndexOf(ddlIdentyProf.Items.FindByValue(dt.Rows[0]["ApplIdentyPRofId"].ToString()));
                    ddlCoAppID.SelectedIndex = ddlCoAppID.Items.IndexOf(ddlCoAppID.Items.FindByValue(dt.Rows[0]["CoApplIdentyPRofId"].ToString()));

                    if (Convert.ToInt32(Session[gblValue.RoleId]) != 1)
                    {
                        if (dt.Rows[0]["ApplIdentyPRofId"].ToString() == "1")
                        {
                            hdnApplAadhar.Value = Convert.ToString(dt.Rows[0]["ApplIdentyProfNo"]);
                            txtIdntPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["ApplIdentyProfNo"]).Substring(Convert.ToString(dt.Rows[0]["ApplIdentyProfNo"]).Length - 4, 4));
                        }
                        else
                        {
                            txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["ApplIdentyProfNo"]);
                        }
                        if (dt.Rows[0]["ApplAddProfId"].ToString() == "1")
                        {
                            hdnApplAadhar.Value = Convert.ToString(dt.Rows[0]["ApplAddProfNo"]);
                            txtAddPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["ApplAddProfNo"]).Substring(Convert.ToString(dt.Rows[0]["ApplAddProfNo"]).Length - 4, 4));
                        }
                        else
                        {
                            txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["ApplAddProfNo"]);
                        }
                        if (dt.Rows[0]["CoApplIdentyPRofId"].ToString() == "1")
                        {
                            hdnCoApplAadhar.Value = Convert.ToString(dt.Rows[0]["CoApplIdentyProfNo"]);
                            txtCoAppID.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["CoApplIdentyProfNo"]).Substring(Convert.ToString(dt.Rows[0]["CoApplIdentyProfNo"]).Length - 4, 4));
                        }
                        else
                        {
                            txtCoAppID.Text = Convert.ToString(dt.Rows[0]["CoApplIdentyProfNo"]);
                        }
                    }
                    else
                    {
                        if (dt.Rows[0]["ApplIdentyPRofId"].ToString() == "1")
                        {
                            hdnApplAadhar.Value = Convert.ToString(dt.Rows[0]["ApplIdentyProfNo"]);
                        }
                        else if (dt.Rows[0]["ApplAddProfId"].ToString() == "1")
                        {
                            hdnApplAadhar.Value = Convert.ToString(dt.Rows[0]["ApplAddProfNo"]);
                        }
                        if (dt.Rows[0]["CoApplIdentyPRofId"].ToString() == "1")
                        {
                            hdnCoApplAadhar.Value = Convert.ToString(dt.Rows[0]["CoApplIdentyProfNo"]);
                        }
                        txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["ApplIdentyProfNo"]);
                        txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["ApplAddProfNo"]);
                        txtCoAppID.Text = Convert.ToString(dt.Rows[0]["CoApplIdentyProfNo"]);
                    }

                    //----------------------------------------------------------------------------------------------
                    //ClientScript.RegisterStartupScript(this.GetType(), "script", "ShowHideCoApplInc()", true);

                    if (ddlCpAppOthrIncm.SelectedValue == "N")
                    {
                        trInc.Attributes.Add("style", "visibility:hidden;");
                        tdTypeOfInc.Attributes.Add("style", "visibility:hidden;");
                        tdTypeOfIncDdl.Attributes.Add("style", "visibility:hidden;");
                    }
                    else
                    {
                        trInc.Attributes.Add("style", "visibility:visible;");
                        tdTypeOfInc.Attributes.Add("style", "visibility:visible;");
                        tdTypeOfIncDdl.Attributes.Add("style", "visibility:visible;");
                    }
                    //----------------------------------------------------------------------------------------
                    if (Convert.ToInt32(Session[gblValue.RoleId]) == 1 || Convert.ToInt32(Session[gblValue.RoleId]) == 11
                        || Convert.ToInt32(Session[gblValue.RoleId]) == 25 || Convert.ToInt32(Session[gblValue.RoleId]) == 57)
                    {
                        fuApplPhoto.Visible = true;
                    }
                    else
                    {
                        fuApplPhoto.Visible = false;
                        if (gblFuction.setDate(Session["EnquiryDate"].ToString()) >= gblFuction.setDate("12/06/2025"))
                        {
                            if (ddlIdentyProf.SelectedValue == "13")
                            {
                                fuApplID1F.Visible = false;
                                fuApplID1B.Visible = false;
                            }
                            else
                            {
                                fuApplID1F.Visible = true;
                                fuApplID1B.Visible = true;
                            }
                            if (ddlCoAppID.SelectedValue == "13")
                            {
                                fuCoApplID1F.Visible = false;
                                fuCoApplID1B.Visible = false;
                            }
                            else
                            {
                                fuCoApplID1F.Visible = true;
                                fuCoApplID1B.Visible = true;
                            }
                        }
                    }
                    //----------------------------------------------------------------------------------------

                    //if (dt.Rows[0]["ApplIdentyPRofId"].ToString() == "13")
                    //{
                    //    fuApplID1F.Enabled = false;
                    //    fuApplID1B.Enabled = false;
                    //}
                    //if (dt.Rows[0]["ApplAddProfId"].ToString() == "13")
                    //{
                    //    fuApplID2F.Enabled = false;
                    //    fuApplID2B.Enabled = false;
                    //}
                    //if (dt.Rows[0]["CoApplIdentyPRofId"].ToString() == "13")
                    //{
                    //    fuCoApplID1F.Enabled = false;
                    //    fuCoApplID1B.Enabled = false;
                    //}

                    tbEmp.ActiveTabIndex = 1;
                    if (Session[gblValue.BrnchCode].ToString() == "0000")
                    {
                        StatusButton("View");
                    }
                    else
                    {
                        if (Convert.ToString(dt.Rows[0]["PdByBMDate"]) == "")
                        {
                            StatusButton("Show");
                        }
                        else
                        {
                            StatusButton("View");
                        }
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

        protected void chkCoAppCommAdr_CheckedChanged(object sender, EventArgs e)
        {
            txtCoAppCommAddr.Text = txtCoApplAddr.Text;
            txtCoAppCommPin.Text = txtCoApplPin.Text;
            ddlCoAppCommState.SelectedIndex = ddlCoAppCommState.Items.IndexOf(ddlCoAppCommState.Items.FindByValue(Convert.ToString(ddlCoApplState.SelectedValue)));
            ddlCoApplPreAddrType.SelectedIndex = ddlCoApplPreAddrType.Items.IndexOf(ddlCoApplPreAddrType.Items.FindByValue(Convert.ToString(ddlCoApplPerAddrType.SelectedValue)));

        }

        protected void btnShwPotenMem_Click(object sender, EventArgs e)
        {
            string vUrl = hdnProtenUrl.Value;
            string url = vUrl + "BIJLI";
            string s = "window.open('" + url + "', '_blank', 'width=900,height=600,left=100,top=100,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            return;
        }

        protected void btnUpdateUcic_Click(object sender, EventArgs e)
        {
            string pMemberId = Convert.ToString(ViewState["MemberId"]);
            int vPdId = Convert.ToInt32(ViewState["PdId"]);
            string pUcicId = getUcic(pMemberId, Convert.ToInt32(Session[gblValue.UserId]), vPdId);
            int pErr = -1;
            CPdMst pd = new CPdMst();
            if (pUcicId != "")
            {
                pErr = pd.UpdateUcicId(pUcicId, pMemberId, vPdId);
                if (pErr == 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.EditMsg);
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                }
            }
            else
            {
                gblFuction.MsgPopup("Respose Error");
            }
        }

        private string SaveMemberImages(FileUpload flup, string imageGroup, string imageName, string Mode, string isImageSaved, string ImagePath, string module)
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
                isImageSaved = oAC.UploadFileMinio(ResizeImage(ImgByte, 0.8), imageName + ".png", imageGroup, module == "I" ? InitialBucket : PdBucket, MinioUrl);
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

        #region JocataCalling

        public string GetJokataToken()
        {
            string postURL = "https://aml.unitybank.co.in//ramp/webservices/createToken";
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                request.Method = "POST";
                request.ContentType = "text/plain";
                request.Headers.Add("username", "BU_Bijli");
                request.Headers.Add("password", "BU_Bijli");
                request.Headers.Add("clientId", "BU_Bijli");
                request.Headers.Add("clientSecret", "BU_Bijli");
                request.Headers.Add("subBu", "Sub_BU_IB");
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                dynamic res = JsonConvert.DeserializeObject(fullResponse);
                string vJokataToken = res.token;
                return vJokataToken;
            }
            catch (WebException we)
            {
                string Response = "";
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Response = reader.ReadToEnd();
                }
                return Response;
            }
            finally
            {
                // streamWriter = null;
            }
        }

        public string RampRequest(PostRampRequest postRampRequest)
        {
            string vJokataToken = vJocataToken, vMemberId = "", vRampResponse = "";
            try
            {
                //-----------------------Create Token--------------------------         
                //vJokataToken = GetJokataToken();
                //vMemberId = postRampRequest.rampRequest.listMatchingPayload.requestListVO.requestVOList[0].customerId;
                //CPdMst oPD = new CPdMst();
                //oPD.SaveJocataToken(vMemberId, vJokataToken);
                //-----------------------Ramp Request------------------------
                string postURL = "https://aml.unitybank.co.in/ramp/webservices/request/handle-ramp-request";
                string Requestdata = JsonConvert.SerializeObject(postRampRequest);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + vJokataToken);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vRampResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                return vRampResponse;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vRampResponse = reader.ReadToEnd();
                }
            }
            finally
            {
            }
            return vRampResponse;
        }

        public string JocataRequest(string pMemberID, Int32 pPdID, Int32 pCreatedBy)
        {
            try
            {
                DataTable dt = new DataTable();
                CPdMst oPD = null;
                oPD = new CPdMst();
                dt = oPD.GetJocataRequestData(pMemberID);
                if (dt.Rows.Count > 0)
                {
                    List<RequestVOList> vRVL = new List<RequestVOList>();
                    vRVL.Add(new RequestVOList
                    {
                        aadhar = dt.Rows[0]["Aadhar"].ToString(),
                        address = dt.Rows[0]["ParAddress"].ToString(),
                        city = dt.Rows[0]["District"].ToString(),
                        country = dt.Rows[0]["Country"].ToString(),
                        concatAddress = dt.Rows[0]["PreAddr"].ToString(),
                        customerId = dt.Rows[0]["MemberID"].ToString(),
                        digitalID = "",
                        din = "",
                        dob = dt.Rows[0]["DOB"].ToString(),
                        docNumber = "",
                        drivingLicence = dt.Rows[0]["DL"].ToString(),
                        email = "",
                        entityName = "",
                        name = dt.Rows[0]["MemberName"].ToString(),
                        nationality = "Indian",
                        pan = dt.Rows[0]["Pan"].ToString(),
                        passport = dt.Rows[0]["Passport"].ToString(),
                        phone = dt.Rows[0]["Mobile"].ToString(),
                        pincode = dt.Rows[0]["PinCode"].ToString(),
                        rationCardNo = dt.Rows[0]["RationCard"].ToString(),
                        ssn = "",
                        state = dt.Rows[0]["State"].ToString(),
                        tin = "",
                        voterId = dt.Rows[0]["Voter"].ToString()
                    });

                    var vLV = new RequestListVO();
                    vLV.businessUnit = "BU_Bijli";
                    vLV.subBusinessUnit = "Sub_BU_IB";
                    vLV.requestType = "API";
                    vLV.requestVOList = vRVL;

                    var vLMP = new ListMatchingPayload();
                    vLMP.requestListVO = vLV;

                    var vRR = new RampRequest();
                    vRR.listMatchingPayload = vLMP;

                    var req = new PostRampRequest();
                    req.rampRequest = vRR;

                    string vResponseData = RampRequest(req);
                    dynamic vResponse = JsonConvert.DeserializeObject(vResponseData);
                    string vScreeningId = "";
                    if (vResponse.rampResponse.statusCode == "200")
                    {
                        Boolean vMatchFlag = vResponse.rampResponse.listMatchResponse.matchResult.matchFlag;
                        vScreeningId = vResponse.rampResponse.listMatchResponse.matchResult.uniqueRequestId;
                        string vStatus = "P";
                        if (vMatchFlag == true)
                        {
                            vStatus = "N";
                        }
                        else
                        {
                            try
                            {
                                Prosidex(pMemberID, Convert.ToString(pPdID), pCreatedBy);
                            }
                            finally { }
                        }
                        oPD = new CPdMst();
                        oPD.UpdateJocataStatus(pMemberID, pPdID, vScreeningId, vStatus, pCreatedBy, "", "LOW");
                    }
                    string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));
                    oPD.SaveJocataLog(pMemberID, Convert.ToInt32(pPdID), vResponseXml, vScreeningId);
                }
            }
            finally { }
            return "";
        }

        #endregion

        #region Common
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
        #endregion

        #region Prosidex Integration
        public ProsidexResponse Prosidex(string pMemberID, string pPDId, Int32 pCreatedBy)
        {
            DataTable dt = new DataTable();
            CPdMst oPD = new CPdMst();

            ProsidexRequest pReqData = new ProsidexRequest();
            Request pReq = new Request();
            DG pDg = new DG();
            ProsidexResponse pResponseData = null;

            dt = oPD.GetProsidexReqData(pMemberID, pCreatedBy);
            if (dt.Rows.Count > 0)
            {
                pDg.ACCOUNT_NUMBER = dt.Rows[0]["ACCOUNT_NUMBER"].ToString();
                pDg.ALIAS_NAME = dt.Rows[0]["ALIAS_NAME"].ToString();
                pDg.APPLICATIONID = pPDId;
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

        public ProsidexResponse ProsidexSearchCustomer(ProsidexRequest prosidexRequest)
        {
            string vResponse = "", vUCIC = "", vRequestId = "", vMemberId = "";
            Int32 vCreatedBy = 1;
            ProsidexResponse oProsidexResponse = null;
            CPdMst oPD = new CPdMst();
            try
            {
                vRequestId = prosidexRequest.Request.UnitySfb_RequestId;
                vMemberId = prosidexRequest.Request.DG.CUST_ID;

                string Requestdata = JsonConvert.SerializeObject(prosidexRequest);
                //string postURL = "http://144.24.116.182:9002/UnitySfbWS/searchCustomer";
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
                vUCIC = res.Response.StatusInfo.POSIDEX_GENERATED_UCIC;
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oPD.SaveProsidexLog(vMemberId, vRequestId, vResponseXml, vCreatedBy, vUCIC);
                //--------------------------------------------------------------------------------------  
                oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 200);
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
                oPD.SaveProsidexLog(vMemberId, vRequestId, vResponseXml, vCreatedBy, vUCIC);
                //--------------------------------------------------------------------------------------
            }
            finally
            {
            }
            oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 500);
            return oProsidexResponse;
        }

        public string getUcic(string pMemberId, int pCreatedBy, int pPDId)
        {
            string vResponse = "", vUcic = "", vMemberId = "";
            CPdMst oPD = new CPdMst();
            try
            {
                vMemberId = "S" + pMemberId;
                string Requestdata = "{\"cust_id\" :" + "\"" + vMemberId + "\"" + ",\"source_system_name\":\"BIJLI\"}";
                //  string postURL = "http://144.24.116.182:9002/UnitySfbWS/getUcic";
                string postURL = "https://ucic.unitybank.co.in:9002/UnitySfbWS/getUcic";
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
                if (res.ResponseCode == "200")
                {
                    vUcic = res.Ucic;
                }
                //----------------------------Save Log---------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oPD.SaveProsidexLogUCIC(pMemberId, pPDId, vResponseXml, pCreatedBy, vUcic);
                //---------------------------------------------------------------------------------
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
                //----------------------------Save Log---------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oPD.SaveProsidexLogUCIC(pMemberId, pPDId, vResponseXml, pCreatedBy, vUcic);
                //---------------------------------------------------------------------------------
            }
            return vUcic;
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

    #region Jocata_Class
    public class RequestListVO
    {
        public string businessUnit { get; set; }
        public string subBusinessUnit { get; set; }
        public string requestType { get; set; }
        public List<RequestVOList> requestVOList { get; set; }
    }
    public class RequestVOList
    {
        public string aadhar { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string concatAddress { get; set; }
        public string country { get; set; }
        public string customerId { get; set; }
        public string digitalID { get; set; }
        public string din { get; set; }
        public string dob { get; set; }
        public string docNumber { get; set; }
        public string drivingLicence { get; set; }
        public string email { get; set; }
        public string entityName { get; set; }
        public string name { get; set; }
        public string nationality { get; set; }
        public string pan { get; set; }
        public string passport { get; set; }
        public string phone { get; set; }
        public string pincode { get; set; }
        public string rationCardNo { get; set; }
        public string ssn { get; set; }
        public string state { get; set; }
        public string tin { get; set; }
        public string voterId { get; set; }
    }
    public class ListMatchingPayload
    {
        public RequestListVO requestListVO { get; set; }
    }
    public class RampRequest
    {
        public ListMatchingPayload listMatchingPayload { get; set; }
    }
    public class PostRampRequest
    {
        public RampRequest rampRequest { get; set; }
    }
    #endregion

    #region Prosidex_Class
    public class DG
    {
        public string ACCOUNT_NUMBER { get; set; }
        public string ALIAS_NAME { get; set; }
        public string APPLICATIONID { get; set; }
        public string Aadhar { get; set; }
        public string CIN { get; set; }
        public string CKYC { get; set; }
        public string CUSTOMER_STATUS { get; set; }
        public string CUST_ID { get; set; }
        public string DOB { get; set; }
        public string DrivingLicense { get; set; }
        public string Father_First_Name { get; set; }
        public string Father_Last_Name { get; set; }
        public string Father_Middle_Name { get; set; }
        public string Father_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Gender { get; set; }
        public string GSTIN { get; set; }
        public string Lead_Id { get; set; }
        public string NREGA { get; set; }
        public string Pan { get; set; }
        public string PassportNo { get; set; }
        public string RELATION_TYPE { get; set; }
        public string RationCard { get; set; }
        public string Registration_NO { get; set; }
        public string SALUTATION { get; set; }
        public string TAN { get; set; }
        public string Udyam_aadhar_number { get; set; }
        public string VoterId { get; set; }
        public string Tasc_Customer { get; set; }
    }
    public class Request
    {
        public DG DG { get; set; }
        public List<object> ACE { get; set; }
        public string UnitySfb_RequestId { get; set; }
        public string CUST_TYPE { get; set; }
        public string CustomerCategory { get; set; }
        public string MatchingRuleProfile { get; set; }
        public string Req_flag { get; set; }
        public string SourceSystem { get; set; }
    }
    public class ProsidexRequest
    {
        public Request Request { get; set; }
    }
    public class ProsiReq
    {
        public string pMemberId { get; set; }
        public string pCreatedBy { get; set; }
    }
    public class ProsidexResponse
    {
        public string RequestId { get; set; }
        public string UCIC { get; set; }
        public int response_code { get; set; }
        public ProsidexResponse(string RequestId, string UCIC, int response_code)
        {
            this.RequestId = RequestId;
            this.UCIC = UCIC;
            this.response_code = response_code;
        }
    }
    #endregion
}