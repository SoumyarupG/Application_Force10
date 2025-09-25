using System;
using System.Data;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HiMarkDataSubmission : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch(Session[gblValue.UserName].ToString());
                txtAsDt.Text = Session[gblValue.LoginDate].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }
        
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "HighMark Data Submission";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOHghMrkDataSub);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "High Mark Submission Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void rdbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbMode.SelectedValue == "M")
            {
                tbl1Dt.Visible = true;
                tbl2Dt.Visible = false;
            }
            else if (rdbMode.SelectedValue == "W")
            {
                tbl1Dt.Visible = false;
                tbl2Dt.Visible = true;
            }
        }
        
        private void GetBranch()
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
            {
                if (chkBr.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
            }
            ViewState["ID"] = strin;
        }
        
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
            ViewState["ID"] = null;
            try
            {

                if (dt.Rows.Count > 0)
                {
                    chkBr.DataSource = dt;
                    chkBr.DataTextField = "BranchName";
                    chkBr.DataValueField = "BranchCode";
                    chkBr.DataBind();
                    CheckAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }

        }

        private void CheckAll()
        {
            Int32 vRow;
            string strin = "";
            if (ddlSel.SelectedValue == "C")
            {
                chkBr.Enabled = false;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                {
                    chkBr.Items[vRow].Selected = true;
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
                ViewState["ID"] = strin;
            }
            else if (ddlSel.SelectedValue == "B")
            {
                ViewState["ID"] = null;
                chkBr.Enabled = true;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                    chkBr.Items[vRow].Selected = false;
            }
        }

        protected void ddlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (rdbMode.SelectedValue == "M")
                PrintMonthlyRpt();
            if (rdbMode.SelectedValue == "W")
                PrintWeeklyRpt();
        }

        private void PrintMonthlyRpt()
        {
            DataTable dtAdd = null, dtMem = null, dtAcc = null;
            CHighMarkDataSubmission oHM = null;
            string vFolderPath = "C:\\HighMark_WebM";
            string vBrCode = "A";
            string vFileNm = "";

            try
            {
                if (ddlSel.SelectedValue == "B")
                {
                    GetBranch();
                    vBrCode = Convert.ToString(ViewState["ID"]);
                }
                if (chOption.Items[0].Selected == true)
                {
                    oHM = new CHighMarkDataSubmission();
                    dtAdd = oHM.GetHighMarkAddress(gblFuction.setDate(txtAsDt.Text.Trim()), vBrCode);
                    vFileNm = vFolderPath + "\\Address_DataM.txt";

                    try
                    {
                        if (System.IO.Directory.Exists(vFolderPath))
                        {
                            foreach (var file in Directory.GetFiles(vFolderPath))
                            {
                                if (File.Exists(vFileNm) == true)
                                    File.Delete(vFileNm);
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(vFolderPath);
                        }
                        Write(dtAdd, vFileNm);
                        downloadfile(vFileNm);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                if (chOption.Items[1].Selected == true)
                {
                    oHM = new CHighMarkDataSubmission();
                    dtMem = oHM.GetHighMarkMember(gblFuction.setDate(txtAsDt.Text.Trim()), vBrCode);
                    vFileNm = vFolderPath + "\\Member_DataM.txt";

                    try
                    {
                        if (System.IO.Directory.Exists(vFolderPath))
                        {
                            foreach (var file in Directory.GetFiles(vFolderPath))
                            {
                                if (File.Exists(vFileNm) == true)
                                    File.Delete(vFileNm);
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(vFolderPath);
                        }
                        Write(dtMem, vFileNm);
                        downloadfile(vFileNm);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                if (chOption.Items[2].Selected == true)
                {
                    oHM = new CHighMarkDataSubmission();
                    dtAcc = oHM.GetHighMarkAccount(gblFuction.setDate(txtAsDt.Text.Trim()), vBrCode);
                    vFileNm = vFolderPath + "\\Account_DataM.txt";

                    try
                    {
                        if (System.IO.Directory.Exists(vFolderPath))
                        {
                            foreach (var file in Directory.GetFiles(vFolderPath))
                            {
                                if (File.Exists(vFileNm) == true)
                                    File.Delete(vFileNm);
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(vFolderPath);
                        }
                        Write(dtAcc, vFileNm);
                        downloadfile(vFileNm);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                gblFuction.MsgPopup("Done");
                btnExit.Enabled = true;

            }
        }

        private void PrintWeeklyRpt()
        {
            DataTable dtAdd = null, dtMem = null, dtAcc = null;
            CHighMarkDataSubmission oHM = null;
            string vFolderPath = "C:\\HighMark_WebW";
            string vBrCode = "A";
            string vFileNm = "";

            try
            {
                if (ddlSel.SelectedValue == "B")
                {
                    GetBranch();
                    vBrCode = Convert.ToString(ViewState["ID"]);
                }
                if (chOption.Items[0].Selected == true)
                {
                    oHM = new CHighMarkDataSubmission();
                    dtAdd = oHM.GetHighMarkAddressW(gblFuction.setDate(txtFrmDt.Text.Trim()), gblFuction.setDate(txtToDt.Text.Trim()), vBrCode);
                    vFileNm = vFolderPath + "\\Address_DataW.txt";

                    try
                    {
                        if (System.IO.Directory.Exists(vFolderPath))
                        {
                            foreach (var file in Directory.GetFiles(vFolderPath))
                            {
                                if (File.Exists(vFileNm) == true)
                                    File.Delete(vFileNm);
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(vFolderPath);
                        }
                        Write(dtAdd, vFileNm);
                        downloadfile(vFileNm);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                if (chOption.Items[1].Selected == true)
                {
                    oHM = new CHighMarkDataSubmission();
                    dtMem = oHM.GetHighMarkMemberW(gblFuction.setDate(txtFrmDt.Text.Trim()), gblFuction.setDate(txtToDt.Text.Trim()), vBrCode);
                    vFileNm = vFolderPath + "\\Member_DataW.txt";

                    try
                    {
                        if (System.IO.Directory.Exists(vFolderPath))
                        {
                            foreach (var file in Directory.GetFiles(vFolderPath))
                            {
                                if (File.Exists(vFileNm) == true)
                                    File.Delete(vFileNm);
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(vFolderPath);
                        }
                        Write(dtMem, vFileNm);
                        downloadfile(vFileNm);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                if (chOption.Items[2].Selected == true)
                {
                    oHM = new CHighMarkDataSubmission();
                    dtAcc = oHM.GetHighMarkAccountW(gblFuction.setDate(txtFrmDt.Text.Trim()), gblFuction.setDate(txtToDt.Text.Trim()), vBrCode);
                    vFileNm = vFolderPath + "\\Account_DataW.txt";

                    try
                    {
                        if (System.IO.Directory.Exists(vFolderPath))
                        {
                            foreach (var file in Directory.GetFiles(vFolderPath))
                            {
                                if (File.Exists(vFileNm) == true)
                                    File.Delete(vFileNm);
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(vFolderPath);
                        }
                        Write(dtAcc, vFileNm);
                        downloadfile(vFileNm);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                gblFuction.MsgPopup("Done");
                btnExit.Enabled = true;

            }
        }

        static void Write(DataTable dt, string outputFilePath)
        {
            int[] maxLengths = new int[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                maxLengths[i] = dt.Columns[i].ColumnName.Length;
                foreach (DataRow row in dt.Rows)
                {
                    if (!row.IsNull(i))
                    {
                        int length = row[i].ToString().Length;
                        if (length > maxLengths[i])
                        {
                            maxLengths[i] = length;
                        }
                    }
                }
            }
            using (StreamWriter sw = new StreamWriter(outputFilePath, false))
            {
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!row.IsNull(i))
                        {
                            sw.Write(row[i].ToString().PadRight(maxLengths[i] + 2));
                        }
                        else
                        {
                            sw.Write(new string(' ', maxLengths[i] + 2));
                        }
                    }
                    sw.WriteLine();
                }
                sw.Close();
            }
        }



        protected void downloadfile(string filename)
        {

            // check to see that the file exists 
            if (File.Exists(filename))
            {
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(filename);
                Response.End();
                File.Delete(filename);
            }
            else
            {
                gblFuction.AjxMsgPopup("File could not be found");
            }
        }
    }
}