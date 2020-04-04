using System;

namespace NinjaCsv
{
    public class Column : Attribute
    {
		public Column(int position)
        {
            Position = position;
        }

        public int Position { get; }
    }
}