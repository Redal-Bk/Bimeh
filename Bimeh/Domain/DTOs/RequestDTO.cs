using System.ComponentModel.DataAnnotations;

namespace Bimeh.Domain.DTOs
{
    public class RequestDTO
    {
        [Required]
        public string TitleRequst { get; set; } = null!;
        [Required]
        public string[] InsuranceCoverage { get; set; } = null!;
        [Required]
        public int Price { get; set; }
    }
}
