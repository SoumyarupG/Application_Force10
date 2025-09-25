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

namespace CENTRUM.WebPages.Private.Inventory
{
    public partial class StckDamage : CENTRUMBase
    {
        private int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtVeri.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StockDam"] = null;
                //LoadGrid(0);
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
            CItem oItm = null;
            DataTable dt = null;
            DateTime pVeriDt = gblFuction.setDate(txtDtVeri.Text);
            string vBranch = Session[gblValue.BrnchCode].ToString();
            try
            {
                oItm = new CItem();
                dt = oItm.BranchWiseStckSummforDamage(pVeriDt, vBranch);
                gvStock.DataSource = dt;
                gvStock.DataBind();
                ViewState["StockDam"] = dt;
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
                this.PageHeading = "Stock Damage Entry";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuStckDamage);
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
        public void DtToXml()
        {
            DataTable dt = (DataTable)ViewState["StockDam"];
            dt.TableName = "Table1";
            if (dt.Rows.Count > 0)
            {
                foreach (GridViewRow gr in gvStock.Rows)
                {
                    TextBox txtVeriStock = (TextBox)gr.FindControl("txtVeriStock");
                    if (Convert.ToInt32(txtVeriStock.Text) > 0)
                    {
                        if (Convert.ToInt32(gr.Cells[3].Text.Trim()) > Convert.ToInt32(txtVeriStock.Text))
                            dt.Rows[gr.RowIndex]["DamageQty"] = Convert.ToInt32(txtVeriStock.Text);
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Damage Quantity Cannot be Greater then Stock in "+ gr.Cells[2].Text);
                        return;
                    }
                }
                dt.AcceptChanges();

            }
            ViewState["StockDam"] = dt;
           
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 dChk = Convert.ToInt32(ViewState["ChkId"]);
            CItem oItm = null;
            DataTable dtDtl = null, dt = null;
            string vXml = string.Empty;

            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                int vVeriId = Convert.ToInt32(ViewState["ChkId"]);
                DateTime vVeriDt = gblFuction.setDate(txtDtVeri.Text);
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                DtToXml();
                dt = (DataTable)ViewState["StockDam"];
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
                    dChk = oItm.SaveStckDamage(ref dChk, vVeriDt, this.UserID, vBrCode, vXml, "Save");
                    if (dChk > 0)
                    {
                        ViewState["ChkId"] = dChk;
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
                    dChk = oItm.SaveStckDamage(ref dChk, vVeriDt, this.UserID, vBrCode, vXml, "Edit");
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
                    dChk = oItm.SaveStckDamage(ref dChk, vVeriDt, this.UserID, vBrCode, vXml, "Del");
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
        

        
        
    }
}