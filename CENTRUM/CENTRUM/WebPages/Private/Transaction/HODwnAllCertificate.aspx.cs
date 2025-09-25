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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Net;


namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class HODwnAllCertificate : CENTRUMBase
    {
        string ImagePath = "";
        string PathDeathDoc = ConfigurationManager.AppSettings["PathDeathDoc"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    btnSave.Enabled = false;
                else
                    btnSave.Enabled = true;
                //popLO();
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
                this.PageHeading = "Download and Verification Document";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHoDownloadCertificate);
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
                ddlCenter.DataTextField = "Market";
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
                dt = oFs.GetAllDownDocList(vBrCode, vMarketId, vEoid, vLogDt);
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
                ImageButton btnPhotoIdDwn = ((ImageButton)e.Row.FindControl("btnPhotoIdDwn"));
                ImageButton btnBPDown = ((ImageButton)e.Row.FindControl("btnBPDown"));
                ImageButton btnDCDown = ((ImageButton)e.Row.FindControl("btnDCDown"));
                ImageButton btnClaimStDown = ((ImageButton)e.Row.FindControl("btnClaimStDown"));
                ImageButton btnHospiDown = ((ImageButton)e.Row.FindControl("btnHospiDown"));

                TextBox txtbankRecipDate = ((TextBox)e.Row.FindControl("txtbankRecipDate"));

                Label lblNomIdProofDoc = ((Label)e.Row.FindControl("lblNomIdProofDoc"));
                Label lblBPDoc = ((Label)e.Row.FindControl("lblBPDoc"));
                Label lblDCDoc = ((Label)e.Row.FindControl("lblDCDoc"));
                Label lblCSDoc = ((Label)e.Row.FindControl("lblCSDoc"));
                Label lblHospiCash = ((Label)e.Row.FindControl("lblHospiCash"));
                if (lblNomIdProofDoc.Text != "")
                {
                    btnPhotoIdDwn.Visible = true;
                }
                if (lblBPDoc.Text != "")
                {
                    btnBPDown.Visible = true;
                }
                if (lblDCDoc.Text != "")
                {
                    btnDCDown.Visible = true;
                }
                if (lblCSDoc.Text != "")
                {
                    btnClaimStDown.Visible = true;
                }
                if (lblHospiCash.Text == "HospiCash")
                {
                    btnHospiDown.Visible = true;
                }

            }
        }

        private DataTable Xml()
        {         
            DataTable dt = new DataTable("UpdateDwnVerify");
            dt.Columns.Add("LoanId");
            dt.Columns.Add("VeriftYN");
            dt.Columns.Add("SendBack");
            dt.Columns.Add("Revoked");
            dt.Columns.Add("Remarks");
            dt.Columns.Add("Typ");
            foreach (GridViewRow gr in gvSecRef.Rows)
            {
                CheckBox chkVerify = (CheckBox)gr.FindControl("chkVerify");
                CheckBox chkSendBack = (CheckBox)gr.FindControl("chkSendBack");
                CheckBox chkRevoke = (CheckBox)gr.FindControl("chkRevoke");
                Label lbLoanId = (Label)gr.FindControl("lbLoanId");
                TextBox txtRemarks = (TextBox)gr.FindControl("txtRemarks");
                Label lblHospiCash = (Label)gr.FindControl("lblHospiCash");
                if (chkVerify.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["LoanId"] = lbLoanId.Text;
                    dr["VeriftYN"] = "Y";
                    dr["SendBack"] = "N";
                    dr["Revoked"] = "N";
                    dr["Remarks"] = "";
                    dr["Typ"] = lblHospiCash.Text;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();                   
                }
                else if(chkSendBack.Checked==true)
                {
                    DataRow dr = dt.NewRow();
                    dr["LoanId"] = lbLoanId.Text;
                    dr["VeriftYN"] = "N";
                    dr["SendBack"] = "Y";
                    dr["Revoked"] = "N";
                    dr["Remarks"] = txtRemarks.Text;
                    dr["Typ"] = lblHospiCash.Text;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
                else if (chkRevoke.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["LoanId"] = lbLoanId.Text;
                    dr["VeriftYN"] = "N";
                    dr["SendBack"] = "N";
                    dr["Revoked"] = "Y";
                    dr["Remarks"] = txtRemarks.Text;
                    dr["Typ"] = lblHospiCash.Text;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
            }
            return dt;
        }

        private void SaveRecords(string Mode)
        {

            string vBranch = "", vFinYear = "";
            int vCreatedBy = 0;
            vBranch = Session[gblValue.BrnchCode].ToString();
            vFinYear = Session[gblValue.FinYear].ToString();
            vCreatedBy = Convert.ToInt32(Session[gblValue.UserId]);
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
                    vErr = oFS.UpdateDeathMemDOcument(vXml, vLoginDt,vCreatedBy);
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

        protected void DownloadDoc(string vLoanId, string vFileName)
        {            
            try
            {                
                ImagePath = ConfigurationManager.AppSettings["pathDeathMember"];
                string filename = string.Format("{0}/{1}/{2}", ImagePath, vLoanId, vFileName);  
                Int32 flength = filename.Length;
                string fname;
                fname = vFileName;
                if (fname != "")
                {
                    if (File.Exists(filename))
                    {
                        Response.AddHeader("Content-Type", "Application/octet-stream");
                        Response.AddHeader("Content-Disposition", "attachment;   filename=" + fname);
                        Response.WriteFile(filename);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        string[] arrPathNetwork = PathDeathDoc.Split(',');                        
                        string vPathDeathDoc = "";
                        for (int i = 0; i <= arrPathNetwork.Length - 1; i++)
                        {
                            if (isValidUrl(arrPathNetwork[i] + "DeathMember/" + vLoanId + "/" + vFileName))
                            {
                                vPathDeathDoc = arrPathNetwork[i] + "DeathMember/" + vLoanId + "/" + vFileName;
                                break;
                            }
                            else if (isValidUrl(arrPathNetwork[i] + "jlgdeathmember/" + vLoanId + "_" + vFileName))
                            {
                                vPathDeathDoc = arrPathNetwork[i] + "jlgdeathmember/" + vLoanId + "_" + vFileName;
                                break;
                            }
                        }
                        if (vPathDeathDoc != "")
                        {
                            WebClient cln = new WebClient();
                            byte[] vDoc = cln.DownloadData(vPathDeathDoc);
                            Response.AddHeader("Content-Type", "Application/octet-stream");
                            Response.AddHeader("Content-Disposition", "attachment;   filename=" + fname);
                            Response.BinaryWrite(vDoc);
                            Response.Flush();
                            Response.End();
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("No Data Found..");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        protected void btnPhotoIdDwn_Click(object sender, EventArgs e)
        {
            ImageButton button = sender as ImageButton;
            GridViewRow gvRow = (GridViewRow)button.NamingContainer;
            Label lbLoanId = (Label)gvRow.FindControl("lbLoanId");
            Label lblNomIdProofDoc = (Label)gvRow.FindControl("lblNomIdProofDoc");
            DownloadDoc(lbLoanId.Text, "Nominee_PhotoId_Proof" + lblNomIdProofDoc.Text);
        }

        protected void btnBPDown_Click(object sender, EventArgs e)
        {
            ImageButton button = sender as ImageButton;
            GridViewRow gvRow = (GridViewRow)button.NamingContainer;
            Label lbLoanId = (Label)gvRow.FindControl("lbLoanId");
            Label lblBPDoc = (Label)gvRow.FindControl("lblBPDoc");
            DownloadDoc(lbLoanId.Text, "Nominee_Bank_PassBook" + lblBPDoc.Text);
        }

        protected void btnDCDown_Click(object sender, EventArgs e)
        {
            ImageButton button = sender as ImageButton;
            GridViewRow gvRow = (GridViewRow)button.NamingContainer;
            Label lbLoanId = (Label)gvRow.FindControl("lbLoanId");
            Label lblDCDoc = (Label)gvRow.FindControl("lblDCDoc");
            DownloadDoc(lbLoanId.Text, "Death_Certificate" + lblDCDoc.Text);
        }

        protected void btnClaimStDown_Click(object sender, EventArgs e)
        {
            ImageButton button = sender as ImageButton;
            GridViewRow gvRow = (GridViewRow)button.NamingContainer;
            Label lbLoanId = (Label)gvRow.FindControl("lbLoanId");
            Label lblCSDoc = (Label)gvRow.FindControl("lblCSDoc");
            DownloadDoc(lbLoanId.Text, "Claiment_Statement" + lblCSDoc.Text);
        }

        protected void btnHospiDown_Click(object sender, EventArgs e)
        {
            ImageButton button = sender as ImageButton;
            GridViewRow gvRow = (GridViewRow)button.NamingContainer;
            Label lbLoanId = (Label)gvRow.FindControl("lbLoanId");
            DownloadDoc(lbLoanId.Text, "Hospi_Cash_Claim_Sickness.pdf");
        }

        #region URLExist
        public static bool isValidUrl(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
                using (WebResponse response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}