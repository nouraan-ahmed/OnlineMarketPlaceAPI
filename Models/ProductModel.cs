using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarketplaceAPI.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        //   public string Image { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        //public int? User_Id { get; set; }
        //[ForeignKey("User_Id")]
      //   public int? SecondaryUser { get; set; }
        //[ForeignKey("SecondaryUser")]
        // [JsonIgnore]
        //public virtual User User { get; set; }
        //public int Status { get; set; }
    }
}
