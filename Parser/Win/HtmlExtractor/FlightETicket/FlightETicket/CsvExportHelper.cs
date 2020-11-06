using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using C1.TextParser;

namespace FlightETicket
{
    public static class CsvExportHelper
    {
        public static StringBuilder ExportDataTable(DataTable dataTable)
        {
            var stringBuilder = new StringBuilder();
            for (int column = 0; column < dataTable.Columns.Count; column++)
            {
                //add separator
                stringBuilder.Append(dataTable.Columns[column].ColumnName + ',');
            }
            //append new line
            stringBuilder.Append("rn");
            for (int rows = 0; rows < dataTable.Rows.Count; rows++)
            {
                for (int column = 0; column < dataTable.Columns.Count; column++)
                {
                    //add separator
                    stringBuilder.Append(dataTable.Rows[rows][column].ToString().Replace(",", ";") + ',');
                }
                //append new line
                stringBuilder.Append("rn");
            }
            return stringBuilder;
        }
        public static StringBuilder ExportList<T>(IEnumerable<T> list)
        {
            var stringBuilder = new StringBuilder();
            //Create Header Part
            var headerProperties = typeof(T).GetProperties();
            for (int i = 0; i < headerProperties.Length - 1; i++)
            {
                stringBuilder.Append(headerProperties[i].Name + ",");
            }
            var lastProp = headerProperties[headerProperties.Length - 1].Name;
            stringBuilder.Append(lastProp + Environment.NewLine);

            if (list == null) return stringBuilder;
            //Create Rows
            foreach (var item in list)
            {
                var rowValues = typeof(T).GetProperties();
                for (int i = 0; i < rowValues.Length; i++)
                {
                    var prop = rowValues[i];
                    var obj = prop.GetValue(item);
                    stringBuilder.Append(obj.ToCustomString() + ",");
                }
                stringBuilder.Append(Environment.NewLine);
            }
            return stringBuilder;
        }
    }
    public static class Extension
    {
        public static string ToCustomString(this object obj)
        {
            if (obj == null) return string.Empty;
            Type objType = obj.GetType();
            if (objType.IsPrimitive || objType == typeof(string))
            {
                return obj.ToString();
            }
            StringBuilder sb = new StringBuilder();
            if (objType.FullName.StartsWith("System.Collections.Generic.List"))
            {
                sb.Append('"');
                int i = 1;
                foreach (object child in (IList)obj)
                {
                    sb.Append(i);
                    sb.Append('\n');
                    sb.Append(child.ToCustomString());
                    sb.Append('\n');
                    i++;
                }
                sb.Append('"');
                return sb.ToString();
            }
            var objProperties = objType.GetProperties();

            for (int i = 0; i < objProperties.Length; i++)
            {
                var prop = objProperties[i];
                var obj1 = prop.GetValue(obj);
                sb.Append(prop.Name);
                sb.Append(" : ");
                sb.Append(obj1.ToCustomString());
                if (i < objProperties.Length - 1)
                    sb.Append('\n');
            }

            string val = sb.ToString();
            val = '"' + val.Replace("\"", string.Empty) + '"';
            return val;
        }
    }
}