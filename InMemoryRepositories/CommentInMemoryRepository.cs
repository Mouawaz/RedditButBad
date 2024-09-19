using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository:ICommentRepository
{
    List<Comment> comments;

    public CommentInMemoryRepository()
    {
        comments = new List<Comment>();
    }
    private Comment? GetCommentById(int id)
    {
        return comments.SingleOrDefault(x => x.Id == id) ?? throw new InvalidOperationException($"Comment with ID '{id}' not found");
    }
    
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any() ? comments.Max(x => x.Id) + 1 : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }   

    public Task UpdateAsync(Comment comment)
    {
        comments.Remove(GetCommentById(comment.Id));
        comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        comments.Remove(GetCommentById(id));
        return Task.CompletedTask;
    }

    public Task<Comment> getSingleAsync(int id)
    {
        Comment? comment = GetCommentById(id);
        return Task.FromResult(comment);
    }

    public IQueryable<Comment> getMany()
    {
        return comments.AsQueryable();
    }
}