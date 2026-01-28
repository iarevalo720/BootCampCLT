using BootcampCLT.Application.Command;
using BootcampCLT.Infraestructure.Context;
using MediatR;

public class PatchProductHandler :
    IRequestHandler<PatchProductCommand, bool>
{
    private readonly PostegresDbContext _context;
    public PatchProductHandler(PostegresDbContext context) => _context = context;

    public async Task<bool> Handle(PatchProductCommand req, CancellationToken ct)
    {
        var p = await _context.Productos.FindAsync(req.Id);
        if (p == null) return false;

        if (req.Request.Codigo != null) p.Codigo = req.Request.Codigo;
        if (req.Request.Nombre != null) p.Nombre = req.Request.Nombre;
        if (req.Request.Precio.HasValue) p.Precio = req.Request.Precio.Value;
        // ... repetir para el resto de campos

        p.FechaActualizacion = DateTime.UtcNow;
        return await _context.SaveChangesAsync() > 0;
    }
}