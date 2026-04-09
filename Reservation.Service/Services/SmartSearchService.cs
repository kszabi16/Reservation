using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Microsoft.Extensions.Configuration;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Context; // Ehhez kell
using Microsoft.EntityFrameworkCore; // Ehhez is kell
using AutoMapper; // És az AutoMapper is

namespace Reservation.Service.Services
{
    public class SmartSearchService
    {
        private readonly SearchClient _searchClient;
        private readonly ReservationDbContext _context;
        private readonly IMapper _mapper;

        public SmartSearchService(IConfiguration configuration, ReservationDbContext context, IMapper mapper)
        {
            string searchEndpoint = configuration["AzureSearch:Endpoint"];
            string queryApiKey = configuration["AzureSearch:ApiKey"];
            string indexName = "property-vector-index";

            _searchClient = new SearchClient(
                new Uri(searchEndpoint),
                indexName,
                new AzureKeyCredential(queryApiKey));

            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PropertyDto>> SearchAsync(string userQuery)
        {
            var options = new SearchOptions
            {
                Select = { "PropertyId" },
                QueryType = SearchQueryType.Semantic,
                SemanticSearch = new SemanticSearchOptions
                {
                    SemanticConfigurationName = "my-semantic-config"
                },
                Size = 10
            };

            options.VectorSearch = new VectorSearchOptions();
            options.VectorSearch.Queries.Add(new VectorizableTextQuery(text: userQuery)
            {
                KNearestNeighborsCount = 10,
                Fields = { "DescriptionVector" }
            });

            SearchResults<SearchDocument> response = await _searchClient.SearchAsync<SearchDocument>(userQuery, options);

            var propertyScores = new Dictionary<int, double?>();

            await foreach (SearchResult<SearchDocument> result in response.GetResultsAsync())
            {
                var doc = result.Document;
                if (doc.TryGetValue("PropertyId", out var idObj) && idObj != null)
                {
                    int propId = Convert.ToInt32(idObj.ToString());
                    propertyScores[propId] = result.SemanticSearch.RerankerScore;
                }
            }

            var matchingIds = propertyScores.Keys.ToList();

            if (!matchingIds.Any())
            {
                return new List<PropertyDto>(); 
            }

            var propertiesFromDb = await _context.Properties
                .Where(p => matchingIds.Contains(p.Id) && p.IsApproved)
                .Include(p => p.Ratings)
                .Include(p => p.Images)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .AsSplitQuery()
                .ToListAsync();

            var dtos = _mapper.Map<List<PropertyDto>>(propertiesFromDb);

            foreach (var dto in dtos)
            {
                if (propertyScores.TryGetValue(dto.Id, out var score))
                {
                    dto.RelevancyScore = score;
                }
            }
            return dtos.OrderByDescending(d => d.RelevancyScore).ToList();
        }
    }
}