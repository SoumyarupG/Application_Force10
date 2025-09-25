using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.IO;
using FORCECA;
using FORCEBA;

namespace CENTRUM
{
    public static class gblFuction
    {
        public static string getFinYrNo(string pYear)
        {
            string vYrNo = pYear;
            if (pYear.Length == 1)
                vYrNo = pYear.Insert(0, "0");           
            return (vYrNo);
        }

        public static string getStrDate(string pDate)
        {
            string StrDD, StrMM, StrYYYY, strDate;
            if (pDate == "")
                strDate = null;
            else
            {
                StrDD = string.Format(Convert.ToString(Convert.ToDateTime(pDate).Day), "00");
                if (StrDD.Length < 2)
                    StrDD = StrDD.Insert(0, "0");

                StrMM = string.Format(Convert.ToString(Convert.ToDateTime(pDate).Month), "00");
                if (StrMM.Length < 2)
                    StrMM = StrMM.Insert(0, "0");

                StrYYYY = Convert.ToString(Convert.ToDateTime(pDate).Year);
                strDate = (StrDD + "/" + StrMM + "/" + StrYYYY);
            }
            return strDate;
        }

        public static string setJpnFrmt(string pDate)
        {
            string StrDD, StrMM, StrYYYY, strDate;
            if (pDate == "" || pDate == null) return "";
            StrDD = pDate.Substring(0, 2);
            StrMM = pDate.Substring(3, 2);
            StrYYYY = pDate.Substring(6, 4);
            strDate = (StrDD + "-" + StrMM + "-" + StrYYYY);
            return strDate;
        }

        public static string putStrDate(DateTime pDate)
        {
            string StrDD, StrMM, StrYYYY, strDate;
            StrDD = pDate.Day.ToString();
            if (StrDD.Length == 1)
                StrDD = StrDD.Insert(0, "0");

            StrMM = pDate.Month.ToString();
            if (StrMM.Length == 1)
                StrMM = StrMM.Insert(0, "0");
            StrYYYY = pDate.Year.ToString();
            strDate = (StrDD + "/" + StrMM + "/" + StrYYYY);
            return strDate;
        }

        public static string setStrDate(string pDate)
        {
            string StrDD, StrMM, StrYYYY, strDate;
            if (pDate == "")
                strDate = null;
            else
            {
                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
                strDate = (StrMM + "/" + StrDD + "/" + StrYYYY);
            }
            return strDate;
        }

        public static string GetStartDate(DateTime pLoanDt, Int32 pCollDay, Int32 pCollDayNo, string pPaySch, Int32 pGroupID)
        {
            //Int32 vWkNo = GetWeekNumber(pLoanDt);
            //Int32 vWKDay=Convert.ToInt32(pLoanDt.DayOfWeek);
            DataTable dt = null;
            CCollectionRoutine oCR = null;
            DateTime vNextDate = pLoanDt.AddMonths(1);
            string ReternStartDate = string.Empty;
            if (pPaySch == "M")
            {
                ReternStartDate = ReturenDate(vNextDate, pCollDayNo, pCollDay);
            }
            if (pPaySch == "W")
            {
                if ((Int32)pLoanDt.DayOfWeek >= pCollDay)
                    pLoanDt = pLoanDt.AddDays(7);

                DayOfWeek day = pLoanDt.DayOfWeek;
                int days = day - DayOfWeek.Monday;
                DateTime start = pLoanDt.AddDays(-days);
                ReternStartDate = putStrDate(start.AddDays(pCollDay - 1));
            }
            if (pPaySch == "F")
            {
                oCR = new CCollectionRoutine();
                dt = oCR.GetNextCollDate(pGroupID, pLoanDt);
                if (setDate(Convert.ToString(dt.Rows[0]["DueDt"])) > pLoanDt)
                {
                    if ((setDate(Convert.ToString(dt.Rows[0]["DueDt"])) - pLoanDt).TotalDays < 7)
                        ReternStartDate = putStrDate(setDate(Convert.ToString(dt.Rows[0]["DueDt"])).AddDays(14));
                    else
                        ReternStartDate = putStrDate(setDate(Convert.ToString(dt.Rows[0]["DueDt"])));
                }
                else
                {
                    pLoanDt = pLoanDt.AddDays(14);
                    DayOfWeek day = pLoanDt.DayOfWeek;
                    int days = day - DayOfWeek.Monday;
                    DateTime start = pLoanDt.AddDays(-days);
                    ReternStartDate = putStrDate(start.AddDays(pCollDay - 1));
                }
                //ReternStartDate = ReturenDate(vNextDate, pCollDayNo, pCollDay);
            }

            return ReternStartDate;
        }

        public static string ReturenDate(DateTime pNextDate, Int32 pCollDayNo, Int32 pCollDay)
        {
            Int32 vYear = pNextDate.Year;
            Int32 vMonth = pNextDate.Month; //Jan=1, Feb=2, ...
            string vReturenDate = string.Empty;
            DateTime dateValue = new DateTime(vYear, vMonth, 1);
            Int32 firstDay = (Int32)dateValue.DayOfWeek;
            dateValue = dateValue.AddDays(pCollDay - firstDay + (pCollDayNo - 1) * 7);
            vReturenDate = putStrDate(dateValue);
            return vReturenDate;
        }

        //public static Int32 GetWeekNumber(DateTime dtPassed)
        //{
        //    CultureInfo ciCurr = CultureInfo.CurrentCulture;
        //    int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        //    return weekNum;
        //}

        public static DateTime getDate(string pDate)
        {
            string StrDD, StrMM, StrYYYY, strDate;
            DateTime dDate = System.DateTime.Now;
            if (pDate == "")
                dDate = Convert.ToDateTime("01/01/1900");
            else
            {
                if (pDate.Length == 9)
                    pDate = pDate.Insert(0, "0");

                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
                strDate = (StrDD + "/" + StrMM + "/" + StrYYYY);
                dDate = Convert.ToDateTime(strDate);
            }
            return dDate;
        }

        public static DateTime setDate(string pDate)
        {
            string StrDD, StrMM, StrYYYY, strDate;

            string pattern = "MM/dd/yyyy"; //date pattern

            var dDate = DateTime.Now;

            if (pDate == "")
            {
                dDate = Convert.ToDateTime("01/01/1900");
            }
            else
            {
                if (pDate.Length == 9)
                {
                    pDate = pDate.Insert(0, "0");
                }
                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
                strDate = (StrMM + "/" + StrDD + "/" + StrYYYY);

                dDate = DateTime.ParseExact(strDate, pattern, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            return dDate;
        }

        public static Boolean CheckDtRange(string pFromDt, string pToDate)
        {
            bool vRest = true;
            try
            {
                if (IsDate(pFromDt) == true && IsDate(pToDate) == true)
                {
                    DateTime vFrDt = setDate(pFromDt);
                    DateTime vToDt = setDate(pToDate);
                    if (vToDt < vFrDt)
                        vRest = false;
                }
                else
                {
                    vRest = false;
                }
            }
            catch
            {
                vRest = false;
            }
            return vRest;
        }

        public static Boolean IsDate(string pDate)
        {
            bool bRest = true;
            string pattern = "MM/dd/yyyy"; //date pattern
            try
            {
                //DateTime dt = System.DateTime.Parse(gblFuction.setStrDate(pDate));
                DateTime dt = DateTime.ParseExact(gblFuction.setStrDate(pDate), pattern, CultureInfo.InvariantCulture, DateTimeStyles.None);
                bRest = true;
            }
            catch (Exception)
            {
                bRest = false;
            }
            return bRest;
        }

        public static void MsgPopup(string pMsg)
        {
            Page page = System.Web.HttpContext.Current.Handler as Page;
            string vMsg = "<Script Language='JavaScript' type='text/javascript'>";
            vMsg = vMsg + "alert('" + pMsg + "');";
            vMsg = vMsg + "</Script>";
            if (!page.ClientScript.IsStartupScriptRegistered("alert"))
                page.ClientScript.RegisterStartupScript(page.GetType(), "alert", vMsg);
        }

        public static void AjxMsgPopup(string pMsg)
        {
            Page page = HttpContext.Current.Handler as Page;
            string vMsg = "alert('" + pMsg + "');";
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", vMsg, true);
        }

        public static void AjxFocus(string pMsg)
        {
            Page page = HttpContext.Current.Handler as Page;
            string vMsg = "document.getElementById('" + pMsg + "').focus();";
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", vMsg, true);
        }

        public static void focus(string id)
        {
            Page page = HttpContext.Current.Handler as Page;
            string strScript = "<Script Language=JavaScript>";
            strScript += "Sys.Application.add_load(function(){window.setTimeout(focus, 1);});";
            strScript += "function focus(){document.getElementById('" + id + "').focus();}";
            strScript += "</Script>";
            if ((!page.ClientScript.IsStartupScriptRegistered("key")))
                page.ClientScript.RegisterStartupScript(page.GetType(), "key", strScript);
        }

        public static Boolean IsInteger(string text)
        {
            int b;
            bool bRest = true;
            try
            {
                b = Int32.Parse(text);
                bRest = true;
            }
            catch (Exception)
            {
                bRest = false;
            }
            return bRest;
        }

        public static Boolean IsDouble(string text)
        {
            double Num;
            bool bRest = true;
            try
            {
                bRest = double.TryParse(text, out Num);
            }
            catch (Exception)
            {
                bRest = false;
            }
            return bRest;
        }

        public static string Left(string sparam, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable
            string result = "";
            if (sparam.Length > length)
                result = sparam.Substring(0, length);
            //return the result of the operation
            return result;
        }

        public static string Right(string sparam, int length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable
            string result = "";
            if (sparam.Length > length)
                result = sparam.Substring(sparam.Length - length, length);
            //return the result of the operation
            return result;
        }

        public static string Mid(string sparam, int startIndex, int length)
        {
            //start at the specified index in the string ang get N number of
            //characters depending on the lenght and assign it to a variable
            string result = "";
            if (sparam.Length > length)
                result = sparam.Substring(startIndex, length);
            //return the result of the operation
            return result;
        }

        public static string Mid(string sparam, int startIndex)
        {
            //start at the specified index and return all characters after it
            //and assign it to a variable
            string result = "";
            if (sparam.Length > startIndex)
                result = sparam.Substring(startIndex);
            //return the result of the operation
            return result;
        }

        public static Boolean IsEmail(string pEmail)
        {
            bool bRest = true;
            if ((pEmail.IndexOf("@") == -1) || (pEmail.IndexOf(".") == -1))
            {
                bRest = false;
            }
            return bRest;
        }
    }
}