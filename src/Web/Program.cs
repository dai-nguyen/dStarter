using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Modules.CRM;
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
using Serilog.Sinks.PostgreSQL.ColumnWriters;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Information("Starting web host");
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var configuration = services.GetRequiredService<IConfiguration>();
                        string connStr = configuration.GetSection("DefaultConnection").Value;
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
                            .WriteTo.PostgreSQL(
                                connStr,
                                tableName,
                                columnWriters,
                                needAutoCreateTable: true
                                //useCopy: true
                                )
                            .Enrich.WithMachineName()
                            .CreateLogger();


                        var dbContext = services.GetRequiredService<AppDbContext>();
                        dbContext.Database.Migrate();
                        // https://www.npgsql.org/efcore/mapping/enum.html?tabs=tabid-1
                        //using (var conn = (Npgsql.NpgsqlConnection)dbContext.Database.GetDbConnection())
                        //{
                        //    conn.Open();
                        //    conn.ReloadTypes();
                        //}

                        var logFactory = services.GetRequiredService<ILoggerFactory>();
                        var userManager = services.GetRequiredService<UserManager<AppUser>>();
                        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                        AsyncHelper.RunSync(() => AppDbContextSeed.SeedAsync(dbContext, userManager, roleManager, logFactory));
                        AsyncHelper.RunSync(() => CrmSeed.SeedAsync(dbContext, logFactory));
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
                .ConfigureServices((context, services) =>
                {
                    HostConfig.CertPath = context.Configuration["CertPath"];
                    HostConfig.CertPassword = context.Configuration["CertPassword"];
                    HostConfig.CertData = context.Configuration["CertData"];
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //var host = Dns.GetHostEntry("");

                    webBuilder.ConfigureKestrel(o =>
                    {
                        //o.Listen(host.AddressList.First(), 5000);

                        o.ListenAnyIP(5000);
                        o.ListenAnyIP(5001, listOpt =>
                        {
                            if (!string.IsNullOrEmpty(HostConfig.CertData))
                            {
                                try
                                {
                                    var bytes = Convert.FromBase64String(HostConfig.CertData); //Encoding.ASCII.GetBytes(HostConfig.CertData);
                                    var cert = new X509Certificate2(bytes, HostConfig.CertPassword);
                                    listOpt.UseHttps(cert);
                                }
                                catch (Exception)
                                { }
                            }
                            else if (!string.IsNullOrEmpty(HostConfig.CertPath) 
                                && !string.IsNullOrEmpty(HostConfig.CertPassword))
                            {
                                try
                                {
                                    listOpt.UseHttps(HostConfig.CertPath, HostConfig.CertPassword);
                                }
                                catch (Exception)
                                { }
                            }
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });

        public static class HostConfig
        {
            public static string CertPath { get; set; }
            public static string CertPassword { get; set; }
            public static string CertData { get; set; }
        }
    }
}
