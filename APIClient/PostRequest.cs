using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace APIClient
{
    public class PostRequest : HttpRequest
    {
        private Uri _address = null;
        private HttpWebRequest _request;

        public PostRequest(Uri address)
        {
            this._address = address;
            this._request = WebRequest.Create(address) as HttpWebRequest;
            this._request.Method = "POST";
            this._request.ContentType = "application/x-www-form-urlencoded";
        }

        public override string Fire()
        {
            return this.FireRequest(null);
        }

        public string Fire(List<KeyValuePair<string, string>> requestParams)
        {
            byte[] byteData = this.ConstructRequestParams(requestParams);
            return this.FireRequest(byteData);
        }

        private string FireRequest(byte[] byteData)
        {
            if (byteData != null && byteData.Length > 0)
            {
                this._request.ContentLength = byteData.Length;
                using (Stream postStream = this._request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, (Int32)this._request.ContentLength);
                }
            }

            try
            {
                using (HttpWebResponse response = this._request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    return reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private byte[] ConstructRequestParams(List<KeyValuePair<string, string>> requestParams)
        {
            if (requestParams.Count == 0)
                throw new ArgumentException("Request Parameter List is empty. If there are no parameters for your POST request try using Fire() method instead of Fire(List<KeyValuePair<string, string>> requestParams). ");

            StringBuilder data = new StringBuilder();
            string appendParam = "";
            foreach (var param in requestParams)
            {
                data.Append(appendParam);
                data.Append(param.Key.ToString()+ "=" + HttpUtility.UrlEncode(param.Value.ToString()));
                appendParam = "&";

            }
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            return byteData;
        }
    }
}
