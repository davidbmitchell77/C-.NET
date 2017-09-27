using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.Text;

namespace RestService
{
    public class RestServiceImpl : IRestServiceImpl
    {
        #region IRestService Members
        /*
        public string XMLData(string id)
        {
            return "You requested product " + id;
        }

        public string JSONData(string id)
        {
            return "You requested product " + id;
        }
        */

        public PCHEmailAPIResponse Pulse()
        {
            PCHEmailAPIResponse response = new PCHEmailAPIResponse();
            response.responseMessage = "Success";
            response.responseCode = 1;
            return response;
        }

        public PCHEmailAPIResponse Preview(PCHEmailAPI request)
        {
            PCHEmailAPIResponse response = new PCHEmailAPIResponse();
            response.responseCode = 1;
            response.responseMessage = "Success";
            return response;
        }

        public PCHEmailAPIResponse Send(PCHEmailAPI request)
        {
            PCHEmailAPIResponse response = new PCHEmailAPIResponse();
            response.responseCode = 1;
            response.responseMessage = "Success";
            return response;
        }

        #endregion
    }
}
