using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.Manage.ViewModels
{
    public class LoginVM
    {
        [Required]
        [MinLength(4)]
        [MaxLength(254)]
        public string UsernameOrEmail { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemembered { get; set; }
    }
}
