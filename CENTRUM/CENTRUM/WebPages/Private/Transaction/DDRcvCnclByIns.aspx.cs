using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class DDRcvCnclByIns : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["PDDMember"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtDt.Text = Session[gblValue.LoginDate].ToString();
                txtDt.Enabled = false;
                //PopBranch();
                PopBranch(Session[gblValue.UserName].ToString());
                LoadGrid(ddlBranch.SelectedValues.Replace("|", ","), ddlType.SelectedValue);
                

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
                this.PageHeading = "Death Document Received/Cancel By Insurance Company";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDDInsRcvCncl);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Death Document Received/Cancel By Insurance Company", false);
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
            //DateTime v
            if (ddlBranch.SelectedValues == "")
            {
                gblFuction.MsgPopup("Please Select the Branch");
                vResult = false;
            }

            foreach (GridViewRow row in gvSanc.Rows)
            {
                TextBox txtInsRcvDt = (TextBox)row.FindControl("txtInsRcvDt");
                TextBox txtCnclDt = (TextBox)row.FindControl("txtCnclDt");
                //dt.Rows[row.RowIndex]["PDDDate"] = row.Cells[7].Text;
                //gblFuction.setDate(Session[gblValue.EndDate].ToString());
                //vPDDDate = gblFuction.setDate(row.Cells[7].Text.ToString());
                //Convert.ToDateTime(txtSndDt.Text)
                //Convert.ToDateTime(vPDDDate)
                CheckBox chkCancel = (CheckBox)row.FindControl("chkCancel");
                TextBox txtCnclRsn = (TextBox)row.FindControl("txtCnclRsn");
                if (chkCancel.Checked == true)
                {
                    if (txtCnclRsn.Text == "")
                    {
                        gblFuction.MsgPopup("Cancel Reason can not be blank");
                        vResult = false;
                    }
                }
                if (txtInsRcvDt.Text.ToString()!="")
                {
                    if (gblFuction.setDate(txtInsRcvDt.Text) < gblFuction.setDate(row.Cells[7].Text.ToString()))
                    {
                        gblFuction.MsgPopup("Death Document sent by HO Can not be greater than  Received date by Insurance company");
                        vResult = false;
                    }
                    if (gblFuction.setDate(Session[gblValue.LoginDate].ToString()) < gblFuction.setDate(txtInsRcvDt.Text))
                    {

                        gblFuction.MsgPopup("Death Document Insurance Company Claimed Date Can not be greater than login Date date");
                        txtInsRcvDt.Text = "";
                        vResult = false;
                    }
                }
                if (txtCnclDt.Text.ToString() != "")
                {
                    if (gblFuction.setDate(txtCnclDt.Text) < gblFuction.setDate(row.Cells[7].Text.ToString()))
                    {
                        gblFuction.MsgPopup("Death Document sent by HO Can not be greater than  Cancel date by Insurance company");
                        vResult = false;
                    }
                    if (gblFuction.setDate(Session[gblValue.LoginDate].ToString()) < gblFuction.setDate(txtCnclDt.Text))
                    {

                        gblFuction.MsgPopup("Death Document Insurance Company Cancel Date Can not be greater than login Date date");
                        txtInsRcvDt.Text = "";
                        vResult = false;
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
                    CheckBox chkRcv = (CheckBox)row.FindControl("chkRcv");
                    TextBox txtInsRcvDt = (TextBox)row.FindControl("txtInsRcvDt");

                    CheckBox chkCancel = (CheckBox)row.FindControl("chkCancel");
                    TextBox txtCnclDt = (TextBox)row.FindControl("txtCnclDt");
                    TextBox txtCnclRsn = (TextBox)row.FindControl("txtCnclRsn");
                    //TextBox txtReffNo = (TextBox)row.FindControl("txtReffNo");
                    //TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");
                    //TextBox txtSndDt = (TextBox)row.FindControl("txtSndDt");
                    //DropDownList ddlSent = (DropDownList)row.FindControl("ddlSent");
                    if (chkRcv.Checked == true)
                    {
                        //dt.Rows[row.RowIndex]["ProvDeathDec"] = "Y";
                        dt.Rows[row.RowIndex]["DDClmdByInsYN"] = "Y";
                        dt.Rows[row.RowIndex]["DDClmdByInsDate"] = gblFuction.setDate(txtInsRcvDt.Text.ToString());
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
                       
                    }
                    if (chkCancel.Checked == true)
                    {
                        dt.Rows[row.RowIndex]["DDCnclByInsYN"] = "Y";
                        dt.Rows[row.RowIndex]["DDCnclByInsDate"] = gblFuction.setDate(txtCnclDt.Text.ToString());
                        dt.Rows[row.RowIndex]["DDCnclByInsReason"] = txtCnclRsn.Text.ToString();
                    }
                    if (txtInsRcvDt.Text=="")
                    {
                        dt.Rows[row.RowIndex]["DDClmdByInsDate"] = "";
                    }
                    if (txtCnclRsn.Text == "")
                    {
                        dt.Rows[row.RowIndex]["DDCnclByInsDate"] = "";
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
                    vErr = oApp.SaveDeathDocument(vXmlData, this.UserID, ddlBranch.SelectedValues.Replace("|", ","), vEffDate, vLogUser, ref pMsg, "Y");
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
            CDDRcvCnclIns oLS = null;

            try
            {
                string vBrCode = pBranch;
                string vType = pType;
                oLS = new CDDRcvCnclIns();
                //dt = oLS.GetPDDMemDetails(vBrCode, vType, gblFuction.setDate(txtDt.Text));
                //dt = oLS.GetDDSntDetails(vBrCode, vType, gblFuction.setDate(txtDt.Text));
                dt = oLS.GetDDRcvInsDetails(vBrCode, vType, gblFuction.setDate(txtDt.Text));
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
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Bind Approval
                    CheckBox chkRcv = (CheckBox)e.Row.FindControl("chkRcv");
                    TextBox txtInsRcvDt = (TextBox)e.Row.FindControl("txtInsRcvDt");
                    txtInsRcvDt.Enabled = false;

                    CheckBox chkCancel = (CheckBox)e.Row.FindControl("chkCancel");
                    TextBox txtCnclDt = (TextBox)e.Row.FindControl("txtCnclDt");
                    TextBox txtCnclRsn = (TextBox)e.Row.FindControl("txtCnclRsn");
                    txtCnclDt.Enabled = false;
                    txtCnclRsn.Enabled = false;
                   
                    ///////////////
                    if (e.Row.Cells[10].Text == "Y")
                    {
                        chkRcv.Checked = true;
                    }
                    else if (e.Row.Cells[10].Text == "N")
                    {
                        chkRcv.Checked = false;
                    }
                    //ddlSent.SelectedValue = e.Row.Cells[15].Text;
                    ////////////////////////////////

                    if (e.Row.Cells[14].Text == "Y")
                    {
                        chkCancel.Checked = true;
                    }
                    else if (e.Row.Cells[14].Text == "N")
                    {
                        chkCancel.Checked = false;
                    }

                    ////////////
                    if (chkRcv.Checked == true)
                    {
                        txtInsRcvDt.Enabled = true;
                        chkCancel.Enabled = false;
                    }
                    if (chkRcv.Checked == false)
                    {
                        txtInsRcvDt.Enabled = false;
                        txtInsRcvDt.Text = "";
                        chkCancel.Enabled = true;
                    }
                    //////////////
                    if (chkCancel.Checked == true)
                    {
                        txtCnclDt.Enabled = true;
                        txtCnclRsn.Enabled = true;
                        chkRcv.Enabled = false;
                    }
                    if (chkCancel.Checked == false)
                    {
                        txtCnclDt.Enabled = false;
                        txtCnclRsn.Enabled = false;
                        chkRcv.Enabled = true;
                        txtCnclDt.Text = "";
                        txtCnclRsn.Text = "";
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
        protected void chkRcv_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            try
            {
                dt = (DataTable)ViewState["PDDMember"];
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkRcv = (CheckBox)row.FindControl("chkRcv");
                //TextBox txtReffNo = (TextBox)row.FindControl("txtReffNo");
                //TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");chkInsSnt
                CheckBox chkCancel = (CheckBox)row.FindControl("chkCancel");
                TextBox txtInsRcvDt = (TextBox)row.FindControl("txtInsRcvDt");
                if (chkRcv.Checked == true)
                {
                    //dt.Rows[row.RowIndex]["ProvDeathDec"] = "Y";
                    dt.Rows[row.RowIndex]["DDClmdByInsYN"] = "Y";
                    txtInsRcvDt.Enabled = true;
                    chkCancel.Enabled = false;
                    txtInsRcvDt.Text = Session[gblValue.LoginDate].ToString();
                    
                }
                else
                {
                    //dt.Rows[row.RowIndex]["ProvDeathDec"] = "N";
                    dt.Rows[row.RowIndex]["DDClmdByInsYN"] = "N";
                    txtInsRcvDt.Enabled = false;
                    chkCancel.Enabled = true;
                    txtInsRcvDt.Text = "";

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
        protected void chkCancel_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;

            try
            {
                dt = (DataTable)ViewState["PDDMember"];
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkRcv = (CheckBox)row.FindControl("chkRcv");
                //TextBox txtReffNo = (TextBox)row.FindControl("txtReffNo");
                //TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");chkInsSnt
                CheckBox chkCancel = (CheckBox)row.FindControl("chkCancel");
                TextBox txtCnclDt = (TextBox)row.FindControl("txtCnclDt");
                TextBox txtCnclRsn = (TextBox)row.FindControl("txtCnclRsn");
                if (chkCancel.Checked == true)
                {
                    chkRcv.Enabled = false;
                    txtCnclDt.Enabled = true;
                    txtCnclRsn.Enabled = true;
                    dt.Rows[row.RowIndex]["DDCnclByInsYN"] = "Y";
                    txtCnclDt.Text = Session[gblValue.LoginDate].ToString();

                }
                if (chkCancel.Checked == false)
                {
                    chkRcv.Enabled = true;
                    txtCnclDt.Enabled = false;
                    txtCnclRsn.Enabled = false;
                    txtCnclDt.Text = "";
                    txtCnclRsn.Text = "";
                    dt.Rows[row.RowIndex]["DDCnclByInsYN"] = "N";


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