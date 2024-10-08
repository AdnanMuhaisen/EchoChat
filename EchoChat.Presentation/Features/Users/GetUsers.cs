﻿using EchoChat.Dtos;
using EchoChat.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EchoChat.Features.Users;

public static class GetUsers
{
    public class Query:IRequest<List<ApplicationUserDto>> { }

    public sealed class Handler(AppDbContext appDbContext) : IRequestHandler<Query, List<ApplicationUserDto>>
    {
        public async Task<List<ApplicationUserDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await appDbContext
                .Users
                .AsNoTracking()
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