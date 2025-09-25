using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using FORCEDA;

namespace FORCECA
{
    public class CGblIdGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pCondition"></param>
        /// <param name="pCodeAdd"></param>
        /// <param name="pCodeName"></param>
        /// <param name="pIDName"></param>
        /// <param name="pName"></param>
        /// <param name="pTbl"></param>
        /// <param name="pConID"></param>
        /// <param name="pConIDName"></param>
        /// <param name="pConIDDate"></param>
        /// <param name="pConDate"></param>
        /// <param name="pBranchCode"></param>
        /// <returns></returns>
        public DataTable PopComboMIS(string pCondition, string pCodeAdd, string pCodeName, string pIDName, string pName, string pTbl, string pConID,
                                    string pConIDName, string pConIDDate, DateTime pConDate, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopComboMISString";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pCondition", pCondition);// N=Normal, S=one condition, D=Double Condition
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pCodeAdd", pCodeAdd);// Y=Yes, N=No for (Name + Code)
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pCodeName", pCodeName);// DataTextField Name of Code [Default AA]
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIDName", pIDName);// Table Field Name(Id)
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pName", pName);// Table Field Name(Name)
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTbl", pTbl);// Table Name
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pConID", pConID);// Conditional popup upon another field [Default 0]
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pCodeAdd"></param>
        /// <param name="pTbl"></param>
        /// <param name="pCondition"></param>
        /// <param name="pConDate"></param>
        /// <param name="pBranchCode"></param>
        /// <returns></returns>
        public DataTable PopTransferMIS(string pCodeAdd, string pTbl, string pCondition, DateTime pConDate, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopTransferMIS";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pCodeAdd", pCodeAdd);// Y=Yes, N=No for (Name + Code)
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTbl", pTbl);// Table Name
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pCondition", pCondition);// Conditional popup upon another field [Default 0]
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

        public DataTable PopTransferMIS(string pCodeAdd, string pTbl, string pCondition, DateTime pConDate, string pBranchCode, string pCallSource)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopTransferMIS";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pCodeAdd", pCodeAdd);// Y=Yes, N=No for (Name + Code)
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTbl", pTbl);// Table Name
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pCondition", pCondition);// Conditional popup upon another field [Default 0]
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pConDate", pConDate);// Date (Parameter) [Default 01/01/1900]
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranchCode", pBranchCode); // Branch Code [Default 000]
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pCallSource", pCallSource);
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

        public DataTable PopGroupBranchWise(string pCodeAdd, string pTbl, string PopGroupBranchWise, DateTime pConDate, string pBranchCode, string pCenterID)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopGroupBranchWise";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pCodeAdd", pCodeAdd);// Y=Yes, N=No for (Name + Code)
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTbl", pTbl);// Table Name
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pCondition", PopGroupBranchWise);// Conditional popup upon another field [Default 0]
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pConDate", pConDate);// Date (Parameter) [Default 01/01/1900]
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranchCode", pBranchCode); // Branch Code [Default 000]
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pCondition2", pCenterID);// Table Name
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
        /// <param name="pCondition"></param>
        /// <param name="pCodeAdd"></param>
        /// <param name="pCodeName"></param>
        /// <param name="pIDName"></param>
        /// <param name="pName"></param>
        /// <param name="pTbl"></param>
        /// <param name="pConID"></param>
        /// <param name="pConIDName"></param>
        /// <param name="pConIDDate"></param>
        /// <param name="pConDate"></param>
        /// <param name="pBranchCode"></param>
        /// <returns></returns>
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
        public DataTable PopExRn()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopExRn";
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
        /// <param name="pID"></param>
        /// <param name="pField"></param>
        /// <param name="pTbl"></param>
        /// <returns></returns>
        public Int32 ChkSystem(Int32 pID, string pIDFieldName, string pFieldName, string pTbl) //Checking the particular field is syetem define or not
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vRec = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkSystem";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pID", pID);// ID Value
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pIDFieldName", pIDFieldName);// ID Field Name
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pFieldName", pFieldName);// Field Name(System)
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pTbl", pTbl);// Table Name
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@vRec", vRec);// ID Value
                DBUtility.Execute(oCmd);
                vRec = Convert.ToInt32(oCmd.Parameters["@vRec"].Value);
                if (vRec == 0)
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
        /// <param name="pFieldDate"></param>
        /// <param name="pTbl"></param>
        /// <param name="pFieldID"></param>
        /// <param name="pID"></param>
        /// <param name="pBranchCode"></param>
        /// <returns></returns>
        //public DataTable ChkDate(string pFieldDate, string pTbl, string pFieldID, string pID, string pBranchCode)
        //{
        //    DataTable dt = new DataTable();
        //    SqlCommand oCmd = new SqlCommand();
        //    try
        //    {
        //        oCmd.CommandType = CommandType.StoredProcedure;
        //        oCmd.CommandText = "ChkDate";
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pFieldDate", pFieldDate);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTbl", pTbl);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pFieldID", pFieldID);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pID", pID);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pBranchCode", pBranchCode);
        //        DBUtility.ExecuteForSelect(oCmd, dt);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        oCmd.Dispose();
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pID"></param>
        /// <param name="pField"></param>
        /// <param name="pTbl"></param>
        /// <returns></returns>
        public Int32 ChkDelete(Int32 pID, string pField, string pTbl)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vRec = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkDelete";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pID", pID);// ID Value
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pField", pField);// Field Name
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pTbl", pTbl);// Table Name
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@vRec", vRec);// ID Value
                DBUtility.Execute(oCmd);
                vRec = Convert.ToInt32(oCmd.Parameters["@vRec"].Value);
                if (vRec == 0)
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
        /// <param name="pID"></param>
        /// <param name="pField"></param>
        /// <param name="pTbl"></param>
        /// <returns></returns>
        public Int32 ChkDeleteString(String pID, string pField, string pTbl)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vRec = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkDeleteString";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pID", pID);// ID Value
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pField", pField);// Field Name
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pTbl", pTbl);// Table Name
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@vRec", vRec);// ID Value
                DBUtility.Execute(oCmd);
                vRec = Convert.ToInt32(oCmd.Parameters["@vRec"].Value);
                if (vRec == 0)
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

        public Int32 ChkgrpActvForEO(String pEoId, DateTime pEfftiveDt)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vRec = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkgrpActvForEO";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pEoId", pEoId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pEfftiveDt", pEfftiveDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@vRec", vRec);

                DBUtility.Execute(oCmd);
                vRec = Convert.ToInt32(oCmd.Parameters["@vRec"].Value);
                if (vRec == 0)
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

        public Int32 ChkSaralMemActvForEO(String pEoId, DateTime pEfftiveDt)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vRec = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkSaralMemActvForEO";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pEoId", pEoId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pEfftiveDt", pEfftiveDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@vRec", vRec);

                DBUtility.Execute(oCmd);
                vRec = Convert.ToInt32(oCmd.Parameters["@vRec"].Value);
                if (vRec == 0)
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

        public Int32 ChkMelMemActvForEO(String pEoId, DateTime pEfftiveDt)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vRec = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkMelMemActvForEO";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pEoId", pEoId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pEfftiveDt", pEfftiveDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@vRec", vRec);

                DBUtility.Execute(oCmd);
                vRec = Convert.ToInt32(oCmd.Parameters["@vRec"].Value);
                if (vRec == 0)
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
        /// <param name="pMemberID"></param>
        /// <param name="pLoanTypeId"></param>
        /// <param name="pBrCode"></param>
        /// <returns></returns>
        public string ChkforOpenApplication(string pMemberID)
        {
            string vMsg = "";
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkforOpenApplication";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pMemberID", pMemberID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@vMsg", vMsg);
                DBUtility.Execute(oCmd);
                vMsg = Convert.ToString(oCmd.Parameters["@vMsg"].Value);

                return vMsg;
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
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pBranch"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbName"></param>
        /// <param name="pdbpath"></param>
        /// <param name="pBrCode"></param>
        /// <param name="pPDate"></param>
        /// <returns></returns>
        public static Int32 ProcDbBackup(string pDbName, string pdbpath, string pBrCode, string pPDate)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ProcDbBackup";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDbName", pDbName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pdbpath", pdbpath);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPDate", pPDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.BigInt, 1, "@pErr", 0);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pID"></param>
        /// <param name="pBrCode"></param>
        /// <returns></returns>
        public Int32 ChkDuplicate(string pTable, string pField1, string pValue1, string pField2, string pValue2, string pPKField, string pPKId, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 pRet = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkDuplicate";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTable", pTable);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pField1", pField1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pValue1", pValue1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pField2", pField2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pValue2", pValue2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pPKField", pPKField);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pPKId", pPKId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", pMode);
                pRet = DBUtility.ExecIntScaler(oCmd);
                return pRet;
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
        /// <param name="pTable"></param>
        /// <param name="pField1"></param>
        /// <param name="pValue1"></param>
        /// <param name="pField2"></param>
        /// <param name="pValue2"></param>
        /// <param name="pPKField"></param>
        /// <param name="pPKId"></param>
        /// <param name="pMode"></param>
        /// <param name="pYear"></param>
        /// <returns></returns>
        public Int32 ChkDuplicate(string pTable, string pField1, string pValue1, string pField2, string pValue2, string pPKField, string pPKId, string pMode, string pField3, string pValue3)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 pRet = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkDuplicateYear";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTable", pTable);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pField1", pField1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pValue1", pValue1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pField2", pField2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pValue2", pValue2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pPKField", pPKField);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPKId", pPKId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pField3", pField3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pValue3", pValue3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", pMode);
                pRet = DBUtility.ExecIntScaler(oCmd);
                return pRet;
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

        public Int32 ChkDuplicateHoli(string pTable, string pField1, string pValue1, string pField2, string pValue2, string pPKField, string pPKId, string pMode, string pField3, string pValue3, string pField4, Int32 pValue4)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 pRet = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkDuplicateHoli";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTable", pTable);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pField1", pField1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pValue1", pValue1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pField2", pField2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pValue2", pValue2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pPKField", pPKField);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPKId", pPKId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pField3", pField3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pValue3", pValue3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pField4", pField4);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pValue4", pValue4);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", pMode);
                pRet = DBUtility.ExecIntScaler(oCmd);
                return pRet;
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

        public static string ChkLoanParameterByLoanTypId(string pFieldNm, Int32 pLoanTypId, string pBranch)
        {
            string vResult = "";
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkLoanParameterByLoanTypId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pFieldNm", pFieldNm);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypId", pLoanTypId);
                DBUtility.ExecuteForSelect(oCmd, dt);
                if (dt.Rows.Count > 0)
                    vResult = Convert.ToString(dt.Rows[0][0]);
                else
                    vResult = "";
                return vResult;
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
        public static string GetImprestAc(string pBranchCode, string pSearch)
        {
            string vResult = "";
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetImprestAcByBranch";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 50, "@pImAc", vResult);
                DBUtility.ExecuteForSelect(oCmd, dt);
                vResult = Convert.ToString(oCmd.Parameters["@pImAc"].Value);

                return vResult;
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
        public Int32 ChkExRn(ref Int32 pExitReasonId)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkExRn";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pExitReasonId", pExitReasonId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);

                //return pExitReasonId = Convert.ToInt32(oCmd.Parameters["@pExitReasonId"].Value);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pExitReasonId = Convert.ToInt32(oCmd.Parameters["@pExitReasonId"].Value);
                if (vErr > 0)
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
        public Int32 GetLoanNO(string vMarketId)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 pRet = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanNo";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMarketId", vMarketId);
                pRet = DBUtility.ExecIntScaler(oCmd);
                return pRet;
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
        public string getMobileUser(string pMobile)
        {
            string vMemberName = "";
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "getMobileUser";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pMobile", pMobile);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 150, "@pMemberName", vMemberName);
                DBUtility.Execute(oCmd);
                vMemberName = Convert.ToString(oCmd.Parameters["@pMemberName"].Value);
                return vMemberName;
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

        public string getMobileUserAllProject(string pMobile)
        {
            string vMemberName = "";
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "getMobileUserAllProject";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pMobile", pMobile);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 150, "@pMemberName", vMemberName);
                DBUtility.Execute(oCmd);
                vMemberName = Convert.ToString(oCmd.Parameters["@pMemberName"].Value);
                return vMemberName;
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


        public string getAccountUser(string pAccNo)
        {
            string vMemberName = "";
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "getAccountUser";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pAccNo", pAccNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 150, "@pMemberName", vMemberName);
                DBUtility.Execute(oCmd);
                vMemberName = Convert.ToString(oCmd.Parameters["@pMemberName"].Value);
                return vMemberName;
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

        public DataTable GetEquiFaxSrchForAppr(DateTime pDate, string pBranchCode, string pEoId, string pMarketId, string pGroupId, string pMemberName, DateTime pDateFrm, DateTime pDateTo)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetEquiFaxSrchForAppr";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", pDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBrCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoId", pEoId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMarketId", pMarketId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pGroupId", pGroupId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDateFrm", pDateFrm);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDateTo", pDateTo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pMemberName", pMemberName);
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

        public DataTable GetBatchNoByTransDate(DateTime pTransDate)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBatchNoByTransDate";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pTransDate", pTransDate);
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

        public DataTable GetAllStateList()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetStateList";
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

        public DataTable popHospiCash(DateTime pDate, string pBranchCode, Int32 pHospiId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "popHospiCash";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 12, "@pDate", pDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pHospiId", pHospiId);
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

        public DataTable popDeviationRemarks()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "popDeviationRemarks";
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

        public Int32 ChkDupMobNoForEO(string pMobileNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 pRet = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkDupMobNoForEO";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMobileNo", pMobileNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@vRec", 0);
                DBUtility.ExecIntScaler(oCmd);
                pRet = Convert.ToInt32(oCmd.Parameters["@vRec"].Value);
                return pRet;
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
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopState";
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

        public DataTable GetSpclAble()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSpclAble";
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
