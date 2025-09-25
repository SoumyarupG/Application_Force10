using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Data;

namespace CENTRUM.WebPages.Private.BCOperation
{
    public partial class BC_UpdateDuringUpload : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
            string vAppId = "";
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    vAppId = Request.QueryString["id"].ToString();
                    LoadDetails(vAppId);
                }
                else
                {
                    ViewState["AppId"] = null;
                    LoadDetails("");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSrch"></param>
        private void LoadDetails(string vAppId)
        {
            DataTable dt = null;
            CBCCgt oBC = null;
            ViewState["AppId"] = vAppId;
            try
            {
                oBC = new CBCCgt();
                dt = oBC.BC_Get_DetailsDuringUpload(vAppId);
                if (dt.Rows.Count > 0)
                {
                    lblNameOFClient.Text = Convert.ToString(dt.Rows[0]["MEM_NAME"]).Trim();
                    lblDOB.Text = Convert.ToString(dt.Rows[0]["DOB"]).Trim();
                    lblAge.Text = Convert.ToString(dt.Rows[0]["Age"]).Trim();
                    lblURN.Text = Convert.ToString(dt.Rows[0]["URNID"]).Trim();
                    lblSPName.Text = Convert.ToString(dt.Rows[0]["SpouseName"]).Trim();

                    lblPhotoIDtype.Text = Convert.ToString(dt.Rows[0]["PhotoIdType"]).Trim();
                    lblPhotoIDNo.Text = Convert.ToString(dt.Rows[0]["PhotoIdNo"]).Trim();
                    lblAddType.Text = Convert.ToString(dt.Rows[0]["AddIdType"]).Trim();
                    lblAddIDNo.Text = Convert.ToString(dt.Rows[0]["AddIdNo"]).Trim();
                   
                    lblAdd2.Text = Convert.ToString(dt.Rows[0]["MAddress"]).Trim();
                    lblLnAmount.Text = Convert.ToString(dt.Rows[0]["LoanAmt"]).Trim();
                    lblBlock.Text = Convert.ToString(dt.Rows[0]["BlockName"]).Trim();
                    lblDistrict.Text = Convert.ToString(dt.Rows[0]["DistrictName"]).Trim();
                    lblState.Text = Convert.ToString(dt.Rows[0]["StateName"]).Trim();
                    lblPin.Text = Convert.ToString(dt.Rows[0]["Pin"]).Trim();

                    txtMob.Text = Convert.ToString(dt.Rows[0]["ContactNo"]).Trim();
                    txtAnnualInc.Text = Convert.ToString(dt.Rows[0]["Income"]).Trim();

                    txtCaste.Text = dt.Rows[0]["Caste"].ToString();
                    txtOccu.Text = dt.Rows[0]["Occupation"].ToString();
                    txtquali.Text = dt.Rows[0]["Qualification"].ToString();

                    txtRlgn.Text = dt.Rows[0]["Religion"].ToString();
                }
                
            }
            finally
            {
                dt = null;
                oBC = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
           
        }

       
    }
}