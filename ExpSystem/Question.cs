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
        private double answer;

        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public float Answer { get; set; }

        public void GetAnswer(float answer)
        {
            this.answer = answer;
        }

        public Question(string questionText)
        {
            QuestionText = questionText;
        }

        public void setAnswer (float answer)
        {

        }
    }
}
