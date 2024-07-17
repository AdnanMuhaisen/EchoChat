using EchoChat.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace EchoChat.Domain.UserAggregates;

public class ApplicationUser : IdentityUser<int>, IEntity { }