using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpSystem
{
    class Question
    {
        private int questionNumber;
        private string questionText;
        private string answer;

        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public string Answer { get; set; }


        public Question(string questionText)
        {
            QuestionText = questionText;
        }

    }
}
