using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.IO;
using FORCEBA;
using FORCECA;
using Excel;
using System.Linq;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class ImportEquifaxData : CENTRUMBase
    {
        private static string vFile;
        private static string vExcelPath = ConfigurationManager.AppSettings["DBPath"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Import Equifax Data";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuImprtEqfxData);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Import Equifax Data", false);
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
                myCommand.Fill(ds, "ImportEquifax");
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
            
            //if (fuExcl.HasFile == true)
            //{
            //    if (!Directory.Exists(vExcelPath))
            //    {
            //        Directory.CreateDirectory(vExcelPath);
            //    }
            //    vFile = fuExcl.FileName;
            //    FileInfo vfile = new FileInfo(vExcelPath + vFile);
            //    if (vfile.Exists)
            //    {
            //        vfile.Delete();
            //    }
            //    fuExcl.SaveAs(vExcelPath + vFile);
                
            //    if (vfile.Exists)
            //    {
            //        gblFuction.AjxMsgPopup("Upload Successful !!");

            //    }
            //}
            //else
            //    gblFuction.MsgPopup("Please Select a File");
            Int32 vTotLine;
            int vLine = 0, vCol = 0;
            if (fuExcl.HasFile == true)
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                try
                {
                    if (!Directory.Exists(vExcelPath))
                    {
                        Directory.CreateDirectory(vExcelPath);
                    }
                    vFile = fuExcl.FileName;
                    FileInfo vfile = new FileInfo(vExcelPath + vFile);
                    string vExt = Path.GetExtension(fuExcl.PostedFile.FileName).Substring(1);
                    if (vExt.ToLower() == "csv")
                    {
                        
                        //string csvPath = Server.MapPath("~/Files/") + Path.GetFileName(fuExcl.PostedFile.FileName);
                        //fuExcl.SaveAs(csvPath);
                        if (vfile.Exists)
                        {
                            vfile.Delete();
                        }
                        fuExcl.SaveAs(vExcelPath + vFile);

                        dt = new DataTable();
                        dt.Columns.AddRange(new DataColumn[96] { new DataColumn("DATEOFISSUE", typeof(string)),
                        new DataColumn("HITCODE", typeof(string)),
                        new DataColumn("REPORTID",typeof(string)),
                        new DataColumn("REFERENCENUMBER",typeof(string)),
                        new DataColumn("NUMPREVENQS", typeof(string)),
                        new DataColumn("MEMBERNAME", typeof(string)),
                        new DataColumn("AGE", typeof(string)),
                        new DataColumn("UNIQUEACCNTNO", typeof(string)),
                        new DataColumn("DATEOFBIRTH", typeof(string)),
                        new DataColumn("ADDRESS", typeof(string)),
                        new DataColumn("STATE", typeof(string)),
                        new DataColumn("POSTAL", typeof(string)),
                        new DataColumn("ADDLNAME1", typeof(string)),
                        new DataColumn("ADDLNAMETYP1", typeof(string)),
                        new DataColumn("ADDLNAME2", typeof(string)),
                        new DataColumn("ADDLNAMETYP2", typeof(string)),
                        new DataColumn("PAN", typeof(string)),
                        new DataColumn("UID", typeof(string)),
                        new DataColumn("RATIONCARD", typeof(string)),
                        new DataColumn("VOTERID", typeof(string)),
                        new DataColumn("ADDITIONALID1", typeof(string)),
                        new DataColumn("ADDITIONALID2", typeof(string)),
                        new DataColumn("LANDPHONENUMBER", typeof(string)),
                        new DataColumn("MOBILEPHONE", typeof(string)),
                        new DataColumn("OWNMFIINDICATOR", typeof(string)),
                        new DataColumn("TOTALRESPONSES", typeof(string)),
                        new DataColumn("TOTALRESPONSESOWN", typeof(string)),
                        new DataColumn("TOTALRESPONSESOTHERS", typeof(string)),
                        new DataColumn("NUMOFNON_OWNOPENMFIS", typeof(string)),
                        new DataColumn("NUMOPENACCOUNT", typeof(string)),
                        new DataColumn("NUMOPENACCOUNTOWN", typeof(string)),
                        new DataColumn("NUMOPENACCOUNTNON_OWN", typeof(string)),
                        new DataColumn("NUMCLOSEDACCOUNTNON_WRITTEN_OFF", typeof(string)),
                        new DataColumn("NUMCLOSEDACCOUNTNON_WRITTEN_OFFOWN", typeof(string)),
                        new DataColumn("NUMCLOSEDACCOUNTNON_WRITTEN_OFFNON_OWN", typeof(string)),
                        new DataColumn("NUMPASTDUEACCOUNT", typeof(string)),
                        new DataColumn("NUMPASTDUEACCOUNTOWN", typeof(string)),
                        new DataColumn("NUMPASTDUEACCOUNTNON_OWN", typeof(string)),
                        new DataColumn("NUMWRITTENOFFACCOUNT", typeof(string)),
                        new DataColumn("NUMWRITTENOFFACCOUNTOWN", typeof(string)),
                        new DataColumn("NUMWRITTENOFFACCOUNTNON_OWN", typeof(string)),
                        new DataColumn("SUMOUTSTANDINGBALANCE", typeof(string)),
                        new DataColumn("SUMOUTSTANDINGBALANCEOWN", typeof(string)),
                        new DataColumn("SUMOUTSTANDINGBALANCENON_OWN", typeof(string)),
                        new DataColumn("SUMDISBURSED", typeof(string)),
                        new DataColumn("SUMDISBURSEDOWN", typeof(string)),
                        new DataColumn("SUMDISBURSEDNON_OWN", typeof(string)),
                        new DataColumn("SUMINSTALLMENTAMOUNT", typeof(string)),
                        new DataColumn("SUMINSTALLMENTAMOUNTOWN", typeof(string)),
                        new DataColumn("SUMINSTALLMENTAMOUNTNON_OWN", typeof(string)),
                        new DataColumn("SUMINSTALLMENTAMOUNTOPEN", typeof(string)),
                        new DataColumn("SUMINSTALLMENTAMOUNTOPENOWN", typeof(string)),
                        new DataColumn("SUMINSTALLMENTAMOUNTOPENNON_OWN", typeof(string)),
                        new DataColumn("SUMOVERDUEAMOUNT", typeof(string)),
                        new DataColumn("SUMOVERDUEAMOUNTOWN", typeof(string)),
                        new DataColumn("SUMOVERDUEAMOUNTNON_OWN", typeof(string)),
                        new DataColumn("SUMOVERDUEAMOUNTOPEN", typeof(string)),
                        new DataColumn("SUMOVERDUEAMOUNTOPENOWN", typeof(string)),
                        new DataColumn("SUMOVERDUEAMOUNTOPENNON_OWN", typeof(string)),
                        new DataColumn("SUMWRITTENOFFAMOUNT", typeof(string)),
                        new DataColumn("SUMWRITTENOFFAMOUNTOWN", typeof(string)),
                        new DataColumn("SUMWRITTENOFFAMOUNTNON_OWN", typeof(string)),
                        new DataColumn("ACCOUNTNUMBER", typeof(string)),
                        new DataColumn("MFINAME", typeof(string)),
                        new DataColumn("FREQUENCY", typeof(string)),
                        new DataColumn("ACCOUNTSTATUS", typeof(string)),
                        new DataColumn("CURRENTBALANCE", typeof(string)),
                        new DataColumn("INSTALLMENTAMOUNT", typeof(string)),
                        new DataColumn("OVERDUEAMOUNT", typeof(string)),
                        new DataColumn("DISBURSEDAMOUNT", typeof(string)),
                        new DataColumn("WRITTENOFFAMOUNT", typeof(string)),
                        new DataColumn("DATECLOSED", typeof(string)),
                        new DataColumn("ACCNTLEVELNAME", typeof(string)),
                        new DataColumn("NOMINEENAME", typeof(string)),
                        new DataColumn("NOMINEERELATION", typeof(string)),
                        new DataColumn("KEYPERSIONNAME", typeof(string)),
                        new DataColumn("KEYPERSONRELATION", typeof(string)),
                        new DataColumn("ACCNTLEVELDOB", typeof(string)),
                        new DataColumn("ACCNTLEVELADDRESS", typeof(string)),
                        new DataColumn("ACCNTLEVELSTATE", typeof(string)),
                        new DataColumn("ACCNTLEVELPOSTAL", typeof(string)),
                        new DataColumn("ACCNTLEVELPAN", typeof(string)),
                        new DataColumn("ACCNTLEVELRATIONCARD", typeof(string)),
                        new DataColumn("ACCNTLEVELUID", typeof(string)),
                        new DataColumn("ACCNTLEVELVOTERID", typeof(string)),
                        new DataColumn("ACCNTLEVELOTHERID", typeof(string)),
                        new DataColumn("ACCNTLEVELDATEOPEN", typeof(string)),
                        new DataColumn("ACCNTLEVELDATEREPORTED", typeof(string)),
                        new DataColumn("ACCNTLEVELDATELASTPAYMENT", typeof(string)),
                        new DataColumn("ACCNTLEVELLOANCATEGORY", typeof(string)),
                        new DataColumn("ACCNTLEVELLOANCYCLEID", typeof(string)),
                        new DataColumn("INPUTBRANCHID", typeof(string)),
                        new DataColumn("INPUTKENDRA", typeof(string)),
                        new DataColumn("ACCNTLEVELBRANCHID", typeof(string)),
                        new DataColumn("ACCNTLEVELKENDRAID", typeof(string)),
                        new DataColumn("OVERDUEDAYS", typeof(string))
                        });

                        string[] lines = File.ReadAllLines(vExcelPath + vFile);
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
                                    //if (vCol<96)
                                    //{
                                        dt.Rows[dt.Rows.Count - 1][vCol] = cell.Replace("\"", "");

                                        vCol++;
                                    //}
                                }
                            }
                        }
                    }
                    dt.AcceptChanges();
                    try
                    {
                        DataTable dtFinal = new DataView(dt).ToTable(false, "DATEOFISSUE", "HITCODE", "REPORTID", "REFERENCENUMBER","NUMPREVENQS","MEMBERNAME","AGE",
                            "UNIQUEACCNTNO","DATEOFBIRTH","ADDRESS","STATE","POSTAL","ADDLNAME1","ADDLNAMETYP1","ADDLNAME2","ADDLNAMETYP2","PAN","UID","RATIONCARD",
                            "VOTERID","ADDITIONALID1","ADDITIONALID2","LANDPHONENUMBER","MOBILEPHONE","OWNMFIINDICATOR","TOTALRESPONSES","TOTALRESPONSESOWN",
                            "TOTALRESPONSESOTHERS","NUMOFNON_OWNOPENMFIS","NUMOPENACCOUNT","NUMOPENACCOUNTOWN","NUMOPENACCOUNTNON_OWN","NUMCLOSEDACCOUNTNON_WRITTEN_OFF",
                            "NUMCLOSEDACCOUNTNON_WRITTEN_OFFOWN","NUMCLOSEDACCOUNTNON_WRITTEN_OFFNON_OWN","NUMPASTDUEACCOUNT","NUMPASTDUEACCOUNTOWN","NUMPASTDUEACCOUNTNON_OWN",
                            "NUMWRITTENOFFACCOUNT","NUMWRITTENOFFACCOUNTOWN","NUMWRITTENOFFACCOUNTNON_OWN","SUMOUTSTANDINGBALANCE","SUMOUTSTANDINGBALANCEOWN",
                            "SUMOUTSTANDINGBALANCENON_OWN","SUMDISBURSED","SUMDISBURSEDOWN","SUMDISBURSEDNON_OWN","SUMINSTALLMENTAMOUNT","SUMINSTALLMENTAMOUNTOWN",
                            "SUMINSTALLMENTAMOUNTNON_OWN","SUMINSTALLMENTAMOUNTOPEN","SUMINSTALLMENTAMOUNTOPENOWN","SUMINSTALLMENTAMOUNTOPENNON_OWN","SUMOVERDUEAMOUNT",
                            "SUMOVERDUEAMOUNTOWN","SUMOVERDUEAMOUNTNON_OWN","SUMOVERDUEAMOUNTOPEN","SUMOVERDUEAMOUNTOPENOWN","SUMOVERDUEAMOUNTOPENNON_OWN","SUMWRITTENOFFAMOUNT",
                            "SUMWRITTENOFFAMOUNTOWN","SUMWRITTENOFFAMOUNTNON_OWN","ACCOUNTNUMBER","MFINAME","FREQUENCY","ACCOUNTSTATUS","CURRENTBALANCE","INSTALLMENTAMOUNT",
                            "OVERDUEAMOUNT","DISBURSEDAMOUNT","WRITTENOFFAMOUNT","DATECLOSED","ACCNTLEVELNAME","NOMINEENAME","NOMINEERELATION","KEYPERSIONNAME",
                            "KEYPERSONRELATION","ACCNTLEVELDOB","ACCNTLEVELADDRESS","ACCNTLEVELSTATE","ACCNTLEVELPOSTAL","ACCNTLEVELPAN","ACCNTLEVELRATIONCARD","ACCNTLEVELUID",
                            "ACCNTLEVELVOTERID","ACCNTLEVELOTHERID","ACCNTLEVELDATEOPEN","ACCNTLEVELDATEREPORTED","ACCNTLEVELDATELASTPAYMENT","ACCNTLEVELLOANCATEGORY",
                            "ACCNTLEVELLOANCYCLEID","INPUTBRANCHID","INPUTKENDRA","ACCNTLEVELBRANCHID","ACCNTLEVELKENDRAID","OVERDUEDAYS");
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

        private void LoadGridFrmExl(DataTable dtEqui)
        {
            string vXmlAtten = "", vDateTxt = "";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            
            if (dtEqui.Rows.Count > 0)
            {
            //    vDateTxt = gblFuction.setStrDate(dtEqui.Rows[0]["Date"].ToString());
            //}
            //DateTime vDate;
            //CAttendance oHR = null;
            //DataTable dt = null;
            //string pMode = "";

                dtEqui.TableName = "Equifax";
            //using (StringWriter oSW = new StringWriter())
            //{
            //    dtAtten.WriteXml(oSW);
            //    vXmlAtten = oSW.ToString();
            //}

            //pMode = rdHO.Checked == true ? "HO" : rdBr.Checked == true ? "BR" : "None";
            //if (vDateTxt != "")
            //{
            //    vDate = gblFuction.setDate(vDateTxt);
            //    txtEftDt.Text = vDateTxt;
            //    oHR = new CAttendance();
            //    dt = oHR.GetEmployeeAttendanceExl(vXmlAtten, vBrCode, vDate, pMode);
                gvSchdl.DataSource = dtEqui;
                gvSchdl.DataBind();
            }
            else
            {
                gvSchdl.DataSource = null;
                gvSchdl.DataBind();
            }
        }

        private Boolean ValidateExl(DataTable dtAtten)
        {
            Boolean vResult = true;
            //CAttendance oHR = null;
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            //if (rdBr.Checked == true)
            //    vBrCode = ddlBr.SelectedValue;
            //string vXmlAtten = "", pMode = "", vMsg = "";
            //pMode = rdHO.Checked == true ? "HO" : rdBr.Checked == true ? "BR" : "None";

            //foreach (DataColumn col in dtAtten.Columns)
            //{
            //    foreach (DataRow row in dtAtten.Rows)
            //    {
            //        if (row[col].ToString() == "")
            //        {
            //            gblFuction.AjxMsgPopup("Excel sheet cannot contain blank cells.");
            //            return false;
            //        }
            //    }
            //}

            //dtAtten.TableName = "Attendance";
            //using (StringWriter oSW = new StringWriter())
            //{
            //    dtAtten.WriteXml(oSW);
            //    vXmlAtten = oSW.ToString();
            //}

            //try
            //{
            //    oHR = new CAttendance();
            //    vMsg = oHR.ChkImpAttendance(vXmlAtten, vBrCode, pMode, this.UserID);
            //    if (vMsg != "")
            //    {
            //        gblFuction.AjxMsgPopup(vMsg);
            //        vResult = false;
            //    }
            //    else
            vResult = true;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    oHR = null;
            //}
            return vResult;
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
            //ds = ExcelToDS(vExcelPath + vFile);
            dt = GetTable();
            string vSaveRec = "";

            if (dt.Rows.Count > 0)
            {
                vSaveRec = SaveRecord(dt);
                if (vSaveRec == "0" || vSaveRec == "")
                {
                    FileInfo vfile = new FileInfo(vExcelPath + vFile);
                    if (vfile.Exists)
                    {
                        vfile.Delete();
                    }
                    gblFuction.AjxMsgPopup("Equifax Data Successfully Updated ..");
                }
                else //if (vSaveRec != "")
                {
                    FileInfo vfile = new FileInfo(vExcelPath + vFile);
                    if (vfile.Exists)
                    {
                        vfile.Delete();
                    }
                    gblFuction.AjxMsgPopup("Equifax Data Successfully Updated ..But LoanApplicationId " + vSaveRec + " are not Updated");
                }
            }
        }

        private DataTable GetTable()
        {
            DataTable dt = new DataTable("ImportEquifax");

            DataColumn dc1 = new DataColumn("DATEOFISSUE");
            dt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn("REPORTID");
            dt.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn("REFERENCENUMBER");
            dt.Columns.Add(dc3);
            DataColumn dc4 = new DataColumn("DATECLOSED");
            dt.Columns.Add(dc4);
            DataColumn dc5 = new DataColumn("CURRENTBALANCE");
            dt.Columns.Add(dc5);
            DataColumn dc6 = new DataColumn("MFINAME");
            dt.Columns.Add(dc6);
            DataColumn dc7 = new DataColumn("OVERDUEAMOUNT");
            dt.Columns.Add(dc7);
            DataColumn dc8 = new DataColumn("ACCOUNTNUMBER");
            dt.Columns.Add(dc8);
            foreach (GridViewRow gR in gvSchdl.Rows)
            {
                
                DataRow dR = dt.NewRow();
                if (gR.Cells[0].Text.Trim().Length == 10)
                {
                    dR["DATEOFISSUE"] = gR.Cells[0].Text.Trim().Substring(6, 4) + "-" + gR.Cells[0].Text.Trim().Substring(3, 2) + "-" + gR.Cells[0].Text.Trim().Substring(0, 2);
                    dR["REPORTID"] = gR.Cells[2].Text.Trim();
                    dR["REFERENCENUMBER"] = gR.Cells[3].Text.Trim();
                    dR["DATECLOSED"] = gR.Cells[71].Text.Trim();
                    dR["CURRENTBALANCE"] = gR.Cells[66].Text.Trim();
                    dR["MFINAME"] = gR.Cells[63].Text.Trim();
                    dR["OVERDUEAMOUNT"] = gR.Cells[68].Text.Trim();
                    dR["ACCOUNTNUMBER"] = gR.Cells[62].Text.Trim();
                    dt.Rows.Add(dR);
                }
                

            }
            return dt;
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
        private string SaveRecord(DataTable dt)
        {
            bool vRes = true;
            string vXmlA = "";
            int vErr = 0;
            string vMsg = "";
            CNpsMember oAv = null;

            foreach (DataColumn dc in dt.Columns)
            {
               // dr["DATEOFISSUE"] = gblFuction.setDate(dr["DATEOFISSUE"].ToString().Substring(0, 10));
                if (object.ReferenceEquals(dc.DataType, typeof(DateTime)))
                {
                    dc.DateTimeMode = DataSetDateTime.Unspecified;
                }
            }
            dt.AcceptChanges();
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXmlA = oSW.ToString();
                
            }
            //vXmlA.Replace("xs:dateTime", "xs:string");
            //XML.Replace("xs:dateTime", "xs:string");
            //if (validate(vXmlA.Replace("_x0020_", "").Trim().Replace("'", "")) == false)
            //    return false;
            //Though it is a different operation Still this CNpsMember Class is used
            oAv = new CNpsMember();
            //vErr = oAv.ImportEquifax(vXmlA.Replace("_x0020_", "").Trim().Replace("'", ""), this.UserID, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
            vMsg = oAv.ImportEquifax(vXmlA.Replace("_x0020_", "").Trim().Replace("'", ""), this.UserID, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
            return vMsg;
        }
    }
}
