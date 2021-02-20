using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPCorpBot.Models
{
    public class Client
    {
        private long chatId;
        private string name;
        private int stage;  
        private DateTime lastDateTime;
        private int score;


        public long ChatId
        { 
          get { return chatId; } 
          set { //if(value!=n)
                chatId = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Stage
        {
            get { return stage; }
            set { stage = value; }
        }        

               
        public void NextStage(int text)
        {
            Stage = text;
        }
      
         public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public DateTime LastDateTime
        {
            get {return lastDateTime; }
            set { lastDateTime = value; }
        }
        public Client()
        {
            this.Stage = 0;          
            this.Name = "-1";
            this.score = 0;
        }
        
    }
}
