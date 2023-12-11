using Microsoft.AspNetCore.ResponseCompression;
using Musikfestival.Repositories;
using Musikfestival.Shared.Models;
using Blazored.LocalStorage;
using Blazored.Modal;

namespace Musikfestival
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            //Dependency Injections

            builder.Services.AddSingleton<IBruger, BrugerinfoRepository>();
            builder.Services.AddSingleton<IVagter, VagtplanRepository>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredModal();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();


            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}