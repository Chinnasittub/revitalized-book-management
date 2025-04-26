using BookManagementPrj.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookManagementContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "BookManagementAPI", 
        Version = "v1",
        Description = "This is Technical Assessment Question." 
    });
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

app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("Application has started.");
});

app.Lifetime.ApplicationStopped.Register(() =>
{
    Console.WriteLine("Application is stopping.");
});

app.MapControllers();

app.Run();
