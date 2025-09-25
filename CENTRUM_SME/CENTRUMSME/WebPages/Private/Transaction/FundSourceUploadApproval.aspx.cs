using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Configuration;
using CENTRUMCA;
using CENTRUMBA;
using Newtonsoft.Json;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class FundSourceUploadApproval : CENTRUMBAse
    {
        string vMobService = ConfigurationManager.AppSettings["MobService"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopDtl(Session[gblValue.LoginDate].ToString());
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                //this.PageHeading = "HO To Branch Fund Transfer (With Reverse)";
                this.PageHeading = "Source of Fund Bulk Update - Chacker";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuSFIDUpdateMELAppr);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Source of Fund Bulk Update - Chacker", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopDtl(string pDate)
        {
            CProvisionalDeathDeclare oAv = null;

            DataTable dt = null;
            DateTime vDate = gblFuction.setDate(pDate);
            try
            {
                oAv = new CProvisionalDeathDeclare();
                dt = oAv.PendingFundSourceUpload(vDate);
                gvDtl.DataSource = dt;
                gvDtl.DataBind();
            }
            catch
            {
                gvDtl.DataSource = null;
                gvDtl.DataBind();
            }
            finally
            {
                oAv = null;
                dt = null;
            }
        }

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


        protected void btnVerify_Click(object sender, EventArgs e)
        {
            Button btnEqVerify = (Button)sender;
            GridViewRow gR = (GridViewRow)btnEqVerify.NamingContainer;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            int vErr = 0;
            Int64 vFSUID = Convert.ToInt64(gR.Cells[1].Text.Trim());
            int vCreatedBy = 0;
            vCreatedBy = Convert.ToInt32(Session[gblValue.UserId]);

            CProvisionalDeathDeclare oAv = null;

            try
            {
                oAv = new CProvisionalDeathDeclare();
                vErr = oAv.ApproveFundSourceUpload_Initial(vFSUID, vLogDt, vCreatedBy, "Y");
                if (vErr == 0)
                {
                    var req = new InsertBulkFunderUploadApprove()
                    {

                        pFSUID = vFSUID.ToString(),
                        pLoginDt = (string)Session[gblValue.LoginDate],
                        pAppBy = vCreatedBy.ToString(),
                        pAppRej = "Y"
                    };

                    string Requestdata = JsonConvert.SerializeObject(req);
                    GenerateReport("InsertBulkFunderUploadApprove", Requestdata, vMobService);
                }
                else if (vErr == 2)
                {
                    gblFuction.AjxMsgPopup("Maker user and checker user can not be same.");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data not saved.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oAv = null;
                PopDtl(Session[gblValue.LoginDate].ToString());
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            Button btnEqVerify = (Button)sender;
            GridViewRow gR = (GridViewRow)btnEqVerify.NamingContainer;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            int vErr = 0;
            Int64 vFSUID = Convert.ToInt64(gR.Cells[1].Text.Trim());
            int vCreatedBy = 0;
            vCreatedBy = Convert.ToInt32(Session[gblValue.UserId]);

            CProvisionalDeathDeclare oAv = null;

            try
            {
                oAv = new CProvisionalDeathDeclare();
                vErr = oAv.ApproveFundSourceUpload_Initial(vFSUID, vLogDt, vCreatedBy, "N");
                if (vErr == 0)
                {
                    var req = new InsertBulkFunderUploadApprove()
                    {

                        pFSUID = vFSUID.ToString(),
                        pLoginDt = (string)Session[gblValue.LoginDate],
                        pAppBy = vCreatedBy.ToString(),
                        pAppRej = "N"
                    };

                    string Requestdata = JsonConvert.SerializeObject(req);
                    GenerateReport("InsertBulkFunderUploadApprove", Requestdata, vMobService);
                }
                else if (vErr == 2)
                {
                    gblFuction.AjxMsgPopup("Maker user and checker user can not be same.");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data not saved.");
                }
            }
            catch
            {

            }
            finally
            {
                oAv = null;
                PopDtl(Session[gblValue.LoginDate].ToString());
            }
        }

        private void GenerateReport(string pApiName, string pRequestdata, string pReportUrl)
        {
            string vMsg = "";
            CApiCalling oAPI = new CApiCalling();
            try
            {
                vMsg = oAPI.GenerateReport(pApiName, pRequestdata, pReportUrl);
            }
            finally
            {
                gblFuction.AjxMsgPopup("Fund Source Upload process is running on background.");
            }
        }

        public class InsertBulkFunderUploadApprove
        {

            public string pFSUID { get; set; }
            public string pLoginDt { get; set; }
            public string pAppBy { get; set; }
            public string pAppRej { get; set; }

        }
    }
}