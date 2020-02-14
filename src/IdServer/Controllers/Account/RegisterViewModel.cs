using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdServer.Data;
using IdServer.Utilities;
using Microsoft.EntityFrameworkCore;

namespace IdServer.Controllers.UI
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public string ReturnUrl { get; set; }
    }
}
