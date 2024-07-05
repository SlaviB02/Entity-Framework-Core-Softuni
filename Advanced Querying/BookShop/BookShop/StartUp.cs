namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
            Console.WriteLine(RemoveBooks(db));
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb=new StringBuilder();
            //0-Minor
            //1-Teen
            //2-Adult
           command=command.ToLower();
            AgeRestriction ag=new AgeRestriction();
            if(command=="minor")
            {
                ag = AgeRestriction.Minor;
            }
            if(command=="teen")
            {
                ag = AgeRestriction.Teen;
            }
            if(command=="adult")
            {
                ag=AgeRestriction.Adult;
            }

            var books = context.Books
                .Where(b => b.AgeRestriction == ag)
                .Select(e => e.Title)
                .OrderBy(e => e)
                .ToList();
            foreach (var book in books)
            {
                sb.AppendLine(book);
            }
         

            return sb.ToString().TrimEnd();
        }
        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(e=>e.Copies<5000 && e.EditionType==EditionType.Gold)
                .Select(e => new
                {
                    e.Title,
                    e.BookId
                })
                .OrderBy(e=>e.BookId)
                .ToList();

            foreach(var book in books)
            {
                sb.AppendLine(book.Title);
            }


            return sb.ToString().TrimEnd(); 
        }
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books= context.Books
                .Where(e=>e.Price>40)
                .Select(e => new
                {
                    e.Title,
                    e.Price
                })
                .OrderByDescending(e=>e.Price)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb=new StringBuilder();

            var books=context.Books
                .Where(b=>b.ReleaseDate.Value.Year!=year)
                .Select(e => new
                {
                    e.Title,
                    e.BookId
                })
                .OrderBy (e=>e.BookId)
                .ToList();

            foreach(var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            List<string>categories=input
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            StringBuilder sb=new StringBuilder();

            var books=context.BooksCategories
                .Where(c=>categories.Contains(c.Category.Name.ToLower()))
                .Select(b=>b.Book.Title)
                .OrderBy(b=>b)
                .ToList();
               
                
            foreach (var book in books)
            {
                sb.AppendLine(book);
            }
            return sb.ToString().TrimEnd(); 
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            var dateparts=date.Split("-").ToArray();

            int day = int.Parse(dateparts[0]);
            int month=int.Parse(dateparts[1]);
            int year=int.Parse(dateparts[2]);

            var requiredDate=new DateTime(year, month, day);



            var books=context.Books
                .Where(b=>b.ReleaseDate<requiredDate)
                .Select(e => new
                {
                    e.ReleaseDate,
                    e.Title,
                    e.EditionType,
                    e.Price
                })
                .OrderByDescending(e=>e.ReleaseDate)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var authors=context.Authors
                .Where(a=>a.FirstName.EndsWith(input))
                .Select(e=>new
                {
                    e.FirstName,
                    e.LastName,
                })
                .OrderBy(e=>e.FirstName)
                .ThenBy(e=>e.LastName)
                .ToList();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var books=context.Books
                .Where(b=>b.Title.ToLower().Contains(input.ToLower()))
                .Select(e=>e.Title)
                .OrderBy(e=>e)
                .ToList();

            foreach(var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var books= context.Books
                .Where(b=>b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(e => new
                {
                    AuthorName=e.Author.FirstName+" "+e.Author.LastName,
                    e.Title,
                    e.BookId
                })
                .OrderBy(e=>e.BookId)
                .ToList();

            foreach(var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorName})");
            }

            return sb.ToString().TrimEnd();
        }
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
         
            var books = context.Books
                .Where(b=>b.Title.Length>lengthCheck)
                .Select(e=>e.BookId)
                .ToList();

            return books.Count;
        }
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb=new StringBuilder();

            var bookCopiesAuthors= context.Authors
                
                .Select(e => new
                {
                   FullName=e.FirstName+" "+e.LastName,
                   TotalCopies=e.Books.Sum(e=>e.Copies)
                })
                .OrderByDescending(e=>e.TotalCopies)
                .ToList();

             foreach(var bc in bookCopiesAuthors)
            {
                sb.AppendLine($"{bc.FullName} - {bc.TotalCopies}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var totalProfit=context.Categories
                .Select(e => new
                {
                   CategoryName=e.Name,
                    TotalProfits=e.CategoryBooks.Sum(cb=>cb.Book.Price*cb.Book.Copies)
                })
                .OrderByDescending(e=>e.TotalProfits)
                .ThenBy(e=>e.CategoryName)
                .ToList();

            foreach(var tp in totalProfit)
            {
                sb.AppendLine($"{tp.CategoryName} ${tp.TotalProfits:F2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categories = context.Categories
                .Select(e => new
                {
                    CategoryName = e.Name,
                    books = e.CategoryBooks
                    .Select(cb => cb.Book)
                    .OrderByDescending(e => e.ReleaseDate)
                    .Select(b => new
                    {
                        b.Title,
                        b.ReleaseDate
                    })
                    .Take(3)
                    .ToList()
                })
                .OrderBy(c => c.CategoryName)
                .ToList();
               
            foreach(var c in categories)
            {
                sb.AppendLine($"--{c.CategoryName}");
                foreach(var book in c.books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }
               

            return sb.ToString().TrimEnd(); 
        }
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();
            foreach (var book in books)
            {
                book.Price += 5;
            }
            context.SaveChanges();
        }
        public static int RemoveBooks(BookShopContext context)
        {
            int count = 0;
            
            var booksToRemove=context.Books
                .Where(b=>b.Copies<4200)
                .ToList();

            count=booksToRemove.Count;

            foreach(var book in booksToRemove)
            {
                context.Books.Remove(book);
            }

            context.SaveChanges();

            return count;
        }
    }
}


