using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VeldeBotTelegram.Models;
namespace VeldeBotTelegram.Models.Commands
{
    public class InlineImageGalleryCommand : Command
    {
        InlineImageGallery inlineImageGallery { get; set; }
        
        public InlineImageGalleryCommand(InlineImageGallery inline)
        {
            inlineImageGallery = inline;
        }
        public override bool Contains(Message message)
        {
            throw new NotImplementedException();
        }

        public override async Task Execute(Message message, TelegramBotClient botClient, Client person)
        {
           
           
            InlineKeyboardMarkup inlineKeyboardMarkup = GetImageKeyboard();

            List<InputMediaPhoto> album = new List<InputMediaPhoto>();
            foreach(var photo in inlineImageGallery.ImageGalleries)
            {
                InputMediaPhoto p = new InputMediaPhoto();
               
                p.Media = photo.URLImage;
                p.Caption = photo.Descriptipon;
                album.Add(p);
            }
            //InputMedia media = new InputMedia(inlineImageGallery.ImageGalleries[0].URLImage, "123");
            // InputMediaPhoto photo = new InputMediaPhoto(media);
            try
                {
               var t= await botClient.SendMediaGroupAsync(message.Chat.Id, album);
                  //   await botClient.SendPhotoAsync(message.Chat.Id, inlineImageGallery.ImageGalleries[0].URLImage, inlineImageGallery.ImageGalleries[0].Descriptipon, replyMarkup: inlineKeyboardMarkup, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                    //    await botClient.EditMessageMediaAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, photo);
                    //    await botclient.SendPhotoAsync(callbackQuery.Message.Chat.Id, stream);

                }
                catch (Exception ex)
                {
                Bot.MyLogger(ex.Message);
                Bot.MyLogger(ex.StackTrace);

            }
            }

        public async Task ExecuteQuery (CallbackQuery callbackQuery, TelegramBotClient botClient)
        {
            try
            {              
                string url = "1";
                var m = from t in inlineImageGallery.ImageGalleries
                        where t.Descriptipon == callbackQuery.Data.Substring(callbackQuery.Data.IndexOf('z') + 1)
                        select t;
                url = m.ToList()[0].URLImage;

                InputMediaPhoto media = new InputMediaPhoto(new InputMedia(url));
                media.Caption = m.ToList()[0].Descriptipon;

                await botClient.EditMessageMediaAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, media, replyMarkup: GetImageKeyboard());
                // string s = callbackQuery.Data.Substring(callbackQuery.Data.IndexOf(' ') + 1);
                // int level = int.Parse(callbackQuery.Data.Remove(callbackQuery.Data.IndexOf(' ')));
                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "");
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
            return inlineImageGallery.NextStage;
        }



        private InlineKeyboardMarkup GetImageKeyboard()
        {
            int i = 1;
            int k = 2; // тут можно делать разметку по количеству кнопок
            List<List<InlineKeyboardButton>> inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();
            List<InlineKeyboardButton> inlineKeyboardButtons1 = new List<InlineKeyboardButton>();

            foreach (ImageGallery gallery in inlineImageGallery.ImageGalleries)
            {
                InlineKeyboardButton inlineKeyboardButton = new InlineKeyboardButton();
                inlineKeyboardButton.Text = i.ToString() + ". " + gallery.Descriptipon;
                inlineKeyboardButton.CallbackData = inlineImageGallery.Stage + "z" + gallery.Descriptipon;
                inlineKeyboardButtons1.Add(inlineKeyboardButton);
                if (i % k == 0)
                {
                    inlineKeyboardButtons.Add(inlineKeyboardButtons1);

                    inlineKeyboardButtons1 = new List<InlineKeyboardButton>();
                }

                i++;
            }


            return new InlineKeyboardMarkup(inlineKeyboardButtons);
        }
    }
}
