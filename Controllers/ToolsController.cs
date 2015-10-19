using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace WebYoutubeAudio.Controllers
{
    public class ToolsController : Controller
    {
        //
        // GET: /GetAudioLinks/

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult GetAudioLinks() {
            return View();
        }

        [HttpPost]
        public ActionResult GetAudioLinks(string sss)
        {
            var yturl = Request.Form["yturl"];

            List<string> links = new List<string>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.youtube.com/watch?v=1ANSAh1Q_NE");
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://r1---sn-5aanugx5h-tuvl.googlevideo.com/videoplayback/" + qs);
            CookieContainer cookieJar = new CookieContainer();

            request.CookieContainer = cookieJar;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            int cookieCount = cookieJar.Count;

            WebHeaderCollection header = response.Headers;

            var encoding = System.Text.ASCIIEncoding.UTF8;
            string responseText;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                responseText = reader.ReadToEnd();
            }
            if (String.IsNullOrEmpty(responseText))
            {
                return View();
            }
            var strarr = responseText.Split(new string[] { @"type=audio" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < strarr.Length; i++)
            {
                var s = strarr[i];
                var istart = s.IndexOf("url=");
                var substr = s.Substring(istart + 4);
                var iend = substr.IndexOf(@"\u0026");
                if (iend < 0) continue;
                substr = substr.Substring(0, iend);
                var substrdecoded = HttpUtility.UrlDecode(substr);
                links.Add(substrdecoded);
                //Console.WriteLine(substrdecoded);
            }
            ViewBag.links = links;

            return View();
        }
    }
}
