using System.Runtime.Serialization;
using System.Data;
using System.Collections.Generic;

namespace CentrumCF_MobService
{
    [DataContract]
    public class ICICBalanceFetchResponse
    {
        [DataMember]
        public string AGGR_ID { get; set; }
        [DataMember]
        public string CORP_ID { get; set; }
        [DataMember]
        public string USER_ID { get; set; }
        [DataMember]
        public string URN { get; set; }
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string ACCOUNTNO { get; set; }
        [DataMember]
        public string DATE { get; set; }
        [DataMember]
        public string EFFECTIVEBAL { get; set; }
        [DataMember]
        public string CURRENCY { get; set; }
        [DataMember]
        public string MESSAGE { get; set; }

        public ICICBalanceFetchResponse(string AGGR_ID, string CORP_ID, string USER_ID, string URN, string Response, string ACCOUNTNO, string DATE, string EFFECTIVEBAL, string CURRENCY, string MESSAGE)
        {
            this.AGGR_ID = AGGR_ID;
            this.CORP_ID = CORP_ID;
            this.USER_ID = USER_ID;
            this.URN = URN;
            this.Response = Response;
            this.ACCOUNTNO = ACCOUNTNO;
            this.DATE = DATE;
            this.EFFECTIVEBAL = EFFECTIVEBAL;
            this.CURRENCY = CURRENCY;
            this.MESSAGE = MESSAGE;
        }

    }

    [DataContract]
    public class ICICBankTransactionResponse
    {
        [DataMember]
        public string AGGR_ID { get; set; }
        [DataMember]
        public string AGGR_NAME { get; set; }
        [DataMember]
        public string CORP_ID { get; set; }
        [DataMember]
        public string USER_ID { get; set; }
        [DataMember]
        public string URN { get; set; }
        [DataMember]
        public string UNIQUEID { get; set; }
        [DataMember]
        public string UTRNUMBER { get; set; }
        [DataMember]
        public string REQID { get; set; }
        [DataMember]
        public string STATUS { get; set; }
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string message { get; set; }

        public ICICBankTransactionResponse(string AGGR_ID, string AGGR_NAME, string CORP_ID, string USER_ID, string URN, string UNIQUEID, string UTRNUMBER
            , string REQID, string STATUS, string Response, string message)
        {
            this.AGGR_ID = AGGR_ID;
            this.AGGR_NAME = AGGR_NAME;
            this.CORP_ID = CORP_ID;
            this.USER_ID = USER_ID;
            this.URN = URN;
            this.UNIQUEID = UNIQUEID;
            this.UTRNUMBER = UTRNUMBER;
            this.REQID = REQID;
            this.STATUS = STATUS;
            this.Response = Response;
            this.message = message;
        }

    }
}