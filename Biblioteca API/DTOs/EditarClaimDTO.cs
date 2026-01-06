using System.ComponentModel.DataAnnotations;

namespace Biblioteca_API.DTOs
{
    public class EditarClaimDTO
    {
        [Required]
        [EmailAddress]
       public required string Email { get; set; }
    }
}
