using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeldeBotTelegram.Models
{
    public static class AppSettings
    {
        static List<string> config = DBHelper.GetAppConfig();
      //   public static string Url { get; set; } = config[2];
        public static string Url { get; set; } = "https://ea7d5eb30e11.ngrok.io/{0}";
       public static string Name { get; set; } =config[0];
        public static string Key { get; set; } = config[1];
    }
}
