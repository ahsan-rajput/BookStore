using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        // Static lists keep the in-memory data available between requests.
        private static List<Author> Authors = new List<Author>
        {
            new Author { Id = 1, Name = "J.K. Rowling", Country = "United Kingdom" },
            new Author { Id = 2, Name = "Paulo Coelho", Country = "Brazil" }
        };

        private static List<Book> Books = new List<Book>
        {
            new Book { Id = 1, Title = "Harry Potter", Price = 2500, AuthorId = 1 },
            new Book { Id = 2, Title = "The Alchemist", Price = 1800, AuthorId = 2 },
            new Book { Id = 3, Title = "Fantastic Beasts", Price = 2200, AuthorId = 1 }
        };

        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                Authors = Authors,
                Books = Books.Select(book => new BookCardViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Price = book.Price,
                    AuthorName = Authors.FirstOrDefault(a => a.Id == book.AuthorId)?.Name
                                 ?? "Unknown Author"
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddBook(string title, decimal price, int authorId)
        {
            if (string.IsNullOrWhiteSpace(title) ||
                price < 0 ||
                !Authors.Any(a => a.Id == authorId))
            {
                return RedirectToAction("Index");
            }

            int newId = Books.Count == 0 ? 1 : Books.Max(b => b.Id) + 1;

            Books.Add(new Book
            {
                Id = newId,
                Title = title,
                Price = price,
                AuthorId = authorId
            });

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddAuthor(string name, string country)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(country))
            {
                return RedirectToAction("Index");
            }

            int newId = Authors.Count == 0 ? 1 : Authors.Max(a => a.Id) + 1;

            Authors.Add(new Author
            {
                Id = newId,
                Name = name,
                Country = country
            });

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveBook(int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);

            if (book != null)
            {
                Books.Remove(book);
            }

            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
