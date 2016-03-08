using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Zw.JsonLogViewer.ViewModels
{
    public class ShellViewModel : Screen, IShell
    {

        private const string TITLE = "Zw.JsonLogViewer";

        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsAutoRefreshEnabled { get; set; }

        public bool IsLoadLastOnStartupEnabled { get; set; }

        public bool IsLoading { get; set; }

        public bool IsLogLoaded { get; set; }

        public string SearchText { get; set; }

        public LogViewModel LogView { get; protected set; }

        public ShellViewModel()
        {
            this.IsLoading = true;
            this.IsLogLoaded = false;
        }

        public void CloseApplication()
        {
            TryClose();
        }

        public void Search()
        {
            this.LogView.Search(this.SearchText);
        }

        public void OpenLog()
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            if (!ofd.ShowDialog().GetValueOrDefault(false)) return;
            log.DebugFormat("Selected file '{0}'", ofd.FileName);
            OpenLogFile(ofd.FileName);
        }

        protected async override void OnInitialize()
        {
            this.LogView = new LogViewModel();
            this.DisplayName = TITLE;
            this.IsAutoRefreshEnabled = Properties.Settings.Default.AutoRefresh;
            this.IsLoadLastOnStartupEnabled = Properties.Settings.Default.LoadLastOnStartup;
            await Task.Delay(250);
            this.IsLoading = false;

            var lastLogFile = Properties.Settings.Default.LastLogfile;
            if ((this.IsLoadLastOnStartupEnabled) && (!String.IsNullOrWhiteSpace(lastLogFile)))
            {
                if (File.Exists(lastLogFile))
                {
                    log.InfoFormat("Loading last-used logfile: '{0}'", lastLogFile);
                    OpenLogFile(lastLogFile);
                }
                else
                {
                    log.WarnFormat("Could not open last-used logfile, does not seem to exist any more: '{0}'", lastLogFile);
                }
            }
        }

        protected override void OnDeactivate(bool close)
        {
            if (!close) return;

            Properties.Settings.Default.AutoRefresh = this.IsAutoRefreshEnabled;
            Properties.Settings.Default.LoadLastOnStartup = this.IsLoadLastOnStartupEnabled;
            Properties.Settings.Default.Save();
        }

        protected async void OpenLogFile(string filename)
        {
            this.IsLoading = true;
            this.DisplayName = TITLE;
            try
            {
                bool openedLog = await Task.Run(() => this.LogView.OpenLog(filename));
                if (openedLog)
                {
                    this.DisplayName = String.Format("{0} [{1}]", TITLE, filename);
                    Properties.Settings.Default.LastLogfile = filename;
                }
                this.IsLogLoaded = openedLog;
            }
            finally
            {
                this.IsLoading = false;
            }
        }

    }
}