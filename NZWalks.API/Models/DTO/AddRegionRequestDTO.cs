using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Minimum Three Characters")]
        [MaxLength(3, ErrorMessage = "Maximum Three Characters")]
        public string Code { get; set; }
        public string Name { get; set; }
        public string? ImageURL { get; set; }
    }
}
