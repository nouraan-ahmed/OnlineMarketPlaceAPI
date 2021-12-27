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
        public IActionResult GetProduct(string name)
        {
            var product = _db.Product.Where(p => p.Name == name).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
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
                p.Id = obj.Id;
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


            //Edit information of spasciffic product
            [HttpPut("update-product-by-id/{id}")]
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
 

    /*        [HttpPost]
            public IActionResult CreateProduct([FromBody]Product product)
            {
                _db.Product.Add(product);
                _db.SaveChanges();
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }*/

}
}
