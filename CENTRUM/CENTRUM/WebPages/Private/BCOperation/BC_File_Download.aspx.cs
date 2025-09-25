using System.Web.UI.WebControls;
using FORCECA;
using System;
using System.Data;
using System.IO;
namespace CENTRUM.WebPages.Private.BCOperation
{
    public partial class BC_File_Download : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
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
                this.PageHeading = "SFTP FILE DOWNLOAD";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuSFTPFILEDOWNLOAD);
            }
            catch
            {
                Response.Redirect("~/Margdarshak.aspx", false);
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
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            DateTime dtFrm = gblFuction.setDate(txtFrmDt.Text);
            DateTime dtTo = gblFuction.setDate(txtToDt.Text);
            GetAllFilesFromFolder(txtSearch.Text.Trim());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vFrmdate"></param>
        /// <param name="vTodate"></param>
        /// <param name="vFileType"></param>
        protected void GetAllFilesFromFolder(string vSearchString)
        {
            string vFolderPath = "E:\\FTPFolder\\IDBI_File_Database\\JAGARAN\\Send\\CUST";  
            string vFolderPath1 = "E:\\FTPFolder\\IDBI_File_Database\\JAGARAN\\Send\\JLG";
            string vFolderPath2 =  "E:\\FTPFolder\\IDBI_File_Database\\JAGARAN\\Send\\JCONF";
               
            DataTable dt = null;
            DataRow dr = null;
            string vFileNm = "", vFileNmCut = "";
            Int32 vRow = -1;
            Int32 vFileRead = 0;
            try
            {
                if (System.IO.Directory.Exists(vFolderPath))
                {
                    dt = new DataTable("Table1");
                    dt.Columns.Add("FileName");
                    dt.Columns.Add("vFileName");

                    dt.AcceptChanges();
                    
                    // Customer File Search
                    foreach (string file in Directory.EnumerateFiles(vFolderPath, "JAGA_CUST_*.txt"))
                    {
                        vFileNm = file;
                        ViewState["vFile"] = vFileNm;
                        vFileNmCut = vFileNm.Substring(42);

                        int Vara = File.ReadAllText(vFileNm).Contains(vSearchString) ? 1 : 0;
                        if (Vara == 1)
                        {
                            vFileRead = 1;
                            dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            vRow = vRow + 1;
                            dt.Rows[vRow][0] = vFileNmCut;
                            dt.Rows[vRow][1] = vFileNm;
                        }
                    }
                    // JLG File Search
                    foreach (string file in Directory.EnumerateFiles(vFolderPath1, "JAGA_JLG_*.txt"))
                    {
                        vFileNm = file;
                        ViewState["vFile"] = vFileNm;
                        vFileNmCut = vFileNm.Substring(41);

                        int Vara = File.ReadAllText(vFileNm).Contains(vSearchString) ? 1 : 0;
                        if (Vara == 1)
                        {
                            vFileRead = 2;
                            dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            vRow = vRow + 1;
                            dt.Rows[vRow][0] = vFileNmCut;
                            dt.Rows[vRow][1] = vFileNm;
                        }
                    }
                    // JCONF File Search
                    foreach (string file in Directory.EnumerateFiles(vFolderPath2, "JAGA_JCONF_*.txt"))
                    {
                        vFileNm = file;
                        ViewState["vFile"] = vFileNm;
                        vFileNmCut = vFileNm.Substring(43);

                        int Vara = File.ReadAllText(vFileNm).Contains(vSearchString) ? 1 : 0;
                        if (Vara == 1)
                        {
                            vFileRead = 3;
                            dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            vRow = vRow + 1;
                            dt.Rows[vRow][0] = vFileNmCut;
                            dt.Rows[vRow][1] = vFileNm;
                        }
                    }   
                    dt.AcceptChanges();
                    gvFile.DataSource = dt;
                    gvFile.DataBind();

                    if (vFileRead == 0)
                    {
                        lblSearchResult.Text = "No File(s) with the given Data";
                    }
                    else if (vSearchString.Substring(3,2)=="BC")
                    {
                        lblSearchResult.Text = "You have Entered URNID...Data Searched in Customer and JLG File(s)";
                    }
                    else if (vSearchString.Substring(3,2)!="BC")
                    {
                        lblSearchResult.Text = "You have Entered JLGID...Data Searched in JCONF and JLG File(s)";
                    }
                    
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
        /// <param name="vFrmdate"></param>
        /// <param name="vTodate"></param>
        /// <param name="vFileType"></param>
        protected void GetFilesFromFolder(DateTime vFrmdate,DateTime vTodate,string vFileType)
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
                    dt.Columns.Add("FileName");
                    dt.Columns.Add("vFileName");

                    dt.AcceptChanges();
                    vMydate = vFrmdate;
                    while (vMydate <= vTodate)
                    {
                        vDate = vMydate.ToString("dd/MM/yyyy").Replace("/", "-");
                        vFileNm = "";
                        foreach (string file in Directory.EnumerateFiles(vFolderPath, "JAGA_" + vFileType + "_" + vDate + "_*.txt"))
                        {
                            vFileNm = file;
                            ViewState["vFile"] = vFileNm; 
                            switch (vFileType)
                            {
                                case "CUST":
                                    vFileNmCut = vFileNm.Substring(42);
                                    break;
                                case "JCONF":
                                    vFileNmCut = vFileNm.Substring(43);
                                    break;
                                default:
                                    vFileNmCut = vFileNm.Substring(41);
                                    break;
                            }


                            dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            vRow = vRow + 1;
                            dt.Rows[vRow][0] = vFileNmCut;
                            dt.Rows[vRow][1] = vFileNm;

                        }
                        vMydate = vMydate.AddDays(1);
                    }
                    dt.AcceptChanges();
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

    }
}
   