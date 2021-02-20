using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;

namespace VeldeBotTelegram.Models.Commands
{
    public class QuestionCommand : Command
    {
        public QuestionCommand(Question q)
        {
 
            Quest=q;
        }
        public Question Quest { get; set; }
        //   Client person = new Client();

        public override string GetStageMessage(Client person)
        {

            return Quest.Stage.ToString();
           // throw new System.NotImplementedException();
        }
        public override int NextStage(Message message, Client person)
        {

            //throw new System.NotImplementedException();

            var command = from t in Quest.Answers
                          where t.RightAnswer.Trim() == message.Text.Trim()
                          select t;

            return command.ToList()[0].NextStage;
        

        }
        public override bool Contains(Message message)
        {
            var q = Quest;
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;
            var command = from t in Quest.Answers
                          where t.RightAnswer.Trim() == message.Text.Trim()
                          select t;
            if (command.ToList().Count != 0)
                return message.Text.Trim().Contains(command.ToList()[0].RightAnswer.Trim());
            else return false;
        }

        public override async Task Execute(Message message, TelegramBotClient botClient, Client client)
        {
            var chatId = message.Chat.Id;
            // await botClient.SendTextMessageAsync(chatId, "", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            List<List<KeyboardButton>> buttonsList = new List<List<KeyboardButton>>() ;
            List<KeyboardButton> buttons = new List<KeyboardButton>();
            // int i = 0;
            foreach (Answer an in Quest.Answers)
            {
                buttons = new List<KeyboardButton>();
                var button = new Telegram.Bot.Types.ReplyMarkups.KeyboardButton();
                //button.RequestContact = true;
                button.Text = an.RightAnswer;
                buttons.Add(button);
                buttonsList.Add(buttons);
            }
            
            var keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(buttonsList);
            keyboard.ResizeKeyboard = true;
            keyboard.OneTimeKeyboard = false;
           
            await botClient.SendTextMessageAsync(message.Chat.Id, Quest.Message, replyMarkup: keyboard, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        //    await botClient.SendTextMessageAsync(chatId, "Спс", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }

    }
    /*
    public class CommandsChat : Command
    {
        public Question quest;
         public override string Name => quest.Answers
    }
    */
}
