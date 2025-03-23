using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UnoLib1;

namespace UnoMigrations;

class Program
{
    static void Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        
        builder.ConfigureServices((context, services) =>
        {
            services.AddDbContext<UnoLibDbContext>(options =>
                options.UseSqlite("Data Source=mydatabase.db"));
        });
        
        builder.RunConsoleAsync();
    }
}
