using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentAll.Data;
using RentAll.Model;
using RentAll.Services;
using System.Text;

//builder is initialized
var builder = WebApplication.CreateBuilder(args);


//for CORS service this allows the 2 server to communicate.
const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";



//Adding cors services for the 3000 or the server hosted on.

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
    builder =>
    {
        builder.WithOrigins(["JWT:ClientUrl"]).AllowAnyHeader().AllowAnyMethod();
    });
});


// Add services to the container.
builder.Services.AddControllers();

//Configuring service to return error object when error occurs in the server.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
        .Where(x => x.Value.Errors.Count > 0)   //displays if error count is greater than 0
        .SelectMany(x => x.Value.Errors)
        .Select(x => x.ErrorMessage).ToArray(); //it will convert into string array of error.

        var toReturn = new
        {
            Errors = errors
        };
        return new BadRequestObjectResult(toReturn);
    };
});


// we have setup middleware to use AppDbContext class to be registered as a service
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//be able to inject JWTService class inside our controllers
builder.Services.AddScoped<JWTService>();
//we add the service so we can inject EmailService inside our controllers.
builder.Services.AddScoped<EmailService>();

//Defining our IdentityCore Service
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequiredLength = 10;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;

    //for email confirmation
    options.SignIn.RequireConfirmedEmail = true;

})
    .AddRoles<IdentityRole>() //be able to add roles
    .AddRoleManager<RoleManager<IdentityRole>>() // be able to make user of RoleManager
    .AddEntityFrameworkStores<AppDbContext>()//providing our context
    .AddSignInManager<SignInManager<User>>() //make user of signin manager
    .AddUserManager<UserManager<User>>() //make use of usermanager to create user
    .AddDefaultTokenProviders(); //be able to create tokens for email confirmation



//adding authentication in order to authenticate user using JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        //validate the token based on the key we hve provided inside appsettings.json
        ValidateIssuerSigningKey = true,
        //the issuer signing key based on JWT: Key
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        //validate the issuer (who ever is issuing the jwt)
        ValidateIssuer = true,
        //don't validate audience (angular side)
        ValidateAudience = false
    };
});



builder.Services.AddAuthorization();

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

//Adding userAuthentication into our pipeline and this should come before authorization
//Authentication verifies the identity of a user or service and authorization determines their access rights.
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
