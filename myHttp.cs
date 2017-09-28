using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace myHttp
{
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

    public class AAA
    {
        public string AAAID;
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

    public class AAAEmailAPI
    {
        public string EmailID;
        public string PersonNumber;
        public string PlatformName;
        public string PartyId;
        public String TemplateID;
        public string SubjectLine;
        public string ToEmailAddress;
        public List<string> SendACopyList;
        public AssociateProfileNode AssociatePersonalization;
        public List<AgendaItem> AgendaItems;
        public string Appointment;
        public string Meeting;
        public List<Brochure> BrochureList;
        public List<AdditionalData> AdditionalData;
    }

    public class AAAEmailAPIResponse
    {
        public int responseCode;
        public string responseMessage;
    }

    public class AccessTokenRequest
    {
        public string clientId;
        public string clientSecret;
    }

    public class AccessTokenResponse
    {
        public string message;
        public string documentaion;
        public string errorcode;
    }

    class Program
    {
        static HttpClient auth = new HttpClient();
        static HttpClient restAPI = new HttpClient();

        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            // POST access token request
            auth.BaseAddress = new Uri(Properties.Settings.Default.AuthURITest);
            auth.DefaultRequestHeaders.Clear();
            auth.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            AccessTokenRequest tokenReq = new AccessTokenRequest();
            tokenReq.clientId = Properties.Settings.Default.ClientId;
            tokenReq.clientSecret = Properties.Settings.Default.ClientSecret;
            AccessTokenResponse tokenResp = new AccessTokenResponse();
            tokenResp = await PostTokenRequest("v1/requestToken", tokenReq);
            Console.Write("Auth: ");
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(AccessTokenResponse));
            ser.WriteObject(ms, tokenResp);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            Console.WriteLine(sr.ReadToEnd());
            Console.WriteLine();

            // set REST API URL and http request headers
            restAPI.BaseAddress = new Uri(Properties.Settings.Default.BaseURI);
            restAPI.DefaultRequestHeaders.Clear();
            restAPI.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // check REST API service is available
            AAAEmailAPIResponse AAAResp = new AAAEmailAPIResponse();
            AAAResp = await GetPulse("pulse");
            Console.Write("Pulse: {");
            Console.Write("\"{0}\":{1},\"{2}\":\"{3}\"", "responseCode", AAAResp.responseCode, "responseMessage", AAAResp.responseMessage);
            Console.WriteLine("}\n");
            
            // POST email preview request
            AAAEmailAPI AAAEmailPreview = sampleJSONMessage();
            AAAEmailAPI AAAEmailPreviewResp = new AAAEmailAPI();
            AAAEmailPreviewResp = await PostEmailPreview("preview", AAAEmailPreview);
            Console.Write("Preview: ");
            MemoryStream ms1 = new MemoryStream();
            DataContractJsonSerializer ser1 = new DataContractJsonSerializer(typeof(AAAEmailAPI));
            ser1.WriteObject(ms1, AAAEmailPreviewResp);
            ms1.Position = 0;
            StreamReader sr1 = new StreamReader(ms1);
            Console.WriteLine(sr1.ReadToEnd());
            Console.WriteLine();

            // POST email send request
            AAAEmailAPI AAAEmail = sampleJSONMessage();
            AAAResp = await PostEmailSend("send", AAAEmail);
            Console.Write("Send: {");
            Console.Write("\"{0}\":{1},\"{2}\":\"{3}\"", "responseCode", AAAResp.responseCode, "responseMessage", AAAResp.responseMessage);
            Console.WriteLine("}");

            Console.ReadLine();
        }

        static async Task<AAAEmailAPIResponse> GetPulse(string path)
        {
            AAAEmailAPIResponse AAAResp = null;

            HttpResponseMessage httpResp = await restAPI.GetAsync(path);
            if (httpResp.IsSuccessStatusCode)
            {
                AAAResp = await httpResp.Content.ReadAsAsync<AAAEmailAPIResponse>();
            }

            return AAAResp;
        }

        static async Task<AccessTokenResponse> PostTokenRequest(string path, AccessTokenRequest tokenRequest)
        {
            AccessTokenResponse tokenResponse = new AccessTokenResponse();

            HttpResponseMessage httpResp = await auth.PostAsJsonAsync(path, tokenRequest);
            tokenResponse = await httpResp.Content.ReadAsAsync<AccessTokenResponse>();

            return tokenResponse;
        }

        static async Task<AAAEmailAPI> PostEmailPreview(string path, AAAEmailAPI AAAEmail)
        {
            AAAEmailAPI AAAResp = new AAAEmailAPI();

            HttpResponseMessage httpResp = await restAPI.PostAsJsonAsync(path, AAAEmail);
            if (httpResp.IsSuccessStatusCode)
            {
                AAAResp = await httpResp.Content.ReadAsAsync<AAAEmailAPI>();
            }

            return AAAResp;
        }

        static async Task<AAAEmailAPIResponse> PostEmailSend(string path, AAAEmailAPI AAAEmail)
        {
            AAAEmailAPIResponse AAAResp = new AAAEmailAPIResponse();
    
            HttpResponseMessage httpResp = await restAPI.PostAsJsonAsync(path, AAAEmail);
            if (httpResp.IsSuccessStatusCode)
            {
               AAAResp = await httpResp.Content.ReadAsAsync<AAAEmailAPIResponse>();
            }

            return AAAResp;
        }

        static AAAEmailAPI sampleJSONMessage()
        {
            AAAEmailAPI jsonBody = new AAAEmailAPI();

            jsonBody.EmailID = System.Convert.ToString(System.Guid.NewGuid()).ToUpper();
            jsonBody.PersonNumber = "217005555";
            jsonBody.PlatformName = "iPad";
            jsonBody.PartyId = null;
            jsonBody.TemplateID = "myTemplateID";
            jsonBody.SubjectLine = "Dear Valued Customer";
            jsonBody.ToEmailAddress = "receipient.name@receipientemail.com";

            List<string> ccList = new List<string>();
            ccList.Add("gene.simmons@yahoo.com");
            ccList.Add("paul.stanley@att.net");
            ccList.Add("ace.frehley@outlook.com");
            ccList.Add("peter.criss@gmail.com");
            jsonBody.SendACopyList = ccList;

            AssociateProfileNode profileNode = new AssociateProfileNode();
            profileNode.FirstName = "Jane";
            profileNode.LastName = "Doe";
            profileNode.Designation = "CDFA™";
            profileNode.FunctionalTitle = "Jack of All Trades";
            profileNode.CorporateTitle = "Chief JSON Officer";
            profileNode.Company = "Merrill Edge";
            profileNode.PhotoURL = "https://snapfish.com/12345/myPhoto.jpg";
            profileNode.eMail = "some.body@bankofamerica.com";
            profileNode.Address1 = "7711 Rosemead Blvd";
            profileNode.Address2 = "Suite 81";
            profileNode.City = "Denver";
            profileNode.State = "CO";
            profileNode.Zip = "81301";
            profileNode.PreferredPhone = "8055552222";
            profileNode.OptionalExtension = "1781";
            profileNode.BBA_URL = "https://bba.url.com";
            profileNode.NMLS = "12345678";
            profileNode.SupervisoryAddr1 = "122 Supervisory Rd";
            profileNode.SupervisoryAddr2 = "2nd Floor";
            profileNode.SupervisoryCity = "Emeryville";
            profileNode.SupervisoryState = "CA";
            profileNode.SupervisoryZIP = "94101";
            profileNode.SupervisoryPhone = "4155555656";
            profileNode.CAID = "1234567";
            profileNode.BC_ZIP = "90210";
            profileNode.VanityURL = "https://jsonrocks.com";
            jsonBody.AssociatePersonalization = profileNode;

            List<AgendaItem> agendaItems = new List<AgendaItem>();
            for (int i = 0; i < 4; i++)
            {
                AgendaItem ai = new AgendaItem();
                ai.Item = "Agenda Item " + (i + 1);
                agendaItems.Add(ai);
            }
            jsonBody.AgendaItems = agendaItems;

            jsonBody.Appointment = Convert.ToString(DateTime.Now);
            jsonBody.Meeting = Convert.ToString(DateTime.Today);

            List<Brochure> BrochureList = new List<Brochure>();
            Brochure Document1 = new Brochure();
            Document1.ID = System.Convert.ToString(System.Guid.NewGuid());
            Document1.AssetTitle = "English Brochure 1";
            Document1.ILXURL = "http://emailviewer-uat.informationlogix.com/email?id=051ad4a6d0eb40ae92326085fbff2ef0";
            Document1.ILXURLnoMetrics = null;
            Document1.Branding = "BAC";
            Document1.PublishDate = Convert.ToString(DateTime.Today);
            Document1.IsPersonalized = true;
            Document1.Language = "English";
            BrochureList.Add(Document1);
            Brochure Document2 = new ();
            Document2.ID = System.Convert.ToString(System.Guid.NewGuid());
            Document2.AssetTitle = "Espanol Brochure 2";
            Document2.ILXURL = "http://emailviewer-uat.informationlogix.com/email?id=051bd4a6d0eb40ae92326085fbff2ef0";
            Document1.ILXURLnoMetrics = null;
            Document2.Branding = "Merril Edge";
            Document2.PublishDate = Convert.ToString(DateTime.Today);
            Document2.IsPersonalized = false;
            Document2.Language = "Spanish";
            BrochureList.Add(Document2);
            jsonBody.BrochureList = BrochureList;

            List<AdditionalData> adList = new List<AdditionalData>();
            for (int i = 0; i < 10; i++)
            {
                AdditionalData ad = new AdditionalData();
                ad.Key = "Key" + (i + 1);
                ad.Type = "Alphanumeric";
                ad.Value = "Value " + (i + 1);
                if (i == 0)
                {
                    ad.Type = "Alphanumeric";
                    ad.Value = "Miscellaneous Piece of Data";
                }
                else if (i == 1)
                {
                    ad.Type = "Boolean";
                    ad.Value = "false";
                }
                else if (i == 2)
                {
                    ad.Type = "Date";
                    ad.Value = Convert.ToString(DateTime.Today);
                }
                adList.Add(ad);
            }
            jsonBody.AdditionalData = adList;

            return jsonBody;
        }
    }
}
