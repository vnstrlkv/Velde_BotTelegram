using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeldeBotTelegram.Models
{
    public static class AppSettings
    {
        static List<string> config ;
         public static string Url { get; set; }     
       public static string Name { get; set; } 
        public static string Key { get; set; } 


       public static void LoadBotConfig(int ID)
        {
            config = DBHelper.GetBOTConfig(ID);
            Name = config[0];
            Key = config[1];
            Url = config[2];
        }
    }
}
