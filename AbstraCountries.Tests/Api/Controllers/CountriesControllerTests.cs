using AbstraCountries.Contracts.Dtos;
using System.Net;
using System.Net.Http.Json;

namespace AbstraCountries.Tests.Api.Controllers
{
    [TestClass]
    public class CountriesControllerTests : ControllerTestBase
    {
        [TestClass]
        public class WhenGettingAllCountries : CountriesControllerTests
        {
            private HttpResponseMessage? _response;

            [TestInitialize]
            public override async Task BeforeEach()
            {
                await base.BeforeEach();
                _response = await _client!.GetAsync("/countries");
            }

            [TestMethod]
            public void ReturnsSuccess() =>
                Assert.IsTrue(_response!.IsSuccessStatusCode);

            [TestMethod]
            public async Task ReturnsCollection()
            {
                var countries = await _response!.Content.ReadFromJsonAsync<ICollection<CountryDto>>();
                Assert.IsNotNull(countries);
                Assert.AreNotEqual(0, countries!.Count);
            }
        }
    }

    [TestClass]
    public class WhenGettingCountryById : CountriesControllerTests
    {
        private HttpResponseMessage? _response;

        [TestInitialize]
        public override async Task BeforeEach()
        {
            await base.BeforeEach();

            _response = await _client!.GetAsync("/countries/CR");
        }

        [TestMethod]
        public void ReturnsSuccess() =>
            Assert.IsTrue(_response!.IsSuccessStatusCode);

        [TestMethod]
        public async Task ReturnsCountry()
        {
            var country = await _response!.Content.ReadFromJsonAsync<CountryDto>();
            Assert.IsNotNull(country);
            Assert.AreEqual("CR", country!.Id);
        }
    }

    [TestClass]
    public class WhenGettingCountryThatDoesNotExist : CountriesControllerTests
    {
        private HttpResponseMessage? _response;

        [TestInitialize]
        public override async Task BeforeEach()
        {
            await base.BeforeEach();

            _response = await _client!.GetAsync("/countries/ZZ");
        }

        [TestMethod]
        public void ReturnsNotFound() =>
            Assert.AreEqual(HttpStatusCode.NotFound, _response!.StatusCode);
    }

    [TestClass]
    public class WhenCreatingCountry : CountriesControllerTests
    {
        private HttpResponseMessage? _response;
        private CountryDto? _created;

        [TestInitialize]
        public override async Task BeforeEach()
        {
            await base.BeforeEach();
            var dto = new CountryDto { Id = "WK", Name = "Wakanda" };
            _response = await _client!.PostAsJsonAsync("/countries", dto);

            if (_response!.IsSuccessStatusCode || _response.StatusCode == HttpStatusCode.Created)
            {
                _created = await _response.Content.ReadFromJsonAsync<CountryDto>();
            }
        }

        [TestMethod]
        public void ReturnsCreated() =>
            Assert.AreEqual(HttpStatusCode.Created, _response!.StatusCode);

        [TestMethod]
        public void ReturnsCreatedCountry() =>
            Assert.AreEqual("Wakanda", _created!.Name);
    }

    [TestClass]
    public class WhenUpdatingCountry : CountriesControllerTests
    {
        private HttpResponseMessage? _response;
        private CountryDto? _updated;

        [TestInitialize]
        public override async Task BeforeEach()
        {
            await base.BeforeEach();

            var dto = new CountryDto { Id = "CR", Name = "Costa Rica Updated" };
            _response = await _client!.PostAsJsonAsync("/countries/CR", dto);
            _updated = await _response.Content.ReadFromJsonAsync<CountryDto>();
        }

        [TestMethod]
        public void ReturnsSuccess() =>
            Assert.IsTrue(_response!.IsSuccessStatusCode);

        [TestMethod]
        public void ReturnsUpdatedCountry() =>
            Assert.AreEqual("Costa Rica Updated", _updated!.Name);
    }


    [TestClass]
    public class WhenDeletingCountry : CountriesControllerTests
    {
        private HttpResponseMessage? _response;

        [TestInitialize]
        public override async Task BeforeEach()
        {
            await base.BeforeEach();

            _response = await _client!.DeleteAsync("/countries/US");
        }

        [TestMethod]
        public void ReturnsNoContent() =>
            Assert.AreEqual(HttpStatusCode.NoContent, _response!.StatusCode);
    }

}
