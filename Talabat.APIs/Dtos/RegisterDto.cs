using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string PnoneNumber { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
