using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.DTOs.UserDTOs;
using Project.Exceptions;
using Project.Services;

namespace Project.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    
}
