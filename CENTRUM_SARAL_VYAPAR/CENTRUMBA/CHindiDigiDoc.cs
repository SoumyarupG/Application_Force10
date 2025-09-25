using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Text;
using System.Net;

namespace CENTRUMBA
{
    public class CHindiDigiDoc
    {
        string InitialApproachURL = ConfigurationManager.AppSettings["InitialApproachURL"];
        string DigiDocOTPURL = ConfigurationManager.AppSettings["DigiDocOTPURL"];
        string HindiDocHeader = ConfigurationManager.AppSettings["HindiDocHeader"];

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

        public string GetDigitalDocs(string pLoanAppNo, Int64 vDigiDocDtlsId, Int32 vUserId)
        {
            DataSet ds = null;
            DataSet dsDigiDoc = null;
            DataTable dtAppFrm1 = null, dtAppFrm2 = null, dtSancLetter = null, dtEMISchedule = null;
            DataTable dtLoanAgr = null, dtAuthLetter = null, dtKotak = null, dtDigiDocDtls = null;
            DataTable dtScheduleOfCharges = null, dtSchedule = null, dtScheduleOwn = null; string finalHtml = "";
            string IDProofImage = "", IDProofImageBack = "", AddressProofImage = "", AddressProofImageBack = "", AddressProofImage2 = "", AddressProofImage2Back = "";
            CReports oRpt = new CReports();
            string vFileName = "No File Created";
            CDigiDoc oUsr = null;
            CMember oMem = null;
            CApiCalling oAC = new CApiCalling();

            try
            {
                oUsr = new CDigiDoc();
                oMem = new CMember();

                DataTable dt = new DataTable();

                ds = new DataSet();
                ds = oRpt.GetDigitalDocs(pLoanAppNo, 0, vUserId);
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
                //AuditImage = GetStringImage(vLoanAppId + ".jpg", vLoanAppId, "O");

                oMem = new CMember();
                dtScheduleOfCharges = oMem.GetScheduleofChargesByLnAppId(pLoanAppNo, vDigiDocDtlsId);
                oMem = new CMember();
                dtSchedule = oMem.GetSanctionLetterDtlByLnAppId(pLoanAppNo, vDigiDocDtlsId);


                if (dtAppFrm1.Rows.Count > 0)
                {
                   
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
                        sb.Append("body { font-family: 'Nirmala' !important; }");
                        sb.Append(".page-break { page-break-after: always; }");
                        sb.Append(".xsmall_font { font-size: 12pt !important; }");
                        sb.Append(".small_font { font-size: 14pt !important; }");
                        sb.Append(".normal_font { font-size: 16pt !important; }");
                        sb.Append(".large_font { font-size: 18pt !important; }");
                        sb.Append("</style>");
                        sb.Append("</head>");
                        sb.Append("<body>");


                        #region 1st_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%; border-collapse:collapse;' border='1'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px; width:50%;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>ऋण का प्रकार: {0}</li>", "अरक्षित ऋण");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px; width:50%;'>");
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
                        sb.AppendFormat("<li>ब्याज दर: {0}</li>", Convert.ToString(Math.Round(Convert.ToDouble(dtAppFrm1.Rows[0]["ApplLoanRate"]), 2)));
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

                        // th
                        sb.Append("<tr>");
                        sb.Append("<th colspan='2' class='small_font' style='text-align:center; padding:10px; background-color:#f2f2f2;'>आवेदक का विवरण</th>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>आवेदक का पूरा नाम: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["FullName"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>जन्म तिथि: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplDOB"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>लिंग: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["Gender"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>वैवाहिक स्थिति: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplMaritalName"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>शैक्षणिक स्तर : {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplQualification"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>व्यवसाय का प्रकार: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplOccupation"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>व्यावसायिक गतिविधि: {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>आय का स्रोत: {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>कुल कारोबार (वार्षिक): {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["AnnualIncome"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>श्रेणी: {0}</li>", "एमईएल सरल व्यापार");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>धर्म: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplReligion"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>दिव्यांग: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["IsAbledYN"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>श्रेणी: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["SpeciallyAbled"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // th
                        sb.Append("<tr>");
                        sb.Append("<th colspan='2' class='small_font' style='text-align:center; padding:10px; background-color:#f2f2f2;'>सह-आवेदक का विवरण</th>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>सह-आवेदक का पूरा नाम: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoFullName"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>जन्म तिथि: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplDOB"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>लिंग: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoGender"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>वैवाहिक स्थिति: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplMaritalName"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>शैक्षणिक स्तर : {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplQualification"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>व्यवसाय का प्रकार: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplOccupation"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>व्यावसायिक गतिविधि: {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>आय का स्रोत: {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>कुल कारोबार (वार्षिक): {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>श्रेणी: {0}</li>", "एमईएल सरल व्यापार");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>धर्म: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplReligion"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");



                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;background-color:#d9d9d9;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.Append("<li><b>आवेदक (केवाईसी विवरण)</b></li>");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;background-color:#d9d9d9;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.Append("<li><b>सह-आवेदक (केवाईसी विवरण)</b></li>");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>केवाईसी का प्रकार: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["IDType1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>केवाईसी का प्रकार: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoIDType1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>केवाईसी क्र: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["IDNo1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>केवाईसी क्र: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoIDNo1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;word-wrap:break-word; white-space:normal; max-width:300px;vertical-align:top;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>आवेदक का स्थायी पता: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplPAddress1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;word-wrap:break-word; white-space:normal; max-width:300px;vertical-align:top;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>सह-आवेदक का स्थायी पता: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplPAddress1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;word-wrap:break-word; white-space:normal; max-width:300px;vertical-align:top;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>आवेदक का पता: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplCAddress1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;word-wrap:break-word; white-space:normal; max-width:300px;vertical-align:top;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>सह-आवेदक का पता: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplCAddress1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>पिन कोड: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplPPinCode"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>पिन कोड: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplPPinCode"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>आवास स्वामित्व: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplResidentialStatR"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>आवास स्वामित्व: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["CoApplResidentialStatR"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // th
                        sb.Append("<tr>");
                        sb.Append("<th colspan='2' class='small_font' style='text-align:center; padding:10px; background-color:#f0f0f0;'>व्यवसाय का विवरण</th>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>व्यवसाय का पता: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplBusiAddress1"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>पिन कोड: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplBusiPincode"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>पिन कोड: {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>मोबाइल नं: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["ApplBusiMobile"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>अतिरिक्त संपर्क क्र.: {0}</li>", "");
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
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%; border-collapse:collapse;' border='1'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px; width:55%;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>A. व्यवसाय संदर्भ 1 {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px; width:45%;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>B. व्यवसाय संदर्भ 2 {0}</li>", "");
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>नाम: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["BR1Name"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>नाम: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["BR2Name"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>संपर्क क्र.: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["BR1ContactNo"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>संपर्क क्र.: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["BR2ContactNo"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px; word-wrap:break-word; white-space:normal; max-width:300px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>पता: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["BR1PlaceOfOffice"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px; word-wrap:break-word; white-space:normal; max-width:300px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>पता: {0}</li>", Convert.ToString(dtAppFrm1.Rows[0]["BR2PlaceOfOffice"]));
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // th
                        sb.Append("<tr>");
                        sb.Append("<th colspan='2' class='small_font' style='text-align:center; padding:10px; background-color:#f0f0f0;'>बैंकिंग और मौजूदा ऋण का विवरण</th>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        #region Bank_Details
                        sb.Append("<h3 style='padding-top:20px;text-decoration: underline;'><strong>बैंकिंग:</strong></h3>");
                        sb.Append("<table  class='small_font' style='border-collapse: collapse; width: 100%; text-align: left;' border='1' >");
                        // Add header row
                        sb.Append("<tr>");
                        sb.Append("<th style='padding: 4px;'>खाता धारक(आवेदक)</th>");
                        sb.Append("<th style='padding: 4px;'>बैंक का नाम और शाखा</th>");
                        sb.Append("<th style='padding: 4px;'>खाता क्र.</th>");
                        sb.Append("<th style='padding: 4px;'>चालू/बचत</th>");
                        sb.Append("<th style='padding: 4px;'>आईएफएससी कोड</th>");
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
                        sb.Append("<h3 style='padding-top:20px;text-decoration: underline;'><strong>मौजूदा ऋण :</strong></h3>");
                        sb.Append("<table  class='small_font' style='border-collapse: collapse; width: 100%;  text-align: left;' border='1' >");
                        // Add header row
                        sb.Append("<tr>");
                        sb.Append("<th style='padding: 4px;'>ऋण देने वाली संस्था का नाम</th>");
                        sb.Append("<th style='padding: 4px;'>ऋण का उद्देश्य</th>");
                        sb.Append("<th style='padding: 4px;'>ऋण राशि</th>");
                        sb.Append("<th style='padding: 4px;'>ऋण की अवधि(महीने)</th>");
                        sb.Append("<th style='padding: 4px;'>मासिक किश्त</th>");
                        sb.Append("<th style='padding: 4px;'>वर्तमान बकाया राशि</th>");
                        sb.Append("<th style='padding: 4px;'>शेष अवधि (महीने)</th>");
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
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr><td style='padding-top:5px;'><strong>घोषणा/सहमति</strong></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:1px; width:100%;'>");
                        sb.Append("<ol>");
                        sb.Append("<li>कर्जदार घोषित करता है /करते हैं कि आवेदनपत्र में दिए गए सभी विवरण एवं जानकारियाँ सही, सत्य और संपूर्ण हैं और कोई भी जानकारी छिपाई/दबाई नहीं गई है।</li>");
                        sb.Append("<li>कर्जदार वचन देता है/देते हैं कि प्रस्तुत की गई जानकारी में किसी भी बदलाव के बारे में वह/वे तुंरत यूनिटी स्मॉल फाइनेंस बैंक को सूचित करेगा (करेगी)/ करेंगे। इसके साथ-साथ, यूनिटी स्मॉल फाइनेंस बैंक और/या इसकी समूह कंपनियों और/या इसके एजेंटों के लिए आवश्यक किसी भी अन्य जानकारी/दस्तावेज को उपलब्ध कराने का भी कर्जदार वचन देता है /देते हैं।</li>");
                        sb.Append("<li>कर्जदार यह घोषित करता है / करते हैं कि निधि का उपयोग बताए गए उद्देश्य के लिए किया जाएगा और इसका उपयोग सट्टे या असामाजिक उद्देश्य के लिए नहीं किया जाएगा।</li>");
                        sb.Append("<li>कर्जदार को यह पता हैं कि यूनिटी स्मॉल फाइनेंस बैंक और/या इसकी समूह कंपनियों के पास इस आवेदन के साथ जमा की गई तस्वीरों और दस्तावेजों को अपने पास बनाए रखने का अधिकार सुरक्षित है।</li>");
                        sb.Append("<li>कर्जदार को यह पता हैं कि इस ऋण का मंजूर किया जाना यूनिटी स्मॉल फाइनेंस बैंक के विवेक और यूनिटी स्मॉल फाइनेंस बैंक द्वारा आवश्यक अनिवार्य दस्तावेज़ों और अन्य औपचारिकताओं के निष्पादन पर निर्भर करता है।  इसके अलावा, यूनिटी स्मॉल फाइनेंस बैंक के पास इस आवेदन को अस्वीकृत करने (कर्जदार को सूचित करके या किए बिना) का अधिकार सुरक्षित है, और यह कि इस प्रकार की अस्वीकृति या इस प्रकार की अस्वीकृति के बारे में मुझे/हमें सूचित करने के लिए यूनिटी स्मॉल फाइनेंस बैंक किसी भी प्रकार से ज़िम्मेदार / जवाबदेह नहीं होगी। </li>");
                        sb.Append("<li>यूनिटी स्मॉल फाइनेंस बैंक द्वारा अपनी संबद्ध कंपनियों/ सरकारी/विनियामक संस्थाओं, क्रेडिट ब्यूरो/रेटिंग एजेंसियों, सेवा प्रदाताओं, बैंक/वित्तीय संस्थानों, सीकेवाईसी रजिस्ट्री, थर्ड पार्टी / तीसरे पक्षों और इस प्रकार के अन्य प्राधिकरणों, जैसा आवश्यक हो, के साथ केवाईसी जानकारी का आदान-प्रदान करने या साझा करने पर कर्जदार(रों) को किसी प्रकार की आपत्ति नहीं होगी।</li>");
                        sb.Append("<li>कर्जदार अभिस्वीकृति देते हैं कि भारतीय विशिष्ट पहचान प्राधिकरण, क्रेडिट इन्फॉर्मेशन ब्यूरो ऑफ इंडिया लि. और अन्य संस्थाओं सहित तीसरे पक्षों से कर्जदार का केवाईसी और क्रेडिट/ऋण संबंधी जानकारी/दस्तावेज प्राप्त करने के लिए कर्जदार द्वारा यूनिटी स्मॉल फाइनेंस बैंक को सहमति प्रदान की गई है, और इसके साथ ही यह सहमति भी दी गई है कि समय-समय पर कर्जदार की वास्तविकता और/या ऋण पात्रता का पता लगाने के लिए यूनिटी स्मॉल फाइनेंस बैंक, स्वयं या प्राधिकृत व्यक्तियों के माध्यम से, दी गई किसी भी जानकारी का सत्यापन, क्रेडिट संदर्भों की जांच, रोज़गार विवरण और केवाईसी संबंधी दस्तावेज और ऋण रिपोर्ट प्राप्त कर सकती है।   इसके साथ ही कर्जदार अभिस्वीकृति देता है कि यूनिटी स्मॉल फाइनेंस बैंक के साथ कर्जदार के संबंध में जानकारी साझा करने के लिए भारतीय विशिष्ट पहचान प्राधिकरण (यूआईडीएआई) या इस प्रकार की अन्य किसी भी तीसरे पक्ष को स्वेच्छा से सहमति प्रदान की गई है।  इसके साथ ही, कर्जदार यह अभिस्वीकृति भी देता है / देते हैं कि यूआईडीएआई के माध्यम से सत्यापन होने पर जो जानकारी साझा की जा सकती है, उसका स्वरूप समझाया गया है और यह कि इस प्रकार की जानकारी का उपयोग यूनिटी स्मॉल फाइनेंस बैंक से ऋण सुविधा का लाभ लेने के लिए किया जाएगा। </li>");
                        sb.Append("<li>कर्जदार एतदद्वारा उपरोक्त पंजीकृत नंबर/ईमेल पते पर एसएमएस/ईमेल के माध्यम से केंद्रीय केवाईसी रजिस्ट्री से जानकारी प्राप्त करने के लिए सहमति प्रदान करता है / करते हैं।</li>");
                        sb.Append("<li>कर्जदार पुष्टि करता है / करते हैं कि ऋण पर प्रयोज्य ब्याज दर और अन्य प्रभार को भविष्यलक्षी प्रभाव से बदलना यूनिटी स्मॉल फाइनेंस बैंक के विवेकाधीन होगा।</li>");
                        sb.Append("<li>ऐसे मामलों में जहाँ एक से अधिक कर्जदार हैं, प्रत्येक कर्जदार स्वीकृति देता है और वचन देता है कि प्रत्येक कर्जदार ऋण के अंतर्गत भुगतान करने के लिए संयुक्त रूप से और पृथक रूप से ज़िम्मेदार है।</li>");
                        sb.Append("<li>कर्जदार समझता है/ समझते हैं कि किसी भी बीमा उत्पाद को खरीदना शुद्ध रूप से स्वैच्छिक है और यूनिटी स्मॉल फाइनेंस बैंक की ओर से किसी अन्य सुविधा का लाभ मिलने से नहीं जुड़ा है।</li>");
                        sb.Append("<li>कर्जदार यह प्रदर्शित करता है/ करते हैं कि उसके निदेशकों/भागीदारों (यदि कोई हो) को दिवालिया घोषित नहीं किया गया है, और ना ही उनके विरोध में दिवालिएपन की कोई कार्यवाही शुरू की गई है।</li>");
                        sb.Append("<li>यूनिटी स्मॉल फाइनेंस बैंक और/या इसकी समूह कंपनियों और/या इसके एजेंटों की ओर से मुझे/हमें यूनिटी स्मॉल फाइनेंस बैंक और/या इसकी समूह कंपनियों द्वारा प्रदान किए जाने वाले विभिन्न उत्पादों, योजनाओं और सेवाओं की किसी भी माध्यम से (टेलिफोन कॉल, एमएमएस/ईमेल, पत्र, इत्यादि सहित) जानकारी दी जाने तथा उपरोक्त उद्देश्य के लिए यूनिटी स्मॉल फाइनेंस बैंक और/या इसकी समूह कंपनियों और/या इसके एजेंटों को प्राधिकृत किये जाने पर कर्जदार को कोई आपत्ति नहीं हैं|  मैं पुष्टि करता/करती हूँ कि मुझे दी गई इस प्रकार की जानकारी/संचार के लिए भारतीय दूरसंचार विनियामक प्राधिकरण द्वारा निर्धारित “नेशनल डू नॉट कॉल रजिस्ट्री” में संदर्भित अवांछित संचार के संबंध में कानून लागू नहीं होंगे।</li>");
                        sb.Append("<li>ऋण करार और किसी भी अन्य संबंधित दस्तावेज ईलेक्ट्रॉनिक हस्ताक्षर/ई-साइनिंग के माध्यम से निष्पादित करने के लिए कर्जदार ने सहमति प्रदान की है, जिसमें अन्य चीजों के साथ-साथ आधार कार्ड आधारित ई-साइन और पंजीकृत मोबाइल क्रमांक पर प्राप्त होनेवाला वन टाइम पासवर्ड (ओटीपी) भी शामिल हैं। यह सुनिश्चित करना संपूर्ण रूप से कर्जदार की ज़िम्मेदारी होगी कि ओटीपी से कोई छेड़छाड़ नहीं की जाएं या इसे किसी गैर-प्राधिकृत उपयोगकर्ता के साथ साझा नहीं किया जाएं।ईलेक्ट्रॉनिक हस्ताक्षर के उपयोग से उत्पन्न होने वाले सभी रिकॉर्ड/अभिलेख लेन-देन की प्रामाणिकता और सटीकता के निर्णायक सबूत होंगे और कर्जदार के लिए बाध्यकारी होंगे।</li>");
                        sb.Append("<li>उपरोक्त सामग्री मुझे/हमें मेरी/हमारी स्थानीय भाषा में पढ़कर सुनाई और समझाई गई है।</li>");
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
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='font-weight:bold; text-align:center; padding:10px;'>");
                        sb.Append("यूनिटी स्मॉल फाइनेंस बैंक लिमिटेड/ अन्य बैंकों के साथ संबंध पर ग्राहक की घोषणा");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // Question (a)
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<b>(a)</b> क्या आवेदक बैंक का निदेशक/बैंक के निदेशक का रिश्तेदार या बैंक का वरिष्ठ अधिकारी या बैंक के वरिष्ठ अधिकारी का रिश्तेदार है?");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>☐ हाँ &nbsp;&nbsp;&nbsp;☑ नहीं</td>");
                        sb.Append("</tr>");

                        // Question (b)
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<b>(b)</b> क्या आवेदक अन्य बैंक का निदेशक/अन्य बैंक के निदेशक का रिश्तेदार है?");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>☐ हाँ &nbsp;&nbsp;&nbsp;☑ नहीं</td>");
                        sb.Append("</tr>");

                        // Question (c)
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<b>(c)</b> क्या आवेदक एक फर्म/कंपनी है जिसमें अन्य बैंकों के निदेशक/किसी अन्य बैंक के निदेशकों के रिश्तेदार भागीदार/गारंटर/निदेशक के रूप में रुचि रखते हैं/नियंत्रण में हैं*/प्रमुख शेयरधारक हैं?");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>☐ हाँ &nbsp;&nbsp;&nbsp;☑ नहीं</td>");
                        sb.Append("</tr>");

                        // Question (d)
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<b>(d)</b> क्या आवेदक एक फर्म/कंपनी है जिसमें बैंक के निदेशक या बैंक के निदेशक के रिश्तेदार या बैंक के वरिष्ठ अधिकारी या उनके रिश्तेदार भागीदार/प्रबंधक/कर्मचारी/गारंटर/निदेशक/नियंत्रण*/प्रमुख शेयरधारक के रूप में रुचि रखते हैं?");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>☐ हाँ &nbsp;&nbsp;&nbsp;☑ नहीं</td>");
                        sb.Append("</tr>");

                        // Footnote
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:10px;'>");
                        sb.Append("* “नियंत्रण” शब्द में निदेशकों के बहुमत को नियुक्त करने या किसी व्यक्ति या व्यक्तियों द्वारा व्यक्तिगत रूप से या सामूहिक रूप से, प्रत्यक्ष या अप्रत्यक्ष रूप से, जिसमें उनके शेयरधारिता या प्रबंधन अधिकार या शेयरधारक समझौते या मतदान समझौते या किसी अन्य तरीके से शामिल हैं, द्वारा प्रयोग किए जाने वाले प्रबंधन या नीतिगत निर्णयों को नियंत्रित करने का अधिकार शामिल होगा।<br/><br/>");
                        sb.Append("** “प्रमुख शेयरधारक” शब्द का अर्थ है अनुसूचित सहकारी बैंकों के निदेशकों, यूनिटी स्मॉल फाइनेंस बैंक या किसी अन्य बैंक द्वारा स्थापित म्यूचुअल फंड/वेंचर कैपिटल फंड की सहायक कंपनियों/ट्रस्टी के निदेशकों सहित चुकता शेयर पूंजी का 10% या उससे अधिक या चुकता शेयरों में पाँच करोड़ रुपये, जो भी कम हो, रखने वाला व्यक्ति।<br/><br/>");
                        sb.Append("उधारकर्ता समझता है और सहमत है कि यदि उधारकर्ता द्वारा की गई उपरोक्त घोषणा झूठी पाई जाती है, तो बैंक ऋण सुविधा को रद्द करने और/या वापस लेने का हकदार होगा।");
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
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%;border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align:center; font-weight:bold; padding:10px;'>");
                        sb.Append("केवाईसी तस्वीरें");
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
                        sb.Append("<td style='text-align:center; padding:5px;'>केवाईसी-1 (आवेदक) सामने का भाग</td>");
                        sb.Append("<td style='text-align:center; padding:5px;'>केवाईसी-1 पीछे का भाग</td>");
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
                        sb.Append("<td style='text-align:center; padding:5px;'>केवाईसी-2 (आवेदक) सामने का भाग</td>");
                        sb.Append("<td style='text-align:center; padding:5px;'>केवाईसी-2 पीछे का भाग</td>");
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
                        sb.Append("<td style='text-align:center; padding:5px;'>केवाईसी-1 (सह-आवेदक) सामने का भाग</td>");
                        sb.Append("<td style='text-align:center; padding:5px;'>केवाईसी-1 पीछे का भाग</td>");
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
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;font-weight:bold;'>संस्वीकृति पत्र</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>दिनांक: {0}</td></tr>", Convert.ToString(dtSancLetter.Rows[0]["FinalSanctionDate"]));
                        sb.AppendFormat("<tr><td style='width:100%;'>श्री/श्रीमती: {0}</td></tr>", Convert.ToString(dtSancLetter.Rows[0]["CompanyName"]));
                        sb.AppendFormat("<tr><td style='width:100%;'>पता: {0}</td></tr>", Convert.ToString(dtSancLetter.Rows[0]["ComAddress"]));
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td><hr></td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>विषय: एमईएल सरल व्यापार आवेदन क्र. <span style='border-bottom: 1px dotted black;'>{0}</span> के लिए संस्वीकृति पत्र</td></tr>", Convert.ToString(dtSancLetter.Rows[0]["SanctionId"]));
                        sb.Append("<tr><td style='padding:10px;'>आवेदन के विषय के संदर्भ में, यूनिटी स्मॉल फाइनेंस बैंक (‘ऋणदाता’ जिस अभिव्यक्ति में, जब तक कि यह संदर्भ या अर्थ के प्रतिकूल न हो, इसके उत्तराधिकारी और समनुदेशिती शामिल समझे जाएंगे) को ऋण के लिए स्वीकृति प्रदान करते हुए प्रसन्नता हो रही है जो नीचे विस्तापूर्वक दिए गए नियमों और शर्तों के अधीन है:</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;width:35%;'>विवरण</td>");
                        sb.Append("<td  style='padding:5px;width:65%;'>वर्णन</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>कर्जदार का नाम</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:left;'>{0}</td>", Convert.ToString(dtSancLetter.Rows[0]["CompanyName"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>सह-कर्जदार का नाम</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:left;'>{0}</td>", Convert.ToString(dtSancLetter.Rows[0]["CoAppName"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>सुविधा का प्रकार</td>");
                        sb.Append("<td  style='padding:5px;'>ऋण की मियाद</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>स्वीकृत ऋण राशि</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:left;'>रू. {0}</td>", Convert.ToString(dtSancLetter.Rows[0]["SanctionAmt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>ऋण अवधि</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:left;'>{0} महीने</td>", Convert.ToString(dtSancLetter.Rows[0]["Tenure"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>ब्याज का प्रकार</td>");
                        sb.Append("<td  style='padding:5px;'>घटती ब्याज दर</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>ब्याज दर</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:left;'>{0} % प्रति वर्ष <br/> निम्नलिखित में से कुछ भी घटित होने की स्थिति में यूएसएफबी के पास ब्याज दर फिर से निर्धारित करने का अधिकार होगा: 1. भुगतान में चूक होना; या 2| कोई अन्य घटना जिसके कारण ऋणदाता के विचार में उनकी आंतरिक नीति के अनुसार ब्याज दर फिर से तय करने की आवश्यकता है।</td>", Convert.ToString(Math.Round(Convert.ToDouble(dtSancLetter.Rows[0]["RIntRate"]), 2)));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>समान मासिक किश्तों (ईएमआई) की संख्या</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:left;'>{0}</td>", Convert.ToString(dtSancLetter.Rows[0]["Tenure"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>ईएमआई</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:left;'>मासिक रूप से / वार्षिक रूप से रू. {0} देय</td>", Convert.ToString(dtSancLetter.Rows[0]["EMI"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>ईएमआई की तारीख</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:left;'>हर महीने/तिमाही का {0} दिन</td>", "5th");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>प्रसंस्करण शुल्क</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:left;'>रू. {0} वापस न करने योग्य, अग्रिम तौर पर देय</td>", Convert.ToString(dtSancLetter.Rows[0]["ProcFees"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>भुगतान में देरी / दंडात्मक प्रभार</td>");
                        sb.Append("<td  style='padding:5px;'>(बोल्ड में होना चाहिए)</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 7th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;'>अनुसूची I</td></tr>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;'>महत्वपूर्ण नियमों और शर्तों के उल्लंघन पर दंडात्मक प्रभार का विवरण </td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;width:20%;'>अनुक्रमांक</td>");
                        sb.Append("<td  style='padding:5px;width:50%;'>महत्वपूर्ण नियम और शर्तें</td>");
                        sb.Append("<td  style='padding:5px;width:50%;'>दंडात्मक प्रभार</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>1.</td>");
                        sb.Append("<td  style='padding:5px;'>बैंक को भुगतान की जाने वाली देय राशि (मूलधन, ब्याज, लागत, प्रभार, कर, व्यय इत्यादि सहित) की चुकौती में चूक या देरी होने पर</td>");
                        sb.Append("<td  style='padding:5px;'>3% प्रति माह (36% प्रति वर्ष) + बकाया दिनों के हिसाब से अतिदेय राशि पर 18% जीएसटी</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>2.</td>");
                        sb.Append("<td  style='padding:5px;'>भुगतान में चूक घटित होने पर</td>");
                        sb.Append("<td  style='padding:5px;'>न्यूनतम रू. 500/- (भारतीय रूपए पाँच सौ मात्र) और  अधिकतम रू. 700/- (भारतीय रूपए सात सौ मात्र)</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td style='padding:5px;'>");
                        sb.Append("<ul>");
                        sb.Append("<li>उपरोक्त दंडात्मक प्रभार लागू कर के अधीन होंगे।</li>");
                        sb.Append("<li>अन्य सभी प्रभारों का उसी प्रकार से लागू होना जारी रहेगा जैसा ऋण करार के अंतर्गत विशिष्ट रूप से निर्दिष्ट किया गया है।</li>");
                        sb.Append("<li>कर्जदार को पता है कि अनुसूची, नियम और शर्तें, प्रभार वर्तमान में प्रचलित हैं और बदले जा सकते हैं और/या नवीन/नई शर्तें किसी भी समय या समय-समय पर जोड़े जा सकते हैं, जो संपूर्ण रूप से बैंक स्व-विवेक पर निर्भर करते हैं और इस प्रकार के बदलाव कर्जदार पर बंधनकारक होते हैं। विनियामक प्रकटीकरण - नीतियाँ, इस खंड के अंतर्गत प्रभार की अद्यतन अनुसूची बैंक की वेबसाइट www.theunitybank.com पर प्रदर्शित की जाएगी</li>");
                        sb.Append("</ul>");
                        sb.Append("</td></tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>सभी नियमों और शर्तों / सामान्य शर्तों और खंडों को, जो इस पत्र के एक भाग को गठित करते हैं, यथोचित परिवर्तनों सहित ऋण करार के साथ सामंजस्य में पढ़ना होगा। उन सभी शब्दों को, जिन्हें यहाँ विशिष्ट रूप से नहीं रखा गया है और जो ऋण करार का भाग हैं, इस पत्र के साथ सामंजस्य में पढ़ा जाएगा जैसा कि स्वीकृति पत्र, ऋण करार, एमआईटीसी और इस लेन-देन से तैयार किए गए दस्तावेज़ों का अर्थ लेन-देन दस्तावेज के रूप में समझा जाएगा।</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>कर्जदार यह समझता है/ समझते हैं कि ऋणदाता ने जोखिम आधारित कीमत को अंगीकृत किया है, जिसे ग्राहक प्रोफाइल, वित्तीय आंकड़े, निधि का स्रोत, ग्राहक का जोखिम प्रोफाइल, ऋण का स्वरूप इत्यादि जैसे व्यापक मापदंडों को विचार में लिए जाने के बाद तय किया जाता है और इसलिए विभिन्न कर्जदारों के लिए ब्याज दर अलग-अलग हो सकती है।</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>निरसन या समाप्ति: उपलब्धता की अवधि के दौरान, यदि भुगतान में चूक या संभाव्य रूप से भुगतान में चूक की स्थिति उत्पन्न हुई है या ऋणदाता के लिए कर्जदार को ऋण संवितरण करना या सुविधाएं जारी रखना ग़ैर-कानूनी हो जाता है तो ऋणदाता अपने स्व-विवेक के आधार पर, सुविधाओं को रद्द कर सकता है।  इसके आगे, कर्जदार बिना शर्त स्वीकृति, वचन और अभिस्वीकृति देता है कि ऋणदाता के पास अप्रतिबंधित अधिकार है कि वह इस प्रकार के निरसन के लिए कर्जदार को किसी भी प्रकार की पूर्व-सूचना दिए बिना उस सुविधा/ऋण की वैधता अवधि के दौरान किसी भी समय उस सुविधा का वह हिस्सा आंशिक या सपूर्ण रूप से निरस्त कर सकता हैं जिसका उपयोग ना किया गया हो।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>लागू कानूनों के अंतर्गत प्रभारित जीएसटी, शिक्षा उपकर आदि के साथ-साथ सभी अप्रत्यक्ष कर, शुल्क और प्रभार आदि का, समय समय पर जैसा संशोधित किया गया हो, कर्जदार द्वारा अतिरिक्त रूप से वहन किया जाएगा।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>कृपया नोट करें कि आय कर, जीएसटी या किसी भी लागू कर कानूनों में बदलाव के कारण किश्तों में योग्य संशोधन किया जाएगा। अन्य सभी नियम और शर्तें ऋणदाता के साथ निष्पादित किए जाने वाले ऋण करार(रों) के अनुसार होंगे।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>सुविधाएं, जब तक कि स्पष्ट रूप से इसके प्रतिकूल ना कहा गया हो, मांगे जाने पर पुन:देय / निर्धारण करने योग्य हैं और किसी भी समय ऋणदाता द्वारा समीक्षा के अधीन हैं।ऋणदाता अपने स्व-विवेक पर पूर्वोक्त अवधि के आगे सुविधाओं को जारी रखने / नवीकरण करने का निर्णय कर सकता है।  </td></tr>");
                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 8th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>कर्जदार को 30 दिनों के भीतर इस संस्वीकृति पत्र को स्वीकार करके हस्ताक्षरित प्रति सौंपनी होगी, यदि ऋणदाता द्वारा समय-विस्तार ना किया गया हो।</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td>&nbsp;</td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>कृपया नोट करें कि संस्वीकृति के नियम और शर्तें संस्वीकृति पत्र की तारीख से 30 दिनों की अवधि तक वैध हैं और यदि वैधता अवधि के भीतर दस्तावेजीकरण और आहरण द्वारा कमी पूरी ना की गई हो, तो ऋणदाता के पास ब्याज दर और संस्वीकृति के किसी भी अन्य नियम और शर्तों को संशोधित करने, या अपने स्व-विवेक पर संस्वीकृति को रद्द समझे जाने का अधिकार सुरक्षित है।</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>पूर्वोक्त ऋण की संस्वीकृति निम्नलिखित शर्तों के अधीन होगी:</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;'>");
                        sb.Append("<ul>");
                        sb.Append("<li>ऋण का उपयोग केवल उसी उद्देश्य के लिए किया जाना चाहिए जैसा कर्जदार द्वारा आवेदन प्रपत्र और निधि के अंतिम उपयोग की घोषणा में दर्शाया गया है और इसका उपयोग किसी भी अन्य उद्देश्य के लिए नहीं किया जा सकता है।</li>");
                        sb.Append("<li>णदाता के साथ ऋण करार और अन्य दस्तावेज़ों का निष्पादन ऋणदाता की नीति और प्रारूप के अनुसार किया गया है। </li>");
                        sb.Append("</ul>");
                        sb.Append("</td></tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>हम आपके साथ लंबी अवधि तक संबंध बनाये रखने की उम्मीद करते हैं।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>धन्यवाद</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>भवदीय</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>यूनिटी स्मॉल फाइनेंस बैंक के लिए</td>");
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
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;'>प्रमुख तथ्य विवरण (केएफएस)</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>दिनांक: {0}</td>", Convert.ToString(dtSancLetter.Rows[0]["FinalSanctionDate"]));
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='width:100%;' >विनियमित संस्था/ ऋण दाता का नाम: यूनिटी स्मॉल फाइनेंस बैंक लिमिटेड</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>उधारकर्ता का नाम और पता: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["MemberName"]));
                        sb.Append("</tr>");
                        sb.Append("<tr><td>&nbsp;</td></tr>");
                        sb.Append("<tr><td style='font-weight:bold;text-align:center;width:100%;'>भाग 1 (ब्याज दर और फीस/शुल्क)</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td style='padding:5px; width:5%;text-align:center;'>1</td>");
                        sb.Append("<td style='padding:5px; width:22%;'>ऋण प्रस्ताव / खाता क्र.</td>");
                        sb.AppendFormat("<td style='padding:5px; width:25%;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanNo"]));
                        sb.Append("<td style='padding:5px; width:22%;'>ऋण का प्रकार</td>");
                        sb.AppendFormat("<td colspan='4' style='padding:5px; width:26%;'>{0}</td>", "MEL Loan");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'>2</td>");
                        sb.Append("<td colspan='2' style='padding:5px;'> संस्वीकृत ऋण राशि ( रूपए में)</td>");
                        sb.AppendFormat("<td colspan='5' style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanAmt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'>3</td>");
                        sb.Append("<td colspan='3'>संवितरण अनुसूची<br/>(i) चरणों में संवितरण या 100% अग्रिम<br/>(ii) यदि यह चरणवार है, तो ऋण करार के उस खंड का उल्लेख करें जिसमें सुसंगत विवरण उपलब्ध है।</td>");
                        sb.AppendFormat("<td colspan='4' style='padding:5px;text-align:center;'>{0}</td>", "100% Upfront");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'>4</td>");
                        sb.Append("<td colspan='2' >4 ऋण की अवधि (वर्ष/महीने/दिन)</td>");
                        sb.AppendFormat("<td colspan='5' style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["Tenure"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'>5</td>");
                        sb.Append("<td colspan='7' style='text-align:center;' >किस्त का विवरण</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='text-align:center;' >किस्त का प्रकार</td>");
                        sb.Append("<td style='text-align:center;' >ईएमआई की संख्या</td>");
                        sb.Append("<td style='text-align:center;' >ईएमआई (₹)</td>");
                        sb.Append("<td colspan='3' style='text-align:center;' >पेमेंट की शुरूआत, स्वीकृति के बाद</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='3' style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["RepFreq"]));
                        sb.AppendFormat("<td  style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInstNo"]));
                        sb.AppendFormat("<td  style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["EMIAmt"]));
                        sb.AppendFormat("<td colspan='3' style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["FInstDt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'>6</td>");
                        sb.Append("<td colspan='3' style='text-align:center;' >ब्याज दर (%) और प्रकार (निश्चितया अस्थिर या हाइब्रिड)</td>");
                        sb.AppendFormat("<td colspan='4' style='padding:5px;text-align:center;'>{0}(Fixed)</td>", Convert.ToString(Math.Round(Convert.ToDouble(dtScheduleOwn.Rows[0]["IntRate"]), 2)));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'>7</td>");
                        sb.Append("<td colspan='7' style='text-align:center;' >ब्याज की अस्थिर दर के मामले में अतिरिक्त जानकारी </td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;' >कसौटी</td>");
                        sb.Append("<td style='text-align:center;' >बेंचमार्क दर (%) (बी)</td>");
                        sb.Append("<td style='text-align:center;' >फैलना दर प्रतिशतता (%) (एस)</td>");
                        sb.Append("<td style='text-align:center;' >अंतिम दर प्रतिशत (%) = (बी) + (एस)</td>");
                        sb.Append("<td colspan='2' style='text-align:center;' >पुनर्निधारण आवधिकता (महीने)</td>");
                        sb.Append("<td colspan='2' style='text-align:center;' >संदर्भ बेंचमार्क में परिवर्तन का प्रभाव (‘आर’में 25 आधार अंक परिवर्तन के लिए,ईएमआईमें परिवर्तन:)</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;' ><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' ><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' ><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' ><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' >बी <br><br> NA</td>");
                        sb.Append("<td style='text-align:center;' >एस <br><br> NA</td>");
                        sb.Append("<td style='text-align:center;' >ईएमआई (₹) <br> NA</td>");
                        sb.Append("<td style='text-align:center;' >ईएमआई (₹) <br> NA</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'>8</td>");
                        sb.Append("<td colspan='7' style='text-align:center;' >फीस/शुल्क</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");


                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 10th_Page

                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='padding:5px; width:25%;'></td>");
                        sb.Append("<td colspan='2' style='padding:5px; width:35%;'>विनियमित संस्था (आरई) (ए) के लिए देय</td>");
                        sb.Append("<td colspan='2' style='padding:5px; width:40%;'>विनियमित संस्था (आरई) (बी) के माध्यम से तृतीय पक्ष को देय</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'></td>");
                        sb.Append("<td style='padding:5px;text-align:center;'></td>");
                        sb.Append("<td style='padding:5px;text-align:center;'>एकबारगी/ आवर्ती </td>");
                        sb.Append("<td style='padding:5px;text-align:center;'>राशि (₹ में) याप्रतिशत (%) जैसा लागू हो</td>");
                        sb.Append("<td style='padding:5px;text-align:center;'>एकबारगी/ आवर्ती</td>");
                        sb.Append("<td style='padding:5px;text-align:center;'>राशि (₹ में) याप्रतिशत (%) जैसा लागू हो</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(i)</td>");
                        sb.Append("<td style='padding:5px;text-align:left;'>प्रोसेसिंग फीस</td>");
                        sb.Append("<td style='padding:5px;text-align:center;'><br/>One Time</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>(जीएसटी सहित)<br/>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["procfee"]));
                        sb.Append("<td style='padding:5px;text-align:center;'><br/>One Time</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>(जीएसटी सहित)<br/>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["procfeeBC"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(ii)</td>");
                        sb.Append("<td style='padding:5px;text-align:left;'>बीमा शुल्क</td>");
                        sb.Append("<td style='padding:5px;text-align:center;'><br/>One Time</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;' >(जीएसटी सहित)<br/>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInsurance"]));
                        sb.Append("<td style='padding:5px;text-align:center;'><br/>One Time</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>(जीएसटी सहित)<br/>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInsurance"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(iii)</td>");
                        sb.Append("<td  style='padding:5px;text-align:left;'>मूल्यांकन शुल्क </td>");
                        sb.Append("<td colspan='4' style='padding:5px;text-align:center;'>No Fees</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(iv)</td>");
                        sb.Append("<td style='padding:5px;text-align:left;'>कोई अन्य (कृपयास्पष्ट रूप से बताएं)</td>");
                        sb.Append("<td style='padding:5px;text-align:center;'></td>");
                        sb.Append("<td style='padding:5px;' ></td>");
                        sb.Append("<td style='padding:5px;text-align:center;'></td>");
                        sb.Append("<td style='padding:5px;'></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'></td>");
                        sb.Append("<td style='padding:5px;text-align:left;'>कुल शुल्क</td>");
                        sb.Append("<td style='padding:5px;text-align:center;'>One Time</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;' >{0}</td>", Convert.ToString(vTotalPayRE));
                        sb.Append("<td style='padding:5px;text-align:center;'>One Time</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(vTotalPayTP));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>9</td>");
                        sb.Append("<td colspan='2' style='padding:5px;text-align:center;'>वार्षिक प्रतिशत दर (एपीआर) (%)</td>");
                        sb.AppendFormat("<td colspan='3' style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["IRRIC"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>10</td>");
                        sb.Append("<td colspan='5' style='padding:5px;text-align:center;'>आकस्मिक शुल्कका ब्यौरा ( ₹ या % में, जैसा लागू हो) **</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(i)</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>दंडात्मक शुल्क, यदि कोई हो, भुगतान में देरी के मामले में</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "3% + 18% GST");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(ii)</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>अन्य दंडात्मक शुल्क, यदि कोई हो</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>A</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>ईएमआई बाउंस होने पर शुल्क (जीएसटी सहित)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "Rs. 590 (Incl. GST)");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>B</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>विस्तार शुल्क (जीएसटी सहित)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>C</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>विज़िट शुल्क (जीएसटी सहित)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "Rs. 236 (Incl. GST)");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(iii)</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>पुरोबंधात्मक शुल्क, यदि लागू हो ** (जीएसटी सहित)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "3% of OS (Allowed after 1st EMI paid) + GST");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(iv)</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>ऋण को निश्चितदर से अस्थिर दर में बदलने के लिए शुल्कऔर इसकाउल्टा</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;' >{0}</td>", "NA");
                        sb.Append("</tr>");


                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(v)</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>कोई अन्य शुल्क(कृपया स्पष्ट रूप सेबताएं)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>a</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>पूर्व-भुगतान शुल्क**</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>b</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>ऋणपूर्वसमापन शुल्क</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "कुलबकायामूलधनपर 3%");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        sb.Append("<p>*शुल्कपर जीएसटी लागू होगा</p>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 11th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td style='font-weight:bold;text-align:center;width:100%;'>भाग 2 (अन्य गुणात्मक जानकारी)</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px; width:5%;'>1</td>");
                        sb.Append("<td colspan='2' style='padding:5px; width:30%;'>वसूली एजेंटों की नियुक्ति से संबंधित ऋण करार का खंड</td>");
                        sb.Append("<td colspan='3' style='padding:5px; width:65%;'>यदि किस्त ऋणदाता के खाते में नियत दिन तक जमा नहीं होती है, तो यह माना जाएगा कि उधारकर्ता भुगतान करने से चूक गया है और चूक के परिणाम स्वरूप खंड लागू होंगे</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>2</td>");
                        sb.Append("<td colspan='2' style='padding:5px;'>ऋण करार का खंड जिसमें शिकायत निवारण व्यवस्था का विवरण दिया गया है</td>");
                        sb.Append("<td colspan='3' style='padding:5px;'>इस करार की वैधता, व्याख्या या प्रदर्शन से संबंधित सभी सवाल भारतीय कानूनों द्वारा शासित होंगे और महाराष्ट्र और मुंबई में स्थित अदालतों के अधिकार क्षेत्रों के अधीन होंगे।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>3</td>");
                        sb.Append("<td colspan='2' style='padding:5px;'>केंद्रीय शिकायत निवारण अधिकारी का फोन नंबरऔर ईमेल आईडी</td>");
                        sb.Append("<td colspan='3' style='padding:5px;'>id-care@unitybank.co.in टोल फ्री नंबर-18002091122</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>4</td>");
                        sb.Append("<td colspan='2' style='padding:5px;'>क्या ऋण वर्तमान में, या भविष्य में शायद, किसी अन्य विनियमित संस्था (आरई) को अंतरित करने या प्रतिभूतीकरण के अधीन है (हाँ/नहीं)</td>");
                        sb.Append("<td colspan='3' style='padding:5px;'>हाँ, उधारकर्ता के अधीन</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>5</td>");
                        sb.Append("<td colspan='2' style='padding:5px;'>अग्रिम भुगतान के मामले में ऋण अवधि में कमी से संबंधित प्रावधान।</td>");
                        sb.Append("<td colspan='3' style='padding:5px;'>यदि आवेदक मासिक या साप्ताहिक ऋण EMI के लिए अग्रिम भुगतान करता है, तो ऐसा भुगतान बकाया मूलधन के खिलाफ समायोजित किया जाएगा, जिससे ऋण की अवधि में समानुपातिक कमी आएगी।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>6</td>");
                        sb.Append("<td colspan='5' style='padding:5px;'>सहयोगात्मक ऋण व्यवस्था (उदा. सह-ऋण / आउटसोर्सिंग) के अंतर्गत ऋण के मामले में, निम्नलिखित अतिरिक्त ब्यौरा प्रस्तुत किया जा सकता है:</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>प्रवर्तक विनियमित संस्था (आरई) का नाम और साथ ही इसके निधीयन का अनुपात</td>");
                        sb.Append("<td colspan='1' style='padding:5px;text-align:center;'>भागीदार विनियमित संस्था का नाम (आरई) और साथ ही इसके निधीयन का अनुपात</td>");
                        sb.Append("<td colspan='2' style='padding:5px;text-align:center;'>ब्याज की मिश्रित दर</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='6' style='padding:5px;text-align:center;'>सभी तीनों सेक्शन के लिए लागू नहीं (बिंदु 5)</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>7</td>");
                        sb.Append("<td colspan='5' style='padding:5px;'>डिजिटल ऋण के मामले में, निम्नलिखित विशिष्ट प्रकटीकरण प्रस्तुत किया जा सकता है:</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='4' style='padding:5px;'>(i) विनियमित संस्था (आरई) के बोर्ड द्वारा स्वीकृत नीति के अनुसारविराम अवधि/ लुक-अप पीरियड, जिस दौरान ऋण के पूर्व-भुगतान पर कोई भी दंड नहीं लगाया जाना चाहिए। </td>");
                        sb.Append("<td colspan='2' style='padding:5px;text-align:center;'>NA</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='4' style='padding:5px;'>(ii) वसूली एजेंट के तौर पर कार्य कर रहे एलएसपी का ब्यौरा जिसे उधारकर्ता से संपर्क करने के लिए प्राधिकृत किया गया है।</td>");
                        sb.Append("<td colspan='2' style='padding:5px;text-align:center;'>NA</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");


                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 12th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td style='font-weight:bold;text-align:left;width:100%;'>(उत्पाद का नाम – संवर्ग का नाम – खुदरा और एमएसएमई ऋण) के लिए एपीआर की गणना हेतु चित्रण  </td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px; width:10%;'>अ. क्र.</td>");
                        sb.Append("<td  style='padding:5px; width:60%;'>मापदंड</td>");
                        sb.Append("<td  style='padding:5px; width:30%;'>विवरण</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>1</td>");
                        sb.Append("<td  style='padding:5px;'>संस्वीकृत ऋण राशि (रूपए में) (केएफएस टेम्पलेट का अनु. क्र. 2– भाग 1)</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanAmt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>2</td>");
                        sb.Append("<td  style='padding:5px;'>ऋण की अवधि (वर्ष / महीने / दिनों में) (केएफएस टेम्पलेट का अनु. क्र. 4– भाग 1)</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["Tenure"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>a)</td>");
                        sb.Append("<td  style='padding:5px;'>मूलधन के भुगतान के लिए किस्तों की संख्या, ग़ैर-समान आवधिक ऋण के मामले में</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td rowspan='3' style='padding:5px;'>b)</td>");
                        sb.Append("<td  style='padding:5px;'>ईएमआई का प्रकार</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["RepFreq"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>प्रत्येक ईएमआई की राशि (रूपए में)</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["EMIAmt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>ईएमआई की संख्या (उदा., मासिक किस्तों के मामले में ईएमआई की संख्या) (केएफएस टेम्पलेट का अनु. क्र. 5– भाग 1)</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInstNo"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>c)</td>");
                        sb.Append("<td  style='padding:5px;'>पूँजीकृत ब्याज के भुगतान के लिए किस्तों की संख्या, यदि कोई हो</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>d)</td>");
                        sb.Append("<td  style='padding:5px;'>रीपेमेंटका प्रारंभ, संस्वीकृति के बाद  (केएफएस टेम्पलेट का अनु. क्र. 5– भाग 1)</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["FInstDt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>3</td>");
                        sb.Append("<td  style='padding:5px;'>ब्याज दर का प्रकार (निश्चितया अस्थिर या हाइब्रिड) (केएफएस टेम्पलेट का अनु. क्र. 6– भाग 1)</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", "Fixed");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>4</td>");
                        sb.Append("<td  style='padding:5px;'>ब्याज का दर (केएफएस टेम्पलेट का अनु. क्र. 6– भाग 1)</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(Math.Round(Convert.ToDouble(dtScheduleOwn.Rows[0]["IntRate"]), 2)));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>5</td>");
                        sb.Append("<td  style='padding:5px;'>संस्वीकृति तारीख पर प्रचलित दर के अनुसार ऋण की संपूर्ण अवधि के दौरान वसूली जाने वाली कुल ब्याज राशि (रूपए में)</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["IntAmt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>6</td>");
                        sb.Append("<td  style='padding:5px;'>देय शुल्क / फीस(रूपए में)</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(vTotalPayRE));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>A</td>");
                        sb.Append("<td  style='padding:5px;'>विनियमित संस्था (आरई) कोदेय (केएफएस टेम्पलेट का अनु. क्र. 8A– भाग 1)</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(vTotalPayRE));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>B</td>");
                        sb.Append("<td  style='padding:5px;'>विनियमित संस्था (आरई) के माध्यम से रूट किएगए तृतीय पक्ष कोदेय (केएफएस टेम्पलेट का अनु. क्र. 8B– भाग 1)</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(vTotalPayTP));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>7</td>");
                        sb.Append("<td  style='padding:5px;'>निवल संवितरित राशि (1-6) (रूपए में)</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["NetDisbAmt"]));
                        sb.Append("</tr>");

                        sb.Append("</table>");


                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 13th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td style='font-weight:bold;text-align:left;width:100%;'>(उत्पाद का नाम – संवर्ग का नाम – खुदरा और एमएसएमई ऋण) के लिए एपीआर की गणना हेतु चित्रण  </td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;width:10%;'>8</td>");
                        sb.Append("<td  style='padding:5px;width:60%;'>उधारकर्ता द्वारा भुगतान की जानेवाली कुल राशि (1 और 5 का योग)(रूपए में)</td>");
                        sb.AppendFormat("<td style='padding:5px;width:30%;text-align:center;'>{0}</td>", Convert.ToString(vTotalPI));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>9</td>");
                        sb.Append("<td  style='padding:5px;'>वार्षिक प्रतिशत दर – प्रभावी वार्षिकीकृत ब्याज दर (प्रतिशत में) (केएफएस टेम्पलेट का अनु. क्र. 9– भाग 1)</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["IRRIC"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>10</td>");
                        sb.Append("<td  style='padding:5px;'>नियम और शर्तों के अनुसार संवितरण की अनुसूची</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", "EMI Schedule");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>11</td>");
                        sb.Append("<td  style='padding:5px;'>किस्त के भुगतान की देय तारीख और ब्याज</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["FInstDt"]));
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:5px;'>कृपया ध्यान दें कि उपरोक्त शुल्क वार्षिक नहीं हैं।दस्तावेज़ 5 दिनों की अवधि के लिए स्वीकृति के लिए वैध रहेगा, जिसके बाद संवितरण प्रक्रिया शुरू हो जाएगी।*जीएसटी शुल्क पर लागू होगा।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:5px;'>&nbsp;</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:5px;'>उधारकर्ता और सह-उधारकर्ता (ओं) द्वारा निष्पादित और वितरित किया गया और उनके पढ़ने (और/या उन्हें समझाया था), सत्यापित, समझ, अपरिवर्तनीय रूप से सहमत, स्वीकृत, पुष्टि, और स्वीकृति पत्र के सभी खंडों, मुख्य तथ्य पत्रक, सामान्य नियम और शर्तों के दस्तावेज, और अनुसूची, जिसमें सभी सामग्री शामिल हैं, की घोषणा की,  और सभी दस्तावेजों की प्राप्ति को स्वीकार करना, जिससे उसी की सटीकता और शुद्धता को प्रमाणित किया जा सके।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:5px;'>&nbsp;</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:5px;'>उधारकर्ता स्वीकार करते हैं कि वर्तमान में प्रभावी अनुसूची, नियम और शर्तें, और प्रभार संशोधित किए जा सकते हैं और/या नई शर्तों और प्रभारों को किसी भी समय और समय-समय पर बैंक के एकमात्र और पूर्ण विवेक पर पेश किया जा सकता है। ऐसे परिवर्तन उधारकर्ता(ओं) के लिए बाध्यकारी होंगे। ऐसे सभी परिवर्तनों या परिवर्धन को विधिवत सूचित किया जाएगा और संभावित आधार पर प्रभावी होंगे।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:5px;'>&nbsp;</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='padding:5px;'>**फोरक्लोज़र शुल्क: पर्सनल लोन के फोरक्लोज़र शुल्क निर्धारित अवधि समाप्त होने से पहले पूरी बकाया लोन राशि के शीघ्र पुनर्भुगतान को संदर्भित करते हैं. उधारकर्ता शेष मूलधन और अर्जित ब्याज का भुगतान करके ऋण को जल्दी निपटाने का विकल्प चुन सकते हैं। ऋणदाता इसके लिए फोरक्लोज़र शुल्क लेते हैं।**प्री-पेमेंट शुल्क: प्री-पेमेंट शुल्क उन शुल्कों या शुल्कों को संदर्भित करता है जो एक उधारकर्ता सहमत शेड्यूल से पहले लोन के हिस्से का भुगतान करने के लिए खर्च कर सकता है. कर्जदाता इसके लिए प्रीपेमेंट फीस लेते हैं।</td>");
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
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr><td colspan='2' style='padding-top:20px;text-decoration: underline;'><strong>ऋण पुनर्भुगतान अनुसूची</strong></td></tr>");
                        sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table  class='xsmall_font' style='border-collapse: collapse; width: 100%;  text-align: left;' border='1' >");
                        sb.Append("<tr>");
                        sb.Append("<th style='padding: 4px;'>क्रमांक<br/>Sr.No.</th>");
                        sb.Append("<th style='padding: 4px;'>नियत तिथि<br/>Due Date</th>");
                        sb.Append("<th style='padding: 4px;'>मुख्य अवधि<br/>Principle Due</th>");
                        sb.Append("<th style='padding: 4px;'>ब्याज की अवधि<br/>Interest Due</th>");
                        sb.Append("<th style='padding: 4px;'>मासिक किस्त राशि<br/>EMI Amount</th>");
                        sb.Append("<th style='padding: 4px;'>मुख्य बकाया<br/>Principle Outstanding</th>");
                        sb.Append("<th style='padding: 4px;'>ब्याज बकाया<br/>Interest Outstanding</th>");
                        sb.Append("<th style='padding: 4px;'>कुल बकाया<br/>Total Outstanding</th>");
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

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // Start Loan Agreement
                        #region 15th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='1' class='large_font' style='text-align:center; font-weight:bold; padding:10px;'>ऋण करार</td>");
                        sb.Append("</tr>");

                        // Agreement Start
                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding-top:10px;'>यह करार <span style='border-bottom: 1px dotted black;'>{0}</span> के <span style='border-bottom: 1px dotted black;'>{1}</span> महीने में <span style='border-bottom: 1px dotted black;'>{2}</span> दिन निम्नलिखित के बीच किया गया है यूनिटीस्मॉलफाइनेंसबैंक, जोकंपनियोंकेअधिनियम 2013 केअंतर्गतपंजीकृतएककंपनीहै, जिसकापंजीकृतकार्यालययूनीटीस्मॉलफाइनेंसबैंकलिमिटेड, 5वांऔर 6वांमंजिल, टॉवर - 1 एलएंडटीसीवूड्सटॉवर, प्लॉटनंबर - आर - 1, सेक्टर - 40, सीवूड्सरेलवेस्टेशन, Navi मुंबई - 400706 मेंस्थित है, जिसका प्रतिनिधित्व कंपनी के विद्याविहार स्थित शाखा प्रबंधक द्वारा किया जा रहा है, इसके बाद “ऋणदाता/कंपनी” के रूप में संदर्भित किया गया है (जिस अभिव्यक्ति में, जब तक कि यह संदर्भ या अर्थ के प्रतिकूल न हो, पहले भाग में इसके उत्तराधिकारी और समनुदेशित शामिल समझे जाएंगे)</td></tr>", Convert.ToString(dtLoanAgr.Rows[0]["AgreeYear"]), Convert.ToString(dtLoanAgr.Rows[0]["AgreeMonth"]), Convert.ToString(dtLoanAgr.Rows[0]["AgreeDay"]));

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding-top:10px;'>और मैं, श्री/श्रीमती ,<span style='border-bottom: 1px dotted black;'>{0}</span> ,<span style='border-bottom: 1px dotted black;'>{1}</span> की पत्नी, स्थायी निवासी {2} और, इसके बाद के रूप में संदर्भित किया गया है (जिस अभिव्यक्ति में, जब तक कि यह संदर्भ या अर्थ के प्रतिकूल न हो, दूसरे भाग में इसके उत्तराधिकारी, निष्पादक और कानूनी प्रतिनिधि शामिल समझे जाएंगे)</td></tr>", Convert.ToString(dtLoanAgr.Rows[0]["Borrower"]), Convert.ToString(dtLoanAgr.Rows[0]["CoBorrower"]), Convert.ToString(dtLoanAgr.Rows[0]["BorrowerAddr"]));


                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding-top:10px;'>2. श्री/श्रीमती <span style='border-bottom: 1px dotted black;'>{0}</span> और, इसके बाद “” के रूप में संदर्भित किया गया है (जिस अभिव्यक्ति में, जब तक कि यह संदर्भ या अर्थ के प्रतिकूल न हो, तीसरे भाग में इसके उत्तराधिकारी, निष्पादक और कानूनी प्रतिनिधि शामिल समझे जाएंगे)</td></tr>", Convert.ToString(dtLoanAgr.Rows[0]["Borrower"]));
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding-top:10px;'>जबकि कर्जदार ने कंपनी में व्यापार विस्तार के उद्देश्य के लिए रू. दो लाख (शब्दों में) के एमईएल सरल व्यापार (इसके बाद “ऋण” के रूप में संदर्भित किया गया है) के लिए आवेदन किया है और कंपनी ने कर्जदार के समुचित मूल्यांकन के बाद, निम्नलिखित नियमों और शर्तों के अधीन, दिनांक <span style='border-bottom: 1px dotted black;'>{0}</span> की स्वीकृति के हवाले से रू. <span style='border-bottom: 1px dotted black;'>{1}</span> तक की सीमा का ऋण स्वीकृत किया है।</td></tr>", Convert.ToString(dtLoanAgr.Rows[0]["RepayStartDate"]), Convert.ToString(dtLoanAgr.Rows[0]["AppAmount"]));

                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("इसलिए अब, यूनिटी स्मॉल फाइनेंस बैंक द्वारा सुविधा दस्तावेजों में उल्लिखित सुविधा प्रदान किए जाने/ सुविधा प्रदान करने को सहमति देना विचार में लेते हुए, कर्जदार और सह-कर्जदार एतदद्वारा सहमति, पारस्परिक स्वीकृति देते हैं, पुष्टि करते हैं और निम्नलिखित नियम और शर्तें अभिलेखित करते हैं:");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // Section: नियम और शर्तें
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px; font-weight:bold;'>नियम और शर्तें</td>");
                        sb.Append("</tr>");

                        // a) राशि
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.AppendFormat("<b>a)</b> राशि: ऋणदाता कर्जदार को रू. <span style='border-bottom: 1px dotted black;'>{0}</span> राशि का एक मियादी ऋण देंगे, जो ऋणदाता इस प्रकार संवितरित करेंगे, जो उनके विचार में  केवल उस उद्देश्य के लिए उपयोग में लाए जाने के लिए उपयुक्त होगा जिसका उल्लेख ऋणदाता को प्रस्तुत किए गए ऋण आवेदन में किया गया है।</td></tr>", Convert.ToString(dtLoanAgr.Rows[0]["AppAmount"]));

                        // b) ब्याज
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>b)</b> ब्याज: ऋण पर वार्षिक % दर से ब्याज लागू होगा जिसे घटती शेष राशि के आधार पर परिकलित किया जाएगा। निधि की लागत, जोखिम प्रीमियम और मार्जिन के आधार पर ब्याज दर निर्धारित किया जाता है। किश्त(श्तों) के भुगतान में किसी भी प्रकार का विलंब होने पर, इस प्रकार की भुगतान चूक के संबंध में ऋणदाता के अन्य अधिकारों के प्रति पूर्वाग्रह के बिना, ऋण के अतिदेय शेष पर प्रचलित दर के अलावा (3%) का अतिरिक्त ब्याज देना होगा।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // c) दंडात्मक प्रभार
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>c)</b> “दंडात्मक प्रभार” का मतलब है महत्वपूर्ण नियमों और शर्तों का उल्लंघन/ग़ैर-अनुपालन करने पर कर्जदार द्वारा देय दंडात्मक प्रभार, जिसे नीचे दी गई अनुसूची I में अधिक विशिष्ट रूप से निर्दिष्ट किया गया है, और/या जैसा कि समय समय पर बैंक द्वारा कर्जदार को अधिसूचित किया गया हो या बैंक की वेबसाइट पर जैसा अपडेट किया गया हो।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // d) महत्वपूर्ण नियम और शर्तें
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>d)</b> महत्वपूर्ण नियमों और शर्तों का मतलब है इन मानक नियमों के महत्वपूर्ण नियम और शर्तें जो अधिक विशिष्ट रूप से नीचे दी गई अनुसूची में निर्दिष्ट की गई है। किसी भी महत्वपूर्ण नियमों और शर्तों के उल्लंघन / ग़ैर-अनुपालन के लिए, पूर्वगामी की व्यापकता के प्रति पूर्वाग्रह के बिना, बैंक के पास यह प्रभार वसूलने का अधिकार होगा, और कर्जदार इस प्रकार के दंडात्मक प्रभार का भुगतान करने के लिए ज़िम्मेदार होगा और इस प्रकार की अवधि तब तक के लिए होगी जब तक यह अनियमितता या उल्लंघन जारी रहता है या उतने समय के लिए जैसा बैंक के निर्णय के अनुसार आवश्यक हो। लेकिन इसके लिए आवश्यक हैं कि दंडात्मक प्रभार लगाया जाना तथा उसका भुगतान बैंक के नीचे दिए गए या अन्य अधिकारों या उपायों के प्रति अथवा कर्जदार के खिलाफ इस प्रकार की अनियमितता या उल्लंघन के लिए आगे की जानेवाली क़ानूनी कार्रवाई के प्रति किसी भी पूर्वाग्रह के बिना हो।");
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
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%;border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.AppendFormat("<b>e)</b> प्रसंस्करण शुल्क: कर्जदार को ऋणदाता द्वारा ऋण संवितरित किए जाने के समय पर रू. ( <span style='border-bottom: 1px dotted black;'>{0}</span> ) का एकबारगी प्रसंस्करण शुल्क का भुगतान करना होगा।कर्जदार सहमत है कि उपरोक्त शुल्क का उपयोग आंशिक रूप से उन खर्चों को अदा करने के लिए हो सकता है जो ऋणदाता द्वारा दस्तावेजीकरण में या ऋण स्वीकृति के लिए होने वाले अन्य प्रासंगिक व्यय में खर्च हो सकते हैं।", AmtInWrds);
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // f) किश्त
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.AppendFormat("<b>f)</b> किश्त: उपरोक्त ब्याज दर के आधार पर, किश्त रू. <span style='border-bottom: 1px dotted black;'>{0}</span> होगी। पहली किश्त <span style='border-bottom: 1px dotted black;'>{1}</span> को देय होगी।इसके बाद, हर अनुवर्ती मासिक किश्त का भुगतान <span style='border-bottom: 1px dotted black;'>{2}</span> महीनों की अवधि के लिए किया जाएगा। समानमासिक किश्त (ईएमआई) हर महीने की <span style='border-bottom: 1px dotted black;'>{3}</span> वीं तारीख को वसूल की जाएगी।</td></tr>", Convert.ToString(dtLoanAgr.Rows[0]["EMIAmt"]), Convert.ToString(dtLoanAgr.Rows[0]["RepayStartDate"]), Convert.ToString(dtLoanAgr.Rows[0]["Tenure"]), "5");

                        // g) चुकौती
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>g)</b> चुकौती: ऋणदाता द्वारा ऋण दिए जाने का विचार करते हुए, कर्जदार उपरोक्त ब्याज दर और मासिक किश्तों के आधार पर ब्याज सहित ऋण की चुकौती करेगा। ");
                        sb.Append("अवलोकन करने, समझने और सहमत होने के पश्चात एतदद्वारा कर्जदार ईएमआई की गणना करने की विधि और प्रभावी ब्याज दर की पुष्टि करता है। ");
                        sb.Append("इसके साथ ही कर्जदार किसी भी समय सरकार द्वारा लगाए जानेवाले ब्याज कर या अन्य कर/प्रभार जैसे अन्य प्रभार का भुगतान करने की स्वीकृति देता है।<br/><br/>");

                        sb.Append("कर्जदार द्वारा चुकौती का साधन या तो राष्ट्रीय स्वचालित समाशोधन गृह (एनएसीएच) होगा, या जारी की गई रसीद जारी किये जाने पर नकदी द्वारा या किसी भी अन्य साधन द्वारा होगा, जिसके लिए ऋणदाता द्वारा लिखित में स्वीकृति दी गई हो। ");
                        sb.Append("फिर भी, यदि नियत तिथि तक किश्त ऋणदाता के खाते में जमा नहीं की जाती है, तो यह माना जाएगा कि कर्जदार से भुगतान करने में चूक हुई है और चूक होने के परिणामस्वरूप प्रयोज्य खंड लागू होंगे।<br/><br/>");

                        sb.Append("इसके साथ ही कर्जदार खाते में पर्याप्त शेष बनाए रखने के लिए स्वीकृति देता है ताकि उसके बैंकर उसके खाते से धन निकासी कर सके और इस प्रकार निकासी की गई राशि का विप्रेषण ऋणदाता के पास जमा हो सके।<br/><br/>");

                        sb.Append("ऋणदाता की पूर्व लिखित स्वीकृति के बिना कर्जदार उस बैंक खाते में नाम या गठन में बदलाव नहीं करेगा या खाता बंद नहीं करेगा, जहाँ से एनएसीएच अधिदेश पंजीकृत है। ");
                        sb.Append("इसके साथ ही, कर्जदार किसी भी चेक को प्रस्तुत करने से रोकने के लिए ऋणदाता से अनुरोध नहीं करेगा और किसी भी प्रकार के कारण के लिए “भुगतान रोकें” के निर्देश अपने बैंकर को जारी नहीं करेगा और यदि कर्जदार ऐसा करता है, तो ऋणदाता इस प्रकार की सूचनाओं को संज्ञान में नहीं लेगा और चेक प्रस्तुत करना जारी रखेगा। ");
                        sb.Append("यदि चेक अस्वीकार होता है, तो देय राशि की वसूली करने के लिए उचित कार्रवाई करने का मार्ग ऋणदाता के लिए खुला है। ");
                        sb.Append("ऐसे मामलों में कर्जदार, चेक राशि के विप्रेषण के अलावा चेक नकारे जाने पर लागू प्रभार और चेक राशि के भुगतान करने तक अतिदेय ब्याज का भुगतान करने के लिए बिना शर्त स्वीकृति देता है। ");
                        sb.Append("कर्जदार एतदद्वारा स्वीकार करता है कि उसके द्वारा जारी किए गए चेक के नकारे जाने के बारे में अपने बैंकर से पुष्टि करने की ज़िम्मेदारी उसकी है और किसी भी समय पर वह यह कहने का प्रयास या अभिप्राय व्यक्त नहीं करेगा कि उसके द्वारा जारी किए गए चेक के अस्वीकृत होने के तथ्य के बारे में उसे ऋणदाता द्वारा सूचित नहीं किया गया।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // h) सूचना
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>h)</b> सूचना: कर्जदार / सह-कर्जदार को ऋणदाता द्वारा कोई भी नोटिस/पत्र/अन्य दस्तावेज इस करार में दिए गए पते पर भेजी जाएगी और यह माना जाएगा कि पंजीकृत डाक / कूरियर द्वारा भेजने के बाद 5 कार्य/व्यापारिक दिनों के भीतर उन्हें वह प्राप्त हो गया है। ");
                        sb.Append("संवितरण कार्यक्रम, ब्याज दर, सेवा प्रभार इत्यादि सहित नियम और शर्तों के बारे मंए कंपनी कर्जदार को नोटिस द्वारा सूचित करेगी और यह सुनिश्चित करेगी कि ब्याज दर और प्रभारों में किए गए बदलाव केवल भविष्यलक्षी प्रभाव से लागू किए जा रहे हैं।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // i) क्षतिपूर्ति
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>i)</b> क्षतिपूर्ति: कर्जदार सहमत है कि यह करार कर्जदार और ऋणदाता के बीच संपूर्ण रूप से एक वित्तीय व्यवस्था है। ");
                        sb.Append("कर्जदार और/या सह-कर्जदार, उनके/अपने संबंधित कर्मचारियों, एजेंटों, या समनुदेशिती की किसी भी कृतियों, त्रृटियों या भूल के परिणामस्वरूप होने वाले नुकसान, दावों, दायित्वों, या हर्जाने के खिलाफ़, या इस करार या इस करार के अनुसरण में निष्पादित किसी भी अन्य दस्तावेज से संबंधित अनुचित प्रदर्शन या ग़ैर-प्रदर्शन के लिए ऋणदाता और इसके संचालकों, अधिकारियों, कर्मचारियों, एजेंटों और सलाहकारों को सुरक्षित करने और हानिरहित रखने के लिए एतदद्वारा कर्जदार (और सह-कर्जदार) स्वीकृति प्रदान करते हैं।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");


                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 17th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append("<b>j)</b> उपयोगिता: कर्जदार स्वीकृति देता है और वचन देता है कि यहाँ वर्णित उद्देश्य को छोड़कर किसी भी अन्य असामाजिक या सट्टे के उद्देश्य के लिए ऋण का उपयोग नहीं किया जाएगा।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // k) भुगतान में चूक की स्थिति में
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>k)</b> भुगतान में चूक की स्थिति में: इस करार में कहीं और निहित ऋणदाता के अधिकारों के प्रति पूर्वाग्रह के बिना, यदि इस खंड मंं निर्दिष्ट एक या इससे अधिक घटनाएँ घटित होती हैं, तो ऐसी स्थिति में, ऋणदाता, कर्जदार को एक लिखित नोटिस के द्वारा घोषित कर सकता है कि इस करार या नियम और/या कर्जदार और ऋणदाता के बीच किए गए किसी अन्य करार या दस्तावेजों के अंतर्गत मूलधन राशि, इसके साथ ऋण पर उपचित ब्याज और अन्य राशि, यदि कोई हो, कर्जदार द्वारा ऋणदाता को तुरंत देय है, भले ही कर्जदार और ऋणदाता के बीच हुए इस करार में या किसी अन्य करार में या लिखत में इसके विपरीत कोई चीज़ मौजूद हो, यह समान राशि तुरंत बकाया और देय होगी।<br/><br/>");

                        sb.Append("निम्नलिखित घटनाएं भुगतान में चूक मानी जाएगी: <br/>");
                        sb.Append("<b>i.</b> कर्जदार के यहाँ निहित तरीके से ऋण या किसी शुल्क, प्रभार या खर्चों का भुगतान करने में विफल होने या जिस तिथि को यह देय हो जाती है तब से एक से अधिक दिन की अवधि के लिए किसी किश्त या नीचे दी गई किसी अन्य बकाया राशि का भुगतान नहीं करने पर; या<br/>");
                        sb.Append("<b>ii.</b> कर्जदार द्वारा यहाँ निहित किसी भी नियम और शर्तों का उल्लंघन किया गया हो या ऋणदाता को किसी प्रकार की गलतबयानी की गई हो; या<br/>");
                        sb.Append("<b>iii.</b> कोई प्राप्तकर्ता नियुक्त किया जा रहा हो या कर्जदार की किसी परिसंपत्ति के लिए कुर्की हो रही हो या कर्जदार, किसी स्वतंत्र व्यक्ति के तौर पर, दिवालिया घोषित किया गया हो या दिवालिएपन की कोई कृति कर रहा हो या<br/>");
                        sb.Append("<b>iv.</b> र्जदार के खिलाफ पेशेवर कदाचार के लिए कार्यवाही हो रही हो; या<br/>");
                        sb.Append("<b>v.</b> दार कोई जानकारी या दस्तावेज प्रस्तुत करने में विफल रहा हो जो ऋणदाता के लिए आवश्यक हो सकती है; या<br/>");
                        sb.Append("<b>vi.</b> कर्जदार द्वारा किसी अन्य ऋण के किसी नियम और शर्तों का पालन करने में या कर्जदार को ऋणदाता द्वारा प्रदान की गई सुविधा में चूक होने पर; या<br/>");
                        sb.Append("<b>vii.</b> कोई ऐसी अन्य स्थितियाँ उत्पन्न हुई हों, जो ऋणदाता की एकमात्र राय में ऋणदाता के हितों को जोखिम में डाल रही हों।<br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // l) भुगतान में चूक के परिणाम
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>l)</b> भुगतान में चूक के परिणाम:<br/>");
                        sb.Append("<b>i.</b> भुगतान में चूक के उपरोक्त किसी भी घटना के घटित होने पर, ऋणदाता ऋण और इसके संबंध में देय अन्य राशि को रद्द कर सकता है। यदि ऋण का बकाया शेष और इसके संबंध में देय अन्य राशि का भुगतान, इस प्रकार मांग किए जाने के 7 (सात) दिनों के भीतर नहीं किया जाता है, तो ऐसे मामले में ऋणदाता के पास संपूर्ण धारणाधिकार होंगे और ऋणदाता के पास कर्जदार के नाम पर मौजूद सभी धनराशि और खातों का समंजन करने का अधिकार होगा, और ऋणदाता को यह अधिकार प्राप्त होंगे कि वह स्वतंत्र रूप से कर्जदार के विरूद्ध कार्रवाई कर सके।<br/>");
                        sb.Append("<b>ii.</b> ऋणदाता को यह अधिकार प्राप्त होगा कि ब्याज, प्रभार, और व्यय के साथ ऋण राशि की वसूली के लिए कर्जदार और /या सह-कर्जदार/कर्जदार के खिलाफ आगे बढ़े और कार्रवाई करे। कर्जदार और सह-कर्जदार एतदद्वारा बकाया राशि पर उस समय तक @% प्रति महीने अतिदेय ब्याज का भुगतान करने के लिए स्वीकृति प्रदान करते हैं, जब तक ऋणदाता को ऋण राशि का संपूर्ण रूप से भुगतान न किया जाए।<br/>");
                        sb.Append("<b>iii.</b> स करार में निर्दिष्ट किए गए अधिकारों के अलावा, ऋणदाता के पास यह अधिकार होगा कि वह इस करार के अंतर्गत बकाया सारी धनराशि और कर्जदार और/या सह-कर्जदार द्वारा देय राशि की वसूली के लिए कोर्ट के हस्तक्षेप के साथ या बिना सभी प्रकार की या कोई भी कार्रवाई करे।<br/>");
                        sb.Append("<b>iv.</b> यदि किसी भी समय ऋणदाता के अपने विवेक पर उसे यह मानने के लिए पर्याप्त आधार मौजूद हो कि कर्जदार और/या सह-कर्जदार ने कोई गलतबयानी की है और/या ऋणदाता के समक्ष कोई जाली दस्तावेज या मिथ्या आंकड़े प्रस्तुत किए हैं, तो इस करार के अंतर्गत ऋणदाता के लिए अन्य दूसरे अधिकारों के उपलब्ध होने के बावजूद, ऋणदाता को यह अधिकार प्राप्त होगा कि वह कर्जदार और/या सह-कर्जदार के खिलाफ आपराधिक कार्यवाही शुरू करे या कोई अन्य उपयुक्त कार्रवाई करे।<br/>");

                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 18th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%;border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append("<b>v)</b> इस करार के अंतर्गत ऋणदाता को प्रदान किए गए सभी अधिकार और शक्तियाँ उन किसी भी अधिकारों और सुरक्षा दस्तावेज़ों के अलावा और अनुपूरक होंगे, जो ऋणदाता को ऋणप्रदाता के रूप में फिलहाल लागू किसी कानून के अंतर्गत कर्जदार और/या सह-कर्जदार के खिलाफ प्राप्त हैं और यह किसी भी तरह इनके अनादर में नहीं होंगे।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // vi)
                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append("<b>vi)</b> इस करार के अंतर्गत शेष बकाया राशि और कर्जदार द्वारा देय राशि के संबंध में ऋणदाता द्वारा प्रस्तुत कोई भी लेखा-विवरण कर्जदार द्वारा स्वीकार्य होगा और उस पर बाध्यकारी होगा और इसमें उल्लिखित राशि की सत्यता का निर्णायक सबूत होगा। ऊपर जो बताया गया है उसके प्रति पूर्वाग्रह के बिना, यदि कर्जदार किसी विवरण या उसके किसी भाग पर सवाल पूछने की इच्छा रखता है, तो कर्जदार द्वारा विवरण प्राप्त होने के 7 (सात) कारोबारी दिनों के भीतर कर्जदार को इस संबंध में विस्तृत सूचना ऋणदाता के समक्ष प्रस्तुत करनी होगी और ऋणदाता द्वारा इस पर विचार किया जाएगा और इसके बाद कर्जदार के पास इस संबंध में किसी भी अन्य आधार पर आपत्ति उठाने का अधिकार नहीं होगा। हालांकि यह स्पष्ट किया जाता है कि कर्जदार को यह अधिकार नहीं होगा कि ऋणदाता द्वारा प्रस्तुत किए गए त्रुटिपूर्ण लेखा-विवरण या किसी अन्य आधार पर ईएमआई के भुगतान में चूक या विलंब करे।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // m) लेखा परीक्षण
                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append("<b>m)</b> लेखा परीक्षण: यदि किसी ऋण सुविधा/ऋण खाते का वर्गीकरण खतरे के झंडे (रेड फ्लैग्ड) के तौर पर किया गया हो, तो ऐसे मामले में बैंक को स्व-विवेक के आधार पर इसे आगे जांच करने के लिए लेखा परीक्षण संचालित करना चाहिए, जो बैंक के संचालक मंडल द्वारा स्वीकृत नीति के अनुसार होगा। कर्जदार के असहयोग करने की स्थिति में लेखा-परीक्षण रिपोर्ट अनिर्णायक हो या विलंब से प्राप्त हो, तो बैंक द्वारा खाते की स्थिति का निष्कर्ष धोखाधड़ी के मामले के रूप में किया जायेगा या अन्यथा बैंक के रिकॉर्ड पर उपलब्ध सामग्री और उसकी अपनी आंतरिक जाँच / मूल्यांकन के आधार पर निष्कर्ष पर पहुँच जायेगा।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // n) शिकायत निवारण
                        sb.Append("<tr>");
                        sb.Append("<td style='padding-top:10px;'>");
                        sb.Append("<b>n)</b> शिकायत निवारण: यदि बैंक द्वारा दी गई सेवाओं से ग्राहक संतुष्ट नहीं है तो उसके पास अपनी शिकायत दर्ज करने का अधिकार सुरक्षित है। ग्राहक अपनी सुविधा के अनुसार अपनी शिकायत व्यक्तिगत रूप से, लिखित में, टेलिफोन पर, ईमेल के माध्यम से या ग्राहक देखभाल केंद्र care@unitybank.co.in के ज़रिए प्रस्तुत कर सकता है। यदि शिकायत 7 (सात) दिनों के भीतर नहीं सुलझती है या ग्राहक बैंक के निर्णय से संतुष्ट नहीं है, तो वह केंद्रीकृत ग्राहक देखभाल टीम/प्रादेशिक नोडल कार्यालय से संपर्क कर सकता है। यदि वे निर्णय से संतुष्ट नहीं है, तो ग्राहक प्रधान नोडल अधिकारी/शिकायत निवारण अधिकारी (केंद्रीय) को लिखित या फोन द्वारा संपर्क कर सकता है। पदाधिकारियों के संपर्क विवरण बैंक की वेबसाइट www.theunitybank.co.in पर उपलब्ध होंगे।<br/><br/>");
                        sb.Append("<b>प्रधान नोडल अधिकारी / शिकायत निवारण अधिकारी (केंद्रीय):</b><br/>");
                        sb.Append("श्री महेन्द्र नबन्द्रापता<br/>");
                        sb.Append("यूनिटी स्मॉल फाइनेंस बैंक लिमिटेड, 13वां माला, रूपा रेनेसां, डी-33, TTC MIDC रोड, TTC औद्योगिक क्षेत्र, MIDC औद्योगिक क्षेत्र, तुडे, नवी मुंबई, महाराष्ट्र 400705<br/>");
                        sb.Append("ईमेल: level3escalation@unitybank.co.in<br/>");
                        sb.Append("फोन: 9152366104 (मोबाइल) सुबह 9:30 से शाम 6:00 बजे, सोमवार से शनिवार");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 19th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%;border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:10px;'>");
                        sb.Append("<b>o)</b> वसूली एजेंट/एजेंसी: कर्जदार और/या जमानतदार से इस सुविधा करार के अंतर्गत किसी भी बकाया देय राशि की वसूली के उद्देश्य के लिए किसी भी स्वतंत्र एजेंट/एजेंसियों को नियुक्त करने का अधिकार बैक के पास सुरक्षित है।  बैंक के कर्मचारी सहित इस प्रकार के एजेंट/एजेंसियाँ ऋण की अवधि के दौरान या उसके बाद किसी भी समय कर्जदार और/या जमानतदार से उनके निवास स्थान पर या व्यवसाय के स्थान पर या किसी अन्य जगह पर ऋण की बकाया राशि वसूल सकते हैं, और इस संबंध में बैंक की आंतरिक नीति के अनुसार, जिसे समय समय पर अपडेट किया जाता है।  किसी वसूली एजेंसी/एजेंट को नियुक्त करने पर बैंक कर्जदार को वसूली एजेंसी /एजेंट के नाम और संपर्क विवरणों के संबंध में अपडेट करेगी।  इसके अलावा, पारदर्शिता सुनिश्चित करने और कर्जदार और/या जमानतदार और वसूली एजेंसी/एजेंट के बीच संचार सुगम बनाने के लिए बैंक अपनी अधिकृत वेबसाइट पर अपने सेवा प्रदाता के रूप में बैंक के साथ संबद्ध वसूली एजेंसी/एजेंट के नाम और संपर्क जानकारी प्रकाशित करेगी।");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        // p) सामान्य नियम एवं शर्तें
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:10px;'>");
                        sb.Append("<b>p)</b> सामान्य नियम एवं शर्तें: इस करार के पक्ष एतदद्वारा सामान्य नियमों और शर्तों और प्रसंविदा के लिए स्वीकृति प्रदान करते हैं:<br/>");
                        sb.Append("i. मांग किए जाने पर ऋण प्रतिदेय होगा। हालांकि चुकौती कार्यक्रम में कुछ निर्दिष्ट न होने पर भी, ऋणदाता के पास अपने विवेक पर किसी भी कारण के लिए, जो केवल उसे ही ज्ञात हो, ऋण के संबंध में अन्य देय राशि के साथ संपूर्ण ऋण का भुगतान करने के लिए वसूली करने और मांग करने का अधिकार होगा।   <br/>");
                        sb.Append("ii. ब्याज दर में बदलाव के मामले में चुकौती कार्यक्रम ब्याज को पुन:परिकलित करने के ऋणदाता के अधिकार के प्रति पूर्वाग्रह रहित है और इस प्रकार पुन:परिकलित करने पर कर्जदार इस प्रकार के परिवर्तित चुकौती कार्यक्रम के अनुसार भुगतान करेगा।   ब्याज गणना के संबंध में किसी भी विवाद की स्थिति में कर्जदार को किसी भी किश्त के भुगतान को रोके रखने का अधिकार नहीं होगा।<br/>");
                        sb.Append("iii. ऋणदाता, अपने संपूर्ण विवेकाधिकार में कर्जदार को यह अनुमति प्रदान करता है कि वह ऋण संवितरित होने के बाद जारी किए जाने वाले चुकौती कार्यक्रम में निर्दिष्ट किश्तों में ऋण और देय ब्याज की चुकौती करे और मांग किए जाने पर इस प्रकार के किश्तों की चुकौती बाध्यकारी होगी, यदि ऋणदाता द्वारा ऐसी आवश्यकता हो।<br/>");
                        sb.Append("iv. ऋणदाता के पास यह अधिकार होगा कि वह बिना कोई कारण बताये किसी भी समय अपने विवेकानुसार ऋण को रद्द करे|<br/>");
                        sb.Append("v. ऋणदाता द्वारा रखरखाव किए गए रिकॉर्ड/अभिलेख कर्जदार द्वारा देय राशि के निर्णायक सबूत होंगे।<br/>");
                        sb.Append("vi. ऋणदाता के किसी अधिकारी द्वारा हस्ताक्षरित लिखित प्रमाणपत्र जो किसी भी समय देय राशि के बारे में बताता है, कर्जदार के विरूद्ध निर्णायक सबूत होगा। हालांकि, यहाँ कोई भी चीज़ ऋणदाता के हितों को पूर्वाग्रह से ग्रसित नहीं कर सकती यदि परिकलित देय ब्याज और कर्जदार द्वारा देय राशि में कोई लिपिकीय या अंकगणित की त्रृटि हो।<br/>");
                        sb.Append("vii. कर्जदार लिखित में कम से कम 3 (तीन) दिनों की पूर्व नोटिस देकर ऋण का संपूर्ण बकाया शेष पुरोबंधित कर सकता है। ऐसी स्थिति में, ऋणदाता के पास पुरोबंध प्रभार लगाने का अधिकार होगा, जो ऋणदाता की नीतिनुसार के समय, इस प्रकार पुरोबंधित की गई धनराशि यानी उस समय ऋण खाते में मौजूद संपूर्ण बकाया शेष राशि पर लागू होगा। ब्याज (बदले गए ब्याज सहित) और कोई भी अन्य प्रभार इत्यादि उस महीने के अंत तक लगाए जाएंगे जिसमें ऋणपूर्वसमापननोटिस की अवधि समाप्त होती है।  ऋणपूर्वसमापनकेवल तभी प्रभाव में होगा जब वास्तविक भुगतान प्राप्त कर लिया जाएगा।<br/>");
                        sb.Append("viii. कर्जदार और सह-कर्जदार के पते में किसी भी प्रकार के बदलाव के बारे में, इस प्रकार के बदलाव होने के बाद एक सप्ताह के भीतर कर्जदा और सह-कर्जदार लिखित में ऋणदाता को सूचित करेंगे।<br/>");
                        sb.Append("ix. कर्जदार यह घोषणा करता है कि ऋण के लिए इस आवेदन में दी गई सारी जानकारी संपूर्ण रूप से सही है और या तो प्रत्यक्ष रूप से या अप्रत्यक्ष रूप से कोई गलतबयानी नहीं की गई है और यह कि इसमें किसी भी तरह की गलती या असत्यता को, अन्य बातों के साथ-साथ, पेशेवर कदाचार और ज़िम्मेदार आचार संहिता के लिए प्रतिकूल कृति समझा जाएगा।<br/>");
                        sb.Append("x. इस करार के प्रवर्तन के लिए या इस करार के अंतर्गत ऋण और सभी देय राशि की वसूली के लिए कर्जदार सभी शुल्क, प्रभार, लागत और अन्य व्यय का भुगतान करेगा|<br/>");

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
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%;border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:10px;'>");
                        sb.Append("xi. कर्जदार स्पष्ट रूप से मान्यता देता है और स्वीकार करता है कि  ऋणदाता को, स्वयं या अपने अधिकारियों या कर्मचारियों के माध्यम से इस प्रकार के उपाय करने के अपने अधिकारों के प्रति किसी भी पूर्वाग्रह के बिना, यह अधिकार होगा और संपूर्ण शक्ति और प्राधिकार होगा कि वह एक या उससे अधिक तीसरे पक्षों की नियुक्ति करे (ऋणदाता के लिए वसूली करने वाले सहयोगी, एजेंसियाँ), और इस प्रकार चुने गए तीसरे पक्ष को इस करार के अंतर्गत ऋण के प्रशासन से संबंधित सभी या अपने कुछ कार्य, अधिकार और शक्तियाँ सौंपे और समनुदेशित करे, जिसमें शामिल है इस करार के अंतर्गत ऋणदाता की ओर से कर्जदार से सभी बकाया किश्तों और कर्जदार द्वारा देय अन्य राशि की वसूली और प्राप्त करने का अधिकार और प्राधिकार, और इससे संबंधित सभी विधिमान्य कृतियों, विलेखों, विषयों और चीज़ों का निष्पादन करना और जो प्रासंगिक हो, जिसमें शामिल है उत्पाद का पुन:कब्जा प्राप्त करना, जो इस करार में कहीं अन्य जगह उल्लिखित है, नोटिस भेजना, कर्जदार से संपर्क करना, कर्जदार से नकद/चेक/ड्राफ्ट प्राप्त करना और कर्जदार को वैध और प्रभावी रसीदें और उन्मोचन प्रदान करना। उपरोक्त उद्देश्य के लिए ऋणदाता के पास यह अधिकार होगा कि वह इस प्रकार के तीसरे पक्ष के समक्ष कर्जदार और ऋण से संबंधित आवश्यक और उपयुक्त जानकारी प्रकट करे और एतदद्वारा कर्जदार ऋणदाता द्वारा ऐसे प्रकटन के लिए सहमति प्रदान करता है।<br/>");
                        sb.Append("xii. इच्छात्मकडीफ़ल्ट:<br/>");
                        sb.Append("<ul>");
                        sb.Append("<li>ऋणदाता को प्रकट की गई सीमा को छोड़कर: (ए) किसी भी सहयोगी या समूह कंपनियों (यदि लागू हो) के साथ उधारकर्ता के सभी अनुबंध या समझौते, या किसी भी प्रतिबद्धता के आधार पर हैं; और (बी) कोई भी निदेशक/भागीदार/सदस्य/न्यासी/प्रभारी व्यक्ति और उधारकर्ता के मामलों के प्रबंधन के लिए जिम्मेदार, जैसा भी मामला हो, को आरबीआई या किसी क्रेडिट सूचना कंपनी या निर्यात क्रेडिट गारंटी निगम द्वारा इरादतन चूककर्ता घोषित नहीं किया गया है;</li>");
                        sb.Append("<li>उधारकर्ता पुष्टि करता है कि कोई भी निदेशक/भागीदार/न्यासी/सदस्य/प्रभारी व्यक्ति और उधारकर्ता के मामलों के प्रबंधन के लिए जिम्मेदार निदेशक/भागीदार/न्यासी/सदस्य/प्रभारी व्यक्ति नहीं है और किसी कंपनी/फर्म/ट्रस्ट/सोसाइटी/एसोसिएशन ऑफ पर्सन्स में मामलों के प्रबंधन के लिए जिम्मेदार नहीं है जिसे आरबीआई/किसी क्रेडिट सूचना कंपनी या किसी नियामक प्राधिकरण द्वारा इरादतन चूककर्ता के रूप में पहचाना गया है।</li>");
                        sb.Append("<li>उधारकर्ता निदेशक/प्रवर्तक/भागीदार/न्यासी/सदस्य/प्रभारी व्यक्ति की हैसियत से किसी ऐसे व्यक्ति को शामिल नहीं करेगा जो उधारकर्ता के मामलों के प्रबंधन के लिए जिम्मेदार हो और जो निदेशक/भागीदार/सदस्य/न्यासी/प्रभारी व्यक्ति हो और किसी कंपनी/फर्म/व्यक्तियों के संघ/न्यास/सोसायटी, जैसा भी मामला हो, के मामलों के प्रबंधन के लिए जिम्मेदार हो,  विलफुल डिफॉल्टर के रूप में पहचानी गई। यदि ऐसा व्यक्ति निदेशक/भागीदार/सदस्य/न्यासी/प्रभारी व्यक्ति पाया जाता है और इरादतन चूककर्ता के रूप में पहचाने गए कंपनी/फर्म/व्यक्तियों के संघ/न्यास के मामलों के प्रबंधन के लिए जिम्मेदार पाया जाता है, तो उधारकर्ता ऐसे व्यक्ति को हटाने के लिए शीघ्र और प्रभावी कदम उठाएगा।</li>");
                        sb.Append("<li>उधारकर्ता समझता है कि यदि उधारकर्ता या उसके निदेशक/प्रवर्तक/भागीदार/न्यासी/सदस्य/प्रभारी व्यक्ति और उधारकर्ता के मामलों के प्रबंधन के लिए जिम्मेदार व्यक्ति को इरादतन चूककर्ता के रूप में वर्गीकृत किया जाता है, तो इरादतन चूककर्ताओं के संबंध में प्रतिबंध जैसा कि आरबीआई के 30 जुलाई के इरादतन चूककर्ताओं और बड़े चूककर्ताओं के उपचार पर मास्टर निदेश के तहत निर्दिष्ट है,  2024, समय-समय पर संशोधित और संशोधित, लागू होगा।</li>");

                        sb.Append("</ul>");
                        sb.Append("xiii. लेखा परीक्षकों की भूमिका: निधियों के अंतिम उपयोग की निगरानी की दृष्टि से, उधारकर्ता ऋणदाता को एक अंतिम उपयोग प्रमाण पत्र प्रस्तुत करेगा, जो ऋणदाता के लिए संतोषजनक रूप और तरीके से होगा, जिसमें अन्य बातों के साथ-साथ यह प्रमाणित होगा कि सुविधा का कोई विपथन/साइफनिंग नहीं हुई है। उधारकर्ता एतद्द्वारा ऋणदाता को सहमति देता है - (ए) या तो उधारकर्ता के लेखा परीक्षक की आवश्यकता होती है; या (बी) एक उधारदाताओं के लेखा परीक्षक को नियुक्त करता है- यह पुष्टि करने के लिए कि सुविधा का कोई डायवर्जन / उधारदाताओं का लेखा परीक्षक सुविधा के अंतिम उपयोग को प्रमाणित करने के लिए यथोचित आवश्यक जानकारी पूछ सकता है और उधारकर्ता ऐसे डेटा को साझा करने का वचन देता है। यदि बैंक द्वारा उधारकर्ताओं की ओर से खातों में कोई जालसाजी, गलत बयानी या हेरफेर पाया जाता है, और लेखा परीक्षक लेखापरीक्षा करने में लापरवाह या कमी करते पाए जाते हैं, तो बैंक को उधारकर्ताओं के सांविधिक लेखा परीक्षकों के खिलाफ अनुशासनात्मक कार्रवाई के लिए उपयुक्त प्राधिकारी के साथ औपचारिक शिकायत दर्ज करने का अधिकार होगा। बैंक लेखा परीक्षकों को निर्धारित प्रक्रियाओं का पालन करते हुए सुनवाई का अवसर भी प्रदान करेगा, जिसे रिकॉर्ड भी किया जाएगा।<br/>");

                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 21th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%;border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:10px;'>");

                        sb.Append("<b>q)</b> समनुदेशन:<br/>");
                        sb.Append("i. कर्जदार स्पष्ट रूप से मान्यता देता है और स्वीकार करता है कि ऋणदाता कर्जदार की पूर्व-सहमति के बिना और ऐसी शर्तों पर जिसका निर्णय ऋणदाता ले सकता है, संपूर्ण रूप से या आंशिक रूप से अपने सभी या कुछ अधिकारों, लाभों या दायित्वों को किसी भी पक्ष को बेच, हस्तांतरित, या समनुदेशित कर सकता है, इसमें इस करार के अंतर्गत देय किसी भी राशि के भुगतान में चूक की घटना में इस प्रकार के खरीददार, हस्तांतरिती या समनुदेशिती की ओर से कर्जदार के विरूद्ध कार्यवाही करने का अधिकार सुरक्षित रखना भी शामिल है। इस प्रकार की कोई भी बिक्री, हस्तांतरण या समनुदेशन कर्जदार पर बंधनकारक होंगे और कर्जदार ऐसे खरीददार, हस्तांतरिती या समनुदेशिती को ऋणदाता के साथ अपना एकमात्र ऋणप्रदाता या संयुक्त रूप से ऋणप्रदाता स्वीकार करेगा और ऐसी स्थिति में इस करार के अंतर्गत कर्जदार द्वारा देय बकाया राशि का भुगतान कर्जदार की ओर से ऋणदाता या ऐसे ऋणप्रदाता को या ऋणदाता द्वारा दिए गए निर्देश के अनुसार करना होगा।    किसी भी तरह का खर्चा, चाहे इस प्रकार की बिक्री, हस्तांतरण या समनुदेशन या प्रवर्तन के अधिकारों और बकाया और देय राशि की वसूली के लिए हो, कर्जदार के खाते से किया जायेगा।<br/>");
                        sb.Append("ii. कर्जदार के पास यह अधिकार नहीं होगा कि वह यहाँ दिए गए दायित्वों को हस्तांतरित या समनुदेशित कर सके।<br/>");

                        sb.Append("iii. कर्जदार एतदद्वारा ऋणदाता को कर्जदार के लिए पूर्व-नोटिस और संदर्भ के बिना निम्नलिखित के लिए स्वीकृति देता है और स्पष्ट रूप से प्राधिकृत करता है:<br/>");
                        sb.Append("  1.ऋणदाता के प्रधान कार्यालय या ऋणदाता की किसी अन्य शाखा या किसी सहायक कंपनी को कर्जदार के संबंध मंव कोई भी ऋण की जानकारी या अन्य जानकारी प्रकट करने के लिए। <br/>");
                        sb.Append("  2.किसी भी समय तीसरे पक्षों (बैंकों, वित्तीय संस्थाओं, क्रेडिट ब्यूरो, सांविधिक और विनियामक प्राधिकरणों सहित) के साथ ऋणदाता को ज्ञात सभी और ऋण और/या कर्जदार से संबंधित (कर्जदार का ऋण इतिहास और ऋण स्थिति सहित) जानकारी साझा करने के लिए, जो ऋणदाता के विचार में आवश्यक हो या जैसा अनुरोध या निर्देशित किया गया हो;<br/>");
                        sb.Append("  3.यह कि, भुगतान में चूक होने की स्थिति में या उस मामले में जहाँ ऋण के किसी भाग या इससे संबंधित अन्य देय राशि से संबंधित चुकौती में कर्जदार द्वारा चूक हुई हो, तो ऋणदाता को यह अधिकार होगा (और कर्जदार स्पष्ट रूप से ऋणदाता को ऐसा करने के लिए सहमति प्रदान करता है) कि वह किसी भी सांविधिक संस्था या विनियामक प्राधिकरण या भारतीय रिज़र्व बैंक, या किसी अन्य संघ, जिसका ऋणदाता सदस्य हो, को व्यतिक्रमी (डिफाल्टर) के रूप में कर्जदार के नाम का प्रकटन कर सकता है।<br/>");
                        sb.Append("<b>r)</b> विवाद समाधान:<br/>");
                        sb.Append("ऐसी स्थिति में जहाँ करार या ऋण के संबंध में कर्जदार और ऋणदाता के बीच कोई विवाद हो, तो यह एकमात्र मध्यस्थ के निर्णय के अधीन होगा, जिसकी नियुक्ति ऋणदाता द्वारा की जाएगी। मध्यस्थता का स्थान मुंबई होगा। मध्यस्थता अंग्रेजी भाषा में संचालित की जाएगी। मध्यस्थ के पास अंतरिम अधिनिर्णय या आदेश जारी करने की शक्ति होगी और मध्यस्थता और सुलह अधिनियम, 1996 या किसी अन्य कानून, जो मध्यस्थता के लिए संदर्भित विवाद के समय पर लागू होगा, के अनुसार कार्यवाही संचालित होगी।<br/>");
                        sb.Append("<b>s)</b> अन्य सुसंगत खंड:<br/>");
                        sb.Append("i. यह करार और इसके साथ गारंटीकर्ता का पत्र पक्षों के बीच संपूर्ण करार के रूप में माना जाएगा।<br/>");
                        sb.Append("ii. इस करार की वैधता, व्याख्या या प्रदर्शन से संबंधित सभी सवाल भारतीय कानूनों द्वारा शासित होंगे और महाराष्ट्र और मुंबई में स्थित अदालतों के क्षेत्राधिकार में होंगे। <br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 22th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%;order-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:10px;'>");


                        sb.Append("iii. यह माना जाता है कि कर्जदार इस करार का निष्पादन स्वेच्छा से और इस करार के संपूर्ण निहितार्थों को समझने के बाद और उचित कानूनी सलाह प्राप्त करने के बाद कर रहा है।<br/>");
                        sb.Append("iv. इस करार के अंतर्गत उपाय संचयी होंगे और अनन्य नहीं होंगे, और एक उपाय का चयन करने पर अन्य उपायों के आगे की कार्रवाई पर रोक नहीं लगेगी।<br/>");
                        sb.Append("v. ऐसी स्थिति में जब यहाँ निहित एक या इससे अधिक प्रावधानों को  किसी सक्षम क्षेत्राधिकार वाली अदालत द्वारा किसी भी संबंध में अवैध, ग़ैर-कानूनी या अप्रवर्तनीय ठहराया गया हो, तो यहाँ निहित शेष प्रावधानों की वैधता, विधिमान्यता और प्रवर्तनीयता किसी भी तरह से प्रभावित या ह्रासित नहीं होगी। इतना ही नहीं, ऐसे व्यक्तियों के मामले में जो संयुक्त कर्जदार हैं, दोनों ही यहाँ निहित सभी दायित्वों की पूर्तता के लिए संयुक्त रूप से और पृथक रूप से ज़िम्मेदार होंगे और “कर्जदार” शब्द का मतलब उसी के अनुसार समझा जाएगा। <br/>");
                        sb.Append("vi. एक दूसरे के विरूद्ध किसी भी अधिकार या दावे को प्रवर्तित करने में किसी भी पक्ष की विफलता या सहनशीलता को इस प्रकार के अधिकार या दावे या किसी भी अधिकार या दावे को धारण करने वाले पक्ष द्वारा अधित्याग/छूट नहीं समझा जाएगा। किसी भी उल्लंघन का पार्टी द्वारा अधित्याग का परिचालन या इसी या किसी अन्य प्रावधान के इसके बाद होने वाले उल्लंघनों का अधित्याग नहीं समझा जाएगा।<br/>");
                        sb.Append("vii. खंड शीर्षकों को केवल सुविधा के लिए शामिल किया गया है और इसका उपयोग इस करार का अर्थ समझने या व्याख्या करने के लिए नहीं होना चाहिए।<br/>");
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
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;'>प्रमुख तथ्य विवरण (केएफएस)</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>दिनांक: {0}</td>", Convert.ToString(dtSancLetter.Rows[0]["FinalSanctionDate"]));
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='width:100%;' >विनियमित संस्था/ ऋण दाता का नाम: यूनिटी स्मॉल फाइनेंस बैंक लिमिटेड</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>उधारकर्ता का नाम और पता: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["MemberName"]));
                        sb.Append("</tr>");
                        sb.Append("<tr><td>&nbsp;</td></tr>");
                        sb.Append("<tr><td style='font-weight:bold;text-align:center;width:100%;'>भाग 1 (ब्याज दर और फीस/शुल्क)</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td style='padding:5px; width:5%;text-align:center;'>1</td>");
                        sb.Append("<td style='padding:5px; width:22%;'>ऋण प्रस्ताव / खाता क्र.</td>");
                        sb.AppendFormat("<td style='padding:5px; width:25%;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanNo"]));
                        sb.Append("<td style='padding:5px; width:22%;'>ऋण का प्रकार</td>");
                        sb.AppendFormat("<td colspan='4' style='padding:5px; width:26%;'>{0}</td>", "MEL Loan");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'>2</td>");
                        sb.Append("<td colspan='2' style='padding:5px;'> संस्वीकृत ऋण राशि ( रूपए में)</td>");
                        sb.AppendFormat("<td colspan='5' style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanAmt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'>3</td>");
                        sb.Append("<td colspan='3'>संवितरण अनुसूची<br/>(i) चरणों में संवितरण या 100% अग्रिम<br/>(ii) यदि यह चरणवार है, तो ऋण करार के उस खंड का उल्लेख करें जिसमें सुसंगत विवरण उपलब्ध है।</td>");
                        sb.AppendFormat("<td colspan='4' style='padding:5px;text-align:center;'>{0}</td>", "100% Upfront");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'>4</td>");
                        sb.Append("<td colspan='2' >4 ऋण की अवधि (वर्ष/महीने/दिन)</td>");
                        sb.AppendFormat("<td colspan='5' style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["Tenure"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'>5</td>");
                        sb.Append("<td colspan='7' style='text-align:center;' >किस्त का विवरण</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='3' style='text-align:center;' >किस्त का प्रकार</td>");
                        sb.Append("<td style='text-align:center;' >ईएमआई की संख्या</td>");
                        sb.Append("<td style='text-align:center;' >ईएमआई (₹)</td>");
                        sb.Append("<td colspan='3' style='text-align:center;' >पेमेंट की शुरूआत, स्वीकृति के बाद</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='3' style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["RepFreq"]));
                        sb.AppendFormat("<td  style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInstNo"]));
                        sb.AppendFormat("<td  style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["EMIAmt"]));
                        sb.AppendFormat("<td colspan='3' style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["FInstDt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'>6</td>");
                        sb.Append("<td colspan='3' style='text-align:center;' >ब्याज दर (%) और प्रकार (निश्चितया अस्थिर या हाइब्रिड)</td>");
                        sb.AppendFormat("<td colspan='4' style='padding:5px;text-align:center;'>{0}(Fixed)</td>", Convert.ToString(dtScheduleOwn.Rows[0]["IntRate"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'>7</td>");
                        sb.Append("<td colspan='7' style='text-align:center;' >ब्याज की अस्थिर दर के मामले में अतिरिक्त जानकारी </td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;' >कसौटी</td>");
                        sb.Append("<td style='text-align:center;' >बेंचमार्क दर (%) (बी)</td>");
                        sb.Append("<td style='text-align:center;' >फैलना दर प्रतिशतता (%) (एस)</td>");
                        sb.Append("<td style='text-align:center;' >अंतिम दर प्रतिशत (%) = (बी) + (एस)</td>");
                        sb.Append("<td colspan='2' style='text-align:center;' >पुनर्निधारण आवधिकता (महीने)</td>");
                        sb.Append("<td colspan='2' style='text-align:center;' >संदर्भ बेंचमार्क में परिवर्तन का प्रभाव (‘आर’में 25 आधार अंक परिवर्तन के लिए,ईएमआईमें परिवर्तन:)</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;' ><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' ><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' ><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' ><br><br>NA</td>");
                        sb.Append("<td style='text-align:center;' >बी <br><br> NA</td>");
                        sb.Append("<td style='text-align:center;' >एस <br><br> NA</td>");
                        sb.Append("<td style='text-align:center;' >ईएमआई (₹) <br> NA</td>");
                        sb.Append("<td style='text-align:center;' >ईएमआई (₹) <br> NA</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'>8</td>");
                        sb.Append("<td colspan='7' style='text-align:center;' >फीस/शुल्क</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");


                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 24th_Page

                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='padding:5px; width:25%;'></td>");
                        sb.Append("<td colspan='2' style='padding:5px; width:35%;'>विनियमित संस्था (आरई) (ए) के लिए देय</td>");
                        sb.Append("<td colspan='2' style='padding:5px; width:40%;'>विनियमित संस्था (आरई) (बी) के माध्यम से तृतीय पक्ष को देय</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td style='padding:5px;text-align:center;'></td>");
                        sb.Append("<td style='padding:5px;text-align:center;'></td>");
                        sb.Append("<td style='padding:5px;text-align:center;'>एकबारगी/ आवर्ती </td>");
                        sb.Append("<td style='padding:5px;text-align:center;'>राशि (₹ में) याप्रतिशत (%) जैसा लागू हो</td>");
                        sb.Append("<td style='padding:5px;text-align:center;'>एकबारगी/ आवर्ती</td>");
                        sb.Append("<td style='padding:5px;text-align:center;'>राशि (₹ में) याप्रतिशत (%) जैसा लागू हो</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(i)</td>");
                        sb.Append("<td style='padding:5px;text-align:left;'>प्रोसेसिंग फीस</td>");
                        sb.Append("<td style='padding:5px;text-align:center;'><br/>One Time</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>(जीएसटी सहित)<br/>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["procfee"]));
                        sb.Append("<td style='padding:5px;text-align:center;'><br/>One Time</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>(जीएसटी सहित)<br/>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["procfeeBC"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(ii)</td>");
                        sb.Append("<td style='padding:5px;text-align:left;'>बीमा शुल्क</td>");
                        sb.Append("<td style='padding:5px;text-align:center;'><br/>One Time</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;' >(जीएसटी सहित)<br/>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInsurance"]));
                        sb.Append("<td style='padding:5px;text-align:center;'><br/>One Time</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>(जीएसटी सहित)<br/>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInsurance"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(iii)</td>");
                        sb.Append("<td  style='padding:5px;text-align:left;'>मूल्यांकन शुल्क </td>");
                        sb.Append("<td colspan='4' style='padding:5px;text-align:center;'>No Fees</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(iv)</td>");
                        sb.Append("<td style='padding:5px;text-align:left;'>कोई अन्य (कृपयास्पष्ट रूप से बताएं)</td>");
                        sb.Append("<td style='padding:5px;text-align:center;'></td>");
                        sb.Append("<td style='padding:5px;' ></td>");
                        sb.Append("<td style='padding:5px;text-align:center;'></td>");
                        sb.Append("<td style='padding:5px;'></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'></td>");
                        sb.Append("<td style='padding:5px;text-align:left;'>कुल शुल्क</td>");
                        sb.Append("<td style='padding:5px;text-align:center;'>One Time</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;' >{0}</td>", Convert.ToString(vTotalPayRE));
                        sb.Append("<td style='padding:5px;text-align:center;'>One Time</td>");
                        sb.AppendFormat("<td style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(vTotalPayTP));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>9</td>");
                        sb.Append("<td colspan='2' style='padding:5px;text-align:center;'>वार्षिक प्रतिशत दर (एपीआर) (%)</td>");
                        sb.AppendFormat("<td colspan='3' style='padding:5px;text-align:center;'>{0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["IRRIC"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>10</td>");
                        sb.Append("<td colspan='5' style='padding:5px;text-align:center;'>आकस्मिक शुल्कका ब्यौरा ( ₹ या % में, जैसा लागू हो) **</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(i)</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>दंडात्मक शुल्क, यदि कोई हो, भुगतान में देरी के मामले में</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "3% + 18% GST");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(ii)</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>अन्य दंडात्मक शुल्क, यदि कोई हो</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>A</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>ईएमआई बाउंस होने पर शुल्क (जीएसटी सहित)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "Rs. 590 (Incl. GST)");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>B</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>विस्तार शुल्क (जीएसटी सहित)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>C</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>विज़िट शुल्क (जीएसटी सहित)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "Rs. 236 (Incl. GST)");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(iii)</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>पुरोबंधात्मक शुल्क, यदि लागू हो ** (जीएसटी सहित)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "3% of OS (Allowed after 1st EMI paid) + GST");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(iv)</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>ऋण को निश्चितदर से अस्थिर दर में बदलने के लिए शुल्कऔर इसकाउल्टा</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;' >{0}</td>", "NA");
                        sb.Append("</tr>");


                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>(v)</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>कोई अन्य शुल्क(कृपया स्पष्ट रूप सेबताएं)</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>a</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>पूर्व-भुगतान शुल्क**</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "NA");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;text-align:center;'>b</td>");
                        sb.Append("<td colspan='3' style='padding:5px;text-align:center;'>ऋणपूर्वसमापन शुल्क</td>");
                        sb.AppendFormat("<td colspan='2' style='padding:5px;text-align:center;'>{0}</td>", "कुलबकायामूलधनपर 3%");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        sb.Append("<p>*शुल्कपर जीएसटी लागू होगा</p>");

                        #endregion
                        // END - KFS-2

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // Address Declaration
                        #region 25th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;font-weight:bold;'>पते के संबंध में घोषणा</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>दिनांक: {0}</td>", Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanDt"]));
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='width:100%;' >प्रति,</td></tr>");
                        sb.Append("<tr><td style='width:100%;' >यूनिटी स्मॉल फाइनेंस बैंक लिमिटेड</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>शाखा-: {0}</td>", Convert.ToString(dtAppFrm1.Rows[0]["ApplDepn1Name"]));
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='width:100%;'>विषय: <span style='text-decoration: underline;font-weight:bold;'>वर्तमान पते की घोषणा</span></td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td style='width:100%;' >प्रिय महोदय,</td></tr>");
                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:10px;'>मैं <span style='border-bottom: 1px dotted black;'>{0}</span> वर्तमान में <span style='border-bottom: 1px dotted black;'>{1}</span></td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["FullName"]), Convert.ToString(dtAppFrm1.Rows[0]["ApplPAddress1"]));

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:10px;'>में रहता/ती हूँ, यह घोषित करता/ती हूँ कि मेरे पास ऊपर उल्लिखित वर्तमान पते का सबूत नहीं है। कृपया इस पत्र को मेरे वर्तमान पते के सबूत की घोषणा के रूप में स्वीकार करें और कृपया ऊपर उल्लिखित पते पर सभी पत्र भेजें।</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<tr><td style='padding:10px;'>आवेदक के हस्ताक्षऱ _____________________</td></tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // Form60
                        #region 26th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;font-weight:bold;'>प्रपत्र-60</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td>ऐसे व्यक्ति द्वारा भरा जाने वाला घोषणा प्रपत्र जिसके पास स्थायी खाता संख्या (पैन) नहीं है और जो किसी ऐसे लेन-देन में शामिल है जिसे नियम 114बी/प्रपत्र 61 में निर्दिष्ट किया गया है, ऐसे व्यक्ति द्वारा भरा जाए जो कृषि आय अर्जित करता हो और जो कोई ऐसी अन्य आय प्राप्त नहीं करता है जो नियम 114 बी के खंड (ए) से (एच) तक में निर्दिष्ट किए गए लेन-देन के संबंध में आय कर के लिए प्रभार्य हो</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>घोषणाकर्ता का पूरा नाम व पता <span style='border-bottom: 1px dotted black;'>{0} , {1}</span></td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["FullName"]), Convert.ToString(dtAppFrm1.Rows[0]["ApplPAddress1"]));
                        sb.AppendFormat("<tr><td style='width:100%;'>लेन-देन की राशि: <span style='border-bottom: 1px dotted black;'>{0}</span></td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanAmount"]));
                        sb.AppendFormat("<tr><td style='width:100%;'>जन्म तिथि <span style='border-bottom: 1px dotted black;'>{0}</span></td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["ApplDOB"]));
                        sb.AppendFormat("<tr><td style='padding:5px;'>क्या आपका कर-निर्धारण किया गया है? &nbsp;&nbsp; हाँ ☐ &nbsp;&nbsp;&nbsp;&nbsp; नहीं ☑ &nbsp;&nbsp;&nbsp;&nbsp; यदि हाँ, <span style='border-bottom: 1px dotted black;'>{0}</span></td></tr>", "");
                        sb.AppendFormat("<tr><td style='width:100%;'>a) उस वार्ड/मंडल/क्षेत्र का विवरण भरें जहाँ अंतिम आय़ विवरणी दाखिल की गई हो: <span style='border-bottom: 1px dotted black;'>{0}</span></td></tr>", "");
                        sb.AppendFormat("<tr><td style='width:100%;'>b) पैन ना होने का कारण: <span style='border-bottom: 1px dotted black;'>{0}</span></td></tr>", "");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td style='width:100%;' >सत्यापन:</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>मैं <span style='border-bottom: 1px dotted black;'>{0}</span> एतदद्वारा यह घोषित करता/ती हूँ कि मेरे द्वारा दी गई जानकारी सही है। आज <span style='border-bottom: 1px dotted black;'>{1}</span> के <span style='border-bottom: 1px dotted black;'>{2}</span> दिन <span style='border-bottom: 1px dotted black;'>{3}</span> को सत्यापित।</td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["FullName"]), Convert.ToString(dtAppFrm1.Rows[0]["AppYear"]), Convert.ToString(dtAppFrm1.Rows[0]["AppMonth"]), Convert.ToString(dtAppFrm1.Rows[0]["AppDay"]));
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:10px;font-weight:bold;'>हस्ताक्षर _____________________</td></tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // ScheduleCharges
                        #region 27th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;font-weight:bold;'>प्रभार की अनुसूची</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>मैं/हम प्रभार को स्वीकार करता(ती) हूँ/करते हैं</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:5px;width:50%;'>कर्जदार {0}</td>", "");
                        sb.AppendFormat("<td style='padding:5px;width:50%;'>सह-कर्जदार{0}</td>", "");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:5px;'>1.नाम: {0}</td>", Convert.ToString(dtScheduleOfCharges.Rows[0]["Borrower"]));
                        sb.AppendFormat("<td style='padding:5px;'>2.नाम: {0}</td>", Convert.ToString(dtScheduleOfCharges.Rows[0]["CoBorrower"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:5px;'>हस्ताक्षर: {0}</td>", "");
                        sb.AppendFormat("<td style='padding:5px;'>हस्ताक्षर: {0}</td>", "");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:5px;'>स्थान: {0}</td>", "");
                        sb.AppendFormat("<td style='padding:5px;'>स्थान: {0}</td>", "");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:5px;'>दिनांक: {0}</td>", Convert.ToString(dtScheduleOfCharges.Rows[0]["SanctionDate"]));
                        sb.AppendFormat("<td style='padding:5px;'>दिनांक: {0}</td>", Convert.ToString(dtScheduleOfCharges.Rows[0]["SanctionDate"]));
                        sb.Append("</tr>");

                        sb.Append("<tr><td colspan='2' >&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td colspan='2' >प्रभारों का वर्णन</td></tr>");
                        sb.Append("<tr><td colspan='2' >&nbsp;&nbsp;</td></tr>");

                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;width:60%;'>प्रसंस्करण शुल्क</td>");
                        sb.Append("<td  style='padding:5px;width:40%;'>3% + जीएसटी</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>अपर्याप्त निधि</td>");
                        sb.Append("<td  style='padding:5px;'>500 + जीएसटी</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>ऋण विवरण प्रभार (प्रति छमाही के 1 विवरण के शून्य)</td>");
                        sb.Append("<td  style='padding:5px;'>100 + जीएसटी</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>एनओसी निर्गमन प्रभार (पहली एनओसी के लिए शून्य)</td>");
                        sb.Append("<td  style='padding:5px;'>100 + जीएसटी</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>बैंक को देय चुकौती बकाया राशि (मूलधन, ब्याज, लागत, शुल्क, कर, व्यय, आदि सहित) में चूक या देरी के लिए</td>");
                        sb.Append("<td  style='padding:5px;'>3% प्रतिमाह (36% प्रतिवर्ष) + बकायादिनोंपरअतिदेयराशिपर 18% जीएसटी।</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>डिफ़ॉल्टकीभीघटना (अपर्याप्तफंडऔरविज़िटशुल्क)</td>");
                        sb.Append("<td  style='padding:5px;'>न्यूनतम INR 500/- <br/> (भारतीयरुपयापांचसौकेवल) <br/> औरअधिकतम INR 700/- <br/> (भारतीयरुपयासातसौकेवल) + जीएसटी</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td  style='padding:5px;'>पूर्वभुगतानशुल्क:</td>");
                        sb.Append("<td  style='padding:5px;'>कुलबकायामूलधनपर 3%</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        //Vernacular Language
                        #region 28th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:center;width:100%;font-weight:bold;'>स्थानीय भाषा में घोषणा</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>मैं/हम, एतदद्वारा निवेदन और घोषणा करता(ती) हूँ/ करते हैं कि:</td></tr>");
                        sb.Append("<tr><td>&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>मुझे/हमें ऋण प्राप्त करने से जुड़े संपूर्ण ऋण दस्तावेजों की विषय वस्तु मुझे/हमें ज्ञात भाषा में नीचे प्रतिहस्ताक्षर करनेवाले श्री <span style='border-bottom: 1px dotted black;'>{0}</span> के द्वारा पढ़कर सुनाई और समझाई गई है।</td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["FullName"]));
                        sb.Append("<tr><td>&nbsp;</td></tr>");
                        sb.Append("<tr><td style='width:100%;' >मैंने/हमने इसमें उल्लिखित सभी नियमों और शर्तों को समझने के बाद ऋण दस्तावेज़ों पर हस्ताक्षर किए हैं।</td></tr>");
                        sb.Append("<tr><td>&nbsp;</td></tr>");
                        sb.Append("<tr><td style='width:100%;'><span style='text-decoration: underline;font-weight:bold;'>नाम: </span>श्री/श्रीमती</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'><span style='border-bottom: 1px dotted black;'>{0} , {1}</span></td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["FullName"]), Convert.ToString(dtAppFrm1.Rows[0]["CoFullName"]));
                        sb.Append("<tr><td  style='width:100%;font-weight:bold;'>(कर्जदार/सह-कर्जदार/जमानतदार इत्यादि) के हस्ताक्षर</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;&nbsp;&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>नाम: <span style='border-bottom: 1px dotted black;'>{0}</span> (यूनिटी स्मॉल फाइनेंस बैंक का व्यक्ति)</td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["EoName"]));
                        sb.AppendFormat("<tr><td style='width:100%;'>पता: <span style='border-bottom: 1px dotted black;'>{0}</span> (यूनिटी स्मॉल फाइनेंस बैंक शाखा का पता)</td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["BranchAddress"]));
                        sb.Append("<tr><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td style='text-align: left;'><hr style='border: none; border-top: 1px dotted #000; width: 30%; margin: 0;'></td></tr>");
                        sb.Append("<tr><td style='padding:10px;'>सिग्नेचर</td></tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        //Annexure-2 (CAM-3)
                        #region 29th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='font-weight:bold;text-align:left;width:100%;font-weight:bold;'>अनुलग्नक 2</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>ऋण का अंतिम उपयोग- घोषणा</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td  style='text-align:left;width:100%;'>तिथि: {0}</td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanDt"]));
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>प्रिय महोदय,</td></tr>");
                        sb.Append("<tr><td>&nbsp;</td></tr>");
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>विषय: माल खरीदने/ दुकान के</td></tr>");
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>रखरखाव/ सामान्य व्यापार उद्देश्य के लिए एमईएल सरल व्यापार ऋण हेतु आवेदन।</td></tr>");
                        sb.Append("<tr><td>&nbsp;</td></tr>");
                        sb.AppendFormat("<tr><td style='width:100%;'>मैं, <span style='border-bottom: 1px dotted black;'>{0}</span> देखें आवेदन क्र. <span style='border-bottom: 1px dotted black;'>{1}</span> तारीख <span style='border-bottom: 1px dotted black;'>{2}</span> य़ूएसएफबी लि. में प्रस्तुत: यूनिटी स्मॉल फाइनेंस बैंक से ऋण सुविधा (सुविधा) प्राप्त करने के लिए।</td></tr>", Convert.ToString(dtAppFrm1.Rows[0]["FullName"]), Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanApplNo"]), Convert.ToString(dtAppFrm1.Rows[0]["ApplLoanDt"]));
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>उक्त सुविधा माल की खरीदारी/ दुकान के रखरखाव/ सामान्य व्यापार उद्देश्य के लिए है। मैं यह भी घोषित करता/ती हूँ कि सुविधा के अंतर्गत दिए गए निधि का उपयोग किसी भी स्वरूप में सोना खरीदने के लिए नहीं किया जाएगा, जिसमें शामिल है प्राथमिक सोना, स्वर्ण बुलियन, स्वर्ण-आभूषण, सोने के सिक्के, गोल्ड एक्सचेंजट्रेडेड फंड (ईटीएफ) के यूनिट्स, गोल्ड म्यूचुअल फंड के यूनिट्स, रियल एस्टेट इत्यादि।</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td>मैं एतदद्वारा प्रतिनिधित्व करता/ती हूँ, गांरटी देता/ती हूँ और पुष्टि करता/ती हूँ कि पूर्वोक्त उद्देश्य एक वैध उद्देश्य है और साथ ही स्वीकृति और वचन देता/ती हूँ कि इस सुविधा का उपयोग केवल ऊपर उल्लिखित उद्देश्य के लिए किया जाएगा और यह कि इस सुविधा का उपयोग किसी गैर-कानूनी और/या असामाजिक और/या सट्टे के प्रयोजन के लिए नहीं किया जाएगा, जिसमें अन्य चीजों के साथ-साथ शेयर बाज़ार, आईपीओ और ज़मीन खरीद में सहभागिता भी शामिल हैं।</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td>मैं इसके आगे स्वीकृति देता/ती हूँ, पुष्टि करता/ती हूँ और वचन देता/ती हूँ कि सुविधा के अंतर्गत निधि के उपयोग का उद्देश्य सुविधा की अवधि के दौरान किसी भी तरह से बदला नहीं जाएगा; या यह कि उद्देश्यमें इस प्रकार का बदलाव केवल यूनिटी स्मॉल फाइनेंस बैंक की पूर्व लिखित अनुमति के साथ ही किया जाएगा।</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td>मैं सहमत हूँ कि सभी या किसी भी पूर्वोक्त वचनबंधका उल्लंघन या उसके अनुपालन में की जानेवाली चूक लेनदेन दस्तावेजों के अंतर्गत व्यतिक्रम की स्थिति निर्माण करेगी। धन्यवाद,</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td>भवदीय,</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td>आवेदक</td></tr>");
                        sb.Append("<tr><td>&nbsp;&nbsp;</td></tr>");
                        sb.Append("<tr><td>सह-आवेदक </td></tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // Repayment Schedule
                        #region 30th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td class='xsmall_font' style='text-align:center;font-weight:bold;' >पंजीकृत कार्यालय:50, बसंत लोक, वसंत विहार, नई दिल्ली, भारत – 110057</td></tr>");
                        sb.Append("<tr><td class='xsmall_font' style='text-align:center;font-weight:bold;' > मुख्य कार्यालय:यूनिटी स्मॉल फाइनेंस बैंक लिमिटेड, 5वीं और 6वीं मंजिल, टॉवर - 1, एल एंड टी सिवूड्स टॉवर, प्लॉट नंबर – आर1, सेक्टर - 40, सिवूड्स रेलवे स्टेशन, नवी मुंबई - 400706</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='xsmall_font'  cellspacing='0' cellpadding='2' style='border-collapse:collapse; border:1px solid black; width:100%;'>");

                        sb.Append("<tr style='border:none;'>");
                        sb.Append("<th colspan='2' style='border:1px solid black;'>पुनर्भुगतान अनुसूची</th>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;width:55%;border:none;'>शाखा: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["Branch"]));
                        sb.AppendFormat("<td style='padding:3px;width:45%;border:none;'>{0}</td>", "");
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>ग्राहक का नाम: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["MemberName"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>ऋण चक्र: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanCycle"]));
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:5px;border:none;'>सह-आवेदक का नाम: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["CoApplicantName"]));
                        sb.AppendFormat("<td style='padding:5px;border:none;'>बितरण तिथि: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanDt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>ऋण राशि (रू.): {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanAmt"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>बितरण ऋण संख्या: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanNo"]));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>कुल ब्याज (रू.): {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["IntAmt"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>किस्ती की संख्या: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["TotalInstNo"]));
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>प्रसेसकरण शुल्क + जीएसटी (रू.): {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["ProcFees"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>अंतिम किस्त की तिथि: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LInstDt"]));
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>बीमाकर्ता का नाम(जीवन बीमा): {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["ICName"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>भुगतान आवृत्ति: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["RepFreq"]));
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>बीमा + जीएसटी (रू.): {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["PropertyInsurance"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>ऋण योजना: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["LoanTypeName"]));
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>बीमाकर्ता का नाम(मेडिक्लेम): {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["HospiName"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>ब्याज दर (%): {0}</td>", Convert.ToString(Math.Round(Convert.ToDouble(dtScheduleOwn.Rows[0]["IntRate"]), 2)));
                        sb.Append("</tr>");

                        sb.Append("<tr style='border:none;'>");
                        sb.AppendFormat("<td style='padding:3px;border:none;'>मध्यस्थता शुल्क (रू.): {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["MedAmt"]));
                        sb.AppendFormat("<td style='padding:3px;border:none;'>भुगतान आईडी: {0}</td>", Convert.ToString(dtScheduleOwn.Rows[0]["PID"]));
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        sb.Append("<table class='small_font' style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr><td colspan='2' style='padding-top:20px;text-decoration: underline;'><strong>ईएमआई अनुसूची</strong></td></tr>");
                        sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table  class='xsmall_font' style='border-collapse: collapse; width: 100%;  text-align: left;' border='1' >");
                        sb.Append("<tr>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:6%;'>क्रमांक<br/>Sr.No.</th>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:16%;'>नियत तिथि<br/>Due Date</th>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:11%;'>मुख्य अवधि<br/>Principle Due</th>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:14%;'>ब्याज की अवधि<br/>Interest Due</th>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:14%;'>मासिक किस्त राशि<br/>EMI Amount</th>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:12%;'>मुख्य बकाया<br/>Principle Outstanding</th>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:6%;'>ब्याज बकाया<br/>Interest Outstanding</th>");
                        sb.Append("<th rowspan='2' style='padding: 2px;width:7%;'>कुल बकाया<br/>Total Outstanding</th>");
                        sb.Append("<th colspan='2' style='padding: 2px;width:14%;'>प्रारम्भिक</th>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th style='padding: 2px;'>ऋण अधिकारी</th>");
                        sb.Append("<th style='padding: 2px;'>शाखा प्रबंधक</th>");
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

                        sb.Append("<table class='xsmall_font'  cellspacing='0' cellpadding='2' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td>**यदि वसूली की तारीख किसी अवकाश पर आती है, तो ईएमआई अगले कारोबारी दिन पर वसूली जाएगी। शिकायत निवारण अधिकारी का विवरण: नाम: श्री महेंद्र बिंद्रा ईमेल - id-care@unitybank.co.in टोल फ्री क्रमांक-18002091122 एमएफआईएन टोल फ्री क्रमांक :- 18001021080");
                        sb.Append("</td></tr>");
                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // Start Consumer Education
                        #region 31th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr><td  style='text-align:center;width:100%;font-weight:bold;'>ग्राहक शिक्षा साहित्य</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td  style='text-align:left;width:100%;'>हमारी क्रेडिट/ऋण सुविधा के संबंध में उपयोग हुए कुछ शब्दों पर उदाहरणों के लिए घोषणा</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>हम पुष्टि करते हैं कि आरबीआई द्वारा अनुबद्ध आईआरएसीपी मानदंडों के अनुसार बैंक द्वारा प्रदान किए गए नीचे दर्शाए उदाहरण हमने समझ लिए हैं</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>ग्राहक शिक्षा साहित्य</td></tr>");
                        sb.Append("<tr><td>हमारी क्रेडिट/ऋण सुविधा के संबंध में उपयोग हुए कुछ शब्दों पर उदाहरण</td></tr>");
                        sb.Append("<tr><td>संदर्भ:  आय पहचान, परिसंपत्ति वर्गीकरण पर विवेकपूर्ण मानदंड और अग्रिम से संबंधित प्रावधान (आरबीआई/2022-23/15 DOR.STR.REC.4/21.04.048/2022-23 दिनांक अप्रैल 1, 2022)</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>कर्जदारों के बीच जागरूकता बढ़ाने की दृष्टि से दिन के अंत की प्रक्रिया के विशिष्ट संदर्भ के साथ अतिदेय की तारीख, एसएमए, एनपीए वर्गीकरण और उन्नयन जैसे शब्दों की संकल्पना को उदाहरणों के साथ समझाते हुए ग्राहक शिक्षा साहित्य रखा गया है।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>इस दस्तावेज में उद्धृत किए गए उदाहरण निदर्शनात्मक हैं और इनका स्वरूप विस्तृत नहीं है और वह सामान्य परिदृश्यों से संबंधित हैं। कार्यान्वयन के लिए आरबीआई द्वारा जारी किए गए आईआरएसीपी मानदंड और वर्गीकरण अभिभावी होंगे और आरबीआई द्वारा समय समय पर संशोधित किए जा सकते हैं।</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td style='text-decoration: underline;'><b>संकल्पना:</b></td></tr>");

                        sb.Append("<tr><td><b>1. देय:</b><br>");
                        sb.Append("का मतलब मूलधन/ब्याज/ऋण खाते पर लगाया गया कोई भी प्रभार जो ऋण सुविधा की संस्वीकृति की शर्तों के अनुसार अनुबद्ध अवधि के भीतर भुगतान योग्य है");
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td><b>2. अतिदेय:</b><br>");
                        sb.Append("का मतलब मूलधन/ब्याज/ऋण खाते पर लगाया गया कोई भी प्रभार जो भुगतान योग्य है लेकिन ऋण सुविधा की संस्वीकृति की शर्तों के अनुसार अनुबद्ध अवधि के भीतर जिसका भुगतान नहीं किया गया है। दूसरे शब्दों में, किसी भी ऋण सुविधा के अंतर्गत कोई भी राशि जो बैंक को देय है और यदि बैंक और इसके ग्राहक के बीच अनुबंधित नियत तिथि पर या इससे पहले इसका भुगतान नहीं किया गया है तो यह ‘अतिदेय’ है। नियत तिथि के लिए दिन के अंत की प्रक्रिया के भाग के रूप में ऋणप्रदाता संस्था द्वारा कर्जदार खातों को अतिदेय के रूप में चिन्हित करना होता है, चाहे ऐसी प्रक्रिया चलाने का समय कोई भी हो।");
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td>3. एसएमए/एनपीए स्थिति निर्धारित करने के लिए अतिदेय के दिनों की संख्या का पता लगाने के लिए बैंकों द्वारा उपयोग में लाई जाने वाली लेखा पद्धति:<br>");
                        sb.Append("‘स्पेशल मेंशन अकाउंट/ विशेष उल्लेख खाता’ (एसएमए) एवं ‘नॉन परफॉर्मिंग अकाउंट/ अनर्जक खाता’ (एनपीए) स्थिति की उप श्रेणी निर्धारित करने के लिए अतिदेय के दिनों की संख्या का पता लगाने के लिए एफआईएफओ का सिद्धांत यानी, ‘फर्स्ट इन, फर्स्ट आउट/पहले आओ, पहले जाओ’ लेखा पद्धति सुसंगत है (एसएमए और एनपीए वर्गीकरण को नीचे समझाया गया है)। एफआईएफओ सिद्धांत में यह माना जाता है कि ऋण खाते में सबसे पुरानी बकाया देय राशि का भुगतान पहले करना आवश्यक है। इसलिए एफआईएफओ पद्धति के लिए यह आवश्यक है कि जो पहले देय है उसे कर्जदार द्वारा पहले भुगतान किया जाना चाहिए।");
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td>उदाहरण के लिए, 01.02.2022 तारीख तक ऋण खाते में कोई अतिदेय नहीं है और मूलधन किस्त / ब्याज / प्रभार के लिए रू. 100 की राशि का भुगतान देय है। इसलिए, 01.02.2022 को या इसके बाद ऋण खाते में जमा हो रहे भुगतान का उपयोग 01.02.2022 पर बकाया देय चुकता करने के लिए होगा।");
                        sb.Append("यह मानते हुए कि फरवरी के महीने के दौरान देय राशि के लिए कोई भी भुगतान नहीं किया है / या आंशिक भुगतान (रू. 80) किया है, तो 01.03.2022 तक अतिदेय रू. 20 यानी रू. 100 – रू. 80 होगा।");
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td>यह मानते हुए कि फरवरी के महीने के दौरान देय राशि के लिए कोई भी भुगतान नहीं किया है / या आंशिक भुगतान (रू. 80) किया है, तो 01.03.2022 तक अतिदेय रू. 20 यानी रू. 100 – रू. 80 होगा।");
                        sb.Append("</td></tr>");

                        sb.Append("</table>");
                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 32th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr><td><b>4. सबसे पुराने देय की आयु:</b><br>");
                        sb.Append("जिस तिथि से सबसे पुराना भुगतान देय है और लगातार जिसका भुगतान नहीं किया गया है, उस तिथि से सबसे पुराने देय की आयु दिनों में परिकलित की जाती है। उपरोक्त उदाहरण में, यदि 01.02.2022 तक के देय का भुगतान 01.03.2022 तक नहीं किया जाता है, तो सबसे पुराने देय की आयु का परिकलन 01.03.2022 को 29 दिनों के रूप में किया जाता है।");
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td style='height:5px;'></td></tr>");

                        sb.Append("<tr><td>5. स्पेशल मेंशन अकाउंट/ विशेष उल्लेख खाता (एसएमए) एवं ‘नॉन परफॉर्मिंग अकाउंट/ अनर्जक खाता (एनपीए) के तौर पर वर्गीकरण:<br>");
                        sb.Append("<b>a. एसएमए:</b><br>");
                        sb.Append("भुगतान में चूक होने पर बैंक द्वारा तुरंत ऋण खातों में प्रारंभिक दबाव हैं ऐसा माना जायेगा और उसे ‘स्पेशल मेंशन अकाउंट/ विशेष उल्लेख खाता’ (एसएमए) के रूप में वर्गीकृत किया जाएगा। एसएमए / एनपीए श्रेणी में वर्गीकरण किए जाने के लिए आधार निम्नलिखित है:");
                        sb.Append("</td></tr>");

                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='3' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='padding:3px;'>परिक्रामी सुविधाओं के अलावा अन्य ऋण</td>");
                        sb.Append("<td colspan='2' style='padding:3px;'>नकदी ऋण/ ओवरड्राफ्ट के स्वरूप में ऋण</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;width:15%;'>एसएमए उप-श्रेणियाँ</td>");
                        sb.Append("<td style='padding:3px;width:35%;'>वर्गीकरण का आधार मूलधन या ब्याज का भुगतान या कोई भी अन्य राशि जो संपूर्ण रूप से या आंशिक रूप से अतिदेय हो</td>");
                        sb.Append("<td style='padding:3px;width:15%;'>एसएमए उप-श्रेणियाँ</td>");
                        sb.Append("<td style='padding:3px;width:35%;'>वर्गीकरण का आधार - बकाया शेष राशि लगातार संस्वीकृत सीमा या आहरण शक्ति से अधिक बनी रहती है, जो भी कम हो, निम्नलिखित अवधि के लिए:</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>एसएमए 0</td>");
                        sb.Append("<td style='padding:3px;'>30 दिनों तक</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>एसएमए 1</td>");
                        sb.Append("<td style='padding:3px;'>30 दिनों से अधिक और 60 दिनों तक</td>");
                        sb.Append("<td style='padding:3px;'>एसएमए 1</td>");
                        sb.Append("<td style='padding:3px;'>30 दिनों से अधिक और 60 दिनों तक</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>एसएमए 2</td>");
                        sb.Append("<td style='padding:3px;'>60 दिनों से अधिक और 90 दिनों तक</td>");
                        sb.Append("<td style='padding:3px;'>एसएमए 2</td>");
                        sb.Append("<td style='padding:3px;'>60 दिनों से अधिक और 90 दिनों तक</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='2' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr><td><b>b. नॉन परफॉर्मिंग अकाउंट/ अनर्जक खाता (एनपीए):</b><br>");
                        sb.Append("आय पहचान, परिसंपत्ति वर्गीकरण पर विवेकपूर्ण मानदंड और अग्रिम से संबंधित प्रावधान दिनांक अप्रैल 1, 2022 पर आरबीआई के मास्टर सर्कुलर के प्रावधानों के अनुसार:<br>");
                        sb.Append("i. किसी मियादी ऋण के संबंध में ब्याज और/या मूलधन की किश्त 90 दिनों से अधिक की अवधि के लिए अतिदेय बनी रहती है<br>");
                        sb.Append("ii. किसी ओवरड्राफ्ट / नकदी ऋण के संबंध में नीचे दर्शाए अनुसार खाता ‘आउट ऑफ ऑर्डर’ बना रहता है<br>");
                        sb.Append("iii. क्रेडिट ऋण (ओडी/सीसी)<br>");
                        sb.Append("iv. खरीदे गए और छूट प्राप्त बिल के मामले में यदि 90 दिनों से अधिक अवधि के लिए बिल अतिदेय बना रहता है<br>");
                        sb.Append("v. छोटी अवधि की फ़सल के लिए दो फसलों के मौसम तक मूलधन या ब्याज की किश्त अतिदेय बनी रहती है<br>");
                        sb.Append("vi. लंबी अवधि की फ़सल के लिए एक फसल के मौसम तक मूलधन या ब्याज की किश्त अतिदेय बनी रहती है");
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td><b>6. ‘आउट ऑफ ऑर्डर’ स्थिति:</b><br>");
                        sb.Append("खाते को ‘आउट ऑफ ऑर्डर’ माना जाएगा यदि:<br>");
                        sb.Append("i. सीसी/ओडी खाते में बकाया शेष 90 दिनों तक लगातार संस्वीकृत सीमा / आहरण शक्ति से अधिक बनी रहती है, या<br>");
                        sb.Append("ii. सीसी/ओडी खाते में बकाया शेष संस्वीकृत सीमा / आहरण शक्ति से कम बनी रहती है लेकिन 90 दिनों तक लगातार कोई राशि जमा नहीं हुई हो, या<br>");
                        sb.Append("iii. सीसी/ओडी खाते में बकाया शेष संस्वीकृत सीमा / आहरण शक्ति से कम है लेकिन जमा राशि 90 दिनों की पिछली अवधि के दौरान नामे लिखे गए ब्याज को कवर करने के लिए अपर्याप्त हो।");
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td><b>7. देरी / देय के ग़ैर-भुगतान के आधार पर किसी खाते का एसएमए श्रेणी और एनपीए श्रेणी तक का व्याख्यात्मक परिवर्तन और इसके बाद दिन के अंत की प्रक्रिया में मानक श्रेणी में किया जाने वाला उन्नयन:</b></td></tr>");

                        sb.Append("</table>");

                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;width:12%;'></td>");
                        sb.Append("<td style='padding:3px;width:12%;'>भुगतान तिथि</td>");
                        sb.Append("<td style='padding:3px;width:22%;'>भुगतान में शामिल है</td>");
                        sb.Append("<td style='padding:3px;width:12%;'>सबसेपुरानीदेय राशि की आयु दिनों में</td>");
                        sb.Append("<td style='padding:3px;width:12%;'>एसएमए / एनपीए श्रेणी </td>");
                        sb.Append("<td style='padding:3px;width:12%;'>एसएमए तारीख से / एसएमए श्रेणी तारीख</td>");
                        sb.Append("<td style='padding:3px;width:9%;'>एनपीए वर्गीकरण</td>");
                        sb.Append("<td style='padding:3px;width:9%;'>एनपीए तारीख</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.01.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.01.2022</td>");
                        sb.Append("<td style='padding:3px;'>संपूर्ण देय राशि 01.01.2022 तक</td>");
                        sb.Append("<td style='padding:3px;'>0</td>");
                        sb.Append("<td style='padding:3px;'>शून्य</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.01.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.01.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 तक आंशिक रूप से भुगतान की गई देय राशि</td>");
                        sb.Append("<td style='padding:3px;'>1</td>");
                        sb.Append("<td style='padding:3px;'>एसएमए-0</td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 33th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");


                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;width:12%;'>01.01.2022</td>");
                        sb.Append("<td style='padding:3px;width:11%;'>01.01.2022</td>");
                        sb.Append("<td style='padding:3px;width:26%;'>01.02.2022 तक आंशिक रूप से भुगतान की गई देय राशि</td>");
                        sb.Append("<td style='padding:3px;width:12%;'>2</td>");
                        sb.Append("<td style='padding:3px;width:12%;'>एसएमए-0</td>");
                        sb.Append("<td style='padding:3px;width:9%;'>01.02.2022</td>");
                        sb.Append("<td style='padding:3px;width:9%;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;width:9%;'>लागू नहीं</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.03.2022</td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 की देय राशि का पूर्ण रूप से भुगतान नहीं हुआ, 01.03.2022 भी देय है 01.03.2022 के दिन के अंत तक</td>");
                        sb.Append("<td style='padding:3px;'>29</td>");
                        sb.Append("<td style='padding:3px;'>एसएमए-0</td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 की देय राशि का पूर्ण रूप से भुगतान किया गया, 01.03.2022 के लिए देय राशि का भुगतान 01.03.2022 के दिन के अंत तक नहीं किया गया</td>");
                        sb.Append("<td style='padding:3px;'>1</td>");
                        sb.Append("<td style='padding:3px;'>एसएमए-0</td>");
                        sb.Append("<td style='padding:3px;'>01.03.2022</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 और 01.03.2022 की संपूर्ण देय राशि का भुगतान 03.03.2022 के दिन के अंत तक नहीं किया गया</td>");
                        sb.Append("<td style='padding:3px;'>31</td>");
                        sb.Append("<td style='padding:3px;'>एसएमए-1</td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 / 03.03.2022</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 की देय राशि का पूर्ण रूप से भुगतान किया गया, 01.03.2022 के लिए देय राशि का संपूर्ण रूप से भुगतान01.03.2022 के दिन के अंत तक नहीं किया गया</td>");
                        sb.Append("<td style='padding:3px;'>1</td>");
                        sb.Append("<td style='padding:3px;'>एसएमए-0</td>");
                        sb.Append("<td style='padding:3px;'>01.03.2022</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.04.2022</td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022, 01.03.2022 को देय राशि और 01.04.2022 को देय राशि काभुगतान 01.04.2022 के दिन के अंत तक नहीं किया गया।</td>");
                        sb.Append("<td style='padding:3px;'>60</td>");
                        sb.Append("<td style='padding:3px;'>एसएमए-1</td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 / 03.03.2022</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 की देय राशि का भुगतान 01.04.2022 तक और 02.04.2022 के दिन के अंत तक नहीं किया गया।</td>");
                        sb.Append("<td style='padding:3px;'>61</td>");
                        sb.Append("<td style='padding:3px;'>एसएमए-2</td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 / 02.04.2022</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.05.2022</td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 की देय राशि का भुगतान 01.05.2022 तक और 01.05.2022 के दिन के अंत तक नहीं किया गया।</td>");
                        sb.Append("<td style='padding:3px;'>90</td>");
                        sb.Append("<td style='padding:3px;'>एसएमए-2</td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 / 02.04.2022</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'></td>");
                        sb.Append("<td style='padding:3px;'>01.02.2022 की देय राशि का भुगतान 01.05.2022 तक और 02.05.2022 के दिन के अंत तक नहीं किया</td>");
                        sb.Append("<td style='padding:3px;'>91</td>");
                        sb.Append("<td style='padding:3px;'>एनपीए</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>एनपीए</td>");
                        sb.Append("<td style='padding:3px;'>02.05.2022</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        #endregion

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        #region 34th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");


                        sb.Append("<table class='small_font' border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");


                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;width:12%;'>01.06.2022</td>");
                        sb.Append("<td style='padding:3px;width:12%;'>01.06.2022</td>");
                        sb.Append("<td style='padding:3px;width:22%;'>01.02.2022 की देय राशि का संपूर्ण भुगतान 01.06.2022 के दिन के अंत तक किया गया।</td>");
                        sb.Append("<td style='padding:3px;width:14%;'>93</td>");
                        sb.Append("<td style='padding:3px;width:12%;'>एनपीए</td>");
                        sb.Append("<td style='padding:3px;width:12%;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;width:9%;'>एनपीए</td>");
                        sb.Append("<td style='padding:3px;width:9%;'>02.05.2022</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.06.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.06.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.03.2022 और 01.04.2022 की संपूर्ण देय राशि का भुगतान 01.07.2022 के दिन के अंत तक किया गया।</td>");
                        sb.Append("<td style='padding:3px;'>62</td>");
                        sb.Append("<td style='padding:3px;'>एनपीए</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>एनपीए</td>");
                        sb.Append("<td style='padding:3px;'>02.05.2022</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.08.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.08.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.05.2022 और 01.06.2022 की संपूर्ण देय राशि का भुगतान 01.08.2022 के दिन के अंत तक किया गया।</td>");
                        sb.Append("<td style='padding:3px;'>32</td>");
                        sb.Append("<td style='padding:3px;'>एनपीए</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>एनपीए</td>");
                        sb.Append("<td style='padding:3px;'>02.05.2022</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.09.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.09.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.07.2022 और 01.08.2022 की संपूर्ण देय राशि का भुगतान 01.09.2022 के दिन के अंत तक किया गया।</td>");
                        sb.Append("<td style='padding:3px;'>1</td>");
                        sb.Append("<td style='padding:3px;'>एनपीए</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>एनपीए</td>");
                        sb.Append("<td style='padding:3px;'>02.05.2022</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:3px;'>01.10.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.10.2022</td>");
                        sb.Append("<td style='padding:3px;'>01.09.2022 और 01.10.2022 की संपूर्ण देय राशि का भुगतान किया गया।</td>");
                        sb.Append("<td style='padding:3px;'>0</td>");
                        sb.Append("<td style='padding:3px;'>खाताक्र. के साथ मानक अतिदेय</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>लागू नहीं</td>");
                        sb.Append("<td style='padding:3px;'>मानक 01.10.2022 से</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

                        sb.Append("<tr><td><b>8. एसएमए/एनपीए वर्गीकरण</b><br>");
                        sb.Append("एसएमए/एनपीए रिपोर्टिंग कर्जदार के स्तर पर विनियामक दिशानिर्देशों के अनुसार की जाती है और इस प्रकार, कर्जदार के किसी एक खाते में अतिदेय होने पर, इसका परिणाम कर्जदार की रिपोर्टिंग एसएमए या एनपीए के तौर पर, जैसा भी मामला हो, किए जाने के रूप में होता है।");
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td style='height:5px;'></td></tr>");

                        sb.Append("<tr><td>9. खातों का उन्नयन:<br>");
                        sb.Append("एनपीए के रूप में वर्गीकृत ऋण खातों को ‘मानक’ परिसंपत्ति में अपग्रेड/उन्नयन किया जाता है, केवल तब, जब कर्जदार द्वारा सभी ऋण सुविधाओं से संबंधित ब्याज और मूलधन की संपूर्ण बकाया राशि का भुगतान किया गया हो।  उन खातों के उन्नयन के संबंध में जिन्हें एनपीए के तौर पर वर्गीकृत किया गया है पुनर्रचना, वाणिज्यिक परिचालन शुरू करने की तिथि (डीसीसीओ) की कोई उपलब्धि नहीं, इत्यादि कारणों के लिए, विशिष्ट विनियामक परिपत्र के अनुसार निर्देश लागू होना जारी रहेगा।  मैं/हम एतदद्वारा आगे पुष्टि करता(ती) हूँ/करते हैं कि पूर्वोक्त उदाहरणों को विस्तृत नहीं माना जा सकता और यह कि उनका स्वरूप सामान्य परिदृश्यों को कवर करता है, और यह कि ऊपर संदर्भित विषय में आरबीआई द्वारा दिए गए आईआरएसीपी मानक और स्पष्टीकरण अभिभावी होंगे।");
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>भवदीय,</td></tr>");
                        sb.Append("<tr><td style='height:5px;'></td></tr>");
                        sb.Append("<tr><td>ग्राहक का नाम लिखें</td></tr>");
                        sb.Append("</table>");

                        #endregion
                        // End Consumer Education

                        sb.Append("<div class='page-break'></div>"); // Page Break

                        // DigitalDocSign
                        #region 35th_Page
                        sb.Append("<table style='width:100%; border-collapse:collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>");
                        sb.Append("<img src='" + HindiDocHeader + "' alt='Company Logo' height='100' width='95%' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td>&nbsp;</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='3' style='border-collapse:collapse; width:100%;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;width:80%;'>");
                        sb.Append("<table class='small_font'  cellspacing='0' cellpadding='3' style='border-collapse:collapse; width:100%;'>");

                        sb.AppendFormat("<tr><td style='padding:2px;'>नाम: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["ApplicantName"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>डिवाइस प्लैटफॉर्म: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["DevicePlatform"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>ब्राउज़र: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["Browser"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>भौगोलिक स्थान: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["GeoLocation"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>आईपी पता: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["IpAddress"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>मोबाइल नं: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["MobileNo"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>ओटीपी भेजने की अवधि: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["SMSSendTimeStamp"]));
                        sb.AppendFormat("<tr><td style='padding:2px;'>ओटीपी सत्यापन अवधि: {0}</td></tr>", Convert.ToString(dtDigiDocDtls.Rows[0]["SMSVerifyTimeStamp"]));
                        sb.Append("</table>");

                        sb.Append("</td>");
                        sb.Append("<td style='padding:5px;width:20%;vertical-align:top;'>");
                        sb.Append("<img src='https://dchot1.bijliftt.com:9000/jlginitialapproach/10213710789_AddressProofImage.png' alt='KYC-2 Applicant Front' height='210px !important' width='158px !important' />");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        #endregion

                        sb.Append("</body>");
                        sb.Append("</html>");
                        finalHtml = sb.ToString();
                       
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

    }
}
