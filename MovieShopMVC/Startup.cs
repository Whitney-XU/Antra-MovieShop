﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.RepositoryInterface;
using ApplicationCore.ServiceInterface;
using Infrastructure.data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MovieShopMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IGenreRepository,GenreRepository>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<ICastRepository, CastRepository>();
            services.AddScoped<ICastService, CastService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddHttpContextAccessor();

            //3rd party IoC Autofac, Ninject
            //ASP.NET Core has built-in support for DI and it has built-in container
            //ASP.NET Framework there was no DI, Autofact, Ninject
            //Reflection ?
            // get me total number of methods and properties in a class
            // System.Relection
            services.AddDbContext<MovieShopDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MovieShopDbConnection"));
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options=> {
                    options.Cookie.Name = "MovieShopAuth";
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                    options.LoginPath = "/Account/Login";

                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
