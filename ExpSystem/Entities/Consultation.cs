using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ExpSystem
{
    public class Consultation
    {
        
        public Consultation()
        {

        }

        private int counter;

        public string Dialog { get; set; }

        public void count(FileReader fr)
        {
            if (counter > 0)
            {
                counter++;
                setQuestion(fr);
            }
        }

        public void startConsultation(FileReader fr)
        {
            counter = 1;
            setQuestion(fr);
        }

        public void setQuestion(FileReader fr)
        {
            if (counter == 0)
            {
                Dialog = "Начните консультацию";
            }
            if (fr.Questions.Any(q => q.QuestionNumber.Equals(counter)))
            {
                Question question = fr.Questions.First(q => q.QuestionNumber.Equals(counter));
                Dialog = question.QuestionNumber + ". " + question.QuestionText;
            }
            else
            {
                counter = 0;
                Dialog = "Консультация окончена";
            }
        }
        
        public bool setAnswer(FileReader fr, double answer)
        {
            if (counter == 0)
            {
                Dialog = "Начните консультацию";
                return false;
            }
            if ((fr.Questions.Count > 0) && (counter > 0))
            {
                Question question = fr.Questions.First(q => q.QuestionNumber.Equals(counter));
                question.Answer = answer;
                return true;
            }
            else
            {
                counter = 0;
                Dialog = "Консультация окончена";
                return false;
            }
        }

        public void calculate(FileReader fr)
        {
            double probability;
            Question question = fr.Questions.First(q => q.QuestionNumber.Equals(counter));
         
            foreach (Hypothesis h in fr.Hypotheses)
            {
                foreach(int q in h.questionNumbers)
                {
                    if (q == question.QuestionNumber)
                    {
                        int index = h.questionNumbers.IndexOf(q);
                        double pp = h.pPositive[index];
                        double pn = h.pNegative[index];
                        if(question.Answer < 0.5)
                        {
                            probability = ((1 - pp) * h.CurrentProbability) / ((1 - pp) * h.CurrentProbability + (1 - pn) * (1 - h.CurrentProbability));
                            h.CurrentProbability = probability + ((question.Answer * (h.CurrentProbability - probability)) / (0.5));
                        }
                        if (question.Answer > 0.5)
                        {
                            probability = (pp * h.CurrentProbability) / ((pp * h.CurrentProbability) + (pn * (1 - h.CurrentProbability)));
                            h.CurrentProbability = h.CurrentProbability + (((question.Answer - 0.5) * (probability - h.CurrentProbability)) / (0.5));
                        }
                    }
                }
            }
        }
    }
}
