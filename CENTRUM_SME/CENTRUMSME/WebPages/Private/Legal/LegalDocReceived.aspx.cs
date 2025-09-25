using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using SendSms;

namespace CENTRUMSME.WebPages.Private.Legal
{
    public partial class LegalDocReceived : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtOrgDocRecDate.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["RecYN"] = "N";
                ViewState["State"] = null;
                ViewState["CusTID"] = null;
                ViewState["LoanAppId"] = null;
                GetBranchForFinalLegal();
                hdUserID.Value = this.UserID.ToString();
                mView.ActiveViewIndex = 0;
                GetPendLegalDocRecList();
                StatusButton("View");
                //txttext.Attributes.Add("onkeypress", "return numericOnly(this);");
            }
            else
            {

            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Legal List Of Document Receive";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLegalDocReceived);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = false;
                    //btnAppEdit.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = true;
                    //btnAppEdit.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = true;
                    //btnAppEdit.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = false;
                    //btnAppEdit.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Legal Document Receive", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void ViewAcess()
        {
            if (mView.ActiveViewIndex == 1)
            {
                this.Menu = false;
                this.PageHeading = "Loan Application";
                this.GetModuleByRole(mnuID.mnuAppLnApplication);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 2)
            {
                this.Menu = false;
                this.PageHeading = "Personal Discussion By BM";
                this.GetModuleByRole(mnuID.mnuPDBM);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 3)
            {
                this.Menu = false;
                this.PageHeading = "Personal Discussion By CM";
                this.GetModuleByRole(mnuID.mnuPDCM);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 4)
            {
                this.Menu = false;
                this.PageHeading = "Balance Sheet Information";
                this.GetModuleByRole(mnuID.mnuBSStatement);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 5)
            {
                this.Menu = false;
                this.PageHeading = "Profit And Loss Information";
                this.GetModuleByRole(mnuID.mnuPLStatement);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 6)
            {
                this.Menu = false;
                this.PageHeading = "Reference Information";
                this.GetModuleByRole(mnuID.mnuReference);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else
            {
                this.Menu = false;
                this.PageHeading = "Personal Discussion";
                this.GetModuleByRole(mnuID.mnuPD);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    //btnAppAdd.Enabled = false;
                    //btnAppEdit.Enabled = false;
                    btnExit.Enabled = false;
                    // ClearControls();
                    break;
                case "Show":
                    //btnAppAdd.Enabled = false;
                    //btnAppEdit.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    //btnAppAdd.Enabled = false;
                    //btnAppEdit.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    //btnAppAdd.Enabled = true;
                    //btnAppEdit.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    //btnAppAdd.Enabled = true;
                    //btnAppEdit.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    //btnAppAdd.Visible = false;
                    //btnAppEdit.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        private void EnableControl(Boolean Status)
        {
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            GetPendLegalDocRecList();
        }
        private void GetBranchForFinalLegal()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            dt = oDisb.GetBranchForFinalLegal(Session[gblValue.BrnchCode].ToString());
            if (dt.Rows.Count > 0)
            {
                ddlLegBr.DataSource = dt;
                ddlLegBr.DataValueField = "BranchCode";
                ddlLegBr.DataTextField = "BranchName";
                ddlLegBr.DataBind();
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    ListItem liSel = new ListItem("ALL", "0000");
                    ddlLegBr.Items.Insert(0, liSel);
                }
            }
            else
            {
                ddlLegBr.DataSource = null;
                ddlLegBr.DataBind();
            }
        }
        private void GetPendLegalDocRecList()
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBrCode = ddlLegBr.SelectedValue.ToString();
            string vSearchType = ddlSearchType.SelectedValue.ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetPendLegalDocRecList(txtSearch.Text.Trim(), vBrCode, vSearchType);
                gvLoanApp.DataSource = null;
                gvLoanApp.DataBind();
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvLoanApp.DataSource = dt1;
                        gvLoanApp.DataBind();
                    }
                    else
                    {
                        gvLoanApp.DataSource = null;
                        gvLoanApp.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt1 = null;
                oMem = null;
            }

        }
        private void ShowLoanRelationDetails(string pLnAppId)
        {
            if (pLnAppId != "")
            {
                // get List Of Documents
                GetDocListByAppId(pLnAppId);
            }
        }
        protected void gvLoanApp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vLoanAppId = "";
            vLoanAppId = Convert.ToString(e.CommandArgument);
            ViewState["LoanAppId"] = vLoanAppId;
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvLoanApp.Rows)
                {
                    if ((gr.RowIndex) % 2 == 0)
                    {
                        gr.BackColor = backColor;
                        gr.ForeColor = foreColor;
                    }
                    else
                    {
                        gr.BackColor = System.Drawing.Color.White;
                        gr.ForeColor = foreColor;
                    }
                    gr.Font.Bold = false;
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                tbMem.ActiveTabIndex = 1;
                ShowLoanRelationDetails(vLoanAppId);
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        private void GetDocListByAppId(string pLnAppId)
        {
            ClearDocList();
            DataTable dt = null, dt1 = null;
            CApplication oCG = null;
            oCG = new CApplication();
            dt = oCG.GetReceivedDocListByLnAppId(pLnAppId);
            if (dt.Rows.Count > 0)
            {
                lblDocListId.Text = dt.Rows[0]["DocListId"].ToString();
                txtDocRecListLnAppNo.Text = dt.Rows[0]["LoanAppId"].ToString();
                txtDocRecListCustName.Text = dt.Rows[0]["CustName"].ToString();
                ddlLegalConfirm.SelectedValue = dt.Rows[0]["IsConfirmByLegalForDisb"].ToString();
                txtOrgDocRecDate.Text = dt.Rows[0]["LegOriginalDocRecDate"].ToString();
                gvDocList.DataSource = dt;
                gvDocList.DataBind();
                btnDocListSave.Enabled = false;
                btnDocListUpdate.Enabled = true;
                btnDocListDelete.Enabled = true;
                dt.Columns.Remove("LoanAppId");
                dt.Columns.Remove("SanctionID");
                dt.Columns.Remove("DocListId");
                dt.AcceptChanges();
                ViewState["DocDtl"] = dt;
            }
            else
            {
                dt1 = oCG.GetFinalLegDocListByLnAppId(pLnAppId);
                if (dt1.Rows.Count > 0)
                {
                    txtDocRecListLnAppNo.Text = dt1.Rows[0]["LoanAppId"].ToString();
                    txtDocRecListCustName.Text = dt1.Rows[0]["CustName"].ToString();
                    txtOrgDocRecDate.Text = Session[gblValue.LoginDate].ToString();
                    ddlLegalConfirm.SelectedValue = "H";
                    gvDocList.DataSource = dt1;
                    gvDocList.DataBind();
                    dt1.Columns.Remove("LoanAppId");
                    dt1.Columns.Remove("SanctionID");
                    dt1.Columns.Remove("DocListId");
                    dt1.AcceptChanges();
                    ViewState["DocDtl"] = dt1;
                }
                else
                {
                    lblDocListId.Text = "";
                    txtDocRecListLnAppNo.Text = pLnAppId;
                    GetDocList();
                }
                btnDocListSave.Enabled = true;
                btnDocListUpdate.Enabled = false;
                btnDocListDelete.Enabled = false;
            }
        }
        private void ClearDocList()
        {
            lblDocListId.Text = "";
            gvDocList.DataSource = null;
            gvDocList.DataBind();
        }
        protected void btnDocListSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Save";
            SaveReceiveDocList(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnDocListUpdate_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Edit";
            SaveReceiveDocList(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnDocListDelete_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Delete";
            SaveReceiveDocList(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        private void SaveReceiveDocList(string pMode)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");

            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string pLnAppId = txtDocRecListLnAppNo.Text.ToString();
            if (pLnAppId == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank");
                return;
            }
            CApplication oFS = null;
            DataTable dtXml = CreateXmlData();
            if (ddlLegalConfirm.SelectedValue=="P" && ViewState["RecYN"].ToString() == "N")
            {
                gblFuction.AjxMsgPopup("You have to receive atleast one document for Confirmation By Legal(For Disbursement)..");
                return;
            }
            string pLegalConfirForDisb= ddlLegalConfirm.SelectedValue.ToString();
            DateTime porgDocRecDate = gblFuction.setDate(txtOrgDocRecDate.Text);
            int vErr = 0;
            string vXml = "";
            try
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
                oFS = new CApplication();
                if (pMode == "Save")
                {
                    vErr = oFS.SaveDocReceiveListBulk(pLnAppId, vXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save", 
                        porgDocRecDate, pLegalConfirForDisb);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Record Save Successfully");
                        return;
                    }
                    else
                    {
                        gblFuction.MsgPopup("Error: Data not Saved");
                        return;
                    }
                }
                if (pMode == "Edit")
                {
                    vErr = oFS.SaveDocReceiveListBulk(pLnAppId, vXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save", 
                        porgDocRecDate, pLegalConfirForDisb);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Record Updated Successfully");
                        return;
                    }
                    else
                    {
                        gblFuction.MsgPopup("Error: Data not Saved");
                        return;
                    }
                }
                if (pMode == "Delete")
                {
                    vErr = oFS.SaveDocReceiveListBulk(pLnAppId, vXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete", 
                        porgDocRecDate, pLegalConfirForDisb);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Record Deleted Successfully");
                        return;
                    }
                    else
                    {
                        gblFuction.MsgPopup("Error: Data not Saved");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtXml = null;
                oFS = null;
            }
        }
        protected void gvDocList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CApplication oMem = new CApplication();
            DataTable dt = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlDocType = (e.Row.FindControl("ddlDocType") as DropDownList);
                Label lblDocTypeId = (e.Row.FindControl("lblDocTypeId") as Label);
                Label lblRecv = (e.Row.FindControl("lblRecv") as Label);
                CheckBox chkRecv = (e.Row.FindControl("chkRecv") as CheckBox);

                try
                {
                    dt = oMem.GetDocType();
                    if (dt.Rows.Count > 0)
                    {
                        ddlDocType.DataSource = dt;
                        ddlDocType.DataTextField = "DocTypeName";
                        ddlDocType.DataValueField = "DocTypeId";
                        ddlDocType.DataBind();
                        ListItem oli1 = new ListItem("<--Select-->", "-1");
                        ddlDocType.Items.Insert(0, oli1);
                    }
                    string DocTypeId = lblDocTypeId.Text;
                    if (DocTypeId != " ")
                        ddlDocType.SelectedValue = DocTypeId;
                    if (lblRecv.Text == "Y")
                        chkRecv.Checked = true;
                    else
                        chkRecv.Checked = false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dt = null;
                    oMem = null;
                }
            }
        }
        private void GetDocList()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SLNo", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("DocName", typeof(string));
            dt.Columns.Add("DocNo", typeof(string));
            dt.Columns.Add("DocTypeId", typeof(int));
            dt.Columns.Add("ReceivedYN", typeof(string));

            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[0]["SlNo"] = 1;
            dt.Rows[0]["Date"] = Session[gblValue.LoginDate].ToString();
            dt.Rows[0]["ReceivedYN"] = "N";

            gvDocList.DataSource = dt;
            gvDocList.DataBind();

            ViewState["DocDtl"] = dt;

        }
        private DataTable CreateXmlData()
        {
            DataTable dt = new DataTable("DocList");
            dt.Columns.Add("SLNo", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("DocName", typeof(string));
            dt.Columns.Add("DocNo", typeof(string));
            dt.Columns.Add("DocTypeId", typeof(int));
            dt.Columns.Add("ReceivedYN", typeof(string));
           
            DataRow dr;
            foreach (GridViewRow gr in gvDocList.Rows)
            {
                Label lblSLDoc = (Label)gr.FindControl("lblSLDoc");
                TextBox txtDocDate = (TextBox)gr.FindControl("txtDocDate");
                TextBox txtDocName = (TextBox)gr.FindControl("txtDocName");
                TextBox txtDocNo = (TextBox)gr.FindControl("txtDocNo");
                DropDownList ddlDocType = (DropDownList)gr.FindControl("ddlDocType");
                CheckBox chkRecv = (CheckBox)gr.FindControl("chkRecv");
                if (txtDocDate.Text != "")
                {
                    dr = dt.NewRow();
                    dr["SLNo"] = lblSLDoc.Text;
                    dr["Date"] = gblFuction.setDate(txtDocDate.Text.ToString());
                    dr["DocName"] = txtDocName.Text;
                    dr["DocNo"] = txtDocNo.Text;
                    dr["DocTypeId"] = ddlDocType.SelectedValue.ToString();
                    if (chkRecv.Checked == true)
                    {
                        dr["ReceivedYN"] = "Y";
                        ViewState["RecYN"] = "Y";
                    }
                    else
                    {
                        dr["ReceivedYN"] = "N";
                    }
                    dt.Rows.Add(dr);
                }
            }
            dt.AcceptChanges();
            return dt;
        }
        protected void btnAddDocRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["DocDtl"];
                int curRow = 0, maxRow = 0, vRow = 0;
                ImageButton txtCur = (ImageButton)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvDocList.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                Label lblSLDoc = (Label)gvDocList.Rows[curRow].FindControl("lblSLDoc");
                TextBox txtDocDate = (TextBox)gvDocList.Rows[curRow].FindControl("txtDocDate");
                TextBox txtDocName = (TextBox)gvDocList.Rows[curRow].FindControl("txtDocName");
                TextBox txtDocNo = (TextBox)gvDocList.Rows[curRow].FindControl("txtDocNo");
                CheckBox chkRecv = (CheckBox)gvDocList.Rows[curRow].FindControl("chkRecv");
                Label lblRecv = (Label)gvDocList.Rows[curRow].FindControl("lblRecv");

                dt.Rows[curRow][0] = lblSLDoc.Text;
                if (txtDocDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("Date Can Not Be Blank..");
                    return;
                }
                dt.Rows[curRow][1] = txtDocDate.Text;
                dt.Rows[curRow][2] = txtDocName.Text;
                dt.Rows[curRow][3] = txtDocNo.Text;
                if (((DropDownList)gvDocList.Rows[curRow].FindControl("ddlDocType")).SelectedIndex == -1)
                {
                    ((Label)gvDocList.Rows[curRow].FindControl("lblDocTypeId")).Text = "-1";
                }
                else
                {
                    ((Label)gvDocList.Rows[curRow].FindControl("lblDocTypeId")).Text = ((DropDownList)gvDocList.Rows[curRow].FindControl("ddlDocType")).SelectedValue.ToString();
                }
                Label lblDocTypeId = ((Label)gvDocList.Rows[curRow].FindControl("lblDocTypeId"));
                dt.Rows[curRow][4] = Convert.ToInt32(lblDocTypeId.Text);
                if (chkRecv.Checked == true)
                {
                    dt.Rows[curRow][5] = "Y";
                }
                else
                {
                    dt.Rows[curRow][5] = "N";
                }


                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["SlNo"] = Convert.ToInt32(((Label)gvDocList.Rows[vRow].FindControl("lblSLDoc")).Text) + 1;
                dt.Rows[vRow + 1]["Date"] = Session[gblValue.LoginDate].ToString();
                dt.Rows[vRow + 1]["ReceivedYN"] = "N";
                dt.AcceptChanges();

                ViewState["DocDtl"] = dt;
                gvDocList.DataSource = dt;
                gvDocList.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        protected void ImDelDoc_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["DocDtl"];
                if (dt.Rows.Count > 0)
                {
                    if ((dt.Rows.Count - 1) > gR.RowIndex)
                    {
                        gblFuction.AjxMsgPopup("Only Last Row Can Be Deleted ");
                        return;
                    }
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["DocDtl"] = dt;
                    gvDocList.DataSource = dt;
                    gvDocList.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
    }
}