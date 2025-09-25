using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Text;
using SelectPdf;

namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            // Build dynamic HTML with Nirmala UI font
            StringBuilder html = new StringBuilder();
            html.Append("<html>");
            html.Append("<head>");
            html.Append("<style>");
            html.Append("body { font-family: 'Nirmala UI'; font-size: 14pt; }");
            html.Append("table { width: 100%; border-collapse: collapse; }");
            html.Append("td, th { border: 1px solid #000; padding: 8px; }");
            html.Append("</style>");
            html.Append("</head>");
            html.Append("<body>");
            html.Append("<h2>नमस्ते दुनिया - Dynamic Table Example</h2>");
            html.Append("<table>");
            html.Append("<tr><th>क्रमांक</th><th>नाम</th><th>शहर</th></tr>");

            // Sample data loop
            for (int i = 1; i <= 5; i++)
            {
                html.Append("<tr>");
                html.AppendFormat("<td>{0}</td>", i);
                html.AppendFormat("<td>यूज़र {0}</td>", i);
                html.AppendFormat("<td>दिल्ली</td>");
                html.Append("</tr>");
            }

            html.Append("</table>");
            html.Append("</body>");
            html.Append("</html>");

            // Convert HTML to PDF using SelectPdf
            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(html.ToString());

            // Send PDF to browser
            doc.Save(Response, false, "DynamicTable.pdf");
            doc.Close();
        }
    }
}