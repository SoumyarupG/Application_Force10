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
using System.Net;

using CENTRUM.CentrumMobUAT;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class HONEFTAPITransfer : CENTRUMBase
    {
        protected int cPgNo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                //txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtDt.Enabled = false;
                //popState();
                PopBranch(Session[gblValue.UserName].ToString());
                tabReSchdl.ActiveTabIndex = 0;
                StatusButton("View");
                PopDisbBank();
            }
        }
        private void PopDisbBank()
        {
            DataTable dt = null;
            CNEFTTransfer oNeft = null;
            oNeft = new CNEFTTransfer();
            dt = oNeft.PopDisbBank();
            ddlBank.DataSource = dt;
            ddlBank.DataTextField = "Desc";
            ddlBank.DataValueField = "DescId";
            ddlBank.DataBind();
            ListItem olist = new ListItem("<--select-->", "-1");
            ddlBank.Items.Insert(0, olist);
        }

        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlState.DataSource = dt;
                ddlState.DataTextField = "StateName";
                ddlState.DataValueField = "StateId";
                ddlState.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlState.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
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
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                }
                //dt = oUsr.GetBranchByState(pUser, Convert.ToInt32(Session[gblValue.RoleId]), ddlState.SelectedValue);
                //if (dt.Rows.Count > 0)
                //{
                //    ddlBranch.DataSource = dt;
                //    ddlBranch.DataTextField = "BranchName";
                //    ddlBranch.DataValueField = "BranchCode";
                //    ddlBranch.DataBind();                    
                //}

            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "HO NEFT API Transaction";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHONEFTTransferAPI);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnEdit.Visible = false;
                    //btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnEdit.Visible = false;
                    //btnDelete.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnDelete.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    //btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "HO NEFT Transfer API", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopBranch(Session[gblValue.UserName].ToString());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CNEFTTransfer oNeft = null;
            Int32 vRows = 0;
            if (ddlBranch.SelectedValues == "")
            {
                gblFuction.AjxMsgPopup("Please Select atleast one branch...");
                return;
            }
            try
            {
                oNeft = new CNEFTTransfer();
                dt = oNeft.GetNEFTTransferPGAPI(ddlBank.SelectedValue, gblFuction.setDate(txtDt.Text), ddlBranch.SelectedValues.Replace("|", ","), "N"); //N means NEFT           
                gvDtl.DataSource = dt;
                gvDtl.DataBind();

            }
            finally
            {
                dt = null;
                oNeft = null;
            }
        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }


        protected void ProgressBar1_RunTask(object sender, EO.Web.ProgressTaskEventArgs e)
        {
            Boolean vResult = true;
            string vUTRNumber = "", responsedata = "", vXML = "";
            CNEFTTransfer oNEFT = new CNEFTTransfer();
            CNEFTTransfer oNeft = new CNEFTTransfer();
            DataTable dt = null;
            int vErr = 0, vCnt = 0;



            if (txtUser.Text.Trim().ToUpper() == "")
            {
                e.UpdateProgress(0, "User ID Can Not be Blank...");
            }
            else if (txtUser.Text.Trim().ToUpper() != Session[gblValue.ICICIUser].ToString().Trim().ToUpper())
            {
                e.UpdateProgress(0, "Invalid User ID...");
            }
            else
            {


                #region CREATE DATA TABLE
                DataTable dtdata = new DataTable("Table1");
                dtdata.Columns.Add("LoanId");
                dtdata.Columns.Add("Response");
                dtdata.Columns.Add("Message");
                dtdata.Columns.Add("UTRNUMBER");
                #endregion


                oNeft = new CNEFTTransfer();
                dt = oNeft.GetNEFTTransferPGAPI(ddlBank.SelectedValue, gblFuction.setDate(txtDt.Text), ddlBranch.SelectedValues.Replace("|", ","), "T"); // T for Status Check 
                if (dt.Rows.Count > 0)
                {
                    int vJ = dt.Rows.Count;
                    ProgressBar1.Minimum = 1;
                    ProgressBar1.Maximum = vJ;
                    ProgressBar1.IndicatorIncrement = ProgressBar1.Value;

                    for (int vI = 0; vI < vJ; vI++)//dt.Rows.Count
                    {

                        if (e.IsStopped)
                            break;

                        var vOBJ = new PostBankTransactionStatus()
                        {
                            AGGRID = "OTOE0027",
                            //CORPID = "CENTRUMM29112017",
                            CORPID = "581109799",
                            USERID = txtUser.Text.Trim().ToUpper(),
                            URN = "SR191962059",
                            UNIQUEID = dt.Rows[vI]["LoanId"].ToString(), ///"1333333311111"
                        };
                        string json = "{\"vPostBankTransactionStatus\":" + JsonConvert.SerializeObject(vOBJ) + "}";
                        try
                        {
                            //////var vURLString = "https://centrummob.bijliftt.com/CentrumService.svc/ICICBankTransactionStatus";

                            var vURLString = "http://localhost:3008/CentrumService.svc/ICICBankTransactionStatus";
                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(vURLString);
                            httpWebRequest.Method = "POST";
                            httpWebRequest.Host = "centrummob.bijliftt.com";
                            httpWebRequest.ContentType = "application/json";
                            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                            byte[] data = Encoding.UTF8.GetBytes(json);
                            httpWebRequest.ContentLength = data.Length;
                            Stream requestStream = httpWebRequest.GetRequestStream();
                            requestStream.Write(data, 0, data.Length);
                            requestStream.Close();
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                            {
                                var API_Response = streamReader.ReadToEnd(); ;
                                responsedata = API_Response.ToString().Trim();
                            }

                            responsedata = responsedata.Replace("{\"ICICBankTransactionStatusResult\":", "").Replace("}}", "}");

                            ICICBankTransactionStatusResponse vResponseObj = new ICICBankTransactionStatusResponse();

                            vResponseObj = JsonConvert.DeserializeObject<ICICBankTransactionStatusResponse>(responsedata);


                            #region INSERT DATA INTO DATATABLE
                            DataRow dr = dtdata.NewRow();

                            dr["LoanId"] = (vResponseObj.Response.ToUpper() == "SUCCESS" ? vResponseObj.UNIQUEID.ToUpper() : "");
                            dr["Response"] = vResponseObj.Response.ToUpper();
                            if (vResponseObj.Response.ToUpper() != "SUCCESS")
                            {
                                dr["Message"] = vResponseObj.message.ToUpper();
                            }
                            else
                            {
                                dr["Message"] = vResponseObj.STATUS.ToUpper();
                            }
                            if (vResponseObj.Response.ToUpper() == "SUCCESS")
                            {
                                dr["UTRNUMBER"] = vResponseObj.UTRNUMBER.ToUpper();
                                vUTRNumber = vResponseObj.UTRNUMBER.ToUpper();
                            }
                            else
                            {
                                dr["UTRNUMBER"] = "";
                                vUTRNumber = "UTR Not Generated";
                            }
                            dtdata.Rows.Add(dr);
                            if (dtdata.Rows.Count > 0)
                            {
                                using (StringWriter oSW = new StringWriter())
                                {
                                    dtdata.WriteXml(oSW);
                                    vXML = oSW.ToString();
                                }
                                vErr = oNEFT.InsertNEFTTransferAPI(vXML, Convert.ToInt32(Session[gblValue.UserId]));

                                if (vErr == 0)
                                {
                                    vResult = true;
                                }
                                else
                                {
                                    vResult = false;
                                }
                            }
                            dtdata.Clear();
                            #endregion

                            vCnt = vCnt + 1;

                            e.UpdateProgress((vCnt * 100) / vJ, "Complete Status " + vCnt.ToString() + " / " + vJ.ToString() + " UTR Number " + vUTRNumber);
                        }
                        catch (Exception ex)
                        {
                            e.UpdateProgress(0, ex.Message.ToString());
                        }
                        finally
                        {

                        }
                    }
                }
                else
                {
                    e.UpdateProgress(0, "No Data Found For Status Checking");
                    vResult = false;
                }

                e.UpdateProgress(0, "Task Completed");
                vResult = true;
            }

        }

        protected void ProgressBar2_RunTask(object sender, EO.Web.ProgressTaskEventArgs e)
        {
            //SaveInitiate(e);
            SaveRecords("Save");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBalance_Click(object sender, EventArgs e)
        {
            if (txtUser.Text.Trim().ToUpper() != "")
            {
                if (txtUser.Text.Trim().ToUpper() != Session[gblValue.ICICIUser].ToString().Trim().ToUpper())
                {
                    gblFuction.AjxMsgPopup("Invalid User ID...");
                    return;
                }
                if (Session[gblValue.ICICINEFTYN].ToString() == "Y")
                {
                    string responsedata = "";
                    string vAccountNumber = ddlBank.SelectedItem.Text.Trim().Substring(ddlBank.SelectedItem.Text.IndexOf("#") + 1);
                    var vObj = new PostBalEnqReq()
                    {
                        AGGRID = "OTOE0027",
                        //CORPID = "CENTRUMM29112017",
                        CORPID = "581109799",
                        USERID = txtUser.Text.Trim().ToUpper(),
                        URN = "SR191962059",
                        ACCOUNTNO = vAccountNumber
                    };
                    string json = "{\"vPostBalEnqReq\":" + JsonConvert.SerializeObject(vObj) + "}";
                    try
                    {
                        //var vURLString = "https://centrummob.bijliftt.com/CentrumService.svc/ICICBalanceFetch";
                        //Centrum MOB and Centrum Web hosted in same server
                        var vURLString = "http://localhost:3008/CentrumService.svc/ICICBalanceFetch";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(vURLString);
                        httpWebRequest.Method = "POST";
                        httpWebRequest.Host = "centrummob.bijliftt.com";
                        httpWebRequest.ContentType = "application/json";
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        byte[] data = Encoding.UTF8.GetBytes(json);
                        httpWebRequest.ContentLength = data.Length;
                        Stream requestStream = httpWebRequest.GetRequestStream();
                        requestStream.Write(data, 0, data.Length);
                        requestStream.Close();
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                        {
                            var API_Response = streamReader.ReadToEnd(); ;
                            responsedata = API_Response.ToString().Trim();
                        }

                        responsedata = responsedata.Replace("{\"ICICBalanceFetchResult\":", "").Replace("}}", "}");

                        ICICBalanceFetchResponse vResponseObj = new ICICBalanceFetchResponse();

                        vResponseObj = JsonConvert.DeserializeObject<ICICBalanceFetchResponse>(responsedata);
                        if ((vResponseObj.EFFECTIVEBAL) == null)
                        {
                            gblFuction.AjxMsgPopup("Bank API returns:- " + vResponseObj.MESSAGE);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Account Effective Balance is : " + vResponseObj.EFFECTIVEBAL);
                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {

                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("You do not have Access ");
                    return;
                }
            }
            else
            {
                gblFuction.AjxMsgPopup("User name can not be Blank");
                return;
            }
        }

        private void SaveInitiate(EO.Web.ProgressTaskEventArgs e)
        {
            try
            {
                if (txtUser.Text.Trim().ToUpper() != "")
                {
                    if (txtUser.Text.Trim().ToUpper() != Session[gblValue.ICICIUser].ToString().Trim().ToUpper())
                    {
                        gblFuction.AjxMsgPopup("Invalid User ID...");
                        return;
                    }
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtDt.Text))
                    {
                        gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        return;
                    }
                    if (Session[gblValue.ICICINEFTYN].ToString() == "Y")
                    {
                        //Session[gblValue.ICICIUser] = dt.Rows[0]["ICICIUser"].ToString();
                        string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                        if (vStateEdit == "" || vStateEdit == null)
                            vStateEdit = "Save";
                        if (SaveRecords(vStateEdit, e) == true)
                        {
                            gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                            LoadGrid(0);
                            StatusButton("View");
                            ddlBank.Enabled = false;
                            btnSave.Enabled = false;
                        }
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("You do not have access...");
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("User ID Can not be Blank...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Boolean SaveRecords(string Mode, EO.Web.ProgressTaskEventArgs e)
        {
            Boolean vResult = true;
            string vNEFTAcc = "", vBeneficiaryAcc = "", vBeneficiaryIFSC = "", vNEFTAmount = "", vLoanid = "", vPayeeName = "", responsedata = "", vXML = "";
            CNEFTTransfer oNEFT = new CNEFTTransfer();
            int vErr = 0;
            int vCnt = 0;

            #region CREATE DATA TABLE
            DataTable dtdata = new DataTable("Table1");
            dtdata.Columns.Add("LoanId");
            dtdata.Columns.Add("Response");
            dtdata.Columns.Add("Message");
            dtdata.Columns.Add("UTRNUMBER");
            #endregion
            try
            {
                int vJ = gvDtl.Rows.Count;
                ProgressBar2.Minimum = 1;
                ProgressBar2.Maximum = vJ;
                ProgressBar2.IndicatorIncrement = ProgressBar2.Value;

                foreach (GridViewRow gr in gvDtl.Rows)
                {
                    string vUTRNumber = string.Empty;
                    if (e.IsStopped)
                        break;

                    responsedata = "";
                    CheckBox chkDisb = (CheckBox)gr.FindControl("chkDisb");
                    #region IF Loan selected for NEFT
                    if (chkDisb.Checked == true)
                    {
                        vNEFTAcc = gr.Cells[23].Text;
                        vBeneficiaryAcc = gr.Cells[24].Text;
                        vBeneficiaryIFSC = gr.Cells[25].Text;
                        vNEFTAmount = gr.Cells[26].Text;
                        vLoanid = gr.Cells[27].Text;
                        vPayeeName = gr.Cells[28].Text;
                        var vOBJ = new PostBankTransaction()
                        {
                            AGGRID = "OTOE0027",
                            AGGRNAME = "CENTRUM",
                            // CORPID = "CENTRUMM29112017",
                            CORPID = "581109799",
                            USERID = txtUser.Text.Trim().ToUpper(),
                            URN = "SR191962059",
                            DEBITACC = vNEFTAcc,//"000451000301"
                            CREDITACC = vBeneficiaryAcc, //"000405002777" ;
                            IFSC = vBeneficiaryIFSC, //"ICIC0000011", //
                            AMOUNT = vNEFTAmount, //"1",// ;
                            CURRENCY = "INR",
                            TXNTYPE = "RGS",
                            PAYEENAME = vPayeeName,
                            UNIQUEID = vLoanid, ///"1333333311111"
                            REMARKS = "Disbursement from Unity Small Finance Bank Ltd."
                        };
                        string json = "{\"vPostBankTransaction\":" + JsonConvert.SerializeObject(vOBJ) + "}";
                        try
                        {
                            //////var vURLString = "https://centrummob.bijliftt.com/CentrumService.svc/ICICBankTransaction";
                            //Centrum MOB and Centrum Web hosted in same server
                            var vURLString = "http://localhost:3008/CentrumService.svc/ICICBankTransaction";
                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(vURLString);
                            httpWebRequest.Method = "POST";
                            httpWebRequest.Host = "centrummob.bijliftt.com";
                            httpWebRequest.ContentType = "application/json";
                            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                            byte[] data = Encoding.UTF8.GetBytes(json);
                            httpWebRequest.ContentLength = data.Length;
                            Stream requestStream = httpWebRequest.GetRequestStream();
                            requestStream.Write(data, 0, data.Length);
                            requestStream.Close();
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                            {
                                var API_Response = streamReader.ReadToEnd(); ;
                                responsedata = API_Response.ToString().Trim();
                            }

                            responsedata = responsedata.Replace("{\"ICICBankTransactionResult\":", "").Replace("}}", "}");
                            ICICBankTransactionResponse vResponseObj = new ICICBankTransactionResponse();
                            vResponseObj = JsonConvert.DeserializeObject<ICICBankTransactionResponse>(responsedata);

                            if (vResponseObj != null)
                            {
                                #region INSERT DATA INTO DATATABLE
                                DataRow dr = dtdata.NewRow();
                                dr["LoanId"] = (vResponseObj.Response.ToUpper() == "SUCCESS" ? vResponseObj.UNIQUEID.ToUpper() : vLoanid);
                                //dr["LoanId"] = vLoanid;
                                dr["Response"] = vResponseObj.Response.ToUpper();
                                if (vResponseObj.Response.ToUpper() != "SUCCESS")
                                {
                                    dr["Message"] = vResponseObj.message.ToUpper();
                                }
                                else
                                {
                                    dr["Message"] = vResponseObj.STATUS.ToUpper();
                                }
                                if (vResponseObj.Response.ToUpper() == "SUCCESS")
                                {
                                    dr["UTRNUMBER"] = vResponseObj.UTRNUMBER.ToUpper();
                                    vUTRNumber = vResponseObj.UTRNUMBER.ToUpper();
                                }
                                else
                                {
                                    dr["UTRNUMBER"] = "";
                                }
                                dtdata.Rows.Add(dr);
                                dtdata.AcceptChanges();
                                if (dtdata.Rows.Count > 0)
                                {
                                    using (StringWriter oSW = new StringWriter())
                                    {
                                        dtdata.WriteXml(oSW);
                                        vXML = oSW.ToString();
                                    }
                                    vErr = oNEFT.InsertNEFTTransferAPI(vXML, Convert.ToInt32(Session[gblValue.UserId]));

                                    //if (vErr == 0)
                                    //{
                                    //    gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                                    //    vResult = true;
                                    //}
                                    //else
                                    //{
                                    //    gblFuction.AjxMsgPopup(gblMarg.DBError);
                                    //    vResult = false;
                                    //}
                                }
                                dtdata.Clear();
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            //throw ex;
                            break;
                        }
                        finally
                        {
                        }
                        vCnt = vCnt + 1;

                    }
                    if (vCnt == 0)
                    {
                    }
                    #endregion
                    e.UpdateProgress((vCnt * 100) / vJ, "Complete Status " + vCnt.ToString() + " / " + vJ.ToString() + " UTR Number " + vUTRNumber);
                }
                if (vCnt == 0)
                {
                    gblFuction.AjxMsgPopup("No Loan Selected for NEFT");
                    vResult = false;
                }
                // ScriptManager.RegisterStartupScript(this, typeof(Page), "ClearGrid", "ClearGrid();", true);
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = true;
            string vNEFTAcc = "", vBeneficiaryAcc = "", vBeneficiaryIFSC = "", vNEFTAmount = "", vLoanid = "", vPayeeName = "", responsedata = "", vXML = "", vXmlData = "";
            CNEFTTransfer oNEFT = new CNEFTTransfer();
            int vErr = 0;
            int vCnt = 0;

            #region CREATE DATA TABLE
            DataTable dtdata = new DataTable("Table1");
            dtdata.Columns.Add("LoanId");
            dtdata.Columns.Add("Response");
            dtdata.Columns.Add("Message");
            dtdata.Columns.Add("UTRNUMBER");

            DataTable dtNeft = new DataTable("Table1");
            dtNeft.Columns.Add("AGGRID");
            dtNeft.Columns.Add("AGGRNAME");
            dtNeft.Columns.Add("CORPID");
            dtNeft.Columns.Add("USERID");
            dtNeft.Columns.Add("URN");
            dtNeft.Columns.Add("DEBITACC");
            dtNeft.Columns.Add("CREDITACC");
            dtNeft.Columns.Add("IFSC");
            dtNeft.Columns.Add("AMOUNT");
            dtNeft.Columns.Add("CURRENCY");
            dtNeft.Columns.Add("TXNTYPE");
            dtNeft.Columns.Add("PAYEENAME");
            dtNeft.Columns.Add("UNIQUEID");
            dtNeft.Columns.Add("REMARKS");
            #endregion

            try
            {
                foreach (GridViewRow gr in gvDtl.Rows)
                {
                    responsedata = "";
                    CheckBox chkDisb = (CheckBox)gr.FindControl("chkDisb");
                    #region IF Loan selected for NEFT
                    if (chkDisb.Checked == true)
                    {
                        vNEFTAcc = gr.Cells[23].Text;
                        vBeneficiaryAcc = gr.Cells[24].Text;
                        vBeneficiaryIFSC = gr.Cells[25].Text;
                        vNEFTAmount = gr.Cells[26].Text;
                        vLoanid = gr.Cells[27].Text;
                        vPayeeName = gr.Cells[28].Text;

                        DataRow drNeft = dtNeft.NewRow();
                        drNeft["AGGRID"] = "OTOE0027";
                        drNeft["AGGRNAME"] = "CENTRUM";
                        drNeft["CORPID"] = "581109799";
                        drNeft["USERID"] = txtUser.Text.Trim().ToUpper();
                        drNeft["URN"] = "SR191962059";
                        drNeft["DEBITACC"] = vNEFTAcc;
                        drNeft["CREDITACC"] = vBeneficiaryAcc;
                        drNeft["IFSC"] = vBeneficiaryIFSC;
                        drNeft["AMOUNT"] = vNEFTAmount;
                        drNeft["CURRENCY"] = "INR";
                        drNeft["TXNTYPE"] = "RGS";
                        drNeft["PAYEENAME"] = vPayeeName;
                        drNeft["UNIQUEID"] = vLoanid;
                        drNeft["REMARKS"] = "Disbursement from Unity Small Finance Bank Ltd.";
                        dtNeft.Rows.Add(drNeft);
                        dtNeft.AcceptChanges();

                        //var vOBJ = new PostBankTransaction()
                        //{
                        //    AGGRID = "OTOE0027",
                        //    AGGRNAME = "CENTRUM",
                        //    // CORPID = "CENTRUMM29112017",
                        //    CORPID = "581109799",
                        //    USERID = txtUser.Text.Trim().ToUpper(),
                        //    URN = "SR191962059",
                        //    DEBITACC = vNEFTAcc,//"000451000301"
                        //    CREDITACC = vBeneficiaryAcc, //"000405002777" ;
                        //    IFSC = vBeneficiaryIFSC, //"ICIC0000011", //
                        //    AMOUNT = vNEFTAmount, //"1",// ;
                        //    CURRENCY = "INR",
                        //    TXNTYPE = "RGS",
                        //    PAYEENAME = vPayeeName,
                        //    UNIQUEID = vLoanid, ///"1333333311111"
                        //    REMARKS = "Disbursement from Unity Small Finance Bank Ltd."
                        //};
                        //string json = "{\"vPostBankTransaction\":" + JsonConvert.SerializeObject(vOBJ) + "}";
                        //try
                        //{
                        //    //////var vURLString = "https://centrummob.bijliftt.com/CentrumService.svc/ICICBankTransaction";
                        //    //Centrum MOB and Centrum Web hosted in same server
                        //    var vURLString = "http://localhost:3008/CentrumService.svc/ICICBankTransaction";
                        //    var httpWebRequest = (HttpWebRequest)WebRequest.Create(vURLString);
                        //    httpWebRequest.Method = "POST";
                        //    httpWebRequest.Host = "centrummob.bijliftt.com";
                        //    httpWebRequest.ContentType = "application/json";
                        //    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        //    byte[] data = Encoding.UTF8.GetBytes(json);
                        //    httpWebRequest.ContentLength = data.Length;
                        //    Stream requestStream = httpWebRequest.GetRequestStream();
                        //    requestStream.Write(data, 0, data.Length);
                        //    requestStream.Close();
                        //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                        //    {
                        //        var API_Response = streamReader.ReadToEnd(); ;
                        //        responsedata = API_Response.ToString().Trim();
                        //    }

                        //    responsedata = responsedata.Replace("{\"ICICBankTransactionResult\":", "").Replace("}}", "}");

                        //    ICICBankTransactionResponse vResponseObj = new ICICBankTransactionResponse();

                        //    vResponseObj = JsonConvert.DeserializeObject<ICICBankTransactionResponse>(responsedata);

                        //    if (vResponseObj != null)
                        //    {
                        //        #region INSERT DATA INTO DATATABLE
                        //        DataRow dr = dtdata.NewRow();
                        //        dr["LoanId"] = (vResponseObj.Response.ToUpper() == "SUCCESS" ? vResponseObj.UNIQUEID.ToUpper() : vLoanid);
                        //        //dr["LoanId"] = vLoanid;
                        //        dr["Response"] = vResponseObj.Response.ToUpper();
                        //        if (vResponseObj.Response.ToUpper() != "SUCCESS")
                        //        {
                        //            dr["Message"] = vResponseObj.message.ToUpper();
                        //        }
                        //        else
                        //        {
                        //            dr["Message"] = vResponseObj.STATUS.ToUpper();
                        //        }
                        //        if (vResponseObj.Response.ToUpper() == "SUCCESS")
                        //        {
                        //            dr["UTRNUMBER"] = vResponseObj.UTRNUMBER.ToUpper();
                        //        }
                        //        else
                        //        {
                        //            dr["UTRNUMBER"] = "";
                        //        }
                        //        dtdata.Rows.Add(dr);
                        //        dtdata.AcceptChanges();
                        //        if (dtdata.Rows.Count > 0)
                        //        {
                        //            using (StringWriter oSW = new StringWriter())
                        //            {
                        //                dtdata.WriteXml(oSW);
                        //                vXML = oSW.ToString();
                        //            }
                        //            vErr = oNEFT.InsertNEFTTransferAPI(vXML, Convert.ToInt32(Session[gblValue.UserId]));

                        //            //if (vErr == 0)
                        //            //{
                        //            //    gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                        //            //    vResult = true;
                        //            //}
                        //            //else
                        //            //{
                        //            //    gblFuction.AjxMsgPopup(gblMarg.DBError);
                        //            //    vResult = false;
                        //            //}
                        //        }
                        //        dtdata.Clear();
                        //        #endregion
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    //throw ex;
                        //    break;
                        //}
                        //finally
                        //{
                        //}
                        vCnt = vCnt + 1;
                    }
                    #endregion
                }
                if (vCnt == 0)
                {
                    gblFuction.AjxMsgPopup("No Loan Selected for NEFT");
                    vResult = false;
                }

                using (StringWriter oSW = new StringWriter())
                {
                    dtNeft.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }

                var req = new ICICIDisbursement()
                {
                    pXml = vXmlData,
                    pUserId = Convert.ToString(Session[gblValue.UserId])
                };
                string Requestdata = JsonConvert.SerializeObject(req);
                GenerateReport("ICICIDisbursement", Requestdata, "http://localhost:3008/CentrumService.svc");
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "CallPage", "pageCalling();", true);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            return vResult;
        }

        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
                    break;
                case "Show":
                    //btnEdit.Enabled = true;
                    //btnDelete.Enabled = true;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
                    break;
                case "View":
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        private void ClearControls()
        {
            //txtTotNEFT.Text = "0";
            //LoadGrid(0);
        }

        private void EnableControl(bool Status)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                tabReSchdl.ActiveTabIndex = 0;
                EnableControl(false);
                StatusButton("View");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveInitiate()
        {
            try
            {
                if (txtUser.Text.Trim().ToUpper() != "")
                {
                    if (txtUser.Text.Trim().ToUpper() != Session[gblValue.ICICIUser].ToString().Trim().ToUpper())
                    {
                        gblFuction.AjxMsgPopup("Invalid User ID...");
                        return;
                    }
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtDt.Text))
                    {
                        gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        return;
                    }
                    if (Session[gblValue.ICICINEFTYN].ToString() == "Y")
                    {
                        Int32 vNeftAPI = Convert.ToInt32(Session[gblValue.NeftAPI].ToString());
                        if (vNeftAPI != 0)
                        {
                            Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                            if (unixTicks - vNeftAPI > 600)
                            {
                                Session[gblValue.NeftAPI] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();                              
                                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                                if (vStateEdit == "" || vStateEdit == null)
                                    vStateEdit = "Save";
                                if (SaveRecords(vStateEdit) == true)
                                {
                                    //gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                                    gblFuction.AjxMsgPopup("Disbusrement process is running on background.Please wait..");
                                    LoadGrid(0);
                                    StatusButton("View");
                                    ddlBank.Enabled = false;
                                    //btnSave.Enabled = false; 
                                }
                            }
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("NEFT Request Is Executing ...Please Wait For 10 Mins..And Re Execute..");
                        }
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("You do not have access...");
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("User ID Can not be Blank...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveInitiate();
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void gvDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CVoucher oVoucher = null;
            DataTable dtBrBank = null;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlBrBank = (DropDownList)e.Row.FindControl("ddlBrBank");
                try
                {
                    oVoucher = new CVoucher();
                    ddlBrBank.Items.Clear();
                    dtBrBank = oVoucher.GetAcGenLedCB(e.Row.Cells[20].Text.Trim(), "D", "");
                    if (dtBrBank.Rows.Count > 0)
                    {
                        ddlBrBank.DataSource = dtBrBank;
                        ddlBrBank.DataTextField = "Desc";
                        ddlBrBank.DataValueField = "DescId";
                        ddlBrBank.DataBind();
                    }
                    ListItem oLc = new ListItem("<--Select-->", "-1");
                    ddlBrBank.Items.Insert(0, oLc);
                    ddlBrBank.SelectedIndex = ddlBrBank.Items.IndexOf(ddlBrBank.Items.FindByValue(e.Row.Cells[22].Text.Trim()));
                    ddlBrBank.Enabled = false;
                }
                finally
                {
                    oVoucher = null;
                    dtBrBank = null;
                }
            }
        }

        private String XmlFromGrid()
        {
            Int32 i = 0;
            String vXML = "";
            DataTable dt = new DataTable("Table1");
            dt.Columns.Add("SlNo");
            dt.Columns.Add("LoanAppId");
            dt.Columns.Add("Loantype");
            dt.Columns.Add("LoanAmt");
            dt.Columns.Add("BranchName");
            dt.Columns.Add("EoName");
            dt.Columns.Add("DisbYN");
            dt.Columns.Add("ICID");
            dt.Columns.Add("BankDescId");
            dt.Columns.Add("GroupId");
            dt.Columns.Add("GroupName");
            foreach (GridViewRow gr in gvDtl.Rows)
            {
                CheckBox chkDisb = (CheckBox)gr.FindControl("chkDisb");
                if (chkDisb.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["SlNo"] = Convert.ToString(i);
                    dr["LoanAppId"] = gr.Cells[15].Text;
                    Label lblLnShceme = (Label)gr.FindControl("lblLnShceme");
                    dr["Loantype"] = lblLnShceme.Text;
                    Label lblBranch = (Label)gr.FindControl("lblBranch");
                    dr["BranchName"] = lblBranch.Text;
                    Label lblRO = (Label)gr.FindControl("lblRO");
                    dr["EoName"] = lblRO.Text;
                    Label lblLnAmt = (Label)gr.FindControl("lblLnAmt");
                    dr["LoanAmt"] = lblLnAmt.Text;
                    if (chkDisb.Checked == true)
                        dr["DisbYN"] = 'Y';
                    else
                        dr["DisbYN"] = 'N';
                    dr["ICID"] = gr.Cells[18].Text;
                    DropDownList ddlBrBank = (DropDownList)gr.FindControl("ddlBrBank");
                    dr["BankDescId"] = ddlBrBank.SelectedValue;
                    dr["GroupId"] = gr.Cells[21].Text;
                    Label lblGroup = (Label)gr.FindControl("lblGroup");
                    dr["GroupName"] = lblGroup.Text;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                    i++;
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


        private double TotalDisbAmt()
        {
            double TotAmt = 0;
            foreach (GridViewRow gr in gvDtl.Rows)
            {
                CheckBox chkDisb = (CheckBox)gr.FindControl("chkDisb");
                CheckBox chkAppFrCashDisb = (CheckBox)gr.FindControl("chkAppFrCashDisb");
                CheckBox ChkCancel = (CheckBox)gr.FindControl("ChkCancel");
                Label lblLnAmt = (Label)gr.FindControl("lblLnAmt");
                if (chkDisb.Checked == true)
                {
                    TotAmt += Convert.ToDouble(lblLnAmt.Text.Trim());

                }

            }
            return TotAmt;
        }

        protected void chkDisb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            CheckBox chkDisb = (CheckBox)row.FindControl("chkDisb");
            //CheckBox chkAppFrCashDisb = (CheckBox)row.FindControl("chkAppFrCashDisb");
            //CheckBox ChkCancel = (CheckBox)row.FindControl("ChkCancel");
            double vTotalAmt = 0;
            Int32 vTotCount = 0;
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox chkDisbT = (CheckBox)gR.FindControl("chkDisb");
                Label lblLnAmt = (Label)gR.FindControl("lblLnAmt");
                if (chkDisbT.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(lblLnAmt.Text.Trim());
                    vTotCount = vTotCount + 1;
                }
            }
            txtTotAmt.Text = Convert.ToString(vTotalAmt);
            txtTotCount.Text = Convert.ToString(vTotCount);
            UpTot.Update();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (txtUser.Text.Trim().ToUpper() != "")
            {
                if (txtUser.Text.Trim().ToUpper() != Session[gblValue.ICICIUser].ToString().Trim().ToUpper())
                {
                    gblFuction.AjxMsgPopup("Invalid User ID...");
                    return;
                }
                if (txtDt.Text.Trim() != "")
                {
                    if (gblFuction.IsDate(txtDt.Text) == false)
                    {
                        gblFuction.MsgPopup(gblMarg.ValidDate);
                        gblFuction.focus("ctl00_cph_Main_txtDt");
                    }
                    else if (ddlBank.SelectedValue == "-1")
                    {
                        gblFuction.MsgPopup("please Select a Bank...");
                    }
                    else
                    {
                        LoadGrid(0);
                        SetTotal();
                        txtDt.Enabled = false;
                    }
                }
            }
            else
            {
                gblFuction.AjxMsgPopup("User name can not be Blank");
                return;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void SetTotal()
        {
            double vTotalAmt = 0;
            Int32 vTotCount = 0;
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox chkDisbT = (CheckBox)gR.FindControl("chkDisb");
                CheckBox chkAppFrCashDisbT = (CheckBox)gR.FindControl("chkAppFrCashDisb");
                CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
                CheckBox ChkPostponeT = (CheckBox)gR.FindControl("ChkPostpone");
                if (chkDisbT.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vTotCount = vTotCount + 1;
                }

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
                gblFuction.AjxMsgPopup("Disbusrement process is running on background.");
            }
        }
    }


    public class ICICIDisbursement
    {
        public string pXml { get; set; }
        public string pUserId { get; set; }
        public string pTransDate { get; set; }
        public string pBankDescId { get; set; }
    }
}