using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Globalization;

namespace FIRE.X
{
    public class ImportResult<T>
    {
        public List<T> ImportRules { get; set; }
        public List<DateAmountSum> ChartData { get; set; }

        public DataTable ImportRulesAsDataTable()
        {
            var dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;

            // add the columns
            foreach(var property in typeof(T).GetProperties())
            {
                dt.Columns.Add(property.Name);
            }

            // add the rows
            foreach(var row in ImportRules)
            {
                object[] values = typeof(T).GetProperties().Select(f => f.GetValue(row)).ToArray();
                dt.Rows.Add(values);
            }

            return dt;
        }
    }
}
