using System.Collections.Generic;

namespace Hackathon {
    public class Hackathon {
        private static int seed = 1;
        
        private readonly List<Employee> _juniors;
        
        private readonly List<Employee> _teamLeads;
        
        private readonly HRManager _hrManager;
        
        private readonly HRDirector _hrDirector;

        public Hackathon(List<Employee> juniors, List<Employee> teamLeads, HRManager hrManager, HRDirector hrDirector) {
            _juniors = juniors;
            _teamLeads = teamLeads;
            _hrManager = hrManager;
            _hrDirector = hrDirector;
        }

        public double Start() {
            var wishlistGenerator = new WishlistGenerator(seed++);
            
            var juniorWishlists = wishlistGenerator.GenerateWishlists(_juniors, _teamLeads);
            var teamLeadWishlists = wishlistGenerator.GenerateWishlists(_teamLeads, _juniors);
            
            var dreamTeams = _hrManager.BuildOptimalTeams(_juniors, _teamLeads, juniorWishlists, teamLeadWishlists);
            double averageHarmonicIndex = _hrDirector.CalculateHarmonicMean(dreamTeams, juniorWishlists, teamLeadWishlists);
            return averageHarmonicIndex;
        }
    }
}