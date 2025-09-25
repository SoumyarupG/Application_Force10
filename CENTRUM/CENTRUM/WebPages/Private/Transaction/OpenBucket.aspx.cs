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
using System.Configuration;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class OpenBucket : CENTRUMBase
    {
        string vCBUrl = ConfigurationManager.AppSettings["CBUrl"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                PopBranch();
                txtFrmDt.Text = txtToDt.Text = Session[gblValue.LoginDate].ToString();
                StatusButton("View");
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
                    ClearControls();
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
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Open Bucket";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuOpenBucket);
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
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Open Bucket", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopBranch()
        {
            CUser oUsr = null;
            DataTable dt = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]), "R");
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

        private void ClearControls()
        {
            ddlOp.SelectedIndex = -1;
            gvDtl.DataSource = null;
            gvDtl.DataBind();
            txtAmount.Text = "0.00";
            txtUserRecomAmt.Text = "0.00";
        }

        private void EnableControl(Boolean Status)
        {
            ddlOp.Enabled = Status;
            txtUserRecomAmt.Enabled = Status;
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
                tabCgt.ActiveTabIndex = 1;
                // StatusButton("Add");
                //ClearControls();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabCgt.ActiveTabIndex = 0;
            //EnableControl(false);
            //StatusButton("View");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid();
                StatusButton("View");
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vEnqId = Convert.ToString(ViewState["EnqId"]);
            DataTable dtMain = (DataTable)ViewState["DeviationMain"];
            DataTable dtDtl = (DataTable)ViewState["DeviationDtl"];
            //DataRow dR = dt.Select("[EnquiryId]=" + vEnqId).FirstOrDefault();
            string vXmlMain = "", vXmlDtl = "";
            Int32 vCBID = Convert.ToInt32(ViewState["CbId"]);
            string vDesig = Convert.ToString(Session[gblValue.Designation]).Trim().ToUpper();
            double pRecomAmt = Convert.ToDouble(txtUserRecomAmt.Text == "" ? "0" : txtUserRecomAmt.Text);

            if (vDesig == "BUH")
            {
                for (int i = dtMain.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtMain.Rows[i];
                    if (Convert.ToString(dr["EnquiryId"]) != vEnqId)
                    {
                        dr.Delete();
                        dtMain.AcceptChanges();
                    }
                }

                vXmlMain = dtToXml(dtMain);
                vXmlDtl = dtToXml(dtDtl);
                CDeviationMatrix oCDM = null;
                int vErr = 0;
                string vFileExistYN = "N";

                oCDM = new CDeviationMatrix();
                vErr = oCDM.SaveDeviationMatrix(vXmlMain, vXmlDtl,"","", Convert.ToString(Session[gblValue.UserName]), "", "", Convert.ToInt32(Session[gblValue.UserId]), vFileExistYN, "O", pRecomAmt);
                if (vErr > 0)
                {
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
                    vResult = false;
                }
            }
            else if (vDesig == "R&SI")
            {
                CDeviationMatrix oCDM = null;
                int vErr = 0;
                oCDM = new CDeviationMatrix();
                string vRemarks = "";
                vErr = oCDM.ApproveDeviationMatrix(vEnqId, vCBID, vRemarks, ddlOp.SelectedValue, Convert.ToInt32(Session[gblValue.UserId]), Convert.ToDouble(txtUserRecomAmt.Text),0,0,"ARM","","");
                if (vErr > 0)
                {
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
                gblFuction.AjxMsgPopup("Reccomendation not allowed.");
                vResult = false;
            }
            return vResult;
        }

        private String dtToXml(DataTable dt)
        {
            String vSpecXML = "";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vSpecXML = oSW.ToString();
            }
            return vSpecXML;
        }



        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            DataTable dt = null;
            CDeviationMatrix oCB = null;
            string pBranch = ddlBranch.SelectedValues.Replace("|", ",");
            string pAppMode = "";
            pAppMode = Convert.ToString(Session[gblValue.Designation]).Trim() == "R&SI" ? "OR" : "O";
            try
            {
                oCB = new CDeviationMatrix();
                dt = oCB.GetDeviationData(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), pAppMode, pBranch);
                ViewState["DeviationMain"] = dt;
                gvDeviationMat.DataSource = dt;
                gvDeviationMat.DataBind();
            }
            finally
            {
                dt = null;
                oCB = null;
            }
        }

        protected void gvDeviationMat_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBranchCode = "";
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                string vEnqId = gvRow.Cells[18].Text.Trim();
                ViewState["EnqId"] = vEnqId;
                Int32 vCbId = Convert.ToInt32(gvRow.Cells[19].Text.Trim());
                ViewState["CbId"] = vCbId;
                //-----------------------------------------------------------------------------
                vBranchCode = gvRow.Cells[4].Text.Trim();
                //-----------------------------------------------------------------------------
                foreach (GridViewRow gr in gvDeviationMat.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Blue;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
                //------------------------------------------------------------------------------ 
                DataSet ds = null;
                DataTable dt, dt1, dt2 = null;
                CDeviationMatrix oCDM = null;

                if (Convert.ToString(Session[gblValue.Designation]) == "BUH")
                {
                    dt = new DataTable();
                    oCDM = new CDeviationMatrix();
                    ds = oCDM.GetDeviationDataById(vEnqId, vCbId, "BM");
                    dt = ds.Tables[0];
                    ViewState["DeviationDtl"] = dt;
                    gvDtl.DataSource = dt;
                    gvDtl.DataBind();
                    popOperation("BUH", Convert.ToDouble(dt.Rows[0]["TotalOS"]));

                }
                else if (Convert.ToString(Session[gblValue.Designation]) == "R&SI")
                {
                    dt = new DataTable();
                    oCDM = new CDeviationMatrix();
                    ds = oCDM.GetDeviationDataById(vEnqId, vCbId, "CM");
                    dt = ds.Tables[0];
                    ViewState["DeviationDtl"] = dt;
                    txtUserRecomAmt.Text = dt.Rows[0]["CMRecomAmt"].ToString();
                    gvDtl.DataSource = dt;
                    gvDtl.DataBind();
                    popOperation("R&SI", Convert.ToDouble(dt.Rows[0]["TotalOS"]));
                }
                tabCgt.ActiveTabIndex = 1;
                StatusButton("Show");
            }

            if (e.CommandName == "cmdCbReport")
            {
                GridViewRow gvRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                string vEnqId = gvRow.Cells[18].Text.Trim();
                Int32 vCbId = Convert.ToInt32(gvRow.Cells[19].Text.Trim());             
                string url = vCBUrl + "?vEnqId=" + vEnqId + "&vCbId=" + vCbId;
                string s = "window.open('" + url + "', 'popup_window', 'width=900,height=600,left=100,top=100,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
        }

        private void popOperation(string pDesig, double vTotalOS)
        {
            ListItem oli = null;
            ddlOp.Items.Clear();
            oli = new ListItem("<--Select-->", "-1");

            double vSysRecAmt = 250000.00 - vTotalOS;

            if (pDesig == "BUH")
            {
                ddlOp.Items.Insert(0, oli);
                oli = new ListItem("Reccomendation", "R");
                ddlOp.Items.Insert(1, oli);
            }
            else if (pDesig == "R&SI")
            {
                ddlOp.Items.Insert(0, oli);
                oli = new ListItem("Approve", "A");
                ddlOp.Items.Insert(1, oli);
                oli = new ListItem("Reject", "C");
                ddlOp.Items.Insert(2, oli);
                oli = new ListItem("Send Back", "S");
                ddlOp.Items.Insert(3, oli);
            }
            else
            {
            }
            txtAmount.Text = vSysRecAmt < 0 ? "0" : Convert.ToString(vSysRecAmt);
        }
    }
}