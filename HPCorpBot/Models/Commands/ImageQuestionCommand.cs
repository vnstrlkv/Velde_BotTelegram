using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HPCorpBot.Models.Questions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HPCorpBot.Models.Commands
{
    public class ImageQuestionCommand:Command
    {
       public ImageQuestion Question { get; set; }
       public ImageQuestionCommand(ImageQuestion quest)
        {
            Question = quest;
        }
        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;
            else if (message.Text.ToUpper() == ("!" + Question.RightAnswer).ToUpper())
                return true;
            else return false;
        }

        public override async Task Execute(Message message, TelegramBotClient client, Client person)
        {
           await client.SendTextMessageAsync(message.Chat.Id, Question.URL, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }

        public override string GetStageMessage(Client person)
        {
            return Question.Stage.ToString();
        }

        public override int NextStage(Message message, Client person)
        {
            return Question.NextStage;
        }
    }
}
