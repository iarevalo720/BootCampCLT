using BootcampCLT.Api.Response;
using MediatR;

public record CreateProductCommand(
    string Codigo, string Nombre, string? Descripcion,
    decimal Precio, int CategoriaId) : IRequest<ProductoResponse>;