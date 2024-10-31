using api.Configurations;
using api.Data;
using api.Interfaces;
using api.Repositories;
using CloudinaryDotNet;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string corsPolicyName = "all";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
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

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductDetailRepository, ProductDetailRepository>();
builder.Services.AddScoped<ICloudinaryRepository, CloudinaryRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// Register Cloudinary as a singleton service
Account cloudinaryAccount = new Account(cloudinaryConfig.CloudName, cloudinaryConfig.ApiKey, cloudinaryConfig.ApiSecret);
Cloudinary cloudinary = new Cloudinary(cloudinaryAccount);
cloudinary.Api.Secure = true;
builder.Services.AddSingleton(cloudinary);

builder.Services.AddSingleton(zaloPayConfig);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
