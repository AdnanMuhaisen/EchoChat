﻿namespace EchoChat.Core.Domain.Abstractions;

public interface IDeletableEntity
{
    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }
}