using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcRentC.Models
{
   
        public class Dates
        {
            [Required]
            //[DataType(DataType.Date)]
            //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime StartDate { get; set; }
            [Required]
            //[DataType(DataType.Date)]
            //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime EndDate { get; set; }
        }
    
}