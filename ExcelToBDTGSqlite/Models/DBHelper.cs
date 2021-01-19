using System;
using System.Collections.Generic;
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
                    gallery.UniqueName = reader.GetString(4);
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

       public static void AddClient (Client client)
        {
            DataBase.OpenAsync();
            SqliteCommand command = new SqliteCommand("INSERT INTO CLIENTS (CHATID, PHONENUMBER, NAME, STAGE) VALUES (@ChatId, @PhoneNumber, @Name, @Stage)", DataBase);

         //   if (client.ChatId != null)
            command.Parameters.AddWithValue("ChatId", client.ChatId);
            if (client.PhoneNumber != null)
                command.Parameters.AddWithValue("PhoneNumber", client.PhoneNumber);
            else command.Parameters.AddWithValue("PhoneNumber", -1);
            if (client.Name != null)
                command.Parameters.AddWithValue("Name", client.Name);
            else command.Parameters.AddWithValue("Name", -1);
            command.Parameters.AddWithValue("Stage", client.Stage);
            try
            {
                command.ExecuteNonQuery();
                DataBase.CloseAsync();
            }
            catch (Exception ex)
            {
            
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


        public static Client GetClient(Client client)
        {
            Client cl=new Client();
            DataBase.OpenAsync();

            //SELECT EXISTS(SELECT 1 FROM myTbl WHERE u_tag="tag")
           
                SqliteCommand command = new SqliteCommand("SELECT * FROM CLIENTS WHERE CHATID = " + client.ChatId, DataBase);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cl.ChatId = int.Parse(reader.GetValue(0).ToString());
                    cl.PhoneNumber = reader.GetValue(1).ToString();
                    cl.Name = reader.GetValue(2).ToString();
                    cl.Stage = int.Parse(reader.GetValue(3).ToString());
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
               
                DataBase.CloseAsync();
            }
        }

        public static void EraseTable()
        {
            DataBase.OpenAsync();
            SqliteCommand command = new SqliteCommand("DELETE FROM QUESTIONS", DataBase);
            command.ExecuteNonQuery();
            command = new SqliteCommand("DELETE FROM IMAGES", DataBase);
            command.ExecuteNonQuery();
            command = new SqliteCommand("DELETE FROM ANSWERS", DataBase);
            command.ExecuteNonQuery();
            DataBase.CloseAsync();
        }
      public static void AddMessqges(IMessage message)
        {
            DataBase.OpenAsync();
            switch (typeDict[message.GetType()])
            {

                case 5: //PhoneMessage - 4
                    {
                        Question question = (Question)message;

                        // SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO Book (Id, Title, Language, PublicationDate, Publisher, Edition, OfficialUrl, Description, EBookFormat) VALUES (?,?,?,?,?,?,?,?,?)", sql_con);
                        SqliteCommand command = new SqliteCommand("INSERT INTO QUESTIONS (TYPEMESSAGE, Stage, Message) VALUES (@TypeMessage, @Stage, @Message)", DataBase);
                        //   command.Parameters.Add(question.IdQuestion);
                        command.Parameters.AddWithValue("TypeMessage", typeDict[message.GetType()]);
                        command.Parameters.AddWithValue("Stage", question.Stage);
                        command.Parameters.AddWithValue("Message", question.Message);
                        command.ExecuteNonQuery();
                        foreach (Answer an in question.Answers)
                        {
                            SqliteCommand command2 = new SqliteCommand("INSERT INTO ANSWERS (IdQuestion, RightAnswer, NextStage) VALUES (@IdQuestion, @RightAnswer, @NextStage)", DataBase);

                            command2.Parameters.AddWithValue("IdQuestion", question.Stage);
                            command2.Parameters.AddWithValue("RightAnswer", an.RightAnswer);
                            command2.Parameters.AddWithValue("NextStage", an.NextStage);
                            command2.ExecuteNonQuery();
                        }
;

                        break;

                    }


                case 4: //PhoneMessage - 4
                    {
                        PhoneMessage question = (PhoneMessage)message;
                        SqliteCommand command = new SqliteCommand("INSERT INTO QUESTIONS (TYPEMESSAGE, Stage, Message, Nextstage) VALUES (@TypeMessage, @Stage, @Message, @Nextstage)", DataBase);
                        command.Parameters.AddWithValue("TypeMessage", typeDict[message.GetType()]);
                        command.Parameters.AddWithValue("Stage", question.Stage);
                        command.Parameters.AddWithValue("Message", question.Message);
                        command.Parameters.AddWithValue("Nextstage", question.NextStage);
                        command.ExecuteNonQuery();

                        break;
                       
                    }
                case 3: //InlineGallery - 3
                    {

                        InlineImageGallery question = (InlineImageGallery)message;

                        // SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO Book (Id, Title, Language, PublicationDate, Publisher, Edition, OfficialUrl, Description, EBookFormat) VALUES (?,?,?,?,?,?,?,?,?)", sql_con);
                        SqliteCommand command = new SqliteCommand("INSERT INTO QUESTIONS (TYPEMESSAGE, Stage, Message, NextStage) VALUES (@TypeMessage, @Stage, @Message, @NextStage)", DataBase);
                        //   command.Parameters.Add(question.IdQuestion);
                        command.Parameters.AddWithValue("TypeMessage", typeDict[message.GetType()]);
                        command.Parameters.AddWithValue("Stage", question.Stage);
                        command.Parameters.AddWithValue("Message", question.Message);
                        command.Parameters.AddWithValue("NextStage", question.NextStage);
                        command.ExecuteNonQuery();
                        foreach (ImageGallery an in question.ImageGalleries)
                        {
                            SqliteCommand command2 = new SqliteCommand("INSERT INTO IMAGES (StageImageGallery, URLImage, Description) VALUES (@Stage, @URL, @Description)", DataBase);
                            command2.Parameters.AddWithValue("Stage", question.Stage);
                            command2.Parameters.AddWithValue("URL", an.URLImage);
                            command2.Parameters.AddWithValue("Description", an.Descriptipon);
                            command2.ExecuteNonQuery();
                        }



                        break;
                    }
                case 2: //ImageMessage - 2
                    {

                        break;
                    }
                case 1: //Question - 1
                    {
                        Question question = (Question)message;

                        // SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO Book (Id, Title, Language, PublicationDate, Publisher, Edition, OfficialUrl, Description, EBookFormat) VALUES (?,?,?,?,?,?,?,?,?)", sql_con);
                        SqliteCommand command = new SqliteCommand("INSERT INTO QUESTIONS (TYPEMESSAGE, Stage, Message) VALUES (@TypeMessage, @Stage, @Message)", DataBase);
                        //   command.Parameters.Add(question.IdQuestion);
                        command.Parameters.AddWithValue("TypeMessage", typeDict[message.GetType()]);
                        command.Parameters.AddWithValue("Stage", question.Stage);
                        command.Parameters.AddWithValue("Message", question.Message);
                        command.ExecuteNonQuery();
                        foreach (Answer an in question.Answers)
                        {
                            SqliteCommand command2 = new SqliteCommand("INSERT INTO ANSWERS (IdQuestion, RightAnswer, NextStage) VALUES (@IdQuestion, @RightAnswer, @NextStage)", DataBase);
                            
                            command2.Parameters.AddWithValue("IdQuestion", question.Stage);
                            command2.Parameters.AddWithValue("RightAnswer", an.RightAnswer);
                            command2.Parameters.AddWithValue("NextStage", an.NextStage);
                            command2.ExecuteNonQuery();
                        }


                        break;
                    }
                case 0: //Text message - 0
                    {

                        TextMessage question = (TextMessage)message;
                        SqliteCommand command = new SqliteCommand("INSERT INTO QUESTIONS (TYPEMESSAGE, Stage, Message, Nextstage) VALUES (@TypeMessage, @Stage, @Message, @Nextstage)", DataBase);
                        command.Parameters.AddWithValue("TypeMessage", typeDict[message.GetType()]);
                        command.Parameters.AddWithValue("Stage", question.Stage);
                        command.Parameters.AddWithValue("Message", question.Message);
                        command.Parameters.AddWithValue("Nextstage", question.NextStage);
                        command.ExecuteNonQuery();                       
                        
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            DataBase.CloseAsync();
        }

    }
}
