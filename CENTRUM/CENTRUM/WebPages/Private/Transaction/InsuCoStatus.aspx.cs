using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class InsuCoStatus : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    btnSave.Enabled = false;
                else
                    btnSave.Enabled = true;
            
                PopBranch(Session[gblValue.UserName].ToString());
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Data Communication With Insurance Company";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDocSendRecivedInsuco);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }


        private void popLO()
        {
            DataTable dt = null;
            CEO oCM = null;
            string vBrCode = ddlBranch.SelectedValue;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                oCM = new CEO();
                dt = oCM.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlLO.DataSource = dt;
                ddlLO.DataTextField = "EOName";
                ddlLO.DataValueField = "EOID";
                ddlLO.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLO.Items.Insert(0, oli);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                oCM = null;
                dt = null;
            }
        }

        private void popCenter(string vLOId)
        {
            DataTable dt = null;
            CCenter oCL = null;

            try
            {
                oCL = new CCenter();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                dt = oCL.PopCenterWithCollDay(vLOId, vLogDt, vBrCode, "N"); //With No CollDay
                ViewState["CentreForDt"] = dt;
                ddlCenter.DataSource = dt;
                ddlCenter.DataTextField = "Center";
                ddlCenter.DataValueField = "MarketId";
                ddlCenter.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oli);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                oCL = null;
                dt = null;
            }
        }
        
        protected void ddlLO_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vLOId = ddlLO.SelectedItem.Value;
            popCenter(vLOId);
        }       

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gvRow = (GridViewRow)chk.NamingContainer;
            CheckBox chkAll = (CheckBox)gvRow.FindControl("chkAll");
            if (chkAll.Checked == true)
            {

                foreach (GridViewRow gr in gvSecRef.Rows)
                {
                    CheckBox chkloan = (CheckBox)gr.FindControl("chkloan");
                    chkloan.Checked = true;
                }
            }
            else
            {
                foreach (GridViewRow gr in gvSecRef.Rows)
                {
                    CheckBox chkloan = (CheckBox)gr.FindControl("chkloan");
                    chkloan.Checked = false;
                }
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }


        private void LoadGrid()
        {

            string vBrCode = ddlBranch.SelectedValue;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vMarketId = "-1", vEoid = "-1";
            if (ddlLO.SelectedIndex > 0)
                vEoid = ddlLO.SelectedValue;
            if (ddlCenter.SelectedIndex > 0)
                vMarketId = ddlCenter.SelectedValue;
        
            CFundSource oFs = null;
            DataTable dt = null;
            try
            {
                oFs = new CFundSource();
                dt = oFs.GetPenDocSendList(vBrCode, vMarketId, vEoid, vLogDt, rdBtnSend.SelectedValue);
                gvSecRef.DataSource = dt;
                gvSecRef.DataBind();
            }
            finally
            {
                dt = null;
                oFs = null;
            }
        }

        protected void gvSecRef_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtDocSendDate = ((TextBox)e.Row.FindControl("txtDocSendDate"));
                TextBox txtDocRecivedDate = ((TextBox)e.Row.FindControl("txtDocRecivedDate"));
                if (rdBtnSend.SelectedValue == "S")
                {
                    txtDocSendDate.Enabled = true;
                    txtDocRecivedDate.Enabled = false;
                }
                else
                {
                    txtDocSendDate.Enabled = false;
                    txtDocRecivedDate.Enabled = true;
                }
            }
        }

        private DataTable Xml()
        {
            Int32 i = 0;

            DataTable dt = new DataTable("UpdateDOCSend");
            dt.Columns.Add("LoanId");
            dt.Columns.Add("Date");
            foreach (GridViewRow gr in gvSecRef.Rows)
            {

                Label lbLoanId = (Label)gr.FindControl("lbLoanId");
                TextBox txtDocSendDate = (TextBox)gr.FindControl("txtDocSendDate");
                TextBox txtDocRecivedDate = (TextBox)gr.FindControl("txtDocRecivedDate");
                if (rdBtnSend.SelectedValue == "S")
                {
                    if (!string.IsNullOrEmpty(txtDocSendDate.Text))
                    {
                        DataRow dr = dt.NewRow();
                        dr["LoanId"] = lbLoanId.Text;
                        dr["Date"] = gblFuction.setDate(txtDocSendDate.Text);
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                        i++;
                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["LoanId"] = lbLoanId.Text;
                    dr["Date"] = gblFuction.setDate(txtDocRecivedDate.Text);
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                    i++;
                }

            }

            return dt;
        }


        private void SaveRecords(string Mode)
        {
            string vBranch = "", vFinYear = "", vGroup = "";
            int vCreatedBy = 0;
            vBranch = Session[gblValue.BrnchCode].ToString();
            vFinYear = Session[gblValue.FinYear].ToString();
            vCreatedBy = this.UserID;
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            if (Mode == "Save")
            {
                CFundSource oFS = null;
                DataTable dtXml = Xml();
                if (dtXml.Rows.Count == 0)
                {
                    gblFuction.AjxMsgPopup("Select Member To Declaration");
                    return;
                }

                int vErr = 0;
                string vXml = "";
                try
                {
                    using (StringWriter oSW = new StringWriter())
                    {
                        dtXml.WriteXml(oSW);
                        vXml = oSW.ToString();
                    }
                    oFS = new CFundSource();
                    vErr = oFS.UpdateInsuDocDt(vXml, vLoginDt, rdBtnSend.SelectedValue, Convert.ToInt32(Session[gblValue.UserId]));
                    if (vErr > 0)
                    {
                        gvSecRef.DataSource = null;
                        gvSecRef.DataBind();
                        ClearControls();
                        btnSave.Enabled = false;
                        gblFuction.AjxMsgPopup("Records Save Successfully");

                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Error: Data not Saved");
                    }
                }
                catch
                {
                    gblFuction.AjxMsgPopup("Error: Data not Saved");
                }
                finally
                {
                    dtXml = null;
                    oFS = null;
                }
            }
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                    {
                        dt.DefaultView.RowFilter = "BranchCode ='" + Convert.ToString(Session[gblValue.BrnchCode]) + "'";
                    }
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ListItem liSel = new ListItem("<-- Select -->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecords("Save");
        }

        private void ClearControls()
        {
            ddlLO.SelectedValue = "-1";

            ddlCenter.ClearSelection();
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            popLO();
        }

        protected void rdBtnSend_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvSecRef.DataSource = null;
            gvSecRef.DataBind();
        }
    }
}