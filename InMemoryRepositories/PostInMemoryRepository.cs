using Entities;
using RepositoryContracts;
using InvalidOperationException = System.InvalidOperationException;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts;

    public PostInMemoryRepository()
    {
        posts = new List<Post>();
    }
    private Post? GetPostById(int id)
    {
        return posts.SingleOrDefault(p => p.Id == id) ?? throw new InvalidOperationException($"Post with ID '{id}' not found");
    }
    
    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any() ? posts.Max(p => p.Id) + 1 : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        posts.Remove(GetPostById(post.Id));
        posts.Add(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        posts.Remove(GetPostById(id));
        return Task.CompletedTask;
    }

    public Task<Post> getSingleAsync(int id)
    {
        Post? post = GetPostById(id);
        
        return Task.FromResult(post);
    }

    public IQueryable<Post> getMany()
    {
        return posts.AsQueryable();
    }
}