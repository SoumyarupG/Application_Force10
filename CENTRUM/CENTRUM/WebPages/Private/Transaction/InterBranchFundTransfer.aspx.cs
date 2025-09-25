using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class InterBranchFundTransfer : CENTRUMBase
    {
        protected int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtTrDt.Text=Session[gblValue.LoginDate].ToString();
                StatusButton("View");
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                popBranch();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                LoadGrid(txtFrmDt.Text, txtToDt.Text, 1);
                tbDist.ActiveTabIndex = 0;
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
                this.PageHeading = "Inter Branch Fund Transfer";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuIntBrFundTr);
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
                    btnCancel.Visible = false;
                    
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "District Master", false);
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
                    btnPrint.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnPrint.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnPrint.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnPrint.Enabled = false;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnPrint.Enabled = false;
                    EnableControl(false);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popBranch()
        {
            DataTable dt = null;
            CBranchFundTr oGb = null;
            String vBrCode = vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oGb = new CBranchFundTr();
                dt = oGb.PopInterBranch(vBrCode,"B");
                ddlBr.DataSource = dt;
                ddlBr.DataTextField = "BranchName";
                ddlBr.DataValueField = "BranchCode";
                ddlBr.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBr.Items.Insert(0, oli);
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
                tbDist.ActiveTabIndex = 1;
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
                    LoadGrid(txtFrmDt.Text, txtToDt.Text,1);
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
            tbDist.ActiveTabIndex = 0;
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
                LoadGrid(txtFrmDt.Text, txtToDt.Text,1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                tbDist.ActiveTabIndex = 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
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
            LoadGrid(txtFrmDt.Text, txtToDt.Text,vPgNo);
            tbDist.ActiveTabIndex = 0;
        }
        private void LoadGrid(string pFromDt, string pToDt,Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0;
            string vBrCode = "";
            CBranchFundTr oHr = null;
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oHr = new CBranchFundTr();
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                dt = oHr.GetInterBrFundPG(vFromDt, vToDt, vBrCode, pPgIndx, ref vTotRows);
                gvDist.DataSource = dt;
                gvDist.DataBind();
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
                oHr = null;
                dt = null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDist_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pZoneId = 0, vRow = 0;
            CBranchFundTr OHb = null;
            DataTable dt = null;
            try
            {
                pZoneId = Convert.ToInt32(e.CommandArgument);
                ViewState["DistId"] = pZoneId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvDist.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    OHb = new CBranchFundTr();
                    dt = OHb.GetInterBrFtRDtls(pZoneId);               
                    if (dt.Rows.Count > 0)
                    {
                        txAmt.Text = Convert.ToString(dt.Rows[vRow]["Amount"]);
                        txtTrDt.Text= Convert.ToString(dt.Rows[vRow]["TrDate"]);
                        ddlBr.SelectedIndex = ddlBr.Items.IndexOf(ddlBr.Items.FindByValue(dt.Rows[0]["BrPaidTo"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbDist.ActiveTabIndex = 1;
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
            Int32 vErr = 0;
            string vBrPaidTo = "",vBrPaidFrom = "";
            double vAmt = 0;
            DateTime vTrDt;
            String vBrCode = Session[gblValue.BrnchCode].ToString();
            CBranchFundTr oDist = null;
            int vNewId = 0;
            string Msg = "";
            try
            {
                DataTable  dtRes = null;
                string vTblMst = Session[gblValue.ACVouMst].ToString();
                string vTblDtl = Session[gblValue.ACVouDtl].ToString();
                string vFinYear = Session[gblValue.ShortYear].ToString();
                int vYearNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
                vBrPaidTo = ddlBr.SelectedValue;
                vBrPaidFrom = vBrCode;
                vAmt = Convert.ToDouble(txAmt.Text.Trim() == "" ? "0" : txAmt.Text.Trim());
                vTrDt = gblFuction.setDate(txtTrDt.Text.Trim());
                if (this.RoleId != 1)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtTrDt.Text))
                        {
                            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                            return false;
                        }
                    }
                }
                if (Mode == "Save")
                {
                    oDist = new CBranchFundTr();
                    Msg = oDist.chkCashOrBankBalBfrSave(vBrCode, gblFuction.setDate(txtTrDt.Text),
                              Convert.ToDouble((txAmt.Text == "" || txAmt.Text == ".") ? "0" : txAmt.Text.Trim()),
                              gblFuction.setDate(Session[gblValue.FinFromDt].ToString()),
                              Convert.ToInt32(Session[gblValue.FinYrNo].ToString()),"Cash");
                    if (Msg != "")
                    {
                        gblFuction.AjxMsgPopup(Msg);
                        return false;
                    }
                    this.GetModuleByRole(mnuID.mnuIntBrFundTr);
                    dtRes = oDist.SaveInterBrFFundTr(vNewId, vTrDt, vBrPaidTo, vBrPaidFrom, vAmt, vBrCode, this.UserID, "Save", vTblMst, vTblDtl, vFinYear, vYearNo);
                    if (dtRes.Rows[0]["pErr"].ToString() == "0")
                    {
                        ViewState["DistId"] = Convert.ToInt32(dtRes.Rows[0]["pNewId"].ToString());
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                //else if (Mode == "Edit")
                //{
                //    oDist = new CBranchFundTr();                    
                
                //    vErr = oDist.SaveDistrict(ref vNewId, vDistId, txtDist.Text.Replace("'", "''"), Convert.ToInt32(vStatId), this.UserID, "Edit");
                //    if (vErr > 0)
                //    {
                //        gblFuction.MsgPopup(gblMarg.EditMsg);
                //        vResult = true;
                //    }
                //    else
                //    {
                //        gblFuction.MsgPopup(gblMarg.DBError);
                //        vResult = false;
                //    }
                //}
                else if (Mode == "Delete")
                {
                    vNewId = Convert.ToInt32(ViewState["DistId"]);
                    oDist = new CBranchFundTr();
                    this.GetModuleByRole(mnuID.mnuIntBrFundTr);
                    dtRes = oDist.SaveInterBrFFundTr(vNewId, vTrDt, vBrPaidTo, vBrPaidFrom, vAmt, vBrCode, this.UserID, "Delete", vTblMst, vTblDtl, vFinYear, vYearNo);
                    if (dtRes.Rows[0]["IsDelete"].ToString() != "Y")
                    {
                        if (dtRes.Rows[0]["pErr"].ToString() == "0")
                        {
                            gblFuction.MsgPopup(gblMarg.DeleteMsg);
                            vResult = true;
                            ClearControls();
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                            vResult = false;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Approved Voucher CanNot Be Deleted");
                        vResult = false;
                    }
                }
                return vResult;
            }
            finally
            {
                oDist = null;                
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dt = null,dtHeadID = null;
            CVoucher oVoucher = null;
            string vRptPath = "";
            CBranchFundTr oFT = null;
            string pHeadId;
            double vAllToTal = 0.0;
            Int32 vTranID = 0;
            String vBrCode = Session[gblValue.BrnchCode].ToString();
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.FinYear].ToString();
            int vYearNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            //try
            //{
            vTranID = Convert.ToInt32(ViewState["DistId"]);
            oFT = new CBranchFundTr();
            dtHeadID = oFT.InterBrFFundTrHeadId(vTranID, vTblMst, vTblDtl, vFinYear, vYearNo, vBrCode);
            if (dtHeadID.Rows.Count > 0)
            {
                oVoucher = new CVoucher();
                pHeadId = Convert.ToString(dtHeadID.Rows[0]["HeadId"]);
                //string vBrCode = pHeadId.Trim().Substring(0, 3);//Session[gblValue.BrnchCode].ToString();
                dt = oVoucher.GetVoucherDtl(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), pHeadId, vBrCode);
                foreach (DataRow dr in dt.Rows)
                {
                    vAllToTal = vAllToTal + Convert.ToDouble(dr["Debit"].ToString());
                }
                //dt.DefaultView.Sort = "DC ASC";
                DataView dv = dt.DefaultView;
                //dv.Sort = "DC ASC";
                dv.Sort = "DC DESC";
                DataTable sortedDT = dv.ToTable();
                //if (dt.Rows[0]["VoucherType"].ToString() == "P")
                //{
                // vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPrintVoucherPayment.rpt";
                //}
                //else
                //{
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPrintVoucherJournal.rpt";
                //}
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(sortedDT);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                    rptDoc.SetParameterValue("pBranch", Session[gblValue.BrName].ToString());
                    if (dt.Rows[0]["VoucherType"].ToString() == "R")
                        rptDoc.SetParameterValue("pTitle", "Receipt Voucher");
                    else if (dt.Rows[0]["VoucherType"].ToString() == "P")
                        rptDoc.SetParameterValue("pTitle", "Payment Voucher");
                    else if (dt.Rows[0]["VoucherType"].ToString() == "J")
                        rptDoc.SetParameterValue("pTitle", "Journal Voucher");
                    else
                        rptDoc.SetParameterValue("pTitle", "Contra Voucher");
                    rptDoc.SetParameterValue("pAllTotal", vAllToTal);
                    //rptDoc.SetParameterValue("pFrmDt", txtDtFrm.Text);
                    //rptDoc.SetParameterValue("pToDt", txtToDt.Text);

                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Journal_Voucher");
                    rptDoc.Close();
                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txAmt.Enabled = Status;
            ddlBr.Enabled = Status;
            txtTrDt.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txAmt.Text = "0";
            //txtTrDt.Text = "";
            ddlBr.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            LoadGrid(txtFrmDt.Text, txtToDt.Text, 1);
        }
    }   

}