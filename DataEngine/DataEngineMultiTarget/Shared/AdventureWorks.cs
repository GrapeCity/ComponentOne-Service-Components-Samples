using System;
using C1.PivotEngine;

namespace AdventureWorks
{
    public class Cube
    {
        const string connectionString = @"Data Source=http://ssrs.componentone.com/OLAP/msmdpump.dll;Provider=msolap;Initial Catalog=AdventureWorksDW2012Multidimensional";
        const string cubeName = "Adventure Works";

        public static C1PivotEngine GetPivotEngine()
        {
            C1PivotEngine pe = new C1PivotEngine();
            pe.ConnectCube(cubeName, connectionString);
            pe.BeginUpdate();
            pe.ColumnFields.Add("Color");
            pe.RowFields.Add("Category");
            pe.ValueFields.Add("Order Count");
            pe.EndUpdate();
            return pe;
        }

        public static C1PivotEngine GetPivotEngine(Action<C1PivotEngine> action)
        {
            C1PivotEngine pe = new C1PivotEngine();
            pe.ConnectCube(cubeName, connectionString);
            pe.UpdateCompleted += (s, e) =>
            {
                action.Invoke(pe);
            };
            pe.BeginUpdate();
            pe.ColumnFields.Add("Color");
            pe.RowFields.Add("Category");
            pe.ValueFields.Add("Order Count");
            pe.EndUpdate();
            return pe;
        }
    }
}