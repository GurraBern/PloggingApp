using Microsoft.AspNetCore.Mvc;
using Plogging.Core.Models;
using PloggingAPI.Repository;
using PloggingAPI.Repository.Interfaces;

namespace PloggingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserInfoController : Controller
{
    private readonly IUserInfoRepository _userInfoRepository;

    public UserInfoController(IUserInfoRepository userInfoRepository)
    {
        _userInfoRepository = userInfoRepository;
    }

    [HttpGet("GetUserInfo/{userId}")]
    public async Task<ActionResult> GetUserInfo(string userId)
    {
        try
        {
            await _userInfoRepository.GetUser(userId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult> CreateUser([FromBody] UserInfo user)
    {
        try
        {
            await _userInfoRepository.CreateUser(user);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpDelete("DeleteUser")]
    public async Task<ActionResult> DeleteUser(string userId)
    {
        try
        {
            await _userInfoRepository.DeleteUser(userId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}

