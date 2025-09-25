using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Reporting.WebForms;
using CENTRUMCA;
using CENTRUMBA;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Web.Services;

namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class Customer360 : CENTRUMBAse
    {
        protected int cPgNo = 1;
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string BucketURL = ConfigurationManager.AppSettings["BucketURL"];
        string CFDocumentBucket = ConfigurationManager.AppSettings["CFDocumentBucket"];
        string DocumentBucketURL = ConfigurationManager.AppSettings["DocumentBucketURL"];
        private static string vKarzaKey = ConfigurationManager.AppSettings["KarzaKey"];
        string vKarzaEnv = ConfigurationManager.AppSettings["KarzaEnv"];
        string MaxFileSize = ConfigurationManager.AppSettings["MaxFileSize"];
        string KarzaVoterUrl = ConfigurationManager.AppSettings["KarzaVoterUrl"];
        string KarzaPanUrl = ConfigurationManager.AppSettings["KarzaPanUrl"];
        string KarzaUatVoterUrl = ConfigurationManager.AppSettings["KarzaUatVoterUrl"];
        string KarzaUatPanUrl = ConfigurationManager.AppSettings["KarzaUatPanUrl"];
        private static string KarzaKeyUat = ConfigurationManager.AppSettings["KarzaKeyUat"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopLeadList();
                popCaste();
                PopReligion();
                PopQualification();
                popState();
                PopRelation();

                tbCust360.ActiveTabIndex = 0;
                hdnMaxFileSize.Value = MaxFileSize;

                if (Session[gblValue.MemberID] != null)
                {
                    GetMemberDtl();
                    StatusButton("Show");
                    Int64 LeadId = Convert.ToInt64(Session[gblValue.LeadID]);
                    CheckOprtnStatus(Convert.ToInt64(LeadId));
                }
                else
                {
                    StatusButton("Close");
                    gblFuction.MsgPopup("Please Select Lead from Loan Application Status screen and proceed...");
                    return;
                }



            }

        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Customer 360";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFCust360);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Customer 360", false);
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
                    txtCustName.Enabled = false;
                    txtAppMob.Enabled = false;

                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;

                    EnableControl(false);
                    txtCustName.Enabled = false;
                    txtAppMob.Enabled = false;
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;

                    EnableControl(true);
                    txtCustName.Enabled = false;
                    txtAppMob.Enabled = false;
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;

                    EnableControl(false);
                    txtCustName.Enabled = false;
                    txtAppMob.Enabled = false;
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;

                    EnableControl(false);
                    txtCustName.Enabled = false;
                    txtAppMob.Enabled = false;
                    break;
                case "Exit":
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    break;
                case "Close":
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        private void EnableControl(Boolean Status)
        {
            Boolean vAppPanVerifyYNStatus = hdnAppPanVerifyYN.Value == "Y" ? false : Status;
            Boolean vAppVoterVerifyYNStatus = hdnAppVoterVerifyYN.Value == "Y" ? false : Status;
            Boolean vAppAadhVerifyYNStatus = hdnAppAadhVerifyYN.Value == "Y" ? false : Status;

            Boolean vCoApp1PanVerifyYNStatus = hdnCoApp1PanVerifyYN.Value == "Y" ? false : Status;
            Boolean vCoApp1VoterVerifyYNStatus = hdnCoApp1VoterVerifyYN.Value == "Y" ? false : Status;
            Boolean vCoApp1AadhVerifyYNStatus = hdnCoApp1AadhVerifyYN.Value == "Y" ? false : Status;

            Boolean vCoApp2PanVerifyYNStatus = hdnCoApp2PanVerifyYN.Value == "Y" ? false : Status;
            Boolean vCoApp2VoterVerifyYNStatus = hdnCoApp2VoterVerifyYN.Value == "Y" ? false : Status;
            Boolean vCoApp2AadhVerifyYNStatus = hdnCoApp2AadhVerifyYN.Value == "Y" ? false : Status;

            Boolean vCoApp3PanVerifyYNStatus = hdnCoApp3PanVerifyYN.Value == "Y" ? false : Status;
            Boolean vCoApp3VoterVerifyYNStatus = hdnCoApp3VoterVerifyYN.Value == "Y" ? false : Status;
            Boolean vCoApp3AadhVerifyYNStatus = hdnCoApp3AadhVerifyYN.Value == "Y" ? false : Status;

            Boolean vCoApp4PanVerifyYNStatus = hdnCoApp3PanVerifyYN.Value == "Y" ? false : Status;
            Boolean vCoApp4VoterVerifyYNStatus = hdnCoApp3VoterVerifyYN.Value == "Y" ? false : Status;
            Boolean vCoApp4AadhVerifyYNStatus = hdnCoApp3AadhVerifyYN.Value == "Y" ? false : Status;

            Boolean vGuarPanVerifyYNStatus = hdnGuarPanVerifyYN.Value == "Y" ? false : Status;
            Boolean vGuarVoterVerifyYNStatus = hdnGuarVoterVerifyYN.Value == "Y" ? false : Status;
            Boolean vGuarAadhVerifyYNStatus = hdnGuarAadhVerifyYN.Value == "Y" ? false : Status;


            ddlCustomer.Enabled = Status;
            txtCustId.Enabled = false;
            txtCustName.Enabled = Status;
            ddlApplGender.Enabled = Status;
            txtApplDOB.Enabled = Status;
            txtApplAge.Enabled = Status;
            //  txtApplAgeMaturity.Enabled = Status;
            txtApplPan.Enabled = vAppPanVerifyYNStatus;
            txtApplAadhRefNo.Enabled = false;
            txtApplVoterNo.Enabled = vAppVoterVerifyYNStatus;
            ddlRelWithApp.Enabled = false;
            txtAppMob.Enabled = Status;
            ddlAppEdu.Enabled = Status;
            ddlApplMaritalStatus.Enabled = Status;
            txtNoOfFamilyMem.Enabled = Status;
            txtNoOfDependent.Enabled = Status;
            ddlApplCast.Enabled = Status;
            ddlApplReligion.Enabled = Status;
            ddlAppMinorityYN.Enabled = Status;
            txtApplEmail.Enabled = Status;
            txtAppPerAddress.Enabled = Status;
            ddlApplDist.Enabled = Status;
            // ddlApplState.Enabled = Status;
            txtApplPin.Enabled = Status;
            txtApplCurrAddress.Enabled = Status;
            ddlApplCurrDist.Enabled = Status;
            // ddlApplCurrState.Enabled = Status;
            txtApplCurrPin.Enabled = Status;
            txtApplLandMark.Enabled = Status;
            ddlApplOwnshipStatus.Enabled = Status;
            fuOwnProof.Enabled = Status;
            //ImgOwnshipPhoto.Enabled = Status;
            ddlResiStabYrs.Enabled = Status;

            txtCoApp1Id.Enabled = false;
            txtCoApp1Name.Enabled = Status;
            ddlCoApp1Gender.Enabled = Status;
            txtCoApp1Dob.Enabled = Status;
            txtCoApp1Age.Enabled = Status;
            // txtCoApp1AgeAtLoanMaturity.Enabled = Status;
            txtCoApp1Pan.Enabled = vCoApp1PanVerifyYNStatus;
            txtCoApp1AadhRefNo.Enabled = false;
            txtCoApp1Voter.Enabled = vCoApp1VoterVerifyYNStatus;
            ddlCoApp1RelWithApp.Enabled = Status;
            txtCoApp1Mob.Enabled = Status;
            ddlCoApp1Edu.Enabled = Status;
            ddlCoApp1Marital.Enabled = Status;
            txtCoApp1NoOfFamily.Enabled = Status;
            txtCoApp1NoOfDependents.Enabled = Status;
            ddlCoApp1Caste.Enabled = Status;
            ddlCoApp1Religion.Enabled = Status;
            ddlCoApp1Minority.Enabled = Status;
            txtCoApp1Email.Enabled = Status;
            txtCoApp1PerResiAdd.Enabled = Status;
            ddlCoApp1PerDist.Enabled = Status;
            //  ddlCoApp1PerState.Enabled = Status;
            txtCoApp1PerPin.Enabled = Status;
            txtCoApp1CurrAdd.Enabled = Status;
            ddlCoApp1CurrDist.Enabled = Status;
            //  ddlCoApp1CurrState.Enabled = Status;
            txtCoApp1CurrPin.Enabled = Status;
            txtCoApp1Landmark.Enabled = Status;
            ddlCoApp1OwnShip.Enabled = Status;
            fuCoApp1CurrAddOwnPrf.Enabled = Status;
            //  imgCoApp1OwnProof.Enabled = Status;
            ddlCoApp1ResiStabYrs.Enabled = Status;


            txtCoApp2CustId.Enabled = false;
            txtCoApp2CustName.Enabled = Status;
            ddlCoApp2Gender.Enabled = Status;
            txtCoApp2Dob.Enabled = Status;
            txtCoApp2Age.Enabled = Status;
            //  txtCoApp2AgeAtLoanMaturity.Enabled = Status;
            txtCoApp2Pan.Enabled = vCoApp2PanVerifyYNStatus;
            txtCoApp2AadhRefNo.Enabled = false;
            txtCoApp2Voter.Enabled = vCoApp2VoterVerifyYNStatus;
            ddlCoApp2RelWithApp.Enabled = Status;
            txtCoApp2Mob.Enabled = Status;
            ddlCoApp2Edu.Enabled = Status;
            ddlCoApp2Marital.Enabled = Status;
            txtCoApp2NoOfFamily.Enabled = Status;
            txtCoApp2NoOfDependents.Enabled = Status;
            ddlCoApp2Caste.Enabled = Status;
            ddlCoApp2Religion.Enabled = Status;
            ddlCoApp2Minority.Enabled = Status;
            txtCoApp2Email.Enabled = Status;
            txtCoApp2PerResiAdd.Enabled = Status;
            ddlCoApp2PerDist.Enabled = Status;
            // ddlCoApp2PerState.Enabled = Status;
            txtCoApp2PerPin.Enabled = Status;
            txtCoApp2CurrResiAdd.Enabled = Status;
            ddlCoApp2CurrDist.Enabled = Status;
            //  ddlCoApp2CurrState.Enabled = Status;
            txtCoApp2CurrPin.Enabled = Status;
            txtCoApp2Landmark.Enabled = Status;
            ddlCoApp2Ownship.Enabled = Status;
            fuCoApp2OwnProof.Enabled = Status;
            //  imgCoApp2OwnProof.Enabled = Status;
            ddlCoApp2ResiStabYrs.Enabled = Status;

            txtCoApp3CustId.Enabled = false;
            txtCoApp3CustName.Enabled = Status;
            ddlCoApp3Gender.Enabled = Status;
            txtCoApp3Dob.Enabled = Status;
            txtCoApp3Age.Enabled = Status;
            //  txtCoApp3AgeAtLoanMaturity.Enabled = Status;
            txtCoApp3Pan.Enabled = vCoApp3PanVerifyYNStatus;
            txtCoApp3AadhRefNo.Enabled = false;
            txtCoApp3Voter.Enabled = vCoApp3VoterVerifyYNStatus;
            ddlCoApp3RelWithApp.Enabled = Status;
            txtCoApp3Mob.Enabled = Status;
            ddlCoApp3Edu.Enabled = Status;
            ddlCoApp3Marital.Enabled = Status;
            txtCoApp3NoOfFamily.Enabled = Status;
            txtCoApp3NoOfDependents.Enabled = Status;
            ddlCoApp3Caste.Enabled = Status;
            ddlCoApp3Religion.Enabled = Status;
            ddlCoApp3Minority.Enabled = Status;
            txtCoApp3Email.Enabled = Status;
            txtCoApp3PerResiAdd.Enabled = Status;
            ddlCoApp3PerDist.Enabled = Status;
            // ddlCoApp3PerState.Enabled = Status;
            txtCoApp3PerPin.Enabled = Status;
            txtCoApp3CurrResiAdd.Enabled = Status;
            ddlCoApp3CurrDist.Enabled = Status;
            //  ddlCoApp3CurrState.Enabled = Status;
            txtCoApp3CurrPin.Enabled = Status;
            txtCoApp3Landmark.Enabled = Status;
            ddlCoApp3Ownship.Enabled = Status;
            fuCoApp3OwnProof.Enabled = Status;
            //  imgCoApp3OwnProof.Enabled = Status;
            ddlCoApp3ResiStabYrs.Enabled = Status;

            txtCoApp4CustId.Enabled = false;
            txtCoApp4CustName.Enabled = Status;
            ddlCoApp4Gender.Enabled = Status;
            txtCoApp4Dob.Enabled = Status;
            txtCoApp4Age.Enabled = Status;
            //  txtCoApp4AgeAtLoanMaturity.Enabled = Status;
            txtCoApp4Pan.Enabled = vCoApp4PanVerifyYNStatus;
            txtCoApp4AadhRefNo.Enabled = false;
            txtCoApp4Voter.Enabled = vCoApp4VoterVerifyYNStatus;
            ddlCoApp4RelWithApp.Enabled = Status;
            txtCoApp4Mob.Enabled = Status;
            ddlCoApp4Edu.Enabled = Status;
            ddlCoApp4Marital.Enabled = Status;
            txtCoApp4NoOfFamily.Enabled = Status;
            txtCoApp4NoOfDependents.Enabled = Status;
            ddlCoApp4Caste.Enabled = Status;
            ddlCoApp4Religion.Enabled = Status;
            ddlCoApp4Minority.Enabled = Status;
            txtCoApp4Email.Enabled = Status;
            txtCoApp4PerResiAdd.Enabled = Status;
            ddlCoApp4PerDist.Enabled = Status;
            // ddlCoApp4PerState.Enabled = Status;
            txtCoApp4PerPin.Enabled = Status;
            txtCoApp4CurrResiAdd.Enabled = Status;
            ddlCoApp4CurrDist.Enabled = Status;
            //  ddlCoApp4CurrState.Enabled = Status;
            txtCoApp4CurrPin.Enabled = Status;
            txtCoApp4Landmark.Enabled = Status;
            ddlCoApp4Ownship.Enabled = Status;
            fuCoApp4OwnProof.Enabled = Status;
            //  imgCoApp4OwnProof.Enabled = Status;
            ddlCoApp4ResiStabYrs.Enabled = Status;

            txtGuarId.Enabled = false;
            txtGuarName.Enabled = Status;
            ddlGuarGender.Enabled = Status;
            txtGuarDOB.Enabled = Status;
            txtGuarAge.Enabled = Status;
            //  txtGuarAgeAtLoanMaturity.Enabled = Status;
            txtGuarPan.Enabled = vGuarPanVerifyYNStatus;
            txtGuarAadhRefNo.Enabled = false;
            txtGuarVoter.Enabled = vGuarVoterVerifyYNStatus;
            ddlGuarRelWithApp.Enabled = Status;
            txtGuarMob.Enabled = Status;
            ddlGuarEdu.Enabled = Status;
            ddlGuarMarital.Enabled = Status;
            txtGuarNoOfFamily.Enabled = Status;
            txtGuarNoOfDependents.Enabled = Status;
            ddlGuarCaste.Enabled = Status;
            ddlGuarReligion.Enabled = Status;
            ddlGuarMinority.Enabled = Status;
            txtGuarEmail.Enabled = Status;
            txtGuarPerResiAdd.Enabled = Status;
            ddlGuarPerDist.Enabled = Status;
            //  ddlGuarPerState.Enabled = Status;
            txtGuarPerPin.Enabled = Status;
            txtGuarCurrResiAdd.Enabled = Status;
            ddlGuarCurrDist.Enabled = Status;
            //  ddlGuarCurrState.Enabled = Status;
            txtGuarCurrPin.Enabled = Status;
            txtGuarLandmark.Enabled = Status;
            ddlGuarOwnStat.Enabled = Status;
            fuGuarOwnProof.Enabled = Status;
            //  imgGuarOwnProof.Enabled = Status;
            ddlGuarResiStabYrs.Enabled = Status;

            btnVerifyAppPan.Enabled = vAppPanVerifyYNStatus;
            btnVerifyCoApp1Pan.Enabled = vCoApp1PanVerifyYNStatus;
            btnVerifyCoApp2Pan.Enabled = vCoApp2PanVerifyYNStatus;
            btnVerifyCoApp3Pan.Enabled = vCoApp3PanVerifyYNStatus;
            btnVerifyCoApp4Pan.Enabled = vCoApp4PanVerifyYNStatus;
            btnVerifyGuarPan.Enabled = vGuarPanVerifyYNStatus;

            btnVerifyAppAadhaar.Enabled = vAppAadhVerifyYNStatus;
            btnVerifyCoApp1Aadhaar.Enabled = vCoApp1AadhVerifyYNStatus;
            btnVerifyCoApp2Aadhaar.Enabled = vCoApp2AadhVerifyYNStatus;
            btnVerifyCoApp3Aadhaar.Enabled = vCoApp3AadhVerifyYNStatus;
            btnVerifyCoApp4Aadhaar.Enabled = vCoApp4AadhVerifyYNStatus;
            btnVerifyGuarAadhaar.Enabled = vGuarAadhVerifyYNStatus;

            btnVerifyAppVoter.Enabled = vAppVoterVerifyYNStatus;
            btnVerifyCoApp1Voter.Enabled = vCoApp1VoterVerifyYNStatus;
            btnVerifyCoApp2Voter.Enabled = vCoApp2VoterVerifyYNStatus;
            btnVerifyCoApp3Voter.Enabled = vCoApp3VoterVerifyYNStatus;
            btnVerifyCoApp4Voter.Enabled = vCoApp4VoterVerifyYNStatus;
            btnVerifyGuarVoter.Enabled = vGuarVoterVerifyYNStatus;

            txtApplAadhNo.Enabled = vAppAadhVerifyYNStatus;
            txtCoApp1AadhNo.Enabled = vCoApp1AadhVerifyYNStatus;
            txtCoApp2AadhNo.Enabled = vCoApp2AadhVerifyYNStatus;
            txtGuarAadhNo.Enabled = vGuarAadhVerifyYNStatus;
        }
        private void ClearControls()
        {
            lblBCPNum1.Text = "";
            lblApplNm1.Text = "";
            lblBCPNum2.Text = "";
            lblApplNm2.Text = "";
            lblBCPNum3.Text = "";
            lblApplNm3.Text = "";
            ddlCustomer.SelectedIndex = -1;
            txtCustId.Text = "";
            txtCustName.Text = "";
            ddlApplGender.SelectedIndex = -1;
            txtApplDOB.Text = "";
            txtApplAge.Text = "0";
            txtApplAgeMaturity.Text = "0";
            txtApplPan.Text = "";
            txtApplAadhRefNo.Text = "";
            txtApplVoterNo.Text = "";
            //ddlRelWithApp.SelectedIndex = -1;
            txtAppMob.Text = "";
            ddlAppEdu.SelectedIndex = -1;
            ddlApplMaritalStatus.SelectedIndex = -1;
            txtNoOfFamilyMem.Text = "0";
            txtNoOfDependent.Text = "0";
            ddlApplCast.SelectedIndex = -1;
            ddlApplReligion.SelectedIndex = -1;
            ddlAppMinorityYN.SelectedIndex = -1;
            txtApplEmail.Text = "";
            txtAppPerAddress.Text = "";
            ddlApplDist.SelectedIndex = -1;
            ddlApplState.SelectedIndex = -1;
            txtApplPin.Text = "";
            txtApplCurrAddress.Text = "";
            ddlApplCurrDist.SelectedIndex = -1;
            ddlApplCurrState.SelectedIndex = -1;
            txtApplCurrPin.Text = "";
            txtApplLandMark.Text = "";
            ddlApplOwnshipStatus.SelectedIndex = -1;
            fuOwnProof.Controls.Clear();
            ImgOwnshipPhoto.Controls.Clear();
            ddlResiStabYrs.SelectedIndex = -1;

            txtCoApp1Id.Text = "";
            txtCoApp1Name.Text = "";
            ddlCoApp1Gender.SelectedIndex = -1;
            txtCoApp1Dob.Text = "";
            txtCoApp1Age.Text = "0";
            txtCoApp1AgeAtLoanMaturity.Text = "0";
            txtCoApp1Pan.Text = "";
            txtCoApp1AadhRefNo.Text = "";
            txtCoApp1Voter.Text = "";
            ddlCoApp1RelWithApp.SelectedIndex = -1;
            txtCoApp1Mob.Text = "";
            ddlCoApp1Edu.SelectedIndex = -1;
            ddlCoApp1Marital.SelectedIndex = -1;
            txtCoApp1NoOfFamily.Text = "0";
            txtCoApp1NoOfDependents.Text = "0";
            ddlCoApp1Caste.SelectedIndex = -1;
            ddlCoApp1Religion.SelectedIndex = -1;
            ddlCoApp1Minority.SelectedIndex = -1;
            txtCoApp1Email.Text = "";
            txtCoApp1PerResiAdd.Text = "";
            ddlCoApp1PerDist.SelectedIndex = -1;
            ddlCoApp1PerState.SelectedIndex = -1;
            txtCoApp1PerPin.Text = "";
            txtCoApp1CurrAdd.Text = "";
            ddlCoApp1CurrDist.SelectedIndex = -1;
            ddlCoApp1CurrState.SelectedIndex = -1;
            txtCoApp1CurrPin.Text = "";
            txtCoApp1Landmark.Text = "";
            ddlCoApp1OwnShip.SelectedIndex = -1;
            fuCoApp1CurrAddOwnPrf.Controls.Clear();
            imgCoApp1OwnProof.Controls.Clear();
            ddlCoApp1ResiStabYrs.SelectedIndex = -1;

            txtCoApp2CustId.Text = "";
            txtCoApp2CustName.Text = "";
            ddlCoApp2Gender.SelectedIndex = -1;
            txtCoApp2Dob.Text = "";
            txtCoApp2Age.Text = "0";
            txtCoApp2AgeAtLoanMaturity.Text = "0";
            txtCoApp2Pan.Text = "";
            txtCoApp2AadhRefNo.Text = "";
            txtCoApp2Voter.Text = "";
            ddlCoApp2RelWithApp.SelectedIndex = -1;
            txtCoApp2Mob.Text = "";
            ddlCoApp2Edu.SelectedIndex = -1;
            ddlCoApp2Marital.SelectedIndex = -1;
            txtCoApp2NoOfFamily.Text = "0";
            txtCoApp2NoOfDependents.Text = "0";
            ddlCoApp2Caste.SelectedIndex = -1;
            ddlCoApp2Religion.SelectedIndex = -1;
            ddlCoApp2Minority.SelectedIndex = -1;
            txtCoApp2Email.Text = "";
            txtCoApp2PerResiAdd.Text = "";
            ddlCoApp2PerDist.SelectedIndex = -1;
            ddlCoApp2PerState.SelectedIndex = -1;
            txtCoApp2PerPin.Text = "";
            txtCoApp2CurrResiAdd.Text = "";
            ddlCoApp2CurrDist.SelectedIndex = -1;
            ddlCoApp2CurrState.SelectedIndex = -1;
            txtCoApp2CurrPin.Text = "";
            txtCoApp2Landmark.Text = "";
            ddlCoApp2Ownship.SelectedIndex = -1;
            fuCoApp2OwnProof.Controls.Clear();
            imgCoApp2OwnProof.Controls.Clear();
            ddlCoApp2ResiStabYrs.SelectedIndex = -1;

            txtCoApp3CustId.Text = "";
            txtCoApp3CustName.Text = "";
            ddlCoApp3Gender.SelectedIndex = -1;
            txtCoApp3Dob.Text = "";
            txtCoApp3Age.Text = "0";
            txtCoApp3AgeAtLoanMaturity.Text = "0";
            txtCoApp3Pan.Text = "";
            txtCoApp3AadhRefNo.Text = "";
            txtCoApp3Voter.Text = "";
            ddlCoApp3RelWithApp.SelectedIndex = -1;
            txtCoApp3Mob.Text = "";
            ddlCoApp3Edu.SelectedIndex = -1;
            ddlCoApp3Marital.SelectedIndex = -1;
            txtCoApp3NoOfFamily.Text = "0";
            txtCoApp3NoOfDependents.Text = "0";
            ddlCoApp3Caste.SelectedIndex = -1;
            ddlCoApp3Religion.SelectedIndex = -1;
            ddlCoApp3Minority.SelectedIndex = -1;
            txtCoApp3Email.Text = "";
            txtCoApp3PerResiAdd.Text = "";
            ddlCoApp3PerDist.SelectedIndex = -1;
            ddlCoApp3PerState.SelectedIndex = -1;
            txtCoApp3PerPin.Text = "";
            txtCoApp3CurrResiAdd.Text = "";
            ddlCoApp3CurrDist.SelectedIndex = -1;
            ddlCoApp3CurrState.SelectedIndex = -1;
            txtCoApp3CurrPin.Text = "";
            txtCoApp3Landmark.Text = "";
            ddlCoApp3Ownship.SelectedIndex = -1;
            fuCoApp3OwnProof.Controls.Clear();
            imgCoApp3OwnProof.Controls.Clear();
            ddlCoApp3ResiStabYrs.SelectedIndex = -1;

            txtCoApp4CustId.Text = "";
            txtCoApp4CustName.Text = "";
            ddlCoApp4Gender.SelectedIndex = -1;
            txtCoApp4Dob.Text = "";
            txtCoApp4Age.Text = "0";
            txtCoApp4AgeAtLoanMaturity.Text = "0";
            txtCoApp4Pan.Text = "";
            txtCoApp4AadhRefNo.Text = "";
            txtCoApp4Voter.Text = "";
            ddlCoApp4RelWithApp.SelectedIndex = -1;
            txtCoApp4Mob.Text = "";
            ddlCoApp4Edu.SelectedIndex = -1;
            ddlCoApp4Marital.SelectedIndex = -1;
            txtCoApp4NoOfFamily.Text = "0";
            txtCoApp4NoOfDependents.Text = "0";
            ddlCoApp4Caste.SelectedIndex = -1;
            ddlCoApp4Religion.SelectedIndex = -1;
            ddlCoApp4Minority.SelectedIndex = -1;
            txtCoApp4Email.Text = "";
            txtCoApp4PerResiAdd.Text = "";
            ddlCoApp4PerDist.SelectedIndex = -1;
            ddlCoApp4PerState.SelectedIndex = -1;
            txtCoApp4PerPin.Text = "";
            txtCoApp4CurrResiAdd.Text = "";
            ddlCoApp4CurrDist.SelectedIndex = -1;
            ddlCoApp4CurrState.SelectedIndex = -1;
            txtCoApp4CurrPin.Text = "";
            txtCoApp4Landmark.Text = "";
            ddlCoApp4Ownship.SelectedIndex = -1;
            fuCoApp4OwnProof.Controls.Clear();
            imgCoApp4OwnProof.Controls.Clear();
            ddlCoApp4ResiStabYrs.SelectedIndex = -1;

            txtGuarId.Text = "";
            txtGuarName.Text = "";
            ddlGuarGender.SelectedIndex = -1;
            txtGuarDOB.Text = "";
            txtGuarAge.Text = "0";
            txtGuarAgeAtLoanMaturity.Text = "";
            txtGuarPan.Text = "";
            txtGuarAadhRefNo.Text = "";
            txtGuarVoter.Text = "";
            ddlGuarRelWithApp.SelectedIndex = -1;
            txtGuarMob.Text = "";
            ddlGuarEdu.SelectedIndex = -1;
            ddlGuarMarital.SelectedIndex = -1;
            txtGuarNoOfFamily.Text = "0";
            txtGuarNoOfDependents.Text = "0";
            ddlGuarCaste.SelectedIndex = -1;
            ddlGuarReligion.SelectedIndex = -1;
            ddlGuarMinority.SelectedIndex = -1;
            txtGuarEmail.Text = "";
            txtGuarPerResiAdd.Text = "";
            ddlGuarPerDist.SelectedIndex = -1;
            ddlGuarPerState.SelectedIndex = -1;
            txtGuarPerPin.Text = "";
            txtGuarCurrResiAdd.Text = "";
            ddlGuarCurrDist.SelectedIndex = -1;
            ddlGuarCurrState.SelectedIndex = -1;
            txtGuarCurrPin.Text = "";
            txtGuarLandmark.Text = "";
            ddlGuarOwnStat.SelectedIndex = -1;
            fuGuarOwnProof.Controls.Clear();
            imgGuarOwnProof.Controls.Clear();
            ddlGuarResiStabYrs.SelectedIndex = -1;

            txtApplAadhNo.Text = "";
            txtCoApp1AadhNo.Text = "";
            txtCoApp2AadhNo.Text = "";
            txtGuarAadhNo.Text = "";

            hdnAppPanVerifyYN.Value = "N";
            hdnAppVoterVerifyYN.Value = "N";
            hdnAppAadhVerifyYN.Value = "N";

            hdnCoApp1PanVerifyYN.Value = "N";
            hdnCoApp1VoterVerifyYN.Value = "N";
            hdnCoApp1AadhVerifyYN.Value = "N";

            hdnCoApp2PanVerifyYN.Value = "N";
            hdnCoApp2VoterVerifyYN.Value = "N";
            hdnCoApp2AadhVerifyYN.Value = "N";

            hdnGuarPanVerifyYN.Value = "N";
            hdnGuarVoterVerifyYN.Value = "N";
            hdnGuarAadhVerifyYN.Value = "N";

        }
        protected void CheckOprtnStatus(Int64 vLeadID)
        {
            Int32 vErr = 0;
            CMember oMem = null;
            oMem = new CMember();
            vErr = oMem.CF_chkOperatnStatus(vLeadID);
            if (vErr == 1)
            {
                btnSave.Enabled = false;
                btnEdit.Enabled = false;
                gblFuction.MsgPopup("This Lead is Under Process at Operation Stage.You can not Change or Update it...");
                return;
            }
            else
            {
                //btnSave.Enabled = true;
                btnEdit.Enabled = true;
            }
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
                tbCust360.ActiveTabIndex = 0;
                //pnlCoApp1.Enabled = false;
                //pnlCoApp2.Enabled = false;
                //pnlGuarantor.Enabled = false;
                ClearControls();
                StatusButton("Add");
                btnSave.Text = "Save";

                ddlRelWithApp.SelectedIndex = ddlRelWithApp.Items.IndexOf(ddlRelWithApp.Items.FindByValue("25"));
                ddlRelWithApp.Enabled = false;
                ImgOwnshipPhoto.Enabled = false;
                imgCoApp1OwnProof.Enabled = false;
                imgCoApp2OwnProof.Enabled = false;
                imgCoApp3OwnProof.Enabled = false;
                imgCoApp4OwnProof.Enabled = false;
                imgGuarOwnProof.Enabled = false;

                txtApplAgeMaturity.Text = "0";
                txtApplAgeMaturity.Enabled = false;
                txtCoApp1AgeAtLoanMaturity.Text = "0";
                txtCoApp1AgeAtLoanMaturity.Enabled = false;
                txtCoApp2AgeAtLoanMaturity.Text = "0";
                txtCoApp2AgeAtLoanMaturity.Enabled = false;
                txtCoApp3AgeAtLoanMaturity.Text = "0";
                txtCoApp3AgeAtLoanMaturity.Enabled = false;
                txtCoApp4AgeAtLoanMaturity.Text = "0";
                txtCoApp4AgeAtLoanMaturity.Enabled = false;
                txtGuarAgeAtLoanMaturity.Text = "0";
                txtGuarAgeAtLoanMaturity.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {

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
                //ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
                if (tbCust360.ActiveTabIndex == 0)
                {
                    btnSave.Text = "Save Next";
                    pnlCoApp1.Enabled = true;
                    pnlCoApp2.Enabled = true;
                    pnlCoApp3.Enabled = true;
                    pnlCoApp4.Enabled = true;
                    pnlGuarantor.Enabled = true;
                }
                ddlCustomer.Enabled = false;
                //txtCustName.Enabled = false;
                //txtLeadGenDt.Enabled = false;
                //txtMobNo.Enabled = false;

                txtApplAgeMaturity.Text = "0";
                txtApplAgeMaturity.Enabled = false;

                txtCoApp1AgeAtLoanMaturity.Text = "0";
                txtCoApp1AgeAtLoanMaturity.Enabled = false;

                txtCoApp2AgeAtLoanMaturity.Text = "0";
                txtCoApp2AgeAtLoanMaturity.Enabled = false;

                txtCoApp3AgeAtLoanMaturity.Text = "0";
                txtCoApp3AgeAtLoanMaturity.Enabled = false;

                txtCoApp4AgeAtLoanMaturity.Text = "0";
                txtCoApp4AgeAtLoanMaturity.Enabled = false;

                txtGuarAgeAtLoanMaturity.Text = "0";
                txtGuarAgeAtLoanMaturity.Enabled = false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbCust360.ActiveTabIndex = 0;
            //  ClearControls();
            // EnableControl(false);
            StatusButton("Show");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
              
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                //LoadLeadList(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                GetMemberDtl();
                int activeTabIndex;
                if (int.TryParse(hdActiveTab.Value, out activeTabIndex))
                {
                    tbCust360.ActiveTabIndex = activeTabIndex;
                }
                
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false; Int32 vErr = 0;
            string vNewId = "", vNewMemNo = ""; string pNewCoApplId = "", pNewCoApplNo = "";
            string vAppOwnShipUplYN = "", vBrCode = "", vErrMsg = "", vApplOwnFileName = "", vAppOwnFileExt = "", vAppOwnFileStorePath = "";
            string vCoAppOwnShipUplYN = "", vCoApplOwnFileName = "", vCoAppOwnFileExt = "", vCoAppOwnFileStorePath = ""; string vCoAppTag = "";
            string pCoAppId = ""; string pMemberID = ""; string pMemberNo = ""; Int64 pLeadId = 0; string pCoApplName = ""; string pCoApplGender = "";
            DateTime pDOB = DateTime.Now; Int32 pPreAge = 0; Int32 pAgeAtLoanMaturity = 0; string pPanNo = ""; string pAadhRefNo = ""; string pAadhMaskedNo = ""; string pVoterNo = ""; Int32 pRelWithApp = 0;
            Int32 pApplEdu = 0; string pApplMarital = ""; Int32 pNoOfFamilyMem = 0; Int32 pNoOfDependents = 0; Int32 pCaste = 0; Int32 pReligion = 0; string pMinorityYN = "";
            string pEmail = ""; string pPerAdd = ""; Int32 pPerDist = 0; Int32 pPerState = 0; string pPerPin = ""; string pCurrAdd = ""; Int32 pCurrDist = 0; Int32 pCurrState = 0;
            string pCurrPin = ""; string pLandmark = ""; Int32 pOwnShipStat = 0; string pResiStabYrs = "";
            string pCoAppPanVerifyYN = "N", pCoAppAadhVerifyYN = "N", pCoAppVoterVerifyYN = "N", vCoAppMob = ""; string vErrDdupMsg = "";
            FileUpload vComFileUploader = null; CCust360 oMem = null; Int32 vMaxFileSize = Convert.ToInt32(MaxFileSize);
            string SelectedState = "";
            string SelectedDist = "";
            string SelectedCurState = "";
            string SelectedCurDist = ""; hdActiveTab.Value = "0";
            try
            {

                if (tbCust360.ActiveTabIndex == 0) hdActiveTab.Value = "0";
                else if (tbCust360.ActiveTabIndex == 1) hdActiveTab.Value = "1";
                else if (tbCust360.ActiveTabIndex == 2) hdActiveTab.Value = "2";
                else if (tbCust360.ActiveTabIndex == 3) hdActiveTab.Value = "3";
                else if (tbCust360.ActiveTabIndex == 4) hdActiveTab.Value = "4";
                else if (tbCust360.ActiveTabIndex == 5) hdActiveTab.Value = "5";
             

                #region Tab1
                if (tbCust360.ActiveTabIndex == 1)
                {
                    vCoAppTag = "CA1";
                    vCoAppOwnShipUplYN = fuCoApp1CurrAddOwnPrf.HasFile == true ? "Y" : "N";
                    vComFileUploader = fuCoApp1CurrAddOwnPrf;
                    if (vCoAppOwnShipUplYN == "Y")
                    {
                        vCoApplOwnFileName = fuCoApp1CurrAddOwnPrf.HasFile == true ? fuCoApp1CurrAddOwnPrf.PostedFile.FileName.ToString() : "";
                        vCoAppOwnFileExt = System.IO.Path.GetExtension(fuCoApp1CurrAddOwnPrf.PostedFile.FileName);
                        vCoAppOwnFileStorePath = BucketURL + CFDocumentBucket;
                    }
                    else if (hdnCoApp1OwnShipExt.Value != "")
                    {
                        vCoAppOwnFileExt = hdnCoApp1OwnShipExt.Value;
                        vCoAppOwnShipUplYN = "Y";
                        vCoAppOwnFileStorePath = BucketURL + CFDocumentBucket;
                    }

                    pCoAppId = hdnCoApp1Id.Value;
                    pMemberID = hdnAppId.Value;
                    pMemberNo = txtCustId.Text;
                    pLeadId = Convert.ToInt64(hdnLeadId.Value);
                    pCoApplName = txtCoApp1Name.Text;
                    pCoApplGender = ddlCoApp1Gender.SelectedValue;
                    pDOB = gblFuction.setDate(txtCoApp1Dob.Text.ToString());
                    pPreAge = Convert.ToInt32(txtCoApp1Age.Text);
                    pAgeAtLoanMaturity = Convert.ToInt32(txtCoApp1AgeAtLoanMaturity.Text);
                    pPanNo = txtCoApp1Pan.Text;
                    pAadhRefNo = txtCoApp1AadhRefNo.Text;
                    pVoterNo = txtCoApp1Voter.Text;
                    pRelWithApp = Convert.ToInt32(ddlCoApp1RelWithApp.SelectedValue);
                    pApplEdu = Convert.ToInt32(ddlCoApp1Edu.SelectedValue);
                    pApplMarital = ddlCoApp1Marital.SelectedValue;
                    pNoOfFamilyMem = Convert.ToInt32(txtCoApp1NoOfFamily.Text);
                    pNoOfDependents = Convert.ToInt32(txtCoApp1NoOfDependents.Text);
                    pCaste = Convert.ToInt32(ddlCoApp1Caste.SelectedValue);
                    pReligion = Convert.ToInt32(ddlCoApp1Religion.SelectedValue);
                    pMinorityYN = ddlCoApp1Minority.SelectedValue;
                    pEmail = txtCoApp1Email.Text;
                    pPerAdd = txtCoApp1PerResiAdd.Text;

                    pPerPin = txtCoApp1PerPin.Text;
                    pCurrAdd = txtCoApp1CurrAdd.Text;

                    if (ddlCoApp1PerState.SelectedValue == "" || ddlCoApp1PerState.SelectedValue == "-1") pPerState = Convert.ToInt32(hdnCoApp1PerState.Value); else pPerState = Convert.ToInt32(ddlCoApp1PerState.SelectedValue);
                    if (ddlCoApp1PerDist.SelectedValue == "" || ddlCoApp1PerDist.SelectedValue == "-1") pPerDist = Convert.ToInt32(hdnCoApp1PerDist.Value); else pPerDist = Convert.ToInt32(ddlCoApp1PerDist.SelectedValue);

                    if (ddlCoApp1CurrState.SelectedValue == "" || ddlCoApp1CurrState.SelectedValue == "-1") pCurrState = Convert.ToInt32(hdnCoApp1CurrState.Value); else pCurrState = Convert.ToInt32(ddlCoApp1CurrState.SelectedValue);
                    if (ddlCoApp1CurrDist.SelectedValue == "" || ddlCoApp1CurrDist.SelectedValue == "-1") pCurrDist = Convert.ToInt32(hdnCoApp1CurrDist.Value); else pCurrDist = Convert.ToInt32(ddlCoApp1CurrDist.SelectedValue);


                    pCurrPin = txtCoApp1CurrPin.Text;
                    pLandmark = txtCoApp1Landmark.Text;
                    pOwnShipStat = Convert.ToInt32(ddlCoApp1OwnShip.SelectedValue);
                    pResiStabYrs = ddlCoApp1ResiStabYrs.SelectedValue;
                    pCoAppPanVerifyYN = hdnCoApp1PanVerifyYN.Value;
                    pCoAppAadhVerifyYN = hdnCoApp1AadhVerifyYN.Value;
                    pCoAppVoterVerifyYN = hdnCoApp1VoterVerifyYN.Value;
                    vCoAppMob = txtCoApp1Mob.Text;
                    pAadhMaskedNo = txtCoApp1AadhNo.Text;

                }
                #endregion

                #region Tab2
                else if (tbCust360.ActiveTabIndex == 2)
                {
                    vCoAppTag = "CA2";
                    vCoAppOwnShipUplYN = fuCoApp2OwnProof.HasFile == true ? "Y" : "N";
                    vComFileUploader = fuCoApp2OwnProof;
                    if (vCoAppOwnShipUplYN == "Y")
                    {
                        vCoApplOwnFileName = fuCoApp2OwnProof.HasFile == true ? fuCoApp2OwnProof.PostedFile.FileName.ToString() : "";
                        vCoAppOwnFileExt = System.IO.Path.GetExtension(fuCoApp2OwnProof.PostedFile.FileName);
                        vCoAppOwnFileStorePath = BucketURL + CFDocumentBucket;
                    }
                    else if (hdnCoApp2OwnShipExt.Value != "")
                    {
                        vCoAppOwnFileExt = hdnCoApp2OwnShipExt.Value;
                        vCoAppOwnShipUplYN = "Y";
                        vCoAppOwnFileStorePath = BucketURL + CFDocumentBucket;
                    }

                    pCoAppId = hdnCoApp2CustId.Value;
                    pMemberID = hdnAppId.Value;
                    pMemberNo = txtCustId.Text;
                    pLeadId = Convert.ToInt64(hdnLeadId.Value);
                    pCoApplName = txtCoApp2CustName.Text;
                    pCoApplGender = ddlCoApp2Gender.SelectedValue;
                    pDOB = gblFuction.setDate(txtCoApp2Dob.Text.ToString());
                    pPreAge = Convert.ToInt32(txtCoApp2Age.Text);
                    pAgeAtLoanMaturity = Convert.ToInt32(txtCoApp2AgeAtLoanMaturity.Text);
                    pPanNo = txtCoApp2Pan.Text;
                    pAadhRefNo = txtCoApp2AadhRefNo.Text;
                    pVoterNo = txtCoApp2Voter.Text;
                    pRelWithApp = Convert.ToInt32(ddlCoApp2RelWithApp.SelectedValue);
                    pApplEdu = Convert.ToInt32(ddlCoApp2Edu.SelectedValue);
                    pApplMarital = ddlCoApp2Marital.SelectedValue;
                    pNoOfFamilyMem = Convert.ToInt32(txtCoApp2NoOfFamily.Text);
                    pNoOfDependents = Convert.ToInt32(txtCoApp2NoOfDependents.Text);
                    pCaste = Convert.ToInt32(ddlCoApp2Caste.SelectedValue);
                    pReligion = Convert.ToInt32(ddlCoApp2Religion.SelectedValue);
                    pMinorityYN = ddlCoApp2Minority.SelectedValue;
                    pEmail = txtCoApp2Email.Text;
                    pPerAdd = txtCoApp2PerResiAdd.Text;

                    if (ddlCoApp2PerState.SelectedValue == "" || ddlCoApp2PerState.SelectedValue == "-1") pPerState = Convert.ToInt32(hdnCoApp2PerState.Value); else pPerState = Convert.ToInt32(ddlCoApp2PerState.SelectedValue);
                    if (ddlCoApp2PerDist.SelectedValue == "" || ddlCoApp2PerDist.SelectedValue == "-1") pPerDist = Convert.ToInt32(hdnCoApp2PerDist.Value); else pPerDist = Convert.ToInt32(ddlCoApp2PerDist.SelectedValue);

                    if (ddlCoApp2CurrState.SelectedValue == "" || ddlCoApp2CurrState.SelectedValue == "-1") pCurrState = Convert.ToInt32(hdnCoApp2CurrState.Value); else pCurrState = Convert.ToInt32(ddlCoApp2CurrState.SelectedValue);
                    if (ddlCoApp2CurrDist.SelectedValue == "" || ddlCoApp2CurrDist.SelectedValue == "-1") pCurrDist = Convert.ToInt32(hdnCoApp2CurrDist.Value); else pCurrDist = Convert.ToInt32(ddlCoApp2CurrDist.SelectedValue);

                    pPerPin = txtCoApp2PerPin.Text;
                    pCurrAdd = txtCoApp2CurrResiAdd.Text;


                    pCurrPin = txtCoApp2CurrPin.Text;
                    pLandmark = txtCoApp2Landmark.Text;
                    pOwnShipStat = Convert.ToInt32(ddlCoApp2Ownship.SelectedValue);
                    pResiStabYrs = ddlCoApp2ResiStabYrs.SelectedValue;
                    pCoAppPanVerifyYN = hdnCoApp2PanVerifyYN.Value;
                    pCoAppAadhVerifyYN = hdnCoApp2AadhVerifyYN.Value;
                    pCoAppVoterVerifyYN = hdnCoApp2VoterVerifyYN.Value;
                    vCoAppMob = txtCoApp2Mob.Text;
                    pAadhMaskedNo = txtCoApp2AadhNo.Text;
                }
                #endregion

                #region Tab3
                else if (tbCust360.ActiveTabIndex == 3)
                {
                    vCoAppTag = "CA3";
                    vCoAppOwnShipUplYN = fuCoApp3OwnProof.HasFile == true ? "Y" : "N";
                    vComFileUploader = fuCoApp3OwnProof;
                    if (vCoAppOwnShipUplYN == "Y")
                    {
                        vCoApplOwnFileName = fuCoApp3OwnProof.HasFile == true ? fuCoApp3OwnProof.PostedFile.FileName.ToString() : "";
                        vCoAppOwnFileExt = System.IO.Path.GetExtension(fuCoApp3OwnProof.PostedFile.FileName);
                        vCoAppOwnFileStorePath = BucketURL + CFDocumentBucket;
                    }
                    else if (hdnCoApp3OwnShipExt.Value != "")
                    {
                        vCoAppOwnFileExt = hdnCoApp3OwnShipExt.Value;
                        vCoAppOwnShipUplYN = "Y";
                        vCoAppOwnFileStorePath = BucketURL + CFDocumentBucket;
                    }

                    pCoAppId = hdnCoApp3CustId.Value;
                    pMemberID = hdnAppId.Value;
                    pMemberNo = txtCustId.Text;
                    pLeadId = Convert.ToInt64(hdnLeadId.Value);
                    pCoApplName = txtCoApp3CustName.Text;
                    pCoApplGender = ddlCoApp3Gender.SelectedValue;
                    pDOB = gblFuction.setDate(txtCoApp3Dob.Text.ToString());
                    pPreAge = Convert.ToInt32(txtCoApp3Age.Text);
                    pAgeAtLoanMaturity = Convert.ToInt32(txtCoApp3AgeAtLoanMaturity.Text);
                    pPanNo = txtCoApp3Pan.Text;
                    pAadhRefNo = txtCoApp3AadhRefNo.Text;
                    pVoterNo = txtCoApp3Voter.Text;
                    pRelWithApp = Convert.ToInt32(ddlCoApp3RelWithApp.SelectedValue);
                    pApplEdu = Convert.ToInt32(ddlCoApp3Edu.SelectedValue);
                    pApplMarital = ddlCoApp3Marital.SelectedValue;
                    pNoOfFamilyMem = Convert.ToInt32(txtCoApp3NoOfFamily.Text);
                    pNoOfDependents = Convert.ToInt32(txtCoApp3NoOfDependents.Text);
                    pCaste = Convert.ToInt32(ddlCoApp3Caste.SelectedValue);
                    pReligion = Convert.ToInt32(ddlCoApp3Religion.SelectedValue);
                    pMinorityYN = ddlCoApp3Minority.SelectedValue;
                    pEmail = txtCoApp3Email.Text;
                    pPerAdd = txtCoApp3PerResiAdd.Text;

                    if (ddlCoApp3PerState.SelectedValue == "" || ddlCoApp3PerState.SelectedValue == "-1") pPerState = Convert.ToInt32(hdnCoApp3PerState.Value); else pPerState = Convert.ToInt32(ddlCoApp3PerState.SelectedValue);
                    if (ddlCoApp3PerDist.SelectedValue == "" || ddlCoApp3PerDist.SelectedValue == "-1") pPerDist = Convert.ToInt32(hdnCoApp3PerDist.Value); else pPerDist = Convert.ToInt32(ddlCoApp3PerDist.SelectedValue);

                    if (ddlCoApp3CurrState.SelectedValue == "" || ddlCoApp3CurrState.SelectedValue == "-1") pCurrState = Convert.ToInt32(hdnCoApp3CurrState.Value); else pCurrState = Convert.ToInt32(ddlCoApp3CurrState.SelectedValue);
                    if (ddlCoApp3CurrDist.SelectedValue == "" || ddlCoApp3CurrDist.SelectedValue == "-1") pCurrDist = Convert.ToInt32(hdnCoApp3CurrDist.Value); else pCurrDist = Convert.ToInt32(ddlCoApp3CurrDist.SelectedValue);

                    pPerPin = txtCoApp3PerPin.Text;
                    pCurrAdd = txtCoApp3CurrResiAdd.Text;


                    pCurrPin = txtCoApp3CurrPin.Text;
                    pLandmark = txtCoApp3Landmark.Text;
                    pOwnShipStat = Convert.ToInt32(ddlCoApp3Ownship.SelectedValue);
                    pResiStabYrs = ddlCoApp3ResiStabYrs.SelectedValue;
                    pCoAppPanVerifyYN = hdnCoApp3PanVerifyYN.Value;
                    pCoAppAadhVerifyYN = hdnCoApp3AadhVerifyYN.Value;
                    pCoAppVoterVerifyYN = hdnCoApp3VoterVerifyYN.Value;
                    vCoAppMob = txtCoApp3Mob.Text;
                    pAadhMaskedNo = txtCoApp3AadhNo.Text;
                }
                #endregion

                #region Tab4
                else if (tbCust360.ActiveTabIndex == 4)
                {
                    vCoAppTag = "CA4";
                    vCoAppOwnShipUplYN = fuCoApp4OwnProof.HasFile == true ? "Y" : "N";
                    vComFileUploader = fuCoApp4OwnProof;
                    if (vCoAppOwnShipUplYN == "Y")
                    {
                        vCoApplOwnFileName = fuCoApp4OwnProof.HasFile == true ? fuCoApp4OwnProof.PostedFile.FileName.ToString() : "";
                        vCoAppOwnFileExt = System.IO.Path.GetExtension(fuCoApp4OwnProof.PostedFile.FileName);
                        vCoAppOwnFileStorePath = BucketURL + CFDocumentBucket;
                    }
                    else if (hdnCoApp4OwnShipExt.Value != "")
                    {
                        vCoAppOwnFileExt = hdnCoApp4OwnShipExt.Value;
                        vCoAppOwnShipUplYN = "Y";
                        vCoAppOwnFileStorePath = BucketURL + CFDocumentBucket;
                    }

                    pCoAppId = hdnCoApp4CustId.Value;
                    pMemberID = hdnAppId.Value;
                    pMemberNo = txtCustId.Text;
                    pLeadId = Convert.ToInt64(hdnLeadId.Value);
                    pCoApplName = txtCoApp4CustName.Text;
                    pCoApplGender = ddlCoApp4Gender.SelectedValue;
                    pDOB = gblFuction.setDate(txtCoApp4Dob.Text.ToString());
                    pPreAge = Convert.ToInt32(txtCoApp4Age.Text);
                    pAgeAtLoanMaturity = Convert.ToInt32(txtCoApp4AgeAtLoanMaturity.Text);
                    pPanNo = txtCoApp4Pan.Text;
                    pAadhRefNo = txtCoApp4AadhRefNo.Text;
                    pVoterNo = txtCoApp4Voter.Text;
                    pRelWithApp = Convert.ToInt32(ddlCoApp4RelWithApp.SelectedValue);
                    pApplEdu = Convert.ToInt32(ddlCoApp4Edu.SelectedValue);
                    pApplMarital = ddlCoApp4Marital.SelectedValue;
                    pNoOfFamilyMem = Convert.ToInt32(txtCoApp4NoOfFamily.Text);
                    pNoOfDependents = Convert.ToInt32(txtCoApp4NoOfDependents.Text);
                    pCaste = Convert.ToInt32(ddlCoApp4Caste.SelectedValue);
                    pReligion = Convert.ToInt32(ddlCoApp4Religion.SelectedValue);
                    pMinorityYN = ddlCoApp4Minority.SelectedValue;
                    pEmail = txtCoApp4Email.Text;
                    pPerAdd = txtCoApp4PerResiAdd.Text;

                    if (ddlCoApp4PerState.SelectedValue == "" || ddlCoApp4PerState.SelectedValue == "-1") pPerState = Convert.ToInt32(hdnCoApp4PerState.Value); else pPerState = Convert.ToInt32(ddlCoApp4PerState.SelectedValue);
                    if (ddlCoApp4PerDist.SelectedValue == "" || ddlCoApp4PerDist.SelectedValue == "-1") pPerDist = Convert.ToInt32(hdnCoApp4PerDist.Value); else pPerDist = Convert.ToInt32(ddlCoApp4PerDist.SelectedValue);

                    if (ddlCoApp4CurrState.SelectedValue == "" || ddlCoApp4CurrState.SelectedValue == "-1") pCurrState = Convert.ToInt32(hdnCoApp4CurrState.Value); else pCurrState = Convert.ToInt32(ddlCoApp4CurrState.SelectedValue);
                    if (ddlCoApp4CurrDist.SelectedValue == "" || ddlCoApp4CurrDist.SelectedValue == "-1") pCurrDist = Convert.ToInt32(hdnCoApp4CurrDist.Value); else pCurrDist = Convert.ToInt32(ddlCoApp4CurrDist.SelectedValue);

                    pPerPin = txtCoApp4PerPin.Text;
                    pCurrAdd = txtCoApp4CurrResiAdd.Text;


                    pCurrPin = txtCoApp4CurrPin.Text;
                    pLandmark = txtCoApp4Landmark.Text;
                    pOwnShipStat = Convert.ToInt32(ddlCoApp4Ownship.SelectedValue);
                    pResiStabYrs = ddlCoApp4ResiStabYrs.SelectedValue;
                    pCoAppPanVerifyYN = hdnCoApp4PanVerifyYN.Value;
                    pCoAppAadhVerifyYN = hdnCoApp4AadhVerifyYN.Value;
                    pCoAppVoterVerifyYN = hdnCoApp4VoterVerifyYN.Value;
                    vCoAppMob = txtCoApp4Mob.Text;
                    pAadhMaskedNo = txtCoApp4AadhNo.Text;
                }
                #endregion

                #region Tab5
                else if (tbCust360.ActiveTabIndex == 5)
                {
                    vCoAppTag = "G";
                    vCoAppOwnShipUplYN = fuGuarOwnProof.HasFile == true ? "Y" : "N";
                    vComFileUploader = fuGuarOwnProof;
                    if (vCoAppOwnShipUplYN == "Y")
                    {
                        vCoApplOwnFileName = fuGuarOwnProof.HasFile == true ? fuGuarOwnProof.PostedFile.FileName.ToString() : "";
                        vCoAppOwnFileExt = System.IO.Path.GetExtension(fuGuarOwnProof.PostedFile.FileName);
                        vCoAppOwnFileStorePath = BucketURL + CFDocumentBucket;
                    }
                    else if (hdnGuarOwnShipExt.Value != "")
                    {
                        vCoAppOwnFileExt = hdnGuarOwnShipExt.Value;
                        vCoAppOwnShipUplYN = "Y";
                        vCoAppOwnFileStorePath = BucketURL + CFDocumentBucket;
                    }

                    pCoAppId = hdnGuarId.Value;
                    pMemberID = hdnAppId.Value;
                    pMemberNo = txtCustId.Text;
                    pLeadId = Convert.ToInt64(hdnLeadId.Value);
                    pCoApplName = txtGuarName.Text;
                    pCoApplGender = ddlGuarGender.SelectedValue;
                    pDOB = gblFuction.setDate(txtGuarDOB.Text.ToString());
                    pPreAge = Convert.ToInt32(txtGuarAge.Text);
                    pAgeAtLoanMaturity = Convert.ToInt32(txtGuarAgeAtLoanMaturity.Text);
                    pPanNo = txtGuarPan.Text;
                    pAadhRefNo = txtGuarAadhRefNo.Text;
                    pVoterNo = txtGuarVoter.Text;
                    pRelWithApp = Convert.ToInt32(ddlGuarRelWithApp.SelectedValue);
                    pApplEdu = Convert.ToInt32(ddlGuarEdu.SelectedValue);
                    pApplMarital = ddlGuarMarital.SelectedValue;
                    pNoOfFamilyMem = Convert.ToInt32(txtGuarNoOfFamily.Text);
                    pNoOfDependents = Convert.ToInt32(txtGuarNoOfDependents.Text);
                    pCaste = Convert.ToInt32(ddlGuarCaste.SelectedValue);
                    pReligion = Convert.ToInt32(ddlGuarReligion.SelectedValue);
                    pMinorityYN = ddlGuarMinority.SelectedValue;
                    pEmail = txtGuarEmail.Text;
                    pPerAdd = txtGuarPerResiAdd.Text;

                    pPerPin = txtGuarPerPin.Text;
                    pCurrAdd = txtGuarCurrResiAdd.Text;

                    if (ddlGuarPerState.SelectedValue == "" || ddlGuarPerState.SelectedValue == "-1") pPerState = Convert.ToInt32(hdnGuarPerState.Value); else pPerState = Convert.ToInt32(ddlGuarPerState.SelectedValue);
                    if (ddlGuarPerDist.SelectedValue == "" || ddlGuarPerDist.SelectedValue == "-1") pPerDist = Convert.ToInt32(hdnGuarPerDist.Value); else pPerDist = Convert.ToInt32(ddlGuarPerDist.SelectedValue);

                    if (ddlGuarCurrState.SelectedValue == "" || ddlGuarCurrState.SelectedValue == "-1") pCurrState = Convert.ToInt32(hdnGuarCurrState.Value); else pCurrState = Convert.ToInt32(ddlGuarCurrState.SelectedValue);
                    if (ddlGuarCurrDist.SelectedValue == "" || ddlGuarCurrDist.SelectedValue == "-1") pCurrDist = Convert.ToInt32(hdnGuarCurrDist.Value); else pCurrDist = Convert.ToInt32(ddlGuarCurrDist.SelectedValue);

                    pCurrPin = txtGuarCurrPin.Text;
                    pLandmark = txtGuarLandmark.Text;
                    pOwnShipStat = Convert.ToInt32(ddlGuarOwnStat.SelectedValue);
                    pResiStabYrs = ddlGuarResiStabYrs.SelectedValue;
                    pCoAppPanVerifyYN = hdnGuarPanVerifyYN.Value;
                    pCoAppAadhVerifyYN = hdnGuarAadhVerifyYN.Value;
                    pCoAppVoterVerifyYN = hdnGuarVoterVerifyYN.Value;
                    vCoAppMob = txtGuarMob.Text;
                    pAadhMaskedNo = txtGuarAadhNo.Text;
                }
                #endregion

                vBrCode = Session[gblValue.BrnchCode].ToString();
                if (Mode == "Save")
                {
                    vAppOwnShipUplYN = fuOwnProof.HasFile == true ? "Y" : "N";
                    if (vAppOwnShipUplYN == "Y")
                    {
                        vApplOwnFileName = fuOwnProof.HasFile == true ? fuOwnProof.PostedFile.FileName.ToString() : "";
                        vAppOwnFileExt = System.IO.Path.GetExtension(fuOwnProof.PostedFile.FileName);
                        vAppOwnFileStorePath = BucketURL + CFDocumentBucket;

                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vAppOwnFileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuOwnProof.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuOwnProof.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                    }
                    else if (hdnAppOwnshipExt.Value != "")
                    {
                        vAppOwnFileExt = hdnAppOwnshipExt.Value;
                        vAppOwnShipUplYN = "Y";
                        vAppOwnFileStorePath = BucketURL + CFDocumentBucket;
                    }


                    oMem = new CCust360();

                    vErr = oMem.CF_chkDDup(Convert.ToInt64(hdnLeadId.Value), txtApplPan.Text, txtApplAadhRefNo.Text, txtApplVoterNo.Text, txtAppMob.Text, ref vErrDdupMsg);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(vErrDdupMsg);
                        return false;
                    }

                    SelectedState = hdnApplState.Value;
                    SelectedDist = hdnApplDist.Value;
                    SelectedCurState = hdnApplCurrState.Value;
                    SelectedCurDist = hdnApplCurrDist.Value;

                    // txtApplAadhRefNo.Text = "6354981631880468";

                    /// <Summery>
                    /// Inserting data for Applicant
                    /// </Summery>
                    vErr = oMem.CF_SaveApplicant(ref vNewId, hdnAppId.Value, Convert.ToInt64(ddlCustomer.SelectedValue), ref vNewMemNo, txtCustName.Text,
                        ddlApplGender.SelectedValue, gblFuction.setDate(txtApplDOB.Text.ToString()), Convert.ToInt32(txtApplAge.Text), Convert.ToInt32(txtApplAgeMaturity.Text),
                        txtApplPan.Text, txtApplAadhRefNo.Text, txtApplVoterNo.Text, Convert.ToInt32(ddlRelWithApp.SelectedValue), Convert.ToInt32(ddlAppEdu.SelectedValue),
                        ddlApplMaritalStatus.Text, Convert.ToInt32(txtNoOfFamilyMem.Text), Convert.ToInt32(txtNoOfDependent.Text), Convert.ToInt32(ddlApplCast.SelectedValue),
                        Convert.ToInt32(ddlApplReligion.SelectedValue), ddlAppMinorityYN.SelectedValue, txtApplEmail.Text, txtAppPerAddress.Text,
                        Convert.ToInt32(SelectedDist), Convert.ToInt32(SelectedState), txtApplPin.Text, txtApplCurrAddress.Text,
                        Convert.ToInt32(SelectedCurDist), Convert.ToInt32(SelectedCurState),
                        txtApplCurrPin.Text, txtApplLandMark.Text, Convert.ToInt32(ddlApplOwnshipStatus.SelectedValue), vAppOwnShipUplYN, vAppOwnFileStorePath,
                        ddlResiStabYrs.SelectedValue, vBrCode, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save", hdnAppPanVerifyYN.Value,
                        hdnAppAadhVerifyYN.Value, hdnAppVoterVerifyYN.Value, txtAppMob.Text, vAppOwnFileExt, txtApplAadhNo.Text);
                    if (vErr > 0)
                    {
                        hdnAppId.Value = Convert.ToString(vNewId);
                        Session[gblValue.MemberID] = Convert.ToString(vNewId);
                        ViewState["MemberId"] = vNewId;
                        if (fuOwnProof.HasFile)
                        {
                            if (vAppOwnFileExt.ToLower().Contains(".pdf"))
                            {
                                SaveMemberImages(fuOwnProof, hdnLeadId.Value, hdnLeadId.Value + "_A_" + "ApplOwnship", vAppOwnFileExt, "N");
                            }
                            else
                            {
                                SaveMemberImages(fuOwnProof, hdnLeadId.Value, "A_" + "ApplOwnship", vAppOwnFileExt, "N");
                            }
                        }
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    if (tbCust360.ActiveTabIndex == 0) /// EDIT FOR APPLICANT
                    {
                        vAppOwnShipUplYN = fuOwnProof.HasFile == true ? "Y" : "N";
                        if (vAppOwnShipUplYN == "Y")
                        {
                            vApplOwnFileName = fuOwnProof.HasFile == true ? fuOwnProof.PostedFile.FileName.ToString() : "";
                            vAppOwnFileExt = System.IO.Path.GetExtension(fuOwnProof.PostedFile.FileName);
                            vAppOwnFileStorePath = BucketURL + CFDocumentBucket;

                            //---------------------------------------------------------------
                            bool ValidPdf = false;
                            if (vAppOwnFileExt.ToLower() == ".pdf")
                            {
                                cFileValidate oFile = new cFileValidate();
                                ValidPdf = oFile.ValidatePdf(fuOwnProof.FileBytes);
                                if (ValidPdf == false)
                                {
                                    gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                    fuOwnProof.Focus();
                                    return false;
                                }
                            }
                            //------------------------------------------------------------
                        }
                        else if (hdnAppOwnshipExt.Value != "")
                        {
                            vAppOwnFileExt = hdnAppOwnshipExt.Value;
                            vAppOwnShipUplYN = "Y";
                            vAppOwnFileStorePath = BucketURL + CFDocumentBucket;
                        }

                        if (ddlApplState.SelectedValue == "" || ddlApplState.SelectedValue == "-1") SelectedState = hdnApplState.Value; else SelectedState = ddlApplState.SelectedValue;
                        if (ddlApplDist.SelectedValue == "" || ddlApplDist.SelectedValue == "-1") SelectedDist = hdnApplDist.Value; else SelectedDist = ddlApplDist.SelectedValue;
                        if (ddlApplCurrState.SelectedValue == "" || ddlApplCurrState.SelectedValue == "-1") SelectedCurState = hdnApplCurrState.Value; else SelectedCurState = ddlApplCurrState.SelectedValue;
                        if (ddlApplCurrDist.SelectedValue == "" || ddlApplCurrDist.SelectedValue == "-1") SelectedCurDist = hdnApplCurrDist.Value; else SelectedCurDist = ddlApplCurrDist.SelectedValue;

                        oMem = new CCust360();
                        //txtApplAadhRefNo.Text = "6354981631880468";

                        /// <Summery>
                        /// Updating data for Applicant
                        /// 
                        /// </Summery>
                        vErr = oMem.CF_SaveApplicant(ref vNewId, hdnAppId.Value, Convert.ToInt64(hdnLeadId.Value), ref vNewMemNo, txtCustName.Text,
                            ddlApplGender.SelectedValue, gblFuction.setDate(txtApplDOB.Text.ToString()), Convert.ToInt32(txtApplAge.Text), Convert.ToInt32(txtApplAgeMaturity.Text),
                            txtApplPan.Text, txtApplAadhRefNo.Text, txtApplVoterNo.Text, Convert.ToInt32(ddlRelWithApp.SelectedValue), Convert.ToInt32(ddlAppEdu.SelectedValue),
                            ddlApplMaritalStatus.Text, Convert.ToInt32(txtNoOfFamilyMem.Text), Convert.ToInt32(txtNoOfDependent.Text), Convert.ToInt32(ddlApplCast.SelectedValue),
                            Convert.ToInt32(ddlApplReligion.SelectedValue), ddlAppMinorityYN.SelectedValue, txtApplEmail.Text, txtAppPerAddress.Text,
                            Convert.ToInt32(SelectedDist), Convert.ToInt32(SelectedState), txtApplPin.Text, txtApplCurrAddress.Text,
                            Convert.ToInt32(SelectedCurDist), Convert.ToInt32(SelectedCurState),
                            txtApplCurrPin.Text, txtApplLandMark.Text, Convert.ToInt32(ddlApplOwnshipStatus.SelectedValue), vAppOwnShipUplYN, vAppOwnFileStorePath,
                            ddlResiStabYrs.SelectedValue, vBrCode, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Edit", hdnAppPanVerifyYN.Value,
                            hdnAppAadhVerifyYN.Value, hdnAppVoterVerifyYN.Value, txtAppMob.Text, vAppOwnFileExt, txtApplAadhNo.Text);
                        if (vErr > 0)
                        {
                            hdnAppId.Value = Convert.ToString(vNewId);
                            ViewState["MemberId"] = vNewId;
                            if (fuOwnProof.HasFile)
                            {
                                if (vAppOwnFileExt.ToLower().Contains(".pdf"))
                                {
                                    SaveMemberImages(fuOwnProof, hdnLeadId.Value, hdnLeadId.Value + "_A_" + "ApplOwnship", vAppOwnFileExt, "N");
                                }
                                else
                                {
                                    SaveMemberImages(fuOwnProof, hdnLeadId.Value, "A_" + "ApplOwnship", vAppOwnFileExt, "N");
                                }

                            }
                            gblFuction.MsgPopup(gblPRATAM.EditMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblPRATAM.DBError);
                            vResult = false;
                        }
                    }
                    else if (tbCust360.ActiveTabIndex != 0)
                    {

                        oMem = new CCust360();
                        vErr = oMem.CF_chkDDup(Convert.ToInt64(hdnLeadId.Value), pPanNo, pAadhRefNo, pVoterNo, vCoAppMob, ref vErrDdupMsg);
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup(vErrDdupMsg);
                            return false;
                        }

                        oMem = new CCust360();
                        /// <Summery>
                        /// Inserting data for Co-App1,Co-App2,Co-App3,
                        /// Co-App4,Guarantor
                        /// </Summery>
                        //pAadhRefNo = "6354981631880468";
                        vErr = oMem.CF_SaveCoApplicant(ref pNewCoApplId, ref pNewCoApplNo, pCoAppId, pMemberID, pMemberNo, pLeadId, pCoApplName,
                            pCoApplGender, pDOB, pPreAge, pAgeAtLoanMaturity, pPanNo, pAadhRefNo, pVoterNo, pRelWithApp, pApplEdu,
                            pApplMarital, pNoOfFamilyMem, pNoOfDependents, pCaste, pReligion, pMinorityYN, pEmail, pPerAdd,
                            pPerDist, pPerState, pPerPin, pCurrAdd, pCurrDist, pCurrState,
                            pCurrPin, pLandmark, pOwnShipStat, vCoAppOwnShipUplYN, vCoAppOwnFileStorePath,
                            pResiStabYrs, vCoAppTag, vBrCode, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Edit",
                            pCoAppPanVerifyYN, pCoAppAadhVerifyYN, pCoAppVoterVerifyYN, vCoAppMob, vCoAppOwnFileExt, pAadhMaskedNo);
                        if (vErr > 0)
                        {
                            hdnAppId.Value = Convert.ToString(pNewCoApplId);
                            ViewState["CoApplId"] = pNewCoApplId;
                            if (vComFileUploader.HasFile)
                            {
                                if (vCoAppOwnFileExt.ToLower().Contains(".pdf"))
                                {
                                    SaveMemberImages(vComFileUploader, hdnLeadId.Value, hdnLeadId.Value + "_" + vCoAppTag + "_OwnshipProof", vCoAppOwnFileExt, "N");
                                }
                                else
                                {
                                    SaveMemberImages(vComFileUploader, hdnLeadId.Value, vCoAppTag + "_OwnshipProof", vCoAppOwnFileExt, "N");
                                }
                            }
                            gblFuction.MsgPopup(gblPRATAM.EditMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblPRATAM.DBError);
                            vResult = false;
                        }
                    }
                }
                else // DELETE CUST360
                {

                    oMem = new CCust360();
                    vErr = oMem.CF_SaveApplicant(ref vNewId, hdnAppId.Value, Convert.ToInt32(hdnLeadId.Value), ref vNewMemNo, txtCustName.Text,
                        ddlApplGender.SelectedValue, gblFuction.setDate(txtApplDOB.Text.ToString()), Convert.ToInt32(txtApplAge.Text), Convert.ToInt32(txtApplAgeMaturity.Text),
                        txtApplPan.Text, txtApplAadhRefNo.Text, txtApplVoterNo.Text, Convert.ToInt32(ddlRelWithApp.SelectedValue), Convert.ToInt32(ddlAppEdu.SelectedValue),
                        ddlApplMaritalStatus.Text, Convert.ToInt32(txtNoOfFamilyMem.Text), Convert.ToInt32(txtNoOfDependent.Text), Convert.ToInt32(ddlApplCast.SelectedValue),
                        Convert.ToInt32(ddlApplReligion.SelectedValue), ddlAppMinorityYN.SelectedValue, txtApplEmail.Text, txtAppPerAddress.Text,
                        Convert.ToInt32(ddlApplDist.SelectedValue), Convert.ToInt32(ddlApplState.SelectedValue), txtApplPin.Text, txtApplCurrAddress.Text,
                        Convert.ToInt32(ddlApplCurrDist.SelectedValue), Convert.ToInt32(ddlApplCurrState.SelectedValue),
                        txtApplCurrPin.Text, txtApplLandMark.Text, Convert.ToInt32(ddlApplOwnshipStatus.SelectedValue), vAppOwnShipUplYN, vAppOwnFileStorePath,
                        ddlResiStabYrs.SelectedValue, vBrCode, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete", hdnAppPanVerifyYN.Value,
                        hdnAppAadhVerifyYN.Value, hdnAppVoterVerifyYN.Value, txtAppMob.Text, vAppOwnFileExt, txtApplAadhNo.Text);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
            }
        }        
        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vBrCode = "";
            DataTable dt, dt1 = null;
            CMember oMem = null;
            vBrCode = Session[gblValue.BrnchCode].ToString();
            hdnLeadId.Value = ddlCustomer.SelectedValue;
            dt = new DataTable();
            oMem = new CMember();
            dt = oMem.CF_GetCustNameByLeadID(Convert.ToInt32(ddlCustomer.SelectedValue), vBrCode);
            if (dt.Rows.Count > 0)
            {
                txtCustName.Text = Convert.ToString(dt.Rows[0]["ApplName"]);
                txtAppMob.Text = Convert.ToString(dt.Rows[0]["MobNo"]);
            }
            else
            {
                txtCustName.Text = "";
                txtAppMob.Text = "";
            }

        }
        protected void GetMemberDtl()
        {
            string pMemberId = "";
            string vBrCode = "";
            DataTable dt, dt1, dt2, dt3, dt4, dt5 = null;
            DataSet ds = new DataSet();
            ClearControls();
            if (Session[gblValue.ApplNm] != null)
            {
                lblApplNm1.Text = Convert.ToString(Session[gblValue.ApplNm]);
                lblApplNm2.Text = Convert.ToString(Session[gblValue.ApplNm]);
                lblApplNm3.Text = Convert.ToString(Session[gblValue.ApplNm]);
                lblApplNm4.Text = Convert.ToString(Session[gblValue.ApplNm]);
                lblApplNm5.Text = Convert.ToString(Session[gblValue.ApplNm]);
            }
            if (Session[gblValue.BCPNO] != null)
            {
                lblBCPNum1.Text = Convert.ToString(Session[gblValue.BCPNO]);
                lblBCPNum2.Text = Convert.ToString(Session[gblValue.BCPNO]);
                lblBCPNum3.Text = Convert.ToString(Session[gblValue.BCPNO]);
                lblBCPNum4.Text = Convert.ToString(Session[gblValue.BCPNO]);
                lblBCPNum5.Text = Convert.ToString(Session[gblValue.BCPNO]);
            }
            try
            {
                if (Session[gblValue.MemberID] != null)
                {
                    pMemberId = Convert.ToString(Session[gblValue.MemberID]);
                }
                if (Session[gblValue.LeadID] != null)
                {
                    hdnLeadId.Value = Convert.ToString(Session[gblValue.LeadID]);
                }
                if (Session[gblValue.MobNo] != null)
                {
                    hdMobNo.Value = Convert.ToString(Session[gblValue.MobNo]);
                }
                vBrCode = Session[gblValue.BrnchCode].ToString();
                ViewState["MemberId"] = pMemberId;


                CCust360 oC360 = new CCust360();
                ds = oC360.CF_GetMemberDtl(pMemberId);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                dt2 = ds.Tables[2];
                dt3 = ds.Tables[3];
                dt4 = ds.Tables[4];
                dt5 = ds.Tables[5];

                if (dt.Rows.Count > 0)
                {

                    ViewState["StateEdit"] = "Edit";

                    ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(hdnLeadId.Value));

                    hdnLeadId.Value = dt.Rows[0]["LeadID"].ToString();
                    hdnAppId.Value = Convert.ToString(dt.Rows[0]["MemberID"]);
                    txtCustId.Text = Convert.ToString(dt.Rows[0]["MemberNo"]);
                    txtCustName.Text = Convert.ToString(dt.Rows[0]["MemberName"]);
                    ddlApplGender.SelectedIndex = ddlApplGender.Items.IndexOf(ddlApplGender.Items.FindByValue(dt.Rows[0]["Gender"].ToString()));
                    txtApplDOB.Text = Convert.ToString(dt.Rows[0]["DoB"]);
                    txtApplAge.Text = Convert.ToString(dt.Rows[0]["PresentAge"]);
                    txtApplAgeMaturity.Text = Convert.ToString(dt.Rows[0]["AgeAtLoanMaturity"]);
                    txtApplPan.Text = Convert.ToString(dt.Rows[0]["PanNo"]);
                    txtApplAadhRefNo.Text = Convert.ToString(dt.Rows[0]["AadhRefNo"]);
                    txtApplAadhNo.Text = Convert.ToString(dt.Rows[0]["AadhMaskedNo"]);

                    txtApplVoterNo.Text = Convert.ToString(dt.Rows[0]["VoterID"]);
                    ddlRelWithApp.SelectedIndex = ddlRelWithApp.Items.IndexOf(ddlRelWithApp.Items.FindByValue(dt.Rows[0]["RelWithApp"].ToString()));
                    txtAppMob.Text = Convert.ToString(dt.Rows[0]["MobNo"]);
                    ddlAppEdu.SelectedIndex = ddlAppEdu.Items.IndexOf(ddlAppEdu.Items.FindByValue(dt.Rows[0]["Education"].ToString()));
                    ddlApplMaritalStatus.SelectedIndex = ddlApplMaritalStatus.Items.IndexOf(ddlApplMaritalStatus.Items.FindByValue(dt.Rows[0]["MaritalStatus"].ToString()));
                    txtNoOfFamilyMem.Text = Convert.ToString(dt.Rows[0]["TotalNoFamMem"]);
                    txtNoOfDependent.Text = Convert.ToString(dt.Rows[0]["NoOfDependents"]);
                    ddlApplCast.SelectedIndex = ddlApplCast.Items.IndexOf(ddlApplCast.Items.FindByValue(dt.Rows[0]["Caste"].ToString()));
                    ddlApplReligion.SelectedIndex = ddlApplReligion.Items.IndexOf(ddlApplReligion.Items.FindByValue(dt.Rows[0]["Religion"].ToString()));
                    ddlAppMinorityYN.SelectedIndex = ddlAppMinorityYN.Items.IndexOf(ddlAppMinorityYN.Items.FindByValue(dt.Rows[0]["Minority"].ToString()));
                    txtApplEmail.Text = Convert.ToString(dt.Rows[0]["Email"]);
                    txtAppPerAddress.Text = Convert.ToString(dt.Rows[0]["PerAdd"]);


                    ddlApplState.SelectedIndex = ddlApplState.Items.IndexOf(ddlApplState.Items.FindByValue(dt.Rows[0]["PerState"].ToString()));
                    PopDistrictByState(Convert.ToInt32(ddlApplState.SelectedValue), "AppPer");
                    ddlApplDist.SelectedIndex = ddlApplDist.Items.IndexOf(ddlApplDist.Items.FindByValue(dt.Rows[0]["PerDist"].ToString()));

                    txtApplPin.Text = Convert.ToString(dt.Rows[0]["PerPin"]);
                    txtApplCurrAddress.Text = Convert.ToString(dt.Rows[0]["CurrAdd"]);

                    ddlApplCurrState.SelectedIndex = ddlApplCurrState.Items.IndexOf(ddlApplCurrState.Items.FindByValue(dt.Rows[0]["CurrState"].ToString()));
                    PopDistrictByState(Convert.ToInt32(ddlApplCurrState.SelectedValue), "AppCurr");
                    ddlApplCurrDist.SelectedIndex = ddlApplCurrDist.Items.IndexOf(ddlApplCurrDist.Items.FindByValue(dt.Rows[0]["CurrDist"].ToString()));

                    txtApplCurrPin.Text = Convert.ToString(dt.Rows[0]["CurrPin"]);
                    txtApplLandMark.Text = Convert.ToString(dt.Rows[0]["CurrLandmark"]);
                    ddlApplOwnshipStatus.SelectedIndex = ddlApplOwnshipStatus.Items.IndexOf(ddlApplOwnshipStatus.Items.FindByValue(dt.Rows[0]["OwnShipStatus"].ToString()));
                    ddlResiStabYrs.SelectedIndex = ddlResiStabYrs.Items.IndexOf(ddlResiStabYrs.Items.FindByValue(dt.Rows[0]["ResiStabYrs"].ToString()));

                    hdnAppPanVerifyYN.Value = Convert.ToString(dt.Rows[0]["PanVerifyYN"]);
                    hdnAppAadhVerifyYN.Value = Convert.ToString(dt.Rows[0]["AadhVerifyYN"]);
                    hdnAppVoterVerifyYN.Value = Convert.ToString(dt.Rows[0]["VoterVerifyYN"]);

                    tbCust360.ActiveTabIndex = 0;
                    StatusButton("Show");

                    if (Convert.ToString(dt.Rows[0]["PanVerifyYN"]) == "Y")
                    {
                        btnVerifyAppPan.Enabled = false;
                        txtApplPan.Enabled = false;
                        hdnAppPanVerifyYN.Value = "Y";
                    }
                    if (Convert.ToString(dt.Rows[0]["AadhVerifyYN"]) == "Y")
                    {
                        btnVerifyAppAadhaar.Enabled = false;
                        txtApplAadhNo.Enabled = false;
                        hdnAppAadhVerifyYN.Value = "Y";
                    }

                    if (Convert.ToString(dt.Rows[0]["VoterVerifyYN"]) == "Y")
                    {
                        btnVerifyAppVoter.Enabled = false;
                        txtApplVoterNo.Enabled = false;
                        hdnAppVoterVerifyYN.Value = "Y";
                    }

                    hdnAppOwnshipExt.Value = Convert.ToString(dt.Rows[0]["OwnShipExt"]);
                    lblAppOwnship.Text = "ApplOwnship" + hdnAppOwnshipExt.Value;
                }
                else
                {

                    ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(hdnLeadId.Value));

                    txtCustName.Text = lblApplNm1.Text;
                    ddlRelWithApp.SelectedIndex = ddlRelWithApp.Items.IndexOf(ddlRelWithApp.Items.FindByValue("25"));
                    txtAppMob.Text = Convert.ToString(hdMobNo.Value);
                    ViewState["StateEdit"] = "Add";
                }
                if (dt1.Rows.Count > 0)
                {
                    ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                    hdnCoApp1Id.Value = Convert.ToString(dt1.Rows[0]["CoAppID"]);
                    txtCoApp1Id.Text = Convert.ToString(dt1.Rows[0]["CoAppNo"]);
                    txtCoApp1Name.Text = Convert.ToString(dt1.Rows[0]["CoAppName"]);
                    ddlCoApp1Gender.SelectedIndex = ddlCoApp1Gender.Items.IndexOf(ddlCoApp1Gender.Items.FindByValue(dt1.Rows[0]["Gender"].ToString()));
                    txtCoApp1Dob.Text = Convert.ToString(dt1.Rows[0]["DoB"]);
                    txtCoApp1Age.Text = Convert.ToString(dt1.Rows[0]["PresentAge"]);
                    txtCoApp1AgeAtLoanMaturity.Text = Convert.ToString(dt1.Rows[0]["AgeAtLoanMaturity"]);
                    txtCoApp1Pan.Text = Convert.ToString(dt1.Rows[0]["PanNo"]);
                    txtCoApp1AadhRefNo.Text = Convert.ToString(dt1.Rows[0]["AadhRefNo"]);
                    txtCoApp1AadhNo.Text = Convert.ToString(dt1.Rows[0]["AadhMaskedNo"]);
                    txtCoApp1Voter.Text = Convert.ToString(dt1.Rows[0]["VoterID"]);
                    ddlCoApp1RelWithApp.SelectedIndex = ddlRelWithApp.Items.IndexOf(ddlRelWithApp.Items.FindByValue(dt1.Rows[0]["RelWithApp"].ToString()));
                    txtCoApp1Mob.Text = Convert.ToString(dt1.Rows[0]["MobNo"]);
                    ddlCoApp1Edu.SelectedIndex = ddlCoApp1Edu.Items.IndexOf(ddlCoApp1Edu.Items.FindByValue(dt1.Rows[0]["Education"].ToString()));
                    ddlCoApp1Marital.SelectedIndex = ddlCoApp1Marital.Items.IndexOf(ddlCoApp1Marital.Items.FindByValue(dt1.Rows[0]["MaritalStatus"].ToString()));
                    txtCoApp1NoOfFamily.Text = Convert.ToString(dt1.Rows[0]["TotalNoFamMem"]);
                    txtCoApp1NoOfDependents.Text = Convert.ToString(dt1.Rows[0]["NoOfDependents"]);
                    ddlCoApp1Caste.SelectedIndex = ddlCoApp1Caste.Items.IndexOf(ddlCoApp1Caste.Items.FindByValue(dt1.Rows[0]["Caste"].ToString()));
                    ddlCoApp1Religion.SelectedIndex = ddlCoApp1Religion.Items.IndexOf(ddlCoApp1Religion.Items.FindByValue(dt1.Rows[0]["Religion"].ToString()));
                    ddlCoApp1Minority.SelectedIndex = ddlCoApp1Minority.Items.IndexOf(ddlCoApp1Minority.Items.FindByValue(dt1.Rows[0]["Minority"].ToString()));
                    txtCoApp1Email.Text = Convert.ToString(dt1.Rows[0]["Email"]);
                    txtCoApp1PerResiAdd.Text = Convert.ToString(dt1.Rows[0]["PerAdd"]);


                    ddlCoApp1PerState.SelectedIndex = ddlCoApp1PerState.Items.IndexOf(ddlCoApp1PerState.Items.FindByValue(dt1.Rows[0]["PerState"].ToString()));
                    PopDistrictByState(Convert.ToInt32(ddlCoApp1PerState.SelectedValue), "CoApp1Per");
                    ddlCoApp1PerDist.SelectedIndex = ddlCoApp1PerDist.Items.IndexOf(ddlCoApp1PerDist.Items.FindByValue(dt1.Rows[0]["PerDist"].ToString()));

                    txtCoApp1PerPin.Text = Convert.ToString(dt1.Rows[0]["PerPin"]);
                    txtCoApp1CurrAdd.Text = Convert.ToString(dt1.Rows[0]["CurrAdd"]);

                    ddlCoApp1CurrState.SelectedIndex = ddlCoApp1CurrState.Items.IndexOf(ddlCoApp1CurrState.Items.FindByValue(dt1.Rows[0]["CurrState"].ToString()));
                    PopDistrictByState(Convert.ToInt32(ddlCoApp1CurrState.SelectedValue), "CoApp1Curr");
                    ddlCoApp1CurrDist.SelectedIndex = ddlCoApp1CurrDist.Items.IndexOf(ddlCoApp1CurrDist.Items.FindByValue(dt1.Rows[0]["CurrDist"].ToString()));

                    txtCoApp1CurrPin.Text = Convert.ToString(dt1.Rows[0]["CurrPin"]);
                    txtCoApp1Landmark.Text = Convert.ToString(dt1.Rows[0]["CurrLandmark"]);
                    ddlCoApp1OwnShip.SelectedIndex = ddlCoApp1OwnShip.Items.IndexOf(ddlCoApp1OwnShip.Items.FindByValue(dt1.Rows[0]["OwnShipStatus"].ToString()));
                    ddlCoApp1ResiStabYrs.SelectedIndex = ddlCoApp1ResiStabYrs.Items.IndexOf(ddlCoApp1ResiStabYrs.Items.FindByValue(dt1.Rows[0]["ResiStabYrs"].ToString()));

                    hdnCoApp1PanVerifyYN.Value = Convert.ToString(dt1.Rows[0]["PanVerifyYN"]);
                    hdnCoApp1AadhVerifyYN.Value = Convert.ToString(dt1.Rows[0]["AadhVerifyYN"]);
                    hdnCoApp1VoterVerifyYN.Value = Convert.ToString(dt1.Rows[0]["VoterVerifyYN"]);

                    StatusButton("Show");

                    if (Convert.ToString(dt1.Rows[0]["PanVerifyYN"]) == "Y")
                    {
                        btnVerifyCoApp1Pan.Enabled = false;
                        txtCoApp1Pan.Enabled = false;
                        hdnCoApp1PanVerifyYN.Value = "Y";
                    }
                    if (Convert.ToString(dt1.Rows[0]["AadhVerifyYN"]) == "Y")
                    {
                        btnVerifyCoApp1Aadhaar.Enabled = false;
                        txtCoApp1AadhNo.Enabled = false;
                        hdnCoApp1AadhVerifyYN.Value = "Y";
                    }

                    if (Convert.ToString(dt1.Rows[0]["VoterVerifyYN"]) == "Y")
                    {
                        btnVerifyCoApp1Voter.Enabled = false;
                        txtCoApp1Voter.Enabled = false;
                        hdnCoApp1VoterVerifyYN.Value = "Y";
                    }

                    hdnCoApp1OwnShipExt.Value = Convert.ToString(dt1.Rows[0]["OwnShipExt"]);
                    lbloApp1OwnShip.Text = "CA1OwnshipProof" + hdnCoApp1OwnShipExt.Value;
                }

                if (dt2.Rows.Count > 0)
                {
                    ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                    hdnCoApp2CustId.Value = Convert.ToString(dt2.Rows[0]["CoAppID"]);
                    txtCoApp2CustId.Text = Convert.ToString(dt2.Rows[0]["CoAppNo"]);
                    txtCoApp2CustName.Text = Convert.ToString(dt2.Rows[0]["CoAppName"]);
                    ddlCoApp2Gender.SelectedIndex = ddlCoApp2Gender.Items.IndexOf(ddlCoApp2Gender.Items.FindByValue(dt2.Rows[0]["Gender"].ToString()));
                    txtCoApp2Dob.Text = Convert.ToString(dt2.Rows[0]["DoB"]);
                    txtCoApp2Age.Text = Convert.ToString(dt2.Rows[0]["PresentAge"]);
                    txtCoApp2AgeAtLoanMaturity.Text = Convert.ToString(dt2.Rows[0]["AgeAtLoanMaturity"]);
                    txtCoApp2Pan.Text = Convert.ToString(dt2.Rows[0]["PanNo"]);
                    txtCoApp2AadhRefNo.Text = Convert.ToString(dt2.Rows[0]["AadhRefNo"]);
                    txtCoApp2AadhNo.Text = Convert.ToString(dt2.Rows[0]["AadhMaskedNo"]);
                    txtCoApp2Voter.Text = Convert.ToString(dt2.Rows[0]["VoterID"]);
                    ddlCoApp2RelWithApp.SelectedIndex = ddlCoApp2RelWithApp.Items.IndexOf(ddlCoApp2RelWithApp.Items.FindByValue(dt2.Rows[0]["RelWithApp"].ToString()));
                    txtCoApp2Mob.Text = Convert.ToString(dt2.Rows[0]["MobNo"]);
                    ddlCoApp2Edu.SelectedIndex = ddlCoApp2Edu.Items.IndexOf(ddlCoApp2Edu.Items.FindByValue(dt2.Rows[0]["Education"].ToString()));
                    ddlCoApp2Marital.SelectedIndex = ddlCoApp2Marital.Items.IndexOf(ddlCoApp2Marital.Items.FindByValue(dt2.Rows[0]["MaritalStatus"].ToString()));
                    txtCoApp2NoOfFamily.Text = Convert.ToString(dt2.Rows[0]["TotalNoFamMem"]);
                    txtCoApp2NoOfDependents.Text = Convert.ToString(dt2.Rows[0]["NoOfDependents"]);
                    ddlCoApp2Caste.SelectedIndex = ddlCoApp2Caste.Items.IndexOf(ddlCoApp2Caste.Items.FindByValue(dt2.Rows[0]["Caste"].ToString()));
                    ddlCoApp2Religion.SelectedIndex = ddlCoApp2Religion.Items.IndexOf(ddlCoApp2Religion.Items.FindByValue(dt2.Rows[0]["Religion"].ToString()));
                    ddlCoApp2Minority.SelectedIndex = ddlCoApp2Minority.Items.IndexOf(ddlCoApp2Minority.Items.FindByValue(dt2.Rows[0]["Minority"].ToString()));
                    txtCoApp2Email.Text = Convert.ToString(dt2.Rows[0]["Email"]);
                    txtCoApp2PerResiAdd.Text = Convert.ToString(dt2.Rows[0]["PerAdd"]);


                    ddlCoApp2PerState.SelectedIndex = ddlCoApp2PerState.Items.IndexOf(ddlCoApp2PerState.Items.FindByValue(dt2.Rows[0]["PerState"].ToString()));
                    PopDistrictByState(Convert.ToInt32(ddlCoApp2PerState.SelectedValue), "CoApp2Per");
                    ddlCoApp2PerDist.SelectedIndex = ddlCoApp2PerDist.Items.IndexOf(ddlCoApp2PerDist.Items.FindByValue(dt2.Rows[0]["PerDist"].ToString()));

                    txtCoApp2PerPin.Text = Convert.ToString(dt2.Rows[0]["PerPin"]);
                    txtCoApp2CurrResiAdd.Text = Convert.ToString(dt2.Rows[0]["CurrAdd"]);

                    ddlCoApp2CurrState.SelectedIndex = ddlCoApp2CurrState.Items.IndexOf(ddlCoApp2CurrState.Items.FindByValue(dt2.Rows[0]["CurrState"].ToString()));
                    PopDistrictByState(Convert.ToInt32(ddlCoApp2CurrState.SelectedValue), "CoApp2Curr");
                    ddlCoApp2CurrDist.SelectedIndex = ddlCoApp2CurrDist.Items.IndexOf(ddlCoApp2CurrDist.Items.FindByValue(dt2.Rows[0]["CurrDist"].ToString()));

                    txtCoApp2CurrPin.Text = Convert.ToString(dt2.Rows[0]["CurrPin"]);
                    txtCoApp2Landmark.Text = Convert.ToString(dt2.Rows[0]["CurrLandmark"]);
                    ddlCoApp2Ownship.SelectedIndex = ddlCoApp2Ownship.Items.IndexOf(ddlCoApp2Ownship.Items.FindByValue(dt2.Rows[0]["OwnShipStatus"].ToString()));
                    ddlCoApp2ResiStabYrs.SelectedIndex = ddlCoApp2ResiStabYrs.Items.IndexOf(ddlCoApp2ResiStabYrs.Items.FindByValue(dt2.Rows[0]["ResiStabYrs"].ToString()));

                    hdnCoApp2PanVerifyYN.Value = Convert.ToString(dt2.Rows[0]["PanVerifyYN"]);
                    hdnCoApp2AadhVerifyYN.Value = Convert.ToString(dt2.Rows[0]["AadhVerifyYN"]);
                    hdnCoApp2VoterVerifyYN.Value = Convert.ToString(dt2.Rows[0]["VoterVerifyYN"]);

                    StatusButton("Show");

                    if (Convert.ToString(dt2.Rows[0]["PanVerifyYN"]) == "Y")
                    {
                        btnVerifyCoApp2Pan.Enabled = false;
                        txtCoApp2Pan.Enabled = false;
                        hdnCoApp2PanVerifyYN.Value = "Y";
                    }
                    if (Convert.ToString(dt2.Rows[0]["AadhVerifyYN"]) == "Y")
                    {
                        btnVerifyCoApp2Aadhaar.Enabled = false;
                        txtCoApp2AadhNo.Enabled = false;
                        hdnCoApp2AadhVerifyYN.Value = "Y";
                    }

                    if (Convert.ToString(dt2.Rows[0]["VoterVerifyYN"]) == "Y")
                    {
                        btnVerifyCoApp2Voter.Enabled = false;
                        txtCoApp2Voter.Enabled = false;
                        hdnCoApp2VoterVerifyYN.Value = "Y";
                    }
                    hdnCoApp2OwnShipExt.Value = Convert.ToString(dt2.Rows[0]["OwnShipExt"]);
                    lblCoApp2OwnShip.Text = "CA2OwnshipProof" + hdnCoApp2OwnShipExt.Value;
                }
                if (dt3.Rows.Count > 0)
                {
                    ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                    hdnGuarId.Value = Convert.ToString(dt3.Rows[0]["CoAppID"]);
                    txtGuarId.Text = Convert.ToString(dt3.Rows[0]["CoAppNo"]);
                    txtGuarName.Text = Convert.ToString(dt3.Rows[0]["CoAppName"]);
                    ddlGuarGender.SelectedIndex = ddlGuarGender.Items.IndexOf(ddlGuarGender.Items.FindByValue(dt3.Rows[0]["Gender"].ToString()));
                    txtGuarDOB.Text = Convert.ToString(dt3.Rows[0]["DoB"]);
                    txtGuarAge.Text = Convert.ToString(dt3.Rows[0]["PresentAge"]);
                    txtGuarAgeAtLoanMaturity.Text = Convert.ToString(dt3.Rows[0]["AgeAtLoanMaturity"]);
                    txtGuarPan.Text = Convert.ToString(dt3.Rows[0]["PanNo"]);
                    txtGuarAadhRefNo.Text = Convert.ToString(dt3.Rows[0]["AadhRefNo"]);
                    txtGuarAadhNo.Text = Convert.ToString(dt3.Rows[0]["AadhMaskedNo"]);
                    txtGuarVoter.Text = Convert.ToString(dt3.Rows[0]["VoterID"]);
                    ddlGuarRelWithApp.SelectedIndex = ddlGuarRelWithApp.Items.IndexOf(ddlGuarRelWithApp.Items.FindByValue(dt3.Rows[0]["RelWithApp"].ToString()));
                    txtGuarMob.Text = Convert.ToString(dt3.Rows[0]["MobNo"]);
                    ddlGuarEdu.SelectedIndex = ddlGuarEdu.Items.IndexOf(ddlGuarEdu.Items.FindByValue(dt3.Rows[0]["Education"].ToString()));
                    ddlGuarMarital.SelectedIndex = ddlGuarMarital.Items.IndexOf(ddlGuarMarital.Items.FindByValue(dt3.Rows[0]["MaritalStatus"].ToString()));
                    txtGuarNoOfFamily.Text = Convert.ToString(dt3.Rows[0]["TotalNoFamMem"]);
                    txtGuarNoOfDependents.Text = Convert.ToString(dt3.Rows[0]["NoOfDependents"]);
                    ddlGuarCaste.SelectedIndex = ddlGuarCaste.Items.IndexOf(ddlGuarCaste.Items.FindByValue(dt3.Rows[0]["Caste"].ToString()));
                    ddlGuarReligion.SelectedIndex = ddlGuarReligion.Items.IndexOf(ddlGuarReligion.Items.FindByValue(dt3.Rows[0]["Religion"].ToString()));
                    ddlGuarMinority.SelectedIndex = ddlGuarMinority.Items.IndexOf(ddlGuarMinority.Items.FindByValue(dt3.Rows[0]["Minority"].ToString()));
                    txtGuarEmail.Text = Convert.ToString(dt3.Rows[0]["Email"]);
                    txtGuarPerResiAdd.Text = Convert.ToString(dt3.Rows[0]["PerAdd"]);

                    ddlGuarPerState.SelectedIndex = ddlGuarPerState.Items.IndexOf(ddlGuarPerState.Items.FindByValue(dt3.Rows[0]["PerState"].ToString()));
                    PopDistrictByState(Convert.ToInt32(ddlGuarPerState.SelectedValue), "GuarPer");
                    ddlGuarPerDist.SelectedIndex = ddlGuarPerDist.Items.IndexOf(ddlGuarPerDist.Items.FindByValue(dt3.Rows[0]["PerDist"].ToString()));

                    txtGuarPerPin.Text = Convert.ToString(dt3.Rows[0]["PerPin"]);
                    txtGuarCurrResiAdd.Text = Convert.ToString(dt3.Rows[0]["CurrAdd"]);

                    ddlGuarCurrState.SelectedIndex = ddlGuarCurrState.Items.IndexOf(ddlGuarCurrState.Items.FindByValue(dt3.Rows[0]["CurrState"].ToString()));
                    PopDistrictByState(Convert.ToInt32(ddlGuarCurrState.SelectedValue), "GuarCurr");
                    ddlGuarCurrDist.SelectedIndex = ddlGuarCurrDist.Items.IndexOf(ddlGuarCurrDist.Items.FindByValue(dt3.Rows[0]["CurrDist"].ToString()));

                    txtGuarCurrPin.Text = Convert.ToString(dt3.Rows[0]["CurrPin"]);
                    txtGuarLandmark.Text = Convert.ToString(dt3.Rows[0]["CurrLandmark"]);
                    ddlGuarOwnStat.SelectedIndex = ddlGuarOwnStat.Items.IndexOf(ddlGuarOwnStat.Items.FindByValue(dt3.Rows[0]["OwnShipStatus"].ToString()));
                    ddlGuarResiStabYrs.SelectedIndex = ddlGuarResiStabYrs.Items.IndexOf(ddlGuarResiStabYrs.Items.FindByValue(dt3.Rows[0]["ResiStabYrs"].ToString()));

                    hdnGuarPanVerifyYN.Value = Convert.ToString(dt3.Rows[0]["PanVerifyYN"]);
                    hdnGuarAadhVerifyYN.Value = Convert.ToString(dt3.Rows[0]["AadhVerifyYN"]);
                    hdnGuarVoterVerifyYN.Value = Convert.ToString(dt3.Rows[0]["VoterVerifyYN"]);

                    StatusButton("Show");

                    if (Convert.ToString(dt3.Rows[0]["PanVerifyYN"]) == "Y")
                    {
                        btnVerifyGuarPan.Enabled = false;
                        txtGuarPan.Enabled = false;
                        hdnGuarPanVerifyYN.Value = "Y";
                    }
                    if (Convert.ToString(dt3.Rows[0]["AadhVerifyYN"]) == "Y")
                    {
                        btnVerifyGuarAadhaar.Enabled = false;
                        txtGuarAadhNo.Enabled = false;
                        hdnGuarAadhVerifyYN.Value = "Y";
                    }

                    if (Convert.ToString(dt3.Rows[0]["VoterVerifyYN"]) == "Y")
                    {
                        btnVerifyGuarVoter.Enabled = false;
                        txtGuarVoter.Enabled = false;
                        hdnGuarVoterVerifyYN.Value = "Y";
                    }
                    hdnGuarOwnShipExt.Value = Convert.ToString(dt3.Rows[0]["OwnShipExt"]);
                    lblGuarOwnShip.Text = "GOwnshipProof" + hdnGuarOwnShipExt.Value;
                }
                if (dt4.Rows.Count > 0)
                {
                    ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                    hdnCoApp3CustId.Value = Convert.ToString(dt4.Rows[0]["CoAppID"]);
                    txtCoApp3CustId.Text = Convert.ToString(dt4.Rows[0]["CoAppNo"]);
                    txtCoApp3CustName.Text = Convert.ToString(dt4.Rows[0]["CoAppName"]);
                    ddlCoApp3Gender.SelectedIndex = ddlCoApp3Gender.Items.IndexOf(ddlCoApp3Gender.Items.FindByValue(dt4.Rows[0]["Gender"].ToString()));
                    txtCoApp3Dob.Text = Convert.ToString(dt4.Rows[0]["DoB"]);
                    txtCoApp3Age.Text = Convert.ToString(dt4.Rows[0]["PresentAge"]);
                    txtCoApp3AgeAtLoanMaturity.Text = Convert.ToString(dt4.Rows[0]["AgeAtLoanMaturity"]);
                    txtCoApp3Pan.Text = Convert.ToString(dt4.Rows[0]["PanNo"]);
                    txtCoApp3AadhRefNo.Text = Convert.ToString(dt4.Rows[0]["AadhRefNo"]);
                    txtCoApp3AadhNo.Text = Convert.ToString(dt4.Rows[0]["AadhMaskedNo"]);
                    txtCoApp3Voter.Text = Convert.ToString(dt4.Rows[0]["VoterID"]);
                    ddlCoApp3RelWithApp.SelectedIndex = ddlCoApp3RelWithApp.Items.IndexOf(ddlCoApp3RelWithApp.Items.FindByValue(dt4.Rows[0]["RelWithApp"].ToString()));
                    txtCoApp3Mob.Text = Convert.ToString(dt4.Rows[0]["MobNo"]);
                    ddlCoApp3Edu.SelectedIndex = ddlCoApp3Edu.Items.IndexOf(ddlCoApp3Edu.Items.FindByValue(dt4.Rows[0]["Education"].ToString()));
                    ddlCoApp3Marital.SelectedIndex = ddlCoApp3Marital.Items.IndexOf(ddlCoApp3Marital.Items.FindByValue(dt4.Rows[0]["MaritalStatus"].ToString()));
                    txtCoApp3NoOfFamily.Text = Convert.ToString(dt4.Rows[0]["TotalNoFamMem"]);
                    txtCoApp3NoOfDependents.Text = Convert.ToString(dt4.Rows[0]["NoOfDependents"]);
                    ddlCoApp3Caste.SelectedIndex = ddlCoApp3Caste.Items.IndexOf(ddlCoApp3Caste.Items.FindByValue(dt4.Rows[0]["Caste"].ToString()));
                    ddlCoApp3Religion.SelectedIndex = ddlCoApp3Religion.Items.IndexOf(ddlCoApp3Religion.Items.FindByValue(dt4.Rows[0]["Religion"].ToString()));
                    ddlCoApp3Minority.SelectedIndex = ddlCoApp3Minority.Items.IndexOf(ddlCoApp3Minority.Items.FindByValue(dt4.Rows[0]["Minority"].ToString()));
                    txtCoApp3Email.Text = Convert.ToString(dt4.Rows[0]["Email"]);
                    txtCoApp3PerResiAdd.Text = Convert.ToString(dt4.Rows[0]["PerAdd"]);


                    ddlCoApp3PerState.SelectedIndex = ddlCoApp3PerState.Items.IndexOf(ddlCoApp3PerState.Items.FindByValue(dt4.Rows[0]["PerState"].ToString()));
                    PopDistrictByState(Convert.ToInt32(ddlCoApp3PerState.SelectedValue), "CoApp3Per");
                    ddlCoApp3PerDist.SelectedIndex = ddlCoApp3PerDist.Items.IndexOf(ddlCoApp3PerDist.Items.FindByValue(dt4.Rows[0]["PerDist"].ToString()));

                    txtCoApp3PerPin.Text = Convert.ToString(dt4.Rows[0]["PerPin"]);
                    txtCoApp3CurrResiAdd.Text = Convert.ToString(dt4.Rows[0]["CurrAdd"]);

                    ddlCoApp3CurrState.SelectedIndex = ddlCoApp3CurrState.Items.IndexOf(ddlCoApp3CurrState.Items.FindByValue(dt4.Rows[0]["CurrState"].ToString()));
                    PopDistrictByState(Convert.ToInt32(ddlCoApp3CurrState.SelectedValue), "CoApp3Curr");
                    ddlCoApp3CurrDist.SelectedIndex = ddlCoApp3CurrDist.Items.IndexOf(ddlCoApp3CurrDist.Items.FindByValue(dt4.Rows[0]["CurrDist"].ToString()));

                    txtCoApp3CurrPin.Text = Convert.ToString(dt4.Rows[0]["CurrPin"]);
                    txtCoApp3Landmark.Text = Convert.ToString(dt4.Rows[0]["CurrLandmark"]);
                    ddlCoApp3Ownship.SelectedIndex = ddlCoApp3Ownship.Items.IndexOf(ddlCoApp3Ownship.Items.FindByValue(dt4.Rows[0]["OwnShipStatus"].ToString()));
                    ddlCoApp3ResiStabYrs.SelectedIndex = ddlCoApp3ResiStabYrs.Items.IndexOf(ddlCoApp3ResiStabYrs.Items.FindByValue(dt4.Rows[0]["ResiStabYrs"].ToString()));

                    hdnCoApp3PanVerifyYN.Value = Convert.ToString(dt4.Rows[0]["PanVerifyYN"]);
                    hdnCoApp3AadhVerifyYN.Value = Convert.ToString(dt4.Rows[0]["AadhVerifyYN"]);
                    hdnCoApp3VoterVerifyYN.Value = Convert.ToString(dt4.Rows[0]["VoterVerifyYN"]);

                    StatusButton("Show");

                    if (Convert.ToString(dt4.Rows[0]["PanVerifyYN"]) == "Y")
                    {
                        btnVerifyCoApp3Pan.Enabled = false;
                        txtCoApp3Pan.Enabled = false;
                        hdnCoApp3PanVerifyYN.Value = "Y";
                    }
                    if (Convert.ToString(dt4.Rows[0]["AadhVerifyYN"]) == "Y")
                    {
                        btnVerifyCoApp3Aadhaar.Enabled = false;
                        txtCoApp3AadhNo.Enabled = false;
                        hdnCoApp3AadhVerifyYN.Value = "Y";
                    }

                    if (Convert.ToString(dt4.Rows[0]["VoterVerifyYN"]) == "Y")
                    {
                        btnVerifyCoApp3Voter.Enabled = false;
                        txtCoApp3Voter.Enabled = false;
                        hdnCoApp3VoterVerifyYN.Value = "Y";
                    }
                    hdnCoApp3OwnShipExt.Value = Convert.ToString(dt4.Rows[0]["OwnShipExt"]);
                    lblCoApp3OwnShip.Text = "CA3OwnshipProof" + hdnCoApp3OwnShipExt.Value;
                }
                if (dt5.Rows.Count > 0)
                {
                    ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                    hdnCoApp4CustId.Value = Convert.ToString(dt5.Rows[0]["CoAppID"]);
                    txtCoApp4CustId.Text = Convert.ToString(dt5.Rows[0]["CoAppNo"]);
                    txtCoApp4CustName.Text = Convert.ToString(dt5.Rows[0]["CoAppName"]);
                    ddlCoApp4Gender.SelectedIndex = ddlCoApp4Gender.Items.IndexOf(ddlCoApp4Gender.Items.FindByValue(dt5.Rows[0]["Gender"].ToString()));
                    txtCoApp4Dob.Text = Convert.ToString(dt5.Rows[0]["DoB"]);
                    txtCoApp4Age.Text = Convert.ToString(dt5.Rows[0]["PresentAge"]);
                    txtCoApp4AgeAtLoanMaturity.Text = Convert.ToString(dt5.Rows[0]["AgeAtLoanMaturity"]);
                    txtCoApp4Pan.Text = Convert.ToString(dt5.Rows[0]["PanNo"]);
                    txtCoApp4AadhRefNo.Text = Convert.ToString(dt5.Rows[0]["AadhRefNo"]);
                    txtCoApp4AadhNo.Text = Convert.ToString(dt5.Rows[0]["AadhMaskedNo"]);
                    txtCoApp4Voter.Text = Convert.ToString(dt5.Rows[0]["VoterID"]);
                    ddlCoApp4RelWithApp.SelectedIndex = ddlCoApp4RelWithApp.Items.IndexOf(ddlCoApp4RelWithApp.Items.FindByValue(dt5.Rows[0]["RelWithApp"].ToString()));
                    txtCoApp4Mob.Text = Convert.ToString(dt5.Rows[0]["MobNo"]);
                    ddlCoApp4Edu.SelectedIndex = ddlCoApp4Edu.Items.IndexOf(ddlCoApp4Edu.Items.FindByValue(dt5.Rows[0]["Education"].ToString()));
                    ddlCoApp4Marital.SelectedIndex = ddlCoApp4Marital.Items.IndexOf(ddlCoApp4Marital.Items.FindByValue(dt5.Rows[0]["MaritalStatus"].ToString()));
                    txtCoApp4NoOfFamily.Text = Convert.ToString(dt5.Rows[0]["TotalNoFamMem"]);
                    txtCoApp4NoOfDependents.Text = Convert.ToString(dt5.Rows[0]["NoOfDependents"]);
                    ddlCoApp4Caste.SelectedIndex = ddlCoApp4Caste.Items.IndexOf(ddlCoApp4Caste.Items.FindByValue(dt5.Rows[0]["Caste"].ToString()));
                    ddlCoApp4Religion.SelectedIndex = ddlCoApp4Religion.Items.IndexOf(ddlCoApp4Religion.Items.FindByValue(dt5.Rows[0]["Religion"].ToString()));
                    ddlCoApp4Minority.SelectedIndex = ddlCoApp4Minority.Items.IndexOf(ddlCoApp4Minority.Items.FindByValue(dt5.Rows[0]["Minority"].ToString()));
                    txtCoApp4Email.Text = Convert.ToString(dt5.Rows[0]["Email"]);
                    txtCoApp4PerResiAdd.Text = Convert.ToString(dt5.Rows[0]["PerAdd"]);


                    ddlCoApp4PerState.SelectedIndex = ddlCoApp4PerState.Items.IndexOf(ddlCoApp4PerState.Items.FindByValue(dt5.Rows[0]["PerState"].ToString()));
                    PopDistrictByState(Convert.ToInt32(ddlCoApp4PerState.SelectedValue), "CoApp4Per");
                    ddlCoApp4PerDist.SelectedIndex = ddlCoApp4PerDist.Items.IndexOf(ddlCoApp4PerDist.Items.FindByValue(dt5.Rows[0]["PerDist"].ToString()));

                    txtCoApp4PerPin.Text = Convert.ToString(dt5.Rows[0]["PerPin"]);
                    txtCoApp4CurrResiAdd.Text = Convert.ToString(dt5.Rows[0]["CurrAdd"]);

                    ddlCoApp4CurrState.SelectedIndex = ddlCoApp4CurrState.Items.IndexOf(ddlCoApp4CurrState.Items.FindByValue(dt5.Rows[0]["CurrState"].ToString()));
                    PopDistrictByState(Convert.ToInt32(ddlCoApp4CurrState.SelectedValue), "CoApp4Curr");
                    ddlCoApp4CurrDist.SelectedIndex = ddlCoApp4CurrDist.Items.IndexOf(ddlCoApp4CurrDist.Items.FindByValue(dt5.Rows[0]["CurrDist"].ToString()));

                    txtCoApp4CurrPin.Text = Convert.ToString(dt5.Rows[0]["CurrPin"]);
                    txtCoApp4Landmark.Text = Convert.ToString(dt5.Rows[0]["CurrLandmark"]);
                    ddlCoApp4Ownship.SelectedIndex = ddlCoApp4Ownship.Items.IndexOf(ddlCoApp4Ownship.Items.FindByValue(dt5.Rows[0]["OwnShipStatus"].ToString()));
                    ddlCoApp4ResiStabYrs.SelectedIndex = ddlCoApp4ResiStabYrs.Items.IndexOf(ddlCoApp4ResiStabYrs.Items.FindByValue(dt5.Rows[0]["ResiStabYrs"].ToString()));

                    hdnCoApp4PanVerifyYN.Value = Convert.ToString(dt5.Rows[0]["PanVerifyYN"]);
                    hdnCoApp4AadhVerifyYN.Value = Convert.ToString(dt5.Rows[0]["AadhVerifyYN"]);
                    hdnCoApp4VoterVerifyYN.Value = Convert.ToString(dt5.Rows[0]["VoterVerifyYN"]);

                    StatusButton("Show");

                    if (Convert.ToString(dt5.Rows[0]["PanVerifyYN"]) == "Y")
                    {
                        btnVerifyCoApp4Pan.Enabled = false;
                        txtCoApp4Pan.Enabled = false;
                        hdnCoApp4PanVerifyYN.Value = "Y";
                    }
                    if (Convert.ToString(dt5.Rows[0]["AadhVerifyYN"]) == "Y")
                    {
                        btnVerifyCoApp4Aadhaar.Enabled = false;
                        txtCoApp4AadhNo.Enabled = false;
                        hdnCoApp4AadhVerifyYN.Value = "Y";
                    }

                    if (Convert.ToString(dt5.Rows[0]["VoterVerifyYN"]) == "Y")
                    {
                        btnVerifyCoApp4Voter.Enabled = false;
                        txtCoApp4Voter.Enabled = false;
                        hdnCoApp4VoterVerifyYN.Value = "Y";
                    }
                    hdnCoApp4OwnShipExt.Value = Convert.ToString(dt5.Rows[0]["OwnShipExt"]);
                    lblCoApp4OwnShip.Text = "CA4OwnshipProof" + hdnCoApp4OwnShipExt.Value;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        private void PopLeadList()
        {
            DataTable dt = new DataTable();
            CCust360 oMem = new CCust360();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oMem.CF_GetLeadFromBasicDtl(vBrCode);
                ListItem oli = new ListItem("<--Select-->", "-1");

                ddlCustomer.DataSource = dt;
                ddlCustomer.DataTextField = "BCPropNo";
                ddlCustomer.DataValueField = "LeadID";
                ddlCustomer.DataBind();
                ddlCustomer.Items.Insert(0, oli);
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
        private void popCaste()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                dt = oMem.CF_GetCaste();

                ddlApplCast.DataSource = dt;
                ddlApplCast.DataTextField = "Caste";
                ddlApplCast.DataValueField = "CasteId";
                ddlApplCast.DataBind();
                ddlApplCast.Items.Insert(0, oli1);

                ddlCoApp1Caste.DataSource = dt;
                ddlCoApp1Caste.DataTextField = "Caste";
                ddlCoApp1Caste.DataValueField = "CasteId";
                ddlCoApp1Caste.DataBind();
                ddlCoApp1Caste.Items.Insert(0, oli2);

                ddlCoApp2Caste.DataSource = dt;
                ddlCoApp2Caste.DataTextField = "Caste";
                ddlCoApp2Caste.DataValueField = "CasteId";
                ddlCoApp2Caste.DataBind();
                ddlCoApp2Caste.Items.Insert(0, oli3);

                ddlCoApp3Caste.DataSource = dt;
                ddlCoApp3Caste.DataTextField = "Caste";
                ddlCoApp3Caste.DataValueField = "CasteId";
                ddlCoApp3Caste.DataBind();
                ddlCoApp3Caste.Items.Insert(0, oli5);

                ddlCoApp4Caste.DataSource = dt;
                ddlCoApp4Caste.DataTextField = "Caste";
                ddlCoApp4Caste.DataValueField = "CasteId";
                ddlCoApp4Caste.DataBind();
                ddlCoApp4Caste.Items.Insert(0, oli6);

                ddlGuarCaste.DataSource = dt;
                ddlGuarCaste.DataTextField = "Caste";
                ddlGuarCaste.DataValueField = "CasteId";
                ddlGuarCaste.DataBind();
                ddlGuarCaste.Items.Insert(0, oli4);
            }
            finally
            {
            }
        }
        private void PopReligion()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");

                dt = oMem.CF_GetReligion();
                ddlApplReligion.DataSource = dt;
                ddlApplReligion.DataTextField = "Religion";
                ddlApplReligion.DataValueField = "ReligionId";
                ddlApplReligion.DataBind();
                ddlApplReligion.Items.Insert(0, oli1);

                ddlCoApp1Religion.DataSource = dt;
                ddlCoApp1Religion.DataTextField = "Religion";
                ddlCoApp1Religion.DataValueField = "ReligionId";
                ddlCoApp1Religion.DataBind();
                ddlCoApp1Religion.Items.Insert(0, oli2);

                ddlCoApp2Religion.DataSource = dt;
                ddlCoApp2Religion.DataTextField = "Religion";
                ddlCoApp2Religion.DataValueField = "ReligionId";
                ddlCoApp2Religion.DataBind();
                ddlCoApp2Religion.Items.Insert(0, oli3);

                ddlCoApp3Religion.DataSource = dt;
                ddlCoApp3Religion.DataTextField = "Religion";
                ddlCoApp3Religion.DataValueField = "ReligionId";
                ddlCoApp3Religion.DataBind();
                ddlCoApp3Religion.Items.Insert(0, oli5);

                ddlCoApp4Religion.DataSource = dt;
                ddlCoApp4Religion.DataTextField = "Religion";
                ddlCoApp4Religion.DataValueField = "ReligionId";
                ddlCoApp4Religion.DataBind();
                ddlCoApp4Religion.Items.Insert(0, oli6);

                ddlGuarReligion.DataSource = dt;
                ddlGuarReligion.DataTextField = "Religion";
                ddlGuarReligion.DataValueField = "ReligionId";
                ddlGuarReligion.DataBind();
                ddlGuarReligion.Items.Insert(0, oli4);
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
        private void PopQualification()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                dt = oMem.CF_GetQualification();

                ddlAppEdu.DataSource = dt;
                ddlAppEdu.DataTextField = "QualificationName";
                ddlAppEdu.DataValueField = "QualificationId";
                ddlAppEdu.DataBind();
                ddlAppEdu.Items.Insert(0, oli1);

                ddlCoApp1Edu.DataSource = dt;
                ddlCoApp1Edu.DataTextField = "QualificationName";
                ddlCoApp1Edu.DataValueField = "QualificationId";
                ddlCoApp1Edu.DataBind();
                ddlCoApp1Edu.Items.Insert(0, oli2);

                ddlCoApp2Edu.DataSource = dt;
                ddlCoApp2Edu.DataTextField = "QualificationName";
                ddlCoApp2Edu.DataValueField = "QualificationId";
                ddlCoApp2Edu.DataBind();
                ddlCoApp2Edu.Items.Insert(0, oli3);

                ddlCoApp3Edu.DataSource = dt;
                ddlCoApp3Edu.DataTextField = "QualificationName";
                ddlCoApp3Edu.DataValueField = "QualificationId";
                ddlCoApp3Edu.DataBind();
                ddlCoApp3Edu.Items.Insert(0, oli5);

                ddlCoApp4Edu.DataSource = dt;
                ddlCoApp4Edu.DataTextField = "QualificationName";
                ddlCoApp4Edu.DataValueField = "QualificationId";
                ddlCoApp4Edu.DataBind();
                ddlCoApp4Edu.Items.Insert(0, oli6);

                ddlGuarEdu.DataSource = dt;
                ddlGuarEdu.DataTextField = "QualificationName";
                ddlGuarEdu.DataValueField = "QualificationId";
                ddlGuarEdu.DataBind();
                ddlGuarEdu.Items.Insert(0, oli4);
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
        private void popState()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");

                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                ListItem oli7 = new ListItem("<--Select-->", "-1");
                ListItem oli8 = new ListItem("<--Select-->", "-1");

                ListItem oli9 = new ListItem("<--Select-->", "-1");
                ListItem oli10 = new ListItem("<--Select-->", "-1");
                ListItem oli11 = new ListItem("<--Select-->", "-1");
                ListItem oli12 = new ListItem("<--Select-->", "-1");

                dt = oMem.CF_GetState();

                ddlApplState.DataSource = dt;
                ddlApplState.DataTextField = "StateName";
                ddlApplState.DataValueField = "StateId";
                ddlApplState.DataBind();
                ddlApplState.Items.Insert(0, oli1);

                ddlApplCurrState.DataSource = dt;
                ddlApplCurrState.DataTextField = "StateName";
                ddlApplCurrState.DataValueField = "StateId";
                ddlApplCurrState.DataBind();
                ddlApplCurrState.Items.Insert(0, oli2);

                ddlCoApp1PerState.DataSource = dt;
                ddlCoApp1PerState.DataTextField = "StateName";
                ddlCoApp1PerState.DataValueField = "StateId";
                ddlCoApp1PerState.DataBind();
                ddlCoApp1PerState.Items.Insert(0, oli3);

                ddlCoApp1CurrState.DataSource = dt;
                ddlCoApp1CurrState.DataTextField = "StateName";
                ddlCoApp1CurrState.DataValueField = "StateId";
                ddlCoApp1CurrState.DataBind();
                ddlCoApp1CurrState.Items.Insert(0, oli4);

                ddlCoApp2PerState.DataSource = dt;
                ddlCoApp2PerState.DataTextField = "StateName";
                ddlCoApp2PerState.DataValueField = "StateId";
                ddlCoApp2PerState.DataBind();
                ddlCoApp2PerState.Items.Insert(0, oli5);

                ddlCoApp2CurrState.DataSource = dt;
                ddlCoApp2CurrState.DataTextField = "StateName";
                ddlCoApp2CurrState.DataValueField = "StateId";
                ddlCoApp2CurrState.DataBind();
                ddlCoApp2CurrState.Items.Insert(0, oli6);

                ddlCoApp3PerState.DataSource = dt;
                ddlCoApp3PerState.DataTextField = "StateName";
                ddlCoApp3PerState.DataValueField = "StateId";
                ddlCoApp3PerState.DataBind();
                ddlCoApp3PerState.Items.Insert(0, oli9);

                ddlCoApp3CurrState.DataSource = dt;
                ddlCoApp3CurrState.DataTextField = "StateName";
                ddlCoApp3CurrState.DataValueField = "StateId";
                ddlCoApp3CurrState.DataBind();
                ddlCoApp3CurrState.Items.Insert(0, oli10);

                ddlCoApp4PerState.DataSource = dt;
                ddlCoApp4PerState.DataTextField = "StateName";
                ddlCoApp4PerState.DataValueField = "StateId";
                ddlCoApp4PerState.DataBind();
                ddlCoApp4PerState.Items.Insert(0, oli11);

                ddlCoApp4CurrState.DataSource = dt;
                ddlCoApp4CurrState.DataTextField = "StateName";
                ddlCoApp4CurrState.DataValueField = "StateId";
                ddlCoApp4CurrState.DataBind();
                ddlCoApp4CurrState.Items.Insert(0, oli12);

                ddlGuarPerState.DataSource = dt;
                ddlGuarPerState.DataTextField = "StateName";
                ddlGuarPerState.DataValueField = "StateId";
                ddlGuarPerState.DataBind();
                ddlGuarPerState.Items.Insert(0, oli7);

                ddlGuarCurrState.DataSource = dt;
                ddlGuarCurrState.DataTextField = "StateName";
                ddlGuarCurrState.DataValueField = "StateId";
                ddlGuarCurrState.DataBind();
                ddlGuarCurrState.Items.Insert(0, oli8);
            }
            finally
            {
            }
        }
        private void PopRelation()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                dt = oMem.CF_GetRelation();

                ddlRelWithApp.DataSource = dt;
                ddlRelWithApp.DataTextField = "Relation";
                ddlRelWithApp.DataValueField = "RelationId";
                ddlRelWithApp.DataBind();
                ddlRelWithApp.Items.Insert(0, oli1);

                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    if (dt.Rows[i]["Relation"].ToString() == "Self")
                    {
                        dt.Rows.RemoveAt(i);
                    }
                }

                ddlCoApp1RelWithApp.DataSource = dt;
                ddlCoApp1RelWithApp.DataTextField = "Relation";
                ddlCoApp1RelWithApp.DataValueField = "RelationId";
                ddlCoApp1RelWithApp.DataBind();
                ddlCoApp1RelWithApp.Items.Insert(0, oli2);

                ddlCoApp2RelWithApp.DataSource = dt;
                ddlCoApp2RelWithApp.DataTextField = "Relation";
                ddlCoApp2RelWithApp.DataValueField = "RelationId";
                ddlCoApp2RelWithApp.DataBind();
                ddlCoApp2RelWithApp.Items.Insert(0, oli3);

                ddlCoApp3RelWithApp.DataSource = dt;
                ddlCoApp3RelWithApp.DataTextField = "Relation";
                ddlCoApp3RelWithApp.DataValueField = "RelationId";
                ddlCoApp3RelWithApp.DataBind();
                ddlCoApp3RelWithApp.Items.Insert(0, oli5);

                ddlCoApp4RelWithApp.DataSource = dt;
                ddlCoApp4RelWithApp.DataTextField = "Relation";
                ddlCoApp4RelWithApp.DataValueField = "RelationId";
                ddlCoApp4RelWithApp.DataBind();
                ddlCoApp4RelWithApp.Items.Insert(0, oli6);

                ddlGuarRelWithApp.DataSource = dt;
                ddlGuarRelWithApp.DataTextField = "Relation";
                ddlGuarRelWithApp.DataValueField = "RelationId";
                ddlGuarRelWithApp.DataBind();
                ddlGuarRelWithApp.Items.Insert(0, oli4);
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
        private void PopDistrictByState(int pStateId, string pTag)
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                ListItem oli7 = new ListItem("<--Select-->", "-1");
                ListItem oli8 = new ListItem("<--Select-->", "-1");
                ListItem oli9 = new ListItem("<--Select-->", "-1");
                ListItem oli10 = new ListItem("<--Select-->", "-1");
                ListItem oli11 = new ListItem("<--Select-->", "-1");
                ListItem oli12 = new ListItem("<--Select-->", "-1");

                switch (pTag)
                {
                    case "AppPer":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlApplDist.Items.Insert(0, oli1);
                        ddlApplDist.DataSource = dt;
                        ddlApplDist.DataTextField = "DistrictName";
                        ddlApplDist.DataValueField = "DistrictId";
                        ddlApplDist.DataBind();
                        break;
                    case "AppCurr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlApplCurrDist.Items.Insert(0, oli2);
                        ddlApplCurrDist.DataSource = dt;
                        ddlApplCurrDist.DataTextField = "DistrictName";
                        ddlApplCurrDist.DataValueField = "DistrictId";
                        ddlApplCurrDist.DataBind();
                        break;
                    case "CoApp1Per":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp1PerDist.Items.Insert(0, oli3);
                        ddlCoApp1PerDist.DataSource = dt;
                        ddlCoApp1PerDist.DataTextField = "DistrictName";
                        ddlCoApp1PerDist.DataValueField = "DistrictId";
                        ddlCoApp1PerDist.DataBind();
                        break;
                    case "CoApp1Curr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp1CurrDist.Items.Insert(0, oli4);
                        ddlCoApp1CurrDist.DataSource = dt;
                        ddlCoApp1CurrDist.DataTextField = "DistrictName";
                        ddlCoApp1CurrDist.DataValueField = "DistrictId";
                        ddlCoApp1CurrDist.DataBind();
                        break;
                    case "CoApp2Per":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp2PerDist.Items.Insert(0, oli5);
                        ddlCoApp2PerDist.DataSource = dt;
                        ddlCoApp2PerDist.DataTextField = "DistrictName";
                        ddlCoApp2PerDist.DataValueField = "DistrictId";
                        ddlCoApp2PerDist.DataBind();
                        break;
                    case "CoApp2Curr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp2CurrDist.Items.Insert(0, oli6);
                        ddlCoApp2CurrDist.DataSource = dt;
                        ddlCoApp2CurrDist.DataTextField = "DistrictName";
                        ddlCoApp2CurrDist.DataValueField = "DistrictId";
                        ddlCoApp2CurrDist.DataBind();
                        break;
                    case "CoApp3Per":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp3PerDist.Items.Insert(0, oli9);
                        ddlCoApp3PerDist.DataSource = dt;
                        ddlCoApp3PerDist.DataTextField = "DistrictName";
                        ddlCoApp3PerDist.DataValueField = "DistrictId";
                        ddlCoApp3PerDist.DataBind();
                        break;
                    case "CoApp3Curr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp3CurrDist.Items.Insert(0, oli10);
                        ddlCoApp3CurrDist.DataSource = dt;
                        ddlCoApp3CurrDist.DataTextField = "DistrictName";
                        ddlCoApp3CurrDist.DataValueField = "DistrictId";
                        ddlCoApp3CurrDist.DataBind();
                        break;
                    case "CoApp4Per":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp4PerDist.Items.Insert(0, oli11);
                        ddlCoApp4PerDist.DataSource = dt;
                        ddlCoApp4PerDist.DataTextField = "DistrictName";
                        ddlCoApp4PerDist.DataValueField = "DistrictId";
                        ddlCoApp4PerDist.DataBind();
                        break;
                    case "CoApp4Curr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp4CurrDist.Items.Insert(0, oli12);
                        ddlCoApp4CurrDist.DataSource = dt;
                        ddlCoApp4CurrDist.DataTextField = "DistrictName";
                        ddlCoApp4CurrDist.DataValueField = "DistrictId";
                        ddlCoApp4CurrDist.DataBind();
                        break;
                    case "GuarPer":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlGuarPerDist.Items.Insert(0, oli7);
                        ddlGuarPerDist.DataSource = dt;
                        ddlGuarPerDist.DataTextField = "DistrictName";
                        ddlGuarPerDist.DataValueField = "DistrictId";
                        ddlGuarPerDist.DataBind();
                        break;
                    case "GuarCurr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlGuarCurrDist.Items.Insert(0, oli8);
                        ddlGuarCurrDist.DataSource = dt;
                        ddlGuarCurrDist.DataTextField = "DistrictName";
                        ddlGuarCurrDist.DataValueField = "DistrictId";
                        ddlGuarCurrDist.DataBind();
                        break;
                }
            }
            finally
            {
            }
        }
        protected void ddlApplState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vStateId = Convert.ToInt32(ddlApplState.SelectedValue);
            PopDistrictByState(vStateId, "AppPer");
        }
        protected void ddlApplCurrState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vStateId = Convert.ToInt32(ddlApplCurrState.SelectedValue);
            PopDistrictByState(vStateId, "AppCurr");
        }
        protected void ddlCoApp1PerState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vStateId = Convert.ToInt32(ddlCoApp1PerState.SelectedValue);
            PopDistrictByState(vStateId, "CoApp1Per");
        }
        protected void ddlCoApp1CurrState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vStateId = Convert.ToInt32(ddlCoApp1CurrState.SelectedValue);
            PopDistrictByState(vStateId, "CoApp1Curr");
        }
        protected void ddlCoApp2PerState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vStateId = Convert.ToInt32(ddlCoApp2PerState.SelectedValue);
            PopDistrictByState(vStateId, "CoApp2Per");
        }
        protected void ddlCoApp2CurrState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vStateId = Convert.ToInt32(ddlCoApp2CurrState.SelectedValue);
            PopDistrictByState(vStateId, "CoApp2Curr");
        }
        protected void ddlCoApp3PerState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vStateId = Convert.ToInt32(ddlCoApp3PerState.SelectedValue);
            PopDistrictByState(vStateId, "CoApp3Per");
        }
        protected void ddlCoApp3CurrState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vStateId = Convert.ToInt32(ddlCoApp3CurrState.SelectedValue);
            PopDistrictByState(vStateId, "CoApp3Curr");
        }
        protected void ddlCoApp4PerState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vStateId = Convert.ToInt32(ddlCoApp4PerState.SelectedValue);
            PopDistrictByState(vStateId, "CoApp4Per");
        }
        protected void ddlCoApp4CurrState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vStateId = Convert.ToInt32(ddlCoApp4CurrState.SelectedValue);
            PopDistrictByState(vStateId, "CoApp4Curr");
        }
        protected void ddlGuarPerState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vStateId = Convert.ToInt32(ddlGuarPerState.SelectedValue);
            PopDistrictByState(vStateId, "GuarPer");
        }
        protected void ddlGuarCurrState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vStateId = Convert.ToInt32(ddlGuarCurrState.SelectedValue);
            PopDistrictByState(vStateId, "GuarCurr");
        }
        private string SaveMemberImages(FileUpload flup, string imageGroup, string imageName, string ImgExt, string isImageSaved)
        {
            CApiCalling oAC = new CApiCalling();
            byte[] ImgByte = ConvertFileToByteArray(flup.PostedFile);
            isImageSaved = oAC.UploadFileMinio(ImgByte, imageName + ImgExt, imageGroup, CFDocumentBucket, MinioUrl);
            return isImageSaved;
        }
        protected void btnVerifyAppPan_Click(object sender, EventArgs e)
        {
            VerifyPanNo(txtApplPan, btnVerifyAppPan, "A", hdnAppPanVerifyYN);
        }
        protected void btnVerifyCoApp1Pan_Click(object sender, EventArgs e)
        {
            VerifyPanNo(txtCoApp1Pan, btnVerifyCoApp1Pan, "CA1", hdnCoApp1PanVerifyYN);
        }
        protected void btnVerifyCoApp2Pan_Click(object sender, EventArgs e)
        {
            VerifyPanNo(txtCoApp2Pan, btnVerifyCoApp2Pan, "CA2", hdnCoApp2PanVerifyYN);
        }
        protected void btnVerifyCoApp3Pan_Click(object sender, EventArgs e)
        {
            VerifyPanNo(txtCoApp3Pan, btnVerifyCoApp3Pan, "CA3", hdnCoApp3PanVerifyYN);
        }
        protected void btnVerifyCoApp4Pan_Click(object sender, EventArgs e)
        {
            VerifyPanNo(txtCoApp4Pan, btnVerifyCoApp4Pan, "CA4", hdnCoApp4PanVerifyYN);
        }
        protected void btnVerifyGuarPan_Click(object sender, EventArgs e)
        {
            VerifyPanNo(txtGuarPan, btnVerifyGuarPan, "G", hdnGuarPanVerifyYN);
        }
        public void VerifyPanNo(TextBox txtPan, Button btnVerify, string vCustType, HiddenField hdnVerifyPanYN)
        {
            if (txtPan.Text == "")
            {
                gblFuction.AjxMsgPopup("Please Enter Pan No.");
                txtPan.Focus();
                return;
            }

            if ((ddlCustomer.SelectedValue == "" || ddlCustomer.SelectedValue == "-1") && vCustType == "A")
            {
                gblFuction.AjxMsgPopup("Please Select Customer.");
                ddlCustomer.Focus();
                return;
            }

            string pPanNo = txtPan.Text;
            string vErrMsg = string.Empty; string pRequestXml = "";
            string requestBody = "{\"consent\":\"" + "Y" + "\",\"pan\":\"" + pPanNo + "\"}";

            pRequestXml = AsString(JsonConvert.DeserializeXmlNode(requestBody, "root"));

            string postURL = vKarzaEnv == "PROD" ? "https://api.karza.in/v2/pan" : "https://testapi.karza.in/v2/pan";
            string pPanResponseXml = "", vStatusCode = "", vReqId = "", vName = "";
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("x-karza-key", vKarzaEnv == "PROD" ? vKarzaKey : KarzaKeyUat);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(requestBody);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                fullResponse = fullResponse.Replace("\u0000", "");
                fullResponse = fullResponse.Replace("\\u0000", "");
                //fullResponse = "{\"result\":{\"name\":\"SUBRATA KUMAR PAUL\"},\"request_id\":\"cddc5da3-2a47-48c3-b28c-634ba3bee200\",\"status-code\":101}";
                dynamic res = JsonConvert.DeserializeObject(fullResponse.Replace("status-code", "status_code"));

                pPanResponseXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
                vStatusCode = Convert.ToString(res.status_code);
                vReqId = Convert.ToString(res.request_id);

                if (vStatusCode == "101")
                {
                    string Name = Convert.ToString(res.result.name);
                    //vErrMsg = "101:Success " + txtCustName.Text + "\n" + "Request:" + "\n" + pRequestXml + "\n" + "Response:" + "\n" + pPanResponseXml;  
                    vErrMsg = "101:Success " + Name + "\n" + "Request:" + "\n" + pRequestXml + "\n" + "Response:" + "\n" + pPanResponseXml;
                    SavePanResponse(Convert.ToInt64(hdnLeadId.Value), vCustType, pPanNo, vReqId, pPanResponseXml, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                    btnVerify.Enabled = false;
                    txtPan.Enabled = false;
                    hdnVerifyPanYN.Value = "Y";

                }
                else
                {
                    vErrMsg = "Failed:Enter Valid Pan! " + txtCustName.Text + "\n" + "Request:" + "\n" + pRequestXml + "\n" + "Response:" + "\n" + pPanResponseXml;
                    SavePanResponse(Convert.ToInt64(hdnLeadId.Value), vCustType, pPanNo, vReqId, pPanResponseXml, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                    btnVerify.Enabled = true;
                    txtPan.Enabled = true;
                    hdnVerifyPanYN.Value = "N";

                }
                gblFuction.AjxMsgPopup(vErrMsg);

            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    // Server is down or not reachable
                    vErrMsg = "Unable to connect to API server.";
                }
                else if (ex.Status == WebExceptionStatus.Timeout)
                {
                    // Request timed out
                    vErrMsg = "The request timed out.";
                }
                else if (ex.Response != null)
                {
                    // Handle specific HTTP error response (like 500, 404, etc.)
                    var httpResponse = (HttpWebResponse)ex.Response;                   
                    vErrMsg = "HTTP Error: " + (int)httpResponse.StatusCode + httpResponse.StatusDescription;
                }
                else
                {
                    // General error
                    vErrMsg = "Unable To Connect To The Server...";
                }
                SavePanResponse(Convert.ToInt64(hdnLeadId.Value), vCustType, pPanNo, vReqId, pPanResponseXml, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                btnVerify.Enabled = true;
                txtPan.Enabled = true;
                hdnVerifyPanYN.Value = "N";
                gblFuction.MsgPopup(vErrMsg);
            }
            catch (Exception ex)
            {
                vErrMsg = "Unable To Connect To The Server...";
                SavePanResponse(Convert.ToInt64(hdnLeadId.Value), vCustType, pPanNo, vReqId, pPanResponseXml, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                btnVerify.Enabled = true;
                txtPan.Enabled = true;
                hdnVerifyPanYN.Value = "N";
                gblFuction.MsgPopup(vErrMsg);
            }
            finally { }
            //catch (WebException exp)
            //{
            //    string Response = ""; string error = "";
            //    using (var stream = exp.Response.GetResponseStream())
            //    using (var reader = new StreamReader(stream))
            //    {
            //        Response = reader.ReadToEnd();
            //    }
            //    dynamic res = JsonConvert.DeserializeObject(Response.Replace("status", "status"));
            //    Response = Response.Replace("\u0000", "");
            //    Response = Response.Replace("\\u0000", "");
            //    pPanResponseXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));

            //    vStatusCode = Convert.ToString(res.status);
            //    error = Convert.ToString(res.error);
            //    vReqId = Convert.ToString(res.request_id);
            //    vErrMsg = vStatusCode + ":" + error;

            //    vErrMsg = vErrMsg + "\n" + "Request:" + "\n" + pRequestXml + "\n" + "Response:" + "\n" + pPanResponseXml;
            //    try
            //    {
            //        SavePanResponse(Convert.ToInt64(hdnLeadId.Value), vCustType, pPanNo, vReqId, pPanResponseXml, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
            //        btnVerify.Enabled = true;
            //        txtPan.Enabled = true;
            //        hdnVerifyPanYN.Value = "N";
            //        gblFuction.MsgPopup(vErrMsg);
            //    }

            //    finally { }

            //}

        }
        protected void btnVerifyAppAadhaar_Click(object sender, EventArgs e)
        {
            VerifyAadhNo(txtApplAadhNo, btnVerifyAppAadhaar, ddlCustomer, txtApplAadhRefNo, "A", hdnAppAadhVerifyYN);
        }
        protected void btnVerifyCoApp1Aadhaar_Click(object sender, EventArgs e)
        {
            VerifyAadhNo(txtCoApp1AadhNo, btnVerifyCoApp1Aadhaar, ddlCustomer, txtCoApp1AadhRefNo, "CA1", hdnCoApp1AadhVerifyYN);
        }
        protected void btnVerifyCoApp2Aadhaar_Click(object sender, EventArgs e)
        {
            VerifyAadhNo(txtCoApp2AadhNo, btnVerifyCoApp2Aadhaar, ddlCustomer, txtCoApp2AadhRefNo, "CA2", hdnCoApp2AadhVerifyYN);
        }
        protected void btnVerifyCoApp3Aadhaar_Click(object sender, EventArgs e)
        {
            VerifyAadhNo(txtCoApp3AadhNo, btnVerifyCoApp3Aadhaar, ddlCustomer, txtCoApp3AadhRefNo, "CA3", hdnCoApp3AadhVerifyYN);
        }
        protected void btnVerifyCoApp4Aadhaar_Click(object sender, EventArgs e)
        {
            VerifyAadhNo(txtCoApp4AadhNo, btnVerifyCoApp4Aadhaar, ddlCustomer, txtCoApp4AadhRefNo, "CA4", hdnCoApp4AadhVerifyYN);
        }
        protected void btnVerifyGuarAadhaar_Click(object sender, EventArgs e)
        {
            VerifyAadhNo(txtGuarAadhNo, btnVerifyGuarAadhaar, ddlCustomer, txtGuarAadhRefNo, "G", hdnGuarAadhVerifyYN);
        }
        public void VerifyAadhNo(TextBox txtAadh, Button btnVerify, DropDownList ddlCustomer, TextBox txtAadhRef, string pCustType, HiddenField hdnVerifyAadhYN)
        {
            if (txtAadh.Text == "" || txtAadh.Text.Trim().Length != 12)
            {
                gblFuction.AjxMsgPopup("Please Enter 12 Digit Aadhaar No.");
                txtAadh.Focus();
                hdnVerifyAadhYN.Value = "N";
                return;
            }
            if ((ddlCustomer.SelectedValue == "" || ddlCustomer.SelectedValue == "-1") && pCustType == "A")
            {
                gblFuction.AjxMsgPopup("Please Select Customer.");
                ddlCustomer.Focus();
                hdnVerifyAadhYN.Value = "N";
                return;
            }
            try
            {
                string pRequest = ""; string pResponse = "";
                string vApplAadharNo = txtAadh.Text;
                string vApplMaskedAadhaar = string.Empty, vApplAadhaarRefNo = "";
                string vErrMsg = string.Empty;
                AadhaarVaultResponse vAadharVaultResponse = null;
                vApplMaskedAadhaar = String.Format("{0}{1}", "********", vApplAadharNo.Substring(vApplAadharNo.Length - 4, 4));
                AadhaarVault vAadhaarVault = new AadhaarVault();
                vAadhaarVault.refData = vApplAadharNo;
                vAadhaarVault.refDataType = "U";
                vAadhaarVault.pLeadId = ddlCustomer.SelectedValue;
                vAadhaarVault.pCustType = pCustType;
                vAadhaarVault.pCreatedBy = Convert.ToString(Session[gblValue.UserId]);
                vAadharVaultResponse = AadhaarVault(vAadhaarVault, hdnVerifyAadhYN, out pRequest, out pResponse);
                try
                {
                    if (vAadharVaultResponse.response_code == 1)
                    {
                        vApplAadhaarRefNo = Convert.ToString(vAadharVaultResponse.results[0]);

                        if (vApplAadhaarRefNo != "")
                        {
                            txtAadhRef.Text = Convert.ToString(vAadharVaultResponse.results[0]);
                            btnVerify.Enabled = hdnVerifyAadhYN.Value == "Y" ? false : true;
                            txtAadh.Enabled = hdnVerifyAadhYN.Value == "Y" ? false : true;
                            if (hdnVerifyAadhYN.Value == "Y")
                            {
                                gblFuction.AjxMsgPopup("Aadhaar Verified Successfully!" + txtCustName.Text);
                                return;
                            }
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Failed:Aadhaar Vault API Returns Error:" + "\n" + "Request:" + "\n" + pRequest + "\n" + "Response:" + "\n" + pResponse);
                            return;
                        }
                    }
                    else
                    {
                        string ResMsg = Convert.ToString(vAadharVaultResponse.response_message);
                        gblFuction.AjxMsgPopup("Failed:Aadhaar Vault API Returns Error:" + "\n" + "Request:" + "\n" + pRequest + "\n" + "Response:" + "\n" + pResponse);
                        return;
                    }
                }
                catch (WebException ex)
                {
                    string pErrMsg = "";
                    if (ex.Status == WebExceptionStatus.ConnectFailure)
                    {
                        // Server is down or not reachable
                        pErrMsg = "Unable to connect to API server.";
                    }
                    else if (ex.Status == WebExceptionStatus.Timeout)
                    {
                        // Request timed out
                        pErrMsg = "The request timed out.";
                    }
                    else if (ex.Response != null)
                    {
                        // Handle specific HTTP error response (like 500, 404, etc.)
                        var httpResponse = (HttpWebResponse)ex.Response;
                        pErrMsg = "HTTP Error: " + (int)httpResponse.StatusCode + httpResponse.StatusDescription;
                    }
                    else
                    {
                        // General error
                        pErrMsg = "Unable To Connect To The Server...";
                    }
                    gblFuction.AjxMsgPopup(vErrMsg);
                }
                catch (Exception ex)
                {
                    gblFuction.AjxMsgPopup("Unable To Connect To The Server...");
                }
            }
            catch (WebException ex)
            {
                string vErrMsg = "";
                if (ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    // Server is down or not reachable
                    vErrMsg = "Unable to connect to API server.";
                }
                else if (ex.Status == WebExceptionStatus.Timeout)
                {
                    // Request timed out
                    vErrMsg = "The request timed out.";
                }
                else if (ex.Response != null)
                {
                    // Handle specific HTTP error response (like 500, 404, etc.)
                    var httpResponse = (HttpWebResponse)ex.Response;                   
                    vErrMsg = "HTTP Error: " + (int)httpResponse.StatusCode + httpResponse.StatusDescription;
                }
                else
                {
                    // General error
                    vErrMsg = "Unable To Connect To The Server...";
                }              
                gblFuction.AjxMsgPopup(vErrMsg);
            }
            catch (Exception ex)
            {              
                gblFuction.AjxMsgPopup("Unable To Connect To The Server...");
            }
            //catch (WebException ex)
            //{
            //    if (ex.Response != null)
            //    {
            //        using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
            //        {
            //            string StatusDesc = errorResponse.StatusDescription;
            //            gblFuction.AjxMsgPopup("Connection Failed to Aadhaar API:" + StatusDesc);
            //            return;
            //        }
            //    }

            //}
        }
        protected void btnVerifyAppVoter_Click(object sender, EventArgs e)
        {
            VoterVerify(txtApplVoterNo, btnVerifyAppVoter, "A", hdnAppVoterVerifyYN);
        }
        protected void btnVerifyCoApp1Voter_Click(object sender, EventArgs e)
        {
            VoterVerify(txtCoApp1Voter, btnVerifyCoApp1Voter, "CA1", hdnCoApp1VoterVerifyYN);
        }
        protected void btnVerifyCoApp2Voter_Click(object sender, EventArgs e)
        {
            VoterVerify(txtCoApp2Voter, btnVerifyCoApp2Voter, "CA2", hdnCoApp2VoterVerifyYN);
        }
        protected void btnVerifyCoApp3Voter_Click(object sender, EventArgs e)
        {
            VoterVerify(txtCoApp3Voter, btnVerifyCoApp3Voter, "CA3", hdnCoApp3VoterVerifyYN);
        }
        protected void btnVerifyCoApp4Voter_Click(object sender, EventArgs e)
        {
            VoterVerify(txtCoApp4Voter, btnVerifyCoApp4Voter, "CA4", hdnCoApp4VoterVerifyYN);
        }
        protected void btnVerifyGuarVoter_Click(object sender, EventArgs e)
        {
            VoterVerify(txtGuarVoter, btnVerifyGuarVoter, "G", hdnGuarVoterVerifyYN);
        }
        public void VoterVerify(TextBox txtVoterNo, Button btnVerify, string vCustType, HiddenField hdnVerifyVoterYN)
        {
            if (txtVoterNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Please Enter Voter No.");
                txtVoterNo.Focus();
                return;
            }

            if ((ddlCustomer.SelectedValue == "" || ddlCustomer.SelectedValue == "-1") && vCustType == "A")
            {
                gblFuction.AjxMsgPopup("Please Select Customer.");
                ddlCustomer.Focus();
                return;
            }

            CCust360 oNm = null;
            Int32 vEoId = Convert.ToInt32(Session[gblValue.UserId]);
            string vBranch = Convert.ToString(Session[gblValue.BrnchCode]);
            oNm = new CCust360();
            string vErrMsg = string.Empty;
            string vXml = "";
            var req = new KYCVoterRequest()
            {
                consent = "Y",
                epic_no = txtVoterNo.Text
            };
            string Requestdata = JsonConvert.SerializeObject(req);

            //string postURL = "https://testapi.karza.in/v2/voter";
            string postURL = vKarzaEnv == "PROD" ? "https://api.karza.in/v2/voter" : "https://testapi.karza.in/v2/voter";
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                // Set up the request properties.
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("x-karza-key", vKarzaEnv == "PROD" ? vKarzaKey : KarzaKeyUat);
                request.Host = vKarzaEnv == "PROD" ? "api.karza.in" : "testapi.karza.in";

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                string responsedata = string.Empty;

                byte[] data = Encoding.UTF8.GetBytes(Requestdata);
                request.ContentLength = data.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                {
                    var API_Response = streamReader.ReadToEnd(); ;
                    responsedata = API_Response.ToString().Trim();
                }
                // responsedata = "{\"result\":{\"name\":\"POOJA ASHOK PAWAR\",\"rln_name\":\"ASHOK PAWAR\",\"rln_type\":\"H\",\"gender\":\"F\",\"district\":\"MumbaiSuburban\",\"ac_name\":\"Kalina\",\"pc_name\":\"Mumbai North-Central\",\"state\":\"Maharashtra\",\"epic_no\":\"TMF3673688\",\"dob\":\"\",\"age\":29,\"part_no\":\"42\",\"slno_inpart\":\"501\",\"ps_name\":\"Mary Immaculate High School Ground Floor Room No 3 Kalina Church Marg Santacruz (E) Mumbai -400029\",\"part_name\":\"Kalina Church Road, Santacruz (East), Mumbai 400 098.\",\"last_update\":\"08-04-2021\",\"ps_lat_long\":\"0.0,0.0\",\"rln_name_v1\":\"\\u0905\\u0936\\u094b\\u0915 \\u092a\\u0935\\u093e\\u0930\",\"rln_name_v2\":\"\",\"rln_name_v3\":\"\",\"section_no\":\"11\",\"id\":\"S131750042110501\",\"name_v1\":\"\\u092a\\u0941\\u091c\\u093e \\u0905\\u0936\\u094b\\u0915 \\u092a\\u0935\\u093e\\u0930\",\"name_v2\":\"\",\"name_v3\":\"\",\"ac_no\":\"175\",\"st_code\":\"S13\",\"house_no\":\"\"},\"request_id\":\"2e494b33-cf53-4dd0-a0e6-eb5f4b3a7a28\",\"status-code\":\"101\"}";

                KYCVoterIDResponse vResponseObj = new KYCVoterIDResponse();
                vResponseObj = JsonConvert.DeserializeObject<KYCVoterIDResponse>(responsedata.Replace("status-code", "status_code"));
                try
                {
                    responsedata = responsedata.Replace("\u0000", "");
                    responsedata = responsedata.Replace("\\u0000", "");
                    vXml = AsString(JsonConvert.DeserializeXmlNode(responsedata.Replace("status-code", "status_code"), "root"));
                    oNm.SaveKarzaVoterVerifyData(Convert.ToInt64(hdnLeadId.Value), vCustType, txtVoterNo.Text, vXml, vBranch, vEoId);//Save Response
                }
                finally
                {
                    //---
                }

                if (vResponseObj.status_code == "101")
                {
                    string Name = Convert.ToString(vResponseObj.result.name);
                    //vErrMsg = "101:Valid Authentication " + txtCustName.Text;
                    vErrMsg = "101:Valid Authentication " + Name;
                    //dynamic res = NameMatching(ddlMember.SelectedItem.Text, vResponseObj.result.name);
                    //string vNameJson = JsonConvert.SerializeObject(res);
                    //string vNmXml = "<root>" + AsString(JsonConvert.DeserializeXmlNode(vNameJson, "NameMatchingResult")) + "</root>";
                    string vNmXml = "";
                    oNm = new CCust360();
                    vXml = "<root>" + vXml.Replace("root", "KarzaVoterIDKYCValidationResult") + "</root>";
                    //oNm.UpDateOCRData(Convert.ToString(Session["EnquiryId"]), vXml, vNmXml, "A");
                }

                else if (vResponseObj.status_code == "102")
                {
                    vErrMsg = "102:Invalid ID number or combination of inputs";
                }
                else if (vResponseObj.status_code == "103")
                {
                    vErrMsg = "103:No records found for the given ID or combination of inputs";
                    // oNm = new CNewMember();
                    // oNm.SaveRedFlag(ddlMember.SelectedValue, Convert.ToInt32(Session[gblValue.UserId]));
                }
                else if (vResponseObj.status_code == "104")
                {
                    vErrMsg = "104:Max retries exceeded";
                }
                else if (vResponseObj.status_code == "105")
                {
                    vErrMsg = "105:Missing Consent";
                }
                else if (vResponseObj.status_code == "106")
                {
                    vErrMsg = "106:Multiple Records Exist";
                }
                else if (vResponseObj.status_code == "107")
                {
                    vErrMsg = "107:Not Supported";
                }
                else
                {
                    vErrMsg = vResponseObj.status_code;
                }
                //  vResponseObj.status_code = vErrMsg;

                if (vResponseObj.status_code == "101")
                {
                    btnVerify.Enabled = false;
                    txtVoterNo.Enabled = false;
                    hdnVerifyVoterYN.Value = "Y";

                }
                else
                {
                    txtVoterNo.Enabled = true;
                    btnVerify.Enabled = true;
                    hdnVerifyVoterYN.Value = "N";
                }
                if (hdnVerifyVoterYN.Value == "Y")
                {
                    if (vCustType == "A")
                    {
                        txtApplVoterNo.Enabled = false;
                        btnVerifyAppVoter.Enabled = false;
                    }
                    else if (vCustType == "CA1")
                    {
                        txtApplVoterNo.Enabled = false;
                        btnVerifyCoApp1Voter.Enabled = false;
                    }
                    else if (vCustType == "CA2")
                    {
                        txtCoApp2Voter.Enabled = false;
                        btnVerifyCoApp2Voter.Enabled = false;
                    }
                    else if (vCustType == "G")
                    {
                        txtGuarVoter.Enabled = false;
                        btnVerifyGuarVoter.Enabled = false;
                    }
                }
                else if (hdnVerifyVoterYN.Value == "N")
                {
                    if (vCustType == "A")
                    {
                        txtApplVoterNo.Enabled = true;
                        btnVerifyAppVoter.Enabled = true;
                    }
                    else if (vCustType == "CA1")
                    {
                        txtApplVoterNo.Enabled = true;
                        btnVerifyCoApp1Voter.Enabled = true;
                    }
                    else if (vCustType == "CA2")
                    {
                        txtCoApp2Voter.Enabled = true;
                        btnVerifyCoApp2Voter.Enabled = true;
                    }
                    else if (vCustType == "G")
                    {
                        txtGuarVoter.Enabled = true;
                        btnVerifyGuarVoter.Enabled = true;
                    }
                }
                gblFuction.AjxMsgPopup(vErrMsg);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    // Server is down or not reachable
                    vErrMsg = "Unable to connect to API server.";
                }
                else if (ex.Status == WebExceptionStatus.Timeout)
                {
                    // Request timed out
                    vErrMsg = "The request timed out." ;
                }
                else if (ex.Response != null)
                {
                    // Handle specific HTTP error response (like 500, 404, etc.)
                    var httpResponse = (HttpWebResponse)ex.Response;
                    // Console.WriteLine($"HTTP Error: {(int)httpResponse.StatusCode} - {httpResponse.StatusDescription}");
                    vErrMsg = "HTTP Error: " + (int)httpResponse.StatusCode + httpResponse.StatusDescription;
                }
                else
                {
                    // General error
                    vErrMsg = "Unable To Connect To The Server...";
                }
                oNm.SaveKarzaVoterVerifyData(Convert.ToInt64(hdnLeadId.Value), vCustType, txtVoterNo.Text, vXml, vBranch, vEoId);
                txtVoterNo.Enabled = true;
                btnVerify.Enabled = true;
                hdnVerifyVoterYN.Value = "N";
                gblFuction.AjxMsgPopup(vErrMsg);
            }
            catch (Exception ex)
            {
                oNm.SaveKarzaVoterVerifyData(Convert.ToInt64(hdnLeadId.Value), vCustType, txtVoterNo.Text, vXml, vBranch, vEoId);
                vErrMsg = "Unable To Connect To The Server...";
                txtVoterNo.Enabled = true;
                btnVerify.Enabled = true;
                hdnVerifyVoterYN.Value = "N";
                gblFuction.AjxMsgPopup(vErrMsg);
            }
            //catch (WebException e)
            //{
            //    //string Response = "";
            //    //using (var stream = we.Response.GetResponseStream())
            //    //using (var reader = new StreamReader(stream))
            //    //{
            //    //    Response = reader.ReadToEnd();
            //    //}

            //    string Response = "";
            //    using (WebResponse response = e.Response)
            //    {
            //        // TODO: Handle response being null
            //        HttpWebResponse httpResponse = (HttpWebResponse)response;

            //        using (Stream data = response.GetResponseStream())
            //        using (var reader = new StreamReader(data))
            //        {
            //            Response = reader.ReadToEnd();
            //        }

            //        Response = Response.Replace("status", "status_code");
            //        vXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
            //        oNm.SaveKarzaVoterVerifyData(Convert.ToInt64(hdnLeadId.Value), vCustType, txtVoterNo.Text, vXml, vBranch, vEoId);

            //        Response.Replace("requestId", "request_id");
            //        KYCVoterIDResponse vResponseObj = new KYCVoterIDResponse();
            //        vResponseObj = JsonConvert.DeserializeObject<KYCVoterIDResponse>(Response);

            //        //HttpWebResponse res = (HttpWebResponse)we.Response;
            //        //KYCVoterIDResponse vResponseObj = new KYCVoterIDResponse();

            //        if (Convert.ToString(vResponseObj.status_code) == "400")
            //        {
            //            vErrMsg = "400:Bad Request";
            //        }
            //        else if (Convert.ToString(vResponseObj.status_code) == "401")
            //        {
            //            vErrMsg = "401:Unauthorized Access";
            //        }
            //        else if (Convert.ToString(vResponseObj.status_code) == "402")
            //        {
            //            vErrMsg = "402:Insufficient Credits";
            //        }
            //        else if (Convert.ToString(vResponseObj.status_code) == "500")
            //        {
            //            vErrMsg = "500:Internal Server Error";
            //        }
            //        else if (Convert.ToString(vResponseObj.status_code) == "503")
            //        {
            //            vErrMsg = "503:Source Unavailable";
            //        }
            //        else if (Convert.ToString(vResponseObj.status_code) == "504")
            //        {
            //            vErrMsg = "504:Endpoint Request Timed Out";
            //        }
            //        else
            //        {
            //            vErrMsg = Convert.ToString("Unable To Connect To The Server...");
            //        }
            //    }
            //    txtVoterNo.Enabled = true;
            //    btnVerify.Enabled = true;
            //    hdnVerifyVoterYN.Value = "N";
            //    gblFuction.AjxMsgPopup(vErrMsg);
            //}
            finally
            {
                // streamWriter = null;
            }
        }
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
        public AadhaarVaultResponse AadhaarVault(AadhaarVault AadhaarVault, HiddenField hdnVerifyAadhYN, out string pRequest, out string pResponse)
        {
            string postURL = vKarzaEnv == "PROD" ? "https://avault.unitybank.co.in/vault/insert" : "https://avaultuat.unitybank.co.in/vault/insert";
            string vAadhaarNo = Convert.ToString(AadhaarVault.refData);
            CCust360 obj = null;
            string vMaskedAadhaar = String.Format("{0}{1}", "********", vAadhaarNo.Substring(vAadhaarNo.Length - 4, 4));
            try
            {
                string requestBody = JsonConvert.SerializeObject(AadhaarVault);
                string vRequestXml = AsString(JsonConvert.DeserializeXmlNode(requestBody, "root"));
                pRequest = vRequestXml;
                var vStrMycertPub = System.Web.Hosting.HostingEnvironment.MapPath("~/Certificate/adharvault_unity.pfx");
                string password = "3652145879";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                X509Certificate2Collection certificates = new X509Certificate2Collection();
                certificates.Import(vStrMycertPub, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.AllowAutoRedirect = true;
                request.ClientCertificates = certificates;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("cache-control", "no-cache");
                request.Headers.Add("signed-data", signRequest(requestBody));
                request.Headers.Add("x-key", "9653214879");

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(requestBody);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
                pResponse = vResponseXml;

                obj = new CCust360();
                obj.SaveAadhaarVaultLog(vMaskedAadhaar, Convert.ToInt32(AadhaarVault.pCreatedBy), vRequestXml, vResponseXml, Convert.ToInt64(AadhaarVault.pLeadId), AadhaarVault.pCustType);
                //-------------------------------------------------------------------------------------
                AadhaarVaultResponse myDeserializedClass = JsonConvert.DeserializeObject<AadhaarVaultResponse>(fullResponse);
                hdnVerifyAadhYN.Value = "Y";
                return myDeserializedClass;
            }
            catch (WebException ex)
            {
                string requestBody = JsonConvert.SerializeObject(AadhaarVault);
                string vRequestXml = AsString(JsonConvert.DeserializeXmlNode(requestBody, "root"));
                pRequest = vRequestXml;

                string Response = "";
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Response = reader.ReadToEnd();
                }
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
                pResponse = vResponseXml;
                obj = new CCust360();
                obj.SaveAadhaarVaultLog(vMaskedAadhaar, Convert.ToInt32(AadhaarVault.pCreatedBy), vRequestXml, vResponseXml, Convert.ToInt64(AadhaarVault.pLeadId), AadhaarVault.pCustType);
                //-------------------------------------------------------------------------------------
                AadhaarVaultResponse myDeserializedClass = null;
                try
                {
                    myDeserializedClass = JsonConvert.DeserializeObject<AadhaarVaultResponse>(Response);
                }
                catch
                {
                    myDeserializedClass = new AadhaarVaultResponse("", 0, "", 0, null);
                }
                hdnVerifyAadhYN.Value = "N";
                return myDeserializedClass;
            }
            finally
            {
                // streamWriter = null;
            }
        }
        private string signRequest(string text)
        {
            string dataString = text;
            byte[] originalData = System.Text.Encoding.Default.GetBytes(dataString);
            byte[] signedData;
            var vStrMycertPub = System.Web.Hosting.HostingEnvironment.MapPath("~/Certificate/adharvault_unity.pfx");
            X509Certificate2 certificate = new X509Certificate2(vStrMycertPub, "3652145879",
               X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable
             );

            try
            {
                RSACryptoServiceProvider RSAalg = certificate.PrivateKey as RSACryptoServiceProvider;
                RSAParameters Key = RSAalg.ExportParameters(true);
                signedData = HashAndSignBytes(originalData, Key);
                if (VerifySignedHash(originalData, signedData, Key))
                {
                    Console.WriteLine("The data was verified.");
                }
                else
                {
                    Console.WriteLine("The data does not match the signature.");
                }
                return Convert.ToBase64String(signedData);
            }
            catch (WebException ex)
            {
                using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    string errorResponse = reader.ReadToEnd();
                    gblFuction.AjxMsgPopup("Error : {errorResponse}");
                    return null;
                }
            }
        }
        public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg.ImportParameters(Key);
                return RSAalg.SignData(DataToSign, SHA256.Create());
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
        {
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg.ImportParameters(Key);
                return RSAalg.VerifyData(DataToVerify, SHA256.Create(), SignedData);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
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
        public string SavePanResponse(Int64 pLeadID, string vCustType, string pPanNo, string pRequestId, string pPanResponseXml, DateTime pLoginDt)
        {
            CCust360 oCa = new CCust360();
            string vErrDesc = oCa.SavePanResponse(pLeadID, vCustType, pPanNo, pRequestId, pPanResponseXml, pLoginDt);
            return vErrDesc;
        }
        public bool ValidUrlChk(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
                request.Timeout = 5000;
                using (WebResponse response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #region URLToByte
        public byte[] DownloadByteImage(string URL)
        {
            byte[] imgByte = null;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var wc = new WebClient())
                {
                    imgByte = wc.DownloadData(URL);
                }
            }
            finally { }
            return imgByte;
        }
        #endregion
        public string GetBase64Image(string pImageName, string pLeadId)
        {
            string ActNetImage = "", base64image = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = DocumentBucketURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + pLeadId + "_" + pImageName;
                    if (ValidUrlChk(ActNetImage))
                    {
                        imgByte = DownloadByteImage(ActNetImage);
                        base64image = Convert.ToBase64String(imgByte);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return base64image;
        }

        [WebMethod]
        public static List<Pin> GetPincode(string Pin)
        {
            DataTable dt = new DataTable();
            CCust360 oCa = new CCust360();
            List<Pin> empResult = new List<Pin>();
            dt = oCa.PopPincode(Pin);
            foreach (DataRow dR in dt.Rows)
            {
                Pin oP = new Pin
                {
                    Pincode = Convert.ToString(dR["Pincode"]),
                    StateId = Convert.ToInt32(dR["StateId"])
                };
                empResult.Add(oP);

            }
            return empResult;
        }

        public class Pin
        {
            public string Pincode { get; set; }
            public int StateId { get; set; }
        }

        [WebMethod]
        public static List<Dist> GetDist(string StateID)
        {
            DataTable dt = new DataTable();
            CUser oMem = new CUser();
            List<Dist> distResult = new List<Dist>();
            dt = oMem.CF_GetDisctrictByState(Convert.ToInt32(StateID));
            foreach (DataRow dR in dt.Rows)
            {
                Dist oP = new Dist
                {
                    DistId = Convert.ToInt32(dR["DistrictId"]),
                    DistName = Convert.ToString(dR["DistrictName"])
                };
                distResult.Add(oP);

            }
            return distResult;
        }

        public class Dist
        {
            public string DistName { get; set; }
            public int DistId { get; set; }
        }

        protected void ViewImgDoc(string ID, string FileName, string CustType, string Ext)
        {
            string vBase64String = "";

            string ActNetImage = "", vPdfFile = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + ID + "_" + CustType + "_" + FileName + Ext;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image(CustType + "_" + FileName + Ext, ID);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + ID + "_" + CustType + "_" + FileName + Ext);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }

            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        protected void ImgOwnshipPhoto_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "ApplOwnship", "A", hdnAppOwnshipExt.Value);
        }
        protected void imgCoApp1OwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "CA1", hdnCoApp1OwnShipExt.Value);
        }
        protected void imgCoApp2OwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "CA2", hdnCoApp2OwnShipExt.Value);
        }
        protected void imgCoApp3OwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "CA3", hdnCoApp3OwnShipExt.Value);
        }
        protected void imgCoApp4OwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "CA4", hdnCoApp4OwnShipExt.Value);
        }
        protected void imgGuarOwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "G", hdnGuarOwnShipExt.Value);
        }
    }

    public class KYCVoterRequest
    {
        public string consent { get; set; }
        public string epic_no { get; set; }
    }

    public class KYCVoterIDResponse
    {
        public string request_id { get; set; }
        public string status_code { get; set; }
        public KYCVoterIDResponseResult result { get; set; }
    }

    public class KYCVoterIDResponseResult
    {
        public string ps_lat_long { get; set; }
        public string rln_name_v1 { get; set; }
        public string rln_name_v2 { get; set; }
        public string rln_name_v3 { get; set; }
        public string part_no { get; set; }
        public string rln_type { get; set; }
        public string section_no { get; set; }
        public string id { get; set; }
        public string epic_no { get; set; }
        public string rln_name { get; set; }
        public string district { get; set; }
        public string last_update { get; set; }
        public string state { get; set; }
        public string ac_no { get; set; }
        public string slno_inpart { get; set; }
        public string ps_name { get; set; }
        public string pc_name { get; set; }
        public string house_no { get; set; }
        public string name { get; set; }
        public string part_name { get; set; }
        public string st_code { get; set; }
        public string gender { get; set; }
        public string age { get; set; }
        public string ac_name { get; set; }
        public string name_v1 { get; set; }
        public string dob { get; set; }
        public string name_v3 { get; set; }
        public string name_v2 { get; set; }
    }

    public class AadhaarVaultResponse
    {
        public string action { get; set; }
        public int response_code { get; set; }
        public string response_message { get; set; }
        public int total_size { get; set; }
        public int total_pages { get; set; }
        public List<string> results { get; set; }

        public AadhaarVaultResponse(string action, int response_code, string response_message, int total_size, List<string> results)
        {
            this.action = action;
            this.response_code = response_code;
            this.response_message = response_message;
            this.total_size = total_size;
            this.results = results;
        }
    }

    public class AadhaarVault
    {
        public string refData { get; set; }
        public string refDataType { get; set; }
        public string pLeadId { get; set; }
        public string pCustType { get; set; }
        public string pCreatedBy { get; set; }
    }

}