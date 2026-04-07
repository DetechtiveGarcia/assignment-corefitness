using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers;

public class ErrorController : Controller
{
    [Route("error/{statusCode}")]
    public IActionResult HandleErrorCode(int statusCode)
    {
        Response.StatusCode = statusCode;

        return statusCode switch
        {
            401 => View("UnAuthorized"),
            403 => View("Forbidden"),
            404 => View("NotFound"),
            _ => View("Error"),
        };
    }
}
