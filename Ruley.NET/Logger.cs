using Newtonsoft.Json;
using Ruley.NET.Logging;
using System;

namespace Ruley
{
    public class Logger
    {
        private static ILog _logger = LogProvider.GetLogger("Ruley");

        public bool IsDebugEnabled { get; internal set; }

        public void Debug(object o)
        {
            _logger.DebugFormat(JsonConvert.SerializeObject(o));
        }

        public void Debug(string msg, params object[] p)
        {
            _logger.DebugFormat(string.Format(msg, p));
        }

        public void Info(string msg, params object[] p)
        {
            _logger.InfoFormat(string.Format(msg, p));
        }

        public void Error(Exception e)
        {
            _logger.ErrorException("", e);
        }

        public void Error(string msg, params object[] p)
        {
            Console.WriteLine(string.Format(msg, p));
        }
    }
}