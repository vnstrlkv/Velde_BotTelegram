using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeldeBotTelegram.Models.Interfaces;
namespace VeldeBotTelegram.Models
{
    public class TextMessage : IMessage
    {
        public int Stage { get; set; }
        public string Message { get; set; }
        public int NextStage { get; set; }

    }
}
