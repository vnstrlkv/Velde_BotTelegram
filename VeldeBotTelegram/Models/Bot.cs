using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using VeldeBotTelegram.Models.Commands;
using VeldeBotTelegram.Models.Interfaces;
using System.IO;
namespace VeldeBotTelegram.Models
{



    public class Bot
    {   

        public static void MyLogger (string message)
        {
            Console.WriteLine(message);
            using (FileStream fstream = new FileStream("./Logs/logs.txt", FileMode.Append))
            {
                // преобразуем строку в байты
               // message = ;
                byte[] array = System.Text.Encoding.Default.GetBytes(DateTime.Now.ToString() + " " + message +"\r\n");
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
              //  Console.WriteLine("Текст записан в файл");

            }
            
        }
        public static bool flag { get; set; }

        private static Dictionary<Type, int> typeDict = new Dictionary<Type, int>
            {
             {typeof(TextMessage),0},
             {typeof(Question),1},
             {typeof(ImageMessage),2},
             {typeof(InlineImageGallery),3},
              {typeof(PhoneMessage),4},
               {typeof(QuestionKitchen),5},
             //{typeof(MyClass),2}
            };

        private static TelegramBotClient botClient;
        private static List<Command> commandsList;
       
        //  private static Client person;
        public static List<Client> clients = new List<Client>();

        public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();
        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
            {
                return botClient;
            }

           
            //TODO: Add more commands           
            botClient = new TelegramBotClient(AppSettings.Key);
            string hook = string.Format(AppSettings.Url, "api/message/update");
            await botClient.SetWebhookAsync(hook);
            return botClient;
        }


        public static void AddCommand(Client person)
        {


            commandsList = new List<Command>();



           IMessage message = DBHelper.GetQuestion(person.Stage);

            switch (typeDict[message.GetType()])
            {
                case 5: //QuestionKitchen -5
                    {
                        commandsList.Add(new QuestionKitchenCommand((QuestionKitchen)message));
                        break;
                    }

                case 4: //PhoneMessage - 4
                    {
                        commandsList.Add(new PhoneMessageCommand((PhoneMessage)message));
                        break;
                    }
                case 3: //InlineGallery - 3
                    {
                        commandsList.Add(new InlineImageGalleryCommand((InlineImageGallery)message));
                        break;
                    }
                case 2: //ImageMessage - 2
                    {
                        commandsList.Add(new ImageMessageCommand((ImageMessage)message));
                        break;
                    }
                case 1: //Question - 1
                    {
                        commandsList.Add(new QuestionCommand((Question)message));
                        break;
                    }
                case 0: //Text message - 0
                    {
                        commandsList.Add(new TextMessageCommand((TextMessage)message));
                            break;
                    }
                default:
                    break;

            }
        }




        public static  bool Contains(Client person)
        {
            
            return DBHelper.ChekClient(person);
        }
        public static void UpdateClient(Client person)
        {
            DBHelper.UpdateClient(person);
        }
        public static void AddClient(Client person)
        {
            DBHelper.AddClient(person);
        
        }
        public static void AddKitchen(Kitchen kit)
        {
            DBHelper.AddKitchen(kit);
        }
        public static Client GetClient(Client person)
        {

            return DBHelper.GetClient(person);
        }

        public static void AddMessage(Client person, Telegram.Bot.Types.Message message)
        {
            DBHelper.AddMessage(person, message);
        }
    }
}
