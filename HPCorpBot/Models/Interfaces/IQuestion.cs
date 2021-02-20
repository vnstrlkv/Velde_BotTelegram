using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPCorpBot.Models.Interfaces
{
    public interface IQuestion : IMessage
    {
       public new int Stage { get; set; } 
       public int TimeToAnswer { get; set; }       
       public string RightAnswer { get; set; }    
       public new int NextStage { get; set; }
       public int Score { get; set; }
    }
}
