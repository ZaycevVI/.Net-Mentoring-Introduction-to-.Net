using System;
using System.Globalization;
using System.Windows;
using FileLibrary.Configuration;
using FileLibrary.File;
using ApplicationContent = WpfApplication.Resources.Content;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SettingSection _settingSection;
        private readonly IFileNotifier _fileNotifier;

        public MainWindow()
        {
            var configuration = new Configuration();
            var fileOperation = new FileOperation();
            _fileNotifier = new FileNotifier(configuration, fileOperation);
            _settingSection = configuration.SettingSection;
            
            CultureInfo.CurrentUICulture =
                CultureInfo.CurrentCulture =
                    new CultureInfo(_settingSection.Culture);
            InitializeComponent();
            InitializeContent();
            _fileNotifier.SendMessage += LogMessage;
        }

        
        private void LogMessage(string msg)
        {
            Dispatcher.Invoke(() => LogLb.Items.Add(msg));
        }

        private void InitializeContent()
        {
            Title = ApplicationContent.Title + $" {DateTime.Now.ToString(CultureInfo.CurrentCulture)}";
            TrackedDirectoriesText.Text = ApplicationContent.TrackedDirectories;
            ListBox.ItemsSource = _settingSection.Directories;
            RulesLB.ItemsSource = _settingSection.Rules;
            DefaultText.Text = ApplicationContent.Default;
            DefaultTextValue.Text = _settingSection.DefaultDirectory.Path;
            OrderNumberEnabledText.Text = ApplicationContent.OrderNumber;
            OrderNumberEnabledTextValue.Text = _settingSection.OrderNumber.IsEnabled.ToString();
            DateEnabledText.Text = ApplicationContent.Date;
            DateEnabledTextValue.Text = _settingSection.Date.IsEnabled.ToString();
            RulesTextBlock.Text = ApplicationContent.RulesTextBlock;
        }

        protected override void OnClosed(EventArgs e)
        {
            _fileNotifier.Dispose();
            base.OnClosed(e);
        }
    }
}
