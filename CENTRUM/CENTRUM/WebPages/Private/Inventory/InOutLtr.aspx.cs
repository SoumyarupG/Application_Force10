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
    public partial class InOutLtr : CENTRUMBase
    {
        private int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                StatusButton("View");
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["ItmData"] = null;
                ViewState["ItemData"] = null;
                txtDt.Text = Session[gblValue.LoginDate].ToString();
                LoadGrid(0);
                tbRole.ActiveTabIndex = 0;
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
                this.PageHeading = "Inward / Outward Letter";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuInOutLtr);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Inward Outward", false);
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

                oItm = new CItem();
                dt = oItm.GetInOutLtrPG(pPgIndx, ref vRows, vFrmDt, vToDt, Session[gblValue.BrnchCode].ToString());
                gvList.DataSource = dt;
                gvList.DataBind();
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
        protected void gvDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow Row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            int vRow = Row.RowIndex;
            TextBox txtRecDt = (TextBox)gvDtl.Rows[vRow].FindControl("txtRecDt");
            TextBox txtFrom = (TextBox)gvDtl.Rows[vRow].FindControl("txtFrom");
            TextBox txtPar = (TextBox)gvDtl.Rows[vRow].FindControl("txtPar");
            TextBox txtRef = (TextBox)gvDtl.Rows[vRow].FindControl("txtRef");
            if (e.CommandName == "cmdNewRec")
            {
                if (txtRecDt.Text != "" && txtFrom.Text != "" && txtPar.Text != "" && txtRef.Text != "")
                {
                    NewRow();
                }
                else
                {
                    gblFuction.MsgPopup("Please fill all collumn");
                    return;
                }
            }
            else if (e.CommandName == "cmdDelRec")
                DelRow(vRow);
        }

        /// <summary>
        /// 
        /// </summary>
        private void NewRow()
        {
            DataTable dtNew = null;
            if (ViewState["ItmData"] != null)
            {
                dtNew = (DataTable)ViewState["ItmData"];
                if (dtNew.Rows.Count > 0)
                {
                    DataRow dR = dtNew.NewRow();
                    dR["SlNo"] = dtNew.Rows.Count + 1;
                    dtNew.Rows.Add(dR);
                    ViewState["ItmData"] = dtNew;
                    for (int i = 0; i < dtNew.Rows.Count - 1; i++)
                    {
                        TextBox txtRecDt = (TextBox)gvDtl.Rows[i].FindControl("txtRecDt");
                        TextBox txtFrom = (TextBox)gvDtl.Rows[i].FindControl("txtFrom");
                        TextBox txtPar = (TextBox)gvDtl.Rows[i].FindControl("txtPar");
                        TextBox txtRef = (TextBox)gvDtl.Rows[i].FindControl("txtRef");
                        if (txtRecDt.Text != "" && txtFrom.Text != "" && txtPar.Text != "" && txtRef.Text != "")
                        dtNew.Rows[i]["RecDt"] = txtRecDt.Text;
                        dtNew.Rows[i]["LtrFrom"] = txtFrom.Text;
                        dtNew.Rows[i]["Particular"] = txtPar.Text;
                        dtNew.Rows[i]["RefNo"] = txtRef.Text;
                    }
                }
                gvDtl.DataSource = dtNew;
                gvDtl.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRow"></param>
        private void DelRow(Int32 pRow)
        {
            DataTable dt = (DataTable)ViewState["ItmData"];
            dt.Rows[pRow].Delete();
            dt.AcceptChanges();
            ViewState["ItmData"] = dt;
            gvDtl.DataSource = dt;
            gvDtl.DataBind();
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
            txtDt.Enabled = Status;
            ddlType.Enabled = Status;
            gvDtl.Enabled = Status;
        }

        /// <summary>
        ///  
        /// </summary>
        private void ClearControls()
        {
            txtDt.Text = Session[gblValue.LoginDate].ToString();
            ddlType.SelectedIndex = -1;
            SetInitilize();
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
                string vStaffCode = ddlType.SelectedValue;
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                string vVendId = Convert.ToString(ViewState["VenId"]);
                DateTime vDt = gblFuction.setDate(txtDt.Text);

                dtDtl = (DataTable)ViewState["ItmData"];
                dt = new DataTable();
                dt = dtDtl.Clone();
                foreach (GridViewRow gr in gvDtl.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["SlNo"] = 1;
                    TextBox txtRecDt = (TextBox)gr.FindControl("txtRecDt");
                    TextBox txtFrom = (TextBox)gr.FindControl("txtFrom");
                    TextBox txtPar = (TextBox)gr.FindControl("txtPar");
                    TextBox txtRef = (TextBox)gr.FindControl("txtRef");

                    dr["RecDt"] = gblFuction.setDate(txtRecDt.Text);
                    if (txtFrom.Text == "&nbsp;")
                        dr["LtrFrom"] = "";
                    else
                        dr["LtrFrom"] = txtFrom.Text;

                    if (txtPar.Text == "&nbsp;")
                        dr["Particular"] = "";
                    else
                        dr["Particular"] = txtPar.Text;

                    if (txtRef.Text == "&nbsp;")
                        dr["RefNo"] = "";
                    else
                        dr["RefNo"] = txtRef.Text;

                    if (dr["RecDt"].ToString() != "&nbsp;")
                    {
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                    }
                }
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
                    dChk = oItm.SaveInOutLtr(ref dChk, vDt, ddlType.SelectedValue, Session[gblValue.BrnchCode].ToString(), vXml, "Save", this.UserID);
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
                    dChk = oItm.SaveInOutLtr(ref dChk, vDt, ddlType.SelectedValue, Session[gblValue.BrnchCode].ToString(), vXml, "Edit", this.UserID);
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
                    dChk = oItm.SaveInOutLtr(ref dChk, vDt, ddlType.SelectedValue, Session[gblValue.BrnchCode].ToString(), "", "Del", this.UserID);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pItemName"></param>
        /// <returns></returns>
        private string getItemId(string pItemName)
        {
            string vItemId = string.Empty;
            DataTable dt = (DataTable)ViewState["ItemData"];
            var vID = from row in dt.AsEnumerable()
                      where row.Field<string>("Name") == pItemName
                      select row;
            DataTable dt1 = vID.CopyToDataTable();
            vItemId = dt1.Rows[0][0].ToString();
            return vItemId;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtXml"></param>
        /// <returns></returns>
        private string DtToXml(DataTable dtXml)
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
        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    ds = oItm.GetInOutLtrById(vRecId);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        txtDt.Text = Convert.ToString(dt.Rows[0]["LtrDt"]);
                        ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByValue(Convert.ToString(dt.Rows[0]["LtrType"])));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbRole.ActiveTabIndex = 1;
                        StatusButton("Show");
                        btnAdd.Visible = true;
                    }
                    gvDtl.DataSource = dt1;
                    gvDtl.DataBind();
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

        /// <summary>
        /// 
        /// </summary>
        private void SetInitilize()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SlNo", typeof(int)));
            dt.Columns.Add(new DataColumn("RecDt", typeof(string)));
            dt.Columns.Add(new DataColumn("LtrFrom", typeof(string)));
            dt.Columns.Add(new DataColumn("Particular", typeof(string)));
            dt.Columns.Add(new DataColumn("RefNo", typeof(string)));
            DataRow dr = dt.NewRow();
            dr["SlNo"] = 1;
            dr["RecDt"] = "";
            dr["LtrFrom"] = "";
            dr["Particular"] = "";
            dr["RefNo"] = "";
            dt.Rows.Add(dr);
            ViewState["ItmData"] = dt;
            gvDtl.DataSource = dt;
            gvDtl.DataBind();
        }
    }
}