using System;
using CENTRUMBA;
using CENTRUMCA;
using System.Data;
using SendSms;

namespace CENTRUMSME.Private.Webpages.Admin
{
    public partial class SMSReminder : CENTRUMBAse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                //DateTime dt = DateTime.Now;
                string StrDD = "", pDate;
                pDate = txtDtFrm.Text;
                DateTime dDate = System.DateTime.Now;
                if (txtDtFrm.Text == "")
                    dDate = Convert.ToDateTime("01/01/1900");
                else
                {
                    if (pDate.Length == 9)
                        pDate = pDate.Insert(0, "0");

                    StrDD = pDate.Substring(0, 2);
                }

                int day = Convert.ToInt32(StrDD);
                if (day == 5 || day == 7 || day == 10)
                {
                    btnEMIReminder.Enabled = true;
                }
                else
                {
                    btnEMIReminder.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.PageHeading = "SMS Reminder";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuSMSReminder);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEMIReminder.Visible = false;
                    btnODReminder.Visible = false;
                    btnExit.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEMIReminder.Visible = false;
                    btnODReminder.Visible = false;
                    btnExit.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnEMIReminder.Visible = false;
                    btnODReminder.Visible = false;
                    btnExit.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnEMIReminder.Visible = true;
                    btnODReminder.Visible = true;
                    btnExit.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "SMS Reminder", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateDate()
        {
            bool vRst = true;
            string vLogDate = Session[gblValue.LoginDate].ToString();
            DateTime vLogDt = gblFuction.setDate(vLogDate);
            DateTime vEndDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            if (vLogDt != vEndDt)
            {
                gblFuction.MsgPopup("You Can Not Process Carry Forward On " + vLogDate);
                vRst = false;
            }
            return vRst;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEMIReminder_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 vRst = 0;
                CSMSReminder oReminder = new CSMSReminder();
                vRst = oReminder.GetEMIReminder(gblFuction.setDate(txtDtFrm.Text));
                if (vRst == 0)
                {
                    DataTable dt_Sms = new DataTable();
                    CSMS oSms = null;
                    AuthSms oAuth = null;
                    string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;
                    oSms = new CSMS();
                    oAuth = new AuthSms();
                    // For Applicant,Co-Applicant (CR ----> Collection Reminder)
                    dt_Sms = oSms.Get_ToSend_SMS_Remind("CR");
                    if (dt_Sms.Rows.Count > 0)
                    {
                        foreach (DataRow drSMS in dt_Sms.Rows)
                        {
                            if (drSMS["MobNo"].ToString().Length >= 10)
                            {
                                vRtnGuid = oAuth.SendSms(drSMS["MobNo"].ToString(), drSMS["Msg"].ToString());
                                System.Threading.Thread.Sleep(500);
                                if (!string.IsNullOrEmpty(vRtnGuid))
                                {
                                    vRtnGuid = vRtnGuid.Remove(0, 7);
                                    if (vRtnGuid != "")
                                    {
                                        vStatusDesc = "Message Delivered";
                                        vStatusCode = "Message Delivered Successfully";
                                        oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                                    }
                                    else
                                    {
                                        vStatusDesc = "Unknown Error";
                                        vStatusCode = "Unknown Error";
                                        oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                                    }
                                }
                                else
                                {
                                    vStatusDesc = "Unknown Error";
                                    vStatusCode = "Unknown Error";
                                    oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                                }
                            }
                        }
                        gblFuction.MsgPopup("Message Sent Successfully .");
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Message Not Sent..");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnODReminder_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 vRst = 0;
                CSMSReminder oReminder = new CSMSReminder();
                vRst = oReminder.GetODReminder(gblFuction.setDate(txtDtFrm.Text));
                if (vRst == 0)
                {
                    DataTable dt_Sms = new DataTable();
                    CSMS oSms = null;
                    AuthSms oAuth = null;
                    string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;
                    oSms = new CSMS();
                    oAuth = new AuthSms();
                    // For Applicant  (OD ----> Over Due Reminder)
                    dt_Sms = oSms.Get_ToSend_SMS_Remind("OD");
                    if (dt_Sms.Rows.Count > 0)
                    {
                        foreach (DataRow drSMS in dt_Sms.Rows)
                        {
                            if (drSMS["MobNo"].ToString().Length >= 10)
                            {
                                vRtnGuid = oAuth.SendSms(drSMS["MobNo"].ToString(), drSMS["Msg"].ToString());
                                System.Threading.Thread.Sleep(500);
                                if (!string.IsNullOrEmpty(vRtnGuid))
                                {
                                    vRtnGuid = vRtnGuid.Remove(0, 7);
                                    if (vRtnGuid != "")
                                    {
                                        vStatusDesc = "Message Delivered";
                                        vStatusCode = "Message Delivered Successfully";
                                        oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                                    }
                                    else
                                    {
                                        vStatusDesc = "Unknown Error";
                                        vStatusCode = "Unknown Error";
                                        oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                                    }
                                }
                                else
                                {
                                    vStatusDesc = "Unknown Error";
                                    vStatusCode = "Unknown Error";
                                    oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                                }
                            }
                        }
                    }
                    gblFuction.MsgPopup("Message Sent Successfully .");
                }
                else
                {
                    gblFuction.MsgPopup("Message Not Sent..");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}