using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HPCorpBot.Models.Interfaces;
namespace HPCorpBot.Models
{
    public class TextMessage : IMessage
    {
        public int Stage { get; set; }
        public string Message { get; set; }        
        public int NextStage { get ; set ; }
    }
}
