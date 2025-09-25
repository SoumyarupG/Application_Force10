using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
//using CentrumCF_MobService;


namespace CentrumCF_MobService
{
    [ServiceContract]
    public interface ICentrumCF_MobService
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "VerifyCIBIL",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string VerifyCIBIL(string pLeadId, string pApplicanType);

        // VkycFinalApproval
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "VkycApproval",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string FinalApproval(AgentResponse vAgentRes);


        #region InsertBulkCollection
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "InsertBulkCollection",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Int32 InsertBulkCollection(string pAccDate, string pTblMst, string pTblDtl, string pFinYear, string pBankLedgr, string pCollXml,
        string pBrachCode, string pCreatedBy);
        #endregion


        #region ICICIDisbursement
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ICICBalanceFetch",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ICICBalanceFetchResponse ICICBalanceFetch(PostBalEnqReq vPostBalEnqReq);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ICICIDisbursement",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string ICICIDisbursement(string pXml, string pUserId);

        #endregion
      
    }
}
