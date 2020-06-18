using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Doggo.Models
{
    public class Walker
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Neighborhood")]
        public int NeighborhoodId { get; set; }
        [DisplayName("Image")]
        public string ImageUrl { get; set; }
        public Neighborhood Neighborhood { get; set; }
    }
}
