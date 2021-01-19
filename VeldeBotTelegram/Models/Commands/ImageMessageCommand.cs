using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace VeldeBotTelegram.Models.Commands
{
    public class ImageMessageCommand : Command
    {

        public ImageMessage ImageMessage { get; set; }
        
        public ImageMessageCommand (ImageMessage message)
        {
            ImageMessage = message;
        }
        

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            else return true;
        }

        public override async Task Execute(Message message, TelegramBotClient botClient, Client person)
        {
            try
            {
                await botClient.SendPhotoAsync(message.Chat.Id, photo: ImageMessage.UrlMessage, caption: ImageMessage.Message, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            }
            catch (Exception ex)
            {
                Exception t = ex;
            }
         //   await botClient.SendTextMessageAsync(message.Chat.Id, TextMessage.Message, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }

        public override string GetStageMessage(Client person)
        {
            return ImageMessage.Stage.ToString();
        }

        public override int NextStage(Message message, Client person)
        {
            return ImageMessage.NextStage;
        }
    }
}
