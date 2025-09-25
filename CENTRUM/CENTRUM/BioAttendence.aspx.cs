using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Web.UI;
using System.Drawing;

namespace CENTRUM
{
    public partial class BioAttendence : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LinkButton LogOut = (LinkButton)Master.FindControl("lbLogOut");
            LinkButton ChBranch = (LinkButton)Master.FindControl("lblChBranch");
            LogOut.Text = "";
            ChBranch.Text = "";
            this.Welcome = false;
          
            lblIn.Text = "In (" + Application["BrCode"].ToString() + "-" + Application["LoginDate"].ToString() + ")";
            lblOut.Text = "Out (" + Application["BrCode"].ToString() + "-" + Application["LoginDate"].ToString() + ")";
            //Session[gblValue.LoginDate] = hdnDt.Value;
            DataTable dt = (DataTable)Application["Biometry"];
            gvEmpIn.DataSource = dt;
            gvEmpIn.DataBind();
            gvEmpOut.DataSource = dt;
            gvEmpOut.DataBind();

        }

        protected void ibLogOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();  
            Response.Redirect("Login.aspx", false);
        }

        protected void gvEmpIn_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Color clr_green = ColorTranslator.FromHtml("#98FB98");
            Color clr_red = ColorTranslator.FromHtml("#ff4c4c");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text == "L")
                {
                    e.Row.BackColor = clr_red;
                }
                else
                {
                    if (e.Row.Cells[2].Text != "&nbsp;")
                    {
                        e.Row.BackColor = clr_green;
                    }
                }
            }
        }

        protected void gvEmpOut_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Color clr_green = ColorTranslator.FromHtml("#98FB98");
            Color clr_red = ColorTranslator.FromHtml("#ff4c4c");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text == "L")
                {
                    e.Row.BackColor = clr_red;
                }
                else
                {
                    if (e.Row.Cells[2].Text != "&nbsp;")
                    {
                        e.Row.BackColor = clr_green;
                    }
                }
            }
        }

        protected void gvEmpIn_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Match_LogIn();", true);    
        }

        protected void gvEmpOut_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Match_LogOut();", true);
        }
    }
}