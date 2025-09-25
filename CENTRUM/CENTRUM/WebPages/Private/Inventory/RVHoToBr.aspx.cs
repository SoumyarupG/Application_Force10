using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Inventory
{
    public partial class RVHoToBr : CENTRUMBase
    {
        private int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtRecDt.Text = Session[gblValue.LoginDate].ToString();
                StatusButton("View");
                ViewState["StateEdit"] = null;
                ViewState["ItmData"] = null;
                ViewState["ItemData"] = null;
                popTransaction("A", 0);
                LoadGrid(0);
                tbRole.ActiveTabIndex = 0;
                StatusButton("View");
            }
        }

        private void popTransaction(string pMode, Int32 pTrnID)
        {
            DataTable dt = null;
            CItem oGb = null;
            DateTime vDate = gblFuction.setDate(txtRecDt.Text);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oGb = new CItem();

                dt = oGb.GetSDTransaction("HB", vDate, vBrCode, 0, pMode, pTrnID);
                ddlTrn.DataSource = dt;
                ddlTrn.DataTextField = "ChlnNo";
                ddlTrn.DataValueField = "RecId";
                ddlTrn.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlTrn.Items.Insert(0, oli);
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
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Head Office To Branch";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuRVHoToBrch);
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
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Head Office to Branch", false);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
        }


        /// <summary>
        /// 
        /// </summary>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vRows = 0;
            string vMod = string.Empty;
            CItem oItm = null;
            try
            {
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                if (txtFrmDt.Text != "" || txtToDt.Text != "")
                    vMod = "C";
                oItm = new CItem();
                dt = oItm.GetRVHoToBranchPG(pPgIndx, ref vRows, vFrmDt, vToDt, Session[gblValue.BrnchCode].ToString(), vMod);
                gvRole.DataSource = dt;
                gvRole.DataBind();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvItm_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dtItm = null;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    dtItm = (DataTable)ViewState["ItemData"];
                    DropDownList ddlItm = (DropDownList)e.Row.FindControl("ddlItm");
                    ddlItm.DataTextField = "Name";
                    ddlItm.DataValueField = "ItemId";
                    ddlItm.DataSource = dtItm;
                    ddlItm.DataBind();
                    ListItem oL = new ListItem("<-- Select -->", "-1");
                    ddlItm.Items.Insert(0, oL);
                    ddlItm.SelectedIndex = ddlItm.Items.IndexOf(ddlItm.Items.FindByValue(e.Row.Cells[5].Text));
                }
            }
            finally
            {
                dtItm = null;
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
            tbRole.ActiveTabIndex = 0;
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
                case "Exit":
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

        /// <summary>
        ///  
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtRecDt.Enabled = Status;
            ddlTrn.Enabled = Status;
            gvItm.Enabled = Status;
        }

        /// <summary>
        ///  
        /// </summary>
        private void ClearControls()
        {
            txtRecDt.Text = Session[gblValue.LoginDate].ToString();
            ddlTrn.SelectedIndex = -1;
            gvItm.DataSource = null;
            gvItm.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 dChk = Convert.ToInt32(ViewState["VenId"]);
            CItem oItm = null;
            DataTable dtDtl = null, dt = null;
            string vXml = string.Empty;

            try
            {
                Int32 vTrnID = Convert.ToInt32(ddlTrn.SelectedValue);
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                string vVendId = Convert.ToString(ViewState["VenId"]);
                DateTime vChlnDt = gblFuction.setDate("");
                DateTime vRecvDt = gblFuction.setDate(txtRecDt.Text);

                dt = new DataTable();
                dt = DtPPC();
                if (dt.Rows.Count <= 0)
                {
                    gblFuction.MsgPopup("Please enter valid data.");
                    return false;
                }
                dt.TableName = "Table1";
                vXml = DtToXml(dt);

                if (Mode == "Save")
                {
                    oItm = new CItem();
                    dChk = oItm.SaveRVHoToBranch(ref dChk, vTrnID, vChlnDt, vRecvDt, this.UserID, Session[gblValue.BrnchCode].ToString(), vXml, "Save");
                    if (dChk > 0)
                    {
                        ViewState["VenId"] = dChk;
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
                    dChk = oItm.SaveRVHoToBranch(ref dChk, vTrnID, vChlnDt, vRecvDt, this.UserID, Session[gblValue.BrnchCode].ToString(), vXml, "Edit");
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
                    dChk = oItm.SaveRVHoToBranch(ref dChk, 0, vChlnDt, vRecvDt, this.UserID, "", vXml, "Del");
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

        private string DtToXml(DataTable dtXml)
        {
            string vXml = "";
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

        private DataTable DtPPC()
        {
            DataTable dt = null;
            DataRow dr;
            int i = 1;
            try
            {
                dt = new DataTable("PPCDtl");
                dt.Columns.Add(new DataColumn("ItemId"));
                dt.Columns.Add(new DataColumn("Qty"));

                foreach (GridViewRow gr in gvItm.Rows)
                {
                    CheckBox chkYN = (CheckBox)gr.FindControl("chkYN");
                    if (chkYN.Checked == true)
                    {
                        dr = dt.NewRow();
                        dr["ItemId"] = gr.Cells[4].Text;
                        dr["Qty"] = gr.Cells[2].Text;
                        dt.Rows.Add(dr);
                        i = i + 1;
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                dr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "Save";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                    LoadGrid(0);
                    StatusButton("View");
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
        protected void gvRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vRecId = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            DataSet ds = null;
            CItem oItm = null;
            try
            {
                vRecId = Convert.ToInt32(e.CommandArgument);
                ViewState["VenId"] = vRecId;
                if (e.CommandName == "cmdShow")
                {
                    oItm = new CItem();
                    ds = oItm.GetRVHoToBranchById(vRecId);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        txtRecDt.Text = Convert.ToString(dt.Rows[0]["RecvDt"]);
                        popTransaction("E", vRecId);
                        ddlTrn.SelectedIndex = ddlTrn.Items.IndexOf(ddlTrn.Items.FindByValue(Convert.ToString(dt.Rows[0]["TrnID"])));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbRole.ActiveTabIndex = 1;
                        StatusButton("Show");
                        btnAdd.Visible = true;
                    }
                    gvItm.DataSource = dt1;
                    gvItm.DataBind();
                    ViewState["ItmData"] = dt1;
                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                ds = null;
                oItm = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                tbRole.ActiveTabIndex = 0;
                EnableControl(false);
                StatusButton("View");
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
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
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
                    LoadGrid(0);
                    StatusButton("Delete");
                    tbRole.ActiveTabIndex = 0;
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
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = null;
                tbRole.ActiveTabIndex = 1;
                StatusButton("Add");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlTrn_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CItem oTm = null;
            try
            {
                if (ddlTrn.SelectedIndex > 0)
                {
                    oTm = new CItem();
                    dt = oTm.GetSDItemList(Convert.ToInt32(ddlTrn.SelectedValue));
                    gvItm.DataSource = dt;
                    gvItm.DataBind();
                }
            }
            finally
            {
                oTm = null;
                dt = null;
            }
        }

        protected void txtRecDt_TextChanged(object sender, EventArgs e)
        {
            popTransaction("A", 0); ;
        }
    }
}