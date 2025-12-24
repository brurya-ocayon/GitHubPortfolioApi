using GitHubPortfolio.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitHubPortfolio.Api.Controllers;

[ApiController]
[Route("api/github")]
public class GitHubController : ControllerBase
{
    private readonly GitHubService _gitHubService;

    public GitHubController(GitHubService gitHubService)
    {
        _gitHubService = gitHubService;
    }

    [HttpGet("portfolio")]
    public async Task<IActionResult> GetPortfolio()
    {
        var repositories = await _gitHubService.GetPortfolioAsync();
        return Ok(repositories.Select(repo => new
        {
            repo.Name,
            repo.Language,
            repo.StargazersCount,
            repo.PushedAt,
            repo.HtmlUrl
        }));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchRepositories([FromQuery] string? name, [FromQuery] string? language, [FromQuery] string? user)
    {
        var result = await _gitHubService.SearchRepositoriesAsync(name, language, user);
        return Ok(result.Items.Select(repo => new
        {
            repo.Name,
            repo.Language,
            repo.StargazersCount,
            repo.HtmlUrl
        }));
    }
}
