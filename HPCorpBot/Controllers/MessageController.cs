using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using HPCorpBot.Models;
using System.Net.Http;
using HPCorpBot.Models.Commands;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.RegularExpressions;

namespace HPCorpBot.Controllers
{
    [Route("api/message/update")]
    public class MessageController : Controller
    {
        
        public Client client { get; set; }           
        // GET api/values
        [HttpGet]
        public string Get()
        {
        //    Bot.MyLogger("СТАРТ3231");
            Console.WriteLine(DateTime.Now);
            return "Method GET unuvalable";
        }
                          
        [HttpPost]

        // public async Task<OkResult> Post([FromBody]Update update)
        //    public async Task<OkResult> Post([FromBody]Update update)
          public async Task<OkResult> Post([FromBody]Update update)
   
        {            
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            try
            {             
               
                var botClient = await Bot.GetBotClientAsync();                              

                var message = update.Message;
                string copymessage=null;
                if(update.Message.Text!=null)
                copymessage= (string)update.Message.Text.Clone();
                
                client = new Client();
                client.ChatId = (long)message.Chat.Id;

                if (!Bot.Contains(client))
                {   
                    Bot.AddClient(client);
                   
                    Bot.AddMessage(client, message);
                }
                else
                {
                    client = Bot.GetClient(client);                  
                }
                               


                Bot.AddCommand(client);


                //Bot.AddMessage(client, message);



                //return Ok();
                await ExecuteCommand(message, botClient, copymessage);

                return Ok();
            }
            catch (Exception ex)
            {
                Bot.MyLogger(ex.Message);
                Bot.MyLogger(ex.StackTrace);
                return Ok();
            }
            
           
        }

        private void UpdateClient (Client client, ref Message message)
        {
            
            {
                Bot.UpdateClient(client);

                Bot.AddCommand(client);
                Bot.AddMessage(client, message);
                message.Text = null;
                message.Contact = null;
            }
      
        }
        private async Task ExecuteCommand(Message message, Telegram.Bot.TelegramBotClient botClient, string copymessage)
        {

            var commands = Bot.Commands;
            foreach (var command in commands)
            {
                if (command is TextMessageCommand || command is ImageMessageCommand || command is InlineImageGalleryCommand)
                {
                    await command.Execute(message, botClient, client);
                    client.Stage = command.NextStage(message, client);
                    UpdateClient(client, ref message);

                    await ExecuteCommand(message, botClient, copymessage);
                }
                else if (command is QuestionCommand || command is QuestionKitchenCommand)
                {
                    if (!command.Contains(message))
                        await command.Execute(message, botClient, client);
                        if (command.Contains(message))
                        {
                            client.Stage = command.NextStage(message, client);
                        UpdateClient(client, ref message);

                        if (command is QuestionKitchenCommand)
                        {
                            Kitchen kitchen = DBHelper.GetKitchen(client);
                            if (kitchen.Lenght == -1)
                                kitchen.Lenght = int.Parse(copymessage);
                           else if (kitchen.TypeFace == "null")
                                kitchen.TypeFace = copymessage;
                            else if (kitchen.TypeTable == "null")
                                kitchen.TypeTable = copymessage;
                            DBHelper.UpdateKitchen(kitchen);
                            if (kitchen.Lenght != -1 && kitchen.TypeFace != "null" && kitchen.TypeTable != "null")
                            {
                                var culture = new System.Globalization.CultureInfo("ru-RU");
                                string messageTEXT = "Базовая стоимость: "+ kitchen.GetFullPrice("./DataBase/price.xlsx").ToString("#,#", culture) +"р. минус ваша скидка 10% = "+ kitchen.GetSalePrice("./DataBase/price.xlsx", 1.1).ToString("#,#", culture) +
                                    "р. (Кухня длиной "+kitchen.Lenght+" метра, фасады: "
                                    +kitchen.TypeFace+", фурнитура Blum, " +
                                    "столешница: "+kitchen.TypeTable.ToLower()+". В цену включены: замеры, создание прототипа, доставка, разгрузка, сборка и клининг.)" ;
                                string text = "Хорошие новости, при заказе кухни у Стаса, персональный бонус – скидка 10% 🎁";
                                await botClient.SendTextMessageAsync(message.Chat.Id, text, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                                
                                await botClient.SendTextMessageAsync(message.Chat.Id, messageTEXT, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                                DBHelper.AddMessageTEXT(client, messageTEXT);
                                DBHelper.AddMessageTEXT(client, text);
                                DBHelper.EraseKitchen(kitchen);
                            }
                       
                        }

                        await ExecuteCommand(message, botClient, copymessage);
                        
                            
                        }
                } 
                
                else if (command is NameMessageCommand)
                {
                    await command.Execute(message, botClient, client);

                }
                else if (command is PhoneMessageCommand)
                {
                    if (message.Contact != null)
                    {
                        if (client.Name == "-1" || client.Name == null || client.Name == "")
                            client.Name = message.Contact.FirstName + " " + message.Contact.LastName;
                        client.PhoneNumber = message.Contact.PhoneNumber;
                        client.Stage = command.NextStage(message, client);
                        UpdateClient(client, ref message);                        
                        await ExecuteCommand(message, botClient, copymessage);

                    }

                    else if(message.Text =="Не буду")
                    {
                        client.Stage = command.NextStage(message, client);
                        UpdateClient(client, ref message);

                        await ExecuteCommand(message, botClient, copymessage);
                    }
                    else
                    {
                        // if (client.Name == null)
                        //   client.Name = message.Text;
                        Bot.AddMessage(client, message);
                        await command.Execute(message, botClient, client);
                    }

                }



            }



        }
                          

        }
    }
