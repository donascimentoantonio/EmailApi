using Serilog;
using EmailManagement.IoC;
using EmailManagement.Api.Middlewares;
using EmailManagement.Api.Extensions;
using EmailManagement.Api.Filters;
using EmailManagement.Domain.Models.Email;
using EmailManagement.Application.Services;
using EmailManagement.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomResultFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<EmailApiSettings>(builder.Configuration.GetSection("EmailApi"));
builder.Services.AddHttpClient<IHttpClientService, HttpClientService>();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddService();
builder.Services.AddInfra();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorMiddleware>();

app.Run();
