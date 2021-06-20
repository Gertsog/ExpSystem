using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpSystem.Entities
{
    class ConsultationFiller
    {
        /// <summary>
        /// Наличие ошибки при чтении
        /// </summary>
        private bool hasErrorOccured;
        public bool HasErrorOccured => hasErrorOccured;

        /// <summary>
        /// Наполняемая консультация
        /// </summary>
        private Consultation consultation;

        /// <summary>
        /// Файл для считывания данных
        /// </summary>
        private System.IO.StreamReader file;

        public ConsultationFiller(Consultation consultation)
        {
            this.consultation = consultation;
            hasErrorOccured = false;
        }

        /// <summary>
        /// Считывание данных из файла
        /// </summary>
        public void ReadData(string path)
        {
            file = new System.IO.StreamReader(path, Encoding.GetEncoding(1251));

            try
            {
                ReadTitle();
                if (!hasErrorOccured)
                {
                    ReadQuestions();
                }
                if (!hasErrorOccured)
                {
                    ReadHypotheses();
                }
                file.Close();
            }
            catch (Exception)
            {
                hasErrorOccured = true;
                consultation.Dialog = Phrases.FileReadingError;
            }
        }

        /// <summary>
        /// Считывание заголовка файла
        /// </summary>
        private void ReadTitle()
        {
            consultation.Title = FileReader.ReadFile(file);

            if (consultation.Title.Count == 3)
            {
                consultation.Title.Remove(consultation.Title.Last());
            }
            if (consultation.Title.Count != 2)
            {
                consultation.Dialog = Phrases.HeaderReadingError;
                hasErrorOccured = true;
            }
        }

        /// <summary>
        /// Считывание списка вопросов
        /// </summary>
        public void ReadQuestions()
        {
            List<string> questionLines = FileReader.ReadFile(file);
            if (questionLines.Count <= 1) {
                consultation.Dialog = Phrases.QuestionsReadingError;
                hasErrorOccured = true;
            }
            if (!hasErrorOccured)
            {
                consultation.Questions = new List<Question>();
                foreach (string questionLine in questionLines)
                {
                    consultation.Questions.Add(new Question(questionLine));
                }
                foreach (Question question in consultation.Questions)
                {
                    question.QuestionNumber = consultation.Questions.IndexOf(question) + 1;
                }
            }
        }

        /// <summary>
        /// Считывание списка гипотез
        /// </summary>
        public void ReadHypotheses()
        {
            List<string> hypothesesLines = FileReader.ReadFile(file);
            if (hypothesesLines.Count <= 1)
            {
                consultation.Dialog = Phrases.HypothesesReadingError;
                hasErrorOccured = true;
            }
            if (!hasErrorOccured)
            {
                consultation.Hypotheses = new List<Hypothesis>();
                try
                {
                    foreach (string hypothesesLine in hypothesesLines)
                    {
                        string[] split = hypothesesLine.Split(new char[] { ',' });
                        split[1] = split[1].Replace('.', ',');
                        Hypothesis hypothesis = new Hypothesis(split[0], float.Parse(split[1]));

                        for (int i = 2; i <= split.Length - 3; i += 3)
                        {
                            int questionNumber = int.Parse(split[i]);
                            split[i + 1] = split[i + 1].Replace('.', ',');
                            double pPositive = double.Parse(split[i + 1]);
                            split[i + 2] = split[i + 2].Replace('.', ',');
                            double pNegative = double.Parse(split[i + 2]);

                            hypothesis.questionNumbers.Add(questionNumber);
                            hypothesis.pPositive.Add(pPositive);
                            hypothesis.pNegative.Add(pNegative);
                        }
                        consultation.Hypotheses.Add(hypothesis);
                    }
                }
                catch
                {
                    consultation.Dialog = Phrases.HypothesesFillingError;
                    hasErrorOccured = true;
                }
            }
        }
    }
}