using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository:ICommentRepository
{
    private readonly string filePath = "comments.json";
    
    public CommentFileRepository()
    {
        if(!File.Exists(filePath))
            File.WriteAllText(filePath, "[]");
    }

    private async Task<List<Comment>> GetComments()
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
    }

    private async Task WriteComments(List<Comment> comments)
    {
        string commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }
    
    public async Task<Comment> AddAsync(Comment comment)
    {
        var comments = await GetComments();
        int maxId = comments.Count > 0 ? comments.Max(x => x.Id) : 1;
        comment.Id = maxId + 1;
        comments.Add(comment);
        await WriteComments(comments);
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        var comments = await GetComments();
        Comment commentToUpdate = comments.FirstOrDefault(x => x.Id == comment.Id)!;
        comments.Remove(commentToUpdate);
        comments.Add(comment);
        await WriteComments(comments);
    }

    public async Task DeleteAsync(int id)
    {
        var comments = await GetComments();
        Comment commentToDelete = comments.FirstOrDefault(x => x.Id == id)!;
        comments.Remove(commentToDelete);
        await WriteComments(comments);
    }

    public async Task<Comment> getSingleAsync(int id)
    {
        var comments = await GetComments();
        Comment comment = comments.FirstOrDefault(x => x.Id == id)!;
        await WriteComments(comments);
        return comment;
    }

    public IQueryable<Comment> getMany()
    {
        string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return comments.AsQueryable();
    }
}