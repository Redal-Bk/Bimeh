using System.ComponentModel.DataAnnotations;

namespace Bimeh.Domain.DTOs
{
    public class UserDTO
    {
        [Required(ErrorMessage = "وارد کردن نام کاربری الزامیست")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage ="وارد کردن رمز عبور اجباریست")]
        public string Password { get; set; } = null!;
    }
}
