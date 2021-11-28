using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_BAQS.DTOs.Login
{
    public class LoginDTO
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
