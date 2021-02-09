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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
// using JWTAuthentication.Authentication;

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

            services.AddControllers();

            services.AddCors(opts => 
            {
                opts.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().Build());
            });

            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer(opts => 
            //     {
            //         opts.TokenValidationParameters = new TokenValidationParameters
            //         {
            //             ValidateIssuer = true,
            //             ValidateAudience = true,
            //             ValidateLifetime = true,
            //             ValidateIssuerSigningKey = true,
            //             ValidIssuer = Configuration["Jwt:Issuer"],
            //             ValidAudience = Configuration["Jwt:Issuer"],
            //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]))
            //         };
            //         opts.Audience = "http://localhost:5000";
            //         opts.Authority = "http://localhost:5000/identity";
            //     });

            services.AddIdentity<User, IdentityRole>(opts => {
                opts.User.RequireUniqueEmail = false;
                opts.Password.RequiredLength = 5;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;})
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>  
            {  
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;  
            })  
            .AddJwtBearer(options =>  
            {  
                options.SaveToken = true;  
                options.RequireHttpsMetadata = false;  
                options.TokenValidationParameters = new TokenValidationParameters()  
                {  
                    ValidateIssuer = true,  
                    ValidateAudience = true,  
                    ValidAudience = Configuration["JWT:ValidAudience"],  
                    ValidIssuer = Configuration["JWT:ValidIssuer"],  
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"]))  
                };  
            }); 

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<IPhotoRepo, PhotoRepo>();
            services.AddTransient<IAlbumRepo, AlbumRepo>();
            // services.AddTransient<ICommentRepo, CommentRepo>();

            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IAlbumService, AlbumService>();
            services.AddTransient<IUserService, UserService>();

            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(
                Configuration.GetConnectionString("DevConnectionString")
            ));
            // services.AddDbContext<AppIdentityDbContext>(opt => opt.UseSqlServer(
            //     Configuration.GetConnectionString("DevIdentityConnectionString")
            // ));

            // services.AddIdentity<User, IdentityRole>(opts => {
            //     opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //     opts.User.RequireUniqueEmail = false;
            //     opts.Password.RequiredLength = 5;
            //     opts.Password.RequireNonAlphanumeric = false;
            //     opts.Password.RequireLowercase = true;
            //     opts.Password.RequireUppercase = false;
            //     opts.Password.RequireDigit = false;})
            //         .AddEntityFrameworkStores<AppDbContext>()
                    
            //         .AddDefaultTokenProviders();

           
            
            //services.ConfigureApplicationCookie(opts => opts.LoginPath = "/account/login");

            services.AddMvc();



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ImageAlbumAPI", Version = "v1" });
            });

            services.AddControllers().AddNewtonsoftJson(s => {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                s.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });



        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseSwagger();
                // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ImageAlbumAPI v1"));
            }
            

            // app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
