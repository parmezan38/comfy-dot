using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace IdServer.Models
{
    public class ConnectionData
    {
        public string UserId { get; set; }
        public RoomDisplayable Room { get; set; }
    }
}
