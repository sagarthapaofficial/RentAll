using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RentAll.DAL;
using System.Text;

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

//To create/process JSON Web Token Authentication
// jwt addition
// get key from settings
var appSettings = builder.Configuration.GetSection("AppSettings").GetValue<string>("Secret");
var key = Encoding.ASCII.GetBytes(appSettings);

// add scheme and options
//this is telling the server to use JWT to do its authentication.
builder.Services.AddAuthentication(scheme =>
{
    scheme.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    scheme.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(option =>
{
    option.RequireHttpsMetadata = false;
    option.SaveToken = true;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});



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

app.UseDefaultFiles();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
