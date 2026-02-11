using LMS.Application.DTO.AI;
using LMS.Domain.Entities;
using LMS.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;

namespace LMS.Application.Services.AI
{
    public class OllamaQueryService : IOllamaQueryService
    {
        private readonly HttpClient _http;
        private readonly ApplicationDbContext _dbContext;

        private const string Url = "http://localhost:11434/v1/chat/completions";

        // 🧠 Simple in-memory cache
        private static readonly Dictionary<string, QueryPlan> _cache = new();

        private const string SystemPrompt = """
You are a Query Planner AI.

Return ONLY valid JSON:

{
  "Entity": "ApplicationUser | Book",
  "Filter": [],
  "OrderBy": null,
  "OrderDirection": null,
  "Select": null,
  "Limit": null,
  "Chat": {
    "Intro": "",
    "ItemTemplate": "",
    "Outro": ""
  }
}

Rules:
- Entity: ApplicationUser or Book
- Never invent fields
- ItemTemplate uses {{FieldName}}

Book fields:
Id, Title, Author, Genre, ReadingStatus

User fields:
Id, UserName, Name, Email
""";

        public OllamaQueryService(HttpClient httpClient, ApplicationDbContext dbContext)
        {
            _http = httpClient;
            _http.Timeout = TimeSpan.FromSeconds(15);
            _dbContext = dbContext;
        }

        public async Task<QueryPlan?> GenerateQueryPlanAsync(string question)
        {
            if (_cache.TryGetValue(question, out var cached))
                return cached;

            var body = new
            {
                model = "gemma3:1b", // ⚡ FAST model
                messages = new[]
                {
                    new { role = "system", content = SystemPrompt },
                    new { role = "user", content = question }
                },
                stream = false,
                options = new
                {
                    num_predict = 80,
                    temperature = 0.0,
                    top_p = 0.9,
                    keep_alive = "5m"
                }
            };

            var sw = Stopwatch.StartNew();

            var response = await _http.PostAsJsonAsync(Url, body);
            response.EnsureSuccessStatusCode();

            var rawResponse = await response.Content.ReadAsStringAsync();
            sw.Stop();

            Console.WriteLine($"🧠 AI planning took {sw.ElapsedMilliseconds} ms");

            try
            {
                using var doc = JsonDocument.Parse(rawResponse);
                var content = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                if (string.IsNullOrWhiteSpace(content))
                    return null;

                content = content.Replace("```json", "").Replace("```", "").Trim();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var plan = JsonSerializer.Deserialize<QueryPlan>(content, options);
                if (plan == null)
                    return null;

                _cache[question] = plan;
                return plan;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ AI JSON parse error:");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<(List<object> Data, int Count)> ExecuteQueryPlanAsync(QueryPlan plan)
        {
            var sw = Stopwatch.StartNew();

            IQueryable queryable = plan.Entity.ToLower() switch
            {
                "applicationuser" => _dbContext.Set<ApplicationUser>().AsNoTracking(),
                "book" => _dbContext.Set<Domain.Entities.Book>().AsNoTracking(),
                _ => throw new ArgumentException("Unsupported entity")
            };

            if (plan.Filter?.Any() == true)
            {
                var clauses = new List<string>();
                var values = new List<object>();
                int i = 0;

                foreach (var f in plan.Filter)
                {
                    clauses.Add($"{f.Field} == @{i}");
                    values.Add(f.Value!);
                    i++;
                }

                queryable = queryable.Where(string.Join(" && ", clauses), values.ToArray());
            }

            if (!string.IsNullOrEmpty(plan.OrderBy))
            {
                queryable = plan.OrderDirection?.ToLower() == "desc"
                    ? queryable.OrderBy($"{plan.OrderBy} descending")
                    : queryable.OrderBy(plan.OrderBy);
            }

            if (plan.Limit.HasValue)
                queryable = queryable.Take(plan.Limit.Value);

            if (plan.Select?.Any() == true)
            {
                queryable = queryable.Select($"new({string.Join(",", plan.Select)})");
            }

            var list = await queryable.Cast<object>().ToListAsync();
            sw.Stop();

            Console.WriteLine($"⚡ EF execution took {sw.ElapsedMilliseconds} ms");

            return (list, list.Count);
        }
    }
}