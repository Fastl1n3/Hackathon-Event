using System.Collections.Generic;
using Hackathon;
using Hackathon.hrmanager;
using NUnit.Framework;


namespace HackathonTest;

[TestFixture]
public class HackathonTest {
    private Hackathon.Hackathon _hackathon;

    [SetUp]
    public void Setup() {
        AbstractHRManager hrManager = new DummyHRManager(new DummyTeamBuildingStrategy());
        HRDirector hrDirector = new HRDirector();
        var dbContext = DbTest.CreateInMemoryDbContext();
        _hackathon = new Hackathon.Hackathon(hrManager, hrDirector, dbContext);
    }

    [Test]
    public void CalculateHarmonicMean() {
        var juniors = new List<Employee> {
            new(11, "Junior1"),
            new(22, "Junior2"),
            new(33, "Junior3")
        };

        var teamLeads = new List<Employee> {
            new(1, "TeamLead1"),
            new(2, "TeamLead2"),
            new(3, "TeamLead3")
        };

        var juniorWishlists = new List<Wishlist> {
            new(11, new[] { 1, 3, 2 }), 
            new(22, new[] { 2, 3, 1 }),
            new(33, new[] { 3, 1, 2 }) 
        };

        var teamLeadWishlists = new List<Wishlist> {
            new(1, new[] { 11, 22, 33 }),
            new(2, new[] { 22, 33, 11 }), 
            new(3, new[] { 22, 11, 33 })
        };
            
        double result = _hackathon.Start(juniors, teamLeads, juniorWishlists, teamLeadWishlists);
        Assert.AreEqual(2.25, result, 1e-9);
    }
}