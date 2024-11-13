using Linka.Application.Common;
using Linka.Application.Dtos;
using MediatR;
using System.Collections.Generic;

namespace Linka.Application.Features.Search
{
    public class SearchRequest : IRequest<List<SearchResultDto>>
    {
        public string Search { get; set; }
    }

    public class SearchHandler
        (
        IFeedService feedService
    )
        : IRequestHandler<SearchRequest, List<SearchResultDto>>
    {
        public async Task<List<SearchResultDto>> Handle(SearchRequest request, CancellationToken cancellationToken)
        {
            return await feedService.SearchAsync(request.Search);
        }
    }
}
