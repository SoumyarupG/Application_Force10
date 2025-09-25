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
    public partial class CF_SolarSystem : CENTRUMBAse
    {
        protected int cPgNo = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBasePage();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                
              
                ViewState["SolarId"] = null;
                ViewState["SolarType"] = null;
                if (Session[gblValue.LeadID] != null)
                {
                    hdLeadId.Value = Convert.ToString(Session[gblValue.LeadID]);
                    GenerateSolarSystemGrid(Convert.ToInt64(hdLeadId.Value));
                    StatusButton("Show");
                    CheckOprtnStatus(Convert.ToInt64(hdLeadId.Value));
                }
                else
                {
                    StatusButton("Close");
                    gblFuction.MsgPopup("Please Select Lead from Loan Application Status screen and proceed...");
                    return;
                }
                
                tbSolarDtl.ActiveTabIndex = 0;
            }
            
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Solar Power System Details";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFSolarDtl);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Solar Power System Details", false);
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
            
            tbSolarDtl.ActiveTabIndex = 0;
            StatusButton("Show");
           // EnableControl(false, this.Page);
           // StatusButton("View");
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
                    gvSolarDtl.Enabled = false;
                    break;

                case "View":
                    EnableControl(false, this.Page);
                    btnEdit.Enabled = false;
                  //  btnShow.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    gvSolarDtl.Enabled = false;
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
                    gvSolarDtl.Enabled = true;
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
            //txtSearch.Enabled = true;
            //btnShow.Enabled = true;

        }
        private void ClearControls()
        {

            txtAddress.Text = "";
            ddlPropOwnership.SelectedIndex = -1;
            txtPropOwnerName.Text = "";

        }
        protected void CheckOprtnStatus(Int64 vLeadID)
        {
            Int32 vErr = 0;
            CMember oMem = null;
            oMem = new CMember();
            vErr = oMem.CF_chkOperatnStatus(vLeadID);
            if (vErr == 1)
            {
                btnSave.Enabled = false;
                btnEdit.Enabled = false;
                gblFuction.MsgPopup("This Lead is Under Process at Operation Stage.You can not Change or Update it...");
                return;
            }
            else
            {
                //btnSave.Enabled = true;
                btnEdit.Enabled = true;
            }
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
                GenerateSolarSystemGrid(Convert.ToInt64(hdLeadId.Value));
                StatusButton("Show");                  
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0;
            string vLeadId = "", vBrCode = "", vErrMsg = "";
            DataTable dt = null;
            string vXmlSolar = "";
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



                if (makeData() == true)
                {
                    dt = (DataTable)ViewState["Solar"];
                    Int32 vNumOfObli = 0;
                    DataRow[] vrows;
                    vrows = dt.Select("AssetDesc <> '' and AssetDesc <> '-1'");
                    vNumOfObli = vrows.Length;
                    if (vNumOfObli == 0)
                    {
                        gblFuction.MsgPopup("Please insert atleast one Solar Power Details");
                        return false;
                    }

                    dt.AcceptChanges();
                    dt.TableName = "Table";
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlSolar = oSW.ToString();
                    }

                    if (dt.Rows.Count > 0)
                    {
                        oDis = new CDistrict();
                        vErr = oDis.CF_SaveSolarDtl(vLeadId, vXmlSolar, vBrCode, UID, 0, ref vErrMsg, txtAddress.Text, ddlPropOwnership.SelectedValue, txtPropOwnerName.Text);
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
        protected void gvSolarDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDel")
            {
                DataTable dt = null;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                dt = (DataTable)ViewState["Solar"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    ViewState["Solar"] = dt;
                    gvSolarDtl.DataSource = dt;
                    gvSolarDtl.DataBind();
                }
                else
                {
                    gblFuction.AjxMsgPopup("First Row can not be deleted.");
                    return;
                }
                
                CalculateTotalCost();

            }
        }       
        private void GenerateSolarSystemGrid(Int64 LeadID)
        {
            ClearControls(this.Page);
            DataSet ds = null;
            DataTable dt = null;
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
                ds = ODis.CF_GenerateSolarSystemGrid(LeadID);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    txtAddress.Text = Convert.ToString(dt.Rows[0]["PropAddress"]);
                    ddlPropOwnership.SelectedIndex = ddlPropOwnership.Items.IndexOf(ddlPropOwnership.Items.FindByValue(dt.Rows[0]["PropOwnership"].ToString()));
                    txtPropOwnerName.Text = Convert.ToString(dt.Rows[0]["PropOwnerName"]);

                    ViewState["SolarId"] = Convert.ToString(dt.Rows[0]["SolPwrSysId"]);
                    ViewState["SolarType"] = Convert.ToString(dt.Rows[0]["SolarType"]);

                }
                DataRow dF;
                //dF = dt.NewRow();
                //dt.Rows.Add(dF);
                //dt.AcceptChanges();
                ViewState["Solar"] = dt;
                gvSolarDtl.DataSource = dt;
                gvSolarDtl.DataBind();
                CalculateTotalCost();
               // tbBankDtl.ActiveTabIndex = 0;
            }

            finally
            {
            }
        }
        protected void txtTotalCost_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalCost();

        }
        protected void txtAllowedLTV_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalCost();

        }
        protected void gvSolarDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Solar"];

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSolPwrSysId = (Label)e.Row.FindControl("lblSolPwrSysId");
                Label lblSolarType = (Label)e.Row.FindControl("lblSolarType");

                if (lblSolPwrSysId != null)
                {
                    if (ViewState["SolarId"] != null)
                        lblSolPwrSysId.Text = ViewState["SolarId"].ToString();
                }
                if (lblSolarType != null)
                {
                    if (ViewState["SolarType"] != null)
                        lblSolarType.Text = ViewState["SolarType"].ToString();
                }
            }

        }
        private void CalculateTotalCost()
        {
            decimal totalCost = 0, Cal = 0, totalECost = 0;
            decimal vTCost = 0, vACost = 0;
            foreach (GridViewRow gr in gvSolarDtl.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {

                    TextBox txtTotalCost = (TextBox)gr.FindControl("txtTotalCost");
                    TextBox txtAllowedLTV = (TextBox)gr.FindControl("txtAllowedLTV");
                    Label lblEligibleCost = (Label)gr.FindControl("lblEligibleCost");

                    decimal rowCost = 0;
                    decimal.TryParse(txtTotalCost.Text, out rowCost); //parse the cost from the textbox
                    totalCost += Math.Round(rowCost, 2);


                    if (txtTotalCost != null && txtAllowedLTV != null)
                    {
                        if (txtTotalCost.Text != "" && txtAllowedLTV.Text != "")
                        {
                            if (txtTotalCost.Text != "0" && txtAllowedLTV.Text != "0")
                            {
                                vTCost = Convert.ToDecimal(txtTotalCost.Text);
                                vACost = Convert.ToDecimal(txtAllowedLTV.Text);

                                Cal = (vTCost * vACost) / 100;
                                lblEligibleCost.Text = Convert.ToString(Math.Round(Cal, 2));
                            }
                            else
                            {
                                lblEligibleCost.Text = "0";
                            }
                        }
                        else
                        {
                            lblEligibleCost.Text = "0";
                        }
                    }
                    else
                    {
                        lblEligibleCost.Text = "0";
                    }
                    decimal rowECost = 0;
                    decimal.TryParse(lblEligibleCost.Text, out rowECost); //parse the cost from the textbox
                    totalECost += Math.Round(rowECost, 2);

                }


            }
            GridViewRow footerRow = gvSolarDtl.FooterRow;
            if (footerRow != null)
            {
                Label lbltotal = (Label)gvSolarDtl.FooterRow.FindControl("lbltotal");
                Label lbltotalECost = (Label)gvSolarDtl.FooterRow.FindControl("lbltotalECost");
                if (lbltotal != null)
                {
                    if (totalCost > 0)
                        lbltotal.Text = totalCost.ToString();
                    else
                        lbltotal.Text = "0";
                }
                if (lbltotalECost != null)
                {
                    if (totalCost > 0)
                        lbltotalECost.Text = totalECost.ToString();
                    else
                        lbltotalECost.Text = "0";
                }
                lbltotal.ForeColor = System.Drawing.Color.White;
                lbltotalECost.ForeColor = System.Drawing.Color.White;
            }
            UpFamily.Update();
        }
        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            DataRow dr;
            if (makeData() == true)
            {
                DataTable dt = (DataTable)ViewState["Solar"];
                dt.AcceptChanges();
                dr = dt.NewRow();
                dt.Rows.Add(dr);

                ViewState["Bank"] = dt;
                gvSolarDtl.DataSource = dt;
                gvSolarDtl.DataBind();
                UpFamily.Update();
                CalculateTotalCost();
            }
        }      
        private Boolean makeData()
        {
            DataTable dt = (DataTable)ViewState["Solar"];
            foreach (GridViewRow gr in gvSolarDtl.Rows)
            {
                Label lblSrlNo = (Label)gvSolarDtl.Rows[gr.RowIndex].FindControl("lblSrlNo");
                TextBox txtAssetDesc = (TextBox)gvSolarDtl.Rows[gr.RowIndex].FindControl("txtAssetDesc");
                Label lblSolPwrSysId = (Label)gvSolarDtl.Rows[gr.RowIndex].FindControl("lblSolPwrSysId");
                TextBox txtCapacity = (TextBox)gvSolarDtl.Rows[gr.RowIndex].FindControl("txtCapacity");
                TextBox txtOwnerName = (TextBox)gvSolarDtl.Rows[gr.RowIndex].FindControl("txtOwnerName");
                TextBox txtTotalCost = (TextBox)gvSolarDtl.Rows[gr.RowIndex].FindControl("txtTotalCost");

                TextBox txtAllowedLTV = (TextBox)gvSolarDtl.Rows[gr.RowIndex].FindControl("txtAllowedLTV");
                Label lblEligibleCost = (Label)gvSolarDtl.Rows[gr.RowIndex].FindControl("lblEligibleCost");

                //Inserting data into datatable dt
                if (txtAssetDesc.Text.ToString() == "")
                {
                    gblFuction.AjxMsgPopup("Asset Description is Blank...");
                    return false;
                }
                else if (txtCapacity.Text.ToString() == "")
                {
                    gblFuction.AjxMsgPopup("Solar System Capacity(In KW) is Blank...");
                    return false;
                }
                else if (txtOwnerName.Text.ToString() == "")
                {
                    gblFuction.AjxMsgPopup("Name of Owner(As per Quotation) is Blank...");
                    return false;
                }
                else if ((txtTotalCost.Text.ToString() == "0") || (txtTotalCost.Text.ToString() == ""))
                {
                    gblFuction.AjxMsgPopup("Total Cost is Blank...");
                    return false;
                }
                else if ((txtAllowedLTV.Text.ToString() == "0") || (txtAllowedLTV.Text.ToString() == ""))
                {
                    gblFuction.AjxMsgPopup("Allowed LTV % is Blank...");
                    return false;
                }
                else if (txtAddress.Text == "")
                {
                    gblFuction.AjxMsgPopup("Address of Property where solar to be installed Can not left Blank...");
                    return false;
                }
                else if (ddlPropOwnership.SelectedIndex <= 0)
                {
                    gblFuction.AjxMsgPopup("Ownership of Property where solar to be installed Can not left Blank...");
                    return false;
                }
                else if (txtPropOwnerName.Text == "")
                {
                    gblFuction.AjxMsgPopup("Name of Property Owner Can not left Blank...");
                    return false;
                }
                else
                {
                    dt.Rows[gr.RowIndex]["SolarId"] = Convert.ToInt32(lblSrlNo.Text == "" ? "" : lblSrlNo.Text);
                    dt.Rows[gr.RowIndex]["AssetDesc"] = Convert.ToString(txtAssetDesc.Text == "" ? "" : txtAssetDesc.Text);
                    dt.Rows[gr.RowIndex]["SolPwrSysId"] = Convert.ToInt32(lblSolPwrSysId.Text == "" ? "" : lblSolPwrSysId.Text);
                    dt.Rows[gr.RowIndex]["Capacity"] = Convert.ToString(txtCapacity.Text == "" ? "" : txtCapacity.Text);
                    dt.Rows[gr.RowIndex]["OwnerName"] = Convert.ToString(txtOwnerName.Text == "" ? "" : txtOwnerName.Text);
                    dt.Rows[gr.RowIndex]["TotalCost"] = Convert.ToDouble(txtTotalCost.Text == "" ? "0" : txtTotalCost.Text);
                    dt.Rows[gr.RowIndex]["AllowedLTV"] = Convert.ToDouble(txtAllowedLTV.Text == "" ? "0" : txtAllowedLTV.Text);
                    dt.Rows[gr.RowIndex]["EligibleCost"] = Convert.ToDouble(lblEligibleCost.Text == "" ? "0" : lblEligibleCost.Text);
                }

               
            }
            dt.AcceptChanges();
            ViewState["Solar"] = dt;
            gvSolarDtl.DataSource = dt;
            gvSolarDtl.DataBind();
            return true;
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
    }
}