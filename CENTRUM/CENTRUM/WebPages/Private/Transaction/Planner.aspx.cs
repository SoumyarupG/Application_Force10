using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Data;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class Planner : CENTRUMBase
    {
        protected int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                ViewState["EoID"] = null;
                ViewState["mTable"] = null;
                ViewState["Branch"] = null;
                StatusButton("View");
                tbBnk.ActiveTabIndex = 0;
                GetEoName();
                popBranch();
                //InitGrid();

                ddlYear.SelectedValue = Convert.ToString(gblFuction.setDate(Session[gblValue.LoginDate].ToString()).Year);
                ddlMonth.SelectedValue = Convert.ToString(gblFuction.setDate(Session[gblValue.LoginDate].ToString()).Month);

                popDtl(Convert.ToString(gblFuction.setDate(Session[gblValue.LoginDate].ToString()).Year)
              , Convert.ToString(gblFuction.setDate(Session[gblValue.LoginDate].ToString()).Month)
              , this.UserID);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Margdarshak.aspx", false);

                this.Menu = false;
                this.PageHeading = "Planner";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.GetModuleByRole(mnuID.mnuHRPlanner);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = true;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = true;
                    btnEdit.Visible = true;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnAdd.Visible = true;
                    btnEdit.Visible = true;
                    btnDelete.Visible = true;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Block Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Margdarshak.aspx", false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
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
                    btnExit.Enabled = true;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ViewState["EoID"] = null;
            ViewState["mTable"] = null;
            InitGrid();
            ddlYear.SelectedValue = "-1";
            ddlMonth.SelectedValue = "-1";
            txtEmp.Text = "";
            txtAdv.Text = "0";
            txtDesgn.Text = "";
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ViewState["mTable"] = null;
            try
            {
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = "Add";
                tbBnk.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
                InitGrid();

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
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbBnk.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
            popDtl(ddlYear.SelectedValue, ddlMonth.SelectedValue, this.UserID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords("Save") == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>   
        //public void InitGrid()
        //{
        //    String vYear = "", vMonth = "";
        //    int vTotDays;
        //    DataRow dr = null;
        //    DataTable dt = null;
        //    if (ViewState["mTable"] == null)
        //    {
        //        dt = new DataTable();
        //        dt.Columns.Add("FDt");
        //        dt.Columns.Add("TDt");
        //        dt.Columns.Add("Branch");
        //        dt.Columns.Add("Assignments");
        //        dt.Columns.Add("Remarks");
        //        vTotDays = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue));
        //        for (int i = 1; i <= vTotDays; i++)
        //        {
        //            dr = dt.NewRow();
        //            //vTotDays = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue));
        //            vYear = ddlYear.SelectedValue;
        //            vMonth = Convert.ToInt32(ddlMonth.SelectedValue) < 10 ? "0" + ddlMonth.SelectedValue : ddlMonth.SelectedValue;
        //            //dr["FDt"] = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //            //dr["TDt"] = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //            dr["FDt"] = i + "/" + vMonth + "/" + vYear;
        //            dr["TDt"] = (gblFuction.setDate(i + "/" + vMonth + "/" + vYear)).DayOfWeek; //Convert.ToString(vTotDays) + "/" + vMonth + "/" + vYear;
        //            dr["Branch"] = Session[gblValue.BrnchCode].ToString();
        //            dr["Assignments"] = "";
        //            dr["Remarks"] = "";
        //            dt.Rows.Add(dr);
        //        }
        //        dt.AcceptChanges();
        //        gvSchdl.DataSource = dt;
        //        gvSchdl.DataBind();
        //        ViewState["mTable"] = dt;
        //        upSchdl.Update();
        //    }
        //}
        //public void btnAddNew_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = (DataTable)ViewState["mTable"];
        //    Button btnImDel = (Button)sender;
        //    GridViewRow gv = (GridViewRow)btnImDel.NamingContainer;
        //    if (gv.RowIndex == dt.Rows.Count - 1)
        //    {
        //        AddRow();               
        //    }
        //}
        //public void ImDel_Click(object sender, EventArgs e)
        //{
        //    ImageButton btnImDel = (ImageButton)sender;
        //    GridViewRow gv = (GridViewRow)btnImDel.NamingContainer;
        //    DelRow(gv.RowIndex);
        //}
        //public void AddRow()
        //{
        //    String vYear = "", vMonth = "";
        //    int vTotDays;
        //    DataRow dr = null;
        //    DataTable dt = (DataTable)ViewState["mTable"];
        //    //string max = dt.AsEnumerable()
        //    //            .Max(row => row["TAmt"])
        //    //            .ToString(); 

        //    dr = dt.NewRow();
        //    //dr["FDt"] = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    //dr["TDt"] = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    vTotDays = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue));
        //    vYear = ddlYear.SelectedValue;
        //    vMonth = Convert.ToInt32(ddlMonth.SelectedValue) < 10 ? "0" + ddlMonth.SelectedValue : ddlMonth.SelectedValue;
        //    //dr["FDt"] = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    //dr["TDt"] = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    dr["FDt"] = "01/" + vMonth + "/" + vYear;
        //    dr["TDt"] = Convert.ToString(vTotDays) + "/" + vMonth + "/" + vYear;
        //    dr["Branch"] = Session[gblValue.BrnchCode].ToString();
        //    dr["Assignments"] = "";
        //    dt.Rows.Add(dr);
        //    dt.AcceptChanges();
        //    gvSchdl.DataSource = dt;
        //    gvSchdl.DataBind();
        //    ViewState["mTable"] = dt;
        //    upSchdl.Update();
        //}
        public void DelRow(int vRowIndex)
        {
            if (vRowIndex != 0)
            {
                DataTable dt = (DataTable)ViewState["mTable"];
                dt.Rows[vRowIndex].Delete();
                gvSchdl.DataSource = dt;
                gvSchdl.DataBind();
                ViewState["mTable"] = dt;
            }
            else
            {
                ViewState["mTable"] = null;
                InitGrid();
            }
            upSchdl.Update();
        }
        public void CalculateInt()
        {
            ViewState["mTable"] = null;
            ViewState["mTable"] = GetTable();
            txtAdv.Text = "0";
            gvSchdl.DataSource = (DataTable)ViewState["mTable"]; ;
            gvSchdl.DataBind();
            upSchdl.Update();
        }
        public void txtFDt_TextChanged(object sender, EventArgs e)
        {
            CalculateInt();
        }
        public void txtTDt_TextChanged(object sender, EventArgs e)
        {
            CalculateInt();
        }
        public void txtAmt_TextChanged(object sender, EventArgs e)
        {
            CalAdv();
        }
        public void txtRemarks_TextChanged(object sender, EventArgs e)
        {
            CalculateInt();
        }
        public void ddlHalt_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlHalt = (DropDownList)sender;
            GridViewRow gr = (GridViewRow)ddlHalt.Parent.NamingContainer;
            TextBox txtAmt = (TextBox)gr.FindControl("txtAmt");
            if (ddlHalt.SelectedValue == "Y")
            {
                txtAmt.Enabled = true;
            }
            else
            {
                txtAmt.Text = "0";
                txtAmt.Enabled = false;
                CalAdv();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void GetEoName()
        {
            CUser oCu = new CUser();
            DataTable dt = oCu.GetEoByUser(this.UserID);
            if (dt.Rows.Count > 0)
            {
                txtEmp.Text = dt.Rows[0]["EoName"].ToString();
                txtDesgn.Text = dt.Rows[0]["Designation"].ToString();
                ViewState["EoID"] = dt.Rows[0]["EoID"].ToString();
            }
            else
            {
                txtEmp.Text = "";
                txtDesgn.Text = "";
                ViewState["EoID"] = null;
            }
        }
        public void popDtl(string vYear, string vMonth, int vUserID)
        {
            //DataTable dt = null;
            //CSalary oCS = null;
            //oCS = new CSalary();
            //try
            //{
            //    dt = oCS.GetPlanner(Convert.ToInt32(vYear), Convert.ToInt32(vMonth), vUserID);
            //    if (dt.Rows.Count > 0)
            //    {
            //        txtAdv.Text = dt.Rows[0]["ReqAdv"].ToString();
            //        gvSchdl.DataSource = dt;
            //        gvSchdl.DataBind();
            //        ViewState["mTable"] = dt;
            //        btnDelete.Enabled = true;
            //        if (dt.Rows[0]["IsSubmit"].ToString() == "Y")
            //        {
            //            chkSubmit.Checked = true;
            //            chkSubmit.Enabled = false;
            //        }
            //        else
            //        {
            //        }
            //    }
            //    else
            //    {
            //        InitGrid();
            //    }
            //}
            //finally
            //{
            //    dt = null;
            //    oCS = null;
            //    upSchdl.Update();
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
        private void popBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ViewState["Branch"] = dt;
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["mTable"] = null;
            popDtl(ddlYear.SelectedValue, ddlMonth.SelectedValue, this.UserID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["mTable"] = null;
            txtAdv.Text = "0";
            popDtl(ddlYear.SelectedValue, ddlMonth.SelectedValue, this.UserID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSchdl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlBranch = (DropDownList)e.Row.FindControl("ddlBranch");
                DropDownList ddlBranchDone = (DropDownList)e.Row.FindControl("ddlBranchDone");
                DropDownList ddlHalt = (DropDownList)e.Row.FindControl("ddlHalt");
                DropDownList ddlActStartBranch = (DropDownList)e.Row.FindControl("ddlActStartBranch");
                TextBox txtAssainments = (TextBox)e.Row.FindControl("txtAssainments");
                TextBox txtWorkDone = (TextBox)e.Row.FindControl("txtWorkDone");
                TextBox txtAmt = (TextBox)e.Row.FindControl("txtAmt");
                TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                TextBox txtTDt = (TextBox)e.Row.FindControl("txtTDt");
                TextBox txtFDt = (TextBox)e.Row.FindControl("txtFDt");
                TextBox txtDistance = (TextBox)e.Row.FindControl("txtDistance");

                ListItem oli = new ListItem("<--Select-->", "-1");

                ddlBranch.DataSource = (DataTable)ViewState["Branch"];
                ddlBranch.DataBind();
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                ddlBranch.Items.Insert(0, oli);

                ddlBranchDone.DataSource = (DataTable)ViewState["Branch"];
                ddlBranchDone.DataBind();
                ddlBranchDone.DataTextField = "BranchName";
                ddlBranchDone.DataValueField = "BranchCode";
                ddlBranchDone.DataBind();
                ddlBranchDone.Items.Insert(0, oli);

                ddlActStartBranch.DataSource = (DataTable)ViewState["Branch"];
                ddlActStartBranch.DataBind();
                ddlActStartBranch.DataTextField = "BranchName";
                ddlActStartBranch.DataValueField = "BranchCode";
                ddlActStartBranch.DataBind();
                ddlActStartBranch.Items.Insert(0, oli);

                ddlBranch.SelectedValue = e.Row.Cells[11].Text;
                ddlBranchDone.SelectedValue = e.Row.Cells[13].Text;
                ddlHalt.SelectedValue = e.Row.Cells[12].Text;
                ddlActStartBranch.SelectedValue = e.Row.Cells[14].Text;
                txtAmt.Enabled = false;
                if (txtTDt.Text == "Saturday") e.Row.BackColor = System.Drawing.Color.Yellow;
                if (txtTDt.Text == "Sunday")
                {
                    e.Row.BackColor = System.Drawing.Color.OrangeRed;
                    ddlBranch.Enabled = false;
                    txtAssainments.Enabled = false;
                    ddlBranchDone.Enabled = false;
                    txtWorkDone.Enabled = false;
                    txtRemarks.Enabled = false;
                    txtDistance.Enabled = false;
                    ddlHalt.Enabled = false;
                    ddlActStartBranch.Enabled = false;
                    txtAmt.Enabled = false;
                }
                if (e.Row.Cells[15].Text == "Y")
                {
                    ddlBranch.Enabled = false;
                    txtAssainments.Enabled = false;
                }
                //if (
                //    (gblFuction.setDate(Session[gblValue.LoginDate].ToString()) > gblFuction.setDate(txtFDt.Text.Trim()))                   
                //  )
                //{
                //    ddlBranch.Enabled = false;
                //    txtAssainments.Enabled = false;
                //} 
            }
        }
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>   
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vErr = 0;
            string vXML = "";
            DataTable dtXML = null;
            CSalary oHr = null;

            try
            {
                if (ViewState["EoID"] == null)
                {
                    gblFuction.AjxMsgPopup("No Employee Code found for this User");
                    return false;
                }

                if (!ValidateFields())
                    return false;

                dtXML = GetTable();
                if (dtXML.Rows.Count > 0)
                    vXML = GetXml(dtXML);
                else
                {
                    gblFuction.AjxMsgPopup("No Data found...");
                    return false;
                }

                if (Mode == "Save")
                {
                    //oHr = new CSalary();
                    //vErr = oHr.SavePlanner(chkSubmit.Checked == true ? "Y" : "N", Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue), Convert.ToDouble(txtAdv.Text), vXML, vBrCode, this.UserID, "Save");

                    //if (vErr > 0)
                    //{
                    //    vResult = true;
                    //}
                    //else
                    //{
                    //    gblFuction.MsgPopup(gblMarg.DBError);
                    //    vResult = false;
                    //}
                }
                if (Mode == "Delete")
                {
                    //oHr = new CSalary();
                    //vErr = oHr.SavePlanner(chkSubmit.Checked == true ? "Y" : "N", Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue), Convert.ToDouble(txtAdv.Text), vXML, vBrCode, this.UserID, "Delete");

                    //if (vErr > 0)
                    //{
                    //    vResult = true;
                    //}
                    //else
                    //{
                    //    gblFuction.MsgPopup(gblMarg.DBError);
                    //    vResult = false;
                    //}
                }

                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oHr = null;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //private DataTable GetTable()
        //{
        //    DataRow dr = null;
        //    TextBox txtFDt, txtTDt, txtAssignments, txtRemarks;
        //    DropDownList ddlBranch;
        //    DataTable dt = new DataTable("Table1");

        //    DataColumn dc1 = new DataColumn("FDt");
        //    dt.Columns.Add(dc1);
        //    DataColumn dc2 = new DataColumn("TDt");
        //    dt.Columns.Add(dc2);
        //    DataColumn dc3 = new DataColumn("Assignments");
        //    dt.Columns.Add(dc3);
        //    DataColumn dc4 = new DataColumn("Remarks");
        //    dt.Columns.Add(dc4);
        //    DataColumn dc5 = new DataColumn("Branch");
        //    dt.Columns.Add(dc5);

        //    foreach (GridViewRow gR in gvSchdl.Rows)
        //    {
        //        txtFDt = (TextBox)gR.FindControl("txtFDt");
        //        txtTDt = (TextBox)gR.FindControl("txtTDt");
        //        txtAssignments = (TextBox)gR.FindControl("txtAssainments");
        //        txtRemarks = (TextBox)gR.FindControl("txtRemarks");
        //        ddlBranch = (DropDownList)gR.FindControl("ddlBranch");        

        //        dr = dt.NewRow();
        //        //dr["FDt"] = gblFuction.setDate(txtFDt.Text.Trim());
        //        //dr["TDt"] = gblFuction.setDate(txtTDt.Text.Trim());
        //        dr["FDt"] =txtFDt.Text.Trim();
        //        dr["TDt"] =txtTDt.Text.Trim();
        //        dr["Assignments"] = txtAssignments.Text.Trim();
        //        dr["Remarks"] = txtRemarks.Text.Trim();
        //        dr["Branch"] = ddlBranch.SelectedValue;
        //        dt.Rows.Add(dr);
        //        dt.AcceptChanges();
        //    }
        //    return dt;
        //}
        private DataTable GetTable()
        {
            DataRow dr = null;
            TextBox txtFDt, txtTDt, txtAssignments, txtRemarks, txtAmt, txtWorkDone, txtDistance;
            DropDownList ddlBranch, ddlBranchDone, ddlHalt, ddlActStartBranch;
            DataTable dt = new DataTable("Table1");

            //dt = new DataTable();
            dt.Columns.Add("FDt");
            dt.Columns.Add("TDt");
            dt.Columns.Add("Branch");
            dt.Columns.Add("Assignments");
            dt.Columns.Add("LocationDone");
            dt.Columns.Add("WorkDone");
            dt.Columns.Add("Remarks");
            dt.Columns.Add("Halt");
            dt.Columns.Add("Amt");
            dt.Columns.Add("ActStartBranch");
            dt.Columns.Add("Distance");
            dt.Columns.Add("IsSubmit");

            foreach (GridViewRow gR in gvSchdl.Rows)
            {
                txtFDt = (TextBox)gR.FindControl("txtFDt");
                txtTDt = (TextBox)gR.FindControl("txtTDt");
                txtAssignments = (TextBox)gR.FindControl("txtAssainments");
                txtRemarks = (TextBox)gR.FindControl("txtRemarks");
                txtWorkDone = (TextBox)gR.FindControl("txtWorkDone");
                ddlHalt = (DropDownList)gR.FindControl("ddlHalt");
                txtAmt = (TextBox)gR.FindControl("txtAmt");
                ddlBranch = (DropDownList)gR.FindControl("ddlBranch");
                ddlBranchDone = (DropDownList)gR.FindControl("ddlBranchDone");
                ddlActStartBranch = (DropDownList)gR.FindControl("ddlActStartBranch");
                txtDistance = (TextBox)gR.FindControl("txtDistance");

                dr = dt.NewRow();
                //dr["FDt"] = gblFuction.setDate(txtFDt.Text.Trim());
                //dr["TDt"] = gblFuction.setDate(txtTDt.Text.Trim());
                dr["FDt"] = txtFDt.Text.Trim();
                dr["TDt"] = txtTDt.Text.Trim();
                dr["Assignments"] = txtAssignments.Text.Trim();
                dr["Remarks"] = txtRemarks.Text.Trim();
                dr["Branch"] = ddlBranch.SelectedValue;
                dr["LocationDone"] = ddlBranchDone.SelectedValue;
                dr["WorkDone"] = txtWorkDone.Text.Trim();
                dr["Halt"] = ddlHalt.SelectedValue;
                dr["Amt"] = txtAmt.Text.Trim();
                dr["ActStartBranch"] = ddlActStartBranch.SelectedValue;
                dr["Distance"] = txtDistance.Text.Trim();
                dr["IsSubmit"] = chkSubmit.Checked == true ? "Y" : "N";
                dt.Rows.Add(dr);
                dt.AcceptChanges();
            }
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetXml(DataTable dt)
        {
            string vXmlData = "";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXmlData = oSW.ToString();
            }
            return vXmlData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean ValidateFields()
        {
            Boolean flag = true;
            foreach (GridViewRow gR in gvSchdl.Rows)
            {
                TextBox txtFDt, txtTDt;
                txtFDt = (TextBox)gR.FindControl("txtFDt");
                txtTDt = (TextBox)gR.FindControl("txtTDt");

                if (gblFuction.setDate(txtFDt.Text.Trim()).Month != Convert.ToInt32(ddlMonth.SelectedValue))
                {
                    flag = false;
                    gblFuction.AjxMsgPopup("Selected date in not in Selected Month");
                }
                //if (gblFuction.setDate(txtTDt.Text.Trim()).Month != Convert.ToInt32(ddlMonth.SelectedValue))
                //{
                //    flag = false;
                //    gblFuction.AjxMsgPopup("Selected date in not in Selected Month");
                //}
                if (gblFuction.setDate(txtFDt.Text.Trim()).Year != Convert.ToInt32(ddlYear.SelectedValue))
                {
                    flag = false;
                    gblFuction.AjxMsgPopup("Selected date in not in Selected Year");
                }
                //if (gblFuction.setDate(txtTDt.Text.Trim()).Year != Convert.ToInt32(ddlYear.SelectedValue))
                //{
                //    flag = false;
                //    gblFuction.AjxMsgPopup("Selected date in not in Selected Year");
                //}
                //if (DateTime.Compare(gblFuction.setDate(txtFDt.Text.Trim()), gblFuction.setDate(txtTDt.Text.Trim()))>0)
                //{
                //    flag = false;
                //    gblFuction.AjxMsgPopup("Dates are not in right order");
                //}
            }
            return flag;
        }

        public void InitGrid()
        {
            String vYear = "", vMonth = "";
            int vTotDays;
            DataRow dr = null;
            DataTable dt = null;
            if (ViewState["mTable"] == null)
            {
                dt = new DataTable();
                dt.Columns.Add("FDt");
                dt.Columns.Add("TDt");
                dt.Columns.Add("Branch");
                dt.Columns.Add("Assignments");
                dt.Columns.Add("LocationDone");
                dt.Columns.Add("WorkDone");
                dt.Columns.Add("Remarks");
                dt.Columns.Add("Halt");
                dt.Columns.Add("Amt");
                dt.Columns.Add("ActStartBranch");
                dt.Columns.Add("Distance");
                dt.Columns.Add("IsSubmit");
                vTotDays = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue));
                for (int i = 1; i <= vTotDays; i++)
                {
                    dr = dt.NewRow();
                    vYear = ddlYear.SelectedValue;
                    vMonth = Convert.ToInt32(ddlMonth.SelectedValue) < 10 ? "0" + ddlMonth.SelectedValue : ddlMonth.SelectedValue;
                    dr["FDt"] = i + "/" + vMonth + "/" + vYear;
                    dr["TDt"] = (gblFuction.setDate(i + "/" + vMonth + "/" + vYear)).DayOfWeek; //Convert.ToString(vTotDays) + "/" + vMonth + "/" + vYear;
                    dr["Branch"] = "-1";// Session[gblValue.BrnchCode].ToString();
                    dr["Assignments"] = "";
                    dr["LocationDone"] = "-1";
                    dr["WorkDone"] = "";
                    dr["Remarks"] = "";
                    dr["Halt"] = "N";
                    dr["Amt"] = "0";
                    dr["ActStartBranch"] = "-1";
                    dr["Distance"] = "0";
                    dr["IsSubmit"] = "N";
                    dt.Rows.Add(dr);
                }
                dt.AcceptChanges();
                gvSchdl.DataSource = dt;
                gvSchdl.DataBind();
                ViewState["mTable"] = dt;
                upSchdl.Update();
                chkSubmit.Checked = false;
                chkSubmit.Enabled = true;
            }
        }
        private void CalAdv()
        {
            double vTotAmt = 0;
            foreach (GridViewRow gr in gvSchdl.Rows)
            {
                TextBox txtAmt = (TextBox)gr.FindControl("txtAmt");
                vTotAmt += Convert.ToDouble(txtAmt.Text);
            }
            txtAdv.Text = vTotAmt.ToString();
            UpAdv.Update();
        }
    }
}
