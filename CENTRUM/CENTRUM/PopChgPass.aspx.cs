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
	public partial class PopChgPass : System.Web.UI.Page
    {
        string vUId = "";
		protected void Page_Load(object sender, EventArgs e)
		{
            vUId = Request.QueryString["id"];
            GetUserById(vUId);
		}
        private void GetUserById(string vUID)
        {
            CUser oUser = null;
            DataTable dt = null;
            try
            {
                oUser = new CUser();
                dt = oUser.GetUserById(Convert.ToDouble(vUID));
                if (dt.Rows.Count > 0)
                {
                    ViewState["UserId"] = Convert.ToString(dt.Rows[0]["UserId"]);
                    txtPass.Attributes.Add("value", dataEncryp.DecryptText(dt.Rows[0]["Password"].ToString()));
                    txtUserNm.Text = Convert.ToString(dt.Rows[0]["UserName"]);
                }
            }
            finally
            {
                oUser = null;
                dt = null;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Int32 vErr = 0;
            CUser oUser = null;
            try
            {
                string vPass = "";
                Int32 vUserId = Convert.ToInt32(ViewState["UserId"]);

                if (ValidateFields() == false) return;
                vPass = dataEncryp.EncryptText(txtNewPass.Text);
                oUser = new CUser();
               // vErr = oUser.PopChangePassword(vUserId, vPass);
                if (vErr > 0)
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                else
                    gblFuction.MsgPopup(gblMarg.DBError);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oUser = null;
            }
        }

        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            CUser oUsr = null;
            Int32 vRec = 0;
            try
            {
                string vPass = dataEncryp.EncryptText(txtPass.Text);
                if (txtUserNm.Text.Trim() == "")
                {
                    gblFuction.MsgPopup("User Name cannot be left blank.");
                    vResult = false;
                }
                if (txtPass.Text.Trim() == "")
                {
                    gblFuction.MsgPopup("Password cannot be left blank.");
                    vResult = false;
                }
                if (txtNewPass.Text.Trim() == "")
                {
                    gblFuction.MsgPopup("New Password cannot be left blank.");
                    vResult = false;
                }
                if (txtCPass.Text.Trim() == "")
                {
                    gblFuction.MsgPopup("Confirm Password cannot be left blank.");
                    vResult = false;
                }
                if (txtNewPass.Text.Trim() != "" && txtCPass.Text.Trim() != "")
                {
                    if (txtNewPass.Text.ToString() != txtCPass.Text.ToString())
                    {
                        gblFuction.MsgPopup("New Password does not Matched with the Confirm Password.");
                        vResult = false;
                    }
                    else
                    {
                        oUsr = new CUser();            
                       // vRec = oUsr.ChkDuplicateUser(txtUserNm.Text.Trim(), vPass);
                        if (vRec == 0)
                        {
                            gblFuction.MsgPopup(gblMarg.InvalidUser);
                            vResult = false;
                        }
                    }
                }
                return vResult;
            }
            finally
            {
                oUsr = null;
            }
        }
	}
}