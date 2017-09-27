using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RestService
{
    [ServiceContract]
    public interface IRestServiceImpl
    {
        /*(
        [OperationContract]
        [WebInvoke(Method = "GET",
          ResponseFormat = WebMessageFormat.Xml,
          BodyStyle = WebMessageBodyStyle.Wrapped,
          UriTemplate = "xml/{id}")]
        string XMLData(string id);

        [OperationContract]
        [WebInvoke(Method = "GET",
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Wrapped,
         UriTemplate = "json/{id}")]
        string JSONData(string id);
        */

        [OperationContract]
        [WebInvoke(Method = "GET",
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "pulse")]
        PCHEmailAPIResponse Pulse();

        [OperationContract]
        [WebInvoke(Method = "POST",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "preview")]
        PCHEmailAPIResponse Preview(PCHEmailAPI request);

        [OperationContract]
        [WebInvoke(Method = "POST",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "send")]
        PCHEmailAPIResponse Send(PCHEmailAPI request);
    }

    public class AssociateProfileNode
    {
        public string FirstName;
        public string LastName;
        public string Designation;
        public string FunctionalTitle;
        public string CorporateTitle;
        public string Company;
        public string PhotoURL;
        public string eMail;
        public string Address1;
        public string Address2;
        public string City;
        public string State;
        public string Zip;
        public string PreferredPhone;
        public string OptionalExtension;
        public string BBA_URL;
        public string NMLS;
        public string SupervisoryAddr1;
        public string SupervisoryAddr2;
        public string SupervisoryCity;
        public string SupervisoryState;
        public string SupervisoryZIP;
        public string SupervisoryPhone;
        public string CAID;
        public string BC_ZIP;
        public string VanityURL;
    }

    public class AgendaItem
    {
        public string Item;
    }

    public class PCH
    {
        public string PCHID;
        public string AssetTitle;
        public string ILXURL;
        public string ILXURLnoMetrics;
        public string Branding;
        public string PublishDate;
        public Boolean IsPersonalized;
        public string Language;
    }

    public class AdditionalData
    {
        public string Key;
        public string Type;
        public string Value;
    }

    [DataContract]
    public class PCHEmailAPI
    {
        [DataMember] public string EmailID { get; set; }
        [DataMember] public string PersonNumber { get; set; }
        [DataMember] public string PlatformName { get; set; }
        [DataMember] public string PartyId { get; set; }
        [DataMember] public String TemplateID { get; set; }
        [DataMember] public string SubjectLine { get; set; }
        [DataMember] public string ToEmailAddress { get; set; }
        [DataMember] public List<string> SendACopyList { get; set; }
        [DataMember] public AssociateProfileNode AssociatePersonalization;
        [DataMember] public List<AgendaItem> AgendaItems { get; set; }
        [DataMember] public string Appointment { get; set; }
        [DataMember] public string Meeting { get; set; }
        [DataMember] public List<PCH> PCHList { get; set; }
        [DataMember] public List<AdditionalData> AdditionalData { get; set; }
    }

    [DataContract]
    public class PCHEmailAPIResponse
    {
        [DataMember] public int responseCode { get; set; }
        [DataMember] public string responseMessage { get; set; }
    }
}
