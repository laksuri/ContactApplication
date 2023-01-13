using ContactWebAPI.Context;
using ContactWebAPI.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ContactWebAPI
{
    public class Startup
    {
        private IWebHostEnvironment _webHostEnvironment { get; }
        private IConfiguration _configuration { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _webHostEnvironment = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<ContactContext>(item => item.UseSqlServer(_configuration.GetConnectionString("ContactString")));
            //services.AddCors(c =>
            //{
            //    c.AddPolicy("AllowOrigin", options =>
            //    {
            //        options.AllowAnyOrigin();
            //        options.AllowAnyHeader();
            //    });
            //}); // Enabled CORS to allow any Origin, TODO Need to revisit this later 
            services.AddScoped<IContactRepository, ContactRepository>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
