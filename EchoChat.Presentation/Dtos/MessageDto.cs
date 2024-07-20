namespace EchoChat.Dtos;

public class MessageDto
{
    public string? Id { get; set; }

    public string? ChatId { get; set; }

    public string? SenderId { get; set; }

    public string? ReceiverId { get; set; }

    public string? Text { get; set; }

    public DateTime SentAt { get; set; }

    public DateTime? SeenAt { get; set; }

    public MessageFileDto? MessageFile { get; set; }
}

public class MessageFileDto
{
    public string? Url { get; set; }

    public string? ContentType { get; set; }
}