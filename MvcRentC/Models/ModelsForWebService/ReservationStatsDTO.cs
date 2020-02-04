using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcRentC.Models.ModelsForWebService
{
    public class ReservationStatsDTO
    {
        public byte ReservStatsID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}