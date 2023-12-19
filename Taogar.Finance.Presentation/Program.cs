using Logger;
using Microsoft.OpenApi.Models;
using Taogar.Finance.Application.Interfaces;
using Taogar.Finance.Application.Services;
using Taogar.Finance.Auth.Extensions;
using Taogar.Finance.Presentation.Middlewares;
using Taogar.Finance.Infrastructure.Extensions;
using Taogar.Finance.Database.Extensions;
using Taogar.Finance.Auth.Interfaces;
using Taogar.Finance.Database.Interfaces;
using Taogar.Finance.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<Logger.ILogger, ConsoleLogger>();
builder.Services.AddKeyCloakAuthentication();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    var xmlPath = Path.Combine(AppContext.BaseDirectory, "Taogar.Finance.Presentation.xml");
    c.IncludeXmlComments(xmlPath);
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Finance Service",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
});
builder.Services.AddFinancyDBContext();
builder.Services.AddFinancyReposytories();
builder.Services.AddScoped<IPersonService>(provider =>
{
    if(((IUserService)provider.GetRequiredService(typeof(IUserService))).Role == "Manager")
    {
        return new PersonServiceForManagers((Logger.ILogger)provider.GetRequiredService(typeof(Logger.ILogger)),
            (IGenericRepository<Person>)provider.GetRequiredService(typeof(IGenericRepository<Person>)),
            (IKeyCloakService)provider.GetRequiredService(typeof(IKeyCloakService)));
    }
    else
    {
        return new PersonServiceForUser((Logger.ILogger)provider.GetRequiredService(typeof(Logger.ILogger)),
            (IGenericRepository<Person>)provider.GetRequiredService(typeof(IGenericRepository<Person>)),
            (IUserService)provider.GetRequiredService(typeof(IUserService)));
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<ErrorMiddleware>();
app.Run();
