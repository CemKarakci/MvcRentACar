using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcRentC.Models
{
    public class UserViewModel
    {
        public UserViewModel() { }
        public UserViewModel(ApplicationUser user)
        {
            Id = user.Id;
            UserName = user.UserName;
            
        }
        public string Id { get; set; }
        public string UserName { get; set; }
       

    }
}