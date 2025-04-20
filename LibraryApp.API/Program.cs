using FluentValidation;
using LibraryApp.Application.Concrete;
using LibraryApp.Application.Providers.Validator;
using LibraryApp.Application.Registrations;
using LibraryApp.Application.Services;
using LibraryApp.Data.Context;
using LibraryApp.Data.Registrations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddBusinessService();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var validatorAssemblies = ValidatorAssemblyProvider.GetValidatorAssemblies();

foreach (var assemblyType in validatorAssemblies)
    builder.Services.AddValidatorsFromAssemblyContaining(assemblyType);


var dtoValidatorAssemblies = DTOValidatorAssemblyProvider.GetValidatorAssemblies();

foreach (var assemblyType in dtoValidatorAssemblies)
    builder.Services.AddValidatorsFromAssemblyContaining(assemblyType);

var app = builder.Build();



// Veritabanı migrationlarını uygula (tek seferlik)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // <-- Bu satır çok önemli
}

// HTTPS yönlendirme (opsiyonel ama önce olabilir)
app.UseHttpsRedirection();

// Swagger prod'da da görünsün istiyorsan bunu ayrıca da çağırabilirsin
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers(); // Bu en sona yakın kalmalı

app.Run();
