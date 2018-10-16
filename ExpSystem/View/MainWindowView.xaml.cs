using System.Linq;
using System.Windows;

namespace ExpSystem.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        private MainWindowVM VM;

        public MainWindowView(MainWindowVM VM)
        {
            InitializeComponent();
            DataContext = this.VM = VM;
        }

        /// <summary>
        /// Реализация Drag-and-Drop
        /// </summary>
        private void FileDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                e.Effects = DragDropEffects.All;
        }

        private void FileDragDrop(object sender, DragEventArgs e)
        {
            string droppedFile;
            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            droppedFile = droppedFiles.Last();
            if ((System.IO.Path.GetExtension(droppedFile).ToLowerInvariant() == ".txt") || (System.IO.Path.GetExtension(droppedFile).ToLowerInvariant() == ".mkb"))
            {
                VM.ExpSystemDB = droppedFile;
                Title = "ExpSystem - " + System.IO.Path.GetFileName(VM.ExpSystemDB);
                FileReader fr = new FileReader();
                VM.ReadData(VM.ExpSystemDB);
            }
        }
    }
}
