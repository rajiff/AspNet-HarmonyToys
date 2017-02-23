using System.ComponentModel.DataAnnotations;

namespace HT.UserViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email ID can't be empty")]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Password can't be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
