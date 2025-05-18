using Microsoft.AspNetCore.Mvc;
using Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CV_Site_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {

        private readonly IGitHubService _gitHubService;

        public GitHubController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        [HttpGet("repositories")]
        public async Task<IActionResult> GetRepositories()
        {
            try
            {
                var repositories = await _gitHubService.GetUserRepositoriesAsync();
                return Ok(repositories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchRepos([FromQuery] string? name, [FromQuery] string? language, [FromQuery] string? user)
        {
            var result = await _gitHubService.SearchRepositories(name, language, user);
            return Ok(result);
        }

        [HttpGet("portfolio")]
        public async Task<IActionResult> GetPortfolio()
        {
            try
            {
                var repositories = await _gitHubService.GetPortfolioAsync();
                return Ok(repositories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
