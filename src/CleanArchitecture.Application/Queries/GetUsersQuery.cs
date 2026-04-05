using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Queries;

public sealed record GetUsersQuery : IRequest<IReadOnlyList<User>>;