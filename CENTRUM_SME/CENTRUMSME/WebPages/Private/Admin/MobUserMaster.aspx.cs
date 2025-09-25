using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Admin
{
    public partial class MobUserMaster : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            //GridSearchCache.Remove("GRID_DATA");
            if (!IsPostBack)
            {
                //Permission("btnAdd", "btnEdit", "btnDelete", "btnCancel", "btnSave", "Mobile User Master");
                ViewState["StateEdit"] = null;
                LoadEo(Session[gblValue.BrnchCode].ToString(), System.DateTime.Now);
                popBranch();
               // LoadMobileRoleList();
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()).Selected = true;
                    ddlBranch.Enabled = false;
                }
            }
        }

        
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);
                
                this.Menu = false;
                this.PageHeading = "Mobile User Master";
                this.ShowBranchName = Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuMobUser);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void LoadMobileRoleList(DropDownList ddlRoleList)
        {
            DataTable dtRoleList = null;
            CRole oRole = null;
            try
            {
                oRole = new CRole();
                dtRoleList = oRole.GetRoleList("M");
                ddlRoleList.Items.Clear();
                if (dtRoleList != null && dtRoleList.Rows.Count > 0)
                {
                    ddlRoleList.DataSource = dtRoleList;
                    ddlRoleList.DataValueField = "RoleID";
                    ddlRoleList.DataTextField = "Role";
                    ddlRoleList.DataBind();
                }
                ddlRoleList.Items.Insert(0, new ListItem("<-- Select -->", "-1"));
            }
            finally
            {
                dtRoleList = null;
                oRole = null;
            }
        }

        
        //public override void EnableControl(Boolean Status)
        //{
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public override void ClearControls()
        //{
        //    lblDate.Text = "";
        //    lblUser.Text = "";
        //}

        //protected void ddPageSize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DropDownList ddpagesize = sender as DropDownList;
        //    gvBr.PageSize = Convert.ToInt32(ddpagesize.SelectedItem.Text);
        //    ViewState["PageSize"] = ddpagesize.SelectedItem.Text;
        //    gvBr.DataBind();

        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void gvBr_RowCommand(object sender, GridViewCommandEventArgs e)
        //{

        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void gvBr_DataBound(object sender, EventArgs e)
        //{
        //    if (gvBr.Rows.Count > 0)
        //    {
        //        if (ViewState["PageSize"] != null)
        //        {
        //            DropDownList ddPagesize = gvBr.BottomPagerRow.FindControl("ddPageSize") as DropDownList;
        //            ddPagesize.Items.FindByText((ViewState["PageSize"].ToString())).Selected = true;
        //        }

        //        Label lblCount = gvBr.BottomPagerRow.FindControl("lblPageCount") as Label;
        //        int totRecords = (gvBr.PageIndex * gvBr.PageSize) + gvBr.PageSize;
        //        int totCustomerCount = GridSearch.vRecCount;
        //        totRecords = totRecords > totCustomerCount ? totCustomerCount : totRecords;
        //        lblCount.Text = ((gvBr.PageIndex * gvBr.PageSize) + 1).ToString() + " to " + Convert.ToString(totRecords) + " of " + totCustomerCount.ToString();
        //        gvBr.BottomPagerRow.Visible = true;

        //    }
        //    else
        //    {
        //    }
        //}

        private Boolean SaveRecords()
        {
            Boolean vResult = true;
            String vXML = "";
            Int32 vErr = 0;
            CMob oMob = null;
            try
            {
                vXML = XmlEo();
                if (vXML == "")
                {
                    gblFuction.AjxMsgPopup("Nothing to save.");
                    return false;
                }
                oMob = new CMob();
                String vBr = "";
                vBr = ddlBranch.SelectedValue.ToString();
                vErr = oMob.SaveEoPass(vXML, this.UserID, vBr);
                if (vErr == 0)
                {
                    vResult = true;
                }
                else
                {
                    vResult = false;
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMob = null;
            }
        }

        
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
            ViewState["BranchCode"] = null;
        }

        
        protected void btnDelete_Click(object sender, EventArgs e)
        {


        }

        
        protected void btnEdit_Click(object sender, EventArgs e)
        {


        }

        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
        }

        
        protected void btnAdd_Click(object sender, EventArgs e)
        {
        }

        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveRecords() == true)
            {
                gblFuction.MsgPopup("Record Saved Sucessfully");
            }
            else
            {
                gblFuction.MsgPopup("Data Error...");
            }
        }

        
        private string XmlEo()
        {
            DataTable dt = new DataTable("Table1");

            dt.Columns.Add("EOID");
            dt.Columns.Add("USERNAME");
            dt.Columns.Add("PASSWORD");
            dt.Columns.Add("BRANCH");
            dt.Columns.Add("USERROLE");
            dt.Columns.Add("ALLOW");

            DataRow dr;

            string sXml = "";
            foreach (GridViewRow gr in gvBr.Rows)
            {
                CheckBox chkAllow = (CheckBox)gr.FindControl("chkAllow");
                DropDownList ddlRole = (DropDownList)gr.FindControl("ddlRole");
                TextBox txtPass = (TextBox)gr.Cells[3].FindControl("TxtPas");
                TextBox TxtUser = (TextBox)gr.FindControl("TxtUname");

                if (TxtUser.Text.ToString() != "0" && txtPass.Text.ToString() != "0" && ddlRole.SelectedIndex != 0)
                {
                    dr = dt.NewRow();

                    dr["EOID"] = gr.Cells[0].Text;
                    dr["USERNAME"] = Encryptdata(TxtUser.Text);
                    dr["PASSWORD"] = Encryptdata(txtPass.Text);
                    dr["BRANCH"] = gr.Cells[6].Text;
                    if (chkAllow.Checked == true)
                    {
                        dr["ALLOW"] = "Y";
                    }
                    else
                    {
                        dr["ALLOW"] = "N";
                    }
                    dr["USERROLE"] = ddlRole.SelectedValue;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
            }

            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                sXml = oSW.ToString();
            }
            return sXml;
        }

        
        private void LoadEo(String pBr, DateTime pDate)
        {
            DataTable dt = null;
            CMob oDs = null;
            try
            {
                oDs = new CMob();
                dt = oDs.GetMobEOAloc(pBr, pDate);
                gvBr.DataSource = dt;
                ViewState["DataGrid"] = dt;
                gvBr.DataBind();
            }
            finally
            {
                oDs = null;
            }
        }

        private void popBranch()
        {
            DataTable dt = null;
            CMob oMob = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oMob = new CMob();
                dt = oMob.GetBranchPop();
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                ListItem oli = new ListItem("ALL", "0000");
                ddlBranch.Items.Insert(0, oli);
            }
            finally
            {
                oMob = null;
                dt = null;
            }
        }

        
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            String vBr = "";
            vBr = ddlBranch.SelectedValue.ToString();
            LoadEo(vBr, System.DateTime.Now);
        }

        
        protected void TxtUname_TextChanged(object sender, EventArgs e)
        {
            int Count = 0;
            TextBox TxtUser = (TextBox)sender;
            String Uname = TxtUser.Text.ToString();
            if (Uname.Length > 0)
            {
                foreach (GridViewRow gvRw in gvBr.Rows)
                {

                    TextBox TxtOthUser = (TextBox)gvRw.FindControl("TxtUname");
                    if (TxtOthUser.Text.ToString() == Uname)
                    {
                        Count = Count + 1;
                        if (Count > 1)
                        {
                            TxtUser.Text = "";
                            gblFuction.AjxMsgPopup("User " + Uname + "already exist");
                        }
                    }

                }
            }

        }

        
        private string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        
        private string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }

        
        protected void gvBr_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null; CRole oRole = null; string vRole = "";
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkAllow = (CheckBox)e.Row.FindControl("chkAllow");
                    DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddlRole");
                    TextBox txtAllow = (TextBox)e.Row.FindControl("txtAllow");
                    TextBox txtRole = (TextBox)e.Row.FindControl("txtRole");
                    TextBox TxtPas = (TextBox)e.Row.FindControl("TxtPas");
                    TextBox TxtUser = (TextBox)e.Row.FindControl("TxtUname");
                    TxtPas.Text = Decryptdata(TxtPas.Text.ToString());
                    TxtUser.Text = Decryptdata(TxtUser.Text.ToString());
                    if (txtAllow.Text == "Y")
                        chkAllow.Checked = true;
                    else
                        chkAllow.Checked = false;
                    LoadMobileRoleList(ddlRole);
                    oRole = new CRole();                   
                    DropDownList ddRole = (DropDownList)e.Row.FindControl("ddlRole");
                    dt = oRole.GetRoleList("M");
                    if (dt.Rows.Count > 0)
                    {
                        
                        ddRole.DataSource = dt;
                        ddRole.DataTextField = "Role";
                        ddRole.DataValueField = "RoleID";
                        ddRole.DataBind();
                        ListItem oItem = new ListItem();
                        oItem.Text = "<--- Select --->";
                        oItem.Value = "-1";
                        ddRole.Items.Insert(0, oItem);
                    }
                    vRole = txtRole.Text;
                    ddRole.SelectedIndex = ddRole.Items.IndexOf(ddRole.Items.FindByValue(vRole));

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oRole = null;

            }
        }

    }
}
