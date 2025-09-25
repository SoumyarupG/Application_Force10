using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CENTRUMBA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web.Hosting;
using System.Configuration;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using CENTRUMCA;

namespace CENTRUM_SARALVYAPAR.WebPages.Private.Report
{
    public partial class DigiDocBengali : CENTRUMBAse
    {
        string vpathCreateNewDoc = "";
        string IniPathHDrive = ConfigurationManager.AppSettings["IniPathHDrive"];
        string InitialApproachURL = ConfigurationManager.AppSettings["InitialApproachURL"];
        string DigiDocOTPURL = ConfigurationManager.AppSettings["DigiDocOTPURL"];

        string DigiDocOtpBucket = ConfigurationManager.AppSettings["DigiDocOtpBucket"];
        string DigiDocBucket = ConfigurationManager.AppSettings["DigiDocBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CDigiDoc oUsr = new CDigiDoc();
                DataTable dt = new DataTable();
                dt = oUsr.GetDigiDocReSign();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dR in dt.Rows)
                    {
                        GetReportDocForDigitalSign(dR["LoanAppNo"].ToString(), Convert.ToInt64(dR["DigiDocDtlsId"]));
                    }
                }
            }
        }

        public string Amtwords(double? numbers, Boolean paisaconversion = false)
        {
            var pointindex = numbers.ToString().IndexOf(".");
            var paisaamt = 0;
            if (pointindex > 0)
            {
                paisaamt = Convert.ToInt32(numbers.ToString().Substring(pointindex + 1));
                // Ensure paisaamt has two digits even if the paisa value has only one digit.
                if (paisaamt < 10) paisaamt *= 10;
            }

            int number = Convert.ToInt32(numbers);

            if (number == 0) return "Zero";
            if (number == -2147483648) return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";

            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }

            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

            num[0] = number % 1000; // units
            num[1] = number / 1000;
            num[2] = number / 100000;
            num[1] = num[1] - 100 * num[2]; // thousands
            num[3] = number / 10000000; // crores
            num[2] = num[2] - 100 * num[3]; // lakhs

            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }

            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10; // ones
                t = num[i] / 10;
                h = num[i] / 100; // hundreds
                t = t - 10 * h; // tens

                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0) sb.Append("and ");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }

            // Adding the paisa part
            if (paisaamt == 0 && !paisaconversion)
            {
                sb.Append("Rupees only");
            }
            else if (paisaamt > 0)
            {
                var paisatext = Amtwords(paisaamt, true);  // recursively call for paisa part.
                sb.AppendFormat("rupees {0}paise only", paisatext);
            }
            return sb.ToString().TrimEnd();
        }

        private string GetReportDocForDigitalSign(string pLoanAppNo, Int64 vDigiDocDtlsId)
        {
            DataSet ds = null;
            DataSet dsDigiDoc = null;
            DataTable dtAppFrm1 = null, dtAppFrm2 = null, dtSancLetter = null, dtEMISchedule = null;
            DataTable dtLoanAgr = null, dtAuthLetter = null, dtKotak = null, dtDigiDocDtls = null;
            DataTable dtScheduleOfCharges = null, dtSchedule = null, dtScheduleOwn = null; string finalHtml = "";
            string IDProofImage = "", IDProofImageBack = "", AddressProofImage = "", AddressProofImageBack = "", AddressProofImage2 = "", AddressProofImage2Back = "";
            CReports oRpt = new CReports();
            string vRptPath = "", vFileName = "No File Created";
            CDigiDoc oUsr = null;
            CMember oMem = null;
            CApiCalling oAC = new CApiCalling();

            // ReportDocument rptDoc = new ReportDocument();
            try
            {
                oUsr = new CDigiDoc();
                oMem = new CMember();

                DataTable dt = new DataTable();

                ds = new DataSet();
                ds = oRpt.GetDigitalDocs(pLoanAppNo, vDigiDocDtlsId, 1);
                dtAppFrm1 = ds.Tables[0];
                dtAppFrm2 = ds.Tables[1];
                dtSancLetter = ds.Tables[2];
                dtEMISchedule = ds.Tables[3];
                dtLoanAgr = ds.Tables[4];
                dtAuthLetter = ds.Tables[5];
                dtKotak = ds.Tables[6];
                string vBrCode = dtAppFrm1.Rows[0]["BranchCode"].ToString();
                oRpt = new CReports();
                dtScheduleOwn = oRpt.rptRepaySchedule(pLoanAppNo, vBrCode, "N", 0);

                string vEnqId = dtAppFrm1.Rows[0]["EnquiryId"].ToString();


                //IDProofImage = GetStringImage("IDProofImage.png", vEnqId, "I");
                //IDProofImageBack = GetStringImage("IDProofImageBack.png", vEnqId, "I");
                //AddressProofImage = GetStringImage("AddressProofImage.png", vEnqId, "I");
                //AddressProofImageBack = GetStringImage("AddressProofImageBack.png", vEnqId, "I");
                //AddressProofImage2 = GetStringImage("AddressProofImage2.png", vEnqId, "I");
                //AddressProofImage2Back = GetStringImage("AddressProofImage2Back.png", vEnqId, "I");

                oUsr = new CDigiDoc();
                dsDigiDoc = oUsr.getDigiDocDtlsByDocId(vDigiDocDtlsId, "", "Y");
                dtDigiDocDtls = dsDigiDoc.Tables[0];

                string vLoanAppId = "", vCustId = "";
                if (dtAppFrm1.Rows.Count > 0)
                {
                    vLoanAppId = dtAppFrm1.Rows[0]["LoanAppId"].ToString();
                    vCustId = dtAppFrm1.Rows[0]["CustId"].ToString();
                }

                oMem = new CMember();
                dtScheduleOfCharges = oMem.GetScheduleofChargesByLnAppId(pLoanAppNo, vDigiDocDtlsId);
                oMem = new CMember();
                dtSchedule = oMem.GetSanctionLetterDtlByLnAppId(pLoanAppNo, vDigiDocDtlsId);


                if (dtAppFrm1.Rows.Count > 0)
                {
                    vpathCreateNewDoc = ConfigurationManager.AppSettings["pathCreateNewDoc"];
                    vFileName = vpathCreateNewDoc + pLoanAppNo + ".pdf";

                    #region Formula
                    double vprocfee = 0, vprocfeeBC = 0, vInsuranceAmt = 0, vMedAmt = 0, vTotalPayRE = 0, vTotalPayTP = 0, vLoanAmt = 0, vIntAmt = 0, vTotalPI = 0, vAppAmount = 0;
                    vprocfee = Convert.ToDouble(dtScheduleOwn.Rows[0]["procfee"]);
                    vprocfeeBC = Convert.ToDouble(dtScheduleOwn.Rows[0]["procfeeBC"]);
                    vInsuranceAmt = Convert.ToDouble(dtScheduleOwn.Rows[0]["InsuranceAmt"]);
                    vMedAmt = Convert.ToDouble(dtScheduleOwn.Rows[0]["MedAmt"]);
                    vTotalPayRE = vprocfee + vInsuranceAmt + vMedAmt;
                    vTotalPayTP = vprocfeeBC + vInsuranceAmt + vMedAmt;
                    vLoanAmt = Convert.ToDouble(dtScheduleOwn.Rows[0]["LoanAmt"]);
                    vIntAmt = Convert.ToDouble(dtScheduleOwn.Rows[0]["IntAmt"]);
                    vTotalPI = vLoanAmt + vIntAmt;

                    vAppAmount = Convert.ToDouble(dtLoanAgr.Rows[0]["AppAmount"]);
                    string AmtInWrds = Amtwords(Convert.ToDouble(vAppAmount));
                    #endregion


                    #region HTML
                    StringBuilder sb = new StringBuilder();
                    try
                    {
                        sb.Append("<html>");
                        sb.Append("<head>");
                        sb.Append("<meta charset='UTF-8'>");
                        sb.Append("<style>");
                        sb.Append("body { font-family: 'Noto Sans Bengali' !important; }");
                        sb.Append(".page-break { page-break-after: always; }");
                        sb.Append(".xsmall_font { font-size: 12pt !important; }");
                        sb.Append(".small_font { font-size: 14pt !important; }");
                        sb.Append(".normal_font { font-size: 16pt !important; }");
                        sb.Append(".large_font { font-size: 18pt !important; }");
                        sb.Append(".xlarge_font { font-size: 26pt !important; }");
                        sb.Append("</style>");
                        sb.Append("</head>");
                        sb.Append("<body>");


                        #region 1st_Page
                       
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");


                        sb.Append("<table class='small_font' style='width:100%; border-collapse:collapse;' border='1'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px; width:50%;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>ঋণের ধরন:  {0}</li>", "অসুরক্ষিত ঋণ");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px; width:50%;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>আবেদনের তারিখ: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanDt"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>ঋণের উদ্দেশ্য: {0}</li>", "এমইএল সরল");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>অনুরোধকৃত ঋণের পরিমাণ: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanAmount"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>সুদের হার: {0}</li>", Convert.ToString(Math.Round(Convert.ToDouble(dtAppFrm1.Rows[0]["ApplLoanRate"]), 2)));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>মেয়াদ: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanTenure"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>বিদ্যমান গ্রাহক:         হ্যাঁ / না       হ্যাঁ হলে, গ্রাহক আইডি: </li>");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // th
                        sb.Append("<tr>");
                        sb.Append("<th colspan='2' class='small_font' style='text-align:center; padding:10px; background-color:#f2f2f2;'>আবেদনকারীর বিবরণ</th>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>আবেদনকারীর পুরো নাম: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["FullName"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>জন্ম তারিখ: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplDOB"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>লিঙ্গ: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["Gender"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>বৈবাহিক স্থিতি: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplMaritalName"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>শিক্ষাগত স্তর: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplQualification"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>ব্যবসার ধরন: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplOccupation"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>ব্যবসায়িক কার্যকলাপ: {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>আয়ের উৎস: {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>টার্নওভার (বার্ষিক): {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["AnnualIncome"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>বিভাগ: {0}</li>", "এমইএল মেল সরল ব্যাপার");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>ধর্ম: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplReligion"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>বিশেষভাবে সক্ষম: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["IsAbledYN"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>শ্রেণী: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["SpeciallyAbled"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // th
                        sb.Append("<tr>");
                        sb.Append("<th colspan='2' class='small_font' style='text-align:center; padding:10px; background-color:#f2f2f2;'>যৌথ আবেদনকারীর বিবরণ</th>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>যৌথ আবেদনকারীর পুরো নাম: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoFullName"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>জন্ম তারিখ:: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplDOB"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>লিঙ্গ: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoGender"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>বৈবাহিক স্থিতি: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplMaritalName"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>শিক্ষাগত স্তর: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplQualification"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>ব্যবসার ধরন: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplOccupation"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>ব্যবসায়িক কার্যকলাপ: {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>আয়ের উৎস: {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>টার্নওভার (বার্ষিক): {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>বিভাগ: {0}</li>", "এমইএল সরল ব্যাপার");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>ধর্ম: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplReligion"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;background-color:#d9d9d9;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.Append("<li><b>আবেদনকারী (কেওয়াইসি-এর বিশদ)</b></li>");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;background-color:#d9d9d9;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.Append("<li><b>যৌথ আবেদনকারী (কেওয়াইসি-এর বিশদ)</b></li>");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>কেওয়াইসি-এর ধরন: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["IDType1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>কেওয়াইসি-এর ধরন: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoIDType1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>কেওয়াইসি নং: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["IDNo1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>কেওয়াইসি নং: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoIDNo1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;word-wrap:break-word; white-space:normal; max-width:300px;vertical-align:top;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>আবেদনকারীর স্থায়ী ঠিকানা: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplCAddress1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;word-wrap:break-word; white-space:normal; max-width:300px;vertical-align:top;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>সহ-আবেদনকারীর স্থায়ী ঠিকানা: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplCAddress1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;word-wrap:break-word; white-space:normal; max-width:300px;vertical-align:top;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>আবেদনকারীর ঠিকানা: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplPAddress1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;word-wrap:break-word; white-space:normal; max-width:300px;vertical-align:top;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>সহ-আবেদনকারীর ঠিকানা: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplPAddress1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>পিন কোড: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplPPinCode"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>পিন কোড: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplPPinCode"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>বাসস্থানের মালিকানা: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplResidentialStatR"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>বাসস্থানের মালিকানা: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplResidentialStatR"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>OSV সম্পূর্ণ: {0}</li>", "হ্যাঁ");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>OSV সম্পূর্ণ: {0}</li>", "হ্যাঁ");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // th
                        sb.Append("<tr>");
                        sb.Append("<th colspan='2' class='small_font' style='text-align:center; padding:10px; background-color:#f0f0f0;'>ব্যবসার বিশদ</th>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>ব্যবসার ঠিকানা: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplBusiAddress1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>পিন কোড: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplBusiPincode"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>পিন কোড: {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>মোবাইল নম্বর: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplBusiMobile"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>যোগাযোগের অতিরিক্ত নম্বর: {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 2nd_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%; border-collapse:collapse;' border='1'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px; width:55%;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.Append("<li><b>A. ব্যবসার রেফারেন্স 1</b></li>");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px; width:45%;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.Append("<li><b>B. ব্যবসার রেফারেন্স 2</b></li>");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>নাম: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["BR1Name"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>নাম: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["BR2Name"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>যোগাযোগ না.: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["BR1ContactNo"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>যোগাযোগ না.: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["BR2ContactNo"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px; word-wrap:break-word; white-space:normal; max-width:300px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>ঠিকানা: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["BR1PlaceOfOffice"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px; word-wrap:break-word; white-space:normal; max-width:300px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>ঠিকানা: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["BR2PlaceOfOffice"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // th
                        sb.Append("<tr>");
                        sb.Append("<th colspan='2' class='small_font' style='text-align:center; padding:10px; background-color:#f0f0f0;'>ব্যাঙ্কিং ও বিদ্যমান ঋণের বিশদ</th>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        #region Bank_Details
                        sb.Append("<h3 style='padding-top:20px;text-decoration: underline;'><strong>ব্যাঙ্কিং:</strong></h3>");
                        sb.Append("<table  class='small_font' style='border-collapse: collapse; width: 100%; text-align: left;' border='1' >");
                        // Add header row
                        sb.Append("<tr>");
                        sb.Append("<th style='padding: 4px;'>অ্যাকাউন্ট ধারক(আবেদনকারী)</th>");
                        sb.Append("<th style='padding: 4px;'>ব্যাঙ্ক ও শাখার নাম</th>");
                        sb.Append("<th style='padding: 4px;'>অ্যাকাউন্ট নং</th>");
                        sb.Append("<th style='padding: 4px;'>কারেন্ট/সেভিং</th>");
                        sb.Append("<th style='padding: 4px;'>আইএফএসসি কোড</th>");
                        sb.Append("</tr>");


                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:4px;'>{0}</td>", Convert.ToString(dtAppFrm1.Rows[0]["ApplAcHoldername"]));
                        sb.AppendFormat("<td style='padding:4px;'>{0}</td>", Convert.ToString(dtAppFrm1.Rows[0]["ApplAcBankName"]));
                        sb.AppendFormat("<td style='padding:4px;'>{0}</td>", Convert.ToString(dtAppFrm1.Rows[0]["ApplAcNo"]));
                        sb.AppendFormat("<td style='padding:4px;'>{0}</td>", Convert.ToString(dtAppFrm1.Rows[0]["ApplAccHoldingType"]));
                        sb.AppendFormat("<td style='padding:4px;'>{0}</td>", Convert.ToString(dtAppFrm1.Rows[0]["ApplIfscCode"]));
                        sb.Append("</tr>");

                        sb.Append("</table>");
                        //sb.Append("</td></tr>");
                        #endregion

                        #region EXISTING_LOAN
                        sb.Append("<h3 style='padding-top:20px;text-decoration: underline;'><strong>বিদ্যমান ঋণ:</strong></h3>");
                        sb.Append("<table  class='small_font' style='border-collapse: collapse; width: 100%;  text-align: left;' border='1' >");
                        // Add header row
                        sb.Append("<tr>");
                        sb.Append("<th style='padding: 4px;'>ঋণ প্রদানকারী প্রতিষ্ঠানের নাম</th>");
                        sb.Append("<th style='padding: 4px;'>ঋণের উদ্দেশ্য</th>");
                        sb.Append("<th style='padding: 4px;'>ঋণের পরিমাণ</th>");
                        sb.Append("<th style='padding: 4px;'>ঋণের মেয়াদ(মাস)</th>");
                        sb.Append("<th style='padding: 4px;'>মাসিক কিস্তি</th>");
                        sb.Append("<th style='padding: 4px;'>বর্তমান বকেয়া</th>");
                        sb.Append("<th style='padding: 4px;'>ব্যালেন্স মেয়াদ (মাস)</th>");
                        sb.Append("</tr>");

                        // Fill rows from DataTable
                        foreach (DataRow row in dtAppFrm2.Rows)
                        {
                            sb.Append("<tr>");
                            sb.AppendFormat("<td style='padding:4px;'>{0}</td>", row["CompanyName"]);
                            sb.AppendFormat("<td style='padding:4px;'>{0}</td>", "");
                            sb.AppendFormat("<td style='padding:4px;'>{0}</td>", row["LoanAmount"]);
                            sb.AppendFormat("<td style='padding:4px;'>{0}</td>", row["Term"]);
                            sb.AppendFormat("<td style='padding:4px;'>{0}</td>", row["EMIAmount"]);
                            sb.AppendFormat("<td style='padding:4px;'>{0}</td>", row["BalancePrinc"]);
                            sb.AppendFormat("<td style='padding:4px;'>{0}</td>", row["BalTenure"]);
                            sb.Append("</tr>");
                        }
                        // End table
                        sb.Append("</table>");
                        //sb.Append("</td></tr>");

                        #endregion


                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 3rd_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='xsmall_font' style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr><td style='padding-top:5px;'><strong>ঘোষণা/সম্মতি</strong></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:1px; width:100%;'>");
                        sb.Append("<ol>");
                        sb.Append("<li>ঋণগ্রহণকারী(রা) ঘোষণা করেন যে আবেদন ফর্মে দেওয়া সমস্ত বিবরণ ও তথ্য সত্য, সঠিক, ও সম্পূর্ণ এবং কোনও প্রাসঙ্গিক তথ্য গোপন করা/লুকিয়ে রাখা হয় নি।</li>");
                        sb.Append("<li>ঋণগ্রহণকারী(রা) উপস্থাপিত যেকোনো তথ্যে কোনও পরিবর্তন সম্পর্কে ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্ককে অবিলম্বে জানানোর দায়িত্ব গ্রহণ করেন। অধিকন্তু ঋণগ্রহণকারী(রা) ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্ক এবং/অথবা এর গ্রুপ কোম্পানিগুলি এবং এর এজেন্টদের প্রয়োজনীয় যে কোনও অতিরিক্ত তথ্য/নথিপত্র প্রদান করার দায়িত্বও গ্রহণ করেন।</li>");
                        sb.Append("<li>ঋণগ্রহণকারী(রা) ঘোষণা করেন যে তহবিল নির্ধারিত উদ্দেশ্যে ব্যবহার করা হবে এবং ফটকাবাজি অথবা অসামাজিক উদ্দেশ্যের জন্য ব্যবহার করা হবে না।</li>");                       
                        sb.Append("<li>ঋণগ্রহণকারী(রা) বোঝেন যে ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্ক এবং/অথবা এর গ্রুপ কোম্পানিগুলি এই আবেদনের সাথে জমা দেওয়া ছবি এবং নথিপত্র রেখে দেওয়ার অধিকার সংরক্ষণ করে।</li>");
                        sb.Append("<li>ঋণগ্রহণকারী(রা) বোঝেন যে এই ঋণের অনুমোদন ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্কের একক বিবেচনার উপর নির্ভরশীল এবং প্রয়োজনীয় নথিপত্র ও ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্কের প্রয়োজনীয় অনুযায়ী অন্যান্য নিয়মানুগত্যতা সম্পন্ন করার পরেই দেওয়া হবে। উপরন্তু, ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্ক আবেদনটি (ঋণগ্রহণকারীকে জানিয়ে বা না জানিয়ে) প্রত্যাখ্যান করার অধিকার সংরক্ষণ করে, এবং ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্ক এই ধরনের প্রত্যাখ্যান বা এই ধরনের প্রত্যাখ্যান করার কথা আমাকে জানাতে কোনও বিলম্বের জন্য কোনও ভাবেই দায়ী/দায়বদ্ধ হবে না।</li>");                       
                        sb.Append("<li>ঋণগ্রহণকারীর(দের) ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্কের এর সহযোগী প্রতিষ্ঠান, সরকারী/নিয়ামক সংস্থা, ক্রেডিট ব্যুরো/রেটিং এজেন্সি, পরিষেবা প্রদানকারী, ব্যাঙ্ক/আর্থিক প্রতিষ্ঠান, সিকেওয়াইসি রেজিস্ট্রি, কেওয়াইসি তথ্যের জন্য তৃতীয় পক্ষ এবং এই ধরনের অন্য কোনও কর্তৃপক্ষের কাছে প্রয়োজন অনুযায়ী তথ্য বিনিময় এবং শেয়ার করায় কোনও আপত্তি নেই।</li>");
                        sb.Append("<li>ঋণগ্রহণকারী(রা) ঋণগ্রহণকারীর কেওয়াইসি ও ক্রেডিট সংক্রান্ত তথ্য/নথিপত্র ইউনিক আইডেন্টিফিকেশন অথোরিটি অফ ইন্ডিয়া, ক্রেডিট ইনফরমেশন ব্যুরো অফ ইন্ডিয়া এবং অন্যান্য সংস্থাসহ তৃতীয় পক্ষের থেকে অর্জন করার জন্য ঋণ গ্রহণকারীর ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্ককে দেওয়া সম্মতি স্বীকার করে এবং অধিকতর সম্মতি দেন যে ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্ক নিজে অথবা অনুমোদিত ব্যক্তিদের মাধ্যমে, সময়ে সময়ে ঋণগ্রহণকারীর অকৃত্রিমতা এবং/অথবা ঋণ গ্রহণের যোগ্যতা নির্ধারণ করতে প্রদত্ত যেকোনো তথ্য যাচাই করতে, ক্রেডিট রেফারেন্স, চাকরির বিশদ পরীক্ষা করে দেখতে এবং কেওয়াইসি সংক্রান্ত নথিপত্র অথবা ক্রেডিট রিপোর্টগুলি সংগ্রহ করতে পারে। ঋণগ্রহণকারী ইউনিক আইডেন্টিফিকেশন অথোরিটি অফ ইন্ডিয়া (ইউআইডিএআই) অথবা এই ধরনের অন্য কোনও তৃতীয় পক্ষকে ঋণগ্রহণকারী সম্পর্কিত তথ্য ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্কের সাথে শেয়ার করতে সম্মতি জানাতে ঐচ্ছিক সম্মতিরও অধিকতর স্বীকৃতি দেন। ঋণগ্রহণকারী(রা) অধিকতর স্বীকৃতি দেন যে ইউআইডিএআই-এর মাধ্যমে যাচাই পরবর্তী শেয়ার করা যেকোনো তথ্যের প্রকৃতি ব্যাখ্যা করা হয়েছে এবং এই ধরনের তথ্য ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্কের থেকে ঋণ নেওয়ার উদ্দেশ্যের জন্য ব্যবহার করা হবে।</li>");
                        sb.Append("<li>ঋণগ্রহণকারী(রা) এতদ্বারা উপরে নিবন্ধিত নম্বর/ইমেল অ্যাড্রেসে এসএমএস/ইমেলের মাধ্যমে সেন্ট্রাল কেওয়াইসি রেজিস্ট্রি থেকে তথ্য পাওয়ার বিষয়ে সম্মতি প্রদান করেন।</li>");
                        sb.Append("<li>ঋণগ্রহণকারী(রা) নিশ্চিত করেন যে ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্কের সুদের হার এবং ঋণের ক্ষেত্রে প্রযোজ্য অন্যান্য চার্জগুলি ভবিষ্যতে পরিবর্তন করার বিবেচনা করার সম্পূর্ণ অধিকার থাকবে।</li>");   
                        sb.Append("<li>একাধিক ঋণগ্রহণকারীর(দের) ক্ষেত্রে, প্রত্যেক ঋণগ্রহণকারী(রা) সম্মত হন এবং দায়িত্ব গ্রহণ করেন যে প্রত্যেক ঋণগ্রহণকারী(রা) ঋণের অধীনে অর্থ প্রদান করতে যৌথ অথবা এককভাবে দায়বদ্ধ থাকবেন।</li>");
                        sb.Append("<li>ঋণগ্রহণকারী(রা) বোঝেন যে যেকোনো বীমা পণ্যের ক্রয় সম্পূর্ণরূপে ঐচ্ছিক এবং তা ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্কের থেকে অন্য কোনও সুবিধার পূর্বশর্তের সাথে সংযুক্ত নয়।</li>");
                        sb.Append("<li>ঋণগ্রহণকারী(রা) বিবৃত করেন যে প্রত্যেক ঋণগ্রহণকারী, তার পরিচালক/অংশীদারদের (যদি থাকে) দেউলিয়া ঘোষণা করা হয়নি অথবা তাদের বিরুদ্ধে কোনও শোধনক্ষমতা/দেউলিয়াত্ব সংক্রান্ত কার্যধারা শুরু হয়নি।</li>");
                        sb.Append("<li>ঋণগ্রহণকারীর(দের) ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্ক এবং/অথবা এর গ্রুপ কোম্পানিগুলি এবং/অথবা এর এজেন্টদের আমাকে/আমাদের যেকোনো পদ্ধতির (টেলিফোন কল, এসএমএস/ ইমেল, চিঠি, ইত্যাদি সহ) মাধ্যমে ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্ক এবং/অথবা এর গ্রুপ কম্পানিগুলির প্রদত্ত বিভিন্ন পণ্য, অফার ও পরিষেবাগুলি নিয়ে তথ্য প্রদান করার ক্ষেত্রে কোনও আপত্তি নেই।  আমি নিশ্চিত করি যে ‘ভারতীয় টেলিকম নিয়ামক কর্তৃপক্ষ’-এর বিবৃতি অনুযায়ী “ন্যাশনাল ডু নট কল রেজিস্ট্রি” (কল না করার রেজিস্ট্রি) তে উল্লেখ করা অযাচিত যোগাযোগ সংক্রান্ত আইনগুলি আমাকে প্রদান করা এই ধরনের তথ্য/যোগাযোগের ক্ষেত্রে প্রযোজ্য হবে না।</li>");
                        sb.Append("<li>ঋণগ্রহণকারী ঋণ চুক্তি এবং সম্পর্কিত অন্য যেকোনো নথিপত্র ইলেক্ট্রনিক সই/ই-সাইনিং যার মধ্যে অন্তর্ভুক্ত আধার ভিত্তিক ই-সই এবং নিবন্ধিত মোবাইল নম্বরে পাওয়া ওয়ান টাইম পাসওয়ার্ড (ওটিপি)-এর মাধ্যমে সম্পন্ন করার সম্মতি প্রদান করেছেন।  ওটিপি-এর সাথে আপোস না করা অথবা কোনও অননুমোদিত ব্যবহারকারীর সাথে তা শেয়ার না করা নিশ্চিত করা শুধুমাত্র ঋণগ্রহণকারীরই দায়িত্ব হবে। ইলেক্ট্রনিক সই-এর থেকে উদ্ভূত ঋণদাতার সব রেকর্ড লেনদেনের অকৃত্রিমতা এবং নির্ভুলতার অকাট্য প্রমাণ হবে এবং ঋণগ্রহণকারীর উপর বাধ্যতামূলক হবে।</li>");
                        sb.Append("<li>উপরোক্ত বিষয়বস্তুগুলি আমার/আমাদের স্থানীয় ভাষায় পড়ে আমাকে/আমাদেরকে ব্যাখ্যা করা হয়েছে।</li>");
                        sb.Append("</ol>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 4th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td  style='font-weight:bold;text-decoration: underline; text-align:center; padding:10px;'>");
                        sb.Append("ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্ক লিমিটেড/অন্যান্য ব্যাঙ্কগুলির সাথে সম্পর্কের বিষয়ে গ্রাহকের ঘোষণা");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // Question (a)
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<b>(a)</b> আবেদনকারী কি ব্যাংকের পরিচালক/ব্যাংকের পরিচালকের আত্মীয়, অথবা ব্যাংকের ঊর্ধ্বতন কর্মকর্তা, অথবা ব্যাংকের ঊর্ধ্বতন কর্মকর্তার আত্মীয়?");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>☐ হ্যাঁ &nbsp;&nbsp;&nbsp;☑ না</td>");
                        sb.Append("</tr>");

                        // Question (b)
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<b>(b)</b> আবেদনকারী কি অন্য ব্যাংকের পরিচালক/অন্য ব্যাংকের পরিচালকের আত্মীয়?");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>☐ হ্যাঁ &nbsp;&nbsp;&nbsp;☑ না</td>");
                        sb.Append("</tr>");

                        // Question (c)
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<b>(c)</b> আবেদনকারী কি এমন একটি ফার্ম/কোম্পানি যেখানে অন্যান্য ব্যাংকের পরিচালক/অন্য কোন ব্যাংকের পরিচালকদের আত্মীয়স্বজন অংশীদার/জামিনদার/পরিচালক/নিয়ন্ত্রণে*/প্রধান শেয়ারহোল্ডার হিসেবে আগ্রহী?");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>☐ হ্যাঁ &nbsp;&nbsp;&nbsp;☑ না</td>");
                        sb.Append("</tr>");

                        // Question (d)
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>");
                        sb.Append("<b>(d)</b> আবেদনকারী কি এমন একটি ফার্ম/কোম্পানি যেখানে ব্যাংকের পরিচালক বা ব্যাংকের পরিচালকের আত্মীয়স্বজন বা ব্যাংকের ঊর্ধ্বতন কর্মকর্তা বা তাদের আত্মীয়স্বজন অংশীদার/ব্যবস্থাপক/কর্মচারী/জামিনদার/পরিচালক/নিয়ন্ত্রণে*/প্রধান শেয়ারহোল্ডার হিসেবে আগ্রহী?");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>☐ হ্যাঁ &nbsp;&nbsp;&nbsp;☑ না</td>");
                        sb.Append("</tr>");

                        // Footnote
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:10px;'>");
                        sb.Append("* “নিয়ন্ত্রণ” শব্দটির মধ্যে থাকবে সংখ্যাগরিষ্ঠ পরিচালক নিয়োগের অধিকার অথবা ব্যক্তিগতভাবে বা সম্মিলিতভাবে, প্রত্যক্ষ বা পরোক্ষভাবে, তাদের শেয়ারহোল্ডিং বা ব্যবস্থাপনা অধিকার, শেয়ারহোল্ডারদের চুক্তি বা ভোটদান চুক্তির মাধ্যমে বা অন্য কোনও উপায়ে, পরিচালনা বা নীতিগত সিদ্ধান্ত নিয়ন্ত্রণের অধিকার।<br><br>");
                        sb.Append("** “প্রধান শেয়ারহোল্ডার” শব্দটির অর্থ হল পরিশোধিত শেয়ার মূলধনের ১০% বা তার বেশি বা পরিশোধিত শেয়ারে পাঁচ কোটি টাকা ধারণকারী ব্যক্তি, যার মধ্যে তফসিলি সমবায় ব্যাংকের পরিচালক, ইউনিটি স্মল ফাইন্যান্স ব্যাংক বা অন্য কোনও ব্যাংক দ্বারা প্রতিষ্ঠিত মিউচুয়াল ফান্ড/ভেঞ্চার ক্যাপিটাল তহবিলের সাবসিডিয়ারি/ট্রাস্টির পরিচালক। ঋণগ্রহীতা বোঝেন এবং সম্মত হন যে যদি ঋণগ্রহীতার দ্বারা প্রদত্ত উপরোক্ত ঘোষণাটি মিথ্যা প্রমাণিত হয়, তাহলে ব্যাংক ঋণ সুবিধা প্রত্যাহার এবং/অথবা প্রত্যাহার করার অধিকারী হবে।<br><br>"); 
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        //KFS Image
                        #region 5th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%;border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align:center; font-weight:bold; padding:10px;'>");
                        sb.Append("কেওয়াইসি ছবিগুলি");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // Spacer
                        sb.Append("<tr><td colspan='2' style='height:10px;'></td></tr>");

                        #region Row 1: KYC-1 (Applicant)
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center; padding:10px; border:1px solid #000;width:50%;'>");
                        //sb.Append("<img src='" + IDProofImage + "' alt='KYC-1 Applicant Front' height='230' width='100%' />");
                        sb.Append("<img src='https://dchot1.bijliftt.com:9000/jlginitialapproach/10213710789_AddressProofImage.png' alt='KYC-2 Applicant Front' height='230' width='100%' />");
                        sb.Append("</td>");
                        sb.Append("<td style='text-align:center; padding:10px; border:1px solid #000;width:50%;'>");
                        //sb.Append("<img src='" + IDProofImageBack + "' alt='KYC-1 Applicant Back' height='230' width='100%' />");
                        sb.Append("<img src='https://dchot1.bijliftt.com:9000/jlginitialapproach/10213710789_AddressProofImage.png' alt='KYC-2 Applicant Front' height='230' width='100%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center; padding:2px;'>কেওয়াইসি-1 (আবেদনকারী) সামনের দিক</td>");
                        sb.Append("<td style='text-align:center; padding:2px;'>কেওয়াইসি-1 পেছনের দিক</td>");
                        sb.Append("</tr>");
                        #endregion

                        // Spacer
                        sb.Append("<tr><td colspan='2' style='height:20px;'></td></tr>");

                        #region Row 2: KYC-2 (Applicant)
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center; padding:10px; border:1px solid #000;'>");
                        //sb.Append("<img src='" + AddressProofImage + "' alt='KYC-2 Applicant Front' height='230' width='100%' />");
                        sb.Append("<img src='https://dchot1.bijliftt.com:9000/jlginitialapproach/10213710789_AddressProofImage.png' alt='KYC-2 Applicant Front' height='230' width='100%' />");
                        sb.Append("</td>");
                        sb.Append("<td style='text-align:center; padding:10px; border:1px solid #000;'>");
                        //sb.Append("<img src='" + AddressProofImageBack + "' alt='KYC-2 Applicant Back' height='230' width='100%' />");
                        sb.Append("<img src='https://dchot1.bijliftt.com:9000/jlginitialapproach/10213710789_AddressProofImage.png' alt='KYC-2 Applicant Front' height='230' width='100%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center; padding:2px;'>কেওয়াইসি-2 (আবেদনকারী) সামনের দিক</td>");
                        sb.Append("<td style='text-align:center; padding:2px;'>কেওয়াইসি-2 পেছনের দিক</td>");
                        sb.Append("</tr>");
                        #endregion

                        // Spacer
                        sb.Append("<tr><td colspan='2' style='height:20px;'></td></tr>");

                        #region Row 3: KYC-1 (Co-Applicant)
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center; padding:10px; border:1px solid #000;'>");
                        //sb.Append("<img src='" + AddressProofImage2 + "' alt='KYC-1 Co-Applicant Front' height='230' width='100%' />");
                        sb.Append("<img src='https://dchot1.bijliftt.com:9000/jlginitialapproach/10213710789_AddressProofImage.png' alt='KYC-2 Applicant Front' height='230' width='100%' />");
                        sb.Append("</td>");
                        sb.Append("<td style='text-align:center; padding:10px; border:1px solid #000;'>");
                        //sb.Append("<img src='" + AddressProofImage2Back + "' alt='KYC-1 Co-Applicant Back' height='230' width='100%' />");
                        sb.Append("<img src='https://dchot1.bijliftt.com:9000/jlginitialapproach/10213710789_AddressProofImage.png' alt='KYC-2 Applicant Front' height='230' width='100%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center; padding:2px;'>কেওয়াইসি-1 (যৌথ আবেদনকারী) সামনের দিক</td>");
                        sb.Append("<td style='text-align:center; padding:2px;'>কেওয়াইসি-1 পেছনের দিক</td>");
                        sb.Append("</tr>");
                        #endregion

                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // Start Sanction Letter
                        #region 6th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;font-weight:bold;'>অনুমোদন চিঠি</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>তারিখ: {0}</td></tr>", Convert.ToString(dtSancLetter.Rows[0]["FinalSanctionDate"]));
                        sb.AppendFormat("<tr><td style='width:100%;'>শ্রী/শ্রীমতি: {0}</td></tr>", Convert.ToString(dtSancLetter.Rows[0]["CompanyName"]));
                        sb.AppendFormat("<tr><td style='width:100%;'>ঠিকানা: {0}</td></tr>", Convert.ToString(dtSancLetter.Rows[0]["ComAddress"]));
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td><hr></td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>বিষয়: মেল সরল ব্যাপার আবেদন নং <span style='border-bottom: 1px dotted black;'>{0}</span> এর জন্য অনুমোদন চিঠি</td></tr>", Convert.ToString(dtSancLetter.Rows[0]["SanctionId"]));
                        sb.Append("<tr><td style='padding:10px;'>উক্ত বিষয়ের আবেদন সূত্রে, ইউনিটি স্মল ফাইন্যান্স ব্যাংক (‘ঋণদাতা’ যে অভিব্যক্তিটি বলতে, প্রসঙ্গত অন্যথা প্রয়োজন না হলে, এর উত্তরসূরী ও নিযুক্তদের বোঝাবে এবং অন্তর্ভুক্ত করবে) নীচে বিস্তারিত অনুসারে নিয়ম ও শর্তাবলীর সাপেক্খের অধীনে আনন্দের সাথে ঋণটি অনুমোদন করছে।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;width:35%;'>বিশিষ্ট বিবরণ</td>");
                        sb.Append("<td  style='padding:2px;width:65%;'>বিবরণ</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>ঋণগ্রহণকারীর নাম</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:left;'>{0}</td>", Convert.ToString(dtSancLetter.Rows[0]["CompanyName"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>যৌথ ঋণগ্রহণকারীর নাম</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:left;'>{0}</td>", Convert.ToString(dtSancLetter.Rows[0]["CoAppName"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>সুবিধার প্রকার</td>");
                        sb.Append("<td  style='padding:2px;'>টার্ম ঋণ</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>ঋণের অনুমোদিত পরিমাণ</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:left;'>{0} /-টাকা</td>", Convert.ToString(dtSancLetter.Rows[0]["SanctionAmt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>ঋণের মেয়াদ</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:left;'>{0} মাস</td>", Convert.ToString(dtSancLetter.Rows[0]["Tenure"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>সুদের প্রকার</td>");
                        sb.Append("<td  style='padding:2px;'>অধোগামী সুদের হার</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>সুদের হার</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:left;'>{0} % প্রতি বছর <br> ইউএসএফবি-এর কাছে নিম্নলিখিতগুলির কোনওটি ঘটলে সুদের হার পুনরায় স্থির করার অধিকার থাকবে: 1. ডিফল্টের ঘটনা ঘটলে; অথবা 2. অন্য কোনও ঘটনা যা ঋণদাতার মতে ঋণদাতার অভ্যন্তরীণ নীতি অনুসারে সুদের হার পুনরায় স্থির করার জন্য প্রয়োজন। <br> সুদের হারে যেকোনো পরিবর্তন পরবর্তীকালীন হবে এবং ঋণগ্রহণকারীকে তা জানানো হবে।  ঋণগ্রহণকারী সংশোধিত তারিখের থেকে, ইউএসএফবি-কে সংশোধিত সুদের হারে সুবিধার উপর সুদ প্রদান করবেন।</td>", Convert.ToString(Math.Round(Convert.ToDouble(dtSancLetter.Rows[0]["RIntRate"]), 2)));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>সমভাবে বন্টন করা মাসিক কিস্তি (ইএমআই)-এর সংখ্যা</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:left;'>{0}</td>", Convert.ToString(dtSancLetter.Rows[0]["Tenure"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>ইএমআই</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:left;'>{0} টাকা, মাসিক/ত্রৈমাসিকভাবে প্রদেয়</td>", Convert.ToString(dtSancLetter.Rows[0]["EMI"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>ইএমআই-এর তারিখ</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:left;'>প্রতি মাস/ত্রৈমাসিকের {0} তারিখ</td>", "5th");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>প্রক্রিয়াকরণ ফি</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:left;'>{0} টাকা, ফেরতযোগ্য নয়, অগ্রিমে প্রদানযোগ্য</td>", Convert.ToString(dtSancLetter.Rows[0]["ProcFees"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>অর্থ প্রদানে বিলম্ব/দণ্ডনীয় চার্জগুলি</td>");
                        sb.Append("<td  style='padding:2px;'>(মোটা হরফে হতে হবে)</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 7th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='2' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;'>অনুসূচী I</td></tr>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;'>মূলগত নিয়ম ও শর্তাবলী লঙ্ঘন করলে দণ্ডনীয় চার্জগুলির বিশদ</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='xsmall_font' border='1' cellspacing='0' cellpadding='3' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;width:20%;'>ক্রমিক নং</td>");
                        sb.Append("<td  style='padding:2px;width:50%;'>মূলগত নিয়ম ও শর্তাবলী</td>");
                        sb.Append("<td  style='padding:2px;width:50%;'>দণ্ডনীয় চার্জগুলি</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>1.</td>");
                        sb.Append("<td  style='padding:2px;'>ব্যাংককে প্রদেয় বকেয়াগুলির (প্রিন্সিপাল, সুদ, খরচ, চার্জ, কর, ব্যয় ইত্যাদি সহ) পরিশোধে ডিফল্ট বা বিলম্বের জন্য</td>");
                        sb.Append("<td  style='padding:2px;'>প্রতি মাসে 3% (প্রতি বছরে 36%) + বকেয়া থাকা দিনগুলির হিসাবে বকেয়া পরিমাণের উপর 18% জিএসটি</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>2.</td>");
                        sb.Append("<td  style='padding:2px;'>ডিফল্টের ঘটনা</td>");
                        sb.Append("<td  style='padding:2px;'>সর্বনিম্ন 500/- টাকা (ভারতীয় মুদ্রায় মাত্র পাঁচশো টাকা) এবং সর্বাধিক 700/- টাকা (ভারতীয় মুদ্রায় মাত্র সাতশো টাকা)</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='2' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td>&nbsp;</td></tr>");
                        sb.Append("<tr><td style='padding:2px;'>");
                        sb.Append("<ul>");
                        sb.Append("<li>উপরের দণ্ডনীয় চার্জগুলি প্রযোজ্য কর সাপেক্ষ।</li>");
                        sb.Append("<li>অন্য সমস্ত চার্জগুলি ঋণ চুক্তির অধীনে নির্দিষ্টভাবে করা বিবৃতি অনুযায়ী প্রযোজ্য থাকা অব্যাহত থাকবে।</li>");
                        sb.Append("<li>ঋণগ্রহণকারী অবগত আছেন যে এখনও পর্যন্ত দেওয়া অনুসূচী, নিয়ম ও শর্তাবলী, চার্জগুলি বর্তমানে বিদ্যমান এবং পরিবর্তিত হতে এবং/অথবা নয়া/নতুন শর্তাবলী, চার্জগুলি যেকোনো সময় ও সময়ে সময়ে ব্যাংকের একক ও পরম বিবেচনায় যোগ করা হতে পারে এবং এই ধরনের চার্জগুলি ঋণগ্রহণকারীর কাছে বাধ্যতামূলক হবে। চার্জের আপডেট করা অনুসূচীটি নিয়ামক প্রকাশ- নীতি বিভাগের অধীনে ব্যাংকের ওয়েবসাইট www.theunitybank.comএ প্রদর্শিত থাকবে।</li>");
                        sb.Append("</ul>");
                        sb.Append("</td></tr>");

                        sb.Append("<tr>");
                        sb.Append("<td>এই চিঠি রচনাকারী অংশ হিসেবে থাকা সমস্ত নিয়ম ও শর্তাবলী/সাধারণ শর্তাবলী এবং ধারাগুলিকে ঋণচুক্তির সাথে প্রয়োজনীয় পরিবর্তনসহ একযোগে পড়তে হবে। এখানে নির্দিষ্টভাবে ব্যক্ত না করা এবং ঋণ চুক্তির অংশ হওয়া সমস্ত নিয়মগুলিকে এই চিঠি, যথা অনুমোদন চিঠি, ঋণ চুক্তি, এমআইটিসি-এর সাথে একযোগে পড়তে হবে এবং এইসব লেনদেনের থেকে রচিত অন্য সমস্ত নথিপত্রকে লেনদেনের নথিপত্র হিসেবে ব্যাখ্যা করা হবে।</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td>ঋণগ্রহণকারী(রা) বোঝেন যে ঋণদাতা ঝুঁকি ভিত্তিক মূল্য নির্ধারণ গ্রহণ করেছে যা গ্রাহকের প্রোফাইল, আর্থিক বিষয়, তহবিলের উৎস, গ্রাহকের ঝুঁকির প্রোফাইল, ঋণ দানের প্রকৃতি, ইত্যাদির মতো বৃহত্তর মাপকাঠিগুলিকে বিবেচনা করে গণনা করা হয় এবং সুতরাং, সুদের হার ভিন্ন ঋণগ্রহণকারীদের ক্ষেত্রে ভিন্ন হতে পারে।</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>বাতিলকরণ অথবা পরিসমাপ্তি: উপলভ্যতা সময়কালে, ঋণদাতা হয়তো, তার একক বিবেচনায়, কোনও ডিফল্টের ঘটনা অথবা সম্ভাব্য ডিফল্টের ঘটনা ঘটলে অথবা ঋণদাতার পক্ষে ঋণগ্রহণকারীকে সুবিধাটি প্রদান করা বা অব্যাহত রাখা বেআইনি হয়ে পড়লে, সুবিধাগুলি বাতিল করতে পারে। উপরন্তু ঋণগ্রহণকারী নিঃশর্তভাবে সম্মত হন, দায়ভার নেন ও স্বীকার করেন যে ঋণদাতার কাছে সুবিধার কোনও অ-ব্যবহৃত অংশ, আংশিক বা সম্পূর্ণরূপে যাই হোক না কেন, সুবিধা/ ঋণ প্রচনকালেযেকোনো সময় সেটি বাতিল করার নিঃশর্ত অধিকার আছে এবং এই ধরনের বাতিলকরণ ঋণগ্রহণকারীকে আগাম না জানিয়ে করা হতে পারে।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>প্রযোজ্য আইনের অধীনে জারি করা, এবং সময়ে সময়ে সংশোধি হতে পারা অনুসারে, সমস্ত পরোক্ষ কর, শুল্ক ও আরোপিত শুল্কগুলি যার মধ্যে অন্তর্ভুক্ত কিন্তু এগুলিতেই সীমিত নয় জিএসটি, শিক্ষা উপকর ঋণগ্রহণকারীকে অতিরিক্তভাবে বহন করতে হবে।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>অনুগ্রহ করে লক্ষ্য করবেন যে আয়কর, জিএসটি বা প্রযোজ্য কর সংক্রান্ত যেকোনো আইনগুলির ফলে কিস্তিতে উপযুক্ত সংশোধন করা হবে।  অন্যান্য সমস্ত নিয়ম ও শর্তাবলী ঋণদাতার সাথে সম্পন্ন করা ঋণ চুক্তি(গুলি) অনুযায়ী হবে।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>সুবিধাগুলি, যদি না অন্যথায় স্পষ্টভাবে বিবৃত করা থাকে, সেক্ষেত্রে দাবি অনুসারে পরিশোধযোগ্য / নির্ধারণযোগ্য এবং তা ঋণদাতার যেকোনো সময় পর্যালোচনা সাপেক্ষ। ঋণদাতা হয়তো, তার একক বিবেচনায় উপরিউক্ত সময়কাল অতিক্রম করে অধিকতর একটি সময়কালের জন্য সুবিধাটি অব্যাহত রাখা / পুনর্নবীকরণ করতে বেছে নিতে পারে।</td></tr>");
                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 8th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>ঋণগ্রহণকারীকে, ঋণদাতা সময়কাল বাড়িয়ে না থাকলে, 30 দিনের মধ্যে এই অনুমোদন চিঠিটি গ্রহণ করতে হবে এবং একটি স্বাক্ষরিত কপি হস্তান্তর করতে হবে।</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>অনুগ্রহ করে লক্ষ্য করবেন যে অনুমোদনের নিয়ম ও শর্তাবলী এই অনুমোদন চিঠির তারিখের থেকে 30 দিনের একটি সময়কালের জন্য বৈধ এবং ঋণদাতার কাছে বৈধ্যতার সময়কালের মধ্যে নথিপত্র দাখিল করা ও তহবিল অ্যাক্সেস করা সম্পূর্ণ না হলে সুদের হার এবং অনুমোদনের অন্য কোনও নিয়ম ও শর্তাবলী সংশোধন করার, অথবা তার একক বিবেচনায়, অনুমোদনটি প্রত্যাহার করার অধিকার সংরক্ষণ করে।</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>ঋণের উপরিউক্ত অনুমোদনটি নিম্নলিখিত শর্তাবলী সাপেক্ষ হবে:</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;'>");
                        sb.Append("<ul>");
                        sb.Append("<li>ঋণটি অবশ্যই আবেদন ফর্ম ও তহবিলের ঘোষণার অন্তিম ব্যবহারে ঋণগ্রহণকারীর ইঙ্গিত করা উদ্দেশ্যের জন্যই ব্যবহার করতে হবে এবং অন্য কোনো উদ্দেশ্যে ব্যবহার করা যাবে না।</li>");
                        sb.Append("<li>ঋণদাতার নীতি ও ফরম্যাট অনুযায়ী ঋণ চুক্তি এবং ঋণদাতার সাথে অন্যান্য নথিপত্র সম্পন্ন করা।</li>");
                        sb.Append("</ul>");
                        sb.Append("</td></tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>আমরা আপনার সাথে একটি দীর্ঘস্থায়ী সম্পর্কের জন্য সাগ্রহে প্রত্যাশা করি।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>ধন্যবাদান্তে</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>বিশ্বস্তভাবে</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>ইউনিটি স্মল ফাইন্যান্স ব্যাংক-এর পক্ষে</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        #endregion
                        // END Sanction Letter

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // START- KFS-1
                        #region 9th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;'>মূল তথ্য বিবৃতি (KFS)</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>তারিখ: {0}</td>", Convert.ToString(dtSancLetter.Rows[0]["FinalSanctionDate"]));
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='width:100%;' >নিয়ন্ত্রিত সত্তা/ঋণদাতার নাম:ইউনিটি স্মল ফাইন্যান্স ব্যাংক লিমিটেড</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>ঋণগ্রহীতার নাম ও ঠিকানা: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["MemberName"]));
                        sb.Append("</tr>");
                        sb.Append("<tr><td>&nbsp;</td></tr>");
                        sb.Append("<tr><td style='font-weight:bold;text-align:center;width:100%;'>বিভাগ 1 (সুদের হার এবং ফি/চার্জ)</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td style='padding:2px; width:5%;text-align:center;'>1</td>");
                        sb.Append("<td style='padding:2px; width:22%;'>ঋণ প্রস্তাব/অ্যাকাউন্ট নম্বর</td>");
                        sb.AppendFormat("<td style='padding:2px; width:25%;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanNo"]));
                        sb.Append("<td style='padding:2px; width:22%;'>ঋণের ধরন</td>");
                        sb.AppendFormat("<td colspan='4' style='padding:2px; width:26%;'>{0}</td>", "MEL Loan");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'>2</td>");
                        sb.Append("<td colspan='2' style='padding:2px;'>অনুমোদিত ঋণের পরিমাণ (টাকায় লিখুন)</td>");
                        sb.AppendFormat("<td colspan='5' style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanAmt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'>3</td>");
                        sb.Append("<td colspan='3'>পরিশোধের সময়সূচী<br>(i) পর্যায়ক্রমে বা 100% অগ্রিম পরিশোধ<br>(ii) যদি এটি পর্যায় অনুসারে হয়, তাহলে প্রাসঙ্গিক বিবরণ সহ ঋণ চুক্তির ধারা উল্লেখ করুন।</td>");
                        sb.AppendFormat("<td colspan='4' style='padding:2px;text-align:center;'>{0}</td>", "100% Upfront");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'>4</td>");
                        sb.Append("<td colspan='2' >4 ঋণের মেয়াদ (বছর/মাস/দিন)</td>");
                        sb.AppendFormat("<td colspan='5' style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["Tenure"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'>5</td>");
                        sb.Append("<td colspan='7' style='text-align:center;' >কিস্তির বিবরণ</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='text-align:center;' >কিস্তির প্রকার</td>");
                        sb.Append("<td style='text-align:center;' >সমান মাসিক কিস্তির সংখ্যা</td>");
                        sb.Append("<td style='text-align:center;' >সমান মাসিক কিস্তি (₹)</td>");
                        sb.Append("<td colspan='3' style='text-align:center;' >অনুমোদনের পর, পরিশোধ শুরু</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='3' style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["RepFreq"]));
                        sb.AppendFormat("<td  style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInstNo"]));
                        sb.AppendFormat("<td  style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["EMIAmt"]));
                        sb.AppendFormat("<td colspan='3' style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["FInstDt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'>6</td>");
                        sb.Append("<td colspan='3' style='text-align:center;' >সুদের হার (%) এবং প্রকার (ফিক্সড বা ফ্লোটিং বা হাইব্রিড)</td>");
                        sb.AppendFormat("<td colspan='4' style='padding:2px;text-align:center;'>{0}(Fixed)</td>", Convert.ToString(Math.Round(Convert.ToDouble(dtScheduleOwn.Rows[0]["IntRate"]), 2)));
                        sb.Append("</tr>");  

                        sb.Append("<tr><td style='padding:2px;text-align:center;'>7</td>");
                        sb.Append("<td colspan='7' style='text-align:center;' >সুদের পরিবর্তনশীল হারের ক্ষেত্রে অতিরিক্ত তথ্য</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;' >বেঞ্চমার্ক</td>");
                        sb.Append("<td style='text-align:center;' >বেঞ্চমার্ক হার (%) (B)</td>");
                        sb.Append("<td style='text-align:center;' >পারসেটেনেজের স্প্রেড রেট (%) (S)</td>");
                        sb.Append("<td style='text-align:center;' >চূড়ান্ত হারের শতাংশ (%)(B)+(S)</td>");
                        sb.Append("<td colspan='2' style='text-align:center;' >পর্যাবৃত্তি পুনরায় সেট করুন (মাস)</td>");
                        sb.Append("<td colspan='2' style='text-align:center;' >রেফারেন্স বেঞ্চমার্ক পরিবর্তনের প্রভাব ('R'-এ 25 bps এর পরিবর্তনের জন্য, পরিবর্তন:)</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;' ><br><br><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' ><br><br><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' ><br><br><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' ><br><br><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' >B <br><br><br><br> NA</td>");
                        sb.Append("<td style='text-align:center;' >S <br><br><br><br> NA</td>");
                        sb.Append("<td style='text-align:center;' >সমান মাসিক কিস্তি (₹) <br> NA</td>");
                        sb.Append("<td style='text-align:center;' >সমান মাসিক কিস্তি (₹) <br> NA</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'>8</td>");
                        sb.Append("<td colspan='7' style='text-align:center;' >ফি/চার্জ</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");


                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 10th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");



                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='padding:2px; width:25%;'></td>");
                        sb.Append("<td colspan='2' style='padding:2px; width:35%;'>RE কে প্রদেয় (A)</td>");
                        sb.Append("<td colspan='2' style='padding:2px; width:40%;'>RE এর মাধ্যমে তৃতীয় পক্ষের কাছে প্রদেয়</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'></td>");
                        sb.Append("<td style='padding:2px;text-align:center;'></td>");
                        sb.Append("<td style='padding:2px;text-align:center;'>এক-সময়/পুনরাবৃত্ত</td>");
                        sb.Append("<td style='padding:2px;text-align:center;'>প্রযোজ্য হিসাবে পরিমাণ (₹ টাকায়) বা শতাংশ (%)</td>");
                        sb.Append("<td style='padding:2px;text-align:center;'>এক-সময়/পুনরাবৃত্ত</td>");
                        sb.Append("<td style='padding:2px;text-align:center;'>প্রযোজ্য হিসাবে পরিমাণ (₹ টাকায়) বা শতাংশ (%)</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(i)</td>");
                        sb.Append("<td style='padding:2px;text-align:left;'>প্রসেসিং ফি</td>");
                        sb.Append("<td style='padding:2px;text-align:center;'><br><br>One Time</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>(অন্তর্ভুক্তিমূলক জিএসটি)<br><br>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["procfee"]));
                        sb.Append("<td style='padding:2px;text-align:center;'><br><br>One Time</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>(অন্তর্ভুক্তিমূলক জিএসটি)<br><br>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["procfeeBC"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(ii)</td>");
                        sb.Append("<td style='padding:2px;text-align:left;'>ইনস্যুরেন্স চার্জ</td>");
                        sb.Append("<td style='padding:2px;text-align:center;'><br><br>One Time</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;' >(অন্তর্ভুক্তিমূলক জিএসটি)<br><br>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInsurance"]));
                        sb.Append("<td style='padding:2px;text-align:center;'><br><br>One Time</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>(অন্তর্ভুক্তিমূলক জিএসটি)<br><br>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInsurance"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(iii)</td>");
                        sb.Append("<td  style='padding:2px;text-align:left;'>মূল্যায়ন চার্জ</td>");
                        sb.Append("<td colspan='4' style='padding:2px;text-align:center;'>No Fees</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(iv)</td>");
                        sb.Append("<td style='padding:2px;text-align:left;'>অন্য কোন (দয়া করে উল্লেখ করুন)</td>");
                        sb.Append("<td style='padding:2px;text-align:center;'></td>");
                        sb.Append("<td style='padding:2px;' ></td>");
                        sb.Append("<td style='padding:2px;text-align:center;'></td>");
                        sb.Append("<td style='padding:2px;'></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'></td>");
                        sb.Append("<td style='padding:2px;text-align:left;'>মোট চার্জ</td>");
                        sb.Append("<td style='padding:2px;text-align:center;'>One Time</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;' >{0}</td>", Convert.ToString(vTotalPayRE));
                        sb.Append("<td style='padding:2px;text-align:center;'>One Time</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(vTotalPayTP));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>9</td>");
                        sb.Append("<td colspan='2' style='padding:2px;text-align:center;'>বার্ষিক শতকরা হার (APR) (%)</td>");
                        sb.AppendFormat("<td colspan='3' style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["IRRIC"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>10</td>");
                        sb.Append("<td colspan='5' style='padding:2px;text-align:center;'>কন্টিনজেন্ট চার্জের বিবরণ (প্রযোজ্য হিসাবে, ₹ বা %) **</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(i)</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>পেনাল চার্জ, যদি থাকে, বিলম্বিত অর্থ প্রদানের ক্ষেত্রে</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "3% + 18% GST");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(ii)</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>অন্যান্য পেনাল চার্জ, যদি থাকে</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>a</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>EMI বাউন্স চার্জ (অন্তর্ভুক্তিমূলক জিএসটি)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "Rs. 590 (Incl. GST)");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>b</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>এক্সটেনশন চার্জ (অন্তর্ভুক্তিমূলক জিএসটি)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>c</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>ভিজিট চার্জ  (অন্তর্ভুক্তিমূলক জিএসটি)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "Rs. 236 (Incl. GST)");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(iii)</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>ফোরক্লোজার চার্জ, যদি প্রযোজ্য হয়** (অন্তর্ভুক্তিমূলক জিএসটি)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "3% of OS (Allowed after 1st EMI paid) + GST");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(iv)</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>ফ্লোটিং থেকে ফিক্সড রেটে এবং তার বিপরীতে ঋণ পরিবর্তনের জন্য চার্জ</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;' >{0}</td>", "NA");
                        sb.Append("</tr>");


                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(v)</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>অন্য কোন চার্জ (অনুগ্রহ করে উল্লেখ করুন)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>a</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>প্রি-পেমেন্ট চার্জ**</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>b</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>ঋণ পূর্ব সমাপ্তি ফি**</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "মোট বকেয়া মূলধনে ৩%");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        sb.Append("<p>*চার্জের উপর GST প্রযোজ্য হব</p>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 11th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td style='font-weight:bold;text-align:center;width:100%;'>বিভাগ 2 (অন্যান্য গুণগত তথ্য)</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px; width:5%;'>1</td>");
                        sb.Append("<td colspan='2' style='padding:2px; width:30%;'>রিকভারি এজেন্টদের নিয়োগ সম্পর্কিত ঋণ চুক্তির ধারা</td>");
                        sb.Append("<td colspan='3' style='padding:2px; width:65%;'>যদি নির্ধারিত তারিখের মধ্যে ঋণদাতার অ্যাকাউন্টে কিস্তি জমা না হয়, তাহলে ধরা হবে যে ঋণগ্রহীতা শোধ করত ব্যর্থ এবং ডিফল্টের ফলস্বরূপ ধারাগুলি প্রযোজ্য হবে।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>2</td>");
                        sb.Append("<td colspan='2' style='padding:2px;'>ঋণ চুক্তির ধারা যা অভিযোগ নিষ্পত্তি পদ্ধতির বিস্তারিতভাবে বর্ণনা করে</td>");
                        sb.Append("<td colspan='3' style='padding:2px;'>এই চুক্তির বৈধতা, ব্যাখ্যা বা কার্যকারিতা সম্পর্কিত সমস্ত প্রশ্ন ভারতীয় আইন দ্বারা নিয়ন্ত্রিত হবে এবং মহারাষ্ট্র এবং মুম্বাইয়ের আদালতের এখতিয়ারের অধীন।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>3</td>");
                        sb.Append("<td colspan='2' style='padding:2px;'>নোডাল অভিযোগ নিষ্পত্তি অফিসারের ফোন নম্বর এবং ইমেল আইডি</td>");
                        sb.Append("<td colspan='3' style='padding:2px;'>id-care@unitybank.co.in টোল ফ্রি নম্বর -18002091122</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>4</td>");
                        sb.Append("<td colspan='2' style='padding:2px;'>ঋণটি বর্তমান অথবা ভবিষ্যতে অন্য কোনও RE-এর বা সিকিউরিটাইজেশনের অধীনে যেতে পারে কিনা (হ্যাঁ/না)</td>");
                        sb.Append("<td colspan='3' style='padding:2px;'>হ্যাঁ, ঋণগ্রহীতার সাপেক্ষে</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>5</td>");
                        sb.Append("<td colspan='2' style='padding:2px;'>অগ্রিম অর্থ প্রদানের ক্ষেত্রে ঋণের মেয়াদ হ্রাস সম্পর্কিত ধারা।</td>");
                        sb.Append("<td colspan='3' style='padding:2px;'>যদি আবেদনকারী মাসিক বা সাপ্তাহিক ঋণের ইএমআই-এর জন্য অগ্রিম অর্থ প্রদান করেন, তবে এই অর্থ প্রদান মূল বকেয়ার বিপরীতে সমন্বয় করা হবে, যার ফলে ঋণের মেয়াদ আনুপাতিক হারে হ্রাস পাবে।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>6</td>");
                        sb.Append("<td colspan='5' style='padding:2px;'>সহযোগিতামূলক ঋণের ব্যবস্থার অধীনে ঋণ দেওয়ার ক্ষেত্রে (উদা.,সহ-ঋণ/আউটসোর্সিং), নিম্নলিখিত অতিরিক্ত বিবরণ দেওয়া যেতে পারে:</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>মূল RE-এর নাম, তার অর্থায়নের অনুপাত সহ</td>");
                        sb.Append("<td colspan='1' style='padding:2px;text-align:center;'>অংশীদার RE এর নাম এবং এর অর্থায়ন অনুপাত</td>");
                        sb.Append("<td colspan='2' style='padding:2px;text-align:center;'>সুদের মিশ্র হার</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='6' style='padding:2px;text-align:center;'>তিনটি অনুচ্ছেদের জন্য NA (পয়েন্ট 5)</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>7</td>");
                        sb.Append("<td colspan='5' style='padding:2px;'>ডিজিটাল ঋণের ক্ষেত্রে, নিম্নলিখিত নির্দিষ্ট ডিসক্লোজার দেওয়া যেতে পারে:</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='4' style='padding:2px;'>(i) RE-এর বোর্ড অনুমোদিত নীতি অনুসারে কুলিং অফ/লুক-আপ সময়কাল, এই সময়ের মধ্যে ঋণের অগ্রিম পরিশোধের জন্য ঋণগ্রহীতার কাছ থেকে কোনো জরিমানা নেওয়া হবে না</td>");
                        sb.Append("<td colspan='2' style='padding:2px;text-align:center;'>NA</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='4' style='padding:2px;'>(ii) রিকভারি এজেন্ট হিসাবে কাজ করা এবং ঋণগ্রহীতার সাথে যোগাযোগ করার জন্য অনুমোদিত LSP-এর বিবরণ|</td>");
                        sb.Append("<td colspan='2' style='padding:2px;text-align:center;'>NA</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");


                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 12th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td style='font-weight:bold;text-align:left;width:100%;'>(পণ্যের নাম - সেগমেন্টের নাম-খুচরা এবং MSME ঋণ) এর জন্য APR গণনার চিত্র</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px; width:10%;'>সিরিয়াল নম্বর</td>");
                        sb.Append("<td  style='padding:2px; width:60%;'>প্যারামিটার</td>");
                        sb.Append("<td  style='padding:2px; width:30%;'>বিবরণ</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>1</td>");
                        sb.Append("<td  style='padding:2px;'>অনুমোদিত ঋণের পরিমাণ (টাকা) (KFS টেমপ্লেটের সিরিয়াল নম্বর 2 – ভাগ1)</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanAmt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>2</td>");
                        sb.Append("<td  style='padding:2px;'>ঋণের মেয়াদ (বছর/মাস/দিন) (KFS টেমপ্লেটের সিরিয়াল নম্বর 4 – ভাগ1)</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["Tenure"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>a)</td>");
                        sb.Append("<td  style='padding:2px;'>অসমান পর্যায়ক্রমিক ঋণের ক্ষেত্রে, মূল অর্থ প্রদানের জন্য কিস্তির সংখ্যা</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td rowspan='3' style='padding:2px;'>b)</td>");
                        sb.Append("<td  style='padding:2px;'>EMI-এর ধরন</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["RepFreq"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>প্রতিটি EMI-এর পরিমাণ (টাকা)</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["EMIAmt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>EMI-এর সংখ্যা (অর্থাৎ, মাসিক কিস্তির ক্ষেত্রে EMI-এর সংখ্যা) (KFS টেমপ্লেটের সিরিয়াল নম্বর 5 – ভাগ1)</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInstNo"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>c)</td>");
                        sb.Append("<td  style='padding:2px;'>মূলধনের সুদ প্রদানের জন্য কিস্তির সংখ্যা, যদি থাকে</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>d)</td>");
                        sb.Append("<td  style='padding:2px;'>অনুমোদনের পর, পরিশোধ শুরু (KFS টেমপ্লেটের সিরিয়াল নম্বর 5 – ভাগ1)</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["FInstDt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>3</td>");
                        sb.Append("<td  style='padding:2px;'>সুদের হারের প্রকার ((ফিক্সড বা ফ্লোটিং বা হাইব্রিড) (KFS টেমপ্লেটের সিরিয়াল নম্বর 6 – ভাগ1)</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", "Fixed");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>4</td>");
                        sb.Append("<td  style='padding:2px;'>সুদের হার (KFS টেমপ্লেটের সিরিয়াল নম্বর 6 – ভাগ1)</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(Math.Round(Convert.ToDouble(dtScheduleOwn.Rows[0]["IntRate"]), 2)));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>5</td>");
                        sb.Append("<td  style='padding:2px;'>অনুমোদনের তারিখে বিদ্যমান হার অনুসারে ঋণের পুরো মেয়াদে মোট সুদের পরিমাণ (টাকা) চার্জ করা হবে</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["IntAmt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>6</td>");
                        sb.Append("<td  style='padding:2px;'>ফি/চার্জ প্রদেয় (টাকা)</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(vTotalPayRE));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>A</td>");
                        sb.Append("<td  style='padding:2px;'>RE কে প্রদেয় (KFS টেমপ্লেটের সিরিয়াল নম্বর 8A – ভাগ1)</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(vTotalPayRE));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>B</td>");
                        sb.Append("<td  style='padding:2px;'>RE-এর মাধ্যমে তৃতীয় পক্ষকে প্রদেয় (KFS টেমপ্লেটের সিরিয়াল নম্বর 8B – ভাগ1)</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(vTotalPayTP));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>7</td>");
                        sb.Append("<td  style='padding:2px;'>মোট পরিশোধ করা পরিমাণ (1-6) (টাকা)</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["NetDisbAmt"]));
                        sb.Append("</tr>");

                        sb.Append("</table>");


                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 13th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        //sb.Append("<tr><td style='font-weight:bold;text-align:left;width:100%;'>(उत्पाद का नाम – संवर्ग का नाम – खुदरा और एमएसएमई ऋण) के लिए एपीआर की गणना हेतु चित्रण  </td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;width:10%;'>8</td>");
                        sb.Append("<td  style='padding:2px;width:60%;'>ঋণগ্রহীতাকে মোট অর্থ প্রদান করতে হবে (1 এবং 5 এর যোগফল) (টাকা)</td>");
                        sb.AppendFormat("<td style='padding:2px;width:30%;text-align:center;'>{0}</td>", Convert.ToString(vTotalPI));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>9</td>");
                        sb.Append("<td  style='padding:2px;'>বার্ষিক শতাংশ হার- কার্যকর বার্ষিক সুদের হার (শতাংশে) (KFS টেমপ্লেটের সিরিয়াল নম্বর 9 – ভাগ1)</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["IRRIC"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>10</td>");
                        sb.Append("<td  style='padding:2px;'>শর্তাবলী অনুযায়ী বিতরণের সময়সূচী</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", "EMI Schedule");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>11</td>");
                        sb.Append("<td  style='padding:2px;'>কিস্তি এবং সুদ পরিশোধের শেষ তারিখ</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["FInstDt"]));
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:2px;'>দস্তাবেজটি 5 দিনের জন্য গ্রহণের জন্য বৈধ থাকবে, এরপরে বিতরণ প্রক্রিয়া শুরু হবে। চার্জের উপর জিএসটি প্রয়োগ করা হবে।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:2px;'>&nbsp;</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:2px;'>ঋণগ্রহীতা এবং সহ-ঋণগ্রহীতা (গুলি) দ্বারা সম্পাদিত এবং বিতরণ করা হয়েছে এবং তাদের পড়ার (এবং/অথবা তাদের কাছে ব্যাখ্যা করার পরে) একটি চিহ্ন হিসাবে, যাচাই করা, বোঝা, অপরিবর্তনীয় ভাবেস ম্মত হওয়া, গ্রহণ করা, নিশ্চিত করা এবং অনুমোদন পত্র, মূল ফ্যাক্টশীট, সাধারণ শর্তাদি ও শর্তাবলীনথি এবং তফসিলের সমস্ত বিষয় বস্তুসহ তফসিলের সমস্ত ধারা ঘোষণা করা,  এবং সমস্ত নথির প্রাপ্তি স্বীকার করে, যার ফলে এর যথার্থতা এবং সঠিকতা প্রমাণিত হয়।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:2px;'>&nbsp;</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:2px;'>ঋণগ্রহীতা(গণ) স্বীকারকরেযে, বর্তমানে কার্যকর তফসিল, শর্তাবলী এবং চার্জ সংশোধন করা যেতে পারে এবং/অথবা নতুনশর্তাবলী এবং চার্জ ব্যাংকের একক এবং পরমবিবেচনার ভিত্তিতে যেকোনও সময় এবং সময়ে চালু করা যেতে পারে।এইধরনের পরিবর্তনগুলি ঋণগ্রহীতা(দের) উপর বাধ্যতা মূলক হবে।এই সমস্ত পরিবর্তন বাসংযোজন যথাযথভাবে জানানো হবে এবং একটি সম্ভাব্যভিত্তিতে কার্যকর হবে।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:2px;'>&nbsp;</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:2px;'>**ফোরক্লোজার চার্জ:ব্যক্তিগত ঋণের ফোরক্লোজার চার্জ নির্ধারিত মেয়াদশেষ হওয়ার আগে পুরো বকেয়া ঋণের পরিমাণের তাড়াতাড়ি পরিশোধকে বোঝায়।ঋণগ্রহীতারা অবশিষ্ট আসল এবং অর্জিত সুদ পরিশোধ করে ঋণ তাড়াতাড়ি নিষ্পত্তি করতে বেছে নিতে পারেন।ঋণদাতারা এরজন্য একটি ফোরক্লোজার ফিচার্জ করে।** প্রি-পেমেন্ট চার্জ:প্রি-পেমেন্ট চার্জ বলতে সেই ফি বা চার্জকে বোঝায়যা ঋণগ্রহীতা সম্মত সময়সূচীর আগে ঋণের অংশ পরিশোধ করার জন্য ব্যয় করতে পারে।ঋণদাতারা এরজন্য একটি প্রিপেমেন্ট ফি চার্জ করে।</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");


                        #endregion
                        // END - KFS-1

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // EmiSchedule
                        #region 14th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr><td colspan='2' style='padding-top:20px;text-decoration: underline;'><strong>ঋণ পরিশোধের সময়সূচী</strong></td></tr>");
                        sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table  class='xsmall_font' style='border-collapse: collapse; width: 100%;  text-align: left;' border='1' >");
                        sb.Append("<tr>");
                        sb.Append("<th style='padding: 4px;'>ক্রমিক সংখ্যা<br>Sr.No.</th>");
                        sb.Append("<th style='padding: 4px;'>নির্ধারিত তারিখ<br>Due Date</th>");
                        sb.Append("<th style='padding: 4px;'>মূল বকেয়া<br>Principle Due</th>");
                        sb.Append("<th style='padding: 4px;'>সুদের বকেয়া<br>Interest Due</th>");
                        sb.Append("<th style='padding: 4px;'>মাসিক কিস্তির পরিমাণ<br>EMI Amount</th>");
                        sb.Append("<th style='padding: 4px;'>মূল বকেয়া পরিমাণ<br>Principle Outstanding</th>");
                        sb.Append("<th style='padding: 4px;'>সুদের বকেয়া পরিমাণ<br>Interest Outstanding</th>");
                        sb.Append("<th style='padding: 4px;'>মোট বকেয়া পরিমাণ<br>Total Outstanding</th>");
                        sb.Append("</tr>");

                        // Fill rows from DataTable
                        foreach (DataRow row in dtEMISchedule.Rows)
                        {
                            sb.Append("<tr>");
                            sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["InstNo"]);
                            sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["DueDt"]);
                            sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["PrinceAmt"]);
                            sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["InstAmt"]);
                            sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["ResAmt"]);
                            sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["Outstanding"]);
                            sb.AppendFormat("<td style='padding:8px;'>{0}</td>", "");
                            sb.AppendFormat("<td style='padding:8px;'>{0}</td>", "");
                            sb.Append("</tr>");
                        }
                        // End table
                        sb.Append("</table>");
                        //sb.Append("</td></tr>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // Start Loan Agreement
                        #region 15th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='1' class='large_font' style='text-align:center; font-weight:bold; padding:10px;'>ঋণ চুক্তি</td>");
                        sb.Append("</tr>");

                        // Agreement Start
                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding-top:10px;'>এই চুক্তিটি <span style='border-bottom: 1px dotted black;'>{0}</span> সালের এই <span style='border-bottom: 1px dotted black;'>{1}</span> দিন <span style='border-bottom: 1px dotted black;'>{2}</span> মাসে নিম্নোক্তদের মধ্যে করা হয়েছে|ইউনিটি স্মল ফাইন্যান্স ব্যাংক, কোম্পানি আইন 2013 এর অধীনে নিবন্ধিত একটি সংস্থা, ইউনিটি স্মল ফিনান্স ব্যাংক লিমিটেড, 5 মও 6 ষ্ঠতলা, টাওয়ার - 1 এল অ্যান্ড টি সিউডস টাওয়ার, প্লটনং-এ নিবন্ধিত অফিস রয়েছে। - আর - 1, সেক্টর - 40, সিউডস রেলওয়ে স্টেশন, নভিমুম্বাই - 400706-এ অবস্থিত, যার প্রতিনিধিত্ব করেছেন বিদ্যাবিহার, মুম্বইয়ে অবস্থিত কোম্পানির শাখা ব্যবস্থাপক, যাকে এরপর থেকে “ঋণদাতা/কোম্পানি” (যে অভিব্যক্তিটি বোঝাবে, যদি না অন্যথায় প্রসঙ্গ অনুযায়ী অন্যকিছু আবশ্যক হয়, সেটি প্রথম অংশের আগ্রহ ও নিযুক্তিগুলিতে এর উত্তরসূরীদের বোঝাবে এবং অন্তর্ভুক্ত করবে) বলে উল্লেখ করা হবে|</td></tr>", Convert.ToString(dtLoanAgr.Rows[0]["AgreeYear"]), Convert.ToString(dtLoanAgr.Rows[0]["AgreeDay"]), Convert.ToString(dtLoanAgr.Rows[0]["AgreeMonth"]));
                        
                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding-top:10px;'>এবং আমার, শ্রী/শ্রীমতি, <span style='border-bottom: 1px dotted black;'>{0}</span> ,<span style='border-bottom: 1px dotted black;'>{1}</span> প্রযত্নে স্ত্রী, যিনি স্থায়ীভাবে <span style='border-bottom: 1px dotted black;'>{2}</span> এ বাস করছেন|এবং, এরপর থেকে “” হিসেবে উল্লেখ করা হবে (যে অভিব্যক্তিটি বোঝাবে , যদি না অন্যথায় প্রসঙ্গ অনুযায়ী অন্যকিছু আবশ্যক হয়, সেটি দ্বিতীয় অংশের তার উত্তরাধিকারী, সম্পন্নকারী ও আইনি প্রতিনিধিদের অন্তর্ভুক্ত করবে)</td></tr>", Convert.ToString(dtLoanAgr.Rows[0]["Borrower"]), Convert.ToString(dtLoanAgr.Rows[0]["CoBorrower"]), Convert.ToString(dtLoanAgr.Rows[0]["BorrowerAddr"]));
                        

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding-top:10px;'>2. শ্রী/শ্রীমতি <span style='border-bottom: 1px dotted black;'>{0}</span> এবং, এরপর থেকে “” হিসেবে উল্লেখ করা হবে (যে অভিব্যক্তিটি বোঝাবে , যদি না অন্যথায় প্রসঙ্গ অনুযায়ী অন্যকিছু আবশ্যক হয়, সেটি তৃতীয় অংশের তার উত্তরাধিকারী, সম্পন্নকারী ও আইনি প্রতিনিধিদের অন্তর্ভুক্ত করবে)</td></tr>", Convert.ToString(dtLoanAgr.Rows[0]["Borrower"]));
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding-top:10px;'>যেহেতু ঋণগ্রহণকারী ব্যবসা সম্প্রসারণের উদ্দেশ্যের জন্য দুই লাখ (শব্দে) টাকার মেল সরল ব্যাপার-এর (এরপর থেকে “ঋণ” হিসেবে উল্লেখ করা হবে) জন্য আবেদন করেছেন এবং কোম্পানি ঋণগ্রহণকারীর যথোচিতভাবে মূল্যায়ন করার পরে অনুমোদিত <span style='border-bottom: 1px dotted black;'>{0}</span> তারিখ অনুসারে <span style='border-bottom: 1px dotted black;'>{1}</span> টাকা পর্যন্ত ঋণের অনুমোদন করেছেন, তবে তা নিয়ম ও শর্তাবলী অনুসরণ করা সাপেক্ষ।</td></tr>", Convert.ToString(dtLoanAgr.Rows[0]["RepayStartDate"]), Convert.ToString(dtLoanAgr.Rows[0]["AppAmount"]));
                       

                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("সুতরাং এখন, ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্কের সুবিধা নথিতে উল্লেখ করা সুবিধা মঞ্জুর/সম্মতি করে থাকা অনুসারে, ঋণগ্রহণকারী এবং যৌথ ঋণগ্রহণকারী এতদ্বারা নিম্নোক্ত নিয়ম ও শর্তাবলীতে সম্মত হন, চুক্তি করেন, নিশ্চিত করেন এবং লিপিবদ্ধ করেন:");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // Section: नियम और शर्तें
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px; font-weight:bold;'>নিয়ম ও শর্তাবলী</td>");
                        sb.Append("</tr>");

                        // a) राशि
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>a)</b>রাশি: <br> ");
                        sb.AppendFormat("ঋণদাতা টার্ম ঋণের আকারে ঋণগ্রহণকারীকে <span style='border-bottom: 1px dotted black;'>{0}</span> টাকার একটি সমষ্টি ধার দেবেন, যা ঋণদাতা এমনভাবে বিতরণ করবেন, যেমনটি ঋণদাতাদের কাছে জমা দেওয়া ঋণের আবেদনে উল্লেখ অনুযায়ী শুধুমাত্র সেই উদ্দেশ্যের জন্যই ব্যবহার করার উপযুক্ত অনুসারে বিবেচ্য হিসেবে দেখা হবে।</td></tr>", Convert.ToString(dtLoanAgr.Rows[0]["AppAmount"]));

                        // b) ब्याज
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>b)</b> সুদ: <br>");
                        sb.Append("ঋণটি ক্রমশ হ্রাসমান ব্যালেন্সের ভিত্তিতে করা হিসাব অনুযায়ী প্রতি বছরে% হারে সুদ বহন করবে। তহবিলের খরচ, ঝুঁকি প্রিমিয়াম ও মার্জিনের ভিত্তিতে হিসাব করে সুদের হার পাওয়া যায়। কিস্তি(গুলি) দেওয়ায় যেকোনো বিলম্বের ফলে, এই ধরনের ডিফল্ট সাপেক্ষে ঋণদাতার অন্যান্য অধিকারগুলিকে ক্ষতিগ্রস্ত না করে, ঋণের বকেয়া ব্যালেন্সের উপর বিদ্যমান হারের বেশি হারে প্রতি মাসে (3%) অতিরিক্ত সুদ ধার্য করা হবে।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // c) दंडात्मक प्रभार
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>c)</b> “দণ্ডনীয় চার্জ” বলতে ঋণগ্রহণকারীর অনুসূচি I-এ নির্দিষ্টভাবে উল্লেখিত গুরুত্বপূর্ণ শর্ত ও নিয়মাবলী লঙ্ঘন/অমান্য করার কারণে প্রদানযোগ্য দণ্ডমূলক চার্জ বোঝানো হয়েছে, এবং/অথবা যা ব্যাঙ্ক সময় সময়ে ঋণগ্রহণকারীকে জানাবে বা ব্যাঙ্কের ওয়েবসাইটে আপডেট করবে।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // d) महत्वपूर्ण नियम और शर्तें
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>d)</b> “ বস্তুগত নিয়ম ও শর্তাবলী” বলতে বোঝাবে এই প্রমাণ নিয়মের বস্তুগত নিয়ম ও শর্তাবলী, যেমনটি এর অধীনস্থ অনুসূচীতে আরও নির্দিষ্টভাবে বিবৃত করা আছে। অগ্রবর্তীর অধিকাংশের কোনও ক্ষতি না করে ব্যাঙ্কের কাছে চার্জ করার অধিকারও থাকবে, এবং ঋণগ্রহণকারী বস্তুগত নিয়ম ও শর্তাবলীর যেকোনো লঙ্ঘন / অননুবর্তিতার জন্য এবং যে সময়কাল ধরে অনিয়ম বা লঙ্ঘন চলেছে সেটির জন্য অথবা ব্যাঙ্কের প্রয়োজনীয় হিসেবে মনে হওয়া কোনও সময়ের জন্য এই ধরনের দণ্ডনীয় চার্জগুলি প্রদান করতে দায়বদ্ধ। তবে চার্জিং এবং দণ্ডনীয় চার্জের আদায় হয় এর অধীনে নয়তো অন্যথায় বা আইন অনুযায়ী ঋণগ্রহণকারীর বিরুদ্ধে এই ধরনের অনিয়ম বা লঙ্ঘনের জন্য হওয়া কার্যধারার অন্যান্য অধিকার বা প্রতিকারগুলিকে ক্ষতিগ্রস্ত না করা সাপেক্ষ হবে।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // End Table
                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 16th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%;border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.AppendFormat("<b>e)</b> প্রক্রিয়াকরণ ফি: <br>");
                        sb.AppendFormat("ঋণগ্রহণকারী ঋণদাতার ঋণটি প্রদান করার সময় অগ্রিম হিসেবে ( <span style='border-bottom: 1px dotted black;'>{0}</span> ) টাকার একটি এককালীন প্রক্রিয়াকরণ ফি প্রদান করবেন। ঋণগ্রহণকারী সম্মতি জানান যে উপরের ফি ঋণদাতার নথিপত্র তৈরী এবং ঋণ অনুমোদন করার সময় হওয়া অন্যান্য আনুষঙ্গিক ব্যয়গুলি আংশিকভাবে বহন করতে ব্যবহার করা হতে পারে।", AmtInWrds);
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // f) किश्त
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>f)</b> কিস্তি: <br>");
                        sb.AppendFormat("উপরের সুদের হারের ভিত্তিতে, কিস্তি হবে <span style='border-bottom: 1px dotted black;'>{0}</span> টাকা। প্রথম কিস্তি দিতে হবে <span style='border-bottom: 1px dotted black;'>{1}</span> তারিখে। এরপর থেকে, প্রতিটি পরবর্তীকালীন মাসিক কিস্তি <span style='border-bottom: 1px dotted black;'>{2}</span> মাসের একটি সময়কালের জন্য প্রদান করতে হবে। সমভাবে বন্টন করামাসিক কিস্তি (ইএমআই) প্রতি ইংরেজি ক্যালেন্ডার মাসের <span style='border-bottom: 1px dotted black;'>{3}</span> তারিখে সংগ্রহ করা হবে।</td></tr>", Convert.ToString(dtLoanAgr.Rows[0]["EMIAmt"]), Convert.ToString(dtLoanAgr.Rows[0]["RepayStartDate"]), Convert.ToString(dtLoanAgr.Rows[0]["Tenure"]), "5");

                        // g) चुकौती
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>g)</b> পরিশোধ: <br>");
                        sb.Append("ঋণদাতার ঋণটি প্রদান করার বিবেচনায়, ঋণগ্রহণকারী উপরের সুদের হারের ভিত্তিতে এবং মাসিক কিস্তিতে সুদ সমেত ঋণটি পরিশোধ করবেন। ঋণগ্রহণকারী এতদ্বারা ইএমআই-এর হিসাব এবং কার্যকর সুদের হারের পদ্ধতি অধ্যয়ন করেছেন, বুঝতে পেরেছেন এবং সম্মত হয়েছেন। ঋণগ্রহণকারী অধিকতররূপে অন্য কোনও চার্জ যেমন সুদের কর অথবা যেকোনো সময় সরকারের আরোপ করা অন্যান্য কর/ চার্জগুলি প্রদান করতে সম্মত হন। ");
                        sb.Append("ঋণগ্রহণকারীর পরিশোধ পদ্ধতিটি হয় ন্যাশনাল অটোমেটেড ক্লিয়ারিং হাউস (এনএসিএচ)-এর মাধ্যমে, অথবা রসিদ জারি করার বিনিময়ে নগদ অর্থ প্রদান করে বা ঋণদাতার লিখিতভাবে সম্মত হওয়া অন্য যেকোনো পদ্ধতির মাধ্যমে হতে হবে। যেকোনো কারণেই হোক, যদি বকেয়া তারিখের মধ্যে ঋণদাতার অ্যাকাউন্টে কিস্তি ক্রেডিট না হয়, তাহলে সেক্ষেত্রে ঋণগ্রহণকারীকে ডিফল্ট বা ঋণ শোধে অক্ষম বলে দেখা হবে এবং ডিফল্ট সম্পর্কিত ধারাগুলি প্রযোজ্য হবে। ");
                        sb.Append("ঋণগ্রহণকারী উপরন্তু অ্যাকাউন্টে পর্যাপ্ত ব্যালেন্স বজায় রাখতে সম্মত হন, যাতে তার ব্যাঙ্কাররা অ্যাকাউন্ট থেকে অর্থ ডেবিট করা সহজতর করতে সক্ষম হয় এবং এইভাবে ডেবিটকৃত অর্থ ঋণদাতার ক্রেডিটে প্রেরণ করা যায়।<br><br>");
                        sb.Append("ঋণগ্রহণকারী যে ব্যাঙ্ক অ্যাকাউন্টের থেকে এনএসিএচ নিবন্ধিত রয়েছে, ঋণদাতার অগ্রিম লিখিত সম্মতি ছাড়া, সেটির নাম বা প্রকৃতি পরিবর্তন বা বন্ধ করবেন না। উপরন্তু ঋণগ্রহণকারী কোনও চেক দেওয়া থেকে বিরত থাকতে ঋণদাতাকে কল করবেন না এবং যেকোনো কারণেই হোক তার ব্যাঙ্কারকে “স্টপ পেমেন্ট” নির্দেশাবলীও জারি করবেন না এবং যদি ঋণগ্রহণকারী এমনটি করে থাকেন, তাহলে ঋণদাতা এই ধরনের নির্দেশাবলীকে স্বীকার করবে না এবং চেকটি ব্যাঙ্কে প্রদান করবে। চেকটি প্রত্যাখ্যান করার ক্ষেত্রে, ঋণদাতা অবাধে বকেয়া অর্থ আদায় করার জন্য উপযুক্ত পদক্ষেপ নিতে পারেন। ঋণগ্রহণকারী এতদ্বারা এখানে নিঃশর্তভাবে চেক বাউন্স হওয়ার চার্জ এবং চেকের পরিমাণ মেটানোর তারিখ পর্যন্ত বকেয়া সুদ, ও সেইসাথে চেকের পরিমাণের অর্থ প্রদান করতে সম্মত হন। ঋণগ্রহণকারী এতদ্বারা স্বীকার করেন যে তার ব্যাঙ্কারের থেকে তার জারি করা চেকের(গুলির) বাউন্স হওয়া সম্পর্কে নিশ্চিত করা তার দায়িত্ব এবং কোনও সময়ই এমনটি বলার চেষ্টা করবেন না বা বোঝাতে চাইবেন না যে তার জারি করা চেকটি প্রত্যাখ্যাত হওয়া সম্বন্ধে ঋণদাতা তাকে জানান নি।"); 
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // h) सूचना
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>h)</b> বিজ্ঞপ্তি: <br>");
                        sb.Append("ঋণদাতার ঋণগ্রহণকারী / যৌথ ঋণগ্রহণকারীদের কাছে পাঠানো যেকোনো বিজ্ঞপ্তি/চিঠি/অন্যান্য নথিপত্র এই চুক্তিতে দেওয়া ঠিকানায় পাঠাতে হবে এবং নিবন্ধিত ডাক ব্যবস্থার মাধ্যমে এটি পাঠানোর 5 কর্ম/ব্যবসায়িক দিবসের মধ্যে তা ডেলিভার করা হয়েছে বলে গণ্য করা হবে। কোম্পানি অর্থ প্রদানের সময়সূচী, সুদের হার, পরিষেবা চার্জ, ইত্যাদি সহ নিয়ম ও শর্তাবলীতে কোনও পরিবর্তন জানাতে ঋণগ্রহণকারীকে বিজ্ঞপ্তি দেবে এবং নিশ্চিত করবে যে সুদের হার ও চার্জগুলিতে হওয়া যেকোনো পরিবর্তন যেন পরবর্তীকাল থেকে প্রভাবিত হয়।"); 
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // i) क्षतिपूर्ति
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>i)</b> ক্ষতিপূরণ:<br> "); 
                        sb.Append("ঋণগ্রহণকারী সম্মত হন যে এই চুক্তিটি ঋণগ্রহণকারী এবং ঋণদাতার মধ্যে শুধুমাত্র একটি আর্থিক চুক্তি। ঋণগ্রহণকারী (এবং যৌথ ঋণগ্রহণকারী) এতদ্বারা ঋণগ্রহণকারী এবং/অথবা যৌথ ঋণগ্রহণকারী, তাদের/তার নিজ নিজ কর্মচারী, এজেন্ট, বা নিযুক্ত ব্যক্তির কোনও আচরণ, ত্রুটি, বা বাদ দেওয়ার ফলস্বরূপ হওয়া কন্য লোকসান, দাবি, দায়বদ্ধতা, বা ক্ষতি, অথবা এই চুক্তি বা এই চুক্তি অনুসারে পরবর্তীতে করা অন্য কোনও নথি সংক্রান্ত অনুপযুক্ত কার্য সম্পাদন বা কার্য অসম্পাদনের বিরুদ্ধে ঋণদাতা ও তার ডিরেক্টর, অফিসার, কর্মচারী, এজেন্ট এবং উপদেষ্টাদের ক্ষতিপূরণ দিতে এবং তাদের দায়ী না করতেও সম্মত হন। "); 
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        //sb.Append("<tr>");
                        //sb.Append("<td style='padding-top:5px;'>");
                        //sb.Append("<b>j)</b> ব্যবহার:<br> ");
                        //sb.Append("ঋণগ্রহণকারী ঋণটি শুধুমাত্র এখানে বিবৃত উদ্দেশ্যে এবং কোনও অসামাজিক অথবা ফটকামূলক উদ্দেশ্যের জন্য ব্যবহার না করতে সম্মত হন এবং দায়িত্বগ্রহণ করেন।");
                        //sb.Append("</td>");
                        //sb.Append("</tr>");

                        sb.Append("</table>");


                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 17th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append("<b>j)</b>ব্যবহার:<br>");
                        sb.Append("ঋণগ্রহণকারী ঋণটি শুধুমাত্র এখানে বিবৃত উদ্দেশ্যে এবং কোনও অসামাজিক অথবা ফটকামূলক উদ্দেশ্যের জন্য ব্যবহার না করতে সম্মত হন এবং দায়িত্বগ্রহণ করেন।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // k) भुगतान में चूक की स्थिति में
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>k)</b>ডিফল্টের ঘটনা এই চুক্তিতে অন্যত্র বিদ্যমান ধারাগুলিতে উল্লিখিত অনুসারে ঋণদাতার অধিকারগুলিকে ক্ষতিগ্রস্ত না করে, যদি এই ধারায় নির্দিষ্ট করা এক বা একাধিক ঘটনাগুলি ঘটে, সেক্ষেত্রে, ঋণদাতা ঋণগ্রহণকারীকে লিখিতভাবে একটি বিজ্ঞপ্তি দিয়ে, ঘোষণা করতে পারে যে প্রিন্সিপাল পরিমাণ, ও সেইসাথে ঋণের উপর বকেয়া সুদ এবং অন্যান্য পরিমাণ, যদি থাকে, সেগুলি অবিলম্বে ঋণগ্রহণকারীকে ঋণদাতাকে পরিশোধ করতে হবে, এবং এই চুক্তি এবং/অথবা ঋণগ্রহণকারী ও ঋণদাতার মধ্যে বিদ্যমান অন্য যেকোনো চুক্তি বা নথিপত্রের অধীনে বা শর্ত অনুযায়ী, উক্ত সমস্ত অর্থরাশি, এই চুক্তি অথবা ঋণগ্রহণকারী এবং ঋণদাতার মধ্যে সম্পন্ন করা অন্য কোনও চুক্তি বা সাধনীতে বিবৃত কোনোকিছুর বিপরীতে হলেও, অবিলম্বে বয়েকা ও পরিশোধযোগ্য হবে।<br><br>");

                        sb.Append("নিম্নলিখিতগুলিকে ডিফল্টের ঘটনা হিসেবে বিবেচনা করা হবে:<br>");
                        //sb.Append("<ol style='list-style-type: lower-roman;'>");
                        sb.Append("<b>i.</b> ঋণগ্রহণকারীর এখানে থাকা পদ্ধতিতে ঋণ বা যেকোনো ফি, চার্জ বা খরচ প্রদানে ব্যর্থ হওয়া অথবা এখানে বিবৃত বকেয়া কোনও কিস্তি বা অন্য কোনও পরিমাণ যে তারিখে তা বকেয়া হয়েছিল তার থেকে একদিনের বেশি সময়ের জন্য পরিশোধ না করা হলে; অথবা<br>");
                        sb.Append("<b>ii.</b> ঋণগ্রহণকারীর এই চুক্তিতে বর্ণিত যেকোনো নিয়ম ও শর্তাবলীর লঙ্ঘন করা হলে অথবা ঋণদাতাকে কোনও মিথ্যা বিবরণ দিলে; অথবা<br>");
                        sb.Append("<b>iii.</b> একজন প্রাপককে নিয়োগ করা হলে, বা ঋণগ্রহণকারীর এই সম্পদগুলির কোনওটির জন্য সংযুক্তি আরোপ করা হলে অথবা একজন ব্যক্তি হিসেবে, ঋণগ্রহণকারী দেউলিয়া ঘোষণা করলে বা দেউলিয়াত্বের কোনও পদক্ষেপ নিলে অথবা<br>");
                        sb.Append("<b>iv.</b> ঋণগ্রহণকারীর বিরুদ্ধে পেশাদার অসদাচরণের কার্যধারা গ্রহণ করা হলে; অথবা<br>");
                        sb.Append("<b>v.</b> ঋণগ্রহণকারীর ঋণদাতার প্রয়োজনীয় হতে পারা কোনও তথ্য বা নথিপত্র পেশ করায় ব্যর্থ হলে; অথবা<br>");
                        sb.Append("<b>vi.</b> ঋণগ্রহণকারীর ঋণদাতার ঋণগ্রহণকারীকে প্রদান করা অন্য কোনও ঋণ বা সুবিধার কোনও নিয়ম বা শর্তাবলীতে ডিফল্ট হলে; অথবা<br>");
                        sb.Append("<b>vii.</b> অন্য কোনও পরিস্থিতির থাকলে, যেখানে ঋণদাতার একক মতামতে তা ঋণদাতার স্বার্থকে বিপন্ন করেছে।<br>");
                        //sb.Append("</ol>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // l) भुगतान में चूक के परिणाम
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>l)</b> ডিফল্টের ঘটনার পরিণাম:<br>");
                        //sb.Append("<ol style='list-style-type: lower-roman;'>");
                        sb.Append("<b>i.</b> উপরিউক্ত যেকোনো ডিফল্টের ঘটনা ঘটলে, ঋণদাতা ঋণটি এবং তার সাথে সম্পর্কিত প্রদানযোগ্য অন্যান্য পরিমাণগুলি অবিলম্বে পরিশোধ করতে পারে। এই ধরনের দাবি জানানোর 7 (সাত) দিনের মধ্যে ঋণ এবং তার সাথে সম্পর্কিত প্রদানযোগ্য অন্যান্য পরিমাণগুলির বকেয়া ব্যালেন্স না মেটানোর ক্ষেত্রে, ঋণদাতার কাছে ঋণগ্রহণকারীর নামে থাকা সমস্ত অর্থ এবং অ্যাকাউন্টগুলি সম্পূর্ণরূপে লিয়েনে রাখা এবং প্রেরণ করার অধিকার আছে এবং ঋণদাতার কাছে এই ধরনের সুবিধার জন্য স্বাধীনভাবে ঋণগ্রহণকারীর বিরুদ্ধে কার্যধারা গ্রহণ করার অধিকার থাকবে।<br>");
                        sb.Append("<b>ii.</b> ঋণদাতার কাছে সুদ, চার্জ, ও ব্যয়গুলি সমেত ঋণের পরিমাণ উসুল করতে ঋণগ্রহণকারী এবং/অথবা যৌথ ঋণগ্রহণকারী/ ঋণগ্রহণকারীর বিরুদ্ধে কার্যধারা ও পদক্ষেপ গ্রহণ করার অধিকার আছে। ঋণগ্রহণকারী এবং যৌথ ঋণগ্রহণকারী এতদ্বারা ঋণের পরিমাণ সম্পূর্ণরূপে ঋণদাতাকে শোধ না করা পর্যন্ত বকেয়া পরিমাণের উপর প্রতি মাসে @% বকেয়া সুদ দিতে সম্মত হন।<br>");
                        sb.Append("<b>iii.</b> এই চুক্তিতে নির্দিষ্ট করা অধিকারগুলি ছাড়াও, ঋণদাতার কাছে এই চুক্তির অধীনে ঋণগ্রহণকারী এবং/অথবা যৌথ ঋণগ্রহণকারীর বকেয়া এবং প্রদানযোগ্য অর্থ আদায় করতে আদালতের হস্তক্ষেপের সাথে বা ছাড়া সমস্ত বা যেকোনো পদক্ষেপ নেওয়ার অধিকার আছে।<br>");
                        sb.Append("<b>iv.</b> এই চুক্তির অধীনে ঋণদাতার কাছে উপলভ্য অন্য কোনো অধিকার সত্ত্বেও, ঋণদাতার কাছে ঋণগ্রহণকারী এবং/অথবা যৌথ ঋণগ্রহণকারীর বিরুদ্ধে ফৌজদারি কার্যধারা অথবা অন্য কোনও উপযুক্ত পদক্ষেপের সূচনা করার অধিকার আছে যদি যেকোনো সময় ঋণদাতার কাছে তার একক বিবেচনায় এটি বিশ্বাস করার পর্যাপ্ত কারণ থাকে যে ঋণগ্রহণকারী এবং/অথবা যৌথ ঋণগ্রহণকারী ঋণদাতাকে কোনও মিথ্যা বর্ণনা দিয়েছেন এবং/অথবা কোনও নকল করা নথিপত্র বা মিথ্যাভাবে বানানো ডেটা জমা দিয়েছেন।<br>");
                        //sb.Append("<b>v.</b> এই চুক্তির অধীনে ঋণদাতার উপর ন্যস্ত সমস্ত অধিকার ও ক্ষমতাগুলি এই চুক্তি কার্যকর থাকার সময়কালের জন্য এবং নিরাপত্তা নথিপত্রের অধীনে ঋণগ্রহণকারী এবং/অথবা যৌথ ঋণগ্রহণকারীর বিরুদ্ধে একজন পাওনাদার হিসেবে ঋণদাতার কাছে থাকা যেকোনো অধিকারের সাথে অতিরিক্ত ও সম্পূরক হবে এবং এর থেকে কোনো অবমাননা হবে না।<br>");
                        //sb.Append("<b>vi.</b> এই চুক্তির অধীনে ঋণগ্রহণকারীর বকেয়া এবং প্রদানযোগ্য ব্যালেন্স পরিমাণ সংক্রান্ত ঋণদাতার পেশ করা অ্যাকাউন্টের কোনো স্টেটমেন্ট ঋণগ্রহণকারীকে স্বীকার করতে হবে এবং ঋণগ্রহণকারীর জন্য বাধ্যতামূলক হবে এবং তা এখানে উল্লেখ করা পরিমাণের সঠিকতার চূড়ান্ত প্রমাণ হিসেবে বিবেচিত হবে।  উপরে বিবৃতকে ক্ষতিগ্রস্ত না করে, যদি ঋণগ্রহণকারী কোনও স্টেটমেন্ট অথবা এর কোনও অংশকে প্রশ্ন করার আকাঙ্ক্ষা পোষণ করলে, ঋণগ্রহণকারীকে তার স্টেটমেন্ট গ্রহণ করার 7 (সাত) ব্যবসায়িক দিবসের মধ্যে এই বিষয়ের পূর্ণ বিশদ ঋণদাতাকে প্রদান করবেন এবং ঋণদাতা হয়তো তা বিবেচনা করতে পারে এবং এরপরে ঋণগ্রহণকারী যেকোনো কারণেই হোক না কেন এটি বিরুদ্ধে আপত্তি জানানোর কোনও অধিকার আর থাকবে না। তবে এটি স্পষ্ট করা হয় যে ঋণগ্রহণকারীর কাছে ঋণদাতার পেশ করা অ্যাকাউন্ট স্টেটমেন্ট ভুল হওয়া বা অন্য কোনও কারণের ভিত্তিতে ইএমআই-গুলি প্রদানে ডিফল্ট হওয়া বা বিলম্ব করার অধিকার থাকবে না।<br>");
                        ////sb.Append("</ol>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 18th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%;border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append("<b>v)</b> এই চুক্তির অধীনে ঋণদাতার উপর ন্যস্ত সমস্ত অধিকার ও ক্ষমতাগুলি এই চুক্তি কার্যকর থাকার সময়কালের জন্য এবং নিরাপত্তা নথিপত্রের অধীনে ঋণগ্রহণকারী এবং/অথবা যৌথ ঋণগ্রহণকারীর বিরুদ্ধে একজন পাওনাদার হিসেবে ঋণদাতার কাছে থাকা যেকোনো অধিকারের সাথে অতিরিক্ত ও সম্পূরক হবে এবং এর থেকে কোনো অবমাননা হবে না।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        //// vi)
                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append("<b>vi)</b> এই চুক্তির অধীনে ঋণগ্রহণকারীর বকেয়া এবং প্রদানযোগ্য ব্যালেন্স পরিমাণ সংক্রান্ত ঋণদাতার পেশ করা অ্যাকাউন্টের কোনো স্টেটমেন্ট ঋণগ্রহণকারীকে স্বীকার করতে হবে এবং ঋণগ্রহণকারীর জন্য বাধ্যতামূলক হবে এবং তা এখানে উল্লেখ করা পরিমাণের সঠিকতার চূড়ান্ত প্রমাণ হিসেবে বিবেচিত হবে।  উপরে বিবৃতকে ক্ষতিগ্রস্ত না করে, যদি ঋণগ্রহণকারী কোনও স্টেটমেন্ট অথবা এর কোনও অংশকে প্রশ্ন করার আকাঙ্ক্ষা পোষণ করলে, ঋণগ্রহণকারীকে তার স্টেটমেন্ট গ্রহণ করার 7 (সাত) ব্যবসায়িক দিবসের মধ্যে এই বিষয়ের পূর্ণ বিশদ ঋণদাতাকে প্রদান করবেন এবং ঋণদাতা হয়তো তা বিবেচনা করতে পারে এবং এরপরে ঋণগ্রহণকারী যেকোনো কারণেই হোক না কেন এটি বিরুদ্ধে আপত্তি জানানোর কোনও অধিকার আর থাকবে না। তবে এটি স্পষ্ট করা হয় যে ঋণগ্রহণকারীর কাছে ঋণদাতার পেশ করা অ্যাকাউন্ট স্টেটমেন্ট ভুল হওয়া বা অন্য কোনও কারণের ভিত্তিতে ইএমআই-গুলি প্রদানে ডিফল্ট হওয়া বা বিলম্ব করার অধিকার থাকবে না।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // m) लेखा परीक्षण
                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append("<b>m)</b>অডিট:<br>");
                        sb.Append("কোনও ক্রেডিট সুবিধা/ঋণ অ্যাকাউন্টকে রেড-ফ্ল্যাগ করা (রেড ফ্ল্যাগ করা অ্যাকাউন্ট বলতে এমন এক অ্যাকাউন্টকে বোঝায় যেখানে এক বা একাধিক ইডব্লিউএস (আর্লি ওয়ার্নিং সিস্টেম) সূচকগুলির উপস্থিতির কারণে প্রতারণামূলক কার্যকলাপের সন্দেহ করা হয়, যা সম্ভাব্য জালিয়াতির জন্য আরও গভীর তদন্তের সতর্কতা দেয়/ট্রিগার করে) হিসেবে শ্রেণীভুক্ত করার ক্ষেত্রে ব্যাঙ্ক তার একক এবং পরম বিবেচনায়, এই ধরনের অডিটের জন্য ব্যাঙ্কের পর্ষদের অনুমোদিত নীতির সাথে সঙ্গতি রেখে, এটি অধিকতর তদন্ত করতে একটি অডিট পরিচালনা করবে। ঋণগ্রহণকারীর অসহযোগিতার ফলে অমীমাংসিত বা বিলম্বিত অডিট রিপোর্টের ক্ষেত্রে, ব্যাঙ্ক ব্যাঙ্কের রেকর্ডে উপলভ্য উপাদানের এবং এই ধরনের ক্ষেত্রে তাদের নিজেদের অভ্যন্তরীণ তদন্ত/মূল্যায়নের ভিত্তিতে অ্যাকাউন্টটির স্থিতিকে প্রতারণামূলক অথবা অন্যকিছু হিসেবে চিহ্নিত করবে।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // n) शिकायत निवारण
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>n)</b>অভিযোগের প্রতিকার:<br>");
                        sb.Append("গ্রাহক ব্যাঙ্কের প্রদান করা পরিষেবাগুলির সাথে সন্তুষ্ট না হলে তার কাছে অভিযোগ নিবন্ধন করার পূর্ণ অধিকার আছে। গ্রাহক তাদের পছন্দের যেকোনো পদ্ধতিতে যেমন সশরীরে, লিখিতভাবে, টেলিফোনের মাধ্যমে, ইমেল করে বা care@unitybank.co.in এর মাধ্যমে গ্রাহক সেবা কেন্দ্রের মাধ্যমে তার অভিযোগ জমা দেবেন। যদি কোনও গ্রাহকের অভিযোগ 7 (সাত) দিনের মধ্যে সমাধান না করা হয় অথবা অন্যথায় যদি তিনি ব্যাঙ্কের প্রদান করা সিদ্ধান্তে সন্তুষ্ট না হন, তাহলে গ্রাহক কেন্দ্রীয় গ্রাহক সেবা দল/আঞ্চলিক নোডাল কার্যালয়ের সাথে যোগাযোগ করতে পারেন। গ্রাহক কেন্দ্রীয় গ্রাহক সেবা দল/আঞ্চলিক নোডাল কার্যালয়ের সিদ্ধান্তে সন্তুষ্ট না হলে, গ্রাহক তার অভিযোগটি নীচে উল্লেখিত ইমেলের মাধ্যমে বা টেলিফোন নম্বরে কল করে প্রধান নোডাল অফিসার/অভিযোগের প্রতিকারক অফিসার (কেন্দ্রীয়)-এর কাছে জানাতে পারেন।  আধিকারিকদের সাথে যোগাযোগের বিশদ ব্যাঙ্কের ওয়েবসাইট www.theunitybank.co.in –এ আপডেট করা হবে।<br><br>");
                        
                        
                        //sb.Append("<b>प्रधान नोडल अधिकारी / शिकायत निवारण अधिकारी (केंद्रीय):</b><br>");
                        //sb.Append("श्री महेन्द्र नबन्द्रापता<br>");
                        //sb.Append("यूनिटी स्मॉल फाइनेंस बैंक लिमिटेड, 13वां माला, रूपा रेनेसां, डी-33, TTC MIDC रोड, TTC औद्योगिक क्षेत्र, MIDC औद्योगिक क्षेत्र, तुडे, नवी मुंबई, महाराष्ट्र 400705<br>");
                        //sb.Append("ईमेल: level3escalation@unitybank.co.in<br>");
                        //sb.Append("फोन: 9152366104 (मोबाइल) सुबह 9:30 से शाम 6:00 बजे, सोमवार से शनिवार");
                        //sb.Append("</td>");
                        //sb.Append("</tr>");

                        sb.Append("<b>প্রধান নোডাল আধিকারিক / অভিযোগ নিষ্পত্তি আধিকারিক (কেন্দ্রীয়):</b><br>");
                        sb.Append("শ্রী মহেশ নবাপতা<br>");
                        sb.Append("ইউনিটি স্মল ফাইনান্স ব্যাংক লিমিটেড, ১৩তম তলা, এপা রেনেসাঁ, ডি-৩৩, TTC MIDC রোড, TTC শিল্প এলাকা, MIDC শিল্প এলাকা,তুরভে, নবি মুম্বই, মহারাষ্ট্র ৪০০৭০৫<br>");
                        sb.Append("ইমেল: level3escalation@unitybank.co.in<br>");
                        sb.Append("ফোন: ৯১৫২৩৬৬১০৪ (মোবাইল) সকাল ৯:৩০ থেকে সন্ধ্যা ৬:০০ পর্যন্ত, সোমবার থেকে শনিবার");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 19th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%;border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append("<b>o)</b>আদায়কারী এজেন্ট/এজেন্সি:<br>");
                        sb.Append("ব্যাঙ্ক এই সুবিধা চুক্তির অধীনে ঋণগ্রহণকারী এবং/অথবা গ্যারেন্টারের থেকে যেকোনো বকেয়া পরিমাণ আদায় করার উদ্দেশ্যের জন্য একজন আদায়কারী এজেন্ট হিসেবে যেকোনো স্বাধীন এজেন্ট/এজন্সিকে নিয়োগ করার অধিকার সংরক্ষণ করে। ব্যাঙ্কের কর্মচারীরা সহ এই ধরনের এজেন্ট/এজেন্সিগুলি ঋণের সময়কালে অথবা তার পরবর্তী সময়ে, ঋণগ্রহণকারী এবং/অথবা গ্যারেন্টারের বাসস্থানে অথবা ব্যবসার স্থানে বা অন্য কোথাও এবং সময়ে সময়ে একই বিষয়ে আপডেট হওয়া ব্যাঙ্কের অভ্যন্তরীণ নীতি অনুসারে, তার/তাদের থেকে ঋণের বকেয়া পরিমাণ আদায় করতে পারে। কোনও আদায়কারী এজেন্সি/এজেন্টকে সংশ্লিষ্ট করার পরে, ব্যাঙ্ক ঋণগ্রহণকারীকে আদায়কারী এজেন্সি/এজেন্টের নাম ও যোগাযোগের বিশদ সম্পর্কে আপডেট করবে। সেইসাথে, ব্যাঙ্ক স্বচ্ছতা নিশ্চিত করতে এবং ঋণগ্রহণকারী এবং/অথবা গ্যারেন্টার ও আদায়কারী এজেন্সি/এজেন্টের মধ্যে যোগাযোগ সহজতর করতে তার অফিসিয়াল ওয়েবসাইটে তার পরিষেবা প্রদানকারী হিসেবে ব্যাঙ্কের সাথে সম্বন্ধযুক্ত আদায়কারী এজেন্সি/এজেন্টের নাম ও যোগাযোগের বিশদ প্রকাশ করবে।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // p) सामान्य नियम एवं शर्तें
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>p)</b>সাধারণ নিয়ম ও শর্তাবলী: <br>");
                        sb.Append("এই চুক্তির পক্ষরা এতদ্বারা সাধারণ নিয়ম ও শর্তাবলী এবং চুক্তিপত্রতে সম্মত হন:<br>");
                        sb.Append("i. ঋণটি দাবি অনুযায়ী পরিশোধযোগ্য হবে। পরিশোধ অনুসূচীতে নির্দিষ্ট করা কোনোকিছু সত্ত্বেও, ঋণদাতার কাছে তার একক বিবেচনায় ঋণগ্রহণকারীর কাছে থেকে ঋণের পূর্ণ পরিমাণ ও পাশাপাশি ঋণদাতার জ্ঞাত সেরা কারণের জন্য ঋণ সংক্রান্ত অন্য সমস্ত বকেয়া পরিমাণ আদায় করা ও দাবি জানানোর অধিকার থাকবে।   <br>");
                        sb.Append("ii. পরিশোধের অনুসূচীটি সুদে কোনও তারতম্যের ক্ষেত্রে তা পুনরায় হিসাব করার ঋণদাতার অধিকারের কোনও ক্ষতি না করে কার্যকর থাকে এবং এই ধরনের কোনও পুনর্হিসাবের ক্ষেত্রে ঋণগ্রহণকারীকে এই ধরনের প্রভেদমূলক পরিশোধ অনুসূচী অনুসারে অর্থ প্রদান করতে হবে। সুদের হিসাব সম্পর্কে যেকোনো বিবাদ ঋণগ্রহণকারীকে কোনও কিস্তির অর্থ প্রদান করা রোধ করার অধিকার দেবে না<br>");
                        sb.Append("iii. ঋণদাতা, তার পরম বিবেচনায়, ঋণগ্রহণকারীকে ঋণ এবং ঋণ প্রদান পরবর্তী জারি করা পরিশোধ অনুসূচীতে নির্দিষ্ট করা কিস্তিতে প্রদানযোগ্য সুদ প্রদান করার অনুমতি দেয়, এবং এই ধরনের কিস্তিগুলি, ঋণদাতার কাছে আবশ্যক বলে মনে হলে, দাবি করলে পরিশোধ করা বাধ্যতামূলক।<br>");
                        sb.Append("iv. ঋণদাতার নিজের বিবেচনায় যেকোনো সময় কোনও কারণ না দেখিয়ে ঋণটি অবিলম্বে পরিশোধের জন্য দাবি করার অধিকার থাকবে।<br>");
                        sb.Append("v. ঋণদাতার বজায় রাখা রেকর্ডগুলি ঋণগ্রহণকারীর বকেয়া পরিমাণের অকাট্য প্রমাণ হবে।<br>");
                        sb.Append("vi. ঋণদাতার একজন অফিসারের স্বাক্ষর করা লিখিত একটি সার্টিফিকেট যেখানে যেকোনো সময় বকেয়া পরিমাণ উল্লেখ করা থাকবে সেটি ঋণগ্রহণকারীর বিরুদ্ধে অকাট্য প্রমাণ হবে। তবে, এখানে থাকা কোনোকিছুই ঋণদাতার স্বার্থকে ক্ষতিগ্রস্ত করবে না যদি ঋণগ্রহণকারীর বকেয়া এবং প্রদানযোগ্য সুদের হিসাবে কেরানির বা গণনা করায় কোনও ত্রুটি থাকে|<br>");
                        sb.Append("vii. ঋণগ্রহীতা লিখিতভাবে কমপক্ষে ৩ (তিন) দিন পূর্বে নোটিশ প্রদান করে ঋণের সম্পূর্ণ বকেয়া পরিশোধ করতে পারে। এই পরিস্থিতিতে, ঋণদাতার নীতিমালার অনুযায়ী ঋণদাতার কাছে নির্ধারিত চার্জ আরোপ করার অধিকার থাকবে, যা সেই সময়ে ঋণ অ্যাকাউন্টে বিদ্যমান সম্পূর্ণ বকেয়া পরিমাণের উপর প্রযোজ্য হবে। সুদ (পরিবর্তিত সুদসহ) এবং অন্যান্য চার্জ ইত্যাদি সেই মাসের শেষে আরোপিত হবে, যেই মাসে ঋণ পূর্ব সমাপ্তির নোটিশের সময়সীমা শেষ হয়। ঋণ পূর্ব সমাপ্তি তখনই কার্যকর হবে যখন প্রকৃত অর্থপ্রদান সম্পন্ন হবে।<br>");
                        sb.Append("viii. ঋণগ্রহণকারী এবং/অথবা যৌথ ঋণগ্রহণকারীর ঠিকানায় কোনও পরিবর্তন হওয়ার এক সপ্তাহের মধ্যে ঋণগ্রহণকারী এবং যৌথ ঋণগ্রহণকারী এই ধরনের পরিবর্তনের বিষয়ে ঋণদাতাকে লিখিতভাবে জানাবেন।<br>");
                        sb.Append("ix. ঋণগ্রহণকারী ঘোষণা করেন যে ঋণের আবেদন এবং সমর্থিত নথিপত্রে দেওয়া সমস্ত তথ্য সম্পূর্ণরূপে সত্য এবং, প্রতক্ষ্য বা পরোক্ষভাবে, কোনও মিথ্যা বিবরণ দেওয়া হয় নি এবং উক্ত বিষয়ে কোনও ভুল বা মিথ্যা বর্ণনাকে, প্রসঙ্গত, একটি পেশাদার অসদাচরণ এবং দায়িত্বশীল আচরণ বিধির সাথে বেমানান একটি আচরণ হিসেবে গণ্য করা হবে।<br>");
                        sb.Append("x. এই চুক্তি কার্যকর করা বা ঋণ পুনরুদ্ধার করার জন্য এবং এই চুক্তির অধীনে প্রদানযোগ্য সমস্ত পরিমাণের জন্য ঋণগ্রহণকারী সমস্ত শুল্ক, কর, ব্যয় এবং অন্যান্য খরচ পরিশোধ করবেন|<br>");

                        sb.Append("</td>");
                        sb.Append("</tr>");



                        // End Table
                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 20th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%;border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("xi. ঋণগ্রহণকারী স্পষ্টভাবে স্বীকার এবং গ্রহণ করেন যে ঋণদাতার কাছে, নিজে বা এর কোনও অফিসার বা কর্মচারীর মাধ্যমে নিম্নোক্ত পদক্ষেপগুলি নেওয়ার অধিকারকে ক্ষতিগ্রস্ত না করে, ঋণদাতার নির্বাচন অনুসারে এক বা একাধিক তৃতীয় পক্ষ (ঋণদাতার আদায়কারী সহকারী এজেন্সিগুলি হিসেবে) নিয়োগ করার এবং এই ধরনের তৃতীয় পক্ষকে ঋণদাতার পক্ষ হয়ে ঋণগ্রহণকারীর কাছে থেকে সমস্ত বকেয়া কিস্তি ও অন্যান্য বকেয়া পরিমাণ আদায় ও সংগ্রহ করার অধিকার ও কর্তৃত্ব সহ ঋণ পরিচালনা সংক্রান্ত এই চুক্তির অধীনে এর কার্য, অধিকার ও ক্ষমতার সবগুলি বা কয়েকটি অর্পণ ও নিযুক্ত করা এবং এই চুক্তিই অন্যত্র বিবৃত অনুযায়ী পণ্য পুনরায় অধিগত করা সহ এগুলির সাথে সংযুক্ত ও আনুষঙ্গিক সমস্ত আইনি কার্য, ক্রিয়াকলাপ, বিষয় ও জিনিসগুলি সম্পন্ন ও নিষ্পাদন করা, বিজ্ঞপ্তি পাঠানো, ঋণগ্রহণকারীর সাথে যোগাযোগ করা, ঋণগ্রহণকারীর থেকে নগদ/চেকের ড্রাফ্ট পাওয়া এবং ঋণগ্রহণকারীকে বৈধ ও সফল রসিদ ও সম্পাদনগুলি দেওয়ার অধিকার আছে। উপরিউক্ত উদ্দেশ্যে, ঋণদাতার কাছে ঋণগ্রহণকারী ও ঋণ সংক্রান্ত সমস্ত প্রয়োজনীয় অথবা প্রাসঙ্গিক তথ্য এই ধরনের তৃতীয় পক্ষের কাছে প্রকাশ করার অধিকার আছে এবং এখানে ঋণগ্রহণকারী ঋণদাতার এই ধরনের প্রকাশগুলিতে সম্মতি প্রদান।<br>");
                        sb.Append("xii. ইচ্ছাকৃত ডিফল্ট:<br>");
                        sb.Append("<ul>");
                        sb.Append("<li>ঋণদাতার কাছে প্রকাশিত পরিমাণ ব্যতীত: (ক) কোনও সহযোগী বা গ্রুপ সংস্থাগুলির সাথে ঋণগ্রহীতার সমস্ত চুক্তি বা চুক্তি, বা কোনও প্রতিশ্রুতি (যদি প্রযোজ্য হয়) অস্ত্রের দৈর্ঘ্যের ভিত্তিতে হয়; এবং (খ) ঋণগ্রহীতার বিষয়াদি পরিচালনার দায়িত্বপ্রাপ্ত ও দায়িত্বপ্রাপ্ত কোন ব্যক্তি, ক্ষেত্রমত, আরবিআই বা কোন ক্রেডিট ইনফরমেশন কোম্পানী বা এক্সপোর্ট ক্রেডিট গ্যারান্টি কর্পোরেশন কর্তৃক ইচ্ছাকৃত খেলাপি ঘোষণা করা হয় নাই;</li>");
                        sb.Append("<li>ঋণগ্রহীতা নিশ্চিত করে যে পরিচালক / অংশীদার / ট্রাস্টি / সদস্য / ঋণগ্রহীতার বিষয়াদি পরিচালনার জন্য দায়িত্বপ্রাপ্ত এবং দায়বদ্ধ ব্যক্তি কেউই পরিচালক / অংশীদার / ট্রাস্টি / সদস্য / দায়িত্বপ্রাপ্ত ব্যক্তি নন এবং কোনও সংস্থা / ফার্ম / ট্রাস্ট / সোসাইটি / ব্যক্তিদের সমিতি যা আরবিআই / কোনও ক্রেডিট ইনফরমেশন সংস্থা বা কোনও নিয়ন্ত্রক কর্তৃপক্ষ দ্বারা ইচ্ছাকৃত খেলাপি হিসাবে চিহ্নিত করা হয়েছে।</li>");
                        sb.Append("<li>ঋণগ্রহীতা পরিচালক/প্রবর্তক/অংশীদার/ট্রাস্টি/সদস্য/দায়িত্বপ্রাপ্ত ব্যক্তি এবং ঋণগ্রহীতার বিষয়াদি পরিচালনার দায়িত্বে নিয়োজিত কোন ব্যক্তিকে অন্তর্ভুক্ত করবেন না, যিনি একজন পরিচালক/অংশীদার/সদস্য/ট্রাস্টি/দায়িত্বপ্রাপ্ত ব্যক্তি এবং ক্ষেত্রমত, কোম্পানী / ফার্ম / ব্যক্তি / ব্যক্তি সমিতি / ট্রাস্ট/সোসাইটির বিষয়াদি পরিচালনার জন্য দায়ী,  ইচ্ছাকৃত ঋণখেলাপি হিসেবে চিহ্নিত। যদি এইরূপ কোন ব্যক্তি কোন কোম্পানী/ফার্ম/ব্যক্তি/ট্রাস্টের বিষয়াদি পরিচালনার দায়িত্বে নিয়োজিত এবং দায়িত্বপ্রাপ্ত ব্যক্তি বলিয়া প্রতীয়মান হন, ক্ষেত্রমত, ইচ্ছাকৃত খেলাপি হিসাবে চিহ্নিত হইলে, ঋণগ্রহীতা উক্ত ব্যক্তিকে অপসারণের জন্য দ্রুত ও কার্যকর পদক্ষেপ গ্রহণ করিবেন।</li>");
                        sb.Append("<li>ঋণগ্রহীতা বুঝতে পেরেছেন যে যদি ঋণগ্রহীতা বা তার পরিচালক/প্রবর্তক/অংশীদার/ট্রাস্টি/সদস্য/দায়িত্বপ্রাপ্ত ব্যক্তি এবং ঋণগ্রহীতার বিষয়াদি পরিচালনার দায়িত্বে থাকা ব্যক্তিকে ইচ্ছাকৃত ঋণখেলাপি হিসাবে শ্রেণীবদ্ধ করা হয়, তবে ইচ্ছাকৃত ঋণখেলাপিদের সাথে সম্পর্কিত বিধিনিষেধগুলি 30শে জুলাই তারিখে ইচ্ছাকৃত খেলাপি এবং বড় খেলাপিদের চিকিত্সা সম্পর্কিত আরবিআইয়ের মাস্টার নির্দেশের অধীনে নির্দিষ্ট করা হয়েছে,  সময়ে সময়ে সংশোধিত ও সংশোধিত 2024 প্রযোজ্য হবে।</li>");

                        sb.Append("</ul>");
                        sb.Append("xiii. নিরীক্ষকের ভূমিকা: তহবিলের শেষ ব্যবহার পর্যবেক্ষণের লক্ষ্যে, ঋণগ্রহীতা ঋণদাতার কাছে একটি শেষ ব্যবহারের শংসাপত্র জমা দেবেন, যা ঋণদাতার কাছে সন্তোষজনক ফর্ম এবং পদ্ধতিতে, অন্যান্য বিষয়ের মধ্যে এই মর্মে প্রত্যয়ন করবে যে সুবিধার কোনও ডাইভারশন / পাচার হয়নি। ঋণগ্রহীতা এতদ্বারা ঋণদাতাকে সম্মতি দেয় - (ক) ঋণগ্রহীতার নিরীক্ষকের প্রয়োজন হয়; অথবা (খ) ঋণদাতা নিরীক্ষক নিয়োগ করিবেন- যাহা নিশ্চিত করিতে পারিবেন যে, উক্ত সুবিধার কোন ডাইভারশন/পাচার করা হয় নাই। ঋণদাতাদের নিরীক্ষক সুবিধার শেষ ব্যবহারের প্রত্যয়নের জন্য যুক্তিসঙ্গতভাবে প্রয়োজনীয় তথ্য জিজ্ঞাসা করতে পারে এবং ঋণগ্রহীতা এই জাতীয় ডেটা ভাগ করে নেওয়ার উদ্যোগ নেয়। যদি ঋণগ্রহীতার পক্ষ থেকে কোন মিথ্যাচার, ভুল উপস্থাপনা বা হিসাব কারসাজি ব্যাংক কর্তৃক পরিলক্ষিত হয় এবং নিরীক্ষকগণ নিরীক্ষা পরিচালনায় অবহেলা বা ত্রুটিপূর্ণ বলে প্রমাণিত হয়, সেক্ষেত্রে ঋণগ্রহীতার বিধিবদ্ধ নিরীক্ষকদের বিরুদ্ধে শৃঙ্খলামূলক ব্যবস্থা গ্রহণের জন্য যথাযথ কর্তৃপক্ষের কাছে আনুষ্ঠানিক অভিযোগ দায়ের করার অধিকার ব্যাংকের থাকবে। ব্যাংক নিরীক্ষকগণকে নির্ধারিত পদ্ধতি অনুসরণ করে শুনানির সুযোগ প্রদান করবে, যা লিপিবদ্ধ করতে হবে।<br>");

                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 21th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%;border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:10px;'>");

                        sb.Append("<b>q)</b>নিযুক্তি:<br>");
                        sb.Append("i. ঋণগ্রহণকারী স্পষ্টভাবে স্বীকার এবং গ্রহণ করেন যে ঋণদাতা হয়তো এই চুক্তির অধীনে বকেয়া যেকোনো পরিমাণের জন্য ডিফল্টের ঘটনায় এই ধরনের ক্রেতা, হস্তান্তর বা নিযুক্তির প্রাপকের পক্ষ হয়ে ঋণগ্রহণকারীর বিরুদ্ধে কার্যধারা গ্রহণ করার অধিকার সহ, ঋণগ্রহণকারীর আগাম সম্মতি ছাড়া ও ঋণদাতার সিদ্ধান্ত অনুযায়ী এই ধরনের শর্তাবলীতে সম্পূর্ণরূপে বা আংশিকভাবে, এর সমস্ত বা যেকোনো অধিকার, সুবিধা বা বাধ্যবাধকতা যেকোনো পক্ষকে বিক্রয়, স্থানান্তর, বা নিযুক্ত করতে পারে।  এই ধরনের কোনও বিক্রয়, স্থানান্তর বা নিযুক্তি ঋণগ্রহণকারীর উপর বাধ্যতামূলক হবে এবং ঋণগ্রহণকারী এই ধরনের ক্রেতা, হস্তান্তরকারী বা নিজ্ক্তির প্রাপককে তার একমাত্র ঋণদানকারী বা ঋণদাতার সাথে যৌথভাবে ঋণদানকারী হিসেবে গ্রহণ করবে এবং সেক্ষেত্রে ঋণগ্রহণকারী ঋণদাতা অথবা এই ধরনের ঋণদানকারী বা ঋণদাতার নির্দেশকৃতকে এই চুক্তির অধীনে ঋণগ্রহণকারীর বকেয়া পরিমাণ প্রদান করবে। এই ধরনের বিক্রয়, স্থানান্তর বা নিযুক্তি অথবা বকেয়া পরিমাণের অধিকার বলবৎকরণ ও বকেয়া আদায়ের কারণে যে কোনো খরচই হোক না কেন, তা সম্পূর্ণরূপে ঋণগ্রহণকারীর দায়িত্বে ন্যস্ত হবে।<br>");
                        sb.Append("ii. ঋণগ্রহণকারীর কাছে এখানে বিদ্যমান কোনও বাধ্যবাধকতাই স্থানান্তর বা নিযুক্ত করার অধিকার থাকবে না।<br>");

                        sb.Append("iii. ঋণগ্রহণকারী এতদ্বারা ঋণদাতার ঋণগ্রহণকারীকে আগাম বিজ্ঞপ্তি জানিয়ে বা রেফারেন্স না দিয়ে নিম্নলিখিতগুলি করতে দিতে সম্মত হন এবং স্পষ্টভাবে অনুমোদন করেন:<br>");
                        sb.Append("  1.যেকোনো ঋণের তথ্য অথবা ঋণগ্রহণকারী সংক্রান্ত অন্যান্য তথ্য ঋণদাতার প্রধান কার্যালয় বা অন্য কোনও শাখা কার্যালয় বা ঋণদাতার অধীনস্থ কোনও কোম্পানির কাছে প্রকাশ করতে। <br>");
                        sb.Append("  2.তৃতীয় পক্ষদের (ব্যাংক, আর্থিক সত্তা, ক্রেডিট ব্যুরো, বিধিবদ্ধ ও নিয়ামক কর্তৃপক্ষগুলি সহ) সাথে ঋণদাতার জ্ঞানস্থ এবং ঋণ এবং/অথবা ঋণগ্রহণকারী সংক্রান্ত সমস্ত তথ্য (ঋণগ্রহণকারীর ঋণের ইতিহাস এবং ঋণের স্থিতি সহ) যেকোনো সময় ঋণদাতার প্রয়োজনীয় বলে বিবেচনা করা বা এটি করার অনুরোধ বা নির্দেশ পাওয়া অনুসারে শেয়ার করতে।<br>");
                        sb.Append("  3.ডিফল্টের ঘটনা ঘটলে এবং ঋণগ্রহণকারীর ঋণের কোনও অংশ ও সেই সাপেক্ষে প্রদানযোগ্য অন্যান্য পরিমাণ পরিশোধে কোনও ডিফল্ট করার ক্ষেত্রে, ঋণদাতার কাছে একজন ডিফল্টার হিসেবে ঋণগ্রহণকারীর নাম যেকোনো বিধিবদ্ধ বা নিয়ামক কর্তৃপক্ষ অথবা ভারতীয় রিজার্ভ ব্যাংক, বা এমন কোনও সমিতি ঋণদাতা যার সদস্য তাদের কাছে প্রকাশ করার অধিয়ার থাকবে (এবং ঋণগ্রহণকারী ঋণদাতাকে এমনটি করতে স্পষ্টভাবে সম্মতি জানান)।<br>");
                        sb.Append("<b>r)</b>বিবাদ সমাধান:<br>");
                        sb.Append("চুক্তি অথবা ঋণ সংক্রান্ত ঋণগ্রহণকারী বা ঋণদাতার মধ্যে কোনও বিবাদের ক্ষেত্রে, উক্ত বিষয়টি ঋণদাতার নিয়োগ করা একজন একমাত্র নিষ্পত্তিকারী ব্যক্তির সিদ্ধান্ত সাপেক্ষ হবে। নিষ্পত্তির স্থানটি হবে মুম্বই। নিষ্পত্তিটি ইংরেজি ভাষায় পরিচালিত হবে। নিষ্পত্তিকারীর কাছে ইন্টারিম রায় অথবা আদেশ জারি করার ক্ষমতা থাকবে এবং নিষ্পত্তি এবং মীমাংসাকরণ আইন,1996 অথবা বিবাদের নিষ্পত্তির উল্লেখ করার সময়ে বলবৎ থাকা অন্য কোনও আইন অনুযায়ী কার্যধারা পরিচালনা করা হবে।<br>");
                        sb.Append("<b>s)</b>অন্যান্য প্রাসঙ্গিক ধারাগুলি:<br>");
                        sb.Append("i. এই চুক্তিটিকে গ্যারেন্টার চিঠি সমেত পক্ষদের মধ্যে সম্পূর্ণ চুক্তি হিসেবে গণ্য করা হবে।<br>");
                        sb.Append("ii. এই চুক্তির বৈধতা, ব্যাখ্যা বা নিষ্পাদন সংক্রান্ত সমস্ত প্রশ্ন ভারতীয় আইন দ্বারা এবং মহারাষ্ট্র ও মুম্বইয়ের আদালতগুলির এক্তিয়ারের মধ্যে পরিচালিত হবে। <br>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 22th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%;order-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:10px;'>");


                        sb.Append("iii. এটি ধরে নেওয়া হয় যে ঋণগ্রহণকারী স্বেচ্ছায় এবং এই চুক্তির পূর্ণ নিহিতার্থ বোঝার পরে ও উপযুক্ত আইনি পরামর্শ নিয়ে এই চুক্তিটি সম্পাদন করছেন।<br>");
                        sb.Append("iv. এই চুক্তির অধীনে প্রতিকারগুলি একচেটিয়া নয় বরং সমষ্টিগত হবে, এবং একটি প্রতিকার নির্বাচন করার অর্থ এই নয় যে অন্য প্রতিকারগুলি অনুসরণ করা যাবে না। <br>");
                        sb.Append("v. এখানে থাকা যেকোনো এক বা একাধিক বিধানগুলিকে উপযুক্ত এক্তিয়ারভুক্ত কোনও আদালত যেকোনো প্রসঙ্গে অবৈধ, বেআইনি বা অবলবৎযোগ্য হিসেবে রায় দেওয়ার ক্ষেত্রে, এখানে থাকা অবশিষ্ট বিধানগুলির বৈধতা, আইনের উপযুক্ততা ও বলবৎযোগ্যতা তার ফলে কোনভাবেই প্রভাবিত বা বিকল হবে না। উপরন্তু, যদি ঋণগ্রহীরা যৌথ ঋণগ্রহণকারী হন,  তাহলে উভয়েই এখানে বিদ্যমান সমস্ত বাধ্যবাধকতাগুলি চরিতার্থ করার জন্য যৌথ ও এককভাবে দায়বদ্ধ এবং দায়ী হবেন এবং “ঋণগ্রহণকারী” শব্দটি সেই অনুসারে ব্যাখ্যা করা হবে। <br>");
                        sb.Append("vi. পক্ষদ্বয়ের মধ্যে যেকোনো একজনের দ্বারা অপরজনের বিরুদ্ধে কোনও অধিকার বা দাবি বলবৎ করায় ব্যর্থতা বা পরিহার করাকে সেই পক্ষের দ্বারা এই ধরনের কোনও অধিকার বা দাবি অথবা এখানে বিদ্যমান কোনও অধিকার বা দাবির অব্যাহতি দেওয়া বলে গণ্য করা হবে না। কোনও পক্ষের দ্বারা এর কোনও লঙ্ঘনের অব্যাহতিকে একই অথবা অন্য কোনো বিধানের পরবর্তীকালীন কোনও লঙ্ঘনের অব্যাহতি হিসেবে কাজ করবে না বা গণ্য করা হবে না।<br>");
                        sb.Append("vii. বিভাগের শিরোনামগুলি শুধুমাত্র সুবিহ্দার জন্য অন্তর্ভুক্ত করা হয়েছে এবং তা এই চুক্তির ব্যাখ্যা বা ভাষান্তরিত করতে ব্যবহার করা যাবে না।<br>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        #endregion
                        // End Loan Agreement
                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // START- KFS-2
                        #region 23th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;'>মূল তথ্য বিবৃতি (KFS)</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>তারিখ: {0}</td>", Convert.ToString(dtSancLetter.Rows[0]["FinalSanctionDate"]));
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='width:100%;' >নিয়ন্ত্রিত সত্তা/ঋণদাতার নাম:ইউনিটি স্মল ফাইন্যান্স ব্যাংক লিমিটেড</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>ঋণগ্রহীতার নাম ও ঠিকানা: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["MemberName"]));
                        sb.Append("</tr>");
                        sb.Append("<tr><td>&nbsp;</td></tr>");
                        sb.Append("<tr><td style='font-weight:bold;text-align:center;width:100%;'>বিভাগ 1 (সুদের হার এবং ফি/চার্জ)</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td style='padding:2px; width:5%;text-align:center;'>1</td>");
                        sb.Append("<td style='padding:2px; width:22%;'>ঋণ প্রস্তাব/অ্যাকাউন্ট নম্বর</td>");
                        sb.AppendFormat("<td style='padding:2px; width:25%;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanNo"]));
                        sb.Append("<td style='padding:2px; width:22%;'>ঋণের ধরন</td>");
                        sb.AppendFormat("<td colspan='4' style='padding:2px; width:26%;'>{0}</td>", "MEL Loan");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'>2</td>");
                        sb.Append("<td colspan='2' style='padding:2px;'>অনুমোদিত ঋণের পরিমাণ (টাকায় লিখুন)</td>");
                        sb.AppendFormat("<td colspan='5' style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanAmt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'>3</td>");
                        sb.Append("<td colspan='3'>পরিশোধের সময়সূচী<br>(i) পর্যায়ক্রমে বা 100% অগ্রিম পরিশোধ<br>(ii) যদি এটি পর্যায় অনুসারে হয়, তাহলে প্রাসঙ্গিক বিবরণ সহ ঋণ চুক্তির ধারা উল্লেখ করুন।</td>");
                        sb.AppendFormat("<td colspan='4' style='padding:2px;text-align:center;'>{0}</td>", "100% Upfront");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'>4</td>");
                        sb.Append("<td colspan='2' >4 ঋণের মেয়াদ (বছর/মাস/দিন)</td>");
                        sb.AppendFormat("<td colspan='5' style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["Tenure"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'>5</td>");
                        sb.Append("<td colspan='7' style='text-align:center;' >কিস্তির বিবরণ</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='text-align:center;' >কিস্তির প্রকার</td>");
                        sb.Append("<td style='text-align:center;' >সমান মাসিক কিস্তির সংখ্যা</td>");
                        sb.Append("<td style='text-align:center;' >সমান মাসিক কিস্তি (₹)</td>");
                        sb.Append("<td colspan='3' style='text-align:center;' >অনুমোদনের পর, পরিশোধ শুরু</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='3' style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["RepFreq"]));
                        sb.AppendFormat("<td  style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInstNo"]));
                        sb.AppendFormat("<td  style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["EMIAmt"]));
                        sb.AppendFormat("<td colspan='3' style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["FInstDt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'>6</td>");
                        sb.Append("<td colspan='3' style='text-align:center;' >সুদের হার (%) এবং প্রকার (ফিক্সড বা ফ্লোটিং বা হাইব্রিড)</td>");
                        sb.AppendFormat("<td colspan='4' style='padding:2px;text-align:center;'>{0}(Fixed)</td>", Convert.ToString(dtScheduleOwn.Rows[0]["IntRate"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'>7</td>");
                        sb.Append("<td colspan='7' style='text-align:center;' >সুদের পরিবর্তনশীল হারের ক্ষেত্রে অতিরিক্ত তথ্য</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;' >বেঞ্চমার্ক</td>");
                        sb.Append("<td style='text-align:center;' >বেঞ্চমার্ক হার (%) (B)</td>");
                        sb.Append("<td style='text-align:center;' >পারসেটেনেজের স্প্রেড রেট (%) (S)</td>");
                        sb.Append("<td style='text-align:center;' >চূড়ান্ত হারের শতাংশ (%)(B)+(S)</td>");
                        sb.Append("<td colspan='2' style='text-align:center;' >পর্যাবৃত্তি পুনরায় সেট করুন (মাস)</td>");
                        sb.Append("<td colspan='2' style='text-align:center;' >রেফারেন্স বেঞ্চমার্ক পরিবর্তনের প্রভাব ('R'-এ 25 bps এর পরিবর্তনের জন্য, পরিবর্তন:)</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;' ><br><br><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' ><br><br><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' ><br><br><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' ><br><br><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' >B <br><br><br><br> NA</td>");
                        sb.Append("<td style='text-align:center;' >S <br><br><br><br> NA</td>");
                        sb.Append("<td style='text-align:center;' >সমান মাসিক কিস্তি (₹) <br> NA</td>");
                        sb.Append("<td style='text-align:center;' >সমান মাসিক কিস্তি (₹) <br> NA</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'>8</td>");
                        sb.Append("<td colspan='7' style='text-align:center;' >ফি/চার্জ</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");


                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 24th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");


                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='padding:2px; width:25%;'></td>");
                        sb.Append("<td colspan='2' style='padding:2px; width:35%;'>RE কে প্রদেয় (A)</td>");
                        sb.Append("<td colspan='2' style='padding:2px; width:40%;'>RE এর মাধ্যমে তৃতীয় পক্ষের কাছে প্রদেয়</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:2px;text-align:center;'></td>");
                        sb.Append("<td style='padding:2px;text-align:center;'></td>");
                        sb.Append("<td style='padding:2px;text-align:center;'>এক-সময়/পুনরাবৃত্ত</td>");
                        sb.Append("<td style='padding:2px;text-align:center;'>প্রযোজ্য হিসাবে পরিমাণ (₹ টাকায়) বা শতাংশ (%)</td>");
                        sb.Append("<td style='padding:2px;text-align:center;'>এক-সময়/পুনরাবৃত্ত</td>");
                        sb.Append("<td style='padding:2px;text-align:center;'>প্রযোজ্য হিসাবে পরিমাণ (₹ টাকায়) বা শতাংশ (%)</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(i)</td>");
                        sb.Append("<td style='padding:2px;text-align:left;'>প্রসেসিং ফি</td>");
                        sb.Append("<td style='padding:2px;text-align:center;'><br><br>One Time</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>(অন্তর্ভুক্তিমূলক জিএসটি)<br>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["procfee"]));
                        sb.Append("<td style='padding:2px;text-align:center;'><br><br>One Time</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>(অন্তর্ভুক্তিমূলক জিএসটি)<br><br>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["procfeeBC"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(ii)</td>");
                        sb.Append("<td style='padding:2px;text-align:left;'>ইনস্যুরেন্স চার্জ</td>");
                        sb.Append("<td style='padding:2px;text-align:center;'><br><br>One Time</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;' >(অন্তর্ভুক্তিমূলক জিএসটি)<br>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInsurance"]));
                        sb.Append("<td style='padding:2px;text-align:center;'><br><br>One Time</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>(অন্তর্ভুক্তিমূলক জিএসটি)<br><br>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInsurance"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(iii)</td>");
                        sb.Append("<td  style='padding:2px;text-align:left;'>মূল্যায়ন চার্জ</td>");
                        sb.Append("<td colspan='4' style='padding:2px;text-align:center;'>No Fees</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(iv)</td>");
                        sb.Append("<td style='padding:2px;text-align:left;'>অন্য কোন (দয়া করে উল্লেখ করুন)</td>");
                        sb.Append("<td style='padding:2px;text-align:center;'></td>");
                        sb.Append("<td style='padding:2px;' ></td>");
                        sb.Append("<td style='padding:2px;text-align:center;'></td>");
                        sb.Append("<td style='padding:2px;'></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'></td>");
                        sb.Append("<td style='padding:2px;text-align:left;'>মোট চার্জ</td>");
                        sb.Append("<td style='padding:2px;text-align:center;'>One Time</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;' >{0}</td>", Convert.ToString(vTotalPayRE));
                        sb.Append("<td style='padding:2px;text-align:center;'>One Time</td>");
                        sb.AppendFormat("<td style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(vTotalPayTP));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>9</td>");
                        sb.Append("<td colspan='2' style='padding:2px;text-align:center;'>বার্ষিক শতকরা হার (APR) (%)</td>");
                        sb.AppendFormat("<td colspan='3' style='padding:2px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["IRRIC"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>10</td>");
                        sb.Append("<td colspan='5' style='padding:2px;text-align:center;'>কন্টিনজেন্ট চার্জের বিবরণ (প্রযোজ্য হিসাবে, ₹ বা %) **</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(i)</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>পেনাল চার্জ, যদি থাকে, বিলম্বিত অর্থ প্রদানের ক্ষেত্রে</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "3% + 18% GST");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(ii)</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>অন্যান্য পেনাল চার্জ, যদি থাকে</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>a</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>EMI বাউন্স চার্জ (অন্তর্ভুক্তিমূলক জিএসটি)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "Rs. 590 (Incl. GST)");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>b</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>এক্সটেনশন চার্জ (অন্তর্ভুক্তিমূলক জিএসটি)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>c</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>ভিজিট চার্জ  (অন্তর্ভুক্তিমূলক জিএসটি)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "Rs. 236 (Incl. GST)");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(iii)</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>ফোরক্লোজার চার্জ, যদি প্রযোজ্য হয়** (অন্তর্ভুক্তিমূলক জিএসটি)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "3% of OS (Allowed after 1st EMI paid) + GST");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(iv)</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>ফ্লোটিং থেকে ফিক্সড রেটে এবং তার বিপরীতে ঋণ পরিবর্তনের জন্য চার্জ</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;' >{0}</td>", "NA");
                        sb.Append("</tr>");


                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>(v)</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>অন্য কোন চার্জ (অনুগ্রহ করে উল্লেখ করুন)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>a</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>প্রি-পেমেন্ট চার্জ**</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;text-align:center;'>b</td>");
                        sb.Append("<td colspan='3' style='padding:2px;text-align:center;'>ঋণ পূর্ব সমাপ্তি ফি**</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:2px;text-align:center;'>মোট বকেয়া মূলধনে ৩%</td>", "");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        sb.Append("<p>*চার্জের উপর GST প্রযোজ্য হব</p>");

                        #endregion
                        // END- KFS-2

                        sb.Append("<div class='page-break'></div>"); // Page Break
                        
                        // Address Declaration
                        #region 25th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;font-weight:bold;'>ঠিকানার ঘোষণা</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>তারিখ: {0}</td>", Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanDt"]));
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='width:100%;' >প্রতি,</td></tr>");
                        sb.Append("<tr><td style='width:100%;' >ইউনিটি স্মল ফাইন্যান্স ব্যাঙ্ক লিমিটেড</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>শাখা- {0}</td>", Convert.ToString(dtAppFrm1.Rows[0]["ApplDepn1Name"]));
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='width:100%;'>বিষয়: <span style='text-decoration: underline;font-weight:bold;'>বর্তমান ঠিকানার ঘোষণা</span></td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td style='width:100%;' >প্রিয় মহাশয়,</td></tr>");
                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:10px;'>আমি <span style='border-bottom: 1px dotted black;'>{0}</span> বর্তমানে <span style='border-bottom: 1px dotted black;'>{1}</span> এ থাকি এবং ঘোষণা করি যে আমার কাছে উপরে উল্লেখ করা বর্তমান ঠিকানার প্রমাণ নেই।অনুগ্রহ করে এই চিঠিকে আমার বর্তমান ঠিকানার প্রমাণ হিসেবে গ্রহণ করুন এবং অনুগ্রহ করে সমস্ত চিঠিপত্র উপরে উল্লেখ করা ঠিকানায় পাঠান।</td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["FullName"]), Convert.ToString(dtAppFrm1.Rows[0]["ApplPAddress1"]));

                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td style='padding:10px;'>আবেদনকারীর স্বাক্ষর _____________________</td></tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // Form60
                        #region 26th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='3' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;font-weight:bold;'>ফর্ম-60</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>যার পার্মানেন্ট অ্যাকাউন্ট নম্বর নেই এবং যিনি নিয়ম ১১৪বি তে উল্লেখিত কোনও লেনদেনে প্রবেশ করেন, তাকে ঘোষণাপত্রের ফর্ম পূরণ করতে হবে / ফর্ম 61 এমন ব্যক্তির দ্বারা পূরণ করতে হবে| </td></tr>");
                        sb.Append("<tr><td>যাঁর কৃষি আয় রয়েছে এবং যিনি নিয়ম 114B-এর ধারা (a) থেকে (h)-এ উল্লিখিত লেনদেনের জন্য আয়কর প্রযোজ্য অন্য কোনও আয়ের প্রাপ্তিতে নেই।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>ঘোষণাকারীর পুরো নাম ও ঠিকানা <span style='border-bottom: 1px dotted black;'>{0} , {1}</span></td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["FullName"]), Convert.ToString(dtAppFrm1.Rows[0]["ApplPAddress1"]));
                        sb.AppendFormat("<tr><td style='width:100%;'>লেনদেনের পরিমাণ: <span style='border-bottom: 1px dotted black;'>{0}</span></td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanAmount"]));
                        sb.AppendFormat("<tr><td style='width:100%;'>জন্ম তারিখ <span style='border-bottom: 1px dotted black;'>{0}</span></td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["ApplDOB"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>আপনার কি কর নির্ধারণ করা হয়েছে?  &nbsp;&nbsp; হ্যাঁ ☐ &nbsp;&nbsp;&nbsp;&nbsp; না হ্যাঁ হলে,  ☑ &nbsp;&nbsp;&nbsp;&nbsp; यदि हाँ, <span style='border-bottom: 1px dotted black;'>{0}</span></td></tr>", "");
                        sb.AppendFormat("<tr><td style='width:100%;'>a) আয়ের শেষ রিটার্ন যেখানে দায়ের করা হয়েছিল সেই ওয়ার্ড/সার্কেল/রেঞ্জের বিশদ:  <span style='border-bottom: 1px dotted black;'>{0}</span></td></tr>", "");
                        sb.AppendFormat("<tr><td style='width:100%;'>b) প্যান (PAN) না থাকার কারণ:  <span style='border-bottom: 1px dotted black;'>{0}</span></td></tr>", "");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td style='width:100%;' >যাচাইকরণ:</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>আমি <span style='border-bottom: 1px dotted black;'>{0}</span> এতদ্বারা ঘোষণা করছি যে যা বিবৃত করা হয়েছে তা আমার সর্ব জ্ঞান ও বিশ্বাস অনুযায়ী সত্য। <span style='border-bottom: 1px dotted black;'>{1}</span> এ যাচাইকৃত এটি <span style='border-bottom: 1px dotted black;'>{2}</span> এর <span style='border-bottom: 1px dotted black;'>{3}</span> দিন।</td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["FullName"]), Convert.ToString(dtAppFrm1.Rows[0]["AppYear"]), Convert.ToString(dtAppFrm1.Rows[0]["AppMonth"]), Convert.ToString(dtAppFrm1.Rows[0]["AppDay"]));
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:10px;font-weight:bold;'>স্বাক্ষর _____________________</td></tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break
                      
                        // ScheduleCharges
                        #region 27th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='3' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;font-weight:bold;'>চার্জের অনুসূচী</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>আমি/আমরা উল্লেখ অনুযায়ী নিয়ম ও শর্তাবলী গ্রহণ করি</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;width:50%;'>ঋণগ্রহণকারী</td>");
                        sb.Append("<td style='padding:2px;width:50%;'>যৌথ ঋণ গ্রহণকারী</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:3px;'>1.নাম: {0}</td>", Convert.ToString(dtScheduleOfCharges.Rows[0]["Borrower"]));
                        sb.AppendFormat("<td style='padding:3px;'>2.নাম: {0}</td>", Convert.ToString(dtScheduleOfCharges.Rows[0]["CoBorrower"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:3px;'>স্বাক্ষর: {0}</td>", "");
                        sb.AppendFormat("<td style='padding:3px;'>স্বাক্ষর: {0}</td>", "");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:3px;'>রাখুন: {0}</td>", "");
                        sb.AppendFormat("<td style='padding:3px;'>রাখুন: {0}</td>", "");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:3px;'>তারিখ: {0}</td>", Convert.ToString(dtScheduleOfCharges.Rows[0]["SanctionDate"]));
                        sb.AppendFormat("<td style='padding:3px;'>তারিখ: {0}</td>", Convert.ToString(dtScheduleOfCharges.Rows[0]["SanctionDate"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td colspan='2' >&nbsp;</td></tr>");
                        sb.Append("<tr><td colspan='2' >চার্জের বিবরণ</td></tr>");
                        sb.Append("<tr><td colspan='2' >&nbsp;</td></tr>");

                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;width:60%;'>প্রক্রিয়াকরণ ফি</td>");
                        sb.Append("<td  style='padding:2px;width:40%;'>3% + জিএসটি</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>অপর্যাপ্ত তহবিল</td>");
                        sb.Append("<td  style='padding:2px;'>500 + জিএসটি</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>ঋণ স্টেটমেন্টের চার্জ (প্রতি অর্ধ বছরে 1টি স্টেটমেন্টের জন্য কোনও চার্জ নেই)</td>");
                        sb.Append("<td  style='padding:2px;'>100 + জিএসটি</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>এনওসিজারি করার চার্জ (1ম এনওসি-এর জন্য কোনও চার্জ নেই)</td>");
                        sb.Append("<td  style='padding:2px;'>100 + জিএসটি</td>");
                        sb.Append("</tr>");
                       
                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>ঋণ পরিশোধে খেলাপি বা বিলম্বের জন্য (মূল, সুদ, ব্যয়, চার্জ, কর, ব্যয় ইত্যাদি সহ) ব্যাংককে প্রদেয়</td>");
                        sb.Append("<td  style='padding:2px;'>প্রতিমাসে 3% (বার্ষিক 36%) + বকেয়াদিনগুলিতেওভারডিউপরিমাণেরউপর 18% জিএসটি।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>এমনকিডিফল্টসংঘটন (অপর্যাপ্ততহবিলওভিজিটচার্জ)</td>");
                        sb.Append("<td  style='padding:2px;'>সর্বনিম্ন INR 500/- (ভারতীয়রুপিপাঁচশতমাত্র) এবংসর্বোচ্চ INR 700/- (ভারতীয়রুপিসাতশতশুধুমাত্র) + GST</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:2px;'>ঋণ পূর্ব সমাপ্তি ফি:</td>");
                        sb.Append("<td  style='padding:2px;'>মোট বকেয়া মূলধনে ৩%</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        //Vernacular Language
                        #region 28th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='3' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;font-weight:bold;'>মাতৃভাষার ঘোষণা</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>আমি/আমরা, এতদ্বারা বিবৃত ও ঘোষণা করি যে:</td></tr>");
                        sb.Append("<tr><td>&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>আমি/আমরা আমার/আমাদের জানা ভাষায়, শ্রী  <span style='border-bottom: 1px dotted black;'>{0}</span>, যিনি নীচে প্রতিস্বাক্ষর করেছেন, তার মাধ্যমে ইউনিটি স্মল ফাইন্যান্স ব্যাংকের থেকে ঋণ নেওয়ার সাথে আনুষঙ্গিক ঋণের পূর্ণ নথিপত্র পড়ে শোনানো ও ব্যাখ্যা করা হয়েছে।</td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["FullName"]));
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td style='width:100%;' >আমি/আমরা এখানে উল্লেখিত সমস্ত নিয়ম ও শর্তাবলী বোঝার পরে ঋণের নথিপত্র স্বাক্ষর করেছি।</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td style='width:100%;'><span style='text-decoration: underline;font-weight:bold;'>নাম: </span>শ্রী/শ্রীমতি</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'><span style='border-bottom: 1px dotted black;'>{0} , {1}</span></td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["FullName"]), Convert.ToString(dtAppFrm1.Rows[0]["CoFullName"]));
                        sb.Append("<tr><td  style='width:100%;font-weight:bold;'>(ঋণগ্রহণকারী/যৌথ ঋণগ্রহণকারী/গ্যারেন্টার ইত্যাদির)স্বাক্ষর</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>নাম: <span style='border-bottom: 1px dotted black;'>{0}</span> (ইউনিটি স্মল ফাইন্যান্স ব্যাংকের ব্যক্তি)</td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["EoName"]));
                        sb.AppendFormat("<tr><td style='width:100%;'>ঠিকানা: <span style='border-bottom: 1px dotted black;'>{0}</span> (ইউনিটি স্মল ফাইন্যান্স ব্যাংকের শাখার ঠিকানা)</td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["BranchAddress"]));
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td style='text-align: left;'><hr style='border: none; border-top: 1px dotted #000; width: 30%; margin: 0;'></td></tr>");
                        sb.Append("<tr><td style='padding:10px;'>স্বাক্ষর</td></tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        //Annexure-2 (CAM-3)
                        #region 29th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='3' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:left;width:100%;font-weight:bold;'>পরিশিষ্ট 2</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>"); ;
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>ঋণের অন্তিম ব্যবহার – ঘোষণা</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.AppendFormat("<tr><td  style='text-align:left;width:100%;'>তারিখ: {0}</td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanDt"]));
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>প্রিয় মহাশয়,</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>বিষয়: স্টক কেনা/দোকানের রক্ষণাবেক্ষণ/ব্যবসার সাধারণ উদ্দেশ্যের জন্য মেল সরল ব্যাপার ঋণের জন্য আবেদন।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>"); ;
                        sb.AppendFormat("<tr><td style='width:100%;'>আমি, <span style='border-bottom: 1px dotted black;'>{0}</span> ইউএসএফবি লিমিটেডের কাছে জমা দেওয়া  <span style='border-bottom: 1px dotted black;'>{1}</span> তারিখের আবেদন নং. <span style='border-bottom: 1px dotted black;'>{2}</span> রেফার করুন: ইউনিটি স্মল ফাইন্যান্স ব্যাংকের থেকে ঋণের সুবিধা (সুবিধা) গ্রহণ করা হেতু।</td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["FullName"]), Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanDt"]), Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanApplNo"]));
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>উক্ত সুবিধাটি স্টক কেনা/দোকানের রক্ষণাবেক্ষণ/ব্যবসার সাধারণ উদ্দেশ্যের জন্য।আমি এটিও ঘোষণা করি যেসুবিধার অধীনে থাকা তহবিলটি প্রধান সোনা, সোনার বাট, সোনার গয়না, সোনার কয়েন, সোনা বিনিময়কারবার তহবিল (ইএফটি)-এর ইউনিট, গোল্ড মিউচুয়াল ফান্ডের ইউনিট, রিয়েল এস্টেট ইত্যাদি সহ যেকোনো আকারে সোনা কেনার জন্য ব্যবহার করা হবে না। </td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>আমি এতদ্বারা বর্ণনা, সমর্থন ও নিশ্চিত করি যে উপরিউক্ত উদ্দেশ্যটি একটি বৈধ উদ্দেশ্য এবংসুবিধাটি শুধুমাত্র উপরে উল্লেখিত উদ্দেশ্যেই ব্যবহার করতে সম্মত হই ও দায়গ্রহণও করি এবং সুবিধাটি কোনও বেআইনি এবং/অথবা অসামাজিক এবং/অথবা ফাটকামূলক উদ্দেশ্য যার মধ্যে অন্তর্ভুক্ত কিন্তু এতে সীমাবদ্ধ নয় স্টক মার্কেট, আইপিও ও জমি ক্রয়ে অংশগ্রহণ ইত্যাদির জন্য ব্যবহার করা হবে না।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>আমি অধিকতর সম্মত হই, নিশ্চিত করি এবং দায়গ্রহণ করি যেসুবিধার অধীনে তহবিলের ব্যবহার সুবিধার মেয়াদকালে কোনও ভাবেই পরিবর্তিত হবে না; অথবা উদ্দেশ্যে এই ধরনের পরিবর্তন ইউনিটি স্মল ফাইন্যান্স ব্যাংকের থেকেশুধুমাত্র অগ্রিম লিখিত অনুমতিতেই সংঘটিত হবে।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>আমি সম্মতি জানাই যে উপরিউক্ত সমস্ত বা যেকোনো দায়গ্রহণ মেনে চলায় যেকোনো লঙ্ঘন বা ডিফল্টকে লেনদেনের নথিপত্রের অধীনে ডিফল্টের ঘটনা বলে ব্যাখ্যা করা হবে।ধন্যবাদান্তে,</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>বিনীত,</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>আবেদনকারী</td></tr>");
                        sb.Append("<tr><td style='height:10px;'></td></tr>");
                        sb.Append("<tr><td>যৌথ আবেদনকারী</td></tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // Repayment Schedule
                        #region 30th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td class='xsmall_font' style='text-align:center;font-weight:bold;' >নিবন্ধিত অফিস:৫০, বসন্ত লোক, বসন্ত বিহার, নয়াদিল্লি, ভারত – ১১০০৫৭</td></tr>");
                        sb.Append("<tr><td class='xsmall_font' style='text-align:center;font-weight:bold;' >প্রধান অফিস:ইউনিটি স্মল ফাইন্যান্স ব্যাংক লিমিটেড, ৫ম এবং ৬ষ্ঠ তলা, টাওয়ার - ১, এল অ্যান্ড টি সিওউডস টাওয়ার, প্লট নম্বর - আর - ১, সেক্টর - ৪০, সিওউডস রেলওয়ে স্টেশন,নবি মুম্বই – ৪০০৭০৬</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("</table>");

                        sb.Append("<p>ঋণ কার্ড</p>");
                        sb.Append("<table class='xsmall_font'  cellspacing='0' cellpadding='3' style='border-collapse:collapse; border:1px solid black; width:100%;'>");

                        sb.Append("<tr style='border:none;'>");
                        sb.Append("<th colspan='2' style='border:1px solid black;'>পুনঃভুগতান অনুসূচি</th>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;width:55%;border:none;'>শাখা: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["Branch"]));
                        sb.AppendFormat("<td style='padding:3px;width:45%;border:none;'>{0}</td>", "");
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>গ্রাহকের নাম: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["MemberName"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>ঋণ চক্র: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanCycle"]));
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>সহ-ঋণগ্রহীতার নাম: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["CoApplicantName"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>বিতরণ তারিখ: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanDt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>ঋণ রাশি (रू.): {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanAmt"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>বিতরণ ঋণের সংখ্যা: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanNo"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>মোট সুদ (रू.): {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["IntAmt"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>কিস্তির সংখ্যা: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInstNo"]));
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>পক্রিয়াকরণ ফি + জিএসটি (रू.): {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["ProcFees"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>শেষ কিস্তির তারিখ: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LInstDt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>বীমাকারীর নাম(জীবন বীমা): {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["ICName"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>অর্থ প্রদানের ফ্রিকোয়েন্সি: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["RepFreq"]));
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>বীমা + জিএসটি (रू.): {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["PropertyInsurance"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>ঋণ পরিকল্পনা: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanTypeName"]));
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>বীমাকারীর নাম(মেডিক্লেইম): {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["HospiName"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>সুদের হার (%): {0}</td>", Convert.ToString(Math.Round(Convert.ToDouble(dtScheduleOwn.Rows[0]["IntRate"]), 2)));
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>মধ্যস্থতা ফি (रू.): {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["MedAmt"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>অর্থ প্রদানের আইডি: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["PID"]));
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        sb.Append("<table class='xsmall_font' style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr><td colspan='2' style='padding-top:20px;text-decoration: underline;'><strong>ইএমআই-এর সূচী</strong></td></tr>");
                        sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table  class='xsmall_font' style='border-collapse: collapse; width: 100%;  text-align: left;' border='1' >");
                        sb.Append("<tr>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:6%;'>ক্রমিক সংখ্যা<br>Sr.No.</th>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:19%;'>নির্ধারিত তারিখ<br>Due Date</th>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:11%;'>মূল বকেয়া<br>Principle Due</th>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:14%;'>সুদের বকেয়া<br>Interest Due</th>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:11%;'>মাসিক কিস্তির পরিমাণ<br>EMI Amount</th>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:12%;'>মূল বকেয়া পরিমাণ<br>Principle Outstanding</th>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:6%;'>সুদের বকেয়া পরিমাণ<br>Interest Outstanding</th>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:7%;'>মোট বকেয়া পরিমাণ<br>Total Outstanding</th>");
                        sb.Append("<th colspan='2' style='padding: 2px;width:14%;'>প্রাথমিক</th>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th style='padding: 2px;'>ক্রেডিট অফিসার</th>");
                        sb.Append("<th style='padding: 2px;'>শাখা ব্যবস্থাপক</th>");
                        sb.Append("</tr>");

                        // Fill rows from DataTable
                        foreach (DataRow row in dtScheduleOwn.Rows)
                        {
                            sb.Append("<tr>");
                            sb.AppendFormat("<td style='padding:2px;'>{0}</td>", row["InstNo"]);
                            sb.AppendFormat("<td style='padding:2px;'>{0}</td>", row["DueDt"]);
                            sb.AppendFormat("<td style='padding:2px;'>{0}</td>", row["PrinceAmt"]);
                            sb.AppendFormat("<td style='padding:2px;'>{0}</td>", row["InstAmt"]);
                            sb.AppendFormat("<td style='padding:2px;'>{0}</td>", row["ResAmt"]);
                            sb.AppendFormat("<td style='padding:2px;'>{0}</td>", row["Outstanding"]);
                            sb.AppendFormat("<td style='padding:2px;'>{0}</td>", "");
                            sb.AppendFormat("<td style='padding:2px;'>{0}</td>", "");
                            sb.AppendFormat("<td style='padding:2px;'>{0}</td>", "");
                            sb.AppendFormat("<td style='padding:2px;'>{0}</td>", "");
                            sb.Append("</tr>");
                        }
                        // End table
                        sb.Append("</table>");

                        sb.Append("<table class='xsmall_font'  cellspacing='0' cellpadding='3' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td>**সংগ্রহের তারিখ ছুটির দিনে পড়লে ইএমআই পরবর্তী ব্যাসায়িক্ক দিবসে সংগ্রহ করা হবে অভিযোগ প্রতিকার অফিসারের বিশদ:নাম: শ্রী মহেন্দ্র বিন্দ্রা ইমেল - id-care@unitybank.co.in টোল ফ্রি নম্বর-18002091122 এমএফআইএন টোল ফ্রি নম্বর :- 18001021080");
                        sb.Append("</td></tr>");
                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // Start Consumer Education
                        #region 31th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='3' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='text-align:center;width:100%;font-weight:bold;'>উপভোক্তা শিক্ষা সাহিত্য</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>আমাদের ঋণ/ঋণের সুবিধা সংক্রান্ত ব্যবহার করা নির্দিষ্ট পরিভাষাগুলির ব্যাখ্যার ঘোষণা</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>আমরা নিশ্চিত করি যে আরবিআই-এর সংবিধিবদ্ধ আইআরএসিপি নিয়ম অনুযায়ী ব্যাংকের দ্বারা প্রদান করা নীচের ব্যাখ্যাগুলি বুঝেছি</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>গ্রাহকের শিক্ষা সাহিত্য</td></tr>");
                        sb.Append("<tr><td>আমাদের ঋণ/ ঋণ সংক্রান্ত ব্যবহার করা নির্দিষ্ট পরিভাষাগুলির ব্যাখ্যা</td></tr>");
                        sb.Append("<tr><td>উৎস সূত্র: অগ্রিম অর্থ সংক্রান্ত আয়ের স্বীকৃতি, সম্পদের শ্রেণীবিভাগ এবং ব্যবস্থাপনা বিষয়ক প্রুডেন্সিয়াল নিয়মাবলী (RBI/2022-23/15 DOR.STR.REC.4/21.04.048/2022-23 তারিখ 1 এপ্রিল, 2022)</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>ঋণগ্রহণকারীদের মধ্যে সচেতনতা বৃদ্ধি করার দৃষ্টিভঙ্গির সাথে, উপভোক্তাদের শিক্ষা সাহিত্যে উদাহরণের সাথে ব্যাখ্যা করে, দৈনন্দিন নির্দিষ্ট রেফারেন্সের সাথে বকেয়া তারিখ, এসএমএ ও এনপিএ-এর শ্রেণীবিভাগ ও আপগ্রেডকরণ-এর ধারণাগুলি ব্যক্ত করা হয়েছে।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>এই নথিতে উদ্ধৃত উদাহরণগুলি ব্যাখ্যামূলক এবং বিস্তীর্ণ প্রকৃতির নয় এবং সাধারণ পরিস্থিতি সংক্রান্ত। আরবিআই-এর জারি করা আইআরএসিপি নিয়ম ও স্পষ্টীকরণগুলি বাস্তবায়নের ক্ষেত্রে প্রাধান্য পাবে এবং এটি সময়ে সময়ে আরবিআই দ্বারা সংশোধিত হতে পারে।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td style='text-decoration: underline;'><b>ধারণা:</b></td></tr>");

                        sb.Append("<tr><td><b>1. বকেয়া:</b><br>");
                        sb.Append("এর অর্থ হল ঋণ সুবিধা অনুমোদনের শর্তাবলী অনুযায়ী বিধিবদ্ধ সময়কালের মধ্যে প্রদানযোগ্য ঋণ অ্যাকাউন্টের উপর ধার্য করা প্রিন্সিপাল/সুদ/অন্যান্য চার্জগুলি");
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td><b>2. বাকি পড়া:</b><br>");
                        sb.Append("এর অর্থ হল ঋণ সুবিধা অনুমোদনের শর্তাবলী অনুযায়ী বিধিবদ্ধ সময়কালের মধ্যে প্রদানযোগ্য কিন্তু প্রদান না করা ঋণ অ্যাকাউন্টের উপর ধার্য করা প্রিন্সিপাল/সুদ/অন্যান্য চার্জগুলি। অন্য কথায়, যেকোনো ঋণ সুবিধার অধীনে ব্যাংকের প্রতি বকেয়া যেকোনো পরিমাণই হল ‘বাকি পড়া’ যদি না তা ব্যাংক ও তার গ্রাহকের মধ্যে চুক্তি অনুযায়ী বকেয়া তারিখে বা তার আগে প্রদান করা হয়।ঋণদানকারী প্রতিষ্ঠানকে, দিন শেষের প্রক্রিয়া কতক্ষণ ধরে চলছে তা নির্বিশেষে, বকেয়া তারিখের জন্য এই ধরনের প্রক্রিয়ার অংশ হিসেবে ঋণগ্রহণকারীর অ্যাকাউন্টগুলিকে বাকি পড়া হিসেবে চিহ্নিত করতে হবে।");
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td>3. এসএমএ /এনপিএ স্থিতি নির্ধারণের জন্য ব্যাংকের ব্যবহৃত বাকি পড়া দিন সংখ্যায় উপনীত হওয়ার গণনা পদ্ধতি:ফিফো, অর্থাৎ, ‘ফার্স্ট ইন, ফার্স্ট আউট’ গণনা পদ্ধতির মূলনীতিটি ‘স্পেশাল মেনশন অ্যাকাউন্ট’ (এসএমএ) ও ‘নন পারফর্মিং অ্যাকাউন্ট’ (এনপিএ) স্থিতির (এসএমএ ও এনপিএ শ্রেণীবিভাগ নীচে ব্যাখ্যা করা হয়েছে) উপশ্রেণী নির্ধারণের জন্য বাকি পড়া দিন সংখ্যা উপনীত হওয়ার ক্ষেত্রে প্রাসঙ্গিক। ফিফো নীতিটি ধরে নেয় যে ঋণ অ্যাকাউন্টে সবচেয়ে পুরনো বাকি পড়া বকেয়া পরিমাণটি প্রথমে শোধ করতে হবে। এইভাবে ফিফো পদ্ধতি অনুসারে, প্রথমে যেটি বকেয়া ঋণগ্রহণকারী সেটি প্রথমে শোধ করা প্রয়োজন।");
                        
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td>উদাহরণস্বরূপ, 01.02.2022 তারিখ পর্যন্ত একটি ঋণ অ্যাকাউন্টে, কোনও বাকি পড়া পরিমাণ নেই এবং প্রিন্সিপাল কিস্তি/সুদ/চার্জের প্রতি চার্জের জন্য ১০০ টাকা পরিশোধ করতে হবে। সুতরাং, ঋণ অ্যাকাউন্টে 01.02.2022 তারিখ বা তার পরে ক্রেডিট হওয়া যেকোনো অর্থপ্রদান 01.02.2022 তারিখে বাকি পড়া বকেয়া পরিমাণ শোধ করতে ব্যবহার করা হবে।");  
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td>ফেব্রুয়ারি মাসে কোনো অর্থ প্রদান করা হয়নি / বা বকেয়ার আংশিক অর্থ (80 টাকা) প্রদান করা হয়েছিল ধরে নিয়ে, 01.03.2022 তারিখ পর্যন্ত বাকি পড়া পরিমাণ হবে 20 টাকা অর্থাৎ, 100 টাকা – 80 টাকা।");
                        sb.Append("</td></tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 32th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='2' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr><td><b>4. সবচেয়ে পুরনো বকেয়ার স্থায়িত্বকাল:</b><br>");
                        sb.Append("সবচেয়ে পুরনো বকেয়ার স্থিতিকাল গণনা করা হয় সেই তারিখ থেকে, যেদিন সেটি প্রদেয় হয়েছিল এবং এখনও পর্যন্ত অপরিশোধিত রয়েছে, এবং এটি দিন অনুযায়ী নির্ধারিত হয়। উপরের ব্যাখ্যায়, 01.02.2022 তারিখের বকেয়াগুলি 01.03.2022 তারিখ পর্যন্ত প্রদান না করা হলে, সবচেয়ে পুরনো বকেয়ার স্থিতিকাল 01.03.2022 তারিখ পর্যন্ত 29 দিন হিসেবে গণনা করা হয়।");
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td style='height:5px;'></td></tr>");

                        sb.Append("<tr><td>5. স্পেশাল মেনশন অ্যাকাউন্ট (এসএমএ) ও নন-পারফর্মিং অ্যাসেট (এনপিএ) হিসেবে শ্রেণীবিভাগ:<br>");
                        sb.Append("<b>a. এসএমএ:</b><br>");
                        sb.Append("ডিফল্ট হওয়ার সঙ্গে সঙ্গেই ব্যাংক ঋণ হিসাবগুলোর প্রাথমিক চাপ শনাক্ত করবে, এবং সেগুলোকে স্পেশাল মেনশন অ্যাকাউন্ট (এসএমএ) হিসেবে শ্রেণীবদ্ধ করবে। এসএমএ/এনপিএ শ্রেণীর শ্রেণীবিভাগের ভিত্তি নিম্নরূপ হবে:");
                        sb.Append("</td></tr>");

                        sb.Append("</table>");

                        sb.Append("<table class='xsmall_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='padding:2px;'>পুনরাবৃত্তি হওয়া সুবিধাগুলি ছাড়া ঋণগুলি</td>");
                        sb.Append("<td colspan='2' style='padding:2px;'>নগদ ক্রেডিট/ ওভারড্রাফ্ট প্রকৃতির ঋণগুলি</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;width:15%;'>এসএমএ উপ-শ্রেণী</td>");
                        sb.Append("<td style='padding:2px;width:35%;'>শ্রেণীবিভাগের ভিত্তি প্রিন্সিপাল বা সুদের অর্থপ্রদান অথবা অন্য কোনও পরিমাণের সম্পূর্ণরূপে বা আংশিকভাবে বাকি পড়া</td>");
                        sb.Append("<td style='padding:2px;width:15%;'>এসএমএ উপ-শ্রেণী</td>");
                        sb.Append("<td style='padding:2px;width:35%;'>শ্রেণীবিভাগের ভিত্তি -  বাকি পড়া পরিমাণের ব্যালেন্স ধারাবাহিকভাবে অনুমোদিতসীমা বা টাকা তোলার ক্ষমতা, যেটিই কম হবে, তা পেরিয়ে যায়, নিম্নোক্ত সময়কালের জন্য:</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>এসএমএ 0</td>");
                        sb.Append("<td style='padding:2px;'>30 দিন পর্যন্ত</td>");
                        sb.Append("<td style='padding:2px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:2px;'></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>এসএমএ 1</td>");
                        sb.Append("<td style='padding:2px;'>30 দিনের বেশি ও 60 দিন পর্যন্ত</td>");
                        sb.Append("<td style='padding:2px;'>এসএমএ 1</td>");
                        sb.Append("<td style='padding:2px;'>30 দিনের বেশি ও 60 দিন পর্যন্ত</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;'>এসএমএ 2</td>");
                        sb.Append("<td style='padding:2px;'>60 দিনের বেশি ও 90 দিন পর্যন্ত</td>");
                        sb.Append("<td style='padding:2px;'>এসএমএ 2</td>");
                        sb.Append("<td style='padding:2px;'>60 দিনের বেশি ও 90 দিন পর্যন্ত</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='2' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr><td><b>b. নন-পারফর্মিং অ্যাসেট (এনপিএ):</b><br>"); 
                        sb.Append("1 এপ্রিল, 2022 তারিখের অগ্রিম অর্থ সংক্রান্ত আয় স্বীকৃতি, সম্পদের শ্রেণীবিভাগ ও ব্যবস্থাপনার বিষয়ে প্রুডেন্সিয়াল নিয়মের উপর আরবিআই-এর মাস্টার সার্কুলারের বিধানগুলি অনুযায়ী, নন-পারফর্মিং অ্যাসেট (এনপিএ) হল এমন একটি ঋণ বা অগ্রিম অর্থ যেখানে: <br>");
                        sb.Append("i. সুদ এবং/অথবা প্রিন্সিপালের কিস্তি একটি টার্ম ঋণের সাপেক্ষে 90 দিনের বেশি একটি সময়কালের জন্য বাকি থাকে|<br>");
                        sb.Append("ii. অ্যাকাউন্টটি, একটি ওভারড্রাফ্ট/নগদ ক্রেডিট (ওডি/সিসি) সাপেক্ষে, নিচে নির্দেশিত অনুযায়ী|<br>");
                        sb.Append("iii. আউট অফ অর্ডার’ স্থিতিতে থাকে|<br>");
                        sb.Append("iv. বিল ক্রয় বা ছাড় দেওয়ার ক্ষেত্রে বিলটি 90 দিনের বেশি একটি সময়কালের জন্য বাকি থাকে।<br>");
                        sb.Append("v. এরপর থেকে প্রিন্সিপালের বা সুদের কিস্তি স্বল্প মেয়াদের ফসলের জন্য ফসলের দুটি মরশুম ধরে বাকি থাকে|<br>");
                        sb.Append("vi. এরপর থেকে প্রিন্সিপালের বা সুদের কিস্তি দীর্ঘ মেয়াদের ফসলের জন্য ফসলের একটি মরশুম ধরে বাকি থাকে|<br>");
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td><b>6. ‘আউট অফ অর্ডার’ স্থিতি:</b><br>");
                        sb.Append("একটি অ্যাকাউন্টকে ‘আউট অফ অর্ডার’ বলে গণ্য করা হবে যদি:<br>");
                        sb.Append("i. সিসি/ওডি অ্যাকাউন্টে বকেয়া পরিমাণ 90 দিনের জন্য অনুমোদিত সীমা/টাকা তোলার ক্ষমতার থেকে ধারাবাহিকভাবে বেশি থাকে, অথবা<br>");
                        sb.Append("ii. সিসি/ওডি অ্যাকাউন্টে বকেয়া পরিমাণ অনুমোদিত সীমা/টাকা তোলার ক্ষমতার চেয়ে কম থাকে কিন্তু একটানা 90 দিন ধরে কোনও অর্থ ক্রেডিট হয় না, অথবা সিসি/ওডি অ্যাকাউন্টে বকেয়া পরিমাণ অনুমোদিত সীমা/টাকা তোলার ক্ষমতার চেয়ে কম থাকে কিন্তু পূর্বের 90 দিনের সময়কালে ডেবিট হওয়া সুদ মেটানোর মতো পর্যাপ্ত ক্রেডিট থাকে না। <br>");
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td><b>7. বকেয়ার বিলম্বিত অর্থ প্রদান/অর্থ প্রদান না করার ভিত্তিতে একটি অ্যাকাউন্টের এসএমএ শ্রেণী থেকে এনপিএ শ্রেণীতে পরিণত হওয়া এবং দিন শেষের প্রক্রিয়ায় পরবর্তীতে স্ট্যান্ডার্ড শ্রেণীতে আপগ্রেড হওয়ার ব্যাখ্যা:</b></td></tr>");

                        sb.Append("</table>");

                        sb.Append("<table class='xsmall_font' border='1' cellspacing='0' cellpadding='2' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;width:12%;'></td>");
                        sb.Append("<td style='padding:3px;width:12%;'>অর্থপ্রদানের তারিখ</td>");
                        sb.Append("<td style='padding:3px;width:22%;'>অর্থপ্রদানের কভার</td>");
                        sb.Append("<td style='padding:3px;width:12%;'>সবচেয়েপুরনো বকেয়ারবয়সদিনে</td>");
                        sb.Append("<td style='padding:3px;width:12%;'>এসএমএ/ এনপিএ শ্রেণী </td>");
                        sb.Append("<td style='padding:3px;width:12%;'>এসএমএ তারিখ থেকে / এসএমএ শ্রেণীকরণের তারিখ</td>");
                        sb.Append("<td style='padding:3px;width:9%;'>এনপিএ শ্রেণীবিভাগ করারতারিখ</td>");
                        sb.Append("<td style='padding:3px;width:9%;'>এনপিএ হওয়ারতারিখ</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.01.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.01.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.01.2022 তারিখ পর্যন্ত সম্পূর্ণ বকেয়া</td>");
                        sb.Append("<td style='padding:3px;'>0</td>");
                        sb.Append("<td style='padding:3px;'>শূন্য</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.01.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.01.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 তারিখ পর্যন্ত আংশিক প্রদত্ত বকেয়া</td>");
                        sb.Append("<td style='padding:3px;'>1</td>");
                        sb.Append("<td style='padding:3px;'>এসএমএ-0</td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 33th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");


                        sb.Append("<table class='xsmall_font' border='1' cellspacing='0' cellpadding='3' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;width:12%;'>01.01.2022</td>");
                        sb.Append("<td style='padding:3px;width:11%;'>01.01.2022</td>");
                        sb.Append("<td style='padding:3px;width:26%;'>01.02.2022 তারিখ পর্যন্ত আংশিক প্রদত্ত বকেয়া</td>");
                        sb.Append("<td style='padding:3px;width:12%;'>2</td>");
                        sb.Append("<td style='padding:3px;width:12%;'>এসএমএ-0</td>");
                        sb.Append("<td style='padding:3px;width:9%;'>01.02.2022</td>");
                        sb.Append("<td style='padding:3px;width:9%;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;width:9%;'>প্রযোজ্য নয়</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.03.2022</td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 তারিখের বকেয়া, 01.03.2022 তারিখে পুরোপুরি শোধ করা হয়নি এবং 01.03.2022 তারিখ শেষ হওয়া পর্যন্তও বকেয়া আছে|</td>");
                        sb.Append("<td style='padding:3px;'>29</td>");
                        sb.Append("<td style='padding:3px;'>এসএমএ-0</td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 তারিখের বকেয়া সম্পূর্ণরূপে শোধ করা হয়েছে, 01.03.2022 তারিখের বকেয়া 01.03.2022 তারিখ শেষ হওয়া পর্যন্ত প্রদান করা হয়নি|</td>");
                        sb.Append("<td style='padding:3px;'>1</td>");
                        sb.Append("<td style='padding:3px;'>এসএমএ-0</td>");
                        sb.Append("<td style='padding:3px;'>01.03.2022</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 তারিখের বকেয়া সম্পূর্ণরূপে প্রদান করা হয়েছে, 01.03.2022 তারিখের বকেয়া 03.03.2022 তারিখ শেষ হওয়া পর্যন্ত সম্পূর্ণরূপে প্রদান করা হয়নি|</td>");
                        sb.Append("<td style='padding:3px;'>31</td>");
                        sb.Append("<td style='padding:3px;'>এসএমএ-1</td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 / 03.03.2022</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 তারিখের বকেয়া সম্পূর্ণরূপে প্রদান করা হয়েছে, 01.03.2022 তারিখের বকেয়া 01.03.2022 তারিখ শেষ হওয়া পর্যন্ত সম্পূর্ণরূপে প্রদান করা হয়নি|</td>");
                        sb.Append("<td style='padding:3px;'>1</td>");
                        sb.Append("<td style='padding:3px;'>এসএমএ-0</td>");
                        sb.Append("<td style='padding:3px;'>01.03.2022</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.04.2022</td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022, 01.03.2022 তারিখের কোনও বকেয়া নেই এবং 01.04.2022 তারিখশেষ হওয়া পর্যন্ত 01.04.2022 তারিখে পরিমাণ বকেয়া আছে।</td>");
                        sb.Append("<td style='padding:3px;'>60</td>");
                        sb.Append("<td style='padding:3px;'>এসএমএ-1</td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 / 03.03.2022</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>02.04.2022 তারিখ শেষ হওয়া পর্যন্ত 01.02.2022 থেকে 01.04.2022 তারিখ পর্যন্ত কোনও বকেয়া নেই।</td>");
                        sb.Append("<td style='padding:3px;'>61</td>");
                        sb.Append("<td style='padding:3px;'>এসএমএ-2</td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 / 02.04.2022</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.05.2022</td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>01.05.2022 তারিখ শেষ হওয়া পর্যন্ত 01.02.2022 থেকে 01.05.2022 তারিখ পর্যন্ত কোনও বকেয়া নেই।</td>");
                        sb.Append("<td style='padding:3px;'>90</td>");
                        sb.Append("<td style='padding:3px;'>এসএমএ-2</td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 / 02.04.2022</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>02.05.2022 তারিখ শেষ হওয়া পর্যন্ত 01.02.2022 থেকে 01.05.2022 তারিখ পর্যন্ত কোনও বকেয়া নেই</td>");
                        sb.Append("<td style='padding:3px;'>91</td>");
                        sb.Append("<td style='padding:3px;'>এনপিএ</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>এনপিএ</td>");
                        sb.Append("<td style='padding:3px;'>02.05.2022</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 34th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");


                        sb.Append("<table class='xsmall_font' border='1' cellspacing='0' cellpadding='3' style='border-collapse:collapse; width:100%;'>");
                        
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;width:12%;'>01.06.2022</td>");
                        sb.Append("<td style='padding:3px;width:12%;'>01.06.2022</td>");
                        sb.Append("<td style='padding:3px;width:22%;'>01.06.2022 তারিখ শেষ হওয়া পর্যন্ত 01.02.2022 তারিখে বকেয়া সম্পূর্ণরূপে প্রদান করা হয়েছে।</td>");
                        sb.Append("<td style='padding:3px;width:14%;'>93</td>");
                        sb.Append("<td style='padding:3px;width:12%;'>এনপিএ</td>");
                        sb.Append("<td style='padding:3px;width:12%;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;width:9%;'>এনপিএ</td>");
                        sb.Append("<td style='padding:3px;width:9%;'>02.05.2022</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.06.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.06.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.07.2022 তারিখ শেষ হওয়া পর্যন্ত01.03.2022 ও 01.04.2022 তারিখেরসম্পূর্ণবকেয়া প্রদান করা হয়েছে।</td>");
                        sb.Append("<td style='padding:3px;'>62</td>");
                        sb.Append("<td style='padding:3px;'>এনপিএ</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>এনপিএ</td>");
                        sb.Append("<td style='padding:3px;'>02.05.2022</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.08.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.08.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.08.2022 তারিখ শেষ হওয়া পর্যন্ত01.05.2022 ও 01.06.2022 তারিখেরসম্পূর্ণবকেয়া প্রদান করা হয়েছে।</td>");
                        sb.Append("<td style='padding:3px;'>32</td>");
                        sb.Append("<td style='padding:3px;'>এনপিএ</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>এনপিএ</td>");
                        sb.Append("<td style='padding:3px;'>02.05.2022</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.09.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.09.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.09.2022 তারিখ শেষ হওয়া পর্যন্ত01.07.2022 ও 01.08.2022 তারিখেরসম্পূর্ণবকেয়া প্রদান করা হয়েছে।</td>");
                        sb.Append("<td style='padding:3px;'>1</td>");
                        sb.Append("<td style='padding:3px;'>এনপিএ</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>এনপিএ</td>");
                        sb.Append("<td style='padding:3px;'>02.05.2022</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.10.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.10.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.09.2022 ও 01.10.2022 তারিখের সম্পূর্ণ বকেয়া প্রদান করা হয়েছে|</td>");
                        sb.Append("<td style='padding:3px;'>0</td>");
                        sb.Append("<td style='padding:3px;'>অ্যাকাউন্টের সাথে স্ট্যান্ডার্ড কোনও বকেয়া নেই</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>প্রযোজ্য নয়</td>");
                        sb.Append("<td style='padding:3px;'>এসটিডি 01.10.2022 থেকে</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='3' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr><td><b>8. এসএমএ / এনপিএ শ্রেণীবিভাগ</b><br>");
                        sb.Append("ঋণগ্রহণকারীর স্তরে এসএমএ / এনপিএ রিপোর্ট করা নিয়ামক নির্দেশিকা অনুযায়ী করা হয় এবং এইভাবে, ঋণগ্রহণকারীর যেকোনো একটি অ্যাকাউন্টে বকেয়া থাকার ফলে ঋণগ্রহণকারীকে এসএমএ বা এনপিএ যেটিই প্রযোজ্য হবে সেই হিসেবে রিপোর্ট করা হবে।");
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td style='height:5px;'></td></tr>");

                        sb.Append("<tr><td>9. অ্যাকাউন্ট আপগ্রেড করা:<br>");
                        sb.Append("এনপিএ হিসেবে শ্রেণীবিভাগ করা ঋণ অ্যাকাউন্টগুলিকে শুধুমাত্র তখনই ‘স্ট্যান্ডার্ড’ সম্পদ হিসেবে আপগ্রেড করা হয় যখন সমস্ত ঋণ সুবিধা সংক্রান্ত সুদ ও প্রিন্সিপালের সম্পূর্ণ বকেয়া ঋণগ্রহণকারী প্রদান করে থাকেন। পুনর্গঠন, বাণিজ্যিক কার্যকলাপ শুরুর তারিখ (ডিসিসিও)-এর কোনও অবদান না থাকা, ইত্যাদির কারণে এনপিএ হিসেবে শ্রেণীবিভাগ করা অ্যাকাউন্টগুলি আপগ্রেড করার ক্ষেত্রে, নির্দিষ্ট নিয়ামক সার্কুলার অনুযায়ী নির্দেশাবলী প্রযোজ্য হওয়া অব্যাহত থাকবে। আমি/আমরা এতদ্বারা নিশ্চিত করি যে উপরিউক্ত ব্যাখ্যাকে বিস্তীর্ণ হিসেবে গণ্য করা যাবে না এবং এগুলির প্রকৃতি হল সাধারণ পরিস্থিতিগুলিকে তুলে ধরা, এবং, উপরে উল্লেখিত বিষয়ে আরবিআই দ্বারা প্রদত্ত আইআরএসিপি নিয়ম ও শ্রেণীবিভাগগুলি বহাল থাকবে।");
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>भवदीय,</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>বিশ্বস্তভাবে, </td></tr>");
                        sb.Append("</table>");



                        #endregion
                        // End Consumer Education

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // DigitalDocSign
                        #region 35th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='https://bimaplan.bijliftt.com/DigiBengali.jpg' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td>&nbsp;</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;width:80%;'>");
                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.AppendFormat("<tr><td style='padding:2px;'>নাম: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["ApplicantName"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>ডিভাইস প্ল্যাটফর্ম: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["DevicePlatform"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>ব্রাউজার: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["Browser"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>ভৌগোলিক অবস্থান: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["GeoLocation"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>আইপি অ্যাড্রেস: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["IpAddress"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>মোবাইল নম্বর: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["MobileNo"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>ওটিপি পাঠানোর সময়: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["SMSSendTimeStamp"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>ওটিপি যাচাইকরণের সময়: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["SMSVerifyTimeStamp"]));
                        sb.Append("</table>");

                        sb.Append("</td>");
                        sb.Append("<td style='padding:2px;width:20%;vertical-align:top;'>");
                        sb.Append("<img src='https://dchot1.bijliftt.com:9000/jlginitialapproach/10213710789_AddressProofImage.png' alt='KYC-2 Applicant Front' height='210px !important' width='158px !important' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        #endregion

                        sb.Append("</body>");
                        sb.Append("</html>");

                        finalHtml = sb.ToString();

                        litPage1.Text = sb.ToString();

                        ApiCalling(finalHtml);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                    }
                    #endregion

                }
                else
                {
                    vFileName = "No File Created..As data Not found";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                ds = null;
                dtAppFrm1 = null; dtAppFrm2 = null; dtSancLetter = null;
                dtEMISchedule = null;
                dtLoanAgr = null; dtAuthLetter = null; dtKotak = null;
                oUsr = null; oMem = null;
                dsDigiDoc = null;
            }
            return vFileName;
        }

        protected void ApiCalling(string vHtml)
        {

            var requestBody = new
            {
                pFontName = "Nirmala",
                pHtml = vHtml
            };

            string apiUrl = "https://fttliveness.bijliftt.com/GetPdfFromHtml";
            string jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";

                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(jsonRequestBody);
                    writer.Close();
                }
                try
                {
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    HttpWebResponse sendLinkResponse = (HttpWebResponse)request.GetResponse();


                    string ResponseString = string.Empty;

                    using (StreamReader responseReader = new StreamReader(sendLinkResponse.GetResponseStream()))
                    {
                        ResponseString = responseReader.ReadToEnd();
                        LinkResponseBengali ResponseObject = JsonConvert.DeserializeObject<LinkResponseBengali>(ResponseString);

                        if (ResponseObject.status == "true")
                        {
                            string base64String = ResponseObject.PDFFile;

                            string outputPath = @"C:\DDHTML\OutputBengali.pdf";

                            byte[] pdfBytes = Convert.FromBase64String(base64String);
                            File.WriteAllBytes(outputPath, pdfBytes);
                        }

                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public string GetStringImage(string pImageName, string pId, string Module = "I")
        {
            string ActNetImage = ""; string imgString = "";

            try
            {
                string[] ActNetPath = Module == "I" ? InitialApproachURL.Split(',') : DigiDocOTPURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + (Module == "I" ? pId + "_" + pImageName : pImageName);
                    if (URLExist(ActNetImage))
                    {
                        imgString = ActNetImage;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return imgString;
        }

        public byte[] GetByteImage(string pImageName, string pId, string Module = "I")
        {
            string ActNetImage = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = Module == "I" ? InitialApproachURL.Split(',') : DigiDocOTPURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + (Module == "I" ? pId + "_" + pImageName : pImageName);
                    if (URLExist(ActNetImage))
                    {
                        imgByte = DownloadByteImage(ActNetImage);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return imgByte;
        }

        private bool URLExist(string pPath)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pPath);
                request.Timeout = 5000;
                using (WebResponse response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public byte[] DownloadByteImage(string URL)
        {
            byte[] imgByte = null;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var wc = new WebClient())
                {
                    imgByte = wc.DownloadData(URL);
                }
            }
            finally { }
            return imgByte;
        }
        
    }

    public class LinkResponseBengali
    {
        public string Message { get; set; }
        public string PDFFile { get; set; }
        public string status { get; set; }

    }
}