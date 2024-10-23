using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]

public class UserController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UserController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserDto>> AddUser([FromBody] CreateUserDto req, [FromServices] IUserRepository
        userRepository)
    {
        User user = new User(req.Username, req.Password);
        User created = await userRepository.AddAsync(user);
        CreateUserDto dto = new()
        {
            Username = created.Username,
            Password = created.Password
        };
        return Created($"users/{created.Id}", dto);
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetUser([FromRoute] int id)
    {
        User user = await userRepository.getSingleAsync(id);
        return Results.Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IResult> ReplaceUser([FromRoute] int id, [FromBody] CreateUserDto req, [FromServices] 
        IUserRepository 
            userRepository)
    {
        User existingUser = await userRepository.getSingleAsync(id);
        existingUser.Username = req.Username;
        existingUser.Password = req.Password;
        
        await userRepository.UpdateAsync(existingUser);
        return Results.Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteUser([FromRoute] int id)
    {
        await userRepository.DeleteAsync(id);
        return Results.NoContent();
    }

    [HttpGet]
    public async Task<IResult> GetMany([FromQuery] string? userUsername)
    {
        var users = userRepository.getMany();
        if (!String.IsNullOrEmpty(userUsername))
            users = users.Where(u => u.Username.ToLower().Contains(userUsername.ToLower()));
        return Results.Ok(users);
    }
}