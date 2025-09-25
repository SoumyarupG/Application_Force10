using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CENTRUM
{
    public partial class LoginCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblVersion.Text = String.Format("Version: {0} &nbsp; Dated: {1}",
               System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
               System.IO.File.GetLastWriteTime(Request.PhysicalApplicationPath.ToString() + "\\bin\\CENTRUM.dll").ToString("dd/MMM/yyyy HH:mm:ss"));
            }

        }
    }
}