using System;
using System.Collections;
using System.Collections.Generic;  
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class EmpMst : CENTRUMBase
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
                txtJnDt.Text = Session[gblValue.LoginDate].ToString();     
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                popQualification();
                PopGender();
                popDesignation();
                LoadGrid(1);
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
                this.PageHeading = "Employee Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuEmpMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Employee Master", false);
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
        private void popQualification()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "QualificationId", "Qualification", "QualificationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlQuali.DataSource = dt;
                ddlQuali.DataTextField = "Qualification";
                ddlQuali.DataValueField = "QualificationId";
                ddlQuali.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlQuali.Items.Insert(0, oli);
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
        private void popDesignation()
        {
            Dictionary<string, string> oDic = new Dictionary<string, string>();
            oDic.Add("<-Select->", "0");
            oDic.Add("BM", "BM");
            oDic.Add("TBM", "TBM");
            oDic.Add("SCO", "SCO");
            oDic.Add("CO", "CO");
            oDic.Add("TCO", "TCO");
            oDic.Add("JTCO", "JTCO");
            oDic.Add("GO", "GO");
            oDic.Add("CEO","CEO");
            oDic.Add("COO", "COO");
            oDic.Add("OM", "OM");
            oDic.Add("RM", "RM");
            oDic.Add("AM", "AM");
            oDic.Add("DM", "DM");
            ddlDesig.DataSource = oDic;
            ddlDesig.DataValueField = "value";
            ddlDesig.DataTextField = "key"; 
            ddlDesig.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopGender()
        {
            Dictionary<string, Int32> oGen = new Dictionary<string, int>();
            oGen.Add("<-Select->", 0);
            oGen.Add("Male", 1);
            oGen.Add("Female", 2);
            oGen.Add("Other", 3);
            ddlGendr.DataSource = oGen;
            ddlGendr.DataValueField = "value";
            ddlGendr.DataTextField = "key"; 
            ddlGendr.DataBind();  
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
                tbEmp.ActiveTabIndex = 1;
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
            tbEmp.ActiveTabIndex = 0;
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
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
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
            CEO oEO = null;
            try
            {     
                oEO = new CEO();
                dt = oEO.GetEOListPG("",this.UserID,"",pPgIndx, ref vTotRows);
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
                oEO = null;
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
            LoadGrid(vPgNo);
            tbEmp.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEmp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vEoId = "";
            DataTable dt = null;
            CEO oEo = null;
            try
            {
                vEoId = Convert.ToString(e.CommandArgument);
                ViewState["EoId"] = vEoId;
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
                    oEo = new CEO();
                    dt = oEo.GetEODetails(vEoId);
                    if (dt.Rows.Count > 0)
                    {
                        txtEmpCode.Text = Convert.ToString(dt.Rows[0]["EMPCode"]);
                        txtJnDt.Text = Convert.ToString(dt.Rows[0]["DOJ"]);
                        txtEmpName.Text = Convert.ToString(dt.Rows[0]["EOName"]);
                        txtJnBr.Text = Convert.ToString(dt.Rows[0]["JBranch"]);
                        txtHouse.Text = Convert.ToString(dt.Rows[0]["HouseNo"]);
                        txtStret.Text = Convert.ToString(dt.Rows[0]["Mohalla"]); 
                        txtVilge.Text = Convert.ToString(dt.Rows[0]["Village"]);
                        txtPO.Text = Convert.ToString(dt.Rows[0]["PO"]);
                        txtDist.Text = Convert.ToString(dt.Rows[0]["District"]);
                        txtPIN.Text = Convert.ToString(dt.Rows[0]["PIN"]);
                        txtState.Text = Convert.ToString(dt.Rows[0]["State"]);
                        txtLand.Text = Convert.ToString(dt.Rows[0]["LandMark"]);
                        txtAge.Text = Convert.ToString(dt.Rows[0]["Age"]);
                        txtPh1.Text = Convert.ToString(dt.Rows[0]["PhNo1"]);
                        txtPh2.Text = Convert.ToString(dt.Rows[0]["PhNo2"]);
                        txtEmail.Text = Convert.ToString(dt.Rows[0]["Email"]);
                        txtRefName.Text = Convert.ToString(dt.Rows[0]["ReferedName"]);
                        txtCont.Text = Convert.ToString(dt.Rows[0]["ContPerson"]);
                        txtFather.Text = Convert.ToString(dt.Rows[0]["FatherName"]);
                        ddlQuali.SelectedIndex = ddlQuali.Items.IndexOf(ddlQuali.Items.FindByValue(Convert.ToString(dt.Rows[0]["QualificationId"])));
                        ddlDesig.SelectedIndex = ddlDesig.Items.IndexOf(ddlDesig.Items.FindByValue(Convert.ToString(dt.Rows[0]["Designation"]).Trim()));
                        ddlGendr.SelectedIndex = ddlGendr.Items.IndexOf(ddlGendr.Items.FindByValue(Convert.ToString(dt.Rows[0]["Gender"])));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbEmp.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oEo = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
	    DateTime vEndDt = gblFuction.setDate(Session[gblValue.EndDate].ToString());
	    Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            //string vAllocate="N", vDropYN="N";
            Int32 vEoId = Convert.ToInt32(ViewState["EoId"]);
            Int32 vErr = 0, vRec = 0, vNewId = 0, vQuliId = 0, vAge=0;
            DateTime vDOJ = gblFuction.setDate(txtJnDt.Text);           
            CEO oEo = null;
            CGblIdGenerator oGbl = null;

            if (vDOJ > vEndDt.AddDays(1))
	        {
		        gblFuction.MsgPopup("Joining Date must be Less than or equal with End Date...");
		        return false;
	        }

            try
            {
                vQuliId = Convert.ToInt32(ddlQuali.SelectedValue);
                vAge = Convert.ToInt32(txtAge.Text);    
                if (Mode == "Save")
                {
                    oEo = new CEO();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("EOMst", "EMPCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("EMP Code can not be Duplicate...");
                        return false;
                    }
                    //vErr = oEo.SaveEOMst(ref vNewId, vEoId, txtEmpCode.Text.Replace("'","''"),  vDOJ, txtEmpName.Text.Replace("'","''"), 
                    //    vQuliId, txtJnBr.Text.Replace("'","''"),  vAge,txtHouse.Text.Replace("'","''"), 
                    //    txtStret.Text.Replace("'","''"), txtVilge.Text.Replace("'","''"), txtPO.Text.Replace("'","''"), txtPIN.Text,
                    //    txtLand.Text.Replace("'","''"), txtDist.Text.Replace("'","''"), txtState.Text.Replace("'","''"), txtPh1.Text,
                    //    txtPh2.Text, txtEmail.Text, txtRefName.Text.Replace("'","''"), txtCont.Text.Replace("'","''"), 
                    //    txtFather.Text.Replace("'","''"), ddlDesig.SelectedValue, ddlGendr.SelectedValue,vAllocate, vDropYN,
                    //    this.UserID, "Save");                             
                    if (vErr > 0)
                    {
                        ViewState["EoId"] = vNewId;
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
                    oEo = new CEO();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("EOMst", "EMPCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("EMP Code Can not be Duplicate...");
                        return false;
                    }
                    vRec = oGbl.ChkDelete(vEoId, "EOID", "GroupMst");
                    if (ddlDesig.SelectedValue == "CO" && vRec > 0)
                    {
                        gblFuction.MsgPopup("Please transfer another CO first then you can change the designation.");
                        return false;
                    }
                    //vErr = oEo.SaveEOMst(ref vNewId, vEoId, txtEmpCode.Text.Replace("'", "''"), vDOJ, txtEmpName.Text.Replace("'", "''"),
                    //  vQuliId, txtJnBr.Text.Replace("'", "''"), vAge, txtHouse.Text.Replace("'", "''"),
                    //  txtStret.Text.Replace("'", "''"), txtVilge.Text.Replace("'", "''"), txtPO.Text.Replace("'", "''"), txtPIN.Text,
                    //  txtLand.Text.Replace("'", "''"), txtDist.Text.Replace("'", "''"), txtState.Text.Replace("'", "''"), txtPh1.Text,
                    //  txtPh2.Text, txtEmail.Text, txtRefName.Text.Replace("'", "''"), txtCont.Text.Replace("'", "''"),
                    //  txtFather.Text.Replace("'", "''"), ddlDesig.SelectedValue, ddlGendr.SelectedValue, vAllocate, vDropYN,
                    //  this.UserID, "Edit");   
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
                    oEo = new CEO();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDelete(vEoId, "EOID", "GroupMst");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("The Staff has group, you can not delete the Staff.");
                        return false;
                    }
                    oEo = new CEO();                   
                    //vErr = oEo.SaveEOMst(ref vNewId, vEoId, txtEmpCode.Text.Replace("'", "''"), vDOJ, txtEmpName.Text.Replace("'", "''"),
                    //  vQuliId, txtJnBr.Text.Replace("'", "''"), vAge, txtHouse.Text.Replace("'", "''"),
                    //  txtStret.Text.Replace("'", "''"), txtVilge.Text.Replace("'", "''"), txtPO.Text.Replace("'", "''"), txtPIN.Text,
                    //  txtLand.Text.Replace("'", "''"), txtDist.Text.Replace("'", "''"), txtState.Text.Replace("'", "''"), txtPh1.Text,
                    //  txtPh2.Text, txtEmail.Text, txtRefName.Text.Replace("'", "''"), txtCont.Text.Replace("'", "''"),
                    //  txtFather.Text.Replace("'", "''"), ddlDesig.SelectedValue, ddlGendr.SelectedValue, vAllocate, vDropYN,
                    //  this.UserID, "Delet"); 
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
                oEo = null;
                oGbl = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtEmpCode.Enabled = Status;
            txtJnDt.Enabled = Status;
            txtEmpName.Enabled = Status;
            txtJnBr.Enabled = Status;
            txtHouse.Enabled = Status;
            txtStret.Enabled = Status;
            txtVilge.Enabled = Status;
            txtPO.Enabled = Status;
            txtDist.Enabled = Status;
            txtPIN.Enabled = Status;
            txtState.Enabled = Status;
            txtLand.Enabled = Status;
            txtAge.Enabled = Status;
            txtPh1.Enabled = Status;
            txtPh2.Enabled = Status;
            txtEmail.Enabled = Status;
            txtRefName.Enabled = Status;
            txtCont.Enabled = Status;
            txtFather.Enabled = Status;
            ddlQuali.Enabled = Status;
            ddlDesig.Enabled = Status;
            ddlGendr.Enabled = Status;
        }


        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtEmpCode.Text ="";
            txtEmpName.Text ="";
            txtJnBr.Text ="";
            txtHouse.Text ="";
            txtStret.Text ="";
            txtVilge.Text ="";
            txtPO.Text ="";
            txtDist.Text ="";
            txtPIN.Text ="";
            txtState.Text ="";
            txtLand.Text ="";
            txtAge.Text ="0";
            txtPh1.Text ="";
            txtPh2.Text ="";
            txtEmail.Text ="";
            txtRefName.Text ="";
            txtCont.Text ="";
            txtFather.Text ="";
            ddlQuali.SelectedIndex = -1;
            ddlDesig.SelectedIndex = -1;
            ddlGendr.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
        }
    }
}