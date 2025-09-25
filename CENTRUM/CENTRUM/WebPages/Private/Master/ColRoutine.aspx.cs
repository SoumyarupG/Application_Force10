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
    public partial class ColRoutine : CENTRUMBase
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
                ViewState["EoId"] = null;
                txtEffDt.Text = Session[gblValue.LoginDate].ToString();
                PopRO();
                // PopCollection();
                //PopNewCollection();
                popDate();
                popNewDate();
                ddlCollSche.SelectedIndex = 3;
                ddlDNo.Enabled = false;
                LoadList();
                tbCRtn.ActiveTabIndex = 0;
                StatusButton("View");
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
                this.PageHeading = "Collection Routine Setup";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuColleRoutin);
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
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Collection Routin", false);
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
        private void LoadList()
        {
            DataTable dt = null;
            TreeNode tnRoot = null;
            TreeNode tnNRec = null;
            tvCRtne.Nodes.Clear();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            //CCollectionRoutine oCR = null;
            CEO oRO = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                //oCR = new CCollectionRoutine();
                //dt = oCR.GetCollRoutineList(vBrCode, vLogDt);
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                foreach (DataRow dr in dt.Rows)
                {
                    tnRoot = new TreeNode(dr["EOName"].ToString());
                    tnRoot.PopulateOnDemand = false;
                    tnRoot.SelectAction = TreeNodeSelectAction.None;
                    tvCRtne.Nodes.Add(tnRoot);
                    tnRoot.Value = Convert.ToString("EO" + dr["EOID"]);
                    TreeNode tnDate = new TreeNode("Date-Wise");
                    tnDate.PopulateOnDemand = false;
                    tnDate.Value = "DT";
                    tnRoot.ChildNodes.Add(tnDate);
                    tnRoot.CollapseAll();

                    tnNRec = new TreeNode("No Record");
                    tnNRec.PopulateOnDemand = false;
                    tnNRec.Value = "DN2";
                    tnDate.ChildNodes.Add(tnNRec);
                    tnDate.CollapseAll();

                    TreeNode tnDay = new TreeNode("Day-Wise");
                    tnDay.PopulateOnDemand = false;
                    tnDay.Value = "DY";
                    tnRoot.ChildNodes.Add(tnDay);
                    tnRoot.CollapseAll();

                    tnNRec = new TreeNode("No Record");
                    tnNRec.PopulateOnDemand = false;
                    tnNRec.Value = "DN1";
                    tnDay.ChildNodes.Add(tnNRec);
                    tnDay.CollapseAll();

                    tnDay.SelectAction = TreeNodeSelectAction.None;
                }
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        protected void tvCRtne_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            DataTable dtWeek = null, dtCR = null;
            TreeNode tnDays = null;
            TreeNode tnGrp = null;
            string vEoId = "";
            int vGID = 0;
            CCollectionRoutine oColl = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                if (e.Node.Value.Substring(0, 2) == "EO")
                {
                    ViewState["EoId"] = e.Node.Value.Substring(2);
                }
                else if (e.Node.Value.Substring(0, 2) == "DT")
                {
                    e.Node.ChildNodes.Clear();
                    for (int i = 1; i <= 28; i++)
                    {
                        tnDays = new TreeNode(Convert.ToString(i.ToString("00")));
                        tnDays.Value = Convert.ToString("DN2" + i.ToString());
                        e.Node.ChildNodes.Add(tnDays);
                        tnGrp = new TreeNode("No Record");
                        e.Node.SelectAction = TreeNodeSelectAction.None;
                        tnDays.SelectAction = TreeNodeSelectAction.None;
                        tnGrp.Value = "0";
                        tnDays.ChildNodes.Add(tnGrp);
                    }
                }
                else if (e.Node.Value.Substring(0, 2) == "DY")
                {
                    e.Node.ChildNodes.Clear();
                    dtWeek = getWeekDays();
                    foreach (DataRow drBr in dtWeek.Rows)
                    {
                        tnDays = new TreeNode(Convert.ToString(drBr["Days"]));
                        tnDays.Value = Convert.ToString("DN1" + drBr["DayNo"]);
                        e.Node.ChildNodes.Add(tnDays);
                        tnGrp = new TreeNode("No Record");
                        e.Node.SelectAction = TreeNodeSelectAction.None;
                        tnDays.SelectAction = TreeNodeSelectAction.None;
                        tnGrp.Value = "0";
                        tnDays.ChildNodes.Add(tnGrp);
                    }
                }
                else if (e.Node.Value.Substring(0, 3) == "DN2")
                {
                    oColl = new CCollectionRoutine();
                    e.Node.ChildNodes.Clear();
                    vGID = Convert.ToInt32(e.Node.Value.Substring(3));
                    //vEoId = ViewState["EoId"].ToString();
                    vEoId = e.Node.Parent.Parent.Value.Substring(2);
                    ViewState["EoId"] = vEoId;
                    dtCR = oColl.GetCollRoutineByMarketId(vEoId, vGID, vBrCode, 2);
                    foreach (DataRow drCr in dtCR.Rows)
                    {
                        tnGrp = new TreeNode(drCr["GrpName"].ToString());
                        tnGrp.Value = Convert.ToString(drCr["RoutineId"]);
                        e.Node.ChildNodes.Add(tnGrp);
                    }
                }
                else if (e.Node.Value.Substring(0, 3) == "DN1")
                {
                    oColl = new CCollectionRoutine();
                    e.Node.ChildNodes.Clear();
                    vGID = Convert.ToInt32(e.Node.Value.Substring(3));
                    //vEoId = ViewState["EoId"].ToString();
                    vEoId = e.Node.Parent.Parent.Value.Substring(2);
                    ViewState["EoId"] = vEoId;
                    dtCR = oColl.GetCollRoutineByMarketId(vEoId, vGID, vBrCode, 1);
                    foreach (DataRow drCr in dtCR.Rows)
                    {
                        tnGrp = new TreeNode(drCr["GrpName"].ToString());
                        tnGrp.Value = Convert.ToString(drCr["RoutineId"]);
                        e.Node.ChildNodes.Add(tnGrp);
                    }
                }
            }
            finally
            {
                oColl = null;
                dtWeek = null;
                dtCR = null;
            }
        }

        protected void tvCRtne_SelectedNodeChanged(object sender, EventArgs e)
        {
            DataTable dt;
            Int32 vRtnId = 0;
            CCollectionRoutine oCR = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oCR = new CCollectionRoutine();
                vRtnId = Convert.ToInt32(tvCRtne.SelectedNode.Value);
                ViewState["RoutId"] = vRtnId;
                dt = oCR.GetCollRoutineDtls(vRtnId);
                if (dt.Rows.Count > 0)
                {
                    
                    ddlRO.SelectedIndex = ddlRO.Items.IndexOf(ddlRO.Items.FindByValue(dt.Rows[0]["EOID"].ToString()));
                    //GetCenter(dt.Rows[0]["EOID"].ToString(), dt.Rows[0]["GroupID"].ToString());
                    GetGrpByEo(ddlRO.SelectedValue);
                    ddlMarketId.SelectedIndex = ddlMarketId.Items.IndexOf(ddlMarketId.Items.FindByValue(dt.Rows[0]["GroupID"].ToString()));
                    ddlCollSche.SelectedIndex = ddlCollSche.Items.IndexOf(ddlCollSche.Items.FindByValue(dt.Rows[0]["CollSchedule"].ToString()));

                    ddlCollSche.SelectedIndex = ddlCollSche.Items.IndexOf(ddlCollSche.Items.FindByValue(dt.Rows[0]["CollSchedule"].ToString()));
                    ViewState["CollSchedl"] = dt.Rows[0]["CollSchedule"].ToString();
                    ddlCollType.SelectedIndex = ddlCollType.Items.IndexOf(ddlCollType.Items.FindByValue(dt.Rows[0]["CollType"].ToString()));
                    if (dt.Rows[0]["CollType"].ToString() != "1")
                    {
                        popDate();
                        popNewDate();
                    }
                    else
                    {
                        PopCollection();
                        PopNewCollection();
                    }
                    ddlCollDay.SelectedIndex = ddlCollDay.Items.IndexOf(ddlCollDay.Items.FindByValue(dt.Rows[0]["CollDay"].ToString()));
                    ddlDNo.SelectedIndex = ddlDNo.Items.IndexOf(ddlDNo.Items.FindByValue(dt.Rows[0]["CollDayNo"].ToString()));
                    ddlNewColDay.SelectedIndex = ddlCollDay.Items.IndexOf(ddlCollDay.Items.FindByValue(dt.Rows[0]["CollDay"].ToString()));
                    txtTm.Text = dt.Rows[0]["Colltime"].ToString();
                    ddlAlloSmTym.SelectedIndex = ddlAlloSmTym.Items.IndexOf(ddlAlloSmTym.Items.FindByValue(dt.Rows[0]["Allow"].ToString()));
                    txtEffDt.Text = dt.Rows[0]["EffectiveDate"].ToString();
                    tbCRtn.ActiveTabIndex = 1;
                    StatusButton("Show");
                    if (dt.Rows[0]["CollSchedule"].ToString() == "M" && dt.Rows[0]["CollType"].ToString() != "2")
                        ddlDNo.Enabled = true;
                    else
                        ddlDNo.Enabled = false;
                }
            }
            finally
            {
                dt = null;
                oCR = null;
            }
        }

        private DataTable getWeekDays()
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("Days");
            dt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn("DayNo");
            dt.Columns.Add(dc2);
            DataRow dr1 = dt.NewRow();
            dr1["Days"] = "Monday";
            dr1["DayNo"] = "1";
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2["Days"] = "Tuesday";
            dr2["DayNo"] = "2";
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3["Days"] = "Wednesday";
            dr3["DayNo"] = "3";
            dt.Rows.Add(dr3);
            DataRow dr4 = dt.NewRow();
            dr4["Days"] = "Thursday";
            dr4["DayNo"] = "4";
            dt.Rows.Add(dr4);
            DataRow dr5 = dt.NewRow();
            dr5["Days"] = "Friday";
            dr5["DayNo"] = "5";
            dt.Rows.Add(dr5);
            DataRow dr6 = dt.NewRow();
            dr6["Days"] = "Saturday";
            dr6["DayNo"] = "6";
            dt.Rows.Add(dr6);
            DataRow dr7 = dt.NewRow();
            //dr7["Days"] = "Sunday";
            //dr7["DayNo"] = "7";
            //dt.Rows.Add(dr7);
            dt.AcceptChanges();
            return dt;
        }

        private void EnableControl(Boolean Status)
        {
            ddlRO.Enabled = Status;
            ddlMarketId.Enabled = Status;
            ddlCollDay.Enabled = Status;
            //ddlDNo.Enabled = Status;
            ddlNewColDay.Enabled = Status;
            txtTm.Enabled = Status;
            ddlAlloSmTym.Enabled = Status;
            ddlCollSche.Enabled = Status;
            txtEffDt.Enabled = Status;
        }

        private void ClearControls()
        {
            ddlRO.SelectedIndex = -1;
            ddlMarketId.Items.Clear();
            ddlMarketId.SelectedIndex = -1;
            //ddlCollSche.SelectedIndex = -1;
            //ddlCollDay.SelectedIndex = -1;
            //ddlDNo.SelectedIndex = -1;
            // ddlNewColDay.SelectedIndex = -1;
            txtTm.Text = "";
            ddlAlloSmTym.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
        }

        private void PopRO()
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

        protected void ddlRO_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            //PopCenter(ddlRO.SelectedValue);
            GetGrpByEo(ddlRO.SelectedValue);
           
        }

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
                    ddlMarketId.DataSource = dt;
                    ddlMarketId.DataTextField = "Market";
                    ddlMarketId.DataValueField = "MarketID";
                    ddlMarketId.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlMarketId.Items.Insert(0, oli);
                }
            }
            finally
            {
                dt = null;
                oGb = null;
            }
        }

        private void GetCenter(string pEoId, string pMarketId)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("S", "N", "AA", "MarketID", "Market", "MarketMSt", pEoId, "EoId", "AA", gblFuction.setDate("01/01/1900"), "0000");
                if (dt.Rows.Count > 0)
                {
                    ddlMarketId.DataSource = dt;
                    ddlMarketId.DataTextField = "Market";
                    ddlMarketId.DataValueField = "MarketID";
                    ddlMarketId.DataBind();
                    ddlMarketId.SelectedIndex = ddlMarketId.Items.IndexOf(ddlMarketId.Items.FindByValue(pMarketId));
                }
            }
            finally
            {
                dt = null;
                oGb = null;
            }
        }
        private void GetGrpByEo(string pEoId)
        {
            DataTable dt = null;
            CCollectionRoutine oGb = null;
            try
            {
                oGb = new CCollectionRoutine();
                dt = oGb.GetGrpByEo(pEoId, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                if (dt.Rows.Count > 0)
                {
                    ddlMarketId.DataSource = dt;
                    ddlMarketId.DataTextField = "GroupName";
                    ddlMarketId.DataValueField = "Groupid";
                    ddlMarketId.DataBind();
                    //ddlMarketId.SelectedIndex = ddlMarketId.Items.IndexOf(ddlMarketId.Items.FindByValue(pMarketId));
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlMarketId.Items.Insert(0, oli);
                }
            }
            finally
            {
                dt = null;
                oGb = null;
            }
        }
        protected void ddlCollSche_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCollSche.SelectedValue == "3")
            {
                ddlCollType.ClearSelection();
                ddlCollType.SelectedValue = "2";

                if (ddlCollType.SelectedIndex == 0)
                {
                    ddlDNo.Enabled = true;
                    popDate();
                    popNewDate();
                }
                else
                {
                    ddlDNo.Enabled = true;
                    PopCollection();
                    PopNewCollection();
                }
            }
            else
            {
                ddlDNo.Enabled = false;
                ddlDNo.SelectedIndex = ddlDNo.Items.IndexOf(ddlDNo.Items.FindByValue("0"));
                ddlCollType.SelectedIndex = 1;
                PopCollection();
                PopNewCollection();
            }
        }

        protected void ddlCollType_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlCollType.SelectedIndex == 0)
            {
                popDate();
                popNewDate();
                ddlDNo.SelectedIndex = ddlDNo.Items.IndexOf(ddlDNo.Items.FindByValue("0"));
                ddlDNo.Enabled = false;
            }
            else
            {
                PopCollection();
                PopNewCollection();
                ddlCollDay.Enabled = true;
                if (ddlCollSche.SelectedValue == "3")
                {
                    ddlDNo.Enabled = true;
                }
            }
        }

        private void PopCollection()
        {
            ddlCollDay.Items.Clear();
            ListItem Lst1 = new ListItem("Monday", "1");
            ddlCollDay.Items.Add(Lst1);
            ListItem Lst2 = new ListItem("Tuesday", "2");
            ddlCollDay.Items.Add(Lst2);
            ListItem Lst3 = new ListItem("Wednesday", "3");
            ddlCollDay.Items.Add(Lst3);
            ListItem Lst4 = new ListItem("Thursday", "4");
            ddlCollDay.Items.Add(Lst4);
            ListItem Lst6 = new ListItem("Friday", "5");
            ddlCollDay.Items.Add(Lst6);
            ListItem Lst7 = new ListItem("Saturday", "6");
            ddlCollDay.Items.Add(Lst7);
            //ListItem Lst0 = new ListItem("Sunday", "7");
            //ddlCollDay.Items.Add(Lst0);
            ListItem oLI = new ListItem("<-- Select -->", "-1");
            ddlCollDay.Items.Insert(0, oLI);
        }

        private void PopNewCollection()
        {
            ddlNewColDay.Items.Clear();
            ListItem Lst1 = new ListItem("Monday", "1");
            ddlNewColDay.Items.Add(Lst1);
            ListItem Lst2 = new ListItem("Tuesday", "2");
            ddlNewColDay.Items.Add(Lst2);
            ListItem Lst3 = new ListItem("Wednesday", "3");
            ddlNewColDay.Items.Add(Lst3);
            ListItem Lst4 = new ListItem("Thursday", "4");
            ddlNewColDay.Items.Add(Lst4);
            ListItem Lst6 = new ListItem("Friday", "5");
            ddlNewColDay.Items.Add(Lst6);
            ListItem Lst7 = new ListItem("Saturday", "6");
            ddlNewColDay.Items.Add(Lst7);
            //ListItem Lst0 = new ListItem("Sunday", "7");
            //ddlNewColDay.Items.Add(Lst0);
            ListItem oLI = new ListItem("<-- Select -->", "-1");
            ddlNewColDay.Items.Insert(0, oLI);
        }

        private void popDate()
        {
            ddlCollDay.Items.Clear();
            for (int i = 1; i < 29; i++)
            {
                ListItem Lst1 = new ListItem(i.ToString("00"), i.ToString());
                ddlCollDay.Items.Add(Lst1);
            }
        }

        private void popNewDate()
        {
            ddlNewColDay.Items.Clear();
            for (int i = 1; i < 29; i++)
            {
                ListItem Lst1 = new ListItem(i.ToString("00"), i.ToString());
                ddlNewColDay.Items.Add(Lst1);
            }
        }

        public Int32 GetLoanNo()
        {
            string vEoid = ddlMarketId.SelectedValue;
            int vLoanNO = 0;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                vLoanNO = oGb.GetLoanNO(vEoid);
                return vLoanNO;
            }
            finally
            {
                oGb = null;
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0, vCollDay = 0, vDayNo = 0, vNewId = 0, vNewColDay = 0;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vRoutId = Convert.ToInt32(ViewState["RoutId"]);
            string vColSch = "", vYn = "", vMarketId = "", vEoId = "", vCollType = "";
            CCollectionRoutine oCR = null;
            CGblIdGenerator oGbl = null;
            try
            {
                vEoId = ddlRO.SelectedValue;
                vMarketId = ddlMarketId.SelectedValue;
                vCollType = ddlCollType.SelectedValue;
                vCollDay = Convert.ToInt32(ddlCollDay.SelectedValue);
                vNewColDay = Convert.ToInt32(ddlNewColDay.SelectedValue);
                vDayNo = Convert.ToInt32(ddlDNo.SelectedValue);
                vColSch = ddlCollSche.SelectedValue.ToString();
                vYn = ddlAlloSmTym.SelectedValue.ToString();
                if (Mode == "Save")
                {
                    oCR = new CCollectionRoutine();
                    oGbl = new CGblIdGenerator();
                    //vErr = oGbl.ChkDuplicate("CollectionRoutine", "MarketId", vMarketId, "", "", "RoutineId", vRoutId.ToString(), "Save");//collection routin for a market cannot be more than 1
                    //if (vErr > 0)
                    //{
                    //    gblFuction.MsgPopup("Center already has a collection routine");
                    //    return false;
                    //}
                    //vErr = oCR.ChkSameTimeByCo(vEoId, vBrCode, setTime(txtTm.Text), vYn, vRoutId);
                    //if (vErr > 0)
                    //{
                    //    gblFuction.MsgPopup("The CO has same collection time of another group. Allow Same Time to Yes.");
                    //    return false;
                    //}
                    // Chech for LoanId
                    vErr = oCR.ChkDuplicateCollection(vMarketId, vBrCode);//collection routin for a market cannot be more than 1
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Group already has a collection routine");
                        return false;
                    }

                    vErr = oCR.SaveCollectionRoutine(ref vNewId, vRoutId, vMarketId, vColSch, vCollType, vCollDay, vDayNo, setTime(txtTm.Text),
                        gblFuction.setDate(txtEffDt.Text), vYn, vBrCode, this.UserID, "Save");
                    if (vErr > 0)
                    {
                        if (vErr == 2)
                        {
                            gblFuction.MsgPopup("CO Formation Date Should Be Less Than Collection Routine Effective Date");
                            vResult = false;
                        }
                        else
                        {
                            ViewState["RoutId"] = vNewId;
                            vResult = true;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    Int32 vLoanNo = GetLoanNo();
                    if (vLoanNo > 0)
                    {
                        gblFuction.MsgPopup("The Loan has been disburse under this " + ddlMarketId.SelectedItem + " center,You cannot edit collection routine.");
                        return false;
                    }
                    oCR = new CCollectionRoutine();
                    oGbl = new CGblIdGenerator();
                    //vErr = oGbl.ChkDuplicate("CollectionRoutine", "MarketId", vMarketId, "", "", "RoutineID", vRoutId.ToString(), "Edit");
                    //if (vErr > 0)
                    //{
                    //    gblFuction.MsgPopup("Center has a collection routine already");
                    //    return false;
                    //}
                    //vErr = oCR.ChkSameTimeByCo(vEoId, vBrCode, setTime(txtTm.Text), vYn, vRoutId);
                    //if (vErr > 0)
                    //{
                    //    gblFuction.MsgPopup("The CO has same collection time of another group. Allow Same Time to Yes.");
                    //    return false;
                    //}
                    vErr = oCR.SaveCollectionRoutine(ref vNewId, vRoutId, vMarketId, vColSch, vCollType, vNewColDay, vDayNo, setTime(txtTm.Text),
                        gblFuction.setDate(txtEffDt.Text), vYn, vBrCode, this.UserID, "Edit");
                    if (vErr > 0)
                    {
                        if (vErr == 2)
                        {
                            gblFuction.MsgPopup("Centre Formation Date Should Be Less Than Collection Effective Date");
                            vResult = false;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.EditMsg);
                            vResult = true;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oCR = new CCollectionRoutine();
                    oGbl = new CGblIdGenerator();
                    vErr = oCR.SaveCollectionRoutine(ref vNewId, vRoutId, vMarketId, vColSch, vCollType, vNewColDay, vDayNo, setTime(txtTm.Text),
                        gblFuction.setDate(txtEffDt.Text), vYn, vBrCode, this.UserID, "Delet");
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
                oCR = null;
                oGbl = null;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            ViewState["StateEdit"] = null;
            ViewState["State"] = null;
            ViewState["EoId"] = null;
            ViewState["RoutId"] = null;
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.CanAdd == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Add);
                return;
            }
            ViewState["StateEdit"] = "Add";
            tbCRtn.ActiveTabIndex = 1;
            StatusButton("Add");
            ClearControls();
            ddlNewColDay.Enabled = false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CanDelete == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Del);
                return;
            }
            if (SaveRecords("Delete") == true)
            {
                gblFuction.MsgPopup(gblMarg.DeleteMsg);
                LoadList();
                StatusButton("Delete");
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.CanEdit == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Edit);
                return;
            }

            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
            ddlCollDay.Enabled = false;

            if (ddlCollSche.SelectedValue == "3" && ddlCollType.SelectedValue != "2")
            {
                ddlDNo.Enabled = true;
            }
            else
            {
                ddlDNo.Enabled = false;
            }
            ddlRO.Enabled = false;
            ddlMarketId.Enabled = false;
            //ddlCollSche.Enabled = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbCRtn.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }

        public DateTime setTime(string pTime)
        {
            DateTime vDate = Convert.ToDateTime("01/01/1900" + " " + pTime);
            return vDate;
        }
    }
}