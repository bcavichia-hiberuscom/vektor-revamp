using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs;
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
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var tenants = await _service.GetAll(ct);
        return Ok(tenants.Select(ToDto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var tenant = await _service.GetById(id, ct);

        if (tenant is null)
            return NotFound();

        return Ok(ToDto(tenant));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTenantDto dto, CancellationToken ct)
    {
        var result = await _service.Create(dto, ct);

        if (result.IsError)
            return BadRequest(result.Errors);

        var t = result.Value;
        return CreatedAtAction(nameof(GetById), new { id = t.Id }, ToDto(t));
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

        return Ok(ToDto(result.Value));
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

    private static TenantDto ToDto(Domain.Tenant.Tenant t) =>
        new(t.Id, t.Name, t.Slug, t.IsActive, t.CreatedAt, t.UpdatedAt);
}
