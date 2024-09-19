using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CommentView
{
    private readonly ICommentRepository _commentRepository;

    public CommentView(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }
}