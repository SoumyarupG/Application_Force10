using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using Excel;
using System.IO;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class RetExIDBI : CENTRUMBase
    {
        public static double vTotAmt = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = "Add";
                ViewState["State"] = null;
                StatusButton("Add");
                txtEftDt.Text = Session[gblValue.LoginDate].ToString();
                tbBnk.ActiveTabIndex = 0;
                PopBranch();
                txtEftDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Return IDBI Extra Amount";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuRetExIDBI);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnSave.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Import IDBI Data", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

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

        private void EnableControl(Boolean Status)
        {
            txtEftDt.Enabled = Status;
            ddlBr.Enabled = Status;
        }

        private void ClearControls()
        {
            ddlBr.SelectedIndex = -1;
        }

        private void PopBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "Y", "BranchCode", "BranchCode", "BranchName", "BranchMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlBr.DataSource = dt;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["BranchCode"].ToString() == "0000")
                        dr.Delete();
                }
                dt.AcceptChanges();
                ddlBr.DataTextField = "Name";
                ddlBr.DataValueField = "BranchCode";
                ddlBr.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBr.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (this.CanDelete == "N")
            //    {
            //        gblFuction.MsgPopup(MsgAccess.Del);
            //        return;
            //    }
            //    if (SaveRecords("Delete") == true)
            //    {
            //        gblFuction.MsgPopup(gblMarg.DeleteMsg);
            //        StatusButton("Delete");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }


        protected void btnEdit_Click(object sender, EventArgs e)
        {

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbBnk.ActiveTabIndex = 0;
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
                //gblFuction.MsgPopup(gblMarg.SaveMsg);
                StatusButton("Add");
                ViewState["StateEdit"] = null;
                gvSchdl.DataSource = null;
                gvSchdl.DataBind();
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            Int32 vTotLine;
            int vLine = 0, vCol = 0;
            if (fuAtten.HasFile == true)
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                try
                {
                    string vExt = Path.GetExtension(fuAtten.PostedFile.FileName).Substring(1);
                    if (vExt.ToLower() == "xls")
                    {
                        using (IExcelDataReader oERSum = ExcelReaderFactory.CreateBinaryReader(fuAtten.FileContent))
                        {
                            try
                            {
                                oERSum.IsFirstRowAsColumnNames = true;
                                ds = oERSum.AsDataSet();
                                dt = ds.Tables[0];
                                dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
                            }
                            catch
                            {
                                gblFuction.AjxMsgPopup("Data Import Error: " + oERSum.ExceptionMessage);
                            }
                        }
                    }
                    else if (vExt.ToLower() == "xlsx")
                    {
                        using (IExcelDataReader oERSum = ExcelReaderFactory.CreateOpenXmlReader(fuAtten.FileContent))
                        {
                            try
                            {
                                oERSum.IsFirstRowAsColumnNames = true;
                                ds = oERSum.AsDataSet();
                                dt = ds.Tables[0];
                                dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
                            }
                            catch
                            {
                                gblFuction.AjxMsgPopup("Data Import Error: " + oERSum.ExceptionMessage);
                            }
                        }
                    }
                    else if (vExt.ToLower() == "csv")
                    {
                        //Upload and save the file
                        string csvPath = Server.MapPath("~/Files/") + Path.GetFileName(fuAtten.PostedFile.FileName);
                        fuAtten.SaveAs(csvPath);

                        dt = new DataTable();
                        dt.Columns.AddRange(new DataColumn[4] { new DataColumn("LoanNo", typeof(string)),
                        new DataColumn("BranchCode", typeof(string)),
                        new DataColumn("Amount",typeof(float)),
                        new DataColumn("RetDate")
                        });

                        string[] lines = File.ReadAllLines(csvPath);
                        vTotLine = lines.Count() - 1;
                        for (vLine = 0; vLine <= vTotLine; vLine++)
                        {
                            if (lines[vLine].Trim() == "")
                            {
                                continue;
                            }
                            else
                            {
                                dt.Rows.Add();
                                vCol = 0;
                                foreach (string cell in lines[vLine].Split(','))
                                {
                                    dt.Rows[dt.Rows.Count - 1][vCol] = cell;
                                    vCol++;
                                }
                            }
                        }
                    }
                    dt.AcceptChanges();
                    try
                    {
                        DataTable dtFinal = new DataView(dt).ToTable(false, "LoanNo", "BranchCode", "BranchName", "MemberName", "AMOUNT_TO_BE_RETURN", "DATE_OF_RETURN");
                        if (ValidateExl(dtFinal))
                            LoadGridFrmExl(dtFinal);
                        else
                        {
                            gvSchdl.DataSource = null;
                            gvSchdl.DataBind();
                        }
                    }
                    catch (Exception ex)
                    {
                        gblFuction.AjxMsgPopup("Excel Sheet contains Invalid Columns.");
                        gvSchdl.DataSource = null;
                        gvSchdl.DataBind();
                        return;
                    }
                }
                finally
                {
                    dt = null;
                    ds = null;
                }
                gblFuction.AjxMsgPopup("Upload Successful !!");
            }
            else
                gblFuction.AjxMsgPopup("Please Select a File");
        }

        private Boolean ValidateExl(DataTable dtIDBI)
        {
            Boolean vResult = true;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            vBrCode = ddlBr.SelectedValue;
            string vXmlAtten = "";

            foreach (DataColumn col in dtIDBI.Columns)
            {
                foreach (DataRow row in dtIDBI.Rows)
                {
                    if (row[col].ToString() == "")
                    {
                        gblFuction.AjxMsgPopup("Excel sheet cannot contain blank cells.");
                        return false;
                    }
                }
            }
            foreach (DataRow dr in dtIDBI.Rows)
            {
                if (dr["BranchCode"].ToString() != ddlBr.SelectedValue)
                {
                    dr.Delete();
                }
            }
            dtIDBI.AcceptChanges();
            var duplicates = dtIDBI.AsEnumerable().GroupBy(r => r[0]).Where(gr => gr.Count() > 1).ToList();//.Select(g => g.Key);
            if (duplicates.Count > 0)
            {
                gblFuction.AjxMsgPopup("Duplicate Loan No found");
                return false;
            }
            dtIDBI.TableName = "RetIDBI";
            using (StringWriter oSW = new StringWriter())
            {
                dtIDBI.WriteXml(oSW);
                vXmlAtten = oSW.ToString();
            }
            return vResult;
        }
        private void LoadGridFrmExl(DataTable dtIDBI)
        {
            string vXmlAtten = "", vDateTxt = "";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            vBrCode = ddlBr.SelectedValue;
            if (dtIDBI.Rows.Count > 0)
            {
                vDateTxt = gblFuction.setStrDate(dtIDBI.Rows[0]["DATE_OF_RETURN"].ToString());
            }
            dtIDBI.TableName = "RetIDBI";
            gvSchdl.DataSource = dtIDBI;
            gvSchdl.DataBind();
            using (StringWriter oSW = new StringWriter())
            {
                dtIDBI.WriteXml(oSW);
                vXmlAtten = oSW.ToString();
            }
            ViewState["Return"] = dtIDBI;
        }
        public string DtToXml(DataTable dt)
        {
            string vXml = "";
            dt.Columns.Add("SlNo");
            dt.Columns.Add("AddYN");
            foreach (DataRow dr in dt.Rows)
            {
                dr["SlNo"] = dt.Rows.IndexOf(dr) + 1;
                dr["DATE_OF_RETURN"] = gblFuction.setDate(dr["DATE_OF_RETURN"].ToString());
                vTotAmt = vTotAmt + Convert.ToDouble(dr["AMOUNT_TO_BE_RETURN"].ToString());
            }
            dt.AcceptChanges();
            dt.TableName = "RetIDBI";
            using (StringWriter oSw = new StringWriter())
            {
                dt.WriteXml(oSw);
                vXml = oSw.ToString();
            }
            return vXml;
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = string.Empty, vBrCode = string.Empty, vAppNo = string.Empty;
            Int32 vErr = 0;
            string vAppId = "", vMsg = "", vXml = "";
            DateTime vAppDt = gblFuction.setDate(txtEftDt.Text);
            CBC_Repayment_Tracking oCG = null;
            try
            {
                vBrCode = ddlBr.SelectedValue;
                string vTblMst = Session[gblValue.ACVouMst].ToString();
                string vTblDtl = Session[gblValue.ACVouDtl].ToString();
                string vFinYear = Session[gblValue.ShortYear].ToString();
                vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                vAppId = Convert.ToString(ViewState["AppId"]);
                DataTable dt = (DataTable)ViewState["Return"];
                vXml = DtToXml(dt);
                if (Mode == "Save")
                {
                    oCG = new CBC_Repayment_Tracking();
                    if (this.RoleId != 1)                           //    && this.RoleId != 4     1 for Admin                 4 for BM discussed with Prodip as on 2nd Sep/2014
                    {
                        if (Session[gblValue.EndDate] != null)
                        {
                            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtEftDt.Text))
                            {
                                gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                                return false;
                            }
                        }
                    }

                    this.GetModuleByRole(mnuID.mnuRetExIDBI);
                    vErr = oCG.InsertRetIDBIExtAmt(vAppDt, vBrCode, vXml, this.UserID, "I", "Save", vTblMst, vTblDtl, vFinYear, ref vMsg);
                    if (vErr > 0)
                    {
                        if (vMsg != "")
                        {
                            gblFuction.AjxMsgPopup("Not all Loan No. Saved, This Loan No " + vMsg + " Found in database previously");
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.SaveMsg);
                        }
                        vResult = true;
                        ViewState["Return"] = null;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }

                else if (Mode == "Delete")
                {
                    oCG = new CBC_Repayment_Tracking();

                    this.GetModuleByRole(mnuID.mnuRetExIDBI);
                    vErr = oCG.InsertRetIDBIExtAmt(vAppDt, vBrCode, vXml, this.UserID, "I", "Delete", vTblMst, vTblDtl, vFinYear, ref vMsg);
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCG = null;
                //dt = null;
            }
        }
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
    }
}