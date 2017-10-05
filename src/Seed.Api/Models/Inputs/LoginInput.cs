using System.ComponentModel.DataAnnotations;
using Destructurama.Attributed;

namespace Seed.Api.Models.Inputs
{
    public class LoginInput
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [NotLogged]
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}