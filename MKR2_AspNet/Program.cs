using Microsoft.EntityFrameworkCore;
using MKR1_AspNet;
using MKR1_AspNet.Entities;
using MKR2_AspNet.Entities;
using MKR2_AspNet.NewFolder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<CustomExceptionFilter>();

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SchoolDb;Trusted_Connection=True;"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    if (!context.Classrooms.Any())
    {
        context.Classrooms.Add(new Classroom { Name = "Default Class" });
    }

    if (!context.Books.Any())
    {
        context.Books.AddRange(
            new Book { Title = "Clean Code", Author = "Robert C. Martin" },
            new Book { Title = "Refactoring", Author = "Martin Fowler" }
        );
    }

    context.SaveChanges();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();

app.MapControllers();

app.Run();
