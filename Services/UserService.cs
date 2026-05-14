using LauncherHero.Starter.Data.Repositories;
using LauncherHero.Starter.Models;

namespace LauncherHero.Starter.Services;

public class UserService
{
    private readonly UserRepository _repository;

    public UserService(UserRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _repository.GetAllAsync();
        return users.Select(MapToDto).ToList();
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _repository.GetByIdAsync(id);
        return user is null ? null : MapToDto(user);
    }

    public async Task<UserDto> CreateAsync(UserCreateRequest request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(user);
        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(UserUpdateRequest request)
    {
        var user = new User
        {
            Id = request.Id,
            Name = request.Name,
            Email = request.Email
        };

        return await _repository.UpdateAsync(user);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}
