﻿using System;

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

            _headerRowOptions = options;
        }

        private string _delimiter;
        private HeaderRowOptions _headerRowOptions;
    }

    public class HeaderRowOptions
    {
        public bool UseHeaderNames { get; set; }

        public string HeaderRowText { get; set; }

        internal bool AddHeaderRow => UseHeaderNames || !string.IsNullOrEmpty(HeaderRowText);
    }
}
