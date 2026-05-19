using lab2_11.Data;
using lab2_11.Models;
using lab2_11.Models.DB;
using Microsoft.AspNetCore.Mvc;

namespace lab2_11.Controllers
{
    public class BooksController : Controller
    {
        private readonly Lab2Context _context;

        public BooksController(Lab2Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title) ||
                string.IsNullOrWhiteSpace(book.Author) ||
                string.IsNullOrWhiteSpace(book.Publisher) ||
                book.Pages <= 0)
            {
                ViewBag.Error = "Заполните все поля корректно!";
                return View(book);
            }

            _context.Books.Add(book);
            _context.SaveChanges();

            ViewBag.Success = "Книга добавлена!";
            return View();
        }

        [HttpGet]
        public IActionResult Search(string? author)
        {
            if (author == null)
            {
                return View();
            }

            if (string.IsNullOrWhiteSpace(author))
            {
                var authors = _context.Books
                    .Select(b => b.Author)
                    .Distinct()
                    .ToList();

                return View("Authors", authors);
            }

            var books = _context.Books
                .Where(b => b.Author == author)
                .ToList();

            return View("BooksByAuthor", books);
        }
    }
}