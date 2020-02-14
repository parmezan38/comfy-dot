using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdServer.Models
{
    public class UserDisplayable
    {
        public string Name { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
    }
}
