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
    public class ProductsController : ControllerBase
    {
        private readonly MarketplaceContext _db;
        public ProductsController(MarketplaceContext db)
        {
            _db = db;
        }

        //Get All Products API

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var Products = _db.Product.ToList();
            return Ok(Products);
        }


        [HttpGet("Report")]
        public IActionResult PrintReport(int id)
        {
            if (_db.Product.Find(id) == null)
            {
                return NotFound();
            }

            int d = _db.Transaction.Where(p => p.Product_Id == id).Select(o => o.Id).FirstOrDefault();
            Transaction inst = _db.Transaction.Find(d);
            TransactionModel tran = new TransactionModel();
            tran.Buyer_Name = _db.User.Where(p => p.Id == inst.Seller_Id).Select(o => o.Name).FirstOrDefault();
            tran.Seller_Name = _db.User.Where(p => p.Id == inst.User_Id).Select(o => o.Name).FirstOrDefault();
            tran.Product_Name = _db.Product.Where(p => p.Id == inst.Product_Id).Select(o => o.Name).FirstOrDefault();
            tran.Status = inst.Status;
            if (inst.Id == 0)
            {
                return NotFound();
            }

                return Ok(tran);
           
        }


        //get the products of other users (not the logged in one)
        [HttpGet("Others")]
        public IActionResult GetOthersAllProducts()
        {
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            var otherItems = _db.Product.Where(a => a.User_Id != Reg_Id).ToList();
            return Ok(otherItems);
        }

        //get the products of the logged in user
        [HttpGet("User")]
        public IActionResult GetAllUserProducts()
        {
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            var Items = _db.Product.Where(a => a.User_Id == Reg_Id).ToList();
            return Ok(Items);
        }

        //Get Product With ID API

        [HttpGet("{id:int}")]
        public IActionResult GetProduct(int id)
        {
            var product = _db.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        //Search Products With Name API

        [HttpGet("{name}")]
        public IActionResult SearchProduct(string name)
        {
            var product = _db.Product.Where(p => p.Name == name).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductModel obj)
        {
            // string User_Email = HttpContext.Session.GetString("User_Email");
            // int User_id = _db.User.Where(p => p.Email == User_Email).Select(o => o.Id).FirstOrDefault();

            Product p = new Product();
            p.Name = obj.Name;
            p.Price = obj.Price;
            p.Quantity = obj.Quantity;
            p.Category = obj.Category;
            p.Description = obj.Description;
            // p.User_Id = _db.User.Where(p => p.Id == User_id).Select(o => o.Id).FirstOrDefault();
            p.User_Id = 2;
            p.Image = "https://www.lg.com/lg5-common/images/common/product-default-list-350.jpg";

            if (ModelState.IsValid)
            {
                _db.Add(p);
                _db.SaveChanges();
                var product = _db.Product.Find(p.Id);
                return Ok(product);
            }
            return Ok("Added product");
        }


        //Edit information of specific product
        [HttpPut("{id}")]
        public IActionResult EditProduct(int id, ProductModel Up_pro)
        {
            if (_db.Product.Find(id) == null)
                return Ok("object Not Found");
            else
            {
                //GetProduct(id);
                var obj = _db.Product.Find(id);
                // Product p = new Product();
                obj.Name = Up_pro.Name;
                obj.Price = Up_pro.Price;
                obj.Quantity = Up_pro.Quantity;
                obj.Category = Up_pro.Category;
                obj.Description = Up_pro.Description;
                obj.Id = id;
                // p.User_Id = obj.User_Id;
                if (ModelState.IsValid)
                {
                    //   _db.Update(p);
                    _db.SaveChanges();
                    return Ok(obj);
                }
            }
            return Ok("Not Updated");
        }

        //Delete a product from the Product's list
        [HttpDelete("{id:int}")]
        public IActionResult DeleteProduct(int id)
        {
            var instance = _db.Product.Find(id);

            if (instance == null)
            {
                return NotFound();
            }
            else
            {
                _db.Remove(instance);
                _db.SaveChanges();
                var Products = _db.Product.ToList();

                return Ok(Products);
            }
        }

        [HttpPost("Transfer")]
        public IActionResult Transfer(int id)
        {
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            var product = _db.Product.Where(a => a.Id == id).Select(a => a).FirstOrDefault();
            var product2 = new Product() { Name = product.Name, Price = product.Price, Image = product.Image, Category = product.Category, Quantity = product.Quantity, Description = product.Description, User_Id = product.User_Id, SecondaryUser = Reg_Id };
            var products = _db.Product.Where(a => a.Name == product2.Name && a.User_Id == product2.User_Id && a.SecondaryUser == product2.SecondaryUser).Select(a => a).ToList();


            if (products.Count() == 0)
            {
                _db.Add(product2);
                _db.SaveChanges();
            }

            return Ok("Product Transfered Successfully");
        }

        [HttpPost("Cart/{id}")]
        public IActionResult AddToCart(int id)
        {
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            var product = _db.Product.ToList().Where(p => p.Id == id).FirstOrDefault();
            Transaction tr = new Transaction();
            tr.Seller_Id = (int)product.User_Id;
            tr.User_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            tr.Status = "Pending";
            tr.Product_Id = id;
            _db.Add(tr);
            _db.SaveChanges();

            Product p = new Product();
            p = _db.Product.FirstOrDefault(s => s.Id == id);
            if (p.User_Id == tr.Seller_Id)
            {
                p.Status = 1;
                _db.Update(p);
                _db.SaveChanges();
            }
            Product pro = new Product();
            pro.Name = _db.Product.Where(o => o.Id == id).Select(p => p.Name).FirstOrDefault();
            pro.Price = _db.Product.Where(o => o.Id == id).Select(p => p.Price).FirstOrDefault();
            pro.Quantity = 1;
            pro.Description = _db.Product.Where(o => o.Id == id).Select(p => p.Description).FirstOrDefault();
            pro.Category = _db.Product.Where(o => o.Id == id).Select(p => p.Category).FirstOrDefault();
            // pro.Id = id;
            pro.Image = "https://www.lg.com/lg5-common/images/common/product-default-list-350.jpg";
            pro.User_Id = Reg_Id;

            _db.Add(pro);
            _db.SaveChanges();

            return  Ok("Sucessfully added to cart");
        }

    }
}
