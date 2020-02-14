using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdServer.Models
{
    public class ChatMessage
    {
        public int RoomId { get; set; }
        public UserDisplayable User { get; set; }
        public string Message { get; set; }
    }
}
