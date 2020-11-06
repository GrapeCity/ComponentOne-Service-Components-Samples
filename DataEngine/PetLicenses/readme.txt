PetLicenses
----------------------------------------
Shows how to use C1DataEngine in a console app to import and analyze JSON data.

This sample uses the .NET Standard version of C1DataEngine to analyze a dataset
of pet licenses for the city of Seattle. Specifically, it answers the following
questions:

1. How many licenses were issued, by species?
2. How many licenses were issued, by calendar year?
3. What were the ten most popular dog names?
4. Excluding dogs and cats, which names were registered?
5. How many licenses were issued in King County, by city?

The last question demonstrates the use of a join query to combine two distinct
data sources: the main license table and a secondary table that maps zip codes
to county and city names. See Program.cs for implementation details.

The first time the app is run, the JSON data is extracted from a zip file. See
the file Model.cs for the mappings between JSON property attributes and .NET
property names used in C1DataEngine queries. This file also implements a
JsonConverter that sanitizes the zip codes in the licenses data source (by
converting 9-digit codes to 5-digit codes and treating any code that has fewer
than 5 characters as a null value).

To run the project, open the .csproj file in Visual Studio Code and press F5.
Alternatively, you can open a command prompt, make the project directory
current, then type "dotnet run" to restore NuGet packages and build/run the
executable.
