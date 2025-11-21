using CMSBlog.API;
using CMSBlog.API.Services;
using CMSBlog.Core.Application.Interfaces.Media;
using CMSBlog.Core.Application.Services.Media;
using CMSBlog.Core.ConfigOptions;
using CMSBlog.Core.Domain.Identity;
using CMSBlog.Core.Models.Content;
using CMSBlog.Core.SeedWorks;
using CMSBlog.Data;
using CMSBlog.Data.Repositories;
using CMSBlog.Data.SeedWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scrutor;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ------------------------- CORS -------------------------
var CMSCorsPolicy = "_cmsCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(CMSCorsPolicy, policy =>
    {
        policy.WithOrigins(configuration["AllowedOrigins"]?.Split(";"))
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// ------------------------- DB + Identity -------------------------
builder.Services.AddDbContext<CMSBlogContext>(opt =>
    opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<CMSBlogContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequiredLength = 6;

    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    opt.Lockout.MaxFailedAccessAttempts = 5;

    opt.User.RequireUniqueEmail = true;
});

// ------------------------- Repository + Services -------------------------
builder.Services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Auto register all repository/service using Scrutor
builder.Services.Scan(scan => scan
    .FromAssemblyOf<PostRepository>()
    .AddClasses()
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);
//builder.Services.Scan(scan => scan
//    .FromAssemblyOf<MediaFileService>()
//    .AddClasses()
//    .AsImplementedInterfaces()
//    .WithScopedLifetime()
//);

//---------------------------Đăng ký interface cho StorageService-------------
builder.Services.AddScoped<IStorageService, LocalStorageService>();
builder.Services.AddScoped<IMediaFileService, MediaFileService>();
builder.Services.AddScoped<IStorageServicee, LocalStorageServicee>();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMediaFileService, MediaFileService>();




// ------------------------- Automapper -------------------------
builder.Services.AddAutoMapper(typeof(PostInListDto));

// ------------------------- JWT -------------------------
builder.Services.Configure<JwtTokenSettings>(configuration.GetSection("JwtTokenSettings"));
builder.Services.AddScoped<ITokenService, TokenService>();

// ------------------------- API + Swagger -------------------------
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<IFormFile>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });

    c.OperationFilter<FileUploadOperationFilter>();
});



// ------------------------- App Pipeline -------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CMSCorsPolicy);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Migrate DB + Seed
app.MigrateDatabase();

app.Run();
