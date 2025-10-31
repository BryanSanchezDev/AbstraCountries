using AbstraCountries.Contracts.Dtos;
using AbstraCountries.Managers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace AbstraCountries.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryManager _manager;
        private readonly HybridCache _cache;

        public CountriesController(
            ICountryManager manager,
            HybridCache cache)
        {
            _manager = manager;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var countries = await _cache.GetOrCreateAsync(
                "all-countries",
                async cancel => await _manager.GetAllAsync(),
                cancellationToken: token,
                options: new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromHours(1),
                    LocalCacheExpiration = TimeSpan.FromMinutes(5)
                });

            return Ok(countries);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var country = await _manager.GetByIdAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CountryDto dto)
        {
            var created = await _manager.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CountryDto dto)
        {
            var updated = await _manager.UpdateAsync(id, dto);

            if (updated == null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _manager.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
