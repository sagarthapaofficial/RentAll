using Microsoft.EntityFrameworkCore;
using RentAll.DAL;

var builder = WebApplication.CreateBuilder(args);


//for CORS service this allows the 2 server to communicate.
const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


//Adding cors services for the 3000 or the server hosted on.

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
    builder =>
    {
        builder.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
});



// Add services to the container.

builder.Services.AddControllers();

// we have setup middleware to use AppDbContext class to be registered as a service
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
