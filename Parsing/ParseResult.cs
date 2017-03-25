using System;

namespace Zw.JsonLogViewer.Parsing
{
    class ParseResult<TLogFile> where TLogFile : LogFile
    {
        public TLogFile LogFile { get; private set; }

        public bool Success { get; private set; }

        public string ErrorMessage { get; private set; }

        public Exception Exception { get; private set; }

        public ParseResult(TLogFile logFile)
        {
            this.Success = true;
            this.LogFile = logFile;
        }

        public ParseResult(string errorMessage, Exception exception = null)
        {
            this.Success = false;
            this.ErrorMessage = errorMessage;
            this.Exception = exception;
        }
    }
}
