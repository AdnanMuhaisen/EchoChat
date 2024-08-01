using EchoChat.Dtos;
using EchoChat.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EchoChat.Features.Users;

public static class GetUsersWithoutChat
{
    public class Query(int UserId, IEnumerable<int> usersWithChats) : IRequest<List<ApplicationUserDto>>
    {
        public int UserId { get; set; } = UserId;

        public readonly IEnumerable<int> UsersToExeclude = usersWithChats;
    }

    public sealed class Handler(AppDbContext appDbContext) : IRequestHandler<Query, List<ApplicationUserDto>>
    {
        public async Task<List<ApplicationUserDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await appDbContext
                .Users
                .AsNoTracking()
                .Where(u => !request.UsersToExeclude.Contains(u.Id) && u.Id != request.UserId)
                .Select(u => new ApplicationUserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserName = u.UserName
                })
                .ToListAsync();
        }
    }
}