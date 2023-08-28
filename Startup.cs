using Microsoft.OpenApi.Models;
using WebApplication6.Data;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Options;
using WebApplication6.Repository;
using WebApplication6.Repositoryy;

namespace WebApplication6
{
    public class Startup


    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public class ConnectionStringsConfig
        {
            public string DefaultConnection { get; set; }

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)

        {
            services.AddDbContext<EmployeeDbcontext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<EmployeeDbcontext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection2")));
            services.AddControllers();
            services.AddLogging(builder =>
            {
                builder.AddConsole(); // Use the console logger provider
            });

            services.AddSingleton<DapperDbContext>();
            services.AddScoped<IDepartmentRepo,DepartmentRepo>();

            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Resolve Employee", Version = "v1" });

                });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        

            app.UseCors("AllowAll");
        }

    }
}