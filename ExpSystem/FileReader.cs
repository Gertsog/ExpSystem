using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpSystem
{
    class FileReader
    {
        public List<string> Title { get; set; }

        private bool check = true;
        public List<Hypothesis> hypotheses = new List<Hypothesis>();
        public List<Question> questions = new List<Question>();
        public List<Calculation> calculations = new List<Calculation>();
        public string dialog = "";

        public void readFile (string path)
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(path, Encoding.GetEncoding(1251));

            //Считывание заголовка
            List<string> lines = new List<string>();
            while (!string.IsNullOrEmpty(line = file.ReadLine()))
            {
                lines.Add(line);
            }
            lines.Remove(lines.Last());
            Title = lines;
            if (Title.Count > 2)
                check = false;

            //Считывание вопросов
            lines = new List<string>();
            while (!string.IsNullOrEmpty(line = file.ReadLine()))
            {
                lines.Add(line);
            }
            lines.Remove(lines.First());
            foreach (string l in lines)
            {
                questions.Add(new Question(l));
            }
            foreach (Question q in questions)
            {
                q.QuestionNumber = questions.IndexOf(q) + 1;
            }

            //Считывание гипотез 
            lines = new List<string>();
            while (!string.IsNullOrEmpty(line = file.ReadLine()))
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
                    hypotheses.Add(hypothesis);

                    for (int i = 2; i < split.Length - 3; i += 3)
                    {
                        int questionNumber = int.Parse(split[i]);
                        split[i+1] = split[i+1].Replace('.', ',');
                        float pPositive = float.Parse(split[i + 1]);
                        split[i+2] = split[i+2].Replace('.', ',');
                        float pNegative = float.Parse(split[i + 2]);

                        Calculation calculation = new Calculation(
                            hypothesis,
                            questions.First(q => q.QuestionNumber == questionNumber),
                            pPositive,
                            pNegative
                            );

                    }
                }
            }
            catch
            {
                check = false;
            }

            if (check == false)
                dialog = "something wrong";
            file.Close();
        }
    }
}
