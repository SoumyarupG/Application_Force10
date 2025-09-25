using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CENTRUMBA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web.Hosting;
using System.Configuration;
using System.IO;
using System.Net;

namespace CENTRUM_SARALVYAPAR
{
    public partial class DigiDocReSign : System.Web.UI.Page
    {
        string vpathDigiDocImage = "";
        string vpathCreateNewDoc = "";
        string IniPathHDrive = ConfigurationManager.AppSettings["IniPathHDrive"];
        string InitialApproachURL = ConfigurationManager.AppSettings["InitialApproachURL"];
        string DigiDocOTPURL = ConfigurationManager.AppSettings["DigiDocOTPURL"];

        string DigiDocOtpBucket = ConfigurationManager.AppSettings["DigiDocOtpBucket"];
        string DigiDocBucket = ConfigurationManager.AppSettings["DigiDocBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CDigiDoc oUsr = new CDigiDoc();
                DataTable dt = new DataTable();
                dt = oUsr.GetDigiDocReSign();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dR in dt.Rows)
                    {
                        GetReportDocForDigitalSign(dR["LoanAppNo"].ToString(), Convert.ToInt64(dR["DigiDocDtlsId"]));
                    }
                }
            }
        }

        #region Craete Report Doc

        private string GetReportDocForDigitalSign(string pLoanAppNo, Int64 vDigiDocDtlsId)
        {
            DataSet ds = null;
            DataSet dsDigiDoc = null;
            DataTable dtAppFrm1 = null, dtAppFrm2 = null, dtSancLetter = null, dtEMISchedule = null;
            DataTable dtLoanAgr = null, dtAuthLetter = null, dtKotak = null, dtDigiDocDtls = null;
            DataTable dtScheduleOfCharges = null, dtSchedule = null, dtScheduleOwn = null;

            CReports oRpt = new CReports();
            string vRptPath = "", vFileName = "No File Created";
            CDigiDoc oUsr = null;
            CMember oMem = null;
            CApiCalling oAC = new CApiCalling();

            ReportDocument rptDoc = new ReportDocument();
            try
            {
                oUsr = new CDigiDoc();
                oMem = new CMember();
                vRptPath = string.Empty;
                //vRptPath = HostingEnvironment.MapPath(string.Format("~/Reports/DigitalDocs.rpt"));
                vRptPath = HostingEnvironment.MapPath(string.Format("~/Reports/DigitalDoc_Hindi.rpt"));
                //vRptPath = HostingEnvironment.MapPath(string.Format("~/Reports/DigiDocHindi.rpt"));
                DataTable dt = new DataTable();

                ds = new DataSet();
                ds = oRpt.GetDigitalDocs(pLoanAppNo, vDigiDocDtlsId, 1);
                dtAppFrm1 = ds.Tables[0];
                dtAppFrm2 = ds.Tables[1];
                dtSancLetter = ds.Tables[2];
                dtEMISchedule = ds.Tables[3];
                dtLoanAgr = ds.Tables[4];
                dtAuthLetter = ds.Tables[5];
                dtKotak = ds.Tables[6];

                //dtAppFrm1.Rows[0]["SelfiePath"] = GetImageDrive(Convert.ToString(dtAppFrm1.Rows[0]["SelfiePath"]));
                //dtAppFrm1.Rows[0]["ApplIdProofFront"] = GetImageDrive(Convert.ToString(dtAppFrm1.Rows[0]["ApplIdProofFront"]));
                //dtAppFrm1.Rows[0]["ApplIdProofBack"] = GetImageDrive(Convert.ToString(dtAppFrm1.Rows[0]["ApplIdProofBack"]));
                //dtAppFrm1.Rows[0]["ApplAddrProofFront"] = GetImageDrive(Convert.ToString(dtAppFrm1.Rows[0]["ApplAddrProofFront"]));
                //dtAppFrm1.Rows[0]["ApplAddrProofBack"] = GetImageDrive(Convert.ToString(dtAppFrm1.Rows[0]["ApplAddrProofBack"]));
                //dtAppFrm1.Rows[0]["CoApplIdProofFront"] = GetImageDrive(Convert.ToString(dtAppFrm1.Rows[0]["CoApplIdProofFront"]));
                //dtAppFrm1.Rows[0]["CoApplIdProofBack"] = GetImageDrive(Convert.ToString(dtAppFrm1.Rows[0]["CoApplIdProofBack"]));
                //dtAppFrm1.AcceptChanges();
                string vEnqId = dtAppFrm1.Rows[0]["EnquiryId"].ToString();
                dtAppFrm1.Rows[0]["MemImg"] = GetByteImage("MemberPhoto.png", vEnqId, "I");
                dtAppFrm1.Rows[0]["MemId1FImg"] = GetByteImage("IDProofImage.png", vEnqId, "I");
                dtAppFrm1.Rows[0]["MemId1BImg"] = GetByteImage("IDProofImageBack.png", vEnqId, "I");
                dtAppFrm1.Rows[0]["MemAdd1FImg"] = GetByteImage("AddressProofImage.png", vEnqId, "I");
                dtAppFrm1.Rows[0]["MemAdd1BImg"] = GetByteImage("AddressProofImageBack.png", vEnqId, "I");
                dtAppFrm1.Rows[0]["CoMemId1FImg"] = GetByteImage("AddressProofImage2.png", vEnqId, "I");
                dtAppFrm1.Rows[0]["CoMemId1BImg"] = GetByteImage("AddressProofImage2Back.png", vEnqId, "I");
                dtAppFrm1.Rows[0]["SignPageYN"] = "Y";
                dtAppFrm1.AcceptChanges();

                oUsr = new CDigiDoc();
                dsDigiDoc = oUsr.getDigiDocDtlsByDocId(vDigiDocDtlsId, "", "Y");
                dtDigiDocDtls = dsDigiDoc.Tables[0];

                string vLoanAppId = "", vCustId = "";
                if (dtAppFrm1.Rows.Count > 0)
                {
                    vLoanAppId = dtAppFrm1.Rows[0]["LoanAppId"].ToString();
                    vCustId = dtAppFrm1.Rows[0]["CustId"].ToString();
                }
                dtDigiDocDtls.Rows[0]["ImgPhoto"] = GetByteImage(vLoanAppId + ".jpg", vLoanAppId, "O");
                dtDigiDocDtls.AcceptChanges();
                oMem = new CMember();
                dtScheduleOfCharges = oMem.GetScheduleofChargesByLnAppId(pLoanAppNo, vDigiDocDtlsId);
                oMem = new CMember();
                dtSchedule = oMem.GetSanctionLetterDtlByLnAppId(pLoanAppNo, vDigiDocDtlsId);

                // Added By Priti
                string vBrCode = dtAppFrm1.Rows[0]["BranchCode"].ToString();
                oRpt = new CReports();
                dtScheduleOwn = oRpt.rptRepaySchedule(pLoanAppNo, vBrCode, "N", 0);

                if (dtAppFrm1.Rows.Count > 0)
                {
                    if (vRptPath != string.Empty)
                    {
                        vpathCreateNewDoc = ConfigurationManager.AppSettings["pathCreateNewDoc"];
                        vFileName = vpathCreateNewDoc + pLoanAppNo + ".pdf";
                        using (rptDoc)
                        {
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(dtAppFrm1);

                            // Commented by Priti
                            //rptDoc.Subreports["DigitalDoc_ExistingLoanDtl.rpt"].SetDataSource(dtAppFrm2);
                            //rptDoc.Subreports["DigitalDoc_SancLetter.rpt"].SetDataSource(dtSancLetter);
                            //rptDoc.Subreports["DigitalDoc_EMISchedule.rpt"].SetDataSource(dtEMISchedule);
                            //rptDoc.Subreports["DigitalDoc_LoanAgreement.rpt"].SetDataSource(dtLoanAgr);
                            //rptDoc.Subreports["DigitalDoc_AuthLetter.rpt"].SetDataSource(dtAuthLetter);
                            //rptDoc.Subreports["DigitalDoc_Kotak.rpt"].SetDataSource(dtKotak);

                            rptDoc.Subreports["DigitalDoc_ExistingLoanDtl.rpt"].SetDataSource(dtAppFrm2);
                            rptDoc.Subreports["DigitalDoc_LoanAgreement.rpt"].SetDataSource(dtLoanAgr);
                            rptDoc.Subreports["DigitalDoc_SancLetter.rpt"].SetDataSource(dtSancLetter);
                            rptDoc.Subreports["DigitalDoc_EMISchedule.rpt"].SetDataSource(dtEMISchedule);
                           
                            // Commented by Priti
                          //  rptDoc.Subreports["DigitalDoc_AuthLetter.rpt"].SetDataSource(dtAuthLetter);
                         //   rptDoc.Subreports["DigitalDoc_Kotak.rpt"].SetDataSource(dtKotak);

                            if (dtDigiDocDtls.Rows.Count > 0)
                            {
                                rptDoc.Subreports["DigitalDoc_eSigned.rpt"].SetDataSource(dtDigiDocDtls);
                            }
                            if (dtScheduleOwn.Rows.Count > 0) // Added by Priti
                            {
                                rptDoc.Subreports["RepaySche_Hindi.rpt"].SetDataSource(dtScheduleOwn);
                                rptDoc.Subreports["RepaymentSche.rpt"].SetDataSource(dtScheduleOwn);
                            }
                          //  rptDoc.Subreports["SanctionLetterOwn.rpt"].SetDataSource(dtSchedule);
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
                            //rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, vFileName);M
                            if (MinioYN == "Y")

                            {
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

        private string GetImageDrive(string vPath)
        {
            string vPth = "";
            vPth = vPath.Replace(@"/", "\\");
            if (!File.Exists(vPth))
            {
                vPth = vPath.Trim().Remove(0, 56);
                vPath = IniPathHDrive + vPth.Replace(@"/", "\\");
            }
            return vPath;
        }

        public static byte[] strmToByte(Stream vStream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                vStream.CopyTo(ms);
                return ms.ToArray();
            }
        }

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

        public byte[] GetByteImage(string pImageName, string pId, string Module = "I")
        {
            string ActNetImage = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = Module == "I" ? InitialApproachURL.Split(',') : DigiDocOTPURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + (Module == "I" ? pId + "_" + pImageName : pImageName);
                    if (URLExist(ActNetImage))
                    {
                        imgByte = DownloadByteImage(ActNetImage);
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
    }
}