using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FORCEDA;
using System.Net;
using System.IO;

namespace FORCEBA
{
    public class CDeviationMatrix
    {
        public DataTable GetDeviationData(DateTime pFrmDate, DateTime pToDate, string pMode, string pBranch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDeviationData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length +1, "@pBrCode", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pFrmDate", pFrmDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 1000, "@pToDate", pToDate);
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

        public Int32 SaveDeviationMatrix(string pXmlMst, string pXmlDtl, string pApplObligXmlData, string pCoApplObligXmlData, string pBMId, string pBMRemarks, string pAMId, Int32 pCreatedBy, string pDocUploadYN, string pSRC = "D", double pRecomAmt = 0, double pBMRecoAmt = 0, string pAssetType = "Q")
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveDeviationMatrix";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlMst.Length + 1, "@pXmlMst", pXmlMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlDtl.Length + 1, "@pXmlDtl", pXmlDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pApplObligXmlData.Length + 1, "@pApplObligXmlData", pApplObligXmlData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pCoApplObligXmlData.Length + 1, "@pCoApplObligXmlData", pCoApplObligXmlData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pBMId", pBMId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pBMRemarks", pBMRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pAMId", pAMId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pDocUploadYN", pDocUploadYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSRC", pSRC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pRecomAmt", pRecomAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pBMRecoAmt", pBMRecoAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAssetType", pAssetType);
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

        //public DataTable GetDeviationDataById(string pEnquiryId, Int32 pCBID, string pDesig)
        //{
        //    SqlCommand oCmd = new SqlCommand();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        oCmd.CommandType = CommandType.StoredProcedure;
        //        oCmd.CommandText = "GetDeviationDataById";
        //        oCmd.CommandTimeout = 80000;
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEnquiryId", pEnquiryId);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCBID", pCBID);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pDesig", pDesig);
        //        DBUtility.ExecuteForSelect(oCmd, dt);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet GetDeviationDataById(string pEnquiryId, Int32 pCBID, string pDesig)
        {
            SqlCommand oCmd = new SqlCommand();
            DataSet ds = null;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDeviationDataById";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEnquiryId", pEnquiryId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCBID", pCBID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pDesig", pDesig);
                ds = DBUtility.GetDataSet(oCmd);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 SaveDevRecom(string pEnquiryId, Int32 pCBID, string pEoId, string pRemarks, string pDesig, double pRecomAmt, string pApplObligXmlData, string pCoApplObligXmlData)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveDevRecom";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEnquiryId", pEnquiryId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCBID", pCBID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoId", pEoId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pRemarks", pRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pDesig", pDesig);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pRecomAmt", pRecomAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pApplObligXmlData.Length + 1, "@pApplObligXmlData", pApplObligXmlData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pCoApplObligXmlData.Length + 1, "@pCoApplObligXmlData", pCoApplObligXmlData);
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

        public Int32 ApproveDeviationMatrix(string pEnquiryId, Int32 pCBID, string pRemarks, string pMode, Int32 pCreatedBy,
            double pAmountApplied, double pEligibleEMI, double pTotalObligation, string pDesig, string pApplObligXmlData,
            string pCoApplObligXmlData)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vDigiConcentSMS = "", vDigiConcentSMSTemplateId = "", vDigiConcentSMSLanguage = "", vResultSendDigitalConcentSMS = "", vpMobileNo = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ApproveDeviationMatrix";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEnquiryId", pEnquiryId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCBID", pCBID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pRemarks", pRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAmountApplied", pAmountApplied);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pEligibleEMI", pEligibleEMI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotalObligation", pTotalObligation);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pDesig", pRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pApplObligXmlData.Length + 1, "@pApplObligXmlData", pApplObligXmlData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pCoApplObligXmlData.Length + 1, "@pCoApplObligXmlData", pCoApplObligXmlData);

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

        public string GetEmailIdbyEoid(string pEoid)
        {
            SqlCommand oCmd = new SqlCommand();
            string vEmail = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetEmailIdbyEoid";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoid", pEoid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 150, "@pEmail", vEmail);
                DBUtility.Execute(oCmd);
                vEmail = Convert.ToString(oCmd.Parameters["@pEmail"].Value);
                return vEmail;
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

        #region  Send SMS 
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
        #endregion

        public DataTable getOwnOS(string pEnqId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "getOwnOS";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEnqId", pEnqId);               
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

    }
}
