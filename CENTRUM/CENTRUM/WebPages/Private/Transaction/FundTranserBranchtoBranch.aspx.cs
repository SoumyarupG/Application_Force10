using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using FORCEBA;
using FORCECA;
using System.Web.UI.WebControls;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class FundTranserBranchtoBranch : CENTRUMBase
    {
        private static string vFile;
        private static string vExcelPath = ConfigurationManager.AppSettings["DBPath"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                GetCashBank();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void GetCashBank()
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oVoucher = new CVoucher();
                dt = oVoucher.GetAcGenLedCB(vBrCode, "A", "");
                ddlCashBank.DataSource = dt;
                ddlCashBank.DataTextField = "Desc";
                ddlCashBank.DataValueField = "DescId";
                ddlCashBank.DataBind();
                ListItem Li = new ListItem("<-- Select -->", "-1");
                ddlCashBank.Items.Insert(0, Li);
            }
            finally
            {
                oVoucher = null;
                dt = null;
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
                this.PageHeading = "Inter-Branch Fund Transfer";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuIntrBrFndTr);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Inter Branch Fund Transfer", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public DataSet ExcelToDS(string Path)
        {
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            conn.Open();
            strExcel = "select * from [sheet1$]";
            //strExcel = "select Emp_ID,Employee_Name,BASIC,HRA,Conveyance,Petrol_All,Food_Allow,Special_Al,Arrear,Data_Card,Area_Allow,Arr.Earn," +
            //"Other_All1,Other_All2,Other_All3,Other_All4,Other_All5,Mobile_Ded,Bike_Deduc,Mess,Advance,Loan,PF,ESI,TDS,Other_Deduction from [sheet1$]";
            try
            {
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "FundTransfer");
                conn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                gblFuction.AjxMsgPopup("Error:Please Check Excel");
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //DataSet ds = null;
            //DataTable dt = null;
            if (ddlCashBank.SelectedIndex == -1)
            {
                gblFuction.MsgPopup("Select Cash or Bank");
                return;
            }

            if (fuExcl.HasFile == true)
            {
                if (!Directory.Exists(vExcelPath))
                {
                    Directory.CreateDirectory(vExcelPath);
                }
                vFile = fuExcl.FileName;
                FileInfo vfile = new FileInfo(vExcelPath + vFile);
                if (vfile.Exists)
                {
                    vfile.Delete();
                }
                fuExcl.SaveAs(vExcelPath + vFile);
                //ds = ExcelToDS(vExcelPath + fuExcl.FileName);
                //dt = ds.Tables[0];
                //if (dt.Rows.Count > 0)
                //    gblFuction.AjxMsgPopup("Upload Successful !!");
                if (vfile.Exists)
                {
                    gblFuction.MsgPopup("Upload Successful !!");

                }
            }
            else
                gblFuction.MsgPopup("Please Select a File");
        }
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
            DataSet ds = null;
            DataTable dt = null;
            //DataTable dtTemp = null;
            ds = ExcelToDS(vExcelPath + vFile);
            dt = ds.Tables[0];
            //dtTemp = ds.Tables[0];

            ////LoanAppId	REPORTID		DATEOFISSUE	NUMOFOTHERMFIS	SUMCURRENTBALANCE[OTHER]	

            //foreach (DataColumn dc in dtTemp.Columns)
            //{
            //    if (dc.Caption != "LoanAppId" && dc.Caption != "REPORTID"  && dc.Caption != "NUMOFOTHERMFIS"  && dc.Caption != "DATEOFISSUE"  && dc.Caption != "SUMCURRENTBALANCE")
            //    {
            //        dt.Columns.Remove(dc);
            //    }
            //}
            // dt.AcceptChanges();
            if (dt.Rows.Count > 0)
            {
                if (SaveRecord(dt))
                {
                    FileInfo vfile = new FileInfo(vExcelPath + vFile);
                    if (vfile.Exists)
                    {
                        vfile.Delete();
                    }
                    gblFuction.AjxMsgPopup("Equifax Data Successfully Updated ..");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool validate(string vXml)
        {
            bool vRes = true;
            //string vEmp = "";
            //CAutoVouEntry oAv = null;
            //oAv = new CAutoVouEntry();
            //vEmp = oAv.ChkAtoVouEntry(vXml, ddlStfAc.SelectedValue);
            //if (vEmp.Trim() != "OK")
            //{
            //    gblFuction.AjxMsgPopup(vEmp + " has no Ledger yet ! first create it then proceed");
            //    return false;
            //}
            return vRes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool SaveRecord(DataTable dt)
        {
            bool vRes = true;
            string vXmlA = "";
            int vErr = 0;
            CNpsMember oAv = null;

           
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXmlA = oSW.ToString();
            }

            //if (validate(vXmlA.Replace("_x0020_", "").Trim().Replace("'", "")) == false)
            //    return false;
            //Though it is a different operation Still this CNpsMember Class is used
            oAv = new CNpsMember();
            vErr = oAv.InterBranchFundTransfer(vXmlA.Replace("_x0020_", "").Trim().Replace("'", ""), this.UserID, 
                ddlCashBank.SelectedValue,Convert.ToString(Session[gblValue.BrnchCode]), 
                gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate]))
                , Convert.ToString(Session[gblValue.ACVouMst])
                , Convert.ToString(Session[gblValue.ACVouDtl])
                , Convert.ToString(Session[gblValue.ShortYear])

                );

            return vRes;
        }
    }
}

