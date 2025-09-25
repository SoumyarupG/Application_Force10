using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using FORCEBA;
using FORCECA;
using System.IO;
namespace CENTRUM.WebPages.Private.Master
{
    public partial class VerifyKYCDoc : CENTRUMBase
    {
        static string vLoanAppID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                vLoanAppID = Request.QueryString["id"];
                string vSubID = Request.QueryString["SubId"];
                LoadGrid(vLoanAppID);              
            }
        }
        private void LoadGrid(string pLoanAppID)
        {
            DataTable dt = null;         
            
            CApplication oMember = null;
            try
            {

                 oMember = new CApplication();
                dt = oMember.GetKYCDocVerify(vLoanAppID);
                ViewState["KYCDoc"] = dt;
                gvDocImage.DataSource = dt;
                gvDocImage.DataBind();
            }
            finally
            {
                dt = null;
                oMember = null;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecords(vLoanAppID);           
        }
        private void SaveRecords(string vLoanAppID)
        {
            CApplication oMem = new CApplication();
            DataTable dt = new DataTable();            
            Boolean vResult = false;
            Int32 vErr = 0;
            string vXmlData = "";
            DateTime vdate = gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate]));
            
            try
            {
               
                this.GetModuleByRole(mnuID.mnuMemberMst);
                dt = (DataTable)ViewState["KYCDoc"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DocImageVerfied"].ToString() == "N" && dt.Rows[i]["DocImageCancel"].ToString() == "N" )
                    {
                        gblFuction.AjxMsgPopup("You have to check either approve or cancel checkbox...");
                        return;
                    }
                }
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }
                vErr = oMem.VerifiedImage(vLoanAppID, vXmlData, this.UserID, vdate);
                if (vErr > 0)
                {
                    vResult = true;
                    gblFuction.MsgPopup("Saved successfully");
                }
                else
                {  
                    gblFuction.MsgPopup(gblMarg.DBError);
                    vResult = false;
                }

            }

            finally
            {
                dt = null;
            }
        }

        protected void chkVerify_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            try
            {
                dt = (DataTable)ViewState["KYCDoc"];
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkVerify = (CheckBox)row.FindControl("chkVerify");
                CheckBox chkCan = (CheckBox)row.FindControl("chkCan");
                TextBox txtReason = (TextBox)row.FindControl("txtReason");
                if (chkVerify.Checked == true)
                {
                    chkCan.Enabled = false;
                    txtReason.Enabled = false;
                    dt.Rows[row.RowIndex]["DocImageVerfied"] = "Y";
                    dt.Rows[row.RowIndex]["DocImageCancel"] = "N";
                    dt.AcceptChanges();
                }
                else
                {
                    chkCan.Enabled = true;
                    txtReason.Enabled = true;
                    dt.Rows[row.RowIndex]["DocImageVerfied"] = "N";
                    dt.Rows[row.RowIndex]["DocImageCancel"] = "N";
                    dt.AcceptChanges();
                }

            }
            finally
            {
                dt = null;
            }

        }
        protected void chkCan_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            try
            {
                dt = (DataTable)ViewState["KYCDoc"];
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkVerify = (CheckBox)row.FindControl("chkVerify");
                CheckBox chkCan = (CheckBox)row.FindControl("chkCan");
                if (chkCan.Checked == true)
                {
                    chkVerify.Enabled = false;
                    dt.Rows[row.RowIndex]["DocImageCancel"] = "Y";
                    dt.AcceptChanges();
                }
                else
                {
                    chkVerify.Enabled = true;
                    dt.Rows[row.RowIndex]["DocImageCancel"] = "N";
                    dt.AcceptChanges();
                }
            }
            finally
            {
                dt = null;
            }

        }
        protected void gvDocImage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkVerify = (CheckBox)e.Row.FindControl("chkVerify");
                    CheckBox chkCan = (CheckBox)e.Row.FindControl("chkCan");
                    if (e.Row.Cells[7].Text == "Y")
                    {
                        chkVerify.Checked = true;
                        chkCan.Enabled = false;
                       
                    }
                    if (e.Row.Cells[8].Text == "Y")
                    {
                        chkCan.Checked = true;
                        chkVerify.Enabled = false;
                        
                    }

                }
            }
            finally
            {
            }
        }
        protected void txtReason_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtReason = (TextBox)gvRow.FindControl("txtReason");
            dt = (DataTable)ViewState["KYCDoc"];

            if (txtReason.Text != "")
            {
                dt.Rows[gvRow.RowIndex]["DocCancelReason"] = txtReason.Text;
                dt.AcceptChanges();
                return;
            }
            ViewState["KYCDoc"] = dt;

        }      
        protected void gvDocImage_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vslno = "";
            DataTable dt = null;
            Int32 vDocId = 0;
            string base64String = "";
            CApplication oCG = null;
            try
            {
                vslno = Convert.ToString(e.CommandArgument);
                ViewState["SLNo"] = vslno;
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                vDocId = Convert.ToInt32(gvr.Cells[0].Text);
                ViewState["DocId"] = vDocId;
                if (e.CommandName == "cmdShow")
                {
                    oCG = new CApplication();
                    dt = oCG.GetKYCDocImageByIdVerify(vslno, vDocId);

                    if (dt.Rows.Count > 0)
                    {                                              
                        byte[] vDoc_Image = (byte[])dt.Rows[0]["Doc_Image"];
                        ViewState["Doc_Image"] = vDoc_Image;
                        ViewState["vimage_type"] = Convert.ToString(dt.Rows[0]["image_type"]);
                        base64String = Convert.ToBase64String(vDoc_Image, 0, vDoc_Image.Length);
                        if (base64String != "")
                        {
                            img.Visible = true;
                            img.ImageUrl = "data:image/" + Convert.ToString(dt.Rows[0]["image_type"]) + ";base64," + base64String;
                        }
                        else
                        {
                            img.Visible = true;
                            img.ImageUrl = "~/Images/No_Image_Available.png";
                        }

                    }
                }
                if (e.CommandName == "cmdDownload")
                {
                    oCG = new CApplication();
                    dt = oCG.GetKYCDocImageByIdVerify(vslno, vDocId);
                    if (dt.Rows.Count > 0)
                    {
                        MemoryStream ms = new MemoryStream((byte[])(dt.Rows[0]["Doc_Image"]));
                        Response.ContentType = "application/" + dt.Rows[0]["image_type"].ToString();
                        Response.AddHeader("content-disposition", "attachment;filename=" + dt.Rows[0]["File_name"].ToString());
                        Response.Buffer = true;
                        ms.WriteTo(Response.OutputStream);
                        Response.End();
                    }
                }
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }
    }
}