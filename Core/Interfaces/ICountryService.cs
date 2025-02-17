using Core.Entities;
using Core.ViewModels;

namespace Core.Interfaces;

public interface ICountryService
{
    /// <summary>
    /// Returns a list of supported countries.
    /// If there are none in the DB, fetch from the external API, map, store, and then return.
    /// </summary>
    public Task<List<CountryViewModel>> GetAllCountries();

    public Task<Country> GetCountryByCode(string countryCode);
}