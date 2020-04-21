using System;

namespace NinjaCsv.Common
{
    public class Column : Attribute
    {
		public Column(int position)
        {
            if(position < 0)
                throw new ArgumentException($"{position} cannot be negative");

            Position = position;
        }

        public int Position { get; }
    }
}