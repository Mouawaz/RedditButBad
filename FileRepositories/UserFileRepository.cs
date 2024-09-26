using System.Formats.Tar;
using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository:IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if(!File.Exists(filePath))
            File.WriteAllText(filePath, "[]");
    }

    private async Task<List<User>> GetUsers()
    {
        string json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<User>>(json)!;
    }

    private async Task WriteUsers(List<User> users)
    {
        string json = JsonSerializer.Serialize(users, typeof(List<User>));
        await File.WriteAllTextAsync(filePath, json);
    }
    public async Task<User> AddAsync(User user)
    {
        var users = await GetUsers();
        int maxId = users.Count > 0 ? users.Max(x => x.Id) : 0;
        user.Id = maxId + 1;
        users.Add(user);
        await WriteUsers(users);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var users = await GetUsers();
        User userToUpdate = users.SingleOrDefault(x => x.Id == user.Id)!;
        users.Remove(userToUpdate);
        users.Add(user);
        await WriteUsers(users);
    }

    public async Task DeleteAsync(int id)
    {
        var users = await GetUsers();
        User userToDelete = users.SingleOrDefault(x => x.Id == id)!;
        users.Remove(userToDelete);
        await WriteUsers(users);
    }
    
    public async Task<User> getSingleAsync(int id)
    {
        var users = await GetUsers();
        User user = users.SingleOrDefault(x => x.Id == id)!;
        await WriteUsers(users);
        return user;
    }

    public IQueryable<User> getMany()
    {
        string usersAsJson = File.ReadAllTextAsync(filePath).Result;
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users.AsQueryable();
    }
}