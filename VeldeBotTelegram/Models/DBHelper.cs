using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using VeldeBotTelegram.Models.Interfaces;

namespace VeldeBotTelegram.Models
{
    public class DBHelper
    {
        
        private static string connectionString;
            
       private static SqliteConnection DataBase { get; set; }

       
        public static string ConnectionString
        {set
            {
                SQLitePCL.Batteries_V2.Init();


                var _connSB = new SqliteConnectionStringBuilder
                {
                    //   Cache = SqliteCacheMode.Private,
                    DataSource = value,
                    Mode = SqliteOpenMode.ReadWriteCreate,
                };

                connectionString = value;
                DataBase = new SqliteConnection(_connSB.ToString());
            }
        }

       public static IMessage GetQuestion (int stage)
        {
            DataBase.OpenAsync();
            IMessage quest=null;
           // TextMessage text = new TextMessage();
            
                //  SqliteCommand command = new SqliteCommand("SELECT * FROM QUESTIONS WHERE STAGE = '" + stage+ "' ", DataBase);
            SqliteCommand command = new SqliteCommand("SELECT * FROM [QUESTIONS] WHERE [STAGE] = " + stage, DataBase);
            var reader = command.ExecuteReader();
            while (reader.Read())

            {
                try
                {
                    if (reader.GetString(0) == "0")
                    {
                        TextMessage text = new TextMessage();

                        text.Stage = int.Parse(reader.GetString(1));
                        text.Message = reader.GetString(2).ToString();
                        text.NextStage = int.Parse(reader.GetString(3));
                        quest = text;

                    }

                    else if (reader.GetString(0) == "1")
                    {
                        Question quest1 = new Question();
                        quest1.Stage = int.Parse(reader.GetString(1)); quest1.Message = reader.GetString(2).ToString();

                        reader.Close();
                        command = new SqliteCommand("SELECT * FROM ANSWERS WHERE IdQuestion=" + stage, DataBase);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            quest1.Answers.Add(new Answer(reader.GetString(1), int.Parse(reader.GetString(2))));
                        }
                        quest = quest1;
                    }

                    else if (reader.GetString(0) == "2")
                    {
                        ImageMessage text = new ImageMessage();

                        text.Stage = int.Parse(reader.GetString(1));
                        text.Message = reader.GetString(2).ToString();
                        text.NextStage = int.Parse(reader.GetString(3));
                        text.UrlMessage = reader.GetString(4);
                        quest = text;
                    }

                    else if (reader.GetString(0) == "3")
                    {
                        InlineImageGallery gallery = new InlineImageGallery();
                        gallery.Stage = int.Parse(reader.GetString(1));
                        gallery.Message = reader.GetString(2).ToString();
                        gallery.NextStage = int.Parse(reader.GetString(3));
                        //gallery.UniqueName = reader.GetString(4);
                        reader.Close();
                        command = new SqliteCommand("SELECT * FROM IMAGES WHERE StageImageGallery=" + stage, DataBase);
                        reader = command.ExecuteReader();
                        while (reader.Read())

                        {
                            gallery.ImageGalleries.Add(new ImageGallery(int.Parse(reader.GetString(0)), reader.GetString(1), reader.GetString(2)));
                        }
                        quest = gallery;
                    }


                    else if (reader.GetString(0) == "4")
                    {
                        PhoneMessage text = new PhoneMessage();

                        text.Stage = int.Parse(reader.GetString(1));
                        text.Message = reader.GetString(2).ToString();
                        text.NextStage = int.Parse(reader.GetString(3));
                        quest = text;
                    }
                    else if (reader.GetString(0) == "5")
                    {
                        QuestionKitchen quest1 = new QuestionKitchen();
                        quest1.Stage = int.Parse(reader.GetString(1)); quest1.Message = reader.GetString(2).ToString();

                        reader.Close();
                        command = new SqliteCommand("SELECT * FROM ANSWERS WHERE IdQuestion=" + stage, DataBase);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            quest1.Answers.Add(new Answer(reader.GetString(1), int.Parse(reader.GetString(2))));
                        }
                        quest = quest1;
                    }
                }
                catch (Exception ex)
                {
                    var t = ex;
                }
            }

            DataBase.CloseAsync();
            return quest;
        }
       public static void AddQuestion (Question question)
        {
            DataBase.OpenAsync();
           // SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO Book (Id, Title, Language, PublicationDate, Publisher, Edition, OfficialUrl, Description, EBookFormat) VALUES (?,?,?,?,?,?,?,?,?)", sql_con);
            SqliteCommand command = new SqliteCommand("INSERT INTO QUESTIONS (Stage, Message) VALUES (@Stage, @Message)", DataBase);
         //   command.Parameters.Add(question.IdQuestion);
            command.Parameters.AddWithValue("Stage",question.Stage);
            command.Parameters.AddWithValue("Message",question.Message);
            command.ExecuteNonQuery();
            foreach (Answer an in question.Answers)
            {
                SqliteCommand command2 = new SqliteCommand("INSERT INTO ANSWERS (IdQuestion, RightAnswer, NextStage) VALUES (@IdQuestion, @RightAnswer, @NextStage)", DataBase);
                command2.Parameters.AddWithValue("IdQuestion", question.Stage);
                command2.Parameters.AddWithValue("RightAnswer",an.RightAnswer);
                command2.Parameters.AddWithValue("NextStage",an.NextStage);
                command2.ExecuteNonQuery();
            }
            DataBase.CloseAsync();
        }
        public static void AddKitchen(Kitchen kitchen)
        {
            DataBase.OpenAsync();
            SqliteCommand command = new SqliteCommand("INSERT INTO KITCHENS (CHATID, TYPEFACE, TYPETABLE, LENGHT) VALUES (@ChatId, @TypeFace, @TypeTable, @Lenght)", DataBase);

            //   if (client.ChatId != null)
            command.Parameters.AddWithValue("ChatId", kitchen.ChatId);
        
                command.Parameters.AddWithValue("TypeFace", kitchen.TypeFace);
           
                command.Parameters.AddWithValue("TypeTable", kitchen.TypeTable);
      
                command.Parameters.AddWithValue("Lenght", kitchen.Lenght);
            try
            {
                command.ExecuteNonQuery();
                DataBase.CloseAsync();
            }
            catch (Exception ex)
            {
                Bot.MyLogger(ex.Message);
                var t = ex;
                DataBase.CloseAsync();
            }
        }
        public static void AddClient (Client client)
        {
            DataBase.OpenAsync();
            SqliteCommand command = new SqliteCommand("INSERT INTO CLIENTS (CHATID, PHONENUMBER, NAME, STAGE, METRICA) VALUES (@ChatId, @PhoneNumber, @Name, @Stage, @Metrica)", DataBase);

         //   if (client.ChatId != null)
            command.Parameters.AddWithValue("ChatId", client.ChatId);
            if (client.PhoneNumber != null)
                command.Parameters.AddWithValue("PhoneNumber", client.PhoneNumber);
         //   else command.Parameters.AddWithValue("PhoneNumber", -1);
            if (client.Name != null)
                command.Parameters.AddWithValue("Name", client.Name);
          //  else command.Parameters.AddWithValue("Name", -1);
            command.Parameters.AddWithValue("Stage", client.Stage);
            command.Parameters.AddWithValue("Metrica", client.Metrica);
            try
            {
                command.ExecuteNonQuery();
                DataBase.CloseAsync();
            }
            catch (Exception ex)
            {
                Bot.MyLogger(ex.Message);
                var t = ex;
                DataBase.CloseAsync();
            }
        }

        public static bool ChekClient(Client client)
        {
            bool flag = false;
            DataBase.OpenAsync();

            //SELECT EXISTS(SELECT 1 FROM myTbl WHERE u_tag="tag")
            try
            {
                SqliteCommand command = new SqliteCommand("SELECT EXISTS(SELECT CHATID FROM CLIENTS WHERE CHATID = " + client.ChatId + ")", DataBase);
                var reader = command.ExecuteReader();
                reader.Read();
                if (reader.GetValue(0).ToString() == "1")
                    flag = true;
            }
            catch(Exception ex)
            { var m = ex; }
            DataBase.CloseAsync();
            return flag;

        }
        public static void EraseKitchen(Kitchen kit)
        {
            DataBase.OpenAsync();
            SqliteCommand command = new SqliteCommand("DELETE FROM KITCHENS WHERE ChatId = "+ kit.ChatId, DataBase);
            command.ExecuteNonQuery();

            DataBase.CloseAsync();
        }
        public static bool ChekKitchen(Client kitchen)
        {
            bool flag = false;
            DataBase.OpenAsync();

            //SELECT EXISTS(SELECT 1 FROM myTbl WHERE u_tag="tag")
            try
            {
                SqliteCommand command = new SqliteCommand("SELECT EXISTS(SELECT CHATID FROM KITCHENS WHERE CHATID = " + kitchen.ChatId + ")", DataBase);
                var reader = command.ExecuteReader();
                reader.Read();
                if (reader.GetValue(0).ToString() == "1")
                    flag = true;
            }
            catch (Exception ex)
            { var m = ex; }
            DataBase.CloseAsync();
            return flag;

        }

        public static Kitchen GetKitchen(Client client)
        {
            Kitchen kit = new Kitchen();
            DataBase.OpenAsync();

            //SELECT EXISTS(SELECT 1 FROM myTbl WHERE u_tag="tag")

            SqliteCommand command = new SqliteCommand("SELECT * FROM KITCHENS WHERE CHATID = " + client.ChatId, DataBase);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                
                kit.ChatId = long.Parse(reader.GetValue(0).ToString());
                kit.TypeFace = reader.GetValue(1).ToString();
                kit.TypeTable = reader.GetValue(2).ToString();
                kit.Lenght = int.Parse(reader.GetValue(3).ToString());
            }
            DataBase.CloseAsync();
            return kit;
        }
        public static Client GetClient(Client client)
        {
            Client cl=new Client();
            DataBase.OpenAsync();

            //SELECT EXISTS(SELECT 1 FROM myTbl WHERE u_tag="tag")
           
                SqliteCommand command = new SqliteCommand("SELECT * FROM CLIENTS WHERE CHATID = " + client.ChatId, DataBase);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cl.ChatId = long.Parse(reader.GetValue(0).ToString());
                    cl.PhoneNumber = reader.GetValue(1).ToString();
                    cl.Name = reader.GetValue(2).ToString();
                    cl.Stage = int.Parse(reader.GetValue(3).ToString());
                    cl.Name = reader.GetValue(4).ToString();
                }
            DataBase.CloseAsync();
            return cl;

        }

        public static void UpdateClient(Client client)
        {
            DataBase.OpenAsync();
            SqliteCommand command = new SqliteCommand("UPDATE CLIENTS SET  PHONENUMBER = :Phone, NAME = :name, STAGE = :stage where CHATID=:id", DataBase);
            command.Parameters.Add("Phone", SqliteType.Text).Value = client.PhoneNumber;
            command.Parameters.Add("name", SqliteType.Text).Value = client.Name;
            command.Parameters.Add("stage", SqliteType.Integer).Value = client.Stage;
            command.Parameters.Add("id", SqliteType.Text).Value = client.ChatId;
            try
            {
                command.ExecuteNonQuery();
                DataBase.CloseAsync();
            }
            catch (Exception ex)
            {
                var t = ex;
                Bot.MyLogger(ex.Message);
                DataBase.CloseAsync();
            }
        }

        public static void UpdateKitchen(Kitchen kitchen)
        {
            DataBase.OpenAsync();
            SqliteCommand command = new SqliteCommand("UPDATE KITCHENS SET  Typeface = :typeface, typetable = :Typetable, LENGHT = :lenght where CHATID=:id", DataBase);
            command.Parameters.Add("typeface", SqliteType.Text).Value = kitchen.TypeFace;
            command.Parameters.Add("Typetable", SqliteType.Text).Value = kitchen.TypeTable;
            command.Parameters.Add("lenght", SqliteType.Integer).Value = kitchen.Lenght;
            command.Parameters.Add("id", SqliteType.Text).Value = kitchen.ChatId;
            try
            {
                command.ExecuteNonQuery();
                DataBase.CloseAsync();
            }
            catch (Exception ex)
            {
                var t = ex;
                Bot.MyLogger(ex.Message);
                DataBase.CloseAsync();
            }
        }



        public static void AddMessage(Client client, Telegram.Bot.Types.Message message)
        {
            try
            {
                DataBase.OpenAsync();


                SqliteCommand command = new SqliteCommand("INSERT into messages (ClientId, Stage, text, date) values (@clientid, @stage, @text, @date)", DataBase);
                command.Parameters.AddWithValue("clientid", client.ChatId);
                command.Parameters.AddWithValue("stage", client.Stage);
                if (message.Text != null)
                    command.Parameters.AddWithValue("text", message.Text);
                else if (message.Contact != null)
                    command.Parameters.AddWithValue("text", message.Contact.FirstName + " " + message.Contact.LastName + " " + message.Contact.PhoneNumber);
                else command.Parameters.AddWithValue("text", DBNull.Value);
               // TimeZoneInfo estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("N. Central Asia Standard Time");
               // DateTime estDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, estTimeZone);
                command.Parameters.AddWithValue("date", DateTime.Now.ToString("G"));

                command.ExecuteNonQuery();
                DataBase.CloseAsync();
            }
            catch (Exception ex)
            {
                var t = ex;
                Bot.MyLogger(ex.Message);
                DataBase.CloseAsync();
            }
        }
        public static void AddMessageTEXT(Client client, string message)
        {
            try
            {
                DataBase.OpenAsync();


                SqliteCommand command = new SqliteCommand("INSERT into messages (ClientId, Stage, text, date) values (@clientid, @stage, @text, @date)", DataBase);
                command.Parameters.AddWithValue("clientid", client.ChatId);
                command.Parameters.AddWithValue("stage", 999);              
                command.Parameters.AddWithValue("text", message); 

                // TimeZoneInfo estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("N. Central Asia Standard Time");
                // DateTime estDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, estTimeZone);
                command.Parameters.AddWithValue("date", DateTime.Now.ToString("G"));

                command.ExecuteNonQuery();
                DataBase.CloseAsync();
            }
            catch (Exception ex)
            {
                var t = ex;
                Bot.MyLogger(ex.Message);
                DataBase.CloseAsync();
            }
        }

        public static List<string> GetBOTConfig(int ID)
        {
            List<string> appConfig = new List<string>();
            DataBase.OpenAsync();            

            SqliteCommand command = new SqliteCommand("SELECT * FROM botconfig WHERE ID = " + ID, DataBase);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                
                 
                 appConfig.Add(reader.GetValue(0).ToString());
                appConfig.Add(reader.GetValue(1).ToString());
                appConfig.Add(reader.GetValue(2).ToString());

            }
            DataBase.CloseAsync();
            return appConfig;
        }

        public static List<string> GetBitrixConfig(int ID)
        {
            List<string> appConfig = new List<string>();
            DataBase.OpenAsync();

            SqliteCommand command = new SqliteCommand("SELECT * FROM bitrixconfig WHERE ID = " + ID, DataBase);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {


                appConfig.Add(reader.GetValue(0).ToString());
                appConfig.Add(reader.GetValue(1).ToString());
                appConfig.Add(reader.GetValue(2).ToString());

            }
            DataBase.CloseAsync();
            return appConfig;
        }


        public static List<ClientMessage> GetAllClientMessages(Client client)
        {
            List<ClientMessage> clientMessages = new List<ClientMessage>();
            
            DataBase.OpenAsync();

            //SELECT EXISTS(SELECT 1 FROM myTbl WHERE u_tag="tag")

            SqliteCommand command = new SqliteCommand("SELECT * FROM Messages WHERE ClientID = " + client.ChatId, DataBase);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ClientMessage cl = new ClientMessage();
                cl.Date = DateTime.Parse(reader.GetValue(3).ToString());
                cl.Answer = reader.GetValue(2).ToString();               
                cl.Question = reader.GetValue(1).ToString();               
                clientMessages.Add(cl);
            }
            DataBase.CloseAsync();

            for (int i = 0; i < clientMessages.Count; i++)
            {
                if (clientMessages[i].Question == "999")
                {
                    clientMessages[i].Question = clientMessages[i].Answer;
                    clientMessages[i].Answer = null;
                }
                else
                {
                    clientMessages[i].Question = GetQuestion(int.Parse(clientMessages[i].Question)).Message;
                }
            }

            return clientMessages;
        }
    }
}
