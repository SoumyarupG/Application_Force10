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
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using CENTRUM.WebSrvcs;
using System.Net.Mail;

namespace CENTRUM.WebPages.Private.BCOperation
{
    public partial class BC_File_Receive : CENTRUMBase
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
        /// <param name="vFrmdate"></param>
        /// <param name="vTodate"></param>
        /// <param name="vFileType"></param>
        protected void GetFilesFromFolder(DateTime vFrmdate, DateTime vTodate, string vFileType)
        {
            string vFolderPath = "E:\\FTPFolder\\IDBI_File_Database\\JAGARAN\\Received\\" + vFileType;
            DateTime vMydate;
            DataTable dt = null;
            DataRow dr = null;
            string vFileNm = "", vFileNmCut = "", vDate = "", vFile_NM="";
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
                        vDate = vMydate.ToString("dd/MMM/yyyy").Replace("/", "-");
                        vFileNm = "";
                        if (vFileType == "CUSTSUC")
                        {
                            vFile_NM = "SUC_CUST_JAGA_" + vDate + " *.txt";
                        }
                        else if (vFileType == "CUSTREJ")
                        {
                            vFile_NM = "REJ_CUST_JAGA_" + vDate + " *.txt";
                        }
                        else if (vFileType == "JLGSUC")
                        {
                            vFile_NM = "SUC_JLG_JAGA_" + vDate + " *.txt";
                        }
                        else if (vFileType == "JLGREJ")
                        {
                            vFile_NM = "REJ_JLG_JAGA_" + vDate + " *.txt";
                        }
                        else if (vFileType == "JCONFSUC")
                        {
                            vFile_NM = "SUC_CONF_JLG_JAGA_" + vDate + " *.txt";
                        }

                        foreach (string file in Directory.EnumerateFiles(vFolderPath, vFile_NM))
                        {
                            vFileNm = file;
                            switch (vFileType)
                            {
                                case "CUSTSUC":
                                    vFileNmCut = vFileNm.Substring(49);
                                    break;
                                case "CUSTREJ":
                                    vFileNmCut = vFileNm.Substring(49);
                                    break;
                                case "JLGSUC":
                                    vFileNmCut = vFileNm.Substring(48);
                                    break;
                                case "JLGREJ":
                                    vFileNmCut = vFileNm.Substring(48);
                                    break;
                                default:
                                    vFileNmCut = vFileNm.Substring(50);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            string vFolderPath = "E:\\FTPFolder\\IDBI_File_Database\\JAGARAN\\Received\\CUSTSUC";
            string vFolderPath1 = "E:\\FTPFolder\\IDBI_File_Database\\JAGARAN\\Received\\CUSTREJ";
            string vFolderPath2 = "E:\\FTPFolder\\IDBI_File_Database\\JAGARAN\\Received\\JLGSUC";
            string vFolderPath3 = "E:\\FTPFolder\\IDBI_File_Database\\JAGARAN\\Received\\JLGREJ";
            string vFolderPath4 = "E:\\FTPFolder\\IDBI_File_Database\\JAGARAN\\Received\\JCONFSUC";


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

                    // Customer Success File Search
                    foreach (string file in Directory.EnumerateFiles(vFolderPath, "SUC_CUST_JAGA_*.txt"))
                    {
                        vFileNm = file;
                        ViewState["vFile"] = vFileNm;
                        vFileNmCut = vFileNm.Substring(49);

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
                    // Customer REJECT File Search
                    foreach (string file in Directory.EnumerateFiles(vFolderPath1, "REJ_CUST_JAGA_*.txt"))
                    {
                        vFileNm = file;
                        ViewState["vFile"] = vFileNm;
                        vFileNmCut = vFileNm.Substring(49);

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
                    // JLG Success File Search
                    foreach (string file in Directory.EnumerateFiles(vFolderPath2, "SUC_JLG_JAGA_*.txt"))
                    {
                        vFileNm = file;
                        ViewState["vFile"] = vFileNm;
                        vFileNmCut = vFileNm.Substring(48);

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
                    // JLG REJECT File Search
                    foreach (string file in Directory.EnumerateFiles(vFolderPath3, "REJ_JLG_JAGA_*.txt"))
                    {
                        vFileNm = file;
                        ViewState["vFile"] = vFileNm;
                        vFileNmCut = vFileNm.Substring(48);

                        int Vara = File.ReadAllText(vFileNm).Contains(vSearchString) ? 1 : 0;
                        if (Vara == 1)
                        {
                            vFileRead = 4;
                            dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            vRow = vRow + 1;
                            dt.Rows[vRow][0] = vFileNmCut;
                            dt.Rows[vRow][1] = vFileNm;
                        }
                    }
                    // JLG REJECT File Search
                    foreach (string file in Directory.EnumerateFiles(vFolderPath4, "SUC_CONF_JLG_JAGA*.txt"))
                    {
                        vFileNm = file;
                        ViewState["vFile"] = vFileNm;
                        vFileNmCut = vFileNm.Substring(50);

                        int Vara = File.ReadAllText(vFileNm).Contains(vSearchString) ? 1 : 0;
                        if (Vara == 1)
                        {
                            vFileRead = 5;
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
                    else if (vSearchString.Substring(3, 2) == "BC")
                    {
                        lblSearchResult.Text = "You have Entered URNID...Data Searched in Customer ,JCONF and JLG Success Related File(s)";
                    }
                    else if (vSearchString.Substring(3, 2) != "BC")
                    {
                        lblSearchResult.Text = "You have Entered JLGID...Data Searched in JLG Related File(s)";
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");

        }
        protected void gvFile_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDown")
            {
                string filename = e.CommandArgument.ToString();
                Int32 flength = filename.Length;
                string fname;
                if (flength == 75)
                {
                    fname = filename.Substring(47);
                }
                else if (flength == 76)
                {
                    fname = filename.Substring(48);
                }
                else
                {
                    fname = filename.Substring(46);
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
  