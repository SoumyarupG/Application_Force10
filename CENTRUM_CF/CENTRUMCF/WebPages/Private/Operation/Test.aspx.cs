using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using System.Configuration;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
//using iTextSharp.tool.xml;
//using iTextSharp.tool.xml.css;
//using iTextSharp.tool.xml.pipeline.css;
//using iTextSharp.tool.xml.pipeline.html;
//using iTextSharp.tool.xml.pipeline.end;
//using iTextSharp.tool.xml.html;
//using iTextSharp.tool.xml.parser;

namespace CENTRUMCF.WebPages.Private.Operation
{
    public partial class Test : System.Web.UI.Page
    {
        string HindiFont = ConfigurationManager.AppSettings["fontPathHindi"];
        protected void Page_Load(object sender, EventArgs e)
        {
            // LoadRecord();

//            string html = @"
//            <html>
//            <head>
//                <style>
//                    body {
//                        font-family: 'Nirmala';
//                        font-size: 14pt;
//                    }
//                </style>
//            </head>
//            <body>
//                <p>यह करार 20...... के ............महीने में.............दिन निम्नलिखित के बीच किया गया है</p>
//            </body>
//            </html>";

            string outputPath = "C:/output_2.pdf";


            StringBuilder page1Builder = new StringBuilder();
            page1Builder.Append("<html><head><style>");
            page1Builder.Append("@font-face { font-family: 'NirmalaCustom'; src: url('Nirmala.ttf'); }");
            page1Builder.Append("body, h1, p { font-family: 'NirmalaCustom'; }");
            page1Builder.Append("</style></head><body>");
            page1Builder.Append("<h1>नमस्ते दुनिया</h1>");
            page1Builder.Append("<p>यह एक उदाहरण पीडीएफ फाइल है।</p>");
            page1Builder.Append("<p>Hello1</p>");
            page1Builder.Append("</body></html>");

            using (var sr1 = new StringReader(page1Builder.ToString()))
            {
                parser.Parse(sr1);
            }


            ConvertHtmlToPdf(html, outputPath);
        }

        public void ConvertHtmlToPdf(string html, string outputPdfPath)
        {
            string fontPath = "C:/font/NirmalaUI.ttf";

            using (FileStream stream = new FileStream(outputPdfPath, FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                // 1. Register font
                var fontProvider = new XMLWorkerFontProvider(XMLWorkerFontProvider.DONTLOOKFORFONTS);
                fontProvider.Register(fontPath, "Nirmala");

                // 2. Set up CSS and HTML pipeline with custom font
                var cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                var cssAppliers = new CssAppliersImpl(fontProvider);
                var htmlContext = new HtmlPipelineContext(cssAppliers);
                htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());

                var pdfPipeline = new PdfWriterPipeline(pdfDoc, writer);
                var htmlPipeline = new HtmlPipeline(htmlContext, pdfPipeline);
                var cssPipeline = new CssResolverPipeline(cssResolver, htmlPipeline);

                var worker = new XMLWorker(cssPipeline, true);
                var parser = new XMLParser(true, worker);

                using (var sr = new StringReader(html))
                {
                    parser.Parse(sr);
                }

                pdfDoc.Close();
            }
        }

        private void LoadRecord()
        {
            StringBuilder sb = new StringBuilder();

            DataSet ds = null;
            DataTable dtAppFrm1 = null, dtAppFrm2 = null, dtSancLetter = null, dtEMISchedule = null;
            DataTable dtLoanAgr = null, dtAuthLetter = null, dtKotak = null, dtDigiDocDtls = null;
            DataTable dtScheduleOfCharges = null, dtSchedule = null, dtScheduleOwn = null;

           // CReports oRpt = new CReports();
            ds = new DataSet();
            string Id = "44310000554";
         //   ds = oRpt.GetDigitalDocs(Id, 0, Convert.ToInt32(Session[gblValue.UserId]));
            //dtAppFrm1 = ds.Tables[0];
            //dtAppFrm2 = ds.Tables[1];
            //dtSancLetter = ds.Tables[2];
            //dtEMISchedule = ds.Tables[3];
            //dtLoanAgr = ds.Tables[4];
            //dtAuthLetter = ds.Tables[5];
            //dtKotak = ds.Tables[6];

            //if (dtAppFrm1.Rows.Count > 0)
            //{
                try
                {
                    sb.Append("<style>");
                    sb.Append(".page-break { page-break-after: always; }");
                    sb.Append("</style>");

                    // Page 1 content (table 1)
                    // Title                    
                    sb.Append("<table style='width:100%; font-family:Arial, sans-serif; font-size:14px; border-collapse:collapse;'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='vertical-align:middle;'>");
                    //sb.Append("<img src='~/Images/SFB.png' alt='Company Logo' height='60' />");

                    sb.Append("</td>");
                    sb.Append("<td style='text-align:center; font-weight:bold; font-size:16px; padding-bottom:20px;'>Customer Declaration</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");

                    sb.Append("<div class='page-break'></div>"); // Page Break

                    #region First_Block
                    sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px; width:55%;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>ऋण का प्रकार: {0}</li>", "अरक्षित ऋण");
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("<td style='padding:5px; width:45%;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>आवेदन दिनांक: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanDt"]));
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>ऋण का उद्देश्य: {0}</li>", "एमईएल सरल");
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>अनुरोध की गई ऋण राशि: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanAmount"]));
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>ब्याज दर: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanRate"]));
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>अवधि: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanTenure"]));
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>मौजूदा ग्राहक:         हाँ / नहीं       य़दि हाँ, ग्राहक आईडी: </li>");
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("</table>");
                    #endregion

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                }
            //}

            //// 2. Convert HTML string to PDF using iTextSharp
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    Document doc = new Document(PageSize.A4, 30f, 30f, 30f, 30f);
            //    PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            //    doc.Open();

            //    using (StringReader sr = new StringReader(sb.ToString()))
            //    {
            //        HTMLWorker htmlParser = new HTMLWorker(doc);
            //        htmlParser.Parse(sr);
            //    }

            //    doc.Close();

            //    // 3. Send PDF to browser
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-disposition", "attachment;filename=MyReport.pdf");
            //    Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            //    Response.BinaryWrite(ms.ToArray());
            //    Response.End();
            //}
            ////Literal1.Text = sb.ToString();
        }

        protected void test()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Create PDF document
                Document doc = new Document(PageSize.A4, 30f, 30f, 30f, 30f);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // PAGE 1 - Build using StringBuilder
                StringBuilder sb1 = new StringBuilder();
                sb1.Append("<html ><head>");
                sb1.Append("<meta charset='utf-8'>");
                sb1.Append("</head>");
                sb1.Append("<body>");
                sb1.Append("<h2>Page 1 - Report</h2>");
                sb1.Append("<table border='1' width='100%' style='border-collapse:collapse;'>");
                for (int i = 1; i <= 10; i++)
                    sb1.Append("<tr><td>N'मौजूदा ग्राहक:'</td></tr>");
                sb1.Append("</table>");
                sb1.Append("</body></html>");

                // Render Page 1
                using (StringReader sr1 = new StringReader(sb1.ToString()))
                {
                    HTMLWorker hw = new HTMLWorker(doc);
                    hw.Parse(sr1);
                }

                // Force manual page break
                doc.NewPage();

                // PAGE 2
                StringBuilder sb2 = new StringBuilder();
                sb2.Append("<html><body>");
                sb2.Append("<h2>Page 2 - Continued</h2>");
                sb2.Append("<table border='1' width='100%' style='border-collapse:collapse;'>");
                for (int i = 1; i <= 10; i++)
                    sb2.Append("<tr><td>Page 2 - Row " + i + "</td></tr>");
                sb2.Append("</table>");
                sb2.Append("</body></html>");


                using (StringReader sr2 = new StringReader(sb2.ToString()))
                {
                    HTMLWorker hw = new HTMLWorker(doc);
                    hw.Parse(sr2);
                }

                // Close and output
                doc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=SimpleReport.pdf");
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
        }
        protected void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "mywindow", "printform();", true);

            //LoadRecord();

            //test1();


        }


        protected void test1()
        {

            // 1. Create HTML content using StringBuilder
            string fontPath = Server.MapPath("~/Fonts/Nirmala.ttf");
            string fontName = "NirmalaCustom";

            // PAGE 1 HTML with embedded font
            StringBuilder page1Builder = new StringBuilder();
            page1Builder.Append("<html><head><style>");
            page1Builder.Append("@font-face { font-family: 'NirmalaCustom'; src: url('Nirmala.ttf'); }");
            page1Builder.Append("body, h1, p { font-family: 'NirmalaCustom'; }");
            page1Builder.Append("</style></head><body>");
            page1Builder.Append("<h1>नमस्ते दुनिया</h1>");
            page1Builder.Append("<p>यह एक उदाहरण पीडीएफ फाइल है।</p>");
            page1Builder.Append("<p>Hello1</p>");
            page1Builder.Append("</body></html>");

            // PAGE 2 HTML
            StringBuilder page2Builder = new StringBuilder();
            page2Builder.Append("<html><head><style>");
            page2Builder.Append("@font-face { font-family: 'NirmalaCustom'; src: url('Nirmala.ttf'); }");
            page2Builder.Append("body, h2, p { font-family: 'NirmalaCustom'; }");
            page2Builder.Append("</style></head><body>");
            page2Builder.Append("<h2>दूसरा पृष्ठ</h2>");
            page2Builder.Append("<p>यह दूसरा पृष्ठ है।</p>");
            page2Builder.Append("<p>Hello2</p>");
            page2Builder.Append("</body></html>");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Register font
                XMLWorkerFontProvider fontProvider = new XMLWorkerFontProvider(XMLWorkerFontProvider.DONTLOOKFORFONTS);
                fontProvider.Register(fontPath, fontName);

                CssAppliers cssAppliers = new CssAppliersImpl(fontProvider);
                HtmlPipelineContext htmlContext = new HtmlPipelineContext(cssAppliers);
                htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());

                var htmlPipeline = new HtmlPipeline(htmlContext, new PdfWriterPipeline(document, writer));
                var pipeline = new XMLWorker(htmlPipeline, true);
                var parser = new XMLParser(pipeline);

                // Parse page 1
                using (var sr1 = new StringReader(page1Builder.ToString()))
                {
                    parser.Parse(sr1);
                }

                document.NewPage();

                // Parse page 2
                using (var sr2 = new StringReader(page2Builder.ToString()))
                {
                    parser.Parse(sr2);
                }

                document.Close();

                // Return to browser
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=HindiPDF.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(memoryStream.ToArray());
                Response.End();
            }

        }
    }
}