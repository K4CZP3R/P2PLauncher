using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Modules
{
    class WebRequestModule
    {
        public static string MakeGetRequest(string url)
        {
            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    return stream.ReadToEnd();
                }
            }
        }
    }
}
