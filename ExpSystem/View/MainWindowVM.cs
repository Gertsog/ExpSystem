using ExpSystem.MVVM;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ExpSystem.View
{
    public class MainWindowVM : NotifyPropertyChanged
    {
        FileReader fr = new FileReader();
        Consultation consultation = new Consultation();

        #region properties

        /// <summary>
        /// Заголовок окна
        /// </summary>
        private string windowTitle;
        public string WindowTitle
        {
            get { return windowTitle; }
            set
            {
                {
                    if (windowTitle != value)
                    {
                        windowTitle = value;
                        OnPropertyChanged("WindowTitle");
                    }
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
            hypotheses = new ObservableCollection<Hypothesis>();
            questions = new ObservableCollection<Question>();
            WindowTitle = "ExpSystem";
            Dialog = "Откройте файл или перетащите его на форму";
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
                        ReadData(expSystemDB);
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
                    if (fr.Hypotheses.Count == 0)
                    {
                        Dialog = "Файл не выбран";
                    }
                    else
                    {
                        ReadData(expSystemDB);
                        consultation.StartConsultation(fr);
                        Dialog = consultation.Dialog;
                    }
                });
            }
        }

        /// <summary>
        /// Обработка кнопки "Да"
        /// </summary>
        private ICommand click1;
        public ICommand Click1
        {
            get
            {
                return click1 = click1 ?? new Command(() =>
                {
                    if (consultation.SetAnswer(fr, 1))
                    {
                        consultation.Calculate(fr);
                        consultation.Count(fr);
                        fillHypotheses();
                        fillQuestions();
                    }
                    Dialog = consultation.Dialog;
                });
            }
        }

        /// <summary>
        /// Обработка кнопки "Скорее да"
        /// </summary>
        private ICommand click075;
        public ICommand Click075
        {
            get
            {
                return click075 = click075 ?? new Command(() =>
                {
                    if (consultation.SetAnswer(fr, 0.75))
                    {
                        consultation.Calculate(fr);
                        consultation.Count(fr);
                        fillHypotheses();
                        fillQuestions();
                    }
                    Dialog = consultation.Dialog;
                });
            }
        }

        /// <summary>
        /// Обработка кнопки "Не знаю"
        /// </summary>
        private ICommand click05;
        public ICommand Click05
        {
            get
            {
                return click05 = click05 ?? new Command(() =>
                {
                    if (consultation.SetAnswer(fr, 0.5))
                    {
                        consultation.Calculate(fr);
                        consultation.Count(fr);
                        fillHypotheses();
                        fillQuestions();
                    }
                    Dialog = consultation.Dialog;
                });
            }
        }

        /// <summary>
        /// Обработка кнопки "Скорее нет"
        /// </summary>
        private ICommand click025;
        public ICommand Click025
        {
            get
            {
                return click025 = click025 ?? new Command(() =>
                {
                    if (consultation.SetAnswer(fr, 0.25))
                    {
                        consultation.Calculate(fr);
                        consultation.Count(fr);
                        fillHypotheses();
                        fillQuestions();
                    }
                    Dialog = consultation.Dialog;
                });
            }
        }

        /// <summary>
        /// Обработка кнопки "Нет"
        /// </summary>
        private ICommand click0;
        public ICommand Click0
        {
            get
            {
                return click0 = click0 ?? new Command(() =>
                {
                    if (consultation.SetAnswer(fr, 0))
                    {
                        consultation.Calculate(fr);
                        consultation.Count(fr);
                        fillHypotheses();
                        fillQuestions();
                    }
                    Dialog = consultation.Dialog;
                });
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Чтение данных из файла
        /// </summary>
        public void ReadData(string db)
        {
            if (fr.ReadFile(db))
            {
                Dialog = "Для начала консультации нажмите кнопку \"Старт\"";
                Title = "";
                foreach (string str in fr.Title)
                    Title += str + "\n";

                fillHypotheses();
                fillQuestions();
            }
            else
            {
                Dialog = fr.Dialog;
            }
        }

        /// <summary>
        /// Заполнение таблицы гипотез
        /// </summary>
        private void fillHypotheses()
        {
            hypotheses.Clear();
            foreach (Hypothesis h in fr.Hypotheses)
            {
                hypotheses.Add(h);
            }
            OnPropertyChanged("Hypotheses");
        }

        /// <summary>
        /// Заполнение таблицы вопросов
        /// </summary>
        private void fillQuestions()
        {
            questions.Clear();
            foreach (Question q in fr.Questions)
            {
                questions.Add(q);
            }
            OnPropertyChanged("Questions");
        }

        #endregion
    }
}
