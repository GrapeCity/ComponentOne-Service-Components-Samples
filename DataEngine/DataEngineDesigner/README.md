## DataEngineDesigner
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-Service-Components-Samples/tree/master/DataEngine/DataEngineDesigner)
____
#### Implements a visual designer for C1DataEngine queries using Razor Pages.
____
This sample uses the RuntimeQuery class provided by the C1.DataEngine.Api
package to allow end-users to create and execute ad-hoc C1DataEngine queries.

The designer UI builds a JSON representation of a query and sends it to the
QueryData page using the POST method. The OnPostAsync handler receives an
equivalent RuntimeQuery argument produced by JSON serialization.

NOTE: This project requires Node.js. Either install the Node.js development
tools for Visual Studio, or perform a standalone installation of Node.js and
add the install directory to your PATH environment variable. For download
links, visit nodejs.org.

To run the project, open the .csproj file in Visual Studio Code and press F5.
Alternatively, you can open a command prompt, make the project directory
current, then type "dotnet run" to restore NuGet packages and build/run the
web server. The home page URL is https://localhost:5001.

The project's home page contains detailed usage instructions.
