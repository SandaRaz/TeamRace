using System.Text;

namespace Evaluation.Models.UnitTest
{
    public class ConsoleInterceptor : TextWriter
    {
        private readonly StringBuilder  _stringBuilder = new StringBuilder();
        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value)
        {
            _stringBuilder.Append(value).Append(Environment.NewLine);
            base.Write(value);
        }
        public override void Write(string? value)
        {
            _stringBuilder.Append(value).Append(Environment.NewLine);
            base.Write(value);
        }
        public string GetOutput()
        {
            return _stringBuilder.ToString();
        }
    }
}
