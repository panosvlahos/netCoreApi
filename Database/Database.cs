using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using netCoreApi.ApiModels;
using netCoreApi.Models;
using System.Diagnostics.Metrics;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace netCoreApi.Database
{
    public class Database :IDatabase
    {
        public Database(masterContext context, IMemoryCache memoryCache)
        {
            _context= context;
            _memoryCache= memoryCache;
        }
        public masterContext _context;
        private readonly IMemoryCache _memoryCache;

        public async Task<IEnumerable<Country>> FindCountryByIp( string ip)
        {
            Ipaddress ipaddress = new Ipaddress();
            List<Country> countries = new List<Country>();
            
            ipaddress = _context.Ipaddresses.Where(x => x.Ip == ip).FirstOrDefault();
            countries = await _context.Countries.Where(x => x.Id == ipaddress.CountryId).ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions().
                                  SetSlidingExpiration(TimeSpan.FromHours(100));
            _memoryCache.Set("countries", countries, cacheOptions);
            _memoryCache.Set("ip", ip, cacheOptions);

            return countries;
        }
        public async  Task<IEnumerable<Country>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }
        public async Task<IEnumerable<Ipaddress>> GetIps()
        {
//            select count(*) CountryId, max(UpdatedAt) as max_LogDate, c.name
//from[master].[dbo].[IPAddresses] i
//inner join[master].[dbo].Countries c on(c.Id = i.CountryId)
//group by CountryId, c.name
//order by max_LogDate desc
                
            return await _context.Ipaddresses.OrderByDescending(x=> x.UpdatedAt).ToListAsync();
        }
    }
}
