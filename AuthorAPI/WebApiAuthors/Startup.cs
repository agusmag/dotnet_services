using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.Filters;
using WebApiAuthors.Middlewares;

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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            // Add AutoMapper Dependency Injection to work with EF and DTOs
            services.AddAutoMapper(typeof(Startup));
        }

        // Middlewares 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Example of Middleware using the last response before sending it back to the client from a class
            app.UseHttpResponseLogger();
            
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

