using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdServer.Controllers.UI
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberLogin { get; set; }
        
    }
}
