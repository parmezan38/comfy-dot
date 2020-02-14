using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdServer.Models
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(60, ErrorMessage = "Name should be shorter than 60 characters.")]
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

    }
}
