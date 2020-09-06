namespace NinjaCsv
{
    public class CsvWriterOptions
    {
        private const string DefaultDelimiter = ",";

        public CsvWriterOptions()
        {
            _delimiter = DefaultDelimiter;
        }

        public bool UseObjectToStringMethod { get; set; }

        public string Delimiter
        {
            get => string.IsNullOrEmpty(_delimiter) ? _delimiter = DefaultDelimiter : _delimiter;
            set => _delimiter = value;
        }

        private string _delimiter;
    }
}