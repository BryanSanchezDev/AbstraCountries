using AbstraCountries.Accessors.Interfaces;
using AbstraCountries.Contracts.Dtos;

namespace AbstraCountries.Managers.Interfaces
{
    public interface ICountryManager
    {
        Task<IEnumerable<CountryDto>> GetAllAsync();
        Task<CountryDto?> GetByIdAsync(string id);
        Task<CountryDto> CreateAsync(CountryDto dto);
        Task<CountryDto?> UpdateAsync(string id, CountryDto dto);
        Task<bool> DeleteAsync(string id);
    }

    public class CountryManager : ICountryManager
    {
        private readonly ICountryAccessor _accessor;

        public CountryManager(ICountryAccessor accessor)
        {
            _accessor = accessor;
        }

        public Task<IEnumerable<CountryDto>> GetAllAsync() => _accessor.GetAllAsync();
        public Task<CountryDto?> GetByIdAsync(string id) => _accessor.GetByIdAsync(id);
        public Task<CountryDto> CreateAsync(CountryDto dto) => _accessor.CreateAsync(dto);
        public Task<CountryDto?> UpdateAsync(string id, CountryDto dto) => _accessor.UpdateAsync(id, dto);
        public Task<bool> DeleteAsync(string id) => _accessor.DeleteAsync(id);
    }
}
