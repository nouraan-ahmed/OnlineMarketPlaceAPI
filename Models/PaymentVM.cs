using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketplaceAPI.Models
{
    public class PaymentVM
    {
        [Key]
        public int Id { get; set; }
        public string CardHolderName { get; set; }

        public int CardNumber { get; set; }

        //[DataType(DataType.Date)]
        public string ExpDate { get; set; }

        public int CVcode { get; set; }

        public int ZipCode { get; set; }
    }
}
