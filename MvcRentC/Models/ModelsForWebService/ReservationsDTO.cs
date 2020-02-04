using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcRentC.Models.ModelsForWebService
{
    public class ReservationsDTO
    {
        public int ReservationID { get; set; }
        public int CarID { get; set; }
        public int CostumerID { get; set; }
        public byte ReservStatsID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Location { get; set; }
        public string CouponCode { get; set; }
    }
}