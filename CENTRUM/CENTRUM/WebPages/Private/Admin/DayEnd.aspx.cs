using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Web.Security;

namespace CENTRUM.WebPages.Private.Admin
{
    public partial class DayEnd : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateGrid();

                txtAppDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopulateGrid1();
                txtAppDt.Enabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateGrid()
        {
            DataTable dt = new DataTable();
            CDayEnd oGbl = new CDayEnd();
            try
            {
                dt = oGbl.GetLoanTypeAmt();
                gvDisb.DataSource = dt;
                gvDisb.DataBind();
            }
            finally
            {
                dt = null;
                oGbl= null;
            }
        }
        private void PopulateGrid1()
        {
            DataTable dt = new DataTable();
            CDayEnd oGbl = new CDayEnd();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                dt = oGbl.ChkLessCollection(vBrCode, gblFuction.setDate(txtAppDt.Text));
                gvLessColl.DataSource = dt;
                gvLessColl.DataBind();
            }
            finally
            {
                dt = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtMem_TextChanged(object sender, EventArgs e)
        {
            double vMemTot=0;
            TextBox txtMem = null;
            GridView gv = (GridView)((TextBox)sender).Parent.Parent.NamingContainer;
            foreach (GridViewRow gr in gv.Rows)
            {
                txtMem = (TextBox)gr.FindControl("txtMem");
                vMemTot += Convert.ToDouble((txtMem.Text != "" && txtMem.Text != ".") ? txtMem.Text : "0");
            }
            txttotMem.Text = Convert.ToString(vMemTot);
            UpdatePanel1.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtPFee_TextChanged(object sender, EventArgs e)
        {
            double vPFeeTot = 0;
            TextBox txtPFee = null;
            GridView gv = (GridView)((TextBox)sender).Parent.Parent.NamingContainer;
            foreach (GridViewRow gr in gv.Rows)
            {
                txtPFee = (TextBox)gr.FindControl("txtPFee");
                vPFeeTot += Convert.ToDouble((txtPFee.Text != "" && txtPFee.Text != ".") ? txtPFee.Text : "0");
            }
            txttotPFee.Text = Convert.ToString(vPFeeTot);
            UpdatePanel2.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtInsurnce_TextChanged(object sender, EventArgs e)
        {
            double vInsTot = 0;
            TextBox txtInsurnce = null;
            GridView gv = (GridView)((TextBox)sender).Parent.Parent.NamingContainer;
            foreach (GridViewRow gr in gv.Rows)
            {
                txtInsurnce = (TextBox)gr.FindControl("txtInsurnce");
                vInsTot += Convert.ToDouble((txtInsurnce.Text != "" && txtInsurnce.Text != ".") ? txtInsurnce.Text : "0");
            }
            txttotInsurnce.Text = Convert.ToString(vInsTot);
            UpdatePanel3.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtTotDisb_TextChanged(object sender, EventArgs e)
        {
            double vTotDisbSum = 0;
            TextBox txtTotDisb = null;
            GridView gv = (GridView)((TextBox)sender).Parent.Parent.NamingContainer;
            foreach (GridViewRow gr in gv.Rows)
            {
                txtTotDisb = (TextBox)gr.FindControl("txtTotDisb");
                vTotDisbSum += Convert.ToDouble((txtTotDisb.Text != "" && txtTotDisb.Text != ".") ? txtTotDisb.Text : "0");
            }
            txtTotDisbSum.Text = Convert.ToString(vTotDisbSum);
            UpdatePanel4.Update();
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
                Session.Abandon();
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.RemoveAll();
                Response.Redirect("~/Login.aspx", false);
            }
        }
        protected void btnSave1_Click(object sender, EventArgs e)
        {
            //if (SaveRecords("Save") == true)
            //{
            //    gblFuction.MsgPopup(gblMarg.SaveMsg);

            //}
            Boolean vResult = false;
            string vXml1 = "";
            DataTable dtXml1 = Xml1();
            using (StringWriter oSw = new StringWriter())
            {
                dtXml1.WriteXml(oSw);
                vXml1 = oSw.ToString();
            }
            CDayEnd oCust = null;
            oCust = new CDayEnd();
            Int32 vErr = 0;
            vErr = oCust.SaveLessCollection(vXml1, gblFuction.setDate(txtAppDt.Text));
            if (vErr > 0)
            {
                gblFuction.MsgPopup("Record Save successfully");
                
                vResult = true;
                //ViewState["CustID"] = vCustID;
                //ViewState["CustMobNo"] = txtMobNo.Text;
                //ViewState["MemshipNo"] = txtMemNo.Text;
                //SendSMS();
            }
            else
            {
                gblFuction.MsgPopup(gblMarg.DBError);
                vResult = false;
            }
            //return vResult;

        }

        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0, vDayEndID = 0, vLstDayEnd = 0;
            string vXml, vBrCode = Session[gblValue.BrnchCode].ToString();
            string vMsg = "";
            CDayEnd oCust = null;
            CGblIdGenerator oGbl = null;
            DataTable dtXml = Xml();
            //DataTable dtXml1 = Xml1();
            using (StringWriter oSw = new StringWriter())
            {
                dtXml.WriteXml(oSw);
                vXml = oSw.ToString();
            }

            try
            {
                if (Mode == "Save")
                {
                    oCust = new CDayEnd();
                    oGbl = new CGblIdGenerator();
                    oCust = new CDayEnd();
                    vLstDayEnd = oCust.GetLastDayEnd( gblFuction.setDate(txtAppDt.Text), vBrCode);
                    if (vLstDayEnd == 1)
                    {
                        gblFuction.MsgPopup("Please Run Day End for Previous Date(s)");
                    }

                    vMsg = oCust.chkDayEndBfrSave(vBrCode, gblFuction.setDate(txtAppDt.Text),
                              Convert.ToDouble((txtTotColl.Text == "" || txtTotColl.Text == ".") ? "0" : txtTotColl.Text),
                              Convert.ToDouble((txtOpenCash.Text == "" || txtOpenCash.Text == ".") ? "0" : txtOpenCash.Text),
                              Convert.ToDouble((txtOpenBank.Text == "" || txtOpenBank.Text == ".") ? "0" : txtOpenBank.Text),
                              gblFuction.setDate(Session[gblValue.FinFromDt].ToString()),
                              Convert.ToInt32(Session[gblValue.FinYrNo].ToString()), vXml);
                    if (vMsg != "")
                    {
                        gblFuction.MsgPopup(vMsg);
                        return false;
                    }

                    this.GetModuleByRole(mnuID.mnuDayendProc);

                    vErr = oCust.SaveDayEnd(ref vDayEndID, gblFuction.setDate(txtAppDt.Text),
                        Convert.ToDouble((txtDemand.Text == "" || txtDemand.Text == ".") ? "0" : txtDemand.Text), Convert.ToDouble((txtTotColl.Text == "" || txtTotColl.Text == ".") ? "0" : txtTotColl.Text),
                        Convert.ToDouble((txtRegulrColl.Text == "" || txtRegulrColl.Text == ".") ? "0" : txtRegulrColl.Text), Convert.ToDouble((txtAdvColl.Text == "" ||txtAdvColl.Text == ".") ? "0" : txtAdvColl.Text),
                        Convert.ToDouble((txtODColl.Text == "" || txtODColl.Text == ".") ? "0" : txtODColl.Text), Convert.ToDouble((txtOverDue.Text == "" || txtOverDue.Text == ".") ? "0" : txtOverDue.Text),
                        Convert.ToDouble((txtFundRcvfrmHO.Text == "" ||txtFundRcvfrmHO.Text == ".") ? "0" : txtFundRcvfrmHO.Text), Convert.ToDouble((txtFundRcvfrmBr.Text == "" ||txtFundRcvfrmBr.Text == "") ? "0" : txtFundRcvfrmBr.Text),
                        Convert.ToDouble((txtCashWithOwn.Text == "" ||txtCashWithOwn.Text == ".") ? "0" : txtCashWithOwn.Text), Convert.ToDouble((txtCashDepOwn.Text == "" || txtCashDepOwn.Text == ".") ? "0" : txtCashDepOwn.Text),
                        Convert.ToDouble((txtCashDepHO.Text == "" ||txtCashDepHO.Text == ".") ? "0" : txtCashDepHO.Text), Convert.ToDouble((txtCashTrantoBr.Text == "" ||txtCashTrantoBr.Text == ".") ? "0" : txtCashTrantoBr.Text),
                        Convert.ToDouble((txtOtherIncome.Text == "" || txtOtherIncome.Text == ".") ? "0" : txtOtherIncome.Text), txtOthrIncomNote.Text.Replace("'", "''"),
                        Convert.ToDouble((txtOthrExpense.Text == "" || txtOthrExpense.Text == ".") ? "0" : txtOthrExpense.Text), txtOthrExpnseNote.Text.Replace("'", "''"),
                        Convert.ToDouble((txtOpenCash.Text == "" || txtOpenCash.Text == ".") ? "0" : txtOpenCash.Text), txtOpenCashNote.Text.Replace("'", "''"),
                        Convert.ToDouble((txtOpenBank.Text == "" || txtOpenBank.Text == ".") ? "0" : txtOpenBank.Text), txtOpenBankNote.Text.Replace("'", "''"),
                        vXml, vBrCode, this.UserID,"Save");

                    if (vErr > 0)
                    {
                        vResult = true;
                        //ViewState["CustID"] = vCustID;
                        //ViewState["CustMobNo"] = txtMobNo.Text;
                        //ViewState["MemshipNo"] = txtMemNo.Text;
                        //SendSMS();
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }
            finally
            {
                oCust = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable Xml()
            {
            DataTable dt = new DataTable("Table1");
            dt.Columns.Add("Product");
            dt.Columns.Add("Member");
            dt.Columns.Add("PFee");
            dt.Columns.Add("Insurance");
            dt.Columns.Add("TotDisb");

            foreach (GridViewRow gr in gvDisb.Rows)
            {
                TextBox txtMem =(TextBox)gr.FindControl("txtMem");
                TextBox txtPFee = (TextBox)gr.FindControl("txtPFee");
                TextBox txtInsurnce = (TextBox)gr.FindControl("txtInsurnce");
                TextBox txtTotDisb = (TextBox)gr.FindControl("txtTotDisb");
                if(txtMem.Text != "")
                {
                DataRow dr = dt.NewRow();
                dr["Product"] = gr.Cells[0].Text;
                dr["Member"] = txtMem.Text;
                dr["PFee"] =  txtPFee.Text;
                dr["Insurance"] = txtInsurnce.Text;
                dr["TotDisb"] = txtTotDisb.Text;
                dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        private DataTable Xml1()
        {
            DataTable dt1 = new DataTable("Table2");
            dt1.Columns.Add("LoanId");
            dt1.Columns.Add("RO Name");
            dt1.Columns.Add("Center Name");
            dt1.Columns.Add("Group No");
            dt1.Columns.Add("Group Name");
            dt1.Columns.Add("MemberNo");
            dt1.Columns.Add("Member Name");
            dt1.Columns.Add("Loan No");
            dt1.Columns.Add("Princ OD");
            dt1.Columns.Add("ddlReason");
            dt1.Columns.Add("ddlAction");
            DataRow dr;


            
            foreach (GridViewRow row in gvLessColl.Rows)
            {
                dr = dt1.NewRow();

                DropDownList ddlReason = (DropDownList)row.FindControl("ddlReason");
                DropDownList ddlAction = (DropDownList)row.FindControl("ddlAction");
                dr["LoanId"] = row.Cells[0].Text.Trim();
                dr["RO Name"] = row.Cells[1].Text.Trim();
                dr["Center Name"] = row.Cells[2].Text.Trim();
                dr["Group No"] = row.Cells[3].Text.Trim();
                dr["Group Name"] = row.Cells[4].Text.Trim();
                dr["MemberNo"] = row.Cells[5].Text.Trim();
                dr["Member Name"] = row.Cells[6].Text.Trim();
                dr["Loan No"] = row.Cells[7].Text.Trim();
                dr["Princ OD"] = row.Cells[8].Text.Trim();
                dr["ddlReason"] = ddlReason.SelectedItem.Text;
                dr["ddlAction"] = ddlAction.SelectedItem.Text;
                
                dt1.Rows.Add(dr);
                dt1.AcceptChanges();
            }
            return dt1;
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
        protected void btnExit1_Click(object sender, EventArgs e)
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
    }
}
