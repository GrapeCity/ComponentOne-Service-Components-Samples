using System;
using C1.FlexPivot;

namespace AdventureWorks
{
    public class Cube
    {
        const string connectionString = @"Data Source=http://ssrs.componentone.com/OLAP/msmdpump.dll;Provider=msolap;Initial Catalog=AdventureWorksDW2012Multidimensional";
        const string cubeName = "Adventure Works";

        public static C1FlexPivotEngine GetPivotEngine()
        {
            C1FlexPivotEngine fp = new C1FlexPivotEngine();
            fp.ConnectCube(cubeName, connectionString);
            fp.BeginUpdate();
            fp.ColumnFields.Add("Color");
            fp.RowFields.Add("Category");
            fp.ValueFields.Add("Order Count");
            fp.EndUpdate();
            return fp;
        }

        public static C1FlexPivotEngine GetPivotEngine(Action<C1FlexPivotEngine> action)
        {
            C1FlexPivotEngine fp = new C1FlexPivotEngine();
            fp.ConnectCube(cubeName, connectionString);
            fp.UpdateCompleted += (s, e) =>
            {
                action.Invoke(fp);
            };
            fp.BeginUpdate();
            fp.ColumnFields.Add("Color");
            fp.RowFields.Add("Category");
            fp.ValueFields.Add("Order Count");
            fp.EndUpdate();
            return fp;
        }
    }
}