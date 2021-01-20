using System.ComponentModel.DataAnnotations;

namespace Comics.Web.ViewModel
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "OldPassword")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "NewPassword")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "PasswordConfirm")]
        [DataType(DataType.Password)]
        public string NewPasswordConfirm { get; set; }
    }
}
