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
    public class PaymentsController : ControllerBase
    {
        private readonly MarketplaceContext _db;
        public PaymentsController(MarketplaceContext db)
        {
            _db = db;
        }


        [HttpPost("Deposit/{id}")]
        public IActionResult Deposit(int id, int money)
        {
            var user = _db.User.Find(id);
            user.Wallet += money;
            _db.Update(user);
            _db.SaveChanges();
            return Ok("User Wallet Cash Updated Successfully");
        }

        [HttpGet("User")]
        public IActionResult Payment()
        {
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            var products = (from p in _db.Transaction select p).Where(f => f.User_Id == Reg_Id).Select(h => h.Product_Id).ToList();

            double userMoney = _db.Payment.Where(v => v.Id == Reg_Id).Select(d => d.Money).FirstOrDefault();
            double totalMoney = 0;
            for (var i = 0; i < products.Count(); i++)
            {
                totalMoney += _db.Product.Where(v => v.Id == products[i]).Select(d => d.Price).FirstOrDefault();
            }

            return Ok(totalMoney);
        }

        [HttpPost("User")]
        public IActionResult Payment(PaymentVM pvm)
        {
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            var products = (from p in _db.Transaction select p).Where(f => f.User_Id == Reg_Id).Select(h => h.Product_Id).ToList();
            double userMoney = _db.Payment.Where(v => v.Id == Reg_Id).Select(d => d.Money).FirstOrDefault();
            double totalMoney = 0;
            for (var i = 0; i < products.Count(); i++)
            {
                totalMoney += _db.Product.Where(v => v.Id == products[i]).Select(d => d.Price).FirstOrDefault();
            }

            Payment pp;
            pp = _db.Payment.FirstOrDefault(s => s.User_Id == Reg_Id);
            pp.Money = userMoney - totalMoney;
            _db.Update(pp);
            _db.SaveChanges();

            return Ok("Money Updated Successfully");
        }

/*        [HttpPost("User/Done")]
        public IActionResult savepay()
        {
            Transaction tr;
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            var objList = _db.Transaction.Where(p => p.User_Id == Reg_Id).ToList();
            var productList = new List<Transaction>();

            for (var i = 0; i < objList.Count(); i++)
            {
                tr = _db.Transaction.FirstOrDefault(s => s.Status == "Pending");
                tr.Status = "Done";

                _db.Update(tr);
                _db.SaveChanges();
            }

            return Ok("Payment is Done Successfully");
        }*/

    }

}
