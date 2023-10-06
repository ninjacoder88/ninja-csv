namespace Ninjasoft.Csv
{
    public sealed class CsvReaderOptions
    {
        private const string DefaultDelimiter = ",";

        public CsvReaderOptions()
        {
            _delimiter = DefaultDelimiter;
            ContainsHeaderRow = true;
        }

        public bool ConsiderNonPublic { get; set; }

        public bool ContainsHeaderRow { get; set; }

        public string Delimiter
        {
            get => string.IsNullOrEmpty(_delimiter) ? _delimiter = DefaultDelimiter : _delimiter;
            set => _delimiter = value;
        }

        private string _delimiter;
    }
}
