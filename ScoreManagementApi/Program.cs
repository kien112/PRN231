using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using ScoreManagementApi.Core.DbContext;
using ScoreManagementApi.Core.Entities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
***REMOVED***
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    ***REMOVED***
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
***REMOVED***);

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
            ***REMOVED***
                ***REMOVED***
                    new OpenApiSecurityScheme
                    ***REMOVED***
                        Reference = new OpenApiReference
                        ***REMOVED***
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                ***REMOVED***
            ***REMOVED***,
                    Array.Empty<string>()
        ***REMOVED***
    ***REMOVED***);
***REMOVED***);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//add cors
builder.Services.AddCors(options =>
***REMOVED***
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        ***REMOVED***
            builder
                .WithOrigins("http://127.0.0.1:5500", "http://localhost:5095")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("FileName");
***REMOVED***);
***REMOVED***);


//add db
builder.Services.AddDbContext<ApplicationDbContext>(options =>
***REMOVED***
    var connectionString = builder.Configuration.GetConnectionString("ConnectString");
    options.UseSqlServer(connectionString);
***REMOVED***);

//add identity
builder.Services
    .AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


//config identity
builder.Services.Configure<IdentityOptions>(options =>
***REMOVED***
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedEmail = false;
***REMOVED***);


//add authentication and Jwt
builder.Services
    .AddAuthentication(option =>
    ***REMOVED***
        option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

***REMOVED***)
    .AddJwtBearer(options =>
    ***REMOVED***
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        ***REMOVED***
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:ValidateIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidateAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
***REMOVED***;
***REMOVED***);


var app = builder.Build();

if (app.Environment.IsDevelopment())
***REMOVED***
    app.UseSwagger();
    app.UseSwaggerUI();
***REMOVED***

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
