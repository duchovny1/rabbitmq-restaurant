using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prices.API.Consumers;
using Prices.Business;
using Prices.Data;
using Prices.Data.Contracts;
using RabbitMQ.Client;

namespace Prices.API
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
            services.AddControllers();

            services.AddDbContext<PricesContext>(opt => opt.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<IPricesService, PriceService>();
            services.AddTransient<IChangesHistoryService, ChangesHistoryService>();

            services.AddSingleton<IPersistentConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    DispatchConsumersAsync = true
                };

                return new DefaultPersistentConnection(factory);
            });

            services.AddSingleton<IEventBus, EventBus.EventBus>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IPersistentConnection>();
                rabbitMQPersistentConnection.TryConnect();

                return new EventBus.EventBus(rabbitMQPersistentConnection);
            });

            services.AddHostedService<AddPriceHistoryConsumer>();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<PricesContext>();

                // this line will made database if its not created
                new DbInitializer(dbContext).InitializeAsync().GetAwaiter().GetResult();
            }
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
