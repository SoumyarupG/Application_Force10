using System;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
    public class CDistrict
    {

        public DataTable CF_GetLeadDtlByLeadId(Int64 pLeadId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_GetLeadDtlByLeadId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLeadId", pLeadId);
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

        public DataTable CF_GetUpldDocDtlByLeadID(Int64 pLeadId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_GetUpldDocDtlByLeadID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLeadId", pLeadId);
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

        public DataTable CF_GetOtherRepDtlByLeadID(Int64 pLeadId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_GetOtherRepDtlByLeadID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLeadId", pLeadId);
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

        public DataTable CF_GetBCRepDtlByLeadID(Int32 pLeadId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_GetBCRepDtlByLeadID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLeadId", pLeadId);
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

        public DataTable CF_GetExtrnlChkDtlLeadID(Int64 pLeadId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_GetExtrnlChkDtlLeadID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLeadId", pLeadId);
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

        public DataTable CF_GetElecConDtlByLeadID(Int32 pLeadId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_GetElecConDtlByLeadID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLeadId", pLeadId);
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

        
        public DataTable CF_GetBasicDetailsById(Int32 pBasicId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_GetBasicDetailsById";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBasicId", pBasicId);
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

        public DataTable CF_GetEmployDtlByLeadID(Int64 pLeadID, string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_GetEmployDtlByLeadID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pLeadID", pLeadID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pBrCode", pBrCode);
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

      

        public Int32 CF_SaveEmplymntDet(Int64 pLeadID, string pEmplrName, string pDesignation, string pOffcAddrs, DateTime pJoinDate, Int32 pCrJobStblty, decimal pTotExp
            , string pDocDescr, string pEmpFileName, string pFileStoredPath, Int32 pCreatedBy, string pIsFileUpload, string pBranchCode, string pMode, Int32 pErr, ref string pErrMsg)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_SaveEmplymntDet";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pLeadID", pLeadID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEmplrName", pEmplrName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pDesignation", pDesignation);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pOffcAddrs", pOffcAddrs);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pJoinDate", pJoinDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCrJobStblty", pCrJobStblty);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotExp", pTotExp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pDocDescr", pDocDescr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEmpFileName", pEmpFileName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pFileStoredPath", pFileStoredPath);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pIsFileUpload", pIsFileUpload);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrMsg", pErrMsg);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                if (vErr == 0)
                    return 0;
                else
                    return 1;
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

        public Int32 CF_SaveBusinessDet(Int64 pLeadID, string pBranchCode, string pBusiName, string pIndustry, string pBusiType, DateTime pIncorpDate, string pBusiVintYrs,
            string pBusiStabYrs,Int32 pBusiPremiOwn, string pUdyamRegnNo, string pUdyamRegnCertUpYN, string pUdyamRegnFileName, string pUdyamRegnFileStoredPath,
            string pBKYC2Name, string pBKYC2No, string pBKYC2UpYN, string pBKYC2FileName, string pBKYC2FileStoredPath, string pBusiAddr, string pBusiAddrOwnUpYN,
            string pBusiAddrOwnFileName, string pBusiAddrOwnFileStoredPath,Int32 pCreatedBy, string pMode, Int32 pErr, ref string pErrMsg)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_SaveBusinessDet";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pLeadID", pLeadID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusiName", pBusiName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pIndustry", pIndustry);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBusiType", pBusiType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pIncorpDate", pIncorpDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pBusiVintYrs", pBusiVintYrs);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pBusiStabYrs", pBusiStabYrs);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4,"@pBusiPremiOwn", pBusiPremiOwn);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUdyamRegnNo", pUdyamRegnNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pUdyamRegnCertUpYN", pUdyamRegnCertUpYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pUdyamRegnFileName", pUdyamRegnFileName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pUdyamRegnFileStoredPath", pUdyamRegnFileStoredPath);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBKYC2Name", pBKYC2Name);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pBKYC2No", pBKYC2No);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBKYC2UpYN", pBKYC2UpYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pBKYC2FileName", pBKYC2FileName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBKYC2FileStoredPath", pBKYC2FileStoredPath);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pBusiAddr", pBusiAddr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBusiAddrOwnUpYN", pBusiAddrOwnUpYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pBusiAddrOwnFileName", pBusiAddrOwnFileName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusiAddrOwnFileStoredPath", pBusiAddrOwnFileStoredPath);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrMsg", pErrMsg);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                if (vErr == 0)
                    return 0;
                else
                    return 1;
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

        public DataTable CF_GetBusinessDtlByLeadID(Int64 pLeadID, string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_GetBusinessDtlByLeadID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pLeadID", pLeadID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pBrCode", pBrCode);
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
        public DataSet CF_GetInsuranceChargeByLeadID(Int64 pLeadID)
        {
            DataSet ds = null;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLeadID", pLeadID);
                oCmd.CommandText = "CF_GetInsuranceChargeByLeadID";
                ds = DBUtility.GetDataSet(oCmd);
                return ds;
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

        #region Recommendation
        public DataSet CF_GenerateRecommendation(Int64 pLeadID)
        {
            DataSet ds = null;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadID);
                oCmd.CommandText = "CF_GenerateRecommendation";
                ds = DBUtility.GetDataSet(oCmd);
                return ds;
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

        public Int32 CF_GenerateInternalScore(Int64 pLeadId, Int32 pInternalScore)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vInternalScore = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_GenerateInternalScore";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadId", pLeadId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pInternalScore", pInternalScore);
                DBUtility.Execute(oCmd);
                vInternalScore = Convert.ToInt32(oCmd.Parameters["@pInternalScore"].Value);
                
                return vInternalScore;
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

        public DataTable ViewInternalScore(Int64 pLeadID)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_ViewInternalScore";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pLeadId", pLeadID);               
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

        public Int32 CF_SaveRecommendation(Int64 pLeadId, string pBrCode, double pLoanAmt, double pROI, double pTenure, double pInternalScore, string pAction, string pRemarks, string pFileName, string pIsUpload, string pFileStoredPath, Int32 pCreatedBy, string pMode, Int32 pErr, ref string pErrMsg)
           
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_SaveRecommendation";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@pLoanAmt", pLoanAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@pROI", pROI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@pTenure", pTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@pInternalScore", pInternalScore);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAction", pAction);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pRemarks", pRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pFileName", pFileName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsUpload", pIsUpload);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pFileStoredPath", pFileStoredPath);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, -1, "@pErrMsg", pErrMsg);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                if (vErr == 0)
                    return 0;
                else
                    return 1;
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

        #endregion

        

        #region LTVComputation
        public DataSet CF_GetLTVComputationByLeadID(Int64 pLeadID)
        {
            DataSet ds = null;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadID);
                oCmd.CommandText = "CF_GetLTVComputationByLeadID";
                ds = DBUtility.GetDataSet(oCmd);
                return ds;
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

        public Int32 CF_SaveLTVComputation(Int64 pLeadId, double pQuotationCostVal, double pGovCostVal, double pMaxEligibleFundingA, double pMaxEligibleFundingB, double pMarginMoney, string pMarMonFileNm, string pMarMonIsUpLoad, string pMarMonBankFileNm, string pMarMonBankIsUpLoad, string pQuotCopyFileNm, string pQuotCopyIsUpLoad, string pFileStorePath, string pBrCode, Int32 pCreatedBy, string pMode, Int32 pErr, ref string pErrMsg)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_SaveLTVComputation";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@pQuotationCostVal", pQuotationCostVal);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@pGovCostVal", pGovCostVal);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@pQuotationCostAllowFundPercent", pMaxEligibleFundingA);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@pGovCostAllowFundPercent", pMaxEligibleFundingB);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@pMarginMoneyAmount", pMarginMoney);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMoneyMarginReceiptFN", pMarMonFileNm);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMoneyMarginReceiptIsUp", pMarMonIsUpLoad);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMoneyMarginBankRflctnFN", pMarMonBankFileNm);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMoneyMarginBankRflctnIsUp", pMarMonBankIsUpLoad);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pQuationCopyFN", pQuotCopyFileNm);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQuationCopyIsUp", pQuotCopyIsUpLoad);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pFileStoredPath", pFileStorePath);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranchCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrMsg", pErrMsg);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                if (vErr == 0)
                    return 0;
                else
                    return 1;
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
        #endregion

        #region BankingDetails
        public DataSet CF_GenerateBankingGrid(Int64 pLeadID)
        {
            DataSet ds = null;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadID);
                oCmd.CommandText = "CF_GenerateBankingGrid";
                ds = DBUtility.GetDataSet(oCmd);
                return ds;
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

        public Int32 CF_SaveBankingDtl(Int64 pLeadId, string pXmlBank, string pBrCode, string pDocumentFilePath
            , Int32 pCreatedBy, Int32 pErr, ref string pErrMsg,string pRemarks)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_SaveBankingDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlBank.Length + 1, "@pXmlBank", pXmlBank);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrMsg", pErrMsg);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pRemarks", pRemarks);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                if (vErr == 0)
                    return 0;
                else
                    return 1;
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

        #endregion

        public Int32 CF_SaveInsuChrg(string pLeadId, string pXmlInsuChrg, string pBrCode, Int32 pCreatedBy, Int32 pErr, ref string pErrMsg)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_SaveInsuChrg";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLeadID", pLeadId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlInsuChrg.Length + 1, "@pXmlInsuChrg", pXmlInsuChrg);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrMsg", pErrMsg);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                if (vErr == 0)
                    return 0;
                else
                    return 1;
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

        #region SolarSystem
        public DataSet CF_GenerateSolarSystemGrid(Int64 pLeadID)
        {
            DataSet ds = null;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadID);
                oCmd.CommandText = "CF_GenerateSolarSystemGrid";
                ds = DBUtility.GetDataSet(oCmd);
                return ds;
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

        public Int32 CF_SaveSolarDtl(string pLeadId, string pXmlSolar, string pBrCode
           , Int32 pCreatedBy, Int32 pErr, ref string pErrMsg, string pPropAddress, string pPropOwnership, string pPropOwnerName)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_SaveSolarDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlSolar.Length + 1, "@pXmlSolar", pXmlSolar);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pPropAddress", pPropAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pPropOwnership", pPropOwnership);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPropOwnerName", pPropOwnerName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrMsg", pErrMsg);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                if (vErr == 0)
                    return 0;
                else
                    return 1;
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

        #endregion

        #region SactionCondition
        public DataSet CF_GenerateSanctionGrid(Int64 pLeadID)
        {
            DataSet ds = null;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadID);
                oCmd.CommandText = "CF_GenerateSanctionGrid";
                ds = DBUtility.GetDataSet(oCmd);
                return ds;
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

        public Int32 CF_SaveSactionConditn(Int64 pLeadId, string pXmlSanc, string pBrCode
            , Int32 pCreatedBy, Int32 pErr, ref string pErrMsg)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_SaveSactionConditn";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlSanc.Length + 1, "@pXmlSanc", pXmlSanc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrMsg", pErrMsg);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                if (vErr == 0)
                    return 0;
                else
                    return 1;
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

        #endregion

        #region Deviation
        public DataSet CF_GenerateDeviationGrid(Int64 pLeadID)
        {
            DataSet ds = null;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadID);
                oCmd.CommandText = "CF_GenerateDeviationGrid";
                ds = DBUtility.GetDataSet(oCmd);
                return ds;
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

        public Int32 CF_SaveDeviation(Int64 pLeadId, string pXmlDev, string pBrCode
            , Int32 pCreatedBy, Int32 pErr, ref string pErrMsg)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_SaveDeviation";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlDev.Length + 100, "@pXmlDev", pXmlDev);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrMsg", pErrMsg);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                if (vErr == 0)
                    return 0;
                else
                    return 1;
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

        #endregion

        #region FinalDecision
        public DataSet CF_GenerateFinalDecision(Int64 pLeadID)
        {
            DataSet ds = null;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadID);
                oCmd.CommandText = "CF_GenerateFinalDecision";
                ds = DBUtility.GetDataSet(oCmd);
                return ds;
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



        public Int32 CF_SaveFinalDecision(Int64 pLeadId, Int32 pCreatedBy, Int32 pErr, ref string pErrMsg,
            string pSacComm, string pSacRemarks, DateTime pSacDt, string pRecommRemarks, string pApprovedStatus, DateTime pSacExDt)
        {
            SqlCommand oCmd = new SqlCommand(); 
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_SaveFinalDecision";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadId);                
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);              
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrMsg", pErrMsg);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pSacComm", pSacComm);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pSacRemarks", pSacRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pSacDt", pSacDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pRecommRemarks", pRecommRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApprovedStatus", pApprovedStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pSacExDt", pSacExDt);
          

                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                if (vErr == 0)
                    return 0;
                else
                    return 1;
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

        #endregion

        #region OpsLoanApplication
        public DataTable CF_GetOpsBusinessDtlByLeadID(Int64 pLeadID, string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_GetOpsBusinessDtlByLeadID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pLeadID", pLeadID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pBrCode", pBrCode);
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

        public Int32 CF_SaveOpsBusinessDet(Int64 pLeadID, string pBranchCode, string pBusiName, string pIndustry, string pBusiType, DateTime pIncorpDate, string pBusiVintYrs,
            string pBusiStabYrs, Int32 pBusiPremiOwn, string pUdyamRegnNo, string pUdyamRegnCertUpYN, string pUdyamRegnFileName, string pUdyamRegnFileStoredPath,
            string pBKYC2Name, string pBKYC2No, string pBKYC2UpYN, string pBKYC2FileName, string pBKYC2FileStoredPath, string pBusiAddr, string pBusiAddrOwnUpYN,
            string pBusiAddrOwnFileName, string pBusiAddrOwnFileStoredPath, Int32 pCreatedBy, string pMode, Int32 pErr, ref string pErrMsg, string pMajorActivity,
            string pEntrpriseType, string pPSL, string pOpsIndustry, DateTime pBKYCInDt)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_SaveOpsBusinessDet";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pLeadID", pLeadID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusiName", pBusiName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pIndustry", pIndustry);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBusiType", pBusiType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pIncorpDate", pIncorpDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pBusiVintYrs", pBusiVintYrs);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pBusiStabYrs", pBusiStabYrs);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBusiPremiOwn", pBusiPremiOwn);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUdyamRegnNo", pUdyamRegnNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pUdyamRegnCertUpYN", pUdyamRegnCertUpYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pUdyamRegnFileName", pUdyamRegnFileName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pUdyamRegnFileStoredPath", pUdyamRegnFileStoredPath);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBKYC2Name", pBKYC2Name);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pBKYC2No", pBKYC2No);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBKYC2UpYN", pBKYC2UpYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pBKYC2FileName", pBKYC2FileName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBKYC2FileStoredPath", pBKYC2FileStoredPath);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pBusiAddr", pBusiAddr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBusiAddrOwnUpYN", pBusiAddrOwnUpYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pBusiAddrOwnFileName", pBusiAddrOwnFileName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusiAddrOwnFileStoredPath", pBusiAddrOwnFileStoredPath);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrMsg", pErrMsg);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pMajorActivity", pMajorActivity);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pEnterpriseType", pEntrpriseType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pPSL", pPSL);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pOpsIndustry", pOpsIndustry);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pBKYC2Dt", pBKYCInDt);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
               
                if (vErr == 0)
                    return 0;
                else
                    return 1;
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

        
     


        public Int32 CF_SaveOPSBankingDtl(int pBankSlId, Int64 pLeadID, string pBranchCode, string pACHolderNm, string pAccType, string pBankNm,  string pACNo,
           string pIFCE, string pPenyDrpStatus, string pRseMsg, string PaymentTran,DateTime pEffDt, string pPaymntMode,
           string pNachType, string pNachUMRN, string pNachRef, string pPassbkFileName, string pPassbkYN ,string pFileStoredPath, Int32 pCreatedBy,
            string pMode, Int32 pErr, ref string pErrMsg, Int32 pChequeCnt, Int32 pWaiverCnt, string pXmlChq, string pPhyMandtFilenm, string pPhyMandtIsUpld)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_SaveOPSBankingDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBankSlId", pBankSlId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pLeadID", pLeadID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAcHoldName", pACHolderNm);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pBankName", pBankNm);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAccNo", pACNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAcType", pAccType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 11, "@pIFSCCode", pIFCE);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPassBkFilenm", pPassbkFileName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pPassbkIsUpld", pPassbkYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pFileStoredPath", pFileStoredPath);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pChequeCnt", pChequeCnt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pWaiverCnt", pWaiverCnt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pPennyDropStatus", pPenyDrpStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pResMsg", pRseMsg);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPaymentTran", PaymentTran);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pEffDate", pEffDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPaymentMode", pPaymntMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNachType", pNachType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNachUMRN", pNachUMRN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNachRefNo", pNachRef);            
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrMsg", pErrMsg);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlChq.Length + 1, "@pXmlChq", pXmlChq);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPhyMandtFilenm", pPhyMandtFilenm);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPhyMandtIsUpld", pPhyMandtIsUpld);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);

                if (vErr == 0)
                    return 0;
                else
                    return 1;
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


        public DataTable CF_GetOpsBankingdtls(Int64 pLeadID, string pAcNo)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_GetOpsBankingdtls";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pLeadID", pLeadID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pAcNo", pAcNo);
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


       
        public DataSet GenerateChequeGrid(Int64 pLeadID, string pAccNo)
        {
            DataSet ds = null;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLeadID", pLeadID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAccNo", pAccNo);
                oCmd.CommandText = "CF_GetOpsChequedtls";
                ds = DBUtility.GetDataSet(oCmd);
                return ds;
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


        #endregion


        

    }
}