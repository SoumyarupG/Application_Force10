using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;


namespace CENTRUM.WebPages.Private.Master
{
    public partial class LnAppParam : CENTRUMBase
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
                //popCycl();
                LoadGrid(1);
                StatusButton("View");
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
                this.PageHeading = "Loan Application Parameter";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanAppParam);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application Parameter", false);
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

        /// <summary>
        /// 
        /// </summary>
        //private void popCycl()
        //{
        //    Dictionary<string, string> oDic = new Dictionary<string, string>();
        //    oDic.Add("<-Select->", "0");
        //    oDic.Add("Cycle2", "2");
        //    oDic.Add("Cycle3", "3");
        //    oDic.Add("Cycle4", "4");
        //    oDic.Add("Cycle5", "5");
        //    oDic.Add("Cycle6", "6");
        //    ddlCycl.DataSource = oDic;
        //    ddlCycl.DataValueField = "value";
        //    ddlCycl.DataTextField = "key";
        //    ddlCycl.DataBind();
        //}     

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
            try
            {
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = "Add";
                tbLnAppPara.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid(1);
                    ClearControls();
                    tbLnAppPara.ActiveTabIndex = 0;
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                txtEffDt.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbLnAppPara.ActiveTabIndex = 0;
            LoadGrid(0);
            ClearControls();
            EnableControl(false);
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
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vDistId = 0, vTotRows = 0; 
            string vBrCode = "";
            CLAppPara oVlg = null;
            try
            {
                vDistId = Convert.ToInt32(Session[gblValue.DistrictId].ToString());
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oVlg = new CLAppPara();
                dt = oVlg.GetLnAppParamPG(pPgIndx, ref vTotRows);
                gvLnAppPara.DataSource = dt;
                gvLnAppPara.DataBind();
                ViewState["LnApp"] = dt;
                lblTotPg.Text = CalTotPages(vTotRows).ToString();
                lblCrPg.Text = vPgNo.ToString();
                if (vPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 1)
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (vPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
            }
            finally
            {
                oVlg = null;
                dt=null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Prev":
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadGrid(vPgNo);
            tbLnAppPara.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLnAppPara_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vLAPId = 0, vRow = 0;
            DataTable dt = null;
            try
            {
                vLAPId = Convert.ToInt32(e.CommandArgument);
                ViewState["LAPId"] = vLAPId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvLnAppPara.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    dt = (DataTable)ViewState["LnApp"];
                    dt.PrimaryKey = new DataColumn[] { dt.Columns["LAPId"] };
                    vRow = dt.Rows.IndexOf(dt.Rows.Find(vLAPId));
                    if (dt.Rows.Count > 0)
                    {
                        //ddlCycl.SelectedIndex = ddlCycl.Items.IndexOf(ddlCycl.Items.FindByValue(dt.Rows[vRow]["CycleNo"].ToString()));
                        //txtPar.Text = Convert.ToString(dt.Rows[vRow]["PAR30"]);
                        //txtAtt.Text = Convert.ToString(dt.Rows[vRow]["MemAtt"]);
                        //txtDSCR.Text = Convert.ToString(dt.Rows[vRow]["DSCR"]);
                        txtlnApp.Text=Convert.ToString(dt.Rows[vRow]["NoOfAppliaction"]);
                        txtDisbrs.Text = Convert.ToString(dt.Rows[vRow]["NoOfDisbursement"]);
                        txtEffDt.Text = Convert.ToString(dt.Rows[vRow]["EffDate"]);
                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbLnAppPara.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
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
            Int32 vLAPId = Convert.ToInt32(ViewState["LAPId"]);
            Int32 vErr = 0, vRec = 0,vCycl = 0, vNewId = 0,vNoOfApp=0,vNoOfDisb=0;
            Double  vPar = 0, vAtt=0,vDSCR=0;
            CLAppPara oLAP = null;
            CGblIdGenerator oGbl = null;
            //vCycl=Convert.ToInt32(ddlCycl.SelectedValue);
            //vPar = Convert.ToDouble(txtPar.Text);
            //vAtt = Convert.ToDouble(txtAtt.Text);
            //vDSCR = Convert.ToDouble(txtDSCR.Text);
            if (txtlnApp.Text != "") vNoOfApp = Convert.ToInt32(txtlnApp.Text);
            if (txtDisbrs.Text != "") vNoOfDisb = Convert.ToInt32(txtDisbrs.Text);
            
            try
            {
                if (Mode == "Save")
                {
                    oLAP = new CLAppPara();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("AppParameter", "EffDate",txtEffDt.Text, "", "", "", "", "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Effective Date exists,addition not possible...");
                        return false;
                    }
                    vErr = oLAP.SaveLnApp(ref vNewId, vCycl, vPar, vAtt, vDSCR,gblFuction.setDate(txtEffDt.Text),
                        vBrCode,vNoOfApp,vNoOfDisb, this.UserID, "Save");
                    if (vErr > 0)
                    {
                        ViewState["LAPId"] = vNewId;
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
                    oLAP = new CLAppPara();
                    oGbl = new CGblIdGenerator();
                    vNewId = Convert.ToInt32(ViewState["LAPId"]);                   
                    vErr = oLAP.SaveLnApp(ref vNewId, vCycl, vPar, vAtt, vDSCR, gblFuction.setDate(txtEffDt.Text),
                        vBrCode,vNoOfApp,vNoOfDisb, this.UserID, "Edit");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oLAP = new CLAppPara();
                    vNewId = Convert.ToInt32(ViewState["LAPId"]);
                    vErr = oLAP.SaveLnApp(ref vNewId, vCycl, vPar, vAtt, vDSCR, gblFuction.setDate(txtEffDt.Text),
                        vBrCode,vNoOfApp,vNoOfDisb, this.UserID, "Delet");
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
                oLAP = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            //ddlCycl.Enabled = Status;
            //txtAtt.Enabled = Status;
            //txtDSCR.Enabled = Status;
            txtEffDt.Enabled = Status;
            //txtPar.Enabled = Status;
            txtlnApp.Enabled = Status;
            txtDisbrs.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
        //    ddlCycl.SelectedIndex = -1;
        //    txtAtt.Text = "";
        //    txtDSCR.Text = "";
            txtEffDt.Text = "";
            //txtPar.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
            txtlnApp.Text = "0";
            txtDisbrs.Text = "0";
        }
    }
}