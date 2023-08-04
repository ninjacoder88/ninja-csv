using System;

namespace NinjaCsv.Common
{
    public class Column : Attribute
    {
		public Column(int position, string headerName = null)
        {
            if(position < 0)
                throw new ArgumentException($"{position} cannot be negative");

            Position = position;
            HeaderName = headerName;
        }

        public int Position { get; }

        public string HeaderName { get; }
    }
}