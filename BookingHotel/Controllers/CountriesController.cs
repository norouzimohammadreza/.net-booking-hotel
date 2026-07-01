using BookingHotel.Contracts;
using BookingHotel.Data;
using BookingHotel.DTOs.Country;
using BookingHotel.Results;
using Microsoft.AspNetCore.Mvc;


namespace BookingHotel.Controllers; 

[Route("api/[controller]")]
[ApiController]
public class CountriesController(ICountriesService countriesService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountriesDto>>> GetCountries()
    {
        var countries = await countriesService.GetCountries();
        return ToActionResult(countries);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
    {
        var country = await countriesService.GetCountry(id);
       return ToActionResult(country);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto countryDto)
    { 
       var result = await countriesService.UpdateCountry(id, countryDto);
           return ToActionResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto countryDto)
    {
        var result = await countriesService.CreateCountry(countryDto);
        if (!result.IsSuccess)
        {
           return MapErrorsToResponse(result.Errors);
        }
        return CreatedAtAction("GetCountry", new { id = result.Value!.CountryId }, result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
       var result = await countriesService.DeleteCountryDto(id); 
      return ToActionResult(result);
    } 
    
    private ActionResult<T> ToActionResult<T>(Result<T> result) =>
        result.IsSuccess ? Ok(result.Value) : MapErrorsToResponse(result.Errors);
    
    private ActionResult ToActionResult(Result result) =>
        result.IsSuccess ? NoContent() : MapErrorsToResponse(result.Errors);

    private ActionResult MapErrorsToResponse(Error[] errors)
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
                _ => Conflict(e.Description)
            };
    }

}