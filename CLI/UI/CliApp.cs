using Entities;
using InMemoryRepositories;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository _userRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;

    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        _userRepository = userRepository;
        _commentRepository = commentRepository;
        _postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        Console.WriteLine("CliApp started");
        var createUserView = new UserView(_userRepository);
        var createPostView = new PostView(_postRepository);
        var createCommentView = new CommentView(_commentRepository);

        await createUserView.Init();

        string? input;
        do
        {
            Console.WriteLine("1. Create user");
            Console.WriteLine("2. Create post");
            Console.WriteLine("3. Create comment");
            Console.WriteLine("4. See users");
            Console.WriteLine("5. See posts");
            Console.WriteLine("6. See specific post");
            Console.WriteLine("7. Exit");

            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.WriteLine("Username: ");
                    string? username = Console.ReadLine();
                    Console.WriteLine("Password: ");
                    string? password = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    {
                        Console.WriteLine("Invalid username or password");
                        return;
                    }

                    await _userRepository.AddAsync(new User(1, username, password));
                    Console.WriteLine("User added");
                    break;
                case "2":
                    Console.WriteLine("Title: ");
                    string? title = Console.ReadLine();
                    Console.WriteLine("Body: ");
                    string? body = Console.ReadLine();
                    Console.WriteLine("User Id: ");
                    int userId = Convert.ToInt32(Console.ReadLine());

                    if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
                    {
                        Console.WriteLine("Invalid title or body");
                        return;
                    }

                    await _postRepository.AddAsync(new Post(1, title, body, userId));
                    Console.WriteLine("Post added");
                    break;
                case "3":
                    Console.WriteLine("Body: ");
                    string? comment_body = Console.ReadLine();
                    Console.WriteLine("Post Id: ");
                    string? comment_postId = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(comment_body) || string.IsNullOrWhiteSpace(comment_postId))
                    {
                        Console.WriteLine("Invalid body or comment");
                        return;
                    }

                    await _commentRepository.AddAsync(new Comment(1, comment_body, Convert.ToInt32(comment_postId)));
                    Console.WriteLine("Comment added");
                    break;
                case "4":
                    var users = _userRepository.getMany();
                    foreach (var user in users)
                        Console.WriteLine($"ID: {user.Id}, Name: {user.Username}");
                    break;
                case "5":
                    var posts = _postRepository.getMany();
                    foreach (var post in posts)
                        Console.WriteLine($"ID: {post.Id}, Title: {post.Title}, Body: {post.Body} by {post.UserId}");
                    break;
                case "6":
                    Console.WriteLine("Post Id");
                    string? postId = Console.ReadLine();
                    Post post2 = await _postRepository.getSingleAsync(Convert.ToInt32(postId));
                    Console.WriteLine($"Title: {post2.Title}, Body: {post2.Body}, User Id: {post2.UserId}");
                    var comments = _commentRepository.getMany();
                    foreach (var comment in comments)
                        if (comment.Id == post2.Id)
                            Console.WriteLine($"Comment Id: {comment.Id}, Comment Body: {comment.Body}");
                    break;
            }
        } while (input != "7");
    }
}