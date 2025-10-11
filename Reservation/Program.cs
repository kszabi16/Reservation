using Reservation.DataContext.Context;
using Microsoft.EntityFrameworkCore;
using Reservation.Service.Services;
using Reservation.Service.Interfaces;
using Reservation.Service.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Reservation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

          
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

       
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Reservation API",
                    Version = "v1",
                    Description = "JWT Authentication Enabled API"
                });

                
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Írd be ide a tokened: **Bearer {token}**"
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

            
            builder.Services.AddDbContext<ReservationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("ReservationContext"),
                    b => b.MigrationsAssembly("Reservation")
                ));

           
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IPropertyService, PropertyService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<IFavoriteService, FavoriteService>();
            builder.Services.AddScoped<ILikeService, LikeService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            builder.Services.AddAuthorization();
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
