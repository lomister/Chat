namespace TeamABootcampAplication
{
    #pragma warning disable SA1600 // Elements should be documented
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    public sealed class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
    #pragma warning restore SA1600 // Elements should be documented
}