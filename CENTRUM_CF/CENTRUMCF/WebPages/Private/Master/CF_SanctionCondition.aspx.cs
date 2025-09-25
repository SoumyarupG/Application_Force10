using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using System.Net;
using System.IO;

namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_SanctionCondition : CENTRUMBAse
    {
        protected int cPgNo = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBasePage();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
               
               
                if (Session[gblValue.LeadID] != null)
                {
                    hdLeadId.Value = Convert.ToString(Session[gblValue.LeadID]);
                    GenerateSanctionGrid(Convert.ToInt64(hdLeadId.Value));
                    StatusButton("Show");
                }
                else
                {
                    StatusButton("Close");
                    gblFuction.MsgPopup("Please Select Lead from Loan Application Status screen and proceed...");
                    return;
                }
               
                tbBankDtl.ActiveTabIndex = 1;
            }
            
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Sanction Condition";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFSanction);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {

                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    // btnDelete.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Sanction Condition", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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
            tbBankDtl.ActiveTabIndex = 1;
            EnableControl(false, this.Page);
            StatusButton("Show");
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {

                case "Show":
                    EnableControl(false, this.Page);
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    gvSancDtl.Enabled = false;
                    break;

                case "View":
                    EnableControl(false, this.Page);
                    btnEdit.Enabled = false;
                    btnShow.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    gvSancDtl.Enabled = false;
                    break;
                case "Exit":
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    break;
                case "Edit":
                    EnableControl(true, this.Page);
                    btnEdit.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    gvSancDtl.Enabled = true;
                    break;
                case "Close":
                    btnEdit.Visible = false;                  
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false, this.Page);
                    break;
            }
        }
        private void EnableControl(Boolean Status, Control parent)
        {

            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Enabled = Status;
                }
                else if (c is DropDownList)
                {
                    ((DropDownList)c).Enabled = Status;
                }
                else if (c is FileUpload)
                {
                    ((FileUpload)c).Enabled = Status;
                }

                // Recursively disable controls inside the containers like
                // panels or group controls
                if (c.Controls.Count > 0)
                {
                    EnableControl(Status, c);

                }

            }
            txtSearch.Enabled = true;
            btnShow.Enabled = true;

        }              
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);              
                ViewState["StateEdit"] = null;
                GenerateSanctionGrid(Convert.ToInt64(hdLeadId.Value));
                StatusButton("Show");               
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0;
            string vLeadId = "", vBrCode = "", vErrMsg = "";
            DataTable dt = null;
            string vXmlSanc = "";
            Int32 UID = 0;
            UID = Convert.ToInt32(Session[gblValue.UserId]);

            CDistrict oDis = null;

            try
            {
                if (hdLeadId.Value != "")
                {
                    vLeadId = hdLeadId.Value;
                }


                vBrCode = Session[gblValue.BrnchCode].ToString();

               // makeData(); //Geting data from the Sanction Grid

                if (makeData() == true)
                {
                    dt = (DataTable)ViewState["Sanc"];
                    Int32 vNumOfObli = 0;
                    DataRow[] vrows;
                    vrows = dt.Select("SanctionDesc <> '' and SanctionDesc <> '-1'");
                    vNumOfObli = vrows.Length;
                    if (vNumOfObli == 0)
                    {
                        gblFuction.MsgPopup("Please insert atleast one Sanction Description");
                        return false;
                    }

                    dt.AcceptChanges();
                    dt.TableName = "Table";
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlSanc = oSW.ToString();
                    }

                    if (dt.Rows.Count > 0)
                    {
                        oDis = new CDistrict();
                        vErr = oDis.CF_SaveSactionConditn(Convert.ToInt64(vLeadId), vXmlSanc, vBrCode, UID, 0, ref vErrMsg);
                        if (vErr == 0)
                        {
                            gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(vErrMsg);
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
                oDis = null;
            }

        }
        protected void gvSancDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDel")
            {
                DataTable dt = null;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                dt = (DataTable)ViewState["Sanc"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    ViewState["Sanc"] = dt;
                    gvSancDtl.DataSource = dt;
                    gvSancDtl.DataBind();
                }
                else
                {
                    gblFuction.AjxMsgPopup("First Row can not be deleted.");
                    return;
                }
            }
        }
        private void ClearControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = "";
                }
                if (c is Label)
                {
                    ((Label)c).Text = "";
                }
                else if (c is DropDownList)
                {
                    ((DropDownList)c).SelectedIndex = -1;
                }

                // Recursively disable controls inside the containers like
                // panels or group controls
                if (c.Controls.Count > 0)
                {
                    ClearControls(c);

                }
            }

        }
        private void GenerateSanctionGrid(Int64 LeadID)
        {
            ClearControls(this.Page);
            DataSet ds = null;
            DataTable dt = null;
            DataTable dt1 = null;
            CDistrict ODis = null;
            try
            {
                if (Session[gblValue.ApplNm] != null)
                {
                    lblApplNm.Text = Convert.ToString(Session[gblValue.ApplNm]);
                }
                if (Session[gblValue.BCPNO] != null)
                {
                    lblBCPNum.Text = Convert.ToString(Session[gblValue.BCPNO]);
                }
                ODis = new CDistrict();
                ds = ODis.CF_GenerateSanctionGrid(LeadID);
                dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    DataRow dF;
                    dF = dt.NewRow();
                    dt.Rows.Add(dF);
                    dt.AcceptChanges();
                }               
                ViewState["Sanc"] = dt;
                gvSancDtl.DataSource = dt;
                gvSancDtl.DataBind();
              
            }

            finally
            {
            }
        }
        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            DataRow dr;
            if (makeData() == true)
            {
                DataTable dt = (DataTable)ViewState["Sanc"];
                dt.AcceptChanges();
                dr = dt.NewRow();
                dt.Rows.Add(dr);

                ViewState["Sanc"] = dt;
                gvSancDtl.DataSource = dt;
                gvSancDtl.DataBind();
              
            }
        }
        protected void btnAddRow_ClickOLD(object sender, EventArgs e)
        {
            DataTable dt = null;
            Int32 vR = 0;
            DataRow dr;
            dt = (DataTable)ViewState["Sanc"];
            if (dt.Rows.Count > 0)
            {
                vR = dt.Rows.Count - 1;

                Label lblSrlNo = (Label)gvSancDtl.Rows[vR].FindControl("lblSrlNo");
                dt.Rows[vR]["SanctionId"] = lblSrlNo.Text == "" ? "0" : lblSrlNo.Text;


                TextBox txtSanctionDesc = (TextBox)gvSancDtl.Rows[vR].FindControl("txtSanctionDesc");
                dt.Rows[vR]["SanctionDesc"] = txtSanctionDesc.Text == "" ? "" : txtSanctionDesc.Text;



                if (dt.Rows[gvSancDtl.Rows.Count - 1]["SanctionDesc"].ToString() == "")
                {
                    gblFuction.AjxMsgPopup("Sanction Description is Blank...");
                    return;
                }
                else
                {

                    dt.AcceptChanges();
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);

                    ViewState["Sanc"] = dt;
                    gvSancDtl.DataSource = dt;
                    gvSancDtl.DataBind();
                    UpFamily.Update();
                }
            }


        }
        public Boolean makeData()
        {
            DataTable dt = (DataTable)ViewState["Sanc"];
            foreach (GridViewRow gr in gvSancDtl.Rows)
            {
                Label lblSrlNo = (Label)gvSancDtl.Rows[gr.RowIndex].FindControl("lblSrlNo");
                TextBox txtSanctionDesc = (TextBox)gvSancDtl.Rows[gr.RowIndex].FindControl("txtSanctionDesc");

                if (txtSanctionDesc.Text.ToString() == "")
                {
                    gblFuction.AjxMsgPopup("Sanction Description is Blank..");
                    return false;
                }

              

                dt.Rows[gr.RowIndex]["SanctionId"] = Convert.ToInt32(lblSrlNo.Text == "" ? "" : lblSrlNo.Text);
                dt.Rows[gr.RowIndex]["SanctionDesc"] = Convert.ToString(txtSanctionDesc.Text == "" ? "" : txtSanctionDesc.Text);

                //if (dt.Rows[gvSancDtl.Rows.Count - 1]["SanctionDesc"].ToString() == "")
                //{
                //    gblFuction.AjxMsgPopup("Sanction Description is Blank...");
                //    return false;
                //}

            }

            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            gvSancDtl.DataSource = dt;
            gvSancDtl.DataBind();
            return true;
        }  
    }
}