using System;
using System.Linq;
using JsonEFTest.GeneratedCode.Document;
using JsonEFTest.GeneratedCode.Flatten;
using JsonEFTest.GeneratedCode.Http;
using JsonEFTest.GeneratedCode.Relation;
using Books = JsonEFTest.GeneratedCode.Relation.Books;

namespace JsonEFTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            //SelectDocument();
            //SelectFlatten();
            //SelectRelation();
            //SelectHttp();
            //CUDJsonHttp();
            CUDJsonFile();
            Console.Read();
        }

        static void SelectHttp()
        {
            Console.WriteLine("Query all Album...");
            using (var db = new MainContext())
            {
                var albums = from p in db.Album select p;

                foreach (var h in albums)
                {
                    Console.WriteLine($"{h.AlbumId} - {h.Title} - {h.ArtistId}");
                }
            }
            Console.ReadLine();
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

        static void CUDJsonHttp()
        {
            Console.WriteLine("\nCUD Http...");
            using (var context = new MainContext())
            {
                var album = new Album();

                album.AlbumId = "1099";
                album.Title = "Test Insert EFCore";
                album.ArtistId = "1";
                context.Album.Add(album);

                int result = context.SaveChanges();
                Console.WriteLine("Number of row inserted: " + result);
            }

            using (var context = new MainContext())
            {
                var album = context.Album.Where(x => x.Title.Equals("Test Insert EFCore")).FirstOrDefault();
                if (album != null)
                {
                    album.Title = "Test Update EFCore";
                    int result = context.SaveChanges();
                    Console.WriteLine("Number of row updated: " + result);
                }
            }

            using (var context = new MainContext())
            {
                context.Album.Remove(context.Album.Where(x => x.Title.Equals("Test Update EFCore")).FirstOrDefault());
                var result = context.SaveChanges();
                Console.WriteLine("Number of row deleted: " + result);
            }
        }

        static void CUDJsonFile()
        {
            Console.WriteLine("\nCUD Json file...");
            using (var context = new RelationalContext())
            {
                var book = new Books();

                book.Id = "1";
                book.Title = "Test Insert EFCore";
                book.Price = 400;
                book.Isbn = "1";
                book.Publicationdate = new DateTime(2021, 10, 15);
                context.Books.Add(book);

                int result = context.SaveChanges();
                Console.WriteLine("Number of row inserted: " + result);
            }

            using (var context = new RelationalContext())
            {
                var book = context.Books.Where(x => x.Title.Equals("Test Insert EFCore")).FirstOrDefault();
                if (book != null)
                {
                    book.Title = "Test Update EFCore";
                    int result = context.SaveChanges();
                    Console.WriteLine("Number of row updated: " + result);
                }
            }

            using (var context = new RelationalContext())
            {
                context.Books.Remove(context.Books.Where(x => x.Title.Equals("Test Update EFCore")).FirstOrDefault());
                var result = context.SaveChanges();
                Console.WriteLine("Number of row deleted: " + result);
            }
        }
    }
}
