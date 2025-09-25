using System.Web.UI.WebControls;
using FORCECA;
using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web;

namespace CENTRUM.WebPages.Private.BCOperation
{
    public partial class BC_RepayFileTracking : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["MyDT"] = null;
                ViewState["SFTPDOWNLOAD"] = null;
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);


                this.Menu = false;
                this.PageHeading = "Repayment File Tracking";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuRepayFileTracking);
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
        protected void btnShow_Click(object sender, EventArgs e)
        {

            DateTime dtFrm = gblFuction.setDate(txtFrmDt.Text);
            DateTime dtTo = gblFuction.setDate(txtToDt.Text);
            GetFilesFromFolder(dtFrm, dtTo, ddlFileType.SelectedValue);
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
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            
            string vFileNm = "";
            dt = (DataTable)ViewState["MyDT"];
            dt.Columns.Remove("DC_ID");
            dt.Columns.Remove("vFileName");

            if (ddlDS.SelectedValue == "S")
            {
                dt.Columns[2].ColumnName = "No of Loan";
                dt.Columns[5].ColumnName = "Description";
            }
            System.Web.UI.WebControls.GridView gv = new System.Web.UI.WebControls.GridView();
            gv.DataSource = dt;
            gv.DataBind();

            tdx.Controls.Add(gv);
            tdx.Visible = false;
            vFileNm = "attachment;filename=BC_RepayFile_Track_("+ ddlDS.SelectedItem.Text+").xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan="+ dt.Columns.Count+"><b><u><font size='5'>BC RepayFile_Track_(" + ddlDS.SelectedItem.Text + ")</font></u></b></td></tr>");
            gv.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
            htw.WriteLine("</table>");
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentType.ToString();
            for (int i = 0; i < gv.Rows.Count; i++)
            {
                GridViewRow rows = gv.Rows[i];
                rows.Attributes.Add("class", "textmode");
            }
            gv.RenderControl(htw);
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.ContentType = "application/text";
            Response.Write(sw.ToString());
            Response.End();

        }
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{

        //    DateTime dtFrm = gblFuction.setDate(txtFrmDt.Text);
        //    DateTime dtTo = gblFuction.setDate(txtToDt.Text);
        //    GetAllFilesFromFolder(txtSearch.Text.Trim());
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vFrmdate"></param>
        /// <param name="vTodate"></param>
        /// <param name="vFileType"></param>
        //protected void GetAllFilesFromFolder(string vSearchString)
        //{
        //    string vFolderPath = "E:\\FTPFolder\\IDBI_File_DataBase\\Send\\CUST";
        //    string vFolderPath1 = "E:\\FTPFolder\\IDBI_File_DataBase\\Send\\JLG";
        //    string vFolderPath2 = "E:\\FTPFolder\\IDBI_File_DataBase\\Send\\JCONF";
        //    DataTable dt = null;
        //    DataRow dr = null;
        //    string vFileNm = "", vFileNmCut = "";
        //    Int32 vRow = -1;
        //    Int32 vFileRead = 0;
        //    try
        //    {
        //        if (System.IO.Directory.Exists(vFolderPath))
        //        {
        //            dt = new DataTable("Table1");
        //            dt.Columns.Add("FileName");
        //            dt.Columns.Add("vFileName");

        //            dt.AcceptChanges();

        //            // Customer File Search
        //            foreach (string file in Directory.EnumerateFiles(vFolderPath, "MARG_CUST_*.txt"))
        //            {
        //                vFileNm = file;
        //                ViewState["vFile"] = vFileNm;
        //                vFileNmCut = vFileNm.Substring(42);

        //                int Vara = File.ReadAllText(vFileNm).Contains(vSearchString) ? 1 : 0;
        //                if (Vara == 1)
        //                {
        //                    vFileRead = 1;
        //                    dr = dt.NewRow();
        //                    dt.Rows.Add(dr);
        //                    vRow = vRow + 1;
        //                    dt.Rows[vRow][0] = vFileNmCut;
        //                    dt.Rows[vRow][1] = vFileNm;
        //                }
        //            }
        //            // JLG File Search
        //            foreach (string file in Directory.EnumerateFiles(vFolderPath1, "MARG_JLG_*.txt"))
        //            {
        //                vFileNm = file;
        //                ViewState["vFile"] = vFileNm;
        //                vFileNmCut = vFileNm.Substring(41);

        //                int Vara = File.ReadAllText(vFileNm).Contains(vSearchString) ? 1 : 0;
        //                if (Vara == 1)
        //                {
        //                    vFileRead = 2;
        //                    dr = dt.NewRow();
        //                    dt.Rows.Add(dr);
        //                    vRow = vRow + 1;
        //                    dt.Rows[vRow][0] = vFileNmCut;
        //                    dt.Rows[vRow][1] = vFileNm;
        //                }
        //            }
        //            // JCONF File Search
        //            foreach (string file in Directory.EnumerateFiles(vFolderPath2, "MARG_JCONF_*.txt"))
        //            {
        //                vFileNm = file;
        //                ViewState["vFile"] = vFileNm;
        //                vFileNmCut = vFileNm.Substring(43);

        //                int Vara = File.ReadAllText(vFileNm).Contains(vSearchString) ? 1 : 0;
        //                if (Vara == 1)
        //                {
        //                    vFileRead = 3;
        //                    dr = dt.NewRow();
        //                    dt.Rows.Add(dr);
        //                    vRow = vRow + 1;
        //                    dt.Rows[vRow][0] = vFileNmCut;
        //                    dt.Rows[vRow][1] = vFileNm;
        //                }
        //            }
        //            dt.AcceptChanges();
        //            gvFile.DataSource = dt;
        //            gvFile.DataBind();

        //            if (vFileRead == 0)
        //            {
        //                lblSearchResult.Text = "No File(s) with the given Data";
        //            }
        //            else if (vSearchString.Substring(3, 2) == "BC")
        //            {
        //                lblSearchResult.Text = "You have Entered URNID...Data Searched in Customer and JLG File(s)";
        //            }
        //            else if (vSearchString.Substring(3, 2) != "BC")
        //            {
        //                lblSearchResult.Text = "You have Entered JLGID...Data Searched in JCONF and JLG File(s)";
        //            }

        //        }
        //        else
        //        {
        //            gblFuction.MsgPopup("No Folder Found");
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dt = null;

        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vFrmdate"></param>
        /// <param name="vTodate"></param>
        /// <param name="vFileType"></param>
        protected void GetFilesFromFolder(DateTime vFrmdate, DateTime vTodate, string vFileType)
        {
            string vFolderPath = "E:\\FTPFolder\\IDBI_File_Database\\JAGARAN\\Send\\" + vFileType;
            DateTime vMydate;
            DataTable dt = null;
            DataRow dr = null;
            string vFileNm = "", vFileNmCut = "", vDate = "";
            Int32 vRow = -1;
            try
            {
                if (System.IO.Directory.Exists(vFolderPath))
                {
                    dt = new DataTable("Table1");
                    dt.Columns.Add("SlNo");
                    dt.Columns.Add("FileName");
                    dt.Columns.Add("vFileName");
                    dt.Columns.Add("IDBI Loan Acc No");
                    dt.Columns.Add("Sol ID");
                    dt.Columns.Add("DC_ID");
                    dt.Columns.Add("Repayment Amount");
                    dt.Columns.Add("URN_ID");
                    dt.Columns.Add("UpLoad_Date");
                   
                    dt.AcceptChanges();

                    vMydate = vFrmdate;
                    while (vMydate <= vTodate)
                    {
                        vDate = vMydate.ToString("dd/MM/yyyy").Replace("/", "-");
                        vFileNm = "";
                        foreach (string file in Directory.EnumerateFiles(vFolderPath, "MARG_" + vFileType + "_" + vDate + "_*.txt"))
                        {
                            vFileNm = file;
                            ViewState["vFile"] = vFileNm;
                            vFileNmCut = vFileNm.Substring(41);
                            
                            
                           

                            // Read The File 
                            string[] col;
                            string[] lines = File.ReadAllLines(vFileNm);
                            foreach (string line in lines)
                            {
                                
                                if (line.Trim() != "")
                                {
                                    if (ddlDS.SelectedValue == "D" && line.IndexOf("COLFI")>0)
                                    {
                                        dr = dt.NewRow();
                                        dt.Rows.Add(dr);
                                        vRow = vRow + 1;
                                       col = line.Split(new char[] { '|' });
                                        dt.Rows[vRow][0] = vRow + 1;
                                        dt.Rows[vRow][1] = vFileNmCut;
                                        dt.Rows[vRow][2] = vFileNm;
                                        for (int j = 0; j < dt.Columns.Count - 3; j++)
                                        {
                                            if (j != 4)
                                            {
                                                dt.Rows[vRow][j+3] = col[j];
                                            }
                                            else
                                            {
                                                dt.Rows[vRow][j+3] = col[j].Substring(10);
                                            }
                                        }
                                        
                                    }
                                    if (ddlDS.SelectedValue == "S" && line.IndexOf("COLFI") <0)
                                    {
                                        dr = dt.NewRow();
                                        dt.Rows.Add(dr);
                                        vRow = vRow + 1;
                                        col = line.Split(new char[] { '|' });
                                        dt.Rows[vRow][0] = vRow + 1;
                                        dt.Rows[vRow][1] = vFileNmCut;
                                        dt.Rows[vRow][2] = vFileNm;
                                        for (int j = 0; j < dt.Columns.Count - 3; j++)
                                        {
                                            if (j != 0)
                                            {
                                                dt.Rows[vRow][j+3] = col[j];
                                            }
                                            else
                                            {
                                                dt.Rows[vRow][j+3] =  (lines.Length-1).ToString() ;
                                            }
                                        }
                                        
                                    }
                                }
                                
                                dt.AcceptChanges();
                            }
                            // End of 

                        }
                        dt.AcceptChanges();
                        
                        vMydate = vMydate.AddDays(1);
                    }
                    dt.AcceptChanges();
                    ViewState["MyDT"] = dt;
                    gvFile.DataSource = dt;
                    gvFile.DataBind();
                }
                else
                {
                    gblFuction.MsgPopup("No Folder Found");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="outputFilePath"></param>
        static string Read(DataTable dt, string outputFilePath)
        {
            string fileName = outputFilePath;

            string vXmlA = "";

            if (File.Exists(fileName))
            {
                int i = 0;

                string[] lines = File.ReadAllLines(fileName);
                foreach (string line in lines)
                {
                    if (line.Trim() != "")
                    {
                        string[] col = line.Split(new char[] { '|' });
                        DataRow dr = dt.NewRow();
                        dt.Rows.Add(dr);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            dt.Rows[i][j] = col[j];
                        }
                        dt.AcceptChanges();
                        i = i + 1;
                    }
                }
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW, XmlWriteMode.IgnoreSchema);
                    vXmlA = oSW.ToString();
                    oSW.Flush();
                    oSW.Close();

                }
            }
            else
            {
                vXmlA = "File does not exists";
            }
            return vXmlA;
        }
        //protected void gvFile_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    Int32 vindex = 0;
        //    if (e.CommandName == "cmdDown")
        //    {
        //        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //        LinkButton btnDown = (LinkButton)row.FindControl("btnDown");
        //        vindex = Convert.ToInt32(e.CommandArgument);
        //        row = gvFile.Rows[vindex];
        //        string fName = row.Cells[0].Text;
        //        Response.ContentType = "application/octet-stream";
        //        Response.AddHeader("Content-Disposition", "attachment;filename=" + fName);
        //        Response.WriteFile(fName);
        //        Response.End();
        //    }
        //}

        protected void gvFile_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDown")
            {
                string filename = e.CommandArgument.ToString();
                Int32 flength = filename.Length;
                string fname;
                if (flength == 69)
                {
                    fname = filename.Substring(42);
                }
                else if (flength == 70)
                {
                    fname = filename.Substring(43);
                }
                else
                {
                    fname = filename.Substring(41);
                }
                if (fname != "")
                {
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + fname);
                    Response.WriteFile(filename);
                    Response.Flush();
                    Response.End();
                }
            }

        }

        //protected void btnDown_Click(object sender, EventArgs e)
        //{
        //    //Int32 vIndex = 0;
        //    //GridViewRow row = (GridViewRow)(((LinkButton)sender).Parent.Parent);
        //    //row = gvFile.Rows[vIndex];
        //    //string fName = row.Cells[0].Text;
        //    //Response.ContentType = ContentType;
        //    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fName));
        //    //Response.WriteFile(fName);
        //    //Response.End();

        //    string filePath = (sender as LinkButton).CommandArgument;
        //    Response.ContentType = ContentType;
        //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
        //    Response.WriteFile(filePath);
        //    Response.End();
        //}
    }
}
       