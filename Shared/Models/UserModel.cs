using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class UserModel
    {
        [Key] // This explicitly marks the property as the primary key
        public int Id { get; set; } // Primary key
        public string Username { get; set; }

        public string Password { get; set; }

    }
}
