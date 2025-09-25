using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Data;
using System.IO;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class Attendance : CENTRUMBase
    {
        protected int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                //StatusButton("View");
                tbBnk.ActiveTabIndex = 0;
                txtEftDt.Text = Session[gblValue.LoginDate].ToString();
                //txtEftDt.Enabled = false;

            }


        }
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Attendance";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBlockMst);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Attendance", false);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSch_Click(object sender, EventArgs e)
        {
            LoadGrid();

        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGrid()
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();        
            string vDateTxt = txtEftDt.Text.Trim();
            DateTime vDate;
            int vErr = 0;
            CAttendance oHR = null;
            DataTable dt = null;
            string pMode = "";         
            if (vDateTxt != "")
            {
                vDate = gblFuction.setDate(vDateTxt);
                oHR = new CAttendance();
                dt = oHR.GetEmployeeAttendance(vBrCode, vDate);                 
                gvSchdl.DataSource = dt;
                gvSchdl.DataBind();              

            }
            else
            {
                gvSchdl.DataSource = null;
                gvSchdl.DataBind();
            }
        }


        protected void gvSchdl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int i = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                i++;
                CheckBox chkAtt = (CheckBox)e.Row.FindControl("chkAtt");
                DropDownList ddlType = (DropDownList)e.Row.FindControl("ddlType");
                DropDownList ddlHDay = (DropDownList)e.Row.FindControl("ddlHDay");
                try
                {                   
                    chkAtt.Checked = ((HiddenField)e.Row.FindControl("hdnAttYN")).Value == "Y" ? true : false;                  
                 
                }
                catch (Exception ex)
                {
                    throw ex;

                }
                finally
                {
                    chkAtt = null;
                    ddlType = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (Convert.ToString(Session[gblValue.UserLoginType]) == "E")
            //{
            //    gblFuction.MsgPopup("Only view rights...no Updation from Employee Kiosk");
            //    return;
            //}
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                //StatusButton("View");
                ViewState["StateEdit"] = null;
                gvSchdl.DataSource = null;
                gvSchdl.DataBind();
            }
        }


        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Mode = "Save";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vErr = 0;
            string vEoBrCode = "-1", vXML = "", pMode = "";
            DateTime vEftDt;
            DataTable dtXML = null;
            CAttendance oHr = null;
            try
            {               
                vEftDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                dtXML = GetTable();
                if (dtXML.Rows.Count > 0)
                    vXML = GetXml(dtXML);
                else
                {
                    gblFuction.AjxMsgPopup("No Data found...");
                    return false;
                }

                if (Mode == "Save")
                {
                    oHr = new CAttendance();

                    vErr = oHr.SaveAttendance(vBrCode, vEftDt, vXML);

                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oHr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTime"></param>
        /// <returns></returns>
        public DateTime setTime(string pTime)
        {
            if (pTime == "")
                pTime = "00:00:00 AM";
            DateTime vDate = Convert.ToDateTime("01/01/1900" + " " + pTime);
            return vDate;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetXml(DataTable dt)
        {
            string vXmlData = "";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXmlData = oSW.ToString();
            }
            return vXmlData;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GetTable()
        {
            DataTable dt = new DataTable("Table1");

            DataColumn dc1 = new DataColumn("EmployeeID");
            dt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn("AttYN");
            dt.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn("InTime");
            dt.Columns.Add(dc3);
            DataColumn dc4 = new DataColumn("OutTime");
            dt.Columns.Add(dc4); 
            DataColumn dc5 = new DataColumn("Date");
            dt.Columns.Add(dc5);

            foreach (GridViewRow gR in gvSchdl.Rows)
            {
                TextBox txtIn, txtOut;
                CheckBox chkAtt = (CheckBox)gR.FindControl("chkAtt");
                txtIn = (TextBox)gR.FindControl("txtIn");
                txtOut = (TextBox)gR.FindControl("txtOut");               
                TextBox txtEfctDt = (TextBox)gR.FindControl("txtEfctDt");    
                DataRow dR = dt.NewRow();
                dR["EmployeeID"] = gR.Cells[6].Text.Trim();
                dR["Date"] = gblFuction.setDate(txtEfctDt.Text);
                dR["AttYN"] = chkAtt.Checked == true ? "Y" : "N";
                dR["InTime"] = setTime(txtIn.Text.Trim());
                dR["OutTime"] = setTime(txtOut.Text.Trim());               
                DateTime intime = Convert.ToDateTime(dR["InTime"]);
                DateTime outtime = Convert.ToDateTime(dR["OutTime"]);
                if (chkAtt.Checked == true)
                {
                    if (txtEfctDt.Text == "")
                    {
                        gblFuction.MsgPopup("Give Attendance Date");
                        dt.Clear();
                        return dt;
                    }
                    if (intime > outtime)
                    {
                        gblFuction.MsgPopup("Out Time should not be before than In Time...");
                        dt.Clear();
                        return dt;
                    }
                }              
                dt.Rows.Add(dR);           

            }
            return dt;
        }

    }

}