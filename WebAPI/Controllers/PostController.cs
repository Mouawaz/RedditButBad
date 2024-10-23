using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]

public class PostController : ControllerBase
{
    private readonly IPostRepository postRepository;

    public PostController(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    [HttpPost]
    public async Task<ActionResult<CreateCommentDto>> CreatePost([FromBody] CreatePostDto req, [FromServices]
        IPostRepository
            postRepository)
    {
        Post post = new Post(req.Title, req.Body, req.UserId);
        Post created = await postRepository.AddAsync(post);
        CreateCommentDto dto = new()
        {
            Body = created.Body,
            UserId = created.UserId
        };
        return Created($"posts/{created.Id}", dto);
    }

    [HttpPut("{id}")]
    public async Task<IResult> ReplacePost([FromRoute] int id, [FromBody] CreatePostDto req, [FromServices] IPostRepository postRepository)
    {
        Post existingPost = await postRepository.getSingleAsync(id);
        existingPost.Title = req.Title;
        existingPost.Body = req.Body;
        existingPost.UserId = req.UserId;
        
        await postRepository.UpdateAsync(existingPost);
        return Results.Ok();
    }
      
    [HttpGet("id/{id}")]
    public async Task<IResult> GetPost([FromRoute] int id)
    {
        Post post = await postRepository.getSingleAsync(id);
        return Results.Ok(post);
    }

    [HttpGet]
    public async Task<IResult> GetPostsByTitle([FromQuery] string? title)
    {
        var posts = postRepository.getMany();
        if (!string.IsNullOrEmpty(title))
        {
            posts = posts.Where(t => t.Title.ToLower().Contains(title.ToLower()));
        }
        return Results.Ok(posts);
    }

    [HttpGet("user/{userId}")]
    public async Task<IResult> GetPostsByUser([FromRoute] int? userId)
    {
        var posts = postRepository.getMany();
        posts = posts.Where(t => t.UserId == userId);
        return Results.Ok(posts);
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeletePost([FromRoute] int id)
    {
        await postRepository.DeleteAsync(id);
        return Results.NoContent();
    }
}