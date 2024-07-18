﻿namespace EchoChat.Dtos;

public class ChatDto
{
    public string? Id { get; set; }

    public string? UserId { get; set; }

    public string? ReceiverId { get; set; }

    public string? ReceiverName { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }
}