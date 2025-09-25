using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CENTRUMDA;
using System.Data;
using System.Data.SqlClient;

namespace CENTRUMBA
{
   public class CLoan
    {
       public DataTable GetLoanByMemId(string pMemberId, string pBrCode, DateTime plogindate)
       {
           DataTable dt = new DataTable();
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "GetLoanByMemId";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 11, "@pMemberId", pMemberId);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@plogindate", plogindate);
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

       public DataSet GetLoanDtlByMemberIDLA(string pLoanId, string pInterface, DateTime pDate)
       {
           DataSet dt = new DataSet();
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "GetLoanDtlByMemberIDLA";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pInterface", pInterface);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
               dt = DBUtility.GetDataSet(oCmd);
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

       public Int32 SaveMemberLoanCollectionAdj(string pLoanId, string pOffsetId, string pMemberId, string pLedgerAc, string pPrinAc, string pIntAc, Double pPOffSetAmt, Double pIOffSetAmt, string pTblMst, string pTblDtl, string pFinYear, DateTime pTranDt, string pBranchCode, Int32 pCreatedBy, string pMode, int pAcType)
       {
           SqlCommand oCmd = new SqlCommand();
           Int32 vErr = 0;
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "SaveMemberLoanCollectionAdj";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pOffsetId", pOffsetId);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLedgerAc", pLedgerAc);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pPrinAc", pPrinAc);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pIntAc", pIntAc);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pPOffSetAmt", pPOffSetAmt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pIOffSetAmt", pIOffSetAmt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 8, "@pTranDt", pTranDt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranchCode);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", pMode);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAcType", pAcType);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
               DBUtility.Execute(oCmd);
               vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
               return vErr;
               //if (vErr == 0)
               //{
               //    return 1;
               //}
               //else
               //{
               //    return 0;
               //}
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

       public DataTable GetMemberLoanCollAdjList(DateTime pFromDt, DateTime pToDt, string pBrCode, string pSearch)
       {
           SqlCommand oCmd = new SqlCommand();
           DataTable dt = new DataTable();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "GetMemberLoanCollAdjList";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pSearch", pSearch);
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

       public DataSet GetMemberLoanCollAdjDetails(string pOffSetId)
       {
           SqlCommand oCmd = new SqlCommand();
           DataSet ds = new DataSet();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "GetMemberLoanCollAdjDetails";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pOffSetId", pOffSetId);
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
    }
}
