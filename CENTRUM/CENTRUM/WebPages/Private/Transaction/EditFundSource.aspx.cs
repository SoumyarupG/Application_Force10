using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class EditFundSource : CENTRUMBase
    {
        protected int cPgNo = 1;
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
                ViewState["StateEdit"] = null;
                td1.Visible = false;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                //Int32 vFundSourceId = Convert.ToInt32(ddlFundSource.SelectedValue);
                //LoadGrid(txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                PopFSource();
            }
        }

        protected void PopFSource()
        {
            DataTable dt = null;

            //DropDownList ddlFS = (DropDownList)e.Row.FindControl("ddlFS");
            CGblIdGenerator oGbl = null;
            try
            {
                ddlFundSource.Items.Clear();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
                dt = oGbl.PopComboMIS("N", "N", "AA", "FundSourceId", "FundSource", "FundSourceMst", "0", "AA", "AA", System.DateTime.Now, "0000");
                ddlFundSource.DataSource = dt;
                ddlFundSource.DataTextField = "FundSource";
                ddlFundSource.DataValueField = "FundSourceId";
                ddlFundSource.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlFundSource.Items.Insert(0, oLi);

                ddlSourceFundSource.DataSource = dt;
                ddlSourceFundSource.DataTextField = "FundSource";
                ddlSourceFundSource.DataValueField = "FundSourceId";
                ddlSourceFundSource.DataBind();
                ddlSourceFundSource.Items.Insert(0, oLi);
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
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Edit Source of Fund";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuEdtSorceFnd);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFromDt"></param>
        /// <param name="pToDt"></param>
        /// <param name="pBranch"></param>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(string pFromDt, string pToDt, string pBranch, Int32 pPgIndx, Int32 pSourceofFund)
        {
            DataTable dt = null;
            CEditSource oLS = null;
            Int32 totalRows = 0;
            try
            {
                string vBrCode = pBranch;
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                oLS = new CEditSource();
                dt = oLS.GetEditSourceList(vFromDt, vToDt, vBrCode, pPgIndx, ref totalRows, pSourceofFund);
                ViewState["Sanc"] = dt;
                gvSrc.DataSource = dt;
                gvSrc.DataBind();
                SetPreviousData();
            }
            finally
            {
                dt = null;
                oLS = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            td1.Visible = true;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            LoadGrid(txtFrmDt.Text, txtToDt.Text, vBrCode, 1, Convert.ToInt32(ddlSourceFundSource.SelectedValue));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSrc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    DataTable dt = null;

            //    DropDownList ddlFS = (DropDownList)e.Row.FindControl("ddlFS");
            //    CGblIdGenerator oGbl = null;
            //    try
            //    {
            //        ddlFS.Items.Clear();
            //        string vBrCode = Session[gblValue.BrnchCode].ToString();
            //        oGbl = new CGblIdGenerator();
            //        dt = oGbl.PopComboMIS("N", "N", "AA", "FundSourceId", "FundSource", "FundSourceMst", "0", "AA", "AA", System.DateTime.Now, "0000");
            //        ddlFS.DataSource = dt;
            //        ddlFS.DataTextField = "FundSource";
            //        ddlFS.DataValueField = "FundSourceId";
            //        ddlFS.DataBind();
            //        ListItem oLi = new ListItem("<--Select-->", "-1");
            //        ddlFS.Items.Insert(0, oLi);
            //    }
            //    finally
            //    {
            //        dt = null;
            //        oGbl = null;
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetPreviousData()
        {
            int rowIndex = 0;
            DataTable dt = null;
            DataTable dtFs = null;
            DataRow dr = null;
            CGblIdGenerator oGbl = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                if (ViewState["Sanc"] != null)
                {
                    dt = (DataTable)ViewState["Sanc"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DropDownList ddlFS = (DropDownList)gvSrc.Rows[i].FindControl("ddlFS");
                            oGbl = new CGblIdGenerator();
                            dtFs = oGbl.PopComboMIS("N", "N", "AA", "FundSourceId", "FundSource", "FundSourceMst", "0", "AA", "AA", System.DateTime.Now, "0000");
                            dr = dtFs.NewRow();
                            dr["FundSourceId"] = 0;
                            dr["FundSource"] = string.Empty;
                            dtFs.Rows.InsertAt(dr, dtFs.Rows.Count + 1);
                            dtFs.Rows[dtFs.Rows.Count - 1]["FundSourceId"] = dtFs.Rows[0]["FundSourceId"];
                            dtFs.Rows[dtFs.Rows.Count - 1]["FundSource"] = dtFs.Rows[0]["FundSource"];
                            dtFs.Rows[0]["FundSourceId"] = "-1";
                            dtFs.Rows[0]["FundSource"] = "<--Select-->";
                            ddlFS.DataSource = dtFs;
                            ddlFS.DataTextField = "FundSource";
                            ddlFS.DataValueField = "FundSourceId";
                            ddlFS.DataBind();

                            //ListItem oLi = new ListItem("<--Select-->", "-1");
                            //ddlFS.Items.Insert(0, oLi);

                            //Set the Previous Selected Items on Each DropDownList  on Postbacks
                            ddlFS.ClearSelection();
                            ddlFS.SelectedIndex = ddlFS.Items.IndexOf(ddlFS.Items.FindByValue(dt.Rows[i]["FundSourceId"].ToString()));//.Selected = true;
                            //}
                            rowIndex++;
                        }
                    }
                    ViewState["Sanc"] = dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                dtFs = null;
                dr = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDone_Click(object sender, EventArgs e)
        {
            Boolean vResult = true;
            CEditSource oApp = null;
            DataTable dt = new DataTable();
            Int32 vErr = 0;
            string vXmlData = "";

            try
            {
                dt = (DataTable)ViewState["Sanc"];
                if (dt == null) return;
                if (ValidateFields() == false) return;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                Int32 vFundSourceId = Convert.ToInt32(ddlFundSource.SelectedValue);
                oApp = new CEditSource();
                if (vFundSourceId > 0)
                {
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    //-----------XML Save----------
                    vErr = oApp.UpdateEditSource(vXmlData, vFundSourceId, this.UserID, vBrCode, "E", 0);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                        LoadGrid(txtFrmDt.Text, txtToDt.Text, vBrCode, 1, Convert.ToInt32(ddlSourceFundSource.SelectedValue));
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Please select FundSource");
                    vResult = false;
                }
            }
            finally
            {
                oApp = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()//To Check
        {
            Boolean vResult = true;
            Int32 vRow = 0;
            for (vRow = 0; vRow < gvSrc.Rows.Count; vRow++)
            {
                DropDownList ddlFS = (DropDownList)gvSrc.Rows[vRow].FindControl("ddlFS");
                if (ddlFS.SelectedIndex <=0)
                {
                    gblFuction.MsgPopup("Please select FundSource");
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
        //protected void ddlFS_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DataTable dt = null;
        //    DropDownList dropList = (DropDownList)sender;
        //    GridViewRow row = (GridViewRow)dropList.NamingContainer;
        //    DropDownList ddlFS = (DropDownList)row.FindControl("ddlFS");

        //    dt = (DataTable)ViewState["Sanc"];
        //    if (ddlFS.SelectedIndex > 0)
        //    {
        //        dt.Rows[row.RowIndex]["FundSourceId"] = Convert.ToInt32(ddlFS.SelectedValue);
        //        dt.AcceptChanges();
        //        upSanc.Update();
        //        return;
        //    }
        //    ViewState["Sanc"] = dt;
        //    upSanc.Update();
        //}

        //protected void ddlFundSource_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DataTable dt = null;
        //    DropDownList dropList = (DropDownList)sender;
        //    GridViewRow row = (GridViewRow)dropList.NamingContainer;
        //    DropDownList ddlFS = (DropDownList)row.FindControl("ddlFS");

        //    dt = (DataTable)ViewState["Sanc"];
        //    if (ddlFS.SelectedIndex > 0)
        //    {
        //        dt.Rows[row.RowIndex]["FundSourceId"] = Convert.ToInt32(ddlFS.SelectedValue);
        //        dt.AcceptChanges();
        //        upSanc.Update();
        //        return;
        //    }
        //    ViewState["Sanc"] = dt;
        //    upSanc.Update();
        //}
    }
}
