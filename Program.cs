using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EviCRM.Portal
{
    public class Program
    {
        public static string webhost_url = "$webhost_url";
        public static string GlobalFileName = "111";
        public static string StrSalt = "$str_salte;
        public static string StrServerIPAddress = "$store_ip";
        public static string StrServerAddress = "$bbb_resource";

        public static string sftp_url_server = "$sftp_url";
        public static int sftp_url_port = 9021;
        public static string sftp_user = "$sftp_user";
        public static string sftp_password = "$sftp_password";

        public static Controllers.StoreController sc = new Controllers.StoreController();

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:7000", "https://localhost:7001");
                });
    }
}
