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
                return NotFound();
            }

            if (response.Status == 404)
            {
                return NotFound(response);
            }

            if (response.Status == 400)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
