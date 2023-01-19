using Auth.Providers.Interfaces;
using Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using AutoMapper;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using View.Models.In;
using View.Models.Out;

namespace Auth.Providers;

public class UserProvider:IUserProvider
{
    private readonly DatabaseContext _context;
    private readonly ILogger<UserProvider> _logger;
    private readonly IMapper _mapper;
    
    private readonly int _authTokenLifetime;
    private readonly int _refreshTokenLifetime;
    private readonly string _authSecret;

    public UserProvider(DatabaseContext context, ILogger<UserProvider> logger, IConfiguration configuration, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        // время жизни авторизационного токена
        _authTokenLifetime = configuration.GetValue("Settings:AuthLifetime", 3*60);
        // время жизни токена обновления
        _refreshTokenLifetime = configuration.GetValue("Settings:RefreshLifetime", 7*24*60);
        _authSecret = configuration["Settings:AuthenticationSecret"] ?? string.Empty;
    }

    public async Task<IActionResult> AuthenticateAsync(InAuthView userView, CancellationToken stoppingToken)
    {
        if (string.IsNullOrEmpty(userView.Email) || string.IsNullOrEmpty(userView.Password)) 
            return new BadRequestObjectResult("Логин и пароль должны быть заполнены");
        
        var userDb = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == userView.Email, stoppingToken);
        if (userDb == null)
            return new NotFoundObjectResult($"Пользователь с логином '{userView.Email}' не найден");
        
        // check if password is correct
        var correctPassword = VerifyPasswordHash(userView.Password, userDb);
        if (!correctPassword)
            return new BadRequestObjectResult("Неверный пароль");
        
        // Чистим старые refresh-токены
        var rTokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userDb.Id && rt.Expiration < DateTimeOffset.Now).ToListAsync(stoppingToken);
        if (rTokens.Any())
            _context.RefreshTokens.RemoveRange(rTokens);

        // добавляем новый refresh-токен
        var newRefreshToken = GenerateRefreshToken();
        await _context.RefreshTokens.AddAsync(new RefreshToken
        {
            Expiration = newRefreshToken.expiration,
            Token = newRefreshToken.value,
            UserId = userDb.Id
        }, stoppingToken);
        await _context.SaveChangesAsync(stoppingToken);
        
        var user = _mapper.Map<OutUserView>(userDb);
        user.Token = GenerateAccessToken(userDb);
        user.RefreshToken = newRefreshToken.value;

        return new OkObjectResult(user);
    }
    
    public async Task<IActionResult> RefreshAsync(string refreshToken, CancellationToken stoppingToken)
    {
        throw new UnsupportedContentTypeException("");
    }
    public async Task<IActionResult> GetAllAsync(CancellationToken stoppingToken)
    {
        throw new UnsupportedContentTypeException("");
    }
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken stoppingToken)
    {
        throw new UnsupportedContentTypeException("");
    }

    public async Task<IActionResult> CreateAsync(InUserView userView,
        CancellationToken stoppingToken = default)
    {
        // validation
        if (string.IsNullOrWhiteSpace(userView.Password))
            return new BadRequestObjectResult("Пароль не указан!");

        if (await _context.Users.AsNoTracking().AnyAsync(x => x.Email == userView.Email, cancellationToken: stoppingToken))
            return new ConflictObjectResult("Пользователь с такой почтой \"" + userView.Email + "\" уже зарегистрирован");

        var (passwordHash, passwordSalt) = CreatePasswordHash(userView.Password);
        var userDb = _mapper.Map<User>(userView);
        userDb.PasswordHash = passwordHash;
        userDb.PasswordSalt = passwordSalt;
        _context.Users.Add(userDb);
        await _context.SaveChangesAsync(stoppingToken);

        return new OkObjectResult(userDb);
    }

    public async Task<IActionResult> UpdateAsync(InAuthView user, string? password = null,
        CancellationToken stoppingToken = default)
    {
        throw new UnsupportedContentTypeException("");
    }
    public async Task<IActionResult>  DeleteAsync(int id, CancellationToken stoppingToken = default)
    {
        throw new UnsupportedContentTypeException("");
    }

    #region Helpers

    private bool VerifyPasswordHash(string password, User userDb)
    {
        if (userDb.PasswordHash.Length != 64)
        {
            _logger.LogError("Неверная длина хэша (должно быть 64 байта) у пользователя {UserDbEmail}", userDb.Email);
            return false;
        }

        if (userDb.PasswordSalt.Length != 128)
        {
            _logger.LogError("Неверная длина соли (должно быть 128 байт) у пользователя {UserDbEmail}", userDb.Email);
            return false;
        }

        using var hmac = new HMACSHA512(userDb.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return !computedHash.Where((t, i) => t != userDb.PasswordHash[i]).Any();
    }

    private (string value, DateTimeOffset expiration) GenerateRefreshToken(int size = 32)
    {
        var randomNumber = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return (Convert.ToBase64String(randomNumber), DateTimeOffset.Now.AddMinutes(_refreshTokenLifetime));
    }
    
    private string GenerateAccessToken(User userDb)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authSecret);
        var claims = userDb.Roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
        claims.Add(new Claim(nameof(userDb.Email), userDb.Email));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new GenericIdentity(userDb.Id.ToString()),
                claims.ToArray()
            ),
            Expires = DateTime.Now.AddMinutes(_authTokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    private static (byte[], byte[]) CreatePasswordHash(string password)
    {
        byte[] passwordHash;
        byte[] passwordSalt;

        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        return (passwordHash, passwordSalt);
    }
    #endregion
}