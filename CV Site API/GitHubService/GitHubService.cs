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
        //private readonly GitHubClient _client;
        //private readonly GitHubSettings _settings;
        //private readonly IMemoryCache _cache;

        //public GitHubService(IOptions<GitHubSettings> options, IMemoryCache cache)
        //{
        //    _settings = options.Value;
        //    if (string.IsNullOrWhiteSpace(_settings.Username))
        //    {
        //        throw new ArgumentException("GitHub username is missing.");
        //    }
        //    Console.WriteLine("_setting userName");
        //    Console.WriteLine(_settings.Username);
        //    _client = new GitHubClient(new ProductHeaderValue("CvSiteApp"));

        //    if (!string.IsNullOrWhiteSpace(_settings.Token))
        //    {
        //        _client.Credentials = new Credentials(_settings.Token);
        //    }
        //    _cache = cache;
        //}

        //public async Task<IReadOnlyList<Repository>> GetUserRepositoriesAsync()
        //{
        //    const string cacheKey = "user_repositories";

        //    if (!_cache.TryGetValue(cacheKey, out IEnumerable<Repository> repositories))
        //    {
        //        repositories = await _client.Repository.GetAllForUser(_settings.Username);
        //        _cache.Set(cacheKey, repositories, TimeSpan.FromMinutes(5));
        //    }

        //    return repositories.ToList(); // Explicitly convert IEnumerable to IReadOnlyList  
        //}

        //public async Task<IEnumerable<Repository>> SearchRepositoriesAsync(string query, string language, string user)
        //{
        //    Language? parsedLanguage = null;
        //    if (Enum.TryParse(language, true, out Language resultLanguage))
        //    {
        //        parsedLanguage = resultLanguage;
        //    }

        //    var request = new SearchRepositoriesRequest(query)
        //    {
        //        Language = parsedLanguage,
        //        User = user
        //    };
        //    var searchResult = await _client.Search.SearchRepo(request);
        //    return searchResult.Items;
        //}

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

        //public async Task<IEnumerable<Repository>> SearchRepositoriesAsync(string query, string language, string user)
        //{
        //    Language? parsedLanguage = null;
        //    if (Enum.TryParse(language, true, out Language resultLanguage))
        //    {
        //        parsedLanguage = resultLanguage;
        //    }

        //    var request = new SearchRepositoriesRequest(query)
        //    {
        //        Language = parsedLanguage,
        //        User = user
        //    };
        //    var searchResult = await _client.Search.SearchRepo(request);
        //    return searchResult.Items;
        //}

        public async Task<IEnumerable<Repository>> SearchRepositoriesAsync(string query, string language, string user)
        {
            var request = new SearchRepositoriesRequest(query)
            {
                User = user
            };

            var searchResult = await _client.Search.SearchRepo(request);

            if (!string.IsNullOrWhiteSpace(language))
            {
                var filtered = new List<Repository>();

                foreach (var repo in searchResult.Items)
                {
                    // Retrieve all languages of the repository  
                    var languages = await _client.Repository.GetAllLanguages(repo.Owner.Login, repo.Name);

                    // Check if the requested language exists in the list  
                    if (languages.Any(l => l.Name.Equals(language, StringComparison.OrdinalIgnoreCase)))
                    {
                        filtered.Add(repo);
                    }
                }

                return filtered;
            }

            return searchResult.Items;
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
