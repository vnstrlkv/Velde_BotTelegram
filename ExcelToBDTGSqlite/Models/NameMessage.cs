using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeldeBotTelegram.Models.Interfaces;
namespace VeldeBotTelegram.Models
{
    public class NameMessage : IMessage
    {
        public string Message { get; set; }
        public int Stage { get ; set; }
        public string Name { get; set; }
        public int NextStage { get; set; }
    }
}
