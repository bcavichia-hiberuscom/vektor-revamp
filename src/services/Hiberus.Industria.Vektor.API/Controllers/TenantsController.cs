using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Microsoft.AspNetCore.Mvc;

namespace Hiberus.Industria.Vektor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly TenantAppService _service;

    public TenantsController(TenantAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default
    )
    {
        var result = await _service.GetAllPaginatedAsync(pageNumber, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var tenant = await _service.GetByIdAsDto(id, ct);

        if (tenant is null)
            return NotFound();

        return Ok(tenant);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTenantDto dto, CancellationToken ct)
    {
        var result = await _service.Create(dto, ct);

        if (result.IsError)
            return BadRequest(result.Errors);

        var t = result.Value;
        var tenantDto = await _service.GetByIdAsDto(t.Id, ct);
        
        return CreatedAtAction(nameof(GetById), new { id = t.Id }, tenantDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTenantDto dto,
        CancellationToken ct
    )
    {
        var result = await _service.Update(id, dto, ct);

        if (result.IsError)
            return BadRequest(result.Errors);

        var tenantDto = await _service.GetByIdAsDto(result.Value.Id, ct);
        
        return Ok(tenantDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _service.Delete(id, ct);

        if (result.IsError)
            return result.FirstError.Type == ErrorType.NotFound
                ? NotFound()
                : BadRequest(result.Errors);

        return NoContent();
    }
}
