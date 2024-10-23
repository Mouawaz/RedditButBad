using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]

public class CommentController : ControllerBase
{
    private readonly ICommentRepository commentRepository;

    public CommentController(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<IResult> AddComment([FromBody] CreateCommentDto req,
        [FromServices] ICommentRepository commentRepository)
    {
        Comment comment = new Comment(req.Body, req.PostId, req.UserId);
        Comment created = await commentRepository.AddAsync(comment);
        CreateCommentDto dto = new()
        {
            Body = created.Body,
            PostId = created.PostId,
            UserId = created.UserId
        };
        return Results.Created($"/api/comment/{created.Id}", dto);
    }
    
    [HttpPut("{id}")]
    public async Task<IResult> ReplaceComment([FromRoute] int id, [FromBody] CreateCommentDto req,
        [FromServices] ICommentRepository commentRepository)
    {
        Comment existingComment = await commentRepository.getSingleAsync(id);
        existingComment.Body = req.Body;
        existingComment.PostId = req.PostId;
        existingComment.UserId = req.UserId;
        await commentRepository.UpdateAsync(existingComment);
        return Results.Ok();
    }

    [HttpGet("user/{id}")]
    public async Task<IResult> GetCommentsByUserId([FromRoute] int id)
    {
        var comments = commentRepository.getMany();
        comments = comments.Where(c => c.UserId == id);
        return Results.Ok(comments);
    }

    [HttpGet("post/{postId}")]
    public async Task<IResult> GetCommentsByPostId([FromRoute] int postId)
    {
        var comments = commentRepository.getMany();
        comments = comments.Where(c => c.PostId == postId);
        return Results.Ok(comments);
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteComment([FromRoute] int id)
    {
        await commentRepository.DeleteAsync(id);
        return Results.NoContent();
    }
}