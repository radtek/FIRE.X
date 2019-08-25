using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Globalization;

namespace FIRE.X
{
    public class ImportResult<T>
    {
        public Type RealType { get; set; }

        public List<T> ImportRules { get; set; }

        public DataTable ImportRulesAsDataTable()
        {
            var dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;

            // add the columns
            foreach(var property in RealType.GetProperties())
            {
                dt.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            // add the rows
            foreach(var row in ImportRules)
            {
                object[] values = RealType.GetProperties().Select(f => f.GetValue(row)).ToArray();
                dt.Rows.Add(values);
            }

            return dt;
        }
    }
}
