using netCoreApi.DatabaseModels;


namespace netCoreApi.Database
{
    public interface IDatabase
    {
        Task<IEnumerable<Country>> FindCountryByIp(string ip);
        Task<IEnumerable<Country>> GetCountries();
        Task<IEnumerable<Ipaddress>> GetIps();
        Task<bool> CreateUser(User user);
        User FindUserRegister(User user);
        bool ChangeStatus(string token);
    }
}
