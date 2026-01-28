using BootcampCLT.Api.Response;
using BootcampCLT.Domain.Entity;
using BootcampCLT.Infraestructure.Context;
using MediatR;

namespace BootcampCLT.Application.Command;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductoResponse>
{
    private readonly PostegresDbContext _context;
    public CreateProductHandler(PostegresDbContext context) => _context = context;

    public async Task<ProductoResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var nuevo = new Producto
        {
            Codigo = request.Codigo,
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            Precio = request.Precio,
            CategoriaId = request.CategoriaId,
            Activo = true,
            FechaCreacion = DateTime.UtcNow,
            CantidadStock = 0
        };

        _context.Productos.Add(nuevo);
        await _context.SaveChangesAsync(cancellationToken);

        return new ProductoResponse(nuevo.Id, nuevo.Codigo, nuevo.Nombre, nuevo.Descripcion ?? "",
            nuevo.Precio, nuevo.Activo, nuevo.CategoriaId, nuevo.FechaCreacion, null, nuevo.CantidadStock);
    }
}