using Microsoft.AspNetCore.Mvc;

namespace ProgressPlayMCP.API.Controllers;

/// <summary>
/// Base controller for all API controllers
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
}