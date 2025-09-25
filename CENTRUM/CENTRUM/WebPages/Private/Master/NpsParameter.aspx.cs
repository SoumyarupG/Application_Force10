using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
namespace CENTRUM.WebPages.Private.Master
{
    public partial class NpsParameter : CENTRUMBase
    {
        protected int vPgNo = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {                
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                PopAssets();
                LoadFields();
                tbLnAppPara.ActiveTabIndex = 0;
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
                this.PageHeading = "Parameter";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuNpsParamMst);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {                   
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "NPS Parameter", false);
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
        private void PopAssets()
        {
            DataTable dt = null;
            CGenParameter oGen = null;
            string vGenAcType = "G";
            Int32 vAssets = 4;
            try
            {
                oGen = new CGenParameter();
                dt = oGen.GetLedgerByAcHeadId(vGenAcType, vAssets);
                ListItem Lst1 = new ListItem("<--- Select --->", "-1");
                ddlNpsAc.DataTextField = "Desc";
                ddlNpsAc.DataValueField = "DescId";
                ddlNpsAc.DataSource = dt;
                ddlNpsAc.DataBind();
                ddlNpsAc.Items.Insert(0, Lst1);
            }
            finally
            {
                dt = null;
                oGen = null;
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                ViewState["StateEdit"] = null;
            }
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vId = Convert.ToInt32(ViewState["Id"]), vRec=0;
            CNpsParameter oPrm = null;
            try
            {
                if (Mode == "Save")
                {
                    oPrm = new CNpsParameter();
                    vRec = oPrm.NPS_SaveParameter(ref vId, txtNLAORegNo.Text.Replace("'", "''"), txtNLCCNo.Text.Replace("'", "''"), ddlNpsAc.SelectedValue);
                    if (vRec > 0)
                        gblFuction.MsgPopup("Save NPS Parameter Successfully.");
                    else
                        gblFuction.MsgPopup("Error... For Saving NPS Parameter.");
                }               
                return vResult;
            }
            finally
            {
                oPrm = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            ddlNpsAc.Enabled = Status;
            txtNLAORegNo.Enabled = Status;
            txtNLCCNo.Enabled = Status;          
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlNpsAc.SelectedIndex = -1;
            txtNLAORegNo.Text = "";
            txtNLCCNo.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadFields()
        {
            DataTable dt = null;
            CNpsParameter oPrm = null;

            try
            {
                oPrm = new CNpsParameter();
                dt = oPrm.NPS_GetParameterList();
                if (dt.Rows.Count > 0)
                {
                    ViewState["Id"] = Convert.ToString(dt.Rows[0]["ID"]);
                    ddlNpsAc.SelectedIndex = ddlNpsAc.Items.IndexOf(ddlNpsAc.Items.FindByValue(Convert.ToString(dt.Rows[0]["NPSAc"]).Trim()));
                    txtNLAORegNo.Text = Convert.ToString(dt.Rows[0]["NLAORegNo"]);
                    txtNLCCNo.Text = Convert.ToString(dt.Rows[0]["NLAOOffice"]); 
                }
                else
                {
                    ViewState["Id"] = 0;
                    ddlNpsAc.SelectedIndex = -1;
                    txtNLAORegNo.Text = "";
                    txtNLCCNo.Text = "";
                }
            }
            finally
            {
                dt = null;
                oPrm = null;
            }
        }
    }
}