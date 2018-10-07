using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Library;
using Library.EventArgs;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool IsButtonEnabled { get; set; } = true;
        public bool IsSearchStopped { get; set; } = true;

        public MainWindow()
        {
            InitializeComponent();
            EnterButton.DataContext = this;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            IsButtonEnabled = false;

            ClearControls();

            var path = TextBox.Text;

            if (string.IsNullOrEmpty(path))
                return;

            await Task.Run(() =>
            {
                try
                {
                    var fileSystemVisitor = new FileSystemVisitor(path);
                    AddSubscribers(fileSystemVisitor);
                    fileSystemVisitor.GenerateDirectoryTree();
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => ExceptionLabel.Content = $"[Error Message]:{ex.Message}");
                    IsButtonEnabled = true;

                }

                Dispatcher.Invoke(() => IsButtonEnabled = true);
            });
        }

        private void AddSubscribers(FileSystemVisitor fileSystemVisitor)
        {
            fileSystemVisitor.Start += FileVisitorHandler;

            fileSystemVisitor.FileFinded += FileVisitorHandler;

            fileSystemVisitor.DirectoryFinded += FileVisitorHandler;

            fileSystemVisitor.Finish += FileVisitorHandler;
        }

        private void FileVisitorHandler(object obj, FileSystemVisitorBaseArg arg)
        {
            if (arg is ItemFindedArg<FileSystemInfo> itemFindedArg && IsSearchStopped)
                itemFindedArg.StopSearch = true;

            Dispatcher.Invoke(() => ListBox.Items.Add(arg.Message));
        }

        private void ClearControls()
        {
            IsSearchStopped = false;
            ListBox.Items.Clear();
            ExceptionLabel.Content = string.Empty;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            IsSearchStopped = true;
        }
    }
}
