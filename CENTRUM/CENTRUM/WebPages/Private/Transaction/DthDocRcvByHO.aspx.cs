using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class DthDocRcvByHO : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["PDDMember"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                PopBranch(Session[gblValue.UserName].ToString());
                txtDt.Text = Session[gblValue.LoginDate].ToString();
                //LoadGrid(ddlBranch.SelectedValue.ToString(), ddlType.SelectedValue);

                LoadGrid(ddlBranch.SelectedValues.Replace("|", ","), ddlType.SelectedValue);
                txtDt.Enabled = false;
                  Int32 vRow = 0;
            for (vRow = 0; vRow < gvSanc.Rows.Count; vRow++)
            {
            //    //DropDownList ddlFS = (DropDownList)gvSanc.Rows[vRow].FindControl("ddlFS");
            //    TextBox txtSndDt = (TextBox)gvSanc.Rows[vRow].FindControl("txtSndDt");
                 CheckBox chkInsSnt = (CheckBox)gvSanc.Rows[vRow].FindControl("chkInsSnt");
                CheckBox chkPDD = (CheckBox)gvSanc.Rows[vRow].FindControl("chkPDD");
                chkInsSnt.Enabled=false;

            //    if (ddlFS.SelectedIndex <= 0)
            //    {
            //        gblFuction.MsgPopup("Please select FundSource");
            //        vResult = false;
            //    }
            }
              
            }

        }

        //private void PopBranch()
        //{
        //    DataTable dt = null;
        //    CGblIdGenerator oCG = null;
        //    string vBrCode = "";
        //    Int32 vBrId = 0;
        //    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    vBrCode = (string)Session[gblValue.BrnchCode];
        //    vBrId = Convert.ToInt32(vBrCode);
        //    oCG = new CGblIdGenerator();
        //    dt = oCG.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
        //    if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
        //    {
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            if (Convert.ToString(row["BranchCode"]) != Convert.ToString(Session[gblValue.BrnchCode]))
        //            {
        //                row.Delete();
        //            }
        //        }
        //        dt.AcceptChanges();
        //    }
        //    ddlBranch.DataSource = dt;
        //    ddlBranch.DataTextField = "BranchName";
        //    ddlBranch.DataValueField = "BranchCode";
        //    ddlBranch.DataBind();
        //    if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
        //    {
        //        ListItem Li = new ListItem("<-- Select -->", "-1");
        //        ddlBranch.Items.Insert(0, Li);
        //        ddlBranch.Enabled = true;
        //    }
        //}
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (Convert.ToString(row["BranchCode"]) != Convert.ToString(Session[gblValue.BrnchCode]))
                        {
                            row.Delete();
                        }
                    }
                    dt.AcceptChanges();
                }
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                }

            }
            finally
            {
                dt = null;
                oUsr = null;
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
                this.PageHeading = "Death Document Received By HO";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDDHORcv);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Death Document Received By HO", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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
                    //Sourav
                    string vBrCode = ddlBranch.SelectedValues.Replace("|", ",");
                    LoadGrid(vBrCode, ddlType.SelectedValue);
                    //sourav
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
        /// <returns></returns>
        /// 
        //souarv
        private bool ValidDate()
        {
            Boolean vResult = true;
            //if (ddlBranch.SelectedValue == "-1")
            //{
            //    gblFuction.MsgPopup("Please Select the Branch");
            //    vResult = false;
            //}
            if (ddlBranch.SelectedValues == "")
            {
                gblFuction.MsgPopup("Please Select the Branch");
                  vResult = false;
            }

            foreach (GridViewRow row in gvSanc.Rows)
            {
                TextBox txtRcvDt = (TextBox)row.FindControl("txtRcvDt");
                TextBox txtInsSntDt = (TextBox)row.FindControl("txtInsSntDt");
                //dt.Rows[row.RowIndex]["PDDDate"] = row.Cells[7].Text;
                //gblFuction.setDate(Session[gblValue.EndDate].ToString());
                //vPDDDate = gblFuction.setDate(row.Cells[7].Text.ToString());
                //Convert.ToDateTime(txtSndDt.Text)
                //Convert.ToDateTime(vPDDDate)
                CheckBox chkPDD = (CheckBox)row.FindControl("chkPDD");
                CheckBox chkInsSnt = (CheckBox)row.FindControl("chkInsSnt");
                DropDownList ddlIns = (DropDownList)row.FindControl("ddlIns");
                if (chkPDD.Checked == true)
                {
                    if (gblFuction.setDate(txtRcvDt.Text) < gblFuction.setDate(row.Cells[10].Text.ToString()))
                    {
                        gblFuction.MsgPopup("Death Document sent by branch Can not be greater than HO Received date");
                        vResult = false;
                    }
                    if (gblFuction.setDate(Session[gblValue.LoginDate].ToString()) < gblFuction.setDate(txtRcvDt.Text))
                    {

                        gblFuction.MsgPopup("Death Document HO Received Date Can not be greater than login Date date");
                        txtRcvDt.Text = "";
                        vResult = false;
                    }
                    if (chkInsSnt.Checked == true)
                    {

                        if (gblFuction.setDate(txtInsSntDt.Text) < gblFuction.setDate(txtRcvDt.Text))
                        {
                            gblFuction.MsgPopup("Death Document HO Received date Can not be greater than Sent Date of Insurance company");
                            vResult = false;
                        }
                        if (gblFuction.setDate(Session[gblValue.LoginDate].ToString()) < gblFuction.setDate(txtInsSntDt.Text))
                        {

                            gblFuction.MsgPopup("Death Document Insurance sent Date Can not be greater than login Date date");
                            txtInsSntDt.Text = "";
                            vResult = false;
                        }
                        if (ddlIns.SelectedValue == "-1")
                        {
                            gblFuction.MsgPopup("Insurance company Can not be blank");
                            vResult = false;
                        }
                    }
                }
            }

            return vResult;
        }
        //sourav

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDone_Click(object sender, EventArgs e)
        {
            //CProvisionalDeathDeclare oApp = null;
            CDeathDocSnt oApp = null;
            DataTable dt = new DataTable();
            Int32 vErr = 0;
            string vXmlData = "", pMsg = "";
            //DateTime vEffDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DateTime vEffDate = gblFuction.setDate(txtDt.Text);
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();
            string vLogUser = Session[gblValue.UserId].ToString();

            //if (this.RoleId != 1 && this.RoleId != 23 && this.RoleId != 17)
            //{
            //    if (Session[gblValue.EndDate] != null)
            //    {
            //        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > vEffDate)
            //        {
            //            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
            //            return;
            //        }
            //        else if (this.RoleId != 5 && this.RoleId != 10)
            //        {
            //            gblFuction.AjxMsgPopup("You are not AM or Operation ..Can not Save..");
            //            return;
            //        }
            //    }
            //}

            if (ValidDate() == true)
            {
                //Sourav
                //DataTable dt = null;
                //Text="<% #Bind('DDSentBy') %>"
                //DataRow dr = new DataRow();
                //DataRow dr = null;
                //dr["DDSentBy"] = string.Empty;
                //dt.Columns.Add(new DataColumn("DDSentBy", typeof(string)));
                foreach (GridViewRow row in gvSanc.Rows)
                {
                    dt = (DataTable)ViewState["PDDMember"];
                    CheckBox chkPDD = (CheckBox)row.FindControl("chkPDD");
                    TextBox txtRcvDt = (TextBox)row.FindControl("txtRcvDt");
                    CheckBox chkInsSnt = (CheckBox)row.FindControl("chkInsSnt");

                    
                    TextBox txtInsSntDt = (TextBox)row.FindControl("txtInsSntDt");
                    //TextBox txtReffNo = (TextBox)row.FindControl("txtReffNo");
                    //TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");
                    //TextBox txtSndDt = (TextBox)row.FindControl("txtSndDt");
                    DropDownList ddlIns = (DropDownList)row.FindControl("ddlIns");

                    CheckBox chkHoCncl = (CheckBox)row.FindControl("chkHoCncl");
                    TextBox txtHOCnclDt = (TextBox)row.FindControl("txtHOCnclDt");
                    TextBox txtHOCnclRsn = (TextBox)row.FindControl("txtHOCnclRsn");
                    if (chkPDD.Checked == true)
                    {
                        //dt.Rows[row.RowIndex]["ProvDeathDec"] = "Y";
                        dt.Rows[row.RowIndex]["DeathdocSentYN"] = "Y";
                        dt.Rows[row.RowIndex]["DDHORcvDt"] = gblFuction.setDate(txtRcvDt.Text.ToString());
                        dt.Rows[row.RowIndex]["IncId"] = ddlIns.SelectedValue;

                        //dt.Rows[row.RowIndex]["DDSReferenceNo"] = txtReffNo.Text;
                        //dt.Rows[row.RowIndex]["DDSRemarks"] = txtRemarks.Text;
                        //dt.Rows[row.RowIndex]["DDSentDate"] = gblFuction.setDate(txtSndDt.Text.ToString());
                        ////gblFuction.setDate(Session[gblValue.EndDate].ToString())
                        //dt.Rows[row.RowIndex]["BranchName"] = row.Cells[0].Text;
                        //dt.Rows[row.RowIndex]["EoName"] = row.Cells[1].Text;
                        //dt.Rows[row.RowIndex]["Market"] = row.Cells[2].Text;
                        //dt.Rows[row.RowIndex]["GroupName"] = row.Cells[3].Text;
                        //dt.Rows[row.RowIndex]["Member"] = row.Cells[4].Text;
                        //dt.Rows[row.RowIndex]["LoanNo"] = row.Cells[5].Text;
                        //dt.Rows[row.RowIndex]["LoanAmt"] = row.Cells[6].Text;
                        //dt.Rows[row.RowIndex]["PDDDate"] = row.Cells[7].Text;
                        //dt.Rows[row.RowIndex]["DDSentBy"] = ddlSent.SelectedValue;
                        if (chkInsSnt.Checked == true)
                        {
                            dt.Rows[row.RowIndex]["DDRcvByInsYN"] = "Y";
                            
                        }
                        if (txtInsSntDt.Text != "")
                        {
                            dt.Rows[row.RowIndex]["DDInsRcvDt"] = gblFuction.setDate(txtInsSntDt.Text.ToString());
                        }
                        else
                        {
                            dt.Rows[row.RowIndex]["DDInsRcvDt"] = "";
                        }
                        if (chkHoCncl.Checked==true)
                        {
                            dt.Rows[row.RowIndex]["DDCnclByHOYN"] = "Y";
                            dt.Rows[row.RowIndex]["DDHOCnclRsn"] = txtHOCnclRsn.Text.ToString();
                        }
                        if (txtHOCnclDt.Text != "")
                        {
                            dt.Rows[row.RowIndex]["DDHOCnclDt"] = gblFuction.setDate(txtHOCnclDt.Text.ToString());
                        }
                        else
                        {
                            dt.Rows[row.RowIndex]["DDHOCnclDt"] = "";
                        }
                    }


                }
                
                dt.AcceptChanges();
                ViewState["PDDMember"] = dt;
                upSanc.Update();
                //Sourav
                try
                {
                    dt = (DataTable)ViewState["PDDMember"];
                    if (dt == null)
                    {
                        gblFuction.MsgPopup("Nothing to Save");
                        return;
                    }
                    //foreach (DataRow dr in dt.Select("ProvDeathDec='N'"))
                    //{
                    //    dr.Delete();
                    //}
                    dt.AcceptChanges();
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    //oApp = new CProvisionalDeathDeclare();
                    oApp = new CDeathDocSnt();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    //-----------XML Save----------
                    //sourav
                    //vErr = oApp.SaveProvisionalDeathDec(vXmlData, this.UserID, ddlBranch.SelectedValue.ToString(), vEffDate);
                    vErr = oApp.SaveDeathDocument(vXmlData, this.UserID, ddlBranch.SelectedValues.Replace("|", ","), vEffDate, vLogUser, ref pMsg, "R");
                    //Sourav , ref pMsg
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                        //Sourav
                        //LoadGrid(txtSearch.Text, ddlBranch.SelectedValue.ToString());
                        LoadGrid(ddlBranch.SelectedValues.Replace("|", ","), ddlType.SelectedValue);

                    }
                    else
                    {
                        gblFuction.AjxMsgPopup(pMsg);
                        gblFuction.MsgPopup(gblMarg.DBError);
                    }
                }
                finally
                {
                    oApp = null;
                    dt = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()//To Check
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAppMode"></param>
        /// <param name="pFromDt"></param>
        /// <param name="pToDt"></param>
        /// <param name="pBranch"></param>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(string pBranch, string pType)
        {
            DataTable dt = null;
            //CProvisionalDeathDeclare oLS = null;
            CDDHoRcv oLS = null;

            try
            {
                string vBrCode = pBranch;
                string vType = pType;
                oLS = new CDDHoRcv();
                //dt = oLS.GetPDDMemDetails(vBrCode, vType, gblFuction.setDate(txtDt.Text));
                dt = oLS.GetDDSntDetails(vBrCode, vType, gblFuction.setDate(txtDt.Text));
                //sourav
                //for (int i = 0; i <= dt.Rows.Count; i++)
                //{

                //    //GridViewRow row = gvSanc.Rows[i];
                //    foreach (GridViewRow row in gvSanc.Rows)
                //    {
                //        if (row.RowType == DataControlRowType.DataRow)
                //        {
                //            CheckBox chkPDD = (CheckBox)row.FindControl("chkPDD");
                //            CheckBox chkInsSnt = (CheckBox)row.FindControl("chkInsSnt");

                //            if (dt.Rows[i]["DeathDocReceByHOYN"].ToString() == "Y")
                //            {
                //                chkPDD.Checked = true;
                //            }
                //            else
                //            {
                //                chkPDD.Checked = true;
                //            }

                //            if (dt.Rows[i]["DDRcvByInsYN"].ToString() == "Y")
                //            {
                //                chkInsSnt.Checked = true;
                //            }
                //            else
                //            {
                //                chkInsSnt.Checked = true;
                //            }
                //        }
                //    }
                //}
                //foreach (DataRow dr in dt.Rows)
                //{
                //    CheckBox chkPDD = (CheckBox)dr.;
                //    if (dr["DeathdocSentYN"].ToString() == "Y")
                //    {
                //        chkPDD.Checked == true;
                //    }
                //    if (dr["DDSReferenceNo"].ToString() == "")
                //    {
                //        dr["SanDate"] = "  ";
                //    }
                //    if (dr["DDSRemarks"].ToString() == "")
                //    {
                //        dr["CancelRsn"] = "  ";
                //    }
                //    //if (dr["EQIFAXNo"].ToString() == "")
                //    //{
                //    //    dr["EQIFAXNo"] = "XX";
                //    //}
                //    //if (dr["EQIFAXdt"].ToString() == "")
                //    //{
                //    //    dr["EQIFAXdt"] = "01/01/1900";
                //    //}


                //Sourav
                ViewState["PDDMember"] = dt;
                gvSanc.DataSource = dt;
                gvSanc.DataBind();
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
        protected void gvSanc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                DataTable dtIns = null;
                CDDHoRcv oApp = null;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Bind Approval
                    CheckBox chkPDD = (CheckBox)e.Row.FindControl("chkPDD");
                    //DropDownList ddlSent = (DropDownList)e.Row.FindControl("ddlSent");
                    CheckBox chkInsSnt = (CheckBox)e.Row.FindControl("chkInsSnt");
                    CheckBox chkHoCncl = (CheckBox)e.Row.FindControl("chkHoCncl");
                    TextBox txtInsSntDt = (TextBox)e.Row.FindControl("txtInsSntDt");
                    TextBox txtHOCnclDt = (TextBox)e.Row.FindControl("txtHOCnclDt");
                    TextBox txtHOCnclRsn = (TextBox)e.Row.FindControl("txtHOCnclRsn");
                    DropDownList ddlIns = (DropDownList)e.Row.FindControl("ddlIns");
                    chkInsSnt.Enabled = false;
                    txtInsSntDt.Enabled = false;
                    txtHOCnclDt.Enabled = false;
                    txtHOCnclRsn.Enabled = false;
                    chkHoCncl.Enabled = false;
                    ////////////////////////////////////////////////



                    oApp = new CDDHoRcv();
                   
                    ddlIns.Items.Clear();
                    dtIns = oApp.GetInsComp();
                    if (dtIns.Rows.Count > 0)
                    {
                        ddlIns.DataSource = dtIns;
                        ddlIns.DataTextField = "IncCompName";
                        ddlIns.DataValueField = "IncId";
                        ddlIns.DataBind();
                    }
                    ListItem oLk = new ListItem("<--Select-->", "-1");
                    ddlIns.Items.Insert(0, oLk);
                    ddlIns.SelectedIndex = ddlIns.Items.IndexOf(ddlIns.Items.FindByValue(e.Row.Cells[24].Text.Trim()));

                    ///////////////
                    if (e.Row.Cells[15].Text == "Y")
                    {
                        chkPDD.Checked = true;
                        chkHoCncl.Enabled = false;
                    }
                    else if (e.Row.Cells[15].Text == "N")
                    {
                        chkPDD.Checked = false;
                        chkHoCncl.Enabled = true;
                    }
                    //ddlSent.SelectedValue = e.Row.Cells[15].Text;
                    ////////////////////////////////

                    if (e.Row.Cells[18].Text == "Y")
                    {
                        chkInsSnt.Checked = true;
                    }
                    else if (e.Row.Cells[18].Text == "N")
                    {
                        chkInsSnt.Checked = false;
                    }

                    ////////////
                    if (e.Row.Cells[22].Text == "Y")
                    {
                        chkHoCncl.Checked = true;
                       
                    }
                    else if (e.Row.Cells[22].Text == "N")
                    {
                        chkHoCncl.Checked = false;
                        
                    }
                    //////////////////////
                    if (chkPDD.Checked == true)
                    {
                        chkInsSnt.Enabled = true;
                        chkHoCncl.Enabled = true;
                    }
                    if (chkPDD.Checked == false)
                    {
                        chkInsSnt.Enabled = false;
                        chkHoCncl.Enabled = false;
                    }
                    //////////////
                    if (chkInsSnt.Checked == true)
                    {
                        txtInsSntDt.Enabled = true;
                        chkHoCncl.Enabled = false;
                    }
                    if (chkInsSnt.Checked == false)
                    {
                        txtInsSntDt.Enabled = false;
                        chkHoCncl.Enabled = true;
                    }

                }
            }
            finally
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        //OnCheckedChanged="chkPDD_CheckedChanged"
        protected void chkPDD_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            try
            {
                dt = (DataTable)ViewState["PDDMember"];
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkPDD = (CheckBox)row.FindControl("chkPDD");
                //TextBox txtReffNo = (TextBox)row.FindControl("txtReffNo");
                //TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");chkInsSnt
                CheckBox chkInsSnt = (CheckBox)row.FindControl("chkInsSnt");
                TextBox txtInsSntDt = (TextBox)row.FindControl("txtInsSntDt");
                CheckBox chkHoCncl = (CheckBox)row.FindControl("chkHoCncl");


                TextBox txtRcvDt = (TextBox)row.FindControl("txtRcvDt");
                TextBox txtHOCnclDt = (TextBox)row.FindControl("txtHOCnclDt");
                TextBox txtHOCnclRsn = (TextBox)row.FindControl("txtHOCnclRsn");
                if (chkPDD.Checked == true)
                {
                    //dt.Rows[row.RowIndex]["ProvDeathDec"] = "Y";
                    dt.Rows[row.RowIndex]["DeathDocReceByHOYN"] = "Y";
                    chkInsSnt.Enabled = true;
                    txtRcvDt.Text = Session[gblValue.LoginDate].ToString();
                    chkHoCncl.Enabled = true;
                }
                else
                {
                    //dt.Rows[row.RowIndex]["ProvDeathDec"] = "N";
                    dt.Rows[row.RowIndex]["DeathDocReceByHOYN"] = "N";
                    txtRcvDt.Text = "";
                    dt.Rows[row.RowIndex]["DDRcvByInsYN"] = "N";
                    chkInsSnt.Enabled = false;
                    chkInsSnt.Checked = false;
                    txtInsSntDt.Text = "";
                    txtInsSntDt.Enabled = false;
                    chkHoCncl.Enabled = false;
                    chkHoCncl.Checked = false;
                    txtHOCnclDt.Text = "";
                    txtHOCnclDt.Enabled = false;
                    txtHOCnclRsn.Text = "";
                    txtHOCnclRsn.Enabled = false;



                }
                //dt.Rows[row.RowIndex]["DDSReferenceNo"] = txtReffNo.Text;
                //dt.Rows[row.RowIndex]["DDSRemarks"] = txtRemarks.Text;
                dt.AcceptChanges();
                ViewState["PDDMember"] = dt;
                upSanc.Update();
            }
            finally
            {
                dt = null;
            }
        }
        //////////
        protected void chkInsSnt_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            try
            {
                dt = (DataTable)ViewState["PDDMember"];
               
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                //CheckBox chkPDD = (CheckBox)row.FindControl("chkPDD");
                
                CheckBox chkInsSnt = (CheckBox)row.FindControl("chkInsSnt");
                TextBox txtInsSntDt = (TextBox)row.FindControl("txtInsSntDt");
                CheckBox chkHoCncl = (CheckBox)row.FindControl("chkHoCncl");
                if (chkInsSnt.Checked == true)
                {
                    txtInsSntDt.Enabled = true;
                    dt.Rows[row.RowIndex]["DDRcvByInsYN"] = "Y";
                    txtInsSntDt.Text = Session[gblValue.LoginDate].ToString();
                    chkHoCncl.Enabled = false;

                   
                }
                if (chkInsSnt.Checked == false)
                {
                    txtInsSntDt.Enabled = false;
                    txtInsSntDt.Text = "";
                    dt.Rows[row.RowIndex]["DDRcvByInsYN"] = "N";
                    chkHoCncl.Enabled = true;

                }
                
            }
            finally
            {
               
            }
        }

        protected void chkHOCncl_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            try
            {
                dt = (DataTable)ViewState["PDDMember"];

                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                //CheckBox chkPDD = (CheckBox)row.FindControl("chkPDD");

                CheckBox chkHoCncl = (CheckBox)row.FindControl("chkHoCncl");
                TextBox txtHOCnclDt = (TextBox)row.FindControl("txtHOCnclDt");
                TextBox txtHOCnclRsn = (TextBox)row.FindControl("txtHOCnclRsn");
                CheckBox chkInsSnt = (CheckBox)row.FindControl("chkInsSnt");
                if (chkHoCncl.Checked == true)
                {
                    txtHOCnclDt.Enabled = true;
                    txtHOCnclRsn.Enabled = true;
                    dt.Rows[row.RowIndex]["DDCnclByHOYN"] = "Y";
                    txtHOCnclDt.Text = Session[gblValue.LoginDate].ToString();
                    chkInsSnt.Enabled = false;


                }
                if (chkHoCncl.Checked == false)
                {
                    txtHOCnclDt.Enabled = false;
                    txtHOCnclRsn.Enabled = false;
                    txtHOCnclDt.Text = "";
                    txtHOCnclRsn.Text = "";
                    dt.Rows[row.RowIndex]["DDCnclByHOYN"] = "N";
                    chkInsSnt.Enabled = true;

                }

            }
            finally
            {

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

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid(ddlBranch.SelectedValues.Replace("|", ","), ddlType.SelectedValue);
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid(ddlBranch.SelectedValues.Replace("|", ","), ddlType.SelectedValue);
        }




    }
}