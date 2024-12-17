using System;
using System.Threading.Tasks;
using Hackathon.db.context;
using Hackathon.hrmanager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hackathon;

public class Program {
    public static void Main(string[] args) {
        int numHackathons = int.Parse(args[0]);
        var host = Host.CreateDefaultBuilder(args).ConfigureLogging(logging => {
                logging.ClearProviders();
                logging.AddConsole(); // Используем только консольный логгер
            })
            .ConfigureServices((hostContext, services) => {
                services.AddSingleton(_ => new ProgramContext(numHackathons));
                services.AddTransient<ITeamBuildingStrategy, DummyTeamBuildingStrategy>();
                services.AddTransient<AbstractHRManager, HRManager>();
                services.AddTransient<HRDirector>();
                services.AddTransient<Hackathon>();
                services.AddDbContext<HackathonDbContext>(options =>
                    options.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException())
                        .UseSnakeCaseNamingConvention()
                        .LogTo(Console.WriteLine, LogLevel.Information));
                services.AddHostedService<HackatonSimulator>();
            })
            .Build();

        host.Run();

    }
        
    public static async Task PrintHackathonDetails(int hackathonId, HackathonDbContext dbContext)
    {
        var hackathon = await dbContext.Hackathons
            .Include(h => h.Teams)
            .ThenInclude(t => t.Teamlead)
            .Include(h => h.Teams)
            .ThenInclude(t => t.Junior)
            .FirstOrDefaultAsync(h => h.Id == hackathonId);

        if (hackathon == null)
        {
            Console.WriteLine("Hackathon not found!");
            return;
        }

        Console.WriteLine($"Hackathon ID: {hackathon.Id}, Harmonic Mean: {hackathon.HarmonicMean:F2}");
        Console.WriteLine("Teams:");
        foreach (var team in hackathon.Teams)
        {
            Console.WriteLine($"TeamLead: {team.Teamlead.Name}, Junior: {team.Junior.Name}");
        }
    }
}