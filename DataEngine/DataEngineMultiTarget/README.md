## DataEngineMultiTarget
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-Service-Components-Samples/tree/master/DataEngine/DataEngineMultiTarget)
____
#### Shows how to use C1DataEngine in various combinations of frameworks/runtimes.
____
This sample uses the multi-target version of C1DataEngine to demonstrate its
usage in a variety of application contexts (console, Windows Forms, WPF) and
target frameworks (netstandard2.0, net452). The solution contains 12 projects
organized as follows:

Core

* ConsoleApp
* WinFormsApp
* WinFormsPivotApp
* WpfApp
* WpfPivotApp

Framework

* ConsoleApp
* WinFormsApp
* WinFormsCubeApp
* WinFormsPivotApp
* WpfApp
* WpfCubeApp
* WpfPivotApp

Projects in the Core folder target netstandard2.0. Projects in the Framework
folder target net452. For non-Windows platforms, only the ConsoleApp project
in the Core folder is usable.

ConsoleApp downloads data from a public OData source, populates a DataEngine
workspace, runs a query, and lists the results in CSV format. WinFormsApp
and WpfApp are similar except that the results are shown in a grid control.

WinFormsPivotApp and WpfPivotApp run the same query as the other projects,
but pass the data workspace to an instance of C1FlexPivotEngine, which is
used to create a pivot table view that is displayed in a grid control.

Both WinFormsCubeApp and WpfCubeApp do not use DataEngine workspaces, but
connect directly to a cube hosted on a public SQL Server Analysis Services
instance. C1FlexPivotEngine is also used here to create a pivot table view
that is displayed in a grid control.

Note that direct cube access is not possible when targeting netstandard2.0
because of the dependency on Microsoft.AnalysisServices.AdomdClient, which
is not compatible with netstandard2.0. The multi-target C1.DataEngine package
automatically references the appropriate AdomdClient package from nuget.org,
but only when added to a project that targets net452 or greater.

All non-cube projects link to Northwind.cs in the Shared folder, which
handles data download, workspace initialization, query execution, and pivot
view creation. Both cube projects link to AdventureWorks.cs in the Shared
folder, which handles cube connection and pivot view creation.
