using AbstraCountries.Contracts.Dtos;
using AbstraCountries.Resources.Data;
using Microsoft.EntityFrameworkCore;

namespace AbstraCountries.Accessors.Interfaces
{
    public interface ICountryAccessor
    {
        Task<IEnumerable<CountryDto>> GetAllAsync();
        Task<CountryDto?> GetByIdAsync(string id);
        Task<CountryDto> CreateAsync(CountryDto dto);
        Task<CountryDto?> UpdateAsync(string id, CountryDto dto);
        Task<bool> DeleteAsync(string id);
    }

    public class CountryAccessor : ICountryAccessor
    {
        private readonly AbstraCountriesDbContext _context;

        public CountryAccessor(AbstraCountriesDbContext context) => _context = context;

        public async Task<IEnumerable<CountryDto>> GetAllAsync() =>
            await _context.Countries
                .Select(c => new CountryDto { Id = c.Id, Name = c.Name })
                .ToListAsync();

        public async Task<CountryDto?> GetByIdAsync(string id) =>
            await _context.Countries.Where(c => c.Id == id)
                .Select(c => new CountryDto { Id = c.Id, Name = c.Name })
                .FirstOrDefaultAsync();

        public async Task<CountryDto> CreateAsync(CountryDto dto)
        {
            if (!string.IsNullOrEmpty(dto.Id))
            {
                var exists = await _context.Countries.AnyAsync(c => c.Id == dto.Id);
                if (exists)
                {
                    throw new InvalidOperationException($"Country with ID '{dto.Id}' already exists.");
                }
            }

            var entity = new Country 
            { 
                Id = dto.Id,
                Name = dto.Name 
            };

            _context.Countries.Add(entity);
            await _context.SaveChangesAsync();
            dto.Id = entity.Id;

            return dto;
        }

        public async Task<CountryDto?> UpdateAsync(string id, CountryDto dto)
        {
            var entity = await _context
                .Countries
                .FindAsync(id);

            if (entity == null)
            {
                return null;
            }

            entity.Name = dto.Name;
            await _context.SaveChangesAsync();

            return dto with { Id = id };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context
                .Countries
                .FindAsync(id);

            if (entity == null)
            {
                return false;
            }

            _context.Countries.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
