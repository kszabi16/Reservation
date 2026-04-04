using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Microsoft.Extensions.Configuration;
using Reservation.DataContext.Dtos;


namespace Reservation.Service.Services
{
    public class SmartSearchService
    {
        private readonly SearchClient _searchClient;

        public SmartSearchService(IConfiguration configuration)
        {
            string searchEndpoint = configuration["AzureSearch:Endpoint"];
            string queryApiKey = configuration["AzureSearch:ApiKey"];
            string indexName = "property-vector-index";

            _searchClient = new SearchClient(
                new Uri(searchEndpoint),
                indexName,
                new AzureKeyCredential(queryApiKey));
        }

        public async Task<List<PropertySearchDto>> SearchAsync(string userQuery)
        {
            var results = new List<PropertySearchDto>();

            var options = new SearchOptions
            {
                Select = { "PropertyId", "Title", "Location", "PricePerNight", "AmenitiesList", "MainImageUrl" },
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

            await foreach (SearchResult<SearchDocument> result in response.GetResultsAsync())
            {
                var doc = result.Document;
                results.Add(new PropertySearchDto
                {
                    PropertyId = doc["PropertyId"]?.ToString(),
                    Title = doc["Title"]?.ToString(),
                    Location = doc["Location"]?.ToString(),
                    PricePerNight = Convert.ToDecimal(doc["PricePerNight"]),
                    AmenitiesList = doc["AmenitiesList"]?.ToString(),
                    MainImageUrl = doc["MainImageUrl"]?.ToString(),
                    RelevancyScore = result.SemanticSearch.RerankerScore
                });
            }

            return results;
        }
    }
}