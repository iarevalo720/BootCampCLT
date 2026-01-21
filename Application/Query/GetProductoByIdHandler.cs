using BootcampCLT.Api.Response;
using BootcampCLT.Infraestructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Application.Query
{
    public class GetProductoByIdHandler : IRequestHandler<GetProductoByIdQuery, ProductoResponse?>
    {
        private readonly PostegresDbContext _postegresDbContext;
        public GetProductoByIdHandler(PostegresDbContext postegresDbContext)
        {
            _postegresDbContext = postegresDbContext;
        }

        public async Task<ProductoResponse?> Handle(GetProductoByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _postegresDbContext.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id);

            if (entity == null)
                return null;

            return new ProductoResponse
            (
                Id: entity.Id,
                Codigo: entity.Codigo,
                Nombre: entity.Nombre,
                Descripcion: entity.Descripcion ?? string.Empty,
                Precio: entity.Precio,
                Activo: entity.Activo,
                CategoriaId: entity.CategoriaId,
                FechaCreacion: entity.FechaCreacion,
                FechaActualizacion: entity.FechaActualizacion,
                CantidadStock: entity.CantidadStock
            );
        }
    }
}
