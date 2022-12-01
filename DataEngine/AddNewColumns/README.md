## AddNewColumns
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-Service-Components-Samples/tree/master/DataEngine/AddNewColumns)
____
#### Demonstrates how to add new columns to an existing C1DataEngine base table.
____
The first time the app is run, it imports the Invoices table from an OData source.
When the app is run a second time, it creates a calculated column (LargeOrder) and
adds it to the previously imported base table. You can verify the existence of the
new column by examining the contents of the workspace/Invoices folder relative to
the project root folder. You should see a file named LargeOrder.data.

If the app is run a third time, it throws an exception because the LargeOrder
column already exists.
