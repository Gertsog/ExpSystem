using System.Linq;
using System.Windows;

namespace ExpSystem.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        private MainWindowVM vm;

        public MainWindowView(MainWindowVM vm)
        {
            InitializeComponent();
            DataContext = this.vm = vm;
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
                vm.ExpSystemDB = droppedFile;
                Title = "ExpSystem - " + System.IO.Path.GetFileName(vm.ExpSystemDB);
                vm.ReadData(vm.ExpSystemDB);
            }
        }
    }
}
