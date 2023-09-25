using Historia_Clinica.Data;
using Historia_Clinica.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Historia_Clinica
{
    public static class Startup
    {
        public static WebApplication InicializarApp(string[] args)
        {
            //Crear una nueva instancia de nuestro servidor web.
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder); //Lo configuramos, con sus respectivos servicios.

            var app = builder.Build(); //Sobre esta app, configuraremos luego los middleware.
            Configure(app); //Configuramos los middleware.


            return app; //Retornamos la App ya inicializada.
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {

            builder.Services.AddDbContext<HistoriaClinicaContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("HistoriaClinicaDBCS")));

            #region Identity
            builder.Services.AddIdentity<Persona, Rol>().AddEntityFrameworkStores<HistoriaClinicaContext>(); //Invoco a Identity y me da una persona, la cual es un IdentityUser. Se define donde queremos guardarlo. 

            builder.Services.Configure<IdentityOptions>(opciones =>
            {   //Configuracion por defecto. Pass. Pre-Carga= Password1!
                opciones.Password.RequireNonAlphanumeric = true;
                opciones.Password.RequireLowercase = true;
                opciones.Password.RequireUppercase = true;
                opciones.Password.RequireDigit = true;
                opciones.Password.RequiredLength = 6;
            });

            //cookies path

            builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, 
            opciones =>
            {
                opciones.LoginPath = "/Account/Iniciarsesion";
                opciones.AccessDeniedPath = "/Account/AccesoDenegado";
                opciones.Cookie.Name = "IdentidadHistoriaClinicaApp";
            }
            );
            #endregion

            // Add services to the container.
            builder.Services.AddControllersWithViews();
        }

        private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // forzar Update-Database
            using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<HistoriaClinicaContext>();

                context.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); //Siempre antes de la Autorizacion. 
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
