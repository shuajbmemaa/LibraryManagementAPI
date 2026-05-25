using LMS.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult CreateResponse<T>(BaseResponse<T> response)
        {
            if (response == null)
            {
                return StatusCode(500, BaseResponse.ServerError("Response object was null"));
            }

            return StatusCode(response.Status, response);
        }
    }
}
