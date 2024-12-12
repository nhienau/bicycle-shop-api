using api.Configurations;
using api.Data;
using api.Interfaces;
using api.Repositories;
using CloudinaryDotNet;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using api.Services;


var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

Env.Load();

// Configure Cloudinary
CloudinaryConfig cloudinaryConfig = new CloudinaryConfig
{
    CloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME") ?? throw new ArgumentException("CLOUDINARY_CLOUD_NAME is missing."),
    ApiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY") ?? throw new ArgumentException("CLOUDINARY_API_KEY is missing."),
    ApiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET") ?? throw new ArgumentException("CLOUDINARY_API_SECRET is missing.")
};

ZaloPayConfig zaloPayConfig = new ZaloPayConfig
{
    AppId = Environment.GetEnvironmentVariable("ZALOPAY_APP_ID") ?? throw new ArgumentException("ZALOPAY_APP_ID is missing."),
    Key1 = Environment.GetEnvironmentVariable("ZALOPAY_KEY1") ?? throw new ArgumentException("ZALOPAY_KEY1 is missing."),
    Key2 = Environment.GetEnvironmentVariable("ZALOPAY_KEY2") ?? throw new ArgumentException("ZALOPAY_KEY2 is missing."),
    Endpoint = Environment.GetEnvironmentVariable("ZALOPAY_API_ENDPOINT") ?? throw new ArgumentException("ZALOPAY_API_ENDPOINT is missing."),
    RedirectUrl = Environment.GetEnvironmentVariable("ZALOPAY_REDIRECT_URL") ?? throw new ArgumentException("ZALOPAY_REDIRECT_URL is missing."),
    CallbackUrl = Environment.GetEnvironmentVariable("ZALOPAY_CALLBACK_URL") ?? throw new ArgumentException("ZALOPAY_CALLBACK_URL is missing.")
};

VNPayConfig vnPayConfig = new VNPayConfig
{
    TmnCode = Environment.GetEnvironmentVariable("VNPAY_TMN_CODE") ?? throw new ArgumentException("VNPAY_TMN_CODE is missing."),
    HashSecret = Environment.GetEnvironmentVariable("VNPAY_HASH_SECRET") ?? throw new ArgumentException("VNPAY_HASH_SECRET is missing."),
    BaseUrl = Environment.GetEnvironmentVariable("VNPAY_URL") ?? throw new ArgumentException("VNPAY_URL is missing."),
    Command = "pay",
    CurrCode = "VND",
    Version = Environment.GetEnvironmentVariable("VNPAY_VERSION") ?? throw new ArgumentException("VNPAY_VERSION is missing."),
    Locale = "vn",
    CallbackUrl = Environment.GetEnvironmentVariable("VNPAY_CALLBACK_URL") ?? throw new ArgumentException("VNPAY_CALLBACK_URL is missing."),
};

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    // Cau hinh Swagger Bearer Token
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter a valid token"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Thêm các c?u hình khác n?u c?n
});

string corsPolicyName = "specificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:5176").AllowAnyHeader().AllowAnyMethod().AllowCredentials();// Cho phép thông tin xác th?c (cookie, token);
        });
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IProductDetailRepository, ProductDetailRepository>();
builder.Services.AddScoped<ICloudinaryRepository, CloudinaryRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IVNPayService, VNPayService>();

// Register Cloudinary as a singleton service
Account cloudinaryAccount = new Account(cloudinaryConfig.CloudName, cloudinaryConfig.ApiKey, cloudinaryConfig.ApiSecret);
Cloudinary cloudinary = new Cloudinary(cloudinaryAccount);
cloudinary.Api.Secure = true;
builder.Services.AddSingleton(cloudinary);

builder.Services.AddSingleton(zaloPayConfig);
builder.Services.AddSingleton(vnPayConfig);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseAuthentication(); // ----
app.UseAuthorization();

app.MapControllers();

app.Run();
