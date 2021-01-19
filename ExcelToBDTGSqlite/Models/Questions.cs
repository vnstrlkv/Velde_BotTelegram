using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeldeBotTelegram.Models.Interfaces;
namespace VeldeBotTelegram.Models
{
    public class Question : IMessage
    {

        public int Stage { get; set; }
        public string Message { get; set; }

        public List<Answer> Answers = new List<Answer>();

        public Question()
        {
            Stage = -1;
            Message = null;
        }

        public Question(int stage, string message, Answer answer)
        {
            Stage = stage;
            Message = message;
            Answers.Add(answer);
        }

        public Question(int stage, string message)
        {
            Stage = stage;
            Message = message;

        }



    }

    public class Answer
    {
        public int IdQuestions { get; set; }
        public string RightAnswer { get; set; }

        public int NextStage { get; set; }

        public Answer(string rightAsnw, int nextStage)
        {
            //  IdQuestions = idQues;
            RightAnswer = rightAsnw;
            NextStage = nextStage;
        }
        public Answer(int idQues, string rightAsnw, int nextStage)
        {
            IdQuestions = idQues;
            RightAnswer = rightAsnw;
            NextStage = nextStage;
        }

    }
}

