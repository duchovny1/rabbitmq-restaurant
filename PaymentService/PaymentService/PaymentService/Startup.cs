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
using PaymentService.API.Consumers;
using PaymentService.Business;
using PaymentService.Business.Contracts;
using PaymentService.Data;
using RabbitMQ.Client;

namespace PaymentService
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

            services.AddDbContext<PaymentDbContext>(opt => opt.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddHttpClient<IPaymentService, PaymentService.Business.PaymentService>();
            services.AddTransient<IUsersService, UsersService>();


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

            services.AddHostedService<LogInfoConsumer>();
            services.AddHostedService<OrderValidatorConsumer>();
            services.AddHostedService<GetTotalAmountConsumer>();
            services.AddHostedService<ProcessPaymentConsumer>();

            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<PaymentDbContext>();

                // this line will made database if its not created
                if(dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
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
