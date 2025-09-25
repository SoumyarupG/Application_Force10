using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FORCEDA;
namespace FORCEBA
{
   public class CPettyReplenish
    {
       public DataTable GetUnReplpettyCash(string pBranchCode, DateTime pLoginDt)
       {
           DataTable dt = new DataTable();
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "GetUnReplpettyCash";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pBranchCode", pBranchCode);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLoginDt", pLoginDt);
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

       public DataSet GetLedgerGrid(string pBranchCode, string pPettyCashID, string pBankCash)
       {
           DataSet ds = new DataSet();
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "GetLedgerGridPettyRepl";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 11, "@pPettyCashID", pPettyCashID);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pBankCash", pBankCash);

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

       public DataTable GetPettyCash_ReplenishPG(int pCompId, string pBranchCode, string pTypeSrch)
       {
           DataTable dt = new DataTable();
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "GetPettyCash_ReplenishPG";         
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pTypeSrch", pTypeSrch);
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

       public Int32 InsertVoucherPettyCashReplenish(ref string pHeadID, ref String pVouNo, string pAcVouMstTbl, string pAcVouDtlTbl, DateTime pVoucherDt,
           string pVoucherType, string pReffType, string pReffId, string pNarration, DateTime pChequeDt, string pChequeNo, string pBank,
           string pFormType, DateTime pFinFromDt, DateTime pFinToDt, string pXml, string pFinYear, string pEntType, string pBranch,
           int pCreatedBy, Int32 pSynStatus, string @pPaidTo, double pAmount, string vPettyCashID, string pAcVouPostYN)
       {
           SqlCommand oCmd = new SqlCommand();
           Int32 vErr = 0;
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "InsertVoucherPettyCashReplenish";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pHeadID", pHeadID);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 20, "@pVouNo", pVouNo);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@TableMst", pAcVouMstTbl);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@Tabledtl", pAcVouDtlTbl);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@VoucherDt", pVoucherDt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@VoucherType", pVoucherType);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@ReffType", pReffType);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@ReffId", pReffId);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@Narration", pNarration.ToUpper());
               if (pChequeDt == null)
                   DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@ChequeDt", Convert.ToDateTime("01/01/1900"));
               else
                   DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@ChequeDt", pChequeDt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@ChequeNo", pChequeNo);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@Bank", pBank.ToUpper());
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pFormType", pFormType);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFinFromDt", pFinFromDt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFinToDt", pFinToDt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pFinYear", pFinYear);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@CreatedBy", pCreatedBy);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", pSynStatus);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPaidTo", pPaidTo);              
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAmount", pAmount);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 11, "@pPettyCashID", vPettyCashID);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAcVouPostYN", pAcVouPostYN);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pErrDesc", "");
               DBUtility.Execute(oCmd);
               vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
               string vErrDescc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
               pHeadID = Convert.ToString(oCmd.Parameters["@pHeadID"].Value);
               pVouNo = Convert.ToString(oCmd.Parameters["@pVouNo"].Value);
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

       public DataSet GetPettyCash_ReplenishById(string pID, string pAcVouMst)
       {
           DataSet ds = new DataSet();
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "GetPettyCash_ReplenishById";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pID", pID);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pAcVouMst", pAcVouMst);
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

       public Int32 UpdateVoucherPettyCashReplenish(string pAcVouMstTbl, string pAcVouDtlTbl, string pHeadID, string pVoucherNo, DateTime pVoucherDt,
         string pVoucherType, string pReffType, string pReffId, string pNarration, DateTime pChequeDt, string pChequeNo, string pBank,
         string pFormType, string pXml, string pEntType, string pBranch, Int32 pCreatedBy, Int32 pSynStatus, string pPaidTo
         , string pFinYear, double pAmount)
       {
           SqlCommand oCmd = new SqlCommand();
           Int32 vErr = 0;
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "UpdateVoucherPettyCashReplenish";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@TableMst", pAcVouMstTbl);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@Tabledtl", pAcVouDtlTbl);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pHeadID", pHeadID);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pVoucherNo", pVoucherNo);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pVoucherDt", pVoucherDt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pVoucherType", pVoucherType);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pReffType", pReffType);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pReffId", pReffId);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarration", pNarration.ToUpper());
               if (pChequeDt == null)
                   DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pChequeDt", Convert.ToDateTime("01/01/1900"));
               else
                   DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pChequeDt", pChequeDt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pChequeNo", pChequeNo);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pBank", pBank.ToUpper());
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pFormType", pFormType);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pFinYear", pFinYear);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", pSynStatus);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPaidTo", pPaidTo);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAmount", pAmount);
               DBUtility.Execute(oCmd);
               vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
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


       public Int32 DeleteVoucherPettyCash(string pAcVouMstTbl, string pAcVouDtlTbl, string pHeadID, string pVoucherNo, DateTime pVoucherDt, string pBranch, Int32 pCreatedBy
                               , string pChkAutoRev, string vRFRepId)
       {
           SqlCommand oCmd = new SqlCommand();
           Int32 vErr = 0;
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "DeleteVoucherPettyCash";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pAcVouMstTbl", pAcVouMstTbl);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pAcVouDtlTbl", pAcVouDtlTbl);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pHeadID", pHeadID);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pVoucherNo", pVoucherNo);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pVoucherDt", pVoucherDt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pChkAutoRev", pChkAutoRev);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pRFRepId", vRFRepId);
               DBUtility.Execute(oCmd);
               vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
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

       public DataTable GetPettyCashReplenishList(int pCompId, string pBranchCode, DateTime pFrmDt, DateTime pToDt)
       {
           DataTable dt = new DataTable();
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "GetPettyCashReplenishList";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pFrmDt", pFrmDt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pToDt", pToDt);
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
       public DataTable GetPettyCashReplenishPendingList(int pCompId, string pBranchCode, DateTime pLoginDt)
       {
           DataTable dt = new DataTable();
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "GetPettyCashUnReplenishList";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranchCode.Length+1, "@pBranchCode", pBranchCode);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLoginDt", pLoginDt);
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

       public Int32 PettyCashReplSendBack(string vPettyCashID,string pAcVouMstTbl, string pAcVouDtlTbl, DateTime pVoucherDt,DateTime pFinFromDt, DateTime pFinToDt,
           string pXml, string pFinYear, string pEntType, string pBranch, int pCreatedBy, DateTime pDate, string pSendBackRem)
       {
           SqlCommand oCmd = new SqlCommand();
           Int32 vErr = 0;
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "PettyCashReplSendBack";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 11, "@pPettyCashID", vPettyCashID);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pTableMst", pAcVouMstTbl);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pTableDtl", pAcVouDtlTbl);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pVoucherDt", pVoucherDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFinFromDt", pFinFromDt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFinToDt", pFinToDt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pFinYear", pFinYear);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pSendBackRem", pSendBackRem);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pErrDesc", "");               
               DBUtility.Execute(oCmd);
               vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
               string vErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
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
    }
}
