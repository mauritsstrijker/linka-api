using Linka.Application.Common;
using Linka.Application.Dtos;
using MediatR;

namespace Linka.Application.Features.Search
{
    public class SearchRequest : IRequest<SearchResponse>
    {
        public string Search { get; set; }
    }

    public class SearchResponse
    {
        public List<SearchResultDto> Results { get; set; }
    };

    public class SearchHandler
        (
        IFeedService feedService
        )
        : IRequestHandler<SearchRequest, SearchResponse>
    {
        public async Task<SearchResponse> Handle(SearchRequest request, CancellationToken cancellationToken)
        {
            return new SearchResponse { Results = await feedService.SearchAsync(request.Search) };
        }
    }
}
