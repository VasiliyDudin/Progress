using System.Net.Mime;
using Auth.Providers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using View.Models.In;
using View.Models.Out;

namespace Auth.Controllers;

[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Route("api/[controller]"), ApiController]
public class UserController: ControllerBase
{
    private readonly IUserProvider _userProvider;
    public UserController(IUserProvider userProvider)
    {
        _userProvider = userProvider;
    }
    
    [HttpPost, AllowAnonymous]
    [ProducesResponseType(typeof(OutUserView), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] InCreateUserView userView)
    {
        var userResult = await _userProvider.CreateAsync(userView);
        return userResult;
    }
    
}