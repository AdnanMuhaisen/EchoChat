using EchoChat.Domain.Abstractions;

namespace EchoChat.Domain.ChatAggregates;

public class Chat : IEntity
{
    public int Id { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    // collection of messages
}