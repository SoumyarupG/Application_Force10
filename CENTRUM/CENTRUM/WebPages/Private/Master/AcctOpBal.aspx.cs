using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class AcctOpBal : CENTRUMBase
    {
        protected int vPgNo = 1;
        double vTot = 0;

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
                //Permission("btnAdd", "btnEdit", "btnDelete", "btnCancel", "btnSave", "Accounts Opening Balance");
                PopDebCredit();
                LoadGrid(1);
                PopSubsidiary();
                txtAmt.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                ViewState["StateEdit"] = null;
                ViewState["RecSec"] = null;
                ViewState["Subsidiary"] = null;
                tabOpBal.ActiveTabIndex = 0;
                StatusButton("View");
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
                this.PageHeading = "Opening Balance";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuAccOpBal);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Account Opening Balance", false);
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
                    btnEdit.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnEdit.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnEdit.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnEdit.Enabled = true;
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
        private void PopDebCredit()
        {
            ddlDrCr.Items.Clear();
            ListItem LiDr = new ListItem("Dr", "D");
            ddlDrCr.Items.Add(LiDr);
            ListItem LiCr = new ListItem("Cr", "C");
            ddlDrCr.Items.Add(LiCr);
            ListItem LiSel = new ListItem("<-Select->", "0");
            ddlDrCr.Items.Insert(0, LiSel);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopSubsidiary()
        {
            gvSubs.DataSource = null;
            gvSubs.DataBind();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CAcctOpBal oAO = null;
            Int32 totalRows = 0;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                Int32 vFinYearNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
                oAO = new CAcctOpBal();
                dt = oAO.GetOpBalLedgerList(vBrCode, vFinYearNo, pPgIndx, ref totalRows);
                gvLed.DataSource = dt.DefaultView;
                gvLed.DataBind();
                lblTotPg.Text = CalTotPages(totalRows).ToString();
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
                oAO = null;
                dt = null;
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
            tabOpBal.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLed_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vAcGrpId = "", vBrCode = "";
            Int32 vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
            vBrCode = Session[gblValue.BrnchCode].ToString();
            DataTable dt = null;
            DataSet ds = null;
            CAcctOpBal oActOpBal = null;
            try
            {
                vAcGrpId = Convert.ToString(e.CommandArgument);
                ViewState["GenLedId"] = vAcGrpId;
                ViewState["RecSec"] = "Y";
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvLed.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oActOpBal = new CAcctOpBal();
                    ds = oActOpBal.GetAcctSusidairyOpBalDetails(vAcGrpId, vBrCode, vYrNo);
                    dt = ds.Tables[0];
                    //dt1 = ds.Tables[1];  
                    if (dt.Rows.Count > 0)
                    {
                        txtLedger.Text = Convert.ToString(dt.Rows[0]["GenLed"]);
                        ddlDrCr.SelectedIndex = ddlDrCr.Items.IndexOf(ddlDrCr.Items.FindByValue(dt.Rows[0]["DrCr"].ToString().Trim()));
                        txtAmt.Text = Convert.ToString(dt.Rows[0]["Amt"]);
                        lblUser.Text = "Last Modified By : " + Convert.ToString(dt.Rows[0]["UserName"]);
                        lblDate.Text = "Last Modified Date : " + Convert.ToString(dt.Rows[0]["CreationDateTime"]);
                        StatusButton("view");
                        tabOpBal.ActiveTabIndex = 1;
                    }
                    //if (dt1.Rows.Count > 0)
                    //{
                    //    ViewState["Subsidiary"] = dt1;
                    //    txtRec.Text = "1"; 
                    //    gvSubs.DataSource = dt1;
                    //    gvSubs.DataBind();
                    //}
                    //else
                    //{
                    //    ViewState["Subsidiary"] = null;
                    //    txtRec.Text = "0"; 
                    //    gvSubs.DataSource = null;
                    //    gvSubs.DataBind();
                    //}
                }
            }
            finally
            {
                dt = null;
                //dt1 = null;
                ds = null;
                oActOpBal = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtLedger.Enabled = Status;
            ddlDrCr.Enabled = Status;
            txtAmt.Enabled = Status;
            //gvSubs.Enabled = Status;
            //txtTotAmt.Enabled = Status; 
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtLedger.Text = "";
            ddlDrCr.SelectedIndex = -1;
            txtAmt.Text = "";
            lblUser.Text = "";
            lblDate.Text = "";
            //txtTotAmt.Text = "0";  
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
                LoadGrid(1);
                StatusButton("Show");
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
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string dAcGrpId = Convert.ToString(ViewState["GenLedId"]);
            Int32 vFinYr = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CAcctOpBal oAcBal = null;
            Int32 vErr = 0;
            //string vXml = "";
            try
            {
                if (Mode == "Edit")
                {
                    if (ViewState["Subsidiary"] != null)
                    {
                        //dt1 = (DataTable)ViewState["Subsidiary"];
                        //if (dt1.Rows.Count > 0)
                        //{
                        //    foreach (DataRow dR in dt1.Rows)
                        //    {
                        //        vR = Convert.ToInt32(dR["RowId"]);
                        //        TextBox txtAmt = (TextBox)gvSubs.Rows[vR - 1].FindControl("txtAmt");
                        //        dR["OpAmt"] = txtAmt.Text;
                        //    }
                        //    dt1.AcceptChanges();
                        //    dt1.TableName = "Table1";
                        //    using (StringWriter oSW = new StringWriter())
                        //    {
                        //        dt1.WriteXml(oSW);
                        //        vXml = oSW.ToString();
                        //    }
                        //}
                    }
                    oAcBal = new CAcctOpBal();
                    dt = oAcBal.ChkDupUpdateByGenLedId(dAcGrpId, vFinYr, vBrCode);
                    if (dt.Rows.Count > 0)
                    {
                        vErr = oAcBal.UpdateAcctOpBal(dAcGrpId, vFinYr, ddlDrCr.SelectedValue.ToString(),
                            Convert.ToDouble(txtAmt.Text), vBrCode, this.UserID, 0, "E");
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
                    }
                    else
                    {
                        vErr = oAcBal.InsertAcctOpBal(dAcGrpId, vFinYr, ddlDrCr.SelectedValue.ToString(),
                            Convert.ToDouble(txtAmt.Text), vBrCode, this.UserID, 0, "I");
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
                dt = null;
                //dt1 = null;
                oAcBal = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSubs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtAmt = (TextBox)e.Row.FindControl("txtAmt");
                if (txtAmt.Text != "")
                    vTot = vTot + Convert.ToDouble(txtAmt.Text);
                txtTotAmt.Text = vTot.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabOpBal.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx");
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
                //if (this.CanEdit == "N")
                //{
                //    gblFuction.MsgPopup(MsgAccess.Edit);
                //    return;
                //}
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
                tabOpBal.ActiveTabIndex = 1;
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
        protected void tabGenLed_ActiveTabChanged(object sender, EventArgs e)
        {
            if (tabOpBal.ActiveTabIndex == 1)
            {
                EnableControl(false);
                StatusButton("View");
                ViewState["GenLedId"] = null;
                ViewState["StateEdit"] = null;
            }
        }
    }
}