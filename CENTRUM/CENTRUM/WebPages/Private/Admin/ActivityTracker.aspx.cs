using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;
namespace CENTRUM.WebPages.Private.Admin
{
    public partial class ActivityTracker : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtAsOnDate.Text = Session[gblValue.LoginDate].ToString();
                hdUserID.Value = Session[gblValue.UserName].ToString();
                hdRoleId.Value = Session[gblValue.RoleId].ToString();
                PopROForActivityTrack();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Activity Tracker";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";

                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;

                this.GetModuleByRole(mnuID.mnuAppUpdate);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx", false);
                }
                
            }
            catch
            {
                Response.Redirect("~/UFS.aspx", false);
            }
        }

        private void PopROForActivityTrack()
        {
            DataTable dt = new DataTable();
            string vBrCode = (string)Session[gblValue.BrnchCode];
            CActivity oCM = new CActivity();
            try
            {
                DateTime vActivityDt = gblFuction.setDate(txtAsOnDate.Text);
                dt = oCM.PopROForActivityTrack(vBrCode, vActivityDt);

                ddlRO.DataSource = dt;
                ddlRO.DataValueField = "EoId";
                ddlRO.DataTextField = "EoName";
                ddlRO.DataBind();
                ddlRO.Items.Insert(0, new ListItem("<--Select-->", "-1"));
            }
            finally
            {
                dt = null;
                oCM = null;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
    }
}