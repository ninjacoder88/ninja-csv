namespace Ninjasoft.Csv
{
    public sealed class HeaderRowOptions
    {
        public bool UseHeaderNames { get; set; }

        public string HeaderRowText { get; set; }

        internal bool AddHeaderRow => UseHeaderNames || !string.IsNullOrEmpty(HeaderRowText);
    }
}
