using System;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
    public class CYBLoanCol
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRecDt"></param>
        /// <param name="pBranch"></param>
        /// <returns></returns>
        public DataTable GetYbLoanDue(DateTime pRecDt, string pBranch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetYbLoanDue";
                oCmd.CommandTimeout = 4800;

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pRecDt", pRecDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pBranch", pBranch);
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
        /// <param name="pLoanId"></param>
        /// <param name="pBranch"></param>
        /// <param name="pRecDt"></param>
        /// <returns></returns>
        public DataSet GETYBCOLLECTIONDETAIL(string pLoanId, string pBranch, DateTime pRecDt)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GETYBCOLLECTIONDETAIL";
                oCmd.CommandTimeout = 4800;

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pRecDt", pRecDt);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pLoanId"></param>
        /// <param name="pTotalPaid"></param>
        /// <param name="pCollDt"></param>
        /// <param name="pPColl"></param>
        /// <param name="pIColl"></param>
        /// <param name="pInstSize"></param>
        public void GetPrinIntAsOnDt(string pLoanId, double pTotalPaid, DateTime pCollDt, ref double pPColl, ref double pIColl, ref double pInstSize)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "YBGetPrinIntSv";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pTotalPaid", pTotalPaid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 12, "@pCollDt", pCollDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Float, 10, "@pPColl", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Float, 10, "@pIColl", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Float, 10, "@pInstSize", 0);
                DBUtility.Execute(oCmd);
                pPColl = Convert.ToDouble(oCmd.Parameters["@pPColl"].Value);
                pIColl = Convert.ToDouble(oCmd.Parameters["@pIColl"].Value);
                pInstSize = Convert.ToDouble(oCmd.Parameters["@pInstSize"].Value);
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
        /// <param name="pLoanId"></param>
        /// <param name="pRecDate"></param>
        /// <param name="pXml"></param>
        /// <param name="pBranchCode"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pMode"></param>
        /// <param name="pMaxSlNo"></param>
        /// <returns></returns>
        public Int32 SaveYBLoanCollection(string pLoanId, DateTime pRecDate, string pXml, string pBranchCode, Int32 pCreatedBy, string pMode, Int32 pMaxSlNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveYBLoanCollection";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pRecDate", pRecDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 1, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 5, "@pMaxSlNo", pMaxSlNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", "");
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                //string vErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pBranch"></param>
        /// <param name="pRecDt"></param>
        /// <returns></returns>
        public DataTable GetYBLoanNo(string pBranch, DateTime pRecDt)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetYBLoanNo";
                oCmd.CommandTimeout = 4800;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pRecDt", pRecDt);
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

        public DataSet GetYbLoanDtlById(string pYbLoanId, string pBranch, DateTime pRecDt)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetYbLoanDtlById";
                oCmd.CommandTimeout = 4800;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pYbLoanId", pYbLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pRecDt", pRecDt);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pYBLoaId"></param>
        /// <param name="pPayAmt"></param>
        /// <param name="pPayDt"></param>
        /// <param name="pXml"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pMode"></param>
        /// <param name="pBrCode"></param>
        /// <returns></returns>
        public Int32 InsertYBBulkPmt(string pYBLoaId, double pPayAmt, DateTime pPayDt, Int32 pInstNo, double pOutAmt, string pPayType, string pXml, Int32 pCreatedBy, string pMode, string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertYBBulkPmt";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pYBLoaId", pYBLoaId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPayAmt", pPayAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPayDt", pPayDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pInstNo", pInstNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pOutAmt", pOutAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPayType", pPayType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBrCode", pBrCode);
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
