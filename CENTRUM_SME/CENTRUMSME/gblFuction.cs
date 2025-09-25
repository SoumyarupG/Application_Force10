using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME
{
    /// <summary>
    /// 
    /// </summary>
    public static class gblFuction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pYear"></param>
        /// <returns></returns>
        public static string getFinYrNo(string pYear)
        {
            string vYrNo = "";
            if (pYear.Length == 1)
                vYrNo = pYear.Insert(0, "0");
            return (vYrNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDate"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDate"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDate"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pLoanDt"></param>
        /// <param name="pGroupId"></param>
        /// <param name="pPaySch"></param>
        /// <returns></returns>
        public static string GetStartDate(DateTime pLoanDt, Int32 pCollDay, Int32 pCollDayNo, string pPaySch, string pGroupID, int pCollType)
        {
            //Int32 vWkNo = GetWeekNumber(pLoanDt);
            //Int32 vWKDay=Convert.ToInt32(pLoanDt.DayOfWeek);
            DataTable dt = null;
            CCollectionRoutine oCR = null;
            DateTime vNextDate = pLoanDt.AddMonths(1);
            string ReternStartDate = string.Empty;
            if (pPaySch == "M")
            {
                ReternStartDate = ReturenDate(vNextDate, pCollDayNo, pCollDay, pCollType);
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
                        ReternStartDate = putStrDate(setDate(Convert.ToString(dt.Rows[0]["DueDt"])).AddDays(7));
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNextDate"></param>
        /// <param name="pCollDayNo"></param>
        /// <param name="pCollDay"></param>
        /// <returns></returns>
        public static string ReturenDate(DateTime pNextDate, Int32 pCollDayNo, Int32 pCollDay, Int32 pCollType)
        {
            
            string vReturenDate = string.Empty;
            DateTime dateValue = pNextDate;
            if (pCollType == 2)
            {
                string month, day;      
                month = pNextDate.Month.ToString();
                if (pCollDay.ToString().Length == 1) { day = "0" + pCollDay.ToString(); } else { day = pCollDay.ToString(); }
                if (month.ToString().Length == 1) { month = "0" + month.ToString(); } else { month = month.ToString(); }
                string startDt = day + "/" + month + "/" + pNextDate.Year;
                return startDt;
            }
            else
            {
                if (pCollDayNo == 0)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (Convert.ToInt32(dateValue.DayOfWeek) == pCollDay)
                        {
                            i = 7;
                        }
                        else
                        {
                            dateValue = dateValue.AddDays(1);
                        }
                    }
                }
                else
                {
                    dateValue = Convert.ToDateTime(dateValue.Year.ToString() + "/" + dateValue.Month.ToString() + "/01");

                    int i = 0;
                    i = 7 * (pCollDayNo - 1);
                    dateValue = dateValue.AddDays(i);
                    for (int j = 0; j < 7; j++)
                    {
                        if (Convert.ToInt32(dateValue.DayOfWeek) == pCollDay)
                        {
                            j = 7;
                        }
                        else
                        {
                            dateValue = dateValue.AddDays(1);
                        }
                    }
                    int LimitDay = 0;
                    if (pNextDate.AddMonths(-1).Month == 2) LimitDay = 28; else LimitDay = 30;
                    if ((dateValue - pNextDate.AddMonths(-1)).TotalDays < LimitDay)
                    {
                        dateValue = dateValue.AddMonths(1);
                        dateValue = Convert.ToDateTime(dateValue.Year.ToString() + "/" + dateValue.Month.ToString() + "/01");

                        i = 0;
                        i = 7 * (pCollDayNo - 1);
                        dateValue = dateValue.AddDays(i);
                        for (int j = 0; j < 7; j++)
                        {
                            if (Convert.ToInt32(dateValue.DayOfWeek) == pCollDay)
                            {
                                j = 7;
                            }
                            else
                            {
                                dateValue = dateValue.AddDays(1);
                            }
                        }
                    }

                }
                vReturenDate = putStrDate(dateValue);
                return vReturenDate;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNextDate"></param>
        /// <param name="pCollDayNo"></param>
        /// <param name="pCollDay"></param>
        /// <returns></returns>
        public static DataTable GetInstallSize(Int32 pLoanTypeID, decimal pLoanAmt, decimal pInstRate, Int32 pInstPeriod,
            Int32 pInstallNo, string pPaySchedule)
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            oGbl = new CGblIdGenerator();
            dt = oGbl.GetInstallSize(pLoanTypeID, pLoanAmt, pInstRate, pInstPeriod, pInstallNo, pPaySchedule);
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pLoanDt"></param>
        /// <returns></returns>
        //public static Int32 GetWeekNumber(DateTime dtPassed)
        //{
        //    CultureInfo ciCurr = CultureInfo.CurrentCulture;
        //    int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        //    return weekNum;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDate"></param>
        /// <returns></returns>
        public static DateTime setDate(string pDate)
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
                strDate = (StrMM + "/" + StrDD + "/" + StrYYYY);
                dDate = Convert.ToDateTime(strDate);
            }
            return dDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFromDt"></param>
        /// <param name="pToDate"></param>
        /// <returns></returns>
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

        public static Boolean CheckDtRangeOnlyGreaterThan(string pFromDt, string pToDate)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDate"></param>
        /// <returns></returns>
        public static Boolean IsDate(string pDate)
        {
            bool bRest = true;
            try
            {
                DateTime dt = System.DateTime.Parse(gblFuction.setStrDate(pDate));
                bRest = true;
            }
            catch (Exception)
            {
                bRest = false;
            }
            return bRest;
        }

        public static void ShowMsg(string pMsg)
        {
            Page page = System.Web.HttpContext.Current.Handler as Page;
            string vMsg = "<script language='javaScript' type='text/javascript'>";
            vMsg += "var el = document.createElement('div')";
            vMsg += "el.setAttribute('style', 'position:absolute;top:40%;left:20%;background-color:white;')";
            vMsg += "el.innerHTML = '" + pMsg + "'";
            vMsg += " setTimeout(function () {";
            vMsg += "el.parentNode.removeChild(el)";
            vMsg += "}, 3000)";
            vMsg += "document.body.appendChild(el)";
            vMsg = vMsg + "</script>";
            if (!page.ClientScript.IsStartupScriptRegistered("alert"))
                page.ClientScript.RegisterStartupScript(page.GetType(), "alert", vMsg);
        }

        public static void CheckMsg(string pMsg)
        {
            Page page = System.Web.HttpContext.Current.Handler as Page;
            string vMsg = "<script language='javaScript' type='text/javascript'>";
            vMsg += "var el = document.createElement('div')";
            vMsg += "el.setAttribute('style', 'position:absolute;top:40%;left:20%;background-color:white;')";
            vMsg += "el.innerHTML = '" + pMsg + "'";
            vMsg += " setTimeout(function () {";
            vMsg += "el.parentNode.removeChild(el)";
            vMsg += "}, 3000)";
            vMsg += "document.body.appendChild(el)";
            vMsg = vMsg + "</script>";
            if (!page.ClientScript.IsStartupScriptRegistered("alert"))
                page.ClientScript.RegisterStartupScript(page.GetType(), "alert", vMsg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMsg"></param>
        public static void MsgPopup(string pMsg)
        {
            Page page = System.Web.HttpContext.Current.Handler as Page;
            string vMsg = "<Script Language=JavaScript>";
            vMsg = vMsg + "alert('" + pMsg + "');";
            vMsg = vMsg + "</Script>";
            if (!page.ClientScript.IsStartupScriptRegistered("alert"))
                page.ClientScript.RegisterStartupScript(page.GetType(), "alert", vMsg);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pMsg"></param>
        public static void AjxMsgPopup(string pMsg)
        {
            Page page = HttpContext.Current.Handler as Page;
            string vMsg = "alert('" + pMsg + "');";
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", vMsg, true);
        }

        public static void MsgConfirmationPopup()
        {
            Page page = System.Web.HttpContext.Current.Handler as Page;
            string message = "Do you want to continue! Click 'YES'";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("return confirm('");
            sb.Append(message);
            sb.Append("');");
            page.ClientScript.RegisterOnSubmitStatement(page.GetType(), "alert", sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMsg"></param>
        public static void AjxFocus(string pMsg)
        {
            Page page = HttpContext.Current.Handler as Page;
            string vMsg = "document.getElementById('" + pMsg + "').focus();";
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", vMsg, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDate"></param>
        /// <returns></returns>
        public static string GetStrDatewithDay(DateTime pDate)  //For ddd dd/MMM/yyyy Format
        {
            string Strddd, StrDD, StrMMM, StrYYYY, strDate;
            StrDD = pDate.Day.ToString();
            if (StrDD.Length == 1)
                StrDD = StrDD.Insert(0, "0");

            StrMMM = pDate.Month.ToString();
            if (StrMMM == "1")
                StrMMM = "Jan";
            else if (StrMMM == "2")
                StrMMM = "Feb";
            else if (StrMMM == "3")
                StrMMM = "Mar";
            else if (StrMMM == "4")
                StrMMM = "Apr";
            else if (StrMMM == "5")
                StrMMM = "May";
            else if (StrMMM == "6")
                StrMMM = "Jun";
            else if (StrMMM == "7")
                StrMMM = "Jul";
            else if (StrMMM == "8")
                StrMMM = "Aug";
            else if (StrMMM == "9")
                StrMMM = "Sep";
            else if (StrMMM == "10")
                StrMMM = "Oct";
            else if (StrMMM == "11")
                StrMMM = "Nov";
            else if (StrMMM == "12")
                StrMMM = "Dec";

            Strddd = pDate.DayOfWeek.ToString();
            StrYYYY = pDate.Year.ToString();
            strDate = (Strddd + StrDD + "/" + StrMMM + "/" + StrYYYY);
            return strDate;
        }

    }


    public class SimpleHash
    {
        /// <summary>
        /// Generates a hash for the given plain text value and returns a
        /// base64-encoded result. Before the hash is computed, a random salt
        /// is generated and appended to the plain text. This salt is stored at
        /// the end of the hash value, so it can be used later for hash
        /// verification.
        /// </summary>
        /// <param name="plainText">
        /// Plaintext value to be hashed. The function does not check whether
        /// this parameter is null.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1",
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="saltBytes">
        /// Salt bytes. This parameter can be null, in which case a random salt
        /// value will be generated.
        /// </param>
        /// <returns>
        /// Hash value formatted as a base64-encoded string.
        /// </returns>
        public static string ComputeHash(string plainText,
                                         string hashAlgorithm,
                                         byte[] saltBytes)
        {
            // If salt is not specified, generate it on the fly.
            if (saltBytes == null)
            {
                // Define min and max salt sizes.
                int minSaltSize = 4;
                int maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash;

            // Make sure hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Initialize appropriate hashing algorithm class.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hash = new SHA1Managed();
                    break;

                case "SHA256":
                    hash = new SHA256Managed();
                    break;

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        /// <summary>
        /// Compares a hash of the specified plain text value to a given hash
        /// value. Plain text is hashed with the same salt value as the original
        /// hash.
        /// </summary>
        /// <param name="plainText">
        /// Plain text to be verified against the specified hash. The function
        /// does not check whether this parameter is null.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", 
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified,
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="hashValue">
        /// Base64-encoded hash value produced by ComputeHash function. This value
        /// includes the original salt appended to it.
        /// </param>
        /// <returns>
        /// If computed hash matches the specified hash the function the return
        /// value is true; otherwise, the function returns false.
        /// </returns>
        public static bool VerifyHash(string plainText,
                                      string hashAlgorithm,
                                      string hashValue)
        {
            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            // We must know size of hash (without salt).
            int hashSizeInBits, hashSizeInBytes;

            // Make sure that hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Size of hash is based on the specified algorithm.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hashSizeInBits = 160;
                    break;

                case "SHA256":
                    hashSizeInBits = 256;
                    break;

                case "SHA384":
                    hashSizeInBits = 384;
                    break;

                case "SHA512":
                    hashSizeInBits = 512;
                    break;

                default: // Must be MD5
                    hashSizeInBits = 128;
                    break;
            }

            // Convert size of hash from bits to bytes.
            hashSizeInBytes = hashSizeInBits / 8;

            // Make sure that the specified hash value is long enough.
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            // Allocate array to hold original salt bytes retrieved from hash.
            byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                        hashSizeInBytes];

            // Copy salt from the end of the hash to the new array.
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            // Compute a new hash string.
            string expectedHashString =
                        ComputeHash(plainText, hashAlgorithm, saltBytes);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            return (hashValue == expectedHashString);
        }

    }

    public class SimpleHashTest
    {
        static string password = "myP@5sw0rd";  // original password
        static string wrongPassword = "password";    // wrong password

        string passwordHashMD5 = SimpleHash.ComputeHash(password, "MD5", null);
        string passwordHashSha1 = SimpleHash.ComputeHash(password, "SHA1", null);
        string passwordHashSha256 = SimpleHash.ComputeHash(password, "SHA256", null);
        string passwordHashSha384 = SimpleHash.ComputeHash(password, "SHA384", null);
        string passwordHashSha512 = SimpleHash.ComputeHash(password, "SHA512", null);

        //Console.WriteLine("COMPUTING HASH VALUES\r\n");
        //Console.WriteLine("MD5   : {0}", passwordHashMD5);
        //Console.WriteLine("SHA1  : {0}", passwordHashSha1);
        //Console.WriteLine("SHA256: {0}", passwordHashSha256);
        //Console.WriteLine("SHA384: {0}", passwordHashSha384);
        //Console.WriteLine("SHA512: {0}", passwordHashSha512);
        //Console.WriteLine("");


        //Console.WriteLine("COMPARING PASSWORD HASHES\r\n");
        //Console.WriteLine("MD5    (good): {0}", SimpleHash.VerifyHash(password, "MD5", passwordHashMD5).ToString());
        //Console.WriteLine("MD5    (bad) : {0}", SimpleHash.VerifyHash(wrongPassword, "MD5", passwordHashMD5).ToString());
        //Console.WriteLine("SHA1   (good): {0}", SimpleHash.VerifyHash(password, "SHA1",  passwordHashSha1).ToString());
        //Console.WriteLine("SHA1   (bad) : {0}", SimpleHash.VerifyHash(wrongPassword, "SHA1", passwordHashSha1).ToString());
        //Console.WriteLine("SHA256 (good): {0}", SimpleHash.VerifyHash(password, "SHA256", passwordHashSha256).ToString());
        //Console.WriteLine("SHA256 (bad) : {0}", SimpleHash.VerifyHash(wrongPassword, "SHA256", passwordHashSha256).ToString());
        //Console.WriteLine("SHA384 (good): {0}", SimpleHash.VerifyHash(password, "SHA384",  passwordHashSha384).ToString());
        //Console.WriteLine("SHA384 (bad) : {0}", SimpleHash.VerifyHash(wrongPassword, "SHA384", passwordHashSha384).ToString());
        //Console.WriteLine("SHA512 (good): {0}", SimpleHash.VerifyHash(password, "SHA512",  passwordHashSha512).ToString());
        //Console.WriteLine("SHA512 (bad) : {0}", SimpleHash.VerifyHash( wrongPassword, "SHA512",  passwordHashSha512).ToString());

    }
}