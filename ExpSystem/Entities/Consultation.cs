using System.Linq;

namespace ExpSystem
{
    public class Consultation
    {
        /// <summary>
        /// Текст для диалога с пользователем
        /// </summary>
        private string dialog;
        public string Dialog
        {
            get { return dialog; }
            set
            {
                if (dialog != value)
                {
                    dialog = value;
                }
            }
        }

        private int counter;

        public Consultation()
        {

        }

        /// <summary>
        /// Счётчик вопросов
        /// </summary>
        public void Count(FileReader fr)
        {
            if (counter > 0)
            {
                counter++;
                SetQuestion(fr);
            }
        }

        /// <summary>
        /// Начало консультации
        /// </summary>
        public void StartConsultation(FileReader fr)
        {
            counter = 1;
            SetQuestion(fr);
        }

        /// <summary>
        /// Установка конкретного вопроса в текстовое поле
        /// </summary>
        public void SetQuestion(FileReader fr)
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
        
        /// <summary>
        /// Плучение ответа на конкретный вопрос
        /// </summary>
        public bool SetAnswer(FileReader fr, double answer)
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

        /// <summary>
        /// Пересчёт вероятностей
        /// </summary>
        public void Calculate(FileReader fr)
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
