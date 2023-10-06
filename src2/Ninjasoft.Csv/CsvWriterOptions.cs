using System;

namespace Ninjasoft.Csv
{
    public sealed class CsvWriterOptions
    {
        private const string DefaultDelimiter = ",";

        public CsvWriterOptions()
        {
            _delimiter = DefaultDelimiter;
        }

        public bool ConsiderNonPublic { get; set; }

        public string Delimiter
        {
            get => string.IsNullOrEmpty(_delimiter) ? _delimiter = DefaultDelimiter : _delimiter;
            set => _delimiter = value;
        }

        internal HeaderRowOptions HeaderRowOptions
        {
            get
            {
                if (_headerRowOptions == null)
                    CreateHeaderRow();
                return _headerRowOptions;
            }
        }

        public void CreateHeaderRow(Action<HeaderRowOptions> optionsConfig = null)
        {
            var options = new HeaderRowOptions();
            optionsConfig?.Invoke(options);
            options.UseHeaderNames = true;
            _headerRowOptions = options;
        }

        private string _delimiter;
        private HeaderRowOptions _headerRowOptions;
    }
}
