using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class EditSubPurpose : CENTRUMBase
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
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                LoadGrid(txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
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
                this.PageHeading = "Edit Sub Purpose";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuEdtSubPurpose);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Edit Sub Purpose", false);
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
        private void LoadGrid(string pFromDt, string pToDt, string pBranch, Int32 pPgIndx)
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
                dt = oLS.GetEditSubPurposeList(vFromDt, vToDt, vBrCode, pPgIndx, ref totalRows);
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
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            LoadGrid(txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSrc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataTable dt = null;

                DropDownList ddlSubPurpose = (DropDownList)e.Row.FindControl("ddlSubPurpose");
                CGblIdGenerator oGbl = null;
                try
                {
                    ddlSubPurpose.Items.Clear();
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    oGbl = new CGblIdGenerator();
                    dt = oGbl.PopComboMIS("N", "N", "AA", "SubPurposeID", "SubPurpose", "SubPurPoseMst", "0", "AA", "AA", System.DateTime.Now, "0000");
                    ddlSubPurpose.DataSource = dt;
                    ddlSubPurpose.DataTextField = "SubPurpose";
                    ddlSubPurpose.DataValueField = "SubPurposeID";
                    ddlSubPurpose.DataBind();
                    ListItem oLi = new ListItem("<--Select-->", "-1");
                    ddlSubPurpose.Items.Insert(0, oLi);
                }
                finally
                {
                    dt = null;
                    oGbl = null;
                }
            }
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
                            DropDownList ddlSubPurpose = (DropDownList)gvSrc.Rows[i].FindControl("ddlSubPurpose");
                            oGbl = new CGblIdGenerator();
                            dtFs = oGbl.PopComboMIS("N", "N", "AA", "SubPurposeID", "SubPurpose", "SubPurPoseMst", "0", "AA", "AA", System.DateTime.Now, "0000");
                            dr = dtFs.NewRow();
                            dr["SubPurposeID"] = 0;
                            dr["SubPurpose"] = string.Empty;
                            dtFs.Rows.InsertAt(dr, dtFs.Rows.Count + 1);
                            dtFs.Rows[dtFs.Rows.Count - 1]["SubPurposeID"] = dtFs.Rows[0]["SubPurposeID"];
                            dtFs.Rows[dtFs.Rows.Count - 1]["SubPurpose"] = dtFs.Rows[0]["SubPurpose"];
                            dtFs.Rows[0]["SubPurposeID"] = "-1";
                            dtFs.Rows[0]["SubPurpose"] = "<--Select-->";
                            ddlSubPurpose.DataSource = dtFs;
                            ddlSubPurpose.DataTextField = "SubPurpose";
                            ddlSubPurpose.DataValueField = "SubPurposeID";
                            ddlSubPurpose.DataBind();

                            //ListItem oLi = new ListItem("<--Select-->", "-1");
                            //ddlFS.Items.Insert(0, oLi);

                            //Set the Previous Selected Items on Each DropDownList  on Postbacks
                            ddlSubPurpose.ClearSelection();
                            ddlSubPurpose.SelectedIndex = ddlSubPurpose.Items.IndexOf(ddlSubPurpose.Items.FindByValue(dt.Rows[i]["SubPurposeID"].ToString()));//.Selected = true;
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
                oApp = new CEditSource();
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }
                //-----------XML Save----------
                vErr = oApp.UpdateEditSubPurpose(vXmlData, this.UserID, vBrCode, "E", 0);
                if (vErr > 0)
                {
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                    LoadGrid(txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                }
                else
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
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
                DropDownList ddlFS = (DropDownList)gvSrc.Rows[vRow].FindControl("ddlSubPurpose");
                if (ddlFS.SelectedIndex <= 0)
                {
                    gblFuction.MsgPopup("Please select Sub Purpose");
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
        protected void ddlSubPurpose_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            DropDownList dropList = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropList.NamingContainer;
            DropDownList ddlSubPurpose = (DropDownList)row.FindControl("ddlSubPurpose");
            TextBox txtPurpose = (TextBox)row.FindControl("txtPurpose");
            DataTable dt1 = null;
            CSubPurpose oPr = null;

            dt = (DataTable)ViewState["Sanc"];
            if (ddlSubPurpose.SelectedIndex > 0)
            {
                oPr = new CSubPurpose();
                dt1 = oPr.GetSubPurposeById(Convert.ToInt32(ddlSubPurpose.SelectedValue));
                if (dt1.Rows.Count > 0)
                {
                    txtPurpose.Text = Convert.ToString(dt1.Rows[0]["Purpose"]).Trim();
                }
                dt.Rows[row.RowIndex]["SubPurposeID"] = Convert.ToInt32(ddlSubPurpose.SelectedValue);
                dt.AcceptChanges();
                upSanc.Update();
                return;
            }
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }
    }
}
