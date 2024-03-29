using Infrastructure;
using Infrastructure.Authorization;
using Infrastructure.Data;
using Infrastructure.Modules.CRM;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System.Linq;

namespace Web
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
            services.UseInfrastructure(Configuration);
            
            services.UseCRM();                       

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>,
                AdditionalUserClaimsPrincipalFactory>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
                options.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
                options.AddPolicy(Policies.IsReadOnly, Policies.IsReadOnlyPolicy());
            });

            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddTransient<IAuthorizationHandler, PermissionRequirementHandler>();

            //services.AddScoped<IUserSession, UserSession>();
            services.AddHttpContextAccessor();

            //services.Configure<RequestLoggingOptions>(o =>
            //{
            //    o.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            //    {
            //        diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress.MapToIPv4());
            //    };
            //});

            services.AddControllersWithViews(options =>
            {
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            });                 
            services.AddRazorPages();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseUserSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }
    }
}
