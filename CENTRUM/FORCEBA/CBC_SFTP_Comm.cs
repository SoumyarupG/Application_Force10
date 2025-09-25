using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FORCEDA;

namespace FORCEBA
{
    public class CBC_SFTP_Comm
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet BC_GetIDBI_SFTP_Upload_AfterReject(string pXML, string pBrCode, DateTime pFrmDt, string pFileType)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                if (pFileType == "CUST")
                    oCmd.CommandText = "BC_Re_IDBI_SFTP_Upload_AfterReject";
                else if (pFileType == "JLG")
                    oCmd.CommandText = "BC_GetIDBI_JLGTAG_Upload";
                else if (pFileType == "JCONF")
                    oCmd.CommandText = "BC_GetIDBI_Conf_Disb_Upload";

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXML.Length + 1, "@pXML", pXML);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrmDt", pFrmDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBrCode.Length + 1, "@pBrCode", pBrCode);
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
        /// <returns></returns>
        public DataSet BC_GetIDBI_SFTP_Upload_Repay(string pBrCode, DateTime pFrmDt)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "BC_GetIDBI_Repay_Upload";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrmDt", pFrmDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBrCode.Length + 1, "@pBrCode", pBrCode);
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
        /// <returns></returns>
        public DataSet BC_GetIDBI_SFTP_Upload(string pBrCode, DateTime pFrmDt, string pFileType)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                if (pFileType == "CUST")
                    oCmd.CommandText = "BC_GetIDBI_SFTP_Upload";
                else if (pFileType == "JLG")
                    oCmd.CommandText = "BC_GetIDBI_JLGTAG_Upload";
                else if (pFileType == "JCONF")
                    oCmd.CommandText = "BC_GetIDBI_Conf_Disb_Upload";


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrmDt", pFrmDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBrCode.Length + 1, "@pBrCode", pBrCode);
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
        /// <param name="pXmlData"></param>
        /// <param name="vBranch"></param>
        /// <param name="pCreatedBy"></param>
        /// <returns></returns>
        public Int32 BC_UPDATE_CUST_Reject_Table_After_upload(string pXML, DateTime pFrmDt)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "BC_UPDATE_CUST_Reject_Table_After_upload";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXML.Length + 1, "@pXML", pXML);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrmDt", pFrmDt);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                return vErr;
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
        /// <param name="pXmlData"></param>
        /// <param name="vBranch"></param>
        /// <param name="pCreatedBy"></param>
        /// <returns></returns>
        public Int32 BC_Save_IDBI_Suc_AfterRead(string pXmlData, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                if (pMode == "CUST")
                {
                    oCmd.CommandText = "BC_Save_IDBI_Cust_Suc_AfterRead";
                }
                if (pMode == "JLG")
                {
                    oCmd.CommandText = "BC_Save_IDBI_JLG_Suc_AfterRead";
                }
                if (pMode == "JCONF")
                {
                    oCmd.CommandText = "BC_Save_IDBI_JCONF_Suc_AfterRead";
                }
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlData.Length + 1, "@pXmlData", pXmlData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                return vErr;
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
        /// <param name="pXmlData"></param>
        /// <param name="vBranch"></param>
        /// <param name="pCreatedBy"></param>
        /// <returns></returns>
        public Int32 BC_Save_IDBI_REJ_AfterRead(string pXmlData, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                if (pMode == "CUST")
                {
                    oCmd.CommandText = "BC_Save_IDBI_Cust_REJ_AfterRead";
                }
                if (pMode == "JLG")
                {
                    oCmd.CommandText = "BC_Save_IDBI_JLG_REJ_AfterRead";
                }
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlData.Length + 1, "@pXmlData", pXmlData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                return vErr;
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
