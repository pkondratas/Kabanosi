using System.Net;
using System.Security.Claims;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Kabanosi.Authorization;
using Kabanosi.Constants;
using Kabanosi.Extensions;
using Kabanosi.Entities;
using Kabanosi.Exceptions;
using Kabanosi.Hubs;
using Kabanosi.Interceptors;
using Kabanosi.Persistence;
using Kabanosi.Profiles;
using Kabanosi.Repositories;
using Kabanosi.Repositories.UnitOfWork;
using Kabanosi.Services;
using Kabanosi.Services.Interfaces;
using Kabanosi.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(p =>
    {
        p.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("AzureMySql");
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<DatabaseContext>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);

// App services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IInvitationService, InvitationService>();
builder.Services.AddScoped<IAssignmentStatusService, AssignmentStatusService>();
builder.Services.AddScoped<IAssignmentLabelService, AssignmentLabelService>();
builder.Services.AddScoped<IProjectMemberService, ProjectMemberService>();

builder.Services.AddSignalR();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Custom policy-based authorization service (project scoped auth)
builder.Services.AddSingleton<IAuthorizationHandler, ProjectRoleHandler>();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ProjectMemberAndAdmin", policy =>
        policy.Requirements.Add(new ProjectRoleRequirement([nameof(ProjectRole.ProjectAdmin)])))
    .AddPolicy("ProjectMemberAndAdminOrMember", policy =>
        policy.Requirements.Add(new ProjectRoleRequirement([
            nameof(ProjectRole.ProjectAdmin),
            nameof(ProjectRole.ProjectMember)
        ])));

// Repositories
builder.Services.AddScoped<ProjectRepository>();
builder.Services.AddScoped<ProjectMemberRepository>();
builder.Services.AddScoped<AssignmentRepository>();
builder.Services.AddScoped<InvitationRepository>();
builder.Services.AddScoped<AssignmentStatusRepository>();
builder.Services.AddScoped<AssignmentLabelRepository>();

var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);
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
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme.\r\n\r\n" +
                      "Enter {yourToken}\r\n\r\n" +
                      "Example: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    
    options.EnableAnnotations();
});

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterGeneric(typeof(InterceptorAdapter<>));
    containerBuilder.RegisterType<LoggingInterceptor>().InstancePerLifetimeScope();

    containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
        .AsImplementedInterfaces()
        .InstancePerDependency()
        .EnableInterfaceInterceptors()
        .InterceptedBy(typeof(InterceptorAdapter<LoggingInterceptor>));
});

var app = builder.Build();

app.UseExceptionHandling(
    new Dictionary<Type, HttpStatusCode>
    {
        { typeof(ConflictException), HttpStatusCode.Conflict },
        { typeof(NotFoundException), HttpStatusCode.NotFound },
    });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/hubs/notify");

app.Run();