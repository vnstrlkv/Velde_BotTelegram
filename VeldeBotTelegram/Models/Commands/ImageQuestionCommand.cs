using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace VeldeBotTelegram.Models.Commands
{
    public class ImageQuestionCommand : Command
    {
        // public string
        public override bool Contains(Message message)
        {
            throw new NotImplementedException();
        }

        public override Task Execute(Message message, TelegramBotClient client, Client person)
        {
            throw new NotImplementedException();
        }

        public override string GetStageMessage(Client person)
        {
            throw new NotImplementedException();
        }

        public override int NextStage(Message message, Client person)
        {
            throw new NotImplementedException();
        }
    }
}
