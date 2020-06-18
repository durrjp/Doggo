using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Doggo.Models
{
    public class Dog
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Owner")]
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
        [Required]
        public string Breed { get; set; }
        public string Notes { get; set; }
        [DisplayName("Image")]
        public string ImageURL { get; set; }
    }
}
