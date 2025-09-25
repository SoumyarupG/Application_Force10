using System;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;


namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class FundAllocation : CENTRUMBase
    {
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
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    string vFDt = Session[gblValue.FinFromDt].ToString();
                    string vTDt = Session[gblValue.FinToDt].ToString();
                    ViewState["StateEdit"] = null;
                    PopMY(vFDt, vTDt);
                }
                else
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Fund Allocation", false);
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
                this.PageHeading = "Fund Allocation";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuFndAlloc);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = true;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                    btnDone.Visible = true;
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Fund Allocation", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFDt"></param>
        /// <param name="pTDt"></param>
        private void PopMY(string pFDt, string pTDt)
        {
            try
            {
                DateTime vFrDt = gblFuction.setDate(pFDt);
                DateTime vToDt = gblFuction.setDate(pTDt);
                ddlMY.Items.Clear();
                while (vToDt > vFrDt)
                {
                    string vLsTxt = vToDt.ToString("MMMM") + "-" + vToDt.Year.ToString();
                    string vLsVal = vToDt.ToString("MM") + "/" + vToDt.Year.ToString();
                    ListItem oL = new ListItem(vLsTxt, vLsVal);
                    ddlMY.Items.Insert(0, oL);
                    vToDt = vToDt.AddMonths(-1);
                }
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlMY.Items.Insert(0, oLi);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetInitialRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("SlNo", typeof(int)));
            dt.Columns.Add(new DataColumn("Particular", typeof(string)));
            dt.Columns.Add(new DataColumn("FADate", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(double)));
            dt.Columns.Add(new DataColumn("FAType", typeof(string)));

            dr = dt.NewRow();
            dr["SlNo"] = 1;
            dr["Particular"] = "";
            dr["FADate"] = "";
            dr["Amount"] = 0;
            dr["FAType"] = "";
            dt.Rows.Add(dr);
            //Store the DataTable in ViewState
            ViewState["CurrentTable"] = dt;
            gvSrc.DataSource = dt;
            gvSrc.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetPreviousData()
        {
            int rowIndex = 0;

            try
            {
                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dt = (DataTable)ViewState["CurrentTable"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TextBox txtPar = (TextBox)gvSrc.Rows[i].FindControl("txtPar");
                            TextBox txtFADt = (TextBox)gvSrc.Rows[i].FindControl("txtFADt");
                            TextBox txtAmt = (TextBox)gvSrc.Rows[i].FindControl("txtAmt");
                            DropDownList ddlOpt = (DropDownList)gvSrc.Rows[i].FindControl("ddlOpt");

                            if (i < dt.Rows.Count - 1)
                            {
                                //Set the Previous Selected Items on Each DropDownList  on Postbacks
                                txtPar.Text = dt.Rows[i]["Particular"].ToString();
                                txtFADt.Text = dt.Rows[i]["FADate"].ToString();
                                txtAmt.Text = dt.Rows[i]["Amount"].ToString();
                                ddlOpt.ClearSelection();
                                ddlOpt.SelectedIndex = ddlOpt.Items.IndexOf(ddlOpt.Items.FindByValue(dt.Rows[i]["FAType"].ToString()));//.Selected = true;
                            }
                            rowIndex++;
                        }
                    }
                    ViewState["CurrentTable"] = dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddNewRowToGrid(Int32 vI)
        {
            try
            {
                if (ViewState["CurrentTable"] != null)
                {
                    
                        DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                        DataRow drCurrentRow = null;
                    
                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        if (vI == 1)
                        {
                            drCurrentRow = dtCurrentTable.NewRow();
                            dtCurrentTable.Rows.Add(drCurrentRow);
                        }
                        //Store the current data to ViewState for future reference
                        ViewState["CurrentTable"] = dtCurrentTable;
                        for (int i = 0; i < dtCurrentTable.Rows.Count - 1; i++)
                        {
                            TextBox txtPar = (TextBox)gvSrc.Rows[i].FindControl("txtPar");
                            TextBox txtFADt = (TextBox)gvSrc.Rows[i].FindControl("txtFADt");
                            TextBox txtAmt = (TextBox)gvSrc.Rows[i].FindControl("txtAmt");
                            DropDownList ddlOpt = (DropDownList)gvSrc.Rows[i].FindControl("ddlOpt");

                            dtCurrentTable.Rows[i]["Particular"] = txtPar.Text;
                            dtCurrentTable.Rows[i]["FADate"] = txtFADt.Text;
                            dtCurrentTable.Rows[i]["Amount"] = txtAmt.Text;
                            // Update the DataRow with the DDL Selected Items
                            dtCurrentTable.Rows[i]["FAType"] = ddlOpt.SelectedValue;
                        }
                        //Rebind the Grid with the current data to reflect changes
                        gvSrc.DataSource = dtCurrentTable;
                        gvSrc.DataBind();
                    }
                }
                //Set Previous Data on Postbacks
                SetPreviousData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }


        protected void ddlMY_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            DataRow dr;
            CFA oFA = null;
            string strDate ="01/" + ddlMY.SelectedValue;
            DateTime vFDt = gblFuction.setDate(strDate);
            DateTime vTDt = vFDt.AddMonths(1);
            vTDt = vTDt.AddDays(-(vTDt.Day));
            SetInitialRow();
            try
            {
                if (ddlMY.SelectedIndex > 0)
                {
                    oFA = new CFA();
                    dt = oFA.GetMonthWiseFA(vFDt, vTDt);
                    if (dt.Rows.Count > 0)
                    {
                        dr = dt.NewRow();
                        dr["Particular"] = "";
                        dr["FADate"] = "";
                        dr["Amount"] = 0;
                        dr["FAType"] = "";
                        dt.Rows.InsertAt(dr, dt.Rows.Count + 1);
                        ViewState["CurrentTable"] = dt;
                        gvSrc.DataSource = dt;
                        gvSrc.DataBind();
                        SetPreviousData();
                    }
                }
                else
                    return;
            }
            finally
            {
                dt = null;
                oFA = null;
            }
        }


        protected void ddlOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowindex = 0, vMaxRow = 0;
            DropDownList ddlOp = (DropDownList)sender;
            GridViewRow gvr = (GridViewRow)ddlOp.NamingContainer;

            rowindex = gvr.RowIndex;
            vMaxRow = gvSrc.Rows.Count;

            TextBox txtPar = (TextBox)gvSrc.Rows[rowindex].FindControl("txtPar");
            TextBox txtFADt = (TextBox)gvSrc.Rows[rowindex].FindControl("txtFADt");
            TextBox txtAmt = (TextBox)gvSrc.Rows[rowindex].FindControl("txtAmt");
            DropDownList ddlOpt = (DropDownList)gvSrc.Rows[rowindex].FindControl("ddlOpt");

            if (rowindex == vMaxRow - 1)
            {
                if (txtPar.Text != "" && gblFuction.IsDate(txtFADt.Text) == true && ddlOpt.SelectedIndex > 0 && txtAmt.Text!="")
                {
                    if (txtAmt.Text == "0")
                    {
                        gblFuction.AjxMsgPopup("Please feed all the Details");
                        txtAmt.Focus();
                        return;
                    }
                    AddNewRowToGrid(1);
                }
                else
                {
                    gblFuction.AjxMsgPopup("Please feed all the Details");
                    ddlOpt.Focus();
                    return;
                }
            }
        }


        protected void txtFADt_TextChanged(object sender, EventArgs e)
        {
            int rowindex = 0, vMaxRow = 0;
            TextBox txtVal = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)txtVal.NamingContainer;
            rowindex = gvr.RowIndex;
            vMaxRow = gvSrc.Rows.Count;

            TextBox txtFADt = (TextBox)gvSrc.Rows[rowindex].FindControl("txtFADt");

            if (gblFuction.IsDate(txtFADt.Text) == false)
            {
                gblFuction.AjxMsgPopup("Start Date is invalid");
                return;
            }
            string strDate = "01/" + ddlMY.SelectedValue;
            DateTime vFDt = gblFuction.setDate(strDate);
            DateTime vTDt = vFDt.AddMonths(1);
            vTDt = vTDt.AddDays(-(vTDt.Day));

            if (gblFuction.setDate(txtFADt.Text) < vFDt || gblFuction.setDate(txtFADt.Text) > vTDt)
            {
                gblFuction.AjxMsgPopup("Date Should be within " + ddlMY.SelectedItem.Text);
                txtFADt.Text = "";
                return;
            }
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            string vXml = "";
            Int32 vErr = 0;
            DataTable dt = null;
            CFA oFA = null;
            string strDate = "01/" + ddlMY.SelectedValue;
            DateTime vFDt = gblFuction.setDate(strDate);
            DateTime vTDt = vFDt.AddMonths(1);
            vTDt = vTDt.AddDays(-(vTDt.Day)); ;
            AddNewRowToGrid(2);
            try
            {
                dt = (DataTable)ViewState["CurrentTable"];
                if (dt == null) return;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["FADate"] = gblFuction.setDate(dr["FADate"].ToString());
                }
                //if (ValidateDetail() == false) return;
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
                oFA = new CFA();
                vErr = oFA.InsertFA(vFDt, vTDt, this.UserID, vXml);
                if (vErr > 0)
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                else 
                    gblFuction.MsgPopup(gblMarg.DBError);
            }
            finally
            {
                oFA = null;
                dt = null;
            }

        }


        private Boolean ValidateDetail()//To Check
        {
            Boolean vResult = true;
            //Int32 vRow = 0;
            //for (vRow = 0; vRow < gvSrc.Rows.Count; vRow++)
            //{
            //    TextBox txtPar = (TextBox)gvSrc.Rows[vRow].FindControl("txtPar");
            //    TextBox txtFADt = (TextBox)gvSrc.Rows[vRow].FindControl("txtFADt");
            //    TextBox txtAmt = (TextBox)gvSrc.Rows[vRow].FindControl("txtAmt");
            //    DropDownList ddlOpt = (DropDownList)gvSrc.Rows[vRow].FindControl("ddlOpt");

                //if (gblFuction.setDate(txtEndDt.Text) < gblFuction.setDate(txtStDt.Text))
                //{
                //    gblFuction.AjxMsgPopup("end date cannot be greater than start date");
                //    return;
                //}

            //}

            return vResult;
        }

        protected void gvSrc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vRow = 0, vMaxRow = 0;
            GridViewRow Row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            vRow = Row.RowIndex;
            vMaxRow = gvSrc.Rows.Count;
            DataTable dtDtl = null;
            dtDtl = (DataTable)ViewState["CurrentTable"];
            if (vRow != vMaxRow - 1)
            {
                if (e.CommandName == "cmdDelRec")
                {
                    dtDtl.Rows.RemoveAt(vRow);
                    dtDtl.AcceptChanges();
                    ViewState["CurrentTable"] = dtDtl;
                    gvSrc.DataSource = dtDtl;
                    gvSrc.DataBind();
                }
                SetPreviousData();
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
    }
}
