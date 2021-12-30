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


        [HttpPost("Deposit")]
        public IActionResult Deposit(int money)
        {
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            var user = _db.User.Find(Reg_Id);
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
            var user = _db.User.Find(Reg_Id);
            if (user.Wallet < totalMoney)
            {
                return Ok("you don't have enough monet to pay");
            }
            for (var i = 0; i < products.Count(); i++)
            {
                int seller_id = _db.Transaction.Where(f => f.Product_Id == products[i]).Select(d => d.Seller_Id).FirstOrDefault();
                var user2 = _db.User.Find(seller_id);
                user2.Wallet += _db.Product.Where(v => v.Id == products[i]).Select(d => d.Price).FirstOrDefault();
                _db.Update(user2);
                _db.SaveChanges();
                //int product_p_id = _db.Product.Where(v => v.Id == products[i]).Select(d => d.Id).FirstOrDefault();
                //var product = _db.Product.Find(product_p_id);
                //product.Status = 0;
                //_db.Update(product);
                //_db.SaveChanges();
            }
            user.Wallet -= totalMoney;
            _db.Update(user);
            _db.SaveChanges();
            return Ok(totalMoney);
        }

    }

}
