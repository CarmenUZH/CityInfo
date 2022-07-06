using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class POIforCreationDto //Seperate dto for creating, updating and returnig resources
    {
        [Required]
        [MaxLength(55)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
