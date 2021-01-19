using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeldeBotTelegram.Models
{
    public static class AppSettings
    {
        static List<string> config ;
      //   public static string Url { get; set; } =;
        public static string Url { get; set; } = "https://ea7d5eb30e11.ngrok.io/{0}";
       public static string Name { get; set; } 
        public static string Key { get; set; } 


       public static void LoadConfig(int ID)
        {
            config = DBHelper.GetAppConfig(ID);
            Name = config[0];
            Key = config[1];
          //  Url = config[2];
        }
    }
}
