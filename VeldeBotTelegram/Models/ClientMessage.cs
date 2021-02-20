using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeldeBotTelegram.Models
{
    public class ClientMessage
    {
     public DateTime Date { get; set; }
     public string Question { get; set; }
     public string Answer { get; set; }

     public override string ToString()
        {
            string s="";
            s += Date.ToString();
            if (Answer != null && Answer !="")
                s += "\n" + "Клиент: " + Answer;
            if (Question != null)
                s += "\n" + "БОТ: " + Question + "\n";
            return s;
        }
    }
}
