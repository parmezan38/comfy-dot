using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdServer.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Color1 { get; set; }
        [Required]
        public string Color2 { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
