using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPCorpBot.Models.Interfaces
{
    public interface IMessage
    {
        public string Message { get; set; }
        public int Stage { get; set; }       
        public int NextStage { get; set; }
        
    }
}
