using FluentValidation;
using ShopAppAPI.Apps.AdminApp.Dtos.UserDto;

namespace ShopAppAPI.Apps.AdminApp.Validators.UserValidators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(r => r.Email).EmailAddress().NotEmpty();
            RuleFor(r => r.FullName)
                .NotEmpty()
                .MaximumLength(30);
            RuleFor(r => r.UserName)
                .NotEmpty()
                .MaximumLength(30);
            RuleFor(r => r.Password)
                .MaximumLength(15)
                .MinimumLength(6);
            RuleFor(r => r.RePassword)
          .MaximumLength(15)
          .MinimumLength(6);
            RuleFor(r => r).Custom((r, context) =>
            {
                if (r.Password != r.RePassword)
                {
                    context.AddFailure("Password", "does not match");
                }
            });
        }
    }
}
