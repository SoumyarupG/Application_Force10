using System.Runtime.Serialization;
using System.Data;
using System.Collections.Generic;


namespace CentrumCF_MobService
{
    #region Cibil
    [DataContract]
    public class CIBILAddress
    {
        [DataMember]
        public string index { get; set; }
        [DataMember]
        public string line1 { get; set; }
        [DataMember]
        public string line2 { get; set; }
        [DataMember]
        public string line3 { get; set; }
        [DataMember]
        public string stateCode { get; set; }
        [DataMember]
        public string pinCode { get; set; }
        [DataMember]
        public string addressCategory { get; set; }
        [DataMember]
        public string residenceCode { get; set; }
    }

    [DataContract]
    public class ConsumerInputSubject
    {
        [DataMember]
        public TuefHeader tuefHeader { get; set; }
        [DataMember]
        public List<Name> names { get; set; }
        [DataMember]
        public List<Id> ids { get; set; }
        [DataMember]
        public List<Telephone> telephones { get; set; }
        [DataMember]
        public List<CIBILAddress> addresses { get; set; }
        [DataMember]
        public List<EnquiryAccount> enquiryAccounts { get; set; }
    }

    [DataContract]
    public class EnquiryAccount
    {
        [DataMember]
        public string index { get; set; }
        [DataMember]
        public string accountNumber { get; set; }
    }

    [DataContract]
    public class Id
    {
        [DataMember]
        public string index { get; set; }
        [DataMember]
        public string idNumber { get; set; }
        [DataMember]
        public string idType { get; set; }
    }

    [DataContract]
    public class Name
    {
        [DataMember]
        public string index { get; set; }
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string middleName { get; set; }
        [DataMember]
        public string lastName { get; set; }
        [DataMember]
        public string birthDate { get; set; }
        [DataMember]
        public string gender { get; set; }
    }

    [DataContract]
    public class Root
    {
        [DataMember]
        public string serviceCode { get; set; }
        [DataMember]
        public string monitoringDate { get; set; }
        [DataMember]
        public ConsumerInputSubject consumerInputSubject { get; set; }
    }

    [DataContract]
    public class Telephone
    {
        [DataMember]
        public string index { get; set; }
        [DataMember]
        public string telephoneNumber { get; set; }
        [DataMember]
        public string telephoneType { get; set; }
    }

    [DataContract]
    public class TuefHeader
    {
        [DataMember]
        public string headerType { get; set; }
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public string memberRefNo { get; set; }
        [DataMember]
        public string gstStateCode { get; set; }
        [DataMember]
        public string enquiryMemberUserId { get; set; }
        [DataMember]
        public string enquiryPassword { get; set; }
        [DataMember]
        public string enquiryPurpose { get; set; }
        [DataMember]
        public string enquiryAmount { get; set; }
        [DataMember]
        public string scoreType { get; set; }
        [DataMember]
        public string outputFormat { get; set; }
        [DataMember]
        public string responseSize { get; set; }
        [DataMember]
        public string ioMedia { get; set; }
        [DataMember]
        public string authenticationMethod { get; set; }
    }
    [DataContract]
    public class CibilResponse
    {
        [DataMember]
        public string success { get; set; }
        [DataMember]
        public string score { get; set; }
        [DataMember]
        public string ReportOrderNo { get; set; }

        public CibilResponse(string success, string score, string ReportOrderNo)
        {
            this.success = success;
            this.score = score;
            this.ReportOrderNo = ReportOrderNo;
        }
    }
    #endregion    

    [DataContract]
    public class PostBankTransaction
    {
        [DataMember]
        public string AGGRID { get; set; }
        [DataMember]
        public string AGGRNAME { get; set; }
        [DataMember]
        public string CORPID { get; set; }
        [DataMember]
        public string USERID { get; set; }
        [DataMember]
        public string URN { get; set; }
        [DataMember]
        public string DEBITACC { get; set; }
        [DataMember]
        public string CREDITACC { get; set; }
        [DataMember]
        public string IFSC { get; set; }
        [DataMember]
        public string AMOUNT { get; set; }
        [DataMember]
        public string CURRENCY { get; set; }
        [DataMember]
        public string TXNTYPE { get; set; }
        [DataMember]
        public string PAYEENAME { get; set; }
        [DataMember]
        public string UNIQUEID { get; set; }
        [DataMember]
        public string REMARKS { get; set; }
    }

    [DataContract]
    public class PostBalEnqReq
    {
        [DataMember]
        public string AGGRID { get; set; }
        [DataMember]
        public string CORPID { get; set; }
        [DataMember]
        public string USERID { get; set; }
        [DataMember]
        public string URN { get; set; }
        [DataMember]
        public string ACCOUNTNO { get; set; }
    }
}