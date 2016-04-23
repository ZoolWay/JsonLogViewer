using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Zw.JsonLogViewer.Events;

namespace Zw.JsonLogViewer.ViewModels
{
    public class ShellViewModel : Screen, IShell
    {

        private const string TITLE = "Zw.JsonLogViewer";

        private const int MAX_MRU_LENGTH = 10;

        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IEventAggregator eventAggregator;

        private readonly BindableCollection<string> mruFiles;

        private bool isStripMultiLinesInList;

        public bool IsAutoRefreshEnabled { get; set; }

        public bool IsLoadLastOnStartupEnabled { get; set; }

        public bool IsStripMultiLinesInList
        {
            get { return this.isStripMultiLinesInList; }
            set
            {
                if (this.isStripMultiLinesInList == value) return;
                this.isStripMultiLinesInList = value;
                if (this.LogView != null) this.LogView.ViewRefresh();
            }
        }

        public bool IsLoading { get; set; }

        public bool IsLogLoaded { get; set; }

        public string SearchText { get; set; }

        public string CurrentLogFile { get; set; }

        public BindableCollection<string> MruFiles
        {
            get { return this.mruFiles; }
        }

        public bool CanShowMruFiles
        {
            get { return true && this.mruFiles.Count > 0; }
        }

        public bool CanReload
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.CurrentLogFile);
            }
        }

        public LogViewModel LogView { get; protected set; }

        public ShellViewModel()
        {
            this.IsLoading = true;
            this.IsLogLoaded = false;
            this.eventAggregator = IoC.Get<IEventAggregator>();
            this.CurrentLogFile = null;
            this.mruFiles = new BindableCollection<string>();
            this.mruFiles.CollectionChanged += ((sender, e) => NotifyOfPropertyChange(() => CanShowMruFiles));
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
            OpenLogFile(ofd.FileName, false);
        }

        public void Reload()
        {
            OpenLogFile(this.CurrentLogFile, true);
        }

        public void ClearFilters()
        {
            this.SearchText = String.Empty;
            this.LogView.ClearFilters();
        }

        public void ResetDetailPanelKey()
        {
            this.eventAggregator.PublishOnUIThread(new SetDetailPanelKeyMessage(this, null));
        }

        public void ShowAllColumns()
        {
            this.LogView.ShowAllColumns();
        }

        public void OpenMruFile(string mruFile)
        {
            OpenLogFile(this.CurrentLogFile, false);
        }

        protected async override void OnInitialize()
        {
            this.LogView = new LogViewModel();
            this.DisplayName = TITLE;
            this.IsAutoRefreshEnabled = Properties.Settings.Default.AutoRefresh;
            this.IsLoadLastOnStartupEnabled = Properties.Settings.Default.LoadLastOnStartup;
            this.IsStripMultiLinesInList = Properties.Settings.Default.StripMultiLinesInList;
            string[] mruFilesArray = JsonConvert.DeserializeObject<string[]>(Properties.Settings.Default.MruFiles);
            this.mruFiles.AddRange(mruFilesArray);
            await Task.Delay(250);
            this.IsLoading = false;

            var lastLogFile = Properties.Settings.Default.LastLogfile;
            if ((this.IsLoadLastOnStartupEnabled) && (!String.IsNullOrWhiteSpace(lastLogFile)))
            {
                if (File.Exists(lastLogFile))
                {
                    log.InfoFormat("Loading last-used logfile: '{0}'", lastLogFile);
                    OpenLogFile(lastLogFile, false);
                }
                else
                {
                    log.WarnFormat("Could not open last-used logfile, does not seem to exist any more: '{0}'", lastLogFile);
                }
            }
        }

        protected override void OnActivate()
        {
            ScreenExtensions.TryActivate(this.LogView);
        }

        protected override void OnDeactivate(bool close)
        {
            ScreenExtensions.TryDeactivate(this.LogView, close);

            if (!close) return;

            Properties.Settings.Default.AutoRefresh = this.IsAutoRefreshEnabled;
            Properties.Settings.Default.LoadLastOnStartup = this.IsLoadLastOnStartupEnabled;
            Properties.Settings.Default.MruFiles = JsonConvert.SerializeObject(this.mruFiles.Take(MAX_MRU_LENGTH).ToArray(), Formatting.None);
            Properties.Settings.Default.StripMultiLinesInList = this.IsStripMultiLinesInList;
            Properties.Settings.Default.Save();
        }

        protected async void OpenLogFile(string filename, bool preserveFilters)
        {
            this.IsLoading = true;
            this.DisplayName = TITLE;
            try
            {
                FilterSet filters = null;
                if (preserveFilters) filters = this.LogView.GetCurrentFilters();
                bool openedLog = await Task.Run(() => this.LogView.OpenLog(filename));
                if (openedLog)
                {
                    this.DisplayName = String.Format("{0} [{1}]", TITLE, filename);
                    Properties.Settings.Default.LastLogfile = filename;
                    this.CurrentLogFile = filename;
                    if (!this.mruFiles.Contains(filename)) this.mruFiles.Insert(0, filename);
                }
                else
                {
                    this.CurrentLogFile = null;
                }
                if ((preserveFilters) && (filters != null))
                    this.LogView.ApplyFilters(filters);
                this.IsLogLoaded = openedLog;
            }
            finally
            {
                this.IsLoading = false;
            }
        }

    }
}