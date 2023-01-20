using Microsoft.AspNetCore.Mvc;
using View.Models.In;
using View.Models.Out;

namespace Auth.Providers.Interfaces;

public interface IUserProvider
{
    Task<IActionResult> AuthenticateAsync(InAuthView userView, CancellationToken stoppingToken = default);
    Task<IActionResult> RefreshAsync(string refreshToken, CancellationToken stoppingToken = default);
    Task<IActionResult> GetAllAsync(CancellationToken stoppingToken = default);
    Task<IActionResult> GetByIdAsync(int id, CancellationToken stoppingToken = default);
    Task<IActionResult> CreateAsync(InCreateUserView userView, CancellationToken stoppingToken = default);
    Task<IActionResult> UpdateAsync(InAuthView user, string? password = null, CancellationToken stoppingToken = default);
    Task<IActionResult> DeleteAsync(int id, CancellationToken stoppingToken = default);
}