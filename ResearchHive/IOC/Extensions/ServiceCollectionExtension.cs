
using Application.Abstractions.Data.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Auth;
using Persistence.Context;
using Persistence.Repositories;
using ResearchHive.Authentication;
using ResearchHive.Implementations.Repositories;
using ResearchHive.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using ResearchHive.Abstractions;
using Application.Abstractions;

namespace ResearchHive.IOC.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
        {
           
            services
                .AddDatabase(configuration.GetConnectionString("DefaultConnection"))
                .AddServices(configuration)
                .AddRepositories();
            services.ConfigureJWT(configuration);
            /* .AddCurrentUser();*/

            return services;
        }

        #region
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("Jwt");
            var secretKey = jwtConfig["Key"];
            services
                .AddScoped<IFileService, FileService>()
               .AddSingleton<IJWTTokenHandler>(new JWTTokenHandler(secretKey))
            .AddScoped<IAuthenticationService, AuthService>();
            /*.AddScoped<ITokenService, TokenService>*/
            return services;
        }
        public static IServiceCollection AddDatabase(this IServiceCollection service, string connectionString)
        {
            service.AddDbContext<ApplicationDbContext>(options =>
                 options.UseNpgsql(connectionString));
            return service;
        }
        #endregion
        #region
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IUserAuthenticationRepository, UserAuthenticationRepository>()
                .AddScoped<IAdministratorRepository, AdministratorRepository>()
                .AddScoped<IDepartmentRepository, DepartmentRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IStudentRepository, StudentRepository>()
                .AddScoped<ILecturerRepository, LecturerRepository>()
                .AddScoped<ILecturerDepartmentRepository, LecturerDepartmentRepository>()
                .AddScoped<IReviewRepository, ReviewRepository>()
                .AddScoped<IResearchRepository, ResearchRepository>()
                .AddScoped<IProjectRepository, ProjectRepository>()
                .AddScoped<IProjectDocumentRepository, ProjectDocumentRepository>()
                .AddScoped<IProjectSubmissionWindowRepository, ProjectSubmissionWindowRepository>() 
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IStudentSupervisorRepository, StudentSupervisorRepository>();

        }
        #endregion

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("Jwt");
            var secretKey = jwtConfig["Key"];
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["Issuer"],
                    ValidAudience = null,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }
       

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Research Hive",
                    Version = "v1",
                    Description = "FWCM Research Portal",
                    Contact = new OpenApiContact
                    {
                        Name = "Mubashir Badr."
                    },
                    
                });
               /*  c.SchemaGeneratorOptions = new SchemaGeneratorOptions { SchemaIdSelector = type => type.FullName };*/
                c.CustomSchemaIds(s => s.FullName.Replace("+", "."));
                /*c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());*/
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] {}
                }
            });
            });
        }

    }
}
