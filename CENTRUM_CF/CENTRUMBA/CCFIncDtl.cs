using System;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
    public class CCFIncDtl
    {
        public Int32 CF_SaveIncomeDetail(Int64 pLeadId, Int32 pAssMethodID, double pProfAfterTax, double pDepreciation, double pAmortization, double pInterest,
    double pTaxes, double pTotalIncAnnual, double pTurnover, double pGrossProfitMargin, double pGrossIncAnnual, double pNoMonthsConsiBS, double pBankTurnMonthBS,
            double pProfMargMonthBS, double pGrossIncMonthly, double pOtherInc, double pAverElecBill, double pTotalIncMonthly, double pApplFoirPerc, double pDeviFoirPerc,
            double pFinalFoirConsi, double pFoirInc, double pExistOblig, double pNetIncToUfsbEmi, double pPerLakhEmi, double pLoanEligiblity,
            string pBranchCode, Int32 pCreatedBy, string pMode, ref string vErrMessage)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_SaveIncomeDetail";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pLeadID", pLeadId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAssMethodID", pAssMethodID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pProfAfterTax", pProfAfterTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pDepreciation", pDepreciation);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAmortization", pAmortization);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInterest", pInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTaxes", pTaxes);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotalIncAnnual", pTotalIncAnnual);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTurnover", pTurnover);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pGrossProfitMargin", pGrossProfitMargin);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pGrossIncAnnual", pGrossIncAnnual);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pNoMonthsConsiBS", pNoMonthsConsiBS);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pBankTurnMonthBS", pBankTurnMonthBS);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pProfMargMonthBS", pProfMargMonthBS);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pGrossIncMonthly", pGrossIncMonthly);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pOtherInc", pOtherInc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAverElecBill", pAverElecBill);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotalIncMonthly", pTotalIncMonthly);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pApplFoirPerc", pApplFoirPerc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pDeviFoirPerc", pDeviFoirPerc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFinalFoirConsi", pFinalFoirConsi);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFoirInc", pFoirInc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pExistOblig", pExistOblig);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pNetIncToUfsbEmi", pNetIncToUfsbEmi);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPerLakhEmi", pPerLakhEmi);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanEligiblity", pLoanEligiblity);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrMsg", "");
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                vErrMessage = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
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

        public DataSet CF_GetIncomeDtlByLeadID(Int64 pLeadID)
        {
            SqlCommand oCmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_GetIncomeDtlByLeadID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pLeadID", pLeadID);
                //DBUtility.ExecuteForSelect(oCmd, dt);
                //return dt;
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

        public DataSet CF_CalculateEMIAmount(Int64 pLeadID)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CF_CalculateEMIAmount";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pLeadID", pLeadID);
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

    }
}
