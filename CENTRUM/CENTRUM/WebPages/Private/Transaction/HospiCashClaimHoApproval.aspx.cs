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
    public partial class HospiCashClaimHoApproval : CENTRUMBase
    {
        string ImagePath = "";
        string PathHospiDoc = ConfigurationManager.AppSettings["PathHospiDoc"];

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
                txtToDt.Text = (string)Session[gblValue.LoginDate];
            }
        }

        private void InitBasePage()
        {
            try
            {
                //if (Session[gblValue.BrnchCode].ToString().Trim() != "0000")
                //{
                //    gblFuction.AjxMsgPopup("This page cannot be opened from branch.");
                //    return;
                //}

                this.Menu = false;
                this.PageHeading = "HospiCash HO Approval";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHospiCashHoApproval);
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
        
        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gvRow = (GridViewRow)chk.NamingContainer;
            CheckBox chkAll = (CheckBox)gvRow.FindControl("chkAll");
            if (chkAll.Checked == true)
            {
                foreach (GridViewRow gr in gvHospiCashHo.Rows)
                {
                    CheckBox chkloan = (CheckBox)gr.FindControl("chkloan");
                    chkloan.Checked = true;
                }
            }
            else
            {
                foreach (GridViewRow gr in gvHospiCashHo.Rows)
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

            string vBrCode = ddlBranch.SelectedValues;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            //DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);

            CDeathDocSnt oLD = null;
            DataTable dt = null;
            try
            {
                oLD = new CDeathDocSnt();
                dt = oLD.GetHospiCashClaimHoList( vToDt, ddlBranch.SelectedValues.Replace("|", ","));
                gvHospiCashHo.DataSource = dt;
                gvHospiCashHo.DataBind();
            }
            finally
            {
                dt = null;
                oLD = null;
            }
        }

        protected void gvHospiCashHo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
        }

        private DataTable Xml()
        {
            DataTable dt = new DataTable("UpdateHospiCashHoApproval");
            dt.Columns.Add("LoanId");
            dt.Columns.Add("HID");
            dt.Columns.Add("VeriftYN");
            dt.Columns.Add("SendBack");
            dt.Columns.Add("Remarks");
            foreach (GridViewRow gr in gvHospiCashHo.Rows)
            {
                CheckBox chkVerify = (CheckBox)gr.FindControl("chkVerify");
                CheckBox chkSendBack = (CheckBox)gr.FindControl("chkSendBack");
                string  vLoanId = (string)gr.Cells[13].Text;
                TextBox txtRemarks = (TextBox)gr.FindControl("txtRemarks");
                string vHID = (string)gr.Cells[15].Text;

                if (chkVerify.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["LoanId"] = vLoanId;
                    dr["HID"] = vHID;
                    dr["VeriftYN"] = "Y";
                    dr["SendBack"] = "N";
                    dr["Remarks"] = "";
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
                else if (chkSendBack.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["LoanId"] = vLoanId;
                    dr["HID"] = vHID;
                    dr["VeriftYN"] = "N";
                    dr["SendBack"] = "Y";
                    dr["Remarks"] = txtRemarks.Text;
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
                CDeathDocSnt oFS = null;
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
                    oFS = new CDeathDocSnt();
                    vErr = oFS.UpdateHospiCashHoApproval(vXml, vLoginDt, vCreatedBy);
                    if (vErr > 0)
                    {
                        gvHospiCashHo.DataSource = null;
                        gvHospiCashHo.DataBind();
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
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void DownloadDoc(string vLoanId, string vHid, string vFileName)
        {
            try
            {
                string[] arrPathNetwork = PathHospiDoc.Split(',');
                string vPathDeathDoc = "";
                for (int i = 0; i <= arrPathNetwork.Length - 1; i++)
                {
                    if (isValidUrl(arrPathNetwork[i] + vLoanId + "_" + vHid + "_" + vFileName))
                    {
                        vPathDeathDoc = arrPathNetwork[i] + vLoanId + "_" + vHid + "_" + vFileName;
                        break;
                    }
                }
                if (vPathDeathDoc != "")
                {
                    WebClient cln = new WebClient();
                    byte[] vDoc = cln.DownloadData(vPathDeathDoc);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + vFileName);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Data Found..");
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        protected void btnHospiDown_Click(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(((ImageButton)sender).NamingContainer);
            string vHid = (string)gvRow.Cells[15].Text;
            string lbLoanId = (string)gvRow.Cells[13].Text;
            DownloadDoc(lbLoanId,vHid, "Hospi_Cash_Claim.pdf");
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