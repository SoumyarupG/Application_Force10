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

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class ActivityApproval : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                StatusButton("View");
                LoadGrid();
                PopBranch(Session[gblValue.UserName].ToString());
                txtAsOnDt.Text = Session[gblValue.LoginDate].ToString();
                txtTotalDistance.Text = "0";
                txtApproveDistance.Text = "0";
                tbQly.ActiveTabIndex = 0;
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Activity Approval";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " [ Login Date : " + Session[gblValue.LoginDate].ToString() + " ]";
                this.GetModuleByRole(mnuID.mnuActivityApproval);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Activity Approval", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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
                    // btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = false;
                    // btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    //btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    // btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    // btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid();
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
                tbQly.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
        }

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
                    LoadGrid();
                    tbQly.ActiveTabIndex = 0;
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
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
                Response.Write(ex.Message.ToString());
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbQly.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
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
                ViewState["StateEdit"] = null;
            }
        }

        private void LoadGrid()
        {
            DataTable dt = null;
            CActivity oActiv = null;
            try
            {
                oActiv = new CActivity();
                dt = oActiv.GetActivityListByBranch(ddlBranch.SelectedValue, gblFuction.setDate(txtAsOnDt.Text));
                gvActivity.DataSource = dt.DefaultView;
                gvActivity.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                oActiv = null;
                dt = null;
            }
        }

        protected void gvActivity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string pEoId = "";
            DataTable dt = null;
            CActivity oActiv = null;
            try
            {
                pEoId = Convert.ToString(e.CommandArgument);
                ViewState["EoId"] = pEoId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvActivity.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oActiv = new CActivity();
                    dt = oActiv.GetMapData(ddlBranch.SelectedValue, gblFuction.setDate(txtAsOnDt.Text), pEoId);
                    gvTracker.DataSource = dt;
                    gvTracker.DataBind();
                    txtTotalDistance.Text = dt.Rows[0]["TotalDistance"].ToString();
                    txtApproveDistance.Text = dt.Rows[0]["TotalApproveDistance"].ToString();
                    tbQly.ActiveTabIndex = 1;
                    StatusButton("Show");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                dt = null;
                oActiv = null;
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0;
            string vXml = XmlFromGrid();
            CActivity oAct = null;
            try
            {
                oAct = new CActivity();
                vErr = oAct.ActivityTrackerApprove(gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vXml, Convert.ToInt32(Session[gblValue.UserId]),
                    Convert.ToDouble(txtTotalDistance.Text),Convert.ToDouble(txtApproveDistance.Text));
                if (vErr > 0)
                {
                    gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                    vResult = true;
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblMarg.DBError);
                    vResult = false;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                oAct = null;
            }
            return vResult;
        }

        private void EnableControl(Boolean Status)
        {
            //txtActivity.Enabled = Status;
            //ddlDesig.Enabled = Status;
            
        }

        private void ClearControls()
        {
            // txtActivity.Text = "";
            //ddlDesig.SelectedIndex = -1;            
            lblDate.Text = "";
            lblUser.Text = "";           
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (Convert.ToString(row["BranchCode"]) != Convert.ToString(Session[gblValue.BrnchCode]))
                        {
                            row.Delete();
                        }
                    }
                    dt.AcceptChanges();
                }
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        private String XmlFromGrid()
        {
            String vXML = "";
            DataTable dt = new DataTable("Table1");
            dt.Columns.Add("TrackerID");
            dt.Columns.Add("AppproveYN");
            foreach (GridViewRow gr in gvTracker.Rows)
            {
                CheckBox chkApproveYN = (CheckBox)gr.FindControl("chkApproveYN");
                if (chkApproveYN.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["AppproveYN"] = "Y";
                    dr["TrackerID"] = gr.Cells[7].Text;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
            }
            if (dt.Rows.Count > 0)
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXML = oSW.ToString();
                }
            }
            return vXML;
        }

        protected void gvTracker_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkApproveYN = (CheckBox)e.Row.FindControl("chkApproveYN");
                    if (e.Row.Cells[8].Text == "Y")
                    {
                        chkApproveYN.Checked = true;
                        chkApproveYN.Enabled = false;
                    }
                    else
                    {
                        chkApproveYN.Checked = false;
                        chkApproveYN.Enabled = true;
                    }
                }
            }
            finally
            {

            }
        }

        protected void chkApproveYN_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                if (checkbox.Checked == true)
                {
                    string vTotalDistance = Convert.ToString(Convert.ToDouble(txtTotalDistance.Text) + Convert.ToDouble(row.Cells[4].Text));
                    txtTotalDistance.Text = vTotalDistance;
                    txtApproveDistance.Text = vTotalDistance;
                }
                else
                {
                    string vTotalDistance = Convert.ToString(Convert.ToDouble(txtTotalDistance.Text) - Convert.ToDouble(row.Cells[4].Text));
                    txtTotalDistance.Text = vTotalDistance;
                    txtApproveDistance.Text = vTotalDistance;
                }
            }
            finally
            {
            }
        }

    }
}