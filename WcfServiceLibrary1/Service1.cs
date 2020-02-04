using MvcRentC.Models.ModelsForWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary1
{
    // NOT: "Service1" sınıf adını kodda ve yapılandırma dosyasında birlikte değiştirmek için "Yeniden Düzenle" menüsündeki "Yeniden Adlandır" komutunu kullanabilirsiniz.
    public class Service1 : IService1
    {
        public List<CarsDTO> ListAvailableCars(Date dates)
        {

            var cars = MvcRentC.Models.ModelsForWebService.WSCars.ListCars();
            var reservations = MvcRentC.Models.ModelsForWebService.WSCars.ListReservations();
            var statuses = MvcRentC.Models.ModelsForWebService.WSCars.ListReservStats();
            var canceledReservations = statuses.FindAll(x => x.Name == "CANCELED").ToList();
            var result = reservations.Where(p => !canceledReservations.Any(x => x.ReservStatsID == p.ReservStatsID)).ToList();

            var carsRented = from b in result
                             where
                                     ((dates.StartDate >= b.StartDate) && (dates.StartDate <= b.EndDate)) ||
                                     ((dates.EndDate >= b.StartDate) && (dates.EndDate <= b.EndDate)) ||
                                     ((dates.StartDate <= b.StartDate) && (dates.EndDate >= b.StartDate) && (dates.EndDate <= b.EndDate)) ||
                                     ((dates.StartDate >= b.StartDate) && (dates.StartDate <= b.EndDate) && (dates.EndDate >= b.EndDate)) ||
                                     ((dates.StartDate <= b.StartDate) && (dates.EndDate >= b.EndDate))
                             select b;

            var availableCars = cars.Where(r => !carsRented.Any(b => b.CarID == r.CarID)).ToList();
            return availableCars;
        }
    }
}
