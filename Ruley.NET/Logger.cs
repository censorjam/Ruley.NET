using Newtonsoft.Json;
using Ruley.NET.Logging;
using System;

namespace Ruley
{
    public class Logger
    {
        public Logger(string name, bool debugEnabled)
        {
            _logger = LogProvider.GetLogger(name);
            IsDebugEnabled = debugEnabled;
        }

        private ILog _logger;

        public bool IsDebugEnabled { get; internal set; }

        public void Debug(object o)
        {
            if (IsDebugEnabled)
                _logger.DebugFormat(JsonConvert.SerializeObject(o));
        }

        public void Debug(string msg, params object[] p)
        {
            if (IsDebugEnabled)
                _logger.DebugFormat(string.Format(msg, p));
        }

        public void Info(string msg, params object[] p)
        {
            if (p.Length > 0)
                _logger.InfoFormat(string.Format(msg, p));
            else
                _logger.Info(msg);
        }

        public void Error(Exception e)
        {
            _logger.ErrorFormat(e.ToString());
        }

        public void Error(string msg, params object[] p)
        {
            Console.WriteLine(string.Format(msg, p));
        }
    }
}