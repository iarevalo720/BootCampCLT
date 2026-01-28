using BootcampCLT.Api.Response;
using BootcampCLT.Infraestructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Application.Query;

public class GetProductosHandler : IRequestHandler<GetProductosQuery, IEnumerable<ProductoResponse>>
{
    private readonly PostegresDbContext _context;

    public GetProductosHandler(PostegresDbContext context) => _context = context;

    public async Task<IEnumerable<ProductoResponse>> Handle(GetProductosQuery request, CancellationToken cancellationToken)
    {
        return await _context.Productos
            .AsNoTracking()
            .Select(p => new ProductoResponse(
                p.Id, p.Codigo, p.Nombre, p.Descripcion ?? string.Empty,
                p.Precio, p.Activo, p.CategoriaId, p.FechaCreacion,
                p.FechaActualizacion, p.CantidadStock))
            .ToListAsync(cancellationToken);
    }
}