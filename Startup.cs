using System;
using ImageAlbumAPI.Data;
using ImageAlbumAPI.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Newtonsoft.Json.Serialization;
using ImageAlbumAPI.Services;
using ImageAlbumAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace ImageAlbumAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<IPhotoRepo, PhotoRepo>();
            services.AddTransient<IAlbumRepo, AlbumRepo>();

            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IAlbumService, AlbumService>();
            services.AddTransient<IUserService, UserService>();

            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(
                Configuration.GetConnectionString("DevConnectionString")
            ));

            services.AddIdentity<User, IdentityRole>(opts => {
                opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                opts.User.RequireUniqueEmail = false;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireDigit = true;
            }
            )
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            




            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ImageAlbumAPI", Version = "v1" });
            });

            services.AddControllers().AddNewtonsoftJson(s => {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                s.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddCors();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ImageAlbumAPI v1"));
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
