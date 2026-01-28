using BootcampCLT.Application.Command;
using BootcampCLT.Infraestructure.Context;
using MediatR;

public class UpdateProductHandler :
    IRequestHandler<UpdateProductCommand, bool>
{
    private readonly PostegresDbContext _context;
    public UpdateProductHandler(PostegresDbContext context) => _context = context;

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var p = await _context.Productos.FindAsync(request.Id);
        if (p == null) return false;

        p.Codigo = request.Codigo;
        p.Nombre = request.Nombre;
        p.Descripcion = request.Descripcion;
        p.Precio = request.Precio;
        p.Activo = request.Activo;
        p.CategoriaId = request.CategoriaId;
        p.FechaActualizacion = DateTime.UtcNow;

        return await _context.SaveChangesAsync() > 0;
    }
}