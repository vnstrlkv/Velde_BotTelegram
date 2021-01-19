using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace VeldeBotTelegram.Models.Commands
{
    public class NameMessageCommand : Command
    {
        NameMessage NameMessage = new NameMessage();
        public override bool Contains(Message message)
        {
            throw new NotImplementedException();
        }

        public override async Task Execute(Message message, TelegramBotClient botClient, Client person)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, NameMessage.Message, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }

        public override string GetStageMessage(Client person)
        {
            throw new NotImplementedException();
        }

        public override int NextStage(Message message, Client person)
        {
            return NameMessage.NextStage;
        }
    }
}
