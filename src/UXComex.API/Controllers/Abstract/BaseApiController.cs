using Microsoft.AspNetCore.Mvc;

namespace UXComex.API.Controllers.Abstract;

public class BaseApiController : ControllerBase
{
    protected IActionResult BaseResponse<T>(T response)
    {
        if (response is null)
            return NotFound();

        return Ok(response);
    }
}