using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcRentC.Models.ModelsForWebService
{
    public class CarsDTO
    {
        public int CarID { get; set; }
        public string Plate { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public decimal PricePerDay { get; set; }

    }
}