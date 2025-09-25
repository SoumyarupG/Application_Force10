using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CENTRUMBA;


namespace SendSms
{
    public sealed class MgrSMS
    {
        /// <summary>
        /// SMS Type="D"
        /// </summary>
        public static void DisburseSms(string pLoanId)
        {
            DataTable dt = null;
            CSMS oSms = null;
            AuthSms oAuth = null;
            string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;

            try
            {
                oSms = new CSMS();
                oAuth = new AuthSms();
                dt = oSms.GetList_ToSend_SMS(pLoanId, "D");
            
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["MobNo"].ToString().Length >= 10)
                    {
                        vRtnGuid = oAuth.SendSms(dr["MobNo"].ToString(), dr["Msg"].ToString());
                        System.Threading.Thread.Sleep(500);
                        if (vRtnGuid != "" || vRtnGuid != null)//|| vRtnGuid != "2019" || vRtnGuid != "2020" || vRtnGuid != "2021" || vRtnGuid != "2022" || vRtnGuid != "2023")
                        {
                            oAuth.GetGuidStatus(ref vRtnGuid, ref vStatusCode, ref vStatusDesc);
                            oSms.UpdateSingle_SMS_Status(Convert.ToInt32(dr["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                        }
                    }
                }
            }
            finally
            {
                dt = null;
                oSms = null;
                oAuth = null;
            }
        }

        public static void ApplicationSms(string pLoanNo)
        {
            DataTable dt = null;
            CSMS oSms = null;
            AuthSms oAuth = null;
            string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;

            try
            {
                oSms = new CSMS();
                oAuth = new AuthSms();
                dt = oSms.GetList_ToSend_SMS(pLoanNo, "A");
                foreach (DataRow dr in dt.Rows)
                {
                    vRtnGuid = oAuth.SendSms(dr["MobNo"].ToString(), dr["Msg"].ToString());
                    System.Threading.Thread.Sleep(500);
                    if (vRtnGuid != "" || vRtnGuid != null)//|| vRtnGuid != "2019" || vRtnGuid != "2020" || vRtnGuid != "2021" || vRtnGuid != "2022" || vRtnGuid != "2023")
                    {
                        oAuth.GetGuidStatus(ref vRtnGuid, ref vStatusCode, ref vStatusDesc);
                        oSms.UpdateSingle_SMS_Status(Convert.ToInt32(dr["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                    }
                }
            }
            finally
            {
                dt = null;
                oSms = null;
                oAuth = null;
            }
        }

        
        public static void RepayReminderSms()
        {
            DataTable dt = null;
            CSMS oSms = null;
            AuthSms oAuth = null;
            string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;

            try
            {
                oSms = new CSMS();
                oAuth = new AuthSms();
                dt = oSms.GetList_ToSend_SMS("", "R");
                foreach (DataRow dr in dt.Rows)
                {
                    vRtnGuid = oAuth.SendSms(dr["MobNo"].ToString(), dr["Msg"].ToString());
                    //oSms.UpdateSingle_SMS_Status(Convert.ToInt32(dr["SMSId"].ToString()), vRtnGuid, vStatusCode, vStatusDesc);
                    System.Threading.Thread.Sleep(500);
                    if (vRtnGuid != "" || vRtnGuid != null)// || vRtnGuid != "2019" || vRtnGuid != "2020" || vRtnGuid != "2021" || vRtnGuid != "2022" || vRtnGuid != "2023")
                    {
                        oAuth.GetGuidStatus(ref vRtnGuid, ref vStatusCode, ref vStatusDesc);
                        oSms.UpdateSingle_SMS_Status(Convert.ToInt32(dr["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                    }
                }
            }
            finally
            {
                dt = null;
                oSms = null;
                oAuth = null;
            }
        }

       
        public static void RepayConfirmSms(string pLoanId)
        {
            DataTable dt = null;
            CSMS oSms = null;
            AuthSms oAuth = null;
            string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;

            try
            {
                oSms = new CSMS();
                oAuth = new AuthSms();
                dt = oSms.GetList_ToSend_SMS(pLoanId, "C");
                foreach (DataRow dr in dt.Rows)
                {
                    vRtnGuid = oAuth.SendSms(dr["MobNo"].ToString(), dr["Msg"].ToString());
                    //oSms.UpdateSingle_SMS_Status(Convert.ToInt32(dr["SMSId"].ToString()), vRtnGuid, vStatusCode, vStatusDesc);
                    System.Threading.Thread.Sleep(500);
                    if (vRtnGuid != "" || vRtnGuid != null)// || vRtnGuid != "2019" || vRtnGuid != "2020" || vRtnGuid != "2021" || vRtnGuid != "2022" || vRtnGuid != "2023")
                    {
                        oAuth.GetGuidStatus(ref vRtnGuid, ref vStatusCode, ref vStatusDesc);
                        oSms.UpdateSingle_SMS_Status(Convert.ToInt32(dr["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                    }
                }
            }
            finally
            {
                dt = null;
                oSms = null;
                oAuth = null;
            }
        }

        
        public static void ODReminderSms()
        {
            DataTable dt = null;
            CSMS oSms = null;
            AuthSms oAuth = null;
            string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;

            try
            {
                oSms = new CSMS();
                oAuth = new AuthSms();
                dt = oSms.GetList_ToSend_SMS("", "O");
                foreach (DataRow dr in dt.Rows)
                {
                    vRtnGuid=oAuth.SendSms(dr["MobNo"].ToString(), dr["Msg"].ToString());
                    //oSms.UpdateSingle_SMS_Status(Convert.ToInt32(dr["SMSId"].ToString()), vRtnGuid, vStatusCode, vStatusDesc);
                    System.Threading.Thread.Sleep(500);
                    if (vRtnGuid != "" || vRtnGuid != null)// || vRtnGuid != "2019" || vRtnGuid != "2020" || vRtnGuid != "2021" || vRtnGuid != "2022" || vRtnGuid != "2023")
                    {
                        oAuth.GetGuidStatus(ref vRtnGuid, ref vStatusCode, ref vStatusDesc);
                        oSms.UpdateSingle_SMS_Status(Convert.ToInt32(dr["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                    }
                }
            }
            finally
            {
                dt = null;
                oSms = null;
                oAuth = null;
            }
        }

      
        public static void TotalCollectionSms()
        {
            DataTable dt = null;
            CSMS oSms = null;
            AuthSms oAuth = null;
            string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;
            try
            {
                oSms = new CSMS();
                oAuth = new AuthSms();
                dt = oSms.GetList_ToSend_SMS("", "O");
                foreach (DataRow dr in dt.Rows)
                {
                    vRtnGuid= oAuth.SendSms(dr["MobNo"].ToString(), dr["Msg"].ToString());
                    //oSms.UpdateSingle_SMS_Status(Convert.ToInt32(dr["SMSId"].ToString()), vRtnGuid, vStatusCode, vStatusDesc);
                    System.Threading.Thread.Sleep(500);
                    if (vRtnGuid != "" || vRtnGuid != null)// || vRtnGuid != "2019" || vRtnGuid != "2020" || vRtnGuid != "2021" || vRtnGuid != "2022" || vRtnGuid != "2023")
                    {
                        oAuth.GetGuidStatus(ref vRtnGuid, ref vStatusCode, ref vStatusDesc);
                        oSms.UpdateSingle_SMS_Status(Convert.ToInt32(dr["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                    }
                }
            }
            finally
            {
                dt = null;
                oSms = null;
                oAuth = null;
            }
        }

       
        public static void TestSms()
        {   
            AuthSms oAuth = null;
            string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;
            try
            {
                oAuth = new AuthSms();
                oAuth.SendSms("9830421730", "sending SMS from Force Ten");
                oAuth.GetGuidStatus(ref vRtnGuid, ref vStatusCode, ref vStatusDesc);
                //oAuth.GetSMStoUpdateStatus();
            }
            finally
            {               
                oAuth = null;
            }
        }

        public static void MemberSms()
        {
            DataTable dt = null;
            CSMS oSms = null;
            AuthSms oAuth = null;
            string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;
            try
            {
                oSms = new CSMS();
                oAuth = new AuthSms();
                dt = oSms.GetList_ToSend_SMS("", "M");
                foreach (DataRow dr in dt.Rows)
                {
                    oAuth.SendSms(dr["MobNo"].ToString(), dr["Msg"].ToString());
                    System.Threading.Thread.Sleep(500);
                    if (vRtnGuid != "" || vRtnGuid != null )//|| vRtnGuid != "2019" || vRtnGuid != "2020" || vRtnGuid != "2021" || vRtnGuid != "2022" || vRtnGuid != "2023")
                    {
                         oAuth.GetGuidStatus(ref vRtnGuid, ref vStatusCode, ref vStatusDesc);
                         oSms.UpdateSingle_SMS_Status(Convert.ToInt32(dr["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                    }
                    

                }
            }
            finally
            {
                oAuth = null;
                oSms = null;
            }
        }
        public static void InsertCollectionReminderSMS()
        {
            Int32 VErr = 0;
            CSMS oSms = null;
            

            try
            {
                oSms = new CSMS();
                VErr = oSms.InsertSMS_Coll_Rem_List();
               
            }
            finally
            {
                VErr = 0;
                oSms = null;
               
            }
        }

        public static void ODReminderSMSList()
        {
            Int32 VErr = 0;
            CSMS oSms = null;


            try
            {
                oSms = new CSMS();
                VErr = oSms.OD_Coll_Rem_List();

            }
            finally
            {
                VErr = 0;
                oSms = null;

            }
        }

    }
}
