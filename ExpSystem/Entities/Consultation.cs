using ExpSystem.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ExpSystem
{
    public class Consultation
    {
        // счетчик вопросов
        private int counter;

        /// <summary>
        /// Текст для диалога с пользователем
        /// </summary>
        private string dialog;
        public string Dialog { get => dialog; set => dialog = value; }

        /// <summary>
        /// Строки заголовка
        /// </summary>
        private List<string> title; 
        public List<string> Title { get => title; set => title = value; }

        /// <summary>
        /// Коллекция гипотез
        /// </summary>
        private List<Hypothesis> hypotheses = new List<Hypothesis>();
        public List<Hypothesis> Hypotheses { get => hypotheses; set => hypotheses = value; }

        /// <summary>
        /// Коллекция вопросов
        /// </summary>
        private List<Question> questions = new List<Question>();
        public List<Question> Questions { get => questions; set => questions = value; }

        public Consultation()
        {

        }

        /// <summary>
        /// Начало консультации
        /// </summary>
        public void StartConsultation()
        {
            if (questions.Count >= 1)
            {
                counter = 1;
                SetQuestion();
            }
        }

        /// <summary>
        /// Установка конкретного вопроса в текстовое поле
        /// </summary>
        public void SetQuestion() {
            if (questions.Any(q => q.QuestionNumber.Equals(counter)))
            {
                Question question = questions.First(q => q.QuestionNumber.Equals(counter));
                Dialog = question.QuestionNumber + ". " + question.QuestionText;
            }
            else 
            {
                counter = 0;
                Dialog = Phrases.ConsultationEnded;
            }
        }

        /// <summary>
        /// Плучение ответа на конкретный вопрос
        /// </summary>
        public void SetAnswer(double answer)
        {
            if (counter == 0)
            {
                Dialog = Phrases.StartConsultation;
            }
            else
            {
                Question question = questions.First(q => q.QuestionNumber.Equals(counter));
                question.Answer = answer;
                Calculate();

                counter++;
                SetQuestion();
            }
        }

        /// <summary>
        /// Перерасчёт вероятностей
        /// </summary>
        public void Calculate()
        {
            double probability;
            Question question = questions.First(q => q.QuestionNumber.Equals(counter));
         
            foreach (Hypothesis h in hypotheses)
            {
                foreach(int questionNumber in h.questionNumbers)
                {
                    if (questionNumber == question.QuestionNumber)
                    {
                        int index = h.questionNumbers.IndexOf(questionNumber);
                        double pp = h.pPositive[index];
                        double pn = h.pNegative[index];

                        if (question.Answer < 0.5)
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