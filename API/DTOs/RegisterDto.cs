using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [StringLength(60, MinimumLength = 3)]
        [Required(AllowEmptyStrings =false)]
        public string Username {get; set;}
        
         [Required(AllowEmptyStrings =false)]
        [StringLength(8,MinimumLength =4)]
        public string Password {get; set;}
    }
}