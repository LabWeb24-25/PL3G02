using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using LibSpace_Aspnet.Models;
using LibSpace_Aspnet.Data;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LibSpace_Aspnet.Controllers
{
    public class AIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private const string GEMINI_API_URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp:generateContent";
        
        // Rate limiting variables
        private static ConcurrentQueue<DateTime> _requestTimestamps = new ConcurrentQueue<DateTime>();
        private const int MAX_REQUESTS_PER_MINUTE = 10;
        private static readonly TimeSpan RATE_LIMIT_WINDOW = TimeSpan.FromMinutes(1);
        private static readonly object _lockObject = new object();

        public AIController(IConfiguration configuration, ApplicationDbContext context, HttpClient httpClient)
        {
            _configuration = configuration;
            _context = context;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", "AIzaSyDmrYyo5gjFZ9_PVV_d6Q5qa6CaZK7Zbes");
        }

        [HttpPost]
        public async Task<IActionResult> GetBookRecommendations([FromBody] string userQuery)
        {
            try
            {
                // Check rate limit
                var (canProceed, waitTime) = CheckRateLimit();
                if (!canProceed)
                {
                    return StatusCode(429, new { 
                        error = "Limite de pedidos excedido",
                        waitTime = waitTime.TotalSeconds,
                        message = $"Por favor, aguarde {waitTime.TotalSeconds:F0} segundos antes de tentar novamente."
                    });
                }

                // Add current request to queue
                _requestTimestamps.Enqueue(DateTime.UtcNow);

                // Step 1: Analyze user query to identify relevant book types
                var bookTypes = _context.Generos.Select(g => g.NomeGeneros).ToList();
                var queryAnalysisPrompt = $@"Given these book types: {string.Join(", ", bookTypes)}
User query: '{userQuery}'
Identify the relevant book types for this query.";

                var queryAnalysisResponse = await CallGeminiAPI(queryAnalysisPrompt);
                var relevantBookTypes = JsonSerializer.Deserialize<string[]>(queryAnalysisResponse);

                // Step 2: Query the database for books in the identified categories
                var books = _context.Livros
                    .Where(l => relevantBookTypes.Contains(l.IdGenerosNavigation.NomeGeneros))
                    .Select(l => new {
                        l.IdLivro,
                        l.TituloLivros,
                        l.Sinopse,
                        Autor = l.IdAutorNavigation.NomeAutor,
                        Genero = l.IdGenerosNavigation.NomeGeneros
                    })
                    .ToList();

                // Step 3: Refine recommendations with AI
                var refinementPrompt = $@"Given these books:
{JsonSerializer.Serialize(books)}
User query: '{userQuery}'
Return a JSON array of book IDs that best match the query.";

                var refinementResponse = await CallGeminiAPI(refinementPrompt);
                var recommendedBookIds = JsonSerializer.Deserialize<int[]>(refinementResponse);

                return Json(recommendedBookIds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessMessage([FromBody] string message)
        {
            Debug.WriteLine("\n=== Starting ProcessMessage ===");
            Debug.WriteLine($"Received message: {message}");
            
            try
            {
                var (canProceed, waitTime) = CheckRateLimit();
                if (!canProceed)
                {
                    return StatusCode(429, new { 
                        error = "Limite de pedidos excedido",
                        waitTime = waitTime.TotalSeconds,
                        message = $"Por favor, aguarde {waitTime.TotalSeconds:F0} segundos."
                    });
                }

                _requestTimestamps.Enqueue(DateTime.UtcNow);
                
                // Step 1: Get genres and create a focused prompt for JSON response
                var bookTypes = _context.Generos.Select(g => g.NomeGeneros).ToList();
                var genrePrompt = $@"Given these book genres: {string.Join(", ", bookTypes)}
For the query: '{message}'
Return a JSON array of the most relevant genre names. Example: ['Fiction', 'Fantasy']
Response:";

                var genreResponse = await CallGeminiAPI(genrePrompt, false);
                Debug.WriteLine($"Genre API Response: {genreResponse}");
                
                var relevantGenres = JsonSerializer.Deserialize<string[]>(genreResponse);
                Debug.WriteLine($"Parsed genres: {string.Join(", ", relevantGenres)}");

                // Step 2: Get matching books
                var books = await _context.Livros
                    .Where(l => relevantGenres.Contains(l.IdGenerosNavigation.NomeGeneros))
                    .Include(l => l.IdAutorNavigation)
                    .Include(l => l.IdEditoraNavigation)
                    .Include(l => l.IdGenerosNavigation)
                    .Select(l => new {
                        l.IdLivro,
                        l.TituloLivros,
                        l.Sinopse,
                        l.CapaImg,
                        Autor = l.IdAutorNavigation.NomeAutor,
                        Editora = l.IdEditoraNavigation.NomeEditora,
                        Genero = l.IdGenerosNavigation.NomeGeneros
                    })
                    .Take(10)
                    .ToListAsync();

                if (!books.Any())
                {
                    return PartialView("_BookList", new List<Livro>());
                }

                // Step 3: Get ranked book IDs
                var rankingPrompt = $@"Given these books: {JsonSerializer.Serialize(books)}
For the query: '{message}'
Return a JSON object with:
1. 'bookIds': array of book IDs ordered by relevance
2. 'explanation': a friendly Portuguese text (2-5 sentences) explaining why these books were chosen with deep analysis of the books and justification of the choice. Do not only base your self in the description given by the books, use your own knowledge about the book to justify your choice.
Example: {{
    ""bookIds"": [1, 4, 2],
    ""explanation"": ""Os livros selecionados formam uma combinação perfeita para sua busca. 'O Senhor dos Anéis' se destaca não apenas pela sua narrativa épica de fantasia, mas também pela profunda construção de mundo e temas sobre amizade e coragem que J.R.R. Tolkien magistralmente teceu através de suas mais de mil páginas. 'Duna', de Frank Herbert, complementa a seleção com sua complexa trama política e ecológica, sendo considerada uma das obras mais influentes da ficção científica. A inclusão de 'Fundação', de Isaac Asimov, adiciona uma perspectiva única sobre o futuro da humanidade, com sua análise matemática da psico-história e previsão do comportamento das massas.""
}}
Response:";

                var rankingResponse = await CallGeminiAPI(rankingPrompt, true);
                Debug.WriteLine($"Ranking API Response: {rankingResponse}");
                
                var recommendation = JsonSerializer.Deserialize<BookRecommendation>(rankingResponse);
                
                // Get final ordered books with full details
                var orderedBooks = await _context.Livros
                    .Where(l => recommendation.bookIds.Contains(l.IdLivro))
                    .Include(l => l.IdAutorNavigation)
                    .Include(l => l.IdEditoraNavigation)
                    .ToListAsync();

                var result = orderedBooks
                    .OrderBy(b => Array.IndexOf(recommendation.bookIds, b.IdLivro))
                    .ToList();

                ViewData["Explanation"] = recommendation.explanation;
                return PartialView("_BookList", result);
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"JSON Error: {ex.Message}");
                return BadRequest(new { error = "Erro ao processar a resposta da IA" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return BadRequest(new { error = "Erro ao processar seu pedido" });
            }
        }

        private (bool canProceed, TimeSpan waitTime) CheckRateLimit()
        {
            lock (_lockObject)
            {
                var now = DateTime.UtcNow;

                // Limpar timestamps antigos da fila
                while (_requestTimestamps.TryPeek(out DateTime oldestTimestamp) && 
                       now - oldestTimestamp > RATE_LIMIT_WINDOW)
                {
                    _requestTimestamps.TryDequeue(out _);
                }

                // Ver se já chegámos ao limite de pedidos
                if (_requestTimestamps.Count >= MAX_REQUESTS_PER_MINUTE)
                {
                    // Se chegámos ao limite, vamos ver quanto tempo falta para poder fazer novo pedido
                    if (_requestTimestamps.TryPeek(out DateTime oldestTime))
                    {
                        var waitTime = RATE_LIMIT_WINDOW - (now - oldestTime);
                        return (false, waitTime);
                    }
                }

                return (true, TimeSpan.Zero);
            }
        }

        private async Task<string> CallGeminiAPI(string prompt, bool expectResponse = false)
        {
            try 
            {
                object schema;
                if (expectResponse)
                {
                    schema = new
                    {
                        type = "OBJECT",
                        properties = new
                        {
                            bookIds = new
                            {
                                type = "ARRAY",
                                items = new { type = "INTEGER" }
                            },
                            explanation = new
                            {
                                type = "STRING"
                            }
                        },
                        required = new[] { "bookIds", "explanation" }
                    };
                }
                else
                {
                    schema = new
                    {
                        type = "ARRAY",
                        items = new { type = "STRING" }
                    };
                }

                var request = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        response_mime_type = "application/json",
                        response_schema = schema
                    }
                };

                var response = await _httpClient.PostAsJsonAsync(GEMINI_API_URL, request);
                response.EnsureSuccessStatusCode();
                
                var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
                return geminiResponse?.candidates?[0]?.content?.parts?[0]?.text?.Trim() ?? "[]";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"API Error: {ex.Message}");
                throw;
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }

    // Classes to deserialize Gemini response
    public class GeminiResponse
    {
        public Candidate[] candidates { get; set; }
    }

    public class Candidate
    {
        public Content content { get; set; }
    }

    public class Content
    {
        public Part[] parts { get; set; }
    }

    public class Part
    {
        public string text { get; set; }
    }

    public class BookRecommendation
    {
        public int[] bookIds { get; set; }
        public string explanation { get; set; }
    }
}
