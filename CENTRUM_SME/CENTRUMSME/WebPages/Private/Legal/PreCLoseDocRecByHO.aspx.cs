using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CENTRUMCA;
using CENTRUMBA;
using System.Drawing;
using System.IO;

namespace CENTRUMSME.WebPages.Private.Legal
{
    public partial class PreCLoseDocRecByHO : CENTRUMBAse
    {
        protected int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch();
                PendPreCloseDocRecBrCode();
                PreCloseDocRecBrCode();
                popCustomer();
                txtFrmDt.Text = gblFuction.putStrDate(gblFuction.setDate(Session[gblValue.LoginDate].ToString()).AddDays(-30));
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                //txtSendDate.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PendingPreCloseDocRecList();
                PreCloseDocRecList();
                ddlCust.Enabled = false;
                ddlSancNo.Enabled = false;
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "PRE-CLOSER DOCUMENTS RECEIVE";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuLegalPreCloseDocRecByHO);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "PRE-CLOSER DOCUMENTS RECEIVE", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void PopBranch()
        {
            CMember oCM = null;
            DataTable dt = null;
            oCM = new CMember();
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = oCM.GetBranch();
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataBind();
                    ListItem oItm = new ListItem("<--Select-->", "-1");
                    ddlBranch.Items.Insert(0, oItm);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCM = null;
            }
        }
        private void PendPreCloseDocRecBrCode()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            dt = oDisb.GetBrForPreCloseDocRecByHO(Session[gblValue.BrnchCode].ToString(), "P");
            if (dt.Rows.Count > 0)
            {
                ddlPenDocRecByHOBr.DataSource = dt;
                ddlPenDocRecByHOBr.DataValueField = "BranchCode";
                ddlPenDocRecByHOBr.DataTextField = "BranchName";
                ddlPenDocRecByHOBr.DataBind();
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    ListItem liSel = new ListItem("ALL", "0000");
                    ddlPenDocRecByHOBr.Items.Insert(0, liSel);
                }
            }
            else
            {
                ddlPenDocRecByHOBr.DataSource = null;
                ddlPenDocRecByHOBr.DataBind();
            }
        }
        private void PreCloseDocRecBrCode()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            dt = oDisb.GetBrForPreCloseDocRecByHO(Session[gblValue.BrnchCode].ToString(), "A");
            if (dt.Rows.Count > 0)
            {
                ddlDocRecByHOBr.DataSource = dt;
                ddlDocRecByHOBr.DataValueField = "BranchCode";
                ddlDocRecByHOBr.DataTextField = "BranchName";
                ddlDocRecByHOBr.DataBind();
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    ListItem liSel = new ListItem("ALL", "0000");
                    ddlDocRecByHOBr.Items.Insert(0, liSel);
                }
            }
            else
            {
                ddlDocRecByHOBr.DataSource = null;
                ddlDocRecByHOBr.DataBind();
            }
        }
        private void popCustomer()
        {
            DataTable dt = null;
            CDisburse oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oCD = new CDisburse();
                dt = oCD.GetCustNameForAgreement();
                ddlCust.DataSource = dt;
                ddlCust.DataTextField = "CompanyName";
                ddlCust.DataValueField = "CustId";
                ddlCust.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCust.Items.Insert(0, oli);
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }
        protected void gvPendDocRecveive_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vSanId = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            CApplication oCG = null;
            try
            {
                vSanId = Convert.ToString(e.CommandArgument);
                ViewState["LnSancId"] = vSanId;
                if (e.CommandName == "cmdShow")
                {
                    oCG = new CApplication();
                    ds = oCG.GetPendPreCloseDocRecBySanctId(vSanId, Convert.ToInt32(Session[gblValue.UserId].ToString()));
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];
                    }
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvPendDocRecveive.Rows)
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

                        ddlCust.Enabled = true;
                        ddlSancNo.Enabled = true;
                        btnPreCloseDocRec.Enabled = true;
                        btnPreCloseDocDelete.Enabled = false;

                        ddlCust.Items.Clear();
                        ddlCust.DataSource = null;
                        ddlCust.DataBind();
                        ListItem liSel = new ListItem();
                        liSel.Text = dt.Rows[0]["CustName"].ToString();
                        liSel.Value = dt.Rows[0]["CustID"].ToString();
                        ddlCust.Items.Insert(0, liSel);

                        PopBranch();
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        PopSanctionNo(dt.Rows[0]["CustID"].ToString());
                        ddlSancNo.SelectedIndex = ddlSancNo.Items.IndexOf(ddlSancNo.Items.FindByValue(dt.Rows[0]["SanctionID"].ToString()));
                        txtLnAppNo.Text = dt.Rows[0]["LoanAppID"].ToString();
                        txtDocRecDate.Text = Convert.ToString(Session[gblValue.LoginDate]);
                        txtDocRecBy.Text = dt.Rows[0]["DocRecBy"].ToString();
                        hdIsDisb.Value = dt.Rows[0]["IsDisbursed"].ToString();
                        tabPreCloseDocRec.ActiveTabIndex = 2;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        gvDocList.DataSource = dt1;
                        gvDocList.DataBind();
                    }
                    else
                    {
                        gvDocList.DataSource = null;
                        gvDocList.DataBind();
                    }
                }
            }
            finally
            {
                ds = null;
                dt = null;
                dt1 = null;
                oCG = null;
            }
        }
        protected void gvDocRecveive_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vSanId = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            CApplication oCG = null;
            try
            {
                vSanId = Convert.ToString(e.CommandArgument);
                ViewState["LnSancId"] = vSanId;
                if (e.CommandName == "cmdShow")
                {
                    oCG = new CApplication();
                    ds = oCG.GetPreCloseDocRecBySanctId(vSanId);
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];
                    }
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvPendDocRecveive.Rows)
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


                        ddlCust.Enabled = true;
                        ddlSancNo.Enabled = true;
                        // chkVerify.Enabled = true;

                        btnPreCloseDocRec.Enabled = false;
                        btnPreCloseDocDelete.Enabled = true;

                        ddlCust.Items.Clear();
                        ddlCust.DataSource = null;
                        ddlCust.DataBind();
                        ListItem liSel = new ListItem();
                        liSel.Text = dt.Rows[0]["CustName"].ToString();
                        liSel.Value = dt.Rows[0]["CustID"].ToString();
                        ddlCust.Items.Insert(0, liSel);

                        PopBranch();
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        PopSanctionNo(dt.Rows[0]["CustID"].ToString());
                        ddlSancNo.SelectedIndex = ddlSancNo.Items.IndexOf(ddlSancNo.Items.FindByValue(dt.Rows[0]["SanctionID"].ToString()));
                        txtLnAppNo.Text = dt.Rows[0]["LoanAppID"].ToString();
                        txtDocRecDate.Text = dt.Rows[0]["DocRecDate"].ToString();
                        txtDocRecBy.Text = dt.Rows[0]["DocRecBy"].ToString();
                        hdIsDisb.Value = dt.Rows[0]["IsDisbursed"].ToString();
                        
                        tabPreCloseDocRec.ActiveTabIndex = 2;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        gvDocList.DataSource = dt1;
                        gvDocList.DataBind();
                    }
                    else
                    {
                        gvDocList.DataSource = null;
                        gvDocList.DataBind();
                    }
                }
            }
            finally
            {
                ds = null;
                dt = null;
                dt1 = null;
                oCG = null;
            }
        }
        protected void ddlCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pCustId = (Request[ddlCust.UniqueID] as string == null) ? ddlCust.SelectedValue : Request[ddlCust.UniqueID] as string;
            if (pCustId != "-1")
                PopSanctionNo(pCustId);
        }
        //private Boolean SaveRecords(string Mode)
        //{
        //    Boolean vResult = false;
        //    Label lblMsg = (Label)Master.FindControl("lblMsg");
        //    Int32 pDocSendBy = Convert.ToInt32(Session[gblValue.UserId]);
        //    //DateTime pDocSendDate = gblFuction.setDate(txtSendDate.Text);
        //    CFundSource OFC = new CFundSource();
        //    Int32 vErr = 0;
        //    DataTable dt = new DataTable("HOLegalDocRec");
        //    dt.Columns.Add("SanctionID");
        //    dt.Columns.Add("LoanAppId");
        //    dt.Columns.Add("HORecYN");
        //    dt.Columns.Add("HORecDate");
        //    DataRow dr;
        //    string HoRecYN = "";
        //    foreach (GridViewRow gr in gvPendDocRecveive.Rows)
        //    {
        //        TextBox txtPreCloseDocRecDate = (TextBox)gr.FindControl("txtPreCloseDocRecDate");
        //        LinkButton btnShow = (LinkButton)gr.FindControl("btnShow");
        //        if (txtPreCloseDocRecDate.Text != "")
        //        {
        //            CheckBox cbApprv = (CheckBox)gr.FindControl("cbApprv");
        //            if (cbApprv.Checked == true)
        //                HoRecYN = "Y";
        //            else
        //                HoRecYN = "N";
        //            dr = dt.NewRow();
        //            dr["SanctionID"] = btnShow.Text;
        //            dr["LoanAppId"] = gr.Cells[2].Text;
        //            dr["HORecYN"] = HoRecYN;
        //            if(txtPreCloseDocRecDate.Text.ToString()=="")
        //            {
        //                gblFuction.MsgPopup("Receive Date Can Not be Empty..");
        //                return false;
        //            }
        //            dr["HORecDate"] = gblFuction.setDate(txtPreCloseDocRecDate.Text.ToString());
        //            dt.Rows.Add(dr);
        //        }
        //    }
        //    dt.AcceptChanges();
        //    if (dt.Rows.Count <= 0)
        //    {
        //        gblFuction.MsgPopup("No Record Found To Save..");
        //        return false;
        //    }
        //    string vXml = "";
        //    using (StringWriter oSW = new StringWriter())
        //    {
        //        dt.WriteXml(oSW);
        //        vXml = oSW.ToString();
        //    }
        //    if (Mode == "Save")
        //    {
        //        vErr = OFC.SavePreCloseDocRecByHO(vXml, this.UserID);
        //        if (vErr > 0)
        //        {
        //            gblFuction.MsgPopup("Preclose Documents Received Successfully..");
        //            vResult = true;
        //            //tabDocSend.ActiveTabIndex = 0;
        //        }
        //        else
        //        {
        //            gblFuction.MsgPopup(gblPRATAM.DBError);
        //            vResult = false;
        //        }
        //    }
            
        //    return vResult;
        //}
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
        //    if (SaveRecords("Save") == true)
        //    {
        //        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
        //        ClearControl();
        //        PendPreCloseDocRecBrCode();
        //        PreCloseDocRecBrCode();
        //        PendingPreCloseDocRecList();
        //        PreCloseDocRecList();
        //        ViewState["StateEdit"] = null;
        //    }
        //}
        protected void PopSanctionNo(string pCustId)
        {
            CDisburse oMem = new CDisburse();
            DataTable dt = new DataTable();
            oMem = new CDisburse();
            dt = oMem.GetSancIdForAgreement(pCustId);
            ddlSancNo.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                ddlSancNo.DataSource = dt;
                ddlSancNo.DataTextField = "SanctionNo";
                ddlSancNo.DataValueField = "SanctionID";
                ddlSancNo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSancNo.Items.Insert(0, oli);
            }
            else
            {
                gblFuction.AjxMsgPopup("Final Sanction is not complete for that Customer....");
                ddlSancNo.DataSource = null;
                ddlSancNo.DataBind();
                return;
            }
        }
        private bool ValidateDate()
        {
            bool vRst = true;
            return vRst;
        }
        private void PendingPreCloseDocRecList()
        {
            DataTable dt = null;
            CApplication oCA = null;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oCA = new CApplication();
                dt = oCA.GetPenPreCloseDocRecByHOList(ddlPenDocRecByHOBr.SelectedValue.ToString());
                if (dt.Rows.Count > 0)
                {
                    gvPendDocRecveive.DataSource = dt;
                    gvPendDocRecveive.DataBind();
                }
                else
                {
                    gvPendDocRecveive.DataSource = null;
                    gvPendDocRecveive.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        private void PreCloseDocRecList()
        {
            DataTable dt = null;
            CApplication oCA = null;
            string vMode = string.Empty, vBrCode = string.Empty;
            DateTime pFDate = gblFuction.setDate(txtFrmDt.Text.ToString());
            DateTime pTDate = gblFuction.setDate(txtToDt.Text.ToString());
            string pSearch = txtSearch.Text.Trim();
            try
            {
                // vMode = rdbSel.SelectedValue;
                vBrCode = (string)Session[gblValue.BrnchCode];
                oCA = new CApplication();
                dt = oCA.GetPreCloseDocRecByHOList(pFDate, pTDate, pSearch, ddlDocRecByHOBr.SelectedValue.ToString());
                if (dt.Rows.Count > 0)
                {
                    gvDocRecveive.DataSource = dt;
                    gvDocRecveive.DataBind();
                }
                else
                {
                    gvDocRecveive.DataSource = null;
                    gvDocRecveive.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        protected void ddlPenDocRecByHOBr_SelectedIndexChanged(object sender, EventArgs e)
        {
            PendingPreCloseDocRecList();
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            PreCloseDocRecList();
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            //switch (e.CommandName)
            //{
            //    case "Prev":
            //        vPgNo = Int32.Parse(lblCrPg.Text) - 1;
            //        break;
            //    case "Next":
            //        vPgNo = Int32.Parse(lblCrPg.Text) + 1;
            //        break;
            //}
            //PendingAgrList(vPgNo);
            //tbEmp.ActiveTabIndex = 0;
        }
        private void ClearControl()
        {
            gvDocList.DataSource = null;
            gvDocList.DataBind();
            ddlBranch.SelectedIndex = -1;
            txtLnAppNo.Text = "";
            txtDocRecDate.Text = "";
            txtDocRecBy.Text = "";
        }
        //protected void cbApprvSelectAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox chk;
        //    foreach (GridViewRow rowItem in gvPendDocRecveive.Rows)
        //    {
        //        chk = (CheckBox)(rowItem.Cells[8].FindControl("cbApprv"));
        //        chk.Checked = ((CheckBox)sender).Checked;
        //    }
        //}
        //protected void gvPendDocRecveive_RowDataBound(object senser, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        Label lblHORecYN = (Label)e.Row.FindControl("lblHORecYN");
        //        TextBox txtPreCloseDocRecDate = (TextBox)e.Row.FindControl("txtPreCloseDocRecDate");
        //        CheckBox chkApp = (CheckBox)e.Row.FindControl("cbApprv");
        //        if (lblHORecYN.Text == "N")
        //            chkApp.Checked = false;
        //        else
        //            chkApp.Checked = true;
        //        if (txtPreCloseDocRecDate.Text == "")
        //        {
        //            txtPreCloseDocRecDate.Text = Session[gblValue.LoginDate].ToString();
        //        }
        //    }
        //}
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
        protected void btnPreCloseDocRec_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Save";
            SavePreCloseDocRec(vStateEdit);
            PendPreCloseDocRecBrCode();
            PreCloseDocRecBrCode();
            PendingPreCloseDocRecList();
            PreCloseDocRecList();
            btnPreCloseDocRec.Enabled = false;
            btnPreCloseDocDelete.Enabled = false;
        }
        protected void btnPreCloseDocDelete_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Delete";
            SavePreCloseDocRec(vStateEdit);
            PendPreCloseDocRecBrCode();
            PreCloseDocRecBrCode();
            PendingPreCloseDocRecList();
            PreCloseDocRecList();
            btnPreCloseDocRec.Enabled = false;
            btnPreCloseDocDelete.Enabled = false;
        }
        private void SavePreCloseDocRec(string pMode)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string pSancId = "";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (((Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string) != "-1")
                pSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            else
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No to update verification details");
                return;
            }
            string pLnAppId = txtLnAppNo.Text.ToString();
            if (pLnAppId == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank");
                return;
            }
            CApplication oFS = null;
            DataTable dtXml = CreateXmlData();
            //if (ddlLegalConfirm.SelectedValue == "P" && ViewState["RecYN"].ToString() == "N")
            //{
            //    gblFuction.AjxMsgPopup("You have to receive atleast one document for Confirmation By Legal(For Disbursement)..");
            //    return;
            //}
            
            DateTime pPreCloseDocRecDate = gblFuction.setDate(txtDocRecDate.Text);
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
                    vErr = oFS.SavePreCloseDocRecBulk(pSancId,pLnAppId, vXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save",
                        pPreCloseDocRecDate);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Pre Close Document Received Successfully");
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
                    vErr = oFS.SavePreCloseDocRecBulk(pSancId,pLnAppId, vXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save",
                        pPreCloseDocRecDate);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Pre Close Document Received Successfully");
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
                    vErr = oFS.SavePreCloseDocRecBulk(pSancId,pLnAppId, vXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete",
                        pPreCloseDocRecDate);
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
    }
}