using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Zw.JsonLogViewer.Parsing;

namespace Zw.JsonLogViewer.ViewModels
{
    public class LogViewModel : Screen
    {

        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Parsing.Parser defaultParser;
        private readonly BindableCollection<LogEntry> logEntries;
        private readonly ICollectionView logEntriesView;
        private string currentSearchText;
        private LogEntry selectedLogEntry;

        public string SelectionDisplay { get; set; }

        public ICollectionView LogEntries { get { return this.logEntriesView; } }

        public ColumnConfig ColumnConfig { get; protected set; }

        public LogEntry SelectedLogEntry
        {
            get { return this.selectedLogEntry; }
            set
            {
                if (Object.ReferenceEquals(this.selectedLogEntry, value)) return;
                this.selectedLogEntry = value;
                NotifyOfPropertyChange(() => SelectedLogEntry);
                UpdateSelectionDisplay();
            }
        }

        public LogViewModel()
        {
            this.defaultParser = new Parsing.Parser();
            this.ColumnConfig = new ColumnConfig();
            this.logEntries = new BindableCollection<LogEntry>();
            this.logEntriesView = CollectionViewSource.GetDefaultView(this.logEntries);
            this.logEntriesView.Filter = LogEntriesFilter;
        }

        internal void Search(string searchText)
        {
            this.currentSearchText = searchText;
            this.logEntriesView.Refresh();
        }

        internal async Task<bool> OpenLog(string filename)
        {
            log.DebugFormat("Opening log '{0}'", filename);

            this.currentSearchText = String.Empty;
            this.logEntries.Clear();

            var logfile = defaultParser.ParseLogFile(filename);
            if (logfile == null) return false;

            List<Column> columns = logfile.Keys.Select(ak => new Column() { Header = ak, DataField = String.Format("[{0}]", ak) }).ToList();
            this.ColumnConfig.Columns = columns;
            NotifyOfPropertyChange(() => ColumnConfig);

            this.logEntries.AddRange(logfile.Entries);

            return true;
        }

        protected bool LogEntriesFilter(object obj)
        {
            if (String.IsNullOrWhiteSpace(this.currentSearchText)) return true;
            LogEntry entry = obj as LogEntry;
            if (entry == null) return true;

            foreach (var kvp in entry)
            {
                var str = Convert.ToString(kvp.Value);
                if (str.Contains(this.currentSearchText)) return true;
            }

            return false;
        }

        protected void UpdateSelectionDisplay()
        {
            if (this.selectedLogEntry == null)
            {
                this.SelectionDisplay = String.Empty;
                return;
            }

            var json = JsonConvert.SerializeObject(this.selectedLogEntry, Formatting.Indented);
            this.SelectionDisplay = json;
        }

    }
}
