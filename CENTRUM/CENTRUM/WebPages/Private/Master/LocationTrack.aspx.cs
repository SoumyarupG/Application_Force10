using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Web.Services;
using System.IO;
using System.Web;
using System.Collections.Generic;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class LocationTrack : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch(Session[gblValue.UserName].ToString());
                txtFrmDt.Text = HttpContext.Current.Session[gblValue.LoginDate].ToString().Trim();
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    ddlBranch.SelectedValue = Session[gblValue.BrnchCode].ToString();
                    hdBranch.Value = Session[gblValue.BrnchCode].ToString();
                    ddlBranch.Enabled = false;
                }
               
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Location Tracking";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanDisbursement);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Location Tracking", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
       
        
        [WebMethod]
        public static string GetEmployeeLocation(string vBranchCode, string vDate)
        {
            DateTime vLoginDt = gblFuction.setDate(vDate);
            DataTable dt = null;
            CHRMst oHR = new CHRMst();
            dt = oHR.GetEmployeeLocation(vBranchCode, vLoginDt);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
        [WebMethod]
        public static string GetEmployeeLocationByEmployee(string vBranchCode, string vEOID, string vFrmTm, string vToTm,string vDate)
        {
            DateTime vLoginDt = gblFuction.setDate(vDate);
            DataTable dt = null;
            CHRMst oHR = new CHRMst();
            dt = oHR.GetEmployeeLocationByEmp(vBranchCode, vLoginDt, vEOID, vFrmTm, vToTm);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]), "R");
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
                else
                {
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }
        private void PopEmployeeByBranch(string vBranchCode)
        {
            DataTable dt = null;
            CHRMst oHR = null;
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oHR = new CHRMst();
                dt = oHR.GetEmployeeByBranch(vBranchCode, vLoginDt);
                if (dt.Rows.Count > 0)
                {
                    ddlEmp.DataSource = dt;
                    ddlEmp.DataTextField = "EOName";
                    ddlEmp.DataValueField = "EOID";
                    ddlEmp.DataBind();
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlEmp.Items.Insert(0, liSel);
                }
                else
                {
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlEmp.Items.Insert(0, liSel);
                }
            }
            finally
            {
                dt = null;
                oHR = null;
            }
        }
        [WebMethod]
        public static List<CO> popCO(string vBranchCode)
        {
            DataTable dt = null;
            CHRMst oHR = null;
            DateTime vLoginDt = gblFuction.setDate(HttpContext.Current.Session[gblValue.LoginDate].ToString().Trim());
            oHR = new CHRMst();
            dt = oHR.GetEmployeeByBranch(vBranchCode, vLoginDt);
            List<CO> lst = new List<CO>();
            lst.Add(new CO
            {
                EOID = "-1",
                EOName = "<--Select-->"
            });
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new CO
                {
                    EOID = dr["EOID"].ToString(),
                    EOName = dr["EOName"].ToString()

                });

            }
            return lst;
        }

        public class CO
        {
            public string EOID { get; set; }
            public string EOName { get; set; }
        }
    }
}