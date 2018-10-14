using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpSystem
{
    class Calculation
    {
        private Hypothesis hypothesis;
        private Question question;
        private float pPositive;
        private float pNegative;

        public Hypothesis cHypothesis;
        public Question cQuestion;
        public float PPositive;
        public float PNegative;

        public Calculation(Hypothesis hypothesis, Question question, float pPositive, float pNegative)
        {
            cHypothesis = hypothesis;
            cQuestion = question;
            PPositive = pPositive;
            PNegative = pNegative;
        }


    }
}
