using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class BranchControl : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranchControl();
                txtDate.Text = Session[gblValue.LoginDate].ToString();
                GetBranchControlData("S");
                tbBranchCntrl.ActiveTabIndex = 0;
                StatusButton("View");
               
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Branch Control Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBranchControlMst);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnPopulate.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnPopulate.Visible = true;
                    //btnSave.Visible = true;
                    //btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnPopulate.Visible = true;
                   // btnSave.Visible = true;
                    //btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnPopulate.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Branch Control Master", false);
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
                case "Pop":
                    //EnableControl(true);
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = true; 
                    break;
                case "View":
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    // EnableControl(false);
                    break; 
            }
        }        

        private void LoadGrid()
        {
            DataTable dt = null;
            CBranch oBr = null;
            try
            {
                oBr = new CBranch();
                dt = oBr.PopBranchControlList();
                ViewState["gvMain"] = dt;
                if (dt.Rows.Count > 0)
                {
                    gvMain.DataSource = dt;
                    gvMain.DataBind();
                }
            }
            finally
            {
                dt = null;
                oBr = null;
            }
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            if (gblFuction.setDate(txtDate.Text) <= gblFuction.setDate(Convert.ToString(Session[gblValue.EndDate])))
            {
                gblFuction.AjxMsgPopup("Back Date Cannot Be Entered");
                txtDate.Text = Session[gblValue.LoginDate].ToString();
            }
        }

        protected void rblCntrlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopBranchControl();
            StatusButton("View");
        }

        protected void btnPopulate_Click(object sender, EventArgs e)
        {
            string pControlType = "";
            if (rblCntrlType.SelectedValue == "rbState")
            {
                pControlType = "S";
            }
            else if (rblCntrlType.SelectedValue == "rbBranch")
            {
                pControlType = "B";
            }
            else if (rblCntrlType.SelectedValue == "rbBcPartner")
            {
                pControlType = "BC";
            }

            CreateTrData(pControlType);
            tbBranchCntrl.ActiveTabIndex = 1;          
            StatusButton("Pop"); 
        }

        private DataTable CreateTrData(string pControlType)
        {
            DataTable dt = new DataTable();
            string vId = "";
            CUser oUsr = null;           
            int j = 0;
            for (int i = 0; i <= gvControl.Rows.Count - 1; i++)
            {
                CheckBox chkAllow = (CheckBox)gvControl.Rows[i].FindControl("chkAllow");
                Label lblName = (Label)gvControl.Rows[i].FindControl("lblName");
                string Id = gvControl.Rows[i].Cells[2].Text;
         
                    if (chkAllow.Checked == true)
                    {
                        if (vId == "")
                        {
                            vId = Id;
                        }
                        else
                        {
                            vId = vId + "," + Id;
                        }
                    }
                
            }           

            oUsr = new CUser();
            dt = oUsr.GetBranchForBrCntrl(Convert.ToInt32(Session[gblValue.UserId].ToString()), vId, pControlType);

            if (dt.Rows.Count > 0)
            {
                gvMain.DataSource = dt;
                gvMain.DataBind();

                gvControl.Enabled = false;
                rblCntrlType.Enabled = false;
                btnPopulate.Enabled = false;
            }

            return dt;
        }

        private void PopBranchControl()
        {
            string pControlType = "";
            if (rblCntrlType.SelectedValue == "rbState")
            {
                pControlType = "S";
            }
            else if (rblCntrlType.SelectedValue == "rbBranch")
            {
                pControlType = "B";
            }
            else if (rblCntrlType.SelectedValue == "rbBcPartner")
            {
                pControlType = "BC";
            }

            GetBranchControlData(pControlType);
        }

        protected void GetBranchControlData(string pControlType)
        {
            ViewState["Dtl"] = null;
            DataTable dt = null;
            CUser oUsr = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            oUsr = new CUser();
            dt = oUsr.GetBranchControlData(pControlType, Convert.ToInt32(Session[gblValue.UserId].ToString()), Convert.ToInt32(Session[gblValue.RoleId]));
            ViewState["Dtl"] = dt;

            gvControl.Columns[0].HeaderText = pControlType == "S" ? "State Name" : pControlType == "B" ? "Branch Name" : "BC Partener";
            gvControl.Columns[1].HeaderText = "Allow";
            gvControl.DataSource = dt;
            gvControl.DataBind();
        }
        private DataTable CreateGvMain()
        {
            DataTable dt = new DataTable();
            dt.TableName = "MainTbl";
            dt.Columns.Add("BranchCode", typeof(string));
            dt.Columns.Add("InitialAppJLG", typeof(string));
            dt.Columns.Add("InitialAppSARAL", typeof(string));
            dt.Columns.Add("PreDBJLG", typeof(string));
            dt.Columns.Add("PreDBMEL", typeof(string));
            dt.Columns.Add("PreDBSARAL", typeof(string));
            dt.Columns.Add("AdvCollJLG", typeof(string));
            dt.Columns.Add("AdvCollMEL", typeof(string));
            dt.Columns.Add("AdvCollSARAL", typeof(string));
            dt.Columns.Add("FreshCustJLG", typeof(string));
            dt.Columns.Add("FreshCustSARAL", typeof(string));
            dt.Columns.Add("RepeatCustJLG", typeof(string));
            dt.Columns.Add("RepeatCustSARAL", typeof(string));
            dt.Columns.Add("CashCollJLG", typeof(string));
            dt.Columns.Add("CashCollMEL", typeof(string));
            dt.Columns.Add("CashCollSARAL", typeof(string));
            dt.Columns.Add("BBPSCollJLG", typeof(string));
            dt.Columns.Add("BBPSCollMEL", typeof(string));
            dt.Columns.Add("BBPSCollSARAL", typeof(string));
            dt.Columns.Add("DigiAuthJLG", typeof(string));
            dt.Columns.Add("DigiAuthMEL", typeof(string));
            dt.Columns.Add("DigiAuthSARAL", typeof(string));
            dt.Columns.Add("ManualAuthJLG", typeof(string));
            dt.Columns.Add("ManualAuthMEL", typeof(string));
            dt.Columns.Add("ManualAuthSARAL", typeof(string));
            dt.Columns.Add("BioAuthJLG", typeof(string));
            dt.Columns.Add("BioAuthMEL", typeof(string));
            dt.Columns.Add("BioAuthSARAL", typeof(string));
            dt.Columns.Add("DeviationJLG", typeof(string));
            dt.Columns.Add("DeviationSARAL", typeof(string));
            dt.Columns.Add("CBSYN", typeof(string));

            for (int i = 0; i <= gvMain.Rows.Count - 1; i++)
            {
                CheckBox chkIAJLG = (CheckBox)gvMain.Rows[i].FindControl("chkIAJLG");
                CheckBox chkIASaral = (CheckBox)gvMain.Rows[i].FindControl("chkIASaral");
                CheckBox chkPreDBJLG = (CheckBox)gvMain.Rows[i].FindControl("chkPreDBJLG");
                CheckBox chkPreDBMEL = (CheckBox)gvMain.Rows[i].FindControl("chkPreDBMEL");
                CheckBox chkPreDBSARAL = (CheckBox)gvMain.Rows[i].FindControl("chkPreDBSARAL");
                CheckBox chkAdvCollJLG = (CheckBox)gvMain.Rows[i].FindControl("chkAdvCollJLG");
                CheckBox chkAdvCollMEL = (CheckBox)gvMain.Rows[i].FindControl("chkAdvCollMEL");
                CheckBox chkAdvCollSARAL = (CheckBox)gvMain.Rows[i].FindControl("chkAdvCollSARAL");
                CheckBox chkFreshCustJLG = (CheckBox)gvMain.Rows[i].FindControl("chkFreshCustJLG");
                CheckBox chkFreshCustSARAL = (CheckBox)gvMain.Rows[i].FindControl("chkFreshCustSARAL");
                CheckBox chkRepeatCustJLG = (CheckBox)gvMain.Rows[i].FindControl("chkRepeatCustJLG");
                CheckBox chkRepeatCustSARAL = (CheckBox)gvMain.Rows[i].FindControl("chkRepeatCustSARAL");
                CheckBox chkCashCollJLG = (CheckBox)gvMain.Rows[i].FindControl("chkCashCollJLG");
                CheckBox chkCashCollMEL = (CheckBox)gvMain.Rows[i].FindControl("chkCashCollMEL");
                CheckBox chkCashCollSARAL = (CheckBox)gvMain.Rows[i].FindControl("chkCashCollSARAL");
                CheckBox chkBBPSCollJLG = (CheckBox)gvMain.Rows[i].FindControl("chkBBPSCollJLG");
                CheckBox chkBBPSCollMEL = (CheckBox)gvMain.Rows[i].FindControl("chkBBPSCollMEL");
                CheckBox chkBBPSCollSARAL = (CheckBox)gvMain.Rows[i].FindControl("chkBBPSCollSARAL");
                CheckBox chkDigiAuthJLG = (CheckBox)gvMain.Rows[i].FindControl("chkDigiAuthJLG");
                CheckBox chkDigiAuthMEL = (CheckBox)gvMain.Rows[i].FindControl("chkDigiAuthMEL");
                CheckBox chkDigiAuthSARAL = (CheckBox)gvMain.Rows[i].FindControl("chkDigiAuthSARAL");
                CheckBox chkManualAuthJLG = (CheckBox)gvMain.Rows[i].FindControl("chkDigiAuthSARAL");
                CheckBox chkManualAuthMEL = (CheckBox)gvMain.Rows[i].FindControl("chkManualAuthMEL");
                CheckBox chkManualAuthSARAL = (CheckBox)gvMain.Rows[i].FindControl("chkManualAuthSARAL");
                CheckBox chkBioAuthJLG = (CheckBox)gvMain.Rows[i].FindControl("chkBioAuthJLG");
                CheckBox chkBioAuthMEL = (CheckBox)gvMain.Rows[i].FindControl("chkBioAuthMEL");
                CheckBox chkBioAuthSARAL = (CheckBox)gvMain.Rows[i].FindControl("chkBioAuthSARAL");
                CheckBox chkDeviationJLG = (CheckBox)gvMain.Rows[i].FindControl("chkDeviationJLG");
                CheckBox chkDeviationSARAL = (CheckBox)gvMain.Rows[i].FindControl("chkDeviationSARAL");
                CheckBox chkCBSYN = (CheckBox)gvMain.Rows[i].FindControl("chkCBSYN");
                string BranchCode = gvMain.Rows[i].Cells[31].Text;

                dt.Rows.Add();
                dt.Rows[i]["BranchCode"] = BranchCode;

                dt.Rows[i]["InitialAppJLG"] = chkIAJLG.Checked == true ? "Y" : "N";
                dt.Rows[i]["InitialAppSARAL"] = chkIASaral.Checked == true ? "Y" : "N";
                dt.Rows[i]["PreDBJLG"] = chkPreDBJLG.Checked == true ? "Y" : "N";
                dt.Rows[i]["PreDBMEL"] = chkPreDBMEL.Checked == true ? "Y" : "N";
                dt.Rows[i]["PreDBSARAL"] = chkPreDBSARAL.Checked == true ? "Y" : "N";
                dt.Rows[i]["AdvCollJLG"] = chkAdvCollJLG.Checked == true ? "Y" : "N";
                dt.Rows[i]["AdvCollMEL"] = chkAdvCollMEL.Checked == true ? "Y" : "N";
                dt.Rows[i]["AdvCollSARAL"] = chkAdvCollSARAL.Checked == true ? "Y" : "N";
                dt.Rows[i]["FreshCustJLG"] = chkFreshCustJLG.Checked == true ? "Y" : "N";
                dt.Rows[i]["FreshCustSARAL"] = chkFreshCustSARAL.Checked == true ? "Y" : "N";
                dt.Rows[i]["RepeatCustJLG"] = chkRepeatCustJLG.Checked == true ? "Y" : "N";
                dt.Rows[i]["RepeatCustSARAL"] = chkRepeatCustSARAL.Checked == true ? "Y" : "N";
                dt.Rows[i]["CashCollJLG"] = chkCashCollJLG.Checked == true ? "Y" : "N";
                dt.Rows[i]["CashCollMEL"] = chkCashCollMEL.Checked == true ? "Y" : "N";
                dt.Rows[i]["CashCollSARAL"] = chkCashCollSARAL.Checked == true ? "Y" : "N";
                dt.Rows[i]["BBPSCollJLG"] = chkBBPSCollJLG.Checked == true ? "Y" : "N";
                dt.Rows[i]["BBPSCollMEL"] = chkBBPSCollMEL.Checked == true ? "Y" : "N";
                dt.Rows[i]["BBPSCollSARAL"] = chkBBPSCollSARAL.Checked == true ? "Y" : "N";
                dt.Rows[i]["DigiAuthJLG"] = chkDigiAuthJLG.Checked == true ? "Y" : "N";
                dt.Rows[i]["DigiAuthMEL"] = chkDigiAuthMEL.Checked == true ? "Y" : "N";
                dt.Rows[i]["DigiAuthSARAL"] = chkDigiAuthSARAL.Checked == true ? "Y" : "N";
                dt.Rows[i]["ManualAuthJLG"] = chkManualAuthJLG.Checked == true ? "Y" : "N";
                dt.Rows[i]["ManualAuthMEL"] = chkManualAuthMEL.Checked == true ? "Y" : "N";
                dt.Rows[i]["ManualAuthSARAL"] = chkManualAuthSARAL.Checked == true ? "Y" : "N";
                dt.Rows[i]["BioAuthJLG"] = chkBioAuthJLG.Checked == true ? "Y" : "N";
                dt.Rows[i]["BioAuthMEL"] = chkBioAuthMEL.Checked == true ? "Y" : "N";
                dt.Rows[i]["BioAuthSARAL"] = chkBioAuthSARAL.Checked == true ? "Y" : "N";
                dt.Rows[i]["DeviationJLG"] = chkDeviationJLG.Checked == true ? "Y" : "N";
                dt.Rows[i]["DeviationSARAL"] = chkDeviationSARAL.Checked == true ? "Y" : "N";
                dt.Rows[i]["CBSYN"] = chkCBSYN.Checked == true ? "Y" : "N";
                dt.AcceptChanges();
            }

            return dt;
        }

        private string DtToMtnXml(DataTable dtXml)
        {
            string vXml = string.Empty;
            try
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
                return vXml;
            }
            finally
            {
                dtXml = null;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveRecords() == true)
            {
                UncheckGrid();
                tbBranchCntrl.ActiveTabIndex = 0;
                StatusButton("View");
            }           
        }

        private Boolean SaveRecords()
        {
            CBranch oBrch = null;
            Int32 vErr = 0;
            DataTable dt = null;
            Boolean vResult = false;
            string vXmlMtn = string.Empty;
            DateTime vEffectiveDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            dt = CreateGvMain();

            if (dt.Rows.Count <= 0)
            {
                gblFuction.MsgPopup("Please enter valid data.");
                return false;
            }
            //vXmlMtn = DtToMtnXml(dt);

            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXmlMtn = oSW.ToString();
            }

            oBrch =  new CBranch();
            vErr = oBrch.SaveBranchControlMst(vXmlMtn, Convert.ToInt32(Session[gblValue.UserId].ToString()), vEffectiveDt);
            if (vErr == 0)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                vResult = true;
            }
            else
            {
                gblFuction.MsgPopup(gblMarg.DBError);
                vResult = false;
            }
            return vResult; 
        }       

        protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            DateTime vEndDate = DateTime.Now;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkIAJLG = (CheckBox)e.Row.FindControl("chkIAJLG");
                    CheckBox chkIASaral = (CheckBox)e.Row.FindControl("chkIASaral");
                    CheckBox chkPreDBJLG = (CheckBox)e.Row.FindControl("chkPreDBJLG");
                    CheckBox chkPreDBMEL = (CheckBox)e.Row.FindControl("chkPreDBMEL");
                    CheckBox chkPreDBSARAL = (CheckBox)e.Row.FindControl("chkPreDBSARAL");
                    CheckBox chkAdvCollJLG = (CheckBox)e.Row.FindControl("chkAdvCollJLG");
                    CheckBox chkAdvCollMEL = (CheckBox)e.Row.FindControl("chkAdvCollMEL");
                    CheckBox chkAdvCollSARAL = (CheckBox)e.Row.FindControl("chkAdvCollSARAL");
                    CheckBox chkFreshCustJLG = (CheckBox)e.Row.FindControl("chkFreshCustJLG");
                    CheckBox chkFreshCustSARAL = (CheckBox)e.Row.FindControl("chkFreshCustSARAL");
                    CheckBox chkRepeatCustJLG = (CheckBox)e.Row.FindControl("chkRepeatCustJLG");
                    CheckBox chkRepeatCustSARAL = (CheckBox)e.Row.FindControl("chkRepeatCustSARAL");
                    CheckBox chkCashCollJLG = (CheckBox)e.Row.FindControl("chkCashCollJLG");
                    CheckBox chkCashCollMEL = (CheckBox)e.Row.FindControl("chkCashCollMEL");
                    CheckBox chkCashCollSARAL = (CheckBox)e.Row.FindControl("chkCashCollSARAL");
                    CheckBox chkBBPSCollJLG = (CheckBox)e.Row.FindControl("chkBBPSCollJLG");
                    CheckBox chkBBPSCollMEL = (CheckBox)e.Row.FindControl("chkBBPSCollMEL");
                    CheckBox chkBBPSCollSARAL = (CheckBox)e.Row.FindControl("chkBBPSCollSARAL");
                    CheckBox chkDigiAuthJLG = (CheckBox)e.Row.FindControl("chkDigiAuthJLG");
                    CheckBox chkDigiAuthMEL = (CheckBox)e.Row.FindControl("chkDigiAuthMEL");
                    CheckBox chkDigiAuthSARAL = (CheckBox)e.Row.FindControl("chkDigiAuthSARAL");
                    CheckBox chkManualAuthJLG = (CheckBox)e.Row.FindControl("chkDigiAuthSARAL");
                    CheckBox chkManualAuthMEL = (CheckBox)e.Row.FindControl("chkManualAuthMEL");
                    CheckBox chkManualAuthSARAL = (CheckBox)e.Row.FindControl("chkManualAuthSARAL");
                    CheckBox chkBioAuthJLG = (CheckBox)e.Row.FindControl("chkBioAuthJLG");
                    CheckBox chkBioAuthMEL = (CheckBox)e.Row.FindControl("chkBioAuthMEL");
                    CheckBox chkBioAuthSARAL = (CheckBox)e.Row.FindControl("chkBioAuthSARAL");
                    CheckBox chkDeviationJLG = (CheckBox)e.Row.FindControl("chkDeviationJLG");
                    CheckBox chkDeviationSARAL = (CheckBox)e.Row.FindControl("chkDeviationSARAL");
                    CheckBox chkCBSYN = (CheckBox)e.Row.FindControl("chkCBSYN");

                    string InitialAppJLGYN = Convert.ToString(e.Row.Cells[32].Text);
                    string InitialAppSARALYN = Convert.ToString(e.Row.Cells[33].Text);
                    string PreDBJLGYN = Convert.ToString(e.Row.Cells[34].Text);
                    string PreDBMELYN = Convert.ToString(e.Row.Cells[35].Text);
                    string PreDBSARALYN = Convert.ToString(e.Row.Cells[36].Text);
                    string AdvCollJLGYN = Convert.ToString(e.Row.Cells[37].Text);
                    string AdvCollMELYN = Convert.ToString(e.Row.Cells[38].Text);
                    string AdvCollSARALYN = Convert.ToString(e.Row.Cells[39].Text);
                    string FreshCustJLGYN = Convert.ToString(e.Row.Cells[40].Text);
                    string FreshCustSARAL = Convert.ToString(e.Row.Cells[41].Text);
                    string RepeatCustJLG = Convert.ToString(e.Row.Cells[42].Text);
                    string RepeatCustSARAL = Convert.ToString(e.Row.Cells[43].Text);
                    string CashCollJLG = Convert.ToString(e.Row.Cells[44].Text);
                    string CashCollMEL = Convert.ToString(e.Row.Cells[45].Text);
                    string CashCollSARAL = Convert.ToString(e.Row.Cells[46].Text);
                    string BBPSCollJLG = Convert.ToString(e.Row.Cells[47].Text);
                    string BBPSCollMEL = Convert.ToString(e.Row.Cells[48].Text);
                    string BBPSCollSARAL = Convert.ToString(e.Row.Cells[49].Text);
                    string DigiAuthJLG = Convert.ToString(e.Row.Cells[50].Text);
                    string DigiAuthMEL = Convert.ToString(e.Row.Cells[51].Text);
                    string DigiAuthSARAL = Convert.ToString(e.Row.Cells[52].Text);
                    string ManualAuthJLG = Convert.ToString(e.Row.Cells[53].Text);
                    string ManualAuthMEL = Convert.ToString(e.Row.Cells[54].Text);
                    string ManualAuthSARAL = Convert.ToString(e.Row.Cells[55].Text);
                    string BioAuthJLG = Convert.ToString(e.Row.Cells[56].Text);
                    string BioAuthMEL = Convert.ToString(e.Row.Cells[57].Text);
                    string BioAuthSARAL = Convert.ToString(e.Row.Cells[58].Text);
                    string DeviationJLG = Convert.ToString(e.Row.Cells[59].Text);
                    string DeviationSARAL = Convert.ToString(e.Row.Cells[60].Text);
                    string CBSYN = Convert.ToString(e.Row.Cells[61].Text);



                    chkIAJLG.Checked = InitialAppJLGYN == "Y" ? true : false;
                    chkIASaral.Checked = InitialAppSARALYN == "Y" ? true : false;
                    chkPreDBJLG.Checked = PreDBJLGYN == "Y" ? true : false;
                    chkPreDBMEL.Checked = PreDBMELYN == "Y" ? true : false;
                    chkPreDBSARAL.Checked = PreDBSARALYN == "Y" ? true : false;
                    chkAdvCollJLG.Checked = AdvCollJLGYN == "Y" ? true : false;
                    chkAdvCollMEL.Checked = AdvCollMELYN == "Y" ? true : false;
                    chkAdvCollSARAL.Checked = AdvCollSARALYN == "Y" ? true : false;
                    chkFreshCustJLG.Checked = FreshCustJLGYN == "Y" ? true : false;
                    chkFreshCustSARAL.Checked = FreshCustSARAL == "Y" ? true : false;
                    chkRepeatCustJLG.Checked = RepeatCustJLG == "Y" ? true : false;
                    chkRepeatCustSARAL.Checked = RepeatCustSARAL == "Y" ? true : false;
                    chkCashCollJLG.Checked = CashCollJLG == "Y" ? true : false;
                    chkCashCollMEL.Checked = CashCollMEL == "Y" ? true : false;
                    chkCashCollSARAL.Checked = CashCollSARAL == "Y" ? true : false;
                    chkBBPSCollJLG.Checked = BBPSCollJLG == "Y" ? true : false;
                    chkBBPSCollMEL.Checked = BBPSCollMEL == "Y" ? true : false;
                    chkBBPSCollSARAL.Checked = BBPSCollSARAL == "Y" ? true : false;
                    chkDigiAuthJLG.Checked = DigiAuthJLG == "Y" ? true : false;
                    chkDigiAuthMEL.Checked = DigiAuthMEL == "Y" ? true : false;
                    chkDigiAuthSARAL.Checked = DigiAuthSARAL == "Y" ? true : false;
                    chkManualAuthJLG.Checked = ManualAuthJLG == "Y" ? true : false;
                    chkManualAuthMEL.Checked = ManualAuthMEL == "Y" ? true : false;
                    chkManualAuthSARAL.Checked = ManualAuthSARAL == "Y" ? true : false;
                    chkBioAuthJLG.Checked = BioAuthJLG == "Y" ? true : false;
                    chkBioAuthMEL.Checked = BioAuthMEL == "Y" ? true : false;
                    chkBioAuthSARAL.Checked = BioAuthSARAL == "Y" ? true : false;
                    chkDeviationJLG.Checked = DeviationJLG == "Y" ? true : false;
                    chkDeviationSARAL.Checked = DeviationSARAL == "Y" ? true : false;
                    chkCBSYN.Checked = CBSYN == "Y" ? true : false;
                }
            }
            finally
            {
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        { 
            UncheckGrid();
            tbBranchCntrl.ActiveTabIndex = 0;
            StatusButton("View");
        }

        protected void UncheckGrid()
        {
            GridViewRow headerRow = gvControl.HeaderRow;
            if (headerRow != null)
            {
                CheckBox chkAll = headerRow.FindControl("chkAll") as CheckBox;
                if (chkAll != null)
                {
                    chkAll.Checked = false;
                }
            }

            foreach (GridViewRow row in gvControl.Rows)
            {
                CheckBox chkAllow = row.FindControl("chkAllow") as CheckBox;
                if (chkAllow != null)
                {
                    chkAllow.Checked = false;
                }
            }

            gvControl.Enabled = true;
            rblCntrlType.Enabled = true;
            btnPopulate.Enabled = true;
        }


    }
}