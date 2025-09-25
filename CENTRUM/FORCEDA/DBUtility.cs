using System;
using System.Data;
using System.Data.SqlClient;  
using System.Configuration;
using System.Web;

namespace FORCEDA
{
    /// <summary>
    /// 
    /// </summary>
    public class DBUtility
    {
        private static string vSrvName = ConfigurationManager.AppSettings["SrvName"];
        private static string vDBName = ConfigurationManager.AppSettings["DBName"];
        private static string vPw = ConfigurationManager.AppSettings["PassPW"];
        private static string vDbUser = ConfigurationManager.AppSettings["DbUser"];
        private static string vPassMaskPW = ConfigurationManager.AppSettings["PassMaskPW"];
        private static string vDbMaskUser = ConfigurationManager.AppSettings["DbMaskUser"];

        private DBUtility() 
        { }

        /// <summary>
        /// Get Connection String
        /// </summary>
        /// <returns></returns>
        public static string GetConStr()
        {
            string vPIIMaskingEnable = "N";
            try
            {
                vPIIMaskingEnable = HttpContext.Current.Session["PIIMaskingEnable"].ToString();
            }
            catch
            {
                vPIIMaskingEnable = "N";
            }
            if (vPIIMaskingEnable == "Y")
            {
                vDbUser = vDbMaskUser;
                vPw = vPassMaskPW;
            }
            string ConStr;
            ConStr = "Data Source=" + vSrvName + "; User Id="+ vDbUser +"; Password=" + vPw + "; Database=" + vDBName + "; Connect Timeout=360000; pooling='true'; Max Pool Size=200;";                        
            return ConStr;
        }

        /// <summary>
        /// Get Open Connection
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetConnection()
        {
            try
            {
                string conStr = GetConStr();
                SqlConnection connection = new SqlConnection(conStr);
                if (connection != null)
                    connection.Open();
                return connection;
            }
            catch  (Exception ex)
            {
                throw ex;
            }
        }           
        

        /// <summary>
        /// 
        /// </summary>
        public static void CloseConnection()
        {           
            string conStr = GetConStr();
            SqlConnection oCon = new SqlConnection(conStr);
            if (oCon.State == ConnectionState.Open)
                oCon.Close();           
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="p_connection"></param>
        /// <returns></returns>
        public static SqlTransaction GetTransaction(SqlConnection pConn)
        {
            return pConn.BeginTransaction();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_Prms"></param>
        /// <param name="p_Direction"></param>
        /// <param name="p_DbType"></param>
        /// <param name="p_Size"></param>
        /// <param name="p_Name"></param>
        /// <param name="p_Value"></param>
        public static void AddParameter(SqlParameterCollection pPrms, ParameterDirection pDirection, SqlDbType pDbType, int pSize, string pName, object pValue)
        {
            SqlParameter tmpParam = new SqlParameter(pName, pDbType, pSize);
            tmpParam.Direction = pDirection;
            tmpParam.Value = pValue;
            pPrms.Add(tmpParam);
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_Command"></param>
        public static void Execute(SqlCommand pCommand)
        {
            pCommand.Connection = GetConnection();
            pCommand.CommandTimeout = 3600;
            try
            {
                pCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();
            }
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_SqlString"></param>
        public static void ExecuteSql(string pSqlString)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = pSqlString;
                oCmd.Connection = GetConnection();
                oCmd.CommandTimeout = 3600;
                oCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Connection.Close();
            }
        }
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_Command"></param>
        /// <param name="p_Conn"></param>
        /// <param name="p_Transaction"></param>
        public static void Execute(SqlCommand pCommand, SqlConnection pConn, SqlTransaction pTransaction)
        {
            pCommand.Connection = pConn;
            pCommand.Transaction = pTransaction;
            try
            {
                pCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }           
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_Command"></param>
        public static void ExecuteScaler(SqlCommand pCommand)
        {
            pCommand.Connection = GetConnection();
            pCommand.CommandTimeout = 3600;
            try
            {
                pCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();
            }
        }
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_Command"></param>
        /// <returns></returns>
        public static double ExecDblScaler(SqlCommand pCommand)
        {
            double dRst = 0;
            pCommand.Connection = GetConnection();
            pCommand.CommandTimeout = 3600;
            try
            {
                dRst = Convert.ToDouble(pCommand.ExecuteScalar());
                return dRst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();
            }
        }
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_Command"></param>
        /// <returns></returns>
        public static Int32 ExecIntScaler(SqlCommand pCommand)
        {
            Int32 dRst = 0;
            pCommand.Connection = GetConnection();
            pCommand.CommandTimeout = 3600;
            try
            {
                dRst = Convert.ToInt32(pCommand.ExecuteScalar());
                return dRst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSqlString"></param>
        public static string EexcStrScaler(string pSqlString)
        {
            SqlCommand oCmd = new SqlCommand();
            string vResult = "";
            try
            {
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = pSqlString;                 
                oCmd.Connection = GetConnection();
                oCmd.CommandTimeout = 3600;
                if (oCmd.ExecuteScalar() == DBNull.Value)
                    vResult = "";
                else
                    vResult =(string)oCmd.ExecuteScalar();                
                return vResult; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Connection.Close();   
            }        
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSqlString"></param>
        /// <returns></returns>
        public static double EexcDblScaler(string pSqlString)
        {
            SqlCommand oCmd = new SqlCommand();
            double vResult = 0;
            try
            {
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = pSqlString;               
                oCmd.Connection = GetConnection();
                oCmd.CommandTimeout = 3600;
                vResult = Convert.ToDouble(oCmd.ExecuteScalar());
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Connection.Close();
            }
        }
        

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_command"></param>
        /// <param name="p_datatable"></param>
        public static void ExecuteForSelect(SqlCommand pCommand, DataTable pDataTable)
        {
            pCommand.Connection = GetConnection();
            pCommand.CommandTimeout = 3600;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = pCommand;
            try
            {
                da.Fill(pDataTable);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();
            }
        }        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_Command"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(SqlCommand pCommand)
        {
            DataTable dt = new DataTable();
            pCommand.Connection = GetConnection();
            pCommand.CommandTimeout = 3600;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = pCommand;
            try
            {
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pCommand"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(SqlCommand pCommand)
        {
            DataSet ds = new DataSet();
            try
            {
                pCommand.Connection = GetConnection();
                pCommand.CommandTimeout = 36000;
                SqlDataAdapter da = new SqlDataAdapter(pCommand);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();   
            }
        }
    }
}