using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCEBA;
using FORCECA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class REtExIDBIBr : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected static double vTot = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
            }
        }
        private void LoadGrid(string pAppMode, string pFromDt, string pToDt, string pBranch, Int32 pPgIndx)
        {
            DataTable dt = null;
            CBC_Repayment_Tracking oLS = null;
            Int32 vRows = 0;
            Int32 vLoanProduct = 0;


            try
            {
                string vBrCode = pBranch;
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                oLS = new CBC_Repayment_Tracking();
                dt = oLS.GetRetIDBIList(vFromDt, vToDt, pAppMode, vBrCode, pPgIndx, ref vRows);
                ViewState["Return"] = dt;
                gvSanc.DataSource = dt;
                gvSanc.DataBind();
                lblTotalPages.Text = CalTotPgs(vRows).ToString();
                lblCurrentPage.Text = cPgNo.ToString();
                if (cPgNo == 0)
                {
                    Btn_Previous.Enabled = false;
                    if (Int32.Parse(lblTotalPages.Text) > 1)
                        Btn_Next.Enabled = true;
                    else
                        Btn_Next.Enabled = false;
                }
                else
                {
                    Btn_Previous.Enabled = true;
                    if (cPgNo == Int32.Parse(lblTotalPages.Text))
                        Btn_Next.Enabled = false;
                    else
                        Btn_Next.Enabled = true;
                }
            }
            finally
            {
                dt = null;
                oLS = null;
            }
        }
        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Return To IDBI Extra Amount";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuRetExIDBIBr);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            
                try
                {
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    if (rdbOpt.SelectedValue == "N")
                        LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                    else if (rdbOpt.SelectedValue == "A")
                        LoadGrid("A", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                    else if (rdbOpt.SelectedValue == "C")
                        LoadGrid("C", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnDone_Click(object sender, EventArgs e)
        {
            string vStateEdit = string.Empty, vBrCode = string.Empty, vAppNo = string.Empty;
            DataTable dt = new DataTable();
            Int32 vErr = 0;
            string pXml = "", vXmlAC = "";
            vBrCode = Session[gblValue.BrnchCode].ToString();
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();
            DateTime pLoginDt=gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CBC_Repayment_Tracking oCG = null;
            vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            DataTable dtAccount = new DataTable();

            try
            {
                dt = GetData();
                dt.TableName = "Table1";
                using (StringWriter oSw = new StringWriter())
                {
                    dt.WriteXml(oSw);
                    pXml = oSw.ToString();
                }
                if (dt.Rows.Count == 0)
                {
                    gblFuction.AjxMsgPopup("No data To save...");
                    return;
                }
                DataRow dr;
                DataColumn dc = new DataColumn();
                dc.ColumnName = "DC";
                dtAccount.Columns.Add(dc);

                DataColumn dc1 = new DataColumn();
                dc1.ColumnName = "Amt";
                dc1.DataType = System.Type.GetType("System.Decimal");
                dtAccount.Columns.Add(dc1);

                DataColumn dc2 = new DataColumn();
                dc2.ColumnName = "DescId";
                dtAccount.Columns.Add(dc2);

                DataColumn dc3 = new DataColumn();
                dc3.ColumnName = "DtlId";
                dtAccount.Columns.Add(dc3);

                dtAccount.TableName = "Table1";
                if (vTot > 0)
                {
                    dr = dtAccount.NewRow();
                    dr["DescId"] = "G1080";
                    dr["DC"] = "D";
                    dr["Amt"] = vTot;
                    dr["DtlId"] = 1;
                    dtAccount.Rows.Add(dr);
                    dtAccount.AcceptChanges();

                    dr = dtAccount.NewRow();
                    dr["DescId"] = "C0001";
                    dr["DC"] = "C";
                    dr["Amt"] = vTot;
                    dr["DtlId"] = 2;
                    dtAccount.Rows.Add(dr);
                    dtAccount.AcceptChanges();
                }
                vXmlAC = DataTableTOXml(dtAccount);
                oCG = new CBC_Repayment_Tracking();
                if (this.RoleId != 1)                           
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(Session[gblValue.LoginDate].ToString()))
                        {
                            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                            return;
                        }
                    }
                }

                this.GetModuleByRole(mnuID.mnuRetExIDBIBr);
                vErr = oCG.UpdateRetIDBIExtAmt(pLoginDt, vBrCode, pXml, this.UserID, "I", "Save", vTblMst, vTblDtl, vFinYear, vXmlAC);
                if (vErr > 0)
                {
                    LoadGrid(rdbOpt.SelectedValue, txtFrmDt.Text, txtToDt.Text, vBrCode, cPgNo);
                    gblFuction.MsgPopup(gblMarg.EditMsg);
                    return;
                }
                else
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCG = null;
                //dt = null;
            }

        }
        protected void txtSanDt_TextChanged(object sender, EventArgs e)
        {
            TextBox checkbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            TextBox txtSanDt = (TextBox)row.FindControl("txtSanDt");
            DateTime vRetDt = gblFuction.setDate(txtSanDt.Text);
            DateTime vRetDtHO=gblFuction.setDate(row.Cells[4].Text);
            if (vRetDtHO > vRetDt)
            {
                gblFuction.AjxMsgPopup("Return Date from HO should not be grater than Actual Return date..");
                txtSanDt.Text = "";
                return;
            }
            upSanc.Update();
        }
        protected void chkApp_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            CheckBox chkApp = (CheckBox)row.FindControl("chkApp");
            TextBox txtSanDt = (TextBox)row.FindControl("txtSanDt");
            if (chkApp.Checked == true)
            {
                if (gblFuction.setDate(row.Cells[4].Text) > gblFuction.setDate(Session[gblValue.LoginDate].ToString()))
                {
                    txtSanDt.Text = "";
                    chkApp.Checked = false;
                    gblFuction.AjxMsgPopup("HO sent Date should not be greater than Login Date...");
                    return;
                }
                else
                {
                    txtSanDt.Text = Session[gblValue.LoginDate].ToString();
                    txtSanDt.Enabled = false;
                }
                
            }
            else
            {
                txtSanDt.Text = "";
            }
            upSanc.Update();
        }
        protected void gvSanc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkApp = (CheckBox)e.Row.FindControl("chkApp");
                    if (e.Row.Cells[7].Text == "Y")
                    {
                        chkApp.Checked = true;

                    }
                    else 
                    {
                        chkApp.Checked = false;
                    }

                }
            }
            finally
            {

            }
        }
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }

            LoadGrid(rdbOpt.SelectedValue, txtFrmDt.Text, txtToDt.Text, vBrCode, cPgNo);
            //tabLoanAppl.ActiveTabIndex = 0;
        }
        public DataTable GetData()
        {
            DataTable dt = (DataTable)ViewState["Return"];
            foreach (GridViewRow gr in gvSanc.Rows)
            {
                CheckBox chkApp = (CheckBox)gr.FindControl("chkApp");
                TextBox txtSanDt = (TextBox)gr.FindControl("txtSanDt");

                if (chkApp.Checked == true && txtSanDt.Text != "")
                {
                    dt.Rows[gr.RowIndex]["ActualRetDate"] = gblFuction.setDate(txtSanDt.Text);  
                    vTot=vTot+Convert.ToDouble(gr.Cells[5].Text);
                }
                else
                {
                    dt.Rows[gr.RowIndex].Delete();
                }
            }
            dt.AcceptChanges();
            return dt;
        }
        private string DataTableTOXml(DataTable dt)
        {
            string sXml = "";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                sXml = oSW.ToString();
            }
            return sXml;
        }
    }
}