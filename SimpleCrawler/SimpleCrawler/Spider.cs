using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace SimpleCrawler
{
    public class Spider
    {
        //url: target webpage address
        //RegexString:  set of regex expressions
        //MultiResults:  bool return multiple match result.  false return first result
        public List<string> GetWebPageContent(string url, string[] RegexString, bool MultiResults)
        {
            //Create request object
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //Request setting
            request.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.22 (KHTML, like Gecko) Chrome/25.0.1364.160 Safari/537.22";
            request.Method = "GET";
            request.KeepAlive = true;
            request.AllowAutoRedirect = true;
            request.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
            request.Headers.Add("Accept-Language", "en-US, en; q=0.8, zh-Hans-CN; q=0.5, zh-Hans; q=0.3");
            request.Timeout = 30000;
            request.CookieContainer = new CookieContainer();
            request.Headers.Set("Pragma", "no-cache");
            //Create response object
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream stream = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;
            StreamReader srd = new StreamReader(stream, encoding);
            string strResult = srd.ReadToEnd();
            //Match process
            return GetMatchedValue(RegexString, strResult, MultiResults);
        }

        public List<string> GetMatchedValue(string[] RegexString, string RemoteStr, bool MultiResults)
        {
            MatchCollection r;
            List<string> results = new List<string>();
            //Traversing all regex expressions
            for (int i = 0; i < RegexString.Length; i++)
            {
                r = GetRegValue(RegexString[i], RemoteStr);
                if (r.Count == 0)
                {
                    return null;
                }
                if (MultiResults)
                {
                    foreach (var re in r)
                    {
                        results.Add(re.ToString());
                    }
                }
                else
                {
                    results.Add(r[0].ToString());
                }
            }
            return results;
        }

        //Get all matched contents from target webpage
        public MatchCollection GetRegValue(string RegexString, string RemoteStr)
        {
            var r = new Regex(RegexString, RegexOptions.Multiline);
            MatchCollection matches = r.Matches(RemoteStr);
            return matches;
        }
    }
}
