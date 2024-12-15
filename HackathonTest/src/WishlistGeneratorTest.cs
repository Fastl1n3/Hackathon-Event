using System.Collections.Generic;
using Hackathon;
using NUnit.Framework;

namespace HackathonTest {
    [TestFixture]
    public class WishlistGeneratorTest {
        [Test]
        public void GenerateWishlists_SizeShouldMatch_TeamLeadsCount() {
            var juniors = new List<Employee> {
                new(1, "Junior1"),
                new(2, "Junior2"),
                new(3, "Junior3")
            };
            var teamLeads = new List<Employee> {
                new(1, "TeamLead1"),
                new(2, "TeamLead2"),
                new(3, "TeamLead3")
            };
            var wishlistGenerator = new WishlistGenerator(12345);
            var wishlists = wishlistGenerator.GenerateWishlists(juniors, teamLeads);
            
            Assert.AreEqual(teamLeads.Count, wishlists.Count);
        }

        [Test]
        public void GenerateWishlists_ShouldContainSpecifiedJunior() {
            var juniors = new List<Employee> {
                new(1, "Junior1"),
                new(2, "Junior2"),
                new(3, "Junior3"),
            };
            var teamLeads = new List<Employee> {
                new(27, "TeamLead1"),
                new(33, "TeamLead2"),
                new(51, "TeamLead3")
            };
            var wishlistGenerator = new WishlistGenerator(12345);
            var wishlists = wishlistGenerator.GenerateWishlists(juniors, teamLeads);
            
            foreach (var wishlist in wishlists) {
                Assert.Contains(27, wishlist.DesiredEmployees);
                Assert.Contains(33, wishlist.DesiredEmployees);
                Assert.Contains(51, wishlist.DesiredEmployees);
            }
        }
    }
}
