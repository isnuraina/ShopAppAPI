using FluentValidation;
using ShopAppAPI.Apps.AdminApp.Dtos.CategoryDto;

namespace ShopAppAPI.Apps.AdminApp.Validators.CategoryValidators
{
    public class CategoryCreateValidator:AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("not empty")
                .MaximumLength(50).WithMessage("maximum length 50...");
        }
    }
}
