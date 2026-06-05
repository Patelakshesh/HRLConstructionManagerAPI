using System.Text;
using HRLConstructionManagerAPI.GraphQL;
using HRLConstructionManagerAPI.Repositories;
using HRLConstructionManagerAPI.Services;
using HRLConstructionManagerAPI.Data;
using HRLConstructionManagerAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(System.IO.Path.Combine(builder.Environment.ContentRootPath, "DataProtectionKeys")));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
    ?? throw new InvalidOperationException("JwtSettings configuration is missing.");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services
    .AddAuthentication(options =>
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
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
            ),

            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("JwtBearer");
                logger.LogWarning(context.Exception, "JWT authentication failed.");
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()));

builder.Services.AddScoped<IRoleRepository, EfRoleRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<ISiteRepository, EfSiteRepository>();
builder.Services.AddScoped<ICategoryRepository, EfCategoryRepository>();
builder.Services.AddScoped<IContractorRepository, EfContractorRepository>();
builder.Services.AddScoped<ISupervisorCreditRepository, EfSupervisorCreditRepository>();
builder.Services.AddScoped<IExpenseRepository, EfExpenseRepository>();
builder.Services.AddScoped<IAttendanceRepository, EfAttendanceRepository>();

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IContractorService, ContractorService>();
builder.Services.AddScoped<ISupervisorCreditService, SupervisorCreditService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services
    .AddGraphQLServer("public")
    .AddQueryType<PublicQuery>()
    .AddMutationType<AuthMutation>();

builder.Services
    .AddGraphQLServer("private")
    .AddAuthorization()

    .AddQueryType(d => d.Name("Query"))
    .AddMutationType(d => d.Name("Mutation"))

    .AddTypeExtension<UserManagementQuery>()
    .AddTypeExtension<UserManagementMutation>()
    .AddTypeExtension<SiteManagementQuery>()
    .AddTypeExtension<SiteManagementMutation>()
    .AddTypeExtension<CategoryManagementQuery>()
    .AddTypeExtension<CategoryManagementMutation>()
    .AddTypeExtension<ContractorManagementQuery>()
    .AddTypeExtension<ContractorManagementMutation>()
    .AddTypeExtension<SupervisorCreditManagementQuery>()
    .AddTypeExtension<SupervisorCreditManagementMutation>()
    .AddTypeExtension<ExpenseManagementQuery>()
    .AddTypeExtension<ExpenseManagementMutation>()
    .AddTypeExtension<AttendanceManagementQuery>()
    .AddTypeExtension<AttendanceManagementMutation>()
    .AddTypeExtension<DashboardManagementQuery>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();
app.MapGraphQL("/graphql/public", "public");

app.MapGraphQL("/graphql", "private")
   .RequireAuthorization();

app.Run();
