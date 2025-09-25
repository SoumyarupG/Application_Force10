/***
using System;
using System.Web;
using System.Web.Services;

namespace CENTRUM_VRIDDHIVYAPAR
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static void AbandonSession()
        {
          HttpContext.Current.Session.Abandon();
        }
    }    
}

***/

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace CENTRUM_VRIDDHIVYAPAR
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvHover.DataSource = GetDataTable();
                gvHover.DataBind();
            }
        }

        protected DataTable GetDataTable()
        {
            DataTable dTable = new DataTable();
            DataRow dRow = null;
            Random rnd = new Random();
            dTable.Columns.Add("Serial");
            dTable.Columns.Add("UserName");
            dTable.Columns.Add("Education");

            dTable.Rows.Add(1, "Suresh Dasari", "B.Tech");
            dTable.Rows.Add(2, "Rohini Dasari", "Msc");
            dTable.Rows.Add(3, "Madhav Sai", "Ms");
            dTable.Rows.Add(4, "Praveen", "B.Tech");
            dTable.Rows.Add(6, "Sateesh", "MD");
            dTable.Rows.Add(7, "Mahesh Dasari", "B.Tech");
            dTable.Rows.Add(8, "Mahendra", "CA");
            dTable.Rows.Add(9, "Amit Sharma", "B.Tech");
            dTable.Rows.Add(10, "Atul Deshai", "Msc");
            dTable.Rows.Add(11, "Raghav Roy", "Ms");
            dTable.Rows.Add(12, "Devi Rai", "B.Tech");
            dTable.Rows.Add(13, "Suresh", "MD");
            dTable.Rows.Add(14, "Rohit Verma", "B.Tech");
            dTable.Rows.Add(15, "Ashis Dube", "CA");

            dTable.Rows.Add(16, "Rajesh", "Msc");
            dTable.Rows.Add(17, "Abdul Gani", "Ms");
            dTable.Rows.Add(18, "Sk. Akthar", "B.Tech");
            dTable.Rows.Add(19, "Shakil", "MD");
            dTable.Rows.Add(20, "Arindam Ghosh", "B.Tech");
            dTable.Rows.Add(21, "Samir Paul", "CA");



            return dTable;
        }

        protected void gvHover_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //Add CSS class on header row.
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.CssClass = "header";

            //Add CSS class on normal row.
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Normal)
                e.Row.CssClass = "normal";

            //Add CSS class on alternate row.
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Alternate)
                e.Row.CssClass = "alternate";
        }
    }
}