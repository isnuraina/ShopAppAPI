using FluentValidation;
using ShopAppAPI.Apps.AdminApp.Dtos.UserDto;

namespace ShopAppAPI.Apps.AdminApp.Validators.UserValidators
{
    public class LoginDtoValidator:AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(r => r.UserName)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(r => r.Password)
                .MaximumLength(15)
                .MinimumLength(6);
        }
    }
}
