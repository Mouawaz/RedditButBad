using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository: IUserRepository
{
    private List<User> users;

    public UserInMemoryRepository()
    {
        users = new List<User>();
    }

    private User? GetUserById(int id)
    {
        return users.SingleOrDefault(u => u.Id == id) ?? throw new InvalidOperationException($"User with ID '{id}' not found");
    }

    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any() ? users.Last().Id + 1 : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        users.Remove(GetUserById(user.Id));
        users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        users.Remove(GetUserById(id));
        return Task.CompletedTask;
    }

    public Task<User> getSingleAsync(int id)
    {
        User? user = GetUserById(id);
        return Task.FromResult(user);
    }

    public IQueryable<User> getMany()
    {
        return users.AsQueryable();
    }
}