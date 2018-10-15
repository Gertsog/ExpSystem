using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ExpSystem
{
    class Consultation
    {
        public string dialog;
        public delegate void myDelegate();
        public event myDelegate myEvent;

        FileReader fr = new FileReader();

        public Consultation()
        {

        }

        public void consultate()
        {
            for (int i = 0; i < fr.questions.Count(); i++)
            {
                dialog = fr.questions[i].QuestionNumber + ". " + fr.questions[i].QuestionText;
                myEvent();
            }


        }

        public void calculate(Question question)
        {
            float probability;

            if (question.Answer < 0.5)
            {
                foreach (Hypothesis h in fr.hypotheses)
                {
                    foreach(int q in h.questionNumbers)
                    {
                        if (q == question.QuestionNumber)
                        {
                            int index = h.questionNumbers.IndexOf(q);
                            float pp = h.pPositive[index];
                            float pn = h.pNegative[index];
                            probability = (((1 - pp) * h.DefaultProbability) / ((1 - pp) * h.DefaultProbability) + (1 - pn) * (1 - h.DefaultProbability));
                            h.CurrentProbability = (probability + question.Answer * (h.CurrentProbability - probability)) / (float) 0.5;
                        }
                    }
                }
            }
            if (question.Answer > 0.5)
            {
                foreach (Hypothesis h in fr.hypotheses)
                {
                    foreach (int q in h.questionNumbers)
                    {
                        if (q == question.QuestionNumber)
                        {
                            int index = h.questionNumbers.IndexOf(q);
                            float pp = h.pPositive[index];
                            float pn = h.pNegative[index];
                            probability = (pp * h.DefaultProbability) / (pp * h.DefaultProbability + pn * (1 - h.DefaultProbability));
                            h.CurrentProbability = (probability + (question.Answer - (float)0.5) * (probability - h.CurrentProbability)) / (float)0.5;
                        }
                    }
                }
            }
        }
    }
}
