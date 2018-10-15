using Microsoft.Win32;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ExpSystem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private float answer;
        public string expSystemDB;
        FileReader fr = new FileReader();
        Consultation consultation = new Consultation();

        public MainWindow()
        {
            InitializeComponent();
            dialog.Text = "Откройте файл или перетащите его на форму";
        }

        /// <summary>
        /// Открытие файла по нажатию кнопки
        /// </summary>
        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Filter = "Text files (*.txt;*.mkb)|*.txt;*.mkb|All files (*.*)|*.*";
            if (fileDialog.ShowDialog() == true)
            {
                expSystemDB = fileDialog.FileName;
            }
            this.Title = "ExpSystem - " + System.IO.Path.GetFileName(expSystemDB);
            ReadData(expSystemDB);
        }

        /// <summary>
        /// Реализация Drag-and-Drop
        /// </summary>
        private void FileDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                e.Effects = DragDropEffects.All;
        }

        private void FileDragEnter(object sender, DragEventArgs e)
        {
            string droppedFile;
            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            droppedFile = droppedFiles.Last();
            if ((System.IO.Path.GetExtension(droppedFile).ToLowerInvariant() == ".txt") || (System.IO.Path.GetExtension(droppedFile).ToLowerInvariant() == ".mkb"))
            {
                expSystemDB = droppedFile;
                Title = "ExpSystem - " + System.IO.Path.GetFileName(expSystemDB);
                ReadData(expSystemDB);
            }
        }

        /// <summary>
        /// Начало консультации
        /// </summary>
        private void startConsultation_Click(object sender, RoutedEventArgs e)
        {
            if (!fr.hypotheses.Any())
            {
                dialog.Text = "Файл не выбран";
            }
            else
            {
                new Task(() => consultation.consultate());
            }
        }

        /// <summary>
        /// Обработка кнопки "Да"
        /// </summary>
        private void Click0(object sender, RoutedEventArgs e)
        {
            answer = (float)0;
        }

        /// <summary>
        /// Обработка кнопки "Скорее да"
        /// </summary>
        private void Click025(object sender, RoutedEventArgs e)
        { 
            answer = (float)0.25;
        }

        /// <summary>
        /// Обработка кнопки "Не знаю"
        /// </summary>
        private void Click05(object sender, RoutedEventArgs e)
        {
            answer = (float)0.5;
        }
        /// <summary>
        /// Обработка кнопки "Скорее нет"
        /// </summary>
        private void Click075(object sender, RoutedEventArgs e)
        {
            answer = (float)0.75;
        }
        /// <summary>
        /// Обработка кнопки "Нет"
        /// </summary>
        private void Click1(object sender, RoutedEventArgs e)
        {
            answer = (float)1;
        }

        /// <summary>
        /// Чтение данных из файла
        /// </summary>
        private void ReadData(string db)
        {
            if (fr.readFile(db))
            {
                dialog.Text = "Для начала консультации нажмите кнопку \"Старт\"";
                title.Text = "";
                foreach (string str in fr.Title)
                    title.Text += str + "\n";

                QuestionsGrid.ItemsSource = fr.questions;
                HypothesesGrid.ItemsSource = fr.hypotheses;
            }
            else
            {
                dialog.Text = fr.dialog;
            }
        }

    }
}
