﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeldeBotTelegram.Models
{
    public class Client
    {
        private long chatId;
        private string phoneNumber;
        private string name;
        private int stage;
        private string metrica;

        public long ChatId
        { 
          get { return chatId; } 
          set { //if(value!=n)
                chatId = value; }
        }

        public int Stage
        {
            get { return stage; }
            set { stage = value; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set => phoneNumber = value;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Metrica
        {
            get { return metrica; }
            set { metrica = value; }
        }

        public void NextStage(int text)
        {
            Stage = text;
        }
        public Client()
        {
            this.Stage = 0;
            this.PhoneNumber = "-1";
            this.Name = "-1";
        }

    }
}
