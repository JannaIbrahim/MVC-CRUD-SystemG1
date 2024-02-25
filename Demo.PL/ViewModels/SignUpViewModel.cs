using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class SignUpViewModel
	{
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FName { get; set; }

		[Required(ErrorMessage = "Last Name is required")]
		public string LName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

		[Required(ErrorMessage = "ConfirmPassword is required")]
        [Compare(nameof(Password) , ErrorMessage = "Confirm password does not match Password")]
        [DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}
