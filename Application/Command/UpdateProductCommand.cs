using MediatR;

namespace BootcampCLT.Application.Command;

public record UpdateProductCommand(
    int Id, string Codigo, string Nombre, string? Descripcion,
    decimal Precio, bool Activo, int CategoriaId) : IRequest<bool>;