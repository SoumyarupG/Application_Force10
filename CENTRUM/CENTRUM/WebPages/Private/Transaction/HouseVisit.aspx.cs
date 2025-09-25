using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Configuration;
using System.Net;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Xml;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class HouseVisit : CENTRUMBase
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
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //    StatusButton("Exit");
                //else
                StatusButton("View");
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                LoadGrid(1);
                popRO();
                popHvBy();
                tbHVisit.ActiveTabIndex = 0;
                ddlQ14SubCat.Visible = false;
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
                this.PageHeading = "House Visit";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHVisit);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 3) // role id 3 is for RO
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "GRT", false);
                else
                {
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
                    }
                    else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                    {
                        btnDelete.Visible = false;
                    }
                    else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                    {
                    }
                    else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    {
                        Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "House Visit", false);
                    }
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
                    btnDelete.Enabled = true;
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
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
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
                ddlRo.DataSource = dt;
                ddlRo.DataTextField = "EoName";
                ddlRo.DataValueField = "EoId";
                ddlRo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRo.Items.Insert(0, oli);

            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        private void popHvBy()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("N", "EoMst", "ABM,BM,AM,SBM,AAM,ARM,RM,RO,CO", vLogDt, vBrCode);
                ddlVisitBy.DataSource = dt;
                ddlVisitBy.DataTextField = "EoName";
                ddlVisitBy.DataValueField = "EoId";
                ddlVisitBy.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlVisitBy.Items.Insert(0, oli);
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
        /// <param name="pEoId"></param>
        private void PopCenter(string pEoId)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("S", "N", "AA", "MarketID", "Market", "MarketMSt", pEoId, "EoId", "AA", gblFuction.setDate("01/01/1900"), "0000");
                if (dt.Rows.Count > 0)
                {
                    ddlCentr.DataSource = dt;
                    ddlCentr.DataTextField = "Market";
                    ddlCentr.DataValueField = "MarketID";
                    ddlCentr.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlCentr.Items.Insert(0, oli);
                }

            }
            finally
            { }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popGroup(string vCenter)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "Groupid", "GroupName", "GroupMSt", vCenter, "MarketID", "Tra_DropDate", vLogDt, vBrCode);
                ddlGrup.DataSource = dt;
                ddlGrup.DataTextField = "GroupName";
                ddlGrup.DataValueField = "Groupid";
                ddlGrup.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlGrup.Items.Insert(0, oli);
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
        /// <param name="vGrpId"></param>
        private void PopMember(string vGrpId, string vMode)
        {
            DataTable dt = null;
            string vBrCode = "";
            CHouseVisit oHv = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oHv = new CHouseVisit();
                dt = oHv.GetHVMember(vBrCode, vGrpId, vLogDt, vMode);
                if (dt.Rows.Count > 0)
                {
                    ViewState["CGTID"] = dt;

                    ddlMem.DataSource = dt;
                    ddlMem.DataTextField = "Member";
                    ddlMem.DataValueField = "MemberID";
                    ddlMem.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlMem.Items.Insert(0, oli);
                }
                else
                {
                    ddlMem.Items.Clear();
                    ListItem oli = new ListItem("No More CGT Done", "-1");
                    ddlMem.Items.Insert(0, oli);
                }

            }
            finally
            {
                oHv = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRo_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopCenter(ddlRo.SelectedValue);
            //ddlCentr.SelectedIndex = ddlCentr.Items.IndexOf(ddlCentr.Items.FindByValue(ddlRo.SelectedValue));
            //popGroup(ddlCentr.SelectedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCentr_SelectedIndexChanged(object sender, EventArgs e)
        {
            popGroup(ddlCentr.SelectedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGrup_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopMember(ddlGrup.SelectedValue, "A");
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
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = "Add";
                tbHVisit.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
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
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid(1);
                    StatusButton("Delete");
                    tbHVisit.ActiveTabIndex = 0;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            tbHVisit.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        protected void btnShwPotenMem_Click(object sender, EventArgs e)
        {
            string vUrl = hdnProtenUrl.Value;
            string url = vUrl + "BIJLI";
            string s = "window.open('" + url + "', '_blank', 'width=900,height=600,left=100,top=100,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            return;
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
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(1);
                StatusButton("Show");
                ViewState["StateEdit"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0; ;
            string vBrCode = "";
            CHouseVisit oHv = null;
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oHv = new CHouseVisit();
                dt = oHv.GetHVMasterPG(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), vBrCode, pPgIndx, ref vTotRows);
                gvHVisit.DataSource = dt;
                gvHVisit.DataBind();
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
                oHv = null;
                dt.Dispose();
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
            LoadGrid(vPgNo);
            tbHVisit.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvHVisit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int vHVId = 0;
            DataTable dt = null;
            CHouseVisit oHv = null;
            try
            {
                vHVId = Convert.ToInt32(e.CommandArgument);
                ViewState["CGTBYID"] = vHVId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvHVisit.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oHv = new CHouseVisit();
                    dt = oHv.GetHouseVisitById(vHVId);
                    if (dt.Rows.Count > 0)
                    {
                        ddlRo.SelectedIndex = ddlRo.Items.IndexOf(ddlRo.Items.FindByValue(dt.Rows[0]["EOID"].ToString()));
                        PopCenter(ddlRo.SelectedValue);
                        ddlCentr.SelectedIndex = ddlCentr.Items.IndexOf(ddlCentr.Items.FindByValue(dt.Rows[0]["MarketID"].ToString()));
                        popGroup(ddlCentr.SelectedValue);
                        ddlGrup.SelectedIndex = ddlGrup.Items.IndexOf(ddlGrup.Items.FindByValue(dt.Rows[0]["Groupid"].ToString()));
                        PopMember(ddlGrup.SelectedValue, "E");
                        ddlMem.SelectedIndex = ddlMem.Items.IndexOf(ddlMem.Items.FindByValue(dt.Rows[0]["MemberID"].ToString()));
                        ddlVisitBy.SelectedIndex = ddlVisitBy.Items.IndexOf(ddlVisitBy.Items.FindByValue(dt.Rows[0]["GRTBy"].ToString()));
                        txtVisitDt.Text = Convert.ToString(dt.Rows[0]["HVDate"]);
                        txtExGDt.Text = Convert.ToString(dt.Rows[0]["EXPGRTdt"]);
                        ddlQ1.SelectedIndex = ddlQ1.Items.IndexOf(ddlQ1.Items.FindByValue(dt.Rows[0]["Q1A"].ToString()));
                        ddlQ2.SelectedIndex = ddlQ2.Items.IndexOf(ddlQ2.Items.FindByValue(dt.Rows[0]["Q2A"].ToString()));
                        ddlQ3.SelectedIndex = ddlQ3.Items.IndexOf(ddlQ3.Items.FindByValue(dt.Rows[0]["Q3A"].ToString()));
                        ddlQ4.SelectedIndex = ddlQ4.Items.IndexOf(ddlQ4.Items.FindByValue(dt.Rows[0]["Q4A"].ToString()));
                        ddlQ5.SelectedIndex = ddlQ5.Items.IndexOf(ddlQ5.Items.FindByValue(dt.Rows[0]["Q5A"].ToString()));
                        ddlQ6.SelectedIndex = ddlQ6.Items.IndexOf(ddlQ6.Items.FindByValue(dt.Rows[0]["Q6A"].ToString()));
                        ddlQ7.SelectedIndex = ddlQ7.Items.IndexOf(ddlQ7.Items.FindByValue(dt.Rows[0]["Q7A"].ToString()));
                        ddlQ8.SelectedIndex = ddlQ8.Items.IndexOf(ddlQ8.Items.FindByValue(dt.Rows[0]["Q8A"].ToString()));
                        ddlQ9.SelectedIndex = ddlQ9.Items.IndexOf(ddlQ9.Items.FindByValue(dt.Rows[0]["Q9A"].ToString()));
                        ddlQ10.SelectedIndex = ddlQ10.Items.IndexOf(ddlQ10.Items.FindByValue(dt.Rows[0]["Q10A"].ToString()));
                        ddlQ11.SelectedIndex = ddlQ11.Items.IndexOf(ddlQ11.Items.FindByValue(dt.Rows[0]["Q11A"].ToString()));
                        ddlQ12.SelectedIndex = ddlQ12.Items.IndexOf(ddlQ12.Items.FindByValue(dt.Rows[0]["Q12A"].ToString()));
                        ddlQ13.SelectedIndex = ddlQ13.Items.IndexOf(ddlQ13.Items.FindByValue(dt.Rows[0]["Q13A"].ToString()));
                        ddlQ14.SelectedIndex = ddlQ14.Items.IndexOf(ddlQ14.Items.FindByValue(dt.Rows[0]["Q14A"].ToString()));
                        if (dt.Rows[0]["Q14A"].ToString() == "6")
                        {
                            ddlQ14SubCat.Visible = true;
                            ddlQ14SubCat.SelectedIndex = ddlQ14SubCat.Items.IndexOf(ddlQ14SubCat.Items.FindByValue(dt.Rows[0]["Q14SubCat"].ToString()));
                        }
                        else
                            ddlQ14SubCat.Visible = false;
                        //if (dt.Rows[0]["GRTYN"].ToString() == "Y")
                        //    chkGrt.Checked = true;
                        //else
                        //    chkGrt.Checked = false;

                        //List<string> vli = Convert.ToString(dt.Rows[0]["Q15A"]).Split(',').ToList<string>();
                        //foreach (string vItem in vli)
                        //{
                        //    for (int iLoop = 0; iLoop < chkQ15.Items.Count; iLoop++)
                        //    {
                        //        if (chkQ15.Items[iLoop].Value.ToString() == vItem)
                        //        {
                        //            chkQ15.Items[iLoop].Selected = true;
                        //        }
                        //    }
                        //}
                        //vli.Clear();
                        //vli = Convert.ToString(dt.Rows[0]["Q16A"]).Split(',').ToList<string>();
                        //foreach (string vItem in vli)
                        //{
                        //    for (int iLoop = 0; iLoop < chkQ16.Items.Count; iLoop++)
                        //    {
                        //        if (chkQ16.Items[iLoop].Value.ToString() == vItem)
                        //        {
                        //            chkQ16.Items[iLoop].Selected = true;
                        //        }
                        //    }
                        //}

                        chkQ15.ClearSelection();
                        chkQ16.ClearSelection();
                        if (Convert.ToString(dt.Rows[0]["Q15Electric"]) == "Y") chkQ15.Items[0].Selected = true;
                        if (Convert.ToString(dt.Rows[0]["Q15Water"]) == "Y") chkQ15.Items[1].Selected = true;
                        if (Convert.ToString(dt.Rows[0]["Q15Toilet"]) == "Y") chkQ15.Items[2].Selected = true;
                        if (Convert.ToString(dt.Rows[0]["Q15Sewage"]) == "Y") chkQ15.Items[3].Selected = true;
                        if (Convert.ToString(dt.Rows[0]["Q15LPG"]) == "Y") chkQ15.Items[4].Selected = true;
                        if (Convert.ToString(dt.Rows[0]["Q16Land"]) == "Y") chkQ16.Items[0].Selected = true;
                        if (Convert.ToString(dt.Rows[0]["Q16Vehicle"]) == "Y") chkQ16.Items[1].Selected = true;
                        if (Convert.ToString(dt.Rows[0]["Q16Furniture"]) == "Y") chkQ16.Items[2].Selected = true;
                        if (Convert.ToString(dt.Rows[0]["Q16SmartPhone"]) == "Y") chkQ16.Items[3].Selected = true;
                        if (Convert.ToString(dt.Rows[0]["Q16ElectricItem"]) == "Y") chkQ16.Items[4].Selected = true;

                        bool vViewPotenMem = false;
                        CRole oRl = new CRole();
                        DataTable dt1 = new DataTable();
                        dt1 = oRl.GetRoleById(Convert.ToInt32(Session[gblValue.RoleId].ToString()));
                        if (dt1.Rows.Count > 0)
                        {
                            vViewPotenMem = Convert.ToString(dt.Rows[0]["ShowPotential"]) == "Y" && Convert.ToString(dt1.Rows[0]["PotenMemYN"]) == "Y" ? true : false;
                        }
                        btnShwPotenMem.Visible = vViewPotenMem;
                        btnUpdateUcic.Visible = vViewPotenMem;
                        hdnProtenUrl.Value = (Convert.ToString(dt.Rows[0]["PotenURL"]));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbHVisit.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oHv = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool ValidateFields()
        {
            bool vRes = false;
            //string pathImage = ConfigurationManager.AppSettings["PathImage"];
            //string vUrl = pathImage + "Member/";
            //WebRequest request = WebRequest.Create(vUrl + ddlMem.SelectedValue + "/PassbookImage.png");
            //try
            //{
            //    request.GetResponse();
            //     vRes = true;
            //}
            //catch (Exception ex)
            //{
            //    gblFuction.AjxMsgPopup("Please Upload Passbook Image..!");
            //    vRes = false;
            //}
            string vUrl = ConfigurationManager.AppSettings["pathMember"];
            bool fileExists = (File.Exists(vUrl + ddlMem.SelectedValue + "/PassbookImage.png") ? true : false);
            if (fileExists == true)
            {
                vRes = true;
            }
            else
            {
                gblFuction.AjxMsgPopup("Please Upload Passbook Image..!");
                vRes = false;
            }

            //if (gblFuction.IsEmail(txtEmail.Text.Trim()) == false)
            //{
            //    gblFuction.MsgPopup("Please Enter a Valid Email");
            //    return vRes = false;
            //}
            //if (gblFuction.IsDate(txtOpDt.Text.Trim()) == false)
            //{
            //    gblFuction.MsgPopup("Opening Date is not in DD/MM/YYYY");
            //    return vRes = false;
            //}
            //if (gblFuction.IsDate(txtAgDt.Text.Trim()) == false)
            //{
            //    gblFuction.MsgPopup("Agre Date is not in DD/MM/YYYY");
            //    return vRes = false;
            //}
            //if (gblFuction.IsDate(txtToDt.Text.Trim()) == false)
            //{
            //    gblFuction.MsgPopup("Valid Till is not in DD/MM/YYYY");
            //    return vRes = false;
            //}
            //if (gblFuction.setDate(txtAgDt.Text.Trim()) >= gblFuction.setDate(txtToDt.Text.Trim()))
            //{
            //    gblFuction.MsgPopup("Valid Till Cannot be less than Agre Date");
            //    return vRes = false;
            //}

            return vRes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            if (ValidateFields() == false)
                return false;
            int[] Q2 = new int[] { 0, 0, 3, 5, 7 };
            string vBrCode = Session[gblValue.BrnchCode].ToString();

            Int32 vCGTID = 0;
            DataTable dt = (DataTable)ViewState["CGTID"];

            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vNewId = Convert.ToString(ViewState["HVId"]);
            Int32 vErr = 0, vQ1 = 0, vQ2 = 0, vQ3 = 0, vQ4 = 0, vQ5 = 0, vQ6 = 0, vQ7 = 0, vQ8 = 0, vQ9 = 0, vQ10 = 0, vQ11 = 0, vQ12 = 0, vQ13 = 0, vQ14 = 0, vQ14SubCat = 0;
            string vQ15 = string.Empty, vQ16 = string.Empty;
            string vQ15ElectricYN = "N", vQ15WaterYN = "N", vQ15ToiletYN = "N", vQ15SewageYN = "N", vQ15LPGYN = "N";
            string vQ16LandYN = "N", vQ16VehicleYN = "N", vQ16FurnitureYN = "N", vQ16SmartPhoneYN = "N", vQ16ElectricItemYN = "N";
            CHouseVisit oHv = null;
            CGblIdGenerator oGbl = null;
            try
            {
                //if (chkGrt.Checked == true)
                //    vGrtYn = "Y";
                vQ1 = Convert.ToInt32(ddlQ1.SelectedValue);
                vQ2 = Convert.ToInt32(ddlQ2.SelectedValue);
                vQ3 = Convert.ToInt32(ddlQ3.SelectedValue);
                vQ4 = Convert.ToInt32(ddlQ4.SelectedValue);
                vQ5 = Convert.ToInt32(ddlQ5.SelectedValue);
                vQ6 = Convert.ToInt32(ddlQ6.SelectedValue);
                vQ7 = Convert.ToInt32(ddlQ7.SelectedValue);
                vQ8 = Convert.ToInt32(ddlQ8.SelectedValue);
                vQ9 = Convert.ToInt32(ddlQ9.SelectedValue);
                vQ10 = Convert.ToInt32(ddlQ10.SelectedValue);
                vQ11 = Convert.ToInt32(ddlQ11.SelectedValue);
                vQ12 = Convert.ToInt32(ddlQ12.SelectedValue);
                vQ13 = Convert.ToInt32(ddlQ13.SelectedValue);
                vQ14 = Convert.ToInt32(ddlQ14.SelectedValue);
                vQ14SubCat = Convert.ToInt32(ddlQ14SubCat.SelectedValue);

                DataRow[] result = dt.Select("MemberID = " + ddlMem.SelectedValue);

                foreach (ListItem vItem in chkQ15.Items)
                {
                    //if (vQ15 == "") vQ15 = vItem.Value;
                    //if (vItem.Selected == true) vQ15 = vQ15 + "," + vItem.Value;
                    if (vItem.Selected == true)
                    {
                        if (vQ15 == "") vQ15 = vItem.Value;
                        else vQ15 = vQ15 + "," + vItem.Value;
                    }
                }
                foreach (ListItem vItem in chkQ16.Items)
                {
                    //if (vQ16 == "") vQ16 = vItem.Value;
                    //if (vItem.Selected == true) vQ16 = vQ16 + "," + vItem.Value;
                    if (vItem.Selected == true)
                    {
                        if (vQ16 == "") vQ16 = vItem.Value;
                        else vQ16 = vQ16 + "," + vItem.Value;
                    }
                }

                if (chkQ15.Items[0].Selected == true) vQ15ElectricYN = "Y";
                if (chkQ15.Items[1].Selected == true) vQ15WaterYN = "Y";
                if (chkQ15.Items[2].Selected == true) vQ15ToiletYN = "Y";
                if (chkQ15.Items[3].Selected == true) vQ15SewageYN = "Y";
                if (chkQ15.Items[4].Selected == true) vQ15LPGYN = "Y";
                if (chkQ16.Items[0].Selected == true) vQ16LandYN = "Y";
                if (chkQ16.Items[1].Selected == true) vQ16VehicleYN = "Y";
                if (chkQ16.Items[2].Selected == true) vQ16FurnitureYN = "Y";
                if (chkQ16.Items[3].Selected == true) vQ16SmartPhoneYN = "Y";
                if (chkQ16.Items[4].Selected == true) vQ16ElectricItemYN = "Y";

                if (Mode == "Save")
                {
                    vCGTID = Convert.ToInt32(result[0]["CGTId"].ToString());
                    oHv = new CHouseVisit();
                    oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("BranchMst", "BranchCode", txtBrCode.Text.Replace("'", "''"), "", "", "", vDisId, "Save");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("Branch Code Can not be Duplicate...");
                    //    return false;
                    //}
                    vErr = oHv.SaveHouseStatus(vCGTID, ddlMem.SelectedValue, gblFuction.setDate(txtVisitDt.Text), ddlVisitBy.SelectedValue,
                                gblFuction.setDate(txtExGDt.Text), vQ1, vQ1, vQ2, Q2[vQ2 - 1], vQ3, vQ3, vQ4, vQ4, vQ5, vQ5, vQ6, vQ6, vQ7, vQ7, vQ8, vQ8, vQ9,
                                vQ9, vQ10, vQ10, vQ11, vQ11, vQ12, vQ12, vQ13, vQ13, vQ14, vQ14, vBrCode, this.UserID, "Save",
                                vQ15, vQ15, vQ16, vQ16,
                                vQ15ElectricYN, vQ15WaterYN, vQ15ToiletYN, vQ15SewageYN, vQ15LPGYN,
                                vQ16LandYN, vQ16VehicleYN, vQ16FurnitureYN, vQ16SmartPhoneYN, vQ16ElectricItemYN, vQ14SubCat);
                    if (vErr > 0)
                    {
                        ViewState["HVId"] = vNewId;
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
                    vCGTID = Convert.ToInt32(ViewState["CGTBYID"]);
                    oHv = new CHouseVisit();
                    vErr = oHv.SaveHouseStatus(vCGTID, ddlMem.SelectedValue, gblFuction.setDate(txtVisitDt.Text), ddlVisitBy.SelectedValue,
                        gblFuction.setDate(txtExGDt.Text), vQ1, vQ1, vQ2, Q2[vQ2], vQ3, vQ3, vQ4, vQ4, vQ5, vQ5, vQ6, vQ6, vQ7, vQ7, vQ8, vQ8,
                        vQ9, vQ9, vQ10, vQ10, vQ11, vQ11, vQ12, vQ12, vQ13, vQ13, vQ14, vQ14, vBrCode, this.UserID, "Edit",
                        vQ15, vQ15, vQ16, vQ16,
                        vQ15ElectricYN, vQ15WaterYN, vQ15ToiletYN, vQ15SewageYN, vQ15LPGYN,
                        vQ16LandYN, vQ16VehicleYN, vQ16FurnitureYN, vQ16SmartPhoneYN, vQ16ElectricItemYN, vQ14SubCat);
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
                    vCGTID = Convert.ToInt32(ViewState["CGTBYID"]);
                    oHv = new CHouseVisit();

                    gblFuction.MsgPopup("Delete Not Possible");
                    vResult = false;
                    //vErr = oHv.SaveHouseStatus(vCGTID, ddlMem.SelectedValue, gblFuction.setDate(txtVisitDt.Text), ddlVisitBy.SelectedValue,
                    //    gblFuction.setDate(txtExGDt.Text), vQ1, vQ1, vQ2, Q2[vQ2], vQ3, vQ3, vQ4, vQ4, vQ5, vQ5, vQ6, vQ6, vQ7, vQ7, vQ8, vQ8, 
                    //    vQ9, vQ9, vQ10, vQ10, vQ11, vQ11, vQ12, vQ12, vQ13, vQ13, vQ14, vQ14, vBrCode, this.UserID, "Del",
                    //    vQ15, vQ15, vQ16, vQ16);
                    //if (vErr > 0)
                    //{
                    //    gblFuction.MsgPopup(gblMarg.DeleteMsg);
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
                oHv = null;
                //oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            ddlRo.Enabled = Status;
            ddlCentr.Enabled = Status;
            ddlGrup.Enabled = Status;
            ddlMem.Enabled = Status;
            ddlVisitBy.Enabled = Status;
            txtVisitDt.Enabled = Status;
            ddlQ1.Enabled = Status;
            ddlQ2.Enabled = Status;
            ddlQ3.Enabled = Status;
            ddlQ4.Enabled = Status;
            ddlQ5.Enabled = Status;
            ddlQ6.Enabled = Status;
            ddlQ7.Enabled = Status;
            ddlQ8.Enabled = Status;
            ddlQ9.Enabled = Status;
            ddlQ10.Enabled = Status;
            ddlQ11.Enabled = Status;
            ddlQ12.Enabled = Status;
            txtExGDt.Enabled = Status;
            ddlQ13.Enabled = Status;
            ddlQ14.Enabled = Status;
            chkQ15.Enabled = Status;
            chkQ16.Enabled = Status;
            ddlQ14SubCat.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlRo.SelectedIndex = -1;
            ddlCentr.SelectedIndex = -1;
            ddlGrup.SelectedIndex = -1;
            ddlMem.SelectedIndex = -1;
            ddlVisitBy.SelectedIndex = -1;
            txtVisitDt.Text = Session[gblValue.LoginDate].ToString();
            //txtLnCycl.Text="0";
            ddlQ1.SelectedIndex = -1;
            ddlQ2.SelectedIndex = -1;
            ddlQ3.SelectedIndex = -1;
            ddlQ4.SelectedIndex = -1;
            ddlQ5.SelectedIndex = -1;
            ddlQ6.SelectedIndex = -1;
            ddlQ7.SelectedIndex = -1;
            ddlQ8.SelectedIndex = -1;
            ddlQ9.SelectedIndex = -1;
            ddlQ10.SelectedIndex = -1;
            ddlQ11.SelectedIndex = -1;
            ddlQ12.SelectedIndex = -1;
            txtExGDt.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
            ddlQ14.SelectedIndex = -1;
            chkQ15.ClearSelection();
            chkQ16.ClearSelection();
            ddlQ14SubCat.SelectedIndex = -1;
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        protected void ddlQ14_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ14.SelectedValue == "6")
            {
                ddlQ14SubCat.Visible = true;
            }
            else
            {
                ddlQ14SubCat.Visible = false;
            }

        }

        protected void btnUpdateUcic_Click(object sender, EventArgs e)
        {
            string pMemberId = ddlMem.SelectedValue;
            int pCgtId = Convert.ToInt32(ViewState["CGTBYID"]);
            string pUcicId = getUcic(pMemberId, Convert.ToInt32(Session[gblValue.UserId]), pCgtId);
            int pErr = -1;
            CHouseVisit hv = new CHouseVisit();
            if (pUcicId != "")
            {
                pErr = hv.UpdateUcicId(pUcicId, pMemberId, pCgtId);
                if (pErr == 0)
                {
                    gblFuction.MsgPopup(gblMarg.EditMsg);
                }
                else
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
                }
            }
            else
            {
                gblFuction.MsgPopup("Respose Error");
            }
        }

        public string getUcic(string pMemberId, int pCreatedBy, int pCgtId)
        {
            string vResponse = "", vUcic = "";
            CHouseVisit oHv = new CHouseVisit();
            try
            {
                string Requestdata = "{\"cust_id\" :" + "\"" + pMemberId + "\"" + ",\"source_system_name\":\"BIJLI\"}";
                //string postURL = "http://144.24.116.182:9002/UnitySfbWS/getUcic";
                string postURL = "https://ucic.unitybank.co.in:9002/UnitySfbWS/getUcic";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                dynamic res = JsonConvert.DeserializeObject(vResponse);
                if (res.ResponseCode == "200")
                {
                    vUcic = res.Ucic;
                }
                //----------------------------Save Log---------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oHv.SaveProsidexLogUCIC(pMemberId, pCgtId, vResponseXml, pCreatedBy, vUcic);
                //---------------------------------------------------------------------------------
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
                //----------------------------Save Log---------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oHv.SaveProsidexLogUCIC(pMemberId, pCgtId, vResponseXml, pCreatedBy, vUcic);
                //---------------------------------------------------------------------------------
            }
            return vUcic;
        }

        #region Common
        public string AsString(XmlDocument xmlDoc)
        {
            string strXmlText = null;
            if (xmlDoc != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter tx = new XmlTextWriter(sw))
                    {
                        xmlDoc.WriteTo(tx);
                        strXmlText = sw.ToString();
                    }
                }
            }
            return strXmlText;
        }
        #endregion
    }
}
