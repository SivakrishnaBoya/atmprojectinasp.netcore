using ATMPROCESS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.NetworkInformation;

namespace ATMPROCESS.Controllers
{
    public class ATM : Controller
    {
        Register reg = new Register();
        private readonly RegisterDBContext _context;
        public ATM( RegisterDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Register register)
        {
            
            reg.AccountNumber= register.AccountNumber;
            reg.Pin = new Random().Next(1000, 9999);
            reg.Name = register.Name;
            reg.PhoneNumber = register.PhoneNumber;
            reg.InitialAmount = 2000;
            _context.Add(reg);
            _context.SaveChanges();
            ViewBag.Acc_No = reg.AccountNumber;
            ViewBag.PinNo = reg.Pin;
            return View();
        }
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LogIn(Register register)
        {

            var account = _context.UserRegister.FirstOrDefault(a => a.AccountNumber == register.AccountNumber && a.Pin ==register.Pin);
           if(account !=null)
            {
                HttpContext.Session.SetInt32("AccountNumber", register.AccountNumber);
                return View("MainMenu");
            }
            else
            {
                ViewBag.Result = "LogIn Failure Due To Invalid Creadintails";
            }
            
            return View();
        }
      
        public IActionResult MainMenu()
        {
           var accountnumber = HttpContext.Session.GetInt32("AccountNumber");
            if (string.IsNullOrEmpty("accountnumber"))
            {
                ViewBag.AccountN = accountnumber;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Deposit()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Deposit(int Amt)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var accountnumber = HttpContext.Session.GetInt32("AccountNumber");
                ViewBag.AccNo = accountnumber;
                var account = _context.UserRegister.FirstOrDefault(a => a.AccountNumber == accountnumber);
                if (account != null)
                {
                    account.InitialAmount = account.InitialAmount + Amt;
                    ViewBag.Amount = Amt;
                    _context.SaveChanges();
                    transaction.Commit();
                }
            }
            catch ( Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500,$"error occured:{ex.Message}");
            }
            return View();
        }
        public IActionResult Withdraw()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Withdraw(int Amt)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var accountnumber = HttpContext.Session.GetInt32("AccountNumber");
                ViewBag.AccNo = accountnumber;
                var account = _context.UserRegister.FirstOrDefault(accnumber => accnumber.AccountNumber == accountnumber);
                if (account != null)
                {
                    if (account.InitialAmount > Amt)
                    {
                        account.InitialAmount = account.InitialAmount - Amt;
                        ViewBag.Amount = Amt;
                       
                        _context.SaveChanges();
                        transaction.Commit();
                        ViewBag.Result = "Withdraw Done";
                    }
                    else
                    {
                        ViewBag.Result = "Insufficient funds";
                    }
                }
            }catch(Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, $" error occured :{ex.Message}");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Changepassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Changepassword(int oldpin ,int newpin)
        {
            var accountnumber = HttpContext.Session.GetInt32("AccountNumber");
            ViewBag.AccNo = accountnumber;
            var account = _context.UserRegister.FirstOrDefault(a => a.AccountNumber == accountnumber);

            if (account.Pin==oldpin && oldpin!=newpin)
            {
                account.Pin = newpin;
                _context.SaveChanges();
            }
                return View();
        }
        [HttpGet]
      public IActionResult CheckBalance()
        {
            var accountnumber = HttpContext.Session.GetInt32("AccountNumber");
            ViewBag.AccNo = accountnumber;
            var account = _context.UserRegister.FirstOrDefault(a => a.AccountNumber == accountnumber);

            if (account !=null)
            {
                ViewBag.amt = account.InitialAmount;
               
            }
            return View();
        }
        [HttpGet]
        public IActionResult MiniStatement(MiniStatement min)
        {
            var accountnumber = HttpContext.Session.GetInt32("AccountNumber");
            

            ViewBag.Result= _context.miniStatements.OrderByDescending(e => e.MinistatementId).ToList();

            return View();
        }
        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return View("LogIn");
        }
    }

}
