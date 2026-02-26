using FluentValidation.AspNetCore;
using LibraryManagementAPI.Extensions;
using LMS.Application;
using LMS.Application.Wrappers;
using LMS.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddIdentityAndJwt(builder.Configuration);

builder.Services.AddControllers(o =>
{
    o.Filters.Add<ExceptionFilter>();
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerAndOpenAI(builder.Configuration);

var app = builder.Build();

await app.UseDbSeeder();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocExpansion(DocExpansion.None);
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();