using BootcampCLT.Api.Request;
using BootcampCLT.Api.Response;
using BootcampCLT.Application.Command;
using BootcampCLT.Application.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class ProductoController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductoController> _logger;

    public ProductoController(IMediator mediator, ILogger<ProductoController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    /// <summary>
    /// Obtiene el detalle de un producto por su identificador.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <returns>Producto encontrado.</returns>
    [HttpGet("v1/api/productos")]
    [ProducesResponseType(typeof(IEnumerable<ProductoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductos()
    {
        try
        {
            _logger.LogInformation("Consultando lista completa de productos.");
            var result = await _mediator.Send(new GetProductosQuery());
            return !result.Any() ? NoContent() : Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de productos.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error interno al procesar la solicitud.");
        }
    }

    /// <summary>
    /// Obtiene el detalle de un producto por su identificador.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <returns>Producto encontrado.</returns>
    [HttpGet("v1/api/productos/{id:int}")]
    [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductoResponse>> GetProductoByIdAsync([FromRoute] int id)
    {
        try
        {
            _logger.LogInformation("Inicia la peticion con productoId={ProductoID}", id);
            var result = await _mediator.Send(new GetProductoByIdQuery(id));

            if (result is null)
            {
                _logger.LogWarning("No existe producto con el productoId={ProductoID}", id);
                return NotFound();
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el producto con ID: {ProductoId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error interno al procesar la solicitud.");
        }
    }

    /// <summary>
    /// Crea un nuevo producto.
    /// </summary>
    /// <param name="request">Datos del producto a crear.</param>
    /// <returns>Producto creado.</returns>
    [HttpPost("v1/api/productos")]
    [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductoResponse>> CreateProducto([FromBody] CreateProductoRequest request)
    {
        try
        {
            _logger.LogInformation("Creando nuevo producto con código: {Codigo}", request.Codigo);
            var command = new CreateProductCommand(request.Codigo, request.Nombre, request.Descripcion, request.Precio, request.CategoriaId);
            var result = await _mediator.Send(command);
            return CreatedAtAction("GetProductoById", new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el producto: {Nombre}", request.Nombre);
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error interno al procesar la solicitud.");
        }
    }

    /// <summary>
    /// Actualiza completamente un producto existente.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <param name="request">Datos completos a actualizar.</param>
    [HttpPut("v1/api/productos/{id:int}")]
    [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProducto(int id, [FromBody] UpdateProductoRequest request)
    {
        try
        {
            _logger.LogInformation("Actualizando producto ID: {ProductoId}", id);
            var result = await _mediator.Send(new UpdateProductCommand(id, request.Codigo, request.Nombre, request.Descripcion, request.Precio, request.Activo, request.CategoriaId));
            return result ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el producto ID: {ProductoId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error interno al procesar la solicitud.");
        }
    }

    /// <summary>
    /// Elimina un producto existente.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    [HttpDelete("v1/api/productos/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProducto(int id)
    {
        try
        {
            _logger.LogInformation("Eliminando producto ID: {ProductoId}", id);
            var result = await _mediator.Send(new DeleteProductCommand(id));
            return result ? NoContent() : NotFound();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error crítico al eliminar el producto ID: {ProductoId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error interno al procesar la solicitud.");
        }
    }
}
