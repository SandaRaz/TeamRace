using System.Globalization;
using System.Text;

namespace Evaluation.Models.UnitTest
{
    public class CustomLogger : ILogger
    {
        private readonly List<string> logMessages = new List<string>();

        public CustomLogger()
        {
            Console.SetOut(new StringWriter(new StringBuilder(), CultureInfo.InvariantCulture));
        }

        public void Log(string message)
        {
            logMessages.Add(message);
        }
        public IEnumerable<string> GetLogMessages()
        {
            return logMessages.Concat(Console.Out.ToString().Split(Environment.NewLine));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }
    }
}
