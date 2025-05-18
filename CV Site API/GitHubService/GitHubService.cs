using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class GitHubService : IGitHubService
    {

        private readonly GitHubClient _client;
        private readonly GitHubSettings _settings;
        private readonly IMemoryCache _cache;

        public GitHubService(IOptions<GitHubSettings> options, IMemoryCache cache)
        {
            _settings = options.Value;
            if (string.IsNullOrWhiteSpace(_settings.Username))
            {
                throw new ArgumentException("GitHub username is missing.");
            }
            _client = new GitHubClient(new ProductHeaderValue("CvSiteApp"));

            if (!string.IsNullOrWhiteSpace(_settings.Token))
            {
                _client.Credentials = new Credentials(_settings.Token);
            }

            _cache = cache;
        }

        public async Task<IReadOnlyList<Repository>> GetUserRepositoriesAsync()
        {
            const string cacheKey = "user_repositories";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Repository> repositories))
            {
                repositories = await _client.Repository.GetAllForUser(_settings.Username);
                _cache.Set(cacheKey, repositories, TimeSpan.FromMinutes(5));
            }

            return repositories.ToList();
        }

        public async Task<IReadOnlyList<Repository>> SearchRepositories(string? repoName, string? language, string? user)
        {
            var request = new SearchRepositoriesRequest(repoName ?? "");

            if (!string.IsNullOrWhiteSpace(language) && Enum.TryParse<Language>(language, true, out var parsedLanguage))
            {
                request.Language = parsedLanguage;
            }

            if (!string.IsNullOrWhiteSpace(user))
            {
                request.User = user;
            }

            var result = await _client.Search.SearchRepo(request);
            return result.Items;
        }
        public async Task<List<RepositoryInfo>> GetPortfolioAsync()
        {
            var repositories = await _client.Repository.GetAllForUser(_settings.Username);
            var result = new List<RepositoryInfo>();

            foreach (var repo in repositories)
            {
                var languages = await _client.Repository.GetAllLanguages(_settings.Username, repo.Name);
                var pulls = await _client.PullRequest.GetAllForRepository(_settings.Username, repo.Name);

                result.Add(new RepositoryInfo
                {
                    Name = repo.Name,
                    
                    Description = repo.Description,
                    Stars = repo.StargazersCount,
                    LastCommitDate = repo.PushedAt?.DateTime,
                    Url = repo.HtmlUrl,
                    Languages = languages.Select(l => l.Name).ToList(),
                    PullRequestCount = pulls.Count
                });
            }

            return result;
        }
    }

}
