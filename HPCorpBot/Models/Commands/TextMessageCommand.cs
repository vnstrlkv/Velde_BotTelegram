using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;


namespace HPCorpBot.Models.Commands
{
    public class TextMessageCommand : Command
    {
        public TextMessage TextMessage { get; set; }

        public TextMessageCommand(TextMessage text)
        {
            TextMessage = text;
        }
        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;
            
            else return true;
        }

        public override async Task Execute(Message message, TelegramBotClient botClient, Client person)
        {
            //    var chatId = message.Chat.Id;
            // await botClient.SendTextMessageAsync(chatId, "", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);

            //тест
         //   var keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(buttons);
        ///    keyboard.ResizeKeyboard = true;
        //    keyboard.OneTimeKeyboard = true;
          
            //тест
            await botClient.SendTextMessageAsync(message.Chat.Id, TextMessage.Message, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

            //    await botClient.SendTextMessageAsync(chatId, "Спс", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }

        public override string GetStageMessage(Client person)
        {
            return TextMessage.Stage.ToString();
        }

        public override int NextStage(Message message, Client person)
        {
            return TextMessage.NextStage;
        }
    }
}
