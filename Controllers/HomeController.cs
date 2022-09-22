using EviCRM.Portal.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Net;
using System.Xml.Serialization;
using System.Text;

namespace EviCRM.Portal.Controllers
{
    public class HomeController : Controller
    {
        private IWebHostEnvironment Environment;
        private readonly ILogger<HomeController> _logger;

        [HttpGet]
        public IActionResult auth_session(string padName, string sessionID, string lang, string rtl, string sessionToken)
        {
            foreach (var cookie_var in Request.Cookies)
            {
                Console.WriteLine("Name = " + cookie_var.Key + " ; Value = " + cookie_var.Value);
            }

            Response.Cookies.Append("padName", padName, new CookieOptions { Path = "/", HttpOnly = false, Secure = true, SameSite = SameSiteMode.None });
            Response.Cookies.Append("sessionID", sessionID, new CookieOptions { Path = "/", HttpOnly = false, Secure = true, SameSite = SameSiteMode.None });
            Response.Cookies.Append("lang", lang, new CookieOptions { Path = "/", HttpOnly = false, Secure = true, SameSite = SameSiteMode.None });
            Response.Cookies.Append("rtl", rtl, new CookieOptions { Path = "/", HttpOnly = false, Secure = true, SameSite = SameSiteMode.None });
            Response.Cookies.Append("sessionToken", sessionToken, new CookieOptions { Path = "/", HttpOnly = false, Secure = true, SameSite = SameSiteMode.None });

            return Redirect("https://evicrm.site/portal");
        }

        public IActionResult Index()
        {
            return View();
        }

        public HomeController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult GetTest()
        {
            return Redirect("https://evicrm.site/portal");
        }

        readonly IWebHostEnvironment hostingEnvironment;



        [HttpPost]
        public async Task<IActionResult> Upload(IList<IFormFile> files)
        {
            foreach (IFormFile source in files)
            {
                string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');

                filename = this.EnsureCorrectFilename(filename);
                filename = Path.GetFileNameWithoutExtension(this.SupressRussian(filename)) + Path.GetExtension(filename);
                filename = this.GetPathAndFilename(filename);

                using (FileStream output = System.IO.File.Create(Path.Combine(Environment.WebRootPath, "docs", filename)))
                    await source.CopyToAsync(output);

                using (FileStream output = System.IO.File.Create("/var/lib/onlyoffice/documentserver-example/files/0.0.0.0/" + filename))
                    await source.CopyToAsync(output);

                Program.GlobalFileName = filename;

                System.IO.File.WriteAllText(Path.Combine(Environment.WebRootPath, "configs", "current.txt"), filename);
            }

            return Redirect("https://evicrm.site/portal");
        }

        private string SupressRussian(string s)
        {
            StringBuilder ret = new StringBuilder();

            string[] rus = { "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й",
          "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц",
          "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я",
          "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й",
          "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц",
          "ч", "ш", "ш", "ъ", "ы", "ь", "э", "ю", "я" ,
          " ", "#","$","@","+","=","*","@","!","\"","^","&"};

            string[] eng = { "A", "B", "V", "G", "D", "E", "E", "ZH", "Z", "I", "Y",
          "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "KH", "TS",
          "CH", "SH", "SHCH", null, "Y", null, "E", "YU", "YA",
          "a", "b", "v", "g", "d", "e", "e", "zh", "z", "i", "y",
          "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts",
          "ch", "sh", "shch", null, "y", null, "e", "yu", "ya",
          "_","_","_","_","_","_","_","_","_","_","_","_"};

            bool step_completed = false;

            for (int j = 0; j < s.Length; j++)
            {
                for (int i = 0; i < rus.Length; i++)
                {
                    if (s.Substring(j, 1) == rus[i])
                    {
                        ret.Append(eng[i]);
                        step_completed = true; 
                    }
                }
                if (!step_completed)
                {
                    ret.Append(s[j]);
                }
                step_completed = false;

            }    
               
                   
            return ret.ToString();
        }

        private static readonly HttpClient client = new HttpClient();

        private string genXML(string meetingID, string doc_url, string filename, string filedata)
        {
            string xml = "";
            xml += "<modules>" + "\n";
            xml += "<module name=\"presentation\">" + "\n";
            xml += "<document current=\"true\" downloadable=\"true\" url=\"" + doc_url + "\" filename=\"" + filename + "\"/>" + "\n";
            xml += "<document removable=\"false\" name=\"" + filename + "\">" + "\n";
            xml += filedata + "\n";
            xml += "</document>" + "\n";
            xml += "</module>" + "\n";
            xml += "</modules>" + "\n";
            return xml;
        }

        public async Task<string> reqPOST(string meetingID, string doc_url, string filename, string filedata)
        {
            string answer = null;
            var response = await client.PostAsXmlAsync("https://evicrm.space/bigbluebutton/api/insertDocument?meetingID=" + meetingID + "&checksum=" + Crypto.getSha1("insertDocumentmeetingID=" + meetingID + Program.StrSalt),genXML(meetingID, doc_url, filename, filedata));
            var responseString = await response.Content.ReadAsStringAsync();
            return answer;
        }

        [HttpGet]
        public async Task<IActionResult> reqUpload()
        {
            string answer = null;
            string meetingID = "";
            answer = getMeetings();
            if (answer != null)
            {
                meetingID = answer;
            }

            string filename = System.IO.File.ReadAllText(Path.Combine(Environment.WebRootPath, "configs", "current.txt"));
            string doc_url = "https://evicrm.site/welcome/docs/" + filename;
            string doc_path = "/var/lib/onlyoffice/documentserver-example/files/0.0.0.0/" + filename;

            return Redirect("https://evicrm.site/portal/");
        }

        [HttpGet]
        public string getMeetings()
        {
            Random r = new Random(0);
            string StrParameters = "";
            string StrSHA1_CheckSum = Crypto.getSha1("getMeetings" + StrParameters + Program.StrSalt);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Program.StrServerAddress  + "getMeetings?" + "checksum=" + StrSHA1_CheckSum);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());

            string sr_str = sr.ReadToEnd();
            string meetingID = ParseMeetingID(sr_str);

            return meetingID;
        }

        string ParseMeetingID(string str)
        {
            string data = "";

            if (str.Contains("meetingID"))
            {
                int first_pos = str.IndexOf("<meetingID>");
                int second_pos = str.IndexOf("/meetingID>");
                data = str.Substring(first_pos + 11, second_pos - first_pos - 12);
                return data;
            }
            return null;
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            if (filename.Contains("/"))
                filename = filename.Substring(filename.LastIndexOf("/") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            string filename_lcl = filename;
            if (System.IO.File.Exists(Path.Combine(Environment.WebRootPath,"docs", filename_lcl)))
            {
                string filename_without_ext = Path.GetFileNameWithoutExtension(filename_lcl);
                filename_without_ext += Guid.NewGuid().GetHashCode();
                filename_without_ext += Path.GetExtension(filename_lcl);

                filename_lcl = filename_without_ext;
            }

            return filename_lcl;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
