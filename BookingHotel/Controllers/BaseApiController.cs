using BookingHotel.Results;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Controllers;

public abstract class BaseApiController : ControllerBase
{
    protected ActionResult<T> ToActionResult<T>(Result<T> result) =>
        result.IsSuccess ? Ok(result.Value) : MapErrorsToResponse(result.Errors);
    
    protected ActionResult ToActionResult(Result result) =>
        result.IsSuccess ? NoContent() : MapErrorsToResponse(result.Errors);

    protected ActionResult MapErrorsToResponse(Error[] errors)
    {
        if (errors == null || errors.Length == 0)
        {
            return Problem();
        }
        var e = errors[0];
        return e.Code switch
        {
            "NotFound" => NotFound(e.Description),
            "BadRequest" => BadRequest(e.Description),
            "Validation" => BadRequest(e.Description),
            "Forbidden" => Forbid(e.Description),
            "Conflict" => Conflict(e.Description),
            _ => Problem(detail:string.Join("; ",errors.Select(x=> x.Description)),title:e.Code)
        };
    }
}