using MarketplaceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketplaceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MarketplaceContext _db;
        public UsersController(MarketplaceContext db)
        {
            _db = db;
        }
        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody]User usm)
        {
            User us = new User
            {
                Name = usm.Name,
                Email = usm.Email,
                Phone = usm.Phone,
                Password = BCrypt.Net.BCrypt.HashPassword(usm.Password),
                Wallet = 0
            };
            // TempData["user_reg_id"] = db.User.Where(o => o.Name == usm.Name).Select(p => p.Id).SingleOrDefault();
            var EmailExist = _db.User.ToList().Any(u => u.Email == us.Email);
            var user_id = 0;
            if (EmailExist)
            {
                //throw error
                //ViewBag.EmailExistError = "You have already signed up";
                //go to error page
                //HttpContext.Session.SetInt32("Reg_Id", us.Id);
                return NotFound("This email already exist");
            }
            else
            {
                _db.Add(us);
                _db.SaveChanges();
                user_id = us.Id;
                Payment p = new Payment();
                p.User_Id = user_id;
                p.Money = 100000;
                p.Status = "Credit Card";
                _db.Add(p);
                _db.SaveChanges();
                HttpContext.Session.SetInt32("Reg_Id", us.Id);
                return Ok("User Successfully Registered");

            }


        }

        [HttpGet("Show Users")]
        public ActionResult ShowUsers()
        {
            var users = _db.User.ToList();
            return Ok(users);
        }


        [HttpPost]
        [Route("Login")]
        public ActionResult Login(User usm)
        {
            var exist = _db.User.ToList().Any(i => i.Email == usm.Email);
            HttpContext.Session.SetString("User_Email", usm.Email);
            if (exist)
            {
                var Data = _db.User.Where(f => f.Email == usm.Email).Select(s => new { s.Password, s.Id }).ToList();
                if (BCrypt.Net.BCrypt.Verify(usm.Password, Data[0].Password))
                {
                    User us = new User();
                    HttpContext.Session.SetInt32("Reg_Id", Data[0].Id);
                    //return Redirect("/Home/Index");
                    return Ok("User Logged in Successfully");

                }

            }
            return NotFound("User Not Found Please Try Again");
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.SetInt32("Reg_Id", 0);
            HttpContext.Session.SetString("User_Email", null);
            return Ok("Logged Out Successfully");
        }
        [HttpPost]
        [Route("Profile")]
        public IActionResult Profile(User usm)
        {
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            string User_Email = HttpContext.Session.GetString("User_Email");
            var User_id = _db.User.Where(p => p.Email == User_Email).Select(o => o.Id).FirstOrDefault();
            var User_status = _db.Product.Where(p => p.User_Id == Reg_Id).Select(o => o.Status).FirstOrDefault();
           // ViewData["User_status"] = User_status;
           // ViewBag.status = ViewData["User_status"];
            string Reg_Name = _db.User.Where(l => l.Id == Reg_Id).Select(i => i.Name).FirstOrDefault();
            string Reg_phone = _db.User.Where(l => l.Id == Reg_Id).Select(i => i.Phone).FirstOrDefault();
            string Reg_email = _db.User.Where(l => l.Id == Reg_Id).Select(i => i.Email).FirstOrDefault();
            double Reg_wallet = _db.User.Where(l => l.Id == Reg_Id).Select(i => i.Wallet).FirstOrDefault();
            //ViewData["Reg_wallet"] = Reg_wallet;
            //ViewBag.wallet = ViewData["Reg_wallet"];
            //ViewData["id_get"] = User_id;
            //ViewBag.Id = ViewData["id_get"];
            //ViewData["Reg_phone"] = Reg_phone;
            //ViewBag.phone = ViewData["Reg_phone"];
            //ViewData["Reg_email"] = Reg_email;
            //ViewBag.email = ViewData["Reg_email"];

            //ViewBag.Name = Reg_Name;
            List<Product> objList = new List<Product>();
            objList = (from s in _db.Product select s).Where(p => p.User_Id == Reg_Id).ToList();
           
            //ViewData["products"] = objList;
            //// ViewData["products"] = productList;
            //var otherItems = _db.Product.Where(a => a.SecondaryUser == Reg_Id).Select(a => a).ToList();
            //ViewData["otherStoresProducts"] = otherItems;
            return Ok("Profile User information");

        }


    }
}
