using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HPCorpBot.Models.Interfaces;

namespace HPCorpBot.Models.Questions
{
    public class MusicQuestion : IQuestion
    {
        public int Stage { get; set; }      
        public string URL { get; set; }
        public int TimeToAnswer { get; set; }
        public string RightAnswer { get; set; }
        public int NextStage { get; set; }
        public string Message { get; set ; }
        public int Score { get; set; }
    }
}
