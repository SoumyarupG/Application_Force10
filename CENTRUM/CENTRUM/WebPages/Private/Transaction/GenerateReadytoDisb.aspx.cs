using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class GenerateReadytoDisb : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtAppDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                ViewState["RtoD"] = null;
                ViewState["InOutFlow"] = null;
                //GenerateBankCls();
                txtOpenBank.Text = "0";
                txtOpenBankNote.Text = "The Above Bank Balance is as on Login Date.. It may vary time to time";
                PopBranch();
                CheckAll();
                popDetail();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Generate Ready to Disburse";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuGenRdyToDisb);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnSave.Visible = false;
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Generate Ready To Disburse", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void GenerateBankCls()
        {
            Double vBankCls = 0.0;
            CReadytoDisb obj= null;
            obj = new CReadytoDisb();
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vDateAsOn = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vBranch = Session[gblValue.BrnchCode].ToString();
            Int32 vYearNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());

            vBankCls = obj.GetClosingbank(vFinFrom, vDateAsOn, vBranch,vYearNo);
            txtOpenBank.Text = Convert.ToString(vBankCls);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveRecords("Save") == true)
            {
                gblFuction.AjxMsgPopup("List Generated Properly...");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            ViewState["RtoD"] = null;
            CReadytoDisb obj = null;
            string vBranch = ViewState["Dtl"].ToString();
            obj = new CReadytoDisb();
            DataSet ds = null;
            DataTable dtVal = null;
            DataTable dtList = null;
            Int32 vI = 0;
            Double vAmtAvl = 0.0;
           
            try
            {
                if (txtAppDt.Text == "" || txtOpenBank.Text == "")
                {
                    gblFuction.MsgPopup("Date and Amount can not be blank..");
                    return false;
                }
                ds = obj.CalCulateFundAsonDate(gblFuction.setDate(txtAppDt.Text), vBranch);//
                dtVal = ds.Tables[0];
                
               
                if (ds.Tables[1].Rows.Count > 0)
                {
                    dtList = ds.Tables[1];
                    foreach (DataRow dr in dtVal.Rows)          // Find Demand Branchwise
                    {
                        vBranch = dr["Branch"].ToString();
                        vAmtAvl = Convert.ToDouble(dr["DemandAmt"].ToString());
                        vI = 0;
                        foreach (DataRow drList in dtList.Rows)     // Find Ready Disb Branchwise
                        {
                            if (drList["Branch"].ToString().Trim().ToUpper().Replace(" ", "") == vBranch.Trim().ToUpper().Replace(" ", ""))
                            {
                                if (vI <= 20)
                                {
                                    if (Convert.ToDouble(drList["Loan_Application_Amount"].ToString()) <= vAmtAvl)
                                    {
                                        drList["Ready_to_Disburse_Status"] = "Y";
                                        vAmtAvl = vAmtAvl - Convert.ToDouble(drList["Loan_Application_Amount"].ToString());
                                        vI = vI + 1;
                                    }
                                }
                            }
                        }
                    }

                    ViewState["InOutFlow"] = dtVal;
                    //if (Convert.IsDBNull(dtVal.Rows[0]["Totval"]) == false)
                    //    vAmtAvl = Convert.ToDouble(dtVal.Rows[0]["Totval"]); //Inflow -Outflow  + Demand
                    //else
                    //    vAmtAvl = 0.0;
                    //if (Convert.IsDBNull(dtVal.Rows[0]["TotInFlow"]) == false)
                    //    vTotInFlow = Convert.ToDouble(dtVal.Rows[0]["TotInFlow"]); //Inflow -Outflow  + Demand
                    //else
                    //    vTotInFlow = 0.0;

                    //if (Convert.IsDBNull(dtVal.Rows[0]["TotDemand"]) == false)
                    //    vDemand = Convert.ToDouble(dtVal.Rows[0]["TotDemand"]); //Inflow -Outflow  + Demand
                    //else
                    //    vDemand = 0.0;


                    vAmtAvl = Convert.ToDouble(txtOpenBank.Text);  // Inflow - Outflow  + Demand 

                    //// Set Ready_to_Disburse_Status = 'Y' depending on Fund

                    foreach (DataRow dr in dtList.Rows)
                    {
                        if (dr["Ready_to_Disburse_Status"].ToString().ToUpper() == "N")
                        {
                            if (Convert.ToDouble(dr["Loan_Application_Amount"].ToString()) <= vAmtAvl)
                            {
                                dr["Ready_to_Disburse_Status"] = "Y";
                                vAmtAvl = vAmtAvl - Convert.ToDouble(dr["Loan_Application_Amount"].ToString());
                            }
                        }
                    }


                    //vToDisbNo = Convert.ToInt32(dtList.Rows[dtList.Rows.Count - 1]["Sl_No"].ToString());
                    dtList.AcceptChanges();

                    //dtList.DefaultView.Sort = "Branch" + " " + "ASC";
                    dtList.DefaultView.Sort = "loantypeId" + " " + "ASC";
                    dtList = dtList.DefaultView.ToTable();
                    gvSrc.DataSource = dtList;
                    gvSrc.DataBind();
                    ViewState["RtoD"] = dtList;
                    return true;
                }
                else
                {
                    gvSrc.DataSource = null;
                    gvSrc.DataBind();
                    return false;
                }
                    
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtList = null;
                dtVal = null;
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string vFileNm = "";
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            try
            {
                dt = (DataTable)ViewState["RtoD"];
                if (dt == null)
                {
                    gblFuction.AjxMsgPopup("Please Click on Generate then try to Print");
                    return;
                }
                dt.Columns.Remove("LoanAppId");
                dt.Columns.Remove("MemberId");
                dt.Columns.Remove("BranchCode");
                //dt.Columns.Remove("LoanAppId");
                //dt.Columns.Remove("LoanAppId");
                //dt.Columns.Remove("LoanAppId");
                //dt.Columns.Remove("LoanAppId");

                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();


                tdx.Controls.Add(DataGrid1);
                tdx.Visible = false;
                vFileNm = "attachment;filename=Ready to Disburse List";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                htw.WriteLine("<table border='1' cellpadding='7' widht='100%'>");
                htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='3'>" + gblValue.CompName + "</font></u></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='2'>" + CGblIdGenerator.GetBranchAddress1("0000") + "</font></u></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='2'>Ready to Disburse List As on " + txtAppDt.Text + "</font></u></b></td></tr>");
                DataGrid1.RenderControl(htw);
                htw.WriteLine("</td></tr>");
                htw.WriteLine("<tr><td colspan='7'><b><u><font size='7'></font></u></b></td></tr>");
                htw.WriteLine("</table>");

                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel"; 
                Response.Write(sw.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtRtoD_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["RtoD"];
            TextBox txtRtoD = (TextBox)sender;
            
            GridViewRow gR = (GridViewRow)txtRtoD.NamingContainer;
            TextBox txtRemark = (TextBox)gR.FindControl("txtRemark");
            DateTime vExpDate = Convert.ToDateTime(gR.Cells[15].Text);
            string vRtoD = txtRtoD.Text;
            if (txtRtoD.Text != "Y" && txtRtoD.Text != "N" && txtRtoD.Text != "y" && txtRtoD.Text != "n")
            {
                gblFuction.AjxMsgPopup("Only Y OR N Can be entered");
                txtRtoD.Text = gR.Cells[17].Text;// dt.Rows[gR.RowIndex]["Ready_to_Disburse_Status"].ToString();
                
                return;
            }
            else
            {
                gR.Cells[17].Text = txtRtoD.Text;
                return;
            }

            //dt.Rows[gR.RowIndex]["Expected_Disburse_Date"] = vExpDate;
            //dt.Rows[gR.RowIndex]["Ready_to_Disburse_Status"] = txtRtoD.Text.Trim().ToUpper();
            //dt.Rows[gR.RowIndex]["Remark"] = txtRemark.Text.Trim().ToUpper();

            //dt.AcceptChanges();
            //ViewState["RtoD"] = dt;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtRemark_TextChanged(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)ViewState["RtoD"];
            //TextBox txtRemark = (TextBox)sender;
            //GridViewRow gR = (GridViewRow)txtRemark.NamingContainer;
            //TextBox txtRtoD = (TextBox)gR.FindControl("txtRtoD");
            //DateTime vExpDate = Convert.ToDateTime(gR.Cells[15].Text);
            //dt.Rows[gR.RowIndex]["Expected_Disburse_Date"] = vExpDate;
            //dt.Rows[gR.RowIndex]["Ready_to_Disburse_Status"] = txtRtoD.Text.Trim().ToUpper();
            //dt.Rows[gR.RowIndex]["Remark"] = txtRemark.Text.Trim().ToUpper();

            //dt.AcceptChanges();
            //ViewState["RtoD"] = dt;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ChangeExpDate()
        {
            // Set ExpDate = ExpDate + 7 for Ready_to_Disburse_Status is 'N'
            DataTable dtList = (DataTable)ViewState["RtoD"];
            if (dtList == null)
                return;
            DateTime vExpDate;
            foreach (GridViewRow gr in gvSrc.Rows)
            {
                TextBox txtRtoD = (TextBox)gr.FindControl("txtRtoD");
                dtList.Rows[gr.RowIndex]["Ready_to_Disburse_Status"] = txtRtoD.Text;
            }
            dtList.AcceptChanges();
            foreach (DataRow dr in dtList.Rows)
            {
                if (dr["Ready_to_Disburse_Status"].ToString().Trim().ToUpper() == "N")
                {
                    dr["Ready_to_Disburse_Status"] = "N";
                    vExpDate = Convert.ToDateTime(dr["Expected_Disburse_Date"]);
                    vExpDate = vExpDate.AddDays(7);
                    dr["Expected_Disburse_Date"] = vExpDate;
                }
            }
            foreach (DataColumn dc in dtList.Columns)
            {

                //dr["Expected_Disburse_Date"] = dr["Expected_Disburse_Date"].ToString().Substring(0,10);
                if (object.ReferenceEquals(dc.DataType, typeof(DateTime)))
                {
                    dc.DateTimeMode = DataSetDateTime.Unspecified;
                }
            }
            dtList.AcceptChanges();
            ViewState["RtoD"] = dtList;
            gvSrc.DataSource = dtList;
            gvSrc.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSvtoDB_Click(object sender,EventArgs e)
        {
            
            ChangeExpDate();
            DataTable dtVal = null;
            DataTable dtList = null;
            DataTable dt = null;
            string vXml = "";
            Double vProcfee = 0.0, vInsFee = 0.0, vTotDisb = 0.0, vTotInFlow = 0.0, vDemand = 0.0, vToDisbNo = 0.0;
            Int32 vErr;
            CReadytoDisb obj = null;
            obj = new CReadytoDisb();

            dtVal = (DataTable)ViewState["InOutFlow"]  ;
            dtList = (DataTable)ViewState["RtoD"]  ;

            if (dtVal == null || dtList == null)
            {
                gblFuction.AjxMsgPopup("Please Click on Generate then try to Save");
                return;
            }

            dt = obj.ChkRtoDForThatDate(gblFuction.setDate(txtAppDt.Text));
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToDouble(dt.Rows[0]["NoOfRec"]) > 0)
                {
                    gblFuction.AjxMsgPopup("Ready to Disburse already generated for that date.");
                    return;
                }
            }

            dt = obj.ChkDatewiseBranchDemDisb(gblFuction.setDate(txtAppDt.Text));
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToDouble(dt.Rows[0]["NoOfRec"]) == 0)
                {
                    gblFuction.AjxMsgPopup("Invalid Date (Before 4 days you can generate Ready to Disbursement only).");
                    return;
                }
            }
            //if (Convert.IsDBNull(dtVal.Rows[0]["Totval"]) == false)
            //    vAmtAvl = Convert.ToDouble(dtVal.Rows[0]["Totval"]); //Inflow -Outflow  + Demand
            //else
            //    vAmtAvl = 0.0;
            //if (Convert.IsDBNull(dtVal.Rows[0]["TotInFlow"]) == false)
            //    vTotInFlow = Convert.ToDouble(dtVal.Rows[0]["TotInFlow"]); //Inflow -Outflow  + Demand
            //else
            //    vTotInFlow = 0.0;

            //if (Convert.IsDBNull(dtVal.Rows[0]["TotDemand"]) == false)
            //    vDemand = Convert.ToDouble(dtVal.Rows[0]["TotDemand"]); //Inflow -Outflow  + Demand
            //else
            //    vDemand = 0.0;

            foreach (DataRow dr in dtList.Rows)
            {
                vProcfee = vProcfee + Convert.ToDouble(dr["Procfee"].ToString());
                vInsFee = vInsFee + Convert.ToDouble(dr["InsFee"].ToString());
                vTotDisb = vTotDisb + Convert.ToDouble(dr["Loan_Application_Amount"].ToString());

            }
            using (StringWriter oSW = new StringWriter())
            {
                dtList.WriteXml(oSW);
                vXml = oSW.ToString().Replace("T00:00:00","");
            }

            vErr = obj.InsertRtoD(vXml, vProcfee, vInsFee, vToDisbNo, vTotDisb, vTotInFlow, Convert.ToDouble(txtOpenBank.Text), vDemand, gblFuction.setDate(txtAppDt.Text));
            if (vErr > 0)
                gblFuction.AjxMsgPopup("Saved Successfully");
            else
                gblFuction.AjxMsgPopup("Data not Saved Successfully");

            //Mail(dtList);
                
        }


        //private void Mail(DataTable dtList)
        //{
        //    string vPath = ConfigurationManager.AppSettings["DBPath"];
        //    string vXml = "";
        //    dtList.WriteXml(vPath + "/RToD.xml", XmlWriteMode.WriteSchema);

        //    string vMailAdd = ConfigurationSettings.AppSettings["CompEmail"];
        //    string vPw = ConfigurationSettings.AppSettings["pass@2011"];
        //    string vMailSrvr = ConfigurationSettings.AppSettings["MailSrvr"];
        //    try
        //    {
        //        if (isEmail(txtMailTo.Text) == false)
        //        {
        //            MessageBox.Show("Error: This is Not Valid Email Address.", "BIJLI Management Studio", MessageBoxButtons.OK);
        //            return;
        //        }
        //        MailMessage mail = new MailMessage();
        //        mail.To.Add(txtMailTo.Text);
        //        mail.From = new MailAddress(vMailAdd);
        //        mail.Subject = "Ready To Disburse";
        //        mail.Body = "This mail is processed from Bijli Management Studtio.";
        //        mail.IsBodyHtml = true;
        //        mail.Attachments.Add(new Attachment("RToD.xml"));
        //        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
        //        smtp.Host = "smtp.gmail.com";
        //        smtp.Credentials = new System.Net.NetworkCredential(vMailAdd, vPw);
        //        smtp.EnableSsl = true;
        //        smtp.Timeout = 360000;
        //        smtp.Send(mail);
        //        MessageBox.Show("Data Backup Mail Send Successfully.", "BIJLI Management Studio", MessageBoxButtons.OK);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error: For Sending Data Backup Mail." + ex, "BIJLI Management Studio", MessageBoxButtons.OK);
        //    }
        //}


        //public bool isEmail(string pEmail)
        //{
        //    bool vDot = false, vCom = false, vRst = false;
        //    vDot = pEmail.Contains(".");
        //    vCom = pEmail.Contains("@");
        //    if (vDot == true && vCom == true)
        //        vRst = true;
        //    else
        //        vRst = false;
        //    return vRst;
        //}

        private void PopBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            vBrCode = (string)Session[gblValue.BrnchCode];
            vBrId = Convert.ToInt32(vBrCode);
            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
            chkDtl.DataSource = dt;
            chkDtl.DataTextField = "BranchName";
            chkDtl.DataValueField = "BranchCode";
            chkDtl.DataBind();
        }


        private void CheckAll()
        {
            Int32 vRow;
            if (rblAlSel.SelectedValue == "rbAll")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = true;
                chkDtl.Enabled = false;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = false;
                chkDtl.Enabled = true;
            }
        }


        private void popDetail()
        {
            ViewState["Dtl"] = null;
            string str = "";
            for (int vRow = 0; vRow < chkDtl.Items.Count; vRow++)
            {
                if (chkDtl.Items[vRow].Selected == true)
                {
                    if (str == "")
                        str = chkDtl.Items[vRow].Value;
                    else if (str != "")
                        str = str + "," + chkDtl.Items[vRow].Value;
                }
            }
            if (str == "")
                ViewState["Dtl"] = 0;
            else
                ViewState["Dtl"] = str;
        }


        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
            popDetail();
        }

        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            popDetail();
        }
    }
}
