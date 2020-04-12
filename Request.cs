using System.IO;
using System.Net;

namespace Covid19_UnderNotification
{
    public class Request
    {
         public static string DoRequest(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            StreamReader responseStream = new StreamReader(response.GetResponseStream());
            string responseText = responseStream.ReadToEnd();
            response.Close();

            return responseText;
        }
    }
}
