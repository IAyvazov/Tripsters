using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Tripsters.Web.Areas.Identity.IdentityHostingStartup))]

namespace Tripsters.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
