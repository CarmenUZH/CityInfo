using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class POIforUpdateDto
    {
        [Required(ErrorMessage = "Damn boy, can you at LEAST give me a name?")]
        [MaxLength(55, ErrorMessage="This duud thinks he can get away with writing some buuullshit in the name, huh?")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200, ErrorMessage = "Glad you had fun but you dont need to tell me your WHOLE life story, ya know?")]
        public string? Description { get; set; }
    }
}
