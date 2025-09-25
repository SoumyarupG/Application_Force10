using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Data;
using System.Configuration;
using System.IO;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class ViewMember : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string vMemberId = Request.QueryString["MemberId"];
                string vBrCode = Request.QueryString["BranchCode"];
                string LoanAppId = Request.QueryString["LoanAppId"];
                if (vMemberId != "" && vBrCode != "")
                {
                    ViewState["BrCode"] = vBrCode;
                    gvFamily.DataSource = null;
                    gvAsset.DataSource = null;
                    ViewState["StateEdit"] = null;
                    ViewState["State"] = null;
                    //GenerateGrid();
                    //GenerateGrid1();
                    popQualification();
                    PopGender();
                    popHouseHold();
                    popEO(vBrCode);
                    popRelation();
                    popRelation1();
                    popCaste();
                    popOccupation();
                    popVillage(vBrCode);
                    popReligion();
                    popIdentityProof();
                    popAddProof();
                    popCltype();
                    popBank();
                    popSpeciallyAbled();
                    //GenerateDiscrepencyGrid();
                    btnCgtApply.Enabled = false;
                    tbEmp.ActiveTabIndex = 0;
                    clearMemPhoto();
                    FillMemberDtl(vMemberId, vBrCode);
                }
            }
        }

        private void popSpeciallyAbled()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            oGb = new CGblIdGenerator();
            dt = oGb.GetSpclAble();

            ddlSpclAbled.DataSource = dt;
            ddlSpclAbled.DataTextField = "Question";
            ddlSpclAbled.DataValueField = "Qno";
            ddlSpclAbled.DataBind();
            ListItem oli = new ListItem("<--Select-->", "-1");
            ddlSpclAbled.Items.Insert(0, oli);

        }

        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.GetAllStateList();
                ddlStat.DataSource = dt;
                ddlStat.DataTextField = "StateName";
                ddlStat.DataValueField = "StateId";
                ddlStat.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlStat.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //private void MemberVerify(string vMemberId)
        //{
        //    tbEmp.ActiveTabIndex = 1;
        //    FillMemberDtl(vMemberId);
        //}
        private void popEO(string vBrCode)
        {
            DataTable dt = null;
            CEO oRO = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlRO.DataSource = dt;
                ddlRO.DataTextField = "EoName";
                ddlRO.DataValueField = "Eoid";
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRO.SelectedValue != "-1")
            {
                string vEoId = ddlRO.SelectedItem.Value;
                PopCenter(vEoId, ViewState["BrCode"].ToString());
                ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(Convert.ToString(ViewState["CentreID"]).Trim()));
                PopGroup(ddlCenter.SelectedValue);
            }
            if (ddlRO.SelectedValue == "-1")
            {
                ddlCenter.SelectedValue = "-1";
                ddlGroup.SelectedValue = "-1";
                // ddlMetDay.SelectedValue="0";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pEoId"></param>
        private void PopCenter(string pEoId, string vBrCode)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            //string vBrCode = (string)Session[gblValue.BrnchCode];
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("S", "N", "AA", "MarketID", "Market", "MarketMSt", pEoId, "EoId", "AA", gblFuction.setDate("01/01/1900"), "0000");
                if (dt.Rows.Count > 0)
                {
                    ddlCenter.DataSource = dt;
                    ddlCenter.DataTextField = "Market";
                    ddlCenter.DataValueField = "MarketID";
                    ddlCenter.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlCenter.Items.Insert(0, oli);

                    ViewState["CentreID"] = Convert.ToString(dt.Rows[0]["Marketid"]).Trim();
                }
            }
            finally
            {
                dt = null;
                oGb = null;
            }
        }

        private void GenerateDiscrepencyGrid()
        {
            DataTable dt = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                dt = oMem.GenerateDiscrepencyGrid();
                DataRow dF;
                dF = dt.NewRow();
                dt.Rows.Add(dF);
                dt.AcceptChanges();
                ViewState["DiscrepancyTbl"] = dt;
                gvDiscrepancy.DataSource = dt;
                gvDiscrepancy.DataBind();
            }
            finally
            {
            }
        }

        protected void gvDiscrepancy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt, dt1;
            CMember oMem = null;
            ListItem oL1 = null;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlCategory = (DropDownList)e.Row.FindControl("ddlCategory");
                    HiddenField hdCategory = (HiddenField)e.Row.FindControl("hdCategory");
                    oMem = new CMember();
                    dt = new DataTable();
                    dt = oMem.GetCategory();
                    ddlCategory.Items.Clear();
                    ddlCategory.DataSource = dt;
                    ddlCategory.DataTextField = "CategoryName";
                    ddlCategory.DataValueField = "CategoryId";
                    ddlCategory.DataBind();
                    oL1 = new ListItem("<-- Select -->", "-1");
                    ddlCategory.Items.Insert(0, oL1);
                    //ddlRelation.SelectedIndex = ddlRelation.Items.IndexOf(ddlRelation.Items.FindByValue(e.Row.Cells[13].Text));
                    ddlCategory.ClearSelection();
                    ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(Convert.ToString(hdCategory.Value)));

                    ViewState["CategoryMst"] = dt;

                    DropDownList ddlDiscrepancy = (DropDownList)e.Row.FindControl("ddlDiscrepancy");
                    HiddenField hdDiscrepancy = (HiddenField)e.Row.FindControl("hdDiscrepancy");
                    oMem = new CMember();
                    dt1 = new DataTable();
                    dt1 = oMem.GetDiscrepancy();
                    ddlDiscrepancy.Items.Clear();
                    if (hdCategory.Value == "-1" || hdCategory.Value == "")
                    {
                        ddlDiscrepancy.DataSource = dt1;
                        ddlDiscrepancy.DataTextField = "DiscrepancyName";
                        ddlDiscrepancy.DataValueField = "DiscrepancyId";
                        ddlDiscrepancy.DataBind();
                    }
                    else
                    {
                        dt1 = dt1.Select("CategoryId = " + hdCategory.Value).CopyToDataTable();
                        ddlDiscrepancy.DataSource = dt1;
                        ddlDiscrepancy.DataTextField = "DiscrepancyName";
                        ddlDiscrepancy.DataValueField = "DiscrepancyId";
                        ddlDiscrepancy.DataBind();
                    }
                    oL1 = new ListItem("<-- Select -->", "-1");
                    ddlDiscrepancy.Items.Insert(0, oL1);
                    ddlDiscrepancy.ClearSelection();
                    ddlDiscrepancy.SelectedIndex = ddlDiscrepancy.Items.IndexOf(ddlDiscrepancy.Items.FindByValue(Convert.ToString(hdDiscrepancy.Value)));

                    ViewState["DiscrepancyMst"] = dt1;

                    HiddenField hdBrResolved = (HiddenField)e.Row.FindControl("hdBrResolved");
                    CheckBox cbBrResolved = (CheckBox)e.Row.FindControl("cbBrResolved");
                    if (hdBrResolved.Value == "Y")
                    {
                        cbBrResolved.Checked = true;
                        cbBrResolved.Enabled = false;
                    }
                    else if (hdBrResolved.Value == "N")
                    {
                        cbBrResolved.Checked = false;
                        cbBrResolved.Enabled = Session[gblValue.BrnchCode].ToString() != "0000" ? true : false;
                    }
                    else
                    {
                        cbBrResolved.Checked = false;
                        cbBrResolved.Enabled = false;
                    }

                    HiddenField hdOpsResolved = (HiddenField)e.Row.FindControl("hdOpsResolved");
                    CheckBox cbOpsResolved = (CheckBox)e.Row.FindControl("cbOpsResolved");
                    if (hdOpsResolved.Value == "Y")
                    {
                        cbOpsResolved.Checked = true;
                        cbOpsResolved.Enabled = false;
                    }
                    else
                    {
                        cbOpsResolved.Checked = false;
                        cbOpsResolved.Enabled = (Session[gblValue.BrnchCode].ToString() == "0000" && hdBrResolved.Value == "Y") ? true : false;
                    }

                    TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                    if (Session[gblValue.BrnchCode].ToString() != "0000")
                    {
                        gvDiscrepancy.Columns[7].Visible = false;
                        ddlCategory.Enabled = false;
                        ddlDiscrepancy.Enabled = false;
                        txtRemarks.Enabled = false;
                    }
                    else
                    {
                        gvDiscrepancy.Columns[7].Visible = true;
                        ddlCategory.Enabled = true;
                        ddlDiscrepancy.Enabled = true;
                        txtRemarks.Enabled = true;
                    }

                    //if (Session[gblValue.BrnchCode].ToString() == "0000")
                    //{
                    //    if (hdBrResolved.Value == "Y")
                    //    {
                    //        cbBrResolved.Checked = true;
                    //        cbBrResolved.Enabled = false;

                    //        if (hdOpsResolved.Value == "Y")
                    //        {
                    //            cbOpsResolved.Checked = true;
                    //            cbOpsResolved.Enabled = false;
                    //        }
                    //        else
                    //        {
                    //            cbOpsResolved.Checked = false;
                    //            cbOpsResolved.Enabled = true;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        cbBrResolved.Checked = false;
                    //        cbBrResolved.Enabled = false;

                    //        cbOpsResolved.Checked = false;
                    //        cbOpsResolved.Enabled = false;
                    //    }
                    //}
                    //else
                    //{
                    //    cbBrResolved.Enabled = true;
                    //    cbOpsResolved.Enabled = false;

                    //    if (hdOpsResolved.Value == "Y")
                    //    {
                    //        cbBrResolved.Checked = true;
                    //        cbBrResolved.Enabled = false;

                    //        cbOpsResolved.Checked = true;
                    //        cbOpsResolved.Enabled = false;
                    //    }
                    //    else
                    //    {
                    //        if (hdBrResolved.Value == "Y")
                    //        {
                    //            cbBrResolved.Checked = true;
                    //            cbBrResolved.Enabled = true;

                    //            cbOpsResolved.Checked = false;
                    //            cbOpsResolved.Enabled = false;
                    //        }
                    //        else
                    //        {
                    //            cbBrResolved.Checked = false;
                    //            cbBrResolved.Enabled = true;

                    //            cbOpsResolved.Checked = false;
                    //            cbOpsResolved.Enabled = false;

                    //        }
                    //    }
                    //}

                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                oMem = null;
            }
        }

        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            Int32 vR = 0,vSlid = 0;
            DataRow dr;
            dt.TableName = "DiscrepancyTbl";
            dt.Columns.Add("SlId", typeof(string));
            dt.Columns.Add("Category", typeof(string));
            dt.Columns.Add("Discrepancy", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));
            dt.Columns.Add("cbBrResolved", typeof(string));
            dt.Columns.Add("cbOpsResolved", typeof(string));
            dt.Columns.Add("LoanAppId", typeof(string));
            for (int i = 0; i < gvDiscrepancy.Rows.Count; i++)
            {
                dt.Rows.Add();

                Label lblSlid = (Label)gvDiscrepancy.Rows[i].FindControl("lblSlid");
                dt.Rows[i]["SlId"] = lblSlid.Text;

                DropDownList ddlCategory = (DropDownList)gvDiscrepancy.Rows[i].FindControl("ddlCategory");
                dt.Rows[i]["Category"] = ddlCategory.SelectedValue;
                DropDownList ddlDiscrepancy = (DropDownList)gvDiscrepancy.Rows[i].FindControl("ddlDiscrepancy");
                dt.Rows[i]["Discrepancy"] = ddlDiscrepancy.SelectedValue;
                TextBox txtRemarks = (TextBox)gvDiscrepancy.Rows[i].FindControl("txtRemarks");
                dt.Rows[i]["Remarks"] = txtRemarks.Text;
                CheckBox cbBrResolved = (CheckBox)gvDiscrepancy.Rows[i].FindControl("cbBrResolved");
                dt.Rows[i]["cbBrResolved"] = cbBrResolved.Checked == true ? "Y" : "N";
                CheckBox cbOpsResolved = (CheckBox)gvDiscrepancy.Rows[i].FindControl("cbOpsResolved");
                dt.Rows[i]["cbOpsResolved"] = cbOpsResolved.Checked == true ? "Y" : "N";

                Label lblLoanAppId = (Label)gvDiscrepancy.Rows[i].FindControl("lblLoanAppId");
                dt.Rows[i]["LoanAppId"] = lblLoanAppId.Text;

                dt.AcceptChanges();
            }


            if (dt.Rows[gvDiscrepancy.Rows.Count - 1]["Category"].ToString() == "-1")
            {
                gblFuction.MsgPopup("Category is Blank...");
            }
            else if (dt.Rows[gvDiscrepancy.Rows.Count - 1]["Discrepancy"].ToString() == "-1")
            {
                gblFuction.MsgPopup("Discrepancy is Blank...");
            }
            else if (dt.Rows[gvDiscrepancy.Rows.Count - 1]["Category"].ToString() != "5" && dt.Rows[gvDiscrepancy.Rows.Count - 1]["Remarks"].ToString() == "")
            {
                gblFuction.MsgPopup("Remarks is Blank...");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["LoanAppId"].ToString() == Request.QueryString["LoanAppId"].ToString())
                    {
                        vSlid = vSlid + 1;
                    }
                }
                dr = dt.NewRow();
                dr["SlId"] = vSlid + 1;
                dr["LoanAppId"]  = Request.QueryString["LoanAppId"];
                dt.Rows.Add(dr);
            }
            ViewState["DiscrepancyTbl"] = dt;
            gvDiscrepancy.DataSource = dt;
            gvDiscrepancy.DataBind();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem oL1 = new ListItem("<-- Select -->", "-1");
            GridViewRow gvRow = (GridViewRow)((DropDownList)sender).NamingContainer;
            DropDownList ddlCategory = (DropDownList)gvRow.FindControl("ddlCategory");
            DropDownList ddlDiscrepancy = (DropDownList)gvRow.FindControl("ddlDiscrepancy");
            DataTable dtCategory = new DataTable();
            dtCategory = (DataTable)(ViewState["CategoryMst"]);
            DataTable dtDiscrepancy = new DataTable();
            dtDiscrepancy = (DataTable)ViewState["DiscrepancyMst"];
            if (ddlCategory.SelectedValue != "-1")
            {
                dtDiscrepancy = dtDiscrepancy.Select("CategoryId = " + ddlCategory.SelectedValue).CopyToDataTable();
            }
            else
            {
                dtDiscrepancy = null;
            }
            ddlDiscrepancy.Items.Clear();
            ddlDiscrepancy.DataSource = dtDiscrepancy;
            ddlDiscrepancy.DataTextField = "DiscrepancyName";
            ddlDiscrepancy.DataValueField = "DiscrepancyId";
            ddlDiscrepancy.DataBind();
            oL1 = new ListItem("<-- Select -->", "-1");
            if (ddlCategory.SelectedValue != "5")
            {
                ddlDiscrepancy.Items.Insert(0, oL1);
            }
            ddlDiscrepancy.ClearSelection();
        }

        //protected void ddlDiscrepancy_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    GridViewRow gvRow = (GridViewRow)((DropDownList)sender).NamingContainer;
        //    DropDownList ddlCategory = (DropDownList)gvRow.FindControl("ddlCategory");
        //    DropDownList ddlDiscrepancyCurrent = (DropDownList)gvRow.FindControl("ddlDiscrepancy");

        //    int index = gvRow.RowIndex;

        //    for (int i = 0; i < gvDiscrepancy.Rows.Count; i++)
        //    {
        //        DropDownList ddlDiscrepancy = (DropDownList)gvDiscrepancy.Rows[i].FindControl("ddlDiscrepancy");
        //        if (ddlDiscrepancyCurrent.SelectedValue == ddlDiscrepancy.SelectedValue && i != index)
        //        {
        //            gblFuction.MsgPopup("No Two Discrepancy can be Same");
        //            ddlDiscrepancyCurrent.SelectedValue = "-1";
        //            break;
        //        }
        //    }

        //}

        protected void btnSaveDiscrepancy_Click(object sender, EventArgs e)
        {
            string vXml = "";
            int vErr = 0;
            DataTable dtXml = CreateTrData();

            using (StringWriter oSW = new StringWriter())
            {
                dtXml.WriteXml(oSW);
                vXml = oSW.ToString();
            }
            CMember oMem = new CMember();
            vErr = oMem.SaveLoanDiscrepancy(vXml, Convert.ToInt32(Session[gblValue.UserId]));
            if (vErr > 0)
            {
                gblFuction.MsgPopup("Discrepancy Saved Successfully.");
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "CloseWindowScript", "window.close();", true);
            }
            else
            {
                gblFuction.MsgPopup(gblMarg.DBError);
            }
        }

        private DataTable CreateTrData()
        {
            DataTable dt = new DataTable();
            Int32 vR = 0;
            DataRow dr;
            dt.TableName = "DiscrepancyTbl";
            dt.Columns.Add("SlId", typeof(string));
            dt.Columns.Add("LoanAppId", typeof(string));
            dt.Columns.Add("MemberId", typeof(string));
            dt.Columns.Add("CategoryId", typeof(string));
            dt.Columns.Add("DiscrepancyId", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));
            dt.Columns.Add("BrApprYN", typeof(string));
            dt.Columns.Add("BrApprBy", typeof(string));
            dt.Columns.Add("BrApprDt", typeof(string));
            dt.Columns.Add("OpsApprYN", typeof(string));
            dt.Columns.Add("OpsApprBy", typeof(string));
            dt.Columns.Add("OpsApprDt", typeof(string));

            for (int i = 1; i <= gvDiscrepancy.Rows.Count; i++)
            {
                DropDownList ddlCategory = (DropDownList)gvDiscrepancy.Rows[i - 1].FindControl("ddlCategory");
                DropDownList ddlDiscrepancy = (DropDownList)gvDiscrepancy.Rows[i - 1].FindControl("ddlDiscrepancy");
                TextBox txtRemarks = (TextBox)gvDiscrepancy.Rows[i - 1].FindControl("txtRemarks");

                Label lblSlid = (Label)gvDiscrepancy.Rows[i - 1].FindControl("lblSlid");
                Label lblLoanAppId = (Label)gvDiscrepancy.Rows[i - 1].FindControl("lblLoanAppId");

                if (ddlCategory.SelectedValue != "-1")
                {
                    if (ddlDiscrepancy.SelectedValue != "-1")
                    {
                        if ( txtRemarks.Text != "")
                        {
                            dt.Rows.Add();
                            dt.Rows[i - 1]["SlId"] = lblSlid.Text;
                            dt.Rows[i - 1]["LoanAppId"] = lblLoanAppId.Text;
                            dt.Rows[i - 1]["MemberId"] = Request.QueryString["MemberId"];
                            dt.Rows[i - 1]["CategoryId"] = ddlCategory.SelectedValue;
                            dt.Rows[i - 1]["DiscrepancyId"] = ddlDiscrepancy.SelectedValue;
                            dt.Rows[i - 1]["Remarks"] = txtRemarks.Text;
                            CheckBox cbBrResolved = (CheckBox)gvDiscrepancy.Rows[i - 1].FindControl("cbBrResolved");
                            if (cbBrResolved.Checked == true)
                            {
                                dt.Rows[i - 1]["BrApprYN"] = "Y";
                                dt.Rows[i - 1]["BrApprBy"] = Session[gblValue.UserId].ToString();
                                dt.Rows[i - 1]["BrApprDt"] = Convert.ToString(DateTime.Now);
                            }
                            else
                            {
                                dt.Rows[i - 1]["BrApprYN"] = "N";
                                dt.Rows[i - 1]["BrApprBy"] = DBNull.Value;
                                dt.Rows[i - 1]["BrApprDt"] = DBNull.Value;
                            }

                            CheckBox cbOpsResolved = (CheckBox)gvDiscrepancy.Rows[i - 1].FindControl("cbOpsResolved");
                            if (cbOpsResolved.Checked == true)
                            {
                                dt.Rows[i - 1]["OpsApprYN"] = "Y";
                                dt.Rows[i - 1]["OpsApprBy"] = Session[gblValue.UserId].ToString();
                                dt.Rows[i - 1]["OpsApprDt"] = Convert.ToString(DateTime.Now);
                            }
                            else
                            {
                                dt.Rows[i - 1]["OpsApprYN"] = "N";
                                dt.Rows[i - 1]["OpsApprBy"] = DBNull.Value;
                                dt.Rows[i - 1]["OpsApprDt"] = DBNull.Value;
                            }
                        }
                        else
                        {
                            gblFuction.MsgPopup("Remarks is Blank...");
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Discrepancy is Blank...");
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Category is Blank...");
                }
            }
            dt.AcceptChanges();
            return dt;
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vGroupId = ddlGroup.SelectedValue;
            if (ddlGroup.SelectedValue != "-1")
            {
                PopCollDay(vGroupId, ViewState["BrCode"].ToString());
            }

            if (ddlGroup.SelectedValue == "-1")
            {
                // ddlMetDay.SelectedValue ="0";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vMarketId"></param>
        private void PopGroup(string vMarketId)
        {
            DataTable dt = null;
            string vBrCode = ViewState["BrCode"].ToString();
            DateTime vAdmDt = gblFuction.setDate(txtAdmDt.Text);
            CCenter oCent = null;
            try
            {
                oCent = new CCenter();
                dt = oCent.GetMeetingDayByCenterId(vMarketId, vBrCode, vAdmDt);
                if (dt.Rows.Count > 0)
                {
                    ddlGroup.DataSource = dt;
                    ddlGroup.DataTextField = "GroupName";
                    ddlGroup.DataValueField = "Groupid";
                    ddlGroup.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlGroup.Items.Insert(0, oli);
                    //ddlMetDay.SelectedIndex = ddlMetDay.Items.IndexOf(ddlMetDay.Items.FindByValue(dt.Rows[0]["CollDay"].ToString()));
                }
            }
            finally
            {
                dt = null;
                oCent = null;
            }
        }

        private void PopCollDay(string vGroupId, string vBrCode)
        {
            DataTable dt = null;
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vAdmDt = gblFuction.setDate(txtAdmDt.Text);
            CCenter oCent = null;
            try
            {
                oCent = new CCenter();
                dt = oCent.GetCollDayByGroupId(vGroupId, vBrCode, vAdmDt);
                if (dt.Rows.Count > 0)
                {
                    txtMetDay.Text = dt.Rows[0]["CollDay"].ToString();
                }
            }
            finally
            {
                dt = null;
                oCent = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popQualification()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "QualificationId", "QualificationName", "QualificationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlEduc.DataSource = dt;
                ddlEduc.DataTextField = "QualificationName";
                ddlEduc.DataValueField = "QualificationId";
                ddlEduc.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlEduc.Items.Insert(0, oli);
                ddlBEdu.DataSource = dt;
                ddlBEdu.DataTextField = "QualificationName";
                ddlBEdu.DataValueField = "QualificationId";
                ddlBEdu.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlBEdu.Items.Insert(0, oli1);
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
        private void popOccupation()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "OccupationId", "OccupationName", "OccupationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlOcup.DataSource = dt;
                ddlOcup.DataTextField = "OccupationName";
                ddlOcup.DataValueField = "OccupationId";
                ddlOcup.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlOcup.Items.Insert(0, oli);
                ddlBOcup.DataSource = dt;
                ddlBOcup.DataTextField = "OccupationName";
                ddlBOcup.DataValueField = "OccupationId";
                ddlBOcup.DataBind();
                ListItem ol1 = new ListItem("<--Select-->", "-1");
                ddlBOcup.Items.Insert(0, ol1);
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
        private void popRelation()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "HumanRelationId", "HumanRelationName", "HumanRelationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlBRel.DataSource = dt;
                ddlBRel.DataTextField = "HumanRelationName";
                ddlBRel.DataValueField = "HumanRelationId";
                ddlBRel.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBRel.Items.Insert(0, oli);

                ddlGuarRel.DataSource = dt;
                ddlGuarRel.DataTextField = "HumanRelationName";
                ddlGuarRel.DataValueField = "HumanRelationId";
                ddlGuarRel.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlGuarRel.Items.Insert(0, oli1);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popRelation1()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "HumanRelationId", "HumanRelationName", "HumanRelationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");

            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popBank()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();

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
        private void popVillage(string vBrCode)
        {
            DataTable dt = null;
            CVillage oGb = null;
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oGb = new CVillage();
                dt = oGb.PopVillage(vBrCode);
                //ddlVillg.DataSource = dt;
                //ddlVillg.DataTextField = "VillageName";
                //ddlVillg.DataValueField = "VillageId";
                //ddlVillg.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                //ddlVillg.Items.Insert(0, oli);
                ddlBVillg.DataSource = dt;
                ddlBVillg.DataTextField = "VillageName";
                ddlBVillg.DataValueField = "VillageId";
                ddlBVillg.DataBind();
                ListItem ol1 = new ListItem("<--Select-->", "-1");
                ddlBVillg.Items.Insert(0, ol1);

                ddlCommVill.DataSource = dt;
                ddlCommVill.DataTextField = "VillageName";
                ddlCommVill.DataValueField = "VillageId";
                ddlCommVill.DataBind();
                //ddlCommVill.Items.Insert(0, oli);

            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ddlVillg_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DataTable dt = null;
        //    CVillage oVlg = null;
        //    string vVlgId = "";
        //    try
        //    {
        //        oVlg = new CVillage();
        //        dt = oVlg.GetGpBlkDistStateList(vVlgId);
        //        ddlMuPanc.DataSource = ddlBlk.DataSource =  ddlStat.DataSource = dt;
        //        ddlMuPanc.DataTextField = "GPName";
        //        ddlMuPanc.DataValueField = "GPId";
        //        ddlMuPanc.DataBind();
        //        ddlBlk.DataTextField = "BlockName";
        //        ddlBlk.DataValueField = "BlockId";
        //        ddlBlk.DataBind();
        //        //ddlDist.DataTextField = "DistrictName";
        //        //ddlDist.DataValueField = "DistrictId";
        //        //ddlDist.DataBind();
        //        ddlStat.DataTextField = "StateName";
        //        ddlStat.DataValueField = "StateId";
        //        ddlStat.DataBind();
        //        ddlMuPanc.Enabled = false;
        //        ddlBlk.Enabled = false;
        //        ddlDist.Enabled = false;
        //        ddlStat.Enabled = false;
        //    }
        //    finally
        //    {
        //        oVlg = null;
        //        dt = null;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBVillg_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopAllAgainstVillage();
        }

        private void PopAllAgainstVillage()
        {
            DataTable dt = null;
            CVillage oVlg = null;
            string vVlgId = ddlBVillg.SelectedValue;
            try
            {
                oVlg = new CVillage();
                dt = oVlg.GetGpBlkDistStateList(vVlgId);
                ddlBMunPanca.DataSource = ddlBBlk.DataSource = ddlBDist.DataSource = ddlBStat.DataSource = dt;
                ddlBMunPanca.DataTextField = "GPName";
                ddlBMunPanca.DataValueField = "GPId";
                ddlBMunPanca.DataBind();
                ddlBBlk.DataTextField = "BlockName";
                ddlBBlk.DataValueField = "BlockId";
                ddlBBlk.DataBind();
                ddlBDist.DataTextField = "DistrictName";
                ddlBDist.DataValueField = "DistrictId";
                ddlBDist.DataBind();
                ddlBStat.DataTextField = "StateName";
                ddlBStat.DataValueField = "StateId";
                ddlBStat.DataBind();
                ddlBMunPanca.Enabled = false;
                ddlBBlk.Enabled = false;
                ddlBDist.Enabled = false;
                ddlBStat.Enabled = false;
            }
            finally
            {
                oVlg = null;
                dt = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>


        /// <summary>
        /// 
        /// </summary>
        private void popReligion()
        {
            //Dictionary<string, string> oDic = new Dictionary<string, string>();
            //oDic.Add("<-Select->", "0");
            //oDic.Add("Buddhist", "R05");
            //oDic.Add("Christian", "R03");
            //oDic.Add("Hindu", "R01");
            //oDic.Add("Jain", "R06");
            //oDic.Add("Muslim", "R02");
            //oDic.Add("Others", "R07");
            //oDic.Add("Sikh", "R04");
            //ddlRelg.DataSource = oDic;
            //ddlRelg.DataValueField = "value";
            //ddlRelg.DataTextField = "key";
            //ddlRelg.DataBind();
            //ddlBRelg.DataSource = oDic;
            //ddlBRelg.DataValueField = "value";
            //ddlBRelg.DataTextField = "key";
            //ddlBRelg.DataBind();
            ddlRelg.Items.Clear();
            ddlBRelg.Items.Clear();
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            ListItem oli = new ListItem("<--Select-->", "-1");
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "ReligionId", "Religion", "ReligionMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlRelg.DataSource = dt;
                ddlRelg.DataTextField = "Religion";
                ddlRelg.DataValueField = "ReligionId";
                ddlRelg.DataBind();
                ddlRelg.Items.Insert(0, oli);

                oli = new ListItem("<--Select-->", "-1");
                ddlBRelg.DataSource = dt;
                ddlBRelg.DataTextField = "Religion";
                ddlBRelg.DataValueField = "ReligionId";
                ddlBRelg.DataBind();
                ddlBRelg.Items.Insert(0, oli);
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
        private void popAddProof()
        {
            DataTable dt = null;
            CNewMember oNM = null;
            try
            {
                oNM = new CNewMember();
                dt = oNM.popIdAddrProof("Y", "N");

                ddlBAddProf.DataSource = dt;
                ddlBAddProf.DataTextField = "IDProofName";
                ddlBAddProf.DataValueField = "IDProofId";
                ddlBAddProf.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBAddProf.Items.Insert(0, oli);

                ddlAddPrf.DataSource = dt;
                ddlAddPrf.DataTextField = "IDProofName";
                ddlAddPrf.DataValueField = "IDProofId";
                ddlAddPrf.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                //ddlAddPrf.Items.Insert(0, oli1);

                ddlIdProof3.DataSource = dt;
                ddlIdProof3.DataTextField = "IDProofName";
                ddlIdProof3.DataValueField = "IDProofId";
                ddlIdProof3.DataBind();
                ddlIdProof3.Items.Insert(0, oli1);
            }
            finally
            {
                oNM = null;
                dt = null;
            }
        }

        private void popAgainstVillage2(string vVlgId)
        {
            DataTable dt = null;
            CVillage oVlg = null;
            try
            {
                oVlg = new CVillage();
                dt = oVlg.GetGpBlkDistStateList(vVlgId);
                ddlCommMuni.DataSource = dt;
                ddlCommMuni.DataTextField = "GPName";
                ddlCommMuni.DataValueField = "GPId";
                ddlCommMuni.DataBind();
                ddlCommBlock.DataSource = dt;
                ddlCommBlock.DataTextField = "BlockName";
                ddlCommBlock.DataValueField = "BlockId";
                ddlCommBlock.DataBind();
                ddlCommDist.DataSource = dt;
                ddlCommDist.DataTextField = "DistrictName";
                ddlCommDist.DataValueField = "DistrictId";
                ddlCommDist.DataBind();
                ddlCommState.DataSource = dt;
                ddlCommState.DataTextField = "StateName";
                ddlCommState.DataValueField = "StateId";
                ddlCommState.DataBind();
            }
            finally
            {
                oVlg = null;
                dt = null;
            }
        }

        protected void chkCommAddr_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCommAddr.Checked == true)
            {
                ddlCommAddrType.SelectedIndex = ddlAddrType.SelectedIndex;
                txtCommHouseNo.Text = txtHouNo.Text;
                txtCommSt.Text = txtStName.Text;
                txtCommSubDist.Text = txtWardNo.Text;
                txtCommPost.Text = txtPOff.Text;
                txtCommPin.Text = txtPin.Text;
                txtCommLandmark.Text = txtLandMark.Text;
                txtCommArea.Text = txtArea.Text;
                txtCommMob.Text = txtMob.Text;
                txtCommPhone.Text = txtPhNo.Text;
                txtCommEmail.Text = txtEmail.Text;
                // ddlCommVill.SelectedIndex = ddlVillg.SelectedIndex;
                if (ddlBVillg.SelectedIndex >= 0)
                {
                    popAgainstVillage2(ddlBVillg.SelectedValue);
                    ddlCommMuni.SelectedIndex = ddlMuPanc.SelectedIndex;
                    ddlCommBlock.SelectedIndex = ddlBlk.SelectedIndex;
                    //  ddlCommDist.SelectedIndex = ddlDist.SelectedIndex;
                    ddlCommState.SelectedIndex = ddlStat.SelectedIndex;
                    txtMemCommAddr.Text = txtCommHouseNo.Text + "," + txtCommSt.Text + "," + ddlCommVill.SelectedItem.Text.ToString() + "," + ddlCommMuni.SelectedItem.Text.ToString() + "," + ddlCommBlock.SelectedItem.Text.ToString() + "," + txtCommSubDist.Text + "," + txtCommPost.Text + "," + txtCommPin.Text;

                }
                else
                {
                }
                txtCommHouseNo.Enabled = false;
                txtCommSt.Enabled = false;
                txtCommSubDist.Enabled = false;
                txtCommPost.Enabled = false;
                txtCommPin.Enabled = false;
                ddlCommVill.Enabled = false;
            }
            else
            {
                txtCommHouseNo.Text = "";
                txtCommSt.Text = "";
                txtCommSubDist.Text = "";
                txtCommPost.Text = "";
                txtCommPin.Text = "";
                //ddlCommVill.SelectedIndex = -1;
                ddlCommMuni.SelectedIndex = -1;
                ddlCommBlock.SelectedIndex = -1;
                ddlCommDist.SelectedIndex = -1;
                ddlCommState.SelectedIndex = -1;
                txtCommHouseNo.Enabled = true;
                txtCommSt.Enabled = true;
                txtCommSubDist.Enabled = true;
                txtCommPost.Enabled = true;
                txtCommPin.Enabled = true;
                //ddlCommVill.Enabled = true;
            }
        }

        protected void ddlCommVill_SelectedIndexChanged(object sender, EventArgs e)
        {
            popAgainstVillage2(ddlCommVill.SelectedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        private void popIdentityProof()
        {
            DataTable dt = null;
            CNewMember oNM = null;
            try
            {
                oNM = new CNewMember();
                dt = oNM.popIdAddrProof("N", "Y");

                ddlIdentyProf.DataSource = dt;
                ddlIdentyProf.DataTextField = "IDProofName";
                ddlIdentyProf.DataValueField = "IDProofId";
                ddlIdentyProf.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlIdentyProf.Items.Insert(0, oli);

                ddlBIdntyProf.DataSource = dt;
                ddlBIdntyProf.DataTextField = "IDProofName";
                ddlBIdntyProf.DataValueField = "IDProofId";
                ddlBIdntyProf.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlBIdntyProf.Items.Insert(0, oli1);
            }
            finally
            {
                oNM = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popCaste()
        {
            //Dictionary<string, Int32> oDic = new Dictionary<string, Int32>();
            //oDic.Add("<-Select->", 0);
            //oDic.Add("General", 1);
            //oDic.Add("SC", 2);
            //oDic.Add("ST", 3);
            //oDic.Add("OBC", 4);
            //oDic.Add("Minority", 5);
            //oDic.Add("Other", 6);
            //ddlCaste.DataSource = oDic;
            //ddlCaste.DataValueField = "value";
            //ddlCaste.DataTextField = "key";
            //ddlCaste.DataBind();
            //ddlBCaste.DataSource = oDic;
            //ddlBCaste.DataValueField = "value";
            //ddlBCaste.DataTextField = "key";
            //ddlBCaste.DataBind();

            ddlCaste.Items.Clear();
            ddlBCaste.Items.Clear();
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            ListItem oli = new ListItem("<--Select-->", "-1");
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "CasteId", "Caste", "CasteMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlCaste.DataSource = dt;
                ddlCaste.DataTextField = "Caste";
                ddlCaste.DataValueField = "CasteId";
                ddlCaste.DataBind();
                ddlCaste.Items.Insert(0, oli);

                oli = new ListItem("<--Select-->", "-1");
                ddlBCaste.DataSource = dt;
                ddlBCaste.DataTextField = "Caste";
                ddlBCaste.DataValueField = "CasteId";
                ddlBCaste.DataBind();
                ddlBCaste.Items.Insert(0, oli);
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
        private void popHouseHold()
        {
            Dictionary<string, Int32> oDic = new Dictionary<string, Int32>();
            oDic.Add("<-Select->", 0);
            oDic.Add("Rural", 1);
            oDic.Add("Semi Urban", 2);
            oDic.Add("Urban", 3);
            ddlHHoldTyp.DataSource = oDic;
            ddlHHoldTyp.DataValueField = "value";
            ddlHHoldTyp.DataTextField = "key";
            ddlHHoldTyp.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopGender()
        {
            ddlGend.Items.Clear();
            ddlBGend.Items.Clear();
            ddlGuarGen.Items.Clear();

            Dictionary<string, string> oGen = new Dictionary<string, string>();
            oGen.Add("<-Select->", "-1");
            oGen.Add("Female", "F");
            oGen.Add("Transgender", "O");
            oGen.Add("Third Gender", "T");

            ddlGend.DataSource = oGen;
            ddlGend.DataValueField = "value";
            ddlGend.DataTextField = "key";
            ddlGend.DataBind();

            Dictionary<string, string> oFGen = new Dictionary<string, string>();
            oFGen.Add("<-Select->", "-1");
            oFGen.Add("Male", "M");
            oFGen.Add("Female", "F");
            oFGen.Add("Transgender", "O");
            oFGen.Add("Third Gender", "T");

            ddlBGend.DataSource = oFGen;
            ddlBGend.DataValueField = "value";
            ddlBGend.DataTextField = "key";
            ddlBGend.DataBind();

            ddlGuarGen.DataSource = oFGen;
            ddlGuarGen.DataValueField = "value";
            ddlGuarGen.DataTextField = "key";
            ddlGuarGen.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAprv_Click(object sender, EventArgs e)
        {
            string vMemId = string.Empty;
            CMember oMember = null;
            Int32 vRet = 0;
            try
            {
                vMemId = Convert.ToString(ViewState["MemId"]);
                oMember = new CMember();
                vRet = oMember.ApproveMember(vMemId, "Y");
                if (vRet > 0)
                {
                    gblFuction.MsgPopup("Member Approved Successfully.");
                }
                else
                    gblFuction.MsgPopup(gblMarg.DBError);
            }
            finally
            {
                oMember = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbEmp.ActiveTabIndex = 0;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateField(string pMemberId, string pBrCode)
        {
            bool vRst = true;
            DataTable dt1 = null, dt2 = null;
            Int32 vRet1 = 0;
            DataSet ds = null;
            CMember oMem = null;

            oMem = new CMember();
            ds = oMem.ChkMemStatus(pMemberId, pBrCode);
            dt1 = ds.Tables[0];
            //dt2 = ds.Tables[1];


            if (ddlBGend.SelectedValue == "F" && ddlBRel.SelectedItem.Text == "HUSBAND")
            {
                gblFuction.MsgPopup("Please Husband cannot be female");
                vRst = false;
            }

            if (Convert.ToString(ViewState["StateEdit"]) == "Add")
            {
                if (Convert.ToInt32(Session[gblValue.StateID].ToString()) != 3)
                {
                    if (ddlAddPrf.SelectedValue != "4" && ddlIdentyProf.SelectedValue != "4")
                    {
                        gblFuction.MsgPopup("Adhar Card is mandatory for your Address Proof or Identity Proof");
                        vRst = false;
                    }
                }
            }



            return vRst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vMemberId"></param>
        private void FillMemberDtl(string vMemberId, string vBrCode)
        {
            ClearControls();
            popState();
            string vStat = "";
            ViewState["MemId"] = "";
            ViewState["LoanAppId"] = "";
            DataSet ds = null;
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            DataTable dt4 = null;
            DataTable dt5 = null;
            DataTable dtDisc = null;
            CMember oMem = null;
            int vSlid = 0;
            try
            {
                //vBrCode = Session[gblValue.BrnchCode].ToString();
                //string vBranchCode = ddlBranch.SelectedValue;
                ViewState["MemId"] = vMemberId;
                ViewState["LoanAppId"] = Request.QueryString["LoanAppId"];
                oMem = new CMember();

                //dt = oMem.GetMemberDetails(vMemberId, vBrCode);
                ds = oMem.GetMemberDetails(vMemberId, Request.QueryString["LoanAppId"], vBrCode, Convert.ToInt32(Session[gblValue.UserId]));
                dt = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                dt4 = ds.Tables[3];
                dt5 = ds.Tables[4];
                dtDisc = ds.Tables[6];

                if (dt.Rows.Count > 0)
                {
                    string pReqData = "{\"pId\":\"" + vMemberId + "\"}";
                    //GenerateReport("GetPassBookImage", pReqData);
                    ddlAbledYN.SelectedIndex = ddlAbledYN.Items.IndexOf(ddlAbledYN.Items.FindByValue(dt.Rows[0]["IsAbledYN"].ToString()));
                    ddlSpclAbled.SelectedIndex = ddlSpclAbled.Items.IndexOf(ddlSpclAbled.Items.FindByValue(dt.Rows[0]["SpeciallyAbled"].ToString()));

                    if (Convert.ToInt32(Session[gblValue.RoleId]) == 1)
                    {
                        txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["M_AddProfNo"]);
                        txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]);
                        txtIdProof3.Text = Convert.ToString(dt.Rows[0]["AddProfNo2"]);
                        txtBAddPrfNo.Text = Convert.ToString(dt.Rows[0]["B_AddProfNo"]);
                        txtBIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["B_IdentyProfNo"]);
                    }
                    else
                    {
                        if (dt5.Rows[0]["ViewAadhar"].ToString() == "N" && Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                        {
                            if (dt.Rows[0]["M_IdentyPRofId"].ToString() == "1")
                            {
                                txtIdntPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]).Substring(Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]).Length - 4, 4));
                            }
                            else
                            {
                                txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]);
                            }

                            if (dt.Rows[0]["M_AddProfId"].ToString() == "1")
                            {
                                txtAddPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["M_AddProfNo"]).Substring(Convert.ToString(dt.Rows[0]["M_AddProfNo"]).Length - 4, 4));
                            }
                            else
                            {
                                txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["M_AddProfNo"]);
                            }

                            if (dt.Rows[0]["AddProfId2"].ToString() == "1")
                            {
                                txtIdProof3.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["AddProfNo2"]).Substring(Convert.ToString(dt.Rows[0]["AddProfNo2"]).Length - 4, 4));
                            }
                            else
                            {
                                txtIdProof3.Text = Convert.ToString(dt.Rows[0]["AddProfNo2"]);
                            }

                            if (dt.Rows[0]["B_IdentyProfId"].ToString() == "1")
                            {
                                txtBIdntPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["B_IdentyProfNo"]).Substring(Convert.ToString(dt.Rows[0]["B_IdentyProfNo"]).Length - 4, 4));
                            }
                            else
                            {
                                txtBIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["B_IdentyProfNo"]);
                            }

                            if (dt.Rows[0]["B_AddProfId"].ToString() == "1")
                            {
                                txtBAddPrfNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["B_AddProfNo"]).Substring(Convert.ToString(dt.Rows[0]["B_AddProfNo"]).Length - 4, 4));

                            }
                            else
                            {
                                txtBAddPrfNo.Text = Convert.ToString(dt.Rows[0]["B_AddProfNo"]);
                            }
                        }
                        else
                        {
                            txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["M_AddProfNo"]);
                            txtIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]);
                            txtIdProof3.Text = Convert.ToString(dt.Rows[0]["AddProfNo2"]);
                            txtBAddPrfNo.Text = Convert.ToString(dt.Rows[0]["B_AddProfNo"]);
                            txtBIdntPrfNo.Text = Convert.ToString(dt.Rows[0]["B_IdentyProfNo"]);
                        }
                    }
                    txtMemID.Text = Convert.ToString(dt.Rows[0]["MemberID"]);
                    txtAdmDt.Text = Convert.ToString(dt.Rows[0]["AdmDate"]);
                    txtFName.Text = Convert.ToString(dt.Rows[0]["MF_Name"]);
                    txtMName.Text = Convert.ToString(dt.Rows[0]["MM_Name"]);
                    txtLName.Text = Convert.ToString(dt.Rows[0]["ML_Name"]);

                    txtFirstName.Text = Convert.ToString(dt.Rows[0]["MF_Name"]);
                    txtMiddleName.Text = Convert.ToString(dt.Rows[0]["MM_Name"]);
                    txtLastName.Text = Convert.ToString(dt.Rows[0]["ML_Name"]);

                    txtHFName.Text = Convert.ToString(dt.Rows[0]["MHF_Name"]);
                    vStat = Convert.ToString(dt.Rows[0]["Father_YN"]);
                    if (vStat == "Y")
                        chkFath.Checked = true;
                    else
                        chkFath.Checked = false;
                    txtDOB.Text = Convert.ToString(dt.Rows[0]["M_DOB"]);
                    txtAge.Text = Convert.ToString(dt.Rows[0]["M_Age"]);

                    txtMDOB.Text = Convert.ToString(dt.Rows[0]["M_DOB"]);
                    txtMAge.Text = Convert.ToString(dt.Rows[0]["M_Age"]);

                    txtHouNo.Text = Convert.ToString(dt.Rows[0]["M_HouseNo"]);
                    txtStName.Text = Convert.ToString(dt.Rows[0]["M_Street"]);
                    txtWardNo.Text = Convert.ToString(dt.Rows[0]["M_WardNo"]);
                    txtPOff.Text = Convert.ToString(dt.Rows[0]["M_PostOff"]);
                    txtLandMark.Text = Convert.ToString(dt.Rows[0]["Landmark"]);
                    txtArea.Text = Convert.ToString(dt.Rows[0]["Area"]);
                    txtPin.Text = Convert.ToString(dt.Rows[0]["M_PIN"]);
                    txtMemAddr.Text = Convert.ToString(dt.Rows[0]["MemAddr"]);
                    txtMob.Text = Convert.ToString(dt.Rows[0]["M_Mobile"]);
                    txtPhNo.Text = Convert.ToString(dt.Rows[0]["M_Phone"]);
                    txtCustId.Text = Convert.ToString(dt.Rows[0]["IDBICustId"]);
                    txtSavingsAcNo.Text = Convert.ToString(dt.Rows[0]["IDBISavingsAcNo"]);

                    txtCommHouseNo.Text = Convert.ToString(dt.Rows[0]["HouseNo_p"]);
                    txtCommSt.Text = Convert.ToString(dt.Rows[0]["Street_p"]);
                    txtCommSubDist.Text = Convert.ToString(dt.Rows[0]["WardNo_p"]);
                    txtCommPost.Text = Convert.ToString(dt.Rows[0]["PostOff_p"]);
                    txtCommPin.Text = Convert.ToString(dt.Rows[0]["PIN_p"]);
                    txtCommMob.Text = Convert.ToString(dt.Rows[0]["MobileNo_p"]);
                    txtCommLandmark.Text = Convert.ToString(dt.Rows[0]["Landmark_p"]);
                    txtCommArea.Text = Convert.ToString(dt.Rows[0]["Area_p"]);
                    txtCommEmail.Text = Convert.ToString(dt.Rows[0]["EmailId_p"]);
                    txtStayYear.Text = Convert.ToString(dt.Rows[0]["YearsOfStay"]);
                  
                    txtBFName.Text = Convert.ToString(dt.Rows[0]["B_FName"]);
                    txtBMName.Text = Convert.ToString(dt.Rows[0]["B_MName"]);
                    txtBLName.Text = Convert.ToString(dt.Rows[0]["B_LName"]);
                    txtBDOBDt.Text = Convert.ToString(dt.Rows[0]["B_DOB"]);
                    txtBAge.Text = Convert.ToString(dt.Rows[0]["B_Age"]);
                    txtBHouNo.Text = Convert.ToString(dt.Rows[0]["B_HouseNo"]);
                    txtBStreet.Text = Convert.ToString(dt.Rows[0]["B_Street"]);
                    txtBWardNo.Text = Convert.ToString(dt.Rows[0]["B_WardNo"]);
                    txtBPOff.Text = Convert.ToString(dt.Rows[0]["B_PostOff"]);
                    txtBPin.Text = Convert.ToString(dt.Rows[0]["B_PIN"]);
                    txtCoBrwrAddr.Text = Convert.ToString(dt.Rows[0]["CoBrwrAddr"]);
                    txtBMobNo.Text = Convert.ToString(dt.Rows[0]["B_Mobile"]);
                    txtBPhNo.Text = Convert.ToString(dt.Rows[0]["B_Phone"]);                  
                    txtBLandmark.Text = Convert.ToString(dt.Rows[0]["B_Landmark"]);
                    txtBArea.Text = Convert.ToString(dt.Rows[0]["B_Area"]);
                    txtBEmail.Text = Convert.ToString(dt.Rows[0]["B_Email"]);

                    txtBVillage.Text = Convert.ToString(dt.Rows[0]["B_Village"]);
                    txtBDist.Text = Convert.ToString(dt.Rows[0]["B_District"]);

                    ddlBStat.DataSource = dt;
                    ddlBStat.DataTextField = "B_StateName";
                    ddlBStat.DataValueField = "B_StateId";
                    ddlBStat.DataBind();

                    txtIncome.Text = Convert.ToString(dt.Rows[0]["IncAmt"]);
                    txtExpnses.Text = Convert.ToString(dt.Rows[0]["ExpAmt"]);
                    txtMFIName.Text = Convert.ToString(dt.Rows[0]["MFI_1"]);
                    txtLnAmt.Text = Convert.ToString(dt.Rows[0]["LoanAmt_1"]);
                    if (dt.Rows[0]["LoanDt_1"].ToString() != "01/01/1900")
                        txtLnDt.Text = Convert.ToString(dt.Rows[0]["LoanDt_1"]);
                    txtInstAmt.Text = Convert.ToString(dt.Rows[0]["InstAmt_1"]);
                    txtInstLeft.Text = Convert.ToString(dt.Rows[0]["InstNo_1"]);
                    txtDueAmt.Text = Convert.ToString(dt.Rows[0]["DueAmt_1"]);
                    txtMFIName1.Text = Convert.ToString(dt.Rows[0]["MFI_2"]);
                    txtLnAmt1.Text = Convert.ToString(dt.Rows[0]["LoanAmt_2"]);
                    if (dt.Rows[0]["LoanDt_2"].ToString() != "01/01/1900")
                        txtLnDt1.Text = Convert.ToString(dt.Rows[0]["LoanDt_2"]);
                    txtInstAmt1.Text = Convert.ToString(dt.Rows[0]["InstAmt_2"]);
                    txtInstLeft1.Text = Convert.ToString(dt.Rows[0]["InstNo_2"]);
                    txtDueAmt1.Text = Convert.ToString(dt.Rows[0]["DueAmt_2"]);
                    txtBPLNo.Text = Convert.ToString(dt.Rows[0]["BPLNo"]);
                    txtPjMeetDt.Text = Convert.ToString(dt.Rows[0]["PjMeetDt"]);
                    txtBankName.Text = Convert.ToString(dt.Rows[0]["BankName"]);

                    ddlPvLine.SelectedIndex = ddlPvLine.Items.IndexOf(ddlPvLine.Items.FindByValue(dt.Rows[0]["APL"].ToString()));
                    ddlMrySts.SelectedIndex = ddlMrySts.Items.IndexOf(ddlMrySts.Items.FindByValue(dt.Rows[0]["MM_Status"].ToString()));
                    ddlEduc.SelectedIndex = ddlEduc.Items.IndexOf(ddlEduc.Items.FindByValue(dt.Rows[0]["M_QualificationId"].ToString()));
                    ddlOcup.SelectedIndex = ddlOcup.Items.IndexOf(ddlOcup.Items.FindByValue(dt.Rows[0]["M_OccupationId"].ToString()));
                    ddlGend.SelectedIndex = ddlGend.Items.IndexOf(ddlGend.Items.FindByValue(dt.Rows[0]["M_Gender"].ToString()));
                    ddlRelg.ClearSelection();
                    ddlRelg.SelectedIndex = ddlRelg.Items.IndexOf(ddlRelg.Items.FindByValue(dt.Rows[0]["M_RelgId"].ToString()));
                    ddlCaste.ClearSelection();
                    ddlCaste.SelectedIndex = ddlCaste.Items.IndexOf(ddlCaste.Items.FindByValue(dt.Rows[0]["M_Caste"].ToString()));
                    ddlIdentyProf.SelectedIndex = ddlIdentyProf.Items.IndexOf(ddlIdentyProf.Items.FindByValue(dt.Rows[0]["M_IdentyPRofId"].ToString()));

                    txtVillg.Text = dt.Rows[0]["Village"].ToString();
                    ddlStat.SelectedIndex = ddlStat.Items.IndexOf(ddlStat.Items.FindByText(dt.Rows[0]["State"].ToString()));
                    txtDist.Text = Convert.ToString(dt.Rows[0]["District"]);
                    ddlAddPrf.ClearSelection();
                    ddlAddPrf.SelectedIndex = ddlAddPrf.Items.IndexOf(ddlAddPrf.Items.FindByValue(dt.Rows[0]["M_AddProfId"].ToString()));


                    txtBranch.Text = Convert.ToString(dt.Rows[0]["BankBranch"]);
                    txtAccNo.Attributes.Add("value", Convert.ToString(dt.Rows[0]["AccNo"]));
                    //txtAccNo.Text = Convert.ToString(dt.Rows[0]["AccNo"]);
                    txtReAccNo.Text = Convert.ToString(dt.Rows[0]["AccNo"]);
                    txtIFSC.Text = Convert.ToString(dt.Rows[0]["IFSCCode"]);
                    txtMemNamePBook.Text = Convert.ToString(dt.Rows[0]["MemNamePBook"]);
                    ddlBRel.SelectedIndex = ddlBRel.Items.IndexOf(ddlBRel.Items.FindByValue(dt.Rows[0]["B_HumanRelationId"].ToString()));
                    ddlBEdu.SelectedIndex = ddlBEdu.Items.IndexOf(ddlBEdu.Items.FindByValue(dt.Rows[0]["B_QualificationId"].ToString()));
                    ddlBOcup.SelectedIndex = ddlBOcup.Items.IndexOf(ddlBOcup.Items.FindByValue(dt.Rows[0]["B_OccupationId"].ToString()));
                    ddlBGend.SelectedIndex = ddlBGend.Items.IndexOf(ddlBGend.Items.FindByValue(dt.Rows[0]["B_Gender"].ToString()));
                    ddlBRelg.ClearSelection();
                    ddlBRelg.SelectedIndex = ddlBRelg.Items.IndexOf(ddlBRelg.Items.FindByValue(dt.Rows[0]["B_RelgId"].ToString()));
                    ddlBCaste.ClearSelection();
                    ddlBCaste.SelectedIndex = ddlBCaste.Items.IndexOf(ddlBCaste.Items.FindByValue(dt.Rows[0]["B_Caste"].ToString()));
                    ddlBVillg.SelectedIndex = ddlBVillg.Items.IndexOf(ddlBVillg.Items.FindByValue(dt.Rows[0]["B_VillageID"].ToString()));
                    PopBVillDtl(Convert.ToInt32(dt.Rows[0]["B_VillageID"].ToString()));
                    ddlBAddProf.SelectedIndex = ddlBAddProf.Items.IndexOf(ddlBAddProf.Items.FindByValue(dt.Rows[0]["B_AddProfId"].ToString()));
                    ddlBIdntyProf.SelectedIndex = ddlBIdntyProf.Items.IndexOf(ddlBIdntyProf.Items.FindByValue(dt.Rows[0]["B_IdentyProfId"].ToString()));
                    ////ddlRO.SelectedIndex = ddlRO.Items.IndexOf(ddlRO.Items.FindByValue(dt.Rows[0]["DueAmt_2"].ToString()));
                    ddlAccType.SelectedIndex = ddlAccType.Items.IndexOf(ddlAccType.Items.FindByValue(dt.Rows[0]["Acc_Type"].ToString()));
                    ddlIdProof3.SelectedIndex = ddlIdProof3.Items.IndexOf(ddlIdProof3.Items.FindByValue(dt.Rows[0]["AddProfId2"].ToString()));
                    ddlAddrType.SelectedIndex = ddlAddrType.Items.IndexOf(ddlAddrType.Items.FindByValue(dt.Rows[0]["AddrType"].ToString()));
                    ddlCommAddrType.SelectedIndex = ddlCommAddrType.Items.IndexOf(ddlCommAddrType.Items.FindByValue(dt.Rows[0]["AddrType_p"].ToString()));
                    ddlCommVill.SelectedIndex = ddlCommVill.Items.IndexOf(ddlCommVill.Items.FindByValue(dt.Rows[0]["VillageId_p"].ToString()));
                    popAgainstVillage2(dt.Rows[0]["VillageId_p"].ToString());
                    ddlAreaCategory.SelectedIndex = ddlAreaCategory.Items.IndexOf(ddlAreaCategory.Items.FindByValue(dt.Rows[0]["Area_Category"].ToString()));
                    ////ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["MarketId"].ToString()));
                    ////ddlGroup.SelectedIndex = ddlGroup.Items.IndexOf(ddlGroup.Items.FindByValue(dt.Rows[0]["GroupId"].ToString()));
                    GetGroup(dt.Rows[0]["GroupId"].ToString());
                    PopCollDay(dt.Rows[0]["GroupId"].ToString(), vBrCode);
                    ////ddlMetDay.SelectedIndex = ddlMetDay.Items.IndexOf(ddlMetDay.Items.FindByValue(dt.Rows[0]["MeetingDay"].ToString()));
                    txtMetDay.Text = dt.Rows[0]["MeetingDay"].ToString();
                    //ddlHHoldTyp.SelectedIndex = ddlHHoldTyp.Items.IndexOf(ddlHHoldTyp.Items.FindByValue(dt.Rows[0]["HouseHoldId"].ToString()));
                    txtNoOfDpndnts.Text = Convert.ToString(dt.Rows[0]["NoOfDependants"]);

                    txtMemCommAddr.Text = txtCommHouseNo.Text + "," + txtCommSt.Text + "," + ddlCommVill.SelectedItem.Text.ToString() + "," + ddlCommMuni.SelectedItem.Text.ToString() + "," + ddlCommBlock.SelectedItem.Text.ToString() + "," + txtCommSubDist.Text + "," + txtCommPost.Text + "," + txtCommPin.Text;


                    if (dt.Rows[0]["Tra_DropDate"].ToString() != "01/01/1900" && dt.Rows[0]["Tra_DropDate"].ToString() != "")
                    {
                        txtDtCl.Text = dt.Rows[0]["Tra_DropDate"].ToString();
                        cbDrp.Checked = true;
                        ddlClTyp.SelectedIndex = ddlClTyp.Items.IndexOf(ddlClTyp.Items.FindByValue(dt.Rows[0]["Tra_Drop"].ToString()));
                        txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                    }
                    else
                    {
                        txtDtCl.Text = "";
                        cbDrp.Checked = false;
                        ddlClTyp.SelectedIndex = -1;
                        txtRemarks.Text = "";
                    }
                    txtNoOfHouseMember.Text = Convert.ToString(dt.Rows[0]["No_of_House_Member"]);
                    txtNoOfChild.Text = Convert.ToString(dt.Rows[0]["No_of_Children"]);
                    txtBranchDistance.Text = Convert.ToString(dt.Rows[0]["Distance_frm_Branch"]);
                    txtCollCenterDistance.Text = Convert.ToString(dt.Rows[0]["Distance_frm_Coll_Center"]);
                    //txtIdProof3.Text = Convert.ToString(dt.Rows[0]["AddProfNo2"]);

                    txtMaidenNmF.Text = Convert.ToString(dt.Rows[0]["MaidenNmF"].ToString());
                    txtMaidenNmM.Text = Convert.ToString(dt.Rows[0]["MaidenNmM"].ToString());
                    txtMaidenNmL.Text = Convert.ToString(dt.Rows[0]["MaidenNmL"].ToString());
                    txtGuarName.Text = Convert.ToString(dt.Rows[0]["GuarFName"].ToString());
                    txtGuarLName.Text = Convert.ToString(dt.Rows[0]["GuarLName"].ToString());
                    ddlGuarRel.SelectedIndex = ddlGuarRel.Items.IndexOf(ddlGuarRel.Items.FindByValue(Convert.ToString(dt.Rows[0]["GuarRel"].ToString())));
                    txtGuarDOB.Text = Convert.ToString(dt.Rows[0]["GuarDOB"].ToString());
                    txtGuarAge.Text = Convert.ToString(dt.Rows[0]["GuarAge"].ToString());
                    ddlGuarGen.SelectedIndex = ddlGuarGen.Items.IndexOf(ddlGuarGen.Items.FindByValue(Convert.ToString(dt.Rows[0]["GuarGen"].ToString())));
                    ddlMinority.SelectedIndex = ddlMinority.Items.IndexOf(ddlMinority.Items.FindByValue(dt.Rows[0]["MinorityYN"].ToString()));

                    //if (Session[gblValue.BrnchCode].ToString() == "0000")
                    //{
                    //    hdnLoanAppId.Value = Convert.ToString(dt.Rows[0]["LoanAppId"].ToString());
                    //}

                    //ViewState["LoanAppId"] = hdnLoanAppId.Value;
                    ////
                    DataRow dF;
                    dF = dt2.NewRow();
                    dt2.Rows.Add(dF);
                    dt2.AcceptChanges();
                    ViewState["Fam"] = dt2;
                    gvFamily.DataSource = dt2;
                    gvFamily.DataBind();
                    ////
                    DataRow dF1;
                    dF1 = dt3.NewRow();
                    dt3.Rows.Add(dF1);
                    dt3.AcceptChanges();
                    ViewState["Asset"] = dt3;
                    gvAsset.DataSource = dt3;
                    gvAsset.DataBind();

                    if (Session[gblValue.BrnchCode].ToString() == "0000")
                    {
                        DataRow dF2;
                        
                        if (dtDisc.Rows.Count == 0)
                        {
                            dF2 = dtDisc.NewRow();
                            dF2["SlId"] = vSlid + 1;
                            dF2["LoanAppId"] = Request.QueryString["LoanAppId"];
                            dtDisc.Rows.Add(dF2);
                            dtDisc.AcceptChanges();
                        }

                        //for (int i = 0; i < dtDisc.Rows.Count; i++)
                        //{
                        //    if (dtDisc.Rows[i]["LoanAppId"].ToString() == Request.QueryString["LoanAppId"].ToString())
                        //    {
                        //        vSlid = vSlid + 1;
                        //    }
                        //}
                    }

                    ViewState["DiscrepancyTbl"] = dtDisc;
                    gvDiscrepancy.DataSource = dtDisc;
                    gvDiscrepancy.DataBind();

                    ///income/expenses
                    ///
                    if (dt4.Rows.Count > 0)
                    {
                        txtFamilyInc.Text = Convert.ToString(dt4.Rows[0]["FamilyIncome"]);
                        txtSelfInc.Text = Convert.ToString(dt4.Rows[0]["SlefIncome"]);
                        txtOtherInc.Text = Convert.ToString(dt4.Rows[0]["OtherIncome"]);
                        txtTotInc.Text = Convert.ToString(dt4.Rows[0]["TotIncome"]);
                        txtHsRntAmt.Text = Convert.ToString(dt4.Rows[0]["ExHsRntAmt"]);
                        txtFdAmt.Text = Convert.ToString(dt4.Rows[0]["ExpFdAmt"]);
                        txtEduAmt.Text = Convert.ToString(dt4.Rows[0]["ExpEduAmt"]);
                        txtMedAmt.Text = Convert.ToString(dt4.Rows[0]["ExpMedAmt"]);
                        txtLnInsAmt.Text = Convert.ToString(dt4.Rows[0]["ExpLnInsAmt"]);
                        txtFuelExp.Text = Convert.ToString(dt4.Rows[0]["ExpFuelAmt"]);
                        txtOtherExp.Text = Convert.ToString(dt4.Rows[0]["ExpOtherAmt"]);
                        txtElecExp.Text = Convert.ToString(dt4.Rows[0]["ExpElectricAmt"]);
                        txtTransExp.Text = Convert.ToString(dt4.Rows[0]["ExpTransAmt"]);
                        txtTotExp.Text = Convert.ToString(dt4.Rows[0]["TotalExp"]);
                        txtSurplus.Text = Convert.ToString(dt4.Rows[0]["Surplus"]);
                    }
                    DataTable dtEnq = new DataTable();
                    oMem = new CMember();
                    dtEnq = oMem.GetLastEnqIdByMemId(txtMemID.Text.ToString());
                    if (dtEnq.Rows.Count > 0)
                    {
                        hdnEnqId.Value = dtEnq.Rows[0]["EnquiryId"].ToString();
                        hdnEnqDate.Value = dtEnq.Rows[0]["EnqDate"].ToString();
                        //memberKYC(dtEnq.Rows[0]["EnquiryId"].ToString());
                        //memberPassbook(txtMemID.Text.ToString());
                        //string pRequestdata = "{\"pEnquiryId\":\"" + hdnEnqId.Value + "\"}";
                        //GenerateReport("GetImage", pRequestdata);
                    }
                    else
                    {
                        hdnEnqId.Value = "";
                        hdnEnqDate.Value = "";
                    }

                    EnableControl(false);
                }
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void memberKYC(string InitialApproachId)
        {
            string imgFolder = InitialApproachId;
            string pathImage = ConfigurationManager.AppSettings["PathImage"];
            string vUrl = pathImage + "/InitialApproach/";

            imgMemPhoto.ImageUrl = vUrl + imgFolder + "/MemberPhoto.png";
            imgMemIdProof.ImageUrl = vUrl + imgFolder + "/IDProofImage.png";
            imgMemIdProofBack.ImageUrl = vUrl + imgFolder + "/IDProofImageBack.png";
            imgMemAddrProof.ImageUrl = vUrl + imgFolder + "/AddressProofImage.png";
            imgMemAddrProofBack.ImageUrl = vUrl + imgFolder + "/AddressProofImageBack.png";
            imgMemAddrProof2.ImageUrl = vUrl + imgFolder + "/AddressProofImage2.png";
            imgMemAddrProofBack2.ImageUrl = vUrl + imgFolder + "/AddressProofImage2Back.png";
        }

        private void memberPassbook(string MemberId)
        {
            string imgFolder = MemberId;
            string pathImage = ConfigurationManager.AppSettings["PathImage"];
            imgMemPassbook.ImageUrl = pathImage + "Member/" + MemberId + "/PassbookImage.png";
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnableCGT()
        {
            string vMemberId = ViewState["MemId"].ToString();
            CMember oMem = null;
            string vBrCode = ViewState["BrCode"].ToString();
            int vRet = 0;

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oMem = new CMember();
                vRet = oMem.ChkApplycgt(vMemberId, vLogDt);
                if (vRet > 0)
                    btnCgtApply.Enabled = true;
                else
                    btnCgtApply.Enabled = false;

            }
            finally
            {
                oMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void ChkMemDayEnd()
        {
            string vMemberId = ViewState["MemId"].ToString();
            CMember oMem = null;
            string vBrCode = ViewState["BrCode"].ToString();
            int vRet = 0;

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oMem = new CMember();
                vRet = oMem.ChkMemDayEnd(vMemberId, vBrCode);
                if (vRet > 0)
                {
                    gblFuction.MsgPopup("Day End is already done.You can not edit the member records");

                }
                else
                {
                    gblFuction.MsgPopup("");
                }

            }
            finally
            {
                oMem = null;
            }
        }

        protected void cblTrnsDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vRec = 0;
            string vEoId = Convert.ToString(ViewState["EoId"]);
            CEO oEo = new CEO();
            CGblIdGenerator oGbl = new CGblIdGenerator();
            vRec = oGbl.ChkDeleteString(vEoId, "EOID", "MarketMSt");
            if (vRec > 0)
            {
                gblFuction.MsgPopup("The RO has Center, you can not Transfer the RO.");
                //cblTrnsDrop.ClearSelection();
            }
            else
            {

            }
            if (vRec > 0)
            {
                gblFuction.MsgPopup("The RO has Center, you can not Dropout the RO.");
                //cblTrnsDrop.ClearSelection();
            }
            else
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMrySts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMrySts.SelectedValue == "M01" && ddlGend.SelectedValue == "F")
            {
                chkFath.Enabled = false;
                chkFath.Checked = false;
                txtBFName.Text = txtHFName.Text;
                ddlBRel.SelectedIndex = ddlBRel.Items.IndexOf(ddlBRel.Items.FindByText("HUSBAND"));
                ddlBGend.SelectedIndex = ddlBGend.Items.IndexOf(ddlBGend.Items.FindByText("Male"));
            }
            else if (ddlMrySts.SelectedValue == "M05" && ddlGend.SelectedValue == "F")
            {
                chkFath.Enabled = false;
                chkFath.Checked = false;
                txtBFName.Text = txtHFName.Text;
                ddlBRel.SelectedIndex = -1;
                ddlBGend.SelectedIndex = ddlBGend.Items.IndexOf(ddlBGend.Items.FindByText("Male"));
            }
            else
            {
                chkFath.Enabled = true;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popCltype()
        {
            ListItem[] items = new ListItem[3];
            items[0] = new ListItem("<--Select-->", "-1");
            items[1] = new ListItem("Normal", "N");
            items[2] = new ListItem("Unsatisfied", "U");
            ddlClTyp.Items.AddRange(items);
            ddlClTyp.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PrevDt"></param>
        /// <param name="NxtDt"></param>
        /// <returns></returns>
        protected Boolean Datechk(string PrevDt, string NxtDt)
        {
            if (gblFuction.setDate(PrevDt) < gblFuction.setDate(NxtDt))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pGroupId"></param>
        private void GetGroup(string pGroupId)
        {
            DataTable dt = null, dtCent = null, dtRo = null;
            CMember oMem = null;
            CGblIdGenerator oGb = null;
            try
            {
                oMem = new CMember();
                dt = oMem.GetMemberByGroupId(pGroupId);
                if (dt.Rows.Count > 0)
                {
                    ddlGroup.DataSource = dt;
                    ddlGroup.DataTextField = "GroupName";
                    ddlGroup.DataValueField = "Groupid";
                    ddlGroup.DataBind();
                    ddlGroup.SelectedIndex = ddlGroup.Items.IndexOf(ddlGroup.Items.FindByValue(pGroupId));
                    //Get Center
                    oGb = new CGblIdGenerator();
                    dtCent = oGb.PopComboMIS("S", "N", "AA", "MarketID", "Market", "MarketMSt", dt.Rows[0]["MarketID"].ToString(), "MarketID", "AA", gblFuction.setDate("01/01/1900"), "0000");
                    if (dtCent.Rows.Count > 0)
                    {
                        ddlCenter.DataSource = dtCent;
                        ddlCenter.DataTextField = "Market";
                        ddlCenter.DataValueField = "MarketID";
                        ddlCenter.DataBind();
                        ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["MarketId"].ToString()));
                    }
                    //Get RO
                    oGb = new CGblIdGenerator();
                    dtRo = oGb.PopComboMIS("S", "N", "AA", "Eoid", "EoName", "EoMst", dt.Rows[0]["Eoid"].ToString(), "Eoid", "AA", gblFuction.setDate("01/01/1900"), "0000");
                    if (dtRo.Rows.Count > 0)
                    {
                        ddlRO.DataSource = dtRo;
                        ddlRO.DataTextField = "EoName";
                        ddlRO.DataValueField = "Eoid";
                        ddlRO.DataBind();
                        ddlRO.SelectedIndex = ddlRO.Items.IndexOf(ddlRO.Items.FindByValue(dt.Rows[0]["Eoid"].ToString()));
                    }
                }
            }
            finally
            {
                dt = null;
                dtCent = null;
                dtRo = null;
                oMem = null;
                oGb = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pVillageId"></param>
        private void PopMVillDtl(Int32 pVillageId)
        {
            DataTable dt = null;
            CGroup oGb = null;
            try
            {
                if (pVillageId > 0)
                {
                    oGb = new CGroup();
                    dt = oGb.GetVillageDtls(pVillageId);
                    ddlStat.DataSource = dt;
                    ddlStat.DataTextField = "StateName";
                    ddlStat.DataValueField = "StateID";
                    ddlStat.DataBind();
                    //ddlDist.DataSource = dt;
                    //ddlDist.DataTextField = "DistrictName";
                    //ddlDist.DataValueField = "DistrictId";
                    //ddlDist.DataBind();
                    ddlBlk.DataSource = dt;
                    ddlBlk.DataTextField = "BlockName";
                    ddlBlk.DataValueField = "BlockId";
                    ddlBlk.DataBind();
                    ddlMuPanc.DataSource = dt;
                    ddlMuPanc.DataTextField = "GPName";
                    ddlMuPanc.DataValueField = "GPId";
                    ddlMuPanc.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlStat.Items.Insert(1, oli);
                    //ddlDist.Items.Insert(1, oli);
                    ddlBlk.Items.Insert(1, oli);
                    ddlMuPanc.Items.Insert(1, oli);
                }
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
        /// <param name="pVillageId"></param>
        private void PopBVillDtl(Int32 pVillageId)
        {
            DataTable dt = null;
            CGroup oGb = null;
            try
            {
                if (pVillageId > 0)
                {
                    oGb = new CGroup();
                    dt = oGb.GetVillageDtls(pVillageId);
                    ddlBStat.DataSource = dt;
                    ddlBStat.DataTextField = "StateName";
                    ddlBStat.DataValueField = "StateID";
                    ddlBStat.DataBind();
                    ddlBDist.DataSource = dt;
                    ddlBDist.DataTextField = "DistrictName";
                    ddlBDist.DataValueField = "DistrictId";
                    ddlBDist.DataBind();
                    ddlBBlk.DataSource = dt;
                    ddlBBlk.DataTextField = "BlockName";
                    ddlBBlk.DataValueField = "BlockId";
                    ddlBBlk.DataBind();
                    ddlBMunPanca.DataSource = dt;
                    ddlBMunPanca.DataTextField = "GPName";
                    ddlBMunPanca.DataValueField = "GPId";
                    ddlBMunPanca.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlBStat.Items.Insert(1, oli);
                    ddlBDist.Items.Insert(1, oli);
                    ddlBBlk.Items.Insert(1, oli);
                    ddlBMunPanca.Items.Insert(1, oli);
                }
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
        /// <param name="Status"></param>

        private void EnableControl(Boolean Status)
        {
            txtMemID.Enabled = Status;
            txtMemAddr.Enabled = false;
            txtCoBrwrAddr.Enabled = Status;
            txtPjMeetDt.Enabled = Status;
            cbDrp.Enabled = Status;
            txtDtCl.Enabled = Status;
            ddlClTyp.Enabled = Status;
            txtRemarks.Enabled = Status;
            txtFName.Enabled = false;
            txtMName.Enabled = false;
            txtLName.Enabled = false;
            txtHFName.Enabled = false;
            chkFath.Enabled = false;
            txtDOB.Enabled = false;
            txtAge.Enabled = false;
            txtHouNo.Enabled = false;
            txtStName.Enabled = false;
            txtWardNo.Enabled = false;
            txtPOff.Enabled = false;
            txtPin.Enabled = false;
            txtMob.Enabled = false;
            txtPhNo.Enabled = Status;
            txtPin.Enabled = false;
            txtBFName.Enabled = Status;
            txtBMName.Enabled = Status;
            txtBLName.Enabled = Status;
            txtBDOBDt.Enabled = Status;
            txtBAge.Enabled = Status;
            txtBHouNo.Enabled = Status;
            txtBStreet.Enabled = Status;
            txtNoOfDpndnts.Enabled = Status;
            txtBWardNo.Enabled = Status;
            txtBPOff.Enabled = Status;
            txtBPin.Enabled = Status;
            txtBMobNo.Enabled = Status;
            txtBPhNo.Enabled = Status;
            txtIncome.Enabled = Status;
            txtExpnses.Enabled = Status;
            ddlMrySts.Enabled = Status;
            ddlEduc.Enabled = Status;
            ddlOcup.Enabled = Status;
            ddlGend.Enabled = Status;
            ddlRelg.Enabled = Status;
            ddlCaste.Enabled = Status;
            txtVillg.Enabled = Status;
            ddlAddPrf.Enabled = Status;
            ddlIdentyProf.Enabled = Status;
            txtBankName.Enabled = Status;
            txtBranch.Enabled = Status;
            txtAccNo.Enabled = Status;
            txtIFSC.Enabled = Status;
            txtMemNamePBook.Enabled = Status;
            ddlBRel.Enabled = Status;
            ddlBEdu.Enabled = Status;
            ddlBOcup.Enabled = Status;
            ddlBGend.Enabled = Status;
            ddlBRelg.Enabled = Status;
            ddlBCaste.Enabled = Status;
            ddlBVillg.Enabled = Status;
            ddlBAddProf.Enabled = Status;
            ddlBIdntyProf.Enabled = Status;
            ddlCenter.Enabled = Status;
            ddlHHoldTyp.Enabled = Status;

            ddlGroup.Enabled = Status;
            ddlRO.Enabled = Status;
            gvAsset.Enabled = Status;
            txtNetIncome.Enabled = Status;
            txtIncA.Enabled = Status;
            txtAmtA.Enabled = Status;
            txtIncB.Enabled = Status;
            txtAmtB.Enabled = Status;
            txtIncC.Enabled = Status;
            txtAmtC.Enabled = Status;
            txtIncD.Enabled = Status;
            txtAmtD.Enabled = Status;
            txtIncE.Enabled = Status;
            txtAmtE.Enabled = Status;
            txtIncF.Enabled = Status;
            txtAmtF.Enabled = Status;
            txtTotInc.Enabled = Status;
            txtHsRntAmt.Enabled = Status;
            txtEduAmt.Enabled = Status;
            txtMedAmt.Enabled = Status;
            txtLnInsAmt.Enabled = Status;
            txtExA.Enabled = Status;
            txtExAmtA.Enabled = Status;
            txtExB.Enabled = Status;
            txtExAmtB.Enabled = Status;
            txtTotExp.Enabled = Status;
            txtSurplus.Enabled = Status;
            txtFdAmt.Enabled = Status;
            txtGuarName.Enabled = Status;
            ddlGuarRel.Enabled = Status;
            txtGuarDOB.Enabled = Status;
            txtGuarAge.Enabled = Status;
            ddlGuarGen.Enabled = Status;
            txtSavingsAcNo.Enabled = Status;
            txtCustId.Enabled = Status;
            txtMaidenNmF.Enabled = Status;
            txtMaidenNmM.Enabled = Status;
            txtMaidenNmL.Enabled = Status;
            txtIdntPrfNo.Enabled = Status;
            txtAddPrfNo.Enabled = Status;
            txtMetDay.Enabled = Status;
            txtReAccNo.Enabled = Status;
            txtCollCenterDistance.Enabled = Status;
            txtBranchDistance.Enabled = Status;
            ddlIdProof3.Enabled = Status;
            txtIdProof3.Enabled = Status;
            ddlAddrType.Enabled = Status;
            txtArea.Enabled = Status;
            txtLandMark.Enabled = Status;
            txtEmail.Enabled = Status;

            ddlCommAddrType.Enabled = Status;
            txtCommHouseNo.Enabled = Status;
            txtCommLandmark.Enabled = Status;
            txtCommSt.Enabled = Status;
            txtCommArea.Enabled = Status;
            ddlCommVill.Enabled = Status;
            txtCommSubDist.Enabled = Status;
            txtCommPost.Enabled = Status;
            txtCommMob.Enabled = Status;
            txtCommPin.Enabled = Status;
            txtCommPhone.Enabled = Status;
            ddlCommDist.Enabled = Status;
            ddlCommState.Enabled = Status;
            txtCommEmail.Enabled = Status;
            txtStayYear.Enabled = Status;
            txtMemCommAddr.Enabled = Status;
            ddlAccType.Enabled = Status;
            txtBLandmark.Enabled = Status;
            txtBArea.Enabled = Status;
            txtBEmail.Enabled = Status;
            txtBIdntPrfNo.Enabled = Status;
            txtBAddPrfNo.Enabled = Status;
            txtGuarLName.Enabled = Status;
            txtNoOfHouseMember.Enabled = Status;
            txtNoOfChild.Enabled = Status;

            txtFamilyInc.Enabled = Status;
            txtSelfInc.Enabled = Status;
            txtOtherInc.Enabled = Status;

            txtFuelExp.Enabled = Status;
            txtElecExp.Enabled = Status;
            txtTransExp.Enabled = Status;
            chkCoAdd.Enabled = Status;
            chkCommAddr.Enabled = Status;
            ddlMinority.Enabled = Status;
            txtOtherExp.Enabled = Status;

            ddlAbledYN.Enabled = false;
            ddlSpclAbled.Enabled = false;
            if (ddlPvLine.SelectedIndex > 0)
            {
                if (ddlPvLine.SelectedValue == "BPL")
                    txtBPLNo.Enabled = Status;
                else
                    txtBPLNo.Enabled = false;
            }
            else
                txtBPLNo.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>

        private void ClearControls()
        {
            txtCoBrwrAddr.Text = "";
            txtMemAddr.Text = "";
            txtMemID.Text = "";
            cbDrp.Checked = false;
            txtDtCl.Text = "";
            ddlClTyp.SelectedIndex = -1;
            txtRemarks.Text = "";
            txtAdmDt.Text = Session[gblValue.LoginDate].ToString();
            txtFName.Text = "";
            txtMName.Text = "";
            txtLName.Text = "";
            txtHFName.Text = "";
            chkFath.Checked = false;
            txtDOB.Text = "";
            txtAge.Text = "0";
            txtHouNo.Text = "";
            txtStName.Text = "";
            txtWardNo.Text = "";
            txtPOff.Text = "";
            txtPin.Text = "";
            txtMob.Text = "";
            txtPhNo.Text = "";
            txtPin.Text = "";
            txtBankName.Text = "";
            txtBranch.Text = "";
            //txtAccNo.Text = "";
            txtAccNo.Attributes.Add("value", "");
            txtIFSC.Text = "";
            txtMemNamePBook.Text = "";
            txtBFName.Text = "";
            txtBMName.Text = "";
            txtBLName.Text = "";
            txtNoOfDpndnts.Text = "";
            txtBDOBDt.Text = "";
            txtBAge.Text = "0";
            txtBHouNo.Text = "";
            txtBStreet.Text = "";
            txtBWardNo.Text = "";
            txtBPOff.Text = "";
            txtBPin.Text = "";
            txtBMobNo.Text = "";
            txtBPhNo.Text = "";
            txtAddPrfNo.Text = "";
            txtIdntPrfNo.Text = "";
            txtBAddPrfNo.Text = "";
            txtBIdntPrfNo.Text = "";
            txtIncome.Text = "0";
            txtExpnses.Text = "0";
            txtReAccNo.Text = "";
            txtDist.Text = "";
            ddlMrySts.SelectedIndex = -1;
            ddlEduc.SelectedIndex = -1;
            ddlOcup.SelectedIndex = -1;
            ddlGend.SelectedIndex = -1;
            ddlRelg.SelectedIndex = -1;
            ddlCaste.SelectedIndex = -1;
            //ddlVillg.SelectedIndex = -1;
            ddlMuPanc.Items.Clear();
            ddlMuPanc.SelectedIndex = -1;
            ddlBlk.Items.Clear();
            ddlBlk.SelectedIndex = -1;
            //ddlDist.Items.Clear();
            //ddlDist.SelectedIndex = -1;
            ddlStat.Items.Clear();
            ddlStat.SelectedIndex = -1;
            //ddlAddPrf.SelectedIndex = -1;
            ddlIdentyProf.SelectedIndex = -1;
            ddlBRel.SelectedIndex = -1;
            ddlBEdu.SelectedIndex = -1;
            ddlBOcup.SelectedIndex = -1;
            ddlBGend.SelectedIndex = -1;
            ddlBRelg.SelectedIndex = -1;
            ddlBCaste.SelectedIndex = -1;
            ddlBVillg.SelectedIndex = -1;
            ddlBMunPanca.Items.Clear();
            ddlBMunPanca.SelectedIndex = -1;
            ddlBBlk.Items.Clear();
            ddlBBlk.SelectedIndex = -1;
            ddlBDist.Items.Clear();
            ddlBDist.SelectedIndex = -1;
            ddlBStat.Items.Clear();
            ddlBStat.SelectedIndex = -1;
            ddlBAddProf.SelectedIndex = -1;
            ddlBIdntyProf.SelectedIndex = -1;
            ddlRO.SelectedIndex = -1;
            ddlCenter.Items.Clear();
            ddlCenter.SelectedIndex = -1;
            ddlGroup.SelectedIndex = -1;
            chkCoAdd.Checked = false;
            ddlHHoldTyp.SelectedIndex = -1;
            txtPjMeetDt.Text = "";
            txtNetIncome.Text = "0.00";
            txtIncA.Text = "";
            txtAmtA.Text = "0.00";
            txtIncB.Text = "";
            txtAmtB.Text = "0.00";
            txtIncC.Text = "";
            txtAmtC.Text = "0.00";
            txtIncD.Text = "";
            txtAmtD.Text = "0.00";
            txtIncE.Text = "";
            txtAmtE.Text = "0.00";
            txtIncF.Text = "";
            txtAmtF.Text = "0.00";
            txtTotInc.Text = "0.00";
            txtHsRntAmt.Text = "0.00";
            txtEduAmt.Text = "0.00";
            txtMedAmt.Text = "0.00";
            txtFdAmt.Text = "0.00";
            txtLnInsAmt.Text = "0.00";
            txtExA.Text = "";
            txtExAmtA.Text = "0.00";
            txtExB.Text = "";
            txtExAmtB.Text = "0.00";
            txtTotExp.Text = "0.00";
            txtSurplus.Text = "0.00";
            txtGuarName.Text = "";
            ddlGuarRel.SelectedIndex = -1;
            txtGuarDOB.Text = "";
            txtGuarAge.Text = "0";
            ddlGuarGen.SelectedIndex = -1;
            txtCustId.Text = "";
            txtSavingsAcNo.Text = "";
            txtMetDay.Text = "";
            txtFamilyInc.Text = "0.00";
            txtSelfInc.Text = "0.00";
            txtOtherInc.Text = "0.00";
            txtFuelExp.Text = "0.00";
            txtElecExp.Text = "0.00";
            txtTransExp.Text = "0.00";
            txtOtherExp.Text = "0.00";
            txtBranchDistance.Text = "";
            txtCollCenterDistance.Text = "";
            ddlIdProof3.SelectedIndex = -1;
            txtIdProof3.Text = "";
            ddlAddrType.SelectedIndex = -1;
            txtLandMark.Text = "";
            txtArea.Text = "";
            txtEmail.Text = "";
            chkCommAddr.Checked = false;
            ddlCommAddrType.SelectedIndex = -1;
            txtCommLandmark.Text = "";
            txtCommHouseNo.Text = "";
            txtCommSt.Text = "";
            txtCommArea.Text = "";
            // ddlCommVill.SelectedIndex = -1;
            txtCommSubDist.Text = "";
            txtCommPost.Text = "";
            txtCommPin.Text = "";
            txtCommMob.Text = "";
            txtCommPhone.Text = "";
            ddlCommDist.SelectedIndex = -1;
            ddlCommState.SelectedIndex = -1;
            txtCommEmail.Text = "";
            txtStayYear.Text = "";
            ddlAreaCategory.SelectedIndex = 0;
            txtMemCommAddr.Text = "";
            ddlAccType.SelectedIndex = -1;
            txtBLandmark.Text = "";
            txtBArea.Text = "";
            txtBEmail.Text = "";
            txtNoOfHouseMember.Text = "";
            txtNoOfChild.Text = "";
            ///
            clearMemPhoto();
        }


        private void clearMemPhoto()
        {
            string imgUrl = "~/Images/no-image-icon.jpg";
            imgMemPhoto.ImageUrl = imgUrl;
            imgMemIdProof.ImageUrl = imgUrl;
            imgMemIdProofBack.ImageUrl = imgUrl;
            imgMemAddrProof.ImageUrl = imgUrl;
            imgMemAddrProofBack.ImageUrl = imgUrl;
            imgMemAddrProof2.ImageUrl = imgUrl;
            imgMemAddrProofBack2.ImageUrl = imgUrl;
            imgMemPassbook.ImageUrl = imgUrl;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtDOB_TextChanged(object sender, EventArgs e)
        {
            Int32 vAge = 0, vNoYr = 0;
            Int32 vCurrYear = System.DateTime.Now.Year;
            vAge = Convert.ToInt32(txtDOB.Text.Substring(6, 4));
            vNoYr = vCurrYear - vAge;
            if (txtDOB.Text.Length >= 10)
                txtAge.Text = vNoYr.ToString();
            //txtDOB.Focus();
        }
        protected void txtGuarDOB_TextChanged(object sender, EventArgs e)
        {
            Int32 vAge = 0, vNoYr = 0;
            Int32 vCurrYear = System.DateTime.Now.Year;
            vAge = Convert.ToInt32(txtGuarDOB.Text.Substring(6, 4));
            vNoYr = vCurrYear - vAge;
            if (txtGuarDOB.Text.Length >= 10)
                txtGuarAge.Text = vNoYr.ToString();
            //txtDOB.Focus();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbDrp_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDrp.Checked == true)
            {
                txtDtCl.Text = Session[gblValue.LoginDate].ToString();
                txtDtCl.Enabled = false;
                txtRemarks.Enabled = true;
            }
            if (cbDrp.Checked == false)
            {
                txtDtCl.Text = "";
                txtDtCl.Enabled = true;
                ddlClTyp.SelectedIndex = -1;
                txtRemarks.Text = "";
                txtRemarks.Enabled = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtAge_TextChanged(object sender, EventArgs e)
        {
            if (txtAge.Text != "")
            {
                Int32 vAge = Convert.ToInt32(txtAge.Text);
                DateTime vCurrDate = System.DateTime.Now.AddYears(-vAge);
                txtDOB.Text = gblFuction.putStrDate(vCurrDate);
                //txtAge.Focus();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtIncome_TextChanged(object sender, EventArgs e)
        {
            if (txtIncome.Text != "")
            {
                if (Convert.ToDouble(txtIncome.Text) <= 8333)
                    ddlHHoldTyp.SelectedIndex = ddlHHoldTyp.Items.IndexOf(ddlHHoldTyp.Items.FindByText("Rural"));
                else
                    ddlHHoldTyp.SelectedIndex = ddlHHoldTyp.Items.IndexOf(ddlHHoldTyp.Items.FindByText("Urban"));

                if (Convert.ToDouble(txtIncome.Text) <= 1500)
                    ddlPvLine.SelectedIndex = ddlPvLine.Items.IndexOf(ddlPvLine.Items.FindByText("BPL"));
                else
                    ddlPvLine.SelectedIndex = ddlPvLine.Items.IndexOf(ddlPvLine.Items.FindByText("APL"));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkCoAdd_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCoAdd.Checked == true)
            {
                txtBHouNo.Text = txtHouNo.Text;
                txtBStreet.Text = txtStName.Text;
                txtBWardNo.Text = txtWardNo.Text;
                txtBPOff.Text = txtPOff.Text;
                txtBPin.Text = txtPin.Text;
                //ddlBVillg.SelectedIndex = ddlVillg.SelectedIndex;
                if (ddlBVillg.SelectedIndex >= 0)
                {
                    PopAllAgainstVillage();
                    ddlBMunPanca.SelectedIndex = ddlMuPanc.SelectedIndex;
                    ddlBBlk.SelectedIndex = ddlBlk.SelectedIndex;
                    // ddlBDist.SelectedIndex = ddlDist.SelectedIndex;
                    ddlBStat.SelectedIndex = ddlStat.SelectedIndex;
                }
                else
                {
                }
                txtBHouNo.Enabled = false;
                txtBStreet.Enabled = false;
                txtBWardNo.Enabled = false;
                txtBPOff.Enabled = false;
                txtBPin.Enabled = false;
                ddlBVillg.Enabled = false;
            }
            else
            {
                txtBHouNo.Text = "";
                txtBStreet.Text = "";
                txtBWardNo.Text = "";
                txtBPOff.Text = "";
                txtBPin.Text = "";
                ddlBVillg.SelectedIndex = -1;
                ddlBMunPanca.SelectedIndex = -1;
                ddlBBlk.SelectedIndex = -1;
                ddlBDist.SelectedIndex = -1;
                ddlBStat.SelectedIndex = -1;
                txtBHouNo.Enabled = true;
                txtBStreet.Enabled = true;
                txtBWardNo.Enabled = true;
                txtBPOff.Enabled = true;
                txtBPin.Enabled = true;
                ddlBVillg.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkMProf_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMProf.Checked == true)
            {
                if (ddlIdentyProf.SelectedIndex > 0)
                {
                    if (Convert.ToInt32(ddlIdentyProf.SelectedValue) <= 5)
                    {
                        //ddlAddPrf.SelectedIndex = ddlIdentyProf.SelectedIndex;
                        txtAddPrfNo.Text = txtIdntPrfNo.Text;
                    }
                }
            }
            else
            {
                //     ddlAddPrf.SelectedIndex = -1;
                txtAddPrfNo.Text = "";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkBProf_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBProf.Checked == true)
            {
                if (ddlBIdntyProf.SelectedIndex > 0)
                {
                    if (Convert.ToInt32(ddlBIdntyProf.SelectedValue) <= 5)
                    {
                        ddlBAddProf.SelectedIndex = ddlBIdntyProf.SelectedIndex;
                        txtBAddPrfNo.Text = txtBIdntPrfNo.Text;
                    }
                }
            }
            else
            {
                ddlBAddProf.SelectedIndex = -1;
                txtBAddPrfNo.Text = "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBDOBDt_TextChanged(object sender, EventArgs e)
        {
            Int32 vAge = 0, vNoYr = 0;
            Int32 vCurrYear = System.DateTime.Now.Year;
            vAge = Convert.ToInt32(txtBDOBDt.Text.Substring(6, 4));
            vNoYr = vCurrYear - vAge;
            if (vNoYr < 18)
            {
                gblFuction.AjxMsgPopup("Co borrower age should Greater than 18");
                txtBAge.Text = "0";
                txtBDOBDt.Text = "";
            }
            else
            {
                if (txtBDOBDt.Text.Length >= 10)
                    txtBAge.Text = vNoYr.ToString();
            }
            //txtBDOBDt.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBAge_TextChanged(object sender, EventArgs e)
        {
            if (txtBAge.Text != "")
            {
                Int32 vAge = Convert.ToInt32(txtBAge.Text);
                if (vAge < 18)
                {
                    gblFuction.AjxMsgPopup("Co borrower age should Greater than 21");
                    txtAge.Text = "0";
                    txtBDOBDt.Text = "";
                }
                else
                {
                    DateTime vCurrDate = System.DateTime.Now.AddYears(-vAge);
                    txtBDOBDt.Text = gblFuction.putStrDate(vCurrDate);
                }
                //txtBAge.Focus();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGend_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMrySts.SelectedValue == "M01" && ddlGend.SelectedValue == "F")
            {
                ddlBGend.SelectedIndex = ddlBGend.Items.IndexOf(ddlBGend.Items.FindByText("Male"));
            }
            if (ddlMrySts.SelectedValue == "M01" && ddlGend.SelectedValue == "M")
            {
                ddlBGend.SelectedIndex = ddlBGend.Items.IndexOf(ddlBGend.Items.FindByText("Female"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlIdentyProf_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtIdntPrfNo.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBAddProf_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtBAddPrfNo.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBIdntyProf_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtBIdntPrfNo.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPvLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPvLine.SelectedIndex > 0)
            {
                if (ddlPvLine.SelectedValue == "BPL")
                    txtBPLNo.Enabled = true;
                else
                {
                    txtBPLNo.Text = "";
                    txtBPLNo.Enabled = false;
                }
            }
            else
            {
                txtBPLNo.Text = "";
                txtBPLNo.Enabled = false;
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {

            DataTable dt = null;
            Int32 vR = 0;
            DataRow dr;
            dt = (DataTable)ViewState["Fam"];

            if (dt.Rows.Count > 0)
            {
                vR = dt.Rows.Count - 1;
                TextBox txtFN = (TextBox)gvFamily.Rows[vR].FindControl("txtFamNm");

                dt.Rows[vR]["FName"] = txtFN.Text;

                DropDownList ddlGen = (DropDownList)gvFamily.Rows[vR].FindControl("ddlGndrFam");
                dt.Rows[vR]["Gender"] = ddlGen.SelectedValue;
                DropDownList ddlRl = (DropDownList)gvFamily.Rows[vR].FindControl("ddlReltFam");
                dt.Rows[vR]["RelationId"] = ddlRl.SelectedValue;
                DropDownList ddlSI = (DropDownList)gvFamily.Rows[vR].FindControl("ddlIncmFam");
                dt.Rows[vR]["OccupationId"] = ddlSI.SelectedValue;
                DropDownList ddlEdu = (DropDownList)gvFamily.Rows[vR].FindControl("ddlEduFam");
                dt.Rows[vR]["QualificationId"] = ddlEdu.SelectedValue;
                TextBox txtAg = (TextBox)gvFamily.Rows[vR].FindControl("txtFamAge");
                dt.Rows[vR]["Age"] = txtAg.Text == "" ? "0" : txtAg.Text;
                TextBox txtIncome = (TextBox)gvFamily.Rows[vR].FindControl("txtFamIncome");
                dt.Rows[vR]["Income"] = txtIncome.Text == "" ? "0" : txtIncome.Text;
            }
            dt.AcceptChanges();
            if (dt.Rows[vR]["FName"] != "")
            {
                dr = dt.NewRow();

                dt.Rows.Add(dr);
            }
            else
            {
                gblFuction.MsgPopup("Family member's name is Blank...");
            }

            ViewState["Fam"] = dt;
            gvFamily.DataSource = dt;
            gvFamily.DataBind();
        }

        protected void gvFamily_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null, dt1 = null, dt2 = null;
            CGblIdGenerator oGbl = null;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlGen = (DropDownList)e.Row.FindControl("ddlGndrFam");
                    ddlGen.SelectedIndex = ddlGen.Items.IndexOf(ddlGen.Items.FindByValue(e.Row.Cells[0].Text));

                    DropDownList ddlMrtlStatus = (DropDownList)e.Row.FindControl("ddlMrtlStatus");
                    ddlMrtlStatus.SelectedIndex = ddlMrtlStatus.Items.IndexOf(ddlMrtlStatus.Items.FindByText
                        (e.Row.Cells[4].Text));

                    DropDownList ddlRel = (DropDownList)e.Row.FindControl("ddlReltFam");
                    oGbl = new CGblIdGenerator();
                    dt = oGbl.PopComboMIS("N", "N", "AA", "HumanRelationId", "HumanRelationName", "HumanRelationMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                    ddlRel.DataSource = dt;
                    ddlRel.DataTextField = "HumanRelationName";
                    ddlRel.DataValueField = "HumanRelationId";
                    ddlRel.DataBind();
                    ListItem oL1 = new ListItem("<-- Select -->", "-1");
                    ddlRel.Items.Insert(0, oL1);
                    ddlRel.SelectedIndex = ddlRel.Items.IndexOf(ddlRel.Items.FindByValue(e.Row.Cells[1].Text));

                    DropDownList ddlQly = (DropDownList)e.Row.FindControl("ddlEduFam");
                    dt1 = oGbl.PopComboMIS("N", "N", "AA", "QualificationId", "QualificationName", "QualificationMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                    ddlQly.DataSource = dt1;
                    ddlQly.DataTextField = "QualificationName";
                    ddlQly.DataValueField = "QualificationId";
                    ddlQly.DataBind();
                    ListItem oL2 = new ListItem("<-- Select -->", "-1");
                    ddlQly.Items.Insert(0, oL2);
                    ddlQly.SelectedIndex = ddlQly.Items.IndexOf(ddlQly.Items.FindByValue(e.Row.Cells[3].Text));

                    DropDownList ddlSI = (DropDownList)e.Row.FindControl("ddlIncmFam");
                    dt2 = oGbl.PopComboMIS("N", "N", "AA", "OccupationId", "OccupationName", "OccupationMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                    ddlSI.DataSource = dt2;
                    ddlSI.DataTextField = "OccupationName";
                    ddlSI.DataValueField = "OccupationId";
                    ddlSI.DataBind();
                    ListItem oL3 = new ListItem("<-- Select -->", "-1");
                    ddlSI.Items.Insert(0, oL3);
                    ddlSI.SelectedIndex = ddlSI.Items.IndexOf(ddlSI.Items.FindByValue(e.Row.Cells[2].Text));



                    DropDownList ddOlSI = (DropDownList)e.Row.FindControl("ddlOthIncmFam");
                    dt2 = oGbl.PopComboMIS("N", "N", "AA", "OccupationId", "OccupationName", "OccupationMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                    ddOlSI.DataSource = dt2;
                    ddOlSI.DataTextField = "OccupationName";
                    ddOlSI.DataValueField = "OccupationId";
                    ddOlSI.DataBind();
                    ListItem oL4 = new ListItem("<-- Select -->", "-1");
                    ddOlSI.Items.Insert(0, oL3);
                    ddOlSI.SelectedIndex = ddlSI.Items.IndexOf(ddlSI.Items.FindByValue(e.Row.Cells[2].Text));

                    TextBox txtIncome = (TextBox)e.Row.FindControl("txtFamIncome");
                    TextBox txtAg = (TextBox)e.Row.FindControl("txtFamAge");
                    //        if (txtAg != null)
                    //        {
                    //            txtAg.TextChanged += new EventHandler(txtFamAge_TextChanged);
                    //        }
                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                dt2 = null;
                oGbl = null;
            }
        }

        protected void gvFamily_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDel")
            {
                DataTable dt = null;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                dt = (DataTable)ViewState["Fam"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    ViewState["Fam"] = dt;
                    gvFamily.DataSource = dt;
                    gvFamily.DataBind();
                }
                else
                {
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
        }

        private void GenerateGrid()
        {
            DataSet ds = null;
            DataTable dt = null, dt1 = null, dt2 = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                ds = oMem.GenerateGrid();
                dt = ds.Tables[0];
                DataRow dF;
                dF = dt.NewRow();
                dt.Rows.Add(dF);
                dt.AcceptChanges();
                ViewState["Fam"] = dt;
                gvFamily.DataSource = dt;
                gvFamily.DataBind();

                ViewState["Loan"] = dt1;

                ViewState["IncExp"] = dt2;



            }

            finally
            {
            }
        }

        public void GetData()
        {
            DataTable dt = (DataTable)ViewState["Fam"];
            foreach (GridViewRow gr in gvFamily.Rows)
            {
                TextBox txtFN = (TextBox)gvFamily.Rows[gr.RowIndex].FindControl("txtFamNm");
                DropDownList ddlGen = (DropDownList)gvFamily.Rows[gr.RowIndex].FindControl("ddlGndrFam");
                DropDownList ddlRl = (DropDownList)gvFamily.Rows[gr.RowIndex].FindControl("ddlReltFam");
                DropDownList ddlSI = (DropDownList)gvFamily.Rows[gr.RowIndex].FindControl("ddlIncmFam");
                DropDownList ddlEdu = (DropDownList)gvFamily.Rows[gr.RowIndex].FindControl("ddlEduFam");
                TextBox txtAg = (TextBox)gvFamily.Rows[gr.RowIndex].FindControl("txtFamAge");
                TextBox txtIncome = (TextBox)gvFamily.Rows[gr.RowIndex].FindControl("txtFamIncome");
                DropDownList ddlMrtlStatus = (DropDownList)gvFamily.Rows[gr.RowIndex].FindControl("ddlMrtlStatus");
                DropDownList ddlOthIncmFam = (DropDownList)gvFamily.Rows[gr.RowIndex].FindControl("ddlOthIncmFam");
                dt.Rows[gr.RowIndex]["FName"] = txtFN.Text;
                dt.Rows[gr.RowIndex]["Gender"] = ddlGen.SelectedValue;
                dt.Rows[gr.RowIndex]["RelationId"] = ddlRl.SelectedValue;
                dt.Rows[gr.RowIndex]["OccupationId"] = ddlSI.SelectedValue;
                dt.Rows[gr.RowIndex]["QualificationId"] = ddlEdu.SelectedValue;
                dt.Rows[gr.RowIndex]["Age"] = Convert.ToInt32(txtAg.Text == "" ? "0" : txtAg.Text);
                dt.Rows[gr.RowIndex]["Income"] = Convert.ToDouble(txtIncome.Text == "" ? "0" : txtIncome.Text);

                dt.Rows[gr.RowIndex]["MaritialStatus"] = ddlMrtlStatus.SelectedValue;
                dt.Rows[gr.RowIndex]["ExOccupationId"] = ddlOthIncmFam.SelectedValue;
            }
            dt.AcceptChanges();
            ViewState["Fam"] = dt;
            gvFamily.DataSource = dt;
            gvFamily.DataBind();
        }

        protected void gvAsset_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null, dt1 = null, dt2 = null;
            CGblIdGenerator oGbl = null;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {


                    DropDownList ddlAsset = (DropDownList)e.Row.FindControl("ddlAsset");
                    oGbl = new CGblIdGenerator();
                    dt = oGbl.PopComboMIS("N", "N", "AA", "AssetTypeId", "AssteName", "AssetypeMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                    ddlAsset.DataSource = dt;
                    ddlAsset.DataTextField = "AssteName";
                    ddlAsset.DataValueField = "AssetTypeId";
                    ddlAsset.DataBind();
                    ListItem oL1 = new ListItem("<-- Select -->", "-1");
                    ddlAsset.Items.Insert(0, oL1);
                    ddlAsset.SelectedIndex = ddlAsset.Items.IndexOf(ddlAsset.Items.FindByValue(e.Row.Cells[0].Text));


                    TextBox txtAstQty = (TextBox)e.Row.FindControl("txtAstQty");
                    TextBox txtAstAmt = (TextBox)e.Row.FindControl("txtAstAmt");
                    //        if (txtAg != null)
                    //        {
                    //            txtAg.TextChanged += new EventHandler(txtFamAge_TextChanged);
                    //        }
                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                dt2 = null;
                oGbl = null;
            }
        }

        public void GetData1()
        {
            DataTable dt = (DataTable)ViewState["Asset"];
            foreach (GridViewRow gr in gvAsset.Rows)
            {

                DropDownList ddlAsset = (DropDownList)gvAsset.Rows[gr.RowIndex].FindControl("ddlAsset");

                TextBox txtAstQty = (TextBox)gvAsset.Rows[gr.RowIndex].FindControl("txtAstQty");
                TextBox txtAstAmt = (TextBox)gvAsset.Rows[gr.RowIndex].FindControl("txtAstAmt");


                dt.Rows[gr.RowIndex]["AssetName"] = ddlAsset.SelectedValue;
                dt.Rows[gr.RowIndex]["AssetQty"] = Convert.ToInt32(txtAstQty.Text == "" ? "0" : txtAstQty.Text);
                dt.Rows[gr.RowIndex]["AssetAmt"] = Convert.ToDouble(txtAstAmt.Text == "" ? "0" : txtAstAmt.Text);


            }
            dt.AcceptChanges();
            ViewState["Asset"] = dt;
            gvAsset.DataSource = dt;
            gvAsset.DataBind();
        }
        private void GenerateGrid1()
        {
            DataSet ds = null;
            DataTable dt = null, dt1 = null, dt2 = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                ds = oMem.GenerateGrid1();
                dt = ds.Tables[0];
                //dt1 = ds.Tables[1];
                //dt2 = ds.Tables[2];
                DataRow dF;
                dF = dt.NewRow();
                dt.Rows.Add(dF);
                dt.AcceptChanges();
                ViewState["Asset"] = dt;
                gvAsset.DataSource = dt;
                gvAsset.DataBind();

            }

            finally
            {
            }
        }
        protected void gvAsset_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDel2")
            {
                DataTable dt = null;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                dt = (DataTable)ViewState["Asset"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    ViewState["Asset"] = dt;
                    gvAsset.DataSource = dt;
                    gvAsset.DataBind();
                }
                else
                {
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
        }
        protected void btnAddNew1_Click(object sender, EventArgs e)
        {

            DataTable dt = null;
            Int32 vR = 0;
            DataRow dr;
            dt = (DataTable)ViewState["Asset"];
            if (dt.Rows.Count > 0)
            {
                vR = dt.Rows.Count - 1;

                DropDownList ddlAsset = (DropDownList)gvAsset.Rows[vR].FindControl("ddlAsset");
                dt.Rows[vR]["AssetName"] = ddlAsset.SelectedValue;
                TextBox txtAstQty = (TextBox)gvAsset.Rows[vR].FindControl("txtAstQty");
                dt.Rows[vR]["AssetQty"] = txtAstQty.Text == "" ? "0" : txtAstQty.Text;
                TextBox txtAstAmt = (TextBox)gvAsset.Rows[vR].FindControl("txtAstAmt");
                dt.Rows[vR]["AssetAmt"] = txtAstAmt.Text == "" ? "0" : txtAstAmt.Text;
            }
            dt.AcceptChanges();

            if (dt.Rows[vR]["AssetName"].ToString() != "-1")
            {

                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            else
            {
                gblFuction.MsgPopup("Asset name is Blank...");
            }
            ViewState["Asset"] = dt;
            gvAsset.DataSource = dt;
            gvAsset.DataBind();
        }
        protected void txtNetIncome_Textchange(object sender, EventArgs e)
        {
            txtTotInc.Text = (Convert.ToDouble(txtNetIncome.Text) + Convert.ToDouble(txtAmtA.Text) + Convert.ToDouble(txtAmtB.Text) + Convert.ToDouble(txtAmtC.Text)
                + Convert.ToDouble(txtAmtD.Text) + Convert.ToDouble(txtAmtE.Text) + Convert.ToDouble(txtAmtF.Text)).ToString();

            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtAmtA_Textchange(object sender, EventArgs e)
        {
            txtTotInc.Text = (Convert.ToDouble(txtNetIncome.Text) + Convert.ToDouble(txtAmtA.Text) + Convert.ToDouble(txtAmtB.Text) + Convert.ToDouble(txtAmtC.Text)
                + Convert.ToDouble(txtAmtD.Text) + Convert.ToDouble(txtAmtE.Text) + Convert.ToDouble(txtAmtF.Text)).ToString();

            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtAmtB_Textchange(object sender, EventArgs e)
        {
            txtTotInc.Text = (Convert.ToDouble(txtNetIncome.Text) + Convert.ToDouble(txtAmtA.Text) + Convert.ToDouble(txtAmtB.Text) + Convert.ToDouble(txtAmtC.Text)
                + Convert.ToDouble(txtAmtD.Text) + Convert.ToDouble(txtAmtE.Text) + Convert.ToDouble(txtAmtF.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtAmtC_Textchange(object sender, EventArgs e)
        {
            txtTotInc.Text = (Convert.ToDouble(txtNetIncome.Text) + Convert.ToDouble(txtAmtA.Text) + Convert.ToDouble(txtAmtB.Text) + Convert.ToDouble(txtAmtC.Text)
                + Convert.ToDouble(txtAmtD.Text) + Convert.ToDouble(txtAmtE.Text) + Convert.ToDouble(txtAmtF.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtAmtD_Textchange(object sender, EventArgs e)
        {
            txtTotInc.Text = (Convert.ToDouble(txtNetIncome.Text) + Convert.ToDouble(txtAmtA.Text) + Convert.ToDouble(txtAmtB.Text) + Convert.ToDouble(txtAmtC.Text)
                + Convert.ToDouble(txtAmtD.Text) + Convert.ToDouble(txtAmtE.Text) + Convert.ToDouble(txtAmtF.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtAmtE_Textchange(object sender, EventArgs e)
        {
            txtTotInc.Text = (Convert.ToDouble(txtNetIncome.Text) + Convert.ToDouble(txtAmtA.Text) + Convert.ToDouble(txtAmtB.Text) + Convert.ToDouble(txtAmtC.Text)
                + Convert.ToDouble(txtAmtD.Text) + Convert.ToDouble(txtAmtE.Text) + Convert.ToDouble(txtAmtF.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtAmtF_Textchange(object sender, EventArgs e)
        {
            txtTotInc.Text = (Convert.ToDouble(txtNetIncome.Text) + Convert.ToDouble(txtAmtA.Text) + Convert.ToDouble(txtAmtB.Text) + Convert.ToDouble(txtAmtC.Text)
                + Convert.ToDouble(txtAmtD.Text) + Convert.ToDouble(txtAmtE.Text) + Convert.ToDouble(txtAmtF.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtHsRntAmt_Textchange(object sender, EventArgs e)
        {
            txtTotExp.Text = (Convert.ToDouble(txtHsRntAmt.Text) + Convert.ToDouble(txtMedAmt.Text) + Convert.ToDouble(txtEduAmt.Text) + Convert.ToDouble(txtLnInsAmt.Text)
                + Convert.ToDouble(txtExAmtA.Text) + Convert.ToDouble(txtExAmtB.Text) + Convert.ToDouble(txtFdAmt.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtEduAmt_Textchange(object sender, EventArgs e)
        {
            txtTotExp.Text = (Convert.ToDouble(txtHsRntAmt.Text) + Convert.ToDouble(txtMedAmt.Text) + Convert.ToDouble(txtEduAmt.Text) + Convert.ToDouble(txtLnInsAmt.Text)
                + Convert.ToDouble(txtExAmtA.Text) + Convert.ToDouble(txtExAmtB.Text) + Convert.ToDouble(txtFdAmt.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtMedAmt_Textchange(object sender, EventArgs e)
        {
            txtTotExp.Text = (Convert.ToDouble(txtHsRntAmt.Text) + Convert.ToDouble(txtMedAmt.Text) + Convert.ToDouble(txtEduAmt.Text) + Convert.ToDouble(txtLnInsAmt.Text)
                + Convert.ToDouble(txtExAmtA.Text) + Convert.ToDouble(txtExAmtB.Text) + Convert.ToDouble(txtFdAmt.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtLnInsAmt_Textchange(object sender, EventArgs e)
        {
            txtTotExp.Text = (Convert.ToDouble(txtHsRntAmt.Text) + Convert.ToDouble(txtMedAmt.Text) + Convert.ToDouble(txtEduAmt.Text) + Convert.ToDouble(txtLnInsAmt.Text)
                + Convert.ToDouble(txtExAmtA.Text) + Convert.ToDouble(txtExAmtB.Text) + Convert.ToDouble(txtFdAmt.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();
        }
        protected void txtExAmtA_Textchange(object sender, EventArgs e)
        {
            txtTotExp.Text = (Convert.ToDouble(txtHsRntAmt.Text) + Convert.ToDouble(txtMedAmt.Text) + Convert.ToDouble(txtEduAmt.Text) + Convert.ToDouble(txtLnInsAmt.Text)
                + Convert.ToDouble(txtExAmtA.Text) + Convert.ToDouble(txtExAmtB.Text) + Convert.ToDouble(txtFdAmt.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();

        }
        protected void txtExB_Textchange(object sender, EventArgs e)
        {
            txtTotExp.Text = (Convert.ToDouble(txtHsRntAmt.Text) + Convert.ToDouble(txtMedAmt.Text) + Convert.ToDouble(txtEduAmt.Text) + Convert.ToDouble(txtLnInsAmt.Text)
                + Convert.ToDouble(txtExAmtA.Text) + Convert.ToDouble(txtExAmtB.Text) + Convert.ToDouble(txtFdAmt.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();

        }
        protected void txtFdAmt_Textchange(object sender, EventArgs e)
        {
            txtTotExp.Text = (Convert.ToDouble(txtHsRntAmt.Text) + Convert.ToDouble(txtMedAmt.Text) + Convert.ToDouble(txtEduAmt.Text) + Convert.ToDouble(txtLnInsAmt.Text)
                + Convert.ToDouble(txtExAmtA.Text) + Convert.ToDouble(txtExAmtB.Text) + Convert.ToDouble(txtFdAmt.Text)).ToString();
            txtSurplus.Text = (Convert.ToDouble(txtTotInc.Text) - Convert.ToDouble(txtTotExp.Text)).ToString();

        }

        private void GenerateReport(string pApiName, string pRequestdata)
        {
            string vMsg = "";
            CApiCalling oAPI = new CApiCalling();
            try
            {
                vMsg = oAPI.GenerateReport(pApiName, pRequestdata, "https://unityimage.bijliftt.com/ImageDownloadService.svc");
            }
            finally
            {
            }
        }

    }
}