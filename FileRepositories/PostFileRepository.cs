using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository: IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if(!File.Exists(filePath))
            File.WriteAllText(filePath, "[]");
    }

    private async Task<List<Post>> GetPosts()
    {
        var json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<Post>>(json)!;
    }

    private async Task WritePosts(List<Post> posts)
    {
        string json = JsonSerializer.Serialize(posts, typeof(List<Post>));
        await File.WriteAllTextAsync(filePath, json);
    }
    
    public async Task<Post> AddAsync(Post post)
    {
        var posts = await GetPosts();
        int maxId = posts.Count > 0 ? posts.Max(x => x.Id) : 1;
        post.Id = maxId + 1;
        posts.Add(post);
        await WritePosts(posts);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        var posts = await GetPosts();
        Post oldPost = posts.FirstOrDefault(x => x.Id == post.Id)!;
        posts.Remove(oldPost);
        posts.Add(post);
        await WritePosts(posts);
    }

    public async Task DeleteAsync(int id)
    {
        var posts = await GetPosts();
        Post post = posts.FirstOrDefault(x => x.Id == id)!;
        posts.Remove(post);
        await WritePosts(posts);
    }

    public async Task<Post> getSingleAsync(int id)
    {
        var posts = await GetPosts();
        Post post = posts.FirstOrDefault(x => x.Id == id)!;
        await WritePosts(posts);
        return post;
    }

    public IQueryable<Post> getMany()
    {
        string postsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.AsQueryable();
    }
}