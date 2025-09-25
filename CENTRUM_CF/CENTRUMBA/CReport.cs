using System;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
    public class CReport
    {
        public DataTable GetBranchByBrCode(string pBrCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_GetBranchByBrCode";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
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

        public DataTable GetLoanNoByCustId(string pCustId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSancIdForColl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@CustId", pCustId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@val", "L");
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

        public DataTable GetCustForCollection()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCustNameForRecovery";
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

        public DataTable rptLoanDisb(DateTime pFromDt, DateTime pToDt, string pBranch, string pTypeID)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "rptLoanDisb";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2000, "@pTypeID", pTypeID);
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

        public DataSet rptPartyLedgerPDF1st(string pSancId, string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "rptPartyLedgerPDF1st";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pSancId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
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

        public DataSet rptPartyLedgerPDF(string pSancId, string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "rptPartyLedgerPDF";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pSancId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
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

        public DataSet rptPartyLedger(string pSancId, string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            //DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "rptPartyLedger";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pSancId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
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

        public DataSet rptPartyLedger1st(string pSancId, string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            //DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "rptPartyLedger1st";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pSancId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
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

        public DataTable rptRepaySchedule(string pLoanId, string pBrCode, string pIsDisburse)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "rptRepaySchedule";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsDisburse", pIsDisburse);
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

        public DataTable rptRepaySchedule1st(string pLoanId, string pBrCode, string pIsDisburse)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "rptRepaySchedule1st";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsDisburse", pIsDisburse);
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

        public DataSet CF_Equifax_Report(Int64 pLeadID, string pCustType, ref string pEnqStatusMsg)
        {

            SqlCommand oCmd = new SqlCommand();
            DataSet ds = null;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_Equifax_Report";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pLeadID", pLeadID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pCustType", pCustType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 500, "@pEnqStatusMsg", pEnqStatusMsg);
                ds = DBUtility.GetDataSet(oCmd);
                if (String.IsNullOrEmpty(oCmd.Parameters["@pEnqStatusMsg"].Value.ToString()))
                    pEnqStatusMsg = "";
                else
                    pEnqStatusMsg = oCmd.Parameters["@pEnqStatusMsg"].Value.ToString();
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

        public DataTable rptLoanDisbXls(DateTime pFromDt, DateTime pToDt, string pBranch, string pType, string pTypeID)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "rptLoanDisbXL";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranch", pBranch);
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

        public static string GetBranchAddress1(string pBranch)
        {
            string vAdd1 = "";
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchAddress1";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 150, "@pAdd1", vAdd1);
                DBUtility.Execute(oCmd);
                vAdd1 = Convert.ToString(oCmd.Parameters["@pAdd1"].Value);
                return vAdd1;
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


        public static string GetBranchAddress2(string pBranch)
        {
            string vAdd1 = "";
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchAddress2";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 150, "@pAdd2", vAdd1);
                DBUtility.Execute(oCmd);
                vAdd1 = Convert.ToString(oCmd.Parameters["@pAdd2"].Value);
                return vAdd1;
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

        public DataTable GetInsuClaimFormDtl(string pLoanId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetInsuClaimFormDtl";
                oCmd.CommandTimeout = 96000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanId", pLoanId);
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

        public DataTable PopComboMIS(string pCondition, string pCodeAdd, string pCodeName, string pIDName, string pName, string pTbl, Int32 pConID,
                                   string pConIDName, string pConIDDate, DateTime pConDate, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopComboMIS";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pCondition", pCondition);// N=Normal, S=one condition, D=Double Condition
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pCodeAdd", pCodeAdd);// Y=Yes, N=No for (Name + Code)
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pCodeName", pCodeName);// DataTextField Name of Code [Default AA]
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIDName", pIDName);// Table Field Name(Id)
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pName", pName);// Table Field Name(Name)
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTbl", pTbl);// Table Name
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pConID", pConID);// Conditional popup upon another field [Default 0]
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pConIDName", pConIDName);// Conditional Table Field Name [Default AA]
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pConIDDate", pConIDDate);// Date Table Field Name [Default AA]
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pConDate", pConDate);// Date (Parameter) [Default 01/01/1900]
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranchCode", pBranchCode); // Branch Code [Default 000]
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


        public DataTable GetBranchByState(string pUserName, Int32 pRoleID, string pStateID)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchByState";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUserName", pUserName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pRoleID", pRoleID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pStateID.Length + 1, "@pStateID", pStateID);
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

        public DataTable rptLoanCollectionXL(DateTime pFromDt, DateTime pToDt, string pBranch, string pTypeID)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "rptLoanCollectionXL";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranch", pBranch);             
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8000, "@pTypeID", pTypeID);
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

        public DataTable CF_rptHypothecationdtls(string pSearch, string pBranch, DateTime pFromDt, DateTime pToDt)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_rptHypothecationdtls";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pSearch.Length + 1, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranchCode", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
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

        public DataTable CF_rptCustomerDtls(string pSearch, string pBranch, DateTime pFromDt, DateTime pToDt)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_rptCustomerDtls";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pSearch.Length + 1, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranchCode", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
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

        public DataTable CF_rptEpcBankDtls(string pSearch, DateTime pFromDt, DateTime pToDt)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_rptEpcBankDtls";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pSearch.Length + 1, "@pSearch", pSearch);               
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
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

        public DataTable CF_rptBranchMstDtls(string pSearch, string pBranch)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_rptBranchMstDtls";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pSearch.Length + 1, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranchCode", pBranch);
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

        public DataTable CF_rptAllEpcDtls(string pSearch, DateTime pFromDt, DateTime pToDt)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_rptAllEpcDtls";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pSearch.Length + 1, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
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

        public DataTable CF_rptSanctionCndtn(string pSearch, string pBranch, DateTime pFromDt, DateTime pToDt)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_rptSanctionCndtn";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pSearch.Length + 1, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranchCode", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
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

        public DataTable CF_rptPreDBDtls(string pSearch, string pBranch, DateTime pFromDt, DateTime pToDt)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_rptPreDBDtls";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pSearch.Length + 1, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranchCode", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
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

        public DataTable CF_rptCustomerBankDtl(string pSearch, string pBranch, DateTime pFromDt, DateTime pToDt)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_rptCustomerBankDtl";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pSearch.Length + 1, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranchCode", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
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

        public DataTable CF_rptUserMstDtls(DateTime pAsOnDate, string pBranch, int pRoleId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_rptUserMstDtls";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAsOnDate", pAsOnDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranchCode", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pRoleId", pRoleId);
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

        public DataTable CF_rptDeathTaggingDtls(DateTime pAsOnDate, string pBranch)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_rptDeathTaggingDtls";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranchCode", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAsOnDate", pAsOnDate);
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

        public DataTable CF_rptCustomerChargesDtls(DateTime pAsOnDate, string pBranch)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_rptCustomerChargesDtls";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranchCode", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAsOnDate", pAsOnDate);
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

        public DataTable CF_rptDeathSettlement(DateTime pAsOnDate, string pBranch)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_rptDeathSettlement";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranchCode", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAsOnDate", pAsOnDate);
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

        public DataTable CF_rptPrematureCollection(DateTime pAsOnDate, string pBranch)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_rptPrematureCollection";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranchCode", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAsOnDate", pAsOnDate);
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
