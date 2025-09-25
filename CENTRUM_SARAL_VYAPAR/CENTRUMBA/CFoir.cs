using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;
using System.Net;
using System.IO;

namespace CENTRUMBA
{
    public class CFoir
    {
        public DataTable GetFOIRPendingMember(DateTime pFromDt, DateTime pToDt, string pBranch, string pSearch, Int32 pBCProductId, Int32 pCreatedBy)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetFOIRPendingMember";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDtFrom", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDtTo", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBCProductID", pBCProductId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.ExecuteForSelect(oCmd, dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public DataTable GetMarginByPDId(int pPDId, string pWithShopYN, string pType)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMarginByPDId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPDId", pPDId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pWithShopYN", pWithShopYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pType", pType);
                DBUtility.ExecuteForSelect(oCmd, dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public Int32 SaveFOIRMst(int pPDId, double pApplGrsMonthTurnover, string pApplMarginWithShopYN, double pApplNetMrgPrctg, double pApplNetMrgAmt,
        string pApplOthMonthInc, double pApplBankSalAmt, double pApplCashSalAmt, double pApplRentAmt, double pApplGrsTurnoverOthBus,
        double pApplNetMrgOthBusIncPrctg, double pApplNetMrgOthBusInc, double pApplTotOthInc, double pCoAppGrsMonthTurnover,
        string pCoApplMarginWithShopYN, double pCoAppNetMrgPrctg, double pCoAppNetMrgAmt, string pCoApplOthMonthInc,
        double pCoApplBankSalAmt, double pCoApplCashSalAmt, double pCoApplRentAmt, double pCoApplGrsTurnoverOthBus, double pCoApplNetMrgOthBusIncPrctg,
        double pCoApplNetMrgOthBusInc, double pCoApplTotOthInc, double pTypeOfOwnership, double pNoOfDependent, double pHouseholdExp,
        string pResidenceCategory, double pSurplus, double pLoanAmt, double pLoanTenure, double pROI, double pOthEMI, double pUSFBEMI,
        double pTotEMIObl, double pDBRPrctg, double pFOIRPrctg, string pBusDeviation, string pCreditDeviation,
        int pCreatedBy, string pEntType, string pMode, string pBranchCode, int pYrNo, string pMemberId, int pLoanTypeId, double pLoanAppAmt,
        string pCreditDeviationReason)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveFOIRMst";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPDId", pPDId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pApplGrsMonthTurnover", pApplGrsMonthTurnover);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApplMarginWithShopYN", pApplMarginWithShopYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pApplNetMrgPrctg", pApplNetMrgPrctg);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pApplNetMrgAmt", pApplNetMrgAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplOthMonthInc", pApplOthMonthInc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pApplBankSalAmt", pApplBankSalAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pApplCashSalAmt", pApplCashSalAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pApplRentAmt", pApplRentAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pApplGrsTurnoverOthBus", pApplGrsTurnoverOthBus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pApplNetMrgOthBusIncPrctg", pApplNetMrgOthBusIncPrctg);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pApplNetMrgOthBusInc", pApplNetMrgOthBusInc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pApplTotOthInc", pApplTotOthInc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCoAppGrsMonthTurnover", pCoAppGrsMonthTurnover);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCoApplMarginWithShopYN", pCoApplMarginWithShopYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCoAppNetMrgPrctg", pCoAppNetMrgPrctg);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCoAppNetMrgAmt", pCoAppNetMrgAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCoApplOthMonthInc", pCoApplOthMonthInc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCoApplBankSalAmt", pCoApplBankSalAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCoApplCashSalAmt", pCoApplCashSalAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCoApplRentAmt", pCoApplRentAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCoApplGrsTurnoverOthBus", pCoApplGrsTurnoverOthBus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCoApplNetMrgOthBusIncPrctg", pCoApplNetMrgOthBusIncPrctg);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCoApplNetMrgOthBusInc", pCoApplNetMrgOthBusInc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCoApplTotOthInc", pCoApplTotOthInc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTypeOfOwnership", pTypeOfOwnership);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pNoOfDependent", pNoOfDependent);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pHouseholdExp", pHouseholdExp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pResidenceCategory", pResidenceCategory);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pSurplus", pSurplus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanAmt", pLoanAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanTenure", pLoanTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pROI", pROI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pOthEMI", pOthEMI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pUSFBEMI", pUSFBEMI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotEMIObl", pTotEMIObl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pDBRPrctg", pDBRPrctg);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFOIRPrctg", pFOIRPrctg);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusDeviation", pBusDeviation);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCreditDeviation", pCreditDeviation);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", pMode);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYrNo", pYrNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", pLoanTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanAppAmt", pLoanAppAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCreditDeviationReason", pCreditDeviationReason);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public Int32 SendBackFOIR(int pPDId, int pCreatedBy, string pOperation, string pRejectReason)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vDigiConcentSMS = "", vDigiConcentSMSTemplateId = "", vDigiConcentSMSLanguage = "", vResultSendDigitalConcentSMS = "", vpMobileNo = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SendBackFOIR";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPDId", pPDId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pOperation", pOperation);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pRejectReason", pRejectReason);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.NVarChar, 1000, "@pMobileNo", vpMobileNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.NVarChar, 1000, "@pDigiConcentSMS", vDigiConcentSMS);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSTemplateId", vDigiConcentSMSTemplateId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSLanguage", vDigiConcentSMSLanguage);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);

                vpMobileNo = Convert.ToString(oCmd.Parameters["@pMobileNo"].Value);
                vDigiConcentSMS = Convert.ToString(oCmd.Parameters["@pDigiConcentSMS"].Value);
                vDigiConcentSMSTemplateId = Convert.ToString(oCmd.Parameters["@pDigiConcentSMSTemplateId"].Value);
                vDigiConcentSMSLanguage = Convert.ToString(oCmd.Parameters["@pDigiConcentSMSLanguage"].Value);

                if (vErr == 0)
                {
                    try
                    {
                        if (vDigiConcentSMS.Length > 0)
                        {
                            vResultSendDigitalConcentSMS = SendDigitalConcentSMS(vpMobileNo, vDigiConcentSMS, vDigiConcentSMSTemplateId, vDigiConcentSMSLanguage);
                        }
                    }
                    finally
                    {

                    }
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public string GetPropsedEMI(string pLoanAmt, string pLoanTenure, string pInstRate)
        {
            string pEMIAmt;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPropsedEMI";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanAmt", Convert.ToDouble(pLoanAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pLoanTenure", Convert.ToInt32(pLoanTenure));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstRate", Convert.ToDouble(pInstRate));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Float, 8, "@pEMI", 0);
                DBUtility.Execute(oCmd);
                pEMIAmt = Convert.ToString(oCmd.Parameters["@pEMI"].Value);
                return pEMIAmt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public DataTable GetDetailsForRiskCat(string pLoanNo, string pBrCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDetailsForRiskCat";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pLoanNo", pLoanNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBrCode", pBrCode);
                DBUtility.ExecuteForSelect(oCmd, dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public Int32 SaveRiskCategory(string pLoanID, string pRiskVal)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveRiskCategory";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pLoanID", pLoanID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRiskVal", pRiskVal.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public string SendDigitalConcentSMS(string pMobileNo, string pSMSContent, string pSMSTemplateId, string pSMSLanguage)
        {
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            String url = "";
            try
            {
                string vMsgBody = pSMSContent;
                String sendToPhoneNumber = pMobileNo;
                String userid = "2000204129";
                String passwd = "Unity@1122";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                if (pSMSLanguage.ToUpper() == "ENGLISH")
                {
                    vMsgBody = System.Web.HttpUtility.UrlEncode(vMsgBody, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                    url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=" + pSMSTemplateId + "&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
                }
                else
                {
                    url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=Unicode_text" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=" + pSMSTemplateId + "&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
                }
                request = WebRequest.Create(url);
                response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader reader = new System.IO.StreamReader(stream, ec);
                result = reader.ReadToEnd();
                reader.Close();
                stream.Close();
            }
            catch (Exception exp)
            {
                result = "Error sending SMS.." + exp.ToString();
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return result;
        }
    }
}
