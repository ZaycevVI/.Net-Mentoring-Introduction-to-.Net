using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using ApplicationContent = WpfApplication.Resources.Content;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string SettingSectionKey = "settingSection";
        private readonly SettingSection _settingSection;
        private readonly List<(Regex template, string destination)> _rules;
        private readonly List<FileSystemWatcher> _fileSystemWatchers;

        public MainWindow()
        {
            _fileSystemWatchers = new List<FileSystemWatcher>();
            _rules = new List<(Regex template, string destination)>();
            _settingSection = ReadSettings();
            CultureInfo.CurrentUICulture =
                CultureInfo.CurrentCulture =
                    new CultureInfo(_settingSection.Culture);
            InitializeComponent();
            UpdateContent();
            UpdateDirectories();
            UpdateRules();
        }

        private void FileSystemWatcherOnCreated(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            var rule = _rules.FirstOrDefault(r => r.template.IsMatch(fileSystemEventArgs.Name));
            var destination = rule.destination;

            if (rule.destination == null)
            {
                LogMessage($"No rules were found for file: {fileSystemEventArgs.Name}.");
                destination = _settingSection.DefaultDirectory.Path;
            }

            if (!System.IO.Directory.Exists(destination))
                SafeRun(() =>
                {
                    System.IO.Directory.CreateDirectory(destination);
                    LogMessage($"Create directory by path: {destination}.");
                });

            var fileName = string.Empty;

            if (_settingSection.OrderNumber.IsEnabled)
            {
                var counter = System.IO.Directory.GetFileSystemEntries(destination).Length + 1;
                fileName = $"{counter}. " + fileSystemEventArgs.Name;
            }

            if (_settingSection.Date.IsEnabled)
            {
                fileName = Path.GetFileNameWithoutExtension(fileName) +
                           $" [{DateTime.Now.ToShortDateString()}]" +
                           Path.GetExtension(fileName);
            }

            var path = Path.Combine(destination, fileName);

            if (System.IO.File.Exists(path))
            {
                LogMessage($"File already exists: {path}.");
                return;
            }

            try
            {
                System.IO.File.Copy(fileSystemEventArgs.FullPath, path);
                LogMessage($"File [{path}] was successfully copied");
            }
            catch (Exception e)
            {
                LogMessage(e.Message + $". File: {path}");
            }
        }

        private void LogMessage(string msg)
        {
            Dispatcher.Invoke(() => LogLb.Items.Add(msg + $" ([{DateTime.Now.ToString(CultureInfo.CurrentCulture)}])"));
        }

        private SettingSection ReadSettings()
        {
            return ConfigurationManager.GetSection(SettingSectionKey)
                as SettingSection;
        }

        private void UpdateContent()
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

        private void UpdateRules()
        {
            foreach (var rule in _settingSection.Rules)
            {
                var fileRule = rule as File;

                _rules.Add((new Regex(fileRule.Template), fileRule.Destination));
            }
        }

        private void UpdateDirectories()
        {
            foreach (var directory in _settingSection.Directories)
            {
                var path = (directory as Directory).Name;

                if (!System.IO.Directory.Exists(path))
                {
                    SafeRun(() =>
                    {
                        System.IO.Directory.CreateDirectory(path);
                        LogMessage($"Create directory by path: {path}.");
                    });
                }

                var fileSystemWatcher =
                    new FileSystemWatcher(path) { EnableRaisingEvents = true };

                fileSystemWatcher.Created += FileSystemWatcherOnCreated;
                _fileSystemWatchers.Add(fileSystemWatcher);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            foreach (var fileSystemWatcher in _fileSystemWatchers)
            {
                fileSystemWatcher.Created -= FileSystemWatcherOnCreated;
                fileSystemWatcher.Dispose();
            }

            base.OnClosed(e);
        }

        private void SafeRun(Action action)
        {
            try
            {
                action.Invoke();
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Close();
            }
        }
    }
}
