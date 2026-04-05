using MediatR;

namespace CleanArchitecture.Application.Commands;

public sealed record CreateUserCommand(string Name) : IRequest<Guid>;