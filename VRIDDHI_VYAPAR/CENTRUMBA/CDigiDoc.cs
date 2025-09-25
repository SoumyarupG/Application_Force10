using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
   public class CDigiDoc
    {
       public DataSet getDigiDocDtlsByDocId(Int64 pDigiDocDtlsId, string pOTP, string pAfterSaveRecord)
       {
           DataSet ds = new DataSet();
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "getDigiDocDtlsByDocId";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 10, "@pDigiDocDtlsId", pDigiDocDtlsId);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pOTP", pOTP);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAfterSaveRecord", pAfterSaveRecord);
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

       public DataSet getDigitalDocByToken(string pLoanAppNo, string pToken)
       {
           DataSet ds = new DataSet();
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "getDigitalDocByToken";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pLoanAppNo", pLoanAppNo);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pToken", pToken);
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

       public string DigitalSignStatus(string vReferenceNo)
       {
           string vStatus = "Not Done Succesfully";
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "GetDigitalSignStatus";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pReferenceNo", vReferenceNo);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 50, "@pStatus", vStatus);
               DBUtility.Execute(oCmd);
               vStatus = Convert.ToString(oCmd.Parameters["@pStatus"].Value);
               if (vStatus.ToString().Trim().ToLower() == "not done successfully")
               {
                   vStatus = "N";
               }
               else if (vStatus.ToString().Trim().ToLower() == "done successfully")
               {
                   vStatus = "Y";
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
           finally
           {
               oCmd.Dispose();
           }
           return vStatus;
       }

       public string GetSanctionId(string pLoanAppId)
       {
           SqlCommand oCmd = new SqlCommand();
           Int32 vErr = 0;
           string pSanctionID = "";
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "GetSanctionId";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pSanctionID", "");
               DBUtility.Execute(oCmd);
               pSanctionID = Convert.ToString(oCmd.Parameters["@pSanctionID"].Value);
               return pSanctionID;
           }
           catch (Exception ex)
           {
               return "";
           }
           finally
           {
               oCmd.Dispose();
           }
       }

       public Int32 InActiveDigitalDoc(string pLoanAppNo, string pToken)
       {
           SqlCommand oCmd = new SqlCommand();
           Int32 vErr = 0;
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "InActiveDigitalDoc";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pLoanAppNo", pLoanAppNo);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pToken", pToken);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
               DBUtility.Execute(oCmd);
               vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
               if (vErr == 0)
               {
                   return 1;
               }
               else
               {
                   return 0;
               }
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

       public Int32 SaveDigiDocOTPDetails(string pLoanAppNo, Int64 pDigiDocId, string pOTP, ref Int64 vDigiDocDtlsId)
       {
           SqlCommand oCmd = new SqlCommand();
           Int32 vErr = 0;
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "SaveDigiDocOTPDetails";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pDigiDocId", pDigiDocId);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pLoanAppNo", pLoanAppNo);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pOTP", pOTP);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.BigInt, 4, "@pDigiDocDtlsId", vDigiDocDtlsId);
               DBUtility.Execute(oCmd);
               vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
               vDigiDocDtlsId = Convert.ToInt64(oCmd.Parameters["@pDigiDocDtlsId"].Value);
               if (vErr == 0)
               {
                   return 1;
               }
               else
               {
                   return 0;
               }
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

       public Int32 UpdateDigiDocOTPDetails(Int64 pDigiDocDtlsId, string pApplicantName, string pDevicePlatform, string pBrowser, string pGeoLocation, string pIpAddress, string pMobileNo, string pDeviceDetails)
       {
           SqlCommand oCmd = new SqlCommand();
           Int32 vErr = 0;
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "UpdateDigiDocOTPDetails";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pDigiDocDtlsId", pDigiDocDtlsId);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pApplicantName", pApplicantName);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pDevicePlatform", pDevicePlatform);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pBrowser", pBrowser);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pGeoLocation", pGeoLocation);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pIpAddress", pIpAddress);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pMobileNo", pMobileNo);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pDeviceDetails", pDeviceDetails);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
               DBUtility.Execute(oCmd);
               vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
               if (vErr == 0)
               {
                   return 1;
               }
               else
               {
                   return 0;
               }
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

       public DataTable GetDigiDocReSign()
       {
           DataTable dt = new DataTable();
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "GetDigiDocReSign";              
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
