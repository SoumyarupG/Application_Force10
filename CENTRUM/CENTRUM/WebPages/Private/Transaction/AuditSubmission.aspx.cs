using System;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class AuditSubmission : CENTRUMBase 
    {
        protected int vPgNo = 1;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                StatusButton("View");
                popBranch();
                LoadGrid(1);
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                tbEmp.ActiveTabIndex = 0;
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
                this.PageHeading = "Online Internal Inspection Submission";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuAuditSubmission);
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
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Online Internal Inspection Submission", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtSDt.Enabled = Status;
            txtCrStrDt.Enabled = Status;
            txtCrEndDt.Enabled = Status;
            txtPrStrDt.Enabled = Status;
            txtPrEndDt.Enabled = Status;
            ddlBranch.Enabled = Status;
            ddlQ1ChkYn.Enabled = Status;
            txtQ1Comment.Enabled = Status;
            txtQ1DtChk.Enabled = Status;
            txtQ1SysAmt.Enabled = Status;
            txtQ1PhyAmt.Enabled = Status;
            txtQ1CBAmt.Enabled = Status;
            txtQ1Score.Enabled = Status;
            ddlQ2ChkYn.Enabled = Status;
            txtQ2Comment.Enabled = Status;
            txtQ2DtChk.Enabled = Status;
            txtQ2SysAmt.Enabled = Status;
            txtQ2PhyAmt.Enabled = Status;
            txtQ2CBAmt.Enabled = Status;
            ddlQ3ChkYn.Enabled = Status;
            txtQ3Comment.Enabled = Status;
            txtQ3DtChk.Enabled = Status;
            txtQ3SysAmt.Enabled = Status;
            txtQ3PhyAmt.Enabled = Status;
            txtQ3CBAmt.Enabled = Status;
            ddlQ4ChkYn.Enabled = Status;
            txtQ4Score.Enabled = Status;
            ddlQ5Gap.Enabled = Status;
            txtQ5Score.Enabled = Status;
            ddlQ6Gap.Enabled = Status;
            txtQ6Score.Enabled = Status;
            ddlQ7ChkYn.Enabled = Status;
            txtQ7Score.Enabled = Status;
            ddlQ8ChkYn.Enabled = Status;
            txtQ8Score.Enabled = Status;
            ddlQ9Gap.Enabled = Status;
            txtQ9Score.Enabled = Status;
            ddlQ10ChkYn.Enabled = Status;
            txtQ10Score.Enabled = Status;
            ddlQ11ChkYn.Enabled = Status;
            txtQ11Score.Enabled = Status;
            ddlQ12Gap.Enabled = Status;
            txtQ12Score.Enabled = Status;
            ddlQ13Gap.Enabled = Status;
            txtQ13Score.Enabled = Status;
            ddlQ14Gap.Enabled = Status;
            txtQ14Score.Enabled = Status;
            ddlQ15Gap.Enabled = Status;
            txtQ15Score.Enabled = Status;
            ddlQ16Gap.Enabled = Status;
            txtQ16Score.Enabled = Status;
            ddlQ17ChkYn.Enabled = Status;
            txtQ17Score.Enabled = Status;
            ddlQ18ChkYn.Enabled = Status;
            txtQ18Score.Enabled = Status;
            ddlQ19ChkYn.Enabled = Status;
            txtQ19Score.Enabled = Status;
            ddlQ20Gap.Enabled = Status;
            txtQ20Score.Enabled = Status;
            ddlQ21Gap.Enabled = Status;
            txtQ21Score.Enabled = Status;
            ddlQ22Gap.Enabled = Status;
            txtQ22Score.Enabled = Status;
            ddlQ23ChkYn.Enabled = Status;
            txtQ23Score.Enabled = Status;
            ddlQ24Gap.Enabled = Status;
            txtQ24Score.Enabled = Status;
            ddlQ25ChkYn.Enabled = Status;
            txtQ25Score.Enabled = Status;
            gv26.Enabled = Status;
            gv27.Enabled = Status;
            gv28.Enabled = Status;
            gv29.Enabled = Status;
            gv30.Enabled = Status;
            gv31.Enabled = Status;
            gv32.Enabled = Status;
            gv33.Enabled = Status;
            gv34.Enabled = Status;
        }


        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtSDt.Text = Session[gblValue.LoginDate].ToString();
            txtCrStrDt.Text = "";
            txtCrEndDt.Text = "";
            txtPrStrDt.Text = "";
            txtPrEndDt.Text = "";
            ddlBranch.SelectedIndex = -1;
            ddlQ1ChkYn.SelectedIndex = -1;
            txtQ1Comment.Text = "";
            txtQ1DtChk.Text = "";
            txtQ1SysAmt.Text = "";
            txtQ1PhyAmt.Text = "";
            txtQ1CBAmt.Text = "";
            txtQ1Score.Text = "0";
            ddlQ2ChkYn.SelectedIndex = -1;
            txtQ2Comment.Text = "";
            txtQ2DtChk.Text = "";
            txtQ2SysAmt.Text = "";
            txtQ2PhyAmt.Text = "";
            txtQ2CBAmt.Text = "";
            ddlQ3ChkYn.SelectedIndex = -1;
            txtQ3Comment.Text = "";
            txtQ3DtChk.Text = "";
            txtQ3SysAmt.Text = "";
            txtQ3PhyAmt.Text = "";
            txtQ3CBAmt.Text = "";
            ddlQ4ChkYn.SelectedIndex = -1;
            txtQ4Score.Text = "0";
            ddlQ5Gap.SelectedIndex = -1;
            txtQ5Score.Text = "0";
            ddlQ6Gap.SelectedIndex = -1;
            txtQ6Score.Text = "0";
            ddlQ7ChkYn.SelectedIndex = -1;
            txtQ7Score.Text = "0";
            ddlQ8ChkYn.SelectedIndex = -1;
            txtQ8Score.Text = "0";
            ddlQ9Gap.SelectedIndex = -1;
            txtQ9Score.Text = "0";
            ddlQ10ChkYn.SelectedIndex = -1;
            txtQ10Score.Text = "0";
            ddlQ11ChkYn.SelectedIndex = -1;
            txtQ11Score.Text = "0";
            ddlQ12Gap.SelectedIndex = -1;
            txtQ12Score.Text = "0";
            ddlQ13Gap.SelectedIndex = -1;
            txtQ13Score.Text = "0";
            ddlQ14Gap.SelectedIndex = -1;
            txtQ14Score.Text = "0";
            ddlQ15Gap.SelectedIndex = -1;
            txtQ15Score.Text = "0";
            ddlQ16Gap.SelectedIndex = -1;
            txtQ16Score.Text = "0";
            ddlQ17ChkYn.SelectedIndex = -1;
            txtQ17Score.Text = "0";
            ddlQ18ChkYn.SelectedIndex = -1;
            txtQ18Score.Text = "0";
            ddlQ19ChkYn.SelectedIndex = -1;
            txtQ19Score.Text = "0";
            ddlQ20Gap.SelectedIndex = -1;
            txtQ20Score.Text = "0";
            ddlQ21Gap.SelectedIndex = -1;
            txtQ21Score.Text = "0";
            ddlQ22Gap.SelectedIndex = -1;
            txtQ22Score.Text = "";
            ddlQ23ChkYn.SelectedIndex = -1;
            txtQ23Score.Text = "0";
            ddlQ24Gap.SelectedIndex = -1;
            txtQ24Score.Text = "0";
            ddlQ25ChkYn.SelectedIndex = -1;
            txtQ25Score.Text = "0";
            txtQ26Score.Text = "0";
            txtQ27Score.Text = "0";
            txtQ28Score.Text = "0";
            txtQ29Score.Text = "0";
            txtQ30Score.Text = "0";
            txtQ31Score.Text = "0";
            txtQ32Score.Text = "0";
            txtQ33Score.Text = "0";
            txtQ34Score.Text = "0";

            lblDate.Text = "";
            lblUser.Text = "";

        }


        /// <summary>
        /// 
        /// </summary>
        private void popBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBranch.Items.Insert(0, oli);
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
        private void InitialRow26()
        {
            DataTable dt=null;
            DataRow dr = null;
            DataTable dtMkt = null;
            CAudit oAu = null;
            string vBrCode = ddlBranch.SelectedValue;
            ViewState["CenId26"] = null;
            ViewState["MemId26"] = null;
            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("SlNo", typeof(string)));
                dt.Columns.Add(new DataColumn("MarketId", typeof(string)));
                dt.Columns.Add(new DataColumn("MemberId", typeof(string)));
                dt.Columns.Add(new DataColumn("CurrYN", typeof(string)));
                dr = dt.NewRow();
                dr["SlNo"] = 1;
                dr["MarketId"] = "";
                dr["MemberId"] = "";
                dr["CurrYN"] = "";
                dt.Rows.Add(dr);
                //Store the DataTable in ViewState
                ViewState["CTable26"] = dt;
                gv26.DataSource = dt;
                gv26.DataBind();

                oAu = new CAudit();
                dtMkt = oAu.GetCenterAudit(vBrCode);
                DropDownList ddl26center = (DropDownList)gv26.Rows[0].FindControl("ddl26center");
                DropDownList ddl26Mem = (DropDownList)gv26.Rows[0].FindControl("ddl26Mem");

                ddl26center.DataSource = dtMkt;
                ddl26center.DataTextField = "Market";
                ddl26center.DataValueField = "MarketId";
                ddl26center.DataBind();

                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl26center.Items.Insert(0, oLi);
                ddl26Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddRow26()
        {
            DataTable dtCTable = null;
            DataRow drCRow = null;

            string strMem = "";
            try
            {
                if (ViewState["CTable26"] != null)
                {
                    dtCTable = (DataTable)ViewState["CTable26"];
                    
                    if (dtCTable.Rows.Count > 0)
                    {
                        drCRow = dtCTable.NewRow();
                        drCRow["SlNo"] = dtCTable.Rows.Count + 1;

                        //add new row to DataTable
                        dtCTable.Rows.Add(drCRow);
                        //Store the current data to ViewState for future reference
                        ViewState["CTable26"] = dtCTable;
                        for (int i = 0; i < dtCTable.Rows.Count - 1; i++)
                        {
                            //extract the DropDownList Selected Items
                            DropDownList ddl26center = (DropDownList)gv26.Rows[i].FindControl("ddl26center");
                            DropDownList ddl26Mem = (DropDownList)gv26.Rows[i].FindControl("ddl26Mem");
                            DropDownList ddl26Opt = (DropDownList)gv26.Rows[i].FindControl("ddl26Opt");

                            // Update the DataRow with the DDL Selected Items
                            dtCTable.Rows[i]["MarketId"] = ddl26center.SelectedValue;
                            dtCTable.Rows[i]["MemberId"] = ddl26Mem.SelectedValue;
                            dtCTable.Rows[i]["CurrYN"] = ddl26Opt.SelectedValue;

                            if (strMem == "")
                                strMem = ddl26Mem.SelectedValue;
                            else if (strMem != "")
                                strMem = strMem + "," + ddl26Mem.SelectedValue;
                        }
                        ViewState["MemId26"] = strMem;

                        //Rebind the Grid with the current data to reflect changes
                        gv26.DataSource = dtCTable;
                        gv26.DataBind();
                    }
                }

                //Set Previous Data on Postbacks
                SetPrevData26();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtCTable = null;
                drCRow = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void SetPrevData26()
        {
            int rowIndex = 0;
            DataTable dt = null;
            CAudit oAu = null;
            DataTable dtMkt=null;
            DataTable dtMem = null;
            DataRow dr = null;
            DataRow drMem = null;
            string vBrCode = ddlBranch.SelectedValue;
            //string strMem = (string)ViewState["MemId26"];
            try
            {
                if (ViewState["CTable26"] != null)
                {
                    dt = (DataTable)ViewState["CTable26"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DropDownList ddl26center = (DropDownList)gv26.Rows[i].FindControl("ddl26center");
                            DropDownList ddl26Mem = (DropDownList)gv26.Rows[i].FindControl("ddl26Mem");
                            DropDownList ddl26Opt = (DropDownList)gv26.Rows[i].FindControl("ddl26Opt");


                            //if (ViewState["MemId26"] == null)
                            //{
                            //    if (strMem == "" || strMem == null)
                            //        strMem = dt.Rows[i]["MemberId"].ToString();
                            //    else if (strMem != "")
                            //        strMem = strMem + "," + dt.Rows[i]["MemberId"].ToString();
                            //}

                            oAu = new CAudit();
                            dtMkt = oAu.GetCenterAudit(vBrCode);

                            dr = dtMkt.NewRow();
                            dr["MarketId"] = "";
                            dr["Market"] = string.Empty;
                            dtMkt.Rows.InsertAt(dr, dtMkt.Rows.Count + 1);
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["MarketId"] = dtMkt.Rows[0]["MarketId"];
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["Market"] = dtMkt.Rows[0]["Market"];
                            dtMkt.Rows[0]["MarketId"] = "-1";
                            dtMkt.Rows[0]["Market"] = "<--Select-->";
                            ddl26center.DataSource = dtMkt;
                            ddl26center.DataTextField = "Market";
                            ddl26center.DataValueField = "MarketId";
                            ddl26center.DataBind();

                            ListItem oLi = new ListItem("<--Select-->", "-1");
                            ddl26Mem.Items.Insert(0, oLi);

                            if (i < dt.Rows.Count - 1)
                            {
                                dtMem = oAu.GetMemberByMktAudit(Convert.ToString(dt.Rows[i]["MarketId"]), vBrCode, "");
                                drMem = dtMem.NewRow();
                                drMem["MemberID"] = 0;
                                drMem["Member"] = string.Empty;
                                dtMem.Rows.InsertAt(drMem, dtMem.Rows.Count + 1);
                                dtMem.Rows[dtMem.Rows.Count - 1]["MemberID"] = dtMem.Rows[0]["MemberID"];
                                dtMem.Rows[dtMem.Rows.Count - 1]["Member"] = dtMem.Rows[0]["Member"];
                                dtMem.Rows[0]["MemberID"] = "-1";
                                dtMem.Rows[0]["Member"] = "<--Select-->";

                                ddl26Mem.DataSource = dtMem;
                                ddl26Mem.DataTextField = "Member";
                                ddl26Mem.DataValueField = "MemberID";
                                ddl26Mem.DataBind();
                                //Set the Previous Selected Items on Each DropDownList  on Postbacks
                                ddl26center.ClearSelection();
                                ddl26center.SelectedIndex = ddl26center.Items.IndexOf(ddl26center.Items.FindByValue(dt.Rows[i]["MarketId"].ToString()));//.Selected = true;

                                ddl26Mem.ClearSelection();
                                ddl26Mem.SelectedIndex = ddl26Mem.Items.IndexOf(ddl26Mem.Items.FindByValue(dt.Rows[i]["MemberID"].ToString()));

                                ddl26Opt.ClearSelection();
                                ddl26Opt.SelectedIndex = ddl26Opt.Items.IndexOf(ddl26Opt.Items.FindByValue(dt.Rows[i]["CurrYN"].ToString()));

                            }
                            rowIndex++;
                        }
                    }
                    ViewState["CTable26"] = dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dtMem = null;
                dr = null;
                drMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl26center_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CAudit oAu = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string strMem = (string)ViewState["MemId26"];
            if (strMem == null) strMem = "";
            try
            {
                oAu = new CAudit();
                DropDownList ddlCent = (DropDownList)sender;
                GridViewRow gvr = (GridViewRow)ddlCent.NamingContainer;
                int rowindex = gvr.RowIndex;

                DropDownList ddl26center = (DropDownList)gv26.Rows[rowindex].FindControl("ddl26center");
                DropDownList ddl26Mem = (DropDownList)gv26.Rows[rowindex].FindControl("ddl26Mem");

                dt = oAu.GetMemberByMktAudit(ddl26center.SelectedValue, vBrCode, strMem);
                ddl26Mem.DataSource = dt;
                ddl26Mem.DataTextField = "Member";
                ddl26Mem.DataValueField = "MemberID";
                ddl26Mem.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl26Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl26Opt_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowindex = 0, vMaxRow = 0;
            double vScore = 0;
            DropDownList ddlOpt = (DropDownList)sender;
            GridViewRow gvr = (GridViewRow)ddlOpt.NamingContainer;

            rowindex = gvr.RowIndex;
            vMaxRow = gv26.Rows.Count;

            DropDownList ddl26center = (DropDownList)gv26.Rows[rowindex].FindControl("ddl26center");
            DropDownList ddl26Mem = (DropDownList)gv26.Rows[rowindex].FindControl("ddl26Mem");
            DropDownList ddl26Opt = (DropDownList)gv26.Rows[rowindex].FindControl("ddl26Opt");

            if (rowindex == vMaxRow - 1)
            {
                if (ddl26center.SelectedIndex > 0 && ddl26Mem.SelectedIndex > 0 && ddl26Opt.SelectedIndex >0)
                {
                    AddRow26();
                    DataTable dt = (DataTable)ViewState["CTable26"];
                    DataRow[] drs = dt.Select("(CurrYN='Y')");
                    //make a new "results" datatable via clone to keep structure
                    DataTable dt2 = dt.Clone();
                    foreach (DataRow d in drs)
                    {
                        dt2.ImportRow(d);
                    }
                    if (dt.Rows.Count > 1 && dt2.Rows.Count > 0)
                    {
                        vScore = (Convert.ToDouble(dt2.Rows.Count) * 100) / Convert.ToDouble(dt.Rows.Count-1);
                        if (vScore>=80)
                            txtQ26Score.Text = "5";
                        else if (vScore>=60 && vScore<80)
                            txtQ26Score.Text = "3";
                        else if (vScore < 60)
                            txtQ26Score.Text = "1";
                    }
                    else
                        txtQ26Score.Text = "0";

                }
                else
                {
                    gblFuction.AjxMsgPopup("Please feed all the Details");
                    ddl26Opt.Focus();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitialRow27()
        {
            DataTable dt = null;
            DataRow dr = null;
            DataTable dtMkt = null;
            CAudit oAu = null;
            string vBrCode = ddlBranch.SelectedValue;
            ViewState["CenId27"] = null;
            ViewState["MemId27"] = null;
            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("SlNo", typeof(string)));
                dt.Columns.Add(new DataColumn("MarketId", typeof(string)));
                dt.Columns.Add(new DataColumn("MemberId", typeof(string)));
                dt.Columns.Add(new DataColumn("CurrYN", typeof(string)));
                dr = dt.NewRow();
                dr["SlNo"] = 1;
                dr["MarketId"] = "";
                dr["MemberId"] = "";
                dr["CurrYN"] = "";
                dt.Rows.Add(dr);
                //Store the DataTable in ViewState
                ViewState["CTable27"] = dt;
                gv27.DataSource = dt;
                gv27.DataBind();

                oAu = new CAudit();
                dtMkt = oAu.GetCenterAudit(vBrCode);
                DropDownList ddl27center = (DropDownList)gv27.Rows[0].FindControl("ddl27center");
                DropDownList ddl27Mem = (DropDownList)gv27.Rows[0].FindControl("ddl27Mem");

                ddl27center.DataSource = dtMkt;
                ddl27center.DataTextField = "Market";
                ddl27center.DataValueField = "MarketId";
                ddl27center.DataBind();

                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl27center.Items.Insert(0, oLi);
                ddl27Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddRow27()
        {
            DataTable dtCTable = null;
            DataRow drCRow = null;
            string strMem = "";
            try
            {
                if (ViewState["CTable27"] != null)
                {
                    dtCTable = (DataTable)ViewState["CTable27"];

                    if (dtCTable.Rows.Count > 0)
                    {
                        drCRow = dtCTable.NewRow();
                        drCRow["SlNo"] = dtCTable.Rows.Count + 1;

                        //add new row to DataTable
                        dtCTable.Rows.Add(drCRow);
                        //Store the current data to ViewState for future reference
                        ViewState["CTable27"] = dtCTable;
                        for (int i = 0; i < dtCTable.Rows.Count - 1; i++)
                        {
                            //extract the DropDownList Selected Items
                            DropDownList ddl27center = (DropDownList)gv27.Rows[i].FindControl("ddl27center");
                            DropDownList ddl27Mem = (DropDownList)gv27.Rows[i].FindControl("ddl27Mem");
                            DropDownList ddl27Opt = (DropDownList)gv27.Rows[i].FindControl("ddl27Opt");

                            // Update the DataRow with the DDL Selected Items
                            dtCTable.Rows[i]["MarketId"] = ddl27center.SelectedValue;
                            dtCTable.Rows[i]["MemberId"] = ddl27Mem.SelectedValue;
                            dtCTable.Rows[i]["CurrYN"] = ddl27Opt.SelectedValue;

                            if (strMem == "")
                                strMem = ddl27Mem.SelectedValue;
                            else if (strMem != "")
                                strMem = strMem + "," + ddl27Mem.SelectedValue;
                        }
                        ViewState["MemId27"] = strMem;
                        //Rebind the Grid with the current data to reflect changes
                        gv27.DataSource = dtCTable;
                        gv27.DataBind();
                    }
                }

                //Set Previous Data on Postbacks
                SetPrevData27();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtCTable = null;
                drCRow = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void SetPrevData27()
        {
            int rowIndex = 0;
            DataTable dt = null;
            CAudit oAu = null;
            DataTable dtMkt = null;
            DataTable dtMem = null;
            DataRow dr = null;
            DataRow drMem = null;
            string vBrCode = ddlBranch.SelectedValue;
            //string strMem = (string)ViewState["MemId27"];
            try
            {
                if (ViewState["CTable27"] != null)
                {
                    dt = (DataTable)ViewState["CTable27"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DropDownList ddl27center = (DropDownList)gv27.Rows[i].FindControl("ddl27center");
                            DropDownList ddl27Mem = (DropDownList)gv27.Rows[i].FindControl("ddl27Mem");
                            DropDownList ddl27Opt = (DropDownList)gv27.Rows[i].FindControl("ddl27Opt");

                            //if (ViewState["MemId27"] == null)
                            //{
                            //    if (strMem == "" || strMem == null)
                            //        strMem = dt.Rows[i]["MemberId"].ToString();
                            //    else if (strMem != "")
                            //        strMem = strMem + "," + dt.Rows[i]["MemberId"].ToString();
                            //}

                            oAu = new CAudit();
                            dtMkt = oAu.GetCenterAudit(vBrCode);

                            dr = dtMkt.NewRow();
                            dr["MarketId"] = "";
                            dr["Market"] = string.Empty;
                            dtMkt.Rows.InsertAt(dr, dtMkt.Rows.Count + 1);
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["MarketId"] = dtMkt.Rows[0]["MarketId"];
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["Market"] = dtMkt.Rows[0]["Market"];
                            dtMkt.Rows[0]["MarketId"] = "-1";
                            dtMkt.Rows[0]["Market"] = "<--Select-->";
                            ddl27center.DataSource = dtMkt;
                            ddl27center.DataTextField = "Market";
                            ddl27center.DataValueField = "MarketId";
                            ddl27center.DataBind();

                            ListItem oLi = new ListItem("<--Select-->", "-1");
                            ddl27Mem.Items.Insert(0, oLi);

                            if (i < dt.Rows.Count - 1)
                            {
                                dtMem = oAu.GetMemberByMktAudit(Convert.ToString(dt.Rows[i]["MarketId"]), vBrCode, "");
                                drMem = dtMem.NewRow();
                                drMem["MemberID"] = 0;
                                drMem["Member"] = string.Empty;
                                dtMem.Rows.InsertAt(drMem, dtMem.Rows.Count + 1);
                                dtMem.Rows[dtMem.Rows.Count - 1]["MemberID"] = dtMem.Rows[0]["MemberID"];
                                dtMem.Rows[dtMem.Rows.Count - 1]["Member"] = dtMem.Rows[0]["Member"];
                                dtMem.Rows[0]["MemberID"] = "-1";
                                dtMem.Rows[0]["Member"] = "<--Select-->";

                                ddl27Mem.DataSource = dtMem;
                                ddl27Mem.DataTextField = "Member";
                                ddl27Mem.DataValueField = "MemberID";
                                ddl27Mem.DataBind();
                                //Set the Previous Selected Items on Each DropDownList  on Postbacks
                                ddl27center.ClearSelection();
                                ddl27center.SelectedIndex = ddl27center.Items.IndexOf(ddl27center.Items.FindByValue(dt.Rows[i]["MarketId"].ToString()));//.Selected = true;

                                ddl27Mem.ClearSelection();
                                ddl27Mem.SelectedIndex = ddl27Mem.Items.IndexOf(ddl27Mem.Items.FindByValue(dt.Rows[i]["MemberID"].ToString()));

                                ddl27Opt.ClearSelection();
                                ddl27Opt.SelectedIndex = ddl27Opt.Items.IndexOf(ddl27Opt.Items.FindByValue(dt.Rows[i]["CurrYN"].ToString()));

                            }
                            rowIndex++;
                        }
                    }
                    ViewState["CTable27"] = dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dtMem = null;
                dr = null;
                drMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl27center_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CAudit oAu = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string strMem = (string)ViewState["MemId27"];
            if (strMem == null) strMem = "";
            try
            {
                oAu = new CAudit();
                DropDownList ddlCent = (DropDownList)sender;
                GridViewRow gvr = (GridViewRow)ddlCent.NamingContainer;
                int rowindex = gvr.RowIndex;

                DropDownList ddl27center = (DropDownList)gv27.Rows[rowindex].FindControl("ddl27center");
                DropDownList ddl27Mem = (DropDownList)gv27.Rows[rowindex].FindControl("ddl27Mem");

                dt = oAu.GetMemberByMktAudit(ddl27center.SelectedValue, vBrCode,strMem);
                ddl27Mem.DataSource = dt;
                ddl27Mem.DataTextField = "Member";
                ddl27Mem.DataValueField = "MemberID";
                ddl27Mem.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl27Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl27Opt_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowindex = 0, vMaxRow = 0;
            double vScore = 0;
            DropDownList ddlOpt = (DropDownList)sender;
            GridViewRow gvr = (GridViewRow)ddlOpt.NamingContainer;

            rowindex = gvr.RowIndex;
            vMaxRow = gv27.Rows.Count;

            DropDownList ddl27center = (DropDownList)gv27.Rows[rowindex].FindControl("ddl27center");
            DropDownList ddl27Mem = (DropDownList)gv27.Rows[rowindex].FindControl("ddl27Mem");
            DropDownList ddl27Opt = (DropDownList)gv27.Rows[rowindex].FindControl("ddl27Opt");

            if (rowindex == vMaxRow - 1)
            {
                if (ddl27center.SelectedIndex > 0 && ddl27Mem.SelectedIndex > 0 && ddl27Opt.SelectedIndex > 0)
                {
                    AddRow27();
                    DataTable dt = (DataTable)ViewState["CTable27"];
                    DataRow[] drs = dt.Select("(CurrYN='Y')");
                    //make a new "results" datatable via clone to keep structure
                    DataTable dt2 = dt.Clone();
                    foreach (DataRow d in drs)
                    {
                        dt2.ImportRow(d);
                    }
                    if (dt.Rows.Count > 1 && dt2.Rows.Count > 0)
                    {
                        vScore = (Convert.ToDouble(dt2.Rows.Count) * 100) / Convert.ToDouble(dt.Rows.Count - 1);
                        if (vScore >= 80)
                            txtQ27Score.Text = "5";
                        else if (vScore >= 60 && vScore < 80)
                            txtQ27Score.Text = "3";
                        else if (vScore < 60)
                            txtQ27Score.Text = "1";
                    }
                    else
                        txtQ27Score.Text = "0";
                }
                else
                {
                    gblFuction.AjxMsgPopup("Please feed all the Details");
                    ddl27Opt.Focus();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitialRow28()
        {
            DataTable dt = null;
            DataRow dr = null;
            DataTable dtMkt = null;
            CAudit oAu = null;
            string vBrCode = ddlBranch.SelectedValue;
            ViewState["CenId28"] = null;
            ViewState["MemId28"] = null;
            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("SlNo", typeof(string)));
                dt.Columns.Add(new DataColumn("MarketId", typeof(string)));
                dt.Columns.Add(new DataColumn("MemberId", typeof(string)));
                dt.Columns.Add(new DataColumn("CurrYN", typeof(string)));
                dr = dt.NewRow();
                dr["SlNo"] = 1;
                dr["MarketId"] = "";
                dr["MemberId"] = "";
                dr["CurrYN"] = "";
                dt.Rows.Add(dr);
                //Store the DataTable in ViewState
                ViewState["CTable28"] = dt;
                gv28.DataSource = dt;
                gv28.DataBind();

                oAu = new CAudit();
                dtMkt = oAu.GetCenterAudit(vBrCode);
                DropDownList ddl28center = (DropDownList)gv28.Rows[0].FindControl("ddl28center");
                DropDownList ddl28Mem = (DropDownList)gv28.Rows[0].FindControl("ddl28Mem");

                ddl28center.DataSource = dtMkt;
                ddl28center.DataTextField = "Market";
                ddl28center.DataValueField = "MarketId";
                ddl28center.DataBind();

                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl28center.Items.Insert(0, oLi);
                ddl28Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddRow28()
        {
            DataTable dtCTable = null;
            DataRow drCRow = null;
            string strMem = "";
            try
            {
                if (ViewState["CTable28"] != null)
                {
                    dtCTable = (DataTable)ViewState["CTable28"];

                    if (dtCTable.Rows.Count > 0)
                    {
                        drCRow = dtCTable.NewRow();
                        drCRow["SlNo"] = dtCTable.Rows.Count + 1;

                        //add new row to DataTable
                        dtCTable.Rows.Add(drCRow);
                        //Store the current data to ViewState for future reference
                        ViewState["CTable28"] = dtCTable;
                        for (int i = 0; i < dtCTable.Rows.Count - 1; i++)
                        {
                            //extract the DropDownList Selected Items
                            DropDownList ddl28center = (DropDownList)gv28.Rows[i].FindControl("ddl28center");
                            DropDownList ddl28Mem = (DropDownList)gv28.Rows[i].FindControl("ddl28Mem");
                            DropDownList ddl28Opt = (DropDownList)gv28.Rows[i].FindControl("ddl28Opt");

                            // Update the DataRow with the DDL Selected Items
                            dtCTable.Rows[i]["MarketId"] = ddl28center.SelectedValue;
                            dtCTable.Rows[i]["MemberId"] = ddl28Mem.SelectedValue;
                            dtCTable.Rows[i]["CurrYN"] = ddl28Opt.SelectedValue;

                            if (strMem == "")
                                strMem = ddl28Mem.SelectedValue;
                            else if (strMem != "")
                                strMem = strMem + "," + ddl28Mem.SelectedValue;
                        }

                        ViewState["MemId28"] = strMem;
                        //Rebind the Grid with the current data to reflect changes
                        gv28.DataSource = dtCTable;
                        gv28.DataBind();
                    }
                }

                //Set Previous Data on Postbacks
                SetPrevData28();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtCTable = null;
                drCRow = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void SetPrevData28()
        {
            int rowIndex = 0;
            DataTable dt = null;
            CAudit oAu = null;
            DataTable dtMkt = null;
            DataTable dtMem = null;
            DataRow dr = null;
            DataRow drMem = null;
            string vBrCode = ddlBranch.SelectedValue;
            //string strMem = (string)ViewState["MemId28"];
            try
            {
                if (ViewState["CTable28"] != null)
                {
                    dt = (DataTable)ViewState["CTable28"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DropDownList ddl28center = (DropDownList)gv28.Rows[i].FindControl("ddl28center");
                            DropDownList ddl28Mem = (DropDownList)gv28.Rows[i].FindControl("ddl28Mem");
                            DropDownList ddl28Opt = (DropDownList)gv28.Rows[i].FindControl("ddl28Opt");

                            //if (ViewState["MemId28"] == null)
                            //{
                            //    if (strMem == "" || strMem == null)
                            //        strMem = dt.Rows[i]["MemberId"].ToString();
                            //    else if (strMem != "")
                            //        strMem = strMem + "," + dt.Rows[i]["MemberId"].ToString();
                            //}
                            oAu = new CAudit();
                            dtMkt = oAu.GetCenterAudit(vBrCode);

                            dr = dtMkt.NewRow();
                            dr["MarketId"] = "";
                            dr["Market"] = string.Empty;
                            dtMkt.Rows.InsertAt(dr, dtMkt.Rows.Count + 1);
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["MarketId"] = dtMkt.Rows[0]["MarketId"];
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["Market"] = dtMkt.Rows[0]["Market"];
                            dtMkt.Rows[0]["MarketId"] = "-1";
                            dtMkt.Rows[0]["Market"] = "<--Select-->";
                            ddl28center.DataSource = dtMkt;
                            ddl28center.DataTextField = "Market";
                            ddl28center.DataValueField = "MarketId";
                            ddl28center.DataBind();

                            ListItem oLi = new ListItem("<--Select-->", "-1");
                            ddl28Mem.Items.Insert(0, oLi);

                            if (i < dt.Rows.Count - 1)
                            {
                                dtMem = oAu.GetMemberByMktAudit(Convert.ToString(dt.Rows[i]["MarketId"]), vBrCode, "");
                                drMem = dtMem.NewRow();
                                drMem["MemberID"] = 0;
                                drMem["Member"] = string.Empty;
                                dtMem.Rows.InsertAt(drMem, dtMem.Rows.Count + 1);
                                dtMem.Rows[dtMem.Rows.Count - 1]["MemberID"] = dtMem.Rows[0]["MemberID"];
                                dtMem.Rows[dtMem.Rows.Count - 1]["Member"] = dtMem.Rows[0]["Member"];
                                dtMem.Rows[0]["MemberID"] = "-1";
                                dtMem.Rows[0]["Member"] = "<--Select-->";

                                ddl28Mem.DataSource = dtMem;
                                ddl28Mem.DataTextField = "Member";
                                ddl28Mem.DataValueField = "MemberID";
                                ddl28Mem.DataBind();
                                //Set the Previous Selected Items on Each DropDownList  on Postbacks
                                ddl28center.ClearSelection();
                                ddl28center.SelectedIndex = ddl28center.Items.IndexOf(ddl28center.Items.FindByValue(dt.Rows[i]["MarketId"].ToString()));//.Selected = true;

                                ddl28Mem.ClearSelection();
                                ddl28Mem.SelectedIndex = ddl28Mem.Items.IndexOf(ddl28Mem.Items.FindByValue(dt.Rows[i]["MemberID"].ToString()));

                                ddl28Opt.ClearSelection();
                                ddl28Opt.SelectedIndex = ddl28Opt.Items.IndexOf(ddl28Opt.Items.FindByValue(dt.Rows[i]["CurrYN"].ToString()));

                            }
                            rowIndex++;
                        }
                    }
                    ViewState["CTable28"] = dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dtMem = null;
                dr = null;
                drMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl28center_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CAudit oAu = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string strMem = (string)ViewState["MemId28"];
            if (strMem == null) strMem = "";
            try
            {
                oAu = new CAudit();
                DropDownList ddlCent = (DropDownList)sender;
                GridViewRow gvr = (GridViewRow)ddlCent.NamingContainer;
                int rowindex = gvr.RowIndex;

                DropDownList ddl28center = (DropDownList)gv28.Rows[rowindex].FindControl("ddl28center");
                DropDownList ddl28Mem = (DropDownList)gv28.Rows[rowindex].FindControl("ddl28Mem");

                dt = oAu.GetMemberByMktAudit(ddl28center.SelectedValue, vBrCode,strMem);
                ddl28Mem.DataSource = dt;
                ddl28Mem.DataTextField = "Member";
                ddl28Mem.DataValueField = "MemberID";
                ddl28Mem.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl28Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl28Opt_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowindex = 0, vMaxRow = 0;
            double vScore = 0;
            DropDownList ddlOpt = (DropDownList)sender;
            GridViewRow gvr = (GridViewRow)ddlOpt.NamingContainer;

            rowindex = gvr.RowIndex;
            vMaxRow = gv28.Rows.Count;

            DropDownList ddl28center = (DropDownList)gv28.Rows[rowindex].FindControl("ddl28center");
            DropDownList ddl28Mem = (DropDownList)gv28.Rows[rowindex].FindControl("ddl28Mem");
            DropDownList ddl28Opt = (DropDownList)gv28.Rows[rowindex].FindControl("ddl28Opt");

            if (rowindex == vMaxRow - 1)
            {
                if (ddl28center.SelectedIndex > 0 && ddl28Mem.SelectedIndex > 0 && ddl28Opt.SelectedIndex > 0)
                {
                    AddRow28();
                    DataTable dt = (DataTable)ViewState["CTable28"];
                    DataRow[] drs = dt.Select("(CurrYN='Y')");
                    //make a new "results" datatable via clone to keep structure
                    DataTable dt2 = dt.Clone();
                    foreach (DataRow d in drs)
                    {
                        dt2.ImportRow(d);
                    }
                    if (dt.Rows.Count > 1 && dt2.Rows.Count > 0)
                    {
                        vScore = (Convert.ToDouble(dt2.Rows.Count) * 100) / Convert.ToDouble(dt.Rows.Count - 1);
                        if (vScore >= 80)
                            txtQ28Score.Text = "5";
                        else if (vScore >= 60 && vScore < 80)
                            txtQ28Score.Text = "3";
                        else if (vScore < 60)
                            txtQ28Score.Text = "1";
                    }
                    else
                        txtQ28Score.Text = "0";
                }
                else
                {
                    gblFuction.AjxMsgPopup("Please feed all the Details");
                    ddl28Opt.Focus();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitialRow29()
        {
            DataTable dt = null;
            DataRow dr = null;
            DataTable dtMkt = null;
            CAudit oAu = null;
            string vBrCode = ddlBranch.SelectedValue;
            ViewState["CenId29"] = null;
            ViewState["MemId29"] = null;
            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("SlNo", typeof(string)));
                dt.Columns.Add(new DataColumn("MarketId", typeof(string)));
                dt.Columns.Add(new DataColumn("MemberId", typeof(string)));
                dt.Columns.Add(new DataColumn("CurrYN", typeof(string)));
                dr = dt.NewRow();
                dr["SlNo"] = 1;
                dr["MarketId"] = "";
                dr["MemberId"] = "";
                dr["CurrYN"] = "";
                dt.Rows.Add(dr);
                //Store the DataTable in ViewState
                ViewState["CTable29"] = dt;
                gv29.DataSource = dt;
                gv29.DataBind();

                oAu = new CAudit();
                dtMkt = oAu.GetCenterAudit(vBrCode);
                DropDownList ddl29center = (DropDownList)gv29.Rows[0].FindControl("ddl29center");
                DropDownList ddl29Mem = (DropDownList)gv29.Rows[0].FindControl("ddl29Mem");

                ddl29center.DataSource = dtMkt;
                ddl29center.DataTextField = "Market";
                ddl29center.DataValueField = "MarketId";
                ddl29center.DataBind();

                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl29center.Items.Insert(0, oLi);
                ddl29Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddRow29()
        {
            DataTable dtCTable = null;
            DataRow drCRow = null;
            string strMem = "";
            try
            {
                if (ViewState["CTable29"] != null)
                {
                    dtCTable = (DataTable)ViewState["CTable29"];

                    if (dtCTable.Rows.Count > 0)
                    {
                        drCRow = dtCTable.NewRow();
                        drCRow["SlNo"] = dtCTable.Rows.Count + 1;

                        //add new row to DataTable
                        dtCTable.Rows.Add(drCRow);
                        //Store the current data to ViewState for future reference
                        ViewState["CTable29"] = dtCTable;
                        for (int i = 0; i < dtCTable.Rows.Count - 1; i++)
                        {
                            //extract the DropDownList Selected Items
                            DropDownList ddl29center = (DropDownList)gv29.Rows[i].FindControl("ddl29center");
                            DropDownList ddl29Mem = (DropDownList)gv29.Rows[i].FindControl("ddl29Mem");
                            DropDownList ddl29Opt = (DropDownList)gv29.Rows[i].FindControl("ddl29Opt");

                            // Update the DataRow with the DDL Selected Items
                            dtCTable.Rows[i]["MarketId"] = ddl29center.SelectedValue;
                            dtCTable.Rows[i]["MemberId"] = ddl29Mem.SelectedValue;
                            dtCTable.Rows[i]["CurrYN"] = ddl29Opt.SelectedValue;

                            if (strMem == "")
                                strMem = ddl29Mem.SelectedValue;
                            else if (strMem != "")
                                strMem = strMem + "," + ddl29Mem.SelectedValue;
                        }

                        ViewState["MemId29"] = strMem;
                        //Rebind the Grid with the current data to reflect changes
                        gv29.DataSource = dtCTable;
                        gv29.DataBind();
                    }
                }

                //Set Previous Data on Postbacks
                SetPrevData29();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtCTable = null;
                drCRow = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void SetPrevData29()
        {
            int rowIndex = 0;
            DataTable dt = null;
            CAudit oAu = null;
            DataTable dtMkt = null;
            DataTable dtMem = null;
            DataRow dr = null;
            DataRow drMem = null;
            string vBrCode = ddlBranch.SelectedValue;
            //string strMem = (string)ViewState["MemId29"];
            try
            {
                if (ViewState["CTable29"] != null)
                {
                    dt = (DataTable)ViewState["CTable29"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DropDownList ddl29center = (DropDownList)gv29.Rows[i].FindControl("ddl29center");
                            DropDownList ddl29Mem = (DropDownList)gv29.Rows[i].FindControl("ddl29Mem");
                            DropDownList ddl29Opt = (DropDownList)gv29.Rows[i].FindControl("ddl29Opt");

                            //if (ViewState["MemId29"] == null)
                            //{
                            //    if (strMem == "" || strMem == null)
                            //        strMem = dt.Rows[i]["MemberId"].ToString();
                            //    else if (strMem != "")
                            //        strMem = strMem + "," + dt.Rows[i]["MemberId"].ToString();
                            //}
                            oAu = new CAudit();
                            dtMkt = oAu.GetCenterAudit(vBrCode);

                            dr = dtMkt.NewRow();
                            dr["MarketId"] = "";
                            dr["Market"] = string.Empty;
                            dtMkt.Rows.InsertAt(dr, dtMkt.Rows.Count + 1);
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["MarketId"] = dtMkt.Rows[0]["MarketId"];
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["Market"] = dtMkt.Rows[0]["Market"];
                            dtMkt.Rows[0]["MarketId"] = "-1";
                            dtMkt.Rows[0]["Market"] = "<--Select-->";
                            ddl29center.DataSource = dtMkt;
                            ddl29center.DataTextField = "Market";
                            ddl29center.DataValueField = "MarketId";
                            ddl29center.DataBind();

                            ListItem oLi = new ListItem("<--Select-->", "-1");
                            ddl29Mem.Items.Insert(0, oLi);

                            if (i < dt.Rows.Count - 1)
                            {
                                dtMem = oAu.GetMemberByMktAudit(Convert.ToString(dt.Rows[i]["MarketId"]), vBrCode, "" );
                                drMem = dtMem.NewRow();
                                drMem["MemberID"] = 0;
                                drMem["Member"] = string.Empty;
                                dtMem.Rows.InsertAt(drMem, dtMem.Rows.Count + 1);
                                dtMem.Rows[dtMem.Rows.Count - 1]["MemberID"] = dtMem.Rows[0]["MemberID"];
                                dtMem.Rows[dtMem.Rows.Count - 1]["Member"] = dtMem.Rows[0]["Member"];
                                dtMem.Rows[0]["MemberID"] = "-1";
                                dtMem.Rows[0]["Member"] = "<--Select-->";

                                ddl29Mem.DataSource = dtMem;
                                ddl29Mem.DataTextField = "Member";
                                ddl29Mem.DataValueField = "MemberID";
                                ddl29Mem.DataBind();
                                //Set the Previous Selected Items on Each DropDownList  on Postbacks
                                ddl29center.ClearSelection();
                                ddl29center.SelectedIndex = ddl29center.Items.IndexOf(ddl29center.Items.FindByValue(dt.Rows[i]["MarketId"].ToString()));//.Selected = true;

                                ddl29Mem.ClearSelection();
                                ddl29Mem.SelectedIndex = ddl29Mem.Items.IndexOf(ddl29Mem.Items.FindByValue(dt.Rows[i]["MemberID"].ToString()));

                                ddl29Opt.ClearSelection();
                                ddl29Opt.SelectedIndex = ddl29Opt.Items.IndexOf(ddl29Opt.Items.FindByValue(dt.Rows[i]["CurrYN"].ToString()));

                            }
                            rowIndex++;
                        }
                    }
                    ViewState["CTable29"] = dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dtMem = null;
                dr = null;
                drMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl29center_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CAudit oAu = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string strMem = (string)ViewState["MemId29"];
            if (strMem == null) strMem = "";
            try
            {
                oAu = new CAudit();
                DropDownList ddlCent = (DropDownList)sender;
                GridViewRow gvr = (GridViewRow)ddlCent.NamingContainer;
                int rowindex = gvr.RowIndex;

                DropDownList ddl29center = (DropDownList)gv29.Rows[rowindex].FindControl("ddl29center");
                DropDownList ddl29Mem = (DropDownList)gv29.Rows[rowindex].FindControl("ddl29Mem");

                dt = oAu.GetMemberByMktAudit(ddl29center.SelectedValue, vBrCode,strMem);
                ddl29Mem.DataSource = dt;
                ddl29Mem.DataTextField = "Member";
                ddl29Mem.DataValueField = "MemberID";
                ddl29Mem.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl29Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl29Opt_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowindex = 0, vMaxRow = 0;
            double vScore = 0;
            DropDownList ddlOpt = (DropDownList)sender;
            GridViewRow gvr = (GridViewRow)ddlOpt.NamingContainer;

            rowindex = gvr.RowIndex;
            vMaxRow = gv29.Rows.Count;

            DropDownList ddl29center = (DropDownList)gv29.Rows[rowindex].FindControl("ddl29center");
            DropDownList ddl29Mem = (DropDownList)gv29.Rows[rowindex].FindControl("ddl29Mem");
            DropDownList ddl29Opt = (DropDownList)gv29.Rows[rowindex].FindControl("ddl29Opt");

            if (rowindex == vMaxRow - 1)
            {
                if (ddl29center.SelectedIndex > 0 && ddl29Mem.SelectedIndex > 0 && ddl29Opt.SelectedIndex > 0)
                {
                    AddRow29();
                    DataTable dt = (DataTable)ViewState["CTable29"];
                    DataRow[] drs = dt.Select("(CurrYN='Y')");
                    //make a new "results" datatable via clone to keep structure
                    DataTable dt2 = dt.Clone();
                    foreach (DataRow d in drs)
                    {
                        dt2.ImportRow(d);
                    }
                    if (dt.Rows.Count > 1 && dt2.Rows.Count > 0)
                    {
                        vScore = (Convert.ToDouble(dt2.Rows.Count) * 100) / Convert.ToDouble(dt.Rows.Count - 1);
                        if (vScore >= 80)
                            txtQ29Score.Text = "5";
                        else if (vScore >= 60 && vScore < 80)
                            txtQ29Score.Text = "3";
                        else if (vScore < 60)
                            txtQ29Score.Text = "1";
                    }
                    else
                        txtQ29Score.Text = "0";
                }
                else
                {
                    gblFuction.AjxMsgPopup("Please feed all the Details");
                    ddl29Opt.Focus();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitialRow30()
        {
            DataTable dt = null;
            DataRow dr = null;
            DataTable dtMkt = null;
            CAudit oAu = null;
            string vBrCode = ddlBranch.SelectedValue;
            ViewState["CenId30"] = null;
            ViewState["MemId30"] = null;
            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("SlNo", typeof(string)));
                dt.Columns.Add(new DataColumn("MarketId", typeof(string)));
                dt.Columns.Add(new DataColumn("MemberId", typeof(string)));
                dt.Columns.Add(new DataColumn("CurrYN", typeof(string)));
                dr = dt.NewRow();
                dr["SlNo"] = 1;
                dr["MarketId"] = "";
                dr["MemberId"] = "";
                dr["CurrYN"] = "";
                dt.Rows.Add(dr);
                //Store the DataTable in ViewState
                ViewState["CTable30"] = dt;
                gv30.DataSource = dt;
                gv30.DataBind();

                oAu = new CAudit();
                dtMkt = oAu.GetCenterAudit(vBrCode);
                DropDownList ddl30center = (DropDownList)gv30.Rows[0].FindControl("ddl30center");
                DropDownList ddl30Mem = (DropDownList)gv30.Rows[0].FindControl("ddl30Mem");

                ddl30center.DataSource = dtMkt;
                ddl30center.DataTextField = "Market";
                ddl30center.DataValueField = "MarketId";
                ddl30center.DataBind();

                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl30center.Items.Insert(0, oLi);
                ddl30Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddRow30()
        {
            DataTable dtCTable = null;
            DataRow drCRow = null;
            string strMem = "";
            try
            {
                if (ViewState["CTable30"] != null)
                {
                    dtCTable = (DataTable)ViewState["CTable30"];

                    if (dtCTable.Rows.Count > 0)
                    {
                        drCRow = dtCTable.NewRow();
                        drCRow["SlNo"] = dtCTable.Rows.Count + 1;

                        //add new row to DataTable
                        dtCTable.Rows.Add(drCRow);
                        //Store the current data to ViewState for future reference
                        ViewState["CTable30"] = dtCTable;
                        for (int i = 0; i < dtCTable.Rows.Count - 1; i++)
                        {
                            //extract the DropDownList Selected Items
                            DropDownList ddl30center = (DropDownList)gv30.Rows[i].FindControl("ddl30center");
                            DropDownList ddl30Mem = (DropDownList)gv30.Rows[i].FindControl("ddl30Mem");
                            DropDownList ddl30Opt = (DropDownList)gv30.Rows[i].FindControl("ddl30Opt");

                            // Update the DataRow with the DDL Selected Items
                            dtCTable.Rows[i]["MarketId"] = ddl30center.SelectedValue;
                            dtCTable.Rows[i]["MemberId"] = ddl30Mem.SelectedValue;
                            dtCTable.Rows[i]["CurrYN"] = ddl30Opt.SelectedValue;

                            if (strMem == "")
                                strMem = ddl30Mem.SelectedValue;
                            else if (strMem != "")
                                strMem = strMem + "," + ddl30Mem.SelectedValue;
                        }

                        ViewState["MemId30"] = strMem;
                        //Rebind the Grid with the current data to reflect changes
                        gv30.DataSource = dtCTable;
                        gv30.DataBind();
                    }
                }

                //Set Previous Data on Postbacks
                SetPrevData30();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtCTable = null;
                drCRow = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void SetPrevData30()
        {
            int rowIndex = 0;
            DataTable dt = null;
            CAudit oAu = null;
            DataTable dtMkt = null;
            DataTable dtMem = null;
            DataRow dr = null;
            DataRow drMem = null;
            string vBrCode = ddlBranch.SelectedValue;
            //string strMem = (string)ViewState["MemId30"];
            try
            {
                if (ViewState["CTable30"] != null)
                {
                    dt = (DataTable)ViewState["CTable30"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DropDownList ddl30center = (DropDownList)gv30.Rows[i].FindControl("ddl30center");
                            DropDownList ddl30Mem = (DropDownList)gv30.Rows[i].FindControl("ddl30Mem");
                            DropDownList ddl30Opt = (DropDownList)gv30.Rows[i].FindControl("ddl30Opt");

                            //if (ViewState["MemId30"] == null)
                            //{
                            //    if (strMem == "" || strMem == null)
                            //        strMem = dt.Rows[i]["MemberId"].ToString();
                            //    else if (strMem != "")
                            //        strMem = strMem + "," + dt.Rows[i]["MemberId"].ToString();
                            //}
                            oAu = new CAudit();
                            dtMkt = oAu.GetCenterAudit(vBrCode);

                            dr = dtMkt.NewRow();
                            dr["MarketId"] = "";
                            dr["Market"] = string.Empty;
                            dtMkt.Rows.InsertAt(dr, dtMkt.Rows.Count + 1);
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["MarketId"] = dtMkt.Rows[0]["MarketId"];
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["Market"] = dtMkt.Rows[0]["Market"];
                            dtMkt.Rows[0]["MarketId"] = "-1";
                            dtMkt.Rows[0]["Market"] = "<--Select-->";
                            ddl30center.DataSource = dtMkt;
                            ddl30center.DataTextField = "Market";
                            ddl30center.DataValueField = "MarketId";
                            ddl30center.DataBind();

                            ListItem oLi = new ListItem("<--Select-->", "-1");
                            ddl30Mem.Items.Insert(0, oLi);

                            if (i < dt.Rows.Count - 1)
                            {
                                dtMem = oAu.GetMemberByMktAudit(Convert.ToString(dt.Rows[i]["MarketId"]), vBrCode,"");
                                drMem = dtMem.NewRow();
                                drMem["MemberID"] = 0;
                                drMem["Member"] = string.Empty;
                                dtMem.Rows.InsertAt(drMem, dtMem.Rows.Count + 1);
                                dtMem.Rows[dtMem.Rows.Count - 1]["MemberID"] = dtMem.Rows[0]["MemberID"];
                                dtMem.Rows[dtMem.Rows.Count - 1]["Member"] = dtMem.Rows[0]["Member"];
                                dtMem.Rows[0]["MemberID"] = "-1";
                                dtMem.Rows[0]["Member"] = "<--Select-->";

                                ddl30Mem.DataSource = dtMem;
                                ddl30Mem.DataTextField = "Member";
                                ddl30Mem.DataValueField = "MemberID";
                                ddl30Mem.DataBind();
                                //Set the Previous Selected Items on Each DropDownList  on Postbacks
                                ddl30center.ClearSelection();
                                ddl30center.SelectedIndex = ddl30center.Items.IndexOf(ddl30center.Items.FindByValue(dt.Rows[i]["MarketId"].ToString()));//.Selected = true;

                                ddl30Mem.ClearSelection();
                                ddl30Mem.SelectedIndex = ddl30Mem.Items.IndexOf(ddl30Mem.Items.FindByValue(dt.Rows[i]["MemberID"].ToString()));

                                ddl30Opt.ClearSelection();
                                ddl30Opt.SelectedIndex = ddl30Opt.Items.IndexOf(ddl30Opt.Items.FindByValue(dt.Rows[i]["CurrYN"].ToString()));

                            }
                            rowIndex++;
                        }
                    }
                    ViewState["CTable30"] = dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dtMem = null;
                dr = null;
                drMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl30center_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CAudit oAu = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string strMem = (string)ViewState["MemId30"];
            if (strMem == null) strMem = "";
            try
            {
                oAu = new CAudit();
                DropDownList ddlCent = (DropDownList)sender;
                GridViewRow gvr = (GridViewRow)ddlCent.NamingContainer;
                int rowindex = gvr.RowIndex;

                DropDownList ddl30center = (DropDownList)gv30.Rows[rowindex].FindControl("ddl30center");
                DropDownList ddl30Mem = (DropDownList)gv30.Rows[rowindex].FindControl("ddl30Mem");

                dt = oAu.GetMemberByMktAudit(ddl30center.SelectedValue, vBrCode,strMem);
                ddl30Mem.DataSource = dt;
                ddl30Mem.DataTextField = "Member";
                ddl30Mem.DataValueField = "MemberID";
                ddl30Mem.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl30Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl30Opt_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowindex = 0, vMaxRow = 0;
            double vScore = 0;
            DropDownList ddlOpt = (DropDownList)sender;
            GridViewRow gvr = (GridViewRow)ddlOpt.NamingContainer;

            rowindex = gvr.RowIndex;
            vMaxRow = gv30.Rows.Count;

            DropDownList ddl30center = (DropDownList)gv30.Rows[rowindex].FindControl("ddl30center");
            DropDownList ddl30Mem = (DropDownList)gv30.Rows[rowindex].FindControl("ddl30Mem");
            DropDownList ddl30Opt = (DropDownList)gv30.Rows[rowindex].FindControl("ddl30Opt");

            if (rowindex == vMaxRow - 1)
            {
                if (ddl30center.SelectedIndex > 0 && ddl30Mem.SelectedIndex > 0 && ddl30Opt.SelectedIndex > 0)
                {
                    AddRow30();
                    DataTable dt = (DataTable)ViewState["CTable30"];
                    DataRow[] drs = dt.Select("(CurrYN='Y')");
                    //make a new "results" datatable via clone to keep structure
                    DataTable dt2 = dt.Clone();
                    foreach (DataRow d in drs)
                    {
                        dt2.ImportRow(d);
                    }
                    if (dt.Rows.Count > 1 && dt2.Rows.Count > 0)
                    {
                        vScore = (Convert.ToDouble(dt2.Rows.Count) * 100) / Convert.ToDouble(dt.Rows.Count - 1);
                        if (vScore >= 80)
                            txtQ30Score.Text = "5";
                        else if (vScore >= 60 && vScore < 80)
                            txtQ30Score.Text = "3";
                        else if (vScore < 60)
                            txtQ30Score.Text = "1";
                    }
                    else
                        txtQ30Score.Text = "0";

                }
                else
                {
                    gblFuction.AjxMsgPopup("Please feed all the Details");
                    ddl30Opt.Focus();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitialRow31()
        {
            DataTable dt = null;
            DataRow dr = null;
            DataTable dtMkt = null;
            CAudit oAu = null;
            string vBrCode = ddlBranch.SelectedValue;
            ViewState["CenId31"] = null;
            ViewState["MemId31"] = null;
            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("SlNo", typeof(string)));
                dt.Columns.Add(new DataColumn("MarketId", typeof(string)));
                dt.Columns.Add(new DataColumn("MemberId", typeof(string)));
                dt.Columns.Add(new DataColumn("CurrYN", typeof(string)));
                dr = dt.NewRow();
                dr["SlNo"] = 1;
                dr["MarketId"] = "";
                dr["MemberId"] = "";
                dr["CurrYN"] = "";
                dt.Rows.Add(dr);
                //Store the DataTable in ViewState
                ViewState["CTable31"] = dt;
                gv31.DataSource = dt;
                gv31.DataBind();

                oAu = new CAudit();
                dtMkt = oAu.GetCenterAudit(vBrCode);
                DropDownList ddl31center = (DropDownList)gv31.Rows[0].FindControl("ddl31center");
                DropDownList ddl31Mem = (DropDownList)gv31.Rows[0].FindControl("ddl31Mem");

                ddl31center.DataSource = dtMkt;
                ddl31center.DataTextField = "Market";
                ddl31center.DataValueField = "MarketId";
                ddl31center.DataBind();

                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl31center.Items.Insert(0, oLi);
                ddl31Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddRow31()
        {
            DataTable dtCTable = null;
            DataRow drCRow = null;
            string strMem = "";
            try
            {
                if (ViewState["CTable31"] != null)
                {
                    dtCTable = (DataTable)ViewState["CTable31"];

                    if (dtCTable.Rows.Count > 0)
                    {
                        drCRow = dtCTable.NewRow();
                        drCRow["SlNo"] = dtCTable.Rows.Count + 1;

                        //add new row to DataTable
                        dtCTable.Rows.Add(drCRow);
                        //Store the current data to ViewState for future reference
                        ViewState["CTable31"] = dtCTable;
                        for (int i = 0; i < dtCTable.Rows.Count - 1; i++)
                        {
                            //extract the DropDownList Selected Items
                            DropDownList ddl31center = (DropDownList)gv31.Rows[i].FindControl("ddl31center");
                            DropDownList ddl31Mem = (DropDownList)gv31.Rows[i].FindControl("ddl31Mem");
                            DropDownList ddl31Opt = (DropDownList)gv31.Rows[i].FindControl("ddl31Opt");

                            // Update the DataRow with the DDL Selected Items
                            dtCTable.Rows[i]["MarketId"] = ddl31center.SelectedValue;
                            dtCTable.Rows[i]["MemberId"] = ddl31Mem.SelectedValue;
                            dtCTable.Rows[i]["CurrYN"] = ddl31Opt.SelectedValue;

                            if (strMem == "")
                                strMem = ddl31Mem.SelectedValue;
                            else if (strMem != "")
                                strMem = strMem + "," + ddl31Mem.SelectedValue;
                        }

                        ViewState["MemId31"] = strMem;
                        //Rebind the Grid with the current data to reflect changes
                        gv31.DataSource = dtCTable;
                        gv31.DataBind();
                    }
                }

                //Set Previous Data on Postbacks
                SetPrevData31();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtCTable = null;
                drCRow = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void SetPrevData31()
        {
            int rowIndex = 0;
            DataTable dt = null;
            CAudit oAu = null;
            DataTable dtMkt = null;
            DataTable dtMem = null;
            DataRow dr = null;
            DataRow drMem = null;
            string vBrCode = ddlBranch.SelectedValue;
            //string strMem = (string)ViewState["MemId31"];
            try
            {
                if (ViewState["CTable31"] != null)
                {
                    dt = (DataTable)ViewState["CTable31"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DropDownList ddl31center = (DropDownList)gv31.Rows[i].FindControl("ddl31center");
                            DropDownList ddl31Mem = (DropDownList)gv31.Rows[i].FindControl("ddl31Mem");
                            DropDownList ddl31Opt = (DropDownList)gv31.Rows[i].FindControl("ddl31Opt");

                            //if (ViewState["MemId31"] == null)
                            //{
                            //    if (strMem == "" || strMem == null)
                            //        strMem = dt.Rows[i]["MemberId"].ToString();
                            //    else if (strMem != "")
                            //        strMem = strMem + "," + dt.Rows[i]["MemberId"].ToString();
                            //}
                            oAu = new CAudit();
                            dtMkt = oAu.GetCenterAudit(vBrCode);

                            dr = dtMkt.NewRow();
                            dr["MarketId"] = "";
                            dr["Market"] = string.Empty;
                            dtMkt.Rows.InsertAt(dr, dtMkt.Rows.Count + 1);
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["MarketId"] = dtMkt.Rows[0]["MarketId"];
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["Market"] = dtMkt.Rows[0]["Market"];
                            dtMkt.Rows[0]["MarketId"] = "-1";
                            dtMkt.Rows[0]["Market"] = "<--Select-->";
                            ddl31center.DataSource = dtMkt;
                            ddl31center.DataTextField = "Market";
                            ddl31center.DataValueField = "MarketId";
                            ddl31center.DataBind();

                            ListItem oLi = new ListItem("<--Select-->", "-1");
                            ddl31Mem.Items.Insert(0, oLi);

                            if (i < dt.Rows.Count - 1)
                            {
                                dtMem = oAu.GetMemberByMktAudit(Convert.ToString(dt.Rows[i]["MarketId"]), vBrCode,"");
                                drMem = dtMem.NewRow();
                                drMem["MemberID"] = 0;
                                drMem["Member"] = string.Empty;
                                dtMem.Rows.InsertAt(drMem, dtMem.Rows.Count + 1);
                                dtMem.Rows[dtMem.Rows.Count - 1]["MemberID"] = dtMem.Rows[0]["MemberID"];
                                dtMem.Rows[dtMem.Rows.Count - 1]["Member"] = dtMem.Rows[0]["Member"];
                                dtMem.Rows[0]["MemberID"] = "-1";
                                dtMem.Rows[0]["Member"] = "<--Select-->";

                                ddl31Mem.DataSource = dtMem;
                                ddl31Mem.DataTextField = "Member";
                                ddl31Mem.DataValueField = "MemberID";
                                ddl31Mem.DataBind();
                                //Set the Previous Selected Items on Each DropDownList  on Postbacks
                                ddl31center.ClearSelection();
                                ddl31center.SelectedIndex = ddl31center.Items.IndexOf(ddl31center.Items.FindByValue(dt.Rows[i]["MarketId"].ToString()));//.Selected = true;

                                ddl31Mem.ClearSelection();
                                ddl31Mem.SelectedIndex = ddl31Mem.Items.IndexOf(ddl31Mem.Items.FindByValue(dt.Rows[i]["MemberID"].ToString()));

                                ddl31Opt.ClearSelection();
                                ddl31Opt.SelectedIndex = ddl31Opt.Items.IndexOf(ddl31Opt.Items.FindByValue(dt.Rows[i]["CurrYN"].ToString()));

                            }
                            rowIndex++;
                        }
                    }
                    ViewState["CTable31"] = dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dtMem = null;
                dr = null;
                drMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl31center_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CAudit oAu = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string strMem = (string)ViewState["MemId31"];
            if (strMem == null) strMem = "";
            try
            {
                oAu = new CAudit();
                DropDownList ddlCent = (DropDownList)sender;
                GridViewRow gvr = (GridViewRow)ddlCent.NamingContainer;
                int rowindex = gvr.RowIndex;

                DropDownList ddl31center = (DropDownList)gv31.Rows[rowindex].FindControl("ddl31center");
                DropDownList ddl31Mem = (DropDownList)gv31.Rows[rowindex].FindControl("ddl31Mem");

                dt = oAu.GetMemberByMktAudit(ddl31center.SelectedValue, vBrCode,strMem);
                ddl31Mem.DataSource = dt;
                ddl31Mem.DataTextField = "Member";
                ddl31Mem.DataValueField = "MemberID";
                ddl31Mem.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl31Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl31Opt_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowindex = 0, vMaxRow = 0;
            double vScore = 0;
            DropDownList ddlOpt = (DropDownList)sender;
            GridViewRow gvr = (GridViewRow)ddlOpt.NamingContainer;

            rowindex = gvr.RowIndex;
            vMaxRow = gv31.Rows.Count;

            DropDownList ddl31center = (DropDownList)gv31.Rows[rowindex].FindControl("ddl31center");
            DropDownList ddl31Mem = (DropDownList)gv31.Rows[rowindex].FindControl("ddl31Mem");
            DropDownList ddl31Opt = (DropDownList)gv31.Rows[rowindex].FindControl("ddl31Opt");

            if (rowindex == vMaxRow - 1)
            {
                if (ddl31center.SelectedIndex > 0 && ddl31Mem.SelectedIndex > 0 && ddl31Opt.SelectedIndex > 0)
                {
                    AddRow31();
                    DataTable dt = (DataTable)ViewState["CTable31"];
                    DataRow[] drs = dt.Select("(CurrYN='Y')");
                    //make a new "results" datatable via clone to keep structure
                    DataTable dt2 = dt.Clone();
                    foreach (DataRow d in drs)
                    {
                        dt2.ImportRow(d);
                    }
                    if (dt.Rows.Count > 1 && dt2.Rows.Count > 0)
                    {
                        vScore = (Convert.ToDouble(dt2.Rows.Count) * 100) / Convert.ToDouble(dt.Rows.Count - 1);
                        if (vScore >= 80)
                            txtQ31Score.Text = "5";
                        else if (vScore >= 60 && vScore < 80)
                            txtQ31Score.Text = "3";
                        else if (vScore < 60)
                            txtQ31Score.Text = "1";
                    }
                    else
                        txtQ31Score.Text = "0";

                }
                else
                {
                    gblFuction.AjxMsgPopup("Please feed all the Details");
                    ddl31Opt.Focus();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitialRow32()
        {
            DataTable dt = null;
            DataRow dr = null;
            DataTable dtMkt = null;
            CAudit oAu = null;
            string vBrCode = ddlBranch.SelectedValue;
            ViewState["CenId32"] = null;
            ViewState["MemId32"] = null;
            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("SlNo", typeof(string)));
                dt.Columns.Add(new DataColumn("MarketId", typeof(string)));
                dt.Columns.Add(new DataColumn("MemberId", typeof(string)));
                dt.Columns.Add(new DataColumn("CurrYN", typeof(string)));
                dr = dt.NewRow();
                dr["SlNo"] = 1;
                dr["MarketId"] = "";
                dr["MemberId"] = "";
                dr["CurrYN"] = "";
                dt.Rows.Add(dr);
                //Store the DataTable in ViewState
                ViewState["CTable32"] = dt;
                gv32.DataSource = dt;
                gv32.DataBind();

                oAu = new CAudit();
                dtMkt = oAu.GetCenterAudit(vBrCode);
                DropDownList ddl32center = (DropDownList)gv32.Rows[0].FindControl("ddl32center");
                DropDownList ddl32Mem = (DropDownList)gv32.Rows[0].FindControl("ddl32Mem");

                ddl32center.DataSource = dtMkt;
                ddl32center.DataTextField = "Market";
                ddl32center.DataValueField = "MarketId";
                ddl32center.DataBind();

                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl32center.Items.Insert(0, oLi);
                ddl32Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddRow32()
        {
            DataTable dtCTable = null;
            DataRow drCRow = null;
            string strMem = "";
            try
            {
                if (ViewState["CTable32"] != null)
                {
                    dtCTable = (DataTable)ViewState["CTable32"];

                    if (dtCTable.Rows.Count > 0)
                    {
                        drCRow = dtCTable.NewRow();
                        drCRow["SlNo"] = dtCTable.Rows.Count + 1;

                        //add new row to DataTable
                        dtCTable.Rows.Add(drCRow);
                        //Store the current data to ViewState for future reference
                        ViewState["CTable32"] = dtCTable;
                        for (int i = 0; i < dtCTable.Rows.Count - 1; i++)
                        {
                            //extract the DropDownList Selected Items
                            DropDownList ddl32center = (DropDownList)gv32.Rows[i].FindControl("ddl32center");
                            DropDownList ddl32Mem = (DropDownList)gv32.Rows[i].FindControl("ddl32Mem");
                            DropDownList ddl32Opt = (DropDownList)gv32.Rows[i].FindControl("ddl32Opt");

                            // Update the DataRow with the DDL Selected Items
                            dtCTable.Rows[i]["MarketId"] = ddl32center.SelectedValue;
                            dtCTable.Rows[i]["MemberId"] = ddl32Mem.SelectedValue;
                            dtCTable.Rows[i]["CurrYN"] = ddl32Opt.SelectedValue;


                            if (strMem == "")
                                strMem = ddl32Mem.SelectedValue;
                            else if (strMem != "")
                                strMem = strMem + "," + ddl32Mem.SelectedValue;
                        }

                        ViewState["MemId32"] = strMem;
                        //Rebind the Grid with the current data to reflect changes
                        gv32.DataSource = dtCTable;
                        gv32.DataBind();
                    }
                }

                //Set Previous Data on Postbacks
                SetPrevData32();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtCTable = null;
                drCRow = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void SetPrevData32()
        {
            int rowIndex = 0;
            DataTable dt = null;
            CAudit oAu = null;
            DataTable dtMkt = null;
            DataTable dtMem = null;
            DataRow dr = null;
            DataRow drMem = null;
            string vBrCode = ddlBranch.SelectedValue;
            //string strMem = (string)ViewState["MemId32"];
            try
            {
                if (ViewState["CTable32"] != null)
                {
                    dt = (DataTable)ViewState["CTable32"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DropDownList ddl32center = (DropDownList)gv32.Rows[i].FindControl("ddl32center");
                            DropDownList ddl32Mem = (DropDownList)gv32.Rows[i].FindControl("ddl32Mem");
                            DropDownList ddl32Opt = (DropDownList)gv32.Rows[i].FindControl("ddl32Opt");

                            //if (ViewState["MemId32"] == null)
                            //{
                            //    if (strMem == "" || strMem == null)
                            //        strMem = dt.Rows[i]["MemberId"].ToString();
                            //    else if (strMem != "")
                            //        strMem = strMem + "," + dt.Rows[i]["MemberId"].ToString();
                            //}
                            oAu = new CAudit();
                            dtMkt = oAu.GetCenterAudit(vBrCode);

                            dr = dtMkt.NewRow();
                            dr["MarketId"] = "";
                            dr["Market"] = string.Empty;
                            dtMkt.Rows.InsertAt(dr, dtMkt.Rows.Count + 1);
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["MarketId"] = dtMkt.Rows[0]["MarketId"];
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["Market"] = dtMkt.Rows[0]["Market"];
                            dtMkt.Rows[0]["MarketId"] = "-1";
                            dtMkt.Rows[0]["Market"] = "<--Select-->";
                            ddl32center.DataSource = dtMkt;
                            ddl32center.DataTextField = "Market";
                            ddl32center.DataValueField = "MarketId";
                            ddl32center.DataBind();

                            ListItem oLi = new ListItem("<--Select-->", "-1");
                            ddl32Mem.Items.Insert(0, oLi);

                            if (i < dt.Rows.Count - 1)
                            {
                                dtMem = oAu.GetMemberByMktAudit(Convert.ToString(dt.Rows[i]["MarketId"]), vBrCode,"");
                                drMem = dtMem.NewRow();
                                drMem["MemberID"] = 0;
                                drMem["Member"] = string.Empty;
                                dtMem.Rows.InsertAt(drMem, dtMem.Rows.Count + 1);
                                dtMem.Rows[dtMem.Rows.Count - 1]["MemberID"] = dtMem.Rows[0]["MemberID"];
                                dtMem.Rows[dtMem.Rows.Count - 1]["Member"] = dtMem.Rows[0]["Member"];
                                dtMem.Rows[0]["MemberID"] = "-1";
                                dtMem.Rows[0]["Member"] = "<--Select-->";

                                ddl32Mem.DataSource = dtMem;
                                ddl32Mem.DataTextField = "Member";
                                ddl32Mem.DataValueField = "MemberID";
                                ddl32Mem.DataBind();
                                //Set the Previous Selected Items on Each DropDownList  on Postbacks
                                ddl32center.ClearSelection();
                                ddl32center.SelectedIndex = ddl32center.Items.IndexOf(ddl32center.Items.FindByValue(dt.Rows[i]["MarketId"].ToString()));//.Selected = true;

                                ddl32Mem.ClearSelection();
                                ddl32Mem.SelectedIndex = ddl32Mem.Items.IndexOf(ddl32Mem.Items.FindByValue(dt.Rows[i]["MemberID"].ToString()));

                                ddl32Opt.ClearSelection();
                                ddl32Opt.SelectedIndex = ddl32Opt.Items.IndexOf(ddl32Opt.Items.FindByValue(dt.Rows[i]["CurrYN"].ToString()));

                            }
                            rowIndex++;
                        }
                    }
                    ViewState["CTable32"] = dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dtMem = null;
                dr = null;
                drMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl32center_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CAudit oAu = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string strMem = (string)ViewState["MemId32"];
            if (strMem == null) strMem = "";
            try
            {
                oAu = new CAudit();
                DropDownList ddlCent = (DropDownList)sender;
                GridViewRow gvr = (GridViewRow)ddlCent.NamingContainer;
                int rowindex = gvr.RowIndex;

                DropDownList ddl32center = (DropDownList)gv32.Rows[rowindex].FindControl("ddl32center");
                DropDownList ddl32Mem = (DropDownList)gv32.Rows[rowindex].FindControl("ddl32Mem");

                dt = oAu.GetMemberByMktAudit(ddl32center.SelectedValue, vBrCode,strMem);
                ddl32Mem.DataSource = dt;
                ddl32Mem.DataTextField = "Member";
                ddl32Mem.DataValueField = "MemberID";
                ddl32Mem.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl32Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl32Opt_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowindex = 0, vMaxRow = 0;
            double vScore = 0;
            DropDownList ddlOpt = (DropDownList)sender;
            GridViewRow gvr = (GridViewRow)ddlOpt.NamingContainer;

            rowindex = gvr.RowIndex;
            vMaxRow = gv32.Rows.Count;

            DropDownList ddl32center = (DropDownList)gv32.Rows[rowindex].FindControl("ddl32center");
            DropDownList ddl32Mem = (DropDownList)gv32.Rows[rowindex].FindControl("ddl32Mem");
            DropDownList ddl32Opt = (DropDownList)gv32.Rows[rowindex].FindControl("ddl32Opt");

            if (rowindex == vMaxRow - 1)
            {
                if (ddl32center.SelectedIndex > 0 && ddl32Mem.SelectedIndex > 0 && ddl32Opt.SelectedIndex > 0)
                {
                    AddRow32();
                    DataTable dt = (DataTable)ViewState["CTable32"];
                    DataRow[] drs = dt.Select("(CurrYN='Y')");
                    //make a new "results" datatable via clone to keep structure
                    DataTable dt2 = dt.Clone();
                    foreach (DataRow d in drs)
                    {
                        dt2.ImportRow(d);
                    }
                    if (dt.Rows.Count > 1 && dt2.Rows.Count > 0)
                    {
                        vScore = (Convert.ToDouble(dt2.Rows.Count) * 100) / Convert.ToDouble(dt.Rows.Count - 1);
                        if (vScore >= 80)
                            txtQ32Score.Text = "5";
                        else if (vScore >= 60 && vScore < 80)
                            txtQ32Score.Text = "3";
                        else if (vScore < 60)
                            txtQ32Score.Text = "1";
                    }
                    else
                        txtQ32Score.Text = "0";
                }
                else
                {
                    gblFuction.AjxMsgPopup("Please feed all the Details");
                    ddl32Opt.Focus();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitialRow33()
        {
            DataTable dt = null;
            DataRow dr = null;
            DataTable dtMkt = null;
            CAudit oAu = null;
            string vBrCode = ddlBranch.SelectedValue;
            ViewState["CenId33"] = null;
            ViewState["MemId33"] = null;
            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("SlNo", typeof(string)));
                dt.Columns.Add(new DataColumn("MarketId", typeof(string)));
                dt.Columns.Add(new DataColumn("MemberId", typeof(string)));
                dt.Columns.Add(new DataColumn("CurrYN", typeof(string)));
                dr = dt.NewRow();
                dr["SlNo"] = 1;
                dr["MarketId"] = "";
                dr["MemberId"] = "";
                dr["CurrYN"] = "";
                dt.Rows.Add(dr);
                //Store the DataTable in ViewState
                ViewState["CTable33"] = dt;
                gv33.DataSource = dt;
                gv33.DataBind();

                oAu = new CAudit();
                dtMkt = oAu.GetCenterAudit(vBrCode);
                DropDownList ddl33center = (DropDownList)gv33.Rows[0].FindControl("ddl33center");
                DropDownList ddl33Mem = (DropDownList)gv33.Rows[0].FindControl("ddl33Mem");

                ddl33center.DataSource = dtMkt;
                ddl33center.DataTextField = "Market";
                ddl33center.DataValueField = "MarketId";
                ddl33center.DataBind();

                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl33center.Items.Insert(0, oLi);
                ddl33Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddRow33()
        {
            DataTable dtCTable = null;
            DataRow drCRow = null;
            string strMem = "";
            try
            {
                if (ViewState["CTable33"] != null)
                {
                    dtCTable = (DataTable)ViewState["CTable33"];

                    if (dtCTable.Rows.Count > 0)
                    {
                        drCRow = dtCTable.NewRow();
                        drCRow["SlNo"] = dtCTable.Rows.Count + 1;

                        //add new row to DataTable
                        dtCTable.Rows.Add(drCRow);
                        //Store the current data to ViewState for future reference
                        ViewState["CTable33"] = dtCTable;
                        for (int i = 0; i < dtCTable.Rows.Count - 1; i++)
                        {
                            //extract the DropDownList Selected Items
                            DropDownList ddl33center = (DropDownList)gv33.Rows[i].FindControl("ddl33center");
                            DropDownList ddl33Mem = (DropDownList)gv33.Rows[i].FindControl("ddl33Mem");
                            DropDownList ddl33Opt = (DropDownList)gv33.Rows[i].FindControl("ddl33Opt");

                            // Update the DataRow with the DDL Selected Items
                            dtCTable.Rows[i]["MarketId"] = ddl33center.SelectedValue;
                            dtCTable.Rows[i]["MemberId"] = ddl33Mem.SelectedValue;
                            dtCTable.Rows[i]["CurrYN"] = ddl33Opt.SelectedValue;


                            if (strMem == "")
                                strMem = ddl33Mem.SelectedValue;
                            else if (strMem != "")
                                strMem = strMem + "," + ddl33Mem.SelectedValue;
                        }

                        ViewState["MemId33"] = strMem;
                        //Rebind the Grid with the current data to reflect changes
                        gv33.DataSource = dtCTable;
                        gv33.DataBind();
                    }
                }

                //Set Previous Data on Postbacks
                SetPrevData33();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtCTable = null;
                drCRow = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void SetPrevData33()
        {
            int rowIndex = 0;
            DataTable dt = null;
            CAudit oAu = null;
            DataTable dtMkt = null;
            DataTable dtMem = null;
            DataRow dr = null;
            DataRow drMem = null;
            string vBrCode = ddlBranch.SelectedValue;
            //string strMem = (string)ViewState["MemId33"];
            try
            {
                if (ViewState["CTable33"] != null)
                {
                    dt = (DataTable)ViewState["CTable33"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DropDownList ddl33center = (DropDownList)gv33.Rows[i].FindControl("ddl33center");
                            DropDownList ddl33Mem = (DropDownList)gv33.Rows[i].FindControl("ddl33Mem");
                            DropDownList ddl33Opt = (DropDownList)gv33.Rows[i].FindControl("ddl33Opt");

                            // if (ViewState["MemId33"] == null)
                            //{
                            //    if (strMem == "" || strMem == null)
                            //        strMem = dt.Rows[i]["MemberId"].ToString();
                            //    else if (strMem != "")
                            //        strMem = strMem + "," + dt.Rows[i]["MemberId"].ToString();
                            //}
                            oAu = new CAudit();
                            dtMkt = oAu.GetCenterAudit(vBrCode);

                            dr = dtMkt.NewRow();
                            dr["MarketId"] = "";
                            dr["Market"] = string.Empty;
                            dtMkt.Rows.InsertAt(dr, dtMkt.Rows.Count + 1);
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["MarketId"] = dtMkt.Rows[0]["MarketId"];
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["Market"] = dtMkt.Rows[0]["Market"];
                            dtMkt.Rows[0]["MarketId"] = "-1";
                            dtMkt.Rows[0]["Market"] = "<--Select-->";
                            ddl33center.DataSource = dtMkt;
                            ddl33center.DataTextField = "Market";
                            ddl33center.DataValueField = "MarketId";
                            ddl33center.DataBind();

                            ListItem oLi = new ListItem("<--Select-->", "-1");
                            ddl33Mem.Items.Insert(0, oLi);

                            if (i < dt.Rows.Count - 1)
                            {
                                dtMem = oAu.GetMemberByMktAudit(Convert.ToString(dt.Rows[i]["MarketId"]), vBrCode,"");
                                drMem = dtMem.NewRow();
                                drMem["MemberID"] = 0;
                                drMem["Member"] = string.Empty;
                                dtMem.Rows.InsertAt(drMem, dtMem.Rows.Count + 1);
                                dtMem.Rows[dtMem.Rows.Count - 1]["MemberID"] = dtMem.Rows[0]["MemberID"];
                                dtMem.Rows[dtMem.Rows.Count - 1]["Member"] = dtMem.Rows[0]["Member"];
                                dtMem.Rows[0]["MemberID"] = "-1";
                                dtMem.Rows[0]["Member"] = "<--Select-->";

                                ddl33Mem.DataSource = dtMem;
                                ddl33Mem.DataTextField = "Member";
                                ddl33Mem.DataValueField = "MemberID";
                                ddl33Mem.DataBind();
                                //Set the Previous Selected Items on Each DropDownList  on Postbacks
                                ddl33center.ClearSelection();
                                ddl33center.SelectedIndex = ddl33center.Items.IndexOf(ddl33center.Items.FindByValue(dt.Rows[i]["MarketId"].ToString()));//.Selected = true;

                                ddl33Mem.ClearSelection();
                                ddl33Mem.SelectedIndex = ddl33Mem.Items.IndexOf(ddl33Mem.Items.FindByValue(dt.Rows[i]["MemberID"].ToString()));

                                ddl33Opt.ClearSelection();
                                ddl33Opt.SelectedIndex = ddl33Opt.Items.IndexOf(ddl33Opt.Items.FindByValue(dt.Rows[i]["CurrYN"].ToString()));

                            }
                            rowIndex++;
                        }
                    }
                    ViewState["CTable33"] = dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dtMem = null;
                dr = null;
                drMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl33center_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CAudit oAu = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string strMem = (string)ViewState["MemId33"];
            if (strMem == null) strMem = "";
            try
            {
                oAu = new CAudit();
                DropDownList ddlCent = (DropDownList)sender;
                GridViewRow gvr = (GridViewRow)ddlCent.NamingContainer;
                int rowindex = gvr.RowIndex;

                DropDownList ddl33center = (DropDownList)gv33.Rows[rowindex].FindControl("ddl33center");
                DropDownList ddl33Mem = (DropDownList)gv33.Rows[rowindex].FindControl("ddl33Mem");

                dt = oAu.GetMemberByMktAudit(ddl33center.SelectedValue, vBrCode,strMem);
                ddl33Mem.DataSource = dt;
                ddl33Mem.DataTextField = "Member";
                ddl33Mem.DataValueField = "MemberID";
                ddl33Mem.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl33Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl33Opt_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowindex = 0, vMaxRow = 0;
            double vScore = 0;
            DropDownList ddlOpt = (DropDownList)sender;
            GridViewRow gvr = (GridViewRow)ddlOpt.NamingContainer;

            rowindex = gvr.RowIndex;
            vMaxRow = gv33.Rows.Count;

            DropDownList ddl33center = (DropDownList)gv33.Rows[rowindex].FindControl("ddl33center");
            DropDownList ddl33Mem = (DropDownList)gv33.Rows[rowindex].FindControl("ddl33Mem");
            DropDownList ddl33Opt = (DropDownList)gv33.Rows[rowindex].FindControl("ddl33Opt");

            if (rowindex == vMaxRow - 1)
            {
                if (ddl33center.SelectedIndex > 0 && ddl33Mem.SelectedIndex > 0 && ddl33Opt.SelectedIndex > 0)
                {
                    AddRow33();
                    DataTable dt = (DataTable)ViewState["CTable33"];
                    DataRow[] drs = dt.Select("(CurrYN='Y')");
                    //make a new "results" datatable via clone to keep structure
                    DataTable dt2 = dt.Clone();
                    foreach (DataRow d in drs)
                    {
                        dt2.ImportRow(d);
                    }
                    if (dt.Rows.Count > 1 && dt2.Rows.Count > 0)
                    {
                        vScore = (Convert.ToDouble(dt2.Rows.Count) * 100) / Convert.ToDouble(dt.Rows.Count - 1);
                        if (vScore >= 80)
                            txtQ33Score.Text = "5";
                        else if (vScore >= 60 && vScore < 80)
                            txtQ33Score.Text = "3";
                        else if (vScore < 60)
                            txtQ33Score.Text = "1";
                    }
                    else
                        txtQ33Score.Text = "0";
                }
                else
                {
                    gblFuction.AjxMsgPopup("Please feed all the Details");
                    ddl33Opt.Focus();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitialRow34()
        {
            DataTable dt = null;
            DataRow dr = null;
            DataTable dtMkt = null;
            CAudit oAu = null;
            string vBrCode = ddlBranch.SelectedValue;
            ViewState["CenId34"] = null;
            ViewState["MemId34"] = null;
            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("SlNo", typeof(string)));
                dt.Columns.Add(new DataColumn("MarketId", typeof(string)));
                dt.Columns.Add(new DataColumn("MemberId", typeof(string)));
                dt.Columns.Add(new DataColumn("CurrYN", typeof(string)));
                dr = dt.NewRow();
                dr["SlNo"] = 1;
                dr["MarketId"] = "";
                dr["MemberId"] = "";
                dr["CurrYN"] = "";
                dt.Rows.Add(dr);
                //Store the DataTable in ViewState
                ViewState["CTable34"] = dt;
                gv34.DataSource = dt;
                gv34.DataBind();

                oAu = new CAudit();
                dtMkt = oAu.GetCenterAudit(vBrCode);
                DropDownList ddl34center = (DropDownList)gv34.Rows[0].FindControl("ddl34center");
                DropDownList ddl34Mem = (DropDownList)gv34.Rows[0].FindControl("ddl34Mem");

                ddl34center.DataSource = dtMkt;
                ddl34center.DataTextField = "Market";
                ddl34center.DataValueField = "MarketId";
                ddl34center.DataBind();

                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl34center.Items.Insert(0, oLi);
                ddl34Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddRow34()
        {
            DataTable dtCTable = null;
            DataRow drCRow = null;
            string strMem = "";
            try
            {
                if (ViewState["CTable34"] != null)
                {
                    dtCTable = (DataTable)ViewState["CTable34"];

                    if (dtCTable.Rows.Count > 0)
                    {
                        drCRow = dtCTable.NewRow();
                        drCRow["SlNo"] = dtCTable.Rows.Count + 1;

                        //add new row to DataTable
                        dtCTable.Rows.Add(drCRow);
                        //Store the current data to ViewState for future reference
                        ViewState["CTable34"] = dtCTable;
                        for (int i = 0; i < dtCTable.Rows.Count - 1; i++)
                        {
                            //extract the DropDownList Selected Items
                            DropDownList ddl34center = (DropDownList)gv34.Rows[i].FindControl("ddl34center");
                            DropDownList ddl34Mem = (DropDownList)gv34.Rows[i].FindControl("ddl34Mem");
                            DropDownList ddl34Opt = (DropDownList)gv34.Rows[i].FindControl("ddl34Opt");

                            // Update the DataRow with the DDL Selected Items
                            dtCTable.Rows[i]["MarketId"] = ddl34center.SelectedValue;
                            dtCTable.Rows[i]["MemberId"] = ddl34Mem.SelectedValue;
                            dtCTable.Rows[i]["CurrYN"] = ddl34Opt.SelectedValue;

                            if (strMem == "")
                                strMem = ddl34Mem.SelectedValue;
                            else if (strMem != "")
                                strMem = strMem + "," + ddl34Mem.SelectedValue;
                        }

                        ViewState["MemId34"] = strMem;
                        //Rebind the Grid with the current data to reflect changes
                        gv34.DataSource = dtCTable;
                        gv34.DataBind();
                    }
                }

                //Set Previous Data on Postbacks
                SetPrevData34();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtCTable = null;
                drCRow = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void SetPrevData34()
        {
            int rowIndex = 0;
            DataTable dt = null;
            CAudit oAu = null;
            DataTable dtMkt = null;
            DataTable dtMem = null;
            DataRow dr = null;
            DataRow drMem = null;
            string vBrCode = ddlBranch.SelectedValue;
            //string strMem = (string)ViewState["MemId34"];
            try
            {
                if (ViewState["CTable34"] != null)
                {
                    dt = (DataTable)ViewState["CTable34"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DropDownList ddl34center = (DropDownList)gv34.Rows[i].FindControl("ddl34center");
                            DropDownList ddl34Mem = (DropDownList)gv34.Rows[i].FindControl("ddl34Mem");
                            DropDownList ddl34Opt = (DropDownList)gv34.Rows[i].FindControl("ddl34Opt");

                            //if (ViewState["MemId34"] == null)
                            //{
                            //    if (strMem == "" || strMem == null)
                            //        strMem = dt.Rows[i]["MemberId"].ToString();
                            //    else if (strMem != "")
                            //        strMem = strMem + "," + dt.Rows[i]["MemberId"].ToString();
                            //}
                            oAu = new CAudit();
                            dtMkt = oAu.GetCenterAudit(vBrCode);

                            dr = dtMkt.NewRow();
                            dr["MarketId"] = "";
                            dr["Market"] = string.Empty;
                            dtMkt.Rows.InsertAt(dr, dtMkt.Rows.Count + 1);
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["MarketId"] = dtMkt.Rows[0]["MarketId"];
                            dtMkt.Rows[dtMkt.Rows.Count - 1]["Market"] = dtMkt.Rows[0]["Market"];
                            dtMkt.Rows[0]["MarketId"] = "-1";
                            dtMkt.Rows[0]["Market"] = "<--Select-->";
                            ddl34center.DataSource = dtMkt;
                            ddl34center.DataTextField = "Market";
                            ddl34center.DataValueField = "MarketId";
                            ddl34center.DataBind();

                            ListItem oLi = new ListItem("<--Select-->", "-1");
                            ddl34Mem.Items.Insert(0, oLi);

                            if (i < dt.Rows.Count - 1)
                            {
                                dtMem = oAu.GetMemberByMktAudit(Convert.ToString(dt.Rows[i]["MarketId"]), vBrCode,"");
                                drMem = dtMem.NewRow();
                                drMem["MemberID"] = 0;
                                drMem["Member"] = string.Empty;
                                dtMem.Rows.InsertAt(drMem, dtMem.Rows.Count + 1);
                                dtMem.Rows[dtMem.Rows.Count - 1]["MemberID"] = dtMem.Rows[0]["MemberID"];
                                dtMem.Rows[dtMem.Rows.Count - 1]["Member"] = dtMem.Rows[0]["Member"];
                                dtMem.Rows[0]["MemberID"] = "-1";
                                dtMem.Rows[0]["Member"] = "<--Select-->";

                                ddl34Mem.DataSource = dtMem;
                                ddl34Mem.DataTextField = "Member";
                                ddl34Mem.DataValueField = "MemberID";
                                ddl34Mem.DataBind();
                                //Set the Previous Selected Items on Each DropDownList  on Postbacks
                                ddl34center.ClearSelection();
                                ddl34center.SelectedIndex = ddl34center.Items.IndexOf(ddl34center.Items.FindByValue(dt.Rows[i]["MarketId"].ToString()));//.Selected = true;

                                ddl34Mem.ClearSelection();
                                ddl34Mem.SelectedIndex = ddl34Mem.Items.IndexOf(ddl34Mem.Items.FindByValue(dt.Rows[i]["MemberID"].ToString()));

                                ddl34Opt.ClearSelection();
                                ddl34Opt.SelectedIndex = ddl34Opt.Items.IndexOf(ddl34Opt.Items.FindByValue(dt.Rows[i]["CurrYN"].ToString()));

                            }
                            rowIndex++;
                        }
                    }
                    ViewState["CTable34"] = dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
                dtMkt = null;
                dtMem = null;
                dr = null;
                drMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl34center_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CAudit oAu = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string strMem = (string)ViewState["MemId34"];
            if (strMem == null) strMem = "";
            try
            {
                oAu = new CAudit();
                DropDownList ddlCent = (DropDownList)sender;
                GridViewRow gvr = (GridViewRow)ddlCent.NamingContainer;
                int rowindex = gvr.RowIndex;

                DropDownList ddl34center = (DropDownList)gv34.Rows[rowindex].FindControl("ddl34center");
                DropDownList ddl34Mem = (DropDownList)gv34.Rows[rowindex].FindControl("ddl34Mem");

                dt = oAu.GetMemberByMktAudit(ddl34center.SelectedValue, vBrCode,strMem);
                ddl34Mem.DataSource = dt;
                ddl34Mem.DataTextField = "Member";
                ddl34Mem.DataValueField = "MemberID";
                ddl34Mem.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddl34Mem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oAu = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl34Opt_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowindex = 0, vMaxRow = 0;
            double vScore = 0;
            DropDownList ddlOpt = (DropDownList)sender;
            GridViewRow gvr = (GridViewRow)ddlOpt.NamingContainer;

            rowindex = gvr.RowIndex;
            vMaxRow = gv34.Rows.Count;

            DropDownList ddl34center = (DropDownList)gv34.Rows[rowindex].FindControl("ddl34center");
            DropDownList ddl34Mem = (DropDownList)gv34.Rows[rowindex].FindControl("ddl34Mem");
            DropDownList ddl34Opt = (DropDownList)gv34.Rows[rowindex].FindControl("ddl34Opt");

            if (rowindex == vMaxRow - 1)
            {
                if (ddl34center.SelectedIndex > 0 && ddl34Mem.SelectedIndex > 0 && ddl34Opt.SelectedIndex > 0)
                {
                    AddRow34();
                    DataTable dt = (DataTable)ViewState["CTable34"];
                    DataRow[] drs = dt.Select("(CurrYN='Y')");
                    //make a new "results" datatable via clone to keep structure
                    DataTable dt2 = dt.Clone();
                    foreach (DataRow d in drs)
                    {
                        dt2.ImportRow(d);
                    }
                    if (dt.Rows.Count > 1 && dt2.Rows.Count > 0)
                    {
                        vScore = (Convert.ToDouble(dt2.Rows.Count) * 100) / Convert.ToDouble(dt.Rows.Count - 1);
                        if (vScore >= 80)
                            txtQ34Score.Text = "5";
                        else if (vScore >= 60 && vScore < 80)
                            txtQ34Score.Text = "3";
                        else if (vScore < 60)
                            txtQ34Score.Text = "1";
                    }
                    else
                        txtQ34Score.Text = "0";
                }
                else
                {
                    gblFuction.AjxMsgPopup("Please feed all the Details");
                    ddl34Opt.Focus();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (this.CanAdd == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Add);
                return;
            }
            ViewState["StateEdit"] = "Add";
            tbEmp.ActiveTabIndex = 1;
            StatusButton("Add");
            ClearControls();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //string vEoId = Convert.ToString(ViewState["EoId"]);
            //if (this.CanEdit == "N")
            //{
            //    gblFuction.MsgPopup(MsgAccess.Edit);
            //    return;
            //}
            tbEmp.ActiveTabIndex = 1;
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbEmp.ActiveTabIndex = 0;
            ClearControls();
            LoadGrid(0);
            EnableControl(false);
            StatusButton("View");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                EnableControl(false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0;
            CAudit oAd = null;
            try
            {
                oAd = new CAudit();
                dt = oAd.GetAuditSubmissionPG(pPgIndx, ref vTotRows);
                gvEmp.DataSource = dt;
                gvEmp.DataBind();
                lblTotPg.Text = CalTotPages(vTotRows).ToString();
                lblCrPg.Text = vPgNo.ToString();
                if (vPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 1)
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (vPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
            }
            finally
            {
                oAd = null;
                dt = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Prev":
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            tbEmp.ActiveTabIndex = 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNxt_Click(object sender, EventArgs e)
        {
            int vViewIndx = 0;
            if (mvPos.ActiveViewIndex == 0)
            {
                if (ddlBranch.SelectedIndex <= 0)
                {
                    gblFuction.AjxMsgPopup("Select Branch");
                    return;
                }
            }

            vViewIndx = mvPos.ActiveViewIndex;
            if (vViewIndx >= 0)
                mvPos.ActiveViewIndex = vViewIndx + 1;

            //if (vViewIndx == 3)
            //    btnNxt.Enabled = false;
            //else 
            //    btnNxt.Enabled = true;
            //switch (vViewIndx)
            //{
            //    case 1:
            //        ShowBar();
            //        break;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            int vViewIndx = 0;
            vViewIndx = mvPos.ActiveViewIndex;
            if (vViewIndx > 0)
                mvPos.ActiveViewIndex = vViewIndx - 1;
            //switch (vViewIndx)
            //{
            //    case 1:
            //        ShowBar();
            //        break;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEmp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vInsPecId = "";
            DataTable dt = null;
            DataTable dt26 = null;
            DataTable dt27 = null;
            DataTable dt28 = null;
            DataTable dt29 = null;
            DataTable dt30 = null;
            DataTable dt31 = null;
            DataTable dt32 = null;
            DataTable dt33 = null;
            DataTable dt34 = null;
            CAudit oAu = null;
            DataRow dr26;
            DataRow dr27;
            DataRow dr28;
            DataRow dr29;
            DataRow dr30;
            DataRow dr31;
            DataRow dr32;
            DataRow dr33;
            DataRow dr34;
            try
            {
                vInsPecId = Convert.ToString(e.CommandArgument);
                ViewState["InsPecId"] = vInsPecId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvEmp.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oAu = new CAudit();
                    dt = oAu.GetAuditSubmissionById(vInsPecId);
                    dt26 = oAu.GetQ26ById(vInsPecId);
                    dt27 = oAu.GetQ27ById(vInsPecId);
                    dt28 = oAu.GetQ28ById(vInsPecId);
                    dt29 = oAu.GetQ29ById(vInsPecId);
                    dt30 = oAu.GetQ30ById(vInsPecId);
                    dt31 = oAu.GetQ31ById(vInsPecId);
                    dt32 = oAu.GetQ32ById(vInsPecId);
                    dt33 = oAu.GetQ33ById(vInsPecId);
                    dt34 = oAu.GetQ34ById(vInsPecId);
                    if (dt.Rows.Count > 0)
                    {
                        txtSDt.Text = Convert.ToString(dt.Rows[0]["SubmisDt"]);
                        txtCrStrDt.Text = Convert.ToString(dt.Rows[0]["InsCrStDt"]);
                        txtCrEndDt.Text = Convert.ToString(dt.Rows[0]["InsCrEndDt"]);
                        txtPrStrDt.Text = Convert.ToString(dt.Rows[0]["InsPrStDt"]);
                        txtPrEndDt.Text = Convert.ToString(dt.Rows[0]["InsPrEndDt"]);
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["Branch"].ToString()));
                        InitialRow26();
                        InitialRow27();
                        InitialRow28();
                        InitialRow29();
                        InitialRow30();
                        InitialRow31();
                        InitialRow32();
                        InitialRow33();
                        InitialRow34();
                        ddlQ1ChkYn.SelectedIndex = ddlQ1ChkYn.Items.IndexOf(ddlQ1ChkYn.Items.FindByValue(dt.Rows[0]["Q1"].ToString()));
                        txtQ1Comment.Text = Convert.ToString(dt.Rows[0]["Q1Comment"]);
                        txtQ1DtChk.Text = Convert.ToString(dt.Rows[0]["Q1DtChk"]);
                        txtQ1SysAmt.Text = Convert.ToString(dt.Rows[0]["Q1SysAmt"]);
                        txtQ1PhyAmt.Text = Convert.ToString(dt.Rows[0]["Q1PhyAmt"]);
                        txtQ1CBAmt.Text = Convert.ToString(dt.Rows[0]["Q1CBAmt"]);
                        txtQ1Score.Text = Convert.ToString(dt.Rows[0]["Q1Score"]);
                        ddlQ2ChkYn.SelectedIndex = ddlQ2ChkYn.Items.IndexOf(ddlQ2ChkYn.Items.FindByValue(dt.Rows[0]["Q2"].ToString()));
                        txtQ2Comment.Text = Convert.ToString(dt.Rows[0]["Q2Comment"]);
                        txtQ2DtChk.Text = Convert.ToString(dt.Rows[0]["Q2DtChk"]);
                        txtQ2SysAmt.Text = Convert.ToString(dt.Rows[0]["Q2SysAmt"]);
                        txtQ2PhyAmt.Text = Convert.ToString(dt.Rows[0]["Q2PhyAmt"]);
                        txtQ2CBAmt.Text = Convert.ToString(dt.Rows[0]["Q2CBAmt"]);
                        ddlQ3ChkYn.SelectedIndex = ddlQ3ChkYn.Items.IndexOf(ddlQ3ChkYn.Items.FindByValue(dt.Rows[0]["Q3"].ToString()));
                        txtQ3Comment.Text = Convert.ToString(dt.Rows[0]["Q3Comment"]);
                        txtQ3DtChk.Text = Convert.ToString(dt.Rows[0]["Q3DtChk"]);
                        txtQ3SysAmt.Text = Convert.ToString(dt.Rows[0]["Q3SysAmt"]);
                        txtQ3PhyAmt.Text = Convert.ToString(dt.Rows[0]["Q3PhyAmt"]);
                        txtQ3CBAmt.Text = Convert.ToString(dt.Rows[0]["Q3CBAmt"]);
                        ddlQ4ChkYn.SelectedIndex = ddlQ4ChkYn.Items.IndexOf(ddlQ4ChkYn.Items.FindByValue(dt.Rows[0]["Q4"].ToString()));
                        txtQ4Score.Text = Convert.ToString(dt.Rows[0]["Q4Score"]);
                        ddlQ5Gap.SelectedIndex = ddlQ5Gap.Items.IndexOf(ddlQ5Gap.Items.FindByValue(dt.Rows[0]["Q5"].ToString()));
                        txtQ5Score.Text = Convert.ToString(dt.Rows[0]["Q5Score"]);
                        ddlQ6Gap.SelectedIndex = ddlQ6Gap.Items.IndexOf(ddlQ6Gap.Items.FindByValue(dt.Rows[0]["Q6"].ToString()));
                        txtQ6Score.Text = Convert.ToString(dt.Rows[0]["Q6Score"]);
                        ddlQ7ChkYn.SelectedIndex = ddlQ7ChkYn.Items.IndexOf(ddlQ7ChkYn.Items.FindByValue(dt.Rows[0]["Q7"].ToString()));
                        txtQ7Score.Text = Convert.ToString(dt.Rows[0]["Q7Score"]);
                        ddlQ8ChkYn.SelectedIndex = ddlQ8ChkYn.Items.IndexOf(ddlQ8ChkYn.Items.FindByValue(dt.Rows[0]["Q8"].ToString()));
                        txtQ8Score.Text = Convert.ToString(dt.Rows[0]["Q8Score"]);
                        ddlQ9Gap.SelectedIndex = ddlQ9Gap.Items.IndexOf(ddlQ9Gap.Items.FindByValue(dt.Rows[0]["Q9"].ToString()));
                        txtQ9Score.Text = Convert.ToString(dt.Rows[0]["Q9Score"]);
                        ddlQ10ChkYn.SelectedIndex = ddlQ10ChkYn.Items.IndexOf(ddlQ10ChkYn.Items.FindByValue(dt.Rows[0]["Q10"].ToString()));
                        txtQ10Score.Text = Convert.ToString(dt.Rows[0]["Q10Score"]);
                        ddlQ11ChkYn.SelectedIndex = ddlQ11ChkYn.Items.IndexOf(ddlQ11ChkYn.Items.FindByValue(dt.Rows[0]["Q11"].ToString()));
                        txtQ11Score.Text = Convert.ToString(dt.Rows[0]["Q11Score"]);
                        ddlQ12Gap.SelectedIndex = ddlQ12Gap.Items.IndexOf(ddlQ12Gap.Items.FindByValue(dt.Rows[0]["Q12"].ToString()));
                        txtQ12Score.Text = Convert.ToString(dt.Rows[0]["Q12Score"]);
                        ddlQ13Gap.SelectedIndex = ddlQ13Gap.Items.IndexOf(ddlQ13Gap.Items.FindByValue(dt.Rows[0]["Q13"].ToString()));
                        txtQ13Score.Text = Convert.ToString(dt.Rows[0]["Q13Score"]);
                        ddlQ14Gap.SelectedIndex = ddlQ14Gap.Items.IndexOf(ddlQ14Gap.Items.FindByValue(dt.Rows[0]["Q14"].ToString()));
                        txtQ14Score.Text = Convert.ToString(dt.Rows[0]["Q14Score"]);
                        ddlQ15Gap.SelectedIndex = ddlQ15Gap.Items.IndexOf(ddlQ15Gap.Items.FindByValue(dt.Rows[0]["Q15"].ToString()));
                        txtQ15Score.Text = Convert.ToString(dt.Rows[0]["Q15Score"]);
                        txtQ16Score.Text = Convert.ToString(dt.Rows[0]["Q16Score"]);
                        txtQ17Score.Text = Convert.ToString(dt.Rows[0]["Q17Score"]);
                        txtQ18Score.Text = Convert.ToString(dt.Rows[0]["Q18Score"]);
                        txtQ19Score.Text = Convert.ToString(dt.Rows[0]["Q19Score"]);
                        txtQ20Score.Text = Convert.ToString(dt.Rows[0]["Q20Score"]);
                        txtQ21Score.Text = Convert.ToString(dt.Rows[0]["Q21Score"]);
                        txtQ22Score.Text = Convert.ToString(dt.Rows[0]["Q22Score"]);
                        txtQ23Score.Text = Convert.ToString(dt.Rows[0]["Q23Score"]);
                        txtQ24Score.Text = Convert.ToString(dt.Rows[0]["Q24Score"]);
                        txtQ25Score.Text = Convert.ToString(dt.Rows[0]["Q25Score"]);
                        txtQ26Score.Text = Convert.ToString(dt.Rows[0]["Q26Score"]);
                        txtQ27Score.Text = Convert.ToString(dt.Rows[0]["Q27Score"]);
                        txtQ28Score.Text = Convert.ToString(dt.Rows[0]["Q28Score"]);
                        txtQ29Score.Text = Convert.ToString(dt.Rows[0]["Q29Score"]);
                        txtQ30Score.Text = Convert.ToString(dt.Rows[0]["Q30Score"]);
                        txtQ31Score.Text = Convert.ToString(dt.Rows[0]["Q31Score"]);
                        txtQ32Score.Text = Convert.ToString(dt.Rows[0]["Q32Score"]);
                        txtQ33Score.Text = Convert.ToString(dt.Rows[0]["Q33Score"]);
                        txtQ34Score.Text = Convert.ToString(dt.Rows[0]["Q34Score"]);
                        ddlQ16Gap.SelectedIndex = ddlQ16Gap.Items.IndexOf(ddlQ16Gap.Items.FindByValue(dt.Rows[0]["Q16"].ToString()));
                        ddlQ17ChkYn.SelectedIndex = ddlQ17ChkYn.Items.IndexOf(ddlQ17ChkYn.Items.FindByValue(dt.Rows[0]["Q17"].ToString()));
                        ddlQ18ChkYn.SelectedIndex = ddlQ18ChkYn.Items.IndexOf(ddlQ18ChkYn.Items.FindByValue(dt.Rows[0]["Q18"].ToString()));
                        ddlQ19ChkYn.SelectedIndex = ddlQ19ChkYn.Items.IndexOf(ddlQ19ChkYn.Items.FindByValue(dt.Rows[0]["Q19"].ToString()));
                        ddlQ20Gap.SelectedIndex = ddlQ20Gap.Items.IndexOf(ddlQ20Gap.Items.FindByValue(dt.Rows[0]["Q20"].ToString()));
                        ddlQ21Gap.SelectedIndex = ddlQ21Gap.Items.IndexOf(ddlQ21Gap.Items.FindByValue(dt.Rows[0]["Q21"].ToString()));
                        ddlQ22Gap.SelectedIndex = ddlQ22Gap.Items.IndexOf(ddlQ22Gap.Items.FindByValue(dt.Rows[0]["Q22"].ToString()));
                        ddlQ23ChkYn.SelectedIndex = ddlQ23ChkYn.Items.IndexOf(ddlQ23ChkYn.Items.FindByValue(dt.Rows[0]["Q23"].ToString()));
                        ddlQ24Gap.SelectedIndex = ddlQ24Gap.Items.IndexOf(ddlQ24Gap.Items.FindByValue(dt.Rows[0]["Q24"].ToString()));
                        ddlQ25ChkYn.SelectedIndex = ddlQ25ChkYn.Items.IndexOf(ddlQ25ChkYn.Items.FindByValue(dt.Rows[0]["Q25"].ToString()));


                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbEmp.ActiveTabIndex = 1;
                        StatusButton("Show");
                        EnableControl(false);
                    }

                    if (dt26.Rows.Count > 0)
                    {
                        dr26 = dt26.NewRow();
                        dr26["MarketId"] = "";
                        dr26["MemberId"] = "";
                        dr26["CurrYN"] = "";
                        dr26["SlNo"] = Convert.ToInt32(dt26.Rows[dt26.Rows.Count - 1]["SlNo"]) + 1;
                        dt26.Rows.InsertAt(dr26, dt26.Rows.Count + 1);
                        ViewState["CTable26"] = dt26;
                        gv26.DataSource = dt26;
                        gv26.DataBind();
                        SetPrevData26();
                    }

                    if (dt27.Rows.Count > 0)
                    {
                        dr27 = dt27.NewRow();
                        dr27["MarketId"] = "";
                        dr27["MemberId"] = "";
                        dr27["CurrYN"] = "";
                        dr27["SlNo"] = Convert.ToInt32(dt27.Rows[dt27.Rows.Count - 1]["SlNo"]) + 1;
                        dt27.Rows.InsertAt(dr27, dt27.Rows.Count + 1);
                        ViewState["CTable27"] = dt27;
                        gv27.DataSource = dt27;
                        gv27.DataBind();
                        SetPrevData27();
                    }

                    if (dt28.Rows.Count > 0)
                    {
                        dr28 = dt28.NewRow();
                        dr28["MarketId"] = "";
                        dr28["MemberId"] = "";
                        dr28["CurrYN"] = "";
                        dr28["SlNo"] = Convert.ToInt32(dt28.Rows[dt28.Rows.Count - 1]["SlNo"]) + 1;
                        dt28.Rows.InsertAt(dr28, dt28.Rows.Count + 1);
                        ViewState["CTable28"] = dt28;
                        gv28.DataSource = dt28;
                        gv28.DataBind();
                        SetPrevData28();
                    }


                    if (dt29.Rows.Count > 0)
                    {
                        dr29 = dt29.NewRow();
                        dr29["MarketId"] = "";
                        dr29["MemberId"] = "";
                        dr29["CurrYN"] = "";
                        dr29["SlNo"] = Convert.ToInt32(dt29.Rows[dt29.Rows.Count - 1]["SlNo"]) + 1;
                        dt29.Rows.InsertAt(dr29, dt29.Rows.Count + 1);
                        ViewState["CTable29"] = dt29;
                        gv29.DataSource = dt29;
                        gv29.DataBind();
                        SetPrevData29();
                    }

                    if (dt30.Rows.Count > 0)
                    {
                        dr30 = dt30.NewRow();
                        dr30["MarketId"] = "";
                        dr30["MemberId"] = "";
                        dr30["CurrYN"] = "";
                        dr30["SlNo"] = Convert.ToInt32(dt30.Rows[dt30.Rows.Count - 1]["SlNo"]) + 1;
                        dt30.Rows.InsertAt(dr30, dt30.Rows.Count + 1);
                        ViewState["CTable30"] = dt30;
                        gv30.DataSource = dt30;
                        gv30.DataBind();
                        SetPrevData30();
                    }
                    if (dt31.Rows.Count > 0)
                    {
                        dr31 = dt31.NewRow();
                        dr31["MarketId"] = "";
                        dr31["MemberId"] = "";
                        dr31["CurrYN"] = "";
                        dr31["SlNo"] = Convert.ToInt32(dt31.Rows[dt31.Rows.Count - 1]["SlNo"]) + 1;
                        dt31.Rows.InsertAt(dr31, dt31.Rows.Count + 1);
                        ViewState["CTable31"] = dt31;
                        gv31.DataSource = dt31;
                        gv31.DataBind();
                        SetPrevData31();
                    }
                    if (dt32.Rows.Count > 0)
                    {
                        dr32 = dt32.NewRow();
                        dr32["MarketId"] = "";
                        dr32["MemberId"] = "";
                        dr32["CurrYN"] = "";
                        dr32["SlNo"] = Convert.ToInt32(dt32.Rows[dt32.Rows.Count - 1]["SlNo"]) + 1;
                        dt32.Rows.InsertAt(dr32, dt32.Rows.Count + 1);
                        ViewState["CTable32"] = dt32;
                        gv32.DataSource = dt32;
                        gv32.DataBind();
                        SetPrevData32();
                    }
                    if (dt33.Rows.Count > 0)
                    {
                        dr33 = dt33.NewRow();
                        dr33["MarketId"] = "";
                        dr33["MemberId"] = "";
                        dr33["CurrYN"] = "";
                        dr33["SlNo"] = Convert.ToInt32(dt33.Rows[dt33.Rows.Count - 1]["SlNo"]) + 1;
                        dt33.Rows.InsertAt(dr33, dt33.Rows.Count + 1);
                        ViewState["CTable33"] = dt33;
                        gv33.DataSource = dt33;
                        gv33.DataBind();
                        SetPrevData33();
                    }
                    if (dt34.Rows.Count > 0)
                    {
                        dr34 = dt34.NewRow();
                        dr34["MarketId"] = "";
                        dr34["MemberId"] = "";
                        dr34["CurrYN"] = "";
                        dr34["SlNo"] = Convert.ToInt32(dt34.Rows[dt34.Rows.Count - 1]["SlNo"]) + 1;
                        dt34.Rows.InsertAt(dr34, dt34.Rows.Count + 1);
                        ViewState["CTable34"] = dt34;
                        gv34.DataSource = dt34;
                        gv34.DataBind();
                        SetPrevData34();
                    }

                }
            }
            finally
            {
                dt = null;
                dt26 = null;
                dt27 = null;
                dt28 = null;
                dt29 = null;
                dt30 = null;
                dt31 = null;
                dt32 = null;
                dt33 = null;
                dt34 = null;
                oAu = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv26_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtDtl = null;
            try
            {
                Int32 vRow = 0, vMaxRow = 0;
                GridViewRow Row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                vRow = Row.RowIndex;
                vMaxRow = gv26.Rows.Count;
                dtDtl = (DataTable)ViewState["CTable26"];
                if (vRow != vMaxRow - 1)
                {
                    if (e.CommandName == "cmd26DRec")
                    {
                        dtDtl.Rows.RemoveAt(vRow);
                        dtDtl.AcceptChanges();
                        ViewState["CTable26"] = dtDtl;
                        gv26.DataSource = dtDtl;
                        gv26.DataBind();
                    }
                    SetPrevData26();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtDtl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv27_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtDtl = null;
            try
            {
                Int32 vRow = 0, vMaxRow = 0;
                GridViewRow Row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                vRow = Row.RowIndex;
                vMaxRow = gv27.Rows.Count;
                dtDtl = (DataTable)ViewState["CTable27"];
                if (vRow != vMaxRow - 1)
                {
                    if (e.CommandName == "cmd27DRec")
                    {
                        dtDtl.Rows.RemoveAt(vRow);
                        dtDtl.AcceptChanges();
                        ViewState["CTable27"] = dtDtl;
                        gv27.DataSource = dtDtl;
                        gv27.DataBind();
                    }
                    SetPrevData27();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtDtl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv28_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtDtl = null;
            try
            {
                Int32 vRow = 0, vMaxRow = 0;
                GridViewRow Row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                vRow = Row.RowIndex;
                vMaxRow = gv28.Rows.Count;
                dtDtl = (DataTable)ViewState["CTable28"];
                if (vRow != vMaxRow - 1)
                {
                    if (e.CommandName == "cmd28DRec")
                    {
                        dtDtl.Rows.RemoveAt(vRow);
                        dtDtl.AcceptChanges();
                        ViewState["CTable28"] = dtDtl;
                        gv28.DataSource = dtDtl;
                        gv28.DataBind();
                    }
                    SetPrevData28();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtDtl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv29_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtDtl = null;
            try
            {
                Int32 vRow = 0, vMaxRow = 0;
                GridViewRow Row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                vRow = Row.RowIndex;
                vMaxRow = gv29.Rows.Count;
                dtDtl = (DataTable)ViewState["CTable29"];
                if (vRow != vMaxRow - 1)
                {
                    if (e.CommandName == "cmd29DRec")
                    {
                        dtDtl.Rows.RemoveAt(vRow);
                        dtDtl.AcceptChanges();
                        ViewState["CTable29"] = dtDtl;
                        gv29.DataSource = dtDtl;
                        gv29.DataBind();
                    }
                    SetPrevData29();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtDtl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv30_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtDtl = null;
            try
            {
                Int32 vRow = 0, vMaxRow = 0;
                GridViewRow Row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                vRow = Row.RowIndex;
                vMaxRow = gv30.Rows.Count;
                dtDtl = (DataTable)ViewState["CTable30"];
                if (vRow != vMaxRow - 1)
                {
                    if (e.CommandName == "cmd30DRec")
                    {
                        dtDtl.Rows.RemoveAt(vRow);
                        dtDtl.AcceptChanges();
                        ViewState["CTable30"] = dtDtl;
                        gv30.DataSource = dtDtl;
                        gv30.DataBind();
                    }
                    SetPrevData30();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtDtl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv31_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtDtl = null;
            try
            {
                Int32 vRow = 0, vMaxRow = 0;
                GridViewRow Row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                vRow = Row.RowIndex;
                vMaxRow = gv31.Rows.Count;
                dtDtl = (DataTable)ViewState["CTable31"];
                if (vRow != vMaxRow - 1)
                {
                    if (e.CommandName == "cmd31DRec")
                    {
                        dtDtl.Rows.RemoveAt(vRow);
                        dtDtl.AcceptChanges();
                        ViewState["CTable31"] = dtDtl;
                        gv31.DataSource = dtDtl;
                        gv31.DataBind();
                    }
                    SetPrevData31();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtDtl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv32_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtDtl = null;
            try
            {
                Int32 vRow = 0, vMaxRow = 0;
                GridViewRow Row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                vRow = Row.RowIndex;
                vMaxRow = gv32.Rows.Count;
                dtDtl = (DataTable)ViewState["CTable32"];
                if (vRow != vMaxRow - 1)
                {
                    if (e.CommandName == "cmd32DRec")
                    {
                        dtDtl.Rows.RemoveAt(vRow);
                        dtDtl.AcceptChanges();
                        ViewState["CTable32"] = dtDtl;
                        gv32.DataSource = dtDtl;
                        gv32.DataBind();
                    }
                    SetPrevData32();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtDtl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv33_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtDtl = null;
            try
            {
                Int32 vRow = 0, vMaxRow = 0;
                GridViewRow Row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                vRow = Row.RowIndex;
                vMaxRow = gv33.Rows.Count;
                dtDtl = (DataTable)ViewState["CTable33"];
                if (vRow != vMaxRow - 1)
                {
                    if (e.CommandName == "cmd33DRec")
                    {
                        dtDtl.Rows.RemoveAt(vRow);
                        dtDtl.AcceptChanges();
                        ViewState["CTable33"] = dtDtl;
                        gv33.DataSource = dtDtl;
                        gv33.DataBind();
                    }
                    SetPrevData33();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtDtl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv34_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtDtl = null;
            try
            {
                Int32 vRow = 0, vMaxRow = 0;
                GridViewRow Row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                vRow = Row.RowIndex;
                vMaxRow = gv34.Rows.Count;
                dtDtl = (DataTable)ViewState["CTable34"];
                if (vRow != vMaxRow - 1)
                {
                    if (e.CommandName == "cmd34DRec")
                    {
                        dtDtl.Rows.RemoveAt(vRow);
                        dtDtl.AcceptChanges();
                        ViewState["CTable34"] = dtDtl;
                        gv34.DataSource = dtDtl;
                        gv34.DataBind();
                    }
                    SetPrevData34();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtDtl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            string vXml26 = "", vXml27 = "", vXml28 = "", vXml29 = "", vXml30 = "", vXml31 = "", vXml32 = "", vXml33 = "", vXml34 = "";
            double Q1SysAmt = 0, Q1PhyAmt = 0, Q1CBAmt = 0, Q1Score = 0, Q2SysAmt = 0, Q2PhyAmt = 0, Q2CBAmt = 0, Q3SysAmt = 0, Q3PhyAmt = 0, Q3CBAmt = 0;
            double Q4Score = 0, Q5Score = 0, Q6Score = 0, Q7Score = 0, Q8Score = 0, Q9Score = 0, Q10Score = 0, Q11Score = 0, Q12Score = 0, Q13Score = 0, Q14Score = 0;
            double Q15Score = 0, Q16Score = 0, Q17Score = 0, Q18Score = 0, Q19Score = 0, Q20Score = 0, Q21Score = 0, Q22Score = 0, Q23Score = 0, Q24Score = 0, Q25Score = 0;
            double Q26Score = 0, Q27Score = 0, Q28Score = 0, Q29Score = 0, Q30Score = 0, Q31Score = 0, Q32Score = 0, Q33Score = 0, Q34Score = 0;
            DataTable dt26 = new DataTable();
            DataTable dt27 = new DataTable();
            DataTable dt28 = new DataTable();
            DataTable dt29 = new DataTable();
            DataTable dt30 = new DataTable();
            DataTable dt31 = new DataTable();
            DataTable dt32 = new DataTable();
            DataTable dt33 = new DataTable();
            DataTable dt34 = new DataTable();
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vInsPecId = Convert.ToString(ViewState["InsPecId"]);
            Int32 vErr = 0;
            DateTime vSubDt = gblFuction.setDate(txtSDt.Text);
            DateTime vCrStrDt = gblFuction.setDate(txtCrStrDt.Text);
            DateTime vCrEndDt = gblFuction.setDate(txtCrEndDt.Text);
            DateTime vPrStrDt = gblFuction.setDate(txtPrStrDt.Text);
            DateTime vPrEndDt = gblFuction.setDate(txtPrEndDt.Text);
            DateTime vQ1DtChk = gblFuction.setDate(txtQ1DtChk.Text);
            DateTime vQ2DtChk = gblFuction.setDate(txtQ2DtChk.Text);
            DateTime vQ3DtChk = gblFuction.setDate(txtQ3DtChk.Text);
            
            CAudit oAu = null;

            try
            {
                if (txtQ1SysAmt.Text !="") Q1SysAmt = Convert.ToDouble(txtQ1SysAmt.Text);
                if (txtQ1PhyAmt.Text != "") Q1PhyAmt = Convert.ToDouble(txtQ1PhyAmt.Text);
                if (txtQ1CBAmt.Text != "") Q1CBAmt = Convert.ToDouble(txtQ1CBAmt.Text);
                if (txtQ1Score.Text != "") Q1Score = Convert.ToDouble(txtQ1Score.Text);
                if (txtQ2SysAmt.Text != "") Q2SysAmt = Convert.ToDouble(txtQ2SysAmt.Text);
                if (txtQ2PhyAmt.Text != "") Q2PhyAmt = Convert.ToDouble(txtQ2PhyAmt.Text);
                if (txtQ2CBAmt.Text != "") Q2CBAmt = Convert.ToDouble(txtQ2CBAmt.Text);
                if (txtQ3SysAmt.Text != "") Q3SysAmt = Convert.ToDouble(txtQ3SysAmt.Text);
                if (txtQ3PhyAmt.Text != "") Q3PhyAmt = Convert.ToDouble(txtQ3PhyAmt.Text);
                if (txtQ3CBAmt.Text != "") Q3CBAmt = Convert.ToDouble(txtQ3CBAmt.Text);
                if (txtQ4Score.Text != "") Q4Score = Convert.ToDouble(txtQ4Score.Text);
                if (txtQ5Score.Text != "") Q5Score = Convert.ToDouble(txtQ5Score.Text);
                if (txtQ6Score.Text != "") Q6Score = Convert.ToDouble(txtQ6Score.Text);
                if (txtQ7Score.Text != "") Q7Score = Convert.ToDouble(txtQ7Score.Text);
                if (txtQ8Score.Text != "") Q8Score = Convert.ToDouble(txtQ8Score.Text);
                if (txtQ9Score.Text != "") Q9Score = Convert.ToDouble(txtQ9Score.Text);
                if (txtQ10Score.Text != "") Q10Score = Convert.ToDouble(txtQ10Score.Text);
                if (txtQ11Score.Text != "") Q11Score = Convert.ToDouble(txtQ11Score.Text);
                if (txtQ12Score.Text != "") Q12Score = Convert.ToDouble(txtQ12Score.Text);
                if (txtQ13Score.Text != "") Q13Score = Convert.ToDouble(txtQ13Score.Text);
                if (txtQ14Score.Text != "") Q14Score = Convert.ToDouble(txtQ14Score.Text);
                if (txtQ15Score.Text != "") Q15Score = Convert.ToDouble(txtQ15Score.Text);
                if (txtQ16Score.Text != "") Q16Score = Convert.ToDouble(txtQ16Score.Text);
                if (txtQ17Score.Text != "") Q17Score = Convert.ToDouble(txtQ17Score.Text);
                if (txtQ18Score.Text != "") Q18Score = Convert.ToDouble(txtQ18Score.Text);
                if (txtQ19Score.Text != "") Q19Score = Convert.ToDouble(txtQ19Score.Text);
                if (txtQ20Score.Text != "") Q20Score = Convert.ToDouble(txtQ20Score.Text);
                if (txtQ21Score.Text != "") Q21Score = Convert.ToDouble(txtQ21Score.Text);
                if (txtQ22Score.Text != "") Q22Score = Convert.ToDouble(txtQ22Score.Text);
                if (txtQ23Score.Text != "") Q23Score = Convert.ToDouble(txtQ23Score.Text);
                if (txtQ24Score.Text != "") Q24Score = Convert.ToDouble(txtQ24Score.Text);
                if (txtQ25Score.Text != "") Q25Score = Convert.ToDouble(txtQ25Score.Text);

                if (txtQ26Score.Text != "") Q26Score = Convert.ToDouble(txtQ26Score.Text);
                if (txtQ27Score.Text != "") Q27Score = Convert.ToDouble(txtQ27Score.Text);
                if (txtQ28Score.Text != "") Q28Score = Convert.ToDouble(txtQ28Score.Text);
                if (txtQ29Score.Text != "") Q29Score = Convert.ToDouble(txtQ29Score.Text);
                if (txtQ30Score.Text != "") Q30Score = Convert.ToDouble(txtQ30Score.Text);
                if (txtQ31Score.Text != "") Q31Score = Convert.ToDouble(txtQ31Score.Text);
                if (txtQ32Score.Text != "") Q32Score = Convert.ToDouble(txtQ32Score.Text);
                if (txtQ33Score.Text != "") Q33Score = Convert.ToDouble(txtQ33Score.Text);
                if (txtQ34Score.Text != "") Q34Score = Convert.ToDouble(txtQ34Score.Text);


                dt26 = (DataTable)ViewState["CTable26"];
                if (dt26 == null) return false;
                dt27 = (DataTable)ViewState["CTable27"];
                if (dt27 == null) return false;
                dt28 = (DataTable)ViewState["CTable28"];
                if (dt28 == null) return false;
                dt29 = (DataTable)ViewState["CTable29"];
                if (dt29 == null) return false;
                dt30 = (DataTable)ViewState["CTable30"];
                if (dt30 == null) return false;
                dt31 = (DataTable)ViewState["CTable31"];
                if (dt31 == null) return false;
                dt32 = (DataTable)ViewState["CTable32"];
                if (dt32 == null) return false;
                dt33 = (DataTable)ViewState["CTable33"];
                if (dt33 == null) return false;
                dt34 = (DataTable)ViewState["CTable34"];
                if (dt34 == null) return false;

                if (Mode == "Save")
                {
                    oAu = new CAudit();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("EOMst", "EmpCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Save");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("EMP Code can not be Duplicate...");
                    //    return false;
                    //}
                    if (ValidateDetail() == false) return false;
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt26.WriteXml(oSW);
                        vXml26 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt27.WriteXml(oSW);
                        vXml27 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt28.WriteXml(oSW);
                        vXml28 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt29.WriteXml(oSW);
                        vXml29 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt30.WriteXml(oSW);
                        vXml30 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt31.WriteXml(oSW);
                        vXml31 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt32.WriteXml(oSW);
                        vXml32 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt33.WriteXml(oSW);
                        vXml33 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt34.WriteXml(oSW);
                        vXml34 = oSW.ToString();
                    }
                    //vXml = DtToXml(DtBranch());
                    vErr = oAu.InsertAuditSubmission(ref vInsPecId, vSubDt, vCrStrDt, vCrEndDt, vPrStrDt, vPrEndDt, ddlBranch.SelectedValue,
                                ddlQ1ChkYn.SelectedValue, txtQ1Comment.Text.Replace("'", "''"), vQ1DtChk, Q1SysAmt, Q1PhyAmt, Q1CBAmt, Q1Score,
                                ddlQ2ChkYn.SelectedValue, txtQ2Comment.Text.Replace("'", "''"), vQ2DtChk, Q2SysAmt, Q2PhyAmt, Q2CBAmt, 
                                ddlQ3ChkYn.SelectedValue, txtQ3Comment.Text.Replace("'", "''"), vQ3DtChk, Q3SysAmt, Q3PhyAmt, Q3CBAmt, 
                                ddlQ4ChkYn.SelectedValue, Q4Score, ddlQ5Gap.SelectedValue, Q5Score, ddlQ6Gap.SelectedValue, Q6Score,
                                ddlQ7ChkYn.SelectedValue, Q7Score, ddlQ8ChkYn.SelectedValue, Q8Score, ddlQ9Gap.SelectedValue, Q9Score,
                                ddlQ10ChkYn.SelectedValue, Q10Score, ddlQ11ChkYn.SelectedValue, Q11Score, ddlQ12Gap.SelectedValue, Q12Score,
                                ddlQ13Gap.SelectedValue, Q13Score, ddlQ14Gap.SelectedValue, Q14Score, ddlQ15Gap.SelectedValue, Q15Score,
                                ddlQ16Gap.SelectedValue, Q16Score, ddlQ17ChkYn.SelectedValue, Q17Score, ddlQ18ChkYn.SelectedValue, Q18Score,
                                ddlQ19ChkYn.SelectedValue, Q19Score, ddlQ20Gap.SelectedValue, Q20Score, ddlQ21Gap.SelectedValue, Q21Score,
                                ddlQ22Gap.SelectedValue, Q22Score, ddlQ23ChkYn.SelectedValue, Q23Score, ddlQ24Gap.SelectedValue, Q24Score,
                                ddlQ24Gap.SelectedValue, Q25Score, Q26Score, Q27Score, Q28Score, Q29Score, Q30Score, Q31Score, Q32Score,
                                Q33Score, Q34Score, vBrCode, this.UserID, "Save", vXml26, vXml27, vXml28, vXml29, vXml30, vXml31, vXml32, vXml33, vXml34);
                    if (vErr > 0)
                    {
                        ViewState["InsPecId"] = vInsPecId;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    if (ValidateDetail() == false) return false;
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt26.WriteXml(oSW);
                        vXml26 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt27.WriteXml(oSW);
                        vXml27 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt28.WriteXml(oSW);
                        vXml28 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt29.WriteXml(oSW);
                        vXml29 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt30.WriteXml(oSW);
                        vXml30 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt31.WriteXml(oSW);
                        vXml31 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt32.WriteXml(oSW);
                        vXml32 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt33.WriteXml(oSW);
                        vXml33 = oSW.ToString();
                    }
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt34.WriteXml(oSW);
                        vXml34 = oSW.ToString();
                    }
                    oAu = new CAudit();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("EOMst", "EMPCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Edit");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("EMP Code Can not be Duplicate...");
                    //    return false;
                    //}

                    vErr = oAu.InsertAuditSubmission(ref vInsPecId, vSubDt, vCrStrDt, vCrEndDt, vPrStrDt, vPrEndDt, ddlBranch.SelectedValue,
                                ddlQ1ChkYn.SelectedValue, txtQ1Comment.Text.Replace("'", "''"), vQ1DtChk, Q1SysAmt, Q1PhyAmt, Q1CBAmt, Q1Score,
                                ddlQ2ChkYn.SelectedValue, txtQ2Comment.Text.Replace("'", "''"), vQ2DtChk, Q2SysAmt, Q2PhyAmt, Q2CBAmt, 
                                ddlQ3ChkYn.SelectedValue, txtQ3Comment.Text.Replace("'", "''"), vQ3DtChk, Q3SysAmt, Q3PhyAmt, Q3CBAmt, 
                                ddlQ4ChkYn.SelectedValue, Q4Score, ddlQ5Gap.SelectedValue, Q5Score, ddlQ6Gap.SelectedValue, Q6Score,
                                ddlQ7ChkYn.SelectedValue, Q7Score, ddlQ8ChkYn.SelectedValue, Q8Score, ddlQ9Gap.SelectedValue, Q9Score,
                                ddlQ10ChkYn.SelectedValue, Q10Score, ddlQ11ChkYn.SelectedValue, Q11Score, ddlQ12Gap.SelectedValue, Q12Score,
                                ddlQ13Gap.SelectedValue, Q13Score, ddlQ14Gap.SelectedValue, Q14Score, ddlQ15Gap.SelectedValue, Q15Score,
                                ddlQ16Gap.SelectedValue, Q16Score, ddlQ17ChkYn.SelectedValue, Q17Score, ddlQ18ChkYn.SelectedValue, Q18Score,
                                ddlQ19ChkYn.SelectedValue, Q19Score, ddlQ20Gap.SelectedValue, Q20Score, ddlQ21Gap.SelectedValue, Q21Score,
                                ddlQ22Gap.SelectedValue, Q22Score, ddlQ23ChkYn.SelectedValue, Q23Score, ddlQ24Gap.SelectedValue, Q24Score,
                                ddlQ24Gap.SelectedValue, Q25Score, Q26Score, Q27Score, Q28Score, Q29Score, Q30Score, Q31Score, Q32Score,
                                Q33Score, Q34Score, vBrCode, this.UserID, "Edit", vXml26, vXml27, vXml28, vXml29, vXml30, vXml31, vXml32, vXml33, vXml34);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oAu = new CAudit();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDeleteString(vEoId, "EOID", "MarketMSt");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("The RO has group, you can not delete the RO.");
                    //    return false;
                    //}
                    //oEo = new CEO();
                    vErr = oAu.InsertAuditSubmission(ref vInsPecId, vSubDt, vCrStrDt, vCrEndDt, vPrStrDt, vPrEndDt, ddlBranch.SelectedValue,
                                ddlQ1ChkYn.SelectedValue, txtQ1Comment.Text.Replace("'", "''"), vQ1DtChk, Q1SysAmt, Q1PhyAmt, Q1CBAmt, Q1Score,
                                ddlQ2ChkYn.SelectedValue, txtQ2Comment.Text.Replace("'", "''"), vQ2DtChk, Q2SysAmt, Q2PhyAmt, Q2CBAmt, 
                                ddlQ3ChkYn.SelectedValue, txtQ3Comment.Text.Replace("'", "''"), vQ3DtChk, Q3SysAmt, Q3PhyAmt, Q3CBAmt, 
                                ddlQ4ChkYn.SelectedValue, Q4Score, ddlQ5Gap.SelectedValue, Q5Score, ddlQ6Gap.SelectedValue, Q6Score,
                                ddlQ7ChkYn.SelectedValue, Q7Score, ddlQ8ChkYn.SelectedValue, Q8Score, ddlQ9Gap.SelectedValue, Q9Score,
                                ddlQ10ChkYn.SelectedValue, Q10Score, ddlQ11ChkYn.SelectedValue, Q11Score, ddlQ12Gap.SelectedValue, Q12Score,
                                ddlQ13Gap.SelectedValue, Q13Score, ddlQ14Gap.SelectedValue, Q14Score, ddlQ15Gap.SelectedValue, Q15Score,
                                ddlQ16Gap.SelectedValue, Q16Score, ddlQ17ChkYn.SelectedValue, Q17Score, ddlQ18ChkYn.SelectedValue, Q18Score,
                                ddlQ19ChkYn.SelectedValue, Q19Score, ddlQ20Gap.SelectedValue, Q20Score, ddlQ21Gap.SelectedValue, Q21Score,
                                ddlQ22Gap.SelectedValue, Q22Score, ddlQ23ChkYn.SelectedValue, Q23Score, ddlQ24Gap.SelectedValue, Q24Score,
                                ddlQ24Gap.SelectedValue, Q25Score, Q26Score, Q27Score, Q28Score, Q29Score, Q30Score, Q31Score, Q32Score,
                                Q33Score, Q34Score, vBrCode, this.UserID, "Delet", vXml26, vXml27, vXml28, vXml29, vXml30, vXml31, vXml32, vXml33, vXml34);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.DeleteMsg);
                        vResult = true;
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
                oAu = null;
                //oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateDetail()//To Check
        {
            Boolean vResult = true;
            Int32 vRow = 0;
            for (vRow = 0; vRow < gv26.Rows.Count; vRow++)
            {
                DropDownList ddl26center = (DropDownList)gv26.Rows[vRow].FindControl("ddl26center");
                DropDownList ddl26Mem = (DropDownList)gv26.Rows[vRow].FindControl("ddl26Mem");
                DropDownList ddl26Opt = (DropDownList)gv26.Rows[vRow].FindControl("ddl26Opt");

                if (ddl26center.SelectedIndex > 0 && ddl26Mem.SelectedIndex <= 0)
                {
                    gblFuction.MsgPopup("Please Enter member");
                    vResult = false;
                }

                if (ddl26center.SelectedIndex > 0 && ddl26Opt.SelectedIndex <= 0)
                {
                    gblFuction.MsgPopup("Please Correct YN");
                    vResult = false;
                }

                if (ddl26Mem.SelectedIndex > 0 && ddl26center.SelectedIndex <= 0)
                {
                    gblFuction.MsgPopup("Please Center");
                    vResult = false;
                }
                if (ddl26Mem.SelectedIndex > 0 && ddl26Opt.SelectedIndex <= 0)
                {
                    gblFuction.MsgPopup("Please Y/N");
                    vResult = false;
                }
                if (ddl26Opt.SelectedIndex > 0 && ddl26center.SelectedIndex <= 0)
                {
                    gblFuction.MsgPopup("Please Select Center");
                    vResult = false;
                }
                if (ddl26Opt.SelectedIndex > 0 && ddl26Mem.SelectedIndex <= 0)
                {
                    gblFuction.MsgPopup("Please Select Member");
                    vResult = false;
                }

            }

            return vResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtQ1SysAmt_TextChanged(object sender, EventArgs e)
        {
            if (txtQ1SysAmt.Text !="" && txtQ1PhyAmt.Text!="")
            {
                if (Convert.ToDouble(txtQ1SysAmt.Text) == Convert.ToDouble(txtQ1PhyAmt.Text))
                    txtQ1Score.Text = "5";
                else
                    txtQ1Score.Text = "0";
            }
            else
                txtQ1Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtQ1PhyAmt_TextChanged(object sender, EventArgs e)
        {
            if (txtQ1SysAmt.Text != "" && txtQ1PhyAmt.Text != "")
            {
                if (Convert.ToDouble(txtQ1SysAmt.Text) == Convert.ToDouble(txtQ1PhyAmt.Text))
                    txtQ1Score.Text = "5";
                else
                    txtQ1Score.Text = "0";
            }
            else
                txtQ1Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ4ChkYn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ4ChkYn.SelectedIndex > 0)
            {
                if (ddlQ4ChkYn.SelectedValue == "Y")
                    txtQ4Score.Text = "5";
                else
                    txtQ4Score.Text = "0";
            }
            else
                txtQ4Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ5Gap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ5Gap.SelectedIndex > 0)
            {
                if (ddlQ5Gap.SelectedValue == "G")
                    txtQ5Score.Text = "5";
                else if (ddlQ5Gap.SelectedValue == "A")
                    txtQ5Score.Text = "3";
                else if (ddlQ5Gap.SelectedValue == "P")
                    txtQ5Score.Text = "1";
            }
            else
                txtQ5Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ6Gap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ6Gap.SelectedIndex > 0)
            {
                if (ddlQ6Gap.SelectedValue == "G")
                    txtQ6Score.Text = "5";
                else if (ddlQ6Gap.SelectedValue == "A")
                    txtQ6Score.Text = "3";
                else if (ddlQ6Gap.SelectedValue == "P")
                    txtQ6Score.Text = "1";
            }
            else
                txtQ6Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ7ChkYn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ7ChkYn.SelectedIndex > 0)
            {
                if (ddlQ7ChkYn.SelectedValue == "Y")
                    txtQ7Score.Text = "5";
                else
                    txtQ7Score.Text = "0";
            }
            else
                txtQ7Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ8ChkYn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ8ChkYn.SelectedIndex > 0)
            {
                if (ddlQ8ChkYn.SelectedValue == "Y")
                    txtQ8Score.Text = "5";
                else
                    txtQ8Score.Text = "0";
            }
            else
                txtQ8Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ9Gap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ9Gap.SelectedIndex > 0)
            {
                if (ddlQ9Gap.SelectedValue == "G")
                    txtQ9Score.Text = "5";
                else if (ddlQ9Gap.SelectedValue == "A")
                    txtQ9Score.Text = "3";
                else if (ddlQ9Gap.SelectedValue == "P")
                    txtQ9Score.Text = "1";
            }
            else
                txtQ9Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ10ChkYn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ10ChkYn.SelectedIndex > 0)
            {
                if (ddlQ10ChkYn.SelectedValue == "Y")
                    txtQ10Score.Text = "5";
                else
                    txtQ10Score.Text = "0";
            }
            else
                txtQ10Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ11ChkYn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ11ChkYn.SelectedIndex > 0)
            {
                if (ddlQ11ChkYn.SelectedValue == "Y")
                    txtQ11Score.Text = "5";
                else
                    txtQ11Score.Text = "0";
            }
            else
                txtQ11Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ12Gap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ12Gap.SelectedIndex > 0)
            {
                if (ddlQ12Gap.SelectedValue == "G")
                    txtQ12Score.Text = "5";
                else if (ddlQ12Gap.SelectedValue == "A")
                    txtQ12Score.Text = "3";
                else if (ddlQ12Gap.SelectedValue == "P")
                    txtQ12Score.Text = "1";
            }
            else
                txtQ12Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ13Gap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ13Gap.SelectedIndex > 0)
            {
                if (ddlQ13Gap.SelectedValue == "G")
                    txtQ13Score.Text = "5";
                else if (ddlQ13Gap.SelectedValue == "A")
                    txtQ13Score.Text = "3";
                else if (ddlQ13Gap.SelectedValue == "P")
                    txtQ13Score.Text = "1";
            }
            else
                txtQ13Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ14Gap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ14Gap.SelectedIndex > 0)
            {
                if (ddlQ14Gap.SelectedValue == "G")
                    txtQ14Score.Text = "5";
                else if (ddlQ14Gap.SelectedValue == "A")
                    txtQ14Score.Text = "3";
                else if (ddlQ14Gap.SelectedValue == "P")
                    txtQ14Score.Text = "1";
            }
            else
                txtQ14Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ15Gap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ15Gap.SelectedIndex > 0)
            {
                if (ddlQ15Gap.SelectedValue == "G")
                    txtQ15Score.Text = "5";
                else if (ddlQ15Gap.SelectedValue == "A")
                    txtQ15Score.Text = "3";
                else if (ddlQ15Gap.SelectedValue == "P")
                    txtQ15Score.Text = "1";
            }
            else
                txtQ15Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ16Gap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ16Gap.SelectedIndex > 0)
            {
                if (ddlQ16Gap.SelectedValue == "G")
                    txtQ16Score.Text = "5";
                else if (ddlQ16Gap.SelectedValue == "A")
                    txtQ16Score.Text = "3";
                else if (ddlQ16Gap.SelectedValue == "P")
                    txtQ16Score.Text = "1";
            }
            else
                txtQ16Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ17ChkYn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ17ChkYn.SelectedIndex > 0)
            {
                if (ddlQ17ChkYn.SelectedValue == "Y")
                    txtQ17Score.Text = "5";
                else
                    txtQ17Score.Text = "0";
            }
            else
                txtQ17Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ18ChkYn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ18ChkYn.SelectedIndex > 0)
            {
                if (ddlQ18ChkYn.SelectedValue == "Y")
                    txtQ18Score.Text = "5";
                else
                    txtQ18Score.Text = "0";
            }
            else
                txtQ18Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ19ChkYn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ19ChkYn.SelectedIndex > 0)
            {
                if (ddlQ19ChkYn.SelectedValue == "Y")
                    txtQ19Score.Text = "5";
                else
                    txtQ19Score.Text = "0";
            }
            else
                txtQ19Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ20Gap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ20Gap.SelectedIndex > 0)
            {
                if (ddlQ20Gap.SelectedValue == "G")
                    txtQ20Score.Text = "5";
                else if (ddlQ20Gap.SelectedValue == "A")
                    txtQ20Score.Text = "3";
                else if (ddlQ20Gap.SelectedValue == "P")
                    txtQ20Score.Text = "1";
            }
            else
                txtQ20Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ21Gap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ21Gap.SelectedIndex > 0)
            {
                if (ddlQ21Gap.SelectedValue == "G")
                    txtQ21Score.Text = "5";
                else if (ddlQ21Gap.SelectedValue == "A")
                    txtQ21Score.Text = "3";
                else if (ddlQ21Gap.SelectedValue == "P")
                    txtQ21Score.Text = "1";
            }
            else
                txtQ21Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ22Gap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ22Gap.SelectedIndex > 0)
            {
                if (ddlQ22Gap.SelectedValue == "G")
                    txtQ22Score.Text = "5";
                else if (ddlQ22Gap.SelectedValue == "A")
                    txtQ22Score.Text = "3";
                else if (ddlQ22Gap.SelectedValue == "P")
                    txtQ22Score.Text = "1";
            }
            else
                txtQ22Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ23ChkYn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ23ChkYn.SelectedIndex > 0)
            {
                if (ddlQ23ChkYn.SelectedValue == "Y")
                    txtQ23Score.Text = "5";
                else
                    txtQ23Score.Text = "0";
            }
            else
                txtQ23Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ24Gap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ24Gap.SelectedIndex > 0)
            {
                if (ddlQ24Gap.SelectedValue == "G")
                    txtQ24Score.Text = "5";
                else if (ddlQ24Gap.SelectedValue == "A")
                    txtQ24Score.Text = "3";
                else if (ddlQ24Gap.SelectedValue == "P")
                    txtQ24Score.Text = "1";
            }
            else
                txtQ24Score.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ25ChkYn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ25ChkYn.SelectedIndex > 0)
            {
                if (ddlQ25ChkYn.SelectedValue == "Y")
                    txtQ25Score.Text = "5";
                else
                    txtQ25Score.Text = "0";
            }
            else
                txtQ25Score.Text = "0";
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitialRow26();
            InitialRow27();
            InitialRow28();
            InitialRow29();
            InitialRow30();
            InitialRow31();
            InitialRow32();
            InitialRow33();
            InitialRow34();
        }

    }
}
