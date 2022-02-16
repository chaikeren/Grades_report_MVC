using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set;}

    }
}
