using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class UploadPran : CENTRUMBase 
    {
        private static string vFile;
        private static string vExcelPath = ConfigurationManager.AppSettings["DBPath"];
        protected void Page_Load(object sender, EventArgs e)
        {

        }



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
                myCommand.Fill(ds, "AutoVouEntry");
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
                    gblFuction.AjxMsgPopup("Upload Successful !!");

                }
            }
            else
                gblFuction.AjxMsgPopup("Please Select a File");
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

            ds = ExcelToDS(vExcelPath + vFile);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                if (SaveRecord(dt))
                {
                    FileInfo vfile = new FileInfo(vExcelPath + vFile);
                    if (vfile.Exists)
                    {
                        vfile.Delete();
                    }
                    gblFuction.AjxMsgPopup("Pran Upload Successfull ..");
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

            oAv = new CNpsMember();
            vErr = oAv.NPS_UpdateMemPran(vXmlA.Replace("_x0020_", "").Trim().Replace("'", ""), this.UserID);

            return vRes;
        }
    }
}
