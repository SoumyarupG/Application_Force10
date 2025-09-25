using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.Configuration;
using System.IO;
using System.Net;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class DocsDownload : CENTRUMBAse
    {
        string vPathImage = "", vPathHDrive = "", vPathNetworkDrive1 = "", vPathNetworkDrive2 = "", vPathNetworkDrive = "";
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch();
                ViewState["StateEdit"] = null;
                ViewState["DocDownload"] = null;
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "") Response.Redirect("~/Login.aspx", false);
                this.Menu = false;
                this.PageHeading = "Document Verification and Download";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                // this.GetModuleByRole(mnuID.mnuOverride);

                this.GetModuleByRole(mnuID.mnuDocsDownload);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanAdd == "N" || this.CanAdd == null || this.CanAdd == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Document Download", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void PopBranch()
        {
            ddlBranch.Items.Clear();
            CMember oCM = new CMember();
            DataTable dt = new DataTable(); ;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = oCM.GetBranchByBrCode(vBrCode);
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataBind();
                    ListItem oItm = new ListItem();
                    oItm.Text = "<--- Select --->";
                    oItm.Value = "-1";
                    ddlBranch.Items.Insert(0, oItm);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCM = null;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ddlOptions.SelectedValue == "D")
            {
                btnSave.Enabled = false;
            }
            else
            {
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                }
            }
            LoadGrid();
        }
        private void LoadGrid()
        {
            CApplication oLA = new CApplication();
            DataTable dt = new DataTable(); ;
            try
            {
                string vBrCode = Convert.ToString(ddlBranch.SelectedValue);
                DateTime vFrmDt, vToDt;
                vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                vToDt = gblFuction.setDate(txtToDt.Text);
                dt = oLA.GetDocDownloadData(vBrCode, vFrmDt, vToDt, txtSearch.Text.Trim(), Convert.ToString(ddlOptions.SelectedValue));
                gvDocDown.DataSource = dt;
                gvDocDown.DataBind();
                ViewState["DocDownload"] = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oLA = null;
            }
        }

        protected void gvDocDown_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string IsDigitalDocYN = Convert.ToString(e.Row.Cells[20].Text);
                    string IsDisbYN = Convert.ToString(e.Row.Cells[21].Text);
                    CheckBox chkSendback = (CheckBox)e.Row.FindControl("chkSendback");

                    if (IsDisbYN == "Y")
                    {
                        chkSendback.Enabled = false;
                    }
                    else if (IsDigitalDocYN == "Y" )
                    {
                        chkSendback.Enabled = false;
                    }
                    else if (IsDigitalDocYN == "N" && IsDisbYN == "N")
                    {
                        chkSendback.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        protected void chkSendback_CheckChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;

            string IsDigitalDocYN = Convert.ToString(row.Cells[20].Text);
            string IsDisbYN = Convert.ToString(row.Cells[21].Text);
            CheckBox chkSendback = (CheckBox)row.FindControl("chkSendback");

            CMember cMem = new CMember();
            dt = (DataTable)ViewState["Sanc"];
            if (checkbox.Checked == true)
            {
                if (IsDisbYN == "Y")
                {
                    gblFuction.AjxMsgPopup("Loan Already Disbursed Cannot Send Back.");
                    chkSendback.Checked = false;
                    return;
                }
                else if (IsDigitalDocYN == "Y" )
                {
                    gblFuction.AjxMsgPopup("Applicant Assisted Sign is done,Cannot Send Back");
                    chkSendback.Checked = false;
                    return;
                }
                else if (IsDigitalDocYN == "N" && IsDisbYN == "N")
                {
                    chkSendback.Checked = true;
                }
            }
        }

        protected void btnSendBack_Click(object sender, EventArgs e)
        {
            string vXml = "";
            int vErr = 0;
            DataTable dtXml = CreateTrData();
            CApplication oMem = new CApplication();
            DateTime vSendBackDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            using (StringWriter oSW = new StringWriter())
            {
                dtXml.WriteXml(oSW);
                vXml = oSW.ToString();
            }

            vErr = oMem.DigiDocSendback(vXml, Convert.ToInt32(Session[gblValue.UserId]), vSendBackDt);
            if (vErr > 0)
            {
                gblFuction.MsgPopup("Records Sent Back Successfully.");
                LoadGrid();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "CloseWindowScript", "window.close();", true);
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }
        }

        private DataTable CreateTrData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "DigiDocTbl";
            dt.Columns.Add("SanctionID", typeof(string));
            dt.Columns.Add("IsSendBackYN", typeof(string));

            for (int i = 0; i < gvDocDown.Rows.Count; i++)
            {
                CheckBox chkSendback = (CheckBox)gvDocDown.Rows[i].FindControl("chkSendback");
                string SanctionID = Convert.ToString(gvDocDown.Rows[i].Cells[1].Text);

                dt.Rows.Add();
                dt.Rows[i]["SanctionID"] = SanctionID;
                dt.Rows[i]["IsSendBackYN"] = chkSendback.Checked == true ? "Y" : "N";

            }
            dt.AcceptChanges();
            return dt;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            CApplication oCA = null;
            DataTable dt = null;
            string vXmlData = "";
            Int32 vErr = 0;
            try
            {
                if (ViewState["DocDownload"] == null)
                {
                    gblFuction.AjxMsgPopup("Nothing to save");
                    return;
                }
                dt = (DataTable)ViewState["DocDownload"];
                if (dt == null)
                {
                    gblFuction.AjxMsgPopup("Nothing to save");
                    return;
                }
                int cnt = dt.Rows.Count;
                for (int r = 0; r < cnt; r++)
                {
                    CheckBox chkApprvYN = (CheckBox)gvDocDown.Rows[r].FindControl("chkApprvYN");
                    if (chkApprvYN.Checked)
                    {
                        dt.Rows[r]["ApproveYN"] = "Y";
                    }
                    else
                    {
                        dt.Rows[r]["ApproveYN"] = "N";
                    }
                }
                dt.AcceptChanges();
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }
                oCA = new CApplication();
                vErr = oCA.SaveDocsDownApprv(vXmlData, Convert.ToInt32(Session[gblValue.UserId]), gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate])));
                if (vErr > 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    //btnSave.Enabled = false;
                    LoadGrid();
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                    //btnSave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCA = null;
                dt = null;
            }
        }
        protected void btnDownloadSelfie_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["DocDownloadPath"];
            ImageButton btn = sender as ImageButton;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lblLoanAppId = (Label)gvrow.FindControl("lblLoanAppId");
            if (lblLoanAppId.Text != "")
            {
                string folderPath = string.Format("{0}/{1}/{2}", vPathImage, "SELFIE", lblLoanAppId.Text);
                string filePath1 = string.Format("{0}/{1}", folderPath, "SELFIE" + ".pdf");
                string filePath2 = string.Format("{0}/{1}", folderPath, "SELFIE" + ".jpeg");
                string filePath3 = string.Format("{0}/{1}", folderPath, "SELFIE" + ".xml");
                string filePath4 = string.Format("{0}/{1}", folderPath, "SELFIE" + ".png");
                string filePath5 = string.Format("{0}/{1}", folderPath, "SELFIE" + ".jpg");

                if (File.Exists(filePath1))
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "SELFIE.pdf" + "\"");
                    Response.TransmitFile(filePath1);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath2))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "SELFIE.jpeg" + "\"");
                    Response.TransmitFile(filePath2);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath3))
                {
                    Response.ContentType = "application/xml";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "SELFIE.xml" + "\"");
                    Response.TransmitFile(filePath3);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath4))
                {
                    Response.ContentType = "image/png";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "SELFIE.png" + "\"");
                    Response.TransmitFile(filePath4);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath5))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "SELFIE.jpg" + "\"");
                    Response.TransmitFile(filePath5);
                    Response.End();
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("File not Exists");
                    return;
                }
            }
        }
        protected void btnDownloadCAM_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["DocDownloadPath"];
            ImageButton btn = sender as ImageButton;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lblLoanAppId = (Label)gvrow.FindControl("lblLoanAppId");
            if (lblLoanAppId.Text != "")
            {
                string folderPath = string.Format("{0}/{1}/{2}", vPathImage, "CAM", lblLoanAppId.Text);
                string filePath1 = string.Format("{0}/{1}", folderPath, "CAM" + ".pdf");
                string filePath2 = string.Format("{0}/{1}", folderPath, "CAM" + ".jpeg");
                string filePath3 = string.Format("{0}/{1}", folderPath, "CAM" + ".xml");
                string filePath4 = string.Format("{0}/{1}", folderPath, "CAM" + ".png");
                string filePath5 = string.Format("{0}/{1}", folderPath, "CAM" + ".jpg");

                if (File.Exists(filePath1))
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "CAM.pdf" + "\"");
                    Response.TransmitFile(filePath1);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath2))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "CAM.jpeg" + "\"");
                    Response.TransmitFile(filePath2);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath3))
                {
                    Response.ContentType = "application/xml";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "CAM.xml" + "\"");
                    Response.TransmitFile(filePath3);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath4))
                {
                    Response.ContentType = "image/png";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "CAM.png" + "\"");
                    Response.TransmitFile(filePath4);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath5))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "CAM.jpg" + "\"");
                    Response.TransmitFile(filePath5);
                    Response.End();
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("File not Exists");
                    return;
                }
            }
        }
        protected void btnDownloadPAN_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["DocDownloadPath"];
            ImageButton btn = sender as ImageButton;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lblLoanAppId = (Label)gvrow.FindControl("lblLoanAppId");
            if (lblLoanAppId.Text != "")
            {
                string folderPath = string.Format("{0}/{1}/{2}", vPathImage, "PAN", lblLoanAppId.Text);
                string filePath1 = string.Format("{0}/{1}", folderPath, "PAN" + ".pdf");
                string filePath2 = string.Format("{0}/{1}", folderPath, "PAN" + ".jpeg");
                string filePath3 = string.Format("{0}/{1}", folderPath, "PAN" + ".xml");
                string filePath4 = string.Format("{0}/{1}", folderPath, "PAN" + ".png");
                string filePath5 = string.Format("{0}/{1}", folderPath, "PAN" + ".jpg");

                if (File.Exists(filePath1))
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "PAN.pdf" + "\"");
                    Response.TransmitFile(filePath1);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath2))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "PAN.jpeg" + "\"");
                    Response.TransmitFile(filePath2);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath3))
                {
                    Response.ContentType = "application/xml";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "PAN.xml" + "\"");
                    Response.TransmitFile(filePath3);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath4))
                {
                    Response.ContentType = "image/png";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "PAN.png" + "\"");
                    Response.TransmitFile(filePath4);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath5))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "PAN.jpg" + "\"");
                    Response.TransmitFile(filePath5);
                    Response.End();
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("File not Exists");
                    return;
                }
            }
        }
        protected void btnDownloadAadhaarF_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["DocDownloadPath"];
            ImageButton btn = sender as ImageButton;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lblLoanAppId = (Label)gvrow.FindControl("lblLoanAppId");
            if (lblLoanAppId.Text != "")
            {
                string folderPath = string.Format("{0}/{1}/{2}", vPathImage, "POA-AadhaarF", lblLoanAppId.Text);
                string filePath1 = string.Format("{0}/{1}", folderPath, "POA-AadhaarF" + ".pdf");
                string filePath2 = string.Format("{0}/{1}", folderPath, "POA-AadhaarF" + ".jpeg");
                string filePath3 = string.Format("{0}/{1}", folderPath, "POA-AadhaarF" + ".xml");
                string filePath4 = string.Format("{0}/{1}", folderPath, "POA-AadhaarF" + ".png");
                string filePath5 = string.Format("{0}/{1}", folderPath, "POA-AadhaarF" + ".jpg");

                if (File.Exists(filePath1))
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-AadhaarF.pdf" + "\"");
                    Response.TransmitFile(filePath1);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath2))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-AadhaarF.jpeg" + "\"");
                    Response.TransmitFile(filePath2);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath3))
                {
                    Response.ContentType = "application/xml";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-AadhaarF.xml" + "\"");
                    Response.TransmitFile(filePath3);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath4))
                {
                    Response.ContentType = "image/png";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-AadhaarF.png" + "\"");
                    Response.TransmitFile(filePath4);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath5))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-AadhaarF.jpg" + "\"");
                    Response.TransmitFile(filePath5);
                    Response.End();
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("File not Exists");
                    return;
                }
            }
        }
        protected void btnDownloadAadhaarB_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["DocDownloadPath"];
            ImageButton btn = sender as ImageButton;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lblLoanAppId = (Label)gvrow.FindControl("lblLoanAppId");
            if (lblLoanAppId.Text != "")
            {
                string folderPath = string.Format("{0}/{1}/{2}", vPathImage, "POA-AadhaarB", lblLoanAppId.Text);
                string filePath1 = string.Format("{0}/{1}", folderPath, "POA-AadhaarB" + ".pdf");
                string filePath2 = string.Format("{0}/{1}", folderPath, "POA-AadhaarB" + ".jpeg");
                string filePath3 = string.Format("{0}/{1}", folderPath, "POA-AadhaarB" + ".xml");
                string filePath4 = string.Format("{0}/{1}", folderPath, "POA-AadhaarB" + ".png");
                string filePath5 = string.Format("{0}/{1}", folderPath, "POA-AadhaarB" + ".jpg");

                if (File.Exists(filePath1))
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-AadhaarB.pdf" + "\"");
                    Response.TransmitFile(filePath1);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath2))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-AadhaarB.jpeg" + "\"");
                    Response.TransmitFile(filePath2);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath3))
                {
                    Response.ContentType = "application/xml";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-AadhaarB.xml" + "\"");
                    Response.TransmitFile(filePath3);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath4))
                {
                    Response.ContentType = "image/png";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-AadhaarB.png" + "\"");
                    Response.TransmitFile(filePath4);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath5))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-AadhaarB.jpg" + "\"");
                    Response.TransmitFile(filePath5);
                    Response.End();
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("File not Exists");
                    return;
                }
            }
        }
        protected void btnDownloadVoterF_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["DocDownloadPath"];
            ImageButton btn = sender as ImageButton;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lblLoanAppId = (Label)gvrow.FindControl("lblLoanAppId");
            if (lblLoanAppId.Text != "")
            {
                string folderPath = string.Format("{0}/{1}/{2}", vPathImage, "POA-voterIDF", lblLoanAppId.Text);
                string filePath1 = string.Format("{0}/{1}", folderPath, "POA-voterIDF" + ".pdf");
                string filePath2 = string.Format("{0}/{1}", folderPath, "POA-voterIDF" + ".jpeg");
                string filePath3 = string.Format("{0}/{1}", folderPath, "POA-voterIDF" + ".xml");
                string filePath4 = string.Format("{0}/{1}", folderPath, "POA-voterIDF" + ".png");
                string filePath5 = string.Format("{0}/{1}", folderPath, "POA-voterIDF" + ".jpg");

                if (File.Exists(filePath1))
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-voterIDF.pdf" + "\"");
                    Response.TransmitFile(filePath1);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath2))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-voterIDF.jpeg" + "\"");
                    Response.TransmitFile(filePath2);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath3))
                {
                    Response.ContentType = "application/xml";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-voterIDF.xml" + "\"");
                    Response.TransmitFile(filePath3);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath4))
                {
                    Response.ContentType = "image/png";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-voterIDF.png" + "\"");
                    Response.TransmitFile(filePath4);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath5))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-voterIDF.jpg" + "\"");
                    Response.TransmitFile(filePath5);
                    Response.End();
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("File not Exists");
                    return;
                }
            }
        }
        protected void btnDownloadVoterB_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["DocDownloadPath"];
            ImageButton btn = sender as ImageButton;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lblLoanAppId = (Label)gvrow.FindControl("lblLoanAppId");
            if (lblLoanAppId.Text != "")
            {
                string folderPath = string.Format("{0}/{1}/{2}", vPathImage, "POA-voterIDB", lblLoanAppId.Text);
                string filePath1 = string.Format("{0}/{1}", folderPath, "POA-voterIDB" + ".pdf");
                string filePath2 = string.Format("{0}/{1}", folderPath, "POA-voterIDB" + ".jpeg");
                string filePath3 = string.Format("{0}/{1}", folderPath, "POA-voterIDB" + ".xml");
                string filePath4 = string.Format("{0}/{1}", folderPath, "POA-voterIDB" + ".png");
                string filePath5 = string.Format("{0}/{1}", folderPath, "POA-voterIDB" + ".jpg");

                if (File.Exists(filePath1))
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-voterIDB.pdf" + "\"");
                    Response.TransmitFile(filePath1);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath2))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-voterIDB.jpeg" + "\"");
                    Response.TransmitFile(filePath2);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath3))
                {
                    Response.ContentType = "application/xml";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-voterIDB.xml" + "\"");
                    Response.TransmitFile(filePath3);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath4))
                {
                    Response.ContentType = "image/png";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-voterIDB.png" + "\"");
                    Response.TransmitFile(filePath4);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath5))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-voterIDB.jpg" + "\"");
                    Response.TransmitFile(filePath5);
                    Response.End();
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("File not Exists");
                    return;
                }
            }
        }
        protected void btnDownloadDL_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["DocDownloadPath"];
            ImageButton btn = sender as ImageButton;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lblLoanAppId = (Label)gvrow.FindControl("lblLoanAppId");
            if (lblLoanAppId.Text != "")
            {
                string folderPath = string.Format("{0}/{1}/{2}", vPathImage, "POA-dl", lblLoanAppId.Text);
                string filePath1 = string.Format("{0}/{1}", folderPath, "POA-dl" + ".pdf");
                string filePath2 = string.Format("{0}/{1}", folderPath, "POA-dl" + ".jpeg");
                string filePath3 = string.Format("{0}/{1}", folderPath, "POA-dl" + ".xml");
                string filePath4 = string.Format("{0}/{1}", folderPath, "POA-dl" + ".png");
                string filePath5 = string.Format("{0}/{1}", folderPath, "POA-dl" + ".jpg");
                if (File.Exists(filePath1))
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-dl.pdf" + "\"");
                    Response.TransmitFile(filePath1);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath2))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-dl.jpeg" + "\"");
                    Response.TransmitFile(filePath2);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath3))
                {
                    Response.ContentType = "application/xml";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-dl.xml" + "\"");
                    Response.TransmitFile(filePath3);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath4))
                {
                    Response.ContentType = "image/png";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-dl.png" + "\"");
                    Response.TransmitFile(filePath4);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath5))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-dl.jpg" + "\"");
                    Response.TransmitFile(filePath5);
                    Response.End();
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("File not Exists");
                    return;
                }
            }
        }
        protected void btnDownloadXML_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["DocDownloadPath"];
            ImageButton btn = sender as ImageButton;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lblLoanAppId = (Label)gvrow.FindControl("lblLoanAppId");
            if (lblLoanAppId.Text != "")
            {
                string folderPath = string.Format("{0}/{1}/{2}", vPathImage, "POA-xml", lblLoanAppId.Text);
                string filePath1 = string.Format("{0}/{1}", folderPath, "POA-xml" + ".pdf");
                string filePath2 = string.Format("{0}/{1}", folderPath, "POA-xml" + ".jpeg");
                string filePath3 = string.Format("{0}/{1}", folderPath, "POA-xml" + ".xml");
                string filePath4 = string.Format("{0}/{1}", folderPath, "POA-xml" + ".png");
                string filePath5 = string.Format("{0}/{1}", folderPath, "POA-xml" + ".jpg");
                if (File.Exists(filePath1))
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-xml.pdf" + "\"");
                    Response.TransmitFile(filePath1);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath2))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-xml.jpeg" + "\"");
                    Response.TransmitFile(filePath2);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath3))
                {
                    Response.ContentType = "application/xml";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-xml.xml" + "\"");
                    Response.TransmitFile(filePath3);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath4))
                {
                    Response.ContentType = "image/png";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-xml.png" + "\"");
                    Response.TransmitFile(filePath4);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath5))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "POA-xml.jpg" + "\"");
                    Response.TransmitFile(filePath5);
                    Response.End();
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("File not Exists");
                    return;
                }
            }
        }
        protected void btnDownloadDigiDoc_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["DocDownloadPath"];
            // vPathHDrive= ConfigurationManager.AppSettings["DocPathHDrive"];
            vPathNetworkDrive1 = ConfigurationManager.AppSettings["PathNetworkDrive1"];
            vPathNetworkDrive2 = ConfigurationManager.AppSettings["PathNetworkDrive2"];
            vPathNetworkDrive = ConfigurationManager.AppSettings["PathNetworkDrive"];

            ImageButton btn = sender as ImageButton;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lblLoanAppId = (Label)gvrow.FindControl("lblLoanAppId");
            string vLoanAppId = lblLoanAppId.Text;
            if (vLoanAppId != "")
            {
                string folderPath = string.Format("{0}/{1}", vPathImage, "DigitalDoc");
                string filePath1 = string.Format("{0}/{1}", folderPath, vLoanAppId + ".pdf");
                if (File.Exists(filePath1))
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + vLoanAppId + ".pdf" + "\"");
                    Response.TransmitFile(filePath1);
                    Response.End();
                    return;
                }
                else
                {
                    string[] arrPathNetwork = vPathNetworkDrive.Split(',');
                    int i;
                    string vPathDigiDoc = "";
                    for (i = 0; i <= arrPathNetwork.Length - 1; i++)
                    {
                        if (ValidUrlChk(arrPathNetwork[i] + "DigitalDoc/" + vLoanAppId + ".pdf"))
                        {
                            vPathDigiDoc = arrPathNetwork[i] + "DigitalDoc/" + vLoanAppId + ".pdf";
                            break;
                        }
                        if (ValidUrlChk(arrPathNetwork[i] + "smedigitaldoc/" + vLoanAppId + ".pdf"))
                        {
                            vPathDigiDoc = arrPathNetwork[i] + "smedigitaldoc/" + vLoanAppId + ".pdf";
                            break;
                        }
                    }
                    if (vPathDigiDoc != "")
                    {
                        WebClient cln = null;
                        byte[] vDoc = null;
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        cln = new WebClient();
                        vDoc = cln.DownloadData(vPathDigiDoc);
                        Response.AddHeader("Content-Type", "Application/octet-stream");
                        Response.AddHeader("Content-Disposition", "attachment;   filename=" + vLoanAppId + ".pdf");
                        Response.BinaryWrite(vDoc);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("File not Exists..");
                    }

                    //WebClient cln = null;
                    //byte[] vDoc = null;
                    //ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    //if (ValidUrlChk(vPathNetworkDrive1 + "DigitalDoc/" + vLoanAppId + ".pdf"))
                    //{
                    //    cln = new WebClient();
                    //    vDoc = cln.DownloadData(vPathNetworkDrive1 + "DigitalDoc/" + vLoanAppId + ".pdf");
                    //    Response.AddHeader("Content-Type", "Application/octet-stream");
                    //    Response.AddHeader("Content-Disposition", "attachment;   filename=" + vLoanAppId + ".pdf");
                    //    Response.BinaryWrite(vDoc);
                    //    Response.Flush();
                    //    Response.End();
                    //}
                    //else if (ValidUrlChk(vPathNetworkDrive2 + "DigitalDoc/" + vLoanAppId + ".pdf"))
                    //{
                    //    cln = new WebClient();
                    //    vDoc = cln.DownloadData(vPathNetworkDrive2 + "DigitalDoc/" + vLoanAppId + ".pdf");
                    //    Response.AddHeader("Content-Type", "Application/octet-stream");
                    //    Response.AddHeader("Content-Disposition", "attachment;   filename=" + vLoanAppId + ".pdf");
                    //    Response.BinaryWrite(vDoc);
                    //    Response.Flush();
                    //    Response.End();
                    //}
                    //else
                    //{
                    //    gblFuction.AjxMsgPopup("File not Exists");
                    //}

                    return;
                }
            }
        }
        protected void btnDownloadAgreement_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["DocDownloadPath"];
            ImageButton btn = sender as ImageButton;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lblLoanAppId = (Label)gvrow.FindControl("lblLoanAppId");
            if (lblLoanAppId.Text != "")
            {
                string folderPath = string.Format("{0}/{1}/{2}", vPathImage, "LOANAGREE", lblLoanAppId.Text);
                string filePath1 = string.Format("{0}/{1}", folderPath, "LOANAGREE" + ".pdf");
                string filePath2 = string.Format("{0}/{1}", folderPath, "LOANAGREE" + ".jpeg");
                string filePath3 = string.Format("{0}/{1}", folderPath, "LOANAGREE" + ".xml");
                string filePath4 = string.Format("{0}/{1}", folderPath, "LOANAGREE" + ".png");
                string filePath5 = string.Format("{0}/{1}", folderPath, "LOANAGREE" + ".jpg");
                if (File.Exists(filePath1))
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "LOANAGREE.pdf" + "\"");
                    Response.TransmitFile(filePath1);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath2))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "LOANAGREE.jpeg" + "\"");
                    Response.TransmitFile(filePath2);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath3))
                {
                    Response.ContentType = "application/xml";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "LOANAGREE.xml" + "\"");
                    Response.TransmitFile(filePath3);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath4))
                {
                    Response.ContentType = "image/png";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "LOANAGREE.png" + "\"");
                    Response.TransmitFile(filePath4);
                    Response.End();
                    return;
                }
                else if (File.Exists(filePath5))
                {
                    Response.ContentType = "application/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "LOANAGREE.jpg" + "\"");
                    Response.TransmitFile(filePath5);
                    Response.End();
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("File not Exists");
                    return;
                }
            }
        }
        public bool ValidUrlChk(string pUrl)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            WebRequest wR = WebRequest.Create(pUrl);
            WebResponse webResponse;
            try
            {
                wR.Timeout = 10000;
                webResponse = wR.GetResponse();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}