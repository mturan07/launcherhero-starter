using LauncherHero.Starter.Models;
using LauncherHero.Starter.Services;
using Microsoft.AspNetCore.Mvc;

namespace LauncherHero.Starter.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        return user is not null ? Ok(user) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateRequest request)
    {
        var created = await _userService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UserUpdateRequest request)
    {
        if (id != request.Id)
            return BadRequest();

        var updated = await _userService.UpdateAsync(request);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _userService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
