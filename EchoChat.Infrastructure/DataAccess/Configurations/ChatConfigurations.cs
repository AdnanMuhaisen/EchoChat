using EchoChat.Domain.ChatAggregates;
using Microsoft.EntityFrameworkCore;

namespace EchoChat.Infrastructure.DataAccess.Configurations;

internal class ChatConfigurations : IEntityTypeConfiguration<Chat>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Chat> builder)
    {

    }
}
