using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
namespace APIClient
{
    public class GetRequest : HttpRequest
    {
        private Uri _address = null;
        private HttpWebRequest _request;
        public GetRequest(Uri address)
        {
            this._address = address;
            this._request = WebRequest.Create(address) as HttpWebRequest;
        }

        public override string Fire()
        {
            using (HttpWebResponse response = this._request.GetResponse() as HttpWebResponse)
            {                
                StreamReader reader = new StreamReader(response.GetResponseStream());
                return reader.ReadToEnd();
            }  
        }
    }
}
