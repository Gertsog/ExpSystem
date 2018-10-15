﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ExpSystem
{
    class FileReader
    {
        public List<string> Title { get; set; }

        public string dialog;
        public string line;
        public List<string> lines;
        public ObservableCollection<Hypothesis> hypotheses;
        public ObservableCollection<Question> questions;

        public bool readFile (string path)
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

        public bool readTitle(System.IO.StreamReader stream)
        {
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

        public bool readQuestions(System.IO.StreamReader stream)
        {

            lines = new List<string>();
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
            ObservableCollection<Question> temp = new ObservableCollection<Question>();
            foreach (string l in lines)
            {
                temp.Add(new Question(l));
            }
            foreach (Question q in temp)
            {
                q.QuestionNumber = temp.IndexOf(q) + 1;
            }
            questions = temp;
            return true;
        }

        public bool readHypotheses(System.IO.StreamReader stream)
        {
            lines = new List<string>();
            ObservableCollection<Hypothesis> temp = new ObservableCollection<Hypothesis>();
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

                    for (int i = 2; i < split.Length - 3; i += 3)
                    {
                        int questionNumber = int.Parse(split[i]);
                        split[i + 1] = split[i + 1].Replace('.', ',');
                        float pPositive = float.Parse(split[i + 1]);
                        split[i + 2] = split[i + 2].Replace('.', ',');
                        float pNegative = float.Parse(split[i + 2]);

                        hypothesis.questionNumbers.Add(questionNumber);
                        hypothesis.pPositive.Add(pPositive);
                        hypothesis.pNegative.Add(pNegative);
                    }
                    temp.Add(hypothesis);
                }
                hypotheses = temp;
            }
            catch
            {
                dialog = "Ошибка при попытке считывания гипотез";
                return false;
            }
            return true;
        }
    }
}
