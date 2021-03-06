using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyProject.Host.Combi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Backend.Controller.Startup.ConfigureServices(services);
            WasmHosting.Startup.ConfigureServices(services);
            ServerSide.Startup.ConfigureServices(services);
            Backend.Startup.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = { "default.html" } });
            app.UseStaticFiles();

            app.Map("/serverside", ssb => ServerSide.Startup.Configure(ssb, env));
            app.Map("/wasm", wasm => WasmHosting.Startup.Configure(wasm, env));
            app.Map("/api", api => Backend.Controller.Startup.Configure(api, env));
        }
    }
}
