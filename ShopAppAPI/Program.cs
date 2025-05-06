using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopAppAPI.Apps.AdminApp.Validators.ProductValidator;
using ShopAppAPI.Data;
using ShopAppAPI.Entities;
using ShopAppAPI.Profiles;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation(
    config=>config.RegisterValidatorsFromAssemblyContaining<ProductCreateDtoValidator>());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ShopAppContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("ApiConnection"));
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddFluentValidationRulesToSwagger();
builder.Services.AddValidatorsFromAssemblyContaining<ProductCreateDtoValidator>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(opt =>
{
    opt.AddProfile(typeof(MapperProfile));
});


builder.Services.AddAutoMapper(opt =>
{
    opt.AddProfile(new MapperProfile(new HttpContextAccessor()));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
}).AddEntityFrameworkStores<ShopAppContext>().AddDefaultTokenProviders();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
