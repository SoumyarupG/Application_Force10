using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CentrumCF_MobService
{
    public class VkycAgent
    {
    }

    #region VkycAgent

    [DataContract]
    public class AgentResponse
    {
        //[DataMember]
        //public AgentSessionData session_data { get; set; }

        [DataMember]
        public bool IS_VIDO_UPLOADED { get; set; }
        [DataMember]
        public string agent_assignment_time { get; set; }
        [DataMember]
        public string agent_id { get; set; }
        [DataMember]
        public string otp { get; set; }
        [DataMember]
        public string otp_attempts { get; set; }
        [DataMember]
        public string agent_region { get; set; }
        [DataMember]
        public string agent_screen_url { get; set; }
        [DataMember]
        public string agent_tl { get; set; }
        [DataMember]
        public string agent_video_url { get; set; }

        [DataMember]
        public string app_version_number { get; set; }
        [DataMember]
        public string audit_end_time { get; set; }
        [DataMember]
        public string audit_id { get; set; }
        [DataMember]
        public string audit_init_time { get; set; }
        [DataMember]
        public string audit_lock { get; set; }
        [DataMember]
        public string audit_result { get; set; }
        [DataMember]
        public string auditor_fdbk_code { get; set; }
        [DataMember]
        public string auditor_feedback { get; set; }
        [DataMember]
        public string auditor_name { get; set; }

        [DataMember]
        public string captured_images { get; set; }
        [DataMember]
        public string cbs_summary { get; set; }
        [DataMember]
        public string client_name { get; set; }
        [DataMember]
        public string customer_IP { get; set; }
        [DataMember]
        public string end_time { get; set; }
        [DataMember]
        public string extras { get; set; }
        [DataMember]
        public string feedback { get; set; }
        [DataMember]
        public string link_id { get; set; }
        [DataMember]
        public string location { get; set; }
        [DataMember]
        public string number_of_videos_uploaded { get; set; }
        [DataMember]
        public string pan_request_time { get; set; }
        [DataMember]
        public string pan_url { get; set; }
        [DataMember]
        public string phone_number { get; set; }
        [DataMember]
        public string productCode { get; set; }
        [DataMember]
        public string queue { get; set; }
        [DataMember]
        public string queue_mode { get; set; }
        [DataMember]
        public string selfie_request_time { get; set; }
        [DataMember]
        public string selfie_url { get; set; }
        [DataMember]
        public string session_id { get; set; }
        [DataMember]
        public string session_status { get; set; }
        [DataMember]
        public string signature_request_time { get; set; }
        [DataMember]
        public string signature_url { get; set; }
        [DataMember]
        public string stage { get; set; }
        [DataMember]
        public bool stage1_valid { get; set; }
        [DataMember]
        public string stage_data { get; set; }
        [DataMember]
        public string start_time { get; set; }
        [DataMember]
        public AgentSummaryData summary_data { get; set; }
        [DataMember]
        public string summary_json_url { get; set; }
        [DataMember]
        public string summary_pdf_url { get; set; }
        [DataMember]
        public string user_ack { get; set; }
        [DataMember]
        public string user_id { get; set; }
        [DataMember]
        public string user_video_url { get; set; }
        [DataMember]
        public string vkyc_start_time { get; set; }
        [DataMember]
        public string zip_url { get; set; }
        [DataMember]
        public int Status { get; set; }
    }

    [DataContract]
    public class AgentSummaryData
    {
        [DataMember]
        public string agent_id { get; set; }
        [DataMember]
        public string client_name { get; set; }
        [DataMember]
        public List<AgentDoc> docs { get; set; }
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public double lng { get; set; }
        [DataMember]
        public List<AgentOverallSummary> overall_summary { get; set; }
        [DataMember]
        public List<AgentQna> qna { get; set; }
        [DataMember]
        public string session_id { get; set; }
        [DataMember]
        public string user_id { get; set; }
        [DataMember]
        public string journey_id { get; set; }
        [DataMember]
        public string journey_sequence { get; set; }
    }

    [DataContract]
    public class AgentDoc
    {
        [DataMember]
        public AgentDetails details { get; set; }
        [DataMember]
        public AgentFacematchScore facematch_score { get; set; }
        [DataMember]
        public string front_url { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public AgentValidator validator { get; set; }
        [DataMember]
        public string signature_url { get; set; }
    }

    [DataContract]
    public class AgentOverallSummary
    {
        [DataMember]
        public bool success { get; set; }
        [DataMember]
        public string title { get; set; }
    }

    [DataContract]
    public class AgentQna
    {
        [DataMember]
        public string a { get; set; }
        [DataMember]
        public string q { get; set; }
    }

    [DataContract]
    public class AgentDetails
    {
        [DataMember]
        public string dob { get; set; }
        [DataMember]
        public string fathers_name { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string pa_number { get; set; }
    }

    [DataContract]
    public class AgentFacematchScore
    {
        [DataMember]
        public int selfie_pan_match { get; set; }
    }

    [DataContract]
    public class AgentValidator
    {
        [DataMember]
        public AgentRawNsdl raw_nsdl { get; set; }
        [DataMember]
        public bool success { get; set; }
        [DataMember]
        public string type { get; set; }
    }

    [DataContract]
    public class AgentRawNsdl
    {
        [DataMember]
        public AgentNSDLResponse NSDLResponse { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string pan_number { get; set; }
        [DataMember]
        public string response_data { get; set; }
        [DataMember]
        public string response_data1 { get; set; }
        [DataMember]
        public bool success { get; set; }
        [DataMember]
        public string verified { get; set; }
    }

    [DataContract]
    public class AgentNSDLResponse
    {
        [DataMember]
        public string dob { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string panNumber { get; set; }
        [DataMember]
        public string valid { get; set; }
    }


    #endregion
}