using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class PostView
{
    private IPostRepository postRepository;

    public PostView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }
}