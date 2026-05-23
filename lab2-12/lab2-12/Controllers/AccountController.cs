using Microsoft.AspNetCore.Mvc;
using lab2_12.Data;
using lab2_12.Models;
using lab2_12.Models.ViewModels;

namespace lab2_12.Controllers
{
    public class AccountController : Controller
    {
        private readonly LabContext _context;

        private const string AdminPass = "admin123";

        public AccountController(LabContext context)
        {
            _context = context;
        }

        // LOGIN
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var user = _context.Users.FirstOrDefault(x =>
                x.Login == model.Login && x.Password == model.Password);

            if (user == null)
                return View(model);

            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToAction("Index");
        }

        // REGISTER
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
                return View(model);

            _context.Users.Add(new User
            {
                Login = model.Login,
                Password = model.Password,
                Balance = 0
            });

            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // INDEX
        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("UserId");

            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

        // TRANSFER
        [HttpGet]
        public IActionResult Transfer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Transfer(TransferViewModel model)
        {
            Console.WriteLine($"TRANSFER: {model.ReceiverLogin} {model.Amount}");

            var senderId = HttpContext.Session.GetInt32("UserId");

            var sender = _context.Users.FirstOrDefault(x => x.Id == senderId);

            if (sender == null)
                return RedirectToAction("Login");

            if (model.Amount <= 0)
                return View(model);

            var receiver = _context.Users
                .FirstOrDefault(x => x.Login == model.ReceiverLogin);

            if (receiver == null)
            {
                Console.WriteLine("RECEIVER NOT FOUND");
                return View(model);
            }

            if (sender.Balance < model.Amount)
            {
                Console.WriteLine("NOT ENOUGH MONEY");
                return View(model);
            }

            sender.Balance -= model.Amount;
            receiver.Balance += model.Amount;

            _context.SaveChanges();

            Console.WriteLine("TRANSFER OK");

            return RedirectToAction("Index");
        }

        // PASSWORD
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            Console.WriteLine("HIT CHANGE PASSWORD");

            Console.WriteLine($"OLD: {model?.OldPassword}");
            Console.WriteLine($"NEW: {model?.NewPassword}");
            Console.WriteLine($"CONF: {model?.ConfirmPassword}");

            var id = HttpContext.Session.GetInt32("UserId");

            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return RedirectToAction("Login");

            if (user.Password != model.OldPassword)
            {
                Console.WriteLine("WRONG OLD PASSWORD");
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                Console.WriteLine("NOT MATCH");
                return View(model);
            }

            user.Password = model.NewPassword;

            _context.SaveChanges();

            Console.WriteLine("PASSWORD CHANGED");

            return RedirectToAction("Index");
        }

        // DELETE
        public IActionResult Delete()
        {
            var id = HttpContext.Session.GetInt32("UserId");

            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            HttpContext.Session.Clear();

            return RedirectToAction("Register");
        }

        // ADMIN
        public IActionResult AdminConfirm() => View();

        [HttpPost]
        public IActionResult AdminConfirm(string password)
        {
            if (password != AdminPass)
                return View();

            HttpContext.Session.SetString("admin", "ok");

            return RedirectToAction("Admin");
        }

        public IActionResult Admin()
        {
            if (HttpContext.Session.GetString("admin") != "ok")
                return RedirectToAction("AdminConfirm");

            return View(_context.Users.ToList());
        }

        [HttpPost]
        public IActionResult UpdateBalance(int id, decimal balance)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                user.Balance = balance;
                _context.SaveChanges();
            }

            return RedirectToAction("Admin");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}