using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        private readonly CultureInfo culture;
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
            this.culture = CultureInfo.InvariantCulture;
        }

        internal void Search(string searchText)
        {
            this.currentSearchText = searchText;
            this.logEntriesView.Refresh();
        }

        internal bool OpenLog(string filename)
        {
            log.DebugFormat("Opening log '{0}'", filename);

            this.currentSearchText = String.Empty;
            this.logEntries.Clear();

            var logfile = defaultParser.ParseLogFile(filename);
            if (logfile == null) return false;

            if (this.ColumnConfig.Columns != null)
            {
                foreach (var column in this.ColumnConfig.Columns)
                {
                    column.PropertyChanged -= NotifyColumnChanged;
                }
            }

            List<Column> columns = logfile.Keys.Select(ak => new Column() { Header = ak, DataField = String.Format("[{0}]", ak), EntryKey = ak }).ToList();
            this.ColumnConfig.Columns = columns;

            foreach (var column in columns)
            {
                column.PropertyChanged += NotifyColumnChanged;
            }

            NotifyOfPropertyChange(() => ColumnConfig);

            this.logEntries.AddRange(logfile.Entries);

            return true;
        }

        private void NotifyColumnChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FilterValue")
            {
                this.logEntriesView.Refresh();
            }
        }

        protected bool LogEntriesFilter(object obj)
        {
            LogEntry entry = obj as LogEntry;
            if (entry == null) return true;

            var relevantColums = this.ColumnConfig.Columns.Where(c => !String.IsNullOrWhiteSpace(c.FilterValue));
            if (relevantColums.Any())
            {
                foreach (var relevantColumn in relevantColums)
                {
                    if (!entry.ContainsKey(relevantColumn.EntryKey)) return false;
                    var entryValue = Convert.ToString(entry[relevantColumn.EntryKey], this.culture);
                    int idx = this.culture.CompareInfo.IndexOf(entryValue, relevantColumn.FilterValue, CompareOptions.IgnoreCase);
                    if (idx < 0) return false;
                }
            }

            if (String.IsNullOrWhiteSpace(this.currentSearchText)) return true;
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
