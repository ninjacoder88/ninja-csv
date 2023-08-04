namespace NinjaCsv
{
    public class CsvCreatorOptions
    {
        private const string DefaultDelimiter = ",";

        public CsvCreatorOptions()
        {
            _delimiter = DefaultDelimiter;
        }

        public bool ConsiderNonPublic { get; set; }

        public bool UseHeaderNames { get; set; }

        public string Delimiter
        {
            get => string.IsNullOrEmpty(_delimiter) ? _delimiter = DefaultDelimiter : _delimiter;
            set => _delimiter = value;
        }

        private string _delimiter;
    }
}
