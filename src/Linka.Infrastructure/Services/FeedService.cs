using Linka.Application.Common;
using Linka.Application.Dtos;
using Linka.Domain.Entities;
using Linka.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Linka.Infrastructure.Services;
public class FeedService : IFeedService
{
    private readonly Context _context;

    public FeedService(Context context)
    {
        _context = context;
    }

    public async Task<List<Post>> GetFeedForVolunteerAsync(Guid volunteerId)
    {
        var volunteer = await _context.Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteerId);

        if (volunteer == null)
            throw new ArgumentException("Voluntário não encontrado");

        var friends = await _context.Connections
            .Where(c => c.Volunteer1.Id == volunteerId || c.Volunteer2.Id == volunteerId)
            .Select(c => c.Volunteer1.Id == volunteerId ? c.Volunteer2 : c.Volunteer1)
            .ToListAsync();

        var followingOrganizations = await _context.Follows
            .Where(f => f.Volunteer.Id == volunteerId)
            .Select(x => x.Organization)
            .ToListAsync();
            

        var followedOrganizations = await _context.Follows
            .Where(f => f.Volunteer.Id == volunteerId)
            .Select(f => f.Organization)
            .ToListAsync();

        var relevantPosts = await _context.Posts
            .Include(p => p.Likes)
            .Include(p => p.Shares)
            .Include(p => p.AssociatedOrganization)
            .Include(p => p.Author)
            .ToListAsync();

        var scoredPosts = relevantPosts
            .Select(p => new
            {
                Post = p,
                Score = CalculatePostScoreForVolunteerFeed(p, friends, followingOrganizations)
            })
            .OrderByDescending(p => p.Score)
            //.Take(20) 
            .Select(p => p.Post)
            .ToList();

        return scoredPosts;
    }

    private double CalculatePostScoreForVolunteerFeed(Post post, List<Volunteer> friends, List<Organization> organizations)
    {
        double score = 0;

        var friendIds = new HashSet<Guid>(friends.Select(f => f.Id));

        var organizationIds = new HashSet<Guid>(organizations.Select(o => o.Id));

        int totalInteractions = post.Likes.Count + post.Shares.Count;

        score += Math.Sqrt(post.Likes.Count) * 2;

        double friendBonus = friendIds.Contains(post.Author.Id)
            ? 50 * (1 + Math.Log10(totalInteractions + 1)) 
            : 0;
        score += friendBonus;

        double organizationBonus = organizationIds.Contains(post.AssociatedOrganization.Id)
            ? 50 * (1 + Math.Log10(totalInteractions + 1))
            : 0;
        score += organizationBonus;

        score += post.Likes.Count(like => friendIds.Contains(like.User.Id)) * 3;

        score += post.Shares.Count(share => friendIds.Contains(share.User.Id)) * 4;

        var hoursSincePost = (DateTime.UtcNow - post.DateCreated).TotalHours;
        score += Math.Max(0, 10 - hoursSincePost);

        return score;
    }

    public async Task<List<Post>> GetOrganizationFeed(Guid organizationId)
    {
        var posts = await _context.Posts
            .Where(post => post.AssociatedOrganization.Id == organizationId)
            .Include(p => p.Likes)
            .Include(p => p.Shares)
                .ThenInclude(share => share.User)
            .Include(p => p.Comments)
            .Include(p => p.AssociatedOrganization)
            .Include(p => p.Author)
            .ToListAsync();

        var scoredPosts = posts
            .Select(post => new
            {
                Post = post,
                Score = CalculateScoreForOrganizationPost(post)
            })
            .OrderByDescending(x => x.Score)
            .Select(x => x.Post)
            .ToList();

        return scoredPosts;
    }


    private double CalculateScoreForOrganizationPost(Post post)
    {
        double score = 0;

        score += Math.Sqrt(post.Likes.Count) * 2;
        score += Math.Sqrt(post.Comments.Count) * 1.5; 
        score += Math.Sqrt(post.Shares.Count) * 3;     

        var hoursSincePost = (DateTime.UtcNow - post.DateCreated).TotalHours;
        score += Math.Max(0, 10 - hoursSincePost);

        return score;
    }

    public async Task<List<SearchResultDto>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return new List<SearchResultDto>();
        }

        string searchLower = searchTerm.ToLower();

        var volunteers = await _context.Volunteers
            .Where(v => (v.Name + " " + v.Surname).ToLower().Contains(searchLower))
            .Select(v => new SearchResultDto
            {
                Type = "Volunteer",
                DisplayName = v.Name + " " + v.Surname,
                Id = v.Id,
                MatchScore = (v.Name + " " + v.Surname).ToLower().StartsWith(searchLower) ? 1 : 2
            })
            .ToListAsync();

        var organizations = await _context.Organizations
            .Where(o => o.TradingName.ToLower().Contains(searchLower))
            .Select(o => new SearchResultDto
            {
                Type = "Organization",
                DisplayName = o.TradingName,
                Id = o.Id,
                MatchScore = o.TradingName.ToLower().StartsWith(searchLower) ? 1 : 2
            })
            .ToListAsync();

        var events = await _context.Events
            .Where(e => e.Title.ToLower().Contains(searchLower))
            .Select(e => new SearchResultDto
            {
                Type = "Event",
                DisplayName = e.Title,
                Id = e.Id,
                MatchScore = e.Title.ToLower().StartsWith(searchLower) ? 1 : 2
            })
            .ToListAsync();

        var result = new List<SearchResultDto>();
        result.AddRange(volunteers);
        result.AddRange(organizations);
        result.AddRange(events);

        return result.OrderBy(r => r.MatchScore).ThenBy(r => r.DisplayName).ToList();
    }

}

