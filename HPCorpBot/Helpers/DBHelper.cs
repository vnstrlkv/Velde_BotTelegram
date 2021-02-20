using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using HPCorpBot.Models.Interfaces;
using HPCorpBot.Models;
using HPCorpBot.Models.Questions;


namespace HPCorpBot.Helpers
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

       public static IMessage GetMessage (int stage)
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
                        TextQuestion text = new TextQuestion();
                        text.Stage = int.Parse(reader.GetString(1));
                        text.Message = reader.GetString(2).ToString();
                        text.NextStage = int.Parse(reader.GetString(3));
                        text.TimeToAnswer = int.Parse(reader.GetString(4));
                        text.RightAnswer= reader.GetString(5).ToString();
                        text.Score = int.Parse(reader.GetString(6));
                        quest = text;

                    }

                    else if (reader.GetString(0) == "2")
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

                    else if (reader.GetString(0) == "3")
                    {
                        ImageQuestion text = new ImageQuestion();
                        text.Stage = int.Parse(reader.GetString(1));
                        text.Message = reader.GetString(2).ToString();
                        text.NextStage = int.Parse(reader.GetString(3));
                        text.TimeToAnswer = int.Parse(reader.GetString(4));
                        text.RightAnswer = reader.GetString(5).ToString();                        
                        text.Score = int.Parse(reader.GetString(6));
                        text.URL = reader.GetString(7).ToString();
                        quest = text;

                    }

                    else if (reader.GetString(0) == "4")
                    {
                        MusicQuestion text = new MusicQuestion();
                        text.Stage = int.Parse(reader.GetString(1));
                        text.Message = reader.GetString(2).ToString();
                        text.NextStage = int.Parse(reader.GetString(3));
                        text.TimeToAnswer = int.Parse(reader.GetString(4));
                        text.RightAnswer = reader.GetString(5).ToString();
                        text.Score = int.Parse(reader.GetString(6));
                        text.URL = reader.GetString(7).ToString();
                        quest = text;

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
     
        public static void AddClient (Client client)
        {
            DataBase.OpenAsync();
            SqliteCommand command = new SqliteCommand("INSERT INTO CLIENTS (CHATID, NAME, STAGE, LASTDATETIME, SCORE) VALUES (@ChatId, @Name, @Stage, @LastDateTime, @Score)", DataBase);

         //   if (client.ChatId != null)
            command.Parameters.AddWithValue("ChatId", client.ChatId);
           
         //   else command.Parameters.AddWithValue("PhoneNumber", -1);
            if (client.Name != null)
                command.Parameters.AddWithValue("Name", client.Name);
          //  else command.Parameters.AddWithValue("Name", -1);
            command.Parameters.AddWithValue("Stage", client.Stage);
            command.Parameters.AddWithValue("LastDateTime", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            command.Parameters.AddWithValue("Score", client.Score);
            try
            {
                command.ExecuteNonQuery();
                DataBase.CloseAsync();
            }
            catch (Exception ex)
            {
                Bot.MyLogger(ex.Message);
                Bot.MyLogger(ex.StackTrace);
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
            {
                Bot.MyLogger(ex.Message);
                Bot.MyLogger(ex.StackTrace);
                DataBase.CloseAsync();
            }
            DataBase.CloseAsync();
            return flag;

        }      
      
        public static Client GetClient(Client client)
        {
            Client cl=new Client();
            DataBase.OpenAsync();
            CultureInfo provider = CultureInfo.InvariantCulture;
            //SELECT EXISTS(SELECT 1 FROM myTbl WHERE u_tag="tag")

            SqliteCommand command = new SqliteCommand("SELECT * FROM CLIENTS WHERE CHATID = " + client.ChatId, DataBase);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cl.ChatId = long.Parse(reader.GetValue(0).ToString());
                    cl.Name = reader.GetValue(1).ToString();
                    cl.Stage = int.Parse(reader.GetValue(2).ToString());
                    cl.LastDateTime = DateTime.ParseExact(reader.GetValue(3).ToString(), "dd.MM.yyyy HH:mm:ss", provider);
                    cl.Score = int.Parse(reader.GetValue(4).ToString());
            }
            DataBase.CloseAsync();
            return cl;

        }

        public static void UpdateClient(Client client)
        {
            DataBase.OpenAsync();
            SqliteCommand command = new SqliteCommand("UPDATE CLIENTS SET  NAME = :name, STAGE = :stage, LASTDATETIME = :LastDateTime, SCORE = :Score where CHATID=:id", DataBase);
            
            command.Parameters.Add("name", SqliteType.Text).Value = client.Name;
            command.Parameters.Add("stage", SqliteType.Integer).Value = client.Stage;       
            command.Parameters.Add("LastDateTime", SqliteType.Text).Value= DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            command.Parameters.Add("Score",SqliteType.Integer).Value = client.Score;
            try
            {
                command.ExecuteNonQuery();
                DataBase.CloseAsync();
            }
            catch (Exception ex)
            {
               
                Bot.MyLogger(ex.Message);
                Bot.MyLogger(ex.StackTrace);
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
                command.Parameters.AddWithValue("date", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                command.ExecuteNonQuery();
                DataBase.CloseAsync();
            }
            catch (Exception ex)
            {
                
                Bot.MyLogger(ex.Message);
                Bot.MyLogger(ex.StackTrace);
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
      
    }
}
