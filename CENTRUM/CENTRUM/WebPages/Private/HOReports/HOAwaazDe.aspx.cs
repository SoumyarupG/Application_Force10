using System;
using System.Data;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HOAwaazDe : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch(Session[gblValue.UserName].ToString());
                txtAsDt.Text = Session[gblValue.LoginDate].ToString();
                txtFromDt.Text = Session[gblValue.LoginDate].ToString();
                PopState();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Awaaz De Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuAwaazRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Awaaz De Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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
            
                PrintRpt();
            
        }

        private void PrintRpt()
        {
            DataTable dtMem = null;
            CEquiFaxDataSubmission oHM = null;
            string vFolderPath = "C:\\AwaazDe";
            string vBrCode = "A";
            string vFileNm = "";

            try
            {
                TimeSpan t = gblFuction.setDate(txtAsDt.Text.Trim()) - gblFuction.setDate(txtFromDt.Text.Trim());
                if (t.TotalDays > 2)
                {
                    gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                    return;
                }
                //if (ddlSel.SelectedValue == "B")
                //{
                    GetBranch();
                    vBrCode = Convert.ToString(ViewState["ID"]);
                //}
                oHM = new CEquiFaxDataSubmission();
                dtMem = oHM.rptAwaaz(gblFuction.setDate(txtFromDt.Text.Trim()),gblFuction.setDate(txtAsDt.Text.Trim()), vBrCode, rdbMode.SelectedValue);
                vFileNm = vFolderPath + "\\AwaazDe_Data.csv";

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
                int J = 0;
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!row.IsNull(i))
                        {
                            //sw.Write(row[i].ToString().PadRight(maxLengths[i] + 2));
                            sw.Write(row[i].ToString().PadRight(maxLengths[i]));
                        }
                        else
                        {
                            sw.Write(new string(' ', maxLengths[i] + 2));
                        }
                    }
                    J++;
                    if (dt.Rows.Count != J)
                        sw.WriteLine();
                }
                sw.Close();
            }
            ////using (StreamWriter sw = new StreamWriter(outputFilePath, false))
            ////{

            ////    for (int i = 0; i < dt.Columns.Count; i++)
            ////    {

            ////        sw.Write(dt.Columns[i].ColumnName.ToString().Trim() + ',');

            ////    }
            ////    sw.WriteLine();

            ////    foreach (DataRow row in dt.Rows)
            ////    {
            ////        for (int i = 0; i < dt.Columns.Count; i++)
            ////        {
            ////            if (!row.IsNull(i))
            ////            {
            ////                sw.Write(row[i].ToString().Trim() + ',');
            ////            }

            ////        }
            ////        sw.WriteLine();
            ////    }
            ////    sw.Close();
            ////}
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

        private void PopState()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
            ddlState.DataSource = dt;
            ddlState.DataTextField = "StateName";
            ddlState.DataValueField = "StateId";
            ddlState.DataBind();
        }

        protected void btnStateWise_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CUser oUsr = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            string pUser = Session[gblValue.UserName].ToString();
            DateTime vLogDt = gblFuction.setDate(txtAsDt.Text.ToString());
            chkBr.Items.Clear();
            ViewState["Id"] = null;
            try
            {
                oCG = new CGblIdGenerator();
                oUsr = new CUser();
                ViewState["Id"] = null;
                if (vBrCode == "0000")
                {
                    dt = oUsr.GetBranchByState(pUser, Convert.ToInt32(Session[gblValue.RoleId]), ddlState.SelectedValues.Replace("|", ","), ddlBrType.SelectedValue);
                    if (dt.Rows.Count > 0)
                    {
                        chkBr.DataSource = dt;
                        chkBr.DataTextField = "BranchName";
                        chkBr.DataValueField = "BranchCode";
                        chkBr.DataBind();
                        if (ddlSel.SelectedValue == "C")
                            CheckAll();
                    }
                }
                else
                {
                    dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, vBrCode);
                    chkBr.DataSource = dt;
                    chkBr.DataTextField = "Name";
                    chkBr.DataValueField = "BranchCode";
                    chkBr.DataBind();
                    if (ddlSel.SelectedValue == "C")
                        CheckAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
                oCG = null;
            }

        }
    }
}