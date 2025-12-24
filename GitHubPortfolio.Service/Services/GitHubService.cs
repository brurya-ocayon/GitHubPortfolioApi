using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using GitHubPortfolio.Service.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace GitHubPortfolio.Service.Services
{
    public class GitHubService
    {
        private readonly GitHubClient _client;
        private readonly string _username;
        private readonly IMemoryCache _cache;


        public GitHubService(IConfiguration configuration, IMemoryCache cache)
        {
            _cache = cache;
            _username = configuration["GitHub:Username"];
            _client = new GitHubClient(new ProductHeaderValue("GitHubPortfolio"))
            {
                Credentials = new Credentials(configuration["GitHub:Token"])
            };
        }

        public async Task<IReadOnlyList<Repository>> GetPortfolioAsync()
        {
            const string cacheKey = "GitHubPortfolio"; // מפתח ה-Cache
            if (_cache.TryGetValue(cacheKey, out IReadOnlyList<Repository> cachedRepositories))
            {
                return cachedRepositories; // אם הנתונים נמצאים בזיכרון, החזר אותם
            }

            // משיכת הנתונים מ-GitHub
            var repositories = await _client.Repository.GetAllForUser(_username);
            // שמירה ב-Cache ל-5 דקות
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30))
                .SetSlidingExpiration(TimeSpan.FromSeconds(10));

            _cache.Set(cacheKey, repositories, cacheEntryOptions);

            return repositories;
        }

        public async Task<SearchRepositoryResult> SearchRepositoriesAsync(string? name, string? language, string? user)
        {
            var languageEnum = string.IsNullOrEmpty(language) ? (Language?)null : Enum.TryParse(language, true, out Language parsedLanguage) ? parsedLanguage : (Language?)null;

            var request = new SearchRepositoriesRequest(name ?? "")
            {
                Language = languageEnum,
                User = user
            };

            return await _client.Search.SearchRepo(request);
        }
    }
}
