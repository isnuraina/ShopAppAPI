using FluentValidation;
using ShopAppAPI.Apps.AdminApp.Dtos.ProductDto;
using ShopAppAPI.Entities;

namespace ShopAppAPI.Apps.AdminApp.Validators.ProductValidator
{
    public class ProductCreateDtoValidator:AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(p => p.Name).NotEmpty()
                .WithMessage("not empty...")
                .MaximumLength(50)
                .WithMessage("maximum length 50....");
            RuleFor(p => p.SalePrice).NotEmpty()
                .WithMessage("not empty...")
                .GreaterThan(100).WithMessage("should be greather than");
            RuleFor(p => p.CostPrice).NotEmpty()
               .WithMessage("not empty...")
               .GreaterThan(100).WithMessage("should be greather than");
            RuleFor(p => p)
            .Custom((p,context) =>
            {
                context.AddFailure("CostPrice", "CostPrice must be less SalePrice");
            });
        }
    }
 
}
