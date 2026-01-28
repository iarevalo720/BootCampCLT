using BootcampCLT.Application.Command;
using BootcampCLT.Infraestructure.Context;
using MediatR;

public class DeleteProductHandler :
    IRequestHandler<DeleteProductCommand, bool>
{
    private readonly PostegresDbContext _context;
    public DeleteProductHandler(PostegresDbContext context) => _context = context;

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var p = await _context.Productos.FindAsync(request.Id);
        if (p == null) return false;
        _context.Productos.Remove(p);
        return await _context.SaveChangesAsync() > 0;
    }
}