using System;
using VeldeBotTelegram.Models;
using OfficeOpenXml;
using System.IO;
using System.Collections.Generic;
using VeldeBotTelegram.Models.Interfaces;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ExcelToBDTGSqlite
{
    class Program
    {

        

        public static class Bitrix24CRMHelper
        {
            private static string BitrixName = "";
            private static string BitrixPass = "";
            public static async Task<bool> Start()
            {
                bool flag = false;
                try
                {
                    string TaskID = await AddTaskToBitrix();
                    string CommentID = await AddCommentToCRM(TaskID);

                    if (TaskID != "" && CommentID != "")
                        flag = true;
                    //  string IDpath = null ;
                }
                catch (Exception ex)
                {
                    var t = ex;
                }

                Console.WriteLine(DateTime.Now + ": " + "Готово");
                return flag;
            }


            private static string CommentFormat(string id)
            {
                
                string stringPayload;                          
                try
                {
                    stringPayload = JsonConvert.SerializeObject(

                     new
                     {
                         fields = new
                         {
                             ENTITY_ID= id,
                             ENTITY_TYPE= "deal",
                             COMMENT= "New comment was added",
                         }

                     }

                       );
                    return  stringPayload;
                }

                catch (Exception ex)
                {
                    var t = ex;

                }

                return  null;




            }

            private static string CRMFormat()
            {
                string _TITLE;
                string stringPayload;


                _TITLE = "ЧатБотТЕЛЕГРАМ";
            
                try
                {
                    stringPayload =  JsonConvert.SerializeObject(

                     new
                     {
                         fields = new
                         {
                             TITLE = _TITLE,
                             OPENED = "Y",                           
                             CATEGORY_ID = "4",
                         }

                     }

                       );
                    return stringPayload;
                }
               
                catch (Exception ex)
                {
                    var t = ex;
                    
                }

                return  null;
            }

            private static string ContactFormat(string name, string phone)
            {
                
                string stringPayload;                     
                try
                {
                    stringPayload = JsonConvert.SerializeObject(
                     new
                     {
                         fields = new
                         {
                             NAME = name,                             
                             OPENED = "Y",
                             ASSIGNED_BY_ID = 1,
                             TYPE_ID = "CLIENT",
                             SOURCE_ID = "SELF",
                             PHONE =new List<object> { new { VALUE_TYPE = "WORK", TYPE_ID = "PHONE", VALUE = phone } }.ToArray()
                         }

                     }

                       );
                    return stringPayload;
                }

                catch (Exception ex)
                {
                    var t = ex;

                }

                return null;
            }

            private static async Task<string> AddTaskToBitrix()
            {


                
                string task = "crm.deal.add.json";

                // Serialize our concrete class into a JSON String

                string stringPayload1 = CRMFormat();
                if (stringPayload1 != null)
                {

                    string url = "https://" + BitrixName + "/rest/32/" + BitrixPass + "/" + task;
                    // Wrap our JSON inside a StringContent which then can be used by the HttpClient class

                    var httpContent = new StringContent(stringPayload1, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {

                        // Do the actual request and await the response
                        try
                        {
                            var httpResponse = await httpClient.PostAsync(url, httpContent);

                            // If the response contains content we want to read it!
                            if (httpResponse.Content != null)
                            {
                                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                                Console.WriteLine(DateTime.Now + " : " + task + " "+ responseContent);
                                var ID = Regex.Match(Regex.Match(responseContent, @"""result"":\d+").Value, @"\d+").Value;  //@"""result"":\d+"
                                return ID;
                                //   await AddCheklistToBitrixTask(ID, "ТЕСТ", "Иван Стрелков");
                                // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
                            }
                        }
                        catch (Exception ex)
                        {
                            var t = ex;
                        }

                    }
                }

                return null;
            }
            private static async Task<string> AddCommentToCRM(string id)
            {
                
                string task = "crm.timeline.comment.add.json";

                // Serialize our concrete class into a JSON String

                string stringPayload1 = CommentFormat(id);
                if (stringPayload1 != null)
                {

                    string url = "https://" + BitrixName + "/rest/32/" + BitrixPass + "/" + task;
                    // Wrap our JSON inside a StringContent which then can be used by the HttpClient class

                    var httpContent = new StringContent(stringPayload1, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {

                        // Do the actual request and await the response
                        try
                        {
                            var httpResponse = await httpClient.PostAsync(url, httpContent);

                            // If the response contains content we want to read it!
                            if (httpResponse.Content != null)
                            {
                                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                                Console.WriteLine(DateTime.Now + " : " + task);
                                var ID = Regex.Match(Regex.Match(responseContent, @"""result"":\d+").Value, @"\d+").Value;
                                return ID;
                                //   await AddCheklistToBitrixTask(ID, "ТЕСТ", "Иван Стрелков");
                                // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
                            }
                        }
                        catch (Exception ex)
                        {
                            var t = ex;
                        }

                    }
                }

                return null;
            }



        }








        static List<IMessage> messages = new List<IMessage>();
        static void Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
              DBHelper.ConnectionString = new string(@"C:\Users\Администратор\source\repos\VeldeBotTelegram\VeldeBotTelegram\DataBase\DataBase.db");
           //  string pathPrice = "./DataBase/price.xlsx";
       //     string pathPrice = @"C:\Users\Администратор\source\repos\VeldeBotTelegram\ExcelToBDTGSqlite\DataBase\price.xlsx";
       //     Kitchen kitchen = new Kitchen("МДФ плёнка", "Пластик",3);
       //     int mk = kitchen.GetFullPrice(pathPrice);


            ExcelToClass(@"C:\Users\Администратор\source\repos\VeldeBotTelegram\ExcelToBDTGSqlite\bin\Debug\1.xlsx");
          //  Bitrix24CRMHelper.Start().Wait();
            DBHelper.EraseTable();
            foreach(IMessage mes in messages)
            {
                DBHelper.AddMessqges(mes);
            }
            Console.WriteLine("Hello World!");
        }


        public static  void ExcelToClass(string path)
        {
            FileInfo File = new FileInfo(path);
            using (ExcelPackage pck = new ExcelPackage(File))
            {
                var format = new ExcelTextFormat();
                format.Delimiter = '/';
                int k = pck.Workbook.Worksheets.Count;
                ExcelWorksheet ws = null;

                ws = pck.Workbook.Worksheets[1];
                bool flag = true;
                int i = 10;
                int j = 0;
                while (flag)
                {
                    
                    string tmp = ws.Cells[i, 1].Value.ToString().Trim();
                    IMessage temp = null;
                    int l = i;
                    switch (ws.Cells[i, 2].Value.ToString().Trim())
                    {
                        case "0":
                            {
                                TextMessage text = new TextMessage();
                                text.Stage = int.Parse(ws.Cells[i, 1].Value.ToString().Trim());
                                text.Message = ws.Cells[i, 3].Value.ToString().Trim();
                                text.NextStage = int.Parse(ws.Cells[i, 5].Value.ToString().Trim());
                                temp = text;
                                i++;
                                break;
                            }

                        case "1":
                            {
                                Question text = new Question();
                                int p = i;
                                text.Stage= int.Parse(ws.Cells[i, 1].Value.ToString().Trim());
                                text.Message= ws.Cells[i, 3].Value.ToString().Trim();
                                while(ws.Cells[i, 1].Value.ToString().Trim() == tmp.Trim())
                                {
                                    Answer ans = new Answer(ws.Cells[i, 4].Value.ToString().Trim(), int.Parse(ws.Cells[i, 5].Value.ToString().Trim()));
                                    text.Answers.Add(ans);
                                    i++;
                                    if (ws.Cells[i, 1].Value == null)
                                        break;
                                }
                                temp = text;
                                break;
                            }
                        case "2":
                            {
                                break;
                            }
                        case "3":
                            {
                                InlineImageGallery text = new InlineImageGallery();
                                int p = i;
                                text.Stage = int.Parse(ws.Cells[i, 1].Value.ToString().Trim());
                                text.Message = ws.Cells[i, 3].Value.ToString().Trim();
                                text.NextStage = int.Parse( ws.Cells[i, 5].Value.ToString().Trim());
                                while (ws.Cells[i, 1].Value.ToString().Trim() == tmp.Trim())
                                {
                                    ImageGallery ans = new ImageGallery(text.Stage, "https://myvelde.com/data/bot/"+ws.Cells[i, 6].Value.ToString().Trim(), ws.Cells[i, 4].Value.ToString().Trim());
                                    text.ImageGalleries.Add(ans);
                                    i++;
                                    if (ws.Cells[i, 1].Value == null)
                                        break;
                                }
                                temp = text;
                                
                                break;
                            }
                        case "4":
                            {
                                PhoneMessage text = new PhoneMessage();
                                text.Stage = int.Parse(ws.Cells[i, 1].Value.ToString().Trim());
                                text.Message = ws.Cells[i, 3].Value.ToString().Trim();
                                text.NextStage = int.Parse(ws.Cells[i, 5].Value.ToString().Trim());
                                temp = text;
                                i++;
                                break;
                            }
                        case "5":
                            {
                                QuestionKitchen text = new QuestionKitchen();
                                int p = i;
                                text.Stage = int.Parse(ws.Cells[i, 1].Value.ToString().Trim());
                                text.Message = ws.Cells[i, 3].Value.ToString().Trim();
                                while (ws.Cells[i, 1].Value.ToString().Trim() == tmp.Trim())
                                {
                                    Answer ans = new Answer(ws.Cells[i, 4].Value.ToString().Trim(), int.Parse(ws.Cells[i, 5].Value.ToString().Trim()));
                                    text.Answers.Add(ans);
                                    i++;
                                    if (ws.Cells[i, 1].Value == null)
                                        break;
                                }
                                temp = text;
                                break;
                            }
                    }
                    if (flag != false)
                    {
                        messages.Add(temp);
                        // tmp = ws.Cells[i, 1].Value.ToString();
                    }
                  
                    if(ws.Cells[i, 1].Value==null)
                    {
                        flag = false;
                    }
                   
                }
            }
        }
    }
}
