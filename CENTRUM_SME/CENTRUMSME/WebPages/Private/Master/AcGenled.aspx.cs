using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class AcGenled : CENTRUMBAse
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
                try
                {
                    ViewState["GlId"] = Request.QueryString["GlId"].ToString();
                    if (ViewState["GlId"] == null || ViewState["GlId"].ToString() == "")
                        LoadList();
                    else
                        LoadList();
                    //GetLoadList(ViewState["GlId"].ToString());
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
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);
                
                this.Menu = false;
                this.PageHeading = "Account Ledger";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString()+ " ( Login Date " + Session[gblValue.LoginDate].ToString()  + " )";
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadList()
        {
            DataTable dtRoot = null;
            TreeNode tnRoot = null;
            TreeNode tnAccGrp = null;
            Int32 vAcctId = 0;
            tvAcGenLed.Nodes.Clear();
            CGblIdGenerator oCb = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oCb = new CGblIdGenerator();
                dtRoot = oCb.PopComboMIS("N", "N", "AA", "ACHeadId", "ACHead", "ACHead", 0, "AA", "AA", vLogDt, "0000");
                foreach (DataRow dr in dtRoot.Rows)
                {
                    tnRoot = new TreeNode(dr["ACHead"].ToString());
                    tnRoot.PopulateOnDemand = false;
                    tnRoot.SelectAction = TreeNodeSelectAction.None;
                    tvAcGenLed.Nodes.Add(tnRoot);
                    tnRoot.Value = Convert.ToString("AH" + dr["ACHeadId"]);
                    vAcctId = Convert.ToInt32(dr["ACHeadId"]);
                    tnAccGrp = new TreeNode("No Record");
                    tnAccGrp.Value = "AG";
                    tnRoot.ChildNodes.Add(tnAccGrp);
                    tnRoot.CollapseAll();
                }
            }
            finally
            {
                dtRoot=null;
                oCb = null;
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
        protected void tvAcGenLed_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            DataTable dtAcctGrp = null;
            DataTable dtAcctSubGrp = null;
            DataTable dtLedger = null;
            TreeNode tnGroup = null;
            TreeNode tnSubGroup = null;
            TreeNode tnLedger = null;
            Int32 vAHId = 0, vGID = 0, vSGID=0;            
            string vGenDesc = "";
            CGblIdGenerator oCb = null;
            CAcGenled oAcGenLed = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                if (e.Node.Value.Substring(0, 2) == "AH")
                {
                    oCb = new CGblIdGenerator();
                    e.Node.ChildNodes.Clear();
                    vAHId = Convert.ToInt32(e.Node.Value.Substring(2));
                    dtAcctGrp = oCb.PopComboMIS("S", "N", "AA", "ACGroupId", "ACGroup", "AcGroup", vAHId, "ACHeadId", "AA", vLogDt, "0000");
                    foreach (DataRow drBr in dtAcctGrp.Rows)
                    {
                        tnGroup = new TreeNode(Convert.ToString(drBr["AcGroup"]));
                        tnGroup.Value = Convert.ToString("AG" + drBr["ACGroupId"]);
                        e.Node.ChildNodes.Add(tnGroup);
                        tnSubGroup = new TreeNode("No Record");
                        e.Node.SelectAction = TreeNodeSelectAction.None;
                        tnGroup.SelectAction = TreeNodeSelectAction.None;
                        tnSubGroup.Value = "0";
                        tnGroup.ChildNodes.Add(tnSubGroup);
                    }
                }
                else if (e.Node.Value.Substring(0, 2) == "AG")
                {
                    oCb = new CGblIdGenerator();
                    e.Node.ChildNodes.Clear();
                    vGID = Convert.ToInt32(e.Node.Value.Substring(2));
                    dtAcctSubGrp = oCb.PopComboMIS("S", "N", "AA", "AcSubGrpId", "SubGrp", "ACSubGrp", vGID, "AcGroupId", "AA", vLogDt, "0000");
                    foreach (DataRow drGrp in dtAcctSubGrp.Rows)
                    {
                        tnSubGroup = new TreeNode(drGrp["SubGrp"].ToString());
                        tnSubGroup.Value = Convert.ToString("AS" + drGrp["AcSubGrpId"]);
                        e.Node.ChildNodes.Add(tnSubGroup);
                        tnLedger = new TreeNode("No Record");
                        e.Node.SelectAction = TreeNodeSelectAction.None;
                        tnSubGroup.SelectAction = TreeNodeSelectAction.None;
                        tnLedger.Value = "0";
                        tnSubGroup.ChildNodes.Add(tnLedger);
                    }
                }
                else if (e.Node.Value.Substring(0, 2) == "AS")
                {
                    oAcGenLed = new CAcGenled();
                    e.Node.ChildNodes.Clear();
                    vSGID = Convert.ToInt32(e.Node.Value.Substring(2));
                    dtLedger = oAcGenLed.GetGenLedByAcSubGrpId(vSGID, vGenDesc, vBrCode);
                    foreach (DataRow drLed in dtLedger.Rows)
                    {
                        tnLedger = new TreeNode(drLed["GenDesc"].ToString());
                        tnLedger.Value = drLed["DescId"].ToString();
                        tnLedger.PopulateOnDemand = false;
                        e.Node.SelectAction = TreeNodeSelectAction.SelectExpand;
                        e.Node.ChildNodes.Add(tnLedger);
                    }
                }
            }
            finally
            {
                oCb = null;
                oAcGenLed = null;
                dtAcctGrp = null;
                dtAcctSubGrp = null;
                dtLedger = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvAcGenLed_SelectedNodeChanged(object sender, EventArgs e)
        {
            CAcGenled oAcGenLed = new CAcGenled();
            string pGenLedId = "";
            try
            {
                pGenLedId = Convert.ToString(tvAcGenLed.SelectedNode.Value);
                if (pGenLedId == "" || pGenLedId != null)
                {
                    Response.Redirect("~/Webpages/Private/Master/GenLedDtl.aspx?GlId=" + pGenLedId, false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}