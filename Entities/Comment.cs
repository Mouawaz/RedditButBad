namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }
    
    public int PostId { get; set; }
    public Comment(int id, string body, int postId)
    {
        Id = id;
        Body = body;
        PostId = postId;
    }
}