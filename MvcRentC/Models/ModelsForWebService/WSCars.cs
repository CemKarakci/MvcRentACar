using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcRentC.Models.ModelsForWebService
{
    public class WSCars
    {
        public static List<CarsDTO> ListCars()
        {
            var db = new RentCDataBaseEntities();
            var autos = db.Cars.ToList();
            var CarsDTO = ConvertToDTO(autos);
            return CarsDTO;
        }

        private static List<CarsDTO> ConvertToDTO(List<Cars> autos)
        {
            var carsDTO = new List<CarsDTO>();
            foreach (var auto in autos)
            {
                var carDto = new CarsDTO();
                carDto.CarID = auto.CarID;
                carDto.Plate = auto.Plate;
                carDto.Manufacturer = auto.Manufacturer;
                carDto.Model = auto.Model;
                carDto.PricePerDay = auto.PricePerDay;

                carsDTO.Add(carDto);
            }

            return carsDTO;
        }

        public static List<ReservationsDTO> ListReservations()
        {
            var db = new RentCDataBaseEntities();
            var reservs = db.Reservations.ToList();
            var rez = ConvertToDTORez(reservs);

            return rez;
        }

        private static List<ReservationsDTO> ConvertToDTORez(List<Reservations> reservs)
        {
            var reservationsDTO = new List<ReservationsDTO>();

            foreach (var reserv in reservs)
            {
                var reservationDto = new ReservationsDTO();
                reservationDto.ReservationID = reserv.ReservationID;
                reservationDto.CarID = reserv.CarID;
                reservationDto.CostumerID = reserv.CostumerID;
                reservationDto.ReservStatsID = reserv.ReservStatsID;
                reservationDto.StartDate = reserv.StartDate;
                reservationDto.EndDate = reserv.EndDate;
                reservationDto.Location = reserv.Location;
                reservationDto.CouponCode = reserv.CouponCode;

                reservationsDTO.Add(reservationDto);

            }
            return reservationsDTO;
        }

        public static List<ReservationStatsDTO> ListReservStats()
        {
            var db = new RentCDataBaseEntities();
            var stats = db.ReservationStatuses.ToList();
            var stat = ConvertToDTOStats(stats);

            return stat;
        }

        private static List<ReservationStatsDTO> ConvertToDTOStats(List<ReservationStatuses> stats)
        {
            var statsDTO = new List<ReservationStatsDTO>();
            foreach (var stat in stats)
            {
                var statusDto = new ReservationStatsDTO();
                statusDto.ReservStatsID = stat.ReservStatsID;
                statusDto.Name = stat.Name;
                statusDto.Description = stat.Description;

                statsDTO.Add(statusDto);
            }
            return statsDTO;
        }
    }
}