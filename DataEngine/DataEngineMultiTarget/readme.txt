DataEngineMultiTarget
---------------------
Shows how to use C1DataEngine in various combinations of frameworks/runtimes.

This sample uses multi-target versions of C1DataEngine and C1PivotEngine to
demonstrate their usage in a variety of application contexts (console,
Windows Forms, WPF) and target frameworks (net6.0, net452). The solution
contains 14 projects organized as follows:

Core
- ConsoleApp
- WinFormsApp
- WinFormsCubeApp
- WinFormsPivotApp
- WpfApp
- WpfCubeApp
- WpfPivotApp

Framework
- ConsoleApp
- WinFormsApp
- WinFormsCubeApp
- WinFormsPivotApp
- WpfApp
- WpfCubeApp
- WpfPivotApp

Projects in the Core folder target net6.0. Projects in the Framework folder
target net452. For non-Windows platforms, only the ConsoleApp project in the
Core folder is usable.

ConsoleApp downloads data from a public OData source, populates a DataEngine
workspace, runs a query, and lists the results in CSV format. WinFormsApp
and WpfApp are similar except that the results are shown in a grid control.

WinFormsPivotApp and WpfPivotApp run the same query as the other projects,
but pass the data workspace to an instance of the C1PivotEngine class, which
is used to create a pivot table view that is displayed in a grid control.

Both WinFormsCubeApp and WpfCubeApp do not use DataEngine workspaces, but
connect directly to a cube hosted on a public SQL Server Analysis Services
instance. C1PivotEngine is also used here to create a pivot table view that
is displayed in a grid control.

UPDATE: As of 2021 v1, direct cube access is available when targeting .NET
Core (netcoreapp3.0 or greater). Previous versions of C1.DataEngine only
supported direct cube access when targeting .NET Framework (net452 or greater).
Also, pivot table and cube functionality has been moved to a separate package,
C1.PivotEngine.

All non-cube projects link to Northwind.cs in the Shared folder, which
handles data download, workspace initialization, query execution, and pivot
view creation. All cube projects link to AdventureWorks.cs in the Shared
folder, which handles cube connection and pivot view creation.
