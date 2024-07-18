using EchoChat.Core.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace EchoChat.Core.Domain.UserAggregates;

public class ApplicationUser : IdentityUser<int>, IEntity { }