using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace IdServer.Models
{
    public class RoomConnectionInfo
    {
        public ConcurrentDictionary<string, string> ConnectionIds { get; set; }
        public int Capacity { get; set; }
    }
}
