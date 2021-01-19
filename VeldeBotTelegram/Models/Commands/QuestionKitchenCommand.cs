using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace VeldeBotTelegram.Models.Commands
{
    public class QuestionKitchenCommand : QuestionCommand
    {
        public QuestionKitchen qk { get; set; }
        public QuestionKitchenCommand(Question q): base (q)
        {
            Quest = q;
        }

      


    }
}
