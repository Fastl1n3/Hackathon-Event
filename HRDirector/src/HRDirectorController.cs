using Microsoft.AspNetCore.Mvc;

namespace HRDirector;

[Route("hrdirector/api")]
[ApiController]
public class HRDirectorController : ControllerBase {
    private readonly HRDirectorService _hrDirectorService;

    public HRDirectorController(HRDirectorService hrDirectorService) {
        _hrDirectorService = hrDirectorService;
    }

    [HttpPost("teams")]
    public IActionResult PostTeams([FromBody] TeamsRequest request) {
        try {
            Console.WriteLine("Receiving teams...: " + request);
            _hrDirectorService.HandleTeams(request);
            return Ok();
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
}

public record TeamsRequest(List<Team> Teams, List<Wishlist> JuniorWishlists, List<Wishlist> TeamLeadWishlists);