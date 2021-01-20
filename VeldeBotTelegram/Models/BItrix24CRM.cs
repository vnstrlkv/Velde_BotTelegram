using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace VeldeBotTelegram.Models
{
    public static class BItrix24CRM
    {

        
            private static string BitrixName;
            private static string BitrixPass;
            
            public static void LoadBitrixConfig(int ID)
        {
            var conf= DBHelper.GetBitrixConfig(ID);
            BitrixName = conf[1];
            BitrixPass = conf[2];
               
        }
            public static async Task<bool> Start(Client client)
            {
                bool flag = false;
                try
                {

                List<ClientMessage> LastMess=null;
                List<ClientMessage> AllMessage = DBHelper.GetAllClientMessages(client);
                int range = 50; //берем последние 50 сообщений
                if (AllMessage.Count>range)
                {
                     LastMess = AllMessage.GetRange(AllMessage.Count - range, range);
                }
                else
                {
                    LastMess = AllMessage;
                }
                var myString = "";
                var sb = new System.Text.StringBuilder();

                foreach (var s in LastMess)
                {
                    sb.Append(s.ToString()).Append("\n");
                }
                myString = sb.ToString();

                string TaskID = await AddTaskToBitrix();
                string CommentID = await AddCommentToTask(TaskID, myString);
                string ContactID = await AddContactToTask(TaskID,  client);
               
                    
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


            private static string CommentFormat(string id, string comment)
            {

                string stringPayload;
                try
                {
                    stringPayload = JsonConvert.SerializeObject(

                     new
                     {
                         fields = new
                         {
                             ENTITY_ID = id,
                             ENTITY_TYPE = "deal",
                             COMMENT = comment,
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

            private static string CRMFormat()
            {
                string _TITLE;
                string stringPayload;


                _TITLE = "Кухни из тележки";

                try
                {
                    stringPayload = JsonConvert.SerializeObject(

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

                return null;
            }

            private static string AddContactFormat(string id, string name, string phone)
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
                             PHONE = new List<object> { new { VALUE_TYPE = "WORK", TYPE_ID = "PHONE", VALUE = phone } }.ToArray()
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
            private static string SelectContactWithPhoneFormat (string phone)
        {
            string stringPayload;
            try
            {
                stringPayload = JsonConvert.SerializeObject(
                 new
                 {
                     filter = new
                     {
                         PHONE= phone,
               
                     },

                     select = new List<object>
                     {
                        "ID"
                     },

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
           private static string AddContactToTaskFormat (string idTASK, string idCONTACT)
        {
            try
            {
               var stringPayload = JsonConvert.SerializeObject(

                 new
                 {
                     id = idTASK,
                   
                     fields = new
                     {
                         CONTACT_ID=idCONTACT,
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
                                Console.WriteLine(DateTime.Now + " : " + task + " " + responseContent);
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
            private static async Task<string> AddCommentToTask(string id, string comment)
            {

                string task = "crm.timeline.comment.add.json";

                // Serialize our concrete class into a JSON String

                string stringPayload1 = CommentFormat(id, comment);
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

            private static async Task<string> AddNewContactToBitrix(string id, string name, string phone)
        {

            string task = "crm.contact.add.json";

            // Serialize our concrete class into a JSON String

            string stringPayload1 = AddContactFormat(id, name, phone);
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


        private static async Task<string> AddContactToTask(string id, Client client)
        {

            string task = "crm.deal.contact.add";

            // Serialize our concrete class into a JSON String
            string contactID = await  SelectContactWithPhone(client.PhoneNumber);
            if (contactID == "")

            {
                contactID=await AddNewContactToBitrix(id, client.Name, client.PhoneNumber);
            }


            string stringPayload1 = AddContactToTaskFormat(id, contactID);
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

        private static async Task<string> SelectContactWithPhone(string PhoneNumber)
        {
            string task = "crm.contact.list.json";

            // Serialize our concrete class into a JSON String
           

            string stringPayload1 = SelectContactWithPhoneFormat(PhoneNumber);
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
                        //    ID = Regex.Match(Regex.Match(ID, @"""id"":""\d+").Value, @"\d+").Value;
                            var ID = Regex.Match(Regex.Match(responseContent.ToLower(), @"""id"":""\d+").Value, @"\d+").Value;
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
}
