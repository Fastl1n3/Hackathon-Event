using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hackathon.db.context;
using Hackathon.db.entity;
using Hackathon.hrmanager;
using Microsoft.Extensions.Hosting;

namespace Hackathon {
    public class HackatonSimulator : BackgroundService {
        private readonly ProgramContext _programContext;
        private readonly Hackathon _hackathon;
        private readonly HackathonDbContext _dbContext;
        
        private readonly WishlistGenerator _wishlistGenerator = new WishlistGenerator(123);
        public HackatonSimulator(ProgramContext programContext, Hackathon hackathon, HackathonDbContext hackathonDbContext) {
            _programContext = programContext;
            _hackathon = hackathon;
            _dbContext = hackathonDbContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) {
            var juniors = CsvEmployeeReader.ReadCsv("resources/Juniors20.csv");
            var teamLeads = CsvEmployeeReader.ReadCsv("resources/Teamleads20.csv");
            
           // double totalHarmonicLevel = 0.0;
            
            for (int i = 0; i < _programContext.NumHackathons; i++) {
                var juniorWishlists = _wishlistGenerator.GenerateWishlists(juniors, teamLeads);
                var teamLeadWishlists = _wishlistGenerator.GenerateWishlists(teamLeads, juniors);
                double harmonicLevel = _hackathon.Start(juniors, teamLeads,
                    juniorWishlists, teamLeadWishlists);
              //  totalHarmonicLevel += harmonicLevel;
                Console.WriteLine($"Hackathon {i + 1}, harmonic level: {harmonicLevel:F2}");
            }
            
            var totalHarmonicLevel = _dbContext.Hackathons.Sum(h => h.HarmonicMean);
            var hackathonCount = _dbContext.Hackathons.Count();
            double averageHarmonic = totalHarmonicLevel / hackathonCount;
            Console.WriteLine($"Average harmonic: {averageHarmonic:F2}, hackathonCount: {hackathonCount}");
            
            return Task.CompletedTask;
        }
    }
}