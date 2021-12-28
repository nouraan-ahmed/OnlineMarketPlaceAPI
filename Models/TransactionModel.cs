using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarketplaceAPI.Models
{
    public class TransactionModel
    {
        [Required]
        public string Buyer_Name { get; set; }
        [Required]
        public string Seller_Name { get; set; }
        [Required]
        public string Product_Name { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
