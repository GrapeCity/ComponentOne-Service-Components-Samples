using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Collections.Generic;
using C1.AdoNet.Json;

namespace JsonTest
{
    class Program
    {
        static string documentConnectionString = $"Data Model=Document;Uri='json_bookstore.json';Json Path='$.bookstore.books'";
        static string flattenedConnectionString = $"Data Model=FlattenedDocuments;Uri='json_bookstore.json';Json Path='$.bookstore.books;$.bookstore.books.readers'";
        static string relationConnectionString = $"Data Model=Relational;Uri='json_bookstore.json';Json Path='$.bookstore.books;$.bookstore.books.readers'";

        //Odata server protected by oauth 2.0 authentication
        const string TokenEndpoint = @"http://10.41.0.131/Oauth2Server/connect/token";
        const string URI = @"http://10.41.0.131/ODataServerOAuth2/OData/Books";
        const string ClientId = @"carbon";
        const string ClientSecret = @"21B5F798-BE55-42BC-8AA8-0025B903DC3B";
        const string Scope = @"api1";
        const string Username = @"alice";
        const string Password = @"secret";

        static string connectionStringForOAuth = @$"Data Model = Document; Json Path='$.value';OAuth Token Endpoint={TokenEndpoint}; Uri={URI}; OAuth Client Id={ClientId}; OAuth Client Secret={ClientSecret}; OAuth Scope={Scope}; Username={Username}; Password={Password};";

        static void Main(string[] args)
        {
            //SelectDocumentWithDataAdapter();
            //Console.WriteLine("==================================================");
            //SelectDocumentWithOAuth();
            //Console.WriteLine("==================================================");
            //SelectDocument();
            //Console.WriteLine("==================================================");
            //SelectFlattenedDocuments();
            //Console.WriteLine("==================================================");
            //SelectRelational();

            Insert();
            Console.WriteLine("==================================================");
            Update();
            Console.WriteLine("==================================================");
            Delete();

            Console.Read();
        }

        static void SelectDocument()
        {
            Console.WriteLine("Query all Books on Document mode...");
            using(var con = new C1JsonConnection(documentConnectionString))
            {
                con.Open();

                var table = con.GetSchema("columns", new string[] { "books" });
                ShowDataTable(table);

                var cmd = con.CreateCommand();
                cmd.CommandText = "Select * From books";
                var reader = cmd.ExecuteReader();

                PrintStringContentFromReader(reader);
            }
        }

        static void SelectFlattenedDocuments()
        {
            Console.WriteLine("Query all Books on FlattenedDocument mode...");
            using (var con = new C1JsonConnection(flattenedConnectionString))
            {
                con.Open();

                var table = con.GetSchema("columns", new string[] { "books" });
                ShowDataTable(table);

                var cmd = con.CreateCommand();
                cmd.CommandText = "Select * From books";
                var reader = cmd.ExecuteReader();
                
                PrintStringContentFromReader(reader);
            }
        }

        static void SelectRelational()
        {
            Console.WriteLine("Query all Books on Relational mode...");
            using (var con = new C1JsonConnection(relationConnectionString))
            {
                con.Open();

                var table = con.GetSchema("columns", new string[] { "books" });
                ShowDataTable(table);

                var cmd = con.CreateCommand();
                cmd.CommandText = "Select * From books";
                var reader = cmd.ExecuteReader();
                PrintStringContentFromReader(reader);
            }
        }

        static void SelectDocumentWithOAuth()
        {
            Console.WriteLine("Query all Books via OAuth...");
            using (var con = new C1JsonConnection(connectionStringForOAuth))
            {
                con.Open();

                var table = con.GetSchema("columns", new string[] { "value" });
                ShowDataTable(table);

                var cmd = con.CreateCommand();
                cmd.CommandText = "Select * From value";
                var reader = cmd.ExecuteReader();
                PrintStringContentFromReader(reader);
            }
        }

        static void SelectDocumentWithDataAdapter()
        {
            Console.WriteLine("Query all Books with DataAdapter...");
            using (var con = new C1JsonConnection(documentConnectionString))
            {
                con.Open();

                using (var adapter = new C1JsonDataAdapter(con, "Select * From books"))
                {
                    var bookTbl = new DataTable();
                    adapter.Fill(bookTbl);

                    ShowDataTable(bookTbl, 20);
                }
            }
        }

        static void ShowDataTable1(DataTable table, int length = 25)
        {
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn col in table.Columns)
                {
                    string headerCol = string.Format("{0,-" + length + "}", col.ColumnName);
                    string content = string.Empty;
                    if (col.DataType.Equals(typeof(DateTime)))
                        content = string.Format("{0,-" + length + ":d}", row[col]);
                    else if (col.DataType.Equals(typeof(decimal)))
                        content = string.Format("{0,-" + length + ":C}", row[col]);
                    else
                        content = string.Format("{0,-" + length + "}", row[col]);
                    Console.WriteLine(headerCol + content);
                }
                Console.WriteLine("=============================================");
            }
        }

        static void ShowDataTable(DataTable table, int length = 25)
        {
            foreach (DataColumn col in table.Columns)
            {
                Console.Write("{0,-" + length + "}", col.ColumnName);
            }
            Console.WriteLine();

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn col in table.Columns)
                {
                    if (col.DataType.Equals(typeof(DateTime)))
                        Console.Write("{0,-" + length + ":d}", row[col]);
                    else if (col.DataType.Equals(typeof(decimal)))
                        Console.Write("{0,-" + length + ":C}", row[col]);
                    else
                        Console.Write("{0,-" + length + "}", row[col]);
                }
                Console.WriteLine();
            }
        }

        static void Insert()
        {
            string connectionString = string.Format(@"Data Model={0};Uri='{1}';Json Path='{2}';Use Pool=true; username='admin'; password='123456'", "Relational", "http://45.125.239.138:8088/api/Album", "$.Album");
            
            using (var con = new C1JsonConnection(connectionString))
            {
                con.Open();

                var sqlInsert = "Insert Into Album([AlbumId], [Title], [ArtistId]) values (9999667,'test', 1)";
                var cmdInsert = con.CreateCommand();
                cmdInsert.CommandText = sqlInsert;
                var result1 = cmdInsert.ExecuteNonQuery();

                var sqlSelect = "Select [AlbumId], [Title] from Album where [AlbumId] = 9999667";
                var cmdSelect = con.CreateCommand();
                cmdSelect.CommandText = sqlSelect;
                var reader = cmdSelect.ExecuteReader();
                reader.Read();
                Console.WriteLine("Title: " + reader["Title"].ToString());
            }

            string connectionString2 = string.Format(@"Data Model={0};Uri='{1}';Json Path='{2}';Use Pool=true; username='admin'; password='123456'", "Relational", "http://45.125.239.138:8088/api/Customer", "$.Invoice");
            
            using (var con = new C1JsonConnection(connectionString2))
            {
                con.Open();

                var sqlInsert = "Insert Into Customer([CustomerId], [FirstName], [LastName], [Company], [Address], [City], [State], [Country], [PostalCode], [Phone], [Fax], [Email], [SupportRepId]) " +
                    "values (118,'test','test','test','test','test','test','test','test','test','test','test', 1)";
                var cmdInsert = con.CreateCommand();
                cmdInsert.CommandText = sqlInsert;
                var result1 = cmdInsert.ExecuteNonQuery();

                var sqlSelect = "Select [CustomerId], [FirstName], [LastName], [Company], [Address], [City], [State], [Country], [PostalCode], [Phone], [Fax], [Email], [SupportRepId] from Customer where [CustomerId] = 118";
                var cmdSelect = con.CreateCommand();
                cmdSelect.CommandText = sqlSelect;
                var reader = cmdSelect.ExecuteReader();

                reader.Read();
                Console.WriteLine("FirstName: " + reader["FirstName"].ToString());
                Console.WriteLine("LastName: " + reader["LastName"].ToString());
                Console.WriteLine("Company: " + reader["Company"].ToString());
                Console.WriteLine("Address: " + reader["Address"].ToString());
            }
        }

        static void Update()
        {
            string connectionString = string.Format(@"Data Model={0};Uri='{1}';Json Path='{2}';Use Pool=true; username='admin'; password='123456'", "Relational", "http://45.125.239.138:8088/api/Album", "$.Album");

            using (var con = new C1JsonConnection(connectionString))
            {
                con.Open();

                var sqlUpdate = "Update Album set [Title] = 'abcde' where [AlbumId] = 9999667";
                var cmdUpdate = con.CreateCommand();
                cmdUpdate.CommandText = sqlUpdate;
                var result2 = cmdUpdate.ExecuteNonQuery();

                var sqlSelect = "Select [AlbumId], [Title] from Album where [AlbumId] = 9999667";
                var cmdSelect = con.CreateCommand();
                cmdSelect.CommandText = sqlSelect;
                var reader = cmdSelect.ExecuteReader();
                reader.Read();
                Console.WriteLine("Title: " + reader["Title"].ToString());
            }

            string connectionString2 = string.Format(@"Data Model={0};Uri='{1}';Json Path='{2}';Use Pool=true; username='admin'; password='123456'", "Relational", "http://45.125.239.138:8088/api/Customer", "$.Invoice");

            using (var con = new C1JsonConnection(connectionString2))
            {
                con.Open();

                var sqlUpdate = "Update Customer set [FirstName]  = 'abcde', [LastName] = 'abcde', [Company] = 'abcde', [Address] = 'abcde' where [CustomerId] = 118";
                var cmdUpdate = con.CreateCommand();
                cmdUpdate.CommandText = sqlUpdate;
                var result2 = cmdUpdate.ExecuteNonQuery();

                var sqlSelect = "Select [CustomerId], [FirstName], [LastName], [Company], [Address], [City], [State], [Country], [PostalCode], [Phone], [Fax], [Email], [SupportRepId] from Customer where [CustomerId] = 118";
                var cmdSelect = con.CreateCommand();
                cmdSelect.CommandText = sqlSelect;
                var reader = cmdSelect.ExecuteReader();

                reader.Read();
                Console.WriteLine("FirstName: " + reader["FirstName"].ToString());
                Console.WriteLine("LastName: " + reader["LastName"].ToString());
                Console.WriteLine("Company: " + reader["Company"].ToString());
                Console.WriteLine("Address: " + reader["Address"].ToString());
            }
        }

        static void Delete()
        {
            string connectionString = string.Format(@"Data Model={0};Uri='{1}';Json Path='{2}';Use Pool=true; username='admin'; password='123456'", "Relational", "http://45.125.239.138:8088/api/Album", "$.Invoice");

            using (var con = new C1JsonConnection(connectionString))
            {
                con.Open();

                var sqlDelete = "Delete from Album where [AlbumId] = 9999667";
                var cmdDelete = con.CreateCommand();
                cmdDelete.CommandText = sqlDelete;
                var result3 = cmdDelete.ExecuteNonQuery();
            }

            string connectionString2 = string.Format(@"Data Model={0};Uri='{1}';Json Path='{2}';Use Pool=true; username='admin'; password='123456'", "Relational", "http://45.125.239.138:8088/api/Customer", "$.Invoice");

            using (var con = new C1JsonConnection(connectionString2))
            {
                con.Open();

                var sqlDelete = "Delete from Customer where [CustomerId] = 118";
                var cmdDelete = con.CreateCommand();
                cmdDelete.CommandText = sqlDelete;
                cmdDelete.ExecuteNonQuery();
            }
        }

        static void PrintStringContentFromReader(DbDataReader reader)
        {
        NextResult:
            var cols = new List<string>();
            var fieldCount = reader.FieldCount;
            for (var i = 0; i < fieldCount; i++)
            {
                cols.Add(reader.GetName(i));
            }
            Console.WriteLine(string.Join(",", cols));

            while (reader.Read())
            {
                var values = new List<string>();
                for (var i = 0; i < fieldCount; i++)
                {
                    var objValue = reader.GetValue(i);
                    string strValue;
                    if (objValue == null || DBNull.Value == objValue)
                    {
                        strValue = "";
                    }
                    else
                    {
                        var dataType = reader.GetFieldType(i);
                        if (dataType == typeof(DateTime))
                        {
                            strValue = ((DateTime)objValue).ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            strValue = Convert.ToString(Convert.ChangeType(objValue, dataType, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                        }
                    }

                    values.Add(strValue);
                }
                Console.WriteLine(string.Join(",", values));
            }

            if (reader.NextResult())
                goto NextResult;
        }
    }
}
