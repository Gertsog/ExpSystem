using Microsoft.Win32;
using System.Linq;
using System.Windows;

namespace ExpSystem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string expSystemDB;
        
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
            if ((System.IO.Path.GetExtension(droppedFile).ToLowerInvariant() == ".txt") || (System.IO.Path.GetExtension(droppedFile).ToLowerInvariant() ==".mkb"))
            {
                expSystemDB = droppedFile;
                this.Title = "ExpSystem - " + System.IO.Path.GetFileName(expSystemDB);
                ReadData(expSystemDB);
            }
        }

        /// <summary>
        /// Чтение данных из файла
        /// </summary>
        private void ReadData(string db)
        {
            FileReader fr = new FileReader();
            fr.readFile(db);
            title.Text = "";
            foreach (string str in fr.Title)
                title.Text += str + "\n";

            QuestionsGrid.ItemsSource = fr.questions;
            HypothesesGrid.ItemsSource = fr.hypotheses;
            dialog.Text = fr.dialog;
        }

    }
}
