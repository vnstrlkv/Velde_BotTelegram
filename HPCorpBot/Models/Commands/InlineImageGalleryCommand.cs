using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using HPCorpBot.Models;
namespace HPCorpBot.Models.Commands
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
            List<InputMediaPhoto> album = new List<InputMediaPhoto>();
            foreach(var photo in inlineImageGallery.ImageGalleries)
            {
                InputMediaPhoto p = new InputMediaPhoto();
               
                p.Media = photo.URLImage;
                p.Caption = photo.Descriptipon;
                album.Add(p);
            }            
            try
                {
               var t= await botClient.SendMediaGroupAsync(message.Chat.Id, album);
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
       
    }
}
