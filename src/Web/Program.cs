using Infrastructure.Data;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using System;
using System.Collections.Generic;
using System.IO;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connStr = appSettings.GetConnectionString("DefaultConnection");
            string tableName = "Logs";


            var columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {"raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
                {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                {"properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
                {"props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
                {"machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") },
                {"user_name", new SinglePropertyColumnWriter("UserName", PropertyWriteMethod.ToString, NpgsqlDbType.Text) }
            };

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                //.WriteTo.File(
                //    Path.Combine(Directory.GetCurrentDirectory(), @"Logs\log.txt"),
                //    fileSizeLimitBytes: 1_000_000,
                //    rollOnFileSizeLimit: true,
                //    shared: true,
                //    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .WriteTo.PostgreSql(
                    connStr, 
                    tableName, 
                    columnWriters, 
                    //needAutoCreateTable: true,
                    useCopy: true
                    )
                .Enrich.WithMachineName()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var dbContext = services.GetRequiredService<AppDbContext>();
                        dbContext.Database.Migrate();

                        var logFactory = services.GetRequiredService<ILoggerFactory>();
                        var userManager = services.GetRequiredService<UserManager<AppUser>>();
                        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                        AsyncHelper.RunSync(() => AppDbContextSeed.SeedAsync(dbContext, userManager, roleManager, logFactory));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "An error occurred");
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
