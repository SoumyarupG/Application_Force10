using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using System.Configuration;
using System.Data;
using CENTRUMBA;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.Hosting;
using CrystalDecisions.Shared;
using System.IO;
using System.Net;

namespace CENTRUMCF
{
    public partial class DigiDocReSign : CENTRUMBAse
    {
        string vpathDigiDocImage = "";
        string vpathCreateNewDoc = "";
        string DigiDocBucket = ConfigurationManager.AppSettings["DigiDocBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];

        string DocumentURL = ConfigurationManager.AppSettings["DocumentURL"];
        string DigiDocOTPURL = ConfigurationManager.AppSettings["DigiDocOTPURL"];

        protected void Page_Load(object sender, EventArgs e)
        {
            CDigiDoc oUsr = new CDigiDoc();
            DataTable dt = new DataTable();
            dt = oUsr.GetDigiDocReSign();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dR in dt.Rows)
                {
                    GetReportDocForDigitalSign(dR["LoanAppNo"].ToString(), dR["ApplicantName"].ToString(), Convert.ToInt64(dR["DigiDocDtlsId"]));
                }
            }
        }

        #region Craete Report Doc

        private string GetReportDocForDigitalSign(string pLoanAppNo, string pMemberName, Int64 vDigiDocDtlsId)
        {

            DataSet ds = null;
            DataSet dsDigiDoc = null;
            DataTable dtAppFrm1 = null, dtAppFrm2 = null, dtSancLetter = null, dtEMISchedule = null;
            DataTable dtLoanAgr = null, dtAuthLetter = null, dtKotak = null, dtDigiDocDtls = null;

            DataTable dt1 = null, dt2 = null, dt3 = null, dt4 = null, dt5 = null, dt6 = null, dt7 = null
            , dt8 = null, dt9 = null, dt10 = null, dt11 = null, dt12 = null, dt13 = null, dt14 = null, dt15 = null,
            dt16 = null, dt17 = null, dt18 = null, dtScheduleOfCharges = null, dtSchedule = null;


            CReports oRpt = new CReports();
            string vRptPath = "", vFileName = "No File Created";
            CDigiDoc oUsr = null;
            CMember oMem = null;

            ReportDocument rptDoc = new ReportDocument();
            try
            {
                oUsr = new CDigiDoc();
                oMem = new CMember();
                string vSanctionId = oUsr.GetSanctionId(pLoanAppNo);

                string vSancId = vSanctionId;
                vRptPath = string.Empty;
                vRptPath = HostingEnvironment.MapPath(string.Format("~/Reports/DigitalDocs.rpt"));
                DataTable dt = new DataTable();

                ds = new DataSet();
                ds = oMem.GetAgrDigitalDocs(vSancId, vDigiDocDtlsId);
                dtAppFrm1 = ds.Tables[0];
                dtAppFrm2 = ds.Tables[1];
                dtSancLetter = ds.Tables[2];
                dtEMISchedule = ds.Tables[3];
                dtLoanAgr = ds.Tables[4];
                dtAuthLetter = ds.Tables[5];
                dtKotak = ds.Tables[6];

                dsDigiDoc = oUsr.getDigiDocDtlsByDocId(vDigiDocDtlsId, "", "Y");
                dtDigiDocDtls = dsDigiDoc.Tables[0];

                string vLoanAppId = "", vCustId = "";
                string vBrCode = dtAppFrm1.Rows[0]["BrCode"].ToString();
                if (dtAppFrm1.Rows.Count > 0)
                {
                    vLoanAppId = dtAppFrm1.Rows[0]["LoanAppId"].ToString();
                    vCustId = dtAppFrm1.Rows[0]["CustId"].ToString();
                }
                ds = oRpt.rptCAMReport(vCustId, vLoanAppId);
                oMem = new CMember();
                dtScheduleOfCharges = oMem.GetScheduleofChargesByLnAppId(vSancId, vDigiDocDtlsId);
                oMem = new CMember();
                dtSchedule = oMem.GetSanctionLetterDtlByLnAppId(vSancId, vDigiDocDtlsId);

                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    dt3 = ds.Tables[2];
                    dt4 = ds.Tables[3];
                    dt5 = ds.Tables[4];
                    dt6 = ds.Tables[5];
                    dt7 = ds.Tables[6];
                    dt8 = ds.Tables[7];
                    dt9 = ds.Tables[8];
                    dt10 = ds.Tables[9];
                    dt11 = ds.Tables[10];
                    dt12 = ds.Tables[11];
                    dt13 = ds.Tables[12];
                    dt14 = ds.Tables[13];
                    dt15 = ds.Tables[14];
                    dt16 = ds.Tables[15];
                    dt17 = ds.Tables[16];
                    dt18 = ds.Tables[17];
                }

                if (dtAppFrm1.Rows.Count > 0)
                {
                    #region GetBase64Images
                    dtAppFrm1.Rows[0]["SelfieImg"] = GetByteImage("Applicant photo.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["CoAppSelfieImg"] = GetByteImage("Co-applicant photo.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["KYC1FrontImg"] = GetByteImage("Applicant KYC ID 1 Front.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["KYC1BackImg"] = GetByteImage("Applicant KYC ID 1 Back.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["KYC2FrontImg"] = GetByteImage("Applicant KYC ID 2 Front.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["KYC2BackImg"] = GetByteImage("Applicant KYC ID 2 Back.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["CoAppKYC1FrontImg"] = GetByteImage("Co-applicant KYC ID 1 Front.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["CoAppKYC1BackImg"] = GetByteImage("Co-applicant KYC ID 1 Back.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["CoAppKYC2FrontImg"] = GetByteImage("Co-applicant KYC ID 2 Front.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["CoAppKYC2BackImg"] = GetByteImage("Co-applicant KYC ID 2 Back.png", vLoanAppId, "I");
                    dtAppFrm1.AcceptChanges();

                    dtDigiDocDtls.Rows[0]["PhotoImg"] = GetByteImage(vLoanAppId+".jpg", vLoanAppId, "D");
                    dtDigiDocDtls.AcceptChanges();
                    #endregion

                    if (vRptPath != string.Empty)
                    {
                        vpathCreateNewDoc = ConfigurationManager.AppSettings["pathCreateNewDoc"];

                        //vFileName = vpathCreateNewDoc + pLoanAppNo + '_' + "MEL" + "_" + pMemberName + ".pdf";
                        vFileName = vpathCreateNewDoc + pLoanAppNo + ".pdf";

                        using (rptDoc)
                        {
                            //vRptName = "DigitalDocument";
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(dtAppFrm1);
                            rptDoc.Subreports["DigitalDoc_ExistingLoanDtl.rpt"].SetDataSource(dtAppFrm2);
                            rptDoc.Subreports["DigitalDoc_SancLetter.rpt"].SetDataSource(dtSancLetter);
                            rptDoc.Subreports["DigitalDoc_EMISchedule.rpt"].SetDataSource(dtEMISchedule);
                            rptDoc.Subreports["DigitalDoc_LoanAgreement.rpt"].SetDataSource(dtLoanAgr);
                            rptDoc.Subreports["DigitalDoc_AuthLetter.rpt"].SetDataSource(dtAuthLetter);
                            rptDoc.Subreports["DigitalDoc_Kotak.rpt"].SetDataSource(dtKotak);

                            if (dtDigiDocDtls.Rows.Count > 0)
                            {
                                rptDoc.Subreports["DigitalDoc_eSigned.rpt"].SetDataSource(dtDigiDocDtls);
                            }

                            rptDoc.Subreports["CAM1.rpt"].SetDataSource(dt1);
                            rptDoc.Subreports["CAM2.rpt"].SetDataSource(dt1);
                            rptDoc.Subreports["App.rpt"].SetDataSource(dt1);
                            rptDoc.Subreports["CoApp.rpt"].SetDataSource(dt2);
                            rptDoc.Subreports["AppResidence.rpt"].SetDataSource(dt1);
                            rptDoc.Subreports["rptObligation.rpt"].SetDataSource(dt4);
                            rptDoc.Subreports["rptObligationCoAppl.rpt"].SetDataSource(dt17);
                            rptDoc.Subreports["CreditProfile.rpt"].SetDataSource(dt18);
                            rptDoc.Subreports["BankinDetailAppCoApp.rpt"].SetDataSource(dt8);
                            rptDoc.Subreports["rptFOIRIncomeAssesment.rpt"].SetDataSource(dt10);
                            rptDoc.Subreports["CAM6.rpt"].SetDataSource(dt1);
                            rptDoc.Subreports["DecisionSheet.rpt"].SetDataSource(dt16);
                            rptDoc.Subreports["LnSancApprove.rpt"].SetDataSource(dt3);
                            rptDoc.Subreports["SanctionLetterOwn.rpt"].SetDataSource(dtSchedule);

                            rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                            rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                            rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                            rptDoc.SetParameterValue("pBranch", vBrCode);
                            rptDoc.SetParameterValue("pTitle", "CAM Report");
                            if (dtScheduleOfCharges.Rows.Count > 0)
                            {
                                rptDoc.SetParameterValue("pBorrower", dtScheduleOfCharges.Rows[0]["Borrower"].ToString());
                                rptDoc.SetParameterValue("pCoBorrower", dtScheduleOfCharges.Rows[0]["CoBorrower"].ToString());
                                rptDoc.SetParameterValue("pSanctionDate", dtScheduleOfCharges.Rows[0]["SanctionDate"].ToString());
                            }
                            else
                            {
                                rptDoc.SetParameterValue("pBorrower", "");
                                rptDoc.SetParameterValue("pCoBorrower", "");
                                rptDoc.SetParameterValue("pSanctionDate", "01/01/1900");
                            }
                            if (MinioYN == "Y")
                            {
                                CApiCalling oAC = new CApiCalling();
                                Stream reportStream = rptDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                                oAC.UploadFileMinio(strmToByte(reportStream), pLoanAppNo + ".pdf", pLoanAppNo, DigiDocBucket, MinioUrl);
                            }
                            else
                            {
                                rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, vFileName);
                            }
                        }
                    }
                }
                else
                {
                    vFileName = "No File Created..As data Not found";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                rptDoc.Dispose();
                ds = null;
                dtAppFrm1 = null; dtAppFrm2 = null; dtSancLetter = null;
                dtEMISchedule = null;
                dtLoanAgr = null; dtAuthLetter = null; dtKotak = null;

                oUsr = null; oMem = null;
                dsDigiDoc = null;
            }
            return vFileName;
        }
        #endregion

        #region streamtoByte
        public static byte[] strmToByte(Stream vStream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                vStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
        #endregion

        #region URLToByte
        public byte[] DownloadByteImage(string URL)
        {
            byte[] imgByte = null;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var wc = new WebClient())
                {
                    imgByte = wc.DownloadData(URL);
                }
            }
            finally { }
            return imgByte;
        }
        #endregion

        #region GetByteImage
        public byte[] GetByteImage(string ImageName, string Id, string Module)
        {
            string ActNetImage = "", ActNetImage1 = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = Module == "I" ? DocumentURL.Split(',') : DigiDocOTPURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + (Module == "I" ? Id + "_" + ImageName : ImageName);
                    ActNetImage1 = ActNetPath[j] + (Module == "I" ? Id + "/" + ImageName : ImageName);
                    if (URLExist(ActNetImage))
                    {
                        imgByte = DownloadByteImage(ActNetImage);
                        break;
                    }
                    else if (URLExist(ActNetImage1))
                    {
                        imgByte = DownloadByteImage(ActNetImage1);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return imgByte;
        }
        #endregion

        #region URLExist
        private bool URLExist(string pPath)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pPath);
                request.Timeout = 5000;
                using (WebResponse response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}