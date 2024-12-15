using System.Collections.Generic;
using Hackathon;
using NUnit.Framework;

namespace HackathonTest {
    [TestFixture]
    public class HRDirectorTests {
        private HRDirector _hrDirector;

        [SetUp]
        public void Setup() {
            _hrDirector = new HRDirector();
        }

        [Test]
        public void CalculateHarmonicMean_ForEqualNumbers_ShouldReturnEqualValue() {
            var teams = new List<Team> {
                new(new Employee(1, "TeamLead1"), new Employee(1, "Junior1")),
                new(new Employee(2, "TeamLead2"), new Employee(2, "Junior2")),
                new(new Employee(3, "TeamLead3"), new Employee(3, "Junior3"))
            };
            var juniorWishlists = new List<Wishlist> {
                new(1, new[] { 1, 2, 3 }),
                new(2, new[] { 2, 3, 1 }),
                new(3, new[] { 3, 2, 1 })
            };
            var teamLeadWishlists = new List<Wishlist> {
                new(1, new[] { 1, 2, 3 }),
                new(2, new[] { 2, 3, 1 }),
                new(3, new[] { 3, 2, 1 })
            };
            
            double harmonicMean = _hrDirector.CalculateHarmonicMean(teams, juniorWishlists, teamLeadWishlists);
            Assert.AreEqual(3.0, harmonicMean, 1e-9);
        }

        [Test]
        public void CalculateHarmonicMean_WithSpecificValues_ShouldReturnCorrectMean() {
            var teams = new List<Team> {
                new(new Employee(1, "TeamLead1"), new Employee(1, "Junior1")),
                new(new Employee(2, "TeamLead2"), new Employee(2, "Junior2"))
            };
            var juniorWishlists = new List<Wishlist> {
                new(1, new[] { 2, 1 }), // индекс 1
                new(2, new[] { 2, 1 }) // индекс 2
            };
            var teamLeadWishlists = new List<Wishlist> {
                new(1, new[] { 1, 2 }), // индекс 2
                new(2, new[] { 2, 1 }) // индекс 2
            };
            
            double harmonicMean = _hrDirector.CalculateHarmonicMean(teams, juniorWishlists, teamLeadWishlists);
            Assert.AreEqual(1.6, harmonicMean, 1e-9);
        }

        [Test]
        public void CalculateHarmonicMean_WithDefinedPreferences_ShouldReturnExpectedValue() {
            var teams = new List<Team> {
                new (new Employee(1, "TeamLead1"), new Employee(1, "Junior1")),
                new (new Employee(2, "TeamLead2"), new Employee(2, "Junior2")),
                new (new Employee(3, "TeamLead3"), new Employee(3, "Junior3"))
            };
            var juniorWishlists = new List<Wishlist> {
                new(1, new[] { 1, 3, 2 }), // индекс 3
                new(2, new[] { 2, 1, 3 }), // индекс 3
                new(3, new[] { 1, 2, 3 }) // индекс 1
            };
            var teamLeadWishlists = new List<Wishlist> {
                new(1, new[] { 2, 3, 1 }), // индекс 1
                new(2, new[] { 1, 3, 2 }), // индекс 1
                new(3, new[] { 1, 2, 3 }) // индекс 1
            };
            
            double harmonicMean = _hrDirector.CalculateHarmonicMean(teams, juniorWishlists, teamLeadWishlists);
            Assert.AreEqual(1.285, harmonicMean, 1e-3);
        }
    }
}