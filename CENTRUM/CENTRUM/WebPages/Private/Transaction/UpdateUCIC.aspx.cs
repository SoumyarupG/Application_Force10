using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class UpdateUCIC : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtAsOnDt.Text = Session[gblValue.LoginDate].ToString();

                LoadGrid( txtAsOnDt.Text);
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Update UCIC";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuMultiUpdateUcic);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Update UCIC", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ValidDate() == true)
            {
                try
                {
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                        LoadGrid(txtAsOnDt.Text);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private bool ValidDate()
        {
            Boolean vResult = true;
            if (txtAsOnDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtAsOnDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtAsOnDt");
                    vResult = false;
                }
            }
            if (txtAsOnDt.Text.Trim() != "" || txtAsOnDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtAsOnDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtToDt");
                    vResult = false;
                }
            }
            return vResult;
        }

        private void LoadGrid( string pAsOnDt)
        {
            DataTable dt = null;
            CApplication oLS = null;
            Int32 vRows = 0;

            //  Int32.TryParse(ddlLnProd.SelectedValue, out vLoanProduct);

            try
            {
                //string vBrCode = pBranch;
                DateTime vAsOnDt = gblFuction.setDate(pAsOnDt);
                oLS = new CApplication();
                dt = oLS.GetMultiUcicData(vAsOnDt);

                ViewState["UCICData"] = dt;
                gvUCIC.DataSource = dt;
                gvUCIC.DataBind();
                lblTotalPages.Text = CalTotPgs(vRows).ToString();
                lblCurrentPage.Text = cPgNo.ToString();
                if (cPgNo == 0)
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
            finally
            {
                dt = null;
                oLS = null;
            }
        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        protected void ChangePage(object sender, CommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }

            LoadGrid(txtAsOnDt.Text);
            //tabLoanAppl.ActiveTabIndex = 0;
        }

        protected void chkAssignUCIC_CheckedChanged(object sender, EventArgs e)
        {
            string vBranch = "";
            DataTable dt = null;
            try
            {
                dt = (DataTable)ViewState["UCICData"];

                vBranch = Session[gblValue.BrnchCode].ToString();
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkAssignUCIC = (CheckBox)row.FindControl("chkAssignUCIC");
                TextBox txtUCIC = (TextBox)row.FindControl("txtUCIC");

                if (chkAssignUCIC.Checked)
                {
                    txtUCIC.Enabled = true;
                }
                else
                {
                    txtUCIC.Enabled = false;
                    txtUCIC.Text = "";
                }

                dt.AcceptChanges();
                ViewState["UCICData"] = dt;
                upSanc.Update();
            }
            finally
            {
                dt = null;
            }
        }

        protected void txtUCIC_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtUCIC = (TextBox)gvRow.FindControl("txtUCIC");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["UCICData"];
            dt.Rows[gvRow.RowIndex]["AssignUcic"] = txtUCIC.Text.Trim();
            dt.AcceptChanges();
            ViewState["UCICData"] = dt;
            upSanc.Update();
        }

        protected void gvUCIC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkAssignUCIC = (CheckBox)e.Row.FindControl("chkAssignUCIC");
                    TextBox txtUCIC = (TextBox)e.Row.FindControl("txtUCIC");
                    
                    if (chkAssignUCIC.Checked)
                    {
                        txtUCIC.Enabled = true;
                    }
                    else
                    {
                        txtUCIC.Enabled = false;
                        txtUCIC.Text = "";
                    }
                }
            }
            finally
            {
            }
        }

        private DataTable CreateTrData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "UCICTbl";
            dt.Columns.Add("MemberNo", typeof(string));            
            dt.Columns.Add("UpdateUcicYN", typeof(string));
            dt.Columns.Add("AssignUcic", typeof(string));
            dt.Columns.Add("Project", typeof(string));

            for (int i = 1; i <= gvUCIC.Rows.Count; i++)
            {
                CheckBox chkAssignUCIC = (CheckBox)gvUCIC.Rows[i - 1].FindControl("chkAssignUCIC");                
                TextBox txtUCIC = (TextBox)gvUCIC.Rows[i - 1].FindControl("txtUCIC");

                string MemberNo = (string)gvUCIC.Rows[i - 1].Cells[2].Text;
                string Project = (string)gvUCIC.Rows[i - 1].Cells[7].Text;

                if (chkAssignUCIC.Checked)
                {
                    if (txtUCIC.Text != "")
                    {
                        dt.Rows.Add();
                        dt.Rows[i - 1]["MemberNo"] = MemberNo;
                        dt.Rows[i - 1]["UpdateUcicYN"] = "Y";
                        dt.Rows[i - 1]["AssignUcic"] = txtUCIC.Text;
                        dt.Rows[i - 1]["Project"] = Project;
                    }
                    else
                    {
                        gblFuction.MsgPopup("UCIC To Be Assigned is Blank...");
                    }
                }
                else
                {
                    dt.Rows.Add();
                    dt.Rows[i - 1]["MemberNo"] = MemberNo;
                    dt.Rows[i - 1]["UpdateUcicYN"] = "N";
                    dt.Rows[i - 1]["AssignUcic"] = "";
                    dt.Rows[i - 1]["Project"] = Project;
                }
            }
            dt.AcceptChanges();
            return dt;
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            string vXml = "";
            int vErr = 0;
            DataTable dtXml = CreateTrData();

            using (StringWriter oSW = new StringWriter())
            {
                dtXml.WriteXml(oSW);
                vXml = oSW.ToString();
            }
            CMember oMem = new CMember();
            vErr = oMem.UpdateUcicBulk(vXml);
            if (vErr > 0)
            {
                gblFuction.MsgPopup("UCIC Updated Successfully.");                
            }
            else
            {
                gblFuction.MsgPopup(gblMarg.DBError);
            }
            LoadGrid(txtAsOnDt.Text);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }


    }
}