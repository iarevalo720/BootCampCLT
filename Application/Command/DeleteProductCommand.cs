using MediatR;

namespace BootcampCLT.Application.Command;

public record DeleteProductCommand(int Id) : IRequest<bool>;