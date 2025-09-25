using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class CollTimeChange : CENTRUMBase
    {
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
                popDate();
                popNewDate();
                ddlCollSche.SelectedIndex = 3;
                ddlDNo.Enabled = false;
                LoadList();
                tbCRtn.ActiveTabIndex = 1;
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
                this.PageHeading = "Collection Time Change";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCollTimeCng);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Collection Time Change", false);
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

                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;

                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;

                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;

                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;

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
            dt.AcceptChanges();
            return dt;
        }

        private void EnableControl(Boolean Status)
        {
            ddlRO.Enabled = Status;
            ddlMarketId.Enabled = Status;
            ddlCollDay.Enabled = false;
            ddlDNo.Enabled = false;
            ddlNewColDay.Enabled = false;
            txtTm.Enabled = Status;
            ddlAlloSmTym.Enabled = false;
            ddlCollSche.Enabled = false;
            ddlCollType.Enabled = false;
            txtEffDt.Enabled = Status;
        }

        private void ClearControls()
        {
            ddlRO.SelectedIndex = -1;
            ddlMarketId.Items.Clear();
            ddlMarketId.SelectedIndex = -1;
            ddlCollSche.SelectedIndex = -1;
            ddlCollDay.SelectedIndex = -1;
            ddlDNo.SelectedIndex = -1;
            ddlNewColDay.SelectedIndex = -1;
            txtTm.Text = "";
            ddlAlloSmTym.SelectedIndex = -1;
            txtEffDt.Text = Session[gblValue.LoginDate].ToString();
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
            PopCenter(ddlRO.SelectedValue);
        }

        protected void ddlMarketId_SelectedIndexChanged(object sender, EventArgs e)
        {           
            GetMeetingDayInfo(ddlMarketId.SelectedValue);
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

        private void GetMeetingDayInfo(string pMarketId)
        {
            DataTable dt = null;
            CCollectionRoutine oGb = null;
            try
            {
                oGb = new CCollectionRoutine();
                dt = oGb.GetMeetingDayInfo(pMarketId);
                if (dt.Rows.Count > 0)
                {
                    ddlCollSche.SelectedIndex = ddlCollSche.Items.IndexOf(ddlCollSche.Items.FindByValue(dt.Rows[0]["CollSchedule"].ToString()));                    
                    ViewState["RoutId"] = dt.Rows[0]["RoutineID"].ToString();
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
                    txtTm.Text = dt.Rows[0]["Colltime"].ToString();
                    ddlAlloSmTym.SelectedIndex = ddlAlloSmTym.Items.IndexOf(ddlAlloSmTym.Items.FindByValue(dt.Rows[0]["Allow"].ToString()));                   
                    tbCRtn.ActiveTabIndex = 1;                    
                    //if (dt.Rows[0]["CollSchedule"].ToString() == "3" && dt.Rows[0]["CollType"].ToString() == "1")
                    //    ddlDNo.Enabled = true;
                    //else
                    //    ddlDNo.Enabled = false;
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
                    //ddlDNo.Enabled = true;
                    popDate();
                    popNewDate();
                }
                else
                {
                    //ddlDNo.Enabled = true;
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
                    //ddlDNo.Enabled = true;
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
            ListItem oLI = new ListItem("<-- Select -->", "-1");
            ddlNewColDay.Items.Add(oLI);
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
            Int32 vErr = 0, vDayNo = 0, vNewId = 0, vNewColDay = 0;
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vRoutId = Convert.ToInt32(ViewState["RoutId"]);
            string vYn = "", vMarketId = "", vEoId = "", vMsg = "", vCollType = "";
            CCollectionRoutine oCR = null;
            CGblIdGenerator oGbl = null;
            try
            {
                vEoId = ddlRO.SelectedValue;
                vMarketId = ddlMarketId.SelectedValue;
                vCollType = ddlCollType.SelectedValue;

                vNewColDay = Convert.ToInt32(ddlNewColDay.SelectedValue);
                vDayNo = Convert.ToInt32(ddlDNo.SelectedValue);

                vYn = ddlAlloSmTym.SelectedValue.ToString();
                DateTime vCollTime = setTime(txtTm.Text);
                DateTime vSTime = setTime("07:00 AM");
                DateTime vETime = setTime("01:00 PM");
                if (vCollTime < vSTime || vCollTime > vETime)
                {
                    gblFuction.MsgPopup("Collection Time should be from 07:00 AM to 01:00 PM");
                    return false;
                }
                string min = txtTm.Text.ToString().Substring(3, 2);
                if (min != "00")
                {
                    gblFuction.MsgPopup("Collection Time should be in correct format");
                    return false;
                }

                if (Mode == "Save" || Mode == "Edit")
                {
                    if (Convert.ToString(ddlCollSche.SelectedValue) == "3" && Convert.ToString(ddlCollType.SelectedValue) == "1" &&
                             Convert.ToString(ddlDNo.SelectedValue) == "3")
                    {
                        DataTable dtBC;
                        oCR = new CCollectionRoutine();
                        dtBC = oCR.ChkBCBrStat(vBrCode);
                        if (Convert.ToString(dtBC.Rows[0]["BCBranchYN"]) == "Y")
                        {
                            dtBC.Dispose();
                            gblFuction.MsgPopup("BC Branch can not set Collection Day No as 15-21 of Month");
                            return false;
                        }
                        dtBC.Dispose();
                    }
                }

                if (Mode == "Save")
                {
                    oCR = new CCollectionRoutine();
                    oGbl = new CGblIdGenerator();             
                    vMsg = oCR.ChkDupColltime_Name(vEoId, vMarketId, vLoginDt, vCollTime, vNewColDay, vDayNo, vCollType, "Edit");
                    if (vMsg != "")
                    {
                        gblFuction.MsgPopup(vMsg);
                        return false;
                    }
                    vMsg = "";
                    vErr = oCR.UpdateCollectionTime(vRoutId, vMarketId, setTime(txtTm.Text), gblFuction.setDate(txtEffDt.Text), Convert.ToInt32(Session[gblValue.UserId].ToString()));
                    if (vErr > 0)
                    {
                        ViewState["RoutId"] = vRoutId;
                        vResult = true;
                    }
                    else
                    {
                        if (vMsg == "")
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                        }
                        else
                        {
                            gblFuction.MsgPopup(vMsg);
                        }
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
        }       

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbCRtn.ActiveTabIndex = 1;
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