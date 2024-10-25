using System;
using System.Collections.Generic;

namespace Hackathon {
    public class HackatonSimulator {
        private readonly int _numHackathons;

        private readonly HRManager _hrManager;

        private readonly HRDirector _hrDirector;
        
        public HackatonSimulator(int numHackathons) {
            _numHackathons = numHackathons;
            _hrManager = new HRManager(new GaleShapleyTeamBuildingStrategy());
            _hrDirector = new HRDirector();
        }
        
        public void RunMultipleHackathons(List<Employee> juniors, List<Employee> teamLeads) {
            double totalHarmonicLevel = 0.0;
            
            for (int i = 0; i < _numHackathons; i++) {
                var hackathon = new Hackathon(juniors, teamLeads, _hrManager, _hrDirector);
                double harmonicLevel = hackathon.Start();
                totalHarmonicLevel += harmonicLevel;
                Console.WriteLine($"Hackathon {i + 1}, harmonic level: {harmonicLevel:F2}");
            }

            double averageHarmonic = totalHarmonicLevel / _numHackathons;
            Console.WriteLine($"Average harmonic: {averageHarmonic:F2}");
        }
    }
}