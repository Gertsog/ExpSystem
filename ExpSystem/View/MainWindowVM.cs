using ExpSystem.Entities;
using ExpSystem.MVVM;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ExpSystem.View
{
    public class MainWindowVM : NotifyPropertyChanged
    {
        #region properties

        private Consultation consultation;

        /// <summary>
        /// Заголовок окна
        /// </summary>
        private string windowTitle;
        public string WindowTitle
        {
            get { return windowTitle; }
            set
            {
                if (windowTitle != value)
                {
                    windowTitle = value;
                    OnPropertyChanged("WindowTitle");
                }
            }
        }

        /// <summary>
        /// Путь к файлу
        /// </summary>
        private string expSystemDB;
        public string ExpSystemDB
        {
            get { return expSystemDB; }
            set
            {
                if (expSystemDB != value)
                {
                    expSystemDB = value;
                    OnPropertyChanged("ExpSystemDB");
                }
            }
        }

        /// <summary>
        /// Заголовок базы знаний
        /// </summary>
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Сообщения для диалога с пользователем
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
                    OnPropertyChanged("Dialog");
                }
            }
        }

        /// <summary>
        /// Коллекция гипотез
        /// </summary>
        private ObservableCollection<Hypothesis> hypotheses;
        public ObservableCollection<Hypothesis> Hypotheses
        {
            get { return hypotheses; }
            set
            {
                if (hypotheses != value)
                {
                    hypotheses = value;
                    OnPropertyChanged("Hypotheses");
                }
            }
        }

        /// <summary>
        /// Коллекция вопросов
        /// </summary>
        private ObservableCollection<Question> questions;
        public ObservableCollection<Question> Questions
        {
            get { return questions; }
            set
            {
                if (questions != value)
                {
                    questions = value;
                    OnPropertyChanged("Questions");
                }
            }
        }

        #endregion

        #region ctor

        public MainWindowVM()
        {
            consultation = new Consultation();
            hypotheses = new ObservableCollection<Hypothesis>();
            questions = new ObservableCollection<Question>();
            WindowTitle = "ExpSystem";
            Dialog = Phrases.OpenFile;
        }

        #endregion

        #region commands

        /// <summary>
        /// Открытие файла по нажатию кнопки
        /// </summary>
        private ICommand openFile;
        public ICommand OpenFile
        {
            get
            {
                return openFile = openFile ?? new Command(() =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();

                    fileDialog.Multiselect = false;
                    fileDialog.Filter = "Text files (*.txt;*.mkb)|*.txt;*.mkb|All files (*.*)|*.*";
                    if (fileDialog.ShowDialog() == true)
                    {
                        expSystemDB = fileDialog.FileName;
                        WindowTitle = "ExpSystem - " + System.IO.Path.GetFileName(expSystemDB);
                        ParseData(expSystemDB);
                    }
                });
            }
        }

        /// <summary>
        /// Обработка кнопки "Старт"
        /// </summary>
        private ICommand startConsultation;
        public ICommand StartConsultation
        {
            get
            {
                return startConsultation = startConsultation ?? new Command(() =>
                {
                    if (consultation.Hypotheses.Count == 0)
                    {
                        Dialog = Phrases.NoFileSelected;
                    }
                    else
                    {
                        ParseData(expSystemDB); // для сброса параметров к исходным
                        consultation.StartConsultation();
                        Dialog = consultation.Dialog;
                    }
                });
            }
        }

        /// <summary>
        /// Обработка кнопки "Да"
        /// </summary>
        private ICommand click1;
        public ICommand Click1 => click1 = click1 ?? new Command(() => clickAnswer(1));

        /// <summary>
        /// Обработка кнопки "Скорее да"
        /// </summary>
        private ICommand click075;
        public ICommand Click075 => click075 = click075 ?? new Command(() => clickAnswer(0.75));
        

        /// <summary>
        /// Обработка кнопки "Не знаю"
        /// </summary>
        private ICommand click05;
        public ICommand Click05 => click05 = click05 ?? new Command(() => clickAnswer(0.5));
        

        /// <summary>
        /// Обработка кнопки "Скорее нет"
        /// </summary>
        private ICommand click025;
        public ICommand Click025 => click025 = click025 ?? new Command(() => clickAnswer(0.25));

        /// <summary>
        /// Обработка кнопки "Нет"
        /// </summary>
        private ICommand click0;
        public ICommand Click0 => click0 = click0 ?? new Command(() => clickAnswer(0));

        #endregion

        #region methods

        /// <summary>
        /// Чтение данных из файла
        /// </summary>
        public void ParseData(string db)
        {
            consultation = new Consultation();
            ConsultationFiller cf = new ConsultationFiller(consultation);
            cf.ReadData(db);

            if (!cf.HasErrorOccured)
            {
                Dialog = Phrases.PressStart;
                Title = "";

                foreach (string str in consultation.Title)
                    Title += str + "\n";

                fillHypotheses();
                fillQuestions();
            }
            else
            {
                Dialog = consultation.Dialog;
            }
        }
        private void clickAnswer(double answer)
        {
            consultation.SetAnswer(answer);
            Dialog = consultation.Dialog;
            fillHypotheses();
            fillQuestions();
        }

        /// <summary>
        /// Заполнение таблицы гипотез
        /// </summary>
        private void fillHypotheses()
        {
            List<Hypothesis> temp = consultation.Hypotheses.OrderByDescending(h => h.CurrentProbability).ToList();
            hypotheses.Clear();
            foreach (Hypothesis h in temp)
            {
                hypotheses.Add(h);
            }
        }

        /// <summary>
        /// Заполнение таблицы вопросов
        /// </summary>
        private void fillQuestions()
        {
            questions.Clear();
            foreach (Question q in consultation.Questions)
            {
                questions.Add(q);
            }
        }

        #endregion
    }
}
