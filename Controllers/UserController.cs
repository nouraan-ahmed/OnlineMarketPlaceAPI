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
    public class UserController : ControllerBase
    {
        private readonly MarketplaceContext _db;
        public UserController(MarketplaceContext db)
        {
            _db = db;
        }
        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            _db.User.Add(user);
            _db.SaveChanges();
            HttpContext.Session.SetInt32("Reg_Id", user.Id);

            return Ok("Successfully Registered");
        }
    }
}
