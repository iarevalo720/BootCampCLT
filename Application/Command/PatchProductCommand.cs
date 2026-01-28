using BootcampCLT.Api.Request;
using MediatR;

namespace BootcampCLT.Application.Command;

public record PatchProductCommand(int Id, PatchProductoRequest Request) : IRequest<bool>;