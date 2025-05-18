using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IGitHubService
    {
        Task<IReadOnlyList<Repository>> GetUserRepositoriesAsync();
        Task<IReadOnlyList<Repository>> SearchRepositories(string? repoName, string? language, string? user);
        Task<List<RepositoryInfo>> GetPortfolioAsync();


    }
}
