using Core.Entities;
using Core.Interfaces;
using Core.ViewModels;

namespace Application.Services;

public class CountryService : ICountryService
    {
        private readonly IRepository<Country> _countryRepository;
        private readonly IExternalHolidayApiService _externalApiService;

        public CountryService(
            IRepository<Country> countryRepository,
            IExternalHolidayApiService externalApiService)
        {
            _countryRepository = countryRepository;
            _externalApiService = externalApiService;
        }

        /// <summary>
        /// Returns a list of supported countries.
        /// If there are none in the DB, fetch from the external API, map, store, and then return.
        /// </summary>
        public async Task<List<CountryViewModel>> GetAllCountriesAsync()
        {
            var countries = await _countryRepository.QueryAsync(asNoTracking: true);

            if (countries.Count == 0)
            {
                // Fetch countries from the external API.
                var externalCountries = await _externalApiService.GetCountries();

                var countryEntities = externalCountries
                    .Select(ext => MapExternalCountryToDomain(ext))
                    .ToList();

                await _countryRepository.Insert(countryEntities);
                await _countryRepository.SaveChangesAsync();

                countries = countryEntities;
            }

            // Map our domain models to view models.
            var result = countries.Select(c => new CountryViewModel
            {
                CountryCode3Digit = c.CountryCode3Digit,
                CountryCode2Digit = c.CountryCode2Digit,
                FullName = c.FullName,
                FromDate = c.FromDate,
                ToDate = c.ToDate
            }).ToList();

            return result;
        }

        /// <summary>
        /// Maps an external country (3-letter code) to our DB Country model.
        /// Converts the 3-digit code to 2-digit code using the ISO3166 library.
        /// </summary>
        private Country MapExternalCountryToDomain(ExternalCountryViewModel ext)
        {
            // Convert the 3-digit code to 2-digit code using ISO3166.
            var isoCountry = ISO3166.Country.List.FirstOrDefault(c => c.ThreeLetterCode.Equals(ext.CountryCode, StringComparison.OrdinalIgnoreCase));
            var countryCode2Digit = isoCountry != null 
                ? isoCountry.TwoLetterCode 
                : ext.CountryCode;

            var fromDate = new DateTime(ext.FromDate.Year, ext.FromDate.Month, ext.FromDate.Day);
            var toDate = new DateTime(ext.ToDate.Year > DateTime.MaxValue.Year 
                    ? DateTime.MaxValue.Year 
                    : ext.ToDate.Year, 
                ext.ToDate.Month, 
                ext.ToDate.Day);

            return new Country
            {
                Id = Guid.NewGuid(),
                CountryCode3Digit = ext.CountryCode,
                CountryCode2Digit = countryCode2Digit,
                FullName = ext.FullName,
                FromDate = fromDate,
                ToDate = toDate
            };
        }
    }