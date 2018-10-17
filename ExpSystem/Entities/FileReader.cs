using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ExpSystem
{
    public class FileReader
    {
        #region properties
        /// <summary>
        /// Лист строк заголовка базы знаний
        /// </summary>
        public List<string> Title { get; set; }

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

        /// <summary>
        /// Коллекция гипотез
        /// </summary>
        private List<Hypothesis> hypotheses = new List<Hypothesis>();
        public List<Hypothesis> Hypotheses
        {
            get { return hypotheses; }
            set
            {
                if (hypotheses != value)
                    hypotheses = value;
            }
        }

        /// <summary>
        /// Коллекция вопросов
        /// </summary>
        private List<Question> questions = new List<Question>();
        public List<Question> Questions
        {
            get { return questions; }
            set
            {
                if (questions != value)
                    questions = value;
            }
        }

        #endregion

        #region ctor

        public FileReader()
        {

        }

        #endregion

        #region methods

        /// <summary>
        /// Чтение данных из файла
        /// </summary>
        public bool ReadFile (string path)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(path, Encoding.GetEncoding(1251));
            if (readTitle(file) && readQuestions(file) && readHypotheses(file))
            {
                file.Close();
                return true;
            }
            else
            {
                file.Close();
                return false;
            }
        }

        /// <summary>
        /// Считывание заголовка файла
        /// </summary>
        private bool readTitle(System.IO.StreamReader stream)
        {
            string line;
            List<string> lines = new List<string>();
            while (!string.IsNullOrEmpty(line = stream.ReadLine()))
            {
                lines.Add(line);
            }
            if (lines.Count == 3)
            {
                lines.Remove(lines.Last());
                Title = lines;
                return true;
            }
            else
            {
                dialog = "Ошибка при попытке считывания заголовка";
                return false;
            }
        }

        /// <summary>
        /// Считывание вопросов
        /// </summary>
        private bool readQuestions(System.IO.StreamReader stream)
        {
            string line;
            List<string> lines = new List<string>();
            while (!string.IsNullOrEmpty(line = stream.ReadLine()))
            {
                lines.Add(line);
            }
            if (lines.Count <= 1)
            {
                dialog = "Ошибка при попытке считывания вопросов";
                return false;
            }
            lines.Remove(lines.First());
            List<Question> temp = new List<Question>();
            foreach (string l in lines)
            {
                temp.Add(new Question(l));
            }
            foreach (Question q in temp)
            {
                q.QuestionNumber = temp.IndexOf(q) + 1;
            }
            Questions = temp;
            return true;
        }

        /// <summary>
        /// Считывание гипотез
        /// </summary>
        private bool readHypotheses(System.IO.StreamReader stream)
        {
            string line;
            List<string> lines = new List<string>();
            List<Hypothesis> temp = new List<Hypothesis>();
            while (!string.IsNullOrEmpty(line = stream.ReadLine()))
            {
                lines.Add(line);
            }
            try
            {
                foreach (string l in lines)
                {
                    string[] split = l.Split(new char[] { ',' });
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
                    temp.Add(hypothesis);
                }
                Hypotheses = temp;
            }
            catch
            {
                dialog = "Ошибка при попытке считывания гипотез";
                return false;
            }
            return true;
        }

        #endregion
    }
}
