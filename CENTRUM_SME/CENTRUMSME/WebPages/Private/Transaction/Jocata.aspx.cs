using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using System.Data;
using CENTRUMBA;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Xml;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class Jocata : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                LoadJocataList(1);
                ClearControls();
                StatusButton("View");
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Jocata Status";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuJocataStatus);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Jocata Status", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadJocataList(1);
        }

        private void LoadJocataList(int vPgNo)
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oMem.GetJocataList(vBrCode, txtSearch.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    gvJocataList.DataSource = dt;
                    gvJocataList.DataBind();
                }
                else
                {
                    gvJocataList.DataSource = null;
                    gvJocataList.DataBind();
                }
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

        protected void gvJocataList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string pCustId = "";
            ClearControls();
            pCustId = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                // ViewState["ScreeningID"] = gvRow.Cells[4].Text;
                foreach (GridViewRow gr in gvJocataList.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
                txtApplicantName.Text = gvRow.Cells[2].Text;
                txtApplicantId.Text = pCustId;
                txtLoanDt.Text = gvRow.Cells[4].Text;
                txtLoanNo.Text = gvRow.Cells[3].Text;
                txtScreeningId.Text = gvRow.Cells[7].Text;
                txtScreeningStatus.Text = gvRow.Cells[8].Text;
                txtJocataRemarks.Text = gvRow.Cells[9].Text;
                ddlRiskCategory.SelectedIndex = ddlRiskCategory.Items.IndexOf(ddlRiskCategory.Items.FindByValue(gvRow.Cells[10].Text));
                ViewState["LoanId"] = gvRow.Cells[3].Text;
                tbJocaStat.ActiveTabIndex = 1;
                StatusButton("Show");
            }
        }

        private void ClearControls()
        {
            txtApplicantId.Text = "";
            txtApplicantName.Text = "";
            txtJocataRemarks.Text = "";
            txtLoanDt.Text = "";
            txtLoanNo.Text = "";
            txtScreeningId.Text = "";
            txtScreeningStatus.Text = "";
            ddlRiskCategory.SelectedIndex = -1;
            ddlApprove.SelectedIndex = -1;
            txtRemarks.Text = "";
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.CanEdit == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Edit);
                return;
            }
            tbJocaStat.ActiveTabIndex = 1;
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
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
                    // gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_ddlLoanSector");
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
                    // gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_ddlLoanSector");
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
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
            }
        }

        private void EnableControl(Boolean Status)
        {
            ddlApprove.Enabled = Status;
            txtRemarks.Enabled = Status;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbJocaStat.ActiveTabIndex = 0;
            StatusButton("View");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                LoadJocataList(1);
                EnableControl(false);
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
                return;
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            CDisburse oDb = new CDisburse();
            int vErr = 0;
            Boolean vResult = false;           
            string vLoanId = Convert.ToString(ViewState["LoanId"]);
            vErr = oDb.UpdateJocataStatus(vLoanId, txtScreeningId.Text, ddlApprove.SelectedValue, Convert.ToInt32(Session[gblValue.UserId]), txtRemarks.Text, ddlRiskCategory.SelectedValue);
            if (vErr == 0)
            {
                if (ddlApprove.SelectedValue == "Y")
                {
                    Prosidex(txtApplicantId.Text, vLoanId, Convert.ToInt32(Session[gblValue.UserId]));
                }              
                ViewState["ScreeningID"] = "";
                vResult = true;
            }
            return vResult;
        }

        #region Prosidex Integration
        public ProsidexResponse Prosidex(string pMemberID, string pLoanId, Int32 pCreatedBy)
        {
            DataTable dt = new DataTable();
            CMember oDb = new CMember();

            ProsidexRequest pReqData = new ProsidexRequest();
            Request pReq = new Request();
            DG pDg = new DG();
            ProsidexResponse pResponseData = null;

            dt = oDb.GetProsidexReqData(pMemberID, pCreatedBy);
            if (dt.Rows.Count > 0)
            {
                pDg.ACCOUNT_NUMBER = dt.Rows[0]["ACCOUNT_NUMBER"].ToString();
                pDg.ALIAS_NAME = dt.Rows[0]["ALIAS_NAME"].ToString();
                pDg.APPLICATIONID = pLoanId;
                pDg.Aadhar = dt.Rows[0]["Aadhar"].ToString();
                pDg.CIN = dt.Rows[0]["CIN"].ToString();
                pDg.CKYC = dt.Rows[0]["CKYC"].ToString();
                pDg.CUSTOMER_STATUS = dt.Rows[0]["CUSTOMER_STATUS"].ToString();
                pDg.CUST_ID = dt.Rows[0]["CUST_ID"].ToString();
                pDg.DOB = dt.Rows[0]["DOB"].ToString();
                pDg.DrivingLicense = dt.Rows[0]["DrivingLicense"].ToString();
                pDg.Father_First_Name = dt.Rows[0]["Father_First_Name"].ToString();
                pDg.Father_Last_Name = dt.Rows[0]["Father_Last_Name"].ToString();
                pDg.Father_Middle_Name = dt.Rows[0]["Father_Middle_Name"].ToString();
                pDg.Father_Name = dt.Rows[0]["Father_Name"].ToString();
                pDg.First_Name = dt.Rows[0]["First_Name"].ToString();
                pDg.Middle_Name = dt.Rows[0]["Middle_Name"].ToString();
                pDg.Last_Name = dt.Rows[0]["Last_Name"].ToString();
                pDg.Gender = dt.Rows[0]["Gender"].ToString();
                pDg.GSTIN = dt.Rows[0]["GSTIN"].ToString();
                pDg.Lead_Id = dt.Rows[0]["Lead_Id"].ToString();
                pDg.NREGA = dt.Rows[0]["NREGA"].ToString();
                pDg.Pan = dt.Rows[0]["Pan"].ToString();
                pDg.PassportNo = dt.Rows[0]["PassportNo"].ToString();
                pDg.RELATION_TYPE = dt.Rows[0]["RELATION_TYPE"].ToString();
                pDg.RationCard = dt.Rows[0]["RationCard"].ToString();
                pDg.Registration_NO = dt.Rows[0]["Registration_NO"].ToString();
                pDg.SALUTATION = dt.Rows[0]["SALUTATION"].ToString();
                pDg.TAN = dt.Rows[0]["TAN"].ToString();
                pDg.Udyam_aadhar_number = dt.Rows[0]["Udyam_aadhar_number"].ToString();
                pDg.VoterId = dt.Rows[0]["VoterId"].ToString();
                pDg.Tasc_Customer = dt.Rows[0]["Tasc_Customer"].ToString();

                pReq.ACE = new List<object>();
                pReq.UnitySfb_RequestId = dt.Rows[0]["RequestId"].ToString();
                pReq.CUST_TYPE = "I";
                pReq.CustomerCategory = "I";
                pReq.MatchingRuleProfile = "1";
                pReq.Req_flag = "QA";
                pReq.SourceSystem = "BIJLI";
                pReq.DG = pDg;
                pReqData.Request = pReq;
            }
            pResponseData = ProsidexSearchCustomer(pReqData);
            return pResponseData;
        }

        public ProsidexResponse ProsidexSearchCustomer(ProsidexRequest prosidexRequest)
        {
            string vResponse = "", vUCIC = "", vRequestId = "", vMemberId = "", vPotentialYN = "N", vPotenURL = "", vLoanId = "";
            Int32 vCreatedBy = 1;
            ProsidexResponse oProsidexResponse = null;
            CMember oDb = new CMember();
            try
            {
                vRequestId = prosidexRequest.Request.UnitySfb_RequestId;
                vMemberId = prosidexRequest.Request.DG.CUST_ID;
                vLoanId = prosidexRequest.Request.DG.APPLICATIONID;

                string Requestdata = JsonConvert.SerializeObject(prosidexRequest);               
                string postURL = "https://ucic.unitybank.co.in:9002/UnitySfbWS/searchCustomer";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                dynamic res = JsonConvert.DeserializeObject(vResponse);

                if (res.Response.StatusInfo.ResponseCode == "200")
                {
                    vUCIC = res.Response.StatusInfo.POSIDEX_GENERATED_UCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, vUCIC == null ? 300 : 200);
                    vPotentialYN = vUCIC == null ? "Y" : "N";
                    vPotenURL = vUCIC == null ? res.Response.StatusInfo.CRM_URL : "";
                }
                else
                {
                    vUCIC = vUCIC == null ? "" : vUCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 500);
                }
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oDb.SaveProsidexLog(vMemberId, vLoanId, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
                //--------------------------------------------------------------------------------------                
                return oProsidexResponse;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oDb.SaveProsidexLog(vMemberId, vLoanId, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
                //--------------------------------------------------------------------------------------
                oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 500);
            }
            finally
            {
            }
            return oProsidexResponse;
        }
        #endregion

        #region Common
        public string AsString(XmlDocument xmlDoc)
        {
            string strXmlText = null;
            if (xmlDoc != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter tx = new XmlTextWriter(sw))
                    {
                        xmlDoc.WriteTo(tx);
                        strXmlText = sw.ToString();
                    }
                }
            }
            return strXmlText;
        }
      
        #endregion
    }
}