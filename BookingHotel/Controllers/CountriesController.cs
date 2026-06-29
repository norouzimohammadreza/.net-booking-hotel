using BookingHotel.Contracts;
using BookingHotel.Data;
using BookingHotel.DTOs.Country;
using BookingHotel.DTOs.Hotel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Controllers; 

[Route("api/[controller]")]
[ApiController]
public class CountriesController(ICountriesService countriesService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountriesDto>>> GetCountries()
    {
        var countries = await countriesService.GetCountries();
        return Ok(countries);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
    {
        var country = await countriesService.GetCountry(id);
        if (country == null)
        {
            return NotFound();
        } 

        return Ok(country);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto countryDto)
    { 
        if (id != countryDto.CountryId)
        {
            return BadRequest();
        } 
        await countriesService.UpdateCountry(id, countryDto);
        
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto countryDto)
    {
        var country = await countriesService.CreateCountry(countryDto);
        return CreatedAtAction("GetCountry", new { id = country.CountryId }, country);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        await countriesService.DeleteCountryDto(id); 
        return NoContent();
    } 

}