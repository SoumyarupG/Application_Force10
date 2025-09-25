using System;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
    public class CVoucher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pBrCode"></param>
        /// <returns></returns>
        public DataTable GetBank(string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBank";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pBrCode", pBrCode);
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
        public DataTable GetBankAll(string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBankAll";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pBrCode", pBrCode);
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
        public DataTable GetVoucherlist(string pTableMst, string pTableDtl, string pBranch, string pDateFrom, string pDateTo,
                    string pSearch, string pType, Int32 pPgIndx, ref Int32 pMaxRow)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetVoucherlist";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDtFrom", pDateFrom);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDtTo", pDateTo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pAcMst", pTableMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pAcDtl", pTableDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pType", pType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPgIndx", pPgIndx);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pMaxRow", pMaxRow);
                DBUtility.ExecuteForSelect(oCmd, dt);
                if (Convert.IsDBNull(oCmd.Parameters["@pMaxRow"].Value) == true)
                    pMaxRow = 0;
                else
                    pMaxRow = Convert.ToInt32(oCmd.Parameters["@pMaxRow"].Value);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pVoucherid"></param>
        /// <returns></returns>
        public DataTable GetVoucherDtl(string pTableMst, string pTableDtl, string pVoucherid, string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetVoucherDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@TableMst", pTableMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@TableDtl", pTableDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@Voucherid", pVoucherid);
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAcGenLedCB(string pBranch, string pType, string pDescID)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pType", pType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pDescID", pDescID);
                oCmd.CommandText = "GetAcGenLedCB";
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

        public DataTable PopDisbBank()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopDisbBank";
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

        public DataTable GetAcGenLedCB1(string pBranch, string pType, string pDescID, string pSearchText)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandText = "GetAcGenLedCB1";
                oCmd.CommandType = CommandType.StoredProcedure;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pType", pType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pDescID", pDescID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pSearchText", pSearchText);
              

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pVouNo"></param>
        /// <param name="pAcVouMstTbl"></param>
        /// <param name="pAcVouDtlTbl"></param>
        /// <param name="pVoucherDt"></param>
        /// <param name="pVoucherType"></param>
        /// <param name="pReffType"></param>
        /// <param name="pReffId"></param>
        /// <param name="pNarration"></param>
        /// <param name="pChequeDt"></param>
        /// <param name="pChequeNo"></param>
        /// <param name="pBank"></param>
        /// <param name="pFormType"></param>
        /// <param name="vFinFromDt"></param>
        /// <param name="vFinToDt"></param>
        /// <param name="pXmlYN"></param>
        /// <param name="pXml"></param>
        /// <param name="vFinYear"></param>
        /// <param name="pDescCB"></param>
        /// <param name="pDescL"></param>
        /// <param name="pDescI"></param>
        /// <param name="pPrin"></param>
        /// <param name="pInst"></param>
        /// <param name="pEntType"></param>
        /// <param name="pBranch"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pSynStatus"></param>
        /// <returns></returns>
        public Int32 InsertVoucher(ref string pHeadID, ref String pVouNo, string pAcVouMstTbl, string pAcVouDtlTbl, DateTime pVoucherDt,
            string pVoucherType, string pReffType, string pReffId, string pNarration, DateTime pChequeDt, string pChequeNo, string pBank,
            string pFormType, DateTime pFinFromDt, DateTime pFinToDt, string pXml, string pFinYear,
            string pEntType, string pBranch, int pCreatedBy, Int32 pSynStatus)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0; //string ErrDesc="";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertVoucherMst";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 20, "@pHeadID", pHeadID);
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
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length+1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@CreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", pSynStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", "");
                DBUtility.Execute(oCmd);
                //ErrDesc = oCmd.Parameters["@pErrDesc"].ToString();
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAcVouMstTbl"></param>
        /// <param name="pAcVouDtlTbl"></param>
        /// <param name="pHeadID"></param>
        /// <param name="pVoucherNo"></param>
        /// <param name="pVoucherDt"></param>
        /// <param name="pVoucherType"></param>
        /// <param name="pReffType"></param>
        /// <param name="pReffId"></param>
        /// <param name="pNarration"></param>
        /// <param name="pChequeDt"></param>
        /// <param name="pChequeNo"></param>
        /// <param name="pBank"></param>
        /// <param name="pFormType"></param>
        /// <param name="pXmlYN"></param>
        /// <param name="pXml"></param>
        /// <param name="pDescCB"></param>
        /// <param name="pDescL"></param>
        /// <param name="pDescI"></param>
        /// <param name="pPrin"></param>
        /// <param name="pInst"></param>
        /// <param name="pEntType"></param>
        /// <param name="pBranch"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pSynStatus"></param>
        /// <returns></returns>
        public Int32 UpdateVoucher(string pAcVouMstTbl, string pAcVouDtlTbl, string pHeadID, string pVoucherNo, DateTime pVoucherDt,
            string pVoucherType, string pReffType, string pReffId, string pNarration, DateTime pChequeDt, string pChequeNo, string pBank,
            string pFormType, string pXml,
            string pEntType, string pBranch, Int32 pCreatedBy, Int32 pSynStatus)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateVoucher";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@TableMst", pAcVouMstTbl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@Tabledtl", pAcVouDtlTbl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pHeadID", pHeadID);
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
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length+1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", pSynStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAcVouMstTbl"></param>
        /// <param name="pAcVouDtlTbl"></param>
        /// <param name="pHeadID"></param>
        /// <param name="pBranch"></param>
        /// <returns></returns>
        public Int32 DeleteVoucher(string pAcVouMstTbl, string pAcVouDtlTbl, string pHeadID, string pBranch, Int32 pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "DeleteVoucher";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pAcVouMstTbl", pAcVouMstTbl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pAcVouDtlTbl", pAcVouDtlTbl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pHeadID", pHeadID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
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

        public DataTable GetCityNm(string pBranch)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCityNm";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pBranch", pBranch);
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

        public DataTable GetDistNm(string pBranch)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDistNm";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pBranch", pBranch);
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

        public DataTable PopState()
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopState";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 10, "@Type", 'S');
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 12, "@pStateId", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 12, "@pDistId", 0);
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

        public DataTable PopDistNm(Int32 pStateId)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopState";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 10, "@Type", 'D');
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 12, "@pStateId", pStateId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 12, "@pDistId", 0);
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

        public DataTable PopBlockNm(Int32 pDistId)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopState";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 10, "@Type", 'B');
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 12, "@pStateId", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 12, "@pDistId", pDistId);
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
        public DataTable GetAccruedLed()
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAccruedLedList";
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
        public DataTable GetAccruedLedLend()
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAccruedLedListLend";
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
        public DataTable GetLendAccruedLedList()
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLendAccruedLedList";
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

