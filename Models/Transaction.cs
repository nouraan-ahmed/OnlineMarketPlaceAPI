using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarketplaceAPI.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int? User_Id { get; set; }
        [ForeignKey("User_Id")]
        [JsonIgnore]
        public virtual User User { get; set; }
        public int Seller_Id { get; set; }
        public int? Product_Id { get; set; }
        [ForeignKey("Product_Id")]
        [JsonIgnore]
        public virtual Product Product { get; set; }
        public string Status { get; set; }
    }
}
