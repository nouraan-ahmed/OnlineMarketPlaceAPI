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
    }
}
