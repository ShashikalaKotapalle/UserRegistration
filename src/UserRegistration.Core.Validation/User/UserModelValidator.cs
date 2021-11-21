using FluentValidation;
using UserRegistration.Core.Models.User;

namespace UserRegistration.Core.Validation.User
{
	public class UserModelValidator : AbstractValidator<UserModel>
	{
		public UserModelValidator()
		{
			RuleFor(m => m.UserName).NotEmpty().MaximumLength(100);
		}
	}
}
