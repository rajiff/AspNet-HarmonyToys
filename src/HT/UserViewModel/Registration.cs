using System.ComponentModel.DataAnnotations;

namespace HT.UserViewModel
{
    public class Registration
    {
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string Fname { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string Lname { get; set; }

        [Required(ErrorMessage = "Email ID is required")]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        [Display(Name = "Mobile")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "PhoneNumber should contain only numbers and must be 10 digits long")]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        public bool IsAdmin { get; set; }
    }
}
