using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.Filters;
using WebApiAuthors.Middlewares;
using WebApiAuthors.Services;

namespace WebApiAuthors
{
    public class Startup
	{
		public Startup(IConfiguration configuration)
		{
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Services, Dependency Injection
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container ignoring EF Cycles
            services.AddControllers(options =>
            {
                // Add a Global Filter which impacts all the controllers and methods
                options.Filters.Add(typeof(ExceptionFilter));
            }).AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Configure EF Db Context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("defaultConnection")));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddTransient<IService, ServiceA>();

            // Always different, no matter the HTTP context
            services.AddTransient<ServiceTransient>();

            // Same value in the same HTTP context
            services.AddScoped<ServiceScoped>();

            // Same value always, no matter the HTTP context (single instance)
            services.AddSingleton<ServiceSingleton>();

            // Custom Filter
            services.AddTransient<MyActionFilter>();

            // Add IHostedService Service
            services.AddHostedService<WriteOnFile>();

            services.AddResponseCaching();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        }

        // Middlewares 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Example of Middleware using the last response before sending it back to the client from a class
            // app.UseHttpResponseLogger();

            // Example of Middleware which implements routing

                //app.Map("/middleware1", app =>
                //{
                //    app.Run(async context =>
                //    {
                //        await context.Response.WriteAsync("Hello, you've chosen an specific route");
                //    });
                //});

            // Example of Middleware which ends the middleware's pipeline

                //app.Run(async context =>
                //{
                //    await context.Response.WriteAsync("Hello, you've been intercepted!");
                //});
            
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

