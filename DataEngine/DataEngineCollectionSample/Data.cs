﻿using C1.DataCollection;
using C1.DataEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataEngineCollectionSample
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Employee
    {
        [Browsable(false)]
        public int Id { get; set; }
        [DisplayName("Post")]
        public int PostId { get; set; }
        public string FirstName { get; set; }
        [DisplayName("Last name")]
        public string LastName { get; set; }
        [DisplayName("Employment date")]
        public DateTime EmploymentDate { get; set; }
        [DisplayName("Country")]
        [Range(0, 8)]
        public int CountryId { get; set; }

        public static readonly string[] Names = new string[] { "John", "Alex", "Alfred", "Elena", "Eric", "Sara", "Mila", "Gloria", "Mary", "Natalie", "Ivan", "Stan", "Don", "Joseph",
            "Jack", "Olivia", "Caleb", "Dylan", "Samantha", "Tyler", "Victor", "James", "Jason", "Peter","Tony"};

        public static readonly string[] LastNames = new string[] { "Doe", "Red", "Bon", "White", "Green", "Blue", "Vong", "Li", "Black", "Sue", "Ming", "Romanov", "Heck", "Milman",
            "Anderson", "Gun", "Helt", "Iron", "Gold", "Silver", "Steel", "Smith", "Bond", "Bourne", "McClane", "Parker", "Stark" };
    }

    public class DataService
    {
        private static readonly Random _rnd = new Random();

        public const string PostTableName = "post";
        public const string EmployeeTableName = "employee";
        public const string CountryTableName = "country";

        public static string PostTablePath => $"{PostTableName}.json";
        public static string EmployeeTablePath => $"{EmployeeTableName}.json";
        public static string CountryTablePath => $"{CountryTableName}.json";

        public static bool TablesExist(Workspace workspace)
        {
            return workspace.TableExists(PostTableName) && workspace.TableExists(EmployeeTableName) && workspace.TableExists(CountryTableName);
        }

        public static async Task GenerateData(Workspace workspace)
        {
            await SerializeData(PostTablePath, Posts);
            await SerializeData(CountryTablePath, Countries);
            await SerializeData(EmployeeTablePath, GenerateEmployee());

            // data engine
            workspace.Clear(ClearFileType.All);
            var postList = await DeserializeData<Post>(PostTablePath);
            await Task.Run(() => LoadTable(PostTableName, postList, workspace));
            var employeeList = await DeserializeData<Employee>(EmployeeTablePath);
            await Task.Run(() => LoadTable(EmployeeTableName, employeeList, workspace));
            var countryList = await DeserializeData<Country>(CountryTablePath);
            await Task.Run(() => LoadTable(CountryTableName, countryList, workspace));
        }

        public static async Task<IDataCollection<object>> LoadDataCollection(Workspace workspace)
        {
            dynamic employee = workspace.table(EmployeeTableName);
            dynamic post = workspace.table(PostTableName);
            dynamic country = workspace.table(CountryTableName);
            dynamic join = workspace.join(employee, new
            {
                postTitle = post.Title.As("PostTitle") | employee.PostId == post.Id,
                countryName = country.Name.As("CountryName") | employee.CountryId == country.Id,
            });

            return new C1DataEngineCollection(workspace, join);
        }

        private static async Task<IEnumerable<T>> DeserializeData<T>(string fileName)
        {
            using (FileStream stream = File.Open(fileName, FileMode.Open))
            {
                return await JsonSerializer.DeserializeAsync<IEnumerable<T>>(stream);
            }
        }

        private static async Task SerializeData<T>(string fileName, IEnumerable<T> enumerable)
        {
            using (FileStream stream = File.Create(fileName))
            {
                await JsonSerializer.SerializeAsync(stream, enumerable);
                stream.Flush();
            }
        }

        private static void LoadTable<T>(string tableName, IEnumerable<T> list, Workspace workspace)
        {
            var connector = new ObjectConnector<T>(workspace, list);
            connector.GetData(tableName);
            workspace.Save();
        }

        private static IEnumerable<Employee> GenerateEmployee()
        {
            var today = DateTime.Today;
            return Enumerable.Range(0, 1000000).Select(i =>
                 new Employee()
                 {
                     FirstName = Employee.Names[_rnd.Next(0, Employee.Names.Length)],
                     LastName = Employee.LastNames[_rnd.Next(0, Employee.LastNames.Length)],
                     PostId = _rnd.Next(0, Posts.Max(x => x.Id) + 1),
                     Id = i,
                     EmploymentDate = today.AddYears(-_rnd.Next(2, 25)),
                     CountryId = _rnd.Next(0, 9)
                 });
        }

        private static Post[] Posts { get; }
            = new Post[]
            {
                new Post() { Id = 0, Title = "Engineer" },
                new Post() { Id = 1, Title = "Manager" },
                new Post() { Id = 2, Title = "Manager's assistant" },
                new Post() { Id = 3, Title = "Security" },
                new Post() { Id = 4, Title = "Secretary" },
                new Post() { Id = 5, Title = "Accountant" },
                new Post() { Id = 6, Title = "HR" },
                new Post() { Id = 7, Title = "Courier" },
                new Post() { Id = 8, Title = "Cleaner" },
            };

        private static Country[] Countries { get; }
            = new Country[]
            {
                new Country() { Id = 0, Name = "Canada" },
                new Country() { Id = 1, Name = "China" },
                new Country() { Id = 2, Name = "Germany" },
                new Country() { Id = 3, Name = "Ireland" },
                new Country() { Id = 4, Name = "Japan" },
                new Country() { Id = 5, Name = "New Zealand" },
                new Country() { Id = 6, Name = "Poland" },
                new Country() { Id = 7, Name = "United Kingdom" },
                new Country() { Id = 8, Name = "United States" },
            };
    }
}
