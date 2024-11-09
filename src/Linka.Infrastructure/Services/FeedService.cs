using Linka.Application.Common;
using Linka.Domain.Entities;
using Linka.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

        var followedOrganizations = await _context.Follows
            .Where(f => f.Volunteer.Id == volunteerId)
            .Select(f => f.Organization)
            .ToListAsync();

        var relevantPosts = await _context.Posts
            .Include(p => p.Likes)
            .Include(p => p.Shares)
            .Include(p => p.AssociatedOrganization)
            .Where(p =>
                p.Likes.Any(like => friends.Any(f => f.User.Id == like.User.Id)) ||
                p.Shares.Any(share => friends.Any(f => f.User.Id == share.User.Id)) ||
                followedOrganizations.Contains(p.AssociatedOrganization))
            .ToListAsync();

        var scoredPosts = relevantPosts
            .Select(p => new
            {
                Post = p,
                Score = CalculatePostScoreForVolunteerFeed(p, friends)
            })
            .OrderByDescending(p => p.Score)
            .ThenByDescending(p => p.Post.DateCreated)
            //.Take(20) 
            .Select(p => p.Post)
            .ToList();

        return scoredPosts;
    }

    private double CalculatePostScoreForVolunteerFeed(Post post, List<Volunteer> friends)
    {
        double score = 0;

        score += post.Likes.Count * 2; 

        score += post.Likes.Count(like => friends.Any(f => f.User.Id == like.User.Id)) * 3;
        score += post.Shares.Count(share => friends.Any(f => f.User.Id == share.User.Id)) * 4;

        var hoursSincePost = (DateTime.UtcNow - post.DateCreated).TotalHours;
        score += Math.Max(0, 10 - hoursSincePost); 

        return score;
    }
}

