using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.Services;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class NpsColl : CENTRUMBase
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
                ViewState["StateEdit"] = null;
                btnDel.Visible = false;
                ViewState["State"] = null;                
                txtCollDt.Text = Session[gblValue.LoginDate].ToString();
                txtCollDt.Enabled = false;
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
                this.PageHeading = "NPS Collection";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuNpsCol);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "NPS Collection", false);
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {  
                LoadFields();
                gblFuction.MsgPopup(gblMarg.SaveMsg);               
                ViewState["StateEdit"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, EventArgs e)
        {
            CNpsMember oMem = null;
            DataTable dt = null;
            int vErr = 0;
            try
            {

                oMem = new CNpsMember();
                dt = oMem.NPS_ChkIsRemitted(Convert.ToInt32(hdColId.Value));
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["IsRemitted"]) != "Y")
                    {

                        vErr = oMem.NPS_DeleteCollection(Convert.ToInt32(hdColId.Value), Convert.ToInt32(hdNo.Value),
                        (lblMemNo.Text + "-" + hdNo.Value), Session[gblValue.ACVouMst].ToString(),
                        Session[gblValue.ACVouDtl].ToString(), Session[gblValue.BrnchCode].ToString(), this.UserID);
                        if (vErr > 0)
                            gblFuction.MsgPopup("Collection Deleted Sucessfully.");
                        else
                            gblFuction.MsgPopup("Error... For Deleting Collection.");
                        LoadFields();
                    }
                    else
                        gblFuction.MsgPopup("This collection is already remitted. You can not delete this collection.");
                }
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
        protected void rdMod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdMod.SelectedValue == "M")
            {
                btnDel.Visible = true;
                btnSave.Visible = false; 
            }
            else
            {
                btnDel.Visible = false;
                btnSave.Visible = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvArea_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pColId = 0, vRow = 0;
            DataTable dt = null;
            CNpsMember oAgt = null;
            try
            {
                pColId = Convert.ToInt32(e.CommandArgument);
                ViewState["CollID"] = pColId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvArea.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oAgt = new CNpsMember();
                    dt = oAgt.NPS_GetCollectionById(pColId);
                    if (dt.Rows.Count > 0)
                    {
                        txtCollDt.Text = Convert.ToString(dt.Rows[vRow]["RecDate"]);
                        txtPrn.Text = Convert.ToString(dt.Rows[vRow]["PranNo"]);
                        lblMemNo.Text = Convert.ToString(dt.Rows[vRow]["MemID"]);
                        lblMemNm.Text = Convert.ToString(dt.Rows[vRow]["MemName"]);    
                        txtColAmt.Text = Convert.ToString(dt.Rows[vRow]["CollAmt"]);
                        hdColId.Value = Convert.ToString(dt.Rows[vRow]["CollID"]);
                        hdNo.Value = Convert.ToString(dt.Rows[vRow]["InstNo"]);
                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                    }
                }
            }
            finally
            {
                dt = null;
                oAgt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = "", vBrCode ="", vAcMst="", vAcDtl="", vFinYr="";
            Int32 vId = 0, vRec = 0;
            double vColAmt = 0;
            DataTable dt = null;
            CNpsMember oMem = null;

            vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            vBrCode = Session[gblValue.BrnchCode].ToString();
            vAcMst = Session[gblValue.ACVouMst].ToString();
            vAcDtl = Session[gblValue.ACVouDtl].ToString();
            vFinYr = Session[gblValue.ShortYear].ToString();
            vId = Convert.ToInt32(ViewState["CollID"]);
            CNpsMember oPrm = null;            

            try
            {
                if (lblMemNo.Text == "")
                {
                    gblFuction.MsgPopup("Membership No can not be empty.");
                    return false; 
                }

                if (txtColAmt.Text.Equals("0") || txtColAmt.Text.Equals(""))
                    vColAmt = 0;
                else
                {
                    vColAmt = Convert.ToDouble(txtColAmt.Text);
                    if (vColAmt < 100 || vColAmt > 12000)
                    {
                        gblFuction.MsgPopup("Invalid Amount Entered");
                        return false;
                    }
                }
                //
                if (this.RoleId != 1)  //Admin and NPS AGM Role 
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtCollDt.Text))
                        {
                            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                            return false;
                        }
                    }
                }
                oMem = new CNpsMember();
                dt = oMem.NPS_GetMemberByID(lblMemNo.Text.Replace("'", "''"));
                if (dt.Rows.Count > 0)
                {
                    DateTime ct = gblFuction.setDate(Convert.ToString(dt.Rows[0]["DOB"]));
                    DateTime futureDt = ct.AddYears(60);
                    if (futureDt<gblFuction.setDate(txtCollDt.Text))
                    {
                        gblFuction.MsgPopup("Age should not be greate than 60 years");
                        return false;
                    }
                    //Int32 vCurrYear = Convert.ToInt32(txtCollDt.Text.Substring(6, 4));
                    //Int32 vAge = Convert.ToInt32(Convert.ToString(dt.Rows[0]["DOB"]).Substring(6, 4));
                    //Int32 vNoYr = vCurrYear - vAge;
                    //if (vNoYr < 18 || vNoYr >= 60)
                    //{
                    //    gblFuction.MsgPopup("Age is not between 18 and 60 years");
                    //    return false;
                    //}
                }
                if (Convert.ToString(ViewState["PRANNo"]) == "")
                {
                    gblFuction.AjxMsgPopup("PRAN No should not be blank..");
                    return false;
                }
                
                if (Mode == "Save")
                {
                    oPrm = new CNpsMember();
                    vRec = oPrm.NPS_SaveCollection(ref vId, lblMemNo.Text.Replace("'", "''"),
                        txtPrn.Text.Replace("'", "''"), gblFuction.setDate(txtCollDt.Text),
                        vColAmt, "N0001", this.UserID, vBrCode, "Save", vFinYr, vAcMst, vAcDtl);
                    LoadFields(); 
                    if (vRec > 0)
                        gblFuction.MsgPopup("Save Collection Successfully.");
                    else
                        gblFuction.MsgPopup("Error... For Saving Collection.");
                }
                return vResult;
            }
            finally
            {
                oPrm = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            //ddlNpsAc.Enabled = Status;
            //txtNLAORegNo.Enabled = Status;
            //txtNLAOOff.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            //ddlNpsAc.SelectedIndex = -1;
            //txtNLAORegNo.Text = "";
            //txtNLAOOff.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null, dt1 = null;
            CNpsMember oMem = null;
            ViewState["PRANNo"] = null;
            try
            {
                oMem = new CNpsMember();
                ds = oMem.NPS_GetMemberByPran(txtPrn.Text.Trim(), "P", Session[gblValue.BrnchCode].ToString());
                dt = ds.Tables[0];
                dt1 = ds.Tables[1]; 
                if (dt.Rows.Count > 0)
                {
                    txtPrn.Text = Convert.ToString(dt.Rows[0]["PranNo"]);
                    ViewState["PRANNo"] = Convert.ToString(dt.Rows[0]["PranNo"]);
                    txtMemNo.Text = Convert.ToString(dt.Rows[0]["MemID"]);
                    lblMemNo.Text = Convert.ToString(dt.Rows[0]["MemID"]);
                    lblMemNm.Text = Convert.ToString(dt.Rows[0]["MemName"]);   
                    lblDtJn.Text = Convert.ToString(dt.Rows[0]["SubmissionDt"]);
                    lblAgn.Text = Convert.ToString(dt.Rows[0]["Name"]);
                    lblBalAmt.Text = Convert.ToString(dt.Rows[0]["CollAmt"]);
                    txtColAmt.Text = "";
                }
                else
                {
                    txtPrn.Text = "";
                    lblMemNo.Text = "";
                    lblMemNm.Text = "";
                    lblDtJn.Text = "";
                    lblAgn.Text = "";
                    lblBalAmt.Text = "0";
                    txtColAmt.Text = "";
                }
                if (dt1.Rows.Count > 0)
                {
                    gvArea.DataSource = dt1;
                    gvArea.DataBind();
                }
                else
                {
                    gvArea.DataSource = null;
                    gvArea.DataBind();
                }
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMemShow_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null, dt1 = null;
            CNpsMember oMem = null;
            ViewState["PRANNo"] = "";
            try
            {
                oMem = new CNpsMember();
                ds = oMem.NPS_GetMemberByPran(txtMemNo.Text.Trim(), "M", Session[gblValue.BrnchCode].ToString());
                dt = ds.Tables[0];
                dt1 = ds.Tables[1]; 
                if (dt.Rows.Count > 0)
                {
                    txtPrn.Text = Convert.ToString(dt.Rows[0]["PranNo"]);
                    ViewState["PRANNo"] = Convert.ToString(dt.Rows[0]["PranNo"]);
                    lblMemNo.Text = Convert.ToString(dt.Rows[0]["MemID"]);
                    lblMemNm.Text = Convert.ToString(dt.Rows[0]["MemName"]);   
                    lblDtJn.Text = Convert.ToString(dt.Rows[0]["SubmissionDt"]);
                    lblAgn.Text = Convert.ToString(dt.Rows[0]["Name"]);
                    lblBalAmt.Text = Convert.ToString(dt.Rows[0]["CollAmt"]);
                    txtColAmt.Text = "";
                }
                else
                {
                    txtPrn.Text = "";
                    lblMemNo.Text = "";
                    lblMemNm.Text = "";
                    lblDtJn.Text = "";
                    lblAgn.Text = "";
                    lblBalAmt.Text = "0";
                    txtColAmt.Text = "";
                }
                if (dt1.Rows.Count > 0)
                {
                    gvArea.DataSource = dt1;
                    gvArea.DataBind();
                }
                else
                {
                    gvArea.DataSource = null;
                    gvArea.DataBind();
                }
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadFields()
        {
            DataTable dt = null;
            CNpsMember oMem = null;

            try
            {
                oMem = new CNpsMember();
                dt = oMem.NPS_GetCollectionDtls(txtPrn.Text );
                if (dt.Rows.Count > 0)
                {
                    gvArea.DataSource = dt;
                    gvArea.DataBind(); 
                }
                else
                {
                    gvArea.DataSource = null; ;
                    gvArea.DataBind(); 
                }
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }        
    }
}