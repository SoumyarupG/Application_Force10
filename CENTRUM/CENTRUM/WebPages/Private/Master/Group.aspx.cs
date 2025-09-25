using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Web;
using CENTRUM.WebSrvcs;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class Group : CENTRUMBase
    {
        string path = "";
        string GroupBucket = ConfigurationManager.AppSettings["GroupBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string GroupURL = ConfigurationManager.AppSettings["GroupURL"];
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
                txtDtFormation.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                ViewState["CentreForDt"] = null;
                ViewState["GrpId"] = null;
                // ViewState["ImgUploadYN"] = "Y";
                LoadList();
                PopRO();
                popGrpFormedBy();
                popVillage();
                popCltype();
                tbGrp.ActiveTabIndex = 0;
                ClearControls();
                StatusButton("View");
                EnableControl(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtGroupNo.Enabled = Status;
            ddlEo.Enabled = Status;
            ddlCenter.Enabled = Status;
            ddlGrpFormedby.Enabled = Status;
            txtGroup.Enabled = Status;
            ddlVillage.Enabled = Status;
            txtGrpLeader.Enabled = Status;
            cbDrp.Enabled = Status;
            txtDtCl.Enabled = Status;
            txtRem.Enabled = Status;
            txtPin.Enabled = Status;
            txtPh1.Enabled = Status;
            txtPh2.Enabled = Status;
            //txtDtFormation.Enabled = Status;
            ddlClTyp.Enabled = Status;
            Boolean vStatus = Convert.ToInt32(Session[gblValue.RoleId]) == 1 || Convert.ToInt32(Session[gblValue.RoleId]) == 25 ? Status : false;
            fuGroup.Enabled = vStatus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnDelete.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnDelete.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnDelete.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnDelete.Enabled = false;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnDelete.Enabled = false;
                    EnableControl(false);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlEo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vEoId = ddlEo.SelectedItem.Value;
            PopCenter(vEoId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlVillage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vVillageId = Convert.ToInt32(ddlVillage.SelectedItem.Value);
            PopVillDtl(vVillageId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pVillageId"></param>
        private void PopVillDtl(Int32 pVillageId)
        {
            DataTable dt = null;
            CGroup oGb = null;
            try
            {
                if (pVillageId > 0)
                {
                    oGb = new CGroup();
                    dt = oGb.GetVillageDtls(pVillageId);
                    ddlState.DataSource = dt;
                    ddlState.DataTextField = "StateName";
                    ddlState.DataValueField = "StateID";
                    ddlState.DataBind();

                    ddlDist.DataSource = dt;
                    ddlDist.DataTextField = "DistrictName";
                    ddlDist.DataValueField = "DistrictId";
                    ddlDist.DataBind();

                    ddlBlock.DataSource = dt;
                    ddlBlock.DataTextField = "BlockName";
                    ddlBlock.DataValueField = "BlockId";
                    ddlBlock.DataBind();

                    ddlGp.DataSource = dt;
                    ddlGp.DataTextField = "GPName";
                    ddlGp.DataValueField = "GPId";
                    ddlGp.DataBind();

                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlState.Items.Insert(1, oli);
                    ddlDist.Items.Insert(1, oli);
                    ddlBlock.Items.Insert(1, oli);
                    ddlGp.Items.Insert(1, oli);
                }
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.CanAdd == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Add);
                return;
            }
            ViewState["StateEdit"] = "Add";
            tbGrp.ActiveTabIndex = 1;
            StatusButton("Add");
            cbDrp.Enabled = false;
            txtDtCl.Enabled = false;
            txtRem.Enabled = false;
            ddlClTyp.Enabled = false;
            ClearControls();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    gblFuction.MsgPopup("Branch can not Delete Account Group...");
                //    return;
                //}
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadList();
                    StatusButton("Delete");
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
        /// <param name="pEoId"></param>
        private void PopCenter(string pEoId)
        {
            DataTable dt = null;
            CLoanRecovery oCL = null;

            try
            {
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                oCL = new CLoanRecovery();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = oCL.PopCenterWithCollDay(pEoId, vLogDt, vBrCode, "N"); //With No CollDay


                if (dt.Rows.Count > 0)
                {
                    ViewState["CentreForDt"] = dt;
                    ddlCenter.DataSource = dt;
                    ddlCenter.DataTextField = "Market";
                    ddlCenter.DataValueField = "MarketID";
                    ddlCenter.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlCenter.Items.Insert(0, oli);

                    ViewState["CentreID"] = Convert.ToString(dt.Rows[0]["Marketid"]).Trim();
                }
            }
            finally
            {
                dt = null;
                oCL = null;
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
                this.PageHeading = "Group";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuGrpMst);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnDelete.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Group Master", false);
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
        private void popGrpFormedBy()
        {
            DataTable dt = null;
            CEO oGb = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oGb = new CEO();
                dt = oGb.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlGrpFormedby.DataSource = dt;
                if (dt.Rows.Count > 0)
                {
                    ddlGrpFormedby.DataTextField = "EoName";
                    ddlGrpFormedby.DataValueField = "EoId";
                    ddlGrpFormedby.DataBind();
                }
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlGrpFormedby.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //private void popEO()
        //{
        //    DataTable dt = null;
        //    CGblIdGenerator oGb = null;
        //    string vBrCode = "";           
        //    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    try
        //    {
        //        vBrCode = (string)Session[gblValue.BrnchCode];               
        //        oGb = new CGblIdGenerator();
        //        dt = oGb.PopComboMIS("D", "N", "AA", "EoId", "EoName", "EoMst", vBrCode, "BranchCode", "Tra_DropDate", vLogDt, vBrCode);
        //        ddlEo.DataSource = dt;
        //        ddlEo.DataTextField = "EoName";
        //        ddlEo.DataValueField = "EoId";
        //        ddlEo.DataBind();
        //        ListItem oli = new ListItem("<--Select-->", "-1");
        //        ddlEo.Items.Insert(0, oli);
        //    }
        //    finally
        //    {
        //        oGb = null;
        //        dt = null;
        //    }
        //}


        private void PopRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlEo.DataSource = dt;
                ddlEo.DataTextField = "EoName";
                ddlEo.DataValueField = "Eoid";
                ddlEo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlEo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popVillage()
        {
            DataTable dt = null;
            CVillage oGb = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oGb = new CVillage();
                dt = oGb.PopVillage(vBrCode);
                ddlVillage.DataSource = dt;
                ddlVillage.DataTextField = "VillageName";
                ddlVillage.DataValueField = "VillageId";
                ddlVillage.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlVillage.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popCltype()
        {
            ListItem[] items = new ListItem[3];
            items[0] = new ListItem("<--Select-->", "-1");
            items[1] = new ListItem("Normal", "N");
            items[2] = new ListItem("Unsatisfied", "U");
            ddlClTyp.Items.AddRange(items);
            ddlClTyp.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadList()
        {
            DataTable dtRoot = null;
            TreeNode tnRoot = null, tnGrp = null;
            string vEoId = "";
            tvGrp.Nodes.Clear();
            CEO oRO = null;

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dtRoot = oRO.PopRO(Session[gblValue.BrnchCode].ToString(), "0", "0", vLogDt, this.UserID);
                foreach (DataRow dr in dtRoot.Rows)
                {
                    tnRoot = new TreeNode(dr["EoName"].ToString());
                    tnRoot.PopulateOnDemand = false;
                    tnRoot.SelectAction = TreeNodeSelectAction.None;
                    tvGrp.Nodes.Add(tnRoot);
                    tnRoot.Value = Convert.ToString("CM" + dr["Eoid"]);
                    vEoId = Convert.ToString(dr["Eoid"]);
                    tnGrp = new TreeNode("No Record");
                    tnGrp.Value = "CM";
                    tnRoot.ChildNodes.Add(tnGrp);
                    tnRoot.CollapseAll();
                }
            }
            finally
            {
                dtRoot = null;
                oRO = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtDtFormation.Text = Session[gblValue.LoginDate].ToString();
            txtGroupNo.Text = "";
            ddlEo.SelectedIndex = -1;
            ddlCenter.SelectedIndex = -1;
            ddlGrpFormedby.SelectedIndex = -1;
            txtGroup.Text = "";

            ddlState.Items.Clear();
            ddlState.SelectedIndex = -1;
            ddlState.DataValueField = "";

            ddlVillage.SelectedIndex = -1;
            ddlVillage.DataValueField = "";

            ddlDist.Items.Clear();
            ddlDist.SelectedIndex = -1;
            ddlDist.DataValueField = "";

            ddlBlock.Items.Clear();
            ddlBlock.SelectedIndex = -1;
            ddlBlock.DataValueField = "";

            ddlGp.Items.Clear();
            ddlGp.SelectedIndex = -1;
            ddlGp.DataValueField = "";
            txtGrpLeader.Text = "";
            cbDrp.Checked = false;
            txtDtCl.Text = "";
            txtRem.Text = "";
            lblUser.Text = "";
            txtPin.Text = "";
            txtPh1.Text = "";
            txtPh2.Text = "";
            ddlClTyp.SelectedIndex = -1;
            ddlClTyp.DataValueField = "";
            imgGroupPhoto.ImageUrl = "~/Images/no-image-icon.jpg";

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.CanEdit == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Edit);
                return;
            }
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
            txtDtFormation.Enabled = false;
            ddlEo.Enabled = false;
            ddlCenter.Enabled = false;
            ddlClTyp.Enabled = true;
            //if (Convert.ToString(ViewState["ImgUploadYN"]) == "N")
            //{
            //    fuGroup.Enabled = false;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadList();
                StatusButton("Show");
                ViewState["StateEdit"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            EnableControl(false);
            StatusButton("View");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            ViewState["GroupId"] = null;
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbDrp_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDrp.Checked == true)
            {
                txtDtCl.Text = Session[gblValue.LoginDate].ToString();
                txtDtCl.Enabled = false;
            }
            if (cbDrp.Checked == false)
            {
                txtDtCl.Text = "";
                txtDtCl.Enabled = true;
                ddlClTyp.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vGrpCode = "", vGrpName = "", vStat = "N", vCltype = "", vCenterId = "", vFormedBy = "";
            string vNewId = Convert.ToString(ViewState["GrpId"]);
            Int32 vErr = 0, vRec = 0, vGrFrm = 0, vVillageId = 0;
            DateTime vFrmDt = gblFuction.setDate(txtDtFormation.Text);
            DateTime vClDt = gblFuction.setDate(txtDtCl.Text);
            //DateTime vEndDt = gblFuction.setDate(Session[gblValue.EndDate].ToString());
            path = ConfigurationManager.AppSettings["pathGroup"];
            CGroup oGrp = null;
            CGblIdGenerator oGbl = null;
            DataTable dt1 = new DataTable();
            try
            {
                vCenterId = ddlCenter.SelectedValue;
                vGrFrm = Convert.ToInt32(ddlGrpFormedby.SelectedValue);
                vVillageId = Convert.ToInt32(ddlVillage.SelectedValue);
                vCltype = Convert.ToString(ddlClTyp.SelectedValue);
                vGrpName = Convert.ToString(txtGroup.Text.Replace("'", "''"));
                vFormedBy = ddlGrpFormedby.SelectedValue;
                if (cbDrp.Checked == true)
                    vStat = "Y";
                DataTable dt = (DataTable)ViewState["CentreForDt"];
                DateTime vCentForDt = gblFuction.setDate(dt.Rows[ddlCenter.SelectedIndex - 1]["Formationdt"].ToString());
                if (vFrmDt < vCentForDt)
                {
                    gblFuction.MsgPopup("Formation Date must be Greater than or equal with Center Formation Date...");
                    return false;
                }

                //if (vCentForDt > vEndDt.AddDays(1))
                //{
                //    gblFuction.MsgPopup("Formation Date must be Less than or equal with End Date...");
                //    return false;
                //}
                this.GetModuleByRole(mnuID.mnuGrpMst);

                if (Mode == "Save")
                {
                    oGrp = new CGroup();
                    oGbl = new CGblIdGenerator();
                    if (vFrmDt > gblFuction.setDate(Session[gblValue.LoginDate].ToString()))
                    {
                        gblFuction.MsgPopup("Formation Date must be less than or equal with login date...");
                        return false;
                    }
                    vRec = oGbl.ChkDuplicate("GroupMSt", "GroupName", vGrpName, "MarketID", vCenterId, "Groupid", vNewId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Group Can not be Duplicate within same Center...");
                        return false;
                    }
                    //dt1 = oGrp.ChkCenterByGroupId(vCenterId, vBrCode);
                    //if (dt1.Rows.Count > 6)
                    //{
                    //    gblFuction.MsgPopup("You can not enter More than 6 Group in one center...");
                    //    return false;
                    //}
                    vErr = oGrp.SaveGroup(ref vNewId, vCenterId, vFrmDt, vGrpName, ref vGrpCode,
                        txtGrpLeader.Text.Replace("'", "''"), txtPh1.Text, txtPh2.Text, vVillageId,
                        txtPin.Text, vStat, vClDt, vCltype, txtRem.Text.Replace("'", "''"),
                        vBrCode, vFormedBy, Convert.ToInt32(Session[gblValue.UserId]), "Save");
                    if (vErr > 0)
                    {
                        ViewState["GroupId"] = vNewId;
                        txtGroupNo.Text = vGrpCode;
                        txtGroup.Text = vGrpName;
                        if (fuGroup.HasFile)
                        {
                            if (MinioYN == "N")
                            {
                                string vMessage = SaveMemberImages(fuGroup, vNewId, "GroupPhoto", "Edit", "N", path);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                byte[] vFileByte = ConvertFileToByteArray(fuGroup.PostedFile);
                                string isImageSaved = oAC.UploadFileMinio(vFileByte, "GroupPhoto.png", vNewId, GroupBucket, MinioUrl);
                            }
                        }
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    vStat = cbDrp.Checked ? "Y" : "N";
                    if (vStat == "Y")
                    {
                        if (txtDtCl.Text == "")
                        {
                            gblFuction.MsgPopup("Date of Closing can not be empty...");
                            return false;
                        }
                        if (vClDt <= vFrmDt)
                        {
                            gblFuction.MsgPopup("Date of Closing must be greater than Formation Date...");
                            return false;
                        }
                        if (vCltype == "-1")
                        {
                            gblFuction.MsgPopup("Select Closing Type...");
                            return false;
                        }
                        if (txtRem.Text == "")
                        {
                            gblFuction.MsgPopup("Remarks can not be empty...");
                            return false;
                        }
                        oGbl = new CGblIdGenerator();
                        vErr = oGbl.ChkDeleteString(vNewId, "GroupID", "MemberMst");
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup("This Group has active Member, Dropout is Not Allowed");
                            return false;
                        }
                    }
                    oGrp = new CGroup();
                    vErr = oGrp.SaveGroup(ref vNewId, vCenterId, vFrmDt, vGrpName, ref vGrpCode,
                        txtGrpLeader.Text.Replace("'", "''"), txtPh1.Text, txtPh2.Text, vVillageId,
                        txtPin.Text, vStat, vClDt, vCltype, txtRem.Text.Replace("'", "''"),
                        vBrCode, vFormedBy, Convert.ToInt32(Session[gblValue.UserId]), "Edit");
                    if (vErr > 0)
                    {
                        if (fuGroup.HasFile)
                        {
                            if (MinioYN == "N")
                            {
                                string vMessage = SaveMemberImages(fuGroup, vNewId, "GroupPhoto", "Edit", "N", path);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                byte[] vFileByte = ConvertFileToByteArray(fuGroup.PostedFile);
                                string isImageSaved = oAC.UploadFileMinio(vFileByte, "GroupPhoto.png", vNewId, GroupBucket, MinioUrl);
                            }
                        }
                        gblFuction.MsgPopup(gblMarg.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    vStat = cbDrp.Checked ? "Y" : "N";
                    oGbl = new CGblIdGenerator();
                    vErr = oGbl.ChkDeleteString(vNewId, "GroupID", "MemberMst");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("This Group has active Member, the system will not allow to delete.");
                        vResult = false;
                    }
                    oGrp = new CGroup();
                    vErr = oGrp.SaveGroup(ref vNewId, vCenterId, vFrmDt, vGrpName, ref vGrpCode,
                        txtGrpLeader.Text.Replace("'", "''"), txtPh1.Text, txtPh2.Text, vVillageId,
                        txtPin.Text, vStat, vClDt, vCltype, txtRem.Text.Replace("'", "''"),
                        vBrCode, vFormedBy, Convert.ToInt32(Session[gblValue.UserId]), "Delet");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.DeleteMsg);
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
            finally
            {
                oGrp = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvGrp_SelectedNodeChanged(object sender, EventArgs e)
        {
            CGroup oGrp = null;
            DataTable dt = null;
            string vGrpId = "";
            try
            {
                vGrpId = Convert.ToString(tvGrp.SelectedNode.Value);
                ViewState["GrpId"] = vGrpId;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                fillGroupDtl(vGrpId);
            }
            finally
            {
                dt = null;
                oGrp = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvGrp_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            ////////DataTable dt = null;
            ////////TreeNode tnGrp = null;
            ////////CGblIdGenerator oCb = null;
            ////////string vCentId = "";
            ////////string vBrCode = Session[gblValue.BrnchCode].ToString();
            ////////try
            ////////{
            ////////    if (e.Node.Value.Substring(0, 2) == "MK")
            ////////    {
            ////////        oCb = new CGblIdGenerator();
            ////////        e.Node.ChildNodes.Clear();
            ////////        vCentId = e.Node.Value.Substring(2);
            ////////        dt = oCb.PopComboMIS("S", "N", "N", "Groupid", "GroupName", "GroupMSt", vCentId, "Marketid", "AA", gblFuction.setDate(""), vBrCode);
            ////////        if (dt.Rows.Count > 0)
            ////////        {
            ////////            foreach (DataRow drBr in dt.Rows)
            ////////            {
            ////////                tnGrp = new TreeNode(Convert.ToString(drBr["GroupName"]));
            ////////                tnGrp.Value = Convert.ToString(drBr["Groupid"]);
            ////////                tnGrp.PopulateOnDemand = false;
            ////////                e.Node.ChildNodes.Add(tnGrp);
            ////////            }
            ////////            ViewState["GroupId"] = tnGrp.Value;
            ////////        }
            ////////    }
            ////////}
            ////////finally
            ////////{
            ////////    dt = null;
            ////////    oCb = null;
            ////////}
            DataTable dtGroup = null, dtCent = null;
            CMember oMem = null;
            string vCMId = "", vCentId = "";
            TreeNode tnGroup = null, tnCent = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                oMem = new CMember();
                if (e.Node.Value.Substring(0, 2) == "CM")
                {
                    e.Node.ChildNodes.Clear();
                    vCMId = Convert.ToString(e.Node.Value.Substring(2));
                    dtCent = oMem.GetCenterByEoId(vCMId, vBrCode, vLogDt);
                    foreach (DataRow drBr in dtCent.Rows)
                    {
                        tnCent = new TreeNode(Convert.ToString(drBr["Market"]));
                        tnCent.Value = Convert.ToString("CN" + drBr["MarketId"]);
                        e.Node.ChildNodes.Add(tnCent);
                        tnGroup = new TreeNode("No Record");
                        e.Node.SelectAction = TreeNodeSelectAction.None;
                        tnGroup.Value = "0";
                        tnCent.ChildNodes.Add(tnGroup);
                    }
                }
                else if (e.Node.Value.Substring(0, 2) == "CN")
                {
                    e.Node.ChildNodes.Clear();
                    vCentId = Convert.ToString(e.Node.Value.Substring(2));

                    dtGroup = oMem.GetGroupByCenterId(vCentId, vBrCode, vLogDt);
                    foreach (DataRow drGrp in dtGroup.Rows)
                    {
                        tnGroup = new TreeNode(Convert.ToString(drGrp["GrpCode"]));
                        tnGroup.Value = Convert.ToString(drGrp["GroupId"]);
                        tnGroup.PopulateOnDemand = false;
                        e.Node.SelectAction = TreeNodeSelectAction.Select;
                        e.Node.ChildNodes.Add(tnGroup);
                    }
                }
            }
            finally
            {

            }
        }
        
        private void fillGroupDtl(string vGrpId)
        {
            CGroup oGrp = null;
            DataTable dt = null;
            oGrp = new CGroup();
            string vGroupImgPathNew = GroupURL + vGrpId + "_GroupPhoto.png?t=" + DateTime.Now;
            string base64String = ViewGroupImage("GroupPhoto.png", vGrpId);
            imgGroupPhoto.ImageUrl = "data:image/png;base64," + base64String;
          
            dt = oGrp.GetGroupDetails(vGrpId);
            if (dt.Rows.Count > 0)
            {
                txtGroupNo.Text = Convert.ToString(dt.Rows[0]["GroupNo"]);
                ddlEo.SelectedIndex = ddlEo.Items.IndexOf(ddlEo.Items.FindByValue(Convert.ToString(dt.Rows[0]["EOID"]).Trim()));
                PopCenter(ddlEo.SelectedValue);
                ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(Convert.ToString(dt.Rows[0]["Marketid"]).Trim()));
                ddlGrpFormedby.SelectedIndex = ddlGrpFormedby.Items.IndexOf(ddlGrpFormedby.Items.FindByValue(Convert.ToString(dt.Rows[0]["FormedBy"]).Trim()));
                txtGroup.Text = Convert.ToString(dt.Rows[0]["GroupName"]);
                ddlVillage.SelectedIndex = ddlVillage.Items.IndexOf(ddlVillage.Items.FindByValue(Convert.ToString(dt.Rows[0]["VillageID"]).Trim()));
                PopVillDtl(Convert.ToInt32(ddlVillage.SelectedValue));
                txtGrpLeader.Text = Convert.ToString(dt.Rows[0]["GroupLeader"]);
                txtPin.Text = Convert.ToString(dt.Rows[0]["PIN"]);
                txtPh1.Text = Convert.ToString(dt.Rows[0]["GLContactNo1"]);
                txtPh2.Text = Convert.ToString(dt.Rows[0]["GLContactNo2"]);
                // ViewState["ImgUploadYN"] = Convert.ToString(dt.Rows[0]["ImgUploadYN"]);
                //--------------------Group Image------------------------
                //string pathImage = ConfigurationManager.AppSettings["PathImage"];            
                //if (ImageExist(vGroupImgPath) == false)
                //{
                //    vGroupImgPath = pathImage + "Group/" + txtGroupNo.Text + "_GroupPhoto.png?t=" + DateTime.Now;
                //    if (ImageExist(vGroupImgPath) == false)
                //    {
                //        vGroupImgPath = "https://centrumimage.bijliftt.com/Group/" + txtGroupNo.Text + "/GroupPhoto.png?t=" + DateTime.Now;
                //        if (ImageExist(vGroupImgPath) == false)
                //        {
                //            vGroupImgPath = "https://centrumimage.bijliftt.com/Group/" + txtGroupNo.Text + "_GroupPhoto.png?t=" + DateTime.Now;
                //            if (ImageExist(vGroupImgPath) == false)
                //            {
                //                vGroupImgPath = "https://networkimage.bijliftt.com/Centrum_Image/Group/" + txtGroupNo.Text + "/GroupPhoto.png?t=" + DateTime.Now;
                //                if (ImageExist(vGroupImgPath) == false)
                //                {
                //                    vGroupImgPath = "https://networkimage.bijliftt.com/Centrum_Image/Group/" + txtGroupNo.Text + "_GroupPhoto.png?t=" + DateTime.Now;
                //                }
                //            }
                //        }
                //    }
                //}               
                //imgGroupPhoto.ImageUrl = vGroupImgPath;
                //----------------------------------------------------------
                if (dt.Rows[0]["Tra_Drop"].ToString() == "Y")
                {
                    cbDrp.Checked = true;
                    txtDtCl.Text = Convert.ToString(dt.Rows[0]["Tra_DropDate"]);
                    ddlClTyp.SelectedIndex = ddlClTyp.Items.IndexOf(ddlClTyp.Items.FindByValue(Convert.ToString(dt.Rows[0]["ClType"]).Trim()));
                    txtRem.Text = Convert.ToString(dt.Rows[0]["DropOut_Reason"]);
                }
                else
                {
                    cbDrp.Checked = false;
                    txtDtCl.Text = "";
                    txtRem.Text = "";
                }
                lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                lblDate.Text = "Last Modified Date : " + dt.Rows[0]["Formationdt"].ToString();
                txtDtFormation.Text = Convert.ToString(dt.Rows[0]["Formationdt"]);
                tbGrp.ActiveTabIndex = 1;
                StatusButton("Show");
                EnableControl(false);
            }
        }

        public string ViewGroupImage(string pImageName, string pGroupId)
        {
            string base64image = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = GroupURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    if (ValidUrlChk(ActNetPath[j] + pGroupId + "/" + pImageName))
                    {
                        imgByte = DownloadByteImage(ActNetPath[j] + pGroupId + "/" + pImageName);
                        base64image = Convert.ToBase64String(imgByte);                       
                        break;
                    }
                    else if (ValidUrlChk(ActNetPath[j] + pGroupId + "_" + pImageName))
                    {
                        imgByte = DownloadByteImage(ActNetPath[j] + pGroupId + "_" + pImageName);
                        base64image = Convert.ToBase64String(imgByte);                      
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return base64image;
        }

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

        #region URLExist
        public bool ValidUrlChk(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
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

        protected void btnShow1_Click(object sender, EventArgs e)
        {
            CGroup oGrp = new CGroup();
            DataTable dt = new DataTable();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            dt = oGrp.SearchGroup(vBrCode, txtSearch.Text, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
            gvGroup.DataSource = dt;
            gvGroup.DataBind();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vGrpID = "";
            vGrpID = Convert.ToString(e.CommandArgument);
            ViewState["GrpId"] = vGrpID;
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                foreach (GridViewRow gr in gvGroup.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
            }
            fillGroupDtl(vGrpID);
        }

        private string SaveMemberImages(FileUpload flup, string imageGroup, string imageName, string Mode, string isImageSaved, string ImagePath)
        {
            try
            {
                string folderPath = string.Format("{0}{1}", ImagePath, imageGroup/*, folderName*/);
                System.IO.Directory.CreateDirectory(folderPath);
                //  string filePath = string.Format("{0}/{1}.png", folderPath, imageName);
                string filePath = folderPath + "_" + imageName + ".png";
                Stream strm = flup.PostedFile.InputStream;
                var targetFile = filePath;

                if ((Mode == "Delete"))
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    isImageSaved = "N";
                }
                else
                {
                    if (flup.HasFile)
                    {
                        if (Mode == "Edit")
                        {
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                        }
                        File.WriteAllBytes(filePath, Convert.FromBase64String(getBase64String(flup)));
                        //ReduceImageSize(0.5, strm, targetFile); 
                        isImageSaved = "Y";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isImageSaved;
        }
        private string getBase64String(FileUpload flup)
        {
            string base64String = "";
            try
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(flup.PostedFile.InputStream))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return base64String;
        }

        private void ReduceImageSize(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }

        private Boolean ImageExist(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
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

        private byte[] ConvertFileToByteArray(HttpPostedFile postedFile)
        {
            using (Stream stream = postedFile.InputStream)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}