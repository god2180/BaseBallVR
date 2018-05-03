
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using BaseBallVR.DB;

using BaseBallVR.Servicese;
using BaseBallVR.Redis;


namespace BaseBallVR
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            
        }
            

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();

            services.AddDbContext<Context>(options =>
                options.UseSqlServer(Configuration.GetSection("Sql-BaseBallVR:Server").Value, b => b.MigrationsAssembly("BaseBallVR")));

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IMemberRepository, MemberRepository>();

            services.AddSingleton<ICacheRedis, CacheRedis>(provider =>
            {
                return new CacheRedis(Configuration.GetSection("Redis-BaseBallVR:Server").Value);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseCors(builder => builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod())

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            AutoMapperConfiguration.Initialize();

        }

        
    }
}
