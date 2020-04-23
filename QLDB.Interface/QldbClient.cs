using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace QLDB.Interface
{
    public class QldbClient
    {
        public static string QldbApiUri = "https://localhost:44304/api/qldb";
        public static string QldbRead(QldbRequestModel QldbRequest)
        {
            WebRequest Request = BuildRequest(QldbRequest, $"{QldbApiUri}/read");
            string ResponseJson = GetResponseString(Request);

            return ResponseJson;
        }
        public static string QldbWrite(QldbRequestModel QldbRequest)
        {
            WebRequest Request = BuildRequest(QldbRequest, $"{QldbApiUri}/write");
            string ResponseJson = GetResponseString(Request);

            return ResponseJson;
        }
        static string GetResponseString(WebRequest Request)
        {
            WebResponse response;
            response = Request.GetResponse();
            using (StreamReader Sr = new StreamReader(response.GetResponseStream()))
                return Sr.ReadToEnd();
        }
        static WebRequest BuildRequest(QldbRequestModel QldbRequest, string Uri)
        {
            WebRequest Request;
            Request = WebRequest.Create(Uri);
            string PostData = JsonConvert.SerializeObject(QldbRequest);
            byte[] Data = Encoding.UTF8.GetBytes(PostData);
            Request.Method = "POST";
            Request.ContentType = "application/json";
            Request.ContentLength = Data.Length;
            using (Stream stream = Request.GetRequestStream())
                stream.Write(Data, 0, Data.Length);
            return Request;
        }

    }
}
