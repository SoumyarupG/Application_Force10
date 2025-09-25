using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;




namespace CENTRUM.WebPages.Private.Transaction
{
	public partial class LoanDisbGrp : CENTRUMBase
	{
		protected int cPgNo = 1;
		protected int vFlag = 0;

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
				popRO();
				popDisbBy();
				LoadBankLedger();
				PopLoanType();
				PopPurpose();
				PopSubPurpose();
				PopFunder();
				txtFrmDt.Text = (string)Session[gblValue.LoginDate];
				txtToDt.Text = (string)Session[gblValue.LoginDate];
				txtLnDt.Text = (string)Session[gblValue.LoginDate];
				LoadGrid(1);
				ViewState["StateEdit"] = null;
				StatusButton("View");
				tabLoanDisb.ActiveTabIndex = 0;
                //gblFuction.AjxMsgPopup(((int)Convert.ToDateTime(txtLnDt.Text).DayOfWeek).ToString());

			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnShow_Click(object sender, EventArgs e)
		{
            LoadGrid(1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pPgIndx"></param>
		private void LoadGrid(Int32 pPgIndx)
		{
			DataTable dt = null;
			CDisburse oLD = null;
			Int32 vRows = 0;
			string vBrCode = string.Empty;
			vBrCode = (string)Session[gblValue.BrnchCode];
			DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
			DateTime vToDt = gblFuction.setDate(txtToDt.Text);
			oLD = new CDisburse();
			dt = oLD.GetDisburseList(vFrmDt, vToDt, vBrCode,txtSearch.Text.Trim() ,pPgIndx, ref vRows);
			gvLoanAppl.DataSource = dt.DefaultView;
			gvLoanAppl.DataBind();
			lblTotalPages.Text = CalTotPgs(vRows).ToString();
			lblCurrentPage.Text = cPgNo.ToString();
			if (cPgNo == 1)
			{
				Btn_Previous.Enabled = false;
				if (Int32.Parse(lblTotalPages.Text) > 1)
					Btn_Next.Enabled = true;
				else
					Btn_Next.Enabled = false;
			}
			else
			{
				Btn_Previous.Enabled = true;
				if (cPgNo == Int32.Parse(lblTotalPages.Text))
					Btn_Next.Enabled = false;
				else
					Btn_Next.Enabled = true;
			}
		}
        protected void ddlPAIAppli_SelectedIndexChanged(object sender, EventArgs e)
        {
           if(ddlPAIAppli.SelectedValue=="Y")
            {
                txtPAIAmt.Text = hdPAIAmt.Value;
            }
           if(ddlPAIAppli.SelectedValue == "N")
           {
               txtPAIAmt.Text = "0";
           }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pRows"></param>
		/// <returns></returns>
		private int CalTotPgs(double pRows)
		{
			int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
			return totPg;
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
				case "Previous":
					cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
					break;
				case "Next":
					cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
					break;
			}
			LoadGrid(cPgNo);
			tabLoanDisb.ActiveTabIndex = 0;
			//tabLoanDisb.Tabs[0].Enabled = true;
			//tabLoanDisb.Tabs[1].Enabled = false;
			//tabLoanDisb.Tabs[2].Enabled = false;
		}

		/// <summary>
		/// 
		/// </summary>
		private void LoadBankLedger()
		{
			DataTable dt = null;
			CVoucher oVoucher = null;
			string vBranch = Session[gblValue.BrnchCode].ToString();
			oVoucher = new CVoucher();
			dt = oVoucher.GetAcGenLedCB(vBranch, "B","");

			for (int i = 0; i <= dt.Rows.Count-1; i++)
			{
					if (dt.Rows[i]["Desc"].ToString().Contains("NPS") == true)
					{
							dt.Rows[i].Delete();
					}
			}
			dt.AcceptChanges();

			ddlBank.DataSource = dt;
			ddlBank.DataTextField = "Desc";
			ddlBank.DataValueField = "DescId";
			ddlBank.DataBind();
			ListItem liSel = new ListItem("<--- Select --->", "-1");
			ddlBank.Items.Insert(0, liSel);
            oVoucher = null;
            oVoucher = new CVoucher();
            dt = oVoucher.GetAcGenLedCB("0000", "B", "");

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if (dt.Rows[i]["Desc"].ToString().Contains("NPS") == true)
                {
                    dt.Rows[i].Delete();
                }
            }
            dt.AcceptChanges();

            ddlBankHo.DataSource = dt;
            ddlBankHo.DataTextField = "Desc";
            ddlBankHo.DataValueField = "DescId";
            ddlBankHo.DataBind();
            ListItem liSel1 = new ListItem("<--- Select --->", "-1");
            ddlBankHo.Items.Insert(0, liSel1);

		}

		/// <summary>
		/// 
		/// </summary>
		private void PopPurpose()
		{
			DataTable dt = null;
			CGblIdGenerator oGbl = null;
			try
			{
				ddlPurps.Items.Clear();
				string vBrCode = Session[gblValue.BrnchCode].ToString();
				oGbl = new CGblIdGenerator();
				dt = oGbl.PopComboMIS("N", "N", "AA", "PurposeID", "Purpose", "LoanPurposeMst", "0", "AA", "AA", System.DateTime.Now, "0000");
				ddlPurps.DataSource = dt;
				ddlPurps.DataTextField = "Purpose";
				ddlPurps.DataValueField = "PurposeId";
				ddlPurps.DataBind();
				ListItem oLi = new ListItem("<--Select-->", "-1");
				ddlPurps.Items.Insert(0, oLi);
			}
			finally
			{
				dt = null;
				oGbl = null;
			}
		}

		private void PopSubPurpose()
		{
			DataTable dt = null;
			CGblIdGenerator oGbl = null;
			try
			{
				ddlSubPur.Items.Clear();
				string vBrCode = Session[gblValue.BrnchCode].ToString();
				oGbl = new CGblIdGenerator();
				dt = oGbl.PopComboMIS("N", "N", "AA", "SubPurposeID", "SubPurpose", "SubPurposeMst", "0", "AA", "AA", System.DateTime.Now, "0000");
				ddlSubPur.DataSource = dt;
				ddlSubPur.DataTextField = "SubPurpose";
				ddlSubPur.DataValueField = "SubPurposeId";
				ddlSubPur.DataBind();
				ListItem oLi = new ListItem("<--Select-->", "-1");
				ddlSubPur.Items.Insert(0, oLi);
			}
			finally
			{
				dt = null;
				oGbl = null;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void PopFunder()
		{
			DataTable dt = null;
			CGblIdGenerator oGbl = null;
			try
			{
				ddlSrcFund.Items.Clear();
				string vBrCode = Session[gblValue.BrnchCode].ToString();
				oGbl = new CGblIdGenerator();
				dt = oGbl.PopComboMIS("N", "N", "AA", "FundSourceId", "FundSource", "FundSourceMst", "0", "AA", "AA", System.DateTime.Now, "0000");
				ddlSrcFund.DataSource = dt;
				ddlSrcFund.DataTextField = "FundSource";
				ddlSrcFund.DataValueField = "FundSourceId";
				ddlSrcFund.DataBind();
				ListItem oLi = new ListItem("<--Select-->", "-1");
				ddlSrcFund.Items.Insert(0, oLi);
			}
			finally
			{
				dt = null;
				oGbl = null;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void PopLoanType()
		{
			DataTable dt = null;
			CGblIdGenerator oGbl = null;
			try
			{
				ddlLoanType.Items.Clear();
				string vBrCode = Session[gblValue.BrnchCode].ToString();
				oGbl = new CGblIdGenerator();
				dt = oGbl.PopComboMIS("N", "N", "AA", "LoanTypeID", "LoanType", "LoanTypeMst", "0", "AA", "AA", System.DateTime.Now, "0000");
				ddlLoanType.DataSource = dt;
				ddlLoanType.DataTextField = "LoanType";
				ddlLoanType.DataValueField = "LoanTypeId";
				ddlLoanType.DataBind();
				ListItem oLi = new ListItem("<--Select-->", "-1");
				ddlLoanType.Items.Insert(0, oLi);
			}
			finally
			{
				dt = null;
				oGbl = null;
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
				this.PageHeading = "Loan Disbursement";
				this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
				this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
				this.GetModuleByRole(mnuID.mnuLoanDisbursement);
				if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
				//if (this.UserID == 1) return;
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
					Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Disbursement(Group-Leading)", false);
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
		protected void btbSchedul_Click(object sender, EventArgs e)
		{
            if (ValidaeField("Edit") == false)
				return;
			Int32 vCollDay = 0, vCollDayNo = 0;
			CCollectionRoutine oCR = null;
			DataTable dt = null;
			if (ddlAppNo.SelectedIndex <= 0)
			{
				gblFuction.AjxMsgPopup("Please Select the application No...");
				return;
			}
			Int32 vLoanTypeID = Convert.ToInt32(ddlLoanType.SelectedValue);
			double vLoanAmt = Convert.ToDouble(txtLnAmt.Text);
			double vInstRate = Convert.ToDouble(txtIntRate.Text);
			double vInstSize = Convert.ToDouble(txtInstSize.Text);
			Int32 vInstallNo = Convert.ToInt32(txtInstNo.Text);
			Int32 vInstPeriod = Convert.ToInt32(txtIntPeriod.Text);
			DateTime vStartDt = gblFuction.setDate(txtStDt.Text);
            DateTime vLoanDt = gblFuction.setDate(txtLnDt.Text);
			string vBrCode = Session[gblValue.BrnchCode].ToString();
			string vPaySchedule = Convert.ToString(ddlRpSchdle.SelectedValue);
			string vGroupId = ddlGrp.SelectedValue;
			Int32 vFrDueday = Convert.ToInt32((gblFuction.setDate(txtStDt.Text) - gblFuction.setDate(txtLnDt.Text)).TotalDays);
			oCR = new CCollectionRoutine();
			dt = oCR.GetCollDay(vGroupId);
			if (dt.Rows.Count > 0)
			{
				vCollDay = Convert.ToInt32(dt.Rows[0]["CollDay"]);
				vCollDayNo = Convert.ToInt32(dt.Rows[0]["CollSchedule"]);
			}
            if ((int)vStartDt.DayOfWeek == vCollDay+1)
            {
                gblFuction.AjxMsgPopup("Please Select a Start date of same day as collection day of this group");
                return;
            }

			// L is for Only generate Not for Saving
            GetSchedule(vGroupId, vLoanTypeID, "", "N", "L", vLoanAmt, vStartDt, vInstSize, vInstallNo, vPaySchedule, vInstRate, vBrCode, vLoanDt);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pLoanTypeID"></param>
		/// <param name="pLoanAmt"></param>
		/// <param name="pInterest"></param>
		/// <param name="pInstallNo"></param>
		/// <param name="pInstPeriod"></param>
		/// <param name="pStatrDt"></param>
		/// <param name="pType"></param>
		/// <param name="pLoanID"></param>
		/// <param name="pIsDisburse"></param>
		/// <param name="pBranch"></param>
		/// <param name="pPaySchedule"></param>
		/// <param name="pBank"></param>
		/// <param name="pChequeNo"></param>
		/// <param name="pCollDay"></param>
		/// <param name="pCollDayNo"></param>
		/// <param name="pLoanType"></param>
		/// <param name="pFrDueday"></param>
		/// <param name="pPEType"></param>
		//private void GetSchedule(Int32 pLoanTypeID, decimal pLoanAmt, decimal pInterest, Int32 pInstallNo, Int32 pInstPeriod,
		//         DateTime pStatrDt, string pType, string pLoanID, string pIsDisburse, string pBranch, string pPaySchedule,
		//         string pBank, string pChequeNo, Int32 pCollDay, Int32 pCollDayNo, string pLoanType, Int32 pFrDueday, string pPEType)
		//{
		//    DataTable dt = null;
		//    CDisburse oLD = null;
		//    double VLoanInt = 0.0;
		//    oLD = new CDisburse();
		//    dt = oLD.GetSchedule(pLoanTypeID, pLoanAmt, pInterest, pInstallNo, pInstPeriod, pStatrDt, pType, pLoanID, pIsDisburse, pBranch, pPaySchedule, pBank, pChequeNo, pCollDay, pCollDayNo, pLoanType, pFrDueday, pPEType);
		//    if (dt.Rows.Count > 0)
		//    {
		//        gvSchdl.DataSource = dt;
		//        gvSchdl.DataBind();
		//        //foreach (DataRow dr in dt.Rows)
		//        //{
		//        //    VLoanInt = VLoanInt +  0 ;
		//        //}
		//        VLoanInt = Convert.ToDouble(dt.Compute("Sum(InstAmt)", ""));
		//        txtIntAmt.Text = VLoanInt.ToString();


		//    }

		//    tabLoanDisb.ActiveTabIndex = 2;
		//    //tabLoanDisb.Tabs[0].Enabled = false;
		//    //tabLoanDisb.Tabs[1].Enabled = false;
		//    //tabLoanDisb.Tabs[2].Enabled = true;
		//}

		private void GetSchedule(string pMarketID, Int32 pLoanTypeID, string pLoanID, string pIsDisburse, string pType, double pLoanAmt,
								DateTime pStatrDt, double pInstallment, Int32 pInstallNo, string pPaySchedule, double pIntRate, string pBranch,DateTime pLoanDt)
		{
			DataTable dt = null;
			CDisburse oLD = null;
			double VLoanInt = 0.0;
			oLD = new CDisburse();
            dt = oLD.GetSchedule(pMarketID, pLoanTypeID, pLoanID, pIsDisburse, pType, pLoanAmt, pStatrDt, pInstallment, pInstallNo, pPaySchedule, pIntRate, pBranch, pLoanDt);
			ViewState["Schedule"] = dt;
			if (dt.Rows.Count > 0)
			{
				gvSchdl.DataSource = dt;
				gvSchdl.DataBind();
				//foreach (DataRow dr in dt.Rows)
				//{
				//    VLoanInt = VLoanInt +  0 ;
				//}
				VLoanInt = Convert.ToDouble(dt.Compute("Sum(InstAmt)", ""));
				txtIntAmt.Text = VLoanInt.ToString();


			}

			tabLoanDisb.ActiveTabIndex = 2;
			//tabLoanDisb.Tabs[0].Enabled = false;
			//tabLoanDisb.Tabs[1].Enabled = false;
			//tabLoanDisb.Tabs[2].Enabled = true;
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
				tabLoanDisb.ActiveTabIndex = 1;
				//tabLoanDisb.Tabs[0].Enabled = false;
				//tabLoanDisb.Tabs[1].Enabled = true;
				//tabLoanDisb.Tabs[2].Enabled = false;
				StatusButton("Add");
				ClearControls();
				txtLnDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
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
			tabLoanDisb.ActiveTabIndex = 0;
			//tabLoanDisb.Tabs[0].Enabled = true;
			//tabLoanDisb.Tabs[1].Enabled = false;
			//tabLoanDisb.Tabs[2].Enabled = false;
			EnableControl(false);
			StatusButton("View");
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
					//LoadGrid(1);
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
		/// <returns></returns>
		private bool ValidaeField(string vMode)
		{
			bool vRes = true;
			DateTime vLnDt, vLogDt;
			string vMsg = "";
			CDisburse oLD = null;
		   
			vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vUserId = Session[gblValue.UserId].ToString();
            Int32 vUsrId = Convert.ToInt32(vUserId);
            string vLnCycle = txtLnCycle.Text;
            Int32 vLoanCycle = Convert.ToInt32(vLnCycle);
		   
			if (ddlCo.SelectedIndex <= 0)
			{
				gblFuction.MsgPopup("CO Cannot be Left Blank ..");
				gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlCo");
				return vRes = false;
			}

			if (ddlCenter.SelectedIndex <= 0)
			{
				gblFuction.MsgPopup("Center Cannot be Left Blank ..");
				gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlCenter");
				return vRes = false;
			}

			if (ddlGrp.SelectedIndex <= 0)
			{
				gblFuction.MsgPopup("Group Cannot be Left Blank ..");
				gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlGrp");
				return vRes = false;
			}

			if (ddlMembr.SelectedIndex <= 0)
			{
				gblFuction.MsgPopup("Member Cannot be Left Blank ..");
				gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlMembr");
				return vRes = false;
			}


			if (ddlAppNo.SelectedIndex <= 0)
			{
				gblFuction.MsgPopup("Application No Cannot be Left Blank ..");
				gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlAppNo");
				return vRes = false;
			}

			if (gblFuction.IsDate(txtLnDt.Text.Trim()) == false)
			{
				gblFuction.MsgPopup("Loan Date Should be in DD/MM/YYYY Format ..");
				gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_txtLnDt");
				return vRes = false;
			}
			 vLnDt = gblFuction.setDate(txtLnDt.Text);
			 if (vLnDt > vLogDt)
			 {
				 gblFuction.MsgPopup("Loan Date Can not be Greater than Login Date ..");
				 return vRes = false;
			 }
			if (gblFuction.IsDate(txtStDt.Text.Trim()) == false)
			{
				gblFuction.MsgPopup("Loan Start Date Should be in DD/MM/YYYY Format ..");
				gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_txtStDt");
				return vRes = false;
			}

			if (ddlSrcFund.SelectedIndex <= 0)
			{
				gblFuction.MsgPopup("Source of Fund Cannot be Left Blank ..");
				gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlSrcFund");
				return vRes = false;
			}

			if (ddlPurps.SelectedIndex <= 0)
			{
				gblFuction.MsgPopup("Loan Purpose Cannot be Left Blank ..");
				gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlPurps");
				return vRes = false;
			}

			if (ddlSubPur.SelectedIndex <= 0)
			{
				gblFuction.MsgPopup("Loan Sub Purpose Cannot be Left Blank ..");
				gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlSubPur");
				return vRes = false;
			}
            if (ddlIcMst.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Please Select Insurance Company ..");
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlIcMst");
                return vRes = false;
            }

			if (rdbPayMode.SelectedValue == "B")
			{
				if (txtChqNo.Text == "" || txtChqNo.Text == "0" || txtChqNo.Text.Trim().Length < 6)
				{
					gblFuction.MsgPopup("Cheque No Should be Six Digit No ..");
					gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_txtChqNo");
					return vRes = false;
				}
				if (gblFuction.IsDate(txtChqDt.Text.Trim()) == false)
				{
					gblFuction.MsgPopup("Cheque Date Should be in DD/MM/YYYY Format ..");
					gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_txtChqDt");
					return vRes = false;
				}
				if (ddlBank.SelectedIndex <= 0)
				{
					gblFuction.MsgPopup("Bank Cannot be Left Blank ..");
					gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlBank");
					return vRes = false;
				}
			}

            if (ddlRpSchdle.SelectedItem.Text.ToString() == "WEEKLY")
            {
                DateTime Loandt = gblFuction.setDate(txtLnDt.Text);
                DateTime Startdt = gblFuction.setDate(txtStDt.Text);
                Int32 dateDiff = Convert.ToInt32((Startdt - Loandt).TotalDays);
                if (dateDiff < 7 && dateDiff > 13)
                {
                    gblFuction.AjxMsgPopup("The Date Difference between Loan Date and Start Date must be into 7 to 13 Days");
                    return vRes = false;
                }
            }

            else if (ddlRpSchdle.SelectedItem.Text.ToString() == "Bi-Weekly")
            {
                DateTime Loandt = gblFuction.setDate(txtLnDt.Text);
                DateTime Startdt = gblFuction.setDate(txtStDt.Text);
                Int32 dateDiff = Convert.ToInt32((Startdt - Loandt).TotalDays);
                if (dateDiff < 14)
                {
                    gblFuction.AjxMsgPopup("The Date Difference between Loan Date and Start Date must be Greater Than 14 Days");
                    return vRes = false;
                }
            }

			oLD = new CDisburse();

            if (rdbPayMode.SelectedValue == "F")
            {
                if (vMode == "Save")
                {
                    if (this.RoleId != 1)
                    {
                        vMsg = oLD.ChkServerTime();
                        if (vMsg != "")
                        {
                            gblFuction.MsgPopup(vMsg);
                            return vRes = false;
                        }
                    }
                }
                else if (vMode == "Edit")
                {
                    if (this.RoleId != 1 && this.RoleId != 3)
                    {
                        vMsg = oLD.ChkServerTime();
                        if (vMsg != "")
                        {
                            gblFuction.MsgPopup(vMsg);
                            return vRes = false;
                        }
                    }
                }
                else if (vMode == "Delete")
                {
                    if (this.RoleId != 1)
                    {
                        vMsg = oLD.ChkServerTime();
                        if (vMsg != "")
                        {
                            gblFuction.MsgPopup(vMsg);
                            return vRes = false;
                        }
                    }
                }
            }
            vMsg = "";
            vMsg = oLD.chkMembersInfo(ddlAppNo.SelectedValue, "", "LD", vUsrId, vLoanCycle);
			if (vMsg != "")
			{
				gblFuction.MsgPopup(vMsg);
				return vRes = false;
			}
			//if (gvSchdl.Rows.Count <= 0)
			//{
			//    gblFuction.MsgPopup("Please Click on Generate Schedule Button");
			//    return vRes = false;
			//}

			return vRes;
		}


		private Boolean SaveRecords(string Mode)  //Check Account
		{
            Boolean vResult = false;
			Double vAmt = 0.0;
			Double vNPSAmt = 0.0;
			CVoucher oBJV = null;
			oBJV = new CVoucher();
			string vBrCode = Session[gblValue.BrnchCode].ToString();

			btbSchedul_Click(this, EventArgs.Empty);
            if (ValidaeField(Mode) == false)
				return false;
			if (gvSchdl.Rows.Count <= 0)
			{
				gblFuction.MsgPopup("Please Click on Generate Schedule Button");
				return false;
			}

			

			DataTable dt = null;
			string vRschedule = "", vMarketID="", vEoID = "", vMemberID = "", vLoanAppID = "", vLoanId = "", vChqNo = "", vChqDt = "", vLedgerAc = "", vLoanNo = "", vBankID="", vMedYN="N", vAccNo = "";
			Int32  vCGTID=0,vDose = 0, vInstNo = 0, vLoanTypeID = 0, vPurposeID = 0, vSubPurposeID = 0, vFunderID = 0, vErr = 0, vCashOrChq = 1, vCollDay = 0, vCollDayNo = 0,vErr1=0;
            double vLoanAmt = 0, vIntRate = 0, vIntAmt = 0, vPeriod = 0, vProsPer = 0, vProsAmt = 0, vSrvTaxPer = 0, vSrvTaxAmt = 0, vOthFeesPer = 0, vOthFeesAmt = 0,vPAIPer=0,vPAIAmt=0,vMedAmt=0,
            vInsuAmt = 0, vInstallAmt = 0;
			vCGTID = Convert.ToInt32(ViewState["CGTID"]);
			string vXmlAC = string.Empty, vXmlFees = string.Empty, vXmlNEFT=string.Empty;
            string vNarationL = string.Empty, vMsg="";
			string vNarationF = string.Empty;
            string vPAIapli = string.Empty;
			string vPDCBank = "", vPDCBranch = "", vPDCChequeNo = "";
			string vTblMst = Session[gblValue.ACVouMst].ToString();
			string vTblDtl = Session[gblValue.ACVouDtl].ToString();
			string vFinYear = Session[gblValue.ShortYear].ToString();
            int vIcMst = 0;
            if (ddlIcMst.SelectedIndex > 0)
            {
                vIcMst=Convert.ToInt32(ddlIcMst.SelectedValue);
            }
			CDisburse oLD = null;
			CCollectionRoutine oCR = null;
			CApplication oCG = null;
			string Msg = "";
			vEoID = ddlCo.SelectedValue;
			vMarketID = ddlCenter.SelectedValue;
			vMemberID = ddlMembr.SelectedValue;
			vLoanAppID = ddlAppNo.SelectedValue;
			vRschedule = Convert.ToString(ddlRpSchdle.SelectedValue);
			//vInstType = Convert.ToString(ddlInstType.SelectedValue);
			vLoanTypeID = Convert.ToInt32(ddlLoanType.SelectedValue);
			vPurposeID = Convert.ToInt32(ddlPurps.SelectedValue);
			vSubPurposeID = Convert.ToInt32(ddlSubPur.SelectedValue);
			vFunderID = Convert.ToInt32(ddlSrcFund.SelectedValue);
            vAccNo = txtAccNo.Text;
            if (rdbPayMode.SelectedValue != "F")
            {
                vBankID = ddlBank.SelectedValue;
            }
            else
            {
                vBankID = ddlBankHo.SelectedValue;
            }
            vPAIapli = ddlPAIAppli.SelectedValue;

			DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
			DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());

			if (gblFuction.setDate(txtLnDt.Text) < vFinFromDt || gblFuction.setDate(txtLnDt.Text) > vFinToDt)
			{
				gblFuction.AjxMsgPopup("Loan Disbursement Date should be in financial year.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtLnDt");
				return false;
			}

			if (txtLnAmt.Text != "") vLoanAmt = Convert.ToDouble(txtLnAmt.Text);
			if (txtIntRate.Text != "") vIntRate = Convert.ToDouble(txtIntRate.Text);
			if (txtLnCycle.Text != "") vDose = Convert.ToInt32(txtLnCycle.Text);
			if (txtIntAmt.Text != "") vIntAmt = Convert.ToDouble(txtIntAmt.Text);
			if (txtIntPeriod.Text != "") vPeriod = Convert.ToDouble(txtIntPeriod.Text);
			if (txtInstNo.Text != "") vInstNo = Convert.ToInt32(txtInstNo.Text);
			if (txtInstSize.Text != "") vInstallAmt = Convert.ToDouble(txtInstSize.Text);
			if (txtProsFee.Text != "") vProsPer = Convert.ToDouble(txtProsFee.Text);
			if (txtProsFeeAmt.Text != "") vProsAmt = Convert.ToDouble(txtProsFeeAmt.Text);
			if (txtSrvTax.Text != "") vSrvTaxPer = Convert.ToDouble(txtSrvTax.Text);
            if (txtSrvTaxAmt.Text != "") vSrvTaxAmt = Convert.ToDouble(txtSrvTaxAmt.Text);
            if (txtOthFees.Text != "") vOthFeesPer = Convert.ToDouble(txtOthFees.Text);
            if (txtOthFeesAmt.Text != "") vOthFeesAmt = Convert.ToDouble(txtOthFeesAmt.Text);
			if (txtInsurAmt.Text != "") vInsuAmt = Convert.ToDouble(txtInsurAmt.Text);
            if (txtPAI.Text != "") vPAIPer = Convert.ToDouble(txtPAI.Text);
            if (txtPAIAmt.Text != "") vPAIAmt = Convert.ToDouble(Request[txtPAIAmt.UniqueID] as string);
            hdPAIAmt.Value =txtPAI.Text;
            if (chkMedApp.Checked == true)
            {
                vMedYN = "Y";
                vMedAmt = Convert.ToDouble(txtMedAmt.Text);
            }
            else
            {
                vMedYN = "N";
                vMedAmt = Convert.ToDouble(txtMedAmt.Text);
            }

			string vLoanAc = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.PrincipalLoanAc, vLoanTypeID, vBrCode);
			string vProcFeecAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.ProcfeesAC, vLoanTypeID, vBrCode);
			string vIncAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.InsuAC, vLoanTypeID, vBrCode);
            string vServiceAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.ServiceAC, vLoanTypeID, vBrCode);
            string vOthersFeesAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.OthersFeesAC, vLoanTypeID, vBrCode);
            string vPAIAmtAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.PAIAmountAC, vLoanTypeID, vBrCode);
            string vDisbLedger = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.DisbLedger, vLoanTypeID, vBrCode);
            string vMedCLAc = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.MedClaim, vLoanTypeID, vBrCode);
            
            if (Mode == "Save")
            {
                if (vDisbLedger != "-1")
                {
                    vAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                            , gblFuction.setDate(txtLnDt.Text), vBrCode, Convert.ToInt32(Session[gblValue.FinYrNo]), vDisbLedger);
                }
                else
                {
                    if (rdbPayMode.SelectedValue == "C")
                    {
                        vAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                                , gblFuction.setDate(txtLnDt.Text), vBrCode, Convert.ToInt32(Session[gblValue.FinYrNo]), "C0001");
                        vNPSAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                                , gblFuction.setDate(txtLnDt.Text), vBrCode, Convert.ToInt32(Session[gblValue.FinYrNo]), "N0001");
                        vAmt = vAmt + vNPSAmt;
                    }
                    else if (rdbPayMode.SelectedValue == "B")
                    {
                        vAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                                , gblFuction.setDate(txtLnDt.Text), vBrCode, Convert.ToInt32(Session[gblValue.FinYrNo]), ddlBank.SelectedValue);
                    }
                }
            }
            else if (Mode == "Edit")
            {
                if (vDisbLedger != "-1")
                {
                    vAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                            , gblFuction.setDate(txtLnDt.Text), vBrCode, Convert.ToInt32(Session[gblValue.FinYrNo]), vDisbLedger);
                    vAmt = vAmt + vLoanAmt;
                }
                else
                {
                    if (rdbPayMode.SelectedValue == "C")
                    {
                        vAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                                , gblFuction.setDate(txtLnDt.Text), vBrCode, Convert.ToInt32(Session[gblValue.FinYrNo]), "C0001");
                        vNPSAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                                , gblFuction.setDate(txtLnDt.Text), vBrCode, Convert.ToInt32(Session[gblValue.FinYrNo]), "N0001");
                        vAmt = vAmt + vNPSAmt + vLoanAmt;
                    }
                    else if (rdbPayMode.SelectedValue == "B")
                    {
                        vAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                                , gblFuction.setDate(txtLnDt.Text), vBrCode, Convert.ToInt32(Session[gblValue.FinYrNo]), ddlBank.SelectedValue);
                        vAmt = vAmt + vLoanAmt;
                    }
                }
            }

            //if (Mode != "Delete")
            //{
            //    if (vDisbLedger == "-1")
            //    {
            //        if (Math.Round(Convert.ToDouble(txtLnAmt.Text), 2) > vAmt)
            //        {
            //            gblFuction.AjxMsgPopup("Insufficient Balance to Disburse Loan...");
            //            return false;
            //        }
            //    }
            //}
            

            if (vDisbLedger != "-1")
            {
                vCashOrChq = 3;
                vChqNo = "";
                vChqDt = "";
                vLedgerAc = vDisbLedger;
            }
            else if (rdbPayMode.SelectedValue == "C")
            {
                vCashOrChq = 1;
                vChqNo = "";
                vChqDt = "";
                vLedgerAc = "C0001";
            }
            else if (rdbPayMode.SelectedValue == "B")
            {
                vCashOrChq = 2;
                vChqNo = txtChqNo.Text;
                vChqDt = txtChqDt.Text;
                vLedgerAc = Convert.ToString(ddlBank.SelectedValue);
            }
            else if (rdbPayMode.SelectedValue == "F")
            {
                vCashOrChq = 4;
                //vChqNo = txtNeftNo.Text;
                //vChqDt = txtNEFTDt.Text;
                vLedgerAc = CGblIdGenerator.GetImprestAc(vBrCode, vBrCode + " IMPREST");
            }
			oLD = new CDisburse();
			Double vClsBal = oLD.GetClosingBal(gblFuction.setDate(Session[gblValue.FinFromDt].ToString()), gblFuction.setDate(txtLnDt.Text), vBrCode, Convert.ToInt32(Session[gblValue.FinYrNo].ToString()), vLedgerAc);
			if (Mode == "Save")
			{
                if (rdbPayMode.SelectedValue != "F")
                {
                    if (vClsBal < vLoanAmt)
                    {
                        gblFuction.AjxMsgPopup("Not Enough Balance to Disburse..");
                        return false;
                    }
                }
			}
			//------Create Account table----
			DataTable dtAccount = new DataTable();
			DataRow dr;
			DataColumn dc = new DataColumn();
			dc.ColumnName = "DC";
			dtAccount.Columns.Add(dc);

			DataColumn dc1 = new DataColumn();
			dc1.ColumnName = "Amt";
			dc1.DataType = System.Type.GetType("System.Decimal");
			dtAccount.Columns.Add(dc1);

			DataColumn dc2 = new DataColumn();
			dc2.ColumnName = "DescId";
			dtAccount.Columns.Add(dc2);

			DataColumn dc3 = new DataColumn();
			dc3.ColumnName = "DtlId";
			dtAccount.Columns.Add(dc3);

			dtAccount.TableName = "Table1";
            if (Convert.ToDecimal(txtLnAmt.Text) > 0)
            {
                dr = dtAccount.NewRow();
                dr["DescId"] = vLoanAc;
                dr["DC"] = "D";
                dr["Amt"] = Convert.ToDecimal(txtLnAmt.Text);
                dr["DtlId"] = 1;
                dtAccount.Rows.Add(dr);
                dtAccount.AcceptChanges();

                dr = dtAccount.NewRow();
                dr["DescId"] = vLedgerAc;
                dr["DC"] = "C";
                dr["Amt"] = Convert.ToDecimal(txtLnAmt.Text);
                dr["DtlId"] = 2;
                dtAccount.Rows.Add(dr);
                dtAccount.AcceptChanges();
            }
            DataTable dtNEFT = new DataTable();
            DataRow dr2;
            DataColumn dc8 = new DataColumn();
            dc8.ColumnName = "DC";
            dtNEFT.Columns.Add(dc8);

            DataColumn dc9 = new DataColumn();
            dc9.ColumnName = "Amt";
            dc9.DataType = System.Type.GetType("System.Decimal");
            dtNEFT.Columns.Add(dc9);

            DataColumn dc10 = new DataColumn();
            dc10.ColumnName = "DescId";
            dtNEFT.Columns.Add(dc10);

            DataColumn dc11 = new DataColumn();
            dc11.ColumnName = "DtlId";
            dtNEFT.Columns.Add(dc11);

            dtNEFT.TableName = "Table1";
            if (Convert.ToDecimal(txtLnAmt.Text) > 0 && rdbPayMode.SelectedValue == "F")
            {
                dr2 = dtNEFT.NewRow();
                dr2["DescId"] = vLedgerAc;
                dr2["DC"] = "D";
                dr2["Amt"] = Convert.ToDecimal(txtLnAmt.Text);
                dr2["DtlId"] = 1;
                dtNEFT.Rows.Add(dr2);
                dtNEFT.AcceptChanges();

                dr2 = dtNEFT.NewRow();
                dr2["DescId"] = ddlBankHo.SelectedValue;
                dr2["DC"] = "C";
                dr2["Amt"] = Convert.ToDecimal(txtLnAmt.Text);
                dr2["DtlId"] = 2;
                dtNEFT.Rows.Add(dr2);
                dtNEFT.AcceptChanges();
            }
			
			//-------------Fees--------------
			DataTable dtfees = new DataTable();
			DataRow dr1;
			DataColumn dc4 = new DataColumn();
			dc4.ColumnName = "DC";
			dtfees.Columns.Add(dc4);

			DataColumn dc5 = new DataColumn();
			dc5.ColumnName = "Amt";
			dc5.DataType = System.Type.GetType("System.Decimal");
			dtfees.Columns.Add(dc5);

			DataColumn dc6 = new DataColumn();
			dc6.ColumnName = "DescId";
			dtfees.Columns.Add(dc6);

			DataColumn dc7 = new DataColumn();
			dc7.ColumnName = "DtlId";
			dtfees.Columns.Add(dc7);
			dtfees.TableName = "Table1";

            if (Convert.ToDecimal(txtProsFeeAmt.Text) + Convert.ToDecimal(txtInsurAmt.Text) + Convert.ToDecimal(txtSrvTaxAmt.Text) + Convert.ToDecimal(txtOthFeesAmt.Text) + Convert.ToDecimal(txtPAIAmt.Text) + Convert.ToDecimal(txtMedAmt.Text) > 0)
			{
				Int32 i = 1;
				dr1 = dtfees.NewRow();
				dr1["DescId"] = "C0001";
				dr1["DC"] = "D";
                dr1["Amt"] = Convert.ToDecimal(txtProsFeeAmt.Text) + Convert.ToDecimal(txtInsurAmt.Text) + Convert.ToDecimal(txtSrvTaxAmt.Text) + Convert.ToDecimal(txtOthFeesAmt.Text) + Convert.ToDecimal(txtPAIAmt.Text) + Convert.ToDecimal(txtMedAmt.Text);
				dr1["DtlId"] = i;
				dtfees.Rows.Add(dr1);
				dtfees.AcceptChanges();
				i = i + 1;
				if (Convert.ToDecimal(txtProsFee.Text) > 0)
				{
					dr1 = dtfees.NewRow();
					dr1["DescId"] = vProcFeecAC;
					dr1["DC"] = "C";
					dr1["Amt"] = Convert.ToDecimal(txtProsFeeAmt.Text);
					dr1["DtlId"] = i;
					dtfees.Rows.Add(dr1);
					dtfees.AcceptChanges();
					i = i + 1;
				}
				if (Convert.ToDecimal(txtInsurAmt.Text) > 0)
				{
					dr1 = dtfees.NewRow();
					dr1["DescId"] = vIncAC;
					dr1["DC"] = "C";
					dr1["Amt"] = Convert.ToDecimal(txtInsurAmt.Text);
					dr1["DtlId"] = i;
					dtfees.Rows.Add(dr1);
					dtfees.AcceptChanges();
					i = i + 1;
				}
				if (Convert.ToDecimal(txtSrvTaxAmt.Text) > 0)
				{
					dr1 = dtfees.NewRow();
					dr1["DescId"] = vServiceAC;
					dr1["DC"] = "C";
					dr1["Amt"] = Convert.ToDecimal(txtSrvTax.Text);
					dr1["DtlId"] = i;
					dtfees.Rows.Add(dr1);
					dtfees.AcceptChanges();
					i = i + 1;
                }
                if (Convert.ToDecimal(txtOthFeesAmt.Text) > 0)
                {
                    dr1 = dtfees.NewRow();
                    dr1["DescId"] = vOthersFeesAC;
                    dr1["DC"] = "C";
                    dr1["Amt"] = Convert.ToDecimal(txtOthFees.Text);
                    dr1["DtlId"] = i;
                    dtfees.Rows.Add(dr1);
                    dtfees.AcceptChanges();
                    i = i + 1;
                }
                if (Convert.ToDecimal(txtPAIAmt.Text) > 0)
                {
                    dr1 = dtfees.NewRow();
                    dr1["DescId"] = vPAIAmtAC;
                    dr1["DC"] = "C";
                    dr1["Amt"] = Convert.ToDecimal(txtPAI.Text);
                    dr1["DtlId"] = i;
                    dtfees.Rows.Add(dr1);
                    dtfees.AcceptChanges();
                    i = i + 1;
                }
                if (Convert.ToDecimal(txtMedAmt.Text) > 0)
                {
                    dr1 = dtfees.NewRow();
                    dr1["DescId"] = vMedCLAc;
                    dr1["DC"] = "C";
                    dr1["Amt"] = Convert.ToDecimal(txtMedAmt.Text);
                    dr1["DtlId"] = i;
                    dtfees.Rows.Add(dr1);
                    dtfees.AcceptChanges();
                    i = i + 1;
                }
			}
			oCR = new CCollectionRoutine();
			dt = oCR.GetCollDay(ddlGrp.SelectedValue);
			if (dt.Rows.Count > 0)
			{
				vCollDay = Convert.ToInt32(dt.Rows[0]["CollDay"]);
				vCollDayNo = Convert.ToInt32(dt.Rows[0]["CollSchedule"]);
			}
            if ((int)gblFuction.setDate(txtStDt.Text).DayOfWeek == vCollDay + 1)
            {
                gblFuction.AjxMsgPopup("Please Select a Start date of same day as collection day of this group");
                return false;
            }
            if (rdbPayMode.SelectedValue != "F")
            {
                vNarationL = "Being the Amt of Loan Disbursed for " + ddlMembr.SelectedItem;
                vNarationF = "Being the Amt of Fees For Loan Disbursed for " + ddlMembr.SelectedItem;
            }
            else
            {
                vNarationL = "Being the Amt of NEFT Loan Disbursed for " + ddlMembr.SelectedItem;
                vNarationF = "Being the Amt of NEFT Fees For Loan Disbursed for " + ddlMembr.SelectedItem;
            }
			vXmlAC = DataTableTOXml(dtAccount);
			vXmlFees = DataTableTOXml(dtfees);
            vXmlNEFT = DataTableTOXml(dtNEFT);
			oLD = new CDisburse();
            
            
			if (Mode == "Save")
            {
                if (this.RoleId != 1)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtLnDt.Text))
                        {
                            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                            return false;
                        }
                    }
                }
                oCG = new CApplication();
                vMsg = oCG.ChkLoanOther(vMemberID, vLoanTypeID, "D");
                if (vMsg != "")
                {
                    gblFuction.MsgPopup(vMsg);
                    return false;
                }
                
				vErr1 = oLD.chkLoanDisburseNo(vBrCode, gblFuction.setDate(txtLnDt.Text));
				if (vErr1 > 0)
				{
					gblFuction.MsgPopup("Invalid no of Loan Disbursement");
					return false;
				}

				
				vErr = oCG.ChkMemberOpenLoan(vMemberID, gblFuction.setDate(txtLnDt.Text), vLoanAppID);
				if (vErr > 0)
				{
					gblFuction.MsgPopup("Loan Exists. Can not save");
					return false;
				}
                if (rdbPayMode.SelectedValue != "F")
                {
                    Msg = oLD.chkCashOrBankBalBfrSave(vBrCode, gblFuction.setDate(txtLnDt.Text),
                                  Convert.ToDouble((txtLnAmt.Text == "" || txtLnAmt.Text == ".") ? "0" : txtLnAmt.Text.Trim()),
                                  gblFuction.setDate(Session[gblValue.FinFromDt].ToString()),
                                  Convert.ToInt32(Session[gblValue.FinYrNo].ToString()), rdbPayMode.SelectedValue.ToString());
                }
				if (Msg != "")
				{
					gblFuction.AjxMsgPopup(Msg);
					return false;
				}
                
                dt = oLD.ValidateAccNoForDisburse(Convert.ToInt32(ddlLoanType.SelectedValue));
                if (dt.Rows.Count == 0)
                {
                    if (txtAccNo.Text != "")
                    {
                        gblFuction.MsgPopup("Account Number should left blank..");
                        txtAccNo.Text = null;
                        return false;
                    }
                }

				vErr = oLD.InsertLoanMst(ref vLoanNo,vMarketID, vMemberID, gblFuction.setDate(txtLnDt.Text), vIntAmt, vPeriod, vIntRate, vLoanAmt, vProsPer, 
							vProsAmt, vSrvTaxPer, vSrvTaxAmt, vCashOrChq, vChqNo, gblFuction.setDate(vChqDt), vBankID, vLoanTypeID, "O", 0, 
							vRschedule, 0, "", vPurposeID, vSubPurposeID, vInstNo, vFunderID, vDose, gblFuction.setDate(txtLnDt.Text), vLoanAmt, 
							vIntAmt, gblFuction.setDate(txtStDt.Text), vInstallAmt, vInsuAmt, vLoanAppID, vEoID, vCGTID, vBrCode, vXmlAC, vXmlFees, vXmlNEFT,
                            vTblMst, vTblDtl, vFinYear, Convert.ToInt32(Session[gblValue.UserId].ToString()), vNarationL, vNarationF, vPDCBank, vPDCBranch, vPDCChequeNo, vCollDay, vCollDayNo, ""
                            , vOthFeesAmt, vPAIAmt, vPAIapli, vIcMst, vMedYN, vMedAmt, vAccNo);
				if (vErr == 0)
				{
					txtLnNo.Text = vLoanNo;
					gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
					vResult = true;
				}
				else
				{
					gblFuction.AjxMsgPopup(gblMarg.DBError);
					vResult = false;
				}

			}
			else if (Mode == "Edit")
			{
                oCG = new CApplication();
                vErr = oCG.ChkTopUpLoan(vMemberID, vLoanTypeID);
                if (vErr > 0)
                {
                    gblFuction.MsgPopup("Another top-up loan exists for this member. Can not save");
                    return false;
                }
				vLoanId = (string)ViewState["LoanId"];
				CCollectionRoutine oColl = new CCollectionRoutine();
				dt = oColl.GetMaxCollDate(vLoanId, vBrCode, "M");
				if (dt.Rows.Count > 0)
				{
					if (Convert.ToString(dt.Rows[0]["MaxCollDt"]) != "01/01/1900" && gblFuction.setDate(Convert.ToString(dt.Rows[0]["MaxCollDt"]).ToString()) >= gblFuction.setDate(txtLnDt.Text))
					{
						gblFuction.AjxMsgPopup("After collection you can not edit the loan..");
						return false;
					}
				}
                dt = oLD.GetIfNeft(vLoanAppID);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["HODisbYN"]) == "Y")
                    {
                        vErr = oLD.UpdateLoanMstIDBI(vLoanId, vMarketID, vMemberID, gblFuction.setDate(txtLnDt.Text), vIntAmt, vPeriod, vIntRate, vLoanAmt, vDose, vProsPer,
                            vProsAmt, vSrvTaxPer, vSrvTaxAmt, vPurposeID, vSubPurposeID, vCashOrChq, vChqNo, gblFuction.setDate(vChqDt), vBankID,
                            vLoanTypeID, vFunderID, vInstNo, vRschedule, gblFuction.setDate(txtLnDt.Text), vLoanAmt, vIntAmt,
                            gblFuction.setDate(txtStDt.Text), vInstallAmt, vInsuAmt, vLoanAppID, vEoID, vCGTID, vBrCode, vXmlAC, vXmlFees, vXmlNEFT, vTblMst, vTblDtl,
                            vFinYear, Convert.ToInt32(Session[gblValue.UserId].ToString()), vNarationL, vNarationF, vPDCBank, vPDCBranch, vPDCChequeNo, vCollDay, vCollDayNo, ""
                            , vOthFeesAmt, vPAIAmt, vPAIapli, vIcMst, vMedYN, vMedAmt, vAccNo);
                    }
                    else
                    {
                        vErr = oLD.UpdateLoanMst(vLoanId, vMarketID, vMemberID, gblFuction.setDate(txtLnDt.Text), vIntAmt, vPeriod, vIntRate, vLoanAmt, vDose, vProsPer,
                            vProsAmt, vSrvTaxPer, vSrvTaxAmt, vPurposeID, vSubPurposeID, vCashOrChq, vChqNo, gblFuction.setDate(vChqDt), vBankID,
                            vLoanTypeID, vFunderID, vInstNo, vRschedule, gblFuction.setDate(txtLnDt.Text), vLoanAmt, vIntAmt,
                            gblFuction.setDate(txtStDt.Text), vInstallAmt, vInsuAmt, vLoanAppID, vEoID, vCGTID, vBrCode, vXmlAC, vXmlFees, vXmlNEFT, vTblMst, vTblDtl,
                            vFinYear, Convert.ToInt32(Session[gblValue.UserId].ToString()), vNarationL, vNarationF, vPDCBank, vPDCBranch, vPDCChequeNo, vCollDay, vCollDayNo, ""
                            , vOthFeesAmt, vPAIAmt, vPAIapli, vIcMst, vMedYN, vMedAmt, vAccNo);
                    }
                }
                
				
				if (vErr == 0)
				{
					gblFuction.AjxMsgPopup(gblMarg.EditMsg);
					vResult = true;
				}
				else
				{
					gblFuction.AjxMsgPopup(gblMarg.DBError);
					vResult = false;
				}
			}
			else if (Mode == "Delete")
			{
                if (this.RoleId != 1)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtLnDt.Text))
                        {
                            gblFuction.AjxMsgPopup("You can not delete, Day end already done..");
                            return false;
                        }
                    }
                }

				CCollectionRoutine oColl = new CCollectionRoutine();
				vLoanId = (string)ViewState["LoanId"];
				dt = oColl.GetMaxCollDate(vLoanId, vBrCode, "M");
				if (dt.Rows.Count > 0)
				{
					if (Convert.ToString(dt.Rows[0]["MaxCollDt"]) != "01/01/1900" && gblFuction.setDate(Convert.ToString(dt.Rows[0]["MaxCollDt"]).ToString()) >= gblFuction.setDate(txtLnDt.Text))
					{
						gblFuction.AjxMsgPopup("After collection you can not delete the loan..");
						return false;
					}
				}

                dt = oLD.GetIfNeft(vLoanAppID);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["HODisbYN"]) == "Y")
                    {
                        gblFuction.AjxMsgPopup("You can not Delete NEFT loan because it already disbursed by HO..");
                        return false;
                    }
                }

                vErr = oLD.DeleteLoanMst(vLoanId, Convert.ToInt32(Session[gblValue.UserId].ToString()), vBrCode, vTblMst, vTblDtl, vCashOrChq);
				if (vErr == 0)
				{
					vResult = true;
					gblFuction.AjxMsgPopup(gblMarg.DeleteMsg);
				}
				else
				{
					gblFuction.AjxMsgPopup(gblMarg.DBError);
					vResult = false;
				}
			}
			LoadGrid(0);
			return vResult;
		}


		private bool ValidaeCanPostpond()
		{
			bool vRes = true;
			DateTime vLogDt;

			vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

			if (ddlCenter.SelectedIndex <= 0)
			{
				gblFuction.MsgPopup("Center Cannot be Left Blank ..");
				gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlCenter");
				return vRes = false;
			}

			if (ddlMembr.SelectedIndex <= 0)
			{
				gblFuction.MsgPopup("Member Cannot be Left Blank ..");
				gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlMembr");
				return vRes = false;
			}


			if (ddlAppNo.SelectedIndex <= 0)
			{
				gblFuction.MsgPopup("Application No Cannot be Left Blank ..");
				gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlAppNo");
				return vRes = false;
			}
            if(txtPostCanResn.Text=="")
            {
                gblFuction.MsgPopup("Postponed or Cancel Reason Cannot be Left Blank ..");
                gblFuction.focus("ctl00$cph_Main$tabLoanDisb$pnlDtls$txtPostCanResn");
                return vRes = false;
            }

                return vRes;
		}

		private Boolean CancelPostpond(string Mode)  //Check Account
		{
			if (ValidaeCanPostpond() == false)
				return false;


			Boolean vResult = false;
			
			string vMarketID = "", vMemberID = "", vLoanAppID = "";
			Int32 vCGTID = 0, vErr=0;
			vCGTID = Convert.ToInt32(ViewState["CGTID"]);
			string vBrCode = Session[gblValue.BrnchCode].ToString();
			CDisburse oLD = null;
            string vPostCanDt = "", vPostCanResn="";

			vMarketID = ddlCenter.SelectedValue;
			vMemberID = ddlMembr.SelectedValue;
			vLoanAppID = ddlAppNo.SelectedValue;
            vPostCanDt = (string)Session[gblValue.LoginDate];
            vPostCanResn =txtPostCanResn.Text;

			oLD = new CDisburse();
			vErr = oLD.UpdateCancelPostpond(vLoanAppID, this.UserID, Mode,gblFuction.setDate(vPostCanDt),vPostCanResn);
			if (vErr == 0)
			{
				gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
				vResult = true;
			}
			else
			{
				gblFuction.AjxMsgPopup(gblMarg.DBError);
				vResult = false;
			}

			LoadGrid(0);
			return vResult;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		private string DataTableTOXml(DataTable dt)
		{
			string sXml = "";
			using (StringWriter oSW = new StringWriter())
			{
				dt.WriteXml(oSW);
				sXml = oSW.ToString();
			}
			return sXml;
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
                if (hdPSNName.Value == "IDBI")
                {
                    txtStDt.Enabled = true;
                }

                if (this.RoleId != 1)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtLnDt.Text))
                        {
                            EnableControl(false);
                            txtAccNo.Enabled = true;
                        }
                    }
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
			if (vStateEdit == "Add" || vStateEdit == null)
				vStateEdit = "Save";

			if (SaveRecords(vStateEdit) == true)
			{
				gblFuction.MsgPopup(gblMarg.SaveMsg);
				//LoadGrid(1);
				StatusButton("View");
				ViewState["StateEdit"] = null;
			}
		}

		protected void btnCanDisb_Click(object sender, EventArgs e)
		{
			if (CancelPostpond("C") == true)
			{
				gblFuction.MsgPopup(gblMarg.SaveMsg);
				StatusButton("View");
				ViewState["StateEdit"] = null;
			}
		}

		protected void btnPostpond_Click(object sender, EventArgs e)
		{
			if (CancelPostpond("P") == true)
			{
				gblFuction.MsgPopup(gblMarg.SaveMsg);
				StatusButton("View");
				ViewState["StateEdit"] = null;
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
					btnCanDisb.Enabled = true;
					btnPostpond.Enabled = true;
					btnCancel.Enabled = true;
					btnExit.Enabled = false;
					ClearControls();
					//gblFuction.focus("ctl00_cph_Main_tabLnDisb_pnlDtl_ddlCo");
					break;
				case "Show":
					btnAdd.Enabled = true;
					btnEdit.Enabled = true;
					btnDelete.Enabled = true;
					btnSave.Enabled = false;
					btnCanDisb.Enabled = false;
					btnPostpond.Enabled = false;
					btnCancel.Enabled = false;
					btnExit.Enabled = true;
					EnableControl(false);
					break;
				case "Edit":
					btnAdd.Enabled = false;
					btnEdit.Enabled = false;
					btnDelete.Enabled = false;
					btnSave.Enabled = true;
					btnCanDisb.Enabled = false;
					btnPostpond.Enabled = false;
					btnCancel.Enabled = true;
					btnExit.Enabled = false;
					EnableControl(true);
					gblFuction.focus("ctl00_cph_Main_tabLnDisb_pnlDtl_ddlCo");
					break;
				case "View":
					btnAdd.Enabled = true;
					btnEdit.Enabled = false;
					btnDelete.Enabled = false;
					btnSave.Enabled = false;
					btnCanDisb.Enabled = false;
					btnPostpond.Enabled = false;
					btnCancel.Enabled = false;
					btnExit.Enabled = true;
					EnableControl(false);
					break;
				case "Delete":
					btnAdd.Enabled = true;
					btnEdit.Enabled = false;
					btnDelete.Enabled = false;
					btnSave.Enabled = false;
					btnCanDisb.Enabled = false;
					btnPostpond.Enabled = false;
					btnCancel.Enabled = false;
					btnExit.Enabled = true;
					EnableControl(false);
					break;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Status"></param>
		private void EnableControl(Boolean Status)
		{
			ddlDisburseBy.Enabled = Status;
			ddlCo.Enabled = Status;
			ddlGrp.Enabled = Status;
			ddlCenter.Enabled = Status;
			ddlMembr.Enabled = Status;
			ddlAppNo.Enabled = Status;
			txtLnDt.Enabled = Status;
			ddlSrcFund.Enabled = Status;
			rdbPayMode.Enabled = Status;
			txtChqNo.Enabled = Status;
			txtChqDt.Enabled = Status;
            //txtNEFTDt.Enabled = Status;
            //txtNeftNo.Enabled = Status;
			ddlBank.Enabled = Status;
			txtStDt.Enabled = Status;
            ddlPAIAppli.Enabled = Status;
            txtAccNo.Enabled = Status;
            ddlIcMst.Enabled = Status;
		}

		/// <summary>
		/// 
		/// </summary>
		private void ClearControls()
		{
			txtLnNo.Text = "";
			ddlCo.SelectedIndex = -1;
			ddlAppNo.SelectedIndex = -1;
			ddlGrp.SelectedIndex = -1;
			ddlCenter.SelectedIndex = -1;
			ddlRpSchdle.SelectedIndex = -1;
			ddlMembr.SelectedIndex = -1;
			ddlLoanType.SelectedIndex = -1;
			lblMemNm.Text = "";
			txtLnAmt.Text = "0";
			txtIntRate.Text = "0";
			txtLnCycle.Text = "0";
			txtIntAmt.Text = "0";
			txtIntPeriod.Text = "0";
			txtInstNo.Text = "0";
			txtLnDt.Text = Session[gblValue.LoginDate].ToString();
			txtStDt.Text = "";
			txtInstSize.Text = "";
			txtProsFee.Text = "0";
			txtInsurAmt.Text = "0";
			txtSrvTax.Text = "0";
            txtOthFeesAmt.Text = "0";
            txtPAIAmt.Text = "0";
			ddlSrcFund.SelectedIndex = -1;
			ddlPurps.SelectedIndex = -1;
			rdbPayMode.SelectedValue = "C";
			txtChqNo.Text = "";
			txtChqDt.Text = "";
            //txtNEFTDt.Text = "";
            //txtNeftNo.Text = "";
			ddlBank.SelectedIndex = -1;
			ddlBank.SelectedIndex = -1;
			tblBank.Visible = false;
            tblNEFT.Visible = false;
            txtspnm.Text = "";
            txtMedAmt.Text = "0";
            txtAccNo.Text = "";
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
                ddlCo.Items.Clear();
				oRO = new CEO();
				dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
				ddlCo.DataSource = dt;
				ddlCo.DataTextField = "EoName";
				ddlCo.DataValueField = "EoId";
				ddlCo.DataBind();
				ListItem oli = new ListItem("<--Select-->", "-1");
				ddlCo.Items.Insert(0, oli);

				ddlDisburseBy.DataSource = dt;
				ddlDisburseBy.DataTextField = "EoName";
				ddlDisburseBy.DataValueField = "EoId";
				ddlDisburseBy.DataBind();
				ddlDisburseBy.Items.Insert(0, oli);
			}
			finally
			{
				oRO = null;
				dt = null;
			}
		}


		private void popDisbBy()
		{
			DataTable dt = null;
			CGblIdGenerator oRO = null;
			string vBrCode = Session[gblValue.BrnchCode].ToString();
			DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
			try
			{
				oRO = new CGblIdGenerator();
				dt = oRO.PopTransferMIS("N", "EoMst", "ABM,BM,AM,SBM,AAM,ARM,RM,RO,CO", vLogDt, vBrCode);
				//dt = oRO.PopComboMIS("D", "N", "AA", "EoId", "EoName", "EoMst", vBrCode, "BranchCode", "Tra_DropDate", vLogDt, vBrCode);
				ListItem oli = new ListItem("<--Select-->", "-1");
				ddlCo.Items.Insert(0, oli);

				ddlDisburseBy.DataSource = dt;
				ddlDisburseBy.DataTextField = "EoName";
				ddlDisburseBy.DataValueField = "EoId";
				ddlDisburseBy.DataBind();
				ddlDisburseBy.Items.Insert(0, oli);
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
		protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ddlCo.SelectedIndex > 0) PopCenter(ddlCo.SelectedValue);
            ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(ddlCo.SelectedValue));
            if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vCOID"></param>
		private void PopCenter(string vCOID)
		{
			DataTable dtGr = null;
			CGblIdGenerator oGbl = null;
			try
			{
				ddlCenter.Items.Clear();
				ddlCenter.SelectedIndex = -1;
				ddlGrp.Items.Clear();
				ddlGrp.SelectedIndex = -1;
				ddlMembr.Items.Clear();
				ddlMembr.SelectedIndex = -1;
				ddlAppNo.Items.Clear();
				ddlAppNo.SelectedIndex = -1;
				lblMemNm.Text = "";
				string vBrCode = Session[gblValue.BrnchCode].ToString();
				oGbl = new CGblIdGenerator();
				dtGr = oGbl.PopComboMIS("S", "N", "", "MarketID", "Market", "MarketMst", vCOID, "EOID", "DropoutDt", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
				dtGr.AcceptChanges();
				ddlCenter.DataSource = dtGr;
				ddlCenter.DataTextField = "Market";
				ddlCenter.DataValueField = "MarketID";
				ddlCenter.DataBind();
				ListItem oLi = new ListItem("<--Select-->", "-1");
				ddlCenter.Items.Insert(0, oLi);
			}
			finally
			{
				dtGr = null;
				oGbl = null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vCenterID"></param>
		private void PopGroup(string vCenterID)
		{
			ddlGrp.Items.Clear();
			ddlGrp.SelectedIndex = -1;
			ddlMembr.Items.Clear();
			ddlMembr.SelectedIndex = -1;
			ddlAppNo.Items.Clear();
			ddlAppNo.SelectedIndex = -1;
			lblMemNm.Text = "";
			DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
			DataTable dt = null;
			CGblIdGenerator oGb = null;
			string vBrCode = "";
			Int32 vBrId = 0;
			try
			{
				vBrCode = (string)Session[gblValue.BrnchCode];
				vBrId = Convert.ToInt32(vBrCode);
				oGb = new CGblIdGenerator();
				dt = oGb.PopComboMIS("D", "N", "AA", "GroupID", "GroupName", "GroupMst", vCenterID, "MarketID", "Tra_DropDate", vLogDt, vBrCode);
				ddlGrp.DataSource = dt;
				ddlGrp.DataTextField = "GroupName";
				ddlGrp.DataValueField = "GroupID";
				ddlGrp.DataBind();
				ListItem oli = new ListItem("<--Select-->", "-1");
				ddlGrp.Items.Insert(0, oli);
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
		protected void ddlGrp_SelectedIndexChanged(object sender, EventArgs e)
		{
			ddlMembr.SelectedIndex = -1;
			PopMember(ddlGrp.SelectedValue);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="vGroupID"></param>
		private void PopMember(string vGroupID)
		{
			ddlMembr.Items.Clear();
			ddlMembr.SelectedIndex = -1;
			ddlAppNo.Items.Clear();
			ddlAppNo.SelectedIndex = -1;
			lblMemNm.Text = "";
			DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
			DataTable dt = null;
			try
			{
				CMember oMem = new CMember();
				if (Convert.ToString(ddlCo.SelectedValue) != "-1")
				{
					dt = oMem.PopGrpMember(ddlGrp.SelectedValue, Session[gblValue.BrnchCode].ToString(), vLoginDt, "M");
					ddlMembr.DataTextField = "MemberName";
					ddlMembr.DataValueField = "MemberId";
					ddlMembr.DataSource = dt;
					ddlMembr.DataBind();
					ListItem oItm = new ListItem();
					oItm.Text = "<--- Select --->";
					oItm.Value = "-1";
					ddlMembr.Items.Insert(0, oItm);
				}
				ddlMembr.Focus();
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
		protected void ddlMembr_SelectedIndexChanged(object sender, EventArgs e)
		{
			ddlAppNo.Items.Clear();
			ddlAppNo.SelectedIndex = -1;
			DataTable dt = null;
			CDisburse oLD = null;
			string vMemberId = ddlMembr.SelectedValue;
			string vBrCode = Session[gblValue.BrnchCode].ToString();
			try
			{
				oLD = new CDisburse();
				lblMemNm.Text = (ddlMembr.SelectedItem.Text).PadLeft(11);
				dt = oLD.GetLoanAppByMemberId(vMemberId, "N",gblFuction.setDate(txtLnDt.Text));
				if (dt.Rows.Count > 0)
				{
					ddlAppNo.DataTextField = "LoanAppNo";
					ddlAppNo.DataValueField = "LoanAppId";
					ddlAppNo.DataSource = dt;
					ddlAppNo.DataBind();
					ListItem oItm = new ListItem();
					oItm.Text = "<--- Select --->";
					oItm.Value = "-1";
					ddlAppNo.Items.Insert(0, oItm);
				}
				else
				{
					ddlAppNo.Items.Clear();
					ddlAppNo.Items.Add("No Matching Records.");
				}

                fillLnDtl(vMemberId, gblFuction.setDate(txtLnDt.Text));
			   
			}
			finally
			{
				dt = null;
				oLD = null;
			}
		}


        public void fillLnDtl(string Vmember, DateTime vAppDt)
        {
            DataSet ds = null;
            DataTable dt = null, dt1 = null;
            CApplication oCG = null;

            try
            {
                oCG = new CApplication();
                ds = oCG.GetLoanDtlByMember(Vmember, vAppDt);
                dt = ds.Tables[0];
                //dt1 = ds.Tables[1];
                txtspnm.Text = dt.Rows[0]["SPName"].ToString();
                //gvlndtl.DataSource = dt1;
                //gvlndtl.DataBind();
            }
            finally
            {
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vMemberID"></param>
		/// <param name="vIsDisburse"></param>
		private void PopLoanNo(string vMemberID, string vIsDisburse)
		{
			ddlAppNo.Items.Clear();
			DataTable dt = null;
			CDisburse oLD = null;
			//string vBrCode = Session[gblValue.BrnchCode].ToString();
			try
			{
				oLD = new CDisburse();
				dt = oLD.GetLoanAppByMemberId(vMemberID, vIsDisburse,gblFuction.setDate(txtLnDt.Text));
				if (dt.Rows.Count > 0)
				{
					ddlAppNo.DataTextField = "LoanAppNo";
					ddlAppNo.DataValueField = "LoanAppId";
					ddlAppNo.DataSource = dt;
					ddlAppNo.DataBind();
					ListItem oItm = new ListItem();
					oItm.Text = "<--- Select --->";
					oItm.Value = "-1";
					ddlAppNo.Items.Insert(0, oItm);
				}
				else
				{
					ddlAppNo.Items.Clear();
					ddlAppNo.Items.Add("No Matching Records.");
				}

			}
			finally
			{
				dt = null;
				oLD = null;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlAppNo_SelectedIndexChanged(object sender, EventArgs e)
		{
			string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vGrpId = ddlGrp.SelectedValue;
			string vLoanAppID = ddlAppNo.SelectedValue;
			//string vGroupId = ddlCenter.SelectedValue;
			DateTime vLoanDt = gblFuction.setDate(txtLnDt.Text);
			//Int32 vCollDay = 0,vCollDayNo=0;
			DataTable dt = null,dtDay=null;
			CDisburse oLD = null;
			CCollectionRoutine oCR = null;
			try
			{
				oLD = new CDisburse();
				dt = oLD.GetLoanAppdtlGD(vLoanAppID, vBrCode, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
				if (dt.Rows.Count > 0)
				{
					ddlRpSchdle.SelectedIndex = ddlRpSchdle.Items.IndexOf(ddlRpSchdle.Items.FindByValue(dt.Rows[0]["PaySchedule"].ToString().Trim()));
					ddlLoanType.SelectedIndex = ddlLoanType.Items.IndexOf(ddlLoanType.Items.FindByValue(dt.Rows[0]["LoanTypeId"].ToString()));
					//ddlInstType.SelectedIndex = ddlInstType.Items.IndexOf(ddlInstType.Items.FindByValue(dt.Rows[0]["InstType"].ToString()));
					ddlPurps.SelectedIndex = ddlPurps.Items.IndexOf(ddlPurps.Items.FindByValue(dt.Rows[0]["PurposeID"].ToString()));
					ddlSubPur.SelectedIndex = ddlSubPur.Items.IndexOf(ddlSubPur.Items.FindByValue(dt.Rows[0]["SubPurposeID"].ToString()));
					txtLnAmt.Text = Convert.ToString(dt.Rows[0]["ApprovedAmt"]);
					txtIntRate.Text = Convert.ToString(dt.Rows[0]["InstRate"]);
					txtLnCycle.Text = Convert.ToString(dt.Rows[0]["LoanCycle"]);
					txtIntPeriod.Text = Convert.ToString(dt.Rows[0]["InstPeriod"]);
					txtInstNo.Text = Convert.ToString(dt.Rows[0]["InstallNo"]);
					txtProsFee.Text = Convert.ToString(dt.Rows[0]["ProcFeeAmt"]);
					txtSrvTax.Text = Convert.ToString(dt.Rows[0]["ServiceTaxAmt"]);
                    txtInsurfee.Text = Convert.ToString(dt.Rows[0]["IncFeesAmt"]);
                    txtOthFees.Text = Convert.ToString(dt.Rows[0]["OthFeesAmt"]);
                    txtPAI.Text = "0";//Convert.ToString(dt.Rows[0]["PAIAmt"]);
                    //txtLnDt.Text = Convert.ToString(dt.Rows[0]["ExpDate"]);
                    //if (Convert.ToDouble(dt.Rows[0]["PAIAmt"]) > 0)
                    //    ddlPAIAppli.SelectedIndex = ddlPAIAppli.Items.IndexOf(ddlPAIAppli.Items.FindByValue("Y"));
                    //else
                    //    ddlPAIAppli.SelectedIndex = ddlPAIAppli.Items.IndexOf(ddlPAIAppli.Items.FindByValue("N"));
					txtProsFeeAmt.Text = Convert.ToString((Convert.ToDouble(txtLnAmt.Text) * Convert.ToDouble(txtProsFee.Text))/100);
					txtSrvTaxAmt.Text = txtSrvTax.Text;
                    PopIcMst();
                    //txtInsurAmt.Text = txtInsurfee.Text;
                    txtInsurAmt.Text = Convert.ToString(dt.Rows[0]["IncFeesAmt"]);
                    txtOthFeesAmt.Text = txtOthFees.Text;
                    txtPAIAmt.Text = txtPAI.Text;
                    hdPAIAmt.Value=txtPAI.Text;
                    hdnMedAmt.Value = Convert.ToString(dt.Rows[0]["MedClaimAmt"]);
					txtInstSize.Text = Convert.ToString(dt.Rows[0]["InstallmentSize"]);
					ViewState["CGTID"] = Convert.ToString(dt.Rows[0]["CGTID"]);
                    hdPSNName.Value = Convert.ToString(dt.Rows[0]["SName"]);
                    if (hdPSNName.Value == "IDBI")
                    {
                        txtStDt.Enabled = true;
                    }
                    else
                    {
                        txtStDt.Enabled = false;
                    }
					
					oCR = new CCollectionRoutine();
                    dtDay = oCR.GetCollDayNew(vGrpId, vBrCode, ddlRpSchdle.SelectedValue, vLoanDt);
					if (dtDay.Rows.Count > 0)
					{
						//vCollDay = Convert.ToInt32(dtDay.Rows[0]["CollDay"]);
						//vCollDayNo = Convert.ToInt32(dtDay.Rows[0]["CollDayNo"]);
						//txtStDt.Text = gblFuction.GetStartDate(vLoanDt, vCollDay, vCollDayNo, Convert.ToString(ddlRpSchdle.SelectedValue), vGroupId);
						txtStDt.Text = dtDay.Rows[0]["StartDate"].ToString();
						txtDay.Text = dtDay.Rows[0]["ColDay"].ToString();
						//txtDay.Text = Convert.ToString(gblFuction.setDate(txtStDt.Text).DayOfWeek);
					}
					else
					{
						gblFuction.AjxMsgPopup("Please Set the collection Routine of the Group...");
						return;
					}
                    if (Convert.ToString(dt.Rows[0]["NEFTApproveYN"]) == "Y")
                    {
                        tblNEFT.Visible = false;
                        rdbPayMode.SelectedIndex = rdbPayMode.Items.IndexOf(rdbPayMode.Items.FindByValue("F"));
                        
                    }
                    if (Convert.ToString(dt.Rows[0]["CashApproveYN"]) == "Y")
                    {
                        rdbPayMode.SelectedIndex = rdbPayMode.Items.IndexOf(rdbPayMode.Items.FindByValue("C"));
                    }
                    rdbPayMode.Enabled = false;
                    
				}
				//else
				//{
				//    //txtLnScheme.Text = "";
				//    //txtLoanAmt.Text = "";
				//    //txtLoanDt.Text = "";
				//    //txtLnCycle.Text = "";
				//}
			}
			finally
			{
				dt = null;
				oLD = null;
			}
		}
        protected void chkMedApp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMedApp.Checked == true)
            {
                txtMedAmt.Text = hdnMedAmt.Value;
            }
            else
            {
                txtMedAmt.Text="0";
            }
        }

		protected void gvLoanAppl_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			DataTable dt = null;
			DataTable dtDtl = null;
			CDisburse oLD = null;
			string vBrCode = Session[gblValue.BrnchCode].ToString();
			string vLoanId = "";
			try
			{
				if (e.CommandName == "cmdShow")
				{
					oLD = new CDisburse();
					vLoanId = Convert.ToString(e.CommandArgument);
					dt = oLD.GetAllLoanByLoanId(vLoanId);
					if (dt.Rows.Count > 0)
					{
						GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
						LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
						foreach (GridViewRow gr in gvLoanAppl.Rows)
						{
							LinkButton lb = (LinkButton)gr.FindControl("btnShow");
							lb.ForeColor = System.Drawing.Color.Black;
						}
						btnShow.ForeColor = System.Drawing.Color.Red;
						//Load Field
						txtLnNo.Text = Convert.ToString(dt.Rows[0]["LoanNo"]);
						ddlCo.SelectedIndex = ddlCo.Items.IndexOf(ddlCo.Items.FindByValue(dt.Rows[0]["EOID"].ToString()));
						ddlDisburseBy.SelectedIndex = ddlDisburseBy.Items.IndexOf(ddlDisburseBy.Items.FindByValue(dt.Rows[0]["DID"].ToString()));
						PopCenter(ddlCo.SelectedValue);
						ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["MarketID"].ToString()));
						PopGroup(ddlCenter.SelectedValue);
						ddlGrp.SelectedIndex = ddlGrp.Items.IndexOf(ddlGrp.Items.FindByValue(dt.Rows[0]["GroupId"].ToString()));
						PopMember(dt.Rows[0]["GroupId"].ToString());
						ddlMembr.SelectedIndex = ddlMembr.Items.IndexOf(ddlMembr.Items.FindByValue(dt.Rows[0]["MemberId"].ToString()));
						PopLoanNo(ddlMembr.SelectedValue, "Y");
						ddlAppNo.SelectedIndex = ddlAppNo.Items.IndexOf(ddlAppNo.Items.FindByValue(dt.Rows[0]["LoanAppId"].ToString()));
                        PopIcMst();
						ddlRpSchdle.SelectedIndex = ddlRpSchdle.Items.IndexOf(ddlRpSchdle.Items.FindByValue(dt.Rows[0]["RSchedule"].ToString()));
						ddlLoanType.SelectedIndex = ddlLoanType.Items.IndexOf(ddlLoanType.Items.FindByValue(dt.Rows[0]["LoanTypeId"].ToString()));
						lblMemNm.Text = Convert.ToString(dt.Rows[0]["MemberName"]);
						//ddlInstType.SelectedIndex = ddlInstType.Items.IndexOf(ddlInstType.Items.FindByValue(dt.Rows[0]["InstType"].ToString()));
						txtLnAmt.Text = Convert.ToString(dt.Rows[0]["LoanAmt"]);
						txtIntRate.Text = Convert.ToString(dt.Rows[0]["IntRate"]);
						txtLnCycle.Text = Convert.ToString(dt.Rows[0]["Dose"]);
						txtIntAmt.Text = Convert.ToString(dt.Rows[0]["IntAmt"]);
						txtIntPeriod.Text = Convert.ToString(dt.Rows[0]["Period"]);
						txtInstNo.Text = Convert.ToString(dt.Rows[0]["TotalInstNo"]);
						txtLnDt.Text = Convert.ToString(dt.Rows[0]["Loandt"]);
						txtStDt.Text = Convert.ToString(dt.Rows[0]["CollStartDt"]);
                        txtAccNo.Text = Convert.ToString(dt.Rows[0]["AccountNo"]);
						txtDay.Text = Convert.ToString(gblFuction.setDate(txtStDt.Text).DayOfWeek);
						txtInstSize.Text = Convert.ToString(dt.Rows[0]["InstallmentAmt"]);
						txtProsFee.Text = Convert.ToString(dt.Rows[0]["procintfee"]);
						txtProsFeeAmt.Text = Convert.ToString(dt.Rows[0]["procfee"]);
                        hdnMedAmt.Value = Convert.ToString(dt.Rows[0]["MedClaimAmt"]);
                        if (dt.Rows[0]["MedClaimYN"].ToString() == "Y")
                        {
                            chkMedApp.Checked = true;
                            txtMedAmt.Text = hdnMedAmt.Value;
                        }
                        else
                        {
                            chkMedApp.Checked = false;
                            txtMedAmt.Text = "0";
                        }
						txtSrvTax.Text = Convert.ToString(dt.Rows[0]["StPfP"]);
						txtSrvTaxAmt.Text = Convert.ToString(dt.Rows[0]["STPF"]);
                        ddlIcMst.SelectedIndex = ddlIcMst.Items.IndexOf(ddlIcMst.Items.FindByValue(Convert.ToString(dt.Rows[0]["ICId"])));
                        txtInsurfee.Text = Convert.ToString(dt.Rows[0]["IncFeesAmt"]);
                        txtInsurAmt.Text = Convert.ToString(dt.Rows[0]["ICAmt"]);
                        txtOthFees.Text = Convert.ToString(dt.Rows[0]["OthersFees"]);
                        txtOthFeesAmt.Text = Convert.ToString(dt.Rows[0]["OthersFee"]);
                        txtPAI.Text = Convert.ToString(dt.Rows[0]["PAIAmt"]);
                        txtPAIAmt.Text = Convert.ToString(dt.Rows[0]["PAIAmount"]);
                        hdPAIAmt.Value= Convert.ToString(dt.Rows[0]["PAIAmt"]);
						ddlSrcFund.SelectedIndex = ddlSrcFund.Items.IndexOf(ddlSrcFund.Items.FindByValue(Convert.ToString(dt.Rows[0]["SFId"])));
						ddlPurps.SelectedIndex = ddlPurps.Items.IndexOf(ddlPurps.Items.FindByValue(Convert.ToString(dt.Rows[0]["PurposeId"])));
						ddlSubPur.SelectedIndex = ddlSubPur.Items.IndexOf(ddlSubPur.Items.FindByValue(Convert.ToString(dt.Rows[0]["SubPurposeId"])));
                        ddlPAIAppli.SelectedIndex = ddlPAIAppli.Items.IndexOf(ddlPAIAppli.Items.FindByValue(Convert.ToString(dt.Rows[0]["PAIAmtApli"])));
						ViewState["CGTID"] = Convert.ToString(dt.Rows[0]["CGTID"]);
                        DateTime vAppDt = gblFuction.setDate(txtLnDt.Text);
                        fillLnDtl(dt.Rows[0]["MemberId"].ToString(), vAppDt);
						if (Convert.ToInt32(dt.Rows[0]["CashOrChq"]) == 1)
						{
							tblBank.Visible = false;
                            tblNEFT.Visible = false;
							rdbPayMode.SelectedValue = "C";
						}
						else if (Convert.ToInt32(dt.Rows[0]["CashOrChq"]) == 2)
						{
							tblBank.Visible = true;
                            tblNEFT.Visible = false;
							rdbPayMode.SelectedValue = "B";
							txtChqNo.Text = Convert.ToString(dt.Rows[0]["chqno"]);
							txtChqDt.Text = Convert.ToString(dt.Rows[0]["chqdt"]);
							ddlBank.SelectedIndex = ddlBank.Items.IndexOf(ddlBank.Items.FindByValue(Convert.ToString(dt.Rows[0]["BankID"])));
						}
                        else if (Convert.ToInt32(dt.Rows[0]["CashOrChq"]) == 4)
                        {
                            tblNEFT.Visible = true;
                            tblBank.Visible = false;
                            rdbPayMode.SelectedValue = "F";
                            //txtNeftNo.Text = Convert.ToString(dt.Rows[0]["chqno"]);
                            //txtNEFTDt.Text = Convert.ToString(dt.Rows[0]["chqdt"]);
                            ddlBankHo.SelectedIndex = ddlBankHo.Items.IndexOf(ddlBankHo.Items.FindByValue(Convert.ToString(dt.Rows[0]["BankID"])));
                        }
						LblUser.Text = "Last Modified By : " + Convert.ToString(dt.Rows[0]["UserName"]);
						LblDate.Text = "Last Modified Date : " + Convert.ToString(dt.Rows[0]["CreationDateTime"]);
                  
						ViewState["LoanId"] = Convert.ToString(dt.Rows[0]["LoanId"]);
                        hdPSNName.Value = Convert.ToString(dt.Rows[0]["SName"]); 

						//dtDtl = oLD.GetSchedule(1, 1000, 15, 50, 12, gblFuction.setDate(txtLnDt.Text), "", Convert.ToString(dt.Rows[0]["LoanId"]), "Y", vBrCode, "W", "", "", 1, 1, "G", 10, "");
                        dtDtl = oLD.GetSchedule(ddlCenter.SelectedValue, 1, Convert.ToString(dt.Rows[0]["LoanId"]), "Y", "", 1000, gblFuction.setDate(txtLnDt.Text), 190, 50, "W", 15, vBrCode, gblFuction.setDate(txtLnDt.Text));
						if (dtDtl.Rows.Count > 0)
						{
							ViewState["Schedule"] = dtDtl;
							gvSchdl.DataSource = dtDtl;
							gvSchdl.DataBind();
						}
					}
					tabLoanDisb.ActiveTabIndex = 1;
					//tabLoanDisb.Tabs[0].Enabled = false;
					//tabLoanDisb.Tabs[1].Enabled = true;               
					//tabLoanDisb.Tabs[2].Enabled = false;
					StatusButton("Show");
					EnableControl(false);
				}
                if (e.CommandName == "cmdRpt")
                {
                    vLoanId = Convert.ToString(e.CommandArgument);
                    GetData(vLoanId, "PDF");

                }
			}
			finally
			{
				dt = null;
				dtDtl = null;
				oLD = null;
			}
		}
        public void GetData(string pLoanId, string pMode)
        {
            string vBrCode = "", vTitle = "", vRptPath = "";

            DataSet ds = null;
            DataTable dt1 = null, dt2 = null, dt3 = null;
            CReports oRpt = null;
            string vBranch = Session[gblValue.BrName].ToString();
             vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                vTitle = "Loan Crad";
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    oRpt = new CReports();
                    ds = oRpt.RptLoanCard(pLoanId);
                    dt1 = ds.Tables[0];
                    dt1.TableName = "MemInfo";
                    dt2 = ds.Tables[1];
                    if (Convert.ToInt32(dt2.Rows[0]["TotalInstNo"].ToString())>51)
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCard.rpt";
                    else
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardShort.rpt";
                    //dt2.TableName = "LoanSchedule1";
                    //dt3 = ds.Tables[2];
                    //dt3.TableName = "LoanSchedule2";
                    
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(ds);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    //rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    //rptDoc.SetParameterValue("pAddress2", "");
                    rptDoc.SetParameterValue("pBranch", vBranch);
                    rptDoc.SetParameterValue("pBrCode", vBrCode);
                    //rptDoc.SetParameterValue("pTitle", vTitle);
                    //rptDoc.SetParameterValue("dtFrom", "01/01/1900");
                    //rptDoc.SetParameterValue("dtTo", txtToDt.Text);

                    if (pMode == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Loan Card");

                    rptDoc.Dispose();
                    Response.ClearContent();
                    Response.ClearHeaders();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ds = null;
                oRpt = null;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void rdbPayMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (rdbPayMode.SelectedValue == "C")
			{
				tblBank.Visible = false;
                tblNEFT.Visible = false;
			}
			else if (rdbPayMode.SelectedValue == "B")
			{
				tblBank.Visible = true;
                tblNEFT.Visible = false;
			}
            else if (rdbPayMode.SelectedValue == "F")
            {
                tblNEFT.Visible = false;
                tblBank.Visible = false;
            }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void txtLnDt_TextChanged(object sender, EventArgs e)
		{
			//Int32 vCollDay = 0, vCollDayNo = 0;
			DataTable dtDay = null;
			CCollectionRoutine oCR = null;
			string vBrCode = Session[gblValue.BrnchCode].ToString();
			string vMarketId = ddlCenter.SelectedValue;

			if (ddlAppNo.SelectedIndex <= 0)
			{
				gblFuction.AjxMsgPopup("Please Select the application No...");
				return;
			}
			if (txtLnDt.Text != "" && gblFuction.IsDate(txtLnDt.Text) == true)
			{
				DateTime vLoanDt = gblFuction.setDate(txtLnDt.Text);
				oCR = new CCollectionRoutine();
				////dtDay = oCR.GetCollDay(Convert.ToInt32(ddlGrp.SelectedValue));
				//if (dtDay.Rows.Count > 0)
				//{
				//    vCollDay = Convert.ToInt32(dtDay.Rows[0]["CollDay"]);
				//    vCollDayNo = Convert.ToInt32(dtDay.Rows[0]["CollDayNo"]);
				//    txtStDt.Text = gblFuction.GetStartDate(gblFuction.setDate(txtLnDt.Text), vCollDay, vCollDayNo, Convert.ToString(ddlRpSchdle.SelectedValue), Convert.ToInt32(ddlGrp.SelectedValue));
				//    txtDay.Text = Convert.ToString(gblFuction.setDate(txtStDt.Text).DayOfWeek);
				//}

				dtDay = oCR.GetCollDayNew(vMarketId, vBrCode, ddlRpSchdle.SelectedValue, vLoanDt);
				if (dtDay.Rows.Count > 0)
				{
					//vCollDay = Convert.ToInt32(dtDay.Rows[0]["CollDay"]);
					//vCollDayNo = Convert.ToInt32(dtDay.Rows[0]["CollDayNo"]);
					//txtStDt.Text = gblFuction.GetStartDate(vLoanDt, vCollDay, vCollDayNo, Convert.ToString(ddlRpSchdle.SelectedValue), vGroupId);
					txtStDt.Text = dtDay.Rows[0]["StartDate"].ToString();
					txtDay.Text = dtDay.Rows[0]["ColDay"].ToString();
					//txtDay.Text = Convert.ToString(gblFuction.setDate(txtStDt.Text).DayOfWeek);
				}
				else
				{
					gblFuction.AjxMsgPopup("Please Set the collection Routine of the Group...");
					return;
				}
			}
			else
			{
				gblFuction.AjxMsgPopup("Invalid Loan Date...");
				return;
			}
		}

		protected void gvSchdl_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			DataTable dt = new DataTable();
			dt = (DataTable)ViewState["Schedule"];
			gvSchdl.PageIndex = e.NewPageIndex;
			gvSchdl.DataSource = dt;
			gvSchdl.DataBind();
			tabLoanDisb.ActiveTabIndex = 2;
		}
        public void PopIcMst()
        {
            DataTable dt = null;
            CDisburse oDbr = new CDisburse();
            dt = oDbr.GetInSuranceCompany(Session[gblValue.BrnchCode].ToString(), gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
            ViewState["Insurance"] = dt;
            ddlIcMst.Items.Clear();
            ddlIcMst.DataSource = dt;
            ddlIcMst.DataTextField = "ICName";
            ddlIcMst.DataValueField = "ICId";
            ddlIcMst.DataBind();
            ListItem oLi = new ListItem("<--Select-->", "-1");
            ddlIcMst.Items.Insert(0, oLi);

        }
        protected void ddlIcMst_selectedIndexChanged(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)ViewState["Insurance"];
            CDisburse oDbr = new CDisburse();
            double vInsuAmt = 0;
            if (ddlIcMst.SelectedIndex > 0)
            {
                vInsuAmt = oDbr.GetInSuranceAmt(Convert.ToDouble(txtLnAmt.Text), Convert.ToInt32(ddlIcMst.SelectedValue));
                txtInsurAmt.Text = vInsuAmt.ToString();
            }
            
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dt.Rows[i]["ICId"].ToString() == ddlIcMst.SelectedValue)
            //    {
            //        vinsuAmt=((Convert.ToDouble(dt.Rows[i]["ICAmt"].ToString()) * (Convert.ToDouble(txtLnAmt.Text)/1000))+((Convert.ToDouble(dt.Rows[i]["ProcFees"].ToString())*Convert.ToDouble(txtLnAmt.Text))/100));
            //        txtInsurAmt.Text=vinsuAmt.ToString();
            //    }
            //}
        }

	}
}
