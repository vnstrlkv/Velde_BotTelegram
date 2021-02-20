using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HPCorpBot.Models.Commands
{
    public abstract class Command
    {
     //   public abstract Question Quest { get; set; }

        public abstract Task Execute(Message message, TelegramBotClient client, Client person);

        public abstract bool Contains(Message message);

        public abstract int NextStage(Message message, Client person);

        public abstract string GetStageMessage(Client person);
    }

}
