using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zw.JsonLogViewer.Parsing
{
    class LogFile
    {
        public List<LogEntry> Entries { get; protected set; }

        public List<string> Keys { get; protected set; }

        public string SourceFilename { get; protected set; }

        public LogFile()
        {
            this.Entries = new List<LogEntry>();
            this.Keys = new List<string>();
        }

        public LogFile(string sourceFilename) : this()
        {
            this.SourceFilename = sourceFilename;
        }
    }
}
