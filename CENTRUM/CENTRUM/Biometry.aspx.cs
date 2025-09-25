using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Web.UI;

namespace CENTRUM
{
    public partial class Biometry : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LinkButton LogOut = (LinkButton)Master.FindControl("lbLogOut");
            LinkButton ChBranch = (LinkButton)Master.FindControl("lblChBranch");
            LogOut.Text = "";
            ChBranch.Text = "";
            this.Welcome = false;
            Session[gblValue.LoginDate] = hdnDt.Value;
            if (!IsPostBack)
            {

               // txtPass.Attributes.Add("onKeyPress", "doClick('" + btnLog.ClientID + "',event)");
                txtUsrNm.Focus();
            }
        }

        protected void ibBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
        }

        protected void btnAttLogin_Click(object Sender, EventArgs e)
        {
            //DataTable dt = null;
            //CUser oUsr = null;         
            //try
            //{
            //    if (ValidateFields() == true)
            //    {
            //        string username = txtUsrNm.Text;
            //        Session[gblValue.UserName] = txtUsrNm.Text.Trim();
            //        oUsr = new CUser();
            //        dt = oUsr.GetRoleByUser(txtUsrNm.Text.Trim());
            //        Session[gblValue.RoleId] = dt.Rows[0]["RoleId"].ToString();
            //        Session[gblValue.UserId] = dt.Rows[0]["UserID"].ToString();
            //        Session[gblValue.Designation] = dt.Rows[0]["Designation"].ToString();                   
            //        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(3, username, System.DateTime.Now,
            //            System.DateTime.Now.AddMinutes(30), true, "vTkt", FormsAuthentication.FormsCookiePath);
            //        string encTicket = FormsAuthentication.Encrypt(ticket);
            //        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
            //        hdnUser.Value = dt.Rows[0]["UserID"].ToString();
            //        Response.Redirect("~/WebPages/Public/FinYear.aspx", false);               

            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            //finally
            //{
            //    dt = null;
            //    oUsr = null;
            //}

            Boolean vMSg = false;
            string vUserName = string.Empty;
            string vUserPass = string.Empty;
            string vBrCode = string.Empty;
            try
            {
                vUserName = txtUsrNm.Text.Trim();
                vUserPass = txtPass.Text.Trim();
                vMSg = ValidateFields();
                if (vMSg == true)
                {
                    vBrCode = vUserName.Substring(0, 3);
                    Application["BrCode"] = vBrCode;
                    Application["LoginDate"] = DateTime.Today.ToString("dd/MM/yyyy");
                    CUser oUsr = null;
                    DataTable dt = null;
                    oUsr = new CUser();
                    dt = oUsr.GetBiometryData(vBrCode);
                    Application["Biometry"] = dt;
                    Response.Redirect("~/BioAttendence.aspx", false);
                }
                else if (vMSg ==false)
                {
                    Response.RedirectPermanent("~/Biometry.aspx", false);
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("Error");
                    return;
                }
            }
            finally
            {
            }
        }


        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            Int32 vRec = 0;
            string vPass = dataEncryp.EncryptText(txtPass.Text);
            //string vPass1 = dataEncryp.DecryptText(txtPass.Text);
            CUser oUsr = null;
            DataTable dt = null;
            try
            {
                if (txtUsrNm.Text.Trim() != "")
                {
                    if (txtPass.Text.Trim() == "")
                    {
                        gblFuction.AjxMsgPopup("Password cannot be left blank.");
                        gblFuction.AjxFocus("ctl00_cph_Main_txtPass");
                        vResult = false;
                        return vResult;
                    }
                }
                if (txtPass.Text.Trim() != "")
                {
                    if (txtUsrNm.Text != "" && txtPass.Text.Trim() != "")
                    {
                        oUsr = new CUser();
                        vRec = oUsr.ChkDuplicateUser(txtUsrNm.Text, vPass,"A");
                        if (vRec == 0)
                        {
                            gblFuction.AjxMsgPopup(gblMarg.InvalidUser);
                            gblFuction.AjxFocus("ctl00_cph_Main_txtUsrNm");
                            vResult = false;
                            return vResult;
                        }
                    }
                }
                if (txtUsrNm.Text.Trim() != "")
                {
                    if (txtUsrNm.Text != "" && txtPass.Text.Trim() != "")
                    {
                        oUsr = new CUser();
                        dt = oUsr.ChkActiveUser(txtUsrNm.Text, vPass);
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Status"].ToString() != "Y")
                            {
                                gblFuction.AjxMsgPopup(gblMarg.InActiveUser);
                                gblFuction.AjxFocus("ctl00_cph_Main_txtUsrNm");
                                vResult = false;
                                return vResult;
                            }
                            else if (dt.Rows[0]["Password"].ToString() != vPass)
                            {
                                gblFuction.AjxMsgPopup(gblMarg.InvalidUser);
                                gblFuction.AjxFocus("ctl00_cph_Main_txtUsrNm");
                                vResult = false;
                                return vResult;
                            }
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("User not exist.");
                            gblFuction.AjxFocus("ctl00_cph_Main_txtUsrNm");
                            vResult = false;
                            return vResult;
                        }
                    }
                }
                return vResult;
            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }
    }
}