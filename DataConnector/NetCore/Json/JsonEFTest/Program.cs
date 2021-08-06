using System;
using System.Linq;
using JsonEFTest.GeneratedCode.Document;
using JsonEFTest.GeneratedCode.Flatten;
using JsonEFTest.GeneratedCode.Relation;

namespace JsonEFTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            SelectDocument();
            SelectFlatten();
            SelectRelation();
            Console.Read();
        }

        static void SelectDocument()
        {
            Console.WriteLine("Query all Books...");
            using (var db = new DocumentContext())
            {
                var histories = from p in db.Books select p;

                foreach (var h in histories)
                {
                    Console.WriteLine($"{h.AuthorFirstName} - {h.AuthorLastName} - {h.Isbn} - {h.Price} - {h.Title} - {h.Readers} -");
                }
            }
        }

        static void SelectFlatten()
        {
            Console.WriteLine("Query all Books...");
            using (var db = new FlattenedContext())
            {
                var people = from p in db.Books select p;

                foreach (var p in people)
                {
                    Console.WriteLine($"{p.AuthorFirstName} - {p.AuthorLastName} - {p.Isbn} - {p.Price} - {p.Name} -");
                }
            }
        }

        static void SelectRelation()
        {
            Console.WriteLine("Query all Vehicles...");
            using (var db = new RelationalContext())
            {
                var verhicles = from p in db.Readers select p;

                foreach (var v in verhicles)
                {
                    Console.WriteLine($"{v.Id} - {v.BooksId} - {v.Name} - {v.Age} -");
                }
            }
        }

        static void Insert()
        {
            Console.WriteLine("\nQuery Insert...");
        }

        static void Update()
        {
            Console.WriteLine("\nQuery Update...");
        }

        static void Delete()
        {
            Console.WriteLine("\nQuery Delete...");
        }
    }
}
