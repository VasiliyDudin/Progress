using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers;

[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Route("user"), ApiController]
public class UserController: ControllerBase
{
    
}