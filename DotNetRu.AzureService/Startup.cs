using DotNetRu.Azure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNetRu.AzureService
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
            var realmSettings = new RealmSettings();
            Configuration.Bind("RealmOptions", realmSettings);

            var tweetSettings = new TweetSettings();
            Configuration.Bind("TweetOptions", tweetSettings);

            var vkontakteSettings = new VkontakteSettings();
            Configuration.Bind("VkontakteOptions", vkontakteSettings);

            var pushSettings = new PushSettings();
            Configuration.Bind("PushOptions", pushSettings);

            services.AddSingleton(realmSettings);
            services.AddSingleton(tweetSettings);
            services.AddSingleton(vkontakteSettings);
            services.AddSingleton(pushSettings);

            services.AddScoped<PushNotificationsManager>();

            services
                .AddControllers()
                .AddNewtonsoftJson();

            services.AddOpenApiDocument(document => document.DocumentName = "DotNetRu App API");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logFactory)
        {
            ApplicationLogging.LoggerFactory = logFactory;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
