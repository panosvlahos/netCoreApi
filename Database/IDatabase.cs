using netCoreApi.Models;


namespace netCoreApi.Database
{
    public interface IDatabase
    {
        Task<IEnumerable<Country>> FindCountryByIp(string ip);
        Task<IEnumerable<Country>> GetCountries();
        Task<IEnumerable<Ipaddress>> GetIps();
    }
}
