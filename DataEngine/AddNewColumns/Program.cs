var workspace = new Workspace();
workspace.Init("workspace");

const string connectionString = "URL=http://services.odata.org/v4/Northwind/Northwind.svc";
DbConnection connection = new C1ODataConnection(connectionString);
connection.Open();
var command = connection.CreateCommand();

if (!workspace.TableExists("Invoices"))
{
    command.CommandText = "select * from Invoices";
    DbConnector connector = new DbConnector(workspace, connection, command);
    connector.GetData("Invoices");
}
else
{
    command.CommandText = "select Quantity>=50 as LargeOrder from Invoices";
    DbConnector connector = new DbConnector(workspace, connection, command);
    connector.AddNewColumns("Invoices");
}

workspace.Save();
connection.Close();
