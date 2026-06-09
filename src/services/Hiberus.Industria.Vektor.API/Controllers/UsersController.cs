using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hiberus.Industria.Vektor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserAppService _service;

    public UsersController(UserAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid tenantId, CancellationToken ct)
    {
        var users = await _service.GetAll(tenantId, ct);
        return Ok(users.Select(ToDto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var user = await _service.GetById(id, tenantId, ct);

        if (user is null)
            return NotFound();

        return Ok(ToDto(user));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken ct)
    {
        var result = await _service.Create(dto, ct);

        if (result.IsError)
            return BadRequest(result.Errors);

        var u = result.Value;
        return CreatedAtAction(nameof(GetById), new { id = u.Id, tenantId = u.TenantId }, ToDto(u));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromQuery] Guid tenantId,
        [FromBody] UpdateUserDto dto,
        CancellationToken ct
    )
    {
        var result = await _service.Update(id, tenantId, dto, ct);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(ToDto(result.Value));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var result = await _service.Delete(id, tenantId, ct);

        if (result.IsError)
            return result.FirstError.Type == ErrorType.NotFound
                ? NotFound()
                : BadRequest(result.Errors);

        return NoContent();
    }

    // Mapeo centralizado para no repetir en cada action
    private static UserDto ToDto(Domain.User.User u) =>
        new(u.Id, u.TenantId, u.Email, u.Name, u.Role, u.IsActive, u.CreatedAt, u.UpdatedAt);
}
