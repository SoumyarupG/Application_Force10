using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using FORCEBA;
using FORCECA;
using System.Web.UI;
using System.Web;

namespace CENTRUM.WebPages.Private.BCOperation
{
    public partial class BC_RepayTracking : CENTRUMBase
    {
        protected int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                StatusButton("View");
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                tabRepayTrack.ActiveTabIndex = 0;
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
                this.PageHeading = "BC Repayment Tracking";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuRepaymentTracking);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnPrint.Enabled = true;
                    //btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    ClearControls();
                    break;
                case "Show":
                    btnPrint.Enabled = false;
                    //btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnPrint.Enabled = true;
                    //btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnPrint.Enabled = false;
                    //btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnPrint.Enabled = false;
                    //btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnPrint.Visible = false;
                    //btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        private void EnableControl(Boolean Status)
        {
            //txtLoanPurpose.Enabled = Status;
            //ddlLoanSector.Enabled = Status;
        }
        private void ClearControls()
        {
            //txtLoanPurpose.Text = "";
            //ddlLoanSector.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
        }

        //private void PopBranch()
        //{
        //    DataTable dt = null;
        //    CGblIdGenerator oGb = null;
        //    try
        //    {
        //        oGb = new CGblIdGenerator();
        //        dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", 0, "AA", "AA", System.DateTime.Now, "0000");
        //        ddlBr.DataSource = dt;
        //        ddlBr.DataTextField = "BranchName";
        //        ddlBr.DataValueField = "BranchCode";
        //        ddlBr.DataBind();
        //        ListItem oli = new ListItem("<--SELECT-->", "-1");
        //        ddlBr.Items.Insert(0, oli);
        //    }
        //    finally
        //    {
        //        oGb = null;
        //        dt = null;
        //    }
        //}

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]), "IDBI");
                if (dt.Rows.Count > 0)
                {
                    ddlBr.DataSource = dt;
                    ddlBr.DataTextField = "BranchName";
                    ddlBr.DataValueField = "BranchCode";
                    ddlBr.DataBind();
                    ListItem liSel = new ListItem("All", "-1");
                    ddlBr.Items.Insert(0, liSel);
                    if (Session[gblValue.BrnchCode].ToString() != "0000")
                    {
                        ddlBr.SelectedIndex = ddlBr.Items.IndexOf(ddlBr.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                        ddlBr.Enabled = false;
                        popRO();
                    }
                }
                else
                {

                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        protected void ddlBr_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vBrCode = "";
            vBrCode = ddlBr.SelectedValue.ToString();
            popRO();
        }

        //private void popFO(string vBrCode)
        //{
        //    DataTable dt = null;
        //    CCM oCM = null;
        //    try
        //    {
        //        oCM = new CCM();
        //        dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO,UM");
        //        ddlFO.DataTextField = "EOName";
        //        ddlFO.DataValueField = "EOId";
        //        ddlFO.DataSource = dt;
        //        ddlFO.DataBind();
        //        ListItem oItm = new ListItem();
        //        oItm.Text = "<--- Select --->";
        //        oItm.Value = "-1";
        //        ddlFO.Items.Insert(0, oItm);
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oCM = null;
        //    }
        //}
        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode;
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
            }
            else
            {
                vBrCode = ddlBr.SelectedValue.ToString();
            }

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlCo.DataSource = dt;
                ddlCo.DataTextField = "EoName";
                ddlCo.DataValueField = "EoId";
                ddlCo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "A");
                ddlCo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }
        protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCo.SelectedIndex > 0) PopCenter(ddlCo.SelectedValue);
        }
        private void PopCenter(string vCOID)
        {
            DataTable dtGr = null;
            CLoanRecovery oCL = null;
            try
            {
                ddlCenter.Items.Clear();
                string vBrCode;
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
                else
                {
                    vBrCode = ddlBr.SelectedValue.ToString();
                }
                oCL = new CLoanRecovery();
                dtGr = oCL.PopCenterWithCollDay(vCOID, gblFuction.setDate(txtFrmDt.Text), vBrCode, "W"); //With CollDay
                dtGr.AcceptChanges();
                ddlCenter.DataSource = dtGr;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "MarketID";
                ddlCenter.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "A");
                ddlCenter.Items.Insert(0, oLi);
            }
            finally
            {
                dtGr = null;
                oCL = null;
            }
        }
        //protected void ddlFO_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DataTable dtGr = null;
        //    CGblIdGenerator oGbl = null;
        //    try
        //    {
        //        int vEOID = Convert.ToInt32(ddlCo.SelectedValue);
        //        string vBrCode = ddlBr.SelectedValue;
        //        dtGr = new DataTable();
        //        oGbl = new CGblIdGenerator();
        //        dtGr = oGbl.PopComboMIS("S", "N", "AA", "GroupId", "GroupName", "GroupMst", vEOID, "EOID", "ClosingDt", gblFuction.setDate(txtFrmDt.Text), vBrCode);
        //        ddlGroup.DataSource = dtGr;
        //        ddlGroup.DataTextField = "GroupName";
        //        ddlGroup.DataValueField = "GroupID";
        //        ddlGroup.DataBind();
        //        ListItem oLi = new ListItem("All", "-1");
        //        ddlGroup.Items.Insert(0, oLi);
        //    }
        //    finally
        //    {
        //        dtGr = null;
        //        oGbl = null;
        //    }
        //}

        //protected void ChangePage(object sender, CommandEventArgs e)
        //{
        //    switch (e.CommandName)
        //    {
        //        case "Previous":
        //            vPgNo = Int32.Parse(lblCurrentPage.Text) - 1; //lblCurrentPage
        //            break;
        //        case "Next":
        //            vPgNo = Int32.Parse(lblCurrentPage.Text) + 1; //lblTotalPages
        //            break;
        //    }
        //    LoadGrid(vPgNo);
        //    tabRepayTrack.ActiveTabIndex = 0;
        //}
        //private int CalTotPgs(double pRows)
        //{
        //    int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
        //    return totPg;
        //}
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = new DataTable();
            CBC_Repayment_Tracking oBC = null;
            Int32 vRows = 0;
            //Int32 vEOID = 0;
            string vBrCode, vCenterId = "A", vEOID = "A";

            try
            {
                oBC = new CBC_Repayment_Tracking();
                vBrCode = ddlBr.SelectedValue;
                if (vBrCode != "A")
                {
                    vEOID = ddlCo.SelectedValue;
                }
                if (vEOID != "A")
                {
                    vCenterId = ddlCenter.SelectedValue;
                }
                //if (vBrCode !="-1")
                //{
                //    if (Convert.ToInt32(ddlCo.SelectedValue) > 0)
                //    {
                //        vEOID = Convert.ToInt32(ddlCo.SelectedValue);
                //    }
                //    else
                //    {
                //        vEOID = 0;
                //    }
                //}
                //if(vEOID > 0)
                //{
                //    if (ddlCenter.SelectedItem.Text.Trim() != "All")
                //    {
                //        vCenterId = ddlCenter.SelectedItem.Text.Trim();
                //    }
                //    else
                //    {
                //        vCenterId = "A";
                //    }
                //}

                dt = oBC.BC_Repayment_Track(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), ddlBr.SelectedValue, vEOID, vCenterId);
                gvRepTrack.DataSource = dt;
                gvRepTrack.DataBind();
                ViewState["REPAYTRACK"] = dt;

                //if (dt.Rows.Count <= 0)
                //{
                //    lblTotalPages.Text = "0";
                //    lblCurrentPage.Text = "0";
                //    // lblCgtChkRmn.Text = "0";
                //}
                //else
                //{
                //    lblTotalPages.Text = CalTotPgs(vRows).ToString();
                //    lblCurrentPage.Text = vPgNo.ToString();
                //    // lblCgtChkRmn.Text = vRows.ToString();
                //}
                //if (vPgNo == 1)
                //{
                //    Btn_Previous.Enabled = false;
                //    if (Int32.Parse(lblTotalPages.Text) > 0 && vPgNo != Int32.Parse(lblTotalPages.Text))
                //        Btn_Next.Enabled = true;
                //    else
                //        Btn_Next.Enabled = false;
                //}
                //else
                //{
                //    Btn_Previous.Enabled = true;
                //    if (vPgNo == Int32.Parse(lblTotalPages.Text))
                //        Btn_Next.Enabled = false;
                //    else
                //        Btn_Next.Enabled = true;
                //}
            }
            finally
            {
                dt = null;
                oBC = null;
            }
        }

        protected void gvRepTrack_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Convert.ToInt32(e.Row.Cells[9].Text) > 0)
                {
                    // change the color
                    e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
            StatusButton("Add");

        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Print();

        }

        protected void btnPrintAll_Click(object sender, EventArgs e)
        {
            PrintDetails();

        }
        private void Print()
        {
            DataTable dt = null;
            string vFileNm = "";
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            dt = (DataTable)ViewState["REPAYTRACK"];
            dt.Columns.Remove("BranchCode");
            dt.Columns.Remove("EOID");
            //dt.Columns.Remove("GroupId");
            //dt1 = oBC.BC_Repayment_Track(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text),);

            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            vFileNm = "attachment;filename=BC_Repayment_Tracking.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan=" + dt.Columns.Count + "><b><u><font size='3'>BC Repayment Tracking</font></u></b></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
            htw.WriteLine("</table>");
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sw.ToString());
            Response.End();
        }

        private void PrintDetails()
        {
            DataTable dt = null;
            CBC_Repayment_Tracking oBC = new CBC_Repayment_Tracking();
            string vFileNm = "";
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            dt = oBC.BC_rptRepayment_Track_Dtl(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text));
            
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            vFileNm = "attachment;filename=BC_Repayment_Tracking.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan=" + dt.Columns.Count + "><b><u><font size='3'>BC Repayment Tracking</font></u></b></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
            htw.WriteLine("</table>");
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sw.ToString());
            Response.End();
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }


        protected void gvRepTrack_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vMemID = "";
            vMemID = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                foreach (GridViewRow gr in gvRepTrack.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
            }

            GetDetails();

        }
        
        private void GetDetails()
        {
            DataTable dt = null,dt1=null;
            CBC_Repayment_Tracking oBC =  new CBC_Repayment_Tracking();
            //Int32 vEOID = 0;
            //string vBrCode, vCenterId = "";
            string vBrCode, vCenterId = "A", vEOID = "A";

            vBrCode = ddlBr.SelectedValue;
            //if (vBrCode !="-1")
            //{
            //    vEOID = Convert.ToInt32(ddlCo.SelectedValue);
            //}
            //if(vEOID > 0)
            //{
            //    vCenterId = ddlCenter.SelectedItem.Text.Trim();
            //}
            if (vBrCode != "A")
            {
                vEOID = ddlCo.SelectedValue;
            }
            if (vEOID != "A")
            {
                vCenterId = ddlCenter.SelectedValue;
            }    
            string vFileNm = "";
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();

            dt = (DataTable)ViewState["REPAYTRACK"];
            dt1 = oBC.BC_rptRepayment_Track_Summary(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), Convert.ToString(dt.Rows[0]["BranchCode"]), Convert.ToString(dt.Rows[0]["EOID"]), Convert.ToString(dt.Rows[0]["Market"]));
            DataGrid1.DataSource = dt1;
            
            DataGrid1.DataBind();
            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            vFileNm = "attachment;filename=BC_Repayment_Tracking_Dtl.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='11'><b><u><font size='3'>BC Repayment Tracking Details</font></u></b></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
            htw.WriteLine("</table>");
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sw.ToString());
            Response.End();
        }


        //private void LoadGrid(Int32 pPgIndx)
        //{
        //    DataTable dt = new DataTable();
        //    CBC_Repayment_Tracking oBC = null;
        //    Int32 vRows = 0;
        //    Int32 vEOID = 0;
        //    string vBrCode, vGroupId = "";

        //    try
        //    {
        //        oBC = new CBC_Repayment_Tracking();
        //        vBrCode = ddlBr.SelectedValue;
        //        if (vBrCode != "-1")
        //        {
        //            if (Convert.ToInt32(ddlFO.SelectedValue) > 0)
        //            {
        //                vEOID = Convert.ToInt32(ddlFO.SelectedValue);
        //            }
        //            else
        //            {
        //                vEOID = 0;
        //            }
        //        }
        //        if (vEOID > 0)
        //        {
        //            if (ddlGroup.SelectedItem.Text.Trim() != "All")
        //            {
        //                vGroupId = ddlGroup.SelectedItem.Text.Trim();
        //            }
        //            else
        //            {
        //                vGroupId = "A";
        //            }
        //        }

        //        dt = oBC.BC_Repayment_Track(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), ddlBr.SelectedValue, vEOID, vGroupId, pPgIndx, ref vRows);
        //        gvRepTrack.DataSource = dt.DefaultView;
        //        gvRepTrack.DataBind();
        //        ViewState["REPAYTRACK"] = dt;
        //        //if (dt.Rows.Count <= 0)
        //        //{
        //        //    lblTotalPages.Text = "0";
        //        //    lblCurrentPage.Text = "0";
        //        //    // lblCgtChkRmn.Text = "0";
        //        //}
        //        //else
        //        //{
        //        //    lblTotalPages.Text = CalTotPgs(vRows).ToString();
        //        //    lblCurrentPage.Text = vPgNo.ToString();
        //        //    // lblCgtChkRmn.Text = vRows.ToString();
        //        //}
        //        //if (vPgNo == 1)
        //        //{
        //        //    Btn_Previous.Enabled = false;
        //        //    if (Int32.Parse(lblTotalPages.Text) > 0 && vPgNo != Int32.Parse(lblTotalPages.Text))
        //        //        Btn_Next.Enabled = true;
        //        //    else
        //        //        Btn_Next.Enabled = false;
        //        //}
        //        //else
        //        //{
        //        //    Btn_Previous.Enabled = true;
        //        //    if (vPgNo == Int32.Parse(lblTotalPages.Text))
        //        //        Btn_Next.Enabled = false;
        //        //    else
        //        //        Btn_Next.Enabled = true;
        //        //}
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oBC = null;
        //    }
        //}
    }
}
   