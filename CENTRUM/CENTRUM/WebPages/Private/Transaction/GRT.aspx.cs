using System;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class GRT : CENTRUMBase
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
                ViewState["StateEdit"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                popRO();
                LoadGrid("P", 1);
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
                this.PageHeading = "GRT";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuGRT);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 3)// role id 3 is for RO
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "GRT", false);
                else
                {
                    if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                    if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                        btnDone.Visible = false;
                    else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                        btnDone.Visible = true;
                    else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                        btnDone.Visible = true;
                    else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                        Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "GRT", false);
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
        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlRO.DataSource = dt;
                ddlRO.DataTextField = "EoName";
                ddlRO.DataValueField = "EoId";
                ddlRO.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRO.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vEOID"></param>
        private void PopCenter(string vEOID)
        {
            ddlGroup.Items.Clear();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "MarketID", "Market", "MarketMSt", vEOID, "EOID", "Tra_DropDate", vLogDt, vBrCode);
                ddlCenter.DataSource = dt;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "MarketID";
                ddlCenter.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oli);
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
        /// <param name="vCenterID"></param>
        private void PopGroup(string vCenterID)
        {
            ddlGroup.Items.Clear();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "GroupID", "GroupName", "GroupMst", vCenterID, "MarketID", "Tra_DropDate", vLogDt, vBrCode);
                ddlGroup.DataSource = dt;
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataValueField = "GroupID";
                ddlGroup.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlGroup.Items.Insert(0, oli);
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
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ValidDate() == true)
            {
                try
                {
                    if (ddlAN.SelectedValue == "N")
                    {
                        LoadGrid("N", 1); 
                        btnDone.Enabled = true;
                    }
                    else if (ddlAN.SelectedValue == "P")
                    {
                        LoadGrid("P", 1); 
                        btnDone.Enabled = true;
                    }
                    else if (ddlAN.SelectedValue == "F")
                    {
                        LoadGrid("F", 1);
                        btnDone.Enabled = true;
                    }
                    else if (ddlAN.SelectedValue == "G")
                    {
                        LoadGrid("G", 1);
                        btnDone.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDone_Click(object sender, EventArgs e)
        {
            CCGT oCG = null;
            Int32 vErr = 0;
            string vXml = "";

            try
            {
                if (ValidateFields() == false) return;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                vXml = DtToXml(DtGrtTrn());
                oCG = new CCGT();
                vErr = oCG.SaveGRT(vXml, vBrCode, this.UserID);
                if (vErr > 0)
                {
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                    LoadGrid("P", 1);
                    ddlAN.SelectedValue = "P";
                }
                else
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
                }
            }
            finally
            {
                oCG = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtXml"></param>
        /// <returns></returns>
        private string DtToXml(DataTable dtXml)
        {
            string vXml = "";
            try
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
                return vXml;
            }
            finally
            {
                dtXml = null;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRO_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCenter.Items.Clear();
            ddlGroup.Items.Clear();
            if (ddlRO.SelectedIndex > 0) PopCenter(ddlRO.SelectedValue);
            ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(ddlRO.SelectedValue));
            ddlGroup.Items.Clear();
            if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlGroup.Items.Clear();
            if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidDate()
        {
            Boolean vResult = true;
            if (ddlGroup.SelectedIndex<=0)
            {
                gblFuction.MsgPopup("Group Cannot be left blank...");
                vResult = false;
            }
            return vResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable DtGrtTrn()
        {
            DataTable dt = new DataTable("GrtDtl");
            DataRow dr;

            dt.Columns.Add(new DataColumn("MemberId"));
            dt.Columns.Add(new DataColumn("CGTId"));
            dt.Columns.Add(new DataColumn("ActualGRTdt"));
            dt.Columns.Add(new DataColumn("GrtBy"));
            dt.Columns.Add(new DataColumn("GRTResultYN"));

            foreach (GridViewRow gr in gvGRT.Rows)
            {
                TextBox txtGRTDt = (TextBox)gr.FindControl("txtGRTDt");
                DropDownList ddlGrtBy = (DropDownList)gr.FindControl("ddlGrtBy");
                CheckBox ChkPass = (CheckBox)gr.FindControl("ChkPass");
                dr = dt.NewRow();
                dr["MemberId"] = gr.Cells[1].Text;
                dr["CGTId"] = gr.Cells[2].Text;
                dr["ActualGRTdt"] = gblFuction.setDate(txtGRTDt.Text);
                dr["GrtBy"] = ddlGrtBy.SelectedValue;
                if (ChkPass.Checked == true)
                    dr["GRTResultYN"] = "Y";
                else
                    dr["GRTResultYN"] = "N";

                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()//To Check
        {
            Boolean vResult = true;
            Int32 vRow = 0;
            for (vRow = 0; vRow < gvGRT.Rows.Count; vRow++)
            {
                CheckBox ChkPass = (CheckBox)gvGRT.Rows[vRow].FindControl("ChkPass");
                TextBox txtGRTDt = (TextBox)gvGRT.Rows[vRow].FindControl("txtGRTDt");
                DropDownList ddlGrtBy = (DropDownList)gvGRT.Rows[vRow].FindControl("ddlGrtBy");
                if (Convert.ToInt32(ddlGrtBy.SelectedValue) <= 0)
                {
                    gblFuction.MsgPopup("GRT By cannot be blank.");
                    vResult = false;
                }
                if (txtGRTDt.Text.Trim() =="")
                {
                    gblFuction.MsgPopup("Actual GRT date cannot be blank.");
                    vResult = false;
                }
                if (gblFuction.IsDate(txtGRTDt.Text) == false)
                {
                    gblFuction.MsgPopup("Actual GRT date is not valid.");
                    vResult = false;
                }
                if (gblFuction.setDate(txtGRTDt.Text) < gblFuction.setDate(gvGRT.Rows[vRow].Cells[4].Text))
                {
                    gblFuction.MsgPopup("Actual GRT date cannot be less than expected GRT date.");
                    vResult = false;
                }
            }
            return vResult;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(string pMode, Int32 pPgIndx)
        {
            DataTable dt = null;
            CCGT oCG = null;
            Int32 totalRows = 0;
            DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vGroupID = ddlGroup.SelectedValue;
                oCG = new CCGT();
                dt = oCG.GetGRTPG(vGroupID, vBrCode, pMode, vFrmDt, vToDt, pPgIndx, ref totalRows);
                ViewState["GRT"] = dt;
                gvGRT.DataSource = dt;
                gvGRT.DataBind();
                SetPreviousData();
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetPreviousData()
        {
            int rowIndex = 0;
            DataTable dt = null;
            DataTable dtEO = null;
            DataRow dr = null;
            CGblIdGenerator oGb = null;
            
            string vMemID = "", vMemName = "", vCGTID = "", vBrCode = "", vEXPGRTd = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                if (ViewState["GRT"] != null)
                {
                    dt = (DataTable)ViewState["GRT"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            vMemID = gvGRT.Rows[i].Cells[1].Text.ToString();
                            vCGTID = gvGRT.Rows[i].Cells[2].Text.ToString();
                            vMemName = gvGRT.Rows[i].Cells[3].Text.ToString();
                            vEXPGRTd = gvGRT.Rows[i].Cells[4].Text.ToString();
                            TextBox txtGRTDt = (TextBox)gvGRT.Rows[i].FindControl("txtGRTDt");
                            DropDownList ddlGrtBy = (DropDownList)gvGRT.Rows[i].FindControl("ddlGrtBy");
                            CheckBox ChkPass = (CheckBox)gvGRT.Rows[i].FindControl("ChkPass");

                            vBrCode = (string)Session[gblValue.BrnchCode];
                            vBrId = Convert.ToInt32(vBrCode);
                            oGb = new CGblIdGenerator();
                                                      
                            dtEO = oGb.PopTransferMIS("N", "EoMst", "ABM,BM,AM,SBM,AAM,RO,CO", vLogDt, vBrCode);

                            dr = dtEO.NewRow();
                            dr["EOId"] = 0;
                            dr["EOName"] = string.Empty;
                            dtEO.Rows.InsertAt(dr, dtEO.Rows.Count + 1);
                            dtEO.Rows[dtEO.Rows.Count - 1]["EOId"] = dtEO.Rows[0]["EOId"];
                            dtEO.Rows[dtEO.Rows.Count - 1]["EOName"] = dtEO.Rows[0]["EOName"];
                            dtEO.Rows[0]["EOId"] = "-1";
                            dtEO.Rows[0]["EOName"] = "<--Select-->";
                            
                            ddlGrtBy.DataSource = dtEO;
                            ddlGrtBy.DataTextField = "EOName";
                            ddlGrtBy.DataValueField = "EOId";
                            ddlGrtBy.DataBind();
                            //Set the Previous Selected Items on Each DropDownList  on Postbacks
                            ddlGrtBy.ClearSelection();
                            ddlGrtBy.SelectedIndex = ddlGrtBy.Items.IndexOf(ddlGrtBy.Items.FindByValue(dt.Rows[i]["EOId"].ToString()));//.Selected = true;
                            gvGRT.Rows[i].Cells[1].Text = dt.Rows[i]["MemberID"].ToString();
                            gvGRT.Rows[i].Cells[2].Text = dt.Rows[i]["CGTID"].ToString();
                            gvGRT.Rows[i].Cells[3].Text = dt.Rows[i]["Member"].ToString();
                            txtGRTDt.Text = dt.Rows[i]["ActualGRTdt"].ToString();
                            rowIndex++;
                        }
                    }
                    ViewState["GRT"] = dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                dtEO = null;
                dr = null;
                oGb = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvGRT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vBrCode = (string)Session[gblValue.BrnchCode];
            Int32 vBrId = Convert.ToInt32(vBrCode);
            DataTable dt = (DataTable)ViewState["GRT"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataTable dtEO = null;

                DropDownList ddlGrtBy = (DropDownList)e.Row.FindControl("ddlGrtBy");
                CheckBox ChkPass = (CheckBox)e.Row.FindControl("ChkPass");
                HiddenField hdYN = (HiddenField)e.Row.FindControl("hdYN");
                CGblIdGenerator oGb = new CGblIdGenerator();
                dtEO = oGb.PopTransferMIS("N", "EoMst", "ABM,BM,AM,SBM,AAM,RO,CO", vLogDt, vBrCode);

                ddlGrtBy.DataSource = dtEO;
                ddlGrtBy.DataTextField = "EoName";
                ddlGrtBy.DataValueField = "EoId";
                ddlGrtBy.DataBind();

                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlGrtBy.Items.Insert(0, oLi);

                if (hdYN.Value == "Y")
                {
                    ChkPass.Checked = true;
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[e.Row.RowIndex]["LoanAppId"].ToString() != "")
                        {
                            ChkPass.Enabled = false;
                        }
                    }
                }
                else
                {
                    ChkPass.Checked = false;
                }
            }
        }


    }
}
