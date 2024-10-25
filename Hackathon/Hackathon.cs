using System.Collections.Generic;

namespace Hackathon {
    public class Hackathon {
        private static int seed = 1;
        
        private List<Employee> juniors;
        
        private List<Employee> teamLeads;
        
        private HRManager hrManager;
        
        private HRDirector hrDirector;

        public Hackathon(List<Employee> juniors, List<Employee> teamLeads, HRManager hrManager, HRDirector hrDirector) {
            this.juniors = juniors;
            this.teamLeads = teamLeads;
            this.hrManager = hrManager;
            this.hrDirector = hrDirector;
        }

        public double Start() {
            var wishlistGenerator = new WishlistGenerator(seed++);
            
            var juniorWishlists = wishlistGenerator.GenerateWishlists(juniors, teamLeads);
            var teamLeadWishlists = wishlistGenerator.GenerateWishlists(teamLeads, juniors);
            
            var dreamTeams = hrManager.FormOptimalTeams(juniors, teamLeads, juniorWishlists, teamLeadWishlists);
            double averageHarmonicIndex = hrDirector.CalculateAverageHarmonicIndex(dreamTeams, juniorWishlists, teamLeadWishlists);
            return averageHarmonicIndex;
        }
    }
}