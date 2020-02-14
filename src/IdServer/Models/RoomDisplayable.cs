using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdServer.Models
{
    public class RoomDisplayable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfUsers { get; set; }
        public int Capacity { get; set; }
        public bool Owned { get; set; }
    }
}
