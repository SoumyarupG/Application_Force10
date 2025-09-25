using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Data;
using System.IO;

namespace CENTRUM.WebPages.Private.Inventory
{
    public partial class StockSummVeri : CENTRUMBase
    {
        private int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtVeri.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StockSumm"] = null;
                LoadGrid(0);
                tbEmp.ActiveTabIndex = 0;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
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
            StatusButton("Add");
            //pnlROTrnDrp.Enabled = false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string vEoId = Convert.ToString(ViewState["EoId"]);
            if (this.CanEdit == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Edit);
                return;
            }
            tbEmp.ActiveTabIndex = 1;
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
            //GetSupervisor(vEoId, "E");
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbEmp.ActiveTabIndex = 0;
            LoadGrid(0);
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
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                //EnableControl(false);
            }
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    break;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
            CItem oItm = null;
            DataTable dt = null;
            DateTime pVeriDt=gblFuction.setDate(txtDtVeri.Text);
            string vBranch=Session[gblValue.BrnchCode].ToString();
            try
            {
                oItm = new CItem();
                dt = oItm.GetStockData(pVeriDt, vBranch);
                gvStock.DataSource = dt;
                gvStock.DataBind();
                ViewState["StockSumm"] = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Stock Summary and Verification";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuStckSummVeri);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "RO Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        public DataTable DtToXml()
        {
            DataTable dt = (DataTable)ViewState["StockSumm"];
            dt.TableName = "Table1";
            if (dt.Rows.Count > 0)
            {
                foreach (GridViewRow gr in gvStock.Rows)
                {
                    TextBox txtVeriStock = (TextBox)gr.FindControl("txtVeriStock");
                    if (Convert.ToInt32(txtVeriStock.Text) > 0)
                    {
                        dt.Rows[gr.RowIndex]["ActualStock"] = Convert.ToInt32(txtVeriStock.Text);
                    }
                }
                dt.AcceptChanges();
               
            }
            ViewState["StockSumm"] = dt;
            return dt;
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 dChk = Convert.ToInt32(ViewState["VeriId"]);
            CItem oItm = null;
            DataTable dtDtl = null, dt = null;
            string vXml = string.Empty;

            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                int vVeriId = Convert.ToInt32(ViewState["VeriId"]);
                DateTime vVeriDt = gblFuction.setDate(txtDtVeri.Text);
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = DtToXml();
                if (dt.Rows.Count <= 0)
                {
                    gblFuction.MsgPopup("Please enter valid data.");
                    return false;
                }
                else
                {


                    dt.TableName = "Table1";
                    using (StringWriter oSw = new StringWriter())
                    {
                        dt.WriteXml(oSw);
                        vXml = oSw.ToString();
                    }

                }
                if (Mode == "Save")
                {
                    oItm = new CItem();
                    dChk = oItm.SaveStckSummVeri(ref dChk, vVeriDt, this.UserID, vBrCode, vXml, "Save");
                    if (dChk > 0)
                    {
                        ViewState["VeriId"] = dChk;
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
                    oItm = new CItem();
                    dChk = oItm.SaveStckSummVeri(ref dChk, vVeriDt, this.UserID, vBrCode, vXml, "Edit");
                    if (dChk > 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oItm = new CItem();
                    dChk = oItm.SaveStckSummVeri(ref dChk, vVeriDt, this.UserID, vBrCode, vXml, "Del");
                    if (dChk > 0)
                        vResult = true;
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
                oItm = null;
                dtDtl = null;
                dt = null;
            }
        }
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vRows = 0;
            string vMod = string.Empty;
            CItem oItm = null;
            try
            {
               string vBrCode=Session[gblValue.BrnchCode].ToString();
                oItm = new CItem();
                dt = oItm.GetStckSummVeriPG(pPgIndx, ref vRows, vBrCode);
                gvStockList.DataSource = dt;
                gvStockList.DataBind();
                lblTotPg.Text = CalTotPages(vRows).ToString();
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
                oItm = null;
                dt = null;
            }
        }

        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
        }
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
            tbEmp.ActiveTabIndex = 0;
        }
        protected void gvStock_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vVeriId = 0;
            DataTable dt = null, dt1=null;
            DataSet ds = new DataSet();
            CItem oEo = null;
            try
            {
                vVeriId = Convert.ToInt32(e.CommandArgument);
                ViewState["VeriId"] = vVeriId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvStockList.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oEo = new CItem();
                    ds = oEo.GetStckVeriDtls(vVeriId);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {

                        txtDtVeri.Text = Convert.ToString(dt.Rows[0]["VerificationDt"]);
                        if (dt1.Rows.Count > 0)
                        {
                            gvStock.DataSource = dt1;
                            gvStock.DataBind();
                            ViewState["StockSumm"] = dt1;
                        }
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        
                        tbEmp.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oEo = null;
            }
        }
    }
}