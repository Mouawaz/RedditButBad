using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class UserView
{
    private readonly IUserRepository userRepository;

    public UserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task Init()
    {
        await CreateDummyData();
    }

    private async Task<User> AddUserAsync(string name, string password)
    {
        User user = new User(1, name, password);
        User created = await userRepository.AddAsync(user);
        return created;
    }
    
    private async Task CreateDummyData()
    {
        await AddUserAsync("Danny", "Password123");
        await AddUserAsync("Bob", "Password1234");
        await AddUserAsync("Steve", "Password12345");
    }
}