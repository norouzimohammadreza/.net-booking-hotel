using BookingHotel.Contracts;
using BookingHotel.DTOs.Country;
using BookingHotel.DTOs.Pagination;
using BookingHotel.Results;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BookingHotel.Controllers; 

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CountriesController(ICountriesService countriesService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<GetCountriesDto>>> GetCountries([FromQuery]PaginationQuery pagination)
    {
        var countries = await countriesService.GetCountries(pagination);
        return ToActionResult(countries);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
    {
        var country = await countriesService.GetCountry(id);
       return ToActionResult(country);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put(int id, UpdateCountryDto countryDto)
    { 
       var result = await countriesService.UpdateCountry(id, countryDto);
           return ToActionResult(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GetCountryDto>> Post(
        CreateCountryDto countryDto
        //[FromServices] IValidator<CreateCountryDto> validator 
        )
    {
       // var validationResult = await validator.ValidateAsync(countryDto);
        //if (!validationResult.IsValid)
        //{
          //  return BadRequest(validationResult.Errors);
        //}
        
        var result = await countriesService.CreateCountry(countryDto);
        if (!result.IsSuccess)
        {
           return MapErrorsToResponse(result.Errors);
        }
        return CreatedAtAction("GetCountry", new { id = result.Value!.Id }, result.Value);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
       var result = await countriesService.DeleteCountryDto(id); 
      return ToActionResult(result);
    } 
}