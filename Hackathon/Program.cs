
namespace Hackathon {
    class Program {
        public static void Main(string[] args) {
            var juniors = CsvEmployeeReader.ReadCsv("resources/Juniors20.csv");
            var teamleads = CsvEmployeeReader.ReadCsv("resources/Teamleads20.csv");

            var hackatonSimulator = new HackatonSimulator(1000);
            hackatonSimulator.RunMultipleHackathons(juniors, teamleads);
        }
    }
}