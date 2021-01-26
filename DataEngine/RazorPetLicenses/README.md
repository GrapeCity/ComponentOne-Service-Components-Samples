## RazorPetLicenses
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-Service-Components-Samples/tree/master/DataEngine/RazorPetLicenses)
____
#### Shows how to implement a Razor Pages app that visualizes C1DataEngine queries.
____
This sample is derived from the PetLicenses console app, which uses the .NET
Standard version of C1DataEngine to analyze a dataset of pet licenses for the
city of Seattle. See the PetLicenses readme for more information about the data
sources used to build queries.

This sample uses the C1.AspNetCore.MVC package for data visualization. The file
Index.cshtml uses the c1-dashboard-layout tag to implement a tiled display
containing a variety of charts and a grid. These visual controls are bound to
result sets provided by the OnGet handler in Index.cshtml.cs, which retrives
them from the C1DataEngine workspace.

Note the use of the ClassFactory.CreateFromDataList API to transform query data
into a collection of strongly typed objects. Also, since sort criteria are not
part of C1DataEngine queries, the DataList.Sort API is used where appropriate.
Both APIs are provided by the C1.DataEngine.Core.Api package.

To run the project, open the .csproj file in Visual Studio Code and press F5.
Alternatively, you can open a command prompt, make the project directory
current, then type "dotnet run" to restore NuGet packages and build/run the
web server. The home page URL is https://localhost:5001.
