using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using HPCorpBot.Models.Commands;
using HPCorpBot.Models.Interfaces;
using System.IO;
using HPCorpBot.Helpers;
using HPCorpBot.Models.Questions;
namespace HPCorpBot.Models
{
    public class Bot
    {   

        public static void MyLogger (string message)
        {
            Console.WriteLine(message);
            
            using (FileStream fstream = new FileStream("./Logs/logs.log", FileMode.Append))
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
             {typeof(TextQuestion),1},
             {typeof(InlineImageGallery),2},
             {typeof(ImageQuestion),3},
             {typeof(MusicQuestion),4},  
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



           IMessage message = DBHelper.GetMessage(person.Stage);

            switch (typeDict[message.GetType()])
            {
                case 4: 
                    {
                        commandsList.Add(new MusicQuestionCommand((MusicQuestion)message));
                        break;
                    }
                case 3: 
                    {
                        commandsList.Add(new ImageQuestionCommand((ImageQuestion)message));
                        break;
                    }
                case 2: 
                    {
                        commandsList.Add(new InlineImageGalleryCommand((InlineImageGallery)message));
                        break;
                    }

                case 1: 
                    {
                        commandsList.Add(new TextQuestionCommand((TextQuestion)message));
                        break;
                    }             
               
                case 0: 
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
