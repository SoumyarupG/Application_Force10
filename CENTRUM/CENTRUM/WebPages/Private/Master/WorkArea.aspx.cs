using System;
using System.Collections;
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
using System.IO;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class WorkArea : CENTRUMBase
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
                ViewState["State"] = null;             
                popBranch();              
                tbWA.ActiveTabIndex = 0;
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
                this.PageHeading = "Allocation of Work Area";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuWrkAreaMst);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;               
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {                    
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {                    
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {                  
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Allocation of Work Area", false);
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
                    btnSave.Enabled = true;                    
                    btnExit.Enabled = false;
                    ClearControls();                   
                    break;
                case "Show":                    
                    btnSave.Enabled = false;                    
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":                    
                    btnSave.Enabled = true;                   
                    btnExit.Enabled = false;
                    EnableControl(true);                    
                    break;
                case "View":                   
                    btnSave.Enabled = false;                    
                    btnExit.Enabled = true;
                    EnableControl(false);                   
                    break;
                case "Delete":                   
                    btnSave.Enabled = false;                    
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        private void popBranch()
        { 
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBranch.Items.Insert(0, oli);
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
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CWorkArea oWA = null;            
            try
            {
                oWA = new CWorkArea();
                dt = oWA.GetDistrictByBranch(ddlBranch.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    lbxDist.DataSource = dt;
                    lbxDist.DataTextField = "DistrictName";
                    lbxDist.DataValueField = "DistrictId";                   
                    lbxDist.DataBind();
                    for (int vR = 0; vR < dt.Rows.Count; vR++)
                    {
                        if (dt.Rows[vR]["AllocYN"].ToString() == "Y")                        
                            lbxDist.Items[vR].Selected = true;                                                                          
                    }
                    lbxDist_SelectedIndexChanged(this.lbxDist, System.EventArgs.Empty);                   
                }
                else
                {
                    lbxDist.DataSource = null;
                    lbxDist.DataBind();
                    PopList("");
                }
            }
            finally
            {
                oWA = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void lbxDist_SelectedIndexChanged(object sender, EventArgs e)
        {            
            string vDistId = "";             
            foreach (ListItem oLI in lbxDist.Items)
            {
                if (oLI.Selected == true)
                {
                    if (vDistId == "")
                        vDistId += oLI.Value;
                    else
                        vDistId += "," + oLI.Value;
                }                   
            }
            PopList(vDistId);   
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
                tbWA.ActiveTabIndex = 1;
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
            tbWA.ActiveTabIndex = 0;
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
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);                
                StatusButton("View");
                ViewState["StateEdit"] = null;
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
            DataTable dt1 = null, dt2 = null;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = ddlBranch.SelectedValue;
            string vLogDt = Session[gblValue.LoginDate].ToString();    
            string vXmlDist = "", vXmlVilg = "";
            Int32 vThlId = Convert.ToInt32(ViewState["TehId"]);
            Int32 vErr = 0, i=1;         
            CWorkArea oWA = null;
            
            try
            {
                oWA = new CWorkArea();                
                dt1 = GetDistrict();
                dt2 = GetVillage(); 
                if (Mode == "Save")
                {
                    DataRow dR1;
                    foreach (ListItem oL in lbxDist.Items)
                    {
                        dR1 = dt1.NewRow();
                        dR1["DtlId"] = i;
                        dR1["DistrictId"] = oL.Value;
                        if (oL.Selected == true)
                        {
                            dR1["AllocationDt"] = gblFuction.setDate(vLogDt);
                            dR1["DeAllocationDt"] = gblFuction.setDate("");
                        }
                        else
                        {
                            dR1["AllocationDt"] = gblFuction.setDate("");
                            dR1["DeAllocationDt"] = gblFuction.setDate(vLogDt);
                        }
                        dt1.Rows.Add(dR1);
                        i += 1;                            
                    }                   
                    dt1.AcceptChanges();                   
                    //
                    DataRow dR2;
                    i = 1;
                    foreach (TreeNode tn in tvWA.CheckedNodes)
                    {
                        foreach (TreeNode tc in tn.ChildNodes)
                        {
                            foreach (TreeNode tv in tc.ChildNodes)
                            {
                                dR2 = dt2.NewRow();
                                dR2["DtlId"] = i;
                                dR2["VillageId"] = tv.Value;
                                if (tv.Checked == true)
                                {
                                    dR2["AllocationDt"] = gblFuction.setDate(vLogDt);
                                    dR2["DeAllocationDt"] = gblFuction.setDate("");
                                }
                                else
                                {
                                    dR2["AllocationDt"] = gblFuction.setDate("");
                                    dR2["DeAllocationDt"] = gblFuction.setDate(vLogDt);
                                }
                                dt2.Rows.Add(dR2);
                                i += 1;
                            }
                        }
                    }                  
                    dt2.AcceptChanges();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt1.WriteXml(oSW);
                        vXmlDist = oSW.ToString();
                    }
                    using (StringWriter oSW1 = new StringWriter())
                    {
                        dt2.WriteXml(oSW1);
                        vXmlVilg = oSW1.ToString();
                    }
                    vErr = oWA.SaveWorkAreaNew(vBrCode, this.UserID, vXmlDist, vXmlVilg);
                    if (vErr > 0)
                    {                       
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
                dt1 = null;
                dt2 = null;
                oWA = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GetVillage()
        {
            DataTable dt = new DataTable("Village");
            dt.Columns.Add("DtlId");
            dt.Columns.Add("VillageId");
            dt.Columns.Add("AllocationDt");
            dt.Columns.Add("DeAllocationDt");  
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GetDistrict()
        {
            DataTable dt = new DataTable("District");
            dt.Columns.Add("DtlId");
            dt.Columns.Add("DistrictId");
            dt.Columns.Add("AllocationDt");
            dt.Columns.Add("DeAllocationDt"); 
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {            
            ddlBranch.Enabled = Status;
            lbxDist.Enabled = Status;
            tvWA.Enabled = Status; 
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {            
            ddlBranch.SelectedIndex = -1;
            lbxDist.Items.Clear();            
            lblDate.Text = "";
            lblUser.Text = "";
        }
                

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDistId"></param>
        private void PopList(string pDistId)
        {
            DataTable dtBlk = null, dtGp = null, dtVlg = null;
            TreeNode tnBlk = null, tnGp = null, tnVlg = null;
            string vBlkId = "", vGpId = "";
            CWorkArea oWA = null;
            string vBranchCd = ddlBranch.SelectedValue;
            DateTime vLogDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            tvWA.Nodes.Clear();
            try
            {
                oWA = new CWorkArea();
                dtBlk = oWA.GetBlockByDistrict(pDistId);
                foreach (DataRow dr in dtBlk.Rows)
                {
                    tnBlk = new TreeNode(dr["BlockName"].ToString());
                    tnBlk.PopulateOnDemand = false;
                    tnBlk.SelectAction = TreeNodeSelectAction.None;
                    tvWA.Nodes.Add(tnBlk);
                    tnBlk.Value = dr["BlockId"].ToString();
                    vBlkId = tnBlk.Value;
                    dtGp = oWA.GetGPByBlock(vBlkId);
                    foreach (DataRow drDay in dtGp.Rows)
                    {
                        tnGp = new TreeNode(drDay["GPName"].ToString());
                        tnGp.PopulateOnDemand = false;
                        tnBlk.SelectAction = TreeNodeSelectAction.None;
                        tnGp.Value = drDay["GPId"].ToString();
                        vGpId = tnGp.Value;
                        tnBlk.ChildNodes.Add(tnGp);
                        tnBlk.Expand();  
                        dtVlg = oWA.GetVillageByGP(vGpId,ddlBranch.SelectedValue);
                        foreach (DataRow drCent in dtVlg.Rows)
                        {
                            tnVlg = new TreeNode(drCent["VillageName"].ToString());
                            tnVlg.PopulateOnDemand = false;
                            tnBlk.SelectAction = TreeNodeSelectAction.None;
                            tnVlg.Value = drCent["VillageId"].ToString();
                            tnGp.ChildNodes.Add(tnVlg);
                            if (drCent["AllocteYN"].ToString() == "Y")
                            {
                                tnBlk.Checked = true;
                                tnGp.Checked = true;
                                tnVlg.Checked = true;
                            }
                            else
                            {
                                tnVlg.Checked = false;
                            }
                            tnGp.Expand();  
                        }
                    }
                }
            }
            finally
            {
                dtBlk = null;
                dtGp = null;
                dtVlg = null;
                oWA = null;
            }
        }
    }
}