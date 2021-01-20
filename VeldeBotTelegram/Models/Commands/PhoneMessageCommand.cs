using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace VeldeBotTelegram.Models.Commands
{
    public class PhoneMessageCommand : Command
    {
        PhoneMessage PhoneMessage = new PhoneMessage();

        public PhoneMessageCommand(PhoneMessage phone)
        {
            PhoneMessage = phone;
        }
        public override bool Contains(Message message)
        {
            throw new NotImplementedException();
        }

        public override async Task Execute(Message message, TelegramBotClient botClient, Client person)
        {
            try
            {
                var button = new Telegram.Bot.Types.ReplyMarkups.KeyboardButton();
                button.RequestContact = true;
                button.Text = "Поделиться контактом";
                var keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(button);
                keyboard.ResizeKeyboard = true;
                keyboard.OneTimeKeyboard = true;

                await botClient.SendTextMessageAsync(message.Chat.Id, PhoneMessage.Message, replyMarkup: keyboard, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            }
            catch (Exception ex)
            {
                Bot.MyLogger(ex.Message);
                Bot.MyLogger(ex.StackTrace);
            }
        }

        public override string GetStageMessage(Client person)
        {
            throw new NotImplementedException();
        }

        public override int NextStage(Message message, Client person)
        {
            return PhoneMessage.NextStage;
        }
    }
}
