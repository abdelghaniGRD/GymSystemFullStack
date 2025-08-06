using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using GymSystem.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<GymSystemContext>(options =>
options.UseSqlServer(
builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentity<ApiUser, IdentityRole>(options => 
{
	options.Password.RequireDigit = true; 
	options.Password.RequireLowercase = true; 
	options.Password.RequireUppercase = true; 
	options.Password.RequireNonAlphanumeric = true; 
	options.Password.RequiredLength = 12; 
})
.AddEntityFrameworkStores<GymSystemContext>();

builder.Services.AddAuthentication(options => {
	options.DefaultAuthenticateScheme =
	options.DefaultChallengeScheme =
	options.DefaultScheme =
	options.DefaultSignInScheme =
	options.DefaultSignOutScheme =
JwtBearerDefaults.AuthenticationScheme;
	}).AddJwtBearer(options => {
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidIssuer = builder.Configuration["JWT:Issuer"],
		ValidateAudience = true,
		ValidAudience = builder.Configuration["JWT:Audience"],
		ValidateIssuerSigningKey = true,
		//ValidateLifetime = true,
		IssuerSigningKey = new SymmetricSecurityKey(
		System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
	};
		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				context.Token = context.Request.Cookies["AuthToken"]; // Get token from cookie
				return Task.CompletedTask;
			}
		};

	}
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "bearer"
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin", builder =>
	{
		builder.WithOrigins("http://localhost:3000") // Allow requests from this origin
			   .AllowAnyMethod()                    // Allow any HTTP method (GET, POST, etc.)
			   .AllowAnyHeader()                    // Allow any headers (e.g., Content-Type)
			   .AllowCredentials();                 // Allow credentials (cookies, etc.)
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage();
}


//app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();

app.UseAuthorization();


app.MapGet("/error/test", () => { throw new Exception("test"); });

app.MapControllers();

app.Run();
