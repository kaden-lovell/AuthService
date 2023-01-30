using Serilog.Events;

namespace AuthService
{
    public class Logging
    {
        public int FileCountLimit { get; set; }

        public long FileSizeLimit { get; set; }

        public LogEventLevel LogLevel { get; set; }

        public string OutputTemplate { get; set; }

        public string PathFormat { get; set; }
    }
}