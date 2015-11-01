using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zw.JsonLogViewer.Parsing
{
    class Parser
    {

        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Parser()
        {

        }

        public LogFile ParseLogFile(string filename)
        {
            List<LogEntry> entries = new List<LogEntry>();
            log.DebugFormat("Parsing log file '{0}'", filename);
            try
            {
                int lineCount = 0;
                string line;
                using (var stream = new StreamReader(filename))
                {
                    while ((line = stream.ReadLine()) != null)
                    {
                        var entryAttributes = JsonConvert.DeserializeObject<LogEntry>(line);
                        entries.Add(entryAttributes);
                        lineCount++;
                    }
                    stream.Close();
                }
                log.DebugFormat("Parsed {0} lines into {1} log entries", lineCount, entries.Count);

                var allKeys = entries.SelectMany(le => le.Keys).Distinct().ToArray();
                log.DebugFormat("Providing {0} keys as columns: {1}", allKeys.Length, String.Join(",", allKeys));

                LogFile logfile = new LogFile(filename);
                logfile.Entries.AddRange(entries);
                logfile.Keys.AddRange(allKeys);
                return logfile;
            }
            catch (Exception ex)
            {
                log.Error("Failed to parse log file", ex);
                return null;
            }
        }

    }
}
